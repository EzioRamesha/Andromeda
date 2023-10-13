using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimAuthorityLimitMLReViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Department")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        [Required, DisplayName("Name")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public UserBo UserBo { get; set; }

        [StringLength(255)]
        public string Position { get; set; }

        [Required, DisplayName("Allow Overwrite Approval")]
        public bool IsAllowOverwriteApproval { get; set; }

        public virtual ICollection<ClaimAuthorityLimitMLReDetail> ClaimAuthorityLimitMLReDetails { get; set; }

        public ClaimAuthorityLimitMLReViewModel() { }

        public ClaimAuthorityLimitMLReViewModel(ClaimAuthorityLimitMLReBo claimAuthorityLimitMLReBo)
        {
            if (claimAuthorityLimitMLReBo != null)
            {
                Id = claimAuthorityLimitMLReBo.Id;
                DepartmentId = claimAuthorityLimitMLReBo.DepartmentId;
                UserId = claimAuthorityLimitMLReBo.UserId;
                Position = claimAuthorityLimitMLReBo.Position;
                IsAllowOverwriteApproval = claimAuthorityLimitMLReBo.IsAllowOverwriteApproval;

                DepartmentBo = claimAuthorityLimitMLReBo.DepartmentBo;
                UserBo = claimAuthorityLimitMLReBo.UserBo;
            }
        }

        public ClaimAuthorityLimitMLReBo FormBo(int createdById, int updatedById)
        {
            return new ClaimAuthorityLimitMLReBo
            {
                Id = Id,
                DepartmentId = DepartmentId,
                UserId = UserId,
                Position = Position,
                IsAllowOverwriteApproval = IsAllowOverwriteApproval,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimAuthorityLimitMLRe, ClaimAuthorityLimitMLReViewModel>> Expression()
        {
            return entity => new ClaimAuthorityLimitMLReViewModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Position = entity.Position,
                IsAllowOverwriteApproval = entity.IsAllowOverwriteApproval,

                User = entity.User,
                ClaimAuthorityLimitMLReDetails = entity.ClaimAuthorityLimitMLReDetails,
            };
        }

        public List<ClaimAuthorityLimitMLReDetailBo> GetClaimAuthorityLimitMLReDetails(FormCollection form)
        {
            int index = 0;
            List<ClaimAuthorityLimitMLReDetailBo> calMLReDetailBos = new List<ClaimAuthorityLimitMLReDetailBo> { };
            while (form.AllKeys.Contains(string.Format("claimCodeId[{0}]", index)))
            {
                string claimCodeId = form.Get(string.Format("claimCodeId[{0}]", index));
                string amountLimit = form.Get(string.Format("limitValue[{0}]", index));
                string id = form.Get(string.Format("calCedantDetailId[{0}]", index));

                ClaimAuthorityLimitMLReDetailBo calMLReDetailBo = new ClaimAuthorityLimitMLReDetailBo
                {
                    ClaimAuthorityLimitMLReId = Id,
                    ClaimAuthorityLimitValueStr = amountLimit,
                };

                if (!string.IsNullOrEmpty(claimCodeId)) calMLReDetailBo.ClaimCodeId = int.Parse(claimCodeId);
                if (!string.IsNullOrEmpty(id)) calMLReDetailBo.Id = int.Parse(id);

                calMLReDetailBos.Add(calMLReDetailBo);
                index++;
            }
            return calMLReDetailBos;
        }

        public void ProcessClaimAuthorityLimitMLReDetails(List<ClaimAuthorityLimitMLReDetailBo> calCedantDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            foreach (ClaimAuthorityLimitMLReDetailBo calCedantDetailBo in calCedantDetailBos)
            {
                calCedantDetailBo.ClaimAuthorityLimitMLReId = Id;
                calCedantDetailBo.ClaimAuthorityLimitValue = (double)Util.StringToDouble(calCedantDetailBo.ClaimAuthorityLimitValueStr, true, 2);
                calCedantDetailBo.CreatedById = authUserId;
                ClaimAuthorityLimitMLReDetailService.Create(calCedantDetailBo, ref trail);

                savedIds.Add(calCedantDetailBo.Id);
            }
            ClaimAuthorityLimitMLReDetailService.DeleteByClaimAuthorityLimitMLReIdExcept(Id, savedIds, ref trail);
        }
    }
}