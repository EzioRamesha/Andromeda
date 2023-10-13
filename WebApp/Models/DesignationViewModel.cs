using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class DesignationViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), DisplayName("Designation Code")]
        public string Code { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }

        public DesignationViewModel() { }

        public DesignationViewModel(DesignationBo designationBo)
        {
            if (designationBo != null)
            {
                Id = designationBo.Id;
                Code = designationBo.Code;
                Description = designationBo.Description;
            }
        }

        public DesignationBo FormBo(int createdById, int updatedById)
        {
            return new DesignationBo
            {
                Id = Id,
                Code = Code,
                Description = Description,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Designation, DesignationViewModel>> Expression()
        {
            return entity => new DesignationViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
            };
        }
    }
}