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
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingRateTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingRateTable)),
                Controller = ModuleBo.ModuleController.TreatyPricingRateTable.ToString()
            };
        }

        public static Expression<Func<TreatyPricingRateTable, TreatyPricingRateTableBo>> Expression()
        {
            return entity => new TreatyPricingRateTableBo
            {
                Id = entity.Id,
                TreatyPricingRateTableGroupId = entity.TreatyPricingRateTableGroupId,
                BenefitId = entity.BenefitId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingRateTableBo FormBo(TreatyPricingRateTable entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingRateTableBo
            {
                Id = entity.Id,
                TreatyPricingRateTableGroupId = entity.TreatyPricingRateTableGroupId,
                TreatyPricingRateTableGroupBo = TreatyPricingRateTableGroupService.Find(entity.TreatyPricingRateTableGroupId),
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingRateTableBo.GetStatusName(entity.Status),
                TreatyPricingRateTableVersionBos = TreatyPricingRateTableVersionService.GetByTreatyPricingRateTableId(entity.Id),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingRateTableBo> FormBos(IList<TreatyPricingRateTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingRateTableBo> bos = new List<TreatyPricingRateTableBo>() { };
            foreach (TreatyPricingRateTable entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingRateTable FormEntity(TreatyPricingRateTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingRateTable
            {
                Id = bo.Id,
                TreatyPricingRateTableGroupId = bo.TreatyPricingRateTableGroupId,
                BenefitId = bo.BenefitId,
                Code = bo.Code,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingRateTable.IsExists(id);
        }

        public static TreatyPricingRateTableBo Find(int? id)
        {
            return FormBo(TreatyPricingRateTable.Find(id));
        }

        public static TreatyPricingRateTableBo FindLatestByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingRateTables.Where(q => q.TreatyPricingRateTableGroup.TreatyPricingCedantId == treatyPricingCedantId).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static TreatyPricingRateTableBo FindLatestByTreatyPricingRateTableGroupId(int treatyPricingRateTableGroupId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingRateTables.Where(q => q.TreatyPricingRateTableGroupId == treatyPricingRateTableGroupId).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingRateTableBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTables.ToList());
            }
        }

        public static IList<TreatyPricingRateTableBo> GetByTreatyPricingRateTableGroupId(int treatyPricingRateTableGroupId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTables.Where(q => q.TreatyPricingRateTableGroupId == treatyPricingRateTableGroupId).ToList());
            }
        }

        public static IList<BenefitBo> GetDistinctBenefitCodeByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productIds.Contains(q.TreatyPricingProductVersion.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                return BenefitService.FormBos(db.TreatyPricingRateTables
                    .Where(q => ids.Contains(q.Id))
                    .GroupBy(q => q.BenefitId)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.Benefit.Code)
                    .Select(q => q.Benefit)
                    .ToList());
            }
        }

        public static IList<BenefitBo> GetDistinctBenefitCodeByProductVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                return BenefitService.FormBos(db.TreatyPricingRateTables
                    .Where(q => ids.Contains(q.Id))
                    .GroupBy(q => q.BenefitId)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.Benefit.Code)
                    .Select(q => q.Benefit)
                    .ToList());
            }
        }

        public static List<string> GetDistinctBenefitNameByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productIds.Contains(q.TreatyPricingProductVersion.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                return db.TreatyPricingRateTableVersions
                    .Where(q => ids.Contains(q.TreatyPricingRateTableId))
                    .Where(q => !string.IsNullOrEmpty(q.BenefitName))
                    .GroupBy(q => q.BenefitName)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.BenefitName)
                    .Select(q => q.BenefitName)
                    .ToList();
            }
        }

        public static List<string> GetDistinctBenefitNameByProductVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                return db.TreatyPricingRateTableVersions
                    .Where(q => ids.Contains(q.TreatyPricingRateTableId))
                    .Where(q => !string.IsNullOrEmpty(q.BenefitName))
                    .GroupBy(q => q.BenefitName)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.BenefitName)
                    .Select(q => q.BenefitName)
                    .ToList();
            }
        }

        public static List<string> GetDistinctEffectiveDateByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                var effectiveDates = new List<string> { };

                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productIds.Contains(q.TreatyPricingProductVersion.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                var dates = db.TreatyPricingRateTableVersions
                    .Where(q => ids.Contains(q.TreatyPricingRateTableId))
                    .Where(q => q.EffectiveDate.HasValue)
                    .GroupBy(q => q.EffectiveDate)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.EffectiveDate)
                    .Select(q => q.EffectiveDate.Value)
                    .ToList();

                foreach (var date in dates)
                {
                    effectiveDates.Add(date.ToString(Util.GetDateFormat()));
                }

                return effectiveDates;
            }
        }

        public static List<string> GetDistinctEffectiveDateByProductVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                var effectiveDates = new List<string> { };

                var ids = db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTableId.Value)
                    .ToList();

                var dates = db.TreatyPricingRateTableVersions
                    .Where(q => ids.Contains(q.TreatyPricingRateTableId))
                    .Where(q => q.EffectiveDate.HasValue)
                    .GroupBy(q => q.EffectiveDate)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.EffectiveDate)
                    .Select(q => q.EffectiveDate.Value)
                    .ToList();

                foreach (var date in dates)
                {
                    effectiveDates.Add(date.ToString(Util.GetDateFormat()));
                }

                return effectiveDates;
            }
        }

        public static List<string> GetDistinctCodeByProductIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => productIds.Contains(q.TreatyPricingProductVersion.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTable)
                    .GroupBy(q => q.Code)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.Code)
                    .Select(q => q.Code)
                    .ToList();
            }
        }

        public static List<string> GetDistinctCodeByProductVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => productVersionIds.Contains(q.TreatyPricingProductVersionId))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .Select(q => q.TreatyPricingRateTable)
                    .GroupBy(q => q.Code)
                    .Select(g => g.FirstOrDefault())
                    .OrderBy(q => q.Code)
                    .Select(q => q.Code)
                    .ToList();
            }
        }

        public static int CountByTreatyPricingRateTableGroupId(int treatyPricingRateTableGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTables.Where(q => q.TreatyPricingRateTableGroupId == treatyPricingRateTableGroupId).Count();
            }
        }

        public static Result Save(ref TreatyPricingRateTableBo bo)
        {
            if (!TreatyPricingRateTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingRateTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingRateTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingRateTable treatyPricingRateTable)
        {
            return treatyPricingRateTable.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingRateTableBo bo)
        {
            TreatyPricingRateTable entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Rate Table ID", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingRateTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableBo bo)
        {
            Result result = Result();

            TreatyPricingRateTable entity = TreatyPricingRateTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Rate Table ID", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingRateTableGroupId = bo.TreatyPricingRateTableGroupId;
                entity.BenefitId = bo.BenefitId;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingRateTableBo bo)
        {
            TreatyPricingRateTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingRateTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingRateTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
