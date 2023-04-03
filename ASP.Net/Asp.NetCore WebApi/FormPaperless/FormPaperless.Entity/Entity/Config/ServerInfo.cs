using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    ///// <summary>
    ///// 必须使用接口进行实例化，避免暴露出Set方法
    ///// </summary>
    //public interface IServerInfo
    //{
    //    public string MesServer { get; }
    //    public string MesDatabase { get; }
    //    public string AresDatabase { get; }
    //    public string MaterialDatabase { get; }
    //}

    public class ServerInfo
    {
        private string mesServer;
        private string mesDatabase;
        private string aresServer;
        private string aresDatabase;
        private string materialDatabase;
        private string ptiDatabase;
        private string plsDatabase;

        public string MesSrv { get => mesServer; set => mesServer = value; }
        public string MesDb { get => mesDatabase; set => mesDatabase = value; }
        public string AresDb { get => aresDatabase; set => aresDatabase = value; }
        public string MaterialDb { get => materialDatabase; set => materialDatabase = value; }
        public string AresSrv { get => aresServer; set => aresServer = value; }
        public string PtiDatabase { get => ptiDatabase; set => ptiDatabase = value; }
        public string PlsDatabase { get => plsDatabase; set => plsDatabase = value; }
    }
}
