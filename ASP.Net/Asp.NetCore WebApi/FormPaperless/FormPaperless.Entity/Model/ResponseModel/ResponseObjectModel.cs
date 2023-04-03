using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
#nullable enable

namespace FormPaperless.Core
{
    public class ResponseObjectModel<T> where T : class
    {
        private ResultCode responseCode = ResultCode.Success;
        private string? message;
        private T data;

        public ResponseObjectModel()
        {

        }

        public ResponseObjectModel(T model)
        {
            data = model;
        }

        public string? Message { get => message; set => message = value; }
        public ResultCode Code { get => responseCode; set => responseCode = value; }
        public T Data { get => data; set => data = value; }


        /// <summary>
        /// 成功状态返回结果
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static ResponseObjectModel<T> SuccessResult(T data)
        {
            return new ResponseObjectModel<T> { Code = ResultCode.Success, Data = data };
        }
        /// <summary>
        /// 失败状态返回结果
        /// </summary>
        /// <param name="message">失败信息</param>
        /// <returns></returns>
        public static ResponseObjectModel<T> FailResult(string? message = null)
        {
            return new ResponseObjectModel<T> { Code = ResultCode.Fail, Message = message };
        }

        /// <summary>
        /// 异常状态返回结果
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <returns></returns>
        public static ResponseObjectModel<T> ErrorResult(string? message = null)
        {
            return new ResponseObjectModel<T> { Code = ResultCode.Error, Message = message };
        }

        /// <summary>
        /// 自定义状态返回结果
        /// </summary>
        /// <param name="resultCode">状态码</param>
        /// <param name="data">返回的数据</param>
        /// <param name="message">返回的信息</param>
        /// <returns></returns>
        public static ResponseObjectModel<T> Result(ResultCode resultCode, T data, string? message = null)
        {
            return new ResponseObjectModel<T> { Code = resultCode, Data = data, Message = message };
        }

        /// <summary>
        /// 增加隐式转换静态方法
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ResponseObjectModel<T>(T value)
        {
            return new ResponseObjectModel<T> { Data = value, Code = ResultCode.Success, Message = "成功!" };
        }
    }

    public enum ResultCode
    {
        [Description("请求成功")]
        Success = 1,
        [Description("请求失败")]
        Fail = 0,
        [Description("请求异常")]
        Error = -1
    }


}
