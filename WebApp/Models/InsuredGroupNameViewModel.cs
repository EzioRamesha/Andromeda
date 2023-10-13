using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class InsuredGroupNameViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), DisplayName("Insured Group Name")]
        public string Name { get; set; }

        [Required]
        public int Status { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public InsuredGroupNameViewModel() { }

        public InsuredGroupNameViewModel(InsuredGroupNameBo insuredGroupNameBo)
        {
            if (insuredGroupNameBo != null)
            {
                Id = insuredGroupNameBo.Id;
                Name = insuredGroupNameBo.Name;
                Status = insuredGroupNameBo.Status;
                Description = insuredGroupNameBo.Description;
            }
        }

        public InsuredGroupNameBo FormBo(int createdById, int updatedById)
        {
            return new InsuredGroupNameBo
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Status = Status,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<InsuredGroupName, InsuredGroupNameViewModel>> Expression()
        {
            return entity => new InsuredGroupNameViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                Description = entity.Description,
            };
        }
    }
}