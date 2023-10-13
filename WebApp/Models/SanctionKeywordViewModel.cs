using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using Services.Sanctions;
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
    public class SanctionKeywordViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Group"), StringLength(10)]
        public string Code { get; set; }

        public virtual ICollection<SanctionKeywordDetail> SanctionKeywordDetails { get; set; }

        public SanctionKeywordViewModel()
        {
            Set();
        }

        public SanctionKeywordViewModel(SanctionKeywordBo sanctionKeywordBo)
        {
            Set(sanctionKeywordBo);
        }

        public void Set(SanctionKeywordBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Code = bo.Code;
            }
        }

        public SanctionKeywordBo FormBo(int createdById, int updatedById)
        {
            var bo = new SanctionKeywordBo
            {
                Id = Id,
                Code = Code?.Trim(),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<SanctionKeyword, SanctionKeywordViewModel>> Expression()
        {
            return entity => new SanctionKeywordViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                SanctionKeywordDetails = entity.SanctionKeywordDetails,
            };
        }

        public List<SanctionKeywordDetailBo> GetSanctionKeywordDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("detailMaxIndex"));
            List<SanctionKeywordDetailBo> sanctionKeywordBos = new List<SanctionKeywordDetailBo> { };

            while (index <= maxIndex)
            {
                string keyword = form.Get(string.Format("keyword[{0}]", index))?.Trim();
                string id = form.Get(string.Format("detailId[{0}]", index));

                int detailId = 0;
                if (!string.IsNullOrEmpty(id))
                    detailId = int.Parse(id);

                if (string.IsNullOrEmpty(keyword) &&
                    detailId == 0)
                {
                    index++;
                    continue;
                }

                SanctionKeywordDetailBo sanctionKeywordDetailBo = new SanctionKeywordDetailBo
                {
                    SanctionKeywordId = Id,
                    Keyword = keyword,
                };

                if (string.IsNullOrEmpty(keyword) && detailId != 0)
                {
                    result.AddError(string.Format("Keyword is required at row #{0}", index + 1));
                }

                if (detailId != 0)
                {
                    sanctionKeywordDetailBo.Id = detailId;
                }

                sanctionKeywordBos.Add(sanctionKeywordDetailBo);
                index++;
            }

            if (sanctionKeywordBos.IsNullOrEmpty())
            {
                result.AddError("At least one Keyword is required");
            }

            return sanctionKeywordBos;
        }

        public void ValidateDuplicate(List<SanctionKeywordDetailBo> sanctionKeywordDetailBos, ref Result result)
        {
            List<SanctionKeywordDetailBo> duplicates = sanctionKeywordDetailBos.GroupBy(
                q => new { Keyword = q.Keyword.ToLower() })
                .Where(g => g.Count() > 1)
                .Select(r => new SanctionKeywordDetailBo
                {
                    Keyword = r.Key.Keyword,
                }).ToList();

            foreach (SanctionKeywordDetailBo duplicate in duplicates)
            {
                SanctionKeywordDetailBo sanctionKeywordDetailBo = sanctionKeywordDetailBos
                    .Where(q => q.Keyword.ToLower() == duplicate.Keyword)
                    .LastOrDefault();
                int idx = sanctionKeywordDetailBos.IndexOf(sanctionKeywordDetailBo);
                result.AddError(string.Format("Duplicate Keyword Found at row #{0}", idx + 1));
            }

            if (result.Valid)
            {
                foreach(SanctionKeywordDetailBo bo in sanctionKeywordDetailBos)
                {
                    if (SanctionKeywordDetailService.CountByKeywordExceptSanctionKeywordId(bo.Keyword, Id) > 0)
                    {
                        int idx = sanctionKeywordDetailBos.IndexOf(bo);
                        result.AddError(string.Format("Keyword:\"{0}\" at row #{1} Found in other group", bo.Keyword, idx + 1));
                    }
                }
            }
        }

        public void ProcessSanctionKeywordDetails(List<SanctionKeywordDetailBo> sanctionKeywordDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (SanctionKeywordDetailBo bo in sanctionKeywordDetailBos)
            {
                SanctionKeywordDetailBo sanctionKeywordDetailBo = bo;
                sanctionKeywordDetailBo.SanctionKeywordId = Id;
                sanctionKeywordDetailBo.CreatedById = authUserId;
                sanctionKeywordDetailBo.UpdatedById = authUserId;

                SanctionKeywordDetailService.Save(ref sanctionKeywordDetailBo, ref trail);
                savedIds.Add(sanctionKeywordDetailBo.Id);
            }
            SanctionKeywordDetailService.DeleteBySanctionKeywordIdExcept(Id, savedIds, ref trail);
        }
    }
}