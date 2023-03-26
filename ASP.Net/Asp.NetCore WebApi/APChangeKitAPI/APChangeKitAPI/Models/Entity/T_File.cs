using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  T_File    
    /// </summary>
    public class T_File
    {
        /// <summary>
        /// 自增主键    
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 真实文件名
        /// </summary>
        public string TrueFileName { get; set; }

        /// <summary>
        /// 文件名    
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件后缀    
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件路径    
        /// </summary>
        public string FilePath { get; set; }

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
