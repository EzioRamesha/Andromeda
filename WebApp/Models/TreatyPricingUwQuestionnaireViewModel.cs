using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TreatyPricingUwQuestionnaireViewModel : ObjectVersion, IValidatableObject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, Display(Name = "Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Display(Name = "Underwriting Questionnaire ID")]
        public string Code { get; set; }

        [Required, Display(Name = "Underwriting Questionnaire Name")]
        public string Name { get; set; }

        [Required]
        public int Status { get; set; }

        public string Description { get; set; }

        [Required, Display(Name = "Benefit Code")]
        public string BenefitCode { get; set; }

        [Display(Name = "Distribution Channel")]
        public string DistributionChannel { get; set; }

        public int VersionId { get; set; }

        public int Version { get; set; }

        [Required, Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        [Display(Name = "Effective Date")]
        public DateTime? EffectiveAt { get; set; }

        [Display(Name = "Effective Date")]
        public string EffectiveAtStr { get; set; }

        [Required, Display(Name = "Questionnaire Type")]
        public int Type { get; set; }

        [Display(Name = "Note")]
        public string Remarks { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingUwQuestionnaireViewModel()
        {
            Set();
        }

        public TreatyPricingUwQuestionnaireViewModel(TreatyPricingUwQuestionnaireBo treatyPricingUwQuestionnaireBo)
        {
            Set(treatyPricingUwQuestionnaireBo);
            SetVersionObjects(treatyPricingUwQuestionnaireBo.TreatyPricingUwQuestionnaireVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingUwQuestionnaireBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            TreatyPricingCedantId = bo.TreatyPricingCedantId;
            TreatyPricingCedantBo = bo.TreatyPricingCedantBo;

            Code = bo.Code;
            Name = bo.Name;
            Description = bo.Description;
            Status = bo.Status;
            BenefitCode = bo.BenefitCode;
            DistributionChannel = bo.DistributionChannel;
        }

        public TreatyPricingUwQuestionnaireBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingUwQuestionnaireBo
            {
                Id = Id,
                Code = Code,
                Name = Name,
                Description = Description,
                Status = Status,
                BenefitCode = BenefitCode,
                DistributionChannel = DistributionChannel,
                TreatyPricingCedantId = TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(TreatyPricingCedantId),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingUwQuestionnaireVersionBo GetVersionBo(TreatyPricingUwQuestionnaireVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = Util.GetParseDateTime(EffectiveAtStr) ?? DateTime.Now.Date;
            bo.Type = Type;
            bo.Remarks = Remarks;

            return bo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (Type == 0)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Type"),
                    new[] { nameof(Type) }));
            }

            return results;
        }
    }
}