using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Retrocession;
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
    public class PerLifeRetroConfigurationRatioViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int TreatyCodeId { get; set; }

        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [DisplayName("Retro Ratio")]
        public double? RetroRatio { get; set; }

        [Required, DisplayName("Retro Ratio")]
        [ValidateDouble]
        public string RetroRatioStr { get; set; }

        [DisplayName("MLRe Retain Ratio")]
        public double? MlreRetainRatio { get; set; }

        [Required, DisplayName("MLRe Retain Ratio")]
        [ValidateDouble]
        public string MlreRetainRatioStr { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public DateTime? ReinsEffectiveStartDate { get; set; }

        [Required, DisplayName("Reinsurance Effective Start Date")]
        [ValidateDate]
        public string ReinsEffectiveStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public DateTime? ReinsEffectiveEndDate { get; set; }

        [Required, DisplayName("Reinsurance Effective End Date")]
        [ValidateDate]
        public string ReinsEffectiveEndDateStr { get; set; }

        [DisplayName("Risk Quarter Start Date")]
        public DateTime? RiskQuarterStartDate { get; set; }

        [Required, DisplayName("Risk Quarter Start Date")]
        [ValidateDate]
        public string RiskQuarterStartDateStr { get; set; }

        [DisplayName("Risk Quarter End Date")]
        public DateTime? RiskQuarterEndDate { get; set; }

        [Required, DisplayName("Risk Quarter End Date")]
        [ValidateDate]
        public string RiskQuarterEndDateStr { get; set; }

        [DisplayName("Rule Effective Date")]
        public DateTime? RuleEffectiveDate { get; set; }

        [Required, DisplayName("Rule Effective Date")]
        [ValidateDate]
        public string RuleEffectiveDateStr { get; set; }

        [DisplayName("Rule Cease Date")]
        public DateTime? RuleCeaseDate { get; set; }

        [Required, DisplayName("Rule Cease Date")]
        [ValidateDate]
        public string RuleCeaseDateStr { get; set; }

        [DisplayName("Rule Value")]
        public double? RuleValue { get; set; }

        [Required, DisplayName("Rule Value")]
        [ValidateDouble]
        public string RuleValueStr { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        public PerLifeRetroConfigurationRatioViewModel() { }

        public PerLifeRetroConfigurationRatioViewModel(PerLifeRetroConfigurationRatioBo perLifeRetroConfigurationRatioBo)
        {
            Set(perLifeRetroConfigurationRatioBo);
        }

        public void Set(PerLifeRetroConfigurationRatioBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                RetroRatio = bo.RetroRatio;
                RetroRatioStr = Util.DoubleToString(bo.RetroRatio);
                MlreRetainRatio = bo.MlreRetainRatio;
                MlreRetainRatioStr = Util.DoubleToString(bo.MlreRetainRatio);
                ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate;
                ReinsEffectiveStartDateStr = bo.ReinsEffectiveStartDate.ToString(Util.GetDateFormat());
                ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate;
                ReinsEffectiveEndDateStr = bo.ReinsEffectiveEndDate.ToString(Util.GetDateFormat());
                RiskQuarterStartDate = bo.RiskQuarterStartDate;
                RiskQuarterStartDateStr = bo.RiskQuarterStartDate.ToString(Util.GetDateFormat());
                RiskQuarterEndDate = bo.RiskQuarterEndDate;
                RiskQuarterEndDateStr = bo.RiskQuarterEndDate.ToString(Util.GetDateFormat());
                RuleEffectiveDate = bo.RuleEffectiveDate;
                RuleEffectiveDateStr = bo.RuleEffectiveDate.ToString(Util.GetDateFormat());
                RuleCeaseDate = bo.RuleCeaseDate;
                RuleCeaseDateStr = bo.RuleCeaseDate.ToString(Util.GetDateFormat());
                RuleValue = bo.RuleValue;
                RuleValueStr = Util.DoubleToString(bo.RuleValue);
                Description = bo.Description;
            }
        }

        public PerLifeRetroConfigurationRatioBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeRetroConfigurationRatioBo
            {
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),
                RetroRatio = double.Parse(RetroRatioStr),
                MlreRetainRatio = double.Parse(MlreRetainRatioStr),
                ReinsEffectiveStartDate = DateTime.Parse(ReinsEffectiveStartDateStr),
                ReinsEffectiveEndDate = DateTime.Parse(ReinsEffectiveEndDateStr),
                RiskQuarterStartDate = DateTime.Parse(RiskQuarterStartDateStr),
                RiskQuarterEndDate = DateTime.Parse(RiskQuarterEndDateStr),
                RuleEffectiveDate = DateTime.Parse(RuleEffectiveDateStr),
                RuleCeaseDate = DateTime.Parse(RuleCeaseDateStr),
                RuleValue = double.Parse(RuleValueStr),
                Description = Description,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeRetroConfigurationRatio, PerLifeRetroConfigurationRatioViewModel>> Expression()
        {
            return entity => new PerLifeRetroConfigurationRatioViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                RetroRatio = entity.RetroRatio,
                MlreRetainRatio = entity.MlreRetainRatio,
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                RuleEffectiveDate = entity.RuleEffectiveDate,
                RuleCeaseDate = entity.RuleCeaseDate,
                RuleValue = entity.RuleValue,
                Description = entity.Description,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? reinsEffectiveStartDate = Util.GetParseDateTime(ReinsEffectiveStartDateStr);
            DateTime? reinsEffectiveEndDate = Util.GetParseDateTime(ReinsEffectiveEndDateStr);

            DateTime? riskQuarterStartDate = Util.GetParseDateTime(RiskQuarterStartDateStr);
            DateTime? riskQuarterEndDate = Util.GetParseDateTime(RiskQuarterEndDateStr);

            DateTime? ruleEffectiveDate = Util.GetParseDateTime(RuleEffectiveDateStr);
            DateTime? ruleCeaseDate = Util.GetParseDateTime(RuleCeaseDateStr);

            if (reinsEffectiveStartDate != null && reinsEffectiveEndDate != null && reinsEffectiveEndDate <= reinsEffectiveStartDate)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Reinsurance Effective"),
                    new[] { nameof(ReinsEffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                string.Format(MessageBag.EndDateLater, "Reinsurance Effective"),
                new[] { nameof(ReinsEffectiveEndDateStr) }));
            }

            if (riskQuarterStartDate != null && riskQuarterEndDate != null && riskQuarterEndDate <= riskQuarterStartDate)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Risk Quarter"),
                    new[] { nameof(RiskQuarterStartDateStr) }));
                results.Add(new ValidationResult(
                string.Format(MessageBag.EndDateLater, "Risk Quarter"),
                new[] { nameof(RiskQuarterEndDateStr) }));
            }

            if (ruleEffectiveDate != null && ruleCeaseDate != null && ruleCeaseDate <= ruleEffectiveDate)
            {
                results.Add(new ValidationResult(
                    string.Format("The Rule Effective Date Field must be earlier than The Rule Cease Date Field"),
                    new[] { nameof(RuleEffectiveDateStr) }));
                results.Add(new ValidationResult(
                string.Format("The Rule Cease Date Field must be later than The Rule Effective Date Field"),
                new[] { nameof(RuleCeaseDateStr) }));
            }

            return results;
        }
    }
}