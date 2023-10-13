using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TreatyPricingRateTableViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Rate Table Group")]
        public int TreatyPricingRateTableGroupId { get; set; }

        public TreatyPricingRateTableGroup TreatyPricingRateTableGroup { get; set; }

        public TreatyPricingRateTableGroupBo TreatyPricingRateTableGroupBo { get; set; }

        [Required, DisplayName("MLRe Benefit Code")]
        public int BenefitId { get; set; }

        public Benefit Benefit { get; set; }

        public BenefitBo BenefitBo { get; set; }

        [Required, StringLength(255), DisplayName("Rate Table ID")]
        public string Code { get; set; }

        [StringLength(255), DisplayName("Rate Table Name")]
        public string Name { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        // Version
        [DisplayName("Rate Table")]
        public int TreatyPricingRateTableId { get; set; }

        public TreatyPricingRateTable TreatyPricingRateTable { get; set; }

        public TreatyPricingRateTableBo TreatyPricingRateTableBo { get; set; }

        [Required]
        [DisplayName("Version")]
        public int Version { get; set; }

        [RequiredVersion, DisplayName("Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public User PersonInCharge { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        [DisplayName("Benefit Marketing Name")]
        public string BenefitName { get; set; }

        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Effective Date")]
        [ValidateDate]
        public string EffectiveDateStr { get; set; }

        [DisplayName("Age Basis")]
        public int? AgeBasisPickListDetailId { get; set; }

        public PickListDetail AgeBasisPickListDetail { get; set; }

        public PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        [DisplayName("RI Discount")]
        public string RiDiscount { get; set; }

        [DisplayName("Coinsurance RI Discount")]
        public string CoinsuranceRiDiscount { get; set; }

        [DisplayName("Rate Guarantee")]
        public int? RateGuaranteePickListDetailId { get; set; }

        public PickListDetail RateGuaranteePickListDetail { get; set; }

        public PickListDetailBo RateGuaranteePickListDetailBo { get; set; }

        [DisplayName("Rate Guarantee for New Business")]
        public string RateGuaranteeForNewBusiness { get; set; }

        [DisplayName("Rate Guarantee for Renewal Business")]
        public string RateGuaranteeForRenewalBusiness { get; set; }

        [DisplayName("Advantage Program")]
        public string AdvantageProgram { get; set; }

        [DisplayName("Profit Commission")]
        public string ProfitCommission { get; set; }

        [DisplayName("Large Size Discount")]
        public string LargeSizeDiscount { get; set; }

        [DisplayName("Juvenile Lien")]
        public string JuvenileLien { get; set; }

        [DisplayName("Special Lien")]
        public string SpecialLien { get; set; }

        [DisplayName("Additional Remark")]
        public string AdditionalRemark { get; set; }

        public string RateTableRate { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingRateTableViewModel() { }

        public TreatyPricingRateTableViewModel(TreatyPricingRateTableBo treatyPricingRateTableBo)
        {
            Set(treatyPricingRateTableBo);
            SetVersionObjects(treatyPricingRateTableBo.TreatyPricingRateTableVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingRateTableBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingRateTableGroupId = bo.TreatyPricingRateTableGroupId;
                TreatyPricingRateTableGroupBo = bo.TreatyPricingRateTableGroupBo;
                BenefitId = bo.BenefitId;
                BenefitBo = bo.BenefitBo;
                Code = bo.Code;
                Name = bo.Name;
                Description = bo.Description;
                Status = bo.Status;
            }
        }

        public TreatyPricingRateTableBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingRateTableBo
            {
                Id = Id,
                TreatyPricingRateTableGroupId = TreatyPricingRateTableGroupId,
                TreatyPricingRateTableGroupBo = TreatyPricingRateTableGroupService.Find(TreatyPricingRateTableGroupId),
                BenefitId = BenefitId,
                BenefitBo = BenefitBo,
                Code = Code,
                Name = Name,
                Description = Description,
                Status = Status,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<TreatyPricingRateTable, TreatyPricingRateTableViewModel>> Expression()
        {
            return entity => new TreatyPricingRateTableViewModel
            {
                Id = entity.Id,
                TreatyPricingRateTableGroupId = entity.TreatyPricingRateTableGroupId,
                TreatyPricingRateTableGroup = entity.TreatyPricingRateTableGroup,
                BenefitId = entity.BenefitId,
                Benefit = entity.Benefit,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
            };
        }

        public TreatyPricingRateTableVersionBo GetVersionBo(TreatyPricingRateTableVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.EffectiveDateStr = EffectiveDateStr;
            bo.EffectiveDate = Util.GetParseDateTime(EffectiveDateStr);
            bo.AgeBasisPickListDetailId = AgeBasisPickListDetailId;
            bo.RiDiscount = RiDiscount;
            bo.CoinsuranceRiDiscount = CoinsuranceRiDiscount;
            bo.RateGuaranteePickListDetailId = RateGuaranteePickListDetailId;
            bo.RateGuaranteeForNewBusiness = RateGuaranteeForNewBusiness;
            bo.RateGuaranteeForRenewalBusiness = RateGuaranteeForRenewalBusiness;
            bo.AdvantageProgram = AdvantageProgram;
            bo.ProfitCommission = ProfitCommission;
            bo.LargeSizeDiscount = LargeSizeDiscount;
            bo.JuvenileLien = JuvenileLien;
            bo.SpecialLien = SpecialLien;
            bo.AdditionalRemark = AdditionalRemark;
            bo.RateTableRate = RateTableRate;

            return bo;
        }
    }
}