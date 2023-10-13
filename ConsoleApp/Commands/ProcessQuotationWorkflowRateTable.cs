using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using BaseExcel = Microsoft.Office.Interop.Excel;

namespace ConsoleApp.Commands
{
    public class ProcessQuotationWorkflowRateTable : Command
    {
        public string TemplateType { get; set; }

        public string TemplateText { get; set; }

        public string FilePath { get; set; }

        public int ColumnPosition { get; set; }

        public Excel Excel { get; set; }

        public TreatyPricingQuotationWorkflowBo WorkflowBo { get; set; }

        public TreatyPricingQuotationWorkflowVersionBo WorkflowVersionBo { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> WorkflowObjectBos { get; set; }

        public List<string> Errors { get; set; }

        public ProcessQuotationWorkflowRateTable()
        {
            Title = "ProcessQuotationWorkflowRateTable";
            Description = "To process rate table file for quotation workflow";
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
                TemplateText = TemplateType == "Reinsurance"
                    ? "Annual Risk Premium Rates per 1000 Reinsured NAR"
                    : "Annual Risk Contribution Rates per 1000 Retakaful NAR";
                ColumnPosition = 2;
                Excel = new Excel(FilePath, false);

                ProcessRateTables();
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

        public void ProcessRateTables()
        {
            bool objectExists = false;
            TreatyPricingProductBo productBo = null;
            TreatyPricingProductVersionBo productVersionBo = null;
            IList<TreatyPricingProductBenefitBo> productBenefitBos = null;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeProduct)
                {
                    objectExists = true;
                    productBo = TreatyPricingProductService.Find(searchBo.ObjectId);
                    productVersionBo = TreatyPricingProductVersionService.Find(searchBo.ObjectVersionId);
                    productBenefitBos = TreatyPricingProductBenefitService.GetByVersionId(searchBo.ObjectVersionId);
                }
            }

            if (objectExists)
            {
                int i = 0;
                int rateTableSlot = 0;

                Excel.XWorkSheet.Cells[1, 1].Value = WorkflowBo.QuotationId + " Quotation " +
                    (WorkflowBo.FinaliseDate.HasValue ? WorkflowBo.FinaliseDate.Value.ToString(Util.GetDateFormat()) : "");

                foreach (var productBenefitBo in productBenefitBos)
                {
                    if (i == 24)
                        break;

                    if (productBenefitBo.TreatyPricingRateTableVersionId.HasValue)
                    {
                        TreatyPricingRateTableVersionBo rateTableVersionBo = TreatyPricingRateTableVersionService.Find(productBenefitBo.TreatyPricingRateTableVersionId);

                        ProcessSingleRateTable(rateTableSlot, rateTableVersionBo);
                        rateTableSlot++;
                    }

                    i++;
                }
            }
        }

