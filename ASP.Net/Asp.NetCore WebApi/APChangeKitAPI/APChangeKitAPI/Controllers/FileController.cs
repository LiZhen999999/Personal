using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APChangeKitAPI.Models;
using APChangeKitAPI.Bll;
using System.Linq;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using APChangeKitAPI.Models.Entity;
using APChangeKitAPI.Models.Attributes;
using log4net.Core;
using Microsoft.Extensions.Logging;
using log4net.Repository.Hierarchy;

namespace APChangeKitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileBll _fileBll;
        private readonly ILogger<FileController> _logger;

        public FileController(FileBll fileBll, ILogger<FileController> logger)
        {
            _fileBll = fileBll;
            _logger = logger;
        }

        [HttpPost, Route("Upload"), FileUpload]
        public Result<int> Upload()
        {
            var files = HttpContext.Request.Form.Files;
            if (files == null || files.Count == 0)
                return Result<int>.Error("上传文件不能为空").SetCode(Models.Enums.ResultCode.ParametersIncorrect);

            var file = files.FirstOrDefault();
            _logger.LogInformation($"api/File/Upload，FileName：{file.FileName}，Length：{file.Length}，ContentType：{file.ContentType}");
            var id = _fileBll.Upload(file);
            if (id <= 0)
                Result<int>.Error("上传文件失败！");

            return Result<int>.Success().SetData(id);
        }

        [HttpGet, Route("DownloadByName")]
        public IActionResult DownloadByName(string fileGuid)
        {
            var filePath = _fileBll.GetDownloadUrl(fileGuid, out T_File fileInfo);
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(fileInfo.Extension, out string contentType);
            using (var wclient = new WebClient())
            {
                var fileStream = wclient.OpenRead(filePath);
                return File(fileStream, contentType ?? "application/octet-stream", $"{fileInfo.FileName}{fileInfo.Extension}");
            }
        }

        [HttpGet, Route("DownloadByID")]
        public IActionResult DownloadByID(int fileID)
        {
            var filePath = _fileBll.GetDownloadUrl(fileID, out T_File fileInfo);
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(fileInfo.Extension, out string contentType);
            using (var wclient = new WebClient())
            {
                var fileStream = wclient.OpenRead(filePath);
                return File(fileStream, contentType ?? "application/octet-stream", $"{fileInfo.FileName}{fileInfo.Extension}");
            }
        }
    }
}
