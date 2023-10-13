using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseExcel = Microsoft.Office.Interop.Excel;

namespace ConsoleApp.Commands
{
    public class ProcessTreatyPricingRateComparisonReportHtml : Command
    {
        public string ReportType { get; set; }

        public string FilePath { get; set; }

        public List<string> StringMergingRows { get; set; }

        public List<string> StringRows { get; set; }

        public List<string> StringRows2 { get; set; }

        public int ColumnPosition { get; set; }

        public Excel Excel { get; set; }

        public List<string> Errors { get; set; }

        public ProcessTreatyPricingRateComparisonReportHtml()
        {
            Title = "ProcessTreatyPricingRateComparisonReportHtml";
            Description = "To process rate comparison reports from html table";
            Errors = new List<string> { };
        }

        public override bool Validate()
        {
            if (!File.Exists(FilePath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, FilePath));
                return false;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();
            Process();
            PrintEnding();
        }

        public void Process()
        {
            string path = Util.GetTreatyPricingReportGenerationPath();
            Util.MakeDir(path, false);

            try
            {
                Excel = new Excel(FilePath, false);

                ProcessReport();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                Excel.Close();
                return;
            }

            PrintProcessCount();

            
            if (Errors.Count == 0)
            {
                Excel.FilePath = FilePath;
                Excel.Save();
            }
            else
            {
                foreach (string error in Errors)
                {
                    PrintError(error);
                }
                return;
            }
        }

        public void ProcessReport()
        {
            int rowNumber = 1;
            int mergeCount = ReportType == "RateTablePA" ? 2 : 7;

            foreach (string row in StringMergingRows)
            {
                int columnNumber = 1;
                var cellValues = row.Split('|').ToList().Select(s => s.Trim());

                foreach (string cellValue in cellValues)
                {
                    Excel.XWorkSheet.Cells[rowNumber, columnNumber].Value = "'" + cellValue;

                    if (columnNumber != 1 && ((columnNumber-1) % mergeCount == 0))
                    {
                        Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[rowNumber, columnNumber-(mergeCount-1)], Excel.XWorkSheet.Cells[rowNumber, columnNumber]].Merge();
                    }

                    columnNumber++;
                }

                rowNumber++;
            }

            foreach (string row in StringRows)
            {
                int columnNumber = 1;
                var cellValues = row.Split('|').ToList().Select(s => s.Trim());

                foreach (string cellValue in cellValues)
                {
                    Excel.XWorkSheet.Cells[rowNumber, columnNumber].Value = "'" + cellValue;

                    columnNumber++;
                }

                rowNumber++;
            }

            Excel.XWorkSheet.Columns.AutoFit();

            //Format first column
            BaseExcel.Range range = Excel.XWorkSheet.get_Range("A1:A" + (rowNumber - 1).ToString());
            BaseExcel.Borders border = range.Borders;
            range.Font.Bold = true;
            border.LineStyle = BaseExcel.XlLineStyle.xlContinuous;
            border.Weight = 2d;

            if (!StringRows2.IsNullOrEmpty())
            {
                rowNumber = 1;

                Excel.XWorkBook.Worksheets.Add();
                Excel.XWorkSheet = Excel.XWorkBook.Worksheets.get_Item(1);

                foreach (string row in StringRows2)
                {
                    int columnNumber = 1;
                    var cellValues = row.Split('|').ToList().Select(s => s.Trim());

                    foreach (string cellValue in cellValues)
                    {
                        Excel.XWorkSheet.Cells[rowNumber, columnNumber].Value = "'" + cellValue;

                        columnNumber++;
                    }

                    rowNumber++;
                }

                Excel.XWorkSheet.Columns.AutoFit();

                //Format first column
                BaseExcel.Range range2 = Excel.XWorkSheet.get_Range("A1:A" + (rowNumber - 1).ToString());
                BaseExcel.Borders border2 = range2.Borders;
                range2.Font.Bold = true;
                border2.LineStyle = BaseExcel.XlLineStyle.xlContinuous;
                border2.Weight = 2d;
            }
        }
    }
}