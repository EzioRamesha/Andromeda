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
    public class TreatyPricingTierProfitCommissionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingTierProfitCommission)),
                Controller = ModuleBo.ModuleController.TreatyPricingTierProfitCommission.ToString()
            };
        }

        public static TreatyPricingTierProfitCommissionBo FormBo(TreatyPricingTierProfitCommission entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingTierProfitCommissionBo
            {
                Id = entity.Id,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,
                Col1 = entity.Col1,
                Col2 = entity.Col2,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingTierProfitCommissionBo> FormBos(IList<TreatyPricingTierProfitCommission> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingTierProfitCommissionBo> bos = new List<TreatyPricingTierProfitCommissionBo>() { };
            foreach (TreatyPricingTierProfitCommission entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingTierProfitCommission FormEntity(TreatyPricingTierProfitCommissionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingTierProfitCommission
            {
                Id = bo.Id,
                TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId,
                Col1 = bo.Col1,
                Col2 = bo.Col2,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingTierProfitCommission.IsExists(id);
        }

        public static TreatyPricingTierProfitCommissionBo Find(int? id)
        {
            return FormBo(TreatyPricingTierProfitCommission.Find(id));
        }

        public static IList<TreatyPricingTierProfitCommissionBo> GetByParent(int parentId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingTierProfitCommissions
                    .Where(q => q.TreatyPricingProfitCommissionVersionId == parentId);

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
                bos.AddRange(TreatyPricingTierProfitCommissionBo.GetDefaultRow(parentId, bos.Count()));

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByParent(int parentId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTierProfitCommissions
                    .Where(q => q.TreatyPricingProfitCommissionVersionId == parentId)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingTierProfitCommissionBo bo)
        {
            if (!TreatyPricingTierProfitCommission.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingTierProfitCommissionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingTierProfitCommission.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingTierProfitCommissionBo bo)
        {
            TreatyPricingTierProfitCommission entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingTierProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingTierProfitCommissionBo bo)
        {
            Result result = Result();

            TreatyPricingTierProfitCommission entity = TreatyPricingTierProfitCommission.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId;
                entity.Col1 = bo.Col1;
                entity.Col2 = bo.Col2;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingTierProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingTierProfitCommissionBo bo)
        {
            TreatyPricingTierProfitCommission.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingTierProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingTierProfitCommission.Delete(bo.Id);
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
    }
}
