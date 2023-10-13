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
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    public class ProcessGroupReferralReply : Command
    {
        public string FilePath { get; set; }

        public TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }

        public IList<TreatyPricingGroupReferralVersionBenefitBo> TreatyPricingGroupReferralVersionBenefitBos { get; set; }

        public IList<TreatyPricingGroupReferralGtlTableBo> TreatyPricingGroupReferralGtlTableBos { get; set; }

        public Word Word { get; set; }

        public List<string> Errors { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        // determine the GTL or GHS by hardcoding the benefit code: GTL to Death benefit (Benefit code DEA or DEA_N) and GHS to GHS benefit (Benefit code MSE)
        public string GtlBenefitDea = "DEA";
        public string GtlBenefitDeaN = "DEA_N";
        public string GhsBenefitMse = "MSE";

        public ProcessGroupReferralReply()
        {
            Title = "ProcessGroupReferralReply";
            Description = "To generate reply template for group referral";
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
                Word = new Word(FilePath);
                Word.XApp.Caption = "Reply Template";

                ProcessGroupReferralTable();

                Word.Save();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
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

        public void ProcessGroupReferralTable()
        {
            if (TreatyPricingGroupReferralBo == null && TreatyPricingGroupReferralVersionBo == null)
                return;

            var GRBo = TreatyPricingGroupReferralBo;
            var GRVerBo = TreatyPricingGroupReferralVersionBo;

            Word.FindAndReplace("<<Request Received Date>>", GRVerBo.RequestReceivedDateStr);
            Word.FindAndReplace("<<Ceding Company>>", GRBo.CedantBo.Name);

            TreatyPricingGroupReferralVersionBenefitBo dthBenefit = null;
            if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, GtlBenefitDea))
                dthBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(GRVerBo.Id, true).Where(a => a.BenefitBo.Code == GtlBenefitDea).FirstOrDefault();
            else
                dthBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(GRVerBo.Id, true).Where(a => a.BenefitBo.Code == GtlBenefitDeaN).FirstOrDefault();
            if (dthBenefit != null)
            {
                Word.FindAndReplace("<<Requested Free Cover Limit (Non-CI)>>", dthBenefit.RequestedFreeCoverLimitNonCI);
                Word.FindAndReplace("<<Benefit 1>>", dthBenefit.BenefitBo.Description);
                Word.FindAndReplace("<<Other Special Terms>>", dthBenefit.OtherSpecialTerms);
            }

            TreatyPricingGroupReferralVersionBenefitBo ciBenefit = null;
            if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCA))
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCA);
            else if (TreatyPricingGroupReferralVersionBenefitService.ExistByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCS))
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCCS);
            else
                ciBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(GRVerBo.Id, BenefitBo.CodeCI);
            if (ciBenefit != null)
            {
                Word.FindAndReplace("<<Requested Free Cover Limit (CI)>>", ciBenefit.RequestedFreeCoverLimitCI);
            }

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
        }
    }
}
