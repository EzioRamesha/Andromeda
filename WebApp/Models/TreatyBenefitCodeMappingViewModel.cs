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
    public class TreatyBenefitCodeMappingViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Ceding Company")]
        [ValidateCedantId]
        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [Required, DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Ceding Benefit Type Code")]
        [ValidateCedingBenefitTypeCode]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("Ceding Treaty Code")]
        public string CedingTreatyCode { get; set; }

        [DisplayName("Campaign Code")]
        public string CampaignCode { get; set; }


        [DisplayName("Policy Reinsurance Effective Start Date")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Required, DisplayName("Policy Reinsurance Effective Start Date")]
        [ValidateDate]
        public string ReinsEffDatePolStartDateStr { get; set; }

        [DisplayName("Policy Reinsurance Effective End Date")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [DisplayName("Policy Reinsurance Effective End Date")]
        [ValidateDate]
        public string ReinsEffDatePolEndDateStr { get; set; }


        [Required, DisplayName("Reinsurance Basis Code")]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetail ReinsBasisCodePickListDetail { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        [DisplayName("Insured Attained Age From")]
        public int? AttainedAgeFrom { get; set; }

        [DisplayName("Insured Attained Age To")]
        public int? AttainedAgeTo { get; set; }


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


        [Required, DisplayName("MLRe Benefit Code")]
        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

        public BenefitBo BenefitBo { get; set; }

        [Required, DisplayName("Treaty Code")]
        [ValidateTreatyCodeId]
        public int TreatyCodeId { get; set; }

        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public virtual ICollection<TreatyBenefitCodeMappingDetail> TreatyBenefitCodeMappingDetails { get; set; }

        // Phase 2
        //[StringLength(1), DisplayName("Profit Commission")]
        //public string ProfitComm { get; set; }

        [DisplayName("Profit Commission")]
        public int? ProfitCommPickListDetailId { get; set; }

        public PickListDetail ProfitCommPickListDetail { get; set; }

        public PickListDetailBo ProfitCommPickListDetailBo { get; set; }

        [DisplayName("Maximum Age at Expiry")]
        public int? MaxExpiryAge { get; set; }

        [DisplayName("Minimum Issue Age")]
        public int? MinIssueAge { get; set; }

        [DisplayName("Maximum Issue Age")]
        public int? MaxIssueAge { get; set; }

        [DisplayName("Maximum Underwriting Rating")]
        public double? MaxUwRating { get; set; }

        [DisplayName("Maximum Underwriting Rating")]
        [ValidateDouble]
        public string MaxUwRatingStr { get; set; }

        [DisplayName("AP Loading")]
        public double? ApLoading { get; set; }

        [DisplayName("AP Loading")]
        [ValidateDouble]
        public string ApLoadingStr { get; set; }

        [DisplayName("Minimum AAR")]
        public double? MinAar { get; set; }

        [DisplayName("Minimum AAR")]
        [ValidateDouble]
        public string MinAarStr { get; set; }

        [DisplayName("Maximum AAR")]
        public double? MaxAar { get; set; }

        [DisplayName("Maximum AAR")]
        [ValidateDouble]
        public string MaxAarStr { get; set; }

        [DisplayName("ABL Amount")]
        public double? AblAmount { get; set; }

        [DisplayName("ABL Amount")]
        [ValidateDouble]
        public string AblAmountStr { get; set; }

        [DisplayName("Retention Share")]
        public double? RetentionShare { get; set; }

        [DisplayName("Retention Share")]
        [ValidateDouble]
        public string RetentionShareStr { get; set; }

        [DisplayName("Retention Cap")]
        public double? RetentionCap { get; set; }

        [DisplayName("Retention Cap")]
        [ValidateDouble]
        public string RetentionCapStr { get; set; }

        [DisplayName("RI Share 1")]
        public double? RiShare { get; set; }

        [DisplayName("RI Share 1")]
        [ValidateDouble]
        public string RiShareStr { get; set; }

        [DisplayName("RI Share Cap 1")]
        public double? RiShareCap { get; set; }

        [DisplayName("RI Share Cap 1")]
        [ValidateDouble]
        public string RiShareCapStr { get; set; }

        [DisplayName("Service Fee")]
        public double? ServiceFee { get; set; }

        [DisplayName("Service Fee")]
        [ValidateDouble]
        public string ServiceFeeStr { get; set; }

        [DisplayName("Wakalah Fee")]
        public double? WakalahFee { get; set; }

        [DisplayName("Wakalah Fee")]
        [ValidateDouble]
        public string WakalahFeeStr { get; set; }
        
        [DisplayName("Underwriter Rating From")]
        public double? UnderwriterRatingFrom { get; set; }

        [DisplayName("Underwriter Rating From")]
        [ValidateDouble]
        public string UnderwriterRatingFromStr { get; set; }
        
        [DisplayName("Underwriter Rating To")]
        public double? UnderwriterRatingTo { get; set; }

        [DisplayName("Underwriter Rating To")]
        [ValidateDouble]
        public string UnderwriterRatingToStr { get; set; }

        [DisplayName("RI Share 2")]
        public double? RiShare2 { get; set; }

        [DisplayName("RI Share 2")]
        [ValidateDouble]
        public string RiShare2Str { get; set; }

        [DisplayName("RI Share Cap 2")]
        public double? RiShareCap2 { get; set; }

        [DisplayName("RI Share Cap 2")]
        [ValidateDouble]
        public string RiShareCap2Str { get; set; }

        [DisplayName("Ori Sum Assured From")]
        public double? OriSumAssuredFrom { get; set; }

        [DisplayName("Ori Sum Assured From")]
        [ValidateDouble]
        public string OriSumAssuredFromStr { get; set; }

        [DisplayName("Ori Sum Assured To")]
        public double? OriSumAssuredTo { get; set; }

        [DisplayName("Ori Sum Assured To")]
        [ValidateDouble]
        public string OriSumAssuredToStr { get; set; }

        [DisplayName("Treaty/Product Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Treaty/Product Effective Date")]
        public string EffectiveDateStr { get; set; }

        [DisplayName("Reinsurance Issue Age From")]
        public int? ReinsuranceIssueAgeFrom { get; set; }

        [DisplayName("Reinsurance Issue Age To")]
        public int? ReinsuranceIssueAgeTo { get; set; }

        public TreatyBenefitCodeMappingViewModel() { }

        public TreatyBenefitCodeMappingViewModel(TreatyBenefitCodeMappingBo treatyBenefitCodeMappingBo)
        {
            Set(treatyBenefitCodeMappingBo);
        }

        public void Set(TreatyBenefitCodeMappingBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;

                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;

                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                BenefitId = bo.BenefitId;
                BenefitBo = bo.BenefitBo;

                CedingPlanCode = bo.CedingPlanCode;
                Description = bo.Description;
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                CedingTreatyCode = bo.CedingTreatyCode;
                CampaignCode = bo.CampaignCode;

                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                ReinsEffDatePolStartDateStr = bo.ReinsEffDatePolStartDate?.ToString(Util.GetDateFormat());
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                ReinsEffDatePolEndDateStr = bo.ReinsEffDatePolEndDate?.ToString(Util.GetDateFormat());

                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                ReinsBasisCodePickListDetailBo = bo.ReinsBasisCodePickListDetailBo;

                AttainedAgeFrom = bo.AttainedAgeFrom;
                AttainedAgeTo = bo.AttainedAgeTo;

                ReportingStartDate = bo.ReportingStartDate;
                ReportingStartDateStr = bo.ReportingStartDate?.ToString(Util.GetDateFormat());
                ReportingEndDate = bo.ReportingEndDate;
                ReportingEndDateStr = bo.ReportingEndDate?.ToString(Util.GetDateFormat());

                // Phase 2
                ProfitCommPickListDetailId = bo.ProfitCommPickListDetailId;
                ProfitCommPickListDetailBo = bo.ProfitCommPickListDetailBo;
                MaxExpiryAge = bo.MaxExpiryAge;
                MinIssueAge = bo.MinIssueAge;
                MaxIssueAge = bo.MaxIssueAge;

                MaxUwRating = bo.MaxUwRating;
                MaxUwRatingStr = Util.DoubleToString(bo.MaxUwRating);

                ApLoading = bo.ApLoading;
                ApLoadingStr = Util.DoubleToString(bo.ApLoading);

                MinAar = bo.MinAar;
                MinAarStr = Util.DoubleToString(bo.MinAar);
                MaxAar = bo.MaxAar;
                MaxAarStr = Util.DoubleToString(bo.MaxAar);

                AblAmount = bo.AblAmount;
                AblAmountStr = Util.DoubleToString(bo.AblAmount);

                RetentionShare = bo.RetentionShare;
                RetentionShareStr = Util.DoubleToString(bo.RetentionShare);

                RetentionCap = bo.RetentionCap;
                RetentionCapStr = Util.DoubleToString(bo.RetentionCap);

                RiShare = bo.RiShare;
                RiShareStr = Util.DoubleToString(bo.RiShare);

                RiShareCap = bo.RiShareCap;
                RiShareCapStr = Util.DoubleToString(bo.RiShareCap);

                ServiceFee = bo.ServiceFee;
                ServiceFeeStr = Util.DoubleToString(bo.ServiceFee);

                WakalahFee = bo.WakalahFee;
                WakalahFeeStr = Util.DoubleToString(bo.WakalahFee);

                UnderwriterRatingFrom = bo.UnderwriterRatingFrom;
                UnderwriterRatingFromStr = Util.DoubleToString(bo.UnderwriterRatingFrom);

                UnderwriterRatingTo = bo.UnderwriterRatingTo;
                UnderwriterRatingToStr = Util.DoubleToString(bo.UnderwriterRatingTo);

                RiShare2 = bo.RiShare2;
                RiShare2Str = Util.DoubleToString(bo.RiShare2);

                RiShareCap2 = bo.RiShareCap2;
                RiShareCap2Str = Util.DoubleToString(bo.RiShareCap2);

                OriSumAssuredFrom = bo.OriSumAssuredFrom;
                OriSumAssuredFromStr = Util.DoubleToString(bo.OriSumAssuredFrom);

                OriSumAssuredTo = bo.OriSumAssuredTo;
                OriSumAssuredToStr = Util.DoubleToString(bo.OriSumAssuredTo);

                EffectiveDate = bo.EffectiveDate;
                EffectiveDateStr = bo.EffectiveDate?.ToString(Util.GetDateFormat());

                ReinsuranceIssueAgeFrom = bo.ReinsuranceIssueAgeFrom;
                ReinsuranceIssueAgeTo = bo.ReinsuranceIssueAgeTo;
            }
        }

        public TreatyBenefitCodeMappingBo FormBo(int createdById, int updatedById)
        {
            var bo = new TreatyBenefitCodeMappingBo
            {
                Id = Id,
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),

                BenefitId = BenefitId,
                BenefitBo = BenefitService.Find(BenefitId),

                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),

                CedingPlanCode = CedingPlanCode,
                Description = Description?.Trim(),
                CedingBenefitTypeCode = CedingBenefitTypeCode,
                CedingBenefitRiskCode = CedingBenefitRiskCode,
                CedingTreatyCode = CedingTreatyCode,
                CampaignCode = CampaignCode,

                ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(ReinsBasisCodePickListDetailId),

                AttainedAgeFrom = AttainedAgeFrom,
                AttainedAgeTo = AttainedAgeTo,

                // Phase 2
                //ProfitComm = ProfitComm, 
                ProfitCommPickListDetailId = ProfitCommPickListDetailId,
                ProfitCommPickListDetailBo = PickListDetailService.Find(ProfitCommPickListDetailId),
                MaxExpiryAge = MaxExpiryAge,
                MinIssueAge = MinIssueAge,
                MaxIssueAge = MaxIssueAge,

                ReinsuranceIssueAgeFrom = ReinsuranceIssueAgeFrom,
                ReinsuranceIssueAgeTo = ReinsuranceIssueAgeTo,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (!string.IsNullOrEmpty(ReinsEffDatePolStartDateStr))
            {
                bo.ReinsEffDatePolStartDate = DateTime.Parse(ReinsEffDatePolStartDateStr);
            }
            if (!string.IsNullOrEmpty(ReinsEffDatePolEndDateStr))
            {
                bo.ReinsEffDatePolEndDate = DateTime.Parse(ReinsEffDatePolEndDateStr);
            }
            else
            {
                bo.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
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

            // Phase 2
            bo.MaxUwRating = Util.StringToDouble(MaxUwRatingStr);
            bo.ApLoading = Util.StringToDouble(ApLoadingStr);
            bo.MinAar = Util.StringToDouble(MinAarStr);
            bo.MaxAar = Util.StringToDouble(MaxAarStr);
            bo.AblAmount = Util.StringToDouble(AblAmountStr);
            bo.RetentionShare = Util.StringToDouble(RetentionShareStr);
            bo.RetentionCap = Util.StringToDouble(RetentionCapStr);
            bo.RiShare = Util.StringToDouble(RiShareStr);
            bo.RiShareCap = Util.StringToDouble(RiShareCapStr);
            bo.ServiceFee = Util.StringToDouble(ServiceFeeStr);
            bo.WakalahFee = Util.StringToDouble(WakalahFeeStr);
            bo.UnderwriterRatingFrom = Util.StringToDouble(UnderwriterRatingFromStr);
            bo.UnderwriterRatingTo = Util.StringToDouble(UnderwriterRatingToStr);
            bo.RiShare2 = Util.StringToDouble(RiShare2Str);
            bo.RiShareCap2 = Util.StringToDouble(RiShareCap2Str);

            bo.OriSumAssuredFrom = Util.StringToDouble(OriSumAssuredFromStr);
            bo.OriSumAssuredTo = Util.StringToDouble(OriSumAssuredToStr);

            bo.EffectiveDate = !string.IsNullOrEmpty(EffectiveDateStr) ? Util.GetParseDateTime(EffectiveDateStr) : null;

            return bo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? policyStart = Util.GetParseDateTime(ReinsEffDatePolStartDateStr);
            DateTime? policyEnd = Util.GetParseDateTime(ReinsEffDatePolEndDateStr);

            DateTime? reportingStart = Util.GetParseDateTime(ReportingStartDateStr);
            DateTime? reportingEnd = Util.GetParseDateTime(ReportingEndDateStr);

            double? minAar = Util.StringToDouble(MinAarStr);
            double? maxAar = Util.StringToDouble(MaxAarStr);

            double? uwRatingFrom = Util.StringToDouble(UnderwriterRatingFromStr);
            double? uwRatingTo = Util.StringToDouble(UnderwriterRatingToStr);

            double? oriSumAssuredFrom = Util.StringToDouble(OriSumAssuredFromStr);
            double? oriSumAssuredTo = Util.StringToDouble(OriSumAssuredToStr);

            if (policyStart != null  && policyEnd == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= policyStart)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDefaultDate, "Policy Reinsurance Effective", Util.GetDefaultEndDate()),
                        new[] { nameof(ReinsEffDatePolStartDateStr) }
                    ));
                }
            }
            else if (policyStart != null && policyEnd != null)
            {
                if (policyEnd <= policyStart)
                {
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.StartDateEarlier, "Policy Reinsurance Effective"),
                        new[] { nameof(ReinsEffDatePolStartDateStr) }
                    ));
                    results.Add(new ValidationResult(
                        string.Format(MessageBag.EndDateLater, "Policy Reinsurance Effective"),
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

            // Remove Min & Max Issue Age Required
            //if (MaxIssueAge != null && MinIssueAge == null)
            //{
            //    results.Add(new ValidationResult(
            //        string.Format(MessageBag.Required, "The Minimum Issue Age Field"),
            //        new[] { nameof(MinIssueAge) }
            //    ));
            //}
            //else if (MinIssueAge != null && MaxIssueAge == null)
            //{
            //    results.Add(new ValidationResult(
            //        string.Format(MessageBag.Required, "The Maximum Issue Age Field"),
            //        new[] { nameof(MaxIssueAge) }
            //    ));
            //}
            if (MaxIssueAge != null && MinIssueAge != null && MaxIssueAge < MinIssueAge)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Minimum Issue Age", "Maximum Issue Age"),
                    new[] { nameof(MinIssueAge) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Maximum Issue Age", "Minimum Issue Age"),
                    new[] { nameof(MaxIssueAge) }
                ));
            }

            // Remove Min & Max Aar Required
            //if (maxAar != null && minAar == null)
            //{
            //    results.Add(new ValidationResult(
            //        string.Format(MessageBag.Required, "The Minimum AAR Field"),
            //        new[] { nameof(MinAarStr) }
            //    ));
            //}
            //else if (minAar != null && maxAar == null)
            //{
            //    results.Add(new ValidationResult(
            //        string.Format(MessageBag.Required, "The Maximum AAR Field"),
            //        new[] { nameof(MaxAarStr) }
            //    ));
            //}
            if (maxAar != null && minAar != null && maxAar < minAar)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Minimum AAR", "Maximum AAR"),
                    new[] { nameof(MinAarStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Maximum AAR", "Minimum AAR"),
                    new[] { nameof(MaxAarStr) }
                ));
            }

            if (uwRatingTo != null && uwRatingFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Underwriter Rating From Field"),
                    new[] { nameof(UnderwriterRatingFromStr) }
                ));
            }
            else if (uwRatingFrom != null && uwRatingTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Underwriter Rating To Field"),
                    new[] { nameof(UnderwriterRatingToStr) }
                ));
            }
            else if (uwRatingTo != null && uwRatingFrom != null && uwRatingTo < uwRatingFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Underwriter Rating From", "Underwriter Rating To"),
                    new[] { nameof(UnderwriterRatingFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Underwriter Rating To", "Underwriter Rating From"),
                    new[] { nameof(UnderwriterRatingToStr) }
                ));
            }

            if (oriSumAssuredTo != null && oriSumAssuredFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Ori Sum Assured From Field"),
                    new[] { nameof(OriSumAssuredFromStr) }
                ));
            }
            else if (oriSumAssuredFrom != null && oriSumAssuredTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Ori Sum Assured To Field"),
                    new[] { nameof(OriSumAssuredToStr) }
                ));
            }
            else if (oriSumAssuredTo != null && oriSumAssuredFrom != null && oriSumAssuredTo < oriSumAssuredFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Ori Sum Assured From", "Ori Sum Assured To"),
                    new[] { nameof(OriSumAssuredFromStr) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Ori Sum Assured To", "Ori Sum Assured From"),
                    new[] { nameof(OriSumAssuredToStr) }
                ));
            }

            if (ReinsuranceIssueAgeTo != null && ReinsuranceIssueAgeFrom == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Reinsurance Issue Age From Field"),
                    new[] { nameof(ReinsuranceIssueAgeFrom) }
                ));
            }
            else if (ReinsuranceIssueAgeFrom != null && ReinsuranceIssueAgeTo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Reinsurance Issue Age To Field"),
                    new[] { nameof(ReinsuranceIssueAgeTo) }
                ));
            }
            else if (ReinsuranceIssueAgeTo != null && ReinsuranceIssueAgeFrom != null && ReinsuranceIssueAgeTo < ReinsuranceIssueAgeFrom)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.LowerOrEqualThan, "Reinsurance Issue Age From", "Reinsurance Issue Age To"),
                    new[] { nameof(ReinsuranceIssueAgeFrom) }
                ));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterOrEqualTo, "Reinsurance Issue Age To", "Reinsurance Issue Age From"),
                    new[] { nameof(ReinsuranceIssueAgeTo) }
                ));
            }

            return results;
        }

        public static Expression<Func<TreatyBenefitCodeMapping, TreatyBenefitCodeMappingViewModel>> Expression()
        {
            return entity => new TreatyBenefitCodeMappingViewModel
            {
                Id = entity.Id,

                CedantId = entity.CedantId,
                Cedant = entity.Cedant,

                BenefitId = entity.BenefitId,
                Benefit = entity.Benefit,

                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,

                CedingPlanCode = entity.CedingPlanCode,
                Description = entity.Description,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingTreatyCode = entity.CedingTreatyCode,
                CampaignCode = entity.CampaignCode,

                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,

                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetail = entity.ReinsBasisCodePickListDetail,

                AttainedAgeFrom = entity.AttainedAgeFrom,
                AttainedAgeTo = entity.AttainedAgeTo,

                ReportingStartDate = entity.ReportingStartDate,
                ReportingEndDate = entity.ReportingEndDate,

                TreatyBenefitCodeMappingDetails = entity.TreatyBenefitCodeMappingDetails,

                // Phase 2
                //ProfitComm = entity.ProfitComm,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitCommPickListDetail = entity.ProfitCommPickListDetail,
                MaxExpiryAge = entity.MaxExpiryAge,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MaxUwRating = entity.MaxUwRating,
                ApLoading = entity.ApLoading,
                MinAar = entity.MinAar,
                MaxAar = entity.MaxAar,
                AblAmount = entity.AblAmount,
                RetentionShare = entity.RetentionShare,
                RetentionCap = entity.RetentionCap,
                RiShare = entity.RiShare,
                RiShareCap = entity.RiShareCap,
                ServiceFee = entity.ServiceFee,
                WakalahFee = entity.WakalahFee,
                UnderwriterRatingFrom = entity.UnderwriterRatingFrom,
                UnderwriterRatingTo = entity.UnderwriterRatingTo,
                RiShare2 = entity.RiShare2,
                RiShareCap2 = entity.RiShareCap2,

                OriSumAssuredFrom = entity.OriSumAssuredFrom,
                OriSumAssuredTo = entity.OriSumAssuredTo,

                EffectiveDate = entity.EffectiveDate,

                ReinsuranceIssueAgeFrom = entity.ReinsuranceIssueAgeFrom,
                ReinsuranceIssueAgeTo = entity.ReinsuranceIssueAgeTo,
            };
        }
    }

    public class ValidateCedantId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int cedantId = int.Parse(value.ToString());
                if (cedantId == 0)
                {
                    return new ValidationResult("The Ceding Company field is required.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateTreatyCodeId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int treatyCodeId = int.Parse(value.ToString());
                if (treatyCodeId == 0)
                {
                    return new ValidationResult("The Treaty Code field is required.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ValidateCedingBenefitTypeCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string cedingBenefitTypeCode = value.ToString();
                string[] resultsArray = cedingBenefitTypeCode.ToArraySplitTrim();
                IList<PickListDetailBo> pickListDetailBos = PickListDetailService.GetByPickListId(PickListBo.CedingBenefitTypeCode);
                List<string> cedingBenefitTypeCodes = new List<string>();
                if (pickListDetailBos != null)
                {
                    foreach (PickListDetailBo pickListDetailBo in pickListDetailBos)
                    {
                        cedingBenefitTypeCodes.Add(pickListDetailBo.Code);
                    }
                }
                foreach (string result in resultsArray)
                {
                    if (!cedingBenefitTypeCodes.Contains(result))
                    {
                        return new ValidationResult("Please Enter Valid Ceding Benefit Type Code.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}