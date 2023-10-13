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
    public class TreatyPricingCampaignProductService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCampaignProduct)),
            };
        }

        public static TreatyPricingCampaignProductBo FormBo(TreatyPricingCampaignProduct entity = null, bool loadProduct = true, bool loadCampaign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCampaignProductBo
            {
                TreatyPricingCampaignId = entity.TreatyPricingCampaignId,
                TreatyPricingCampaignBo = loadCampaign ? TreatyPricingCampaignService.Find(entity.TreatyPricingCampaignId) : null,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                TreatyPricingProductBo = loadProduct ? TreatyPricingProductService.Find(entity.TreatyPricingProductId) : null,
            };
        }

        public static TreatyPricingCampaignProductBo FormSimplifiedBo(TreatyPricingCampaignProduct entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCampaignProductBo
            {
                TreatyPricingCampaignId = entity.TreatyPricingCampaignId,
                TreatyPricingProductId = entity.TreatyPricingProductId,
            };
        }

        public static IList<TreatyPricingCampaignProductBo> FormBos(IList<TreatyPricingCampaignProduct> entities = null, bool loadProduct = true, bool loadCampaign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCampaignProductBo> bos = new List<TreatyPricingCampaignProductBo>() { };
            foreach (TreatyPricingCampaignProduct entity in entities)
            {
                bos.Add(FormBo(entity, loadProduct, loadCampaign));
            }
            return bos;
        }

        public static IList<TreatyPricingCampaignProductBo> FormSimplifiedBos(IList<TreatyPricingCampaignProduct> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCampaignProductBo> bos = new List<TreatyPricingCampaignProductBo>() { };
            foreach (TreatyPricingCampaignProduct entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static TreatyPricingCampaignProduct FormEntity(TreatyPricingCampaignProductBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCampaignProduct
            {
                TreatyPricingCampaignId = bo.TreatyPricingCampaignId,
                TreatyPricingProductId = bo.TreatyPricingProductId,
            };
        }

        public static bool IsExists(int campaignId, int productId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignProducts
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Where(q => q.TreatyPricingCampaignId == campaignId)
                    .Any();
            }
        }

        public static TreatyPricingCampaignProductBo Find(int treatyPricingCampaignId, int treatyPricingProductId)
        {
            return FormBo(TreatyPricingCampaignProduct.Find(treatyPricingCampaignId, treatyPricingProductId));
        }

        public static IList<TreatyPricingCampaignProductBo> GetByTreatyPricingCampaignId(int treatyPricingCampaignId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingCampaignProducts
                    .Where(q => q.TreatyPricingCampaignId == treatyPricingCampaignId)
                    .OrderBy(q => q.TreatyPricingProductId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingCampaignProductBo> GetByTreatyPricingProductId(int productId)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.TreatyPricingCampaignProducts.Where(q => q.TreatyPricingProductId == productId);

                return FormBos(query.ToList(), false, true);
            }
        }

        public static List<string> GetCodeByProductName(string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaignProducts
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .GroupBy(q => q.TreatyPricingCampaignId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingCampaign.Code)
                    .ToList();
            }
        }

        public static IList<TreatyPricingProductBo> GetProductBySearchParams(int campaignId, int? treatyPricingCedantId, int? productTypeId, string targetSegments, string distributionChannels, string underwritingMethods, string productIdName, string quotationName)
        {
            using (var db = new AppDbContext())
            {
                var searchQuery = TreatyPricingProductVersionService.QueryByParam(db, treatyPricingCedantId, productTypeId, targetSegments, distributionChannels, underwritingMethods, productIdName, quotationName);
                var exceptionQuery = db.TreatyPricingCampaignProducts.Where(q => q.TreatyPricingCampaignId == campaignId).Select(q => q.TreatyPricingProductId);

                return TreatyPricingProductService.LinkFormBos(db.TreatyPricingProducts.Where(q => searchQuery.Contains(q.Id) && !exceptionQuery.Contains(q.Id)).ToList());
            }
        }

        public static Result Create(ref TreatyPricingCampaignProductBo bo)
        {
            TreatyPricingCampaignProduct entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref TreatyPricingCampaignProductBo bo, AppDbContext db)
        {
            TreatyPricingCampaignProduct entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref TreatyPricingCampaignProductBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static void Delete(TreatyPricingCampaignProductBo bo)
        {
            TreatyPricingCampaignProduct.Delete(bo.TreatyPricingCampaignId, bo.TreatyPricingProductId);
        }

        public static Result Delete(TreatyPricingCampaignProductBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCampaignProduct.Delete(bo.TreatyPricingCampaignId, bo.TreatyPricingProductId);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCampaignId(int treatyPricingCampaignId)
        {
            return TreatyPricingCampaignProduct.DeleteAllByTreatyPricingCampaignId(treatyPricingCampaignId);
        }

        public static void DeleteAllByTreatyPricingCampaignId(int treatyPricingCampaignId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCampaignId(treatyPricingCampaignId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingCampaignProduct)));
                }
            }
        }
    }
}
