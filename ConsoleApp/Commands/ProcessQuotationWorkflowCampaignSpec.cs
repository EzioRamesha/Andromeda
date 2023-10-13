using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.Commands
{
    public class ProcessQuotationWorkflowCampaignSpec : Command
    {
        public string FilePath { get; set; }

        public Word Word { get; set; }

        public TreatyPricingQuotationWorkflowBo WorkflowBo { get; set; }

        public TreatyPricingQuotationWorkflowVersionBo WorkflowVersionBo { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> WorkflowObjectBos { get; set; }

        public TreatyPricingCampaignBo CampaignBo { get; set; }

        public TreatyPricingCampaignVersionBo CampaignVersionBo { get; set; }

        public IList<TreatyPricingCampaignProductBo> CampaignProductBos { get; set; }

        public List<string> Errors { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        public ProcessQuotationWorkflowCampaignSpec()
        {
            Title = "ProcessQuotationWorkflowQuoteSpec";
            Description = "To process quote spec file for quotation workflow";
            Errors = new List<string> { };

            //Set formatting values
            FontName = "Calibri";
            FontSize = 12;
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
                if (CheckHasCampaignObject())
                {
                    Word = new Word(FilePath);
                    Word.XApp.Caption = "Campaign Spec Edit - Id: " + WorkflowBo.Id.ToString();

                    ProcessMainTable();

                    ProcessAppendixTables();

                    Word.Save();
                }
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                Word.Close();
                return;
            }

            PrintProcessCount();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public bool CheckHasCampaignObject()
        {
            bool objectExists = false;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeCampaign)
                {
                    objectExists = true;
                    CampaignBo = TreatyPricingCampaignService.Find(searchBo.ObjectId);
                    CampaignVersionBo = TreatyPricingCampaignVersionService.Find(searchBo.ObjectVersionId);
                    CampaignProductBos = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(searchBo.ObjectId);
                }
            }

            return objectExists;
        }

        public void ProcessMainTable()
        {
            var bo = CampaignBo;
            var versionBo = CampaignVersionBo;

            //Process Quotation Workflow information
            Word.FindAndReplace("{campaign_name}", bo.Name);
            Word.FindAndReplace("{ceding_company_name}", CedantService.Find(WorkflowBo.CedantId).Name);
            ProcessProducts();
            Word.FindAndReplace("{system_date}", DateTime.Now.ToString(Util.GetDateFormat()));
            Word.FindAndReplace("{campaign_type}", bo.Name);
            Word.FindAndReplace("{campaign_duration}", bo.Duration.ToString());

            #region Additional fields
            Word.FindAndReplace("{campaign_id}", bo.Code);
            Word.FindAndReplace("{target_take_up_rate}", bo.TargetTakeUpRate);
            Word.FindAndReplace("{expected_no_of_policy}", bo.NoOfPolicy);
            Word.FindAndReplace("{campaign_purpose}", bo.Purpose);
            Word.FindAndReplace("{average_sum_assured}", bo.AverageSumAssured);
            Word.FindAndReplace("{ri_premium_receivable}", bo.RiPremiumReceivable);
            #endregion

            if (bo.PeriodStartDate.HasValue && bo.PeriodEndDate.HasValue)
            {
                DateTime periodStartDate = bo.PeriodStartDate ?? default(DateTime);
                DateTime periodEndDate = bo.PeriodEndDate ?? default(DateTime);
                string campaignPeriod = periodStartDate.ToString(Util.GetDateFormat());
                campaignPeriod += " - " + periodEndDate.ToString(Util.GetDateFormat());

                Word.FindAndReplace("{campaign_period}", campaignPeriod);
            }
            else
            {
                Word.FindAndReplace("{campaign_period}", "");
            }

            Word.FindAndReplace("{benefit}", versionBo.IsPerBenefit == true ? "As per existing" : "Others");
            Word.FindAndReplace("{benefits_remarks}", versionBo.IsPerBenefit == true ? "" : versionBo.BenefitRemark);

            Word.FindAndReplace("{ceding_company_retention}", versionBo.IsPerCedantRetention == true ? "As per existing" : "Others");
            Word.FindAndReplace("{ceding_company_retention_others}", versionBo.IsPerCedantRetention == true ? "" : versionBo.CedantRetention);

            Word.FindAndReplace("{mlre_share}", versionBo.IsPerMlreShare == true ? "As per existing" : "Others");
            Word.FindAndReplace("{mlre_share_others}", versionBo.IsPerMlreShare == true ? "" : versionBo.MlreShare);

            Word.FindAndReplace("{distribution_channel}", versionBo.IsPerDistributionChannel == true ? "As per existing" : "Others");
            Word.FindAndReplace("{distribution_channel_others}", versionBo.IsPerDistributionChannel == true ? "" : versionBo.DistributionChannel);

            Word.FindAndReplace("{age_basis}", versionBo.IsPerAgeBasis == true ? "As per existing" : "Others");
            Word.FindAndReplace("{age_basis_others}", versionBo.IsPerAgeBasis == true ? "" : GetPickListDescription(versionBo.AgeBasisPickListDetailId));

            Word.FindAndReplace("{min_entry_age}", versionBo.IsPerMinEntryAge == true ? "As per existing" : "Others");
            Word.FindAndReplace("{min_entry_age_others}", versionBo.IsPerMinEntryAge == true ? "" : versionBo.MinEntryAge.ToString());

            Word.FindAndReplace("{max_entry_age}", versionBo.IsPerMaxEntryAge == true ? "As per existing" : "Others");
            Word.FindAndReplace("{max_entry_age_others}", versionBo.IsPerMaxEntryAge == true ? "" : versionBo.MaxEntryAge.ToString());

            Word.FindAndReplace("{max_expiry_age}", versionBo.IsPerMaxExpiryAge == true ? "As per existing" : "Others");
            Word.FindAndReplace("{max_expiry_age_others}", versionBo.IsPerMaxExpiryAge == true ? "" : versionBo.MaxExpiryAge.ToString());

            Word.FindAndReplace("{min_sum_assured}", versionBo.IsPerMinSumAssured == true ? "As per existing" : "Others");
            Word.FindAndReplace("{min_sum_assured_others}", versionBo.IsPerMinSumAssured == true ? "" : versionBo.MinSumAssured.ToString());

            Word.FindAndReplace("{max_sum_assured}", versionBo.IsPerMaxSumAssured == true ? "As per existing" : "Others");
            Word.FindAndReplace("{max_sum_assured_others}", versionBo.IsPerMaxSumAssured == true ? "" : versionBo.MaxSumAssured.ToString());

            Word.FindAndReplace("{campaign_funding_mlre}", versionBo.CampaignFundByMlre);
            Word.FindAndReplace("{complimentary_sum_assured}", versionBo.ComplimentarySumAssured);

            Word.FindAndReplace("{underwriting_method}", versionBo.IsPerUnderwritingMethod == true ? "As per existing" : "Others");
            Word.FindAndReplace("{underwriting_method_others}", versionBo.IsPerUnderwritingMethod == true ? "" : versionBo.UnderwritingMethod);

            Word.FindAndReplace("{aggregation_notes}", versionBo.IsPerAggregationNotes == true ? "As per existing" : "Others");
            Word.FindAndReplace("{aggregation_notes_others}", versionBo.IsPerAggregationNotes == true ? "" : versionBo.AggregationNotes);

            Word.FindAndReplace("{waiting_period}", versionBo.IsPerWaitingPeriod == true ? "As per existing" : "Others");
            Word.FindAndReplace("{waiting_period_others}", versionBo.IsPerWaitingPeriod == true ? "" : versionBo.WaitingPeriod);

            Word.FindAndReplace("{survival_period}", versionBo.IsPerSurvivalPeriod == true ? "As per existing" : "Others");
            Word.FindAndReplace("{survival_period_others}", versionBo.IsPerSurvivalPeriod == true ? "" : versionBo.SurvivalPeriod);

            Word.FindAndReplace("{other_campaign_criteria}", versionBo.OtherCampaignCriteria);
            Word.FindAndReplace("{additional_remark}", versionBo.AdditionalRemark);
        }

        public string GetPickListDescription(int? pickListDetailId)
        {
            var bo = PickListDetailService.Find(pickListDetailId);

            return bo.Description == null ? "" : bo.Description;
        }

        public void ProcessAppendixTables()
        {
            ProcessReinsuranceRateTable();
            ProcessProfitCommission();
            ProcessAdvantageProgram();
            ProcessUnderwritingQuestionnaire();
            ProcessMedicalTable();
            ProcessFinancialTable();
        }

        #region Product processing
        public void ProcessProducts()
        {
            object findText = "{product_name}";

            if (CampaignProductBos.Count > 0)
            {
                int rowCount = CampaignProductBos.Count + 1;
                int columnCount = 1;
                Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

                object oMissing = false;

                range.Find.ClearFormatting();

                if (range.Find.Execute(ref findText,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing))
                {
                    range.Select();
                }
                else
                {
                    return;
                }

                Microsoft.Office.Interop.Word.Table table;

                range.End = range.Start;

                table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

                int rowNum = 2;

                table.Cell(1, 1).Range.Text = "Product Name";

                foreach (var campaignProductBo in CampaignProductBos)
                {
                    var productBo = TreatyPricingProductService.Find(campaignProductBo.TreatyPricingProductId);

                    table.Cell(rowNum, 1).Range.Text = productBo.Name;
                    rowNum++;
                }

                Word.FindAndReplace(findText, "");

                //Table formatting
                table.Range.Font.Name = FontName;
                table.Range.Font.Size = FontSize;

                table.Borders.InsideLineStyle = Word.XLineStyle;
                table.Borders.OutsideLineStyle = Word.XLineStyle;

                table.Cell(1, 1).Range.Bold = 1;
            }
            else
            {
                Word.FindAndReplace(findText, "");
            }
        }
        #endregion

        #region Reinsurance Rate Table processing
        public void ProcessReinsuranceRateTable()
        {
            if (CampaignVersionBo.IsPerReinsuranceRate)
            {
                Word.FindAndReplace("{reinsurance_rates}", "As per existing");
            }
            else
            {
                if (CampaignVersionBo.ReinsRateTreatyPricingRateTableId.HasValue)
                {
                    IList<TreatyPricingRateTableVersionBo> rateTableBos = new List<TreatyPricingRateTableVersionBo>();
                    int versionId = CampaignVersionBo.ReinsRateTreatyPricingRateTableId ?? default(int);

                    rateTableBos.Add(TreatyPricingRateTableVersionService.Find(versionId));

                    ProcessReinsuranceRateTableData(rateTableBos);
                }
            }
        }

        public void ProcessReinsuranceRateTableData(IList<TreatyPricingRateTableVersionBo> rateTableBos)
        {
            int rowCount = rateTableBos.Count + 1;
            int columnCount = 6;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{reinsurance_rates}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Benefits";
            table.Cell(1, 2).Range.Text = "Rate Table";
            table.Cell(1, 3).Range.Text = "Rate Guarantee";
            table.Cell(1, 4).Range.Text = "RI Discount";
            table.Cell(1, 5).Range.Text = "Profit Commission";
            table.Cell(1, 6).Range.Text = "Advantage Program";

            foreach (var rateTableBo in rateTableBos)
            {
                table.Cell(rowNum, 1).Range.Text = rateTableBo.BenefitName;
                table.Cell(rowNum, 2).Range.Text = "Appendix A - " + (rowNum - 1);
                table.Cell(rowNum, 3).Range.Text = GetPickListDescription(rateTableBo.RateGuaranteePickListDetailId);
                table.Cell(rowNum, 4).Range.Text = rateTableBo.RiDiscount;
                table.Cell(rowNum, 5).Range.Text = rateTableBo.ProfitCommission;
                table.Cell(rowNum, 6).Range.Text = rateTableBo.AdvantageProgram;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Bold = 1;
            table.Cell(1, 4).Range.Bold = 1;
            table.Cell(1, 5).Range.Bold = 1;
            table.Cell(1, 6).Range.Bold = 1;
        }
        #endregion

        #region Profit Commission processing
        public void ProcessProfitCommission()
        {
            if (CampaignVersionBo.IsPerProfitComm)
            {
                Word.FindAndReplace("{profit_commission}", "As per existing");
            }
            else
            {
                if (CampaignVersionBo.TreatyPricingProfitCommissionVersionId.HasValue)
                {
                    IList<TreatyPricingProfitCommissionDetailBo> detailBos = new List<TreatyPricingProfitCommissionDetailBo>();

                    detailBos.Add(TreatyPricingProfitCommissionDetailService.Find(CampaignVersionBo.TreatyPricingProfitCommissionVersionId));

                    ProcessProfitCommissionData(detailBos);
                }
            }
        }

        public void ProcessProfitCommissionData(IList<TreatyPricingProfitCommissionDetailBo> detailBos)
        {
            int rowCount = detailBos.Count + 1;
            int columnCount = 6;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{profit_commission}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "No";
            table.Cell(1, 2).Range.Text = "Item";
            table.Cell(1, 3).Range.Text = "Component";
            table.Cell(1, 4).Range.Text = "Description";
            table.Cell(1, 5).Range.Text = "Nett/Gross";
            table.Cell(1, 6).Range.Text = "Value";

            foreach (var detailBo in detailBos)
            {
                table.Cell(rowNum, 1).Range.Text = (rowNum - 1).ToString();
                table.Cell(rowNum, 2).Range.Text = TreatyPricingProfitCommissionDetailBo.GetItemName(detailBo.Item);
                table.Cell(rowNum, 3).Range.Text = detailBo.Component;
                table.Cell(rowNum, 4).Range.Text = detailBo.ComponentDescription;

                if (detailBo.IsNetGrossRequired)
                {
                    bool isNetGross = detailBo.IsNetGross ?? default(bool);
                    table.Cell(rowNum, 5).Range.Text = isNetGross ? "Gross" : "Nett";
                }

                table.Cell(rowNum, 6).Range.Text = detailBo.Value;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Bold = 1;
            table.Cell(1, 4).Range.Bold = 1;
            table.Cell(1, 5).Range.Bold = 1;
            table.Cell(1, 6).Range.Bold = 1;

            table.Columns[1].AutoFit();
        }
        #endregion

        #region Advantage Program processing
        public void ProcessAdvantageProgram()
        {
            if (CampaignVersionBo.IsPerAdvantageProgram)
            {
                Word.FindAndReplace("{advantage_program}", "As per existing");
            }
            else
            {
                if (CampaignVersionBo.TreatyPricingAdvantageProgramVersionId.HasValue)
                {
                    IList<TreatyPricingAdvantageProgramVersionBenefitBo> advantageProgramBenefitBos = null;
                    int versionId = CampaignVersionBo.TreatyPricingAdvantageProgramVersionId ?? default(int);

                    advantageProgramBenefitBos = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(versionId);

                    ProcessAdvantageProgramData(advantageProgramBenefitBos);
                }
            }
        }

        public void ProcessAdvantageProgramData(IList<TreatyPricingAdvantageProgramVersionBenefitBo> advantageProgramBenefitBos)
        {
            int rowCount = advantageProgramBenefitBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{advantage_program}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Benefits";
            table.Cell(1, 2).Range.Text = "Extra Mortality/Morbidity";
            table.Cell(1, 3).Range.Text = "Sum Assured Not Exceeding";

            foreach (var advantageProgramBenefitBo in advantageProgramBenefitBos)
            {
                var benefitBo = BenefitService.Find(advantageProgramBenefitBo.BenefitId);

                table.Cell(rowNum, 1).Range.Text = benefitBo.Code;
                table.Cell(rowNum, 2).Range.Text = advantageProgramBenefitBo.ExtraMortality.ToString();
                table.Cell(rowNum, 3).Range.Text = advantageProgramBenefitBo.SumAssured.ToString();
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Bold = 1;
        }
        #endregion

        #region Underwriting Questionnaire processing
        public void ProcessUnderwritingQuestionnaire()
        {
            if (CampaignVersionBo.IsPerUnderwritingQuestion)
            {
                Word.FindAndReplace("{underwriting_questionnaire}", "As per existing");
            }
            else
            {
                if (CampaignVersionBo.TreatyPricingUwQuestionnaireVersionId.HasValue)
                {
                    IList<TreatyPricingUwQuestionnaireVersionDetailBo> questionnaireBos = new List<TreatyPricingUwQuestionnaireVersionDetailBo>();
                    int versionId = CampaignVersionBo.TreatyPricingUwQuestionnaireVersionId ?? default(int);

                    questionnaireBos.Add(TreatyPricingUwQuestionnaireVersionDetailService.Find(versionId));

                    ProcessUnderwritingQuestionnaireData(questionnaireBos);
                }
            }
        }

        public void ProcessUnderwritingQuestionnaireData(IList<TreatyPricingUwQuestionnaireVersionDetailBo> questionnaireBos)
        {
            int rowCount = questionnaireBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{underwriting_questionnaire}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Category Code";
            table.Cell(1, 2).Range.Text = "Category Name";
            table.Cell(1, 3).Range.Text = "Question";

            foreach (var questionnaireBo in questionnaireBos)
            {
                var categoryBo = UwQuestionnaireCategoryService.Find(questionnaireBo.UwQuestionnaireCategoryId);

                table.Cell(rowNum, 1).Range.Text = categoryBo.Code;
                table.Cell(rowNum, 2).Range.Text = categoryBo.Name;
                table.Cell(rowNum, 3).Range.Text = questionnaireBo.Question;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Bold = 1;
        }
        #endregion

        #region Medical Table processing
        public void ProcessMedicalTable()
        {
            if (CampaignVersionBo.IsPerMedicalTable)
            {
                Word.FindAndReplace("{medical_table}", "As per existing");
                Word.FindAndReplace("{medical_table_legends}", "");
            }
            else
            {
                if (CampaignVersionBo.TreatyPricingMedicalTableVersionId.HasValue)
                {
                    IList<TreatyPricingMedicalTableUploadLegendBo> legendBos = null;
                    IList<TreatyPricingMedicalTableUploadRowBo> rowBos = null;
                    IList<TreatyPricingMedicalTableUploadColumnBo> columnBos = null;
                    IList<TreatyPricingMedicalTableUploadCellBo> cellBos = new List<TreatyPricingMedicalTableUploadCellBo>();

                    int versionId = CampaignVersionBo.TreatyPricingMedicalTableVersionId ?? default(int);

                    legendBos = TreatyPricingMedicalTableUploadLegendService.GetByTreatyPricingMedicalTableVersionDetailId(versionId);
                    rowBos = TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailId(versionId);
                    columnBos = TreatyPricingMedicalTableUploadColumnService.GetByTreatyPricingMedicalTableVersionDetailId(versionId);

                    foreach (var columnBo in columnBos)
                    {
                        var columnCellBos = TreatyPricingMedicalTableUploadCellService.GetByTreatyPricingMedicalTableUploadColumnId(columnBo.Id);

                        foreach (var columnCellBo in columnCellBos)
                        {
                            cellBos.Add(columnCellBo);
                        }
                    }

                    ProcessMedicalTableData(rowBos, columnBos, cellBos);
                    ProcessMedicalTableLegends(legendBos);
                }
            }
        }

        public void ProcessMedicalTableData(IList<TreatyPricingMedicalTableUploadRowBo> rowBos
            , IList<TreatyPricingMedicalTableUploadColumnBo> columnBos
            , IList<TreatyPricingMedicalTableUploadCellBo> cellBos)
        {
            int rowCount = rowBos.Count + 3;
            int columnCount = columnBos.Count + 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{medical_table}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum;
            int columnNum = 3;
            bool sumAssuredPlaced = false;

            table.Cell(1, 2).Range.Text = "Age (Min)";
            table.Cell(2, 2).Range.Text = "Age (Max)";
            table.Cell(3, 1).Range.Text = "Sum Assured (Min)";
            table.Cell(3, 2).Range.Text = "Sum Assured (Max)";
            table.Cell(3, 3).Range.Text = "Medical Requirements";

            foreach (var columnBo in columnBos)
            {
                rowNum = 4;
                //Place age values
                table.Cell(1, columnNum).Range.Text = columnBo.MinimumAge.ToString();
                table.Cell(2, columnNum).Range.Text = columnBo.MaximumAge.ToString();

                foreach (var rowBo in rowBos)
                {
                    //Place sum assured values
                    if (!sumAssuredPlaced)
                    {
                        table.Cell(rowNum, 1).Range.Text = rowBo.MinimumSumAssured.ToString();
                        table.Cell(rowNum, 2).Range.Text = rowBo.MaximumSumAssured >= 2000000000 ? "Max" : rowBo.MaximumSumAssured.ToString();
                    }

                    //Place cell values
                    foreach (var cellBo in cellBos)
                    {
                        if (cellBo.TreatyPricingMedicalTableUploadColumnId == columnBo.Id && cellBo.TreatyPricingMedicalTableUploadRowId == rowBo.Id)
                        {
                            table.Cell(rowNum, columnNum).Range.Text = cellBo.Code;
                        }
                    }
                    rowNum++;
                }
                sumAssuredPlaced = true;
                columnNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            for (int i=0; i<columnCount; i++)
            {
                table.Cell(1, i + 1).Range.Bold = 1;
                table.Cell(2, i + 1).Range.Bold = 1;
                table.Cell(3, i + 1).Range.Bold = 1;
            }

            //Merge and align center "Medical Requirements" label
            table.Rows[3].Cells[3].Merge(table.Rows[3].Cells[columnCount]);
            table.Cell(3, 3).Range.ParagraphFormat.Alignment = Word.ParagraphAlignCenter;
        }

        public void ProcessMedicalTableLegends(IList<TreatyPricingMedicalTableUploadLegendBo> legendBos)
        {
            int rowCount = legendBos.Count + 1;
            int columnCount = 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{medical_table_legends}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Code";
            table.Cell(1, 2).Range.Text = "Description";

            foreach (var legendBo in legendBos)
            {
                table.Cell(rowNum, 1).Range.Text = legendBo.Code;
                table.Cell(rowNum, 2).Range.Text = legendBo.Description;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
        }
        #endregion

        #region Financial Table processing
        public void ProcessFinancialTable()
        {
            if (CampaignVersionBo.IsPerFinancialTable)
            {
                Word.FindAndReplace("{financial_table}", "As per existing");
                Word.FindAndReplace("{financial_table_legends}", "");
            }
            else
            {
                if (CampaignVersionBo.TreatyPricingFinancialTableVersionId.HasValue)
                {
                    IList<TreatyPricingFinancialTableUploadBo> uploadBos = null;
                    IList<TreatyPricingFinancialTableUploadLegendBo> legendBos = null;

                    int versionId = CampaignVersionBo.TreatyPricingFinancialTableVersionId ?? default(int);

                    uploadBos = TreatyPricingFinancialTableUploadService.GetByTreatyPricingFinancialTableVersionDetailId(versionId);
                    legendBos = TreatyPricingFinancialTableUploadLegendService.GetByTreatyPricingFinancialTableVersionDetailId(versionId);

                    ProcessFinancialTableData(uploadBos);
                    ProcessFinancialTableLegends(legendBos);
                }
            }
        }

        public void ProcessFinancialTableData(IList<TreatyPricingFinancialTableUploadBo> uploadBos)
        {
            int rowCount = uploadBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{financial_table}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Sum Assured (From)";
            table.Cell(1, 2).Range.Text = "Sum Assured (To)";
            table.Cell(1, 3).Range.Text = "Financial Requirement";

            foreach (var uploadBo in uploadBos)
            {
                table.Cell(rowNum, 1).Range.Text = uploadBo.MinimumSumAssured.ToString();
                table.Cell(rowNum, 2).Range.Text = uploadBo.MaximumSumAssured >= 2000000000 ? "Max" : uploadBo.MaximumSumAssured.ToString();
                table.Cell(rowNum, 3).Range.Text = uploadBo.Code;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 3).Range.Bold = 1;
        }

        public void ProcessFinancialTableLegends(IList<TreatyPricingFinancialTableUploadLegendBo> legendBos)
        {
            int rowCount = legendBos.Count + 1;
            int columnCount = 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{financial_table_legends}";
            object oMissing = false;

            range.Find.ClearFormatting();

            if (range.Find.Execute(ref findText,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing))
            {
                range.Select();
            }
            else
            {
                return;
            }

            Microsoft.Office.Interop.Word.Table table;

            range.End = range.Start;

            table = Word.XDocument.Tables.Add(range, rowCount, columnCount);

            int rowNum = 2;

            table.Cell(1, 1).Range.Text = "Code";
            table.Cell(1, 2).Range.Text = "Description";

            foreach (var legendBo in legendBos)
            {
                table.Cell(rowNum, 1).Range.Text = legendBo.Code;
                table.Cell(rowNum, 2).Range.Text = legendBo.Description;
                rowNum++;
            }

            Word.FindAndReplace(findText, "");

            //Table formatting
            table.Range.Font.Name = FontName;
            table.Range.Font.Size = FontSize;

            table.Borders.InsideLineStyle = Word.XLineStyle;
            table.Borders.OutsideLineStyle = Word.XLineStyle;

            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 2).Range.Bold = 1;
        }
        #endregion
    }
}