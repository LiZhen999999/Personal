using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using System.Net;
using APChangeKitAPI.Models.Attributes;

namespace APChangeKitAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private SortedDictionary<string, object> _data;
        private Stopwatch _stopwatch;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task Invoke(HttpContext context)
        {
            _stopwatch.Restart();
            _data = new SortedDictionary<string, object>();

            HttpRequest request = context.Request;
            _data.Add("request.url", request.Path.ToString());
            _data.Add("request.remoteIpAddress", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            _data.Add("request.headers", request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList())));
            _data.Add("request.method", request.Method);
            _data.Add("request.executeStartTime", DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            // 获取请求body内容
            if (request.Method.ToLower().Equals("post"))
            {
                var isUpload = false;
                var endPoint = context.GetEndpoint();
                if(endPoint != null)
                {
                    var fileUploadAttribute = endPoint.Metadata.GetMetadata<FileUploadAttribute>();
                    isUpload = fileUploadAttribute != null;
                }

                //上传文件操作不记录内容
                if (!isUpload)
                {
                    // 启用倒带功能，让 Request.Body 可以再次读取
                    request.EnableBuffering();
                    var reader = new StreamReader(request.Body, Encoding.UTF8);
                    var contentFromBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                    _data.Add("request.body", contentFromBody);
                }
            }
            else if (request.Method.ToLower().Equals("get"))
            {
                _data.Add("request.body", request.QueryString.Value);
            }

            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                _data.Add("response.body", await GetResponse(context.Response));
                _data.Add("response.executeEndTime", DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                await responseBody.CopyToAsync(originalBodyStream);
            }

            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                _stopwatch.Stop();
                _data.Add("elaspedTime", _stopwatch.ElapsedMilliseconds + "ms");
                var json = JsonConvert.SerializeObject(_data);
                _logger.LogInformation(json);
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }

}
