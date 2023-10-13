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
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductPerLifeRetroService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProductPerLifeRetro)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductPerLifeRetro.ToString()
            };
        }

        public static TreatyPricingProductPerLifeRetroBo FormBo(TreatyPricingProductPerLifeRetro entity = null, bool loadProduct = false)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProductPerLifeRetroBo
            {
                Id = entity.Id,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                TreatyPricingProductBo = loadProduct ? TreatyPricingProductService.Find(entity.TreatyPricingProductId) : null,
                TreatyPricingPerLifeRetroId = entity.TreatyPricingPerLifeRetroId,
                TreatyPricingPerLifeRetroCode = TreatyPricingPerLifeRetroService.Find(entity.TreatyPricingPerLifeRetroId)?.Code,
                Warning = GetWarning(entity.TreatyPricingProductId, entity.TreatyPricingPerLifeRetroId),
                CreatedById = entity.CreatedById,
            };
        }

        public static IList<TreatyPricingProductPerLifeRetroBo> FormBos(IList<TreatyPricingProductPerLifeRetro> entities = null, bool loadProduct = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductPerLifeRetroBo> bos = new List<TreatyPricingProductPerLifeRetroBo>() { };
            foreach (TreatyPricingProductPerLifeRetro entity in entities)
            {
                bos.Add(FormBo(entity, loadProduct));
            }
            return bos;
        }

        public static TreatyPricingProductPerLifeRetro FormEntity(TreatyPricingProductPerLifeRetroBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProductPerLifeRetro
            {
                Id = bo.Id,
                TreatyPricingProductId = bo.TreatyPricingProductId,
                TreatyPricingPerLifeRetroId = bo.TreatyPricingPerLifeRetroId,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProductPerLifeRetro.IsExists(id);
        }

        public static bool IsExists(int productId, string perLifeRetroCode)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Where(q => q.TreatyPricingPerLifeRetro.Code == perLifeRetroCode)
                    .Any();
            }
        }

        public static bool IsExists(int productId, int perLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Where(q => q.TreatyPricingPerLifeRetroId == perLifeRetroId)
                    .Any();
            }
        }

        public static TreatyPricingProductPerLifeRetroBo Find(int productId, int perLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingProductId == productId)
                    .Where(q => q.TreatyPricingPerLifeRetroId == perLifeRetroId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static TreatyPricingProductPerLifeRetroBo Find(int? id)
        {
            return FormBo(TreatyPricingProductPerLifeRetro.Find(id));
        }

        public static IList<TreatyPricingProductPerLifeRetroBo> GetByProductId(int productId, List<string> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingProductId == productId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.TreatyPricingPerLifeRetro.Code));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingProductPerLifeRetroBo> GetByPerLifeRetroId(int perLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingPerLifeRetroId == perLifeRetroId);

                return FormBos(query.ToList(), true);
            }
        }

        public static List<string> GetCodeByProductId(int productId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductPerLifeRetros
                    .Where(q => q.TreatyPricingProductId == productId);

                return query.Select(q => q.TreatyPricingPerLifeRetro.Code).ToList();
            }
        }

        public static string GetJoinCodeByProductId(int productId)
        {
            using (var db = new AppDbContext())
            {
                return string.Join(",", GetCodeByProductId(productId));
            }
        }

        public static string GetWarning(int productId, int perLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                var subQuery = db.TreatyPricingProductBenefits.Where(q => q.TreatyPricingProductVersion.TreatyPricingProductId == productId).Select(q => q.BenefitId);
                bool hasWarning = db.TreatyPricingPerLifeRetroVersionBenefits.Where(q => q.TreatyPricingPerLifeRetroVersion.TreatyPricingPerLifeRetroId == perLifeRetroId)
                    .Any(q => !subQuery.Contains(q.BenefitId));

                return hasWarning ? "Product benefit does not match" : null;
            }
        }

        public static IList<TreatyPricingProductBo> GetProductBySearchParams(int perLifeRetroId, int? treatyPricingCedantId, int? productTypeId, string targetSegments, string distributionChannels, string underwritingMethods, string productIdName, string quotationName)
        {
            using (var db = new AppDbContext())
            {
                var searchQuery = TreatyPricingProductVersionService.QueryByParam(db, treatyPricingCedantId, productTypeId, targetSegments, distributionChannels, underwritingMethods, productIdName, quotationName);
                var exceptionQuery = db.TreatyPricingProductPerLifeRetros.Where(q => q.TreatyPricingPerLifeRetroId == perLifeRetroId).Select(q => q.TreatyPricingProductId);

                return TreatyPricingProductService.LinkFormBos(db.TreatyPricingProducts.Where(q => searchQuery.Contains(q.Id) && !exceptionQuery.Contains(q.Id)).ToList());
            }
        }

        public static Result Create(ref TreatyPricingProductPerLifeRetroBo bo)
        {
            TreatyPricingProductPerLifeRetro entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductPerLifeRetroBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductPerLifeRetroBo bo)
        {
            TreatyPricingProductPerLifeRetro.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductPerLifeRetroBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProductPerLifeRetro.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByProductIdExcept(int productId, List<string> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByProductId(productId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
