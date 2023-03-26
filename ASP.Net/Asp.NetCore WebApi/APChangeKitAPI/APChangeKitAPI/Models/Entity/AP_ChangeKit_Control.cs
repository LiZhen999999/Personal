using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  AP_ChangeKit_Control    
    /// </summary>
    public class AP_ChangeKit_Control
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; } = -1;

        /// <summary>
        /// 输入类型    
        /// </summary>
        public string InputType { get; set; }

        /// <summary>
        /// 输入值类型
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// 卡控表达式    
        /// </summary>
        public string ControlExpression { get; set; }

        /// <summary>
        /// 卡控提示    
        /// </summary>
        public string ControlTip { get; set; }

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
