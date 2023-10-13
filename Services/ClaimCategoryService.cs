using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClaimCategoryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimCategory)),
                Controller = ModuleBo.ModuleController.ClaimCategory.ToString()
            };
        }

        public static Expression<Func<ClaimCategory, ClaimCategoryBo>> Expression()
        {
            return entity => new ClaimCategoryBo
            {
                Id = entity.Id,
                Category = entity.Category,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimCategoryBo FormBo(ClaimCategory entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimCategoryBo
            {
                Id = entity.Id,
                Category = entity.Category,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimCategoryBo> FormBos(IList<ClaimCategory> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimCategoryBo> bos = new List<ClaimCategoryBo>() { };
            foreach (ClaimCategory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimCategory FormEntity(ClaimCategoryBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimCategory
            {
                Id = bo.Id,
                Category = bo.Category?.Trim(),
                Remark = bo.Remark?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimCategory.IsExists(id);
        }

        public static ClaimCategoryBo Find(int id)
        {
            return FormBo(ClaimCategory.Find(id));
        }

        public static ClaimCategoryBo Find(int? id)
        {
            if (id.HasValue)
                return Find(id.Value);
            return null;
        }

        public static IList<ClaimCategoryBo> Get()
        {
            return FormBos(ClaimCategory.Get());
        }

        public static Result Save(ref ClaimCategoryBo bo)
        {
            if (!ClaimCategory.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimCategoryBo bo, ref TrailObject trail)
        {
            if (!ClaimCategory.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCategory(ClaimCategory claimCategory)
        {
            return claimCategory.IsDuplicateCategory();
        }

        public static Result Create(ref ClaimCategoryBo bo)
        {
            ClaimCategory entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCategory(entity))
            {
                result.AddTakenError("Category", bo.Category);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimCategoryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimCategoryBo bo)
        {
            Result result = Result();

            ClaimCategory entity = ClaimCategory.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCategory(FormEntity(bo)))
            {
                result.AddTakenError("Category", bo.Category);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Category = bo.Category;
                entity.Remark = bo.Remark;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimCategoryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimCategoryBo bo)
        {
            ClaimCategory.Delete(bo.Id);
        }

        public static Result Delete(ClaimCategoryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                ReferralClaimService.CountByClaimCategoryId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = ClaimCategory.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
