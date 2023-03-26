using APChangeKitAPI.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using APChangeKitAPI.Models;

namespace APChangeKitAPI.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //如果异常没有被处理则进行处理
            if (!context.ExceptionHandled)
            {
                //日志入库
                var ex = context.Exception;
                //报错地址
                var url = context.HttpContext.Request.Host + context.HttpContext.Request.Path;
                //报错参数
                var parameter = context.HttpContext.Request.QueryString.ToString();
                //报错请求方式
                var method = context.HttpContext.Request.Method.ToString();
                var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                //写入日志
                _logger.LogError($"请求IP:{ip},报错地址:{url},请求方式：{method},参数:{parameter},异常描述：{ex.Message},堆栈信息：{ex.StackTrace}");
                //定义返回信息
                var res = Result<string>.Exception(ex.Message);
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    //设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(res)
                };
            }
            //设置为true，表示异常已经被处理了
            context.ExceptionHandled = true;
        }
    }
}
