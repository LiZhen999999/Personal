using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  AP_ChangeKit_FormDetail    
    /// </summary>
    public class AP_ChangeKit_FormDetail
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 转机单号    
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 项次    
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
        /// 检查项目    
        /// </summary>
        public string CheckItem { get; set; }

        /// <summary>
        /// 检查标准    
        /// </summary>
        public string CheckStandard { get; set; }

        /// <summary>
        /// 检查结果    
        /// </summary>
        public string CheckResult { get; set; }

        /// <summary>
        /// 文件  
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 卡控ID
        /// </summary>
        public int ControlID { get; set; }

        /// <summary>
        /// 备注信息    
        /// </summary>
        public string Remark { get; set; }

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
