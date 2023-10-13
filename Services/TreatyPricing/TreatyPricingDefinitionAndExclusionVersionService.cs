using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Services.TreatyPricing
{
    public class TreatyPricingDefinitionAndExclusionVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingDefinitionAndExclusionVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingDefinitionAndExclusionVersion.ToString()
            };
        }

        public static TreatyPricingDefinitionAndExclusionVersionBo FormBo(TreatyPricingDefinitionAndExclusionVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingDefinitionAndExclusionVersionBo
            {
                Id = entity.Id,
                TreatyPricingDefinitionAndExclusionId = entity.TreatyPricingDefinitionAndExclusionId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName,
                EffectiveAt = entity.EffectiveAt,
                Definitions = entity.Definitions,
                Exclusions = entity.Exclusions,
                DeclinedRisk = entity.DeclinedRisk,
                ReferredRisk = entity.ReferredRisk,
            };
            if (bo.EffectiveAt.HasValue)
                bo.EffectiveAtStr = bo.EffectiveAt.Value.ToString(Util.GetDateFormat());

            if (foreign)
            {
                bo.TreatyPricingDefinitionAndExclusionBo = TreatyPricingDefinitionAndExclusionService.Find(entity.TreatyPricingDefinitionAndExclusionId);
            }

            return bo;
        }

        public static IList<TreatyPricingDefinitionAndExclusionVersionBo> FormBos(IList<TreatyPricingDefinitionAndExclusionVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingDefinitionAndExclusionVersionBo> bos = new List<TreatyPricingDefinitionAndExclusionVersionBo>() { };
            foreach (TreatyPricingDefinitionAndExclusionVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingDefinitionAndExclusionVersion FormEntity(TreatyPricingDefinitionAndExclusionVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingDefinitionAndExclusionVersion
            {
                Id = bo.Id,
                TreatyPricingDefinitionAndExclusionId = bo.TreatyPricingDefinitionAndExclusionId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                Definitions = bo.Definitions,
                Exclusions = bo.Exclusions,
                DeclinedRisk = bo.DeclinedRisk,
                ReferredRisk = bo.ReferredRisk,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingDefinitionAndExclusionVersion.IsExists(id);
        }

        public static TreatyPricingDefinitionAndExclusionVersionBo Find(int id)
        {
            return FormBo(TreatyPricingDefinitionAndExclusionVersion.Find(id));
        }

        public static IList<TreatyPricingDefinitionAndExclusionVersionBo> GetByTreatyPricingDefinitionAndExclusionId(int treatyPricingDefinitionAndExclusionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingDefinitionAndExclusionVersions
                    .Where(q => q.TreatyPricingDefinitionAndExclusionId == treatyPricingDefinitionAndExclusionId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingDefinitionAndExclusionVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingDefinitionAndExclusionVersions
                    .Where(q => q.TreatyPricingDefinitionAndExclusion.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingDefinitionAndExclusionId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static Result Save(ref TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            if (!TreatyPricingDefinitionAndExclusionVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingDefinitionAndExclusionVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingDefinitionAndExclusionVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            TreatyPricingDefinitionAndExclusionVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingDefinitionAndExclusionVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            Result result = Result();

            TreatyPricingDefinitionAndExclusionVersion entity = TreatyPricingDefinitionAndExclusionVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingDefinitionAndExclusionVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            TreatyPricingDefinitionAndExclusionVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingDefinitionAndExclusionVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingDefinitionAndExclusionVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingDefinitionAndExclusionId(int treatyPricingDefinitionAndExclusionId)
        {
            return TreatyPricingDefinitionAndExclusionVersion.DeleteAllByTreatyPricingDefinitionAndExclusionId(treatyPricingDefinitionAndExclusionId);
        }

        public static void DeleteAllByTreatyPricingDefinitionAndExclusionId(int treatyPricingDefinitionAndExclusionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingDefinitionAndExclusionId(treatyPricingDefinitionAndExclusionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingDefinitionAndExclusionVersion)));
                }
            }
        }

        public static TreatyPricingDefinitionAndExclusionVersionBo FindByParentIdVersion(int parentId, int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingDefinitionAndExclusionVersions
                    .Where(q => q.TreatyPricingDefinitionAndExclusionId == parentId)
                    .Where(q => q.Version == version)
                    .FirstOrDefault());
            }
        }

    }
}
