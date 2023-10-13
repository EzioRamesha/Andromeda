using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProductDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductDetail.ToString()
            };
        }

        public static TreatyPricingProductDetailBo FormBo(TreatyPricingProductDetail entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProductDetailBo
            {
                Id = entity.Id,
                TreatyPricingProductVersionId = entity.TreatyPricingProductVersionId,
                Type = entity.Type,
                Col1 = entity.Col1,
                Col2 = entity.Col2,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingProductDetailBo> FormBos(IList<TreatyPricingProductDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductDetailBo> bos = new List<TreatyPricingProductDetailBo>() { };
            foreach (TreatyPricingProductDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingProductDetail FormEntity(TreatyPricingProductDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProductDetail
            {
                Id = bo.Id,
                TreatyPricingProductVersionId = bo.TreatyPricingProductVersionId,
                Type = bo.Type,
                Col1 = bo.Col1,
                Col2 = bo.Col2,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProductDetail.IsExists(id);
        }

        public static TreatyPricingProductDetailBo Find(int? id)
        {
            return FormBo(TreatyPricingProductDetail.Find(id));
        }

        public static IList<TreatyPricingProductDetailBo> GetByParentType(int parentId, int type, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductDetails
                    .Where(q => q.TreatyPricingProductVersionId == parentId)
                    .Where(q => q.Type == type);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static string GetJsonByParentType(int parentId, int type)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByParentType(parentId, type);

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByParentType(int parentId, int type)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductDetails
                    .Where(q => q.TreatyPricingProductVersionId == parentId)
                    .Where(q => q.Type == type)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingProductDetailBo bo)
        {
            if (!TreatyPricingProductDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingProductDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingProductDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProductDetailBo bo)
        {
            TreatyPricingProductDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductDetailBo bo)
        {
            Result result = Result();

            TreatyPricingProductDetail entity = TreatyPricingProductDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingProductVersionId = bo.TreatyPricingProductVersionId;
                entity.Type = bo.Type;
                entity.Col1 = bo.Col1;
                entity.Col2 = bo.Col2;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductDetailBo bo)
        {
            TreatyPricingProductDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProductDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByParentExcept(int parentId, int type, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByParentType(parentId, type, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
