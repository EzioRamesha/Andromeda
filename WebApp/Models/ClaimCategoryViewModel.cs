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
    public class ClaimCategoryViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), DisplayName("Category")]
        public string Category { get; set; }

        [StringLength(255), DisplayName("Remark")]
        public string Remark { get; set; }

        public ClaimCategoryViewModel() { }

        public ClaimCategoryViewModel(ClaimCategoryBo claimCategoryBo)
        {
            Set(claimCategoryBo);
        }

        public void Set(ClaimCategoryBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Category = bo.Category;
                Remark = bo.Remark;
            }
        }

        public ClaimCategoryBo FormBo(int createdById, int updatedById)
        {
            return new ClaimCategoryBo
            {
                Category = Category?.Trim(),
                Remark = Remark?.Trim(),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimCategory, ClaimCategoryViewModel>> Expression()
        {
            return entity => new ClaimCategoryViewModel
            {
                Id = entity.Id,
                Category = entity.Category,
                Remark = entity.Remark,
            };
        }
    }
}