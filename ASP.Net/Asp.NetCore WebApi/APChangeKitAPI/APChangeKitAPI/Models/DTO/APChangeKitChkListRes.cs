using APChangeKitAPI.Models.Enums;
using System.Collections.Generic;

namespace APChangeKitAPI.Models.DTO
{
    public class APChangeKitChkListRes
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
        /// EQID    
        /// </summary>
        public string EQID { get; set; }

        /// <summary>
        /// Stage    
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public APChangeKitStatus Status { get; set; }

        /// <summary>
        /// 转机单类型    
        /// </summary>
        public string ChangKitType { get; set; }

        /// <summary>
        /// 改机前-程序名    
        /// </summary>
        public string Before_Program { get; set; }

        /// <summary>
        /// 改机前-线图    
        /// </summary>
        public string Before_Map { get; set; }

        /// <summary>
        /// 改机前-Group    
        /// </summary>
        public string Before_Group { get; set; }

        /// <summary>
        /// 改机前-BOM    
        /// </summary>
        public string Before_Bom { get; set; }

        /// <summary>
        /// 改机后-程序名    
        /// </summary>
        public string After_Program { get; set; }

        /// <summary>
        /// 改机后-线图    
        /// </summary>
        public string After_Map { get; set; }

        /// <summary>
        /// 改机后-Group    
        /// </summary>
        public string After_Group { get; set; }

        /// <summary>
        /// 改机后-BOM    
        /// </summary>
        public string After_Bom { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }

        public List<APChangeKitChkListItem> Modules { get; set; }
    }

    public class APChangeKitChkListItem
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 模块    
        /// </summary>
        public string ItemBlock { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ItemBlockName { get; set; }

        /// <summary>
        /// 项次    
        /// </summary>
        public string ItemIndex { get; set; }

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
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 输入类型
        /// </summary>
        public string InputType { get; set; }

        /// <summary>
        /// 卡控信息
        /// </summary>
        public string ControlTip { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
    }
}
