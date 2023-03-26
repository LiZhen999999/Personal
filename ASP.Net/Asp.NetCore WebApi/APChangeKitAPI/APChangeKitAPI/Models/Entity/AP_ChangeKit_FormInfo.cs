using APChangeKitAPI.Models.Enums;
using System;

namespace APChangeKitAPI.Models.Entity
{

    /// <summary>
    ///  AP_ChangeKit_FormInfo    
    /// </summary>
    public class AP_ChangeKit_FormInfo
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
        /// EQID    
        /// </summary>
        public string EQID { get; set; }

        /// <summary>
        /// EQType    
        /// </summary>
        public string EQType { get; set; }

        /// <summary>
        /// EQKind    
        /// </summary>
        public string EQKind { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// Stage    
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// PTNLot    
        /// </summary>
        public string PTNLot { get; set; }

        /// <summary>
        /// DEVICE    
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// MASK    
        /// </summary>
        public string MASK { get; set; }

        /// <summary>
        /// 表单详情ID    
        /// </summary>
        public string DetailID { get; set; }

        /// <summary>
        /// 转机单类型    
        /// </summary>
        public string ChangKitType { get; set; }

        /// <summary>
        /// 对应的CheckList类型的ID    
        /// </summary>
        public string CheckListID { get; set; }

        /// <summary>
        /// 验证要求，仅记录信息    
        /// </summary>
        public string TestRequirement { get; set; }

        /// <summary>
        /// DB功能，仅记录信息    
        /// </summary>
        public string DBFunction { get; set; }

        /// <summary>
        /// 预计开始时间    
        /// </summary>
        public DateTime? EstimateStartTime { get; set; }

        /// <summary>
        /// 预计结束时间    
        /// </summary>
        public DateTime? EstimateEndTime { get; set; }

        /// <summary>
        /// 备注    
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// OA审批单号
        /// </summary>
        public string OAOrderNum { get; set; }

        /// <summary>
        /// 审核通过时间    
        /// </summary>
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 结束时间    
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 结束人    
        /// </summary>
        public string EndOP { get; set; }

        /// <summary>
        ///  0：无效 1：暂存 2：审批中 3：已驳回 4：填写CheckList 5：待检查 6：已完成 7：已取消    
        /// </summary>
        public APChangeKitStatus Status { get; set; }

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
