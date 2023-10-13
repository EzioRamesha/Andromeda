using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities;
using DataAccess.Entities.Sanctions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class SanctionExclusionViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Keyword"), StringLength(255)]
        public string Keyword { get; set; }

        public SanctionExclusionViewModel()
        {
            Set();
        }

        public SanctionExclusionViewModel(SanctionExclusionBo sanctionExclusionBo)
        {
            Set(sanctionExclusionBo);
        }

        public void Set(SanctionExclusionBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Keyword = bo.Keyword;
            }
        }

        public SanctionExclusionBo FormBo(int createdById, int updatedById)
        {
            var bo = new SanctionExclusionBo
            {
                Id = Id,
                Keyword = Keyword?.Trim(),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<SanctionExclusion, SanctionExclusionViewModel>> Expression()
        {
            return entity => new SanctionExclusionViewModel
            {
                Id = entity.Id,
                Keyword = entity.Keyword,
            };
        }
    }
}