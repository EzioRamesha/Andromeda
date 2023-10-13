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
    public class TreatyPricingClaimApprovalLimitVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingClaimApprovalLimitVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingClaimApprovalLimitVersion.ToString()
            };
        }

        public static TreatyPricingClaimApprovalLimitVersionBo FormBo(TreatyPricingClaimApprovalLimitVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingClaimApprovalLimitVersionBo
            {
                Id = entity.Id,
                TreatyPricingClaimApprovalLimitId = entity.TreatyPricingClaimApprovalLimitId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName,
                EffectiveAt = entity.EffectiveAt,
                Amount = entity.Amount,
                AdditionalRemarks = entity.AdditionalRemarks,
            };
            if (bo.EffectiveAt.HasValue)
                bo.EffectiveAtStr = bo.EffectiveAt.Value.ToString(Util.GetDateFormat());

            if (foreign)
            {
                bo.TreatyPricingClaimApprovalLimitBo = TreatyPricingClaimApprovalLimitService.Find(entity.TreatyPricingClaimApprovalLimitId);
            }

            return bo;
        }

        public static IList<TreatyPricingClaimApprovalLimitVersionBo> FormBos(IList<TreatyPricingClaimApprovalLimitVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingClaimApprovalLimitVersionBo> bos = new List<TreatyPricingClaimApprovalLimitVersionBo>() { };
            foreach (TreatyPricingClaimApprovalLimitVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingClaimApprovalLimitVersion FormEntity(TreatyPricingClaimApprovalLimitVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingClaimApprovalLimitVersion
            {
                Id = bo.Id,
                TreatyPricingClaimApprovalLimitId = bo.TreatyPricingClaimApprovalLimitId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                Amount = bo.Amount,
                AdditionalRemarks = bo.AdditionalRemarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingClaimApprovalLimitVersion.IsExists(id);
        }

        public static TreatyPricingClaimApprovalLimitVersionBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(TreatyPricingClaimApprovalLimitVersion.Find(id.Value));
        }

        public static IList<TreatyPricingClaimApprovalLimitVersionBo> GetByTreatyPricingClaimApprovalLimitId(int treatyPricingClaimApprovalLimitId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingClaimApprovalLimitVersions
                    .Where(q => q.TreatyPricingClaimApprovalLimitId == treatyPricingClaimApprovalLimitId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingClaimApprovalLimitVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingClaimApprovalLimitVersions
                    .Where(q => q.TreatyPricingClaimApprovalLimit.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingClaimApprovalLimitId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static Result Save(ref TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            if (!TreatyPricingClaimApprovalLimitVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingClaimApprovalLimitVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingClaimApprovalLimitVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            TreatyPricingClaimApprovalLimitVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingClaimApprovalLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            Result result = Result();

            TreatyPricingClaimApprovalLimitVersion entity = TreatyPricingClaimApprovalLimitVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingClaimApprovalLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingClaimApprovalLimitVersionBo bo)
        {
            TreatyPricingClaimApprovalLimitVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingClaimApprovalLimitVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingClaimApprovalLimitVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingClaimApprovalLimitId(int treatyPricingClaimApprovalLimitId)
        {
            return TreatyPricingClaimApprovalLimitVersion.DeleteAllByTreatyPricingClaimApprovalLimitId(treatyPricingClaimApprovalLimitId);
        }

        public static void DeleteAllByTreatyPricingClaimApprovalLimitId(int treatyPricingClaimApprovalLimitId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingClaimApprovalLimitId(treatyPricingClaimApprovalLimitId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingClaimApprovalLimitVersion)));
                }
            }
        }
    }
}
