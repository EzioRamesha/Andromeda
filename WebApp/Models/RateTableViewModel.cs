using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RateTableViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public int? BenefitId { get; set; }

        public Benefit Benefit { get; set; }

        public BenefitBo BenefitBo { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Premium Frequency Code")]
        public int? PremiumFrequencyCodePickListDetailId { get; set; }

        public PickListDetailBo PremiumFrequencyCodePickListDetailBo { get; set; }

        public PickListDetail PremiumFrequencyCodePickListDetail { get; set; }

        [DisplayName("ORI Sum Assured From")]
        public double? PolicyAmountFrom { get; set; }

        [DisplayName("ORI Sum Assured From")]
        [ValidateDouble]
        public string PolicyAmountFromStr { get; set; }

        [DisplayName("ORI Sum Assured To")]
        public double? PolicyAmountTo { get; set; }

        [DisplayName("ORI Sum Assured To")]
        [ValidateDouble]
        public string PolicyAmountToStr { get; set; }

        [DisplayName("Insured Attained Age From")]
        public int? AttainedAgeFrom { get; set; }

        [DisplayName("Insured Attained Age To")]
        public int? AttainedAgeTo { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        [ValidateDate]
        public string ReinsEffDatePolStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        [ValidateDate]
        public string ReinsEffDatePolEndDateStr { get; set; }

        [DisplayName("Ceding Treaty Code")]
        public string CedingTreatyCode { get; set; }

        [DisplayName("Ceding Plan Code 2")]
        public string CedingPlanCode2 { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        [ValidateCedingBenefitTypeCode]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("Group Policy Number")]
        public string GroupPolicyNumber { get; set; }

        [DisplayName("Reinsurance Basis Code")]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [DisplayName("AAR From")]
        public double? AarFrom { get; set; }

        [DisplayName("AAR From")]
        [ValidateDouble]
        public string AarFromStr { get; set; }

        [DisplayName("AAR To")]
        public double? AarTo { get; set; }

        [DisplayName("AAR To")]
        [ValidateDouble]
        public string AarToStr { get; set; }

        // Phase 2
        [DisplayName("Policy Term From")]
        public double? PolicyTermFrom { get; set; }

        [DisplayName("Policy Term From")]
        [ValidateDouble]
        public string PolicyTermFromStr { get; set; }

        [DisplayName("Policy Term To")]
        public double? PolicyTermTo { get; set; }

        [DisplayName("Policy Term To")]
        [ValidateDouble]
        public string PolicyTermToStr { get; set; }

        [DisplayName("Policy Duration From")]
        public double? PolicyDurationFrom { get; set; }

        [DisplayName("Policy Duration From")]
        [ValidateDouble]
        public string PolicyDurationFromStr { get; set; }

        [DisplayName("Policy Duration To")]
        public double? PolicyDurationTo { get; set; }

        [DisplayName("Policy Duration To")]
        [ValidateDouble]
        public string PolicyDurationToStr { get; set; }

        [DisplayName("Rate Table Code")]
        public int? RateId { get; set; }

        public Rate Rate { get; set; }

        public RateBo RateBo { get; set; }

        [DisplayName("Ceding Company")]
        [Required]
        public int? CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [DisplayName("RI Discount Code"), StringLength(30)]
        public string RiDiscountCode { get; set; }

        [DisplayName("Large Discount Code"), StringLength(30)]
        public string LargeDiscountCode { get; set; }

        [DisplayName("Group Discount Code"), StringLength(30)]
        public string GroupDiscountCode { get; set; }

        [DisplayName("Reporting Start Date")]
        public DateTime? ReportingStartDate { get; set; }

        [DisplayName("Reporting Start Date")]
        [ValidateDate]
        public string ReportingStartDateStr { get; set; }

        [DisplayName("Reporting End Date")]
        public DateTime? ReportingEndDate { get; set; }

        [DisplayName("Reporting End Date")]
        [ValidateDate]
        public string ReportingEndDateStr { get; set; }

        public virtual ICollection<RateTableDetail> RateTableDetails { get; set; }

        public RateTableViewModel() { }

        public RateTableViewModel(RateTableBo rateTableBo)
        {
            Set(rateTableBo);
        }

        public void Set(RateTableBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCode = bo.TreatyCode;
                CedingTreatyCode = bo.CedingTreatyCode;
                BenefitId = bo.BenefitId;
                BenefitBo = bo.BenefitBo;
                CedingPlanCode = bo.CedingPlanCode;
                CedingPlanCode2 = bo.CedingPlanCode2;
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                GroupPolicyNumber = bo.GroupPolicyNumber;

                PremiumFrequencyCodePickListDetailId = bo.PremiumFrequencyCodePickListDetailId;
                PremiumFrequencyCodePickListDetailBo = bo.PremiumFrequencyCodePickListDetailBo;
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                ReinsBasisCodePickListDetailBo = bo.ReinsBasisCodePickListDetailBo;

                PolicyAmountFrom = bo.PolicyAmountFrom;
                PolicyAmountFromStr = Util.DoubleToString(bo.PolicyAmountFrom);
                PolicyAmountTo = bo.PolicyAmountTo;
                PolicyAmountToStr = Util.DoubleToString(bo.PolicyAmountTo);
                AttainedAgeFrom = bo.AttainedAgeFrom;
                AttainedAgeTo = bo.AttainedAgeTo;
                AarFrom = bo.AarFrom;
                AarFromStr = Util.DoubleToString(bo.AarFrom);
                AarTo = bo.AarTo;
                AarToStr = Util.DoubleToString(bo.AarTo);

                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                ReinsEffDatePolStartDateStr = bo.ReinsEffDatePolStartDate?.ToString(Util.GetDateFormat());
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                ReinsEffDatePolEndDateStr = bo.ReinsEffDatePolEndDate?.ToString(Util.GetDateFormat());

                // Phase 2
                PolicyTermFrom = bo.PolicyTermFrom;
                PolicyTermFromStr = Util.DoubleToString(bo.PolicyTermFrom);
                PolicyTermTo = bo.PolicyTermTo;
                PolicyTermToStr = Util.DoubleToString(bo.PolicyTermTo);
                PolicyDurationFrom = bo.PolicyDurationFrom;
                PolicyDurationFromStr = Util.DoubleToString(bo.PolicyDurationFrom);
                PolicyDurationTo = bo.PolicyDurationTo;
                PolicyDurationToStr = Util.DoubleToString(bo.PolicyDurationTo);
                RateId = bo.RateId;
                RateBo = bo.RateBo;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
                RiDiscountCode = bo.RiDiscountCode;
                LargeDiscountCode = bo.LargeDiscountCode;
                GroupDiscountCode = bo.GroupDiscountCode;

                ReportingStartDate = bo.ReportingStartDate;
                ReportingStartDateStr = bo.ReportingStartDate?.ToString(Util.GetDateFormat());
                ReportingEndDate = bo.ReportingEndDate;
                ReportingEndDateStr = bo.ReportingEndDate?.ToString(Util.GetDateFormat());
            }
        }

        public RateTableBo FormBo(int createdById, int updatedById)
        {
            var bo = new RateTableBo
            {
                TreatyCode = TreatyCode,
                CedingTreatyCode = CedingTreatyCode,
                BenefitId = BenefitId,
                BenefitBo = BenefitService.Find(BenefitId),
                CedingPlanCode = CedingPlanCode,
                CedingPlanCode2 = CedingPlanCode2,
                CedingBenefitTypeCode = CedingBenefitTypeCode,
                CedingBenefitRiskCode = CedingBenefitRiskCode,
                GroupPolicyNumber = GroupPolicyNumber,
                PremiumFrequencyCodePickListDetailId = PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCodePickListDetailBo = PickListDetailService.Find(PremiumFrequencyCodePickListDetailId),
                ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(ReinsBasisCodePickListDetailId),
                AttainedAgeFrom = AttainedAgeFrom,
                AttainedAgeTo = AttainedAgeTo,

                // Phase 2
                RateId = RateId,
                RateBo = RateService.Find(RateId),
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                RiDiscountCode = RiDiscountCode,
                LargeDiscountCode = LargeDiscountCode,
                GroupDiscountCode = GroupDiscountCode,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (ReinsEffDatePolStartDateStr != null)
            {
                bo.ReinsEffDatePolStartDate = DateTime.Parse(ReinsEffDatePolStartDateStr);
                if (ReinsEffDatePolEndDateStr != null)
                {
                    bo.ReinsEffDatePolEndDate = DateTime.Parse(ReinsEffDatePolEndDateStr);
                }
                else
                {
                    bo.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                }
            }

            if (!string.IsNullOrEmpty(ReportingStartDateStr))
            {
                bo.ReportingStartDate = DateTime.Parse(ReportingStartDateStr);
            }
            if (!string.IsNullOrEmpty(ReportingEndDateStr))
            {
                bo.ReportingEndDate = DateTime.Parse(ReportingEndDateStr);
            }
            else if (bo.ReportingStartDate != null)
            {
                bo.ReportingEndDate = DateTime.Parse(Util.GetDefaultEndDate());
            }

            double? d = Util.StringToDouble(PolicyAmountFromStr);
            bo.PolicyAmountFrom = d;

            d = Util.StringToDouble(PolicyAmountToStr);
            bo.PolicyAmountTo = d;

            d = Util.StringToDouble(AarFromStr);
            bo.AarFrom = d;

            d = Util.StringToDouble(AarToStr);
            bo.AarTo = d;

            d = Util.StringToDouble(PolicyDurationFromStr);
            bo.PolicyDurationFrom = d;

            d = Util.StringToDouble(PolicyDurationToStr);
            bo.PolicyDurationTo = d;

            d = Util.StringToDouble(PolicyTermFromStr);
            bo.PolicyTermFrom = d;

            d = Util.StringToDouble(PolicyTermToStr);
            bo.PolicyTermTo = d;

            return bo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDateStr);
            DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDateStr);

            DateTime? reportingStart = Util.GetParseDateTime(ReportingStartDateStr);
            DateTime? reportingEnd = Util.GetParseDateTime(ReportingEndDateStr);

            double? policyAmountFrom = Util.StringToDouble(PolicyAmountFromStr);
            double? policyAmountTo = Util.StringToDouble(PolicyAmountToStr);

            double? aarAmountFrom = Util.StringToDouble(AarFromStr);
            double? aarAmountTo = Util.StringToDouble(AarToStr);

            double? policyDurationFrom = Util.StringToDouble(PolicyDurationFromStr);
            double? policyDurationTo = Util.StringToDouble(PolicyDurationToStr);

            double? policyTermFrom = Util.StringToDouble(PolicyTermFromStr);
            double? policyTermTo = Util.StringToDouble(PolicyTermToStr);

            if (end != null && start == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Reinsurance Effective"),
                    new[] { nameof(ReinsEffDatePolStartDateStr) }
                ));
            }
            else if (start != null && end == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= start)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDefaultDate, "Reinsurance Effective", Util.GetDefaultEndDate()),
                        new[] { nameof(ReinsEffDatePolStartDateStr) }
                    ));
                }
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.StartDateEarlier, "Reinsurance Effective"),
                        new[] { nameof(ReinsEffDatePolStartDateStr) }
                    ));
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDateLater, "Reinsurance Effective"),
                        new[] { nameof(ReinsEffDatePolEndDateStr) }
                    ));
                }
            }

            if (reportingEnd != null && reportingStart == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Reporting Start Date Field"),
                    new[] { nameof(ReportingStartDateStr) }
                ));
            }
            else if (reportingStart != null && reportingEnd == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= reportingStart)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDefaultDate, "Reporting", Util.GetDefaultEndDate()),
                        new[] { nameof(ReportingStartDateStr) }
                    ));
                }
            }
            else if (reportingStart != null && reportingEnd != null)
            {
                if (reportingEnd <= reportingStart)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.StartDateEarlier, "Reporting"),
                        new[] { nameof(ReportingStartDateStr) }
                    ));
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDateLater, "Reporting"),
                        new[] { nameof(ReportingEndDateStr) }
                    ));
                }
            }

            if (policyAmountTo != null && policyAmountFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The ORI Sum Assured From Field"),
                    new[] { nameof(PolicyAmountFromStr) }
                ));
            }
            else if (policyAmountFrom != null && policyAmountTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The ORI Sum Assured To Field"),
                    new[] { nameof(PolicyAmountToStr) }
                ));
            }
            else if (policyAmountTo != null && policyAmountFrom != null && policyAmountTo < policyAmountFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "ORI Sum Assured From", "ORI Sum Assured To"),
                    new[] { nameof(PolicyAmountFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "ORI Sum Assured To", "ORI Sum Assured From"),
                    new[] { nameof(PolicyAmountToStr) }
                ));
            }

            if (AttainedAgeTo != null && AttainedAgeFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Attained Age From Field"),
                    new[] { nameof(AttainedAgeFrom) }
                ));
            }
            else if (AttainedAgeFrom != null && AttainedAgeTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Attained Age To Field"),
                    new[] { nameof(AttainedAgeTo) }
                ));
            }
            else if (AttainedAgeTo != null && AttainedAgeFrom != null && AttainedAgeTo < AttainedAgeFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Attained Age From", "Attained Age To"),
                    new[] { nameof(AttainedAgeFrom) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Attained Age To", "Attained Age From"),
                    new[] { nameof(AttainedAgeTo) }
                ));
            }

            if (aarAmountTo != null && aarAmountFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The AAR Amount From Field"),
                    new[] { nameof(AarFromStr) }
                ));
            }
            else if (aarAmountFrom != null && aarAmountTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The AAR Amount To Field"),
                    new[] { nameof(AarToStr) }
                ));
            }
            else if (aarAmountTo != null && aarAmountFrom != null && aarAmountTo < aarAmountFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "AAR From", "AAR To"),
                    new[] { nameof(AarFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "AAR To", "AAR From"),
                    new[] { nameof(AarToStr) }
                ));
            }

            if (policyTermTo != null && policyTermFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Policy Term From Field"),
                    new[] { nameof(PolicyTermFromStr) }
                ));
            }
            else if (policyTermFrom != null && policyTermTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Policy Term To Field"),
                    new[] { nameof(PolicyTermToStr) }
                ));
            }
            else if (policyTermTo != null && policyTermFrom != null && policyTermTo < policyTermFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Policy Term From", "Policy Term To"),
                    new[] { nameof(PolicyTermFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Policy Term To", "Policy Term From"),
                    new[] { nameof(PolicyTermToStr) }
                ));
            }

            if (policyDurationTo != null && policyDurationFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Policy Duration From Field"),
                    new[] { nameof(PolicyDurationFromStr) }
                ));
            }
            else if (policyDurationFrom != null && policyDurationTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Policy Duration To Field"),
                    new[] { nameof(PolicyDurationToStr) }
                ));
            }
            else if (policyDurationTo != null && policyDurationFrom != null && policyDurationTo < policyDurationFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Policy Duration From", "Policy Duration To"),
                    new[] { nameof(PolicyDurationFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Policy Duration To", "Policy Duration From"),
                    new[] { nameof(PolicyDurationToStr) }
                ));
            }

            //Treaty Code Validation
            int? cedantId = CedantId;
            if (cedantId.HasValue)
            {
                string[] treatyCodes = TreatyCode.ToArraySplitTrim();
                foreach (var treatyCode in treatyCodes)
                {
                    if (TreatyCodeService.CountByCedantIdCodeStatus(cedantId.Value, treatyCode, TreatyCodeBo.StatusActive) == 0)
                    {
                        results.Add(new ValidationResult(
                            string.Format("Please enter valid Treaty Code and the status is Active."),
                            new[] { nameof(TreatyCode) }
                        ));
                        break;
                    }
                }
            }

            if (!RateId.HasValue &&
                string.IsNullOrEmpty(RiDiscountCode) &&
                string.IsNullOrEmpty(LargeDiscountCode) &&
                string.IsNullOrEmpty(GroupDiscountCode)
            )
            {
                results.Add(new ValidationResult(string.Format("Please enter at least one mapped values")));
            }

            return results;
        }

        public static Expression<Func<RateTable, RateTableViewModel>> Expression()
        {
            return entity => new RateTableViewModel
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                BenefitId = entity.BenefitId,
                Benefit = entity.Benefit,
                CedingPlanCode = entity.CedingPlanCode,
                CedingPlanCode2 = entity.CedingPlanCode2,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                GroupPolicyNumber = entity.GroupPolicyNumber,

                PremiumFrequencyCodePickListDetailId = entity.PremiumFrequencyCodePickListDetailId,
                PremiumFrequencyCodePickListDetail = entity.PremiumFrequencyCodePickListDetail,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetail = entity.ReinsBasisCodePickListDetail,

                PolicyAmountFrom = entity.PolicyAmountFrom,
                PolicyAmountTo = entity.PolicyAmountTo,
                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,
                AarFrom = entity.AarFrom,
                AarTo = entity.AarTo,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                RateTableDetails = entity.RateTableDetails,

                // Phase 2
                PolicyTermFrom = entity.PolicyTermFrom,
                PolicyTermTo = entity.PolicyTermTo,
                PolicyDurationFrom = entity.PolicyDurationFrom,
                PolicyDurationTo = entity.PolicyDurationTo,
                RateId = entity.RateId,
                Rate = entity.Rate,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                RiDiscountCode = entity.RiDiscountCode,
                LargeDiscountCode = entity.LargeDiscountCode,
                GroupDiscountCode = entity.GroupDiscountCode,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,
            };
        }
    }
}