using APChangeKitAPI.Models.Enums;
using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  AP_ChangeKit_Log    
    /// </summary>
    public class AP_ChangeKit_Log
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 操作类型    
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// 转机单号    
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 日志来源    
        /// </summary>
        public LogSource LogSource { get; set; }

        /// <summary>
        /// 请求参数    
        /// </summary>
        public string RequestParam { get; set; }

        /// <summary>
        /// 结果    
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 创建时间    
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人    
        /// </summary>
        public string CreateBy { get; set; }

    }

}
