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
    public class HipsCategoryDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(HipsCategoryDetail)),
                Controller = ModuleBo.ModuleController.HipsCategoryDetail.ToString()
            };
        }

        public static Expression<Func<HipsCategoryDetail, HipsCategoryDetailBo>> Expression()
        {
            return entity => new HipsCategoryDetailBo
            {
                Id = entity.Id,
                HipsCategoryId = entity.HipsCategoryId,
                Subcategory = entity.Subcategory,
                Description = entity.Description,
                ItemType = entity.ItemType,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static HipsCategoryDetailBo FormBo(HipsCategoryDetail entity = null)
        {
            if (entity == null)
                return null;

            return new HipsCategoryDetailBo
            {
                Id = entity.Id,
                HipsCategoryId = entity.HipsCategoryId,
                HipsCategoryBo = HipsCategoryService.Find(entity.HipsCategoryId),
                Subcategory = entity.Subcategory,
                Description = entity.Description,
                ItemType = entity.ItemType,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<HipsCategoryDetailBo> FormBos(IList<HipsCategoryDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<HipsCategoryDetailBo> bos = new List<HipsCategoryDetailBo>() { };
            foreach (HipsCategoryDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static HipsCategoryDetail FormEntity(HipsCategoryDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new HipsCategoryDetail
            {
                Id = bo.Id,
                HipsCategoryId = bo.HipsCategoryId,
                Subcategory = bo.Subcategory,
                Description = bo.Description,
                ItemType = bo.ItemType,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return HipsCategoryDetail.IsExists(id);
        }

        public static HipsCategoryDetailBo Find(int? id)
        {
            return FormBo(HipsCategoryDetail.Find(id));
        }

        public static HipsCategoryDetailBo FindBySubcategory(string subcategory)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.HipsCategoryDetails.Where(q => q.Subcategory == subcategory).FirstOrDefault());
            }
        }

        public static IList<HipsCategoryDetailBo> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.HipsCategoryDetails.OrderBy(q => q.HipsCategory.Code).ToList());
            }
        }

        public static IList<HipsCategoryDetailBo> GetByHipsCategoryId(int hipsCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.HipsCategoryDetails.Where(q => q.HipsCategoryId == hipsCategoryId).ToList());
            }
        }

        public static IList<HipsCategoryDetailBo> GetByHipsCategoryIdExcept(int hipsCategoryId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.HipsCategoryDetails.Where(q => q.HipsCategoryId == hipsCategoryId && !ids.Contains(q.Id)).ToList());
            }
        }

        public static Result Save(ref HipsCategoryDetailBo bo)
        {
            if (!HipsCategoryDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref HipsCategoryDetailBo bo, ref TrailObject trail)
        {
            if (!HipsCategoryDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref HipsCategoryDetailBo bo)
        {
            HipsCategoryDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref HipsCategoryDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref HipsCategoryDetailBo bo)
        {
            Result result = Result();

            HipsCategoryDetail entity = HipsCategoryDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.HipsCategoryId = bo.HipsCategoryId;
                entity.Subcategory = bo.Subcategory;
                entity.Description = bo.Description;
                entity.ItemType = bo.ItemType;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref HipsCategoryDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(HipsCategoryDetailBo bo)
        {
            HipsCategoryDetail.Delete(bo.Id);
        }

        public static Result Delete(HipsCategoryDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = HipsCategoryDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByHipsCategoryIdExcept(int hipsCategoryId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<HipsCategoryDetail> hipsCategoryDetails = HipsCategoryDetail.GetByHipsCategoryIdExcept(hipsCategoryId, saveIds);
            foreach (HipsCategoryDetail hipsCategoryDetail in hipsCategoryDetails)
            {
                DataTrail dataTrail = HipsCategoryDetail.Delete(hipsCategoryDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByHipsCategoryId(int hipsCategoryId)
        {
            return HipsCategoryDetail.DeleteByHipsCategoryId(hipsCategoryId);
        }

        public static void DeleteByHipsCategoryId(int hipsCategoryId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByHipsCategoryId(hipsCategoryId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(HipsCategoryDetail)));
                }
            }
        }
    }
}
