using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    [SugarTable("tbFormMasterBody")]
    public class FormMasterBody
    {
        [Description("表单主键ID")]
        [SugarColumn(ColumnName = "FMB_Id")]
        /// <summary>
        /// 表单基础信息ID
        /// </summary>
        public string Id { get; set; }


        [Description("项目")]
        [SugarColumn(ColumnName = "FMB_Project")]
        /// <summary>
        /// 项目
        /// </summary>
        public string Project { get; set; }

        [Description("项目项次")]
        [SugarColumn(ColumnName = "FMB_ItemNo")]
        /// <summary>
        /// 项目项次
        /// </summary>
        public int ItemNo { get; set; }

        [Description("点检内容")]
        [SugarColumn(ColumnName = "FMB_Content")]
        /// <summary>
        /// 点检内容
        /// </summary>
        public string Content { get; set; }

        [Description("详细描述")]
        [SugarColumn(ColumnName = "FMB_Description")]
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }

        [Description("规格->用作输入参数的校验")]
        [SugarColumn(ColumnName = "FMB_Specification")]
        /// <summary>
        /// 规格->用作输入参数的校验
        /// </summary>
        public string Specification { get; set; }

        [Description("规格不符合是否不能提交")]
        [SugarColumn(ColumnName = "FMB_SpecSubmissionControl")]
        public bool SpecSubmissionControl { get; set; }

        [Description("输入控件的类型")]
        [SugarColumn(ColumnName = "FMB_ControlType")]
        /// <summary>
        /// 输入控件的类型
        /// </summary>
        public InputType Type { get; set; }

        [Description("建议输入项->如下拉菜单的选择项")]
        [SugarColumn(ColumnName = "FMB_InputSuggestions")]
        /// <summary>
        /// 建议输入项->如下拉菜单的选择项
        /// </summary>
        public string InputSuggestions { get; set; }

        [Description("输入的点检数据")]
        [SugarColumn(ColumnName = "FMB_Data")]
        /// <summary>
        /// 输入的点检数据
        /// </summary>
        public string Data { get; set; }

        [Description("关键列对应的列名")]
        [SugarColumn(ColumnName = "FMB_KeyColumnName")]
        /// <summary>
        /// 对应的列名
        /// </summary>
        public string ItemColumnName { get; set; }

        [Description("是否为关键列")]
        [SugarColumn(ColumnName = "FMB_IsKeyColumn")]
        /// <summary>
        /// 是否为关键列
        /// </summary>
        public bool IsKeyColumn { get; set; }
    }

    /// <summary>
    /// 定义输入类型枚举
    /// </summary>
    public enum InputType
    {
        TextBox = 1,
        CheckBox = 2,
        Select = 3,
        MutiplySelect = 4,
        DatePicker = 5
    }
}
