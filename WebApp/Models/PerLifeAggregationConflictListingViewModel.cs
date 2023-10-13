using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeAggregationConflictListingViewModel
    {
        public int Id { get; set; }

        [DisplayName("Treaty Code")]
        public int? TreatyCodeId { get; set; }
        public TreatyCode TreatyCode { get; set; }
        public string TreatyCodeName { get; set; }

        [Required, DisplayName("Treaty Code")]
        public string TreatyCodeStr { get; set; }

        [Required, DisplayName("Risk Year")]
        public int? RiskYear { get; set; }

        [Required, DisplayName("Risk Month")]
        public int? RiskMonth { get; set; }

        [Required, DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [Required, DisplayName("Insured Gender Code")]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        [DisplayName("Insured Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredDateOfBirthStr { get; set; }

        [Required, DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [DisplayName("Reins Effective Date Pol")]
        public DateTime? ReinsEffectiveDatePol { get; set; }
        public string ReinsEffectiveDatePolStr { get; set; }

        [DisplayName("AAR")]
        public double? AAR { get; set; }
        public string AARStr { get; set; }

        [DisplayName("Gross Premium")]
        public double? GrossPremium { get; set; }
        public string GrossPremiumStr { get; set; }

        [DisplayName("Net Premium")]
        public double? NetPremium { get; set; }
        public string NetPremiumStr { get; set; }

        [DisplayName("Premium Frequency Mode")]
        public int? PremiumFrequencyModePickListDetailId { get; set; }
        public PickListDetailBo PremiumFrequencyModePickListDetailBo { get; set; }

        [DisplayName("Retro Premium Frequency Mode")]
        public int? RetroPremiumFrequencyModePickListDetailId { get; set; }
        public PickListDetailBo RetroPremiumFrequencyModePickListDetailBo { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedantPlanCode { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public int? MLReBenefitCodeId { get; set; }
        [Required, DisplayName("MLRe Benefit Code")]
        public string MLReBenefitCodeStr { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        [DisplayName("Territory of Issue Code")]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }
        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public PerLifeAggregationConflictListingViewModel()
        {
            Set();
        }

        public PerLifeAggregationConflictListingViewModel(PerLifeAggregationConflictListingBo PerLifeAggregationConflictListingBo)
        {
            Set(PerLifeAggregationConflictListingBo);
        }

        public void Set(PerLifeAggregationConflictListingBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeStr = bo.TreatyCodeBo?.Code;
                RiskYear = bo.RiskYear;
                RiskMonth = bo.RiskMonth;
                CedantPlanCode = bo.CedantPlanCode;
                InsuredName = bo.InsuredName;
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = bo.InsuredGenderCodePickListDetailBo;
                InsuredDateOfBirth = bo.InsuredDateOfBirth;
                InsuredDateOfBirthStr = bo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                PolicyNumber = bo.PolicyNumber;
                ReinsEffectiveDatePol = bo.ReinsEffectiveDatePol;
                ReinsEffectiveDatePolStr = bo.ReinsEffectiveDatePol?.ToString(Util.GetDateFormat());
                AAR = bo.AAR;
                AARStr = Util.DoubleToString(bo.AAR);
                GrossPremium = bo.GrossPremium;
                GrossPremiumStr = Util.DoubleToString(bo.GrossPremium);
                NetPremium = bo.NetPremium;
                NetPremiumStr = Util.DoubleToString(bo.NetPremium);
                PremiumFrequencyModePickListDetailId = bo.PremiumFrequencyModePickListDetailId;
                PremiumFrequencyModePickListDetailBo = bo.PremiumFrequencyModePickListDetailBo;
                RetroPremiumFrequencyModePickListDetailId = bo.RetroPremiumFrequencyModePickListDetailId;
                RetroPremiumFrequencyModePickListDetailBo = bo.RetroPremiumFrequencyModePickListDetailBo;
                CedantPlanCode = bo.CedantPlanCode;
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                MLReBenefitCodeId = bo.MLReBenefitCodeId;
                MLReBenefitCodeBo = bo.MLReBenefitCodeBo;
                MLReBenefitCodeStr = bo.MLReBenefitCodeBo?.Code;
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
                TerritoryOfIssueCodePickListDetailBo = bo.TerritoryOfIssueCodePickListDetailBo;

                CreatedAt = bo.CreatedAt;
                CreatedAtStr = bo.CreatedAt.ToString(Util.GetDateFormat());
            }
        }

        public PerLifeAggregationConflictListingBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeAggregationConflictListingBo
            {
                Id = Id,
                TreatyCodeId = TreatyCodeId,
                TreatyCodeStr = TreatyCodeStr,
                RiskYear = RiskYear,
                RiskMonth = RiskMonth,
                InsuredName = InsuredName,
                InsuredDateOfBirth = InsuredDateOfBirthStr is null || InsuredDateOfBirthStr == "" ? null : Util.GetParseDateTime(InsuredDateOfBirthStr),
                InsuredDateOfBirthStr = InsuredDateOfBirthStr,
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                PolicyNumber = PolicyNumber,
                ReinsEffectiveDatePol = ReinsEffectiveDatePol,
                ReinsEffectiveDatePolStr = ReinsEffectiveDatePol.HasValue ? ReinsEffectiveDatePol.Value.ToString(Util.GetDateFormat()) : "",
                AAR = Util.StringToDouble(AARStr),
                AARStr = AARStr,
                GrossPremium = Util.StringToDouble(GrossPremiumStr),
                GrossPremiumStr = GrossPremiumStr,
                NetPremium = Util.StringToDouble(NetPremiumStr),
                NetPremiumStr = NetPremiumStr,
                PremiumFrequencyModePickListDetailId = PremiumFrequencyModePickListDetailId,
                RetroPremiumFrequencyModePickListDetailId = InsuredGenderCodePickListDetailId,
                CedantPlanCode = CedantPlanCode,
                CedingBenefitTypeCode = CedingBenefitTypeCode,
                CedingBenefitRiskCode = CedingBenefitRiskCode,
                MLReBenefitCodeId = MLReBenefitCodeId,
                MLReBenefitCodeStr = MLReBenefitCodeStr,
                TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeAggregationConflictListing, PerLifeAggregationConflictListingViewModel>> Expression()
        {
            return entity => new PerLifeAggregationConflictListingViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                RiskYear = entity.RiskYear,
                RiskMonth = entity.RiskMonth,
                InsuredName = entity.InsuredName,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                ReinsEffectiveDatePol = entity.ReinsEffectiveDatePol,
                AAR = entity.AAR,
                GrossPremium = entity.GrossPremium,
                NetPremium = entity.NetPremium,
                PremiumFrequencyModePickListDetailId = entity.PremiumFrequencyModePickListDetailId,
                RetroPremiumFrequencyModePickListDetailId = entity.RetroPremiumFrequencyModePickListDetailId,
                CedantPlanCode = entity.CedantPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            var treatyCodeBo = TreatyCodeService.FindByCode(TreatyCodeStr);

            if (treatyCodeBo == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.NotExistsWithValue, "Treaty Code"),
                    new[] { nameof(TreatyCodeStr) }));
            }

            return results;
        }
    }
}