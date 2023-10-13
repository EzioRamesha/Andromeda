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
    public class TreatyPricingGroupReferralGtlTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralGtlTable)),
            };
        }

        public static TreatyPricingGroupReferralGtlTableBo FormBoForReport(TreatyPricingGroupReferralGtlTable entity = null)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingGroupReferralGtlTableBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = entity.TreatyPricingGroupReferralFileId,
                BenefitCode = entity.BenefitCode,
                RiskRate = entity.RiskRate,
                GrossRate = entity.GrossRate
            };

            return bo;
        }

        public static TreatyPricingGroupReferralGtlTableBo FormBo(TreatyPricingGroupReferralGtlTable entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingGroupReferralGtlTableBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = entity.TreatyPricingGroupReferralFileId,

                Type = entity.Type,

                BenefitCode = entity.BenefitCode,
                Designation = entity.Designation,
                CoverageStartDate = entity.CoverageStartDate,
                CoverageEndDate = entity.CoverageEndDate,
                EventDate = entity.EventDate,
                ClaimListDate = entity.ClaimListDate,
                ClaimantsName = entity.ClaimantsName,
                CauseOfClaim = entity.CauseOfClaim,
                ClaimType = entity.ClaimType,
                AgeBandRange = entity.AgeBandRange,
                BasisOfSA = entity.BasisOfSA,
                GrossClaim = entity.GrossClaim,
                RiClaim = entity.RiClaim,
                RiskRate = entity.RiskRate,
                GrossRate = entity.GrossRate,

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

        public static TreatyPricingGroupReferralGtlTable FormEntity(TreatyPricingGroupReferralGtlTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralGtlTable
            {
                Id = bo.Id,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = bo.TreatyPricingGroupReferralFileId,

                Type = bo.Type,

                BenefitCode = bo.BenefitCode,
                Designation = bo.Designation,
                CoverageStartDate = bo.CoverageStartDate,
                CoverageEndDate = bo.CoverageEndDate,
                EventDate = bo.EventDate,
                ClaimListDate = bo.ClaimListDate,
                ClaimantsName = bo.ClaimantsName,
                CauseOfClaim = bo.CauseOfClaim,
                ClaimType = bo.ClaimType,
                AgeBandRange = bo.AgeBandRange,
                BasisOfSA = bo.BasisOfSA,
                GrossClaim = bo.GrossClaim,
                RiClaim = bo.RiClaim,
                RiskRate = bo.RiskRate,
                GrossRate = bo.GrossRate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<TreatyPricingGroupReferralGtlTableBo> FormBosForReport(IList<TreatyPricingGroupReferralGtlTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralGtlTableBo> bos = new List<TreatyPricingGroupReferralGtlTableBo>() { };
            foreach (TreatyPricingGroupReferralGtlTable entity in entities)
            {
                bos.Add(FormBoForReport(entity));
            }
            return bos;
        }

        public static IList<TreatyPricingGroupReferralGtlTableBo> FormBos(IList<TreatyPricingGroupReferralGtlTable> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralGtlTableBo> bos = new List<TreatyPricingGroupReferralGtlTableBo>() { };
            foreach (TreatyPricingGroupReferralGtlTable entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralGtlTable.IsExists(id);
        }

        public static TreatyPricingGroupReferralGtlTableBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralGtlTable.Find(id));
        }

        public static int CountByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId, AppDbContext db)
        {
            return db.TreatyPricingGroupReferralGtlTables
                .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                .Count();
        }

        public static IList<TreatyPricingGroupReferralGtlTableBo> GetByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralGtlTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralGtlTableBo> GetByTreatyPricingGroupReferralIdTypeForReport(int treatyPricingGroupReferralId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBosForReport(db.TreatyPricingGroupReferralGtlTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Where(q => q.Type == type)
                    .ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralGtlTableBo> GetByTreatyPricingGroupReferralIdType(int treatyPricingGroupReferralId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralGtlTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Where(q => q.Type == type)
                    .ToList());
            }
        }

        public static List<string> GetDistinctBenefitCodesWithType(int type)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGtlTables
                    .Where(q => !string.IsNullOrEmpty(q.BenefitCode))
                    .Where(q => q.Type == type)
                    .Select(q => q.BenefitCode)
                    .Distinct()
                    .ToList();
            }
        }

        public static List<string> GetDistinctAgeBandRange()
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGtlTables
                    .Where(q => !string.IsNullOrEmpty(q.BenefitCode))
                    .Where(q => q.Type == TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded)
                    .Select(q => q.AgeBandRange)
                    .Distinct()
                    .ToList();
            }
        }

        public static List<string> GetDistinctDesignation()
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGtlTables
                    .Where(q => !string.IsNullOrEmpty(q.BenefitCode))
                    .Where(q => q.Type == TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa)
                    .Select(q => q.Designation)
                    .Distinct()
                    .ToList();
            }
        }
        public static IList<string> GetDistinctBenefitCodeByTreatyPricingGroupReferralIdType(int treatyPricingGroupReferralId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                var bos = GetByTreatyPricingGroupReferralIdType(treatyPricingGroupReferralId, type);
                List<string> benefitCodes = new List<string>();

                if (!bos.IsNullOrEmpty())
                    benefitCodes.AddRange(bos
                        .Where(q => !string.IsNullOrEmpty(q.BenefitCode))
                        .Select(q => q.BenefitCode)
                        .Distinct()
                        .ToList());

                return benefitCodes;
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralGtlTableBo bo)
        {
            if (!TreatyPricingGroupReferralGtlTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingGroupReferralGtlTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralGtlTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralGtlTableBo bo)
        {
            TreatyPricingGroupReferralGtlTable entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref TreatyPricingGroupReferralGtlTableBo bo, AppDbContext db)
        {
            TreatyPricingGroupReferralGtlTable entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref TreatyPricingGroupReferralGtlTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralGtlTableBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralGtlTable entity = TreatyPricingGroupReferralGtlTable.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralGtlTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralGtlTableBo bo)
        {
            TreatyPricingGroupReferralGtlTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralGtlTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyPricingGroupReferralGtlTable.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingGroupReferralGtlTables] WHERE [TreatyPricingGroupReferralId] = {0} AND [Type] = {1}", treatyPricingGroupReferralId, type);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
