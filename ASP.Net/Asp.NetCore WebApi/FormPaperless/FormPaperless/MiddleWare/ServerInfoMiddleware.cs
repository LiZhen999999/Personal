using FormPaperless.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormPaperless
{
    /// <summary>
    /// 用于获取服务器信息的中间件
    /// </summary>
    public class ServerInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public ServerInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration, ServerInfo serverInfo)
        {
            InitServerInformation(configuration, serverInfo);
            await _next(context);
        }

        /// <summary>
        /// 获取配置文件中的服务器信息
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="serverInfo"></param>
        private void InitServerInformation(IConfiguration Configuration, ServerInfo serverInfo)
        {
            try
            {
                serverInfo.MesSrv = Configuration["MesServer"];
                serverInfo.MesDb = Configuration["MesDatabase"];
                serverInfo.AresSrv = Configuration["AresServer"];
                serverInfo.AresDb = Configuration["AresDatabase"];
                serverInfo.MaterialDb = Configuration["MaterialDatabase"];
                serverInfo.PtiDatabase = Configuration["PtiDatabase"];
                serverInfo.PlsDatabase = Configuration["PlsDatabase"];
            }
            catch
            {
                //如果初始化数据库信息失败，则不做任何操作
            }
        }
    }
}
