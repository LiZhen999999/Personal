using AutoMapper;
using FormPaperless.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core.Service
{
    public class FormMenuService
    {
        ServerInfo _serverInfo;
        IMapper _mapper;

        public FormMenuService(ServerInfo serverInfo)
        {
            _serverInfo = serverInfo;
            MapperConfiguration Config = new MapperConfiguration(cfg => cfg.AddProfile<GenericFormFrofile>());
            _mapper = Config.CreateMapper();
        }

        public FormSummaryInfo GetMenu()
        {
            FormSummaryInfo summaryInfo = new FormSummaryInfo();
            SqlSugarClient sqlclient = SqlSugarExtension.GetPtnCilent(_serverInfo.MesSrv, _serverInfo.PlsDatabase);
            var formMaster = sqlclient.Queryable<FormMasterHead>().Where(f => f.Effect).ToList();
            List<FormInfo> formInfos = _mapper.Map<List<FormMasterHead>, List<FormInfo>>(formMaster);
            summaryInfo.FormInfos = formInfos;
            return summaryInfo;
        }
    }
}
