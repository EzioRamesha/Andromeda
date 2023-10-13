using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using BusinessObject;
using Services;

namespace WebApp.Models
{
    public class PerLifeDuplicationCheckViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, StringLength(30), DisplayName("Configuration Code")]
        public string ConfigurationCode { get; set; }

        [Required, StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Inclusion")]
        public bool Inclusion { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public DateTime? ReinsuranceEffectiveStartDate { get; set; }
        [Required, DisplayName("Reinsurance Effective Start Date")]
        public string ReinsuranceEffectiveStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public DateTime? ReinsuranceEffectiveEndDate { get; set; }
        [Required, DisplayName("Reinsurance Effective End Date")]
        public string ReinsuranceEffectiveEndDateStr { get; set; }

        [Required, DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("No Of Treaty Code(s)")]
        public int NoOfTreatyCode { get; set; }

        [DisplayName("Enable Reinsurance Basis Code Check")]
        public bool EnableReinsuranceBasisCodeCheck { get; set; }

        public virtual ICollection<PerLifeDuplicationCheckDetail> PerLifeDuplicationCheckDetails { get; set; }

        public PerLifeDuplicationCheckViewModel() { }

        public PerLifeDuplicationCheckViewModel(PerLifeDuplicationCheckBo perLifeDuplicationCheckBo)
        {
            Set(perLifeDuplicationCheckBo);
        }

        public void Set(PerLifeDuplicationCheckBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                ConfigurationCode = bo.ConfigurationCode;
                Inclusion = bo.Inclusion;
                Description = bo.Description;
                ReinsuranceEffectiveStartDate = bo.ReinsuranceEffectiveStartDate;
                ReinsuranceEffectiveStartDateStr = bo.ReinsuranceEffectiveStartDate?.ToString(Util.GetDateFormat());
                ReinsuranceEffectiveEndDate = bo.ReinsuranceEffectiveEndDate;
                ReinsuranceEffectiveEndDateStr = bo.ReinsuranceEffectiveEndDate?.ToString(Util.GetDateFormat());
                TreatyCode = bo.TreatyCode;
                NoOfTreatyCode = bo.NoOfTreatyCode;
                EnableReinsuranceBasisCodeCheck = bo.EnableReinsuranceBasisCodeCheck;
            }
        }

        public PerLifeDuplicationCheckBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeDuplicationCheckBo
            {
                ConfigurationCode = ConfigurationCode,
                Inclusion = Inclusion,
                Description = Description,
                ReinsuranceEffectiveStartDate = ReinsuranceEffectiveStartDateStr is null || ReinsuranceEffectiveStartDateStr == "" ? null : Util.GetParseDateTime(ReinsuranceEffectiveStartDateStr),
                ReinsuranceEffectiveEndDate = ReinsuranceEffectiveEndDateStr is null || ReinsuranceEffectiveEndDateStr == "" ? null : Util.GetParseDateTime(ReinsuranceEffectiveEndDateStr),
                TreatyCode = TreatyCode,
                NoOfTreatyCode = NoOfTreatyCode,
                EnableReinsuranceBasisCodeCheck = EnableReinsuranceBasisCodeCheck,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeDuplicationCheck, PerLifeDuplicationCheckViewModel>> Expression()
        {
            return entity => new PerLifeDuplicationCheckViewModel
            {
                Id = entity.Id,
                ConfigurationCode = entity.ConfigurationCode,
                Description = entity.Description,
                Inclusion = entity.Inclusion,
                ReinsuranceEffectiveStartDate = entity.ReinsuranceEffectiveStartDate,
                ReinsuranceEffectiveEndDate = entity.ReinsuranceEffectiveEndDate,
                TreatyCode = entity.TreatyCode,
                NoOfTreatyCode = entity.NoOfTreatyCode,
                EnableReinsuranceBasisCodeCheck = entity.EnableReinsuranceBasisCodeCheck,

                PerLifeDuplicationCheckDetails = entity.PerLifeDuplicationCheckDetails,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(ReinsuranceEffectiveStartDateStr);
            DateTime? end = Util.GetParseDateTime(ReinsuranceEffectiveEndDateStr);

            if (start != null && end != null && end <= start)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Reinsurance Effective"),
                    new[] { nameof(ReinsuranceEffectiveStartDateStr) }));
                results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Reinsurance Effective"),
                    new[] { nameof(ReinsuranceEffectiveEndDateStr) }));
            }

            if (!string.IsNullOrEmpty(TreatyCode))
            {
                string[] treatyCodes = TreatyCode.ToArraySplitTrim();

                foreach (string treatyCodeStr in treatyCodes)
                {
                    var treatyCode = TreatyCodeService.FindByCode(treatyCodeStr);
                    if (treatyCode != null)
                    {
                        if (treatyCode.Status != TreatyCodeBo.StatusActive)
                        {
                            results.Add(new ValidationResult(
                                string.Format(MessageBag.TreatyCodeStatusInactiveWithCode, treatyCodeStr),
                                new[] { nameof(TreatyCode) }));
                        }
                    }
                    else
                    {
                        results.Add(new ValidationResult(
                            string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr),
                            new[] { nameof(TreatyCode) }));
                    }
                }
            }

            return results;
        }
    }
}