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
    public class PerLifeRetroCountryViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Country")]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        public PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        [StringLength(50), DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Effective Start Date")]
        [ValidateDate]
        public DateTime? EffectiveStartDate { get; set; }

        [Required, DisplayName("Effective Start Date")]
        public string EffectiveStartDateStr { get; set; }

        [DisplayName("Effective End Date")]
        [ValidateDate]
        public DateTime? EffectiveEndDate { get; set; }

        [Required, DisplayName("Effective End Date")]
        public string EffectiveEndDateStr { get; set; }

        public PerLifeRetroCountryViewModel() { }

        public PerLifeRetroCountryViewModel(PerLifeRetroCountryBo perLifeRetroCountryBo)
        {
            Set(perLifeRetroCountryBo);
        }

        public void Set(PerLifeRetroCountryBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
                TerritoryOfIssueCodePickListDetailBo = bo.TerritoryOfIssueCodePickListDetailBo;
                Country = bo.Country;
                EffectiveStartDate = bo.EffectiveStartDate;
                EffectiveStartDateStr = bo.EffectiveStartDate.ToString(Util.GetDateFormat());
                EffectiveEndDate = bo.EffectiveEndDate;
                EffectiveEndDateStr = bo.EffectiveEndDate.ToString(Util.GetDateFormat());
            }
        }

        public PerLifeRetroCountryBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeRetroCountryBo
            {
                TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = PickListDetailService.Find(TerritoryOfIssueCodePickListDetailId),
                Country = Country,
                EffectiveStartDate = DateTime.Parse(EffectiveStartDateStr),
                EffectiveEndDate = DateTime.Parse(EffectiveEndDateStr),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeRetroCountry, PerLifeRetroCountryViewModel>> Expression()
        {
            return entity => new PerLifeRetroCountryViewModel
            {
                Id = entity.Id,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetail = entity.TerritoryOfIssueCodePickListDetail,
                Country = entity.Country,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(EffectiveStartDateStr);
            DateTime? end = Util.GetParseDateTime(EffectiveEndDateStr);

            if (start != null && end != null && end <= start)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Effective"),
                    new[] { nameof(EffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                string.Format(MessageBag.EndDateLater, "Effective"),
                new[] { nameof(EffectiveEndDateStr) }));
            }

            return results;
        }
    }
}