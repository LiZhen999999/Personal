using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;

namespace TestProject.Common
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    /// Microsoft Excel 11.0 Object Library
    public class ExcelHelper
    {
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="file">导入文件</param>
        /// <returns>List<T></returns>
        public static List<T> InputExcel(string filePath)
        {
            var list = new List<T>();
            IWorkbook workbook = new XSSFWorkbook(filePath);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow cellNum = sheet.GetRow(0);
            var propertys = typeof(T).GetProperties();
            string value = null;
            int num = cellNum.LastCellNum;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var obj = new T();
                for (int j = 0; j < num; j++)
                {
                    value = row.GetCell(j).ToString();
                    string str = (propertys[j].PropertyType).FullName;
                    if (str == "System.String")
                    {
                        propertys[j].SetValue(obj, value, null);
                    }
                    else if (str == "System.DateTime")
                    {
                        DateTime pdt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                        propertys[j].SetValue(obj, pdt, null);
                    }
                    else if (str == "System.Boolean")
                    {
                        bool pb = Convert.ToBoolean(value);
                        propertys[j].SetValue(obj, pb, null);
                    }
                    else if (str == "System.Int16")
                    {
                        short pi16 = Convert.ToInt16(value);
                        propertys[j].SetValue(obj, pi16, null);
                    }
                    else if (str == "System.Int32")
                    {
                        int pi32 = Convert.ToInt32(value);
                        propertys[j].SetValue(obj, pi32, null);
                    }
                    else if (str == "System.Int64")
                    {
                        long pi64 = Convert.ToInt64(value);
                        propertys[j].SetValue(obj, pi64, null);
                    }
                    else if (str == "System.Byte")
                    {
                        byte pb = Convert.ToByte(value);
                        propertys[j].SetValue(obj, pb, null);
                    }
                    else
                    {
                        propertys[j].SetValue(obj, null, null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// 将Excel单表转为Datatable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileType"></param>
        /// <param name="strMsg"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(Stream stream, string fileType, out string strMsg, string sheetName = null)
        {
            strMsg = "";
            DataTable dt = new DataTable();
            ISheet sheet = null;
            IWorkbook workbook = null;
            try
            {
                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    throw new Exception("传入的不是Excel文件！");
                }
                #endregion
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum;
                    for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            string cellValue = cell.StringCellValue.Trim();
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                DataColumn dataColumn = new DataColumn(cellValue);
                                dt.Columns.Add(dataColumn);
                            }
                        }
                    }
                    DataRow dataRow = null;
                    //遍历行
                    for (int j = sheet.FirstRowNum + 1; j <= sheet.LastRowNum; j++)
                    {
                        IRow row = sheet.GetRow(j);
                        dataRow = dt.NewRow();
                        if (row == null || row.FirstCellNum < 0)
                        {
                            continue;
                        }
                        //遍历列
                        for (int i = row.FirstCellNum; i < cellCount; i++)
                        {
                            ICell cellData = row.GetCell(i);
                            if (cellData != null)
                            {
                                //判断是否为数字型，必须加这个判断不然下面的日期判断会异常
                                if (cellData.CellType == CellType.Numeric)
                                {
                                    //判断是否日期类型
                                    if (DateUtil.IsCellDateFormatted(cellData))
                                    {
                                        dataRow[i] = cellData.DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[i] = cellData.ToString().Trim();
                                    }
                                }
                                else
                                {
                                    dataRow[i] = cellData.ToString().Trim();
                                }
                            }
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                else
                {
                    throw new Exception("没有获取到Excel中的数据表！");
                }
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// 将Excel单表转为Datatable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileType"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(Stream stream, string fileType, out string strMsg)
        {
            strMsg = "";
            var ds = new DataSet();
            IWorkbook workbook = null;
            try
            {
                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    throw new Exception("传入的不是Excel文件！");
                }
                #endregion

                int count = workbook.NumberOfSheets;

                for(var sheetCount = 0; sheetCount < count; sheetCount++)
                {
                    var dt = new DataTable();
                    var sheet = workbook.GetSheetAt(sheetCount);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellCount = firstRow.LastCellNum;
                        for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue.Trim();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    DataColumn dataColumn = new DataColumn(cellValue);
                                    dt.Columns.Add(dataColumn);
                                }
                            }
                        }
                        DataRow dataRow = null;
                        //遍历行
                        for (int j = sheet.FirstRowNum + 1; j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            dataRow = dt.NewRow();
                            if (row == null || row.FirstCellNum < 0)
                            {
                                continue;
                            }
                            //遍历列
                            for (int i = row.FirstCellNum; i < cellCount; i++)
                            {
                                ICell cellData = row.GetCell(i);
                                if (cellData != null)
                                {
                                    //判断是否为数字型，必须加这个判断不然下面的日期判断会异常
                                    if (cellData.CellType == CellType.Numeric)
                                    {
                                        //判断是否日期类型
                                        if (DateUtil.IsCellDateFormatted(cellData))
                                        {
                                            dataRow[i] = cellData.DateCellValue;
                                        }
                                        else
                                        {
                                            dataRow[i] = cellData.ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        dataRow[i] = cellData.ToString().Trim();
                                    }
                                }
                            }
                            dt.Rows.Add(dataRow);
                        }

                        ds.Tables.Add(dt);
                    }
                    else
                    {
                        throw new Exception("没有获取到Excel中的数据表！");
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
            }
            return ds;
        }
    }
}
