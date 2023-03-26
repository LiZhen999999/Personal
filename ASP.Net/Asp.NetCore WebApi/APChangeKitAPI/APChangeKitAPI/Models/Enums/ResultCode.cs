namespace APChangeKitAPI.Models.Enums
{
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,

        /// <summary>
        /// 无登录
        /// </summary>
        Identity = 3,

        /// <summary>
        /// 无权限
        /// </summary>
        Authentication = 4,

        /// <summary>
        /// 请求参数错误
        /// </summary>
        ParametersIncorrect = 5,

        /// <summary>
        /// 无数据
        /// </summary>
        NoData = 6,

        /// <summary>
        /// 未知异常
        /// </summary>
        Exception = 7
    }
}
