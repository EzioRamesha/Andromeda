using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TreatyPricingProfitCommissionViewModel : ObjectVersion, IValidatableObject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }
        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [DisplayName("Profit Commission ID")]
        [Required, StringLength(255)]
        public string Code { get; set; }

        [DisplayName("Profit Commission Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [ValidateMlreBenefitCode]
        [DisplayName("MLRe Benefit Code")]
        public string BenefitCode { get; set; }

        [DisplayName("Description")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [ValidateDate]
        [DisplayName("Effective Date")]
        public string EffectiveDateStr { get; set; }

        [DisplayName("Profit Commission Start Date")]
        public DateTime? StartDate { get; set; }

        [ValidateDate]
        [DisplayName("Profit Commission Start Date")]
        public string StartDateStr { get; set; }

        [DisplayName("Profit Commission End Date")]
        public DateTime? EndDate { get; set; }

        [ValidateDate]
        [DisplayName("Profit Commission End Date")]
        public string EndDateStr { get; set; }

        [DisplayName("Entitlement")]
        public string Entitlement { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [Required]
        [DisplayName("Version")]
        public int Version { get; set; }

        [RequiredVersion, Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public User PersonInCharge { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        [DisplayName("Profit Sharing")]
        public int? ProfitSharing { get; set; }

        [DisplayName("Description")]
        public string ProfitDescription { get; set; }

        [DisplayName("Percentage of Net Profit")]
        public double? NetProfitPercentage { get; set; }

        [DisplayName("Percentage of Net Profit")]
        [ValidateDouble]
        public string NetProfitPercentageStr { get; set; }

        [DisplayName("Profit Commission Detail")]
        public string ProfitCommissionDetail { get; set; }

        [DisplayName("Tier Profit Commission")]
        public string TierProfitCommission { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingProfitCommissionViewModel() { }

        public TreatyPricingProfitCommissionViewModel(TreatyPricingProfitCommissionBo treatyPricingProfitCommissionBo)
        {
            Set(treatyPricingProfitCommissionBo);
            SetVersionObjects(treatyPricingProfitCommissionBo.TreatyPricingProfitCommissionVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingProfitCommissionBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingCedantId = bo.TreatyPricingCedantId;
                TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
                Code = bo.Code;
                Name = bo.Name;
                BenefitCode = bo.BenefitCode;
                Description = bo.Description;
                Status = bo.Status;
                EffectiveDate = bo.EffectiveDate;
                EffectiveDateStr = bo.EffectiveDate?.ToString(Util.GetDateFormat());
                StartDate = bo.StartDate;
                StartDateStr = bo.StartDate?.ToString(Util.GetDateFormat());
                EndDate = bo.EndDate;
                EndDateStr = bo.EndDate?.ToString(Util.GetDateFormat());
                Entitlement = bo.Entitlement;
                Remark = bo.Remark;
            }
        }

        public TreatyPricingProfitCommissionBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingProfitCommissionBo
            {
                Id = Id,
                TreatyPricingCedantId = TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(TreatyPricingCedantId),
                Code = Code,
                Name = Name,
                BenefitCode = BenefitCode,
                Description = Description,
                Status = Status,
                EffectiveDate = Util.GetParseDateTime(EffectiveDateStr),
                StartDate = Util.GetParseDateTime(StartDateStr),
                EndDate = Util.GetParseDateTime(EndDateStr),
                Entitlement = Entitlement,
                Remark = Remark,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingProfitCommissionBo FormBo(TreatyPricingProfitCommissionBo bo)
        {
            bo.Code = Code;
            bo.Name = Name;
            bo.BenefitCode = BenefitCode;
            bo.Description = Description;
            bo.Status = Status;
            bo.EffectiveDate = Util.GetParseDateTime(EffectiveDateStr);
            bo.StartDate = Util.GetParseDateTime(StartDateStr);
            bo.EndDate = Util.GetParseDateTime(EndDateStr);
            bo.Entitlement = Entitlement;
            bo.Remark = Remark;
            return bo;
        }

        public static Expression<Func<TreatyPricingProfitCommission, TreatyPricingProfitCommissionViewModel>> Expression()
        {
            return entity => new TreatyPricingProfitCommissionViewModel
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                BenefitCode = entity.BenefitCode,
                Description = entity.Description,
                Status = entity.Status,
                EffectiveDate = entity.EffectiveDate,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Entitlement = entity.Entitlement,
                Remark = entity.Remark,
            };
        }

        public TreatyPricingProfitCommissionVersionBo GetVersionBo(TreatyPricingProfitCommissionVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.ProfitSharing = ProfitSharing;
            bo.ProfitDescription = ProfitDescription;
            bo.ProfitCommissionDetail = ProfitCommissionDetail;
            bo.TierProfitCommission = TierProfitCommission;

            if (ProfitSharing.HasValue && ProfitSharing == TreatyPricingProfitCommissionVersionBo.ProfitSharingFlat)
            {
                bo.NetProfitPercentageStr = NetProfitPercentageStr;
                bo.NetProfitPercentage = Util.StringToDouble(NetProfitPercentageStr);
            }
            else
            {
                bo.NetProfitPercentageStr = null;
                bo.NetProfitPercentage = null;
            }

            return bo;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(StartDateStr);
            DateTime? end = Util.GetParseDateTime(EndDateStr);

            if (start == null && end != null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Profit Commission Start Date Field"),
                    new[] { nameof(StartDateStr) }));
            }
            else if (start != null && end == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Profit Commission End Date Field"),
                    new[] { nameof(EndDateStr) }));
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Profit Commission"),
                    new[] { nameof(StartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Profit Commission"),
                    new[] { nameof(EndDateStr) }));
                }
            }

            if (ProfitSharing.HasValue && ProfitSharing.Value == TreatyPricingProfitCommissionVersionBo.ProfitSharingFlat && string.IsNullOrEmpty(NetProfitPercentageStr))
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Percentage of Net Profit"),
                    new[] { nameof(NetProfitPercentageStr) }));
            }

            return results;
        }
    }
}