using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  AP_ChangeKit_FormTemplate    
    /// </summary>
    public class AP_ChangeKit_FormTemplate
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// 机台类型    
        /// </summary>
        public string EQType { get; set; }

        /// <summary>
        /// 改机项目    
        /// </summary>
        public string ChangeType { get; set; }

        /// <summary>
        /// 工站    
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 顺序    
        /// </summary>
        public string ItemIndex { get; set; }

        /// <summary>
        /// 模块    
        /// </summary>
        public string ItemBlock { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ItemBlockName { get; set; }

        /// <summary>
        /// 检查内容    
        /// </summary>
        public string CheckItem { get; set; }

        /// <summary>
        /// 检查标准    
        /// </summary>
        public string CheckStrandard { get; set; }

        /// <summary>
        /// 卡控ID
        /// </summary>
        public int ControlID { get; set; }

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
