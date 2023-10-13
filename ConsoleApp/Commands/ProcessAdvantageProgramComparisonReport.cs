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
    public class ProcessAdvantageProgramComparisonReport : Command
    {
        public string FilePath { get; set; }

        public string FolderPath { get; set; }

        public TreatyPricingReportGenerationBo ReportGenerationBo { get; set; }

        public Excel Excel { get; set; }

        public List<Column> Column { get; set; }

        public int ColumnCount { get; set; }

        public List<string> Errors { get; set; }

        public int UserId { get; set; }

        public ProcessAdvantageProgramComparisonReport()
        {
            Title = "ProcessAdvantageProgramComparisonReport";
            Description = "To process advnatage program comparison into excel file";
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
            UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusProcessing, "Processing Advantage Program Data File");

            ProcessMain();

            PrintProcessCount();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void ProcessMain()
        {
            string fileName = "AdvantageProgramComparison" + ReportGenerationBo.Id.ToString().PadLeft(5, '0') + ".xlsx";
            string hashFileName = Hash.HashFileName(fileName);
            string folderPath = FolderPath;
            string path = Path.Combine(folderPath, hashFileName);
            Util.MakeDir(path);

            try
            {
                Excel = new Excel(path, false);
                //Excel.XApp = new BaseExcel.Application();
                //Excel.XWorkBook = Excel.XApp.Workbooks.Add();
                //Excel.XWorkSheet = (BaseExcel.Worksheet)Excel.XWorkBook.Worksheets.get_Item(1);

                SetRowHeaders();
                ProcessAdvantageProgramData();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                ProcessingFailed();
                return;
            }

            if (Errors.Count == 0)
            {
                //Util.MakeDir(path);
                Excel.FilePath = path;
                Excel.Save();
                UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusCompleted, "Process Advantage Program Comparison Completed", fileName, hashFileName);
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
            Excel.XWorkSheet.Cells[3, 1] = "Advantage Program ID";
            Excel.XWorkSheet.Cells[4, 1] = "Advantage Program Name";
            Excel.XWorkSheet.Cells[5, 1] = "Status";
            Excel.XWorkSheet.Cells[6, 1] = "Description";
            Excel.XWorkSheet.Cells[7, 1] = "Version";
            Excel.XWorkSheet.Cells[8, 1] = "Effective Date";
            Excel.XWorkSheet.Cells[9, 1] = "Advantage Program - Retention";
            Excel.XWorkSheet.Cells[10, 1] = "Advantage Program - MLRe's Share";
            Excel.XWorkSheet.Cells[11, 1] = "Note";
            Excel.XWorkSheet.Cells[12, 1] = "Product (linked)";
            Excel.XWorkSheet.Cells[13, 1] = "Benefit 1";
            Excel.XWorkSheet.Cells[14, 1] = "Benefit 1's Extra Mortality";
            Excel.XWorkSheet.Cells[15, 1] = "Benefit 1's Sum Assured Not Exceeding";
            Excel.XWorkSheet.Cells[16, 1] = "Benefit 2";
            Excel.XWorkSheet.Cells[17, 1] = "Benefit 2's Extra Mortality";
            Excel.XWorkSheet.Cells[18, 1] = "Benefit 3's Sum Assured Not Exceeding";
            Excel.XWorkSheet.Cells[19, 1] = "Benefit 2";
            Excel.XWorkSheet.Cells[20, 1] = "Benefit 3's Extra Mortality";
            Excel.XWorkSheet.Cells[21, 1] = "Benefit 3's Sum Assured Not Exceeding";

            List<int> versionIds = new List<int>() { };
            List<string> cedantList = ReportGenerationBo.ReportParams.Split(',').ToList();
            foreach (string cedant in cedantList)
            {
                var treatyPricingCedantBo = TreatyPricingCedantService.FindByCode(cedant.Trim());
                if (treatyPricingCedantBo != null)
                {
                    IList<TreatyPricingAdvantageProgramBo> advantageProgramBos = TreatyPricingAdvantageProgramService.GetByTreatyPricingCedantId(treatyPricingCedantBo.Id);
                    foreach (var advantageProgramBo in advantageProgramBos)
                    {
                        var versionBo = TreatyPricingAdvantageProgramVersionService.GetLatestByTreatyPricingAdvantageProgramId(advantageProgramBo.Id);
                        versionIds.Add(versionBo.Id);
                    }
                }
            }

            int row = 0;
            if (versionIds.Count > 0)
            {
                int total = TreatyPricingAdvantageProgramVersionBenefitService.GetMaxCountByTreatyPricingAdvantageProgramVersionIds(versionIds);
                if(total != 0)
                {
                    row = 13;
                    for (int i = 1; i <= total; i++)
                    {
                        Excel.XWorkSheet.Cells[row, 1] = string.Format("Benefit {0}", i);
                        Excel.XWorkSheet.Cells[row + 1, 1] = string.Format("Benefit {0}'s Extra Mortality", i);
                        Excel.XWorkSheet.Cells[row + 2, 1] = string.Format("Benefit {0}'s Sum Assured Not Exceeding", i);

                        row = row + 3;
                    }
                }
            }
            else
            {
                Excel.XWorkSheet.Cells[13, 1] = "Benefit 1";
                Excel.XWorkSheet.Cells[14, 1] = "Benefit 1's Extra Mortality";
                Excel.XWorkSheet.Cells[15, 1] = "Benefit 1's Sum Assured Not Exceeding";
                Excel.XWorkSheet.Cells[16, 1] = "Benefit 2";
                Excel.XWorkSheet.Cells[17, 1] = "Benefit 2's Extra Mortality";
                Excel.XWorkSheet.Cells[18, 1] = "Benefit 3's Sum Assured Not Exceeding";
                Excel.XWorkSheet.Cells[19, 1] = "Benefit 2";
                Excel.XWorkSheet.Cells[20, 1] = "Benefit 3's Extra Mortality";
                Excel.XWorkSheet.Cells[21, 1] = "Benefit 3's Sum Assured Not Exceeding";
            }


            Excel.XWorkSheet.Columns.AutoFit();

            //Format first column
            BaseExcel.Range range = Excel.XWorkSheet.get_Range(string.Format("A1:A{0}", row == 0 ? 21 : row - 1));
            BaseExcel.Borders border = range.Borders;
            range.Font.Bold = true;
            border.LineStyle = BaseExcel.XlLineStyle.xlContinuous;
            border.Weight = 2d;
        }

        public void ProcessAdvantageProgramData()
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
                    IList<TreatyPricingAdvantageProgramBo> advantageProgramBos = TreatyPricingAdvantageProgramService.GetByTreatyPricingCedantId(treatyPricingCedantBo.Id);
                    var cedantBo = CedantService.Find(treatyPricingCedantBo.CedantId);

                    foreach (var advantageProgramBo in advantageProgramBos)
                    {
                        var versionBo = TreatyPricingAdvantageProgramVersionService.GetLatestByTreatyPricingAdvantageProgramId(advantageProgramBo.Id);
                        var linkedProducts = TreatyPricingProductVersionService.GetLinkedProductsByAdvantageProgramVersionId(versionBo.Id);
                        DateTime effectiveAt = versionBo.EffectiveAt ?? default(DateTime);

                        Excel.XWorkSheet.Cells[1, currentColumn] = cedantBo.Code;
                        Excel.XWorkSheet.Cells[2, currentColumn] = GetPickListDescription(treatyPricingCedantBo.ReinsuranceTypePickListDetailId);
                        Excel.XWorkSheet.Cells[3, currentColumn] = advantageProgramBo.Code;
                        Excel.XWorkSheet.Cells[4, currentColumn] = advantageProgramBo.Name;
                        Excel.XWorkSheet.Cells[5, currentColumn] = TreatyPricingAdvantageProgramBo.GetStatusName(advantageProgramBo.Status);
                        Excel.XWorkSheet.Cells[6, currentColumn] = advantageProgramBo.Description;
                        Excel.XWorkSheet.Cells[7, currentColumn] = (formatDateAndNumber == true ? "'" : "") + versionBo.Version.ToString();
                        Excel.XWorkSheet.Cells[8, currentColumn] = (formatDateAndNumber == true ? "'" : "") + effectiveAt.ToString(Util.GetDateFormat());
                        Excel.XWorkSheet.Cells[9, currentColumn] = versionBo.RetentionStr;
                        Excel.XWorkSheet.Cells[10, currentColumn] = versionBo.MlreShareStr;
                        Excel.XWorkSheet.Cells[11, currentColumn] = versionBo.Remarks;
                        Excel.XWorkSheet.Cells[12, currentColumn] = string.IsNullOrEmpty(linkedProducts) ? "" : linkedProducts;

                        int row = 13;
                        var benefitBos = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(versionBo.Id);
                        foreach (var benefitBo in benefitBos)
                        {
                            Excel.XWorkSheet.Cells[row, currentColumn] = benefitBo.BenefitCode;
                            Excel.XWorkSheet.Cells[row + 1, currentColumn] = benefitBo.ExtraMortalityStr;
                            Excel.XWorkSheet.Cells[row + 2, currentColumn] = benefitBo.SumAssuredStr;

                            row = row + 3;
                        }

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

            BaseExcel.Range range = Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[1, 1], Excel.XWorkSheet.Cells[87, ColumnCount]];
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
            UpdateReportGenerationStatus(TreatyPricingReportGenerationBo.StatusFailed, "Process Advantage Program Comparison Failed");
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
