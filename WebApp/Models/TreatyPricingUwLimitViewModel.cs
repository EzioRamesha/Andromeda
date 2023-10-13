using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TreatyPricingUwLimitViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int TreatyPricingCedantId { get; set; }

        [DisplayName("Underwriting Limit ID")]
        public string LimitId { get; set; }

        [Required, StringLength(255), DisplayName("Underwriting Limit Name")]
        public string Name { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        public int VersionId { get; set; }

        public int Version { get; set; }

        [Required, DisplayName("Benefit Code")]
        public string BenefitCode { get; set; }

        [RequiredVersion, Display(Name = "Person In-Charge")]
        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        [Display(Name = "Effective Date")]
        public DateTime? EffectiveAt { get; set; }

        [Display(Name = "Effective Date")]
        public string EffectiveAtStr { get; set; }

        [RequiredVersion, DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Underwriting Limit")]
        public string UwLimit { get; set; }

        [DisplayName("Additional Remark")]
        public string Remarks1 { get; set; }

        [DisplayName("Auto Binding Limit - Sum Assured")]
        public string AblSumAssured { get; set; }

        [DisplayName("Additional Remark")]
        public string Remarks2 { get; set; }

        [DisplayName("Auto Binding Limit - Maximum Underwriting Rating (EM)")]
        public string AblMaxUwRating { get; set; }

        [DisplayName("Additional Remark")]
        public string Remarks3 { get; set; }

        [DisplayName("Maximum Sum Assured")]
        public string MaxSumAssured { get; set; }

        [DisplayName("Per Life / Per Life Per Industry")]
        public bool PerLifePerIndustry { get; set; }

        [DisplayName("Issue Limit / Payout Limit")]
        public bool IssuePayoutLimit { get; set; }

        [DisplayName("Additional Remarks")]
        public string Remarks4 { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingUwLimitViewModel()
        {
            Set();
        }

        public TreatyPricingUwLimitViewModel(TreatyPricingUwLimitBo uwLimitBo)
        {
            Set(uwLimitBo);
            SetVersionObjects(uwLimitBo.TreatyPricingUwLimitVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingUwLimitBo uwLimitBo = null)
        {
            if (uwLimitBo != null)
            {
                Id = uwLimitBo.Id;
                TreatyPricingCedantId = uwLimitBo.TreatyPricingCedantId;
                TreatyPricingCedantBo = uwLimitBo.TreatyPricingCedantBo;
                LimitId = uwLimitBo.LimitId;
                Name = uwLimitBo.Name;
                Description = uwLimitBo.Description;
                Status = uwLimitBo.Status;
                BenefitCode = uwLimitBo.BenefitCode;
            }
        }

        public TreatyPricingUwLimitBo FormBo(TreatyPricingUwLimitBo bo)
        {
            bo.LimitId = LimitId;
            bo.Name = Name;
            bo.Description = Description;
            bo.Status = Status;
            bo.BenefitCode = BenefitCode;

            return bo;
        }

        public TreatyPricingUwLimitVersionBo GetVersionBo(TreatyPricingUwLimitVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.CurrencyCode = CurrencyCode;
            bo.UwLimit = UwLimit;
            bo.Remarks1 = Remarks1;
            bo.AblSumAssured = AblSumAssured;
            bo.Remarks2 = Remarks2;
            bo.AblMaxUwRating = AblMaxUwRating;
            bo.Remarks3 = Remarks3;
            bo.MaxSumAssured = MaxSumAssured;
            bo.PerLifePerIndustry = PerLifePerIndustry;
            bo.IssuePayoutLimit = IssuePayoutLimit;
            bo.Remarks4 = Remarks4;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);

            return bo;
        }
    }
}