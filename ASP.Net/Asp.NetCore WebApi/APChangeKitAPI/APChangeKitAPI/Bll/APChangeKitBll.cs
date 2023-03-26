using APChangeKitAPI.Common;
using APChangeKitAPI.DAL;
using APChangeKitAPI.Middleware;
using APChangeKitAPI.Models.DTO;
using APChangeKitAPI.Models.Entity;
using APChangeKitAPI.Models.Enums;
using AutoMapper;
using DbUtils;
using DbUtils.Models;
using DynamicExpresso;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace APChangeKitAPI.Bll
{
    public class APChangeKitBll
    {
        private readonly IMapper _mapper;
        private readonly FileDAL _fileDAL;
        private readonly APChangeKitDAL _apChangeKitDAL;
        private readonly ILogger<APChangeKitBll> _logger;

        public APChangeKitBll(IMapper mapper, FileDAL fileDAL, APChangeKitDAL apChangeKitDAL, ILogger<APChangeKitBll> logger)
        {
            _mapper = mapper;
            _fileDAL = fileDAL;
            _apChangeKitDAL = apChangeKitDAL;
            _logger = logger;
        }

        /// <summary>
        /// 获取转机单详情
        /// </summary>
        /// <param name="orderNumer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public APChangeKitChkListRes GetAPChangeKitCheckList(string orderNumer)
        {
            if (string.IsNullOrEmpty(orderNumer))
                throw new ArgumentException("转机单号不能为空！");

            var model = _apChangeKitDAL.GetAPChangeKit(new AP_ChangeKit_FormInfo() { OrderNum = orderNumer }).FirstOrDefault();
            if (model == null)
                throw new Exception($"转机单号[{orderNumer}]不存在！");

            var res = _mapper.Map<APChangeKitChkListRes>(model);
            res.Modules = new List<APChangeKitChkListItem>();
            var detailList = _apChangeKitDAL.GetAPChangeKitDetail(new AP_ChangeKit_FormDetail() { OrderNum = orderNumer }).OrderBy(x => x.ItemBlock).ThenBy(x => x.ItemIndex);
            if (detailList == null || detailList.Count() <= 0)
                return res;

            res.UpdateBy = detailList.OrderByDescending(x => x.UpdateTime).FirstOrDefault()?.UpdateBy ?? "";
            var controlList = _apChangeKitDAL.GetAPChangeKitControl(new AP_ChangeKit_Control());
            foreach (var row in detailList)
            {
                var detail = _mapper.Map<APChangeKitChkListItem>(row);
                if (detail.FileID > 0)
                {
                    var file = _fileDAL.GetFile(new T_File() { ID = detail.FileID }).FirstOrDefault();
                    if (file != null)
                    {
                        detail.FileName = $"{file.FileName}{file.Extension}";
                        detail.FilePath = file.FilePath;
                    }
                }

                var control = controlList.FirstOrDefault(x => x.ID == row.ControlID);
                if(control != null)
                {
                    detail.InputType = control.InputType;
                    detail.ControlTip = control.ControlTip;
                }

                res.Modules.Add(detail);
            }

            return res;
        }

        /// <summary>
        /// 更新转机单CheckList（接口）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateAPChangeKitCheckList(APChangeKitChkListAddReq param)
        {
            if (param == null)
                throw new ArgumentNullException("参数不能为NULL！");

            var model = _apChangeKitDAL.GetAPChangeKit(new AP_ChangeKit_FormInfo() { OrderNum = param.OrderNum }).FirstOrDefault();
            if (model == null)
                throw new ArgumentNullException($"转机单号[{param.OrderNum}]不存在！");

            if (model.Status != APChangeKitStatus.CheckList)
                throw new ArgumentNullException($"转机单号[{param.OrderNum}]状态不为填写CheckList！");

            //获取工序对应审批节点
            var config = _apChangeKitDAL.GetAPChangeKitConfig(new AP_ChangeKit_Config()
            {
                ConfigType = "CheckListApprovalNextNode",
                ConfigKey = model.Process,
            }).FirstOrDefault();

            if(config == null || string.IsNullOrEmpty(config.ConfigValue))
                throw new DataException($"当前转机单号对应工序未找到审批节点");
            
            //值卡控
            foreach(var item in param.Modules)
            {
                var control = _apChangeKitDAL.GetAPChangeKitControl(item.ID);
                if (control == null || string.IsNullOrEmpty(control.ControlExpression) || string.IsNullOrEmpty(control.ValueType))
                    continue;

                var controlResult = false;
                try
                {
                    var type = Type.GetType(control.ValueType, true, true);
                    var value = Convert.ChangeType(item.CheckResult, type);
                    //controlResult = Eval.Execute<bool>(control.ControlExpression, new { value = value });
                    var parameters = new Parameter[]
                    {
                        new Parameter("value", value.GetType(), value)
                    };
                    var options = InterpreterOptions.Default | InterpreterOptions.LambdaExpressions; // enable lambda expressions
                    var target = new Interpreter(options);
                    controlResult = target.Eval<bool>(control.ControlExpression, parameters);
                }
                catch (Exception) { }
                if (!controlResult)
                    throw new DataException($"当前检查项[{item.CheckItem}]值[{item.CheckResult}]未通过校验：[{control.ControlTip}]");
            }

            if (_apChangeKitDAL.UpdateAPChangeKitCheckList(param))
            {
                _apChangeKitDAL.InsertAPChangeKitLog(new AP_ChangeKit_Log()
                {
                    LogSource = LogSource.Api,
                    LogType = LogType.Update,
                    OrderNum = param.OrderNum,
                    RequestParam = JsonConvert.SerializeObject(param),
                    Result = "true",
                    CreateBy = param.User ?? "",
                });

                //更新成功并且判断所有结果是否填写完毕
                if (param.Modules.Count > 0 && param.Modules.All(x => !string.IsNullOrEmpty(x.CheckResult)))
                {
                    
                    var oaResultFlag = false;
                    var oaResultError = "";
                    var mesResultFlag = false;
                    var mesResultError = "";
                    //调用OA接口进行审批
                    oaResultFlag = SubmitToOA(param.OrderNum, param.User, model.OAOrderNum, config.ConfigValue, ref oaResultError);
                    if (!oaResultFlag)
                        throw new DataException($"保存CheckList成功，但OA提交CheckList审核失败：{oaResultError}");

                    //调用MES系统接口进行解锁
                    if (oaResultFlag)
                    {
                        mesResultFlag = UnlockToMES(model.Process, model.OrderNum, model.EQID, param.User, ref mesResultError);
                        if (!mesResultFlag)
                            throw new DataException($"保存CheckList成功，但MES提交CheckList解锁失败：{mesResultError}");
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 填写完CheckList提交OA进入下一流程
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="userID"></param>
        /// <param name="oaRequestID"></param>
        /// <param name="oaNextStep"></param>
        /// <param name="oaResultError"></param>
        /// <returns></returns>
        /// <exception cref="DataException"></exception>
        public bool SubmitToOA(string orderNum, string userID, string oaRequestID, string oaNextStep, ref string oaResultError)
        {
            var oaResultFlag = false;
            if (string.IsNullOrEmpty(oaRequestID))
                throw new DataException($"转机单号[{orderNum}]未关联OA");

            var param = $"?requestid={oaRequestID}&nextstep={oaNextStep}";
            var httpParam = new HttpItem()
            {
                URL = $"{AppSettings.Data.AppKeys.OAWebAPI.TrimEnd('/')}/api/kf/out/ToMesInterface/NotifyMESToAduit{param}",
                Method = "POST",
            };
            _logger.LogInformation($"[{orderNum}]调用OA系统接口[NotifyMESToAduit]参数：{JsonConvert.SerializeObject(httpParam)}");
            //调用OA系统接口
            var httpResult = new HttpHelper().Execute(httpParam);
            _logger.LogInformation($"[{orderNum}]调用OA系统接口[NotifyMESToAduit]结果：{JsonConvert.SerializeObject(httpResult)}");

            if (httpResult.StatusCode == HttpStatusCode.OK && !httpResult.IsException && !string.IsNullOrEmpty(httpResult.Result))
            {
                var oaResult = JsonConvert.DeserializeObject<OANotifyMESToAduitResultDTO>(httpResult.Result);
                //OA接口成功
                if (oaResult.status.FirstOrDefault() != 0)
                {
                    oaResultFlag = true;
                    _apChangeKitDAL.UpdateAPChangeKitStatus(new AP_ChangeKit_FormInfo()
                    {
                        OrderNum = orderNum,
                        Status = APChangeKitStatus.CheckListApprove,
                        UpdateBy = userID,
                    });
                }
                else
                    oaResultError = oaResult.msg.FirstOrDefault();
            }
            else
                oaResultError = httpResult.Result;

            _apChangeKitDAL.InsertAPChangeKitLog(new AP_ChangeKit_Log()
            {
                LogSource = LogSource.Api,
                LogType = LogType.Update,
                OrderNum = orderNum,
                RequestParam = param,
                Result = $"OA提交CheckList审核结果：{oaResultFlag}. 消息：{oaResultError}",
                CreateBy = userID ?? "",
            });

            return oaResultFlag;
        }

        /// <summary>
        /// 填写完CheckList提交MES进行解锁
        /// </summary>
        /// <param name="process"></param>
        /// <param name="orderNo"></param>
        /// <param name="eqID"></param>
        /// <param name="userID"></param>
        /// <param name="resMsg"></param>
        /// <returns></returns>
        public bool UnlockToMES(string process, string orderNo, string eqID, string userID, ref string resMsg)
        {
            var url = $"{AppSettings.Data.AppKeys.MESWebService.TrimEnd('/')}/mes/maintain/csin0330/kitwebservice.asmx";
            var method = "";
            var keyValues = new Dictionary<string, string>();
            switch (process.ToUpper())
            {
                case "TP":
                case "BSG":
                case "WM":
                case "DC":
                case "MD":
                case "MK":
                case "BM":
                case "SS":
                case "OS":
                case "AVI":
                    //1个lot
                    method = "UnlockMKBMMachine";
                    keyValues.Add("s_OrderNo", orderNo);
                    keyValues.Add("s_Machine", eqID);
                    keyValues.Add("s_Checker", userID);
                    keyValues.Add("s_Status", "HalfClosed");
                    keyValues.Add("s_lotNum", "1");
                    break;
                case "DB":
                    //5K产品
                    method = "UnlockDBMachine";
                    keyValues.Add("s_OrderNo", orderNo);
                    keyValues.Add("s_Machine", eqID);
                    keyValues.Add("s_Checker", userID);
                    keyValues.Add("s_Status", "Half");
                    keyValues.Add("s_ICNum", "5000");
                    break;
                default:
                    return true;
            }
            
            _logger.LogInformation($"[{orderNo}]调用MES系统接口[kitwebservice]参数：{JsonConvert.SerializeObject(keyValues)}");
            var webService = new WebServiceCall(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(webService.callWebService(method, keyValues)));
            resMsg = xmlDoc.InnerText.ToString();
            _logger.LogInformation($"[{orderNo}]调用MES系统接口[kitwebservice]结果：{JsonConvert.SerializeObject(resMsg)}");
            //记录日志
            _apChangeKitDAL.InsertAPChangeKitLog(new AP_ChangeKit_Log()
            {
                LogSource = LogSource.Api,
                LogType = LogType.Update,
                OrderNum = orderNo,
                RequestParam = JsonConvert.SerializeObject(keyValues),
                Result = $"MES提交CheckList解锁结果：{resMsg.Contains("Y|")}. 消息：{resMsg}",
                CreateBy = userID ?? "",
            });

            return resMsg.Contains("Y|");
        }
    }
}
