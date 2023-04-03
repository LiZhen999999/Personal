using FormPaperless.Entity;
using System;

namespace FormPaperless.Service
{
    public class ServiceBase
    {
        protected ServerInfo serverInfo;

        public ServiceBase(ServerInfo serverInfo)
        {
            this.serverInfo = serverInfo;
        }
    }
}
