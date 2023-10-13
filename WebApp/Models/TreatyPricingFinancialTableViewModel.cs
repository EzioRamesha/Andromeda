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
    public class TreatyPricingFinancialTableViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [DisplayName("Financial Table ID")]
        public string FinancialTableId { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        [Required, StringLength(255), DisplayName("Financial Table Name")]
        public string Name { get; set; }

        [Required, StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Benefit Code")]
        public string BenefitCode { get; set; }

        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }

        [Required, DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        public int VersionId { get; set; }

        public int Version { get; set; }

        [RequiredVersion, Display(Name = "Person In-Charge")]
        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        [Display(Name = "Effective Date")]
        public DateTime? EffectiveAt { get; set; }

        [Display(Name = "Effective Date")]
        public string EffectiveAtStr { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Aggregation Note")]
        public string AggregationNote { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingFinancialTableViewModel()
        {
            Set();
        }

        public TreatyPricingFinancialTableViewModel(TreatyPricingFinancialTableBo FinancialTableBo)
        {
            Set(FinancialTableBo);
            SetVersionObjects(FinancialTableBo.TreatyPricingFinancialTableVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingFinancialTableBo FinancialTableBo = null)
        {
            if (FinancialTableBo != null)
            {
                Id = FinancialTableBo.Id;
                TreatyPricingCedantId = FinancialTableBo.TreatyPricingCedantId;
                TreatyPricingCedantBo = FinancialTableBo.TreatyPricingCedantBo;
                FinancialTableId = FinancialTableBo.FinancialTableId;
                Status = FinancialTableBo.Status;
                Name = FinancialTableBo.Name;
                Description = FinancialTableBo.Description;
                BenefitCode = FinancialTableBo.BenefitCode;
                DistributionChannel = FinancialTableBo.DistributionChannel;
                CurrencyCode = FinancialTableBo.CurrencyCode;
            }
        }

        public TreatyPricingFinancialTableBo FormBo(TreatyPricingFinancialTableBo bo)
        {
            bo.FinancialTableId = FinancialTableId;
            bo.Status = Status;
            bo.Name = Name;
            bo.Description = Description;
            bo.BenefitCode = BenefitCode;
            bo.DistributionChannel = DistributionChannel;
            bo.CurrencyCode = CurrencyCode;

            return bo;
        }

        public TreatyPricingFinancialTableVersionBo GetVersionBo(TreatyPricingFinancialTableVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.Remarks = Remarks;
            bo.AggregationNote = AggregationNote;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);

            return bo;
        }
    }
}