using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class TreatyPricingRateTableGroupViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedant TreatyPricingCedant { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }
        
        [Required, StringLength(255), DisplayName("Group Rate Table ID")]
        public string Code { get; set; }
        
        [StringLength(255), DisplayName("Group Rate Table Name")]
        public string Name { get; set; }

        [StringLength(255), DisplayName("Description")]
        public string Description { get; set; }

        public HttpPostedFileBase[] Upload { get; set; }

        [DisplayName("Filename")]
        public string FileName { get; set; }

        [DisplayName("Hash Filename")]
        public string HashFileName { get; set; }

        [DisplayName("No of Rate Table")]
        public int NoOfRateTable { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Error Message")]
        public string Errors { get; set; }

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

        public TreatyPricingRateTableGroupViewModel() { }

        public TreatyPricingRateTableGroupViewModel(TreatyPricingRateTableGroupBo treatyPricingRateTableGroupBo)
        {
            Set(treatyPricingRateTableGroupBo);
        }

        public void Set(TreatyPricingRateTableGroupBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                TreatyPricingCedantId = bo.TreatyPricingCedantId;
                TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
                Code = bo.Code;
                Name = bo.Name;
                Description = bo.Description;
                FileName = bo.FileName;
                HashFileName = bo.HashFileName;
                NoOfRateTable = bo.NoOfRateTable;
                Status = bo.Status;
                Errors = bo.Errors;
                UploadedAt = bo.UploadedAt;
                UploadedAtStr = bo.UploadedAtStr;
                UploadedById = bo.UploadedById;
                UploadedByBo = bo.UploadedByBo;
            }
        }

        public TreatyPricingRateTableGroupBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingRateTableGroupBo
            {
                Id = Id,
                TreatyPricingCedantId = TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(TreatyPricingCedantId),
                Code = Code,
                Name = Name,
                Description = Description,
                FileName = FileName,
                HashFileName = HashFileName,
                NoOfRateTable = NoOfRateTable,
                Status = Status,
                Errors = Errors,
                UploadedAt = UploadedAt,
                UploadedById = UploadedById,
                UploadedByBo = UserService.Find(UploadedById),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<TreatyPricingRateTableGroup, TreatyPricingRateTableGroupViewModel>> Expression()
        {
            return entity => new TreatyPricingRateTableGroupViewModel
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                NoOfRateTable = entity.NoOfRateTable,
                Status = entity.Status,
                Errors = entity.Errors,
                UploadedAt = entity.UploadedAt,
                UploadedById = entity.UploadedById,
                UploadedBy = entity.UploadedBy,
            };
        }
    }
}