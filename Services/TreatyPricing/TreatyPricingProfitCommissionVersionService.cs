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
    public class TreatyPricingProfitCommissionVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProfitCommissionVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingProfitCommissionVersion.ToString()
            };
        }

        public static Expression<Func<TreatyPricingProfitCommissionVersion, TreatyPricingProfitCommissionVersionBo>> Expression()
        {
            return entity => new TreatyPricingProfitCommissionVersionBo
            {
                Id = entity.Id,
                TreatyPricingProfitCommissionId = entity.TreatyPricingProfitCommissionId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                ProfitSharing = entity.ProfitSharing,
                ProfitDescription = entity.ProfitDescription,
                NetProfitPercentage = entity.NetProfitPercentage,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingProfitCommissionVersionBo FormBo(TreatyPricingProfitCommissionVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingProfitCommissionVersionBo
            {
                Id = entity.Id,
                TreatyPricingProfitCommissionId = entity.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionBo = foreign ? TreatyPricingProfitCommissionService.Find(entity.TreatyPricingProfitCommissionId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                ProfitSharing = entity.ProfitSharing,
                ProfitDescription = entity.ProfitDescription,
                NetProfitPercentage = entity.NetProfitPercentage,
                NetProfitPercentageStr = Util.DoubleToString(entity.NetProfitPercentage),
                ProfitCommissionDetail = TreatyPricingProfitCommissionDetailService.GetJsonByParent(entity.Id),
                TierProfitCommission = TreatyPricingTierProfitCommissionService.GetJsonByParent(entity.Id),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingProfitCommissionVersionBo> FormBos(IList<TreatyPricingProfitCommissionVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProfitCommissionVersionBo> bos = new List<TreatyPricingProfitCommissionVersionBo>() { };
            foreach (TreatyPricingProfitCommissionVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingProfitCommissionVersion FormEntity(TreatyPricingProfitCommissionVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProfitCommissionVersion
            {
                Id = bo.Id,
                TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                ProfitSharing = bo.ProfitSharing,
                ProfitDescription = bo.ProfitDescription,
                NetProfitPercentage = bo.NetProfitPercentage,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProfitCommissionVersion.IsExists(id);
        }

        public static TreatyPricingProfitCommissionVersionBo Find(int? id, bool foreign = false)
        {
            return FormBo(TreatyPricingProfitCommissionVersion.Find(id), foreign );
        }

        public static TreatyPricingProfitCommissionVersionBo FindLatestByTreatyPricingProfitCommissionId(int treatyPricingProfitCommissionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingProfitCommissionVersions.Where(q => q.TreatyPricingProfitCommissionId == treatyPricingProfitCommissionId).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingProfitCommissionVersionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissionVersions.ToList());
            }
        }

        public static List<int> GetVersionByTreatyPricingProfitCommissionId(int treatyPricingProfitCommissionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionVersions.Where(q => q.TreatyPricingProfitCommissionId == treatyPricingProfitCommissionId).OrderBy(q => q.Version).Select(q => q.Version).ToList();
            }
        }

        public static IList<TreatyPricingProfitCommissionVersionBo> GetByTreatyPricingProfitCommissionId(int treatyPricingProfitCommissionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissionVersions.Where(q => q.TreatyPricingProfitCommissionId == treatyPricingProfitCommissionId).ToList());
            }
        }

        public static IList<TreatyPricingProfitCommissionVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissionVersions
                    .Where(q => q.TreatyPricingProfitCommission.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingProfitCommissionId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingProfitCommissionVersionBo> GetByTreatyPricingCedantIds(List<int> treatyPricingCedantIds, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissionVersions
                    .Where(q => treatyPricingCedantIds.Contains(q.TreatyPricingProfitCommission.TreatyPricingCedantId))
                    .OrderBy(q => q.TreatyPricingProfitCommissionId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static int CountByTreatyPricingProfitCommissionId(int treatyPricingProfitCommissionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionVersions.Where(q => q.TreatyPricingProfitCommissionId == treatyPricingProfitCommissionId).Count();
            }
        }

        public static Result Save(ref TreatyPricingProfitCommissionVersionBo bo)
        {
            if (!TreatyPricingProfitCommissionVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingProfitCommissionVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingProfitCommissionVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProfitCommissionVersionBo bo)
        {
            TreatyPricingProfitCommissionVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProfitCommissionVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionVersionBo bo)
        {
            Result result = Result();

            TreatyPricingProfitCommissionVersion entity = TreatyPricingProfitCommissionVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId;
                entity.Version = bo.Version;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.ProfitSharing = bo.ProfitSharing;
                entity.ProfitDescription = bo.ProfitDescription;
                entity.NetProfitPercentage = bo.NetProfitPercentage;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProfitCommissionVersionBo bo)
        {
            TreatyPricingProfitCommissionVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProfitCommissionVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProfitCommissionVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