        public void ProcessSingleRateTable(int rateTableSlot, TreatyPricingRateTableVersionBo rateTableVersionBo)
        {
            string cedingCompany = WorkflowBo.CedantBo.Name;
            string quotationName = WorkflowBo.Name;

            var rateTableBo = TreatyPricingRateTableService.Find(rateTableVersionBo.TreatyPricingRateTableId);
            //var rateTableGroupBo = TreatyPricingRateTableGroupService.Find(rateTableBo.TreatyPricingRateTableGroupId);
            IList<TreatyPricingRateTableOriginalRateBo> rateTableRateBos = TreatyPricingRateTableOriginalRateService.GetByTreatyPricingRateTableVersionId(rateTableVersionBo.Id);
            string benefitCode = BenefitService.Find(rateTableBo.BenefitId).Code;
            string benefitDescription = BenefitService.Find(rateTableBo.BenefitId).Description;
            int columnCountForBenefit = 1;
            int i = 0;

            #region Declare counters
            int countMNS = 0;
            int countMS = 0;
            int countFNS = 0;
            int countFS = 0;
            int countM = 0;
            int countF = 0;
            int countUnisex = 0;
            int countUnitRate = 0;
            int countOccClass = 0;
            #endregion

            //Get type count
            foreach (var rateTableRateBo in rateTableRateBos)
            {
                if (rateTableRateBo.MaleNonSmoker.HasValue && rateTableRateBo.MaleNonSmoker != 0)
                {
                    countMNS++;
                }

                if (rateTableRateBo.MaleSmoker.HasValue && rateTableRateBo.MaleSmoker != 0)
                {
                    countMS++;
                }

                if (rateTableRateBo.FemaleNonSmoker.HasValue && rateTableRateBo.FemaleNonSmoker != 0)
                {
                    countFNS++;
                }

                if (rateTableRateBo.FemaleSmoker.HasValue && rateTableRateBo.FemaleSmoker != 0)
                {
                    countFS++;
                }

                if (rateTableRateBo.Male.HasValue && rateTableRateBo.Male != 0)
                {
                    countM++;
                }

                if (rateTableRateBo.Female.HasValue && rateTableRateBo.Female != 0)
                {
                    countF++;
                }

                if (rateTableRateBo.Unisex.HasValue && rateTableRateBo.Unisex != 0)
                {
                    countUnisex++;
                }

                if (rateTableRateBo.UnitRate.HasValue && rateTableRateBo.UnitRate != 0)
                {
                    countUnitRate++;
                }

                if (rateTableRateBo.OccupationClass.HasValue && rateTableRateBo.OccupationClass != 0)
                {
                    countOccClass++;
                }
            }

            #region Get column count
            columnCountForBenefit += countMNS > 0 ? 1 : 0;
            columnCountForBenefit += countMS > 0 ? 1 : 0;
            columnCountForBenefit += countFNS > 0 ? 1 : 0;
            columnCountForBenefit += countFS > 0 ? 1 : 0;
            columnCountForBenefit += countM > 0 ? 1 : 0;
            columnCountForBenefit += countF > 0 ? 1 : 0;
            columnCountForBenefit += countUnisex > 0 ? 1 : 0;
            columnCountForBenefit += countUnitRate > 0 ? 1 : 0;
            columnCountForBenefit += countOccClass > 0 ? 1 : 0;

            List<string> columnHeaderList = new List<string>();
            string ageBasis = rateTableVersionBo.AgeBasisPickListDetailBo.Code;

            //columnHeaderList.Add(benefitCode);
            columnHeaderList.Add(ageBasis);
            if (countMNS > 0) columnHeaderList.Add("MNS");
            if (countMS > 0) columnHeaderList.Add("MS");
            if (countFNS > 0) columnHeaderList.Add("FNS");
            if (countFS > 0) columnHeaderList.Add("FS");
            if (countM > 0) columnHeaderList.Add("M");
            if (countF > 0) columnHeaderList.Add("F");
            if (countUnisex > 0) columnHeaderList.Add("Unisex");
            if (countUnitRate > 0) columnHeaderList.Add("Unit Rate");
            if (countOccClass > 0) columnHeaderList.Add("Occ Class");
            #endregion

            #region Set column headers
            Excel.XWorkSheet.Cells[3, ColumnPosition].Value = "Appendix A - " + (rateTableSlot + 1).ToString();
            Excel.XWorkSheet.Cells[4, ColumnPosition].Value = cedingCompany;
            Excel.XWorkSheet.Cells[5, ColumnPosition].Value = quotationName;
            Excel.XWorkSheet.Cells[6, ColumnPosition].Value = TemplateText;
            Excel.XWorkSheet.Cells[7, ColumnPosition].Value = benefitDescription;

            Excel.XWorkSheet.Cells[3, ColumnPosition].HorizontalAlignment = BaseExcel.XlHAlign.xlHAlignCenter;
            Excel.XWorkSheet.Cells[4, ColumnPosition].HorizontalAlignment = BaseExcel.XlHAlign.xlHAlignCenter;
            Excel.XWorkSheet.Cells[5, ColumnPosition].HorizontalAlignment = BaseExcel.XlHAlign.xlHAlignCenter;
            Excel.XWorkSheet.Cells[6, ColumnPosition].HorizontalAlignment = BaseExcel.XlHAlign.xlHAlignCenter;
            Excel.XWorkSheet.Cells[7, ColumnPosition].HorizontalAlignment = BaseExcel.XlHAlign.xlHAlignCenter;

            Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[3, ColumnPosition], Excel.XWorkSheet.Cells[3, ColumnPosition + columnCountForBenefit - 1]].Merge();
            Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[4, ColumnPosition], Excel.XWorkSheet.Cells[4, ColumnPosition + columnCountForBenefit - 1]].Merge();
            Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[5, ColumnPosition], Excel.XWorkSheet.Cells[5, ColumnPosition + columnCountForBenefit - 1]].Merge();
            Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[6, ColumnPosition], Excel.XWorkSheet.Cells[6, ColumnPosition + columnCountForBenefit - 1]].Merge();
            Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[7, ColumnPosition], Excel.XWorkSheet.Cells[7, ColumnPosition + columnCountForBenefit - 1]].Merge();

            foreach (string columnHeader in columnHeaderList)
            {
                Excel.XWorkSheet.Cells[9, ColumnPosition + i].Value = columnHeader;
                i++;
            }
            #endregion

            //Process data cells
            i = 0;
            foreach (string columnHeader in columnHeaderList)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 101; j++)
                    {
                        Excel.XWorkSheet.Cells[j + 10, ColumnPosition].Value = j.ToString();
                    }
                }
                else
                {
                    foreach (var rateTableRateBo in rateTableRateBos)
                    {
                        if (rateTableRateBo.MaleNonSmoker.HasValue && rateTableRateBo.MaleNonSmoker != 0 && columnHeader == "MNS")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.MaleNonSmoker.ToString();
                        }

                        if (rateTableRateBo.MaleSmoker.HasValue && rateTableRateBo.MaleSmoker != 0 && columnHeader == "MS")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.MaleSmoker.ToString();
                        }

                        if (rateTableRateBo.FemaleNonSmoker.HasValue && rateTableRateBo.FemaleNonSmoker != 0 && columnHeader == "FNS")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.FemaleNonSmoker.ToString();
                        }

                        if (rateTableRateBo.FemaleSmoker.HasValue && rateTableRateBo.FemaleSmoker != 0 && columnHeader == "FS")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.FemaleSmoker.ToString();
                        }

                        if (rateTableRateBo.Male.HasValue && rateTableRateBo.Male != 0 && columnHeader == "M")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.Male.ToString();
                        }

                        if (rateTableRateBo.Female.HasValue && rateTableRateBo.Female != 0 && columnHeader == "F")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.Female.ToString();
                        }

                        if (rateTableRateBo.Unisex.HasValue && rateTableRateBo.Unisex != 0 && columnHeader == "Unisex")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.Unisex.ToString();
                        }

                        if (rateTableRateBo.UnitRate.HasValue && rateTableRateBo.UnitRate != 0 && columnHeader == "Unit Rate")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.UnitRate.ToString();
                        }

                        if (rateTableRateBo.OccupationClass.HasValue && rateTableRateBo.OccupationClass != 0 && columnHeader == "Occ Class")
                        {
                            Excel.XWorkSheet.Cells[rateTableRateBo.Age + 10, ColumnPosition + i].Value = rateTableRateBo.OccupationClass.ToString();
                        }
                    }
                }

                i++;
            }

            BaseExcel.Range range = Excel.XWorkSheet.Range[Excel.XWorkSheet.Cells[9, ColumnPosition], Excel.XWorkSheet.Cells[110, ColumnPosition + columnHeaderList.Count - 1]];
            BaseExcel.Borders border = range.Borders;
            border.LineStyle = BaseExcel.XlLineStyle.xlContinuous;
            border.Weight = 2d;

            ColumnPosition += columnCountForBenefit;
            ColumnPosition++;
        }
    }
}