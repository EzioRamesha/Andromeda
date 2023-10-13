using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingPerLifeRetroProductService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroProduct)),
            };
        }

        public static TreatyPricingPerLifeRetroProductBo FormBo(TreatyPricingPerLifeRetroProduct entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingPerLifeRetroProductBo
            {
                TreatyPricingPerLifeRetroId = entity.TreatyPricingPerLifeRetroId,
                //TreatyPricingCampaignBo = TreatyPricingCampaignService.Find(entity.TreatyPricingCampaignId),
                TreatyPricingProductId = entity.TreatyPricingProductId,
                TreatyPricingProductBo = TreatyPricingProductService.Find(entity.TreatyPricingProductId),
            };
        }

        public static TreatyPricingPerLifeRetroProductBo FormSimplifiedBo(TreatyPricingPerLifeRetroProduct entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingPerLifeRetroProductBo
            {
                TreatyPricingPerLifeRetroId = entity.TreatyPricingPerLifeRetroId,
                TreatyPricingProductId = entity.TreatyPricingProductId,
            };
        }

        public static IList<TreatyPricingPerLifeRetroProductBo> FormBos(IList<TreatyPricingPerLifeRetroProduct> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroProductBo> bos = new List<TreatyPricingPerLifeRetroProductBo>() { };
            foreach (TreatyPricingPerLifeRetroProduct entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<TreatyPricingPerLifeRetroProductBo> FormSimplifiedBos(IList<TreatyPricingPerLifeRetroProduct> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroProductBo> bos = new List<TreatyPricingPerLifeRetroProductBo>() { };
            foreach (TreatyPricingPerLifeRetroProduct entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetroProduct FormEntity(TreatyPricingPerLifeRetroProductBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetroProduct
            {
                TreatyPricingPerLifeRetroId = bo.TreatyPricingPerLifeRetroId,
                TreatyPricingProductId = bo.TreatyPricingProductId,
            };
        }

        public static TreatyPricingPerLifeRetroProductBo Find(int treatyPricingPerLifeRetroId, int treatyPricingProductId)
        {
            return FormBo(TreatyPricingPerLifeRetroProduct.Find(treatyPricingPerLifeRetroId, treatyPricingProductId));
        }

        public static IList<TreatyPricingPerLifeRetroProductBo> GetByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingPerLifeRetroProducts
                    .Where(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId)
                    .OrderBy(q => q.TreatyPricingProductId)
                    .ToList());
            }
        }

        public static Result Create(ref TreatyPricingPerLifeRetroProductBo bo)
        {
            TreatyPricingPerLifeRetroProduct entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref TreatyPricingPerLifeRetroProductBo bo, AppDbContext db)
        {
            TreatyPricingPerLifeRetroProduct entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref TreatyPricingPerLifeRetroProductBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId)
        {
            return TreatyPricingPerLifeRetroProduct.DeleteAllByTreatyPricingPerLifeRetroId(treatyPricingPerLifeRetroId);
        }

        public static void DeleteAllByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingPerLifeRetroId(treatyPricingPerLifeRetroId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroProduct)));
                }
            }
        }
    }
}
