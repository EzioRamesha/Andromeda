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
    public class RetroBenefitCodeViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, StringLength(30), DisplayName("Retro Benefit")]
        public string Code { get; set; }

        [Required, StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Effective Date")]
        [ValidateDate]
        public DateTime? EffectiveDate { get; set; }

        [Required, DisplayName("Effective Date")]
        [ValidateDate]
        public string EffectiveDateStr { get; set; }

        [DisplayName("Cease Date")]
        public DateTime? CeaseDate { get; set; }

        [Required, DisplayName("Cease Date")]
        [ValidateDate]
        public string CeaseDateStr { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        [StringLength(255), DisplayName("Remarks")]
        public string Remarks { get; set; }

        public RetroBenefitCodeViewModel() { }

        public RetroBenefitCodeViewModel(RetroBenefitCodeBo retroBenefitCodeBo)
        {
            Set(retroBenefitCodeBo);
        }

        public void Set(RetroBenefitCodeBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Code = bo.Code;
                Description = bo.Description;
                EffectiveDate = bo.EffectiveDate;
                EffectiveDateStr = bo.EffectiveDate.ToString(Util.GetDateFormat());
                CeaseDate = bo.CeaseDate;
                CeaseDateStr = bo.CeaseDate.ToString(Util.GetDateFormat());
                Status = bo.Status;
                Remarks = bo.Remarks;
            }
        }

        public RetroBenefitCodeBo FormBo(int createdById, int updatedById)
        {
            var bo = new RetroBenefitCodeBo
            {
                Id = Id,
                Code = Code,
                Description = Description,
                Status = Status,
                Remarks = Remarks,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (!string.IsNullOrEmpty(EffectiveDateStr))
            {
                bo.EffectiveDate = DateTime.Parse(EffectiveDateStr);
            }

            if (!string.IsNullOrEmpty(CeaseDateStr))
            {
                bo.CeaseDate = DateTime.Parse(CeaseDateStr);
            }

            return bo;
        }

        public static Expression<Func<RetroBenefitCode, RetroBenefitCodeViewModel>> Expression()
        {
            return entity => new RetroBenefitCodeViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                EffectiveDate = entity.EffectiveDate,
                CeaseDate = entity.CeaseDate,
                Status = entity.Status,
                Remarks = entity.Remarks,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(EffectiveDateStr);
            DateTime? end = Util.GetParseDateTime(CeaseDateStr);

            if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format("The Effective Date Field must be earlier than The Cease Date Field"),
                    new[] { nameof(EffectiveDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format("The Cease Date Field must be later than The Effective Date Field"),
                    new[] { nameof(CeaseDateStr) }));
                }
            }

            return results;
        }
    }
}