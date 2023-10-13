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
    public class PerLifeRetroConfigurationTreatyViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int TreatyCodeId { get; set; }

        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [Required, DisplayName("Treaty Type")]
        public int TreatyTypePickListDetailId { get; set; }

        public PickListDetail TreatyTypePickListDetail { get; set; }

        public PickListDetailBo TreatyTypePickListDetailBo { get; set; }

        [Required, DisplayName("Funds Accounting Type")]
        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetail FundsAccountingTypePickListDetail { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

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

        [Required, DisplayName("To Aggregate")]
        public bool IsToAggregate { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        public PerLifeRetroConfigurationTreatyViewModel() { }

        public PerLifeRetroConfigurationTreatyViewModel(PerLifeRetroConfigurationTreatyBo perLifeRetroConfigurationTreatyBo)
        {
            Set(perLifeRetroConfigurationTreatyBo);
        }

        public void Set(PerLifeRetroConfigurationTreatyBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId;
                TreatyTypePickListDetailBo = bo.TreatyTypePickListDetailBo;
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                FundsAccountingTypePickListDetailBo = bo.FundsAccountingTypePickListDetailBo;
                ReinsEffectiveStartDate = bo.ReinsEffectiveStartDate;
                ReinsEffectiveStartDateStr = bo.ReinsEffectiveStartDate.ToString(Util.GetDateFormat());
                ReinsEffectiveEndDate = bo.ReinsEffectiveEndDate;
                ReinsEffectiveEndDateStr = bo.ReinsEffectiveEndDate.ToString(Util.GetDateFormat());
                RiskQuarterStartDate = bo.RiskQuarterStartDate;
                RiskQuarterStartDateStr = bo.RiskQuarterStartDate.ToString(Util.GetDateFormat());
                RiskQuarterEndDate = bo.RiskQuarterEndDate;
                RiskQuarterEndDateStr = bo.RiskQuarterEndDate.ToString(Util.GetDateFormat());
                IsToAggregate = bo.IsToAggregate;
                Remark = bo.Remark;
            }
        }

        public PerLifeRetroConfigurationTreatyBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeRetroConfigurationTreatyBo
            {
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),
                TreatyTypePickListDetailId = TreatyTypePickListDetailId,
                TreatyTypePickListDetailBo = PickListDetailService.Find(TreatyTypePickListDetailId),
                FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(FundsAccountingTypePickListDetailId),
                ReinsEffectiveStartDate = DateTime.Parse(ReinsEffectiveStartDateStr),
                ReinsEffectiveEndDate = DateTime.Parse(ReinsEffectiveEndDateStr),
                RiskQuarterStartDate = DateTime.Parse(RiskQuarterStartDateStr),
                RiskQuarterEndDate = DateTime.Parse(RiskQuarterEndDateStr),
                IsToAggregate = IsToAggregate,
                Remark = Remark,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeRetroConfigurationTreaty, PerLifeRetroConfigurationTreatyViewModel>> Expression()
        {
            return entity => new PerLifeRetroConfigurationTreatyViewModel
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyTypePickListDetail = entity.TreatyTypePickListDetail,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetail = entity.FundsAccountingTypePickListDetail,
                ReinsEffectiveStartDate = entity.ReinsEffectiveStartDate,
                ReinsEffectiveEndDate = entity.ReinsEffectiveEndDate,
                RiskQuarterStartDate = entity.RiskQuarterStartDate,
                RiskQuarterEndDate = entity.RiskQuarterEndDate,
                IsToAggregate = entity.IsToAggregate,
                Remark = entity.Remark,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? reinsEffectiveStartDate = Util.GetParseDateTime(ReinsEffectiveStartDateStr);
            DateTime? reinsEffectiveEndDate = Util.GetParseDateTime(ReinsEffectiveEndDateStr);

            DateTime? riskQuarterStartDate = Util.GetParseDateTime(RiskQuarterStartDateStr);
            DateTime? riskQuarterEndDate = Util.GetParseDateTime(RiskQuarterEndDateStr);

            if (reinsEffectiveStartDate != null && reinsEffectiveEndDate != null)
            {
                if (reinsEffectiveEndDate <= reinsEffectiveStartDate)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Reinsurance Effective"),
                    new[] { nameof(ReinsEffectiveStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Reinsurance Effective"),
                    new[] { nameof(ReinsEffectiveEndDateStr) }));
                }
            }

            if (riskQuarterStartDate != null && riskQuarterEndDate != null)
            {
                if (riskQuarterEndDate <= riskQuarterStartDate)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Risk Quarter"),
                    new[] { nameof(RiskQuarterStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Risk Quarter"),
                    new[] { nameof(RiskQuarterEndDateStr) }));
                }
            }

            return results;
        }
    }
}