using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.TreatyPricing;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class TreatyPricingCedantViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [Required, DisplayName("Reinsurance Type")]
        public int ReinsuranceTypePickListDetailId { get; set; }

        public PickListDetail ReinsuranceTypePickListDetail { get; set; }

        public PickListDetailBo ReinsuranceTypePickListDetailBo { get; set; }

        [Required, DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("No of Product(s)")]
        public int NoOfProduct { get; set; }

        [DisplayName("No of DOcument(s)")]
        public int NoOfDocument { get; set; }

        public TreatyPricingCedantViewModel() { }

        public TreatyPricingCedantViewModel(TreatyPricingCedantBo treatyPricingCedantBo)
        {
            Set(treatyPricingCedantBo);
        }

        public void Set(TreatyPricingCedantBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
                ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId;
                ReinsuranceTypePickListDetailBo = bo.ReinsuranceTypePickListDetailBo;
                Code = bo.Code;
            }
        }

        public TreatyPricingCedantBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingCedantBo
            {
                Id = Id,
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                ReinsuranceTypePickListDetailId = ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetailBo = PickListDetailService.Find(ReinsuranceTypePickListDetailId),
                Code = Code,
                NoOfProduct = NoOfProduct,
                NoOfDocument = NoOfDocument,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<TreatyPricingCedant, TreatyPricingCedantViewModel>> Expression()
        {
            return entity => new TreatyPricingCedantViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetail = entity.ReinsuranceTypePickListDetail,
                Code = entity.Code,
                NoOfProduct = entity.NoOfProduct,
                NoOfDocument = entity.NoOfDocument,
            };
        }
    }
}