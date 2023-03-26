using APChangeKitAPI.Common;
using APChangeKitAPI.Models.DTO;
using APChangeKitAPI.Models.Entity;
using DbUtils;
using DbUtils.Models;
using System.Collections.Generic;
using System;
using AutoMapper;
using System.Linq;

namespace APChangeKitAPI.DAL
{
    public class APChangeKitDAL
    {
        private readonly IDbUtils _dbUtils = new DbFactory(DatabaseType.SqlServer, AppSettings.Data.ConnectionStrings.EMSDB).CreateDbUtils();
        private readonly IMapper _mapper;
        public APChangeKitDAL(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 获取转机单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<AP_ChangeKit_FormInfo> GetAPChangeKit(AP_ChangeKit_FormInfo param)
        {
            if (param == null)
                throw new ArgumentNullException("转机单参数不能为NULL！");

            var sql = "SELECT * FROM dbo.AP_ChangeKit_FormInfo WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(param.OrderNum))
                sql += " AND OrderNum = @OrderNum ";

            return _dbUtils.Query<AP_ChangeKit_FormInfo>(sql, param);
        }

        /// <summary>
        /// 获取转机单CheckList
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<AP_ChangeKit_FormDetail> GetAPChangeKitDetail(AP_ChangeKit_FormDetail param)
        {
            if (param == null)
                throw new ArgumentNullException("转机单参数不能为NULL！");

            var sql = "SELECT * FROM dbo.AP_ChangeKit_FormDetail WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(param.OrderNum))
                sql += " AND OrderNum = @OrderNum ";

            return _dbUtils.Query<AP_ChangeKit_FormDetail>(sql, param);
        }

        /// <summary>
        /// 更新转机单CheckList
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateAPChangeKitCheckList(APChangeKitChkListAddReq param)
        {
            if (param == null)
                throw new ArgumentNullException("转机单参数不能为NULL！");

            var sql = "UPDATE dbo.AP_ChangeKit_FormDetail SET CheckItem = @CheckItem, CheckStandard = @CheckStandard, CheckResult = @CheckResult, FileID = @FileID, UpdateTime = GETDATE(), UpdateBy = @UpdateBy WHERE ID = @ID;";

            var paramList = new List<DBParamModel>();
            foreach (var item in param.Modules)
            {
                var detail = _mapper.Map<AP_ChangeKit_FormDetail>(item);
                detail.UpdateBy = param.User;
                paramList.Add(new DBParamModel()
                {
                    Sql = sql,
                    Param = detail,
                });
            }

            if (paramList.Count == 0)
                return false;

            return _dbUtils.ExecuteTransaction(paramList);
        }

        /// <summary>
        /// 更新转机单状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateAPChangeKitStatus(AP_ChangeKit_FormInfo param)
        {
            if (param == null)
                throw new ArgumentNullException("转机单参数不能为NULL！");

            var sql = "UPDATE dbo.AP_ChangeKit_FormInfo SET Status = @Status, UpdateTime = getdate(), UpdateBy = @UpdateBy WHERE OrderNum = @OrderNum;";
            return _dbUtils.Execute(sql, param) > 0;
        }

        /// <summary>
        /// 插入转机单操作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int InsertAPChangeKitLog(AP_ChangeKit_Log param)
        {
            var sql = @"
INSERT INTO dbo.AP_ChangeKit_Log(LogType, OrderNum, LogSource, RequestParam, Result, CreateBy)
VALUES(@LogType, -- LogType 
@OrderNum , -- OrderNum - nvarchar(50)
@LogSource , -- LogSource 
@RequestParam , -- RequestParam - nvarchar(max)
@Result , -- Result - nvarchar(max)
@CreateBy -- CreateBy - nvarchar(50)
);SELECT @@IDENTITY;";
            return _dbUtils.ExecuteScalar<int>(sql, param);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<AP_ChangeKit_Config> GetAPChangeKitConfig(AP_ChangeKit_Config param)
        {
            if (param == null)
                throw new ArgumentNullException("获取卡控时参数不能为NULL！");

            var sql = "SELECT * FROM dbo.AP_ChangeKit_Config WHERE 1 = 1 ";
            if (param.ID >= 0)
                sql += " AND ID = @ID ";

            if (!string.IsNullOrEmpty(param.ConfigType))
                sql += " AND ConfigType = @ConfigType ";

            if (!string.IsNullOrEmpty(param.ConfigKey))
                sql += " AND ConfigKey = @ConfigKey ";

            return _dbUtils.Query<AP_ChangeKit_Config>(sql, param);
        }

        /// <summary>
        /// 获取卡控配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<AP_ChangeKit_Control> GetAPChangeKitControl(AP_ChangeKit_Control param)
        {
            if (param == null)
                throw new ArgumentNullException("获取卡控时参数不能为NULL！");

            var sql = "SELECT * FROM dbo.AP_ChangeKit_Control WHERE 1 = 1 ";
            if (param.ID >= 0)
                sql += " AND ID = @ID ";

            return _dbUtils.Query<AP_ChangeKit_Control>(sql, param);
        }

        /// <summary>
        /// 获取卡控配置
        /// </summary>
        /// <param name="detailID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public AP_ChangeKit_Control GetAPChangeKitControl(int detailID)
        {
            var sql = @"
SELECT acc.*
FROM dbo.AP_ChangeKit_FormDetail acfd
JOIN dbo.AP_ChangeKit_Control acc ON acfd.ControlID = acc.ID
WHERE acfd.ID = @ID";
            return _dbUtils.Query<AP_ChangeKit_Control>(sql, new { ID = detailID }).FirstOrDefault();
        }
    }
}
