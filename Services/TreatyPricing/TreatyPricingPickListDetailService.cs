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
    public class TreatyPricingPickListDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPickListDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductDetail.ToString()
            };
        }

        public static TreatyPricingPickListDetailBo FormBo(TreatyPricingPickListDetail entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingPickListDetailBo
            {
                Id = entity.Id,
                PickListId = entity.PickListId,
                ObjectType = entity.ObjectType,
                ObjectId = entity.ObjectId,
                PickListDetailCode = entity.PickListDetailCode,
                CreatedById = entity.CreatedById,
            };
        }

        public static IList<TreatyPricingPickListDetailBo> FormBos(IList<TreatyPricingPickListDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPickListDetailBo> bos = new List<TreatyPricingPickListDetailBo>() { };
            foreach (TreatyPricingPickListDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPickListDetail FormEntity(TreatyPricingPickListDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPickListDetail
            {
                Id = bo.Id,
                PickListId = bo.PickListId,
                ObjectType = bo.ObjectType,
                ObjectId = bo.ObjectId,
                PickListDetailCode = bo.PickListDetailCode,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingPickListDetail.IsExists(id);
        }

        public static bool IsExists(int objectType, int objectId, int pickListId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.PickListId == pickListId)
                    .Where(q => q.PickListDetailCode == code)
                    .Any();
            }
        }

        public static TreatyPricingPickListDetailBo Find(int? id)
        {
            return FormBo(TreatyPricingPickListDetail.Find(id));
        }

        public static IList<TreatyPricingPickListDetailBo> GetByObjectPickList(int objectType, int objectId, int pickListId, List<string> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.PickListId == pickListId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.PickListDetailCode));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingPickListDetailBo> Get()
        {
            return FormBos(TreatyPricingPickListDetail.Get());
        }

        public static List<string> GetCodeByObjectPickList(int objectType, int objectId, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.PickListId == pickListId);

                return query.Select(q => q.PickListDetailCode).ToList();
            }
        }

        public static List<string> GetDistinctCodeByObjectProductObjectIdsPickList(List<int> objectIds, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProduct)
                    .Where(q => objectIds.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .GroupBy(q => q.PickListDetailCode.Trim())
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.PickListDetailCode.Trim());

                return query.Select(q => q.PickListDetailCode.Trim()).ToList();
            }
        }

        public static List<string> GetDistinctCodeByObjectProductVersionProductIdsPickList(List<int> productIds, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductVersions.Where(q => productIds.Contains(q.TreatyPricingProductId)).Select(q => q.Id).ToList();

                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => ids.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .GroupBy(q => q.PickListDetailCode)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.PickListDetailCode);

                return query.Select(q => q.PickListDetailCode).ToList();
            }
        }

        public static List<string> GetDistinctCodeByObjectProductVersionProductIdPickList(int productId, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductVersions.Where(q => q.TreatyPricingProductId == productId).Select(q => q.Id).ToList();

                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => ids.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .GroupBy(q => q.PickListDetailCode.Trim())
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.PickListDetailCode.Trim());

                return query.Select(q => q.PickListDetailCode.Trim()).ToList();
            }
        }

        public static IList<TreatyPricingPickListDetailBo> GetDistinctBoByObjectProductVersionProductIdsPickList(List<int> productIds, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductVersions.Where(q => productIds.Contains(q.TreatyPricingProductId)).Select(q => q.Id).ToList();

                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => ids.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .GroupBy(q => q.PickListDetailCode)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.PickListDetailCode);

                return FormBos(query.ToList());
            }
        }

        public static List<string> GetDistinctCodeByObjectProductVersionProductVersionIdsPickList(List<int> productVersionIds, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => productVersionIds.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .GroupBy(q => q.PickListDetailCode.Trim())
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.PickListDetailCode.Trim());

                return query.Select(q => q.PickListDetailCode.Trim()).ToList();
            }
        }

        public static List<int> GetProductIdByProductIdsUnderwritingMethods(List<int> productIds, List<string> underwritingMethods)
        {
            using (var db = new AppDbContext())
            {
                var count = underwritingMethods.Count();

                return db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProduct)
                    .Where(q => productIds.Contains(q.ObjectId))
                    .Where(q => q.PickListId == PickListBo.UnderwritingMethod)
                    .Where(q => underwritingMethods.Contains(q.PickListDetailCode.Trim()))
                    .GroupBy(q => q.ObjectId)
                    .Where(r => r.Count() == count)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.ObjectId)
                    .Select(q => q.ObjectId)
                    .ToList();
            }
        }

        public static List<int> GetProductVersionIdByProductVersionIdsPickListIdCodes(List<int> productVersionIds, int pickListId, List<string> codes)
        {
            using (var db = new AppDbContext())
            {
                var count = codes.Count();

                return db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => productVersionIds.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .Where(q => codes.Contains(q.PickListDetailCode.Trim()))
                    .GroupBy(q => q.ObjectId)
                    .Where(r => r.Count() == count)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.ObjectId)
                    .Select(q => q.ObjectId)
                    .ToList();
            }
        }

        public static List<int> GetProductVersionIdByProductIdsPickListIdCodes(List<int> productIds, int pickListId, List<string> codes)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductVersions.Where(q => productIds.Contains(q.TreatyPricingProductId)).Select(q => q.Id).ToList();
                var count = codes.Count();

                return db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => ids.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .Where(q => codes.Contains(q.PickListDetailCode))
                    .GroupBy(q => q.ObjectId)
                    .Where(r => r.Count() == count)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.ObjectId)
                    .Select(q => q.ObjectId)
                    .ToList();
            }
        }

        public static List<int> GetProductVersionIdByProductIdPickListIdCodes(int productId, int pickListId, List<string> codes)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductVersions.Where(q => q.TreatyPricingProductId == productId).Select(q => q.Id).ToList();
                var count = codes.Count();

                return db.TreatyPricingPickListDetails
                    .Where(q => q.ObjectType == TreatyPricingCedantBo.ObjectProductVersion)
                    .Where(q => ids.Contains(q.ObjectId))
                    .Where(q => q.PickListId == pickListId)
                    .Where(q => codes.Contains(q.PickListDetailCode.Trim()))
                    .GroupBy(q => q.ObjectId)
                    .Where(r => r.Count() == count)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.ObjectId)
                    .Select(q => q.ObjectId)
                    .ToList();
            }
        }

        public static string GetJoinCodeByObjectPickList(int objectType, int objectId, int pickListId)
        {
            using (var db = new AppDbContext())
            {
                return string.Join(",", GetCodeByObjectPickList(objectType, objectId, pickListId));
            }
        }

        public static Result Create(ref TreatyPricingPickListDetailBo bo)
        {
            TreatyPricingPickListDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPickListDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPickListDetailBo bo)
        {
            TreatyPricingPickListDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPickListDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPickListDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByObjectPickListExcept(int objectType, int objectId, int pickListId, List<string> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByObjectPickList(objectType, objectId, pickListId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
