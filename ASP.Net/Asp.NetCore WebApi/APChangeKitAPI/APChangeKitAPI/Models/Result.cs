using APChangeKitAPI.Models.Enums;

namespace APChangeKitAPI.Models
{

    /// <summary>
    /// 结果数据
    /// </summary>
    public class Result<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResultCode Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <returns></returns>
        public static Result<T> Instance()
        {
            return new Result<T>();
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Instance(T data)
        {
            return new Result<T>() { Data = data };
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Instance(string msg)
        {
            return new Result<T>() { Message = msg };
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Result<T> Instance(ResultCode code)
        {
            return new Result<T>() { Code = code };
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Instance(ResultCode code, string msg)
        {
            return new Result<T>() { Code = code, Message = msg };
        }

        /// <summary>
        /// 实例化错误对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Error(string msg)
        {
            return new Result<T>() { Code = ResultCode.Error, Message = msg };
        }

        /// <summary>
        /// 实例化错误对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Exception(string msg)
        {
            return new Result<T>() { Code = ResultCode.Exception, Message = msg };
        }

        /// <summary>
        /// 实例化成功对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Success(string msg = "OK")
        {
            return new Result<T>() { Code = ResultCode.Success, Message = msg };
        }

        /// <summary>
        /// 设置返回数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Result<T> SetData(T obj)
        {
            this.Data = obj;
            return this;
        }

        /// <summary>
        /// 设置返回码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result<T> SetCode(ResultCode code)
        {
            this.Code = code;
            return this;
        }
    }
}
