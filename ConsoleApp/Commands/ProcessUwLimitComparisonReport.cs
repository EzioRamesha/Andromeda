using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseExcel = Microsoft.Office.Interop.Excel;

namespace ConsoleApp.Commands
{
    public class ProcessUwLimitComparisonReport : Command
    {
        public string FilePath { get; set; }

        public string FolderPath { get; set; }

        public TreatyPricingReportGenerationBo ReportGenerationBo { get; set; }

        public Excel Excel { get; set; }

        public List<Column> Column { get; set; }

        public int ColumnCount { get; set; }

        public List<string> Errors { get; set; }

        public int UserId { get; set; }

        public ProcessUwLimitComparisonReport()
        {
            Title = "ProcessUwLimitComparisonReport";
            Description = "To process underwriting limit comparison into excel file";
            FolderPath = Util.GetTreatyPricingReportGenerationPath();
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
            UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusProcessing, "Processing Underwriting Limit Comparison");

            ProcessMain();

            PrintProcessCount();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void ProcessMain()
        {
            string fileName = "UwLimitComparison" + ReportGenerationBo.Id.ToString().PadLeft(5, '0') + ".xlsx";
            string hashFileName = Hash.HashFileName(fileName);
            string folderPath = FolderPath;
            string path = Path.Combine(folderPath, hashFileName);

            try
            {
                Excel = new Excel(path, false);
                //Excel.XApp = new BaseExcel.Application();
                //Excel.XWorkBook = Excel.XApp.Workbooks.Add();
                //Excel.XWorkSheet = (BaseExcel.Worksheet)Excel.XWorkBook.Worksheets.get_Item(1);

                SetRowHeaders();
                ProcessUwLimitData();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                ProcessingFailed();
                return;
            }

            if (Errors.Count == 0)
            {
                Util.MakeDir(path);
                Excel.FilePath = path;
                Excel.Save();
                UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusCompleted, "Process Underwriting Limit Comparison Completed", fileName, hashFileName);
            }
            else
            {
                ProcessingFailed();
                return;
            }
        }

        public void SetRowHeaders()
        {
            Excel.XWorkSheet.Cells[1, 1] = "Ceding Company";
            Excel.XWorkSheet.Cells[2, 1] = "Reinsurance Basis";
            Excel.XWorkSheet.Cells[3, 1] = "Underwriting Limit ID";
            Excel.XWorkSheet.Cells[4, 1] = "Underwriting Limit Name";
            Excel.XWorkSheet.Cells[5, 1] = "Benefit Code";
            Excel.XWorkSheet.Cells[6, 1] = "Status";
            Excel.XWorkSheet.Cells[7, 1] = "Description";
            Excel.XWorkSheet.Cells[8, 1] = "Version";
            Excel.XWorkSheet.Cells[9, 1] = "Effective Date";
            Excel.XWorkSheet.Cells[10, 1] = "Underwriting Limit";
            Excel.XWorkSheet.Cells[11, 1] = "Additional Remark 1";
            Excel.XWorkSheet.Cells[12, 1] = "Auto Binding Limit - Sum Assured";
            Excel.XWorkSheet.Cells[13, 1] = "Additional Remark 2";
            Excel.XWorkSheet.Cells[14, 1] = "Auto Binding Limits - Maximum Underwriting Rating (EM)";
            Excel.XWorkSheet.Cells[15, 1] = "Additional Remark 3";
            Excel.XWorkSheet.Cells[16, 1] = "Maximum Sum Assured";
            Excel.XWorkSheet.Cells[17, 1] = "Per life/Per life Per Industry";
            Excel.XWorkSheet.Cells[18, 1] = "Issue limit / Payout limit";
            Excel.XWorkSheet.Cells[19, 1] = "Additional Remark 4";
            Excel.XWorkSheet.Cells[20, 1] = "Product (linked)";

            Excel.XWorkSheet.Columns.AutoFit();

            //Format first column
            BaseExcel.Range range = Excel.XWorkSheet.get_Range("A1:A20");
            BaseExcel.Borders border = range.Borders;
            range.Font.Bold = true;
            border.LineStyle = BaseExcel.XlLineStyle.xlContinuous;
            border.Weight = 2d;
        }

