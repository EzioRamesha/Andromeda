using BusinessObject;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ItemCodeViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Item Code")]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [DisplayName("Reporting Type")]
        public int ReportingType { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Business Origin")]
        public int? BusinessOriginPickListDetailId { get; set; }

        public PickListDetail BusinessOriginPickListDetail { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public ItemCodeViewModel() { }

        public ItemCodeViewModel(ItemCodeBo itemCodeBo)
        {
            if (itemCodeBo != null)
            {
                Id = itemCodeBo.Id;
                Code = itemCodeBo.Code;
                ReportingType = itemCodeBo.ReportingType;
                Description = itemCodeBo.Description;
                BusinessOriginPickListDetailId = itemCodeBo.BusinessOriginPickListDetailId;
                BusinessOriginPickListDetailBo = itemCodeBo.BusinessOriginPickListDetailBo;
            }
        }

        public static Expression<Func<ItemCode, ItemCodeViewModel>> Expression()
        {
            return entity => new ItemCodeViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                ReportingType = entity.ReportingType,
                Description = entity.Description,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetail = entity.BusinessOriginPickListDetail,
            };
        }
    }
}