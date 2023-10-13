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
    public class TreatyPricingGroupReferralGhsTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralGhsTable)),
            };
        }

        public static TreatyPricingGroupReferralGhsTableBo FormBo(TreatyPricingGroupReferralGhsTable entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingGroupReferralGhsTableBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = entity.TreatyPricingGroupReferralFileId,

                CoverageStartDate = entity.CoverageStartDate,
                EventDate = entity.EventDate,
                ClaimListDate = entity.ClaimListDate,
                ClaimantsName = entity.ClaimantsName,
                CauseOfClaim = entity.CauseOfClaim,
                RbCovered = entity.RbCovered,
                AolCovered = entity.AolCovered,
                Relationship = entity.Relationship,
                HospitalCovered = entity.HospitalCovered,
                GrossClaimIncurred = entity.GrossClaimIncurred,
                GrossClaimPaid = entity.GrossClaimPaid,
                GrossClaimPaidIbnr = entity.GrossClaimPaidIbnr,
                RiClaimPaid = entity.RiClaimPaid,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(entity.TreatyPricingGroupReferralId);
                bo.TreatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileService.Find(entity.TreatyPricingGroupReferralFileId);
            }

            bo.CoverageStartDateStr = entity.CoverageStartDate?.ToString(Util.GetDateFormat());
            bo.EventDateStr = entity.EventDate?.ToString(Util.GetDateFormat());

            return bo;
        }

        public static TreatyPricingGroupReferralGhsTable FormEntity(TreatyPricingGroupReferralGhsTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralGhsTable
            {
                Id = bo.Id,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = bo.TreatyPricingGroupReferralFileId,

                CoverageStartDate = bo.CoverageStartDate,
                EventDate = bo.EventDate,
                ClaimListDate = bo.ClaimListDate,
                ClaimantsName = bo.ClaimantsName,
                CauseOfClaim = bo.CauseOfClaim,
                RbCovered = bo.RbCovered,
                AolCovered = bo.AolCovered,
                Relationship = bo.Relationship,
                HospitalCovered = bo.HospitalCovered,
                GrossClaimIncurred = bo.GrossClaimIncurred,
                GrossClaimPaid = bo.GrossClaimPaid,
                GrossClaimPaidIbnr = bo.GrossClaimPaidIbnr,
                RiClaimPaid = bo.RiClaimPaid,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<TreatyPricingGroupReferralGhsTableBo> FormBos(IList<TreatyPricingGroupReferralGhsTable> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralGhsTableBo> bos = new List<TreatyPricingGroupReferralGhsTableBo>() { };
            foreach (TreatyPricingGroupReferralGhsTable entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralGhsTable.IsExists(id);
        }

        public static TreatyPricingGroupReferralGhsTableBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralGhsTable.Find(id));
        }

        public static int CountByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId, AppDbContext db)
        {
            return db.TreatyPricingGroupReferralGhsTables
                .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                .Count();
        }

        public static IList<TreatyPricingGroupReferralGhsTableBo> GetByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralGhsTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .OrderBy(q => q.Id)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralGhsTableBo bo)
        {
            if (!TreatyPricingGroupReferralGhsTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingGroupReferralGhsTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralGhsTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralGhsTableBo bo)
        {
            TreatyPricingGroupReferralGhsTable entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref TreatyPricingGroupReferralGhsTableBo bo, AppDbContext db)
        {
            TreatyPricingGroupReferralGhsTable entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref TreatyPricingGroupReferralGhsTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralGhsTableBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralGhsTable entity = TreatyPricingGroupReferralGhsTable.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralGhsTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralGhsTableBo bo)
        {
            TreatyPricingGroupReferralGhsTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralGhsTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyPricingGroupReferralGhsTable.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingGroupReferralGhsTables] WHERE [TreatyPricingGroupReferralId] = {0}", treatyPricingGroupReferralId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
