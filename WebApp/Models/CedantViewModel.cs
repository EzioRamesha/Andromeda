using BusinessObject;
using DataAccess.Entities;
using Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class CedantViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ceding Company Type")]
        public int? CedingCompanyTypePickListDetailId { get; set; }

        [Display(Name = "Ceding Company Type")]
        public virtual PickListDetail CedingCompanyTypePickListDetail { get; set; }

        [Display(Name = "Ceding Company Type")]
        public PickListDetailBo CedingCompanyTypePickListDetailBo { get; set; }

        [Required]
        [StringLength(128)]
        [DisplayName("Ceding Company Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Ceding Company Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Party Code")]
        public string PartyCode { get; set; }

        [Required]
        public int Status { get; set; }

        public string Remarks { get; set; }

        [StringLength(10)]
        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

        public CedantViewModel() { }

        public CedantViewModel(CedantBo cedantBo)
        {
            if (cedantBo != null)
            {
                Id = cedantBo.Id;
                CedingCompanyTypePickListDetailId = cedantBo.CedingCompanyTypePickListDetailId;
                Name = cedantBo.Name;
                Code = cedantBo.Code;
                PartyCode = cedantBo.PartyCode;
                Status = cedantBo.Status;
                Remarks = cedantBo.Remarks;
                AccountCode = cedantBo.AccountCode;

                CedingCompanyTypePickListDetailBo = cedantBo.CedingCompanyTypePickListDetailBo;
            }
        }

        public CedantBo FormBo(int createdById, int updatedById)
        {
            return new CedantBo
            {
                Id = Id,
                CedingCompanyTypePickListDetailId = CedingCompanyTypePickListDetailId,
                CedingCompanyTypePickListDetailBo = PickListDetailService.Find(CedingCompanyTypePickListDetailId),
                Name = Name?.Trim(),
                Code = Code?.Trim(),
                PartyCode = PartyCode?.Trim(),
                Status = Status,
                Remarks = Remarks?.Trim(),
                AccountCode = AccountCode?.Trim(),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Cedant, CedantViewModel>> Expression()
        {
            return entity => new CedantViewModel
            {
                Id = entity.Id,
                CedingCompanyTypePickListDetailId = entity.CedingCompanyTypePickListDetailId,
                CedingCompanyTypePickListDetail = entity.CedingCompanyTypePickListDetail,
                Name = entity.Name,
                Code = entity.Code,
                PartyCode = entity.PartyCode,
                Status = entity.Status,
                Remarks = entity.Remarks,
            };
        }
    }
}