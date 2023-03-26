using APChangeKitAPI.Common;
using APChangeKitAPI.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.StaticFiles;
using System.Linq;
using APChangeKitAPI.Models.Entity;

namespace APChangeKitAPI.Bll
{
    public class FileBll
    {
        private readonly FileDAL _fileDAL;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileBll(FileDAL fileDAL, IWebHostEnvironment webHostEnvironment)
        {
            _fileDAL = fileDAL;
            _webHostEnvironment = webHostEnvironment;
        }

        public int Upload(IFormFile file)
        {
            var fileName = "";
            var extension = "";
            var filePath = "";
            var guid = Guid.NewGuid().ToString();
            var rootDir = $"{_webHostEnvironment.WebRootPath.TrimEnd('\\')}";
            if (!string.IsNullOrEmpty(AppSettings.Data.AppKeys.UploadPath))
                rootDir = AppSettings.Data.AppKeys.UploadPath.TrimEnd('\\');

            var timeDir = DateTime.Now.ToString("yyyy-MM");
            if (!Directory.Exists($"{rootDir}\\{timeDir}"))
                Directory.CreateDirectory($"{rootDir}\\{timeDir}");

            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                var content = reader.ReadToEnd();
                fileName = Path.GetFileNameWithoutExtension(file.FileName);
                extension = Path.GetExtension(file.FileName);
                filePath = $"\\Upload\\{timeDir}\\{guid}{extension}";
                using (var fs = System.IO.File.Create($"{rootDir}{filePath}"))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            var id = _fileDAL.InsertFile(new Models.Entity.T_File()
            {
                TrueFileName = guid,
                FileName = fileName,
                Extension = extension,
                FilePath = filePath,
                CreateBy = "",
                UpdateBy = "",
            });

            return id;
        }

        public string GetDownloadUrl(string fileGuid, out T_File fileInfo)
        {
            //获取文件信息 ,自己的业务
            fileInfo = _fileDAL.GetFile(new T_File()
            {
                TrueFileName = fileGuid,
            }).FirstOrDefault();

            var rootDir = $"{_webHostEnvironment.WebRootPath.TrimEnd('\\')}";
            if (!string.IsNullOrEmpty(AppSettings.Data.AppKeys.UploadPath))
                rootDir = AppSettings.Data.AppKeys.UploadPath.TrimEnd('\\');

            var filePath = $"{rootDir}\\{fileInfo.FilePath}";
            return filePath;
        }

        public string GetDownloadUrl(int fileID, out T_File fileInfo)
        {
            //获取文件信息 ,自己的业务
            fileInfo = _fileDAL.GetFile(new T_File()
            {
                ID = fileID,
            }).FirstOrDefault();

            var rootDir = $"{_webHostEnvironment.WebRootPath.TrimEnd('\\')}";
            if (!string.IsNullOrEmpty(AppSettings.Data.AppKeys.UploadPath))
                rootDir = AppSettings.Data.AppKeys.UploadPath.TrimEnd('\\');

            var filePath = $"{rootDir}\\{fileInfo.FilePath}";
            return filePath;
        }
    }
}
