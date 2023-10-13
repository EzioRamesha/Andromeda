using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
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

namespace Services.TreatyPricing
{
    public class HipsCategoryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(HipsCategory)),
                Controller = ModuleBo.ModuleController.HipsCategory.ToString()
            };
        }

        public static Expression<Func<HipsCategory, HipsCategoryBo>> Expression()
        {
            return entity => new HipsCategoryBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static HipsCategoryBo FormBo(HipsCategory entity = null)
        {
            if (entity == null)
                return null;
            return new HipsCategoryBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<HipsCategoryBo> FormBos(IList<HipsCategory> entities = null)
        {
            if (entities == null)
                return null;
            IList<HipsCategoryBo> bos = new List<HipsCategoryBo>() { };
            foreach (HipsCategory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static HipsCategory FormEntity(HipsCategoryBo bo = null)
        {
            if (bo == null)
                return null;
            return new HipsCategory
            {
                Id = bo.Id,
                Code = bo.Code,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return HipsCategory.IsExists(id);
        }

        public static HipsCategoryBo Find(int? id)
        {
            return FormBo(HipsCategory.Find(id));
        }

        public static HipsCategoryBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.HipsCategories.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static HipsCategoryBo FindByName(string name)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.HipsCategories.Where(q => q.Name.Contains(name)).FirstOrDefault());
            }
        }

        public static IList<HipsCategoryBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.HipsCategories.OrderBy(q => q.Code).ToList());
            }
        }

        public static Result Save(ref HipsCategoryBo bo)
        {
            if (!HipsCategory.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref HipsCategoryBo bo, ref TrailObject trail)
        {
            if (!HipsCategory.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(HipsCategory hipsCategory)
        {
            return hipsCategory.IsDuplicateCode();
        }

        public static Result Create(ref HipsCategoryBo bo)
        {
            HipsCategory entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref HipsCategoryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref HipsCategoryBo bo)
        {
            Result result = Result();

            HipsCategory entity = HipsCategory.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref HipsCategoryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(HipsCategoryBo bo)
        {
            HipsCategoryDetailService.DeleteByHipsCategoryId(bo.Id);
            HipsCategory.Delete(bo.Id);
        }

        public static Result Delete(HipsCategoryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (TreatyPricingGroupReferralHipsTableService.CountByHipsCategoryId(bo.Id) > 0 ||
                TreatyPricingGroupReferralHipsTableService.CountHipsSubCategoryByHipsCategoryId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                HipsCategoryDetailService.DeleteByHipsCategoryId(bo.Id, ref trail);
                DataTrail dataTrail = HipsCategory.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
