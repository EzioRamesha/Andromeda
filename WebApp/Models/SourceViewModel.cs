using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class SourceViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Source"), StringLength(128)]
        public string Name { get; set; }

        public SourceViewModel()
        {
            Set();
        }

        public SourceViewModel(SourceBo sourceBo)
        {
            Set(sourceBo);
        }

        public void Set(SourceBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Name = bo.Name;
            }
        }

        public SourceBo FormBo(int createdById, int updatedById)
        {
            var bo = new SourceBo
            {
                Id = Id,
                Name = Name?.Trim(),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<Source, SourceViewModel>> Expression()
        {
            return entity => new SourceViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}