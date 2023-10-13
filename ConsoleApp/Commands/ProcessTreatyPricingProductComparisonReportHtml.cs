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
    public class ProcessTreatyPricingProductComparisonReportHtml : Command
    {
        public string FilePath { get; set; }

        public List<string> StringRows { get; set; }

        public List<int> BenefitCount { get; set; }

        public int ColumnPosition { get; set; }

        public Excel Excel { get; set; }

        public List<string> Errors { get; set; }

        public ProcessTreatyPricingProductComparisonReportHtml()
        {
            Title = "ProcessTreatyPricingProductComparisonReportHtml";
            Description = "To process product and benefit comparison report from html table";
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
            border[BaseExcel.XlBordersIndex.xlEdgeRight].LineStyle = BaseExcel.XlLineStyle.xlContinuous;
            border.Weight = 2d;

            //Format comparison columns
            int columnPosition = 2;

            foreach (int count in BenefitCount)
            {
                if (count > 0)
                {
                    int usedColumnPosition = columnPosition + count - 1;

                    BaseExcel.Range range1 = Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[1, usedColumnPosition], Excel.XWorkSheet.Cells[128, usedColumnPosition]];
                    BaseExcel.Borders border1 = range1.Borders;
                    border1[BaseExcel.XlBordersIndex.xlEdgeRight].LineStyle = BaseExcel.XlLineStyle.xlContinuous;

                    columnPosition += count;
                }
            }

            //Merge non-benefit columns
            columnPosition = 2;

            foreach (int count in BenefitCount)
            {
                if (count > 0)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        BaseExcel.Range range2 = Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[i + 1, columnPosition], Excel.XWorkSheet.Cells[i + 1, columnPosition + count - 1]];
                        range2.Merge();
                    }
                    columnPosition += count;
                }
            }
        }
    }
}