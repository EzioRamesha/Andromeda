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
    public class TreatyPricingCustomOtherVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCustomOtherVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingCustomOtherVersion.ToString()
            };
        }

        public static TreatyPricingCustomOtherVersionBo FormBo(TreatyPricingCustomOtherVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingCustomOtherVersionBo
            {
                Id = entity.Id,
                TreatyPricingCustomOtherId = entity.TreatyPricingCustomOtherId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName,
                EffectiveAt = entity.EffectiveAt,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                AdditionalRemarks = entity.AdditionalRemarks,
            };
            if (bo.EffectiveAt.HasValue)
                bo.EffectiveAtStr = bo.EffectiveAt.Value.ToString(Util.GetDateFormat());

            return bo;
        }

        public static IList<TreatyPricingCustomOtherVersionBo> FormBos(IList<TreatyPricingCustomOtherVersion> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCustomOtherVersionBo> bos = new List<TreatyPricingCustomOtherVersionBo>() { };
            foreach (TreatyPricingCustomOtherVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingCustomOtherVersion FormEntity(TreatyPricingCustomOtherVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCustomOtherVersion
            {
                Id = bo.Id,
                TreatyPricingCustomOtherId = bo.TreatyPricingCustomOtherId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                AdditionalRemarks = bo.AdditionalRemarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingCustomOtherVersion.IsExists(id);
        }

        public static TreatyPricingCustomOtherVersionBo Find(int id)
        {
            return FormBo(TreatyPricingCustomOtherVersion.Find(id));
        }

        public static IList<TreatyPricingCustomOtherVersionBo> GetByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCustomOtherVersions
                    .Where(q => q.TreatyPricingCustomOtherId == treatyPricingCustomOtherId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingCustomOtherVersionBo bo)
        {
            if (!TreatyPricingCustomOtherVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingCustomOtherVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingCustomOtherVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingCustomOtherVersionBo bo)
        {
            TreatyPricingCustomOtherVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingCustomOtherVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCustomOtherVersionBo bo)
        {
            Result result = Result();

            TreatyPricingCustomOtherVersion entity = TreatyPricingCustomOtherVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingCustomOtherVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingCustomOtherVersionBo bo)
        {
            TreatyPricingCustomOtherVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingCustomOtherVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCustomOtherVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId)
        {
            return TreatyPricingCustomOtherVersion.DeleteAllByTreatyPricingCustomOtherId(treatyPricingCustomOtherId);
        }

        public static void DeleteAllByTreatyPricingCustomOtherId(int treatyPricingCustomOtherId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCustomOtherId(treatyPricingCustomOtherId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingCustomOtherVersion)));
                }
            }
        }
    }
}
