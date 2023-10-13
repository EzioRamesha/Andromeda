using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class GstMaintenanceViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Effective Start Date")]
        public DateTime? EffectiveStartDate { get; set; }
        [Required, DisplayName("Effective Start Date")]
        public string EffectiveStartDateStr { get; set; }

        [DisplayName("Effective End Date")]
        public DateTime? EffectiveEndDate { get; set; }
        [Required, DisplayName("Effective End Date")]
        public string EffectiveEndDateStr { get; set; }

        [DisplayName("Risk Effective Start Date")]
        public DateTime? RiskEffectiveStartDate { get; set; }
        [Required, DisplayName("Risk Effective Start Date")]
        public string RiskEffectiveStartDateStr { get; set; }

        [DisplayName("Risk Effective End Date")]
        public DateTime? RiskEffectiveEndDate { get; set; }
        [Required, DisplayName("Risk Effective End Date")]
        public string RiskEffectiveEndDateStr { get; set; }

        [Required, DisplayName("Rate (%)")]
        public double Rate { get; set; }

        public GstMaintenanceViewModel() { }

        public GstMaintenanceViewModel(GstMaintenanceBo gstMaintenanceBo)
        {
            Set(gstMaintenanceBo);
        }

        public void Set(GstMaintenanceBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                EffectiveStartDate = bo.EffectiveStartDate;
                EffectiveStartDateStr = bo.EffectiveStartDate?.ToString(Util.GetDateFormat());
                EffectiveEndDate = bo.EffectiveEndDate;
                EffectiveEndDateStr = bo.EffectiveEndDate?.ToString(Util.GetDateFormat());
                RiskEffectiveStartDate = bo.RiskEffectiveStartDate;
                RiskEffectiveStartDateStr = bo.RiskEffectiveStartDate?.ToString(Util.GetDateFormat());
                RiskEffectiveEndDate = bo.RiskEffectiveEndDate;
                RiskEffectiveEndDateStr = bo.RiskEffectiveEndDate?.ToString(Util.GetDateFormat());
                Rate = bo.Rate;
            }
        }

        public GstMaintenanceBo FormBo(int createdById, int updatedById)
        {
            return new GstMaintenanceBo
            {
                EffectiveStartDate = Util.GetParseDateTime(EffectiveStartDateStr),
                EffectiveEndDate = Util.GetParseDateTime(EffectiveEndDateStr),
                RiskEffectiveStartDate = Util.GetParseDateTime(RiskEffectiveStartDateStr),
                RiskEffectiveEndDate = Util.GetParseDateTime(RiskEffectiveEndDateStr),
                Rate = Rate,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<GstMaintenance, GstMaintenanceViewModel>> Expression()
        {
            return entity => new GstMaintenanceViewModel
            {
                Id = entity.Id,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                RiskEffectiveStartDate = entity.RiskEffectiveStartDate,
                RiskEffectiveEndDate = entity.RiskEffectiveEndDate,
                Rate = entity.Rate
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? effStart = Util.GetParseDateTime(EffectiveStartDateStr);
            DateTime? effEnd = Util.GetParseDateTime(EffectiveEndDateStr);
            DateTime? riskStart = Util.GetParseDateTime(RiskEffectiveStartDateStr);
            DateTime? riskEnd = Util.GetParseDateTime(RiskEffectiveEndDateStr);

            if (effStart != null && effEnd != null && effEnd <= effStart)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Effective"),
                    new[] { nameof(EffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Effective"),
                    new[] { nameof(EffectiveEndDateStr) }));
            }

            if (riskStart != null && riskEnd != null && riskEnd <= riskStart)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Risk Effective"),
                    new[] { nameof(RiskEffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Risk Effective"),
                    new[] { nameof(RiskEffectiveEndDateStr) }));
            }

            return results;
        }
    }
}