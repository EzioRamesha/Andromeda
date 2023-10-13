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
    public class ProcessQuotationWorkflowQuoteSpec : Command
    {
        public string FilePath { get; set; }

        public Word Word { get; set; }

        public TreatyPricingQuotationWorkflowBo WorkflowBo { get; set; }

        public TreatyPricingQuotationWorkflowVersionBo WorkflowVersionBo { get; set; }

        public IList<TreatyPricingWorkflowObjectBo> WorkflowObjectBos { get; set; }

        public List<string> Errors { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        public ProcessQuotationWorkflowQuoteSpec()
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
                Word = new Word(FilePath);
                Word.XApp.Caption = "Quote Spec Edit - Id: " + WorkflowBo.Id.ToString();

                ProcessMainTable();

                ProcessAppendixTables();

                Word.Save();
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

        public void ProcessMainTable()
        {
            var bo = WorkflowBo;
            var versionBo = WorkflowVersionBo;

            //Process Quotation Workflow information
            Word.FindAndReplace("{quotation_name}", bo.Name);
            Word.FindAndReplace("{ceding_company_name}", CedantService.Find(bo.CedantId).Name);
            Word.FindAndReplace("{system_date}", DateTime.Now.ToString(Util.GetDateFormat()));
            Word.FindAndReplace("{quote_validity_day}", versionBo.QuoteValidityDay.ToString());

            //Additional fields
            Word.FindAndReplace("{reinsurance_type}", GetPickListDescription(bo.ReinsuranceTypePickListDetailId));

            ProcessProducts();
        }

        public void ProcessProducts()
        {
            bool objectExists = false;
            TreatyPricingProductBo productBo = null;
            TreatyPricingProductVersionBo productVersionBo = null;
            IList<TreatyPricingProductBenefitBo> productBenefitBos = null;

            var objectTypeProduct = TreatyPricingCedantBo.ObjectProduct;
            var objectTypeProductVersion = TreatyPricingCedantBo.ObjectProductVersion;

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
                Word.FindAndReplace("{product_name}", productBo.Name);
                Word.FindAndReplace("{product_type}", GetPickListDescription(productVersionBo.ProductTypePickListDetailId));
                Word.FindAndReplace("{product_summary}", productBo.Summary);
                Word.FindAndReplace("{target_segment}", GetTreatyPricingPickListCodes(objectTypeProductVersion, productVersionBo.Id, PickListBo.TargetSegment));

                if (productBo.EffectiveDate.HasValue)
                {
                    Word.FindAndReplace("{product_effective_date}", (productBo.EffectiveDate ?? default(DateTime)).ToString(Util.GetDateFormat()));
                }

                Word.FindAndReplace("{distribution_channel}", GetTreatyPricingPickListCodes(objectTypeProductVersion, productVersionBo.Id, PickListBo.DistributionChannel));
                Word.FindAndReplace("{underwriting_method}", GetTreatyPricingPickListCodes(objectTypeProduct, productBo.Id, PickListBo.UnderwritingMethod));

                #region Additional fields
                //Quote Spec
                Word.FindAndReplace("{per_life_treaty_code}", GetTreatyPricingProductPerLifeRetroTreatyCodes(productBo.Id));
                Word.FindAndReplace("{business_origin}", GetPickListDescription(productVersionBo.BusinessOriginPickListDetailId));

                //Product Details
                Word.FindAndReplace("{business_type}", GetPickListDescription(productVersionBo.BusinessTypePickListDetailId));
                Word.FindAndReplace("{cession_type}", GetTreatyPricingPickListCodes(objectTypeProductVersion, productVersionBo.Id, PickListBo.CessionType));
                Word.FindAndReplace("{product_line}", GetTreatyPricingPickListCodes(objectTypeProductVersion, productVersionBo.Id, PickListBo.ProductLine));

                //Business Volume
                Word.FindAndReplace("{expected_average_sa}", productVersionBo.ExpectedAverageSumAssured);
                Word.FindAndReplace("{expected_ri_premium}", productVersionBo.ExpectedRiPremium);
                Word.FindAndReplace("{expected_no_of_policy}", productVersionBo.ExpectedPolicyNo);

                //Underwriting
                Word.FindAndReplace("{jumbo_limit}", productVersionBo.JumboLimit);
                ProcessSpecialLien(productVersionBo.Id);

                //Miscellaneous
                Word.FindAndReplace("{reinsurance_premium_payment}", GetPickListDescription(productVersionBo.ReinsurancePremiumPaymentPickListDetailId));
                Word.FindAndReplace("{unearned_premium_refund}", GetPickListDescription(productVersionBo.UnearnedPremiumRefundPickListDetailId));
                Word.FindAndReplace("{territory_of_issue_code}", GetPickListDescription(productVersionBo.TerritoryOfIssueCodePickListDetailId));
                //Quarterly Risk Premium moved to new Direct and Inward Retro checking region
                #endregion

                ProcessJuvenileLien(productVersionBo.Id);
                Word.FindAndReplace("{waiting_period}", productVersionBo.WaitingPeriod);
                Word.FindAndReplace("{survival_period}", productVersionBo.SurvivalPeriod);
                Word.FindAndReplace("{reinsur_arrangement}", GetPickListDescription(productVersionBo.ReinsuranceArrangementPickListDetailId));
                Word.FindAndReplace("{retakaful_model}", productVersionBo.RetakafulModel);
                Word.FindAndReplace("{investment_profit_sharing}", productVersionBo.InvestmentProfitSharing);

                #region Direct and Inward Retro
                Word.FindAndReplace("{termination_clause}", productVersionBo.TerminationClause);
                Word.FindAndReplace("{recapture_clause}", productVersionBo.RecaptureClause);
                Word.FindAndReplace("{quarterly_risk_premium}", productVersionBo.QuarterlyRiskPremium);

                Word.FindAndReplace("{dr_termination_clause}", productVersionBo.DirectRetroTerminationClause);
                Word.FindAndReplace("{dr_recapture_clause}", productVersionBo.DirectRetroRecaptureClause);
                Word.FindAndReplace("{dr_quarterly_risk_premium}", productVersionBo.DirectRetroQuarterlyRiskPremium);

                Word.FindAndReplace("{ir_termination_clause}", productVersionBo.InwardRetroTerminationClause);
                Word.FindAndReplace("{ir_recapture_clause}", productVersionBo.InwardRetroRecaptureClause);
                Word.FindAndReplace("{ir_quarterly_risk_premium}", productVersionBo.InwardRetroQuarterlyRiskPremium);
                #endregion

                ProcessBenefitInformation(productBenefitBos);
            }
        }

        public void ProcessSpecialLien(int versionId)
        {
            int type = TreatyPricingProductDetailBo.TypeSpecialLien;
            var productDetailBos = TreatyPricingProductDetailService.GetByParentType(versionId, type);

            int rowCount = productDetailBos.Count + 1;
            int columnCount = 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{special_lien}";
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

            table.Cell(1, 1).Range.Text = "Value 1";
            table.Cell(1, 2).Range.Text = "Value 2";

            foreach (var productDetailBo in productDetailBos)
            {
                table.Cell(rowNum, 1).Range.Text = productDetailBo.Col1;
                table.Cell(rowNum, 2).Range.Text = productDetailBo.Col2;
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

        public void ProcessJuvenileLien(int versionId)
        {
            int type = TreatyPricingProductDetailBo.TypeJuvenileLien;
            var productDetailBos = TreatyPricingProductDetailService.GetByParentType(versionId, type);

            int rowCount = productDetailBos.Count + 1;
            int columnCount = 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{juvenile_lien}";
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

            table.Cell(1, 1).Range.Text = "Value 1";
            table.Cell(1, 2).Range.Text = "Value 2";

            foreach (var productDetailBo in productDetailBos)
            {
                table.Cell(rowNum, 1).Range.Text = productDetailBo.Col1;
                table.Cell(rowNum, 2).Range.Text = productDetailBo.Col2;
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

        public void ProcessBenefitInformation(IList<TreatyPricingProductBenefitBo> productBenefitBos)
        {
            int i = 0;
            List<AuthorityLimits> authorityLimits = new List<AuthorityLimits>();
            List<DefinitionsExlusions> definitionsExlusions = new List<DefinitionsExlusions>();

            foreach (var productBenefitBo in productBenefitBos)
            {
                if (i == 24)
                    break;

                string rowNum = (i + 1).ToString().PadLeft(2, '0');
                Word.FindAndReplace("{benefit_name_" + rowNum + "}", productBenefitBo.Name);

                //Benefit(s) Covered
                Word.FindAndReplace("{basic_rider_" + rowNum + "}", GetPickListDescription(productBenefitBo.BasicRiderPickListDetailId));
                Word.FindAndReplace("{benefit_payout_" + rowNum + "}", productBenefitBo.BenefitPayout);

                //Reinsurance Arrangement
                Word.FindAndReplace("{cedant_retention_" + rowNum + "}", productBenefitBo.CedantRetention);
                Word.FindAndReplace("{reinsurance_share_" + rowNum + "}", productBenefitBo.ReinsuranceShare);

                //Product Description - Age Limit
                Word.FindAndReplace("{min_entry_age_" + rowNum + "}", productBenefitBo.MinimumEntryAge);
                Word.FindAndReplace("{max_entry_age_" + rowNum + "}", productBenefitBo.MaximumEntryAge);
                Word.FindAndReplace("{max_expiry_age_" + rowNum + "}", productBenefitBo.MaximumExpiryAge);

                //Product Description - Policy and Premium Paying Term
                Word.FindAndReplace("{age_basis}", GetPickListDescription(productBenefitBo.AgeBasisPickListDetailId));
                Word.FindAndReplace("{min_policy_term_" + rowNum + "}", productBenefitBo.MinimumPolicyTerm);
                Word.FindAndReplace("{max_policy_term_" + rowNum + "}", productBenefitBo.MaximumPolicyTerm);
                Word.FindAndReplace("{premium_paying_term_" + rowNum + "}", productBenefitBo.PremiumPayingTerm);

                //Product Description - Sum Assured (SA)
                Word.FindAndReplace("{min_sum_assured_" + rowNum + "}", productBenefitBo.MinimumSumAssured);
                Word.FindAndReplace("{max_sum_assured_" + rowNum + "}", productBenefitBo.MaximumSumAssured);

                #region Additional fields
                //General
                Word.FindAndReplace("{payout_type_" + rowNum + "}", GetPickListDescription(productBenefitBo.PayoutTypePickListDetailId));
                Word.FindAndReplace("{rider_attachment_ratio_" + rowNum + "}", productBenefitBo.RiderAttachmentRatio);
                Word.FindAndReplace("{refund_of_premium_" + rowNum + "}", productBenefitBo.RefundOfPremium);
                Word.FindAndReplace("{pre_existing_condition_" + rowNum + "}", productBenefitBo.PreExistingCondition);

                //Pricing
                Word.FindAndReplace("{rtoa_" + rowNum + "}", GetPickListDescription(productBenefitBo.PricingArrangementReinsuranceTypePickListDetailId));
                Word.FindAndReplace("{sarp_" + rowNum + "}", GetPickListDescription(productBenefitBo.RiskPatternSumPickListDetailId));
                #endregion

                Word.FindAndReplace("{wakalah_fee_" + rowNum + "}", productBenefitBo.WakalahFee);

                if (productBenefitBo.TreatyPricingUwLimitVersionId.HasValue)
                {
                    var uwLimitVersionBo = TreatyPricingUwLimitVersionService.Find(productBenefitBo.TreatyPricingUwLimitVersionId);
                    
                    //Benefit Limit
                    Word.FindAndReplace("{max_sum_assured_industry_limit_" + rowNum + "}", uwLimitVersionBo.MaxSumAssured);
                    Word.FindAndReplace("{perlifeperindustry_" + rowNum + "}", uwLimitVersionBo.PerLifePerIndustry ? "Per Industry" : "Per Life");

                    //Authority Limit
                    if (uwLimitVersionBo.IssuePayoutLimit && productBenefitBo.TreatyPricingClaimApprovalLimitVersionId.HasValue)
                    {
                        int claimApprovalLimitVersionId = productBenefitBo.TreatyPricingClaimApprovalLimitVersionId ?? default(int);
                        authorityLimits.Add(new AuthorityLimits(productBenefitBo.Name, uwLimitVersionBo.Id, claimApprovalLimitVersionId));
                    }
                }
                else
                {
                    Word.FindAndReplace("{max_sum_assured_industry_limit_" + rowNum + "}", "");
                }

                //Appendix A
                if (productBenefitBo.TreatyPricingRateTableVersionId.HasValue)
                {
                    var rateTableVersionBo = TreatyPricingRateTableVersionService.Find(productBenefitBo.TreatyPricingRateTableVersionId);

                    Word.FindAndReplace("{rate_guarantee_" + rowNum + "}", GetPickListDescription(rateTableVersionBo.RateGuaranteePickListDetailId));
                    Word.FindAndReplace("{ri_discount_" + rowNum + "}", rateTableVersionBo.RiDiscount);
                }
                else
                {
                    Word.FindAndReplace("{rate_guarantee_" + rowNum + "}", "");
                    Word.FindAndReplace("{ri_discount_" + rowNum + "}", "");
                }
                Word.FindAndReplace("{profit_commission_table_" + rowNum + "}", productBenefitBo.IsProfitCommission ? "Yes" : "No");
                Word.FindAndReplace("{advantage_program_table_" + rowNum + "}", productBenefitBo.IsAdvantageProgram ? "Yes" : "No");

                //Appendix E
                if (productBenefitBo.TreatyPricingDefinitionAndExclusionVersionId.HasValue)
                {
                    int defExcVersionId = productBenefitBo.TreatyPricingDefinitionAndExclusionVersionId ?? default(int);
                    definitionsExlusions.Add(new DefinitionsExlusions(productBenefitBo.Name, defExcVersionId));
                }

                i++;
            }

            //Process Authority Limit
            i = 0;
            foreach (var authorityLimit in authorityLimits)
            {
                string rowNum = (i + 1).ToString().PadLeft(2, '0');

                var uwLimitVersionBo = TreatyPricingUwLimitVersionService.Find(authorityLimit.UwLimitVersionId);
                var claimApprovalLimitVersionBo = TreatyPricingClaimApprovalLimitVersionService.Find(authorityLimit.ClaimApprovalLimitVersionId);

                Word.FindAndReplace("{auth_benefit_name_" + rowNum + "}", authorityLimit.BenefitName);
                Word.FindAndReplace("{underwriting_limit_" + rowNum + "}", uwLimitVersionBo.UwLimit);
                Word.FindAndReplace("{auto_binding_limit_sa_" + rowNum + "}", uwLimitVersionBo.AblSumAssured);
                Word.FindAndReplace("{auto_binding_limit_em_" + rowNum + "}", uwLimitVersionBo.AblMaxUwRating);
                Word.FindAndReplace("{claim_approval_limit_" + rowNum + "}", claimApprovalLimitVersionBo.Amount.ToString());

                i++;
            }

            //Process Appendix E
            i = 0;
            foreach (var definitionsExlusion in definitionsExlusions)
            {
                string rowNum = (i + 1).ToString().PadLeft(2, '0');

                var defExcVersionBo = TreatyPricingDefinitionAndExclusionVersionService.Find(definitionsExlusion.DefExcVersionId);

                Word.FindAndReplace("{defexc_benefit_name_" + rowNum + "}", definitionsExlusion.BenefitName);
                Word.FindAndReplace("{definition_" + rowNum + "}", defExcVersionBo.Definitions);
                Word.FindAndReplace("{exclusion_" + rowNum + "}", defExcVersionBo.Exclusions);

                i++;
            }
        }

        public string GetPickListDescription(int? pickListDetailId)
        {
            if (pickListDetailId.HasValue)
            {
                var bo = PickListDetailService.Find(pickListDetailId);

                if (bo != null)
                    return bo.Description == null ? "" : bo.Description;
            }

            return "";
        }

        public string GetTreatyPricingPickListCodes(int objectType, int objectVersion, int pickListId)
        {
            List<string> pickListItems = TreatyPricingPickListDetailService.GetCodeByObjectPickList(objectType, objectVersion, pickListId);
            string pickListFull = "";
            int i = 0;

            foreach (string pickListItem in pickListItems)
            {
                pickListFull = pickListFull + (i == 0 ? pickListItem : ("," + pickListItem));
                i++;
            }

            return pickListFull;
        }

        public string GetTreatyPricingProductPerLifeRetroTreatyCodes(int productId)
        {
            var treatyBos = TreatyPricingProductPerLifeRetroService.GetByProductId(productId);
            string treatyCodesFull = "";
            int i = 0;

            foreach (var treatyBo in treatyBos)
            {
                treatyCodesFull = treatyCodesFull + (i == 0 ? treatyBo.TreatyPricingPerLifeRetroCode : ("," + treatyBo.TreatyPricingPerLifeRetroCode));
                i++;
            }

            return treatyCodesFull;
        }

        public void ProcessAppendixTables()
        {
            //process others
            ProcessProfitCommission();
            ProcessAdvantageProgram();
            ProcessUnderwritingQuestionnaire();
            ProcessMedicalTable();
            ProcessFinancialTable();
        }

        #region Profit Commission processing
        public void ProcessProfitCommission()
        {
            bool objectExists = false;
            IList<TreatyPricingProfitCommissionDetailBo> detailBos = null;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeProfitCommission)
                {
                    objectExists = true;
                    detailBos = TreatyPricingProfitCommissionDetailService.GetByParentEnabledOnly(searchBo.ObjectVersionId);
                }
            }

            if (objectExists)
            {
                ProcessProfitCommissionData(detailBos);
            }
        }

        public void ProcessProfitCommissionData(IList<TreatyPricingProfitCommissionDetailBo> detailBos)
        {
            int rowCount = detailBos.Count + 1;
            int columnCount = 6;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{profit_commission_details}";
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
            bool objectExists = false;
            IList<TreatyPricingAdvantageProgramVersionBenefitBo> advantageProgramBenefitBos = null;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeAdvantageProgram)
                {
                    objectExists = true;
                    advantageProgramBenefitBos = TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionId(searchBo.ObjectVersionId);
                }
            }

            if (objectExists)
            {
                ProcessAdvantageProgramData(advantageProgramBenefitBos);
            }
        }

        public void ProcessAdvantageProgramData(IList<TreatyPricingAdvantageProgramVersionBenefitBo> advantageProgramBenefitBos)
        {
            int rowCount = advantageProgramBenefitBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{advantage_program_details}";
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
            bool objectExists = false;
            IList<TreatyPricingUwQuestionnaireVersionDetailBo> questionnaireBos = null;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeUwQuestionnaire)
                {
                    objectExists = true;
                    questionnaireBos = TreatyPricingUwQuestionnaireVersionDetailService.GetByTreatyPricingUwQuestionnaireVersionId(searchBo.ObjectVersionId);
                }
            }

            if (objectExists)
            {
                ProcessUnderwritingQuestionnaireData(questionnaireBos);
            }
        }

        public void ProcessUnderwritingQuestionnaireData(IList<TreatyPricingUwQuestionnaireVersionDetailBo> questionnaireBos)
        {
            int rowCount = questionnaireBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{underwriting_questionnaire_details}";
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
            bool objectExists = false;
            IList<TreatyPricingMedicalTableUploadLegendBo> legendBos = null;
            IList<TreatyPricingMedicalTableUploadRowBo> rowBos = null;
            IList<TreatyPricingMedicalTableUploadColumnBo> columnBos = null;
            IList<TreatyPricingMedicalTableUploadCellBo> cellBos = new List<TreatyPricingMedicalTableUploadCellBo>();

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeMedicalTable)
                {
                    objectExists = true;
                    legendBos = TreatyPricingMedicalTableUploadLegendService.GetByTreatyPricingMedicalTableVersionDetailId(searchBo.ObjectVersionId);
                    rowBos = TreatyPricingMedicalTableUploadRowService.GetByTreatyPricingMedicalTableVersionDetailId(searchBo.ObjectVersionId);
                    columnBos = TreatyPricingMedicalTableUploadColumnService.GetByTreatyPricingMedicalTableVersionDetailId(searchBo.ObjectVersionId);

                    foreach (var columnBo in columnBos)
                    {
                        var columnCellBos = TreatyPricingMedicalTableUploadCellService.GetByTreatyPricingMedicalTableUploadColumnId(columnBo.Id);

                        foreach (var columnCellBo in columnCellBos)
                        {
                            cellBos.Add(columnCellBo);
                        }
                    }
                }
            }

            if (objectExists)
            {
                ProcessMedicalTableData(rowBos, columnBos, cellBos);
                ProcessMedicalTableLegends(legendBos);
            }
        }

        public void ProcessMedicalTableData(IList<TreatyPricingMedicalTableUploadRowBo> rowBos
            , IList<TreatyPricingMedicalTableUploadColumnBo> columnBos
            , IList<TreatyPricingMedicalTableUploadCellBo> cellBos)
        {
            int rowCount = rowBos.Count + 3;
            int columnCount = columnBos.Count + 2;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{medical_table_details}";
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
            bool objectExists = false;
            IList<TreatyPricingFinancialTableUploadBo> uploadBos = null;
            IList<TreatyPricingFinancialTableUploadLegendBo> legendBos = null;

            foreach (var searchBo in WorkflowObjectBos)
            {
                if (searchBo.ObjectType == TreatyPricingWorkflowObjectBo.ObjectTypeFinancialTable)
                {
                    objectExists = true;
                    uploadBos = TreatyPricingFinancialTableUploadService.GetByTreatyPricingFinancialTableVersionDetailId(searchBo.ObjectVersionId);
                    legendBos = TreatyPricingFinancialTableUploadLegendService.GetByTreatyPricingFinancialTableVersionDetailId(searchBo.ObjectVersionId);
                }
            }

            if (objectExists)
            {
                ProcessFinancialTableData(uploadBos);
                ProcessFinancialTableLegends(legendBos);
            }
        }

        public void ProcessFinancialTableData(IList<TreatyPricingFinancialTableUploadBo> uploadBos)
        {
            int rowCount = uploadBos.Count + 1;
            int columnCount = 3;
            Microsoft.Office.Interop.Word.Range range = Word.XDocument.Range();

            object findText = "{financial_table_details}";
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
public class AuthorityLimits
{
    public string BenefitName { get; set; }

    public int UwLimitVersionId { get; set; }

    public int ClaimApprovalLimitVersionId { get; set; }

    public AuthorityLimits(string benefitName, int uwLimitVersionId, int claimApprovalLimitVersionId)
    {
        BenefitName = benefitName;
        UwLimitVersionId = uwLimitVersionId;
        ClaimApprovalLimitVersionId = claimApprovalLimitVersionId;
    }
}
public class DefinitionsExlusions
{
    public string BenefitName { get; set; }

    public int DefExcVersionId { get; set; }

    public DefinitionsExlusions(string benefitName, int defExcVersionId)
    {
        BenefitName = benefitName;
        DefExcVersionId = defExcVersionId;
    }
}