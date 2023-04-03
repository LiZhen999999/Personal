using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace FormPaperless.Core
{
    [SugarTable("tbFormMasterHead")]
    public class FormMasterHead
    {
        [Description("表单主键ID")]
        [SugarColumn(ColumnName = "FMH_Id")]
        public string Id { get; set; }

        [Description("表单名")]
        [SugarColumn(ColumnName = "FMH_FormName")]
        public string FormName { get; set; }

        [Description("版本")]
        [SugarColumn(ColumnName = "FMH_Version")]
        public int Version { get; set; }

        [Description("数据库表名")]
        [SugarColumn(ColumnName = "FMH_FormTableName")]
        public string FormTableName { get; set; }

        [Description("表单英文名")]
        [SugarColumn(ColumnName = "FMH_FormEnName")]
        public string FormEnName { get; set; }

        [Description("表单中文名")]
        [SugarColumn(ColumnName = "FMH_FormCnName")]
        public string FormCnName { get; set; }

        [Description("表单文件")]
        [SugarColumn(ColumnName = "FMH_Doc")]
        public string Document { get; set; }

        [Description("创建时间")]
        [SugarColumn(ColumnName = "FMH_CreateTime")]
        public DateTime CreateTime { get; set; }

        [Description("创建人")]
        [SugarColumn(ColumnName = "FMH_Creator")]
        public string Creator { get; set; }

        [Description("生效否")]
        [SugarColumn(ColumnName = "FMH_Effect")]
        public bool Effect { get; set; }
    }
}
