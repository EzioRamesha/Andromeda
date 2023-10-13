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

namespace Services.TreatyPricing
{
    public class TreatyPricingCustomOtherProductService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCustomOtherProduct)),
            };
        }

        public static TreatyPricingCustomOtherProductBo FormBo(TreatyPricingCustomOtherProduct entity = null, bool loadProduct = true, bool loadCustomOther = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCustomOtherProductBo
            {
                TreatyPricingCustomOtherId = entity.TreatyPricingCustomOtherId,
                TreatyPricingCustomOtherBo = loadCustomOther ? TreatyPricingCustomOtherService.Find(entity.TreatyPricingCustomOtherId) : null,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                TreatyPricingProductBo = loadProduct ? TreatyPricingProductService.Find(entity.TreatyPricingProductId) : null,
            };
        }

        public static TreatyPricingCustomOtherProductBo FormSimplifiedBo(TreatyPricingCustomOtherProduct entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCustomOtherProductBo
            {
                TreatyPricingCustomOtherId = entity.TreatyPricingCustomOtherId,
                TreatyPricingProductId = entity.TreatyPricingProductId,
            };
        }

        public static IList<TreatyPricingCustomOtherProductBo> FormBos(IList<TreatyPricingCustomOtherProduct> entities = null, bool loadProduct = true, bool loadCustomOther = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCustomOtherProductBo> bos = new List<TreatyPricingCustomOtherProductBo>() { };
            foreach (TreatyPricingCustomOtherProduct entity in entities)
            {
                bos.Add(FormBo(entity, loadProduct, loadCustomOther));
            }
            return bos;
        }

        public static IList<TreatyPricingCustomOtherProductBo> FormSimplifiedBos(IList<TreatyPricingCustomOtherProduct> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCustomOtherProductBo> bos = new List<TreatyPricingCustomOtherProductBo>() { };
            foreach (TreatyPricingCustomOtherProduct entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static TreatyPricingCustomOtherProduct FormEntity(TreatyPricingCustomOtherProductBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCustomOtherProduct
            {
                TreatyPricingCustomOtherId = bo.TreatyPricingCustomOtherId,
                TreatyPricingProductId = bo.TreatyPricingProductId,
            };
        }

        public static bool IsExists(int customOtherId, int productId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherProducts
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Where(q => q.TreatyPricingCustomOtherId == customOtherId)
                    .Any();
            }
        }

        public static TreatyPricingCustomOtherProductBo Find(int treatyPricingCustomOtherId, int treatyPricingProductId)
        {
            return FormBo(TreatyPricingCustomOtherProduct.Find(treatyPricingCustomOtherId, treatyPricingProductId));
        }

        public static IList<TreatyPricingCustomOtherProductBo> GetByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingCustomOtherProducts
                    .Where(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId)
                    .OrderBy(q => q.TreatyPricingProductId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingCustomOtherProductBo> GetByTreatyPricingProductId(int productId)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.TreatyPricingCustomOtherProducts.Where(q => q.TreatyPricingProductId == productId);

                return FormBos(query.ToList(), false, true);
            }
        }

        public static List<string> GetCodeByProductName(string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCustomOtherProducts
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .GroupBy(q => q.TreatyPricingCustomOtherId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingCustomOther.Code)
                    .ToList();
            }
        }

        public static IList<TreatyPricingProductBo> GetProductBySearchParams(int customOtherId, int? treatyPricingCedantId, int? productTypeId, string targetSegments, string distributionChannels, string underwritingMethods, string productIdName, string quotationName)
        {
            using (var db = new AppDbContext())
            {
                var searchQuery = TreatyPricingProductVersionService.QueryByParam(db, treatyPricingCedantId, productTypeId, targetSegments, distributionChannels, underwritingMethods, productIdName, quotationName);
                var exceptionQuery = db.TreatyPricingCustomOtherProducts.Where(q => q.TreatyPricingCustomOtherId == customOtherId).Select(q => q.TreatyPricingProductId);

                return TreatyPricingProductService.LinkFormBos(db.TreatyPricingProducts.Where(q => searchQuery.Contains(q.Id) && !exceptionQuery.Contains(q.Id)).ToList());
            }
        }

        public static Result Create(ref TreatyPricingCustomOtherProductBo bo)
        {
            TreatyPricingCustomOtherProduct entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref TreatyPricingCustomOtherProductBo bo, AppDbContext db)
        {
            TreatyPricingCustomOtherProduct entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref TreatyPricingCustomOtherProductBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static void Delete(TreatyPricingCustomOtherProductBo bo)
        {
            TreatyPricingCustomOtherProduct.Delete(bo.TreatyPricingCustomOtherId, bo.TreatyPricingProductId);
        }

        public static Result Delete(TreatyPricingCustomOtherProductBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCustomOtherProduct.Delete(bo.TreatyPricingCustomOtherId, bo.TreatyPricingProductId);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            return TreatyPricingCustomOtherProduct.DeleteAllByTreatyPricingCustomOtherId(treatyPricingCustomOtherId);
        }

        public static void DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCustomOtherId(treatyPricingCustomOtherId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingCustomOtherProduct)));
                }
            }
        }
    }
}
