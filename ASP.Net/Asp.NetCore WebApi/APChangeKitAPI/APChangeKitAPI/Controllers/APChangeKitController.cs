using APChangeKitAPI.Bll;
using APChangeKitAPI.Common;
using APChangeKitAPI.Middleware;
using APChangeKitAPI.Models;
using APChangeKitAPI.Models.DTO;
using APChangeKitAPI.Models.Entity;
using APChangeKitAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APChangeKitAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APChangeKitController : ControllerBase
    {
        private readonly APChangeKitBll _bll;

        public APChangeKitController(APChangeKitBll bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// 根据转机单号获取详情
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        [HttpGet, Route("GetInfo")]
        public Result<APChangeKitChkListRes> GetInfo(string orderNumber)
        {
            var data = _bll.GetAPChangeKitCheckList(orderNumber);
            return Result<APChangeKitChkListRes>.Success().SetData(data);
        }

        /// <summary>
        /// 更新转机单CheckList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateCheckList")]
        public Result<bool> UpdateCheckList(APChangeKitChkListAddReq model)
        {
            var data = _bll.UpdateAPChangeKitCheckList(model);
            return Result<bool>.Instance(data).SetCode(data ? ResultCode.Success : ResultCode.Error);
        }

        [HttpGet, Route("Test")]
        public Result<string> Test()
        {
            var db = AppSettings.Data.ConnectionStrings.EMSDB.Split(";")[0];
            var oaUrl = AppSettings.Data.AppKeys.OAWebAPI;
            return Result<string>.Success().SetData(db + ";" + oaUrl);
        }
    }
}
