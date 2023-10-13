using BusinessObject;
using DataAccess.Entities;
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
    public class ClaimAuthorityLimitCedantViewModel
    {
        public int Id { get; set; }

        [ValidateCedantId]
        [DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        [DisplayName("Ceding Company")]
        public virtual Cedant Cedant { get; set; }

        [DisplayName("Ceding Company")]
        public CedantBo CedantBo { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        public virtual ICollection<ClaimAuthorityLimitCedantDetail> ClaimAuthorityLimitCedantDetails { get; set; }

        public ClaimAuthorityLimitCedantViewModel() { }

        public ClaimAuthorityLimitCedantViewModel(ClaimAuthorityLimitCedantBo claimAuthorityLimitCedantBo)
        {
            if (claimAuthorityLimitCedantBo != null)
            {
                Id = claimAuthorityLimitCedantBo.Id;
                CedantId = claimAuthorityLimitCedantBo.CedantId;
                Remarks = claimAuthorityLimitCedantBo.Remarks;

                CedantBo = claimAuthorityLimitCedantBo.CedantBo;
            }
        }

        public ClaimAuthorityLimitCedantBo FormBo(int createdById, int updatedById)
        {
            return new ClaimAuthorityLimitCedantBo
            {
                Id = Id,
                CedantId = CedantId,
                Remarks = Remarks,
                CedantBo = CedantService.Find(CedantId),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimAuthorityLimitCedant, ClaimAuthorityLimitCedantViewModel>> Expression()
        {
            return entity => new ClaimAuthorityLimitCedantViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                Remarks = entity.Remarks,

                ClaimAuthorityLimitCedantDetails = entity.ClaimAuthorityLimitCedantDetails,
            };
        }

        public List<ClaimAuthorityLimitCedantDetailBo> GetClaimAuthorityLimitCedantDetails(FormCollection form)
        {
            int index = 0;
            List<ClaimAuthorityLimitCedantDetailBo> calCedantDetailBos = new List<ClaimAuthorityLimitCedantDetailBo> { };
            while (form.AllKeys.Contains(string.Format("claimCodeId[{0}]", index)))
            {
                string claimCodeId = form.Get(string.Format("claimCodeId[{0}]", index));
                string type = form.Get(string.Format("type[{0}]", index));
                string amountLimit = form.Get(string.Format("limitValue[{0}]", index));
                string id = form.Get(string.Format("calCedantDetailId[{0}]", index));
                string fundAcctCode = form.Get(string.Format("fundAcctCode[{0}]", index));
                string effectiveDate = form.Get(string.Format("effectiveDate[{0}]", index));

                ClaimAuthorityLimitCedantDetailBo calCedantDetailBo = new ClaimAuthorityLimitCedantDetailBo
                {
                    ClaimAuthorityLimitCedantId = Id,
                    ClaimAuthorityLimitValueStr = amountLimit,
                    EffectiveDateStr = effectiveDate,
                };

                if (!string.IsNullOrEmpty(claimCodeId)) calCedantDetailBo.ClaimCodeId = int.Parse(claimCodeId);
                if (!string.IsNullOrEmpty(type)) calCedantDetailBo.Type = int.Parse(type);
                if (!string.IsNullOrEmpty(fundAcctCode)) calCedantDetailBo.FundsAccountingTypePickListDetailId = int.Parse(fundAcctCode);
                if (!string.IsNullOrEmpty(id)) calCedantDetailBo.Id = int.Parse(id);

                calCedantDetailBos.Add(calCedantDetailBo);
                index++;
            }
            return calCedantDetailBos;
        }

        public void ProcessClaimAuthorityLimitCedantDetails(List<ClaimAuthorityLimitCedantDetailBo> calCedantDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            foreach (ClaimAuthorityLimitCedantDetailBo calCedantDetailBo in calCedantDetailBos)
            {
                calCedantDetailBo.ClaimAuthorityLimitCedantId = Id;
                calCedantDetailBo.ClaimAuthorityLimitValue = (double)Util.StringToDouble(calCedantDetailBo.ClaimAuthorityLimitValueStr, true, 2);
                calCedantDetailBo.EffectiveDate = DateTime.Parse(calCedantDetailBo.EffectiveDateStr);
                calCedantDetailBo.CreatedById = authUserId;
                ClaimAuthorityLimitCedantDetailService.Create(calCedantDetailBo, ref trail);

                savedIds.Add(calCedantDetailBo.Id);
            }
            ClaimAuthorityLimitCedantDetailService.DeleteByClaimAuthorityLimitCedantIdExcept(Id, savedIds, ref trail);
        }
    }
}