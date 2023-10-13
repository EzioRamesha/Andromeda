using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Services.TreatyPricing;
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
    public class HipsCategoryViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), DisplayName("Code")]
        public string Code { get; set; }

        [Required, StringLength(255), DisplayName("Name")]
        public string Name { get; set; }

        public HipsCategoryViewModel() { }

        public HipsCategoryViewModel(HipsCategoryBo hipsCategoryBo)
        {
            Set(hipsCategoryBo);
        }

        public void Set(HipsCategoryBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Code = bo.Code;
                Name = bo.Name;
            }
        }

        public HipsCategoryBo FormBo(int createdById, int updatedById)
        {
            return new HipsCategoryBo
            {
                Code = Code,
                Name = Name,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<HipsCategory, HipsCategoryViewModel>> Expression()
        {
            return entity => new HipsCategoryViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
            };
        }

        public List<HipsCategoryDetailBo> GetHipsCategoryDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("hipsCategoryDetailMaxIndex"));
            List<HipsCategoryDetailBo> hipsCategoryDetailBos = new List<HipsCategoryDetailBo> { };

            while (index <= maxIndex)
            {
                string subcategory = form.Get(string.Format("subcategory[{0}]", index));
                string description = form.Get(string.Format("description[{0}]", index));
                string itemType = form.Get(string.Format("itemType[{0}]", index));
                string id = form.Get(string.Format("hipsCategoryDetailId[{0}]", index));

                int hipsCategoryDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    hipsCategoryDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(subcategory) &&
                    string.IsNullOrEmpty(description) &&
                    string.IsNullOrEmpty(itemType) &&
                    hipsCategoryDetailId == 0)
                {
                    index++;
                    continue;
                }

                HipsCategoryDetailBo hipsCategoryDetailBo = new HipsCategoryDetailBo
                {
                    HipsCategoryId = Id,
                    Subcategory = subcategory,
                    Description = description,
                    ItemType = !string.IsNullOrEmpty(itemType) ? Util.GetParseInt(itemType).Value : 0,
                };

                if (string.IsNullOrEmpty(hipsCategoryDetailBo.Subcategory))
                {
                    result.AddError(string.Format("Subcategory is required at row #{0}", index + 1));
                }

                if (string.IsNullOrEmpty(hipsCategoryDetailBo.Description))
                {
                    result.AddError(string.Format("Description is required at row #{0}", index + 1));
                }

                if (hipsCategoryDetailBo.ItemType == 0)
                {
                    result.AddError(string.Format("Item Type is required at row #{0}", index + 1));
                }

                if (hipsCategoryDetailId != 0)
                {
                    hipsCategoryDetailBo.Id = hipsCategoryDetailId;
                }

                hipsCategoryDetailBos.Add(hipsCategoryDetailBo);
                index++;
            }
            return hipsCategoryDetailBos;
        }

        public void ValidateDuplicate(List<HipsCategoryDetailBo> hipsCategoryDetailBos, ref Result result)
        {
            List<HipsCategoryDetailBo> duplicates = hipsCategoryDetailBos.GroupBy(
                q => new
                {
                    q.Subcategory
                }).Where(g => g.Count() > 1)
                .Select(r => new HipsCategoryDetailBo
                {
                    Subcategory = r.Key.Subcategory,
                }).ToList();

            foreach (HipsCategoryDetailBo duplicate in duplicates)
            {
                HipsCategoryDetailBo hipsCategoryDetailBo = hipsCategoryDetailBos
                    .Where(q => q.Subcategory == duplicate.Subcategory)
                    .LastOrDefault();
                int idx = hipsCategoryDetailBos.IndexOf(hipsCategoryDetailBo);
                result.AddError(string.Format("Duplicate Data Found at row #{0}", idx + 1));
            }
        }

        public void ProcessHipsCategoryDetails(List<HipsCategoryDetailBo> hipsCategoryDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (HipsCategoryDetailBo bo in hipsCategoryDetailBos)
            {
                HipsCategoryDetailBo hipsCategoryDetailBo = bo;
                hipsCategoryDetailBo.HipsCategoryId = Id;
                hipsCategoryDetailBo.CreatedById = authUserId;
                hipsCategoryDetailBo.UpdatedById = authUserId;

                HipsCategoryDetailService.Save(ref hipsCategoryDetailBo, ref trail);
                savedIds.Add(hipsCategoryDetailBo.Id);
            }
            HipsCategoryDetailService.DeleteByHipsCategoryIdExcept(Id, savedIds, ref trail);
        }
    }
}