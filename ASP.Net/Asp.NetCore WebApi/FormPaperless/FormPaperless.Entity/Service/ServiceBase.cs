using FormPaperless;
using System;

namespace FormPaperless.Core
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
