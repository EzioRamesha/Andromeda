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

namespace Services.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionTierService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroVersionTier)),
                Controller = ModuleBo.ModuleController.TreatyPricingPerLifeRetroVersionTier.ToString()
            };
        }

        public static TreatyPricingPerLifeRetroVersionTierBo FormBo(TreatyPricingPerLifeRetroVersionTier entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingPerLifeRetroVersionTierBo
            {
                Id = entity.Id,
                TreatyPricingPerLifeRetroVersionId = entity.TreatyPricingPerLifeRetroVersionId,
                Col1 = entity.Col1,
                Col2 = entity.Col2,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingPerLifeRetroVersionTierBo> FormBos(IList<TreatyPricingPerLifeRetroVersionTier> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroVersionTierBo> bos = new List<TreatyPricingPerLifeRetroVersionTierBo>() { };
            foreach (TreatyPricingPerLifeRetroVersionTier entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetroVersionTier FormEntity(TreatyPricingPerLifeRetroVersionTierBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetroVersionTier
            {
                Id = bo.Id,
                TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId,
                Col1 = bo.Col1,
                Col2 = bo.Col2,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingPerLifeRetroVersionTier.IsExists(id);
        }

        public static TreatyPricingPerLifeRetroVersionTierBo Find(int? id)
        {
            return FormBo(TreatyPricingPerLifeRetroVersionTier.Find(id));
        }

        public static IList<TreatyPricingPerLifeRetroVersionTierBo> GetByParent(int parentId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPerLifeRetroVersionTiers
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == parentId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        // PreCreate 6 row if empty
        // Add additional if less than 6 row
        public static string GetJsonByParent(int parentId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByParent(parentId).ToList();
                bos.AddRange(TreatyPricingPerLifeRetroVersionTierBo.GetDefaultRow(parentId, bos.Count()));

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByParent(int parentId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetroVersionTiers
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == parentId)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionTierBo bo)
        {
            if (!TreatyPricingPerLifeRetroVersionTier.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionTierBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingPerLifeRetroVersionTier.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionTierBo bo)
        {
            TreatyPricingPerLifeRetroVersionTier entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionTierBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionTierBo bo)
        {
            Result result = Result();

            TreatyPricingPerLifeRetroVersionTier entity = TreatyPricingPerLifeRetroVersionTier.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId;
                entity.Col1 = bo.Col1;
                entity.Col2 = bo.Col2;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionTierBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPerLifeRetroVersionTierBo bo)
        {
            TreatyPricingPerLifeRetroVersionTier.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPerLifeRetroVersionTierBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPerLifeRetroVersionTier.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByParentExcept(int parentId, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByParent(parentId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }

        public static void Save(string json, int parentId, int? profitSharing, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingPerLifeRetroVersionTierBo> bos = new List<TreatyPricingPerLifeRetroVersionTierBo>();
            List<int> savedIds = new List<int>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingPerLifeRetroVersionTierBo>>(json);

            if (profitSharing.HasValue && profitSharing == TreatyPricingPerLifeRetroVersionBo.ProfitSharingMax)
            {
                foreach (var detailBo in bos)
                {
                    var bo = detailBo;
                    if (resetId)
                        bo.Id = 0;

                    if (string.IsNullOrEmpty(bo.Col1) && string.IsNullOrEmpty(bo.Col2))
                        continue;

                    bo.TreatyPricingPerLifeRetroVersionId = parentId;
                    bo.UpdatedById = authUserId;

                    if (bo.Id == 0)
                        bo.CreatedById = authUserId;

                    Save(ref bo, ref trail);

                    savedIds.Add(bo.Id);
                }
            }

            DeleteByParentExcept(parentId, savedIds, ref trail);
        }
    }
}
