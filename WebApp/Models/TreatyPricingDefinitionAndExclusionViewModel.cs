using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
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
    public class TreatyPricingDefinitionAndExclusionViewModel : ObjectVersion
    {
        public int Id { get; set; }
        
        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedant TreatyPricingCedant { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, StringLength(255), DisplayName("Definition and Exclusion ID")]
        public string Code { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        [Required, StringLength(255), DisplayName("Name")]
        public string Name { get; set; }


        [DisplayName("Additional Remarks")]
        public string AdditionalRemarks { get; set; }

        [Required, StringLength(255), DisplayName("Benefit Code")]
        [ValidateMlreBenefitCode]
        public string BenefitCode { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Errors")]
        public string Errors { get; set; }

        [RequiredVersion, DisplayName("Person In-Charge")]
        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        [DisplayName("Effective Date")]
        public DateTime? EffectiveAt { get; set; }

        [DisplayName("Effective Date")]
        public string EffectiveAtStr { get; set; }

        [RequiredVersion, DisplayName("Definitions")]
        public string Definitions { get; set; }

        [RequiredVersion, DisplayName("Exclusions")]
        public string Exclusions { get; set; }

        [DisplayName("Declined Risk")]
        public string DeclinedRisk { get; set; }

        [DisplayName("Referred Risk")]
        public string ReferredRisk { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingDefinitionAndExclusionViewModel() 
        {
            Set();
        }

        public TreatyPricingDefinitionAndExclusionViewModel(TreatyPricingDefinitionAndExclusionBo bo)
        {
            Set(bo);
            SetVersionObjects(bo.TreatyPricingDefinitionAndExclusionVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingDefinitionAndExclusionBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingCedantId = bo.TreatyPricingCedantId;
                TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
                Code = bo.Code;
                Status = bo.Status;
                Description = bo.Description;
                BenefitCode = bo.BenefitCode;
                Name = bo.Name;
                AdditionalRemarks = bo.AdditionalRemarks;
            }
        }

        public static Expression <Func<TreatyPricingDefinitionAndExclusion, TreatyPricingDefinitionAndExclusionViewModel>> Expression()
        {
            return entity => new TreatyPricingDefinitionAndExclusionViewModel
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Status = entity.Status,
                StatusName = TreatyPricingDefinitionAndExclusionBo.GetStatusName(entity.Status),
                Name = entity.Name,
                Errors = entity.Errors,
                AdditionalRemarks = entity.AdditionalRemarks,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            return results;
        }

        public class ValidateBenefitCode : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string benefitCode = value.ToString();
                    string[] resultsArray = benefitCode.ToArraySplitTrim();
                    IList<BenefitBo> benefitBos = BenefitService.Get();
                    List<string> benefits = new List<string>();
                    if (benefitBos != null)
                    {
                        foreach (BenefitBo benefitBo in benefitBos)
                        {
                            benefits.Add(benefitBo.Code);
                        }
                    }
                    foreach (string result in resultsArray)
                    {
                        if (!benefits.Contains(result))
                        {
                            return new ValidationResult("Please Enter Valid Benefit Code.");
                        }
                    }
                }
                return ValidationResult.Success;
            }
        }

        public TreatyPricingDefinitionAndExclusionBo FormBo(TreatyPricingDefinitionAndExclusionBo bo)
        {
            bo.TreatyPricingCedantId = TreatyPricingCedantId;
            bo.Name = Name;
            bo.Description = Description;
            bo.Status = Status;
            bo.BenefitCode = BenefitCode;
            bo.AdditionalRemarks = AdditionalRemarks;

            return bo;
        }

        public TreatyPricingDefinitionAndExclusionVersionBo GetVersionBo(TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);
            bo.Definitions = Definitions;
            bo.Exclusions = Exclusions;
            bo.DeclinedRisk = DeclinedRisk;
            bo.ReferredRisk = ReferredRisk;

            return bo;
        }


    }
}