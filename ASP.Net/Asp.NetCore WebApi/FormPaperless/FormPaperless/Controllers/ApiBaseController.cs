using FormPaperless.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormPaperless.Controllers
{
    public class ApiControllerBase : Controller
    {
        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        protected ResponseObjectModel<T> SuccessResult<T>(T data) where T : class
        {
            return ResponseObjectModel<T>.SuccessResult(data);
        }

        /// <summary>
        /// 失败状态返回结果
        /// </summary>
        /// <param name="message">失败信息</param>
        /// <returns></returns>
        protected ResponseObjectModel<T> FailResult<T>(string? message = null) where T : class
        {
            return ResponseObjectModel<T>.FailResult(message);
        }

        /// <summary>
        /// 异常状态返回结果
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <returns></returns>
        protected ResponseObjectModel<T> ErrorResult<T>(string? message = null) where T : class
        {
            return ResponseObjectModel<T>.ErrorResult(message);
        }

        /// <summary>
        /// 自定义状态返回结果
        /// </summary>
        /// <param name="resultCode">状态码</param>
        /// <param name="data">返回的数据</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        protected ResponseObjectModel<T> Result<T>(ResultCode resultCode, T data, string? message = null) where T : class
        {
            return ResponseObjectModel<T>.Result(resultCode, data, message);
        }
    }
}
