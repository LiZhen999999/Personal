using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///      
    /// </summary>
    public class AP_ChangeKit_Config
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; } = -1;

        /// <summary>
        /// 配置类型    
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 配置Key    
        /// </summary>
        public string ConfigKey { get; set; }

        /// <summary>
        /// 配置Value    
        /// </summary>
        public string ConfigValue { get; set; }

        /// <summary>
        /// 配置项顺序    
        /// </summary>
        public string ConfigIndex { get; set; }

        /// <summary>
        /// 1：有效 0：无效    
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 创建时间    
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人    
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间    
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 修改人    
        /// </summary>
        public string UpdateBy { get; set; }

    }

}
