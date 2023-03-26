using DbUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TestProject.Common;
using TestProject.Models;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        private IDbUtils _dbUtils = new DbFactory(DatabaseType.SqlServer, "Data Source=szdevqas;Initial Catalog=qemsdb;uid=cimweb;pwd=CIMm00n;").CreateDbUtils();

        private Dictionary<string, string> _changeKitType = new Dictionary<string, string>()
        {
            { "NPI", "1" },
            { "换KIT", "2" },
            { "换程序换KIT", "4" },
            { "换模不换KIT(MD)", "5" },
            { "换程序不换KIT", "6" },
        };

        [TestMethod]
        public void CheckListTemplateSql()
        {
            var filePathList = new List<string>()
            {
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-DB.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-MK.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-OS.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-WB.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-BM.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-SS.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-BSG+TP+WM+DC.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-MD1.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-MD2.xlsx",
                @"D:\yuan\doc\需求\2022\12\开发输出\AP 改机单CheckList模板数据\已处理\APCheckList-MD3.xlsx",
            };
            var sqlSB = new StringBuilder();

            try
            {
                foreach (var filePath in filePathList)
                {
                    using var fileStream = File.OpenRead(filePath);
                    var bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    using Stream stream = new MemoryStream(bytes);
                    var ds = ExcelHelper.ExcelToDataSet(stream, ".xlsx", out string errorMsg);

                    foreach (DataTable dt in ds.Tables)
                    {
                        var eqTypeStr = dt.Rows[0]["EQTYEP"].ToString().Trim();
                        var changeType = dt.Rows[0]["ChangeType"].ToString().Trim().ToUpper();
                        var changeTypeID = _changeKitType[changeType];
                        var stageStr = dt.Rows[0]["Stage"].ToString().Trim().ToUpper();
                        var process = dt.Rows[0]["工序"].ToString().Trim();
                        foreach (var stage in stageStr.Split("/"))
                        {
                            foreach (var eqType in eqTypeStr.Split("/"))
                            {
                                if (!CheckEQType(eqType))
                                    throw new System.Exception($"EQ Type {eqType} 不存在");

                                var i = 0;
                                var blockIndex = 1;
                                var itemNameIndex = 1;
                                var lastModuleName = "";
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (i == 0 || i == 1)
                                    {
                                        i++;
                                        continue;
                                    }

                                    var moduleName = row[0].ToString().Trim().ToUpper();
                                    var itemNo = row[1].ToString().Trim();
                                    var itemContent = row[2].ToString().Trim().Replace("\n", " ");
                                    var itemStandard = row[3].ToString().Trim().Replace("\n", " ");
                                    var checkType = row[4].ToString().Trim().ToUpper();
                                    var controlID = GetControlID(checkType);
                                    if (!string.IsNullOrEmpty(lastModuleName) && lastModuleName != moduleName)
                                    {
                                        blockIndex++;
                                        itemNameIndex = 1;
                                    }

                                    sqlSB.Append($@"
INSERT INTO dbo.AP_ChangeKit_FormTemplate(TypeID, EQType, ChangeType, Stage, ItemBlock, ItemBlockName, ItemIndex, CheckItem, CheckStrandard, ControlID, CreateBy, UpdateBy, Process)
VALUES({changeTypeID}, -- TypeID - int
N'{eqType}', -- EQType - nvarchar(50)
N'{changeType}', -- ChangeType - nvarchar(50)
N'{stage}', -- Stage - nvarchar(50)
N'{blockIndex}', -- ItemBlock - nvarchar(10)
N'{moduleName}', -- ItemBlockName - nvarchar(100)
N'{itemNameIndex}', -- ItemIndex - nvarchar(10)
N'{itemContent}', -- CheckItem - nvarchar(2000)
N'{itemStandard}', -- CheckStrandard - nvarchar(2000)
{controlID}   , -- ControlID - int
'23600066', -- CreateBy - nvarchar(50)
'23600066', -- UpdateBy - nvarchar(50)
N'{process}') -- Process - nvarchar(10)
");
                                    if (string.IsNullOrEmpty(eqType) || string.IsNullOrEmpty(changeType) || string.IsNullOrEmpty(stage) || string.IsNullOrEmpty(moduleName) || string.IsNullOrEmpty(itemContent) || string.IsNullOrEmpty(itemStandard) || string.IsNullOrEmpty(controlID) || string.IsNullOrEmpty(process))
                                    {
                                        throw new System.Exception($"[{filePath}]解析失败");
                                    }

                                    lastModuleName = moduleName;
                                    itemNameIndex++;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
            

            var sql = sqlSB.ToString();
        }

        public bool CheckEQType(string eqType)
        {
            var sql = "SELECT * FROM dbo.ema_file WHERE ema07 = @eqType";
            var data = _dbUtils.QueryDataTable(sql, new { eqType = eqType });
            return data != null && data.Rows.Count > 0;
        }

        public string GetControlID(string value)
        {
            value = value.Trim().Replace(" ", "");
            if (value == "A")
                return "2";
            else if (value == "C")
                return "0";
            else if (value.Contains("B"))
            {
                value = value.Replace("B", "value").Replace("≤", "<=").Replace("≥", ">=").Replace("＜", "<").Replace("＞", ">");
                var control = GetControl(value);
                var controlID = 0;
                if (control == null)
                    controlID = InsertControl(new AP_ChangeKit_Control
                    {
                        InputType = "Input",
                        ControlExpression = value,
                        ControlTip = value.Replace("value", "值"),
                        CreateBy = "23600066",
                        UpdateBy = "23600066",
                        ValueType = "System.Double",
                    });
                else
                    controlID = control.ID;

                return controlID.ToString();
            }

            return "";
        }

        public AP_ChangeKit_Control GetControl(string expression)
        {
            var sql = $"SELECT * FROM dbo.AP_ChangeKit_Control WHERE ControlExpression = @ControlExpression;";
            return _dbUtils.Query<AP_ChangeKit_Control>(sql, new { ControlExpression = expression }).FirstOrDefault();
        }

        public int InsertControl(AP_ChangeKit_Control model)
        {
            var sql = @"
INSERT INTO dbo.AP_ChangeKit_Control(InputType, ControlExpression, ControlTip, CreateBy, UpdateBy, ValueType)
VALUES(@InputType, -- InputType - nvarchar(50)
@ControlExpression, -- ControlExpression - nvarchar(200)
@ControlTip, -- ControlTip - nvarchar(200)
@CreateBy, -- CreateBy - nvarchar(50)
@UpdateBy, -- UpdateBy - nvarchar(50)
@ValueType -- ValueType - nvarchar(50)
);SELECT @@IDENTITY";
            return _dbUtils.ExecuteScalar<int>(sql, new
            {
                InputType = model.InputType,
                ControlExpression = model.ControlExpression,
                ControlTip = model.ControlTip,
                CreateBy = model.CreateBy,
                UpdateBy = model.UpdateBy,
                ValueType = model.ValueType,
            });
        }
    }
}
