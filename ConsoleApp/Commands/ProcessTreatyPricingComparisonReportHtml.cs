using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using BaseExcel = Microsoft.Office.Interop.Excel;

namespace ConsoleApp.Commands
{
    public class ProcessTreatyPricingComparisonReportHtml : Command
    {
        public string ReportType { get; set; }

        public string FilePath { get; set; }

        public List<string> StringRows { get; set; }

        public int ColumnPosition { get; set; }

        public Excel Excel { get; set; }

        public List<string> Errors { get; set; }

        public ProcessTreatyPricingComparisonReportHtml()
        {
            Title = "ProcessTreatyPricingComparisonReportHtml";
            Description = "To process comparison reports from html table";
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

            foreach (string row in StringRows)
            {
                int columnNumber = 1;
                var cellValues = row.Split('|').ToList().Select(s => s.Trim());

                foreach (string cellValue in cellValues)
                {
                    //For medical table comparison legend listing
                    string trimmedCellValue = string.Join("\n", cellValue.Split('\n').Select(s => s.Trim()));

                    Excel.XWorkSheet.Cells[rowNumber, columnNumber].Value = "'" + trimmedCellValue;

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
        }
    }
}