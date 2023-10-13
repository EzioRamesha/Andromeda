using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyPricingCustomOtherViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedant TreatyPricingCedant { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, StringLength(255), DisplayName("Custom / Other ID")]
        public string Code { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        [Required, StringLength(255), DisplayName("Name")]
        public string Name { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

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

        [DisplayName("Upload")]
        public string FileName { get; set; }

        [DisplayName("Upload")]
        public string HashFileName { get; set; }

        public HttpPostedFileBase[] Upload { get; set; }

        [DisplayName("Uploaded At")]
        public DateTime UploadedAt { get; set; }

        [DisplayName("Uploaded At")]
        public string UploadedAtStr { get; set; }

        [DisplayName("Uploaded By")]
        public int UploadedById { get; set; }

        [DisplayName("Uploaded By")]
        public User UploadedBy { get; set; }

        [DisplayName("Uploaded By")]
        public UserBo UploadedByBo { get; set; }

        [DisplayName("Additional Remarks")]
        public string AdditionalRemarks { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingCustomOtherViewModel()
        {
            Set();
        }

        public TreatyPricingCustomOtherViewModel(TreatyPricingCustomOtherBo bo)
        {
            Set(bo);
            SetVersionObjects(bo.TreatyPricingCustomOtherVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingCustomOtherBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingCedantId = bo.TreatyPricingCedantId;
                TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
                Code = bo.Code;
                Status = bo.Status;
                Name = bo.Name;
                Description = bo.Description;
            }
        }

        public static Expression<Func<TreatyPricingCustomOther, TreatyPricingCustomOtherViewModel>> Expression()
        {
            return entity => new TreatyPricingCustomOtherViewModel
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Status = entity.Status,
                Name = entity.Name,
                Description = entity.Description,
                StatusName = TreatyPricingCustomOtherBo.GetStatusName(entity.Status),
                Errors = entity.Errors,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            return results;
        }

        public TreatyPricingCustomOtherBo FormBo(TreatyPricingCustomOtherBo bo)
        {
            bo.TreatyPricingCedantId = TreatyPricingCedantId;
            bo.Code = Code;
            bo.Status = Status;
            bo.Name = Name;
            bo.Description = Description;

            return bo;
        }


        public TreatyPricingCustomOtherVersionBo GetVersionBo(TreatyPricingCustomOtherVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeName = PersonInChargeName;
            bo.EffectiveAtStr = EffectiveAtStr;
            bo.EffectiveAt = EffectiveAtStr is null || EffectiveAtStr == "" ? null : Util.GetParseDateTime(EffectiveAtStr);
            bo.FileName = FileName;
            bo.HashFileName = HashFileName;
            bo.AdditionalRemarks = AdditionalRemarks;

            return bo;
        }

        public void ProcessProducts(FormCollection form, ref TrailObject trail)
        {
            int maxIndex = int.Parse(form.Get("CustomOtherProductsMaxIndex"));
            int index = 0;

            if (maxIndex != index)
            {
                // Delete all
                TreatyPricingCustomOtherProductService.DeleteAllByTreatyPricingCustomOtherId(Id);

                while (index <= maxIndex)
                {
                    // Create
                    string productId = form.Get(string.Format("productId[{0}]", index));
                    if (!string.IsNullOrEmpty(productId))
                    {
                        TreatyPricingCustomOtherProductBo bo = new TreatyPricingCustomOtherProductBo
                        {
                            TreatyPricingCustomOtherId = Id,
                            TreatyPricingProductId = int.Parse(productId),
                        };
                        TreatyPricingCustomOtherProductService.Create(ref bo, ref trail);

                    }
                    index++;
                }
            }
        }
    }
}