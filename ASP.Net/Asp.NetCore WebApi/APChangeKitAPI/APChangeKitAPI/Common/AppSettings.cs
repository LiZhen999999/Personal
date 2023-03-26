using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace APChangeKitAPI.Common
{
    /// <summary>
    /// 应用设置类
    /// 总类：对应json文件，确定json模块与对象
    /// </summary>
    public static class AppSettings
    {
        public static AppSettingsModel Data { get; set; } = new AppSettingsModel();
        public static void Init(IConfiguration configuration)
        {
            Data.ConnectionStrings = new ConnectionStrings();
            Data.AppKeys = new AppKeys();
            configuration.Bind("ConnectionStrings", Data.ConnectionStrings);
            configuration.Bind("AppKeys", Data.AppKeys);
            Data.AllowedHostsCors = configuration.GetSection("AllowedHostsCors").Value;
        }
    }

    public class AppSettingsModel
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string AllowedHostsCors { get; set; }
        public AppKeys AppKeys { get; set; }
    }

    public class ConnectionStrings
    {
        public string EMSDB { get; set; }
    }

    public class AppKeys
    {
        public string UploadPath { get; set; }
        /// <summary>
        /// OA系统Web API服务器
        /// </summary>
        public string OAWebAPI { get; set; }
        /// <summary>
        /// MES服务器
        /// </summary>
        public string MESWebService { get; set; }
    }
}
