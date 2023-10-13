using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyPricingCampaignViewModel : ObjectVersion, IValidatableObject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int TreatyPricingCedantId { get; set; }
        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        [Required, StringLength(60), DisplayName("Campaign ID")]
        public string Code { get; set; }

        [Required, DisplayName("Campaign Name")]
        public string Name { get; set; }

        public int Status { get; set; }

        [DisplayName("Campaign Purpose")]
        public string Purpose { get; set; }

        public string Type { get; set; }

        [DisplayName("Campaign Duration")]
        public string Duration { get; set; }

        [DisplayName("Campaign Period Start Date")]
        public DateTime? PeriodStartDate { get; set; }
        [DisplayName("Campaign Period Start Date")]
        [ValidateDate]
        public string PeriodStartDateStr { get; set; }

        [DisplayName("Campaign Period End Date")]
        public DateTime? PeriodEndDate { get; set; }
        [DisplayName("Campaign Period End Date")]
        [ValidateDate]
        public string PeriodEndDateStr { get; set; }

        [DisplayName("Target Take Up Rate")]
        public string TargetTakeUpRate { get; set; }

        [DisplayName("Expected Average Sum Assured")]
        public string AverageSumAssured { get; set; }

        [DisplayName("Expected RI Premium Receivable")]
        public string RiPremiumReceivable { get; set; }

        [DisplayName("Expected Number of Policies")]
        public string NoOfPolicy { get; set; }

        public string Remarks { get; set; }

        //[DisplayName("Version")]
        //public int Version { get; set; }

        [Required, DisplayName("Person In-Charge")]
        public int PersonInChargeId { get; set; }

        // Campaign Benefits & Terms
        [DisplayName("Benefit")]
        public bool IsPerBenefit { get; set; } = true;
        [DisplayName("Benefit Remarks")]
        public string BenefitRemark { get; set; }

        [DisplayName("Ceding Company's Retention")]
        public bool IsPerCedantRetention { get; set; }
        [DisplayName("If Others")]
        public string CedantRetention { get; set; }

        [DisplayName("MLRe's Share")]
        public bool IsPerMlreShare { get; set; }
        [DisplayName("If Others")]
        public string MlreShare { get; set; }

        [DisplayName("Distribution Channel")]
        public bool IsPerDistributionChannel { get; set; }
        [DisplayName("If Others")]
        public string DistributionChannel { get; set; }

        [DisplayName("Age Basis")]
        public bool IsPerAgeBasis { get; set; }
        [DisplayName("If Others")]
        public int? AgeBasisPickListDetailId { get; set; }

        [DisplayName("Minimum Entry Age")]
        public bool IsPerMinEntryAge { get; set; }
        [DisplayName("If Others")]
        public string MinEntryAge { get; set; }

        [DisplayName("Maximum Entry Age")]
        public bool IsPerMaxEntryAge { get; set; }
        [DisplayName("If Others")]
        public string MaxEntryAge { get; set; }

        [DisplayName("Maximum Expiry Age")]
        public bool IsPerMaxExpiryAge { get; set; }
        [DisplayName("If Others")]
        public string MaxExpiryAge { get; set; }

        [DisplayName("Minimum Sum Assured")]
        public bool IsPerMinSumAssured { get; set; }
        [DisplayName("If Others")]
        public string MinSumAssured { get; set; }

        [DisplayName("Maximum Sum Assured")]
        public bool IsPerMaxSumAssured { get; set; }
        [DisplayName("If Others")]
        public string MaxSumAssured { get; set; }

        [DisplayName("Reinsurance Rate")]
        public bool IsPerReinsuranceRate { get; set; }
        [DisplayName("If Others")]
        public string ReinsRateTreatyPricingRateTableSelect { get; set; }
        [DisplayName("Note")]
        public string ReinsRateNote { get; set; }

        [DisplayName("Reinsurance Discount")]
        public bool IsPerReinsuranceDiscount { get; set; }
        [DisplayName("If Others")]
        public string ReinsDiscountTreatyPricingRateTableSelect { get; set; }
        [DisplayName("Note")]
        public string ReinsDiscountNote { get; set; }

        [DisplayName("Profit Commission")]
        public bool IsPerProfitComm { get; set; }
        [DisplayName("If Others")]
        public string TreatyPricingProfitCommissionSelect { get; set; }
        [DisplayName("Note")]
        public string ProfitCommNote { get; set; }

        [DisplayName("Campaign Funding By MLRe")]
        public string CampaignFundByMlre { get; set; }

        [DisplayName("Complimentary Sum Assured")]
        public string ComplimentarySumAssured { get; set; }

        // Campaign Underwriting Parameters
        [DisplayName("Underwriting Method")]
        public bool IsPerUnderwritingMethod { get; set; }
        [DisplayName("If Others")]
        public string UnderwritingMethod { get; set; }

        [DisplayName("Underwriting Questions")]
        public bool IsPerUnderwritingQuestion { get; set; }
        [DisplayName("If Others")]
        public string TreatyPricingUwQuestionnaireSelect { get; set; }
        [DisplayName("Note")]
        public string UnderwritingQuestionNote { get; set; }

        [DisplayName("Medical Table")]
        public bool IsPerMedicalTable { get; set; }
        [DisplayName("If Others")]
        public string TreatyPricingMedicalTableSelect { get; set; }
        [DisplayName("Note")]
        public string MedicalTableNote { get; set; }

        [DisplayName("Financial Table")]
        public bool IsPerFinancialTable { get; set; }
        [DisplayName("If Others")]
        public string TreatyPricingFinancialTableSelect { get; set; }
        [DisplayName("Note")]
        public string FinancialTableNote { get; set; }

        [DisplayName("Aggregation Notes")]
        public bool IsPerAggregationNotes { get; set; }
        [DisplayName("If Others")]
        public string AggregationNotes { get; set; }

        [DisplayName("Advantage Program")]
        public bool IsPerAdvantageProgram { get; set; }
        [DisplayName("If Others")]
        public string TreatyPricingAdvantageProgramSelect { get; set; }
        [DisplayName("Note")]
        public string AdvantageProgramNote { get; set; }

        // Miscellaneous
        [DisplayName("Waiting Period")]
        public bool IsPerWaitingPeriod { get; set; }
        [DisplayName("If Others")]
        public string WaitingPeriod { get; set; }

        [DisplayName("Survival Period")]
        public bool IsPerSurvivalPeriod { get; set; }
        [DisplayName("If Others")]
        public string SurvivalPeriod { get; set; }

        [DisplayName("Other Campaign Criteria")]
        public string OtherCampaignCriteria { get; set; }

        [DisplayName("Additional Remark")]
        public string AdditionalRemark { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingCampaignViewModel()
        {
            Set();
        }

        public TreatyPricingCampaignViewModel(TreatyPricingCampaignBo advantageProgramBo)
        {
            Set(advantageProgramBo);
            SetVersionObjects(advantageProgramBo.TreatyPricingCampaignVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingCampaignBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            TreatyPricingCedantId = bo.TreatyPricingCedantId;
            TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
            CedantId = bo.TreatyPricingCedantBo.CedantId;
            Code = bo.Code;
            Name = bo.Name;
            Status = bo.Status;
            Type = bo.Type;
            Purpose = bo.Purpose;
            PeriodStartDate = bo.PeriodStartDate;
            PeriodStartDateStr = bo.PeriodStartDateStr;
            PeriodEndDate = bo.PeriodEndDate;
            PeriodEndDateStr = bo.PeriodEndDateStr;
            Duration = bo.Duration;
            TargetTakeUpRate = bo.TargetTakeUpRate;
            AverageSumAssured = bo.AverageSumAssured;
            RiPremiumReceivable = bo.RiPremiumReceivable;
            NoOfPolicy = bo.NoOfPolicy;
            Remarks = bo.Remarks;
        }

        public TreatyPricingCampaignBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingCampaignBo
            {
                Id = Id,
                TreatyPricingCedantId = TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(TreatyPricingCedantId),
                Code = Code,
                Name = Name,
                Status = Status,
                Type = Type,
                Purpose = Purpose,
                PeriodStartDate = Util.GetParseDateTime(PeriodStartDateStr),
                PeriodStartDateStr = PeriodStartDateStr,
                PeriodEndDate = Util.GetParseDateTime(PeriodEndDateStr),
                PeriodEndDateStr = PeriodEndDateStr,
                Duration = Duration,
                TargetTakeUpRate = TargetTakeUpRate,
                AverageSumAssured = AverageSumAssured,
                RiPremiumReceivable = RiPremiumReceivable,
                NoOfPolicy = NoOfPolicy,
                Remarks = Remarks,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingCampaignVersionBo GetVersionBo(TreatyPricingCampaignVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.IsPerBenefit = IsPerBenefit;
            bo.BenefitRemark = BenefitRemark;
            bo.IsPerCedantRetention = IsPerCedantRetention;
            bo.CedantRetention = CedantRetention;
            bo.IsPerMlreShare = IsPerMlreShare;
            bo.MlreShare = MlreShare;
            bo.IsPerDistributionChannel = IsPerDistributionChannel;
            bo.DistributionChannel = DistributionChannel;
            bo.IsPerAgeBasis = IsPerAgeBasis;
            bo.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
            bo.IsPerMinEntryAge = IsPerMinEntryAge;
            bo.MinEntryAge = MinEntryAge;
            bo.IsPerMaxEntryAge = IsPerMaxEntryAge;
            bo.MaxEntryAge = MaxEntryAge;
            bo.IsPerMaxExpiryAge = IsPerMaxExpiryAge;
            bo.MaxExpiryAge = MaxExpiryAge;
            bo.IsPerMinSumAssured = IsPerMinSumAssured;
            bo.MinSumAssured = MinSumAssured;
            bo.IsPerMaxSumAssured = IsPerMaxSumAssured;
            bo.MaxSumAssured = MaxSumAssured;
            bo.IsPerReinsuranceRate = IsPerReinsuranceRate;
            bo.ReinsRateTreatyPricingRateTableSelect = ReinsRateTreatyPricingRateTableSelect;
            bo.ReinsRateNote = ReinsRateNote;
            bo.IsPerReinsuranceDiscount = IsPerReinsuranceDiscount;
            bo.ReinsDiscountTreatyPricingRateTableSelect = ReinsDiscountTreatyPricingRateTableSelect;
            bo.ReinsDiscountNote = ReinsDiscountNote;
            bo.IsPerProfitComm = IsPerProfitComm;
            bo.TreatyPricingProfitCommissionSelect = TreatyPricingProfitCommissionSelect;
            bo.ProfitCommNote = ProfitCommNote;
            bo.CampaignFundByMlre = CampaignFundByMlre;
            bo.ComplimentarySumAssured = ComplimentarySumAssured;
            bo.IsPerUnderwritingMethod = IsPerUnderwritingMethod;
            bo.UnderwritingMethod = UnderwritingMethod;
            bo.IsPerUnderwritingQuestion = IsPerUnderwritingQuestion;
            bo.TreatyPricingUwQuestionnaireSelect = TreatyPricingUwQuestionnaireSelect;
            bo.UnderwritingQuestionNote = UnderwritingQuestionNote;
            bo.IsPerMedicalTable = IsPerMedicalTable;
            bo.TreatyPricingMedicalTableSelect = TreatyPricingMedicalTableSelect;
            bo.MedicalTableNote = MedicalTableNote;
            bo.IsPerFinancialTable = IsPerFinancialTable;
            bo.TreatyPricingFinancialTableSelect = TreatyPricingFinancialTableSelect;
            bo.FinancialTableNote = FinancialTableNote;
            bo.IsPerAggregationNotes = IsPerAggregationNotes;
            bo.AggregationNotes = AggregationNotes;
            bo.IsPerAdvantageProgram = IsPerAdvantageProgram;
            bo.TreatyPricingAdvantageProgramSelect= TreatyPricingAdvantageProgramSelect;
            bo.AdvantageProgramNote = AdvantageProgramNote;
            bo.IsPerWaitingPeriod = IsPerWaitingPeriod;
            bo.WaitingPeriod = WaitingPeriod;
            bo.IsPerSurvivalPeriod = IsPerSurvivalPeriod;
            bo.SurvivalPeriod = SurvivalPeriod;
            bo.OtherCampaignCriteria = OtherCampaignCriteria;
            bo.AdditionalRemark = AdditionalRemark;

            return bo;
        }

        public void ProcessProducts(FormCollection form, ref TrailObject trail)
        {
            int maxIndex = int.Parse(form.Get("CampaignProductsMaxIndex"));
            int index = 0;

            if (maxIndex != index)
            {
                // Delete all
                TreatyPricingCampaignProductService.DeleteAllByTreatyPricingCampaignId(Id);

                while (index <= maxIndex)
                {
                    // Create
                    string productId = form.Get(string.Format("productId[{0}]", index));
                    if (!string.IsNullOrEmpty(productId))
                    {
                        TreatyPricingCampaignProductBo bo = new TreatyPricingCampaignProductBo
                        {
                            TreatyPricingCampaignId = Id,
                            TreatyPricingProductId = int.Parse(productId),
                        };
                        TreatyPricingCampaignProductService.Create(ref bo, ref trail);
                    }
                    index++;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(PeriodStartDateStr);
            DateTime? end = Util.GetParseDateTime(PeriodEndDateStr);

            if (start == null && end != null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Campaign Period Start Date Field"),
                    new[] { nameof(PeriodStartDateStr) }));
            }
            else if (start != null && end == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Campaign Period End Date Field"),
                    new[] { nameof(PeriodEndDateStr) }));
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Campaign Period Start Date"),
                    new[] { nameof(PeriodStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Campaign Period End Date"),
                    new[] { nameof(PeriodEndDateStr) }));
                }
            }

            if (!string.IsNullOrEmpty(AverageSumAssured))
            {
                if (!double.TryParse(AverageSumAssured.ToString(), out double _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", AverageSumAssured, "Expected Average Sum Assured"),
                    new[] { nameof(AverageSumAssured) }));
                }
            }

            if (!string.IsNullOrEmpty(RiPremiumReceivable))
            {
                if (!double.TryParse(RiPremiumReceivable.ToString(), out double _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", RiPremiumReceivable, "Expected RI Premium Receivable"),
                    new[] { nameof(RiPremiumReceivable) }));
                }
            }

            if (!string.IsNullOrEmpty(MinEntryAge))
            {
                if (!int.TryParse(MinEntryAge.ToString(), out int _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", MinEntryAge, "Minimum Entry Age"),
                    new[] { nameof(MinEntryAge) }));
                }
            }

            if (!string.IsNullOrEmpty(MaxEntryAge))
            {
                if (!int.TryParse(MaxEntryAge.ToString(), out int _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", MaxEntryAge, "Maximum Entry Age"),
                    new[] { nameof(MaxEntryAge) }));
                }
            }

            if (!string.IsNullOrEmpty(MaxExpiryAge))
            {
                if (!int.TryParse(MaxExpiryAge.ToString(), out int _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", MaxExpiryAge, "Maximum Expiry Age"),
                    new[] { nameof(MaxExpiryAge) }));
                }
            }

            if (!string.IsNullOrEmpty(MinSumAssured))
            {
                if (!double.TryParse(MinSumAssured.ToString(), out double _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", MinSumAssured, "Minimum Sum Assured"),
                    new[] { nameof(MinSumAssured) }));
                }
            }

            if (!string.IsNullOrEmpty(MaxSumAssured))
            {
                if (!double.TryParse(MaxSumAssured.ToString(), out double _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not valid for {1}.", MaxSumAssured, "Maximum Sum Assured"),
                    new[] { nameof(MaxSumAssured) }));
                }
            }

            return results;
        }
    }
}