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
    public class ClaimReasonViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Type { get; set; }

        [Required, MaxLength(255), DisplayName("Reason")]
        public string Reason { get; set; }

        [MaxLength(255), DisplayName("Remark")]
        public string Remark { get; set; }

        public ClaimReasonViewModel() { }

        public ClaimReasonViewModel(ClaimReasonBo claimReasonBo)
        {
            Set(claimReasonBo);
        }

        public void Set(ClaimReasonBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Type = bo.Type;
                Reason = bo.Reason;
                Remark = bo.Remark;
            }
        }

        public ClaimReasonBo FormBo(int createdById, int updatedById)
        {
            return new ClaimReasonBo
            {
                Type = Type,
                Reason = Reason?.Trim(),
                Remark = Remark?.Trim(),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimReason, ClaimReasonViewModel>> Expression()
        {
            return entity => new ClaimReasonViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                Reason = entity.Reason,
                Remark = entity.Remark,
            };
        }
    }
}