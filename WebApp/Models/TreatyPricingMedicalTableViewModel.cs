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
    public class TreatyPricingMedicalTableViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [DisplayName("Medical Table ID")]
        public string MedicalTableId { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        [Required, StringLength(255), DisplayName("Medical Table Name")]
        public string Name { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Benefit Code")]
        public string BenefitCode { get; set; }

        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }

        [Required, DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [Required, DisplayName("Age Definition")]
        public int? AgeDefinitionPickListDetailId { get; set; }

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

        public TreatyPricingMedicalTableViewModel()
        {
            Set();
        }

        public TreatyPricingMedicalTableViewModel(TreatyPricingMedicalTableBo MedicalTableBo)
        {
            Set(MedicalTableBo);
            SetVersionObjects(MedicalTableBo.TreatyPricingMedicalTableVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingMedicalTableBo MedicalTableBo = null)
        {
            if (MedicalTableBo != null)
            {
                Id = MedicalTableBo.Id;
                TreatyPricingCedantId = MedicalTableBo.TreatyPricingCedantId;
                TreatyPricingCedantBo = MedicalTableBo.TreatyPricingCedantBo;
                MedicalTableId = MedicalTableBo.MedicalTableId;
                Status = MedicalTableBo.Status;
                Name = MedicalTableBo.Name;
                Description = MedicalTableBo.Description;
                BenefitCode = MedicalTableBo.BenefitCode;
                DistributionChannel = MedicalTableBo.DistributionChannel;
                CurrencyCode = MedicalTableBo.CurrencyCode;
                AgeDefinitionPickListDetailId = MedicalTableBo.AgeDefinitionPickListDetailId;
            }
        }

        public TreatyPricingMedicalTableBo FormBo(TreatyPricingMedicalTableBo bo)
        {
            bo.MedicalTableId = MedicalTableId;
            bo.Status = Status;
            bo.Name = Name;
            bo.Description = Description;
            bo.BenefitCode = BenefitCode;
            bo.DistributionChannel = DistributionChannel;
            bo.CurrencyCode = CurrencyCode;
            bo.AgeDefinitionPickListDetailId = AgeDefinitionPickListDetailId;

            return bo;
        }

        public TreatyPricingMedicalTableVersionBo GetVersionBo(TreatyPricingMedicalTableVersionBo bo)
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