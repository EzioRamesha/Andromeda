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

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupMasterLetterGroupReferralService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupMasterLetterGroupReferral)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupMasterLetterGroupReferral.ToString()
            };
        }

        public static TreatyPricingGroupMasterLetterGroupReferralBo FormBo(TreatyPricingGroupMasterLetterGroupReferral entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupMasterLetterGroupReferralBo
            {
                Id = entity.Id,
                TreatyPricingGroupMasterLetterId = entity.TreatyPricingGroupMasterLetterId,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(entity.TreatyPricingGroupReferralId, false),
                CreatedById = entity.CreatedById,
            };
        }

        public static IList<TreatyPricingGroupMasterLetterGroupReferralBo> FormBos(IList<TreatyPricingGroupMasterLetterGroupReferral> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupMasterLetterGroupReferralBo> bos = new List<TreatyPricingGroupMasterLetterGroupReferralBo>() { };
            foreach (TreatyPricingGroupMasterLetterGroupReferral entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupMasterLetterGroupReferral FormEntity(TreatyPricingGroupMasterLetterGroupReferralBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupMasterLetterGroupReferral
            {
                Id = bo.Id,
                TreatyPricingGroupMasterLetterId = bo.TreatyPricingGroupMasterLetterId,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupMasterLetterGroupReferral.IsExists(id);
        }

        public static TreatyPricingGroupMasterLetterGroupReferralBo Find(int? id)
        {
            return FormBo(TreatyPricingGroupMasterLetterGroupReferral.Find(id));
        }

        public static IList<TreatyPricingGroupMasterLetterGroupReferralBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupMasterLetterGroupReferrals.ToList());
            }
        }

        public static IList<TreatyPricingGroupMasterLetterGroupReferralBo> GetByGroupMasterLetterId(int groupMasterLetterId, List<int> ids = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingGroupMasterLetterGroupReferrals
                    .Where(q => q.TreatyPricingGroupMasterLetterId == groupMasterLetterId);

                if (ids != null)
                {
                    query = query.Where(q => !ids.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<TreatyPricingGroupMasterLetterGroupReferralBo> GetByGroupMasterLetterId(int groupMasterLetterId, int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupMasterLetterGroupReferrals
                    .Where(q => q.TreatyPricingGroupMasterLetterId == groupMasterLetterId)
                    .OrderBy(q => q.TreatyPricingGroupReferral.Code)
                    .Skip(skip)
                    .Take(take)
                    .ToList());
            }
        }

        public static int CountByGroupMasterLetterId (int groupMasterLetterId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupMasterLetterGroupReferrals
                    .Where(q => q.TreatyPricingGroupMasterLetterId == groupMasterLetterId)
                    .Count();
            }
        }

        public static Result Create(ref TreatyPricingGroupMasterLetterGroupReferralBo bo)
        {
            TreatyPricingGroupMasterLetterGroupReferral entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupMasterLetterGroupReferralBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupMasterLetterGroupReferralBo bo)
        {
            TreatyPricingGroupMasterLetterGroupReferral.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupMasterLetterGroupReferralBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupMasterLetterGroupReferral.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByGroupMasterLetterIdExcept(int groupMasterLetterId, List<int> ids, ref TrailObject trail)
        {
            foreach (var bo in GetByGroupMasterLetterId(groupMasterLetterId, ids))
            {
                Delete(bo, ref trail);
            }
        }

        public static void DeleteByGroupMasterLetterId(int groupMasterLetterId, ref TrailObject trail)
        {
            foreach (var bo in GetByGroupMasterLetterId(groupMasterLetterId))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
