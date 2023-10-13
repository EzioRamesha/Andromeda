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

namespace ConsoleApp.Commands
{
    public class ProcessGroupReferralRiGroupSlip : Command
    {
        public string FilePath { get; set; }

        public TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }

        public Word Word { get; set; }

        public List<string> Errors { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        // determine the GTL or GHS by hardcoding the benefit code: GTL to Death benefit (Benefit code DEA or DEA_N) and GHS to GHS benefit (Benefit code MSE)
        public string GtlBenefitDea = "DEA";
        public string GtlBenefitDeaN = "DEA_N";
        public string GhsBenefitMse = "MSE";

        public ProcessGroupReferralRiGroupSlip()
        {
            Title = "ProcessGroupReferralRiGroupSlip";
            Description = "To process Ri Group Slip file for group referral";
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

        public bool Process()
        {
            try
            {
                PrintMessage("Processing Ri Group Slip Group Referral Id: " + TreatyPricingGroupReferralBo.Id);

                Word = new Word(FilePath);
                Word.XApp.Caption = "Group Referral Edit - Id: " + TreatyPricingGroupReferralBo.Id;

                ProcessGroupReferralTable();

                Word.Save();

                PrintMessage("Completed Ri Group Slip Group Referral Id: " + TreatyPricingGroupReferralBo.Id);
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                PrintMessage(string.Format("Completed Ri Group Slip Group Referral Id: {0}, Exception: {1}", TreatyPricingGroupReferralBo.Id, e.Message));

                Word.Close();
            }

            PrintProcessCount();

            if (Errors.Count > 0)
            {
                foreach (string error in Errors)
                {
                    PrintError(error);
                }
                return false;
            }

            return true;
        }

        public void FindAndReplaceHeader(object findText, object replaceWithText)
        {
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object replace = 2;
            object wrap = 1;

            // Loop through all sections
            foreach (Microsoft.Office.Interop.Word.Section section in Word.XDocument.Sections)
            {
                //Get all Headers
                Microsoft.Office.Interop.Word.HeadersFooters headers = section.Headers;

                //Section headerfooter loop for all types enum WdHeaderFooterIndex. wdHeaderFooterEvenPages/wdHeaderFooterFirstPage/wdHeaderFooterPrimary;                          
                foreach (Microsoft.Office.Interop.Word.HeaderFooter header in headers)
                {
                    Microsoft.Office.Interop.Word.Range headerRange = header.Range;

                    //execute find and replace
                    headerRange.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                        ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                        ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }
            }

        }

        public void ProcessGroupReferralTable()
        {
            if (TreatyPricingGroupReferralBo == null && TreatyPricingGroupReferralVersionBo == null)
                return;

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace", TreatyPricingGroupReferralBo.Id));

            List<string> CiBenefit = new List<string> { BenefitBo.CodeCI, BenefitBo.CodeCCA, BenefitBo.CodeCCS };

            var GRBo = TreatyPricingGroupReferralBo;
            var GRVerBo = TreatyPricingGroupReferralVersionBo;

            FindAndReplaceHeader("<<RI Group Slip ID>>", GRBo.RiGroupSlipCode);

            Word.FindAndReplace("<<Insured Group Name>>", GRBo.InsuredGroupNameBo?.Name);
            Word.FindAndReplace("<<Referred Type>>", (GRBo.ReferredTypePickListDetailId.HasValue ? PickListDetailService.Find(GRBo.ReferredTypePickListDetailId).Description : ""));
            Word.FindAndReplace("<<Coverage Start Date>>", GRBo.CoverageStartDateStr);
            Word.FindAndReplace("<<Coverage End Date>>", GRBo.CoverageEndDateStr);

            Word.FindAndReplace("<<Underwriting Method>>", GRVerBo.UnderwritingMethod);
            Word.FindAndReplace("<<Compulsory/Voluntary>>", TreatyPricingGroupReferralVersionBo.GetCompulsoryOrVoluntaryName(GRVerBo.IsCompulsoryOrVoluntary));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: foreach gtlbenefits", TreatyPricingGroupReferralBo.Id));

            var gtlBenefits = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdForProcessRiGroupSlip(GRVerBo.Id, true).Where(a => a.BenefitBo.Code != GhsBenefitMse).ToList();
            if (!gtlBenefits.IsNullOrEmpty())
            {
                int gtlBenefitCount = 1;
                foreach (var gtlBenefit in gtlBenefits)
                {
                    Word.FindAndReplace(string.Format("<<Benefit {0}>>", gtlBenefitCount), gtlBenefit.BenefitBo.Description);
                    Word.FindAndReplace(string.Format("<<Free Cover Limit {0}>>", gtlBenefitCount), (CiBenefit.Contains(gtlBenefit.BenefitBo.Code) ? gtlBenefit.RequestedFreeCoverLimitCI : gtlBenefit.RequestedFreeCoverLimitNonCI));
                    Word.FindAndReplace(string.Format("<<Max Free Cover Limit Eligibility Age {0}>>", gtlBenefitCount), (CiBenefit.Contains(gtlBenefit.BenefitBo.Code) ? gtlBenefit.GroupFreeCoverLimitAgeCI : gtlBenefit.GroupFreeCoverLimitAgeNonCI));
                    Word.FindAndReplace(string.Format("<<Min Entry Age {0}>>", gtlBenefitCount), gtlBenefit.MinimumEntryAge);
                    Word.FindAndReplace(string.Format("<<Max Entry Age {0}>>", gtlBenefitCount), gtlBenefit.MaximumEntryAge);
                    Word.FindAndReplace(string.Format("<<Max Expiry Age {0}>>", gtlBenefitCount), gtlBenefit.MaximumExpiryAge);

                    if (gtlBenefit.TreatyPricingUwLimitId.HasValue)
                    {
                        var uwLimitBenefitCodes = TreatyPricingUwLimitService.GetBenefitCodesById(gtlBenefit.TreatyPricingUwLimitId);
                        var uwLimitVer = TreatyPricingUwLimitVersionService.GetLatestByTreatyPricingUwLimitId(gtlBenefit.TreatyPricingUwLimitId.Value);
                        Word.FindAndReplace(string.Format("<<Auto Binding Limit {0}>>", gtlBenefitCount), (!uwLimitBenefitCodes.IsNullOrEmpty() && uwLimitBenefitCodes.Contains(gtlBenefit.BenefitBo.Code) ? (uwLimitVer != null ? uwLimitVer.AblSumAssured : "0") : "0"));
                    }
                    else
                    {
                        Word.FindAndReplace(string.Format("<<Auto Binding Limit {0}>>", gtlBenefitCount), "0");
                    }

                    gtlBenefitCount++;
                }
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: foreach gtlbenefits", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: foreach dthBenefit exists", TreatyPricingGroupReferralBo.Id));

            TreatyPricingGroupReferralVersionBenefitBo dthBenefit = null;
            if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, GtlBenefitDea))
                dthBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdForProcessRiGroupSlip(GRVerBo.Id, true).Where(a => a.BenefitBo.Code == GtlBenefitDea).FirstOrDefault();
            else
                dthBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdForProcessRiGroupSlip(GRVerBo.Id, true).Where(a => a.BenefitBo.Code == GtlBenefitDeaN).FirstOrDefault();
            if (dthBenefit != null)
            {
                string value = "";
                switch (dthBenefit.OtherSpecialReinsuranceArrangement)
                {
                    case TreatyPricingGroupReferralVersionBenefitBo.OtherSpecialReinsuranceArrangementNormal:
                        value = dthBenefit.CedantRetention;
                        break;
                    case TreatyPricingGroupReferralVersionBenefitBo.OtherSpecialReinsuranceArrangementMaxisPooling:
                        value = "AXA shall have another arrangement with MAXIS, where MAXIS retains the first 50% Quota Share. AXA will thereafter retain a maximum of RM60,000";
                        break;
                    default:
                        break;
                }
                Word.FindAndReplace("<<Other Special RI Arrangement >>", value);
                Word.FindAndReplace("<<Cedant’s Retention>>", dthBenefit.CedantRetention);
                Word.FindAndReplace("<<Reinsurance’s Share>>", dthBenefit.ReinsuranceShare);
                //Word.FindAndReplace("<<Reinsurer’s Share>>", dthBenefit.ReinsuranceShare);
                Word.FindAndReplace("<<Age Definition>>", dthBenefit.AgeBasisPickListDetailBo.Description);
                Word.FindAndReplace("<<Other Special Terms>>", dthBenefit.OtherSpecialTerms);

                if (dthBenefit.HasGroupProfitCommission)
                {
                    if (dthBenefit.IsOverwriteGroupProfitCommission)
                        Word.FindAndReplace("<<Profit Commission>>", dthBenefit.OverwriteGroupProfitCommissionRemarks);
                    else
                        Word.FindAndReplace("<<Profit Commission>>", "");
                }
                else
                {
                    Word.FindAndReplace("<<Profit Commission>>", "Nil");
                }

                Word.FindAndReplace("<<GTL Other Special Terms>>", dthBenefit.OtherSpecialTerms);
                Word.FindAndReplace("<<GTL Coinsurance Cedant’s Retention>>", dthBenefit.CoinsuranceCedantRetention);
                Word.FindAndReplace("<<GTL Coinsurance Reinsurer’s Share>>", dthBenefit.CoinsuranceReinsurerShare);
                Word.FindAndReplace("<<GTL Coinsurance RI Discount>>", dthBenefit.CoinsuranceRiDiscount);
                Word.FindAndReplace("<<GTL Cedant’s Retention>>", dthBenefit.CedantRetention);
                Word.FindAndReplace("<<GTL Reinsurer’s Share>>", dthBenefit.ReinsuranceShare);
                Word.FindAndReplace("<<GTL Overwrite Group Profit Commission>>", dthBenefit.IsOverwriteGroupProfitCommission ? dthBenefit.OverwriteGroupProfitCommissionRemarks : "");
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: foreach dthBenefit exists", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: ciBenefit exists", TreatyPricingGroupReferralBo.Id));

            TreatyPricingGroupReferralVersionBenefitBo ciBenefit = null;
            if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCA))
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCodeForProcessRiGroupSlip(GRVerBo.Id, BenefitBo.CodeCCA);
            else if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCS))
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCodeForProcessRiGroupSlip(GRVerBo.Id, BenefitBo.CodeCCS);
            else
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCodeForProcessRiGroupSlip(GRVerBo.Id, BenefitBo.CodeCI);
            if (ciBenefit != null)
            {
                Word.FindAndReplace("<<CI Coinsurance Cedant’s Retention>>", ciBenefit.CoinsuranceCedantRetention);
                Word.FindAndReplace("<<CI Coinsurance Reinsurer’s Share>>", ciBenefit.CoinsuranceReinsurerShare);
                Word.FindAndReplace("<<CI Cedant’s Retention>>", string.Format(", and surplus in excess of Sum Assured {0} for Critical Illness benefit ", ciBenefit.CedantRetention));
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: ciBenefit exists", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: ghsBenefit", TreatyPricingGroupReferralBo.Id));

            var ghsBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdForProcessRiGroupSlip(GRVerBo.Id, true).Where(a => a.BenefitBo.Code == GhsBenefitMse).FirstOrDefault();
            if (ghsBenefit != null)
            {
                Word.FindAndReplace("<<GHS Benefit>>", ghsBenefit.BenefitBo.Description);
                Word.FindAndReplace("<<GHS Free Cover Limit>>", ghsBenefit.RequestedFreeCoverLimitNonCI);
                Word.FindAndReplace("<<GHS Max Free Cover Limit Eligibility Age>>", ghsBenefit.GroupFreeCoverLimitNonCI);
                Word.FindAndReplace("<<GHS Min Entry Age>>", ghsBenefit.MinimumEntryAge);
                Word.FindAndReplace("<<GHS Max Entry Age>>", ghsBenefit.MaximumEntryAge);
                Word.FindAndReplace("<<GHS Max Expiry Age>>", ghsBenefit.MaximumExpiryAge);

                var uwLimitBenefitCodes = TreatyPricingUwLimitService.GetBenefitCodesById(ghsBenefit.TreatyPricingUwLimitId);
                Word.FindAndReplace("<<GHS Auto Binding Limit>>", (!uwLimitBenefitCodes.IsNullOrEmpty() && uwLimitBenefitCodes.Contains(ghsBenefit.BenefitCode) ? (ghsBenefit.TreatyPricingUwLimitVersionBo != null ? ghsBenefit.TreatyPricingUwLimitVersionBo.AblSumAssured : "0") : "0"));

                Word.FindAndReplace("<<GHS Coinsurance Cedant’s Retention>>", ghsBenefit.CoinsuranceCedantRetention);
                Word.FindAndReplace("<<GHS Coinsurance Reinsurer’s Share>> ", ghsBenefit.CoinsuranceReinsurerShare);
                Word.FindAndReplace("<<GHS Coinsurance RI Discount>>", ghsBenefit.CoinsuranceRiDiscount);
                Word.FindAndReplace("<<GHS Reinsurer’s Share>>", ghsBenefit.ReinsuranceShare);
                Word.FindAndReplace("<<GHS Cedant’s Retention>>", ghsBenefit.CedantRetention);
                Word.FindAndReplace("<<GHS RI Discount>>", ghsBenefit.RiDiscount);
                Word.FindAndReplace("<<GHS Other Special Terms>>", ghsBenefit.OtherSpecialTerms);
                Word.FindAndReplace("<<GHS Overwrite Group Profit Commission>>", ghsBenefit.IsOverwriteGroupProfitCommission ? ghsBenefit.OverwriteGroupProfitCommissionRemarks : "Nil");
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: ghsBenefit", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: GtlBasisOfSaBenefitCodes", TreatyPricingGroupReferralBo.Id));

            List<string> GtlBasisOfSaBenefitCodes = new List<string> { "DEA", "CI", "ADD", "TIA", "FUNE" };
            int basisOfSaBenefitCount = 1;
            foreach (var basisOfSaBenefitCode in GtlBasisOfSaBenefitCodes)
            {
                string benefitDesc = "";
                switch (basisOfSaBenefitCode)
                {
                    case "DEA":
                        benefitDesc = "Death";
                        break;
                    case "CI":
                        benefitDesc = "Accelerated CI / Additional CI";
                        break;
                    case "ADD":
                        benefitDesc = "Accidental Death & Dismemberment";
                        break;
                    case "TIA":
                        benefitDesc = "Accelerated TI";
                        break;
                    case "FUNE":
                        benefitDesc = "Funeral Expenses";
                        break;
                }
                Word.FindAndReplace(string.Format("<<SA Benefit {0}>>", basisOfSaBenefitCount), benefitDesc);

                PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: gtlBasisOfSaBo", TreatyPricingGroupReferralBo.Id));

                IList<TreatyPricingGroupReferralGtlTableBo> gtlBasisOfSaBo = null;
                if (basisOfSaBenefitCode == "CI")
                    gtlBasisOfSaBo = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(GRBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa).Where(a => CiBenefit.Contains(a.BenefitCode)).ToList();
                else
                    gtlBasisOfSaBo = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(GRBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa).Where(a => a.BenefitCode == basisOfSaBenefitCode).ToList();
                if (!gtlBasisOfSaBo.IsNullOrEmpty())
                {
                    int basisOfSaCount = 1;
                    foreach (var basisOfSaBo in gtlBasisOfSaBo)
                    {
                        Word.FindAndReplace(string.Format("<<Designation {0} for SA Benefit {1}>>", basisOfSaCount, basisOfSaBenefitCount), basisOfSaBo.Designation);
                        Word.FindAndReplace(string.Format("<<Basis of SA {0} for SA Benefit {1}>>", basisOfSaCount, basisOfSaBenefitCount), basisOfSaBo.BasisOfSA);

                        basisOfSaCount++;
                    }
                }

                PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: gtlBasisOfSaBo", TreatyPricingGroupReferralBo.Id));

                basisOfSaBenefitCount++;
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: GtlBasisOfSaBenefitCodes", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: foreach gtlUnitRateBo", TreatyPricingGroupReferralBo.Id));

            var gtlUnitRateBo = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(GRBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);
            if (!gtlUnitRateBo.IsNullOrEmpty())
            {
                int unitRateCount = 1;
                foreach (var unitRateBo in gtlUnitRateBo)
                {
                    Word.FindAndReplace(string.Format("<<Unit Rates Benefit {0}>>", unitRateCount), BenefitService.FindByCode(unitRateBo.BenefitCode)?.Description);
                    Word.FindAndReplace(string.Format("<<Risk Rate Per Thousand {0}>>", unitRateCount), !string.IsNullOrEmpty(unitRateBo.RiskRate) ? unitRateBo.RiskRate : "0");
                    Word.FindAndReplace(string.Format("<<Gross Rate Per Thousand {0}>>", unitRateCount), !string.IsNullOrEmpty(unitRateBo.GrossRate) ? unitRateBo.GrossRate : "0");

                    unitRateCount++;
                }
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: foreach gtlUnitRateBo", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, Start Find and Replace: foreach gtlAgeBandedBo", TreatyPricingGroupReferralBo.Id));

            var gtlAgeBandedBo = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralIdType(GRBo.Id, TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded);
            if (!gtlAgeBandedBo.IsNullOrEmpty())
            {
                int ageBandedCount = 1;
                foreach (var ageBandedBo in gtlAgeBandedBo)
                {
                    Word.FindAndReplace(string.Format("<<Age Banded Benefit {0}>>", ageBandedCount), BenefitService.FindByCode(ageBandedBo.BenefitCode)?.Description);
                    Word.FindAndReplace(string.Format("<<Age Band Range {0}>>", ageBandedCount), ageBandedBo.AgeBandRange);
                    Word.FindAndReplace(string.Format("<<Age Banded Risk Rate Per Thousand {0}>>", ageBandedCount), !string.IsNullOrEmpty(ageBandedBo.RiskRate) ? ageBandedBo.RiskRate : "0");
                    Word.FindAndReplace(string.Format("<<Age Banded Gross Rate Per Thousand {0}>>", ageBandedCount), !string.IsNullOrEmpty(ageBandedBo.GrossRate) ? ageBandedBo.GrossRate : "0");

                    ageBandedCount++;
                }
            }

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace: foreach gtlAgeBandedBo", TreatyPricingGroupReferralBo.Id));

            PrintMessage(string.Format("ProcessGroupReferralRiGroupSlip - Group Referral Id: {0}, End Find and Replace", TreatyPricingGroupReferralBo.Id));
        }
    }
}
