using BusinessObject;
using DataAccess.Entities;
using Services;
using Services.Identity;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class BenefitViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required, StringLength(10), DisplayName("MLRe Benefit Code")]
        public string Code { get; set; }

        [Required, StringLength(128), DisplayName("Description")]
        public string Description { get; set; }

        [Required, StringLength(128), DisplayName("Benefit Type")]
        public string Type { get; set; }

        [Required, DisplayName("Status")]
        public int Status { get; set; }

        public string StatusStr { get; set; }

        [Required, DisplayName("Valuation Benefit Code")]
        public int? ValuationBenefitCodePickListDetailId { get; set; }

        public virtual PickListDetail ValuationBenefitCodePickListDetail { get; set; }

        [DisplayName("Valuation Benefit Code")]
        public PickListDetailBo ValuationBenefitCodePickListDetailBo { get; set; }

        [Required, DisplayName("Benefit Category")]
        public int? BenefitCategoryPickListDetailId { get; set; }

        public virtual PickListDetail BenefitCategoryPickListDetail { get; set; }

        [DisplayName("Benefit Category")]
        public PickListDetailBo BenefitCategoryPickListDetailBo { get; set; }

        public bool GST { get; set; }

        [DisplayName("Effective Start Date")]
        public DateTime? EffectiveStartDate { get; set; }

        [DisplayName("Effective End Date")]
        public DateTime? EffectiveEndDate { get; set; }

        [Required, DisplayName("Effective Start Date")]
        [ValidateDate]
        public string EffectiveStartDateStr { get; set; }

        [DisplayName("Effective End Date")]
        [ValidateDate]
        public string EffectiveEndDateStr { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        public virtual ICollection<BenefitDetail> BenefitDetails { get; set; }

        public BenefitViewModel() { }

        public BenefitViewModel(BenefitBo benefitBo)
        {
            Set(benefitBo);
        }

        public void Set(BenefitBo benefitBo)
        {
            if (benefitBo != null)
            {
                Id = benefitBo.Id;
                Code = benefitBo.Code;
                Description = benefitBo.Description;
                Type = benefitBo.Type;
                Status = benefitBo.Status;
                ValuationBenefitCodePickListDetailId = benefitBo.ValuationBenefitCodePickListDetailId;
                BenefitCategoryPickListDetailId = benefitBo.BenefitCategoryPickListDetailId;
                GST = benefitBo.GST;
                EffectiveStartDate = benefitBo.EffectiveStartDate;
                EffectiveStartDateStr = benefitBo.EffectiveStartDate?.ToString(Util.GetDateFormat());
                EffectiveEndDate = benefitBo.EffectiveEndDate;
                EffectiveEndDateStr = benefitBo.EffectiveEndDate?.ToString(Util.GetDateFormat());

                CreatedBy = UserService.Find(benefitBo.CreatedById).FullName;
                ValuationBenefitCodePickListDetailBo = benefitBo.ValuationBenefitCodePickListDetailBo;
                BenefitCategoryPickListDetailBo = benefitBo.BenefitCategoryPickListDetailBo;
            }
        }

        public BenefitBo FormBo(int createdById, int updatedById)
        {
            DateTime? EffectiveEndDate;
            if (!string.IsNullOrEmpty(EffectiveEndDateStr))
                EffectiveEndDate = DateTime.Parse(EffectiveEndDateStr);
            else
                EffectiveEndDate = DateTime.Parse(Util.GetDefaultEndDate());

            return new BenefitBo
            {
                Id = Id,
                Code = Code?.Trim(),
                Description = Description?.Trim(),
                Type = Type?.Trim(),
                Status = Status,
                ValuationBenefitCodePickListDetailId = ValuationBenefitCodePickListDetailId,
                ValuationBenefitCodePickListDetailBo = PickListDetailService.Find(ValuationBenefitCodePickListDetailId),
                BenefitCategoryPickListDetailId = BenefitCategoryPickListDetailId,
                BenefitCategoryPickListDetailBo = PickListDetailService.Find(BenefitCategoryPickListDetailId),
                GST = GST,
                EffectiveStartDate = DateTime.Parse(EffectiveStartDateStr),
                EffectiveEndDate = EffectiveEndDate,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Benefit, BenefitViewModel>> Expression()
        {
            return entity => new BenefitViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Type = entity.Type,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                Description = entity.Description,
                Status = entity.Status,
                GST = entity.GST,
                ValuationBenefitCodePickListDetailId = entity.ValuationBenefitCodePickListDetailId,
                BenefitCategoryPickListDetailId = entity.BenefitCategoryPickListDetailId,

                ValuationBenefitCodePickListDetail = entity.ValuationBenefitCodePickListDetail,
                BenefitCategoryPickListDetail = entity.BenefitCategoryPickListDetail,
                BenefitDetails = entity.BenefitDetails,
            };
        }

        public List<BenefitDetailBo> GetBenefitDetails(FormCollection form)
        {
            int index = 0;
            List<BenefitDetailBo> benefitDetailBos = new List<BenefitDetailBo> { };
            while (form.AllKeys.Contains(string.Format("eventCode[{0}]", index)))
            {
                string eventCode = form.Get(string.Format("eventCode[{0}]", index));
                string claimCode = form.Get(string.Format("claimCode[{0}]", index));
                string id = form.Get(string.Format("benefitDetailId[{0}]", index));

                BenefitDetailBo benefitDetailBo = new BenefitDetailBo
                {
                    BenefitId = Id,
                };

                if (!string.IsNullOrEmpty(eventCode)) benefitDetailBo.EventCodeId = int.Parse(eventCode);
                if (!string.IsNullOrEmpty(claimCode)) benefitDetailBo.ClaimCodeId = int.Parse(claimCode);
                if (!string.IsNullOrEmpty(id)) benefitDetailBo.Id = int.Parse(id);

                benefitDetailBos.Add(benefitDetailBo);
                index++;
            }
            return benefitDetailBos;
        }

        public void ProcessBenefitDetails(List<BenefitDetailBo> benefitDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            foreach (BenefitDetailBo benefitDetailBo in benefitDetailBos)
            {
                benefitDetailBo.BenefitId = Id;
                benefitDetailBo.CreatedById = authUserId;
                benefitDetailBo.UpdatedById = authUserId;
                BenefitDetailService.Save(benefitDetailBo, ref trail);

                savedIds.Add(benefitDetailBo.Id);
            }
            BenefitDetailService.DeleteByBenefitIdExcept(Id, savedIds, ref trail);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(EffectiveStartDateStr);
            DateTime? end = Util.GetParseDateTime(EffectiveEndDateStr);

            if (end != null && start == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Effective Start Date"),
                    new[] { nameof(EffectiveStartDateStr) }));
            }
            else if (start != null && end == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDefaultDate, "Effective", Util.GetDefaultEndDate()),
                    new[] { nameof(EffectiveStartDateStr) }));
                }
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Effective"),
                    new[] { nameof(EffectiveStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Effective"),
                    new[] { nameof(EffectiveEndDateStr) }));
                }
            }

            return results;
        }
    }
}