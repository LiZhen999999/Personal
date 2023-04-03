using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using FormPaperless.Core;
using SqlSugar;
using Microsoft.Extensions.Configuration;
using FormPaperless.Core;

namespace FormPaperless.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DirectoryPermissionsController : ApiControllerBase
    {
        private ServerInfo _serverInfo;
        public DirectoryPermissionsController(ServerInfo serverInfo)
        {
            _serverInfo = serverInfo;
        }
        
        /// <summary>
        /// 根据用户ID返回该用户的目录权限
        /// </summary>
        /// <param name="ssDataQuery">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseObjectModel<string> GetDirectories([FromBody] int userID)
        {
            try
            {
                GetUserDirectories getUserDirectories = new GetUserDirectories(_serverInfo);
                string dic = getUserDirectories.GetDirectories(userID);
                return dic;
            }
            catch (Exception Err)
            {
                return FailResult<string>(Err.Message);
            }
        }
    }
}
