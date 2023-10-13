using BusinessObject;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ItemCodeMappingViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Item Code")]
        public int ItemCodeId { get; set; }

        public virtual ItemCode ItemCode { get; set; }

        public ItemCodeBo ItemCodeBo { get; set; }

        [Required, Display(Name = "Invoice Field")]
        public int? InvoiceFieldPickListDetailId { get; set; }

        public virtual PickListDetail InvoiceFieldPickListDetail { get; set; }

        public PickListDetailBo InvoiceFieldPickListDetailBo { get; set; }

        [Required, Display(Name = "Treaty Type")]
        [ValidateTreatyType]
        public string TreatyType { get; set; }

        [Display(Name = "Treaty Code")]
        [ValidateTreatyCode]
        public string TreatyCode { get; set; }

        [Required, Display(Name = "Business Origin")]
        public int? BusinessOriginPickListDetailId { get; set; }

        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public virtual ICollection<ItemCodeMappingDetail> ItemCodeMappingDetails { get; set; }

        public int ReportingType { get; set; }

        public ItemCodeMappingViewModel() { }

        public ItemCodeMappingViewModel(ItemCodeMappingBo itemCodeMappingBo)
        {
            if (itemCodeMappingBo != null)
            {
                Id = itemCodeMappingBo.Id;
                ItemCodeId = itemCodeMappingBo.ItemCodeId;
                InvoiceFieldPickListDetailId = itemCodeMappingBo.InvoiceFieldPickListDetailId;
                BusinessOriginPickListDetailId = itemCodeMappingBo.BusinessOriginPickListDetailId;
                TreatyType = itemCodeMappingBo.TreatyType;
                TreatyCode = itemCodeMappingBo.TreatyCode;

                ItemCodeBo = itemCodeMappingBo.ItemCodeBo;
                InvoiceFieldPickListDetailBo = itemCodeMappingBo.InvoiceFieldPickListDetailBo;
                BusinessOriginPickListDetailBo = itemCodeMappingBo.BusinessOriginPickListDetailBo;
            }
        }

        public ItemCodeMappingBo FormBo(int createdById, int updatedById)
        {
            var bo = new ItemCodeMappingBo
            {
                Id = Id,
                ItemCodeId = ItemCodeId,
                InvoiceFieldPickListDetailId = InvoiceFieldPickListDetailId,
                TreatyType = TreatyType,
                TreatyCode = TreatyCode,
                BusinessOriginPickListDetailId = BusinessOriginPickListDetailId,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<ItemCodeMapping, ItemCodeMappingViewModel>> Expression()
        {
            return entity => new ItemCodeMappingViewModel
            {
                Id = entity.Id,
                ItemCodeId = entity.ItemCodeId,
                ItemCode = entity.ItemCode,
                TreatyType = entity.TreatyType,
                TreatyCode = entity.TreatyCode,
                InvoiceFieldPickListDetailId = entity.InvoiceFieldPickListDetailId,
                InvoiceFieldPickListDetail = entity.InvoiceFieldPickListDetail,
                ItemCodeMappingDetails = entity.ItemCodeMappingDetails,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetail = entity.BusinessOriginPickListDetail,
                ReportingType = entity.ItemCode.ReportingType,
            };
        }
    }
}