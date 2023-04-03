using FormPaperless.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormPaperless;
using DynamicExpresso;
using System.Linq.Expressions;
using SqlSugar;
using AutoMapper;
using System.Data.SqlClient;
using Newtonsoft.Json.Bson;
using System.Data;

namespace FormPaperless.Core
{
    public class GenericFormService
    {
        ServerInfo _serverInfo;
        IMapper _mapper;

        public GenericFormService(ServerInfo serverInfo)
        {
            _serverInfo = serverInfo;
            MapperConfiguration Config = new MapperConfiguration(cfg => cfg.AddProfile<GenericFormFrofile>());
            _mapper = Config.CreateMapper();
        }

        /// <summary>
        /// 通用表单->通过表单名获取注册的表单点检样式及标准
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public GenericFormModel GetFormFormat(string formName)
        {
            GenericFormModel formModel = new GenericFormModel();
            SqlSugarClient sqlclient = SqlSugarExtension.GetPtnCilent(_serverInfo.MesSrv, _serverInfo.PlsDatabase);
            List<FormMasterHead> heads = sqlclient.Queryable<FormMasterHead>().Where(f => f.FormName == formName && f.Effect).ToList();
            if (heads.Count == 1)
            {
                string s_Id = heads[0].Id;
                List<FormMasterBody> bodys = sqlclient.Queryable<FormMasterBody>().Where(f => f.Id == s_Id).OrderBy(f=>f.ItemNo).ToList();
                //对表单主要内容进行转换
                formModel = _mapper.Map<FormMasterHead, GenericFormModel>(heads[0]);
                //对表单详细内容进行转换
                List<GenericFormContentModel> formContents = _mapper.Map<List<FormMasterBody>, List<GenericFormContentModel>>(bodys);
                //详细内容赋值
                formModel.FormContents = formContents;
            }
            return formModel;
        }

        /// <summary>
        /// 新增或更新表单信息
        /// </summary>
        /// <param name="genericFormModel"></param>
        /// <returns></returns>
        public void GenericFormAction(GenericFormModel genericFormModel)
        {
            SqlSugarClient sqlclient = SqlSugarExtension.GetPtnCilent(_serverInfo.MesSrv, _serverInfo.PlsDatabase);
            if (string.IsNullOrEmpty(genericFormModel.Id))
            {
                AddFormAction(sqlclient, genericFormModel);
            }
            else
            {
                UpdateFormAction(sqlclient, genericFormModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="genericFormModel"></param>
        public void AddFormAction(SqlSugarClient sqlSugarClient, GenericFormModel genericFormModel)
        {
            string sTableName = genericFormModel.FormTableName;
            sqlSugarClient.BeginTran();
            var sqlpara = new List<SqlSugar.SugarParameter>();
            string s_SqlCommand = "INSERT INTO " + sTableName;
            string s_Columns = "";
            string s_Values = "";
            List<GenericFormContentModel> frmContents = genericFormModel.FormContents;
            Dictionary<string, string> dic_InputData = new Dictionary<string, string>();

            #region 通用部分信息写入
            //通用列写入
            //ID
            string s_NewGuId = Guid.NewGuid().ToString();
            //ID写入Model
            genericFormModel.Id = s_NewGuId;
            s_Columns += "ID" + ",";
            s_Values += "@ID" + ",";
            sqlpara.Add(new SugarParameter("@ID", s_NewGuId));
            dic_InputData.Add("ID", s_NewGuId);
            //表单名
            s_Columns += "FormName" + ",";
            s_Values += "@FormName" + ",";
            sqlpara.Add(new SugarParameter("@FormName", genericFormModel.FormCnName));
            dic_InputData.Add("FormName", genericFormModel.FormCnName);
            //表单回传对象
            string sFormData = Newtonsoft.Json.JsonConvert.SerializeObject(genericFormModel);
            s_Columns += "FormData" + ",";
            s_Values += "@FormData" + ",";
            sqlpara.Add(new SugarParameter("@FormData", sFormData));
            //表单创建时间

            #endregion

            //定制化列写入
            foreach (var item in frmContents)
            {
                //检查是否满足Spec
                if (!string.IsNullOrEmpty(item.Specification))
                {
                    bool? b_Spec = SpecificationCheck(item.Specification, item.Data);
                    if (b_Spec.HasValue ? b_Spec.Value : false)
                    {

                    }
                }

                //键值对写入
                dic_InputData.Add(item.ItemColumnName, item.Data);

                //如果是关键列，则需要写入
                if (item.IsKeyColumn)
                {
                    s_Columns += item.ItemColumnName + ",";
                    s_Values += "@" + item.ItemColumnName + ",";
                    //将点检数据作为参数写入
                    sqlpara.Add(new SugarParameter("@" + item.ItemColumnName, item.Data));
                }
            }

            //键值对
            string KvData = Newtonsoft.Json.JsonConvert.SerializeObject(dic_InputData);
            s_Columns += "KvData" + ",";
            s_Values += "@KvData" + ",";
            sqlpara.Add(new SugarParameter("@KvData", KvData));

            s_SqlCommand += "(" + s_Columns.TrimEnd(',') + ") VALUES(" + s_Values.TrimEnd(',') + ")";
            sqlSugarClient.Ado.ExecuteCommand(s_SqlCommand, sqlpara);

            sqlSugarClient.CommitTran();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="genericFormModel"></param>
        public void UpdateFormAction(SqlSugarClient sqlSugarClient, GenericFormModel genericFormModel)
        {

        }

        /// <summary>
        /// 将数值与规格进行比对
        /// 目前仅支持单值比对
        /// </summary>
        /// <param name="specExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SpecificationCheck(string specExpression, string value)
        {
            var interpreter = new Interpreter();
            Lambda parsedExpression;
            bool? result;
            //支持值比对和字符串比对 (sValue、dValue)
            if (specExpression.Contains("sValue"))
            {
                parsedExpression = interpreter.Parse(specExpression, new Parameter("sValue", typeof(string)));
                result = parsedExpression.Invoke(value.Trim()) as bool?;
            }
            else
            {
                parsedExpression = interpreter.Parse(specExpression, new Parameter("dValue", typeof(double)));
                double d_Value = 0;
                if (double.TryParse(value.Trim(), out d_Value))
                {
                    result = result = parsedExpression.Invoke(d_Value) as bool?;
                }
                else
                {
                    return false;
                }
            }
            return result.HasValue ? result.Value : false;
        }


        public GenericFormModel QueryFormDetailInfoById(string s_FormTabelName, string s_ID)
        {
            SqlSugarClient sqlclient = SqlSugarExtension.GetPtnCilent(_serverInfo.MesSrv, _serverInfo.PlsDatabase);
            var sqlpara = new List<SqlSugar.SugarParameter>();
            string s_SqlCommand = "SELECT FormData FROM " + s_FormTabelName + " WHERE Id=@Id";
            sqlpara.Add(new SugarParameter("@Id", s_ID));
            List<string> formdatas = sqlclient.Ado.SqlQuery<string>(s_SqlCommand, sqlpara);
            if (formdatas.Count > 0)
            {
                GenericFormModel formModel = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericFormModel>(formdatas[0]);
                return formModel;
            }
            else
            {
                throw new Exception("查询不到数据！");
            }
        }
    }
}
