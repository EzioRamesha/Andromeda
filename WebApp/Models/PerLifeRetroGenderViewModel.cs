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
    public class PerLifeRetroGenderViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, DisplayName("Gender")]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetail InsuredGenderCodePickListDetail { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        [StringLength(15), DisplayName("Gender")]
        public string Gender { get; set; }

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

        public PerLifeRetroGenderViewModel() { }

        public PerLifeRetroGenderViewModel(PerLifeRetroGenderBo perLifeRetroGenderBo)
        {
            Set(perLifeRetroGenderBo);
        }

        public void Set(PerLifeRetroGenderBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                InsuredGenderCodePickListDetailBo = bo.InsuredGenderCodePickListDetailBo;
                Gender = bo.Gender;
                EffectiveStartDate = bo.EffectiveStartDate;
                EffectiveStartDateStr = bo.EffectiveStartDate.ToString(Util.GetDateFormat());
                EffectiveEndDate = bo.EffectiveEndDate;
                EffectiveEndDateStr = bo.EffectiveEndDate.ToString(Util.GetDateFormat());
            }
        }

        public PerLifeRetroGenderBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeRetroGenderBo
            {
                InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(InsuredGenderCodePickListDetailId),
                Gender = Gender,
                EffectiveStartDate = DateTime.Parse(EffectiveStartDateStr),
                EffectiveEndDate = DateTime.Parse(EffectiveEndDateStr),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeRetroGender, PerLifeRetroGenderViewModel>> Expression()
        {
            return entity => new PerLifeRetroGenderViewModel
            {
                Id = entity.Id,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetail = entity.InsuredGenderCodePickListDetail,
                Gender = entity.Gender,
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