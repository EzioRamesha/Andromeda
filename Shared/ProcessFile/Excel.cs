using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BaseExcel = Microsoft.Office.Interop.Excel;

namespace Shared.ProcessFile
{
    public class Excel : IProcessFile
    {
        public BaseExcel.Application XApp { get; set; }
        public BaseExcel.Workbook XWorkBook { get; set; }
        public BaseExcel.Worksheet XWorkSheet { get; set; }
        public BaseExcel.Range XRange { get; set; }
        public object[,] Values { get; set; }
        public object[,] Values2 { get; set; }

        public string FilePath { get; set; }

        // Read number of row at a time
        public int RowRead { get; set; } = 1000;
        public int RowPointer { get; set; } = 0;
        public int RowCount { get; set; } = 0;

        // This is reading row index
        public int RowIndex { get; set; }
        // This is reading col index
        public int ColIndex { get; set; }

        // This is excel col name e.g. A,B,C
        public string ColName { get; set; }

        // Total readable row
        public int TotalRow { get; set; }

        // Total readable col
        public int TotalCol { get; set; }

        public int? MaxRow { get; set; }
        public int? MaxCol { get; set; }

        // Determine if File is Open
        public bool IsOpen { get; set; }

        //Generate Excel
        public string TemplateFilePath { get; set; }

        public object MisValue { get; set; } = System.Reflection.Missing.Value;

        public const string FormatDate = "[$-809]dd-mmm-yyyy;@";
        public const string FormatInteger = "#,##0_ ;[Red]-#,##0";
        public const string FormatAmount = "#,##0.00;[Red]-#,##0.00";
        public const string FormatGeneral = "General";

        public Excel()
        {
        }

        public Excel(string filepath, bool readOnly = true, object worksheet = null, int rowRead = 0)
        {
            if (readOnly)
            {
                if (!File.Exists(filepath))
                {
                    throw new Exception(string.Format(MessageBag.FileNotExists, filepath));
                }
            }
            else
            {
                var finfo = new FileInfo(filepath);
                if (!Directory.Exists(finfo.Directory.FullName))
                {
                    throw new Exception(string.Format(MessageBag.DirectoryNotExists, finfo.Directory.FullName));
                }
            }

            if (worksheet is string @string)
            {
                if (!@string.StartsWith("0") && int.TryParse(@string, out int sheetIndex))
                {
                    if (sheetIndex < 1000) // e.g. 201911 can not convert to int. Because it is string.
                        worksheet = sheetIndex;
                }
            }

            FilePath = filepath;
            XApp = new BaseExcel.Application();

            if (readOnly)
            {
                XWorkBook = XApp.Workbooks.Open(
                    FilePath,
                    0,
                    readOnly,
                    5,
                    "",
                    "",
                    true,
                    BaseExcel.XlPlatform.xlWindows,
                    "\t",
                    false,
                    false,
                    0,
                    true,
                    1,
                    0
                );

                IsOpen = true;

                try
                {
                    XWorkSheet = (BaseExcel.Worksheet)XWorkBook.Worksheets[worksheet ?? 1];
                }
                catch (COMException)
                {
                    Close();
                    throw new Exception(string.Format("Worksheet not found: {0}", worksheet ?? 1));
                }

                XRange = XWorkSheet.UsedRange;

                TotalRow = XRange.Rows.Count;
                if (XRange.Rows[1].Row is int firstRowIndex)
                {
                    TotalRow += firstRowIndex - 1;
                }

                TotalCol = XRange.Columns.Count;
                if (XRange.Columns[1].Column is int firstColIndex)
                {
                    TotalCol += firstColIndex - 1;
                }

                if (rowRead != 0)
                    RowRead = rowRead;

                GetRange();
            }
            else
            {
                XApp.Visible = false;
                XApp.DisplayAlerts = false; // Disable showing alert message of overwritting of existing file

                if (File.Exists(filepath))
                {
                    XWorkBook = XApp.Workbooks.Open(filepath, 0, false, 5, "", "", true, BaseExcel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    XWorkSheet = (BaseExcel.Worksheet)XWorkBook.Worksheets.get_Item(1);
                }
                else
                {
                    XWorkBook = XApp.Workbooks.Add();
                    XWorkSheet = XWorkBook.Worksheets.get_Item(1); // Get first sheet
                }
                IsOpen = true;
            }
        }

        public Excel(string templateFilepath, string filepath, int rowIndex)
        {
            // Generate Excel
            if (!File.Exists(templateFilepath))
            {
                throw new Exception(string.Format(MessageBag.FileNotExists, templateFilepath));
            }
            TemplateFilePath = templateFilepath;
            FilePath = filepath;
            Util.MakeDir(FilePath);
            RowIndex = rowIndex;
        }

        public static void GetNumberFormatByDateType()
        {

        }

        public void GetRange()
        {
            var c1 = XWorkSheet.Cells[RowPointer == 0 ? 1 : RowPointer + 1, 1];

            int c2Row = RowPointer + RowRead;
            if (c2Row > TotalRow)
                c2Row = TotalRow;
            var c2 = XWorkSheet.Cells[c2Row, TotalCol];

            var r = (BaseExcel.Range)XWorkSheet.Range[c1, c2];
            Values = r.Value;
            Values2 = r.Value2;

            RowPointer += RowRead;

            RowCount++;
        }

        public virtual int? GetNextRow()
        {
            if (RowIndex == 0 && TotalRow > 0)
            {
                RowIndex = 1;
                GetCell();
                return RowIndex;
            }
            RowIndex++;

            if (MaxRow != 0 && MaxRow < RowIndex)
                return null;

            if (RowIndex <= TotalRow)
            {
                GetCell();
                return RowIndex;
            }

            return null;
        }

        public List<Row> GetNextRows(int noOfRows = 1)
        {
            bool everyRowEmpty = true;
            var rows = new List<Row> { };
            for (int i = 0; i < noOfRows; i++)
            {
                var r = GetNextRow();
                if (r != null)
                {
                    everyRowEmpty = false;

                    int? rowIndex = GetRowIndex();
                    bool isEmpty = true;

                    var cols = new List<Column> { };
                    while (GetNextCol() != null)
                    {
                        bool hasData = true;
                        var value = GetValue();
                        if (value == null || (value is string && string.IsNullOrEmpty(value)))
                        {
                            hasData = false;
                        }
                        if (hasData)
                        {
                            isEmpty = false;
                        }

                        int? colIndex = GetColIndex();
                        cols.Add(new Column
                        {
                            RowIndex = rowIndex != null ? rowIndex.Value : 0,
                            ColIndex = colIndex != null ? colIndex.Value : 0,
                            ColName = ColName,
                            Value = value,
                            Value2 = GetValue2(),
                            IsExcel = true,
                        });
                    }

                    if (isEmpty)
                    {
                        rows.Add(new Row
                        {
                            IsEmpty = true,
                        });
                    }
                    else
                    {
                        rows.Add(new Row
                        {
                            RowIndex = rowIndex != null ? rowIndex.Value : 0,
                            Columns = cols,
                            IsEmpty = false,
                        });
                    }
                }
                else
                {
                    rows.Add(new Row
                    {
                        IsEmpty = true,
                        IsEnd = true,
                    });
                }
            }
            if (everyRowEmpty)
                return null;

            return rows;
        }

        public virtual int? GetNextCol()
        {
            if (RowIndex == 0)
                return null;
            if (TotalRow <= 0)
                return null;

            if (ColIndex == 0 && TotalCol > 0)
            {
                ColIndex = 1;
                GetCell();
                return ColIndex;
            }

            ColIndex++;

            if (MaxCol != 0 && MaxCol < ColIndex)
            {
                ColIndex = 0;
                return null;
            }

            if (ColIndex <= TotalCol)
            {
                GetCell();
                return ColIndex;
            }

            ColIndex = 0;
            return null;
        }

        public void GetCell()
        {
            ColName = GetExcelColumnName(ColIndex != 0 ? ColIndex : 1);

            if (RowIndex == RowPointer + 1)
            {
                GetRange();
            }
        }

        public int? GetRowIndex()
        {
            return RowIndex;
        }

        public int? GetColIndex()
        {
            return ColIndex;
        }

        public int? GetTotalCol()
        {
            return TotalCol;
        }

        public int? GetDelimiter()
        {
            return null;
        }

        public virtual dynamic GetValue()
        {
            if (Values != null)
            {
                var rowIndex = RowIndex;
                if (RowCount != 1)
                    rowIndex -= (RowRead * (RowCount - 1));

                return Values[rowIndex, ColIndex != 0 ? ColIndex : 1];
            }
            return null;
        }

        public dynamic GetValue2()
        {
            if (Values2 != null)
            {
                var rowIndex = RowIndex;
                if (RowCount != 1)
                    rowIndex -= (RowRead * (RowCount - 1));

                return Values2[rowIndex, ColIndex != 0 ? ColIndex : 1];
            }
            return null;
        }

        public string GetTotal()
        {
            return string.Format(
                "TotalRow: {0}, TotalCol: {1}",
                TotalRow,
                TotalCol
            );
        }

        public void SetNumberFormat(int rowIndex, int colIndex, string format)
        {
            XWorkSheet.Cells[rowIndex, colIndex].EntireColumn.NumberFormat = format;
        }

        public void SetAmountValidation(int rowIndex, int colIndex)
        {
            BaseExcel.Range entireCol = XWorkSheet.Cells[rowIndex, colIndex].EntireColumn;
            entireCol.Validation.Delete();

            entireCol.Validation.Add(
                BaseExcel.XlDVType.xlValidateDecimal,
                BaseExcel.XlDVAlertStyle.xlValidAlertStop,
                BaseExcel.XlFormatConditionOperator.xlBetween,
                decimal.MinValue,
                decimal.MaxValue
            );

            entireCol.Validation.IgnoreBlank = true;

            // Display error message
            entireCol.Validation.ErrorMessage = "Entry is not a valid number";
            entireCol.Validation.ErrorTitle = "Error - invalid entry";
            entireCol.Validation.ShowError = true;

            // use these if you want to display a message each time user activates this cell
            entireCol.Validation.InputTitle = "Entry Rule"; // a message box title
            entireCol.Validation.InputMessage = "You must enter a valid decimal"; // message to instruct user what to do
            entireCol.Validation.ShowInput = true;
        }

        public void SetIntegerValidation(int rowIndex, int colIndex)
        {
            BaseExcel.Range entireCol = XWorkSheet.Cells[rowIndex, colIndex].EntireColumn;
            entireCol.Validation.Delete();

            entireCol.Validation.Add(
                BaseExcel.XlDVType.xlValidateWholeNumber,
                BaseExcel.XlDVAlertStyle.xlValidAlertStop,
                BaseExcel.XlFormatConditionOperator.xlBetween,
                int.MinValue,
                int.MaxValue
            );

            entireCol.Validation.IgnoreBlank = true;

            // Display error message
            entireCol.Validation.ErrorMessage = "Entry is not a numeric";
            entireCol.Validation.ErrorTitle = "Invalid Numeric";
            entireCol.Validation.ShowError = true;

            // use these if you want to display a message each time user activates this cell
            entireCol.Validation.InputTitle = "Entry Rule"; // a message box title
            entireCol.Validation.InputMessage = "You must enter a valid number"; // message to instruct user what to do
            entireCol.Validation.ShowInput = true;
        }

        public void SetQuarterValidation(int rowIndex, int colIndex, string property)
        {
            string letter = "";
            switch (property)
            {
                case "SoaQuarter":
                    letter = "G";
                    break;
                case "RiskQuarter":
                    letter = "H";
                    break;
            }

            BaseExcel.Range entireCol = XWorkSheet.Cells[rowIndex, colIndex].EntireColumn;
            entireCol.Validation.Delete();

            entireCol.Validation.Add(
                BaseExcel.XlDVType.xlValidateCustom,
                BaseExcel.XlDVAlertStyle.xlValidAlertStop,
                Type.Missing,
                "=AND(ISNUMBER(FIND(\"Q\", " + string.Format("{0}{1}", letter, (rowIndex - 1)) + ")),LEN(" + string.Format("{0}{1}", letter, (rowIndex - 1)) + ")=4)",
                Type.Missing
            );

            entireCol.Validation.IgnoreBlank = true;

            // Display error message
            entireCol.Validation.ErrorMessage = "Entry is not a valid format";
            entireCol.Validation.ErrorTitle = "Error - invalid format";
            entireCol.Validation.ShowError = true;

            entireCol.Validation.InputTitle = "Entry Rule";
            entireCol.Validation.InputMessage = "You must enter a valid format for quarter eg: 15Q1";
            entireCol.Validation.ShowInput = true;
        }

        public void ResetColLengths()
        {

        }

        public void AddColLengths(int length)
        {

        }

        public virtual void Close()
        {
            if (!IsOpen)
                return;

            if (XRange != null)
            {
                Marshal.ReleaseComObject(XRange);
            }
            if (XWorkSheet != null)
            {
                Marshal.ReleaseComObject(XWorkSheet);
            }
            if (XWorkBook != null)
            {
                XWorkBook.Close();
                Marshal.ReleaseComObject(XWorkBook);
            }
            if (XApp != null)
            {
                var p = GetExcelProcess();

                XApp.Quit();
                Marshal.ReleaseComObject(XApp);

                p.Kill();
            }
            IsOpen = false;
        }

        public string GetExcelColumnName(int? col)
        {
            if (col == null)
                return null;

            int dividend = col.Value;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        public Process GetExcelProcess()
        {
            GetWindowThreadProcessId(XApp.Hwnd, out int id);
            return Process.GetProcessById(id);
        }

        public void OpenTemplate()
        {
            XApp = new BaseExcel.Application
            {
                DisplayAlerts = false //Ignore message
            };
            XWorkBook = XApp.Workbooks.Open(TemplateFilePath, 0, false, 5, "", "", true, BaseExcel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            XWorkSheet = (BaseExcel.Worksheet)XWorkBook.Worksheets.get_Item(1);
            IsOpen = true;
        }

        public void WriteCell(int rowIndex, int colIndex, object value)
        {
            XWorkSheet.Cells[rowIndex, colIndex] = value;
        }

        public void Save()
        {
            XWorkBook.SaveAs(FilePath, BaseExcel.XlFileFormat.xlOpenXMLWorkbook, MisValue, MisValue, MisValue, MisValue, BaseExcel.XlSaveAsAccessMode.xlExclusive, MisValue, MisValue, MisValue, MisValue, MisValue);
            Close();
        }

        public void SaveMacroEnabled(bool checkRow = false, int? row = null)
        {
            if ((checkRow && row.HasValue && RowIndex > row.Value) || !checkRow)
                XWorkBook.SaveAs(FilePath, BaseExcel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled, MisValue, MisValue, MisValue, MisValue, BaseExcel.XlSaveAsAccessMode.xlExclusive, MisValue, MisValue, MisValue, MisValue, MisValue);
            Close();
        }
    }
}
