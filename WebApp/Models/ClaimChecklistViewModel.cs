using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimChecklistViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Claim Code")]
        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public ClaimCode ClaimCode { get; set; }

        public ClaimChecklistViewModel() { }

        public ClaimChecklistViewModel(ClaimChecklistBo claimChecklistBo)
        {
            Set(claimChecklistBo);
        }

        public void Set(ClaimChecklistBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                ClaimCodeId = bo.ClaimCodeId;
                ClaimCodeBo = bo.ClaimCodeBo;
            }
        }

        public ClaimChecklistBo FormBo(int createdById, int updatedById)
        {
            return new ClaimChecklistBo
            {
                ClaimCodeId = ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(ClaimCodeId),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<ClaimChecklist, ClaimChecklistViewModel>> Expression()
        {
            return entity => new ClaimChecklistViewModel
            {
                Id = entity.Id,
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCode = entity.ClaimCode,
            };
        }

        public List<ClaimChecklistDetailBo> GetClaimChecklistDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("detailMaxIndex"));
            List<ClaimChecklistDetailBo> claimChecklistDetailBos = new List<ClaimChecklistDetailBo> { };

            while (index <= maxIndex)
            {
                string name = form.Get(string.Format("name[{0}]", index))?.Trim();
                string remark = form.Get(string.Format("remark[{0}]", index))?.Trim();
                string id = form.Get(string.Format("detailId[{0}]", index));

                int detailId = 0;
                if (!string.IsNullOrEmpty(id))
                    detailId = int.Parse(id);

                if (string.IsNullOrEmpty(name) &&
                    string.IsNullOrEmpty(remark) &&
                    detailId == 0)
                {
                    index++;
                    continue;
                }

                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    result.AddError(string.Format("Claim Checklist Name is required at row #{0}", index + 1));
                }

                ClaimChecklistDetailBo claimChecklistDetailBo = new ClaimChecklistDetailBo
                {
                    ClaimChecklistId = Id,
                    Name = name,
                    NameToLower = name.ToLower(),
                    Remark = remark,
                };

                if (detailId != 0)
                {
                    claimChecklistDetailBo.Id = detailId;
                }

                claimChecklistDetailBos.Add(claimChecklistDetailBo);
                index++;
            }
            return claimChecklistDetailBos;
        }

        public void ValidateDuplicate(List<ClaimChecklistDetailBo> claimChecklistDetailBos, ref Result result)
        {
            List<ClaimChecklistDetailBo> duplicates = claimChecklistDetailBos.GroupBy(
                q => new
                {
                    q.NameToLower,
                }).Where(g => g.Count() > 1)
                .Select(r => new ClaimChecklistDetailBo
                {
                    NameToLower = r.Key.NameToLower,
                }).ToList();

            foreach (ClaimChecklistDetailBo duplicate in duplicates)
            {
                ClaimChecklistDetailBo claimChecklistDetailBo = claimChecklistDetailBos
                    .Where(q => q.NameToLower == duplicate.NameToLower)
                    .LastOrDefault();
                int idx = claimChecklistDetailBos.IndexOf(claimChecklistDetailBo);
                result.AddError(string.Format("Duplicate Claim Checklist Name at row #{0}", idx + 1));
            }
        }

        public void ProcessClaimChecklistDetails(List<ClaimChecklistDetailBo> claimChecklistDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (ClaimChecklistDetailBo bo in claimChecklistDetailBos)
            {
                ClaimChecklistDetailBo claimChecklistDetailBo = bo;
                claimChecklistDetailBo.ClaimChecklistId = Id;
                claimChecklistDetailBo.CreatedById = authUserId;
                claimChecklistDetailBo.UpdatedById = authUserId;

                ClaimChecklistDetailService.Save(ref claimChecklistDetailBo, ref trail);
                savedIds.Add(claimChecklistDetailBo.Id);
            }
            ClaimChecklistDetailService.DeleteByClaimChecklistDetailIdExcept(Id, savedIds, ref trail);
        }
    }
}