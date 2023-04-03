using AutoMapper;
using FormPaperless.Core;
using FormPaperless.Core.Model;
using FormPaperless.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FormPaperless.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GenericFormController : Controller
    {
        IConfiguration Configuration;
        ServerInfo ServerInfo;

        public GenericFormController(IConfiguration configuration, ServerInfo serverInfo)
        {
            Configuration = configuration;
            ServerInfo = serverInfo;
        }

        /// <summary>
        /// 通过传入FormName获取注册的点检内容
        /// </summary>
        /// <param name="sFormName"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseObjectModel<GenericFormModel> GetFormFormat(string sFormName)
        {
            GenericFormService service = new GenericFormService(ServerInfo);
            var GF_Model = service.GetFormFormat(sFormName);
            return GF_Model;
        }

        /// <summary>
        /// 新增或更新点检数据，通过ID判定，ID为Null则新增，否则更新
        /// </summary>
        /// <param name="genericFormModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseObjectModel<string> PostFormData([FromBody] GenericFormModel genericFormModel)
        {
            GenericFormService service = new GenericFormService(ServerInfo);
            service.GenericFormAction(genericFormModel);
            return "";
        }

        /// <summary>
        /// 通过FormTable名，ID获取已提交表单的详细信息
        /// </summary>
        /// <param name="sFormName"></param>
        /// <param name="sID"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseObjectModel<GenericFormModel> GetFormDataById(string sFormName, string sID)
        {
            GenericFormService service = new GenericFormService(ServerInfo);
            GenericFormModel GF_Model= service.QueryFormDetailInfoById(sFormName, sID);
            return GF_Model;
        }

        /// <summary>
        /// 获取当前有效表单清单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseObjectModel<FormSummaryInfo> GetFormSummaryInfo()
        {
            FormMenuService service = new FormMenuService(ServerInfo);
            var FM_Model = service.GetMenu();
            return FM_Model;
        }

    }
}
