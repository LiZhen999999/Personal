using System.Collections.Generic;

namespace APChangeKitAPI.Models.DTO
{
    public class APChangeKitChkListAddReq
    {
        /// <summary>
        /// 转机单号    
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string User { get; set; }

        public List<APChangeKitChkListItemAdd> Modules { get; set; }
    }

    public class APChangeKitChkListItemAdd
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

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
    }
}
