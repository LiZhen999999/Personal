namespace APChangeKitAPI.Models.Enums
{
    public enum APChangeKitStatus
    {
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 暂存
        /// </summary>
        Create = 1,
        /// <summary>
        /// 审核中
        /// </summary>
        Approve = 2,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject = 3,
        /// <summary>
        /// 填写CheckList
        /// </summary>
        CheckList = 4,
        /// <summary>
        /// CheckList审核
        /// </summary>
        CheckListApprove = 5,
        /// <summary>
        /// 已完成
        /// </summary>
        Complete = 6,
        /// <summary>
        /// 已取消
        /// </summary>
        Cancel = 7
    }

    public partial class EnumUtils
    {
        public static string ConvertString(APChangeKitStatus value)
        {
            switch (value)
            {
                case APChangeKitStatus.Invalid:
                    return "无效";
                case APChangeKitStatus.Create:
                    return "暂存";
                case APChangeKitStatus.Approve:
                    return "审核中";
                case APChangeKitStatus.Reject:
                    return "驳回";
                case APChangeKitStatus.CheckList:
                    return "填写CheckList";
                case APChangeKitStatus.CheckListApprove:
                    return "CheckList审核";
                case APChangeKitStatus.Complete:
                    return "已完成";
                case APChangeKitStatus.Cancel:
                    return "已取消";
                default:
                    return "未知状态";
            }
        }
    }
}
