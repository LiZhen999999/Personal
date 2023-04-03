using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core.Model
{
    public class GenericFormModel
    {
        public string Id { get; set; }

        public string FormName { get; set; }

        public int Version { get; set; }

        public string FormTableName { get; set; }

        public string FormEnName { get; set; }

        public string FormCnName { get; set; }

        public string Document { get; set; }

        public List<GenericFormContentModel> FormContents { get => formContents; set => formContents = value; }

        List<GenericFormContentModel> formContents = new List<GenericFormContentModel>();
    }

    public class GenericFormContentModel
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// 项目项次
        /// </summary>
        public int ItemNo { get; set; }

        /// <summary>
        /// 点检内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 规格->用作输入参数的校验
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 超过规格是否允许提交
        /// </summary>
        public bool SpecSubmissionControl { get; set; }

        /// <summary>
        /// 输入控件的类型
        /// </summary>
        public InputType Type { get; set; }

        /// <summary>
        /// 建议输入项->如下拉菜单的选择项
        /// </summary>
        public List<string> InputSuggestions { get; set; }

        /// <summary>
        /// 对应的列名
        /// </summary>
        public string ItemColumnName { get; set; }

        /// <summary>
        /// 是否为关键列
        /// </summary>
        public bool IsKeyColumn { get; set; }

        /// <summary>
        /// 输入的点检数据
        /// </summary>
        public string Data { get; set; }
    }
}
