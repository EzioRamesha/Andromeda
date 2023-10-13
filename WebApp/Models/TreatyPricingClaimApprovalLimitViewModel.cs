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
    public class TreatyPricingClaimApprovalLimitViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedant TreatyPricingCedant { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, StringLength(255), DisplayName("Claim Approval Limit ID")]
        public string Code { get; set; }

        [Required, StringLength(255), DisplayName("Name")]
        public string Name { get; set; }
        
        [Required, StringLength(255), DisplayName("Benefit Code")]
        [ValidateMlreBenefitCode]
        public string BenefitCode { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Error Message")]
        public string Errors { get; set; }

        public int VersionId { get; set; }

        public int Version { get; set; }

        [RequiredVersion, DisplayName("Person In-Charge")]
        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        [DisplayName("Effective Date")]
        public DateTime? EffectiveAt { get; set; }

        [DisplayName("Effective Date")]
        public string EffectiveAtStr { get; set; }

        [RequiredVersion, DisplayName("Amount"), StringLength(255)]
        public string Amount { get; set; }

        [DisplayName("Additional Remarks")]
        public string AdditionalRemarks { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingClaimApprovalLimitViewModel() 
        {
            Set();
        }

        public TreatyPricingClaimApprovalLimitViewModel(TreatyPricingClaimApprovalLimitBo bo)
        {
            Set(bo);
            SetVersionObjects(bo.TreatyPricingClaimApprovalLimitVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }
        public void Set(TreatyPricingClaimApprovalLimitBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingCedantId = bo.TreatyPricingCedantId;
                TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
                Code = bo.Code;
                Name = bo.Name;
                Description = bo.Description;
                Status = bo.Status;
                BenefitCode = bo.BenefitCode;
                Remarks = bo.Remarks;
            }
        }

        public static Expression <Func<TreatyPricingClaimApprovalLimit, TreatyPricingClaimApprovalLimitViewModel>> Expression()
        {
            return entity => new TreatyPricingClaimApprovalLimitViewModel
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingClaimApprovalLimitBo.GetStatusName(entity.Status),
                BenefitCode = entity.BenefitCode,
                Errors = entity.Errors,
                Remarks = entity.Remarks,
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

        public TreatyPricingClaimApprovalLimitBo FormBo(TreatyPricingClaimApprovalLimitBo bo)
        {
            bo.TreatyPricingCedantId = TreatyPricingCedantId;
            bo.Code = Code;
            bo.Name = Name;
            bo.Description = Description;
            bo.Status = Status;
            bo.BenefitCode = BenefitCode;
            bo.Remarks = Remarks;

            return bo;
        }


        public TreatyPricingClaimApprovalLimitVersionBo GetVersionBo(TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);
            bo.Amount = Amount;
            bo.AdditionalRemarks = AdditionalRemarks;

            return bo;
        }
    }
}