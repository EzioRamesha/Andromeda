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
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingAdvantageProgramVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingAdvantageProgramVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingAdvantageProgramVersion.ToString()
            };
        }

        public static TreatyPricingAdvantageProgramVersionBo FormBo(TreatyPricingAdvantageProgramVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingAdvantageProgramVersionBo
            {
                Id = entity.Id,
                TreatyPricingAdvantageProgramId = entity.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramBo = foreign ? TreatyPricingAdvantageProgramService.Find(entity.TreatyPricingAdvantageProgramId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                EffectiveAt = entity.EffectiveAt,
                Retention = entity.Retention,
                MlreShare = entity.MlreShare,
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TreatyPricingAdvantageProgramVersionBenefit = TreatyPricingAdvantageProgramVersionBenefitService.GetJsonByVersionId(entity.Id),

                EffcetiveAtStr = entity.EffectiveAt?.ToString(Util.GetDateFormat()),
                RetentionStr = Util.DoubleToString(entity.Retention),
                MlreShareStr = Util.DoubleToString(entity.MlreShare),
            };
        }

        public static IList<TreatyPricingAdvantageProgramVersionBo> FormBos(IList<TreatyPricingAdvantageProgramVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingAdvantageProgramVersionBo> bos = new List<TreatyPricingAdvantageProgramVersionBo>() { };
            foreach (TreatyPricingAdvantageProgramVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingAdvantageProgramVersion FormEntity(TreatyPricingAdvantageProgramVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingAdvantageProgramVersion
            {
                Id = bo.Id,
                TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                Retention = bo.Retention,
                MlreShare = bo.MlreShare,
                Remarks = bo.Remarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingAdvantageProgramVersion.IsExists(id);
        }

        public static TreatyPricingAdvantageProgramVersionBo Find(int? id, bool foreign = false)
        {
            return FormBo(TreatyPricingAdvantageProgramVersion.Find(id), foreign);
        }

        public static IList<TreatyPricingAdvantageProgramVersionBo> GetByTreatyPricingAdvantageProgramId(int? treatyPricingAdvantageProgramId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantageProgramVersions
                    .Where(q => q.TreatyPricingAdvantageProgramId == treatyPricingAdvantageProgramId)
                    //.OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingAdvantageProgramVersionBo> GetByTreatyPricingAdvantageProgramVersion(int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantageProgramVersions.Where(q => q.Id == version).ToList());
            }
        }

        public static IList<TreatyPricingAdvantageProgramVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingAdvantageProgramVersions
                    .Where(q => q.TreatyPricingAdvantageProgram.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingAdvantageProgramId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static TreatyPricingAdvantageProgramVersionBo GetLatestByTreatyPricingAdvantageProgramId(int treatyPricingAdvantageProgramId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingAdvantageProgramVersions
                    .Where(q => q.TreatyPricingAdvantageProgramId == treatyPricingAdvantageProgramId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingAdvantageProgramVersionBo bo)
        {
            if (!TreatyPricingAdvantageProgramVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingAdvantageProgramVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingAdvantageProgramVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingAdvantageProgramVersionBo bo)
        {
            TreatyPricingAdvantageProgramVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingAdvantageProgramVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingAdvantageProgramVersionBo bo)
        {
            Result result = Result();

            TreatyPricingAdvantageProgramVersion entity = TreatyPricingAdvantageProgramVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingAdvantageProgramVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingAdvantageProgramVersionBo bo)
        {
            TreatyPricingAdvantageProgramVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingAdvantageProgramVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingAdvantageProgramVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