        public void ProcessUwLimitData()
        {
            int currentColumn = 2;
            bool formatDateAndNumber = false;

            string[] reportParams = ReportGenerationBo.ReportParams.Split('|').ToArray();
            List<string> cedantList = reportParams[0].Split(',').ToList();
            string transposeExcel = reportParams[1];

            foreach (string cedant in cedantList)
            {
                var treatyPricingCedantBo = TreatyPricingCedantService.FindByCode(cedant.Trim());

                if (treatyPricingCedantBo != null)
                {
                    IList<TreatyPricingUwLimitBo> uwLimitBos = TreatyPricingUwLimitService.GetByTreatyPricingCedantId(treatyPricingCedantBo.Id);
                    var cedantBo = CedantService.Find(treatyPricingCedantBo.CedantId);

                    foreach (var uwLimitBo in uwLimitBos)
                    {
                        var versionBo = TreatyPricingUwLimitVersionService.GetLatestByTreatyPricingUwLimitId(uwLimitBo.Id);
                        var linkedProducts = TreatyPricingProductBenefitService.GetLinkedProductsByUwLimitVersionId(versionBo.Id);
                        DateTime effectiveAt = versionBo.EffectiveAt ?? default(DateTime);

                        Excel.XWorkSheet.Cells[1, currentColumn] = cedantBo.Code;
                        Excel.XWorkSheet.Cells[2, currentColumn] = GetPickListDescription(treatyPricingCedantBo.ReinsuranceTypePickListDetailId);
                        Excel.XWorkSheet.Cells[3, currentColumn] = uwLimitBo.LimitId;
                        Excel.XWorkSheet.Cells[4, currentColumn] = uwLimitBo.Name;
                        Excel.XWorkSheet.Cells[5, currentColumn] = uwLimitBo.BenefitCode;
                        Excel.XWorkSheet.Cells[6, currentColumn] = TreatyPricingUwLimitBo.GetStatusName(uwLimitBo.Status);
                        Excel.XWorkSheet.Cells[7, currentColumn] = uwLimitBo.Description;
                        Excel.XWorkSheet.Cells[8, currentColumn] = (formatDateAndNumber == true? "'" : "") + versionBo.Version.ToString();
                        Excel.XWorkSheet.Cells[9, currentColumn] = (formatDateAndNumber == true ? "'" : "") + effectiveAt.ToString(Util.GetDateFormat());
                        Excel.XWorkSheet.Cells[10, currentColumn] = (formatDateAndNumber == true ? "'" : "") + versionBo.UwLimit;
                        Excel.XWorkSheet.Cells[11, currentColumn] = versionBo.Remarks1;
                        Excel.XWorkSheet.Cells[12, currentColumn] = (formatDateAndNumber == true ? "'" : "") + versionBo.AblSumAssured;
                        Excel.XWorkSheet.Cells[13, currentColumn] = versionBo.Remarks2;
                        Excel.XWorkSheet.Cells[14, currentColumn] = (formatDateAndNumber == true ? "'" : "") + versionBo.AblMaxUwRating;
                        Excel.XWorkSheet.Cells[15, currentColumn] = versionBo.Remarks3;
                        Excel.XWorkSheet.Cells[16, currentColumn] = (formatDateAndNumber == true ? "'" : "") + versionBo.MaxSumAssured;
                        Excel.XWorkSheet.Cells[17, currentColumn] = versionBo.PerLifePerIndustry == true ? "Per life Per Industry" : "Per life";
                        Excel.XWorkSheet.Cells[18, currentColumn] = versionBo.IssuePayoutLimit == true ? "Payout limit" : "Issue limit";
                        Excel.XWorkSheet.Cells[19, currentColumn] = versionBo.Remarks4;
                        Excel.XWorkSheet.Cells[20, currentColumn] = String.IsNullOrEmpty(linkedProducts) ? "" : linkedProducts;

                        currentColumn++;
                    }
                }
            }

            ColumnCount = currentColumn - 1;

            if (transposeExcel == "true")
                TransposeData();
        }

        public void TransposeData()
        {
            BaseExcel.Worksheet newWorksheet;
            newWorksheet = Excel.XWorkBook.Worksheets.Add(After: Excel.XWorkBook.Worksheets[Excel.XWorkBook.Worksheets.Count]);

            BaseExcel.Range range = Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[1, 1], Excel.XWorkSheet.Cells[20, ColumnCount]];
            range.Copy();

            newWorksheet.Cells[1, 1].PasteSpecial(Transpose: true);

            Excel.XWorkBook.Worksheets[1].Name = "Sheet3";
            Excel.XWorkBook.Worksheets[2].Name = "Sheet1";
            Excel.XWorkBook.Worksheets[1].Delete();
        }

        public void ProcessingFailed()
        {
            if (Excel != null)
                Excel.Close();
            UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusFailed, "Process Underwriting Limit Comparison Failed");
        }

        public void UpdateReportGenerationStatus(int status, string description, string fileName = "", string hashFileName = "")
        {
            var reportGenerationBo = ReportGenerationBo;
            reportGenerationBo.Status = status;

            if (status == TreatyPricingReportGenerationBo.StatusCompleted)
            {
                reportGenerationBo.FileName = fileName;
                reportGenerationBo.HashFileName = hashFileName;
            }

            if (Errors.Count > 0)
            {
                reportGenerationBo.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = TreatyPricingReportGenerationService.Update(ref reportGenerationBo, ref trail);

            var userTrailBo = new UserTrailBo(
                reportGenerationBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public string GetPickListDescription(int? pickListDetailId)
        {
            var bo = PickListDetailService.Find(pickListDetailId);

            return bo.Description == null ? "" : bo.Description;
        }
    }
}
