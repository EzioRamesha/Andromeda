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
    public class TreatyPricingGroupReferralHipsTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralHipsTable)),
            };
        }

        public static TreatyPricingGroupReferralHipsTableBo FormBo(TreatyPricingGroupReferralHipsTable entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingGroupReferralHipsTableBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralId = entity.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = entity.TreatyPricingGroupReferralFileId,

                HipsCategoryId = entity.HipsCategoryId,
                HipsSubCategoryId = entity.HipsSubCategoryId,
                CoverageStartDate = entity.CoverageStartDate,
                Description = entity.Description,
                PlanA = entity.PlanA,
                PlanB = entity.PlanB,
                PlanC = entity.PlanC,
                PlanD = entity.PlanD,
                PlanE = entity.PlanE,
                PlanF = entity.PlanF,
                PlanG = entity.PlanG,
                PlanH = entity.PlanH,
                PlanI = entity.PlanI,
                PlanJ = entity.PlanJ,
                PlanK = entity.PlanK,
                PlanL = entity.PlanL,
                PlanM = entity.PlanM,
                PlanN = entity.PlanN,
                PlanO = entity.PlanO,
                PlanP = entity.PlanP,
                PlanQ = entity.PlanQ,
                PlanR = entity.PlanR,
                PlanS = entity.PlanS,
                PlanT = entity.PlanT,
                PlanU = entity.PlanU,
                PlanV = entity.PlanV,
                PlanW = entity.PlanW,
                PlanX = entity.PlanX,
                PlanY = entity.PlanY,
                PlanZ = entity.PlanZ,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.TreatyPricingGroupReferralBo = TreatyPricingGroupReferralService.Find(entity.TreatyPricingGroupReferralId);
                bo.TreatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileService.Find(entity.TreatyPricingGroupReferralFileId);
                bo.HipsCategoryBo = HipsCategoryService.Find(entity.HipsCategoryId);
                bo.HipsSubCategoryBo = HipsCategoryDetailService.Find(entity.HipsSubCategoryId);
            }

            bo.CoverageStartDateStr = entity.CoverageStartDate?.ToString(Util.GetDateFormat());
            bo.CategoryCode = HipsCategoryService.Find(entity.HipsCategoryId)?.Code;
            bo.SubCategoryCode = HipsCategoryDetailService.Find(entity.HipsSubCategoryId)?.Subcategory;

            return bo;
        }

        public static TreatyPricingGroupReferralHipsTable FormEntity(TreatyPricingGroupReferralHipsTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralHipsTable
            {
                Id = bo.Id,
                TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId,
                TreatyPricingGroupReferralFileId = bo.TreatyPricingGroupReferralFileId,

                HipsCategoryId = bo.HipsCategoryId,
                HipsSubCategoryId = bo.HipsSubCategoryId,
                CoverageStartDate = bo.CoverageStartDate,
                Description = bo.Description,
                PlanA = bo.PlanA,
                PlanB = bo.PlanB,
                PlanC = bo.PlanC,
                PlanD = bo.PlanD,
                PlanE = bo.PlanE,
                PlanF = bo.PlanF,
                PlanG = bo.PlanG,
                PlanH = bo.PlanH,
                PlanI = bo.PlanI,
                PlanJ = bo.PlanJ,
                PlanK = bo.PlanK,
                PlanL = bo.PlanL,
                PlanM = bo.PlanM,
                PlanN = bo.PlanN,
                PlanO = bo.PlanO,
                PlanP = bo.PlanP,
                PlanQ = bo.PlanQ,
                PlanR = bo.PlanR,
                PlanS = bo.PlanS,
                PlanT = bo.PlanT,
                PlanU = bo.PlanU,
                PlanV = bo.PlanV,
                PlanW = bo.PlanW,
                PlanX = bo.PlanX,
                PlanY = bo.PlanY,
                PlanZ = bo.PlanZ,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<TreatyPricingGroupReferralHipsTableBo> FormBos(IList<TreatyPricingGroupReferralHipsTable> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralHipsTableBo> bos = new List<TreatyPricingGroupReferralHipsTableBo>() { };
            foreach (TreatyPricingGroupReferralHipsTable entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralHipsTable.IsExists(id);
        }

        public static TreatyPricingGroupReferralHipsTableBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralHipsTable.Find(id));
        }

        public static int CountByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId, AppDbContext db)
        {
            return db.TreatyPricingGroupReferralHipsTables
                .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                .Count();
        }

        public static IList<TreatyPricingGroupReferralHipsTableBo> GetByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralHipsTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .OrderBy(q => q.Id)
                    .ToList());
            }
        }

        public static TreatyPricingGroupReferralHipsTableBo GetByTreatyPricingGroupReferralIdHipsSubCategory(int treatyPricingGroupReferralId, int hipsSubCategoryId)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingGroupReferralHipsTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Where(q => q.HipsSubCategoryId == hipsSubCategoryId)
                    .FirstOrDefault());
            }
        }

        public static List<int> GetHipsSubCategoryIdByTreatyPricingGroupReferralIdForHipsReport(int treatyPricingGroupReferralId, List<int> hipsCategoryIds)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralHipsTables
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Where(q => q.HipsCategoryId.HasValue && hipsCategoryIds.Contains(q.HipsCategoryId.Value))
                    .Where(q => q.HipsSubCategoryId.HasValue)
                    .OrderBy(q => q.HipsSubCategoryId)
                    .Select(q => q.HipsSubCategoryId.Value)
                    .Distinct()
                    .ToList();
            }
        }

        public static IList<TreatyPricingGroupReferralHipsTableBo> Get()
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.TreatyPricingGroupReferralHipsTables
                    .ToList());
            }
        }

        public static int CountByHipsCategoryId(int hipsCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralHipsTables.Where(q => q.HipsCategoryId == hipsCategoryId).Count();
            }
        }

        public static int CountByHipsSubCategoryId(int hipsSubCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralHipsTables.Where(q => q.HipsSubCategoryId == hipsSubCategoryId).Count();
            }
        }

        public static int CountHipsSubCategoryByHipsCategoryId(int hipsCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralHipsTables.Where(q => q.HipsSubCategory.HipsCategoryId == hipsCategoryId).Count();
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralHipsTableBo bo)
        {
            if (!TreatyPricingGroupReferralHipsTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingGroupReferralHipsTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralHipsTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralHipsTableBo bo)
        {
            TreatyPricingGroupReferralHipsTable entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref TreatyPricingGroupReferralHipsTableBo bo, AppDbContext db)
        {
            TreatyPricingGroupReferralHipsTable entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref TreatyPricingGroupReferralHipsTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralHipsTableBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralHipsTable entity = TreatyPricingGroupReferralHipsTable.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralHipsTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralHipsTableBo bo)
        {
            TreatyPricingGroupReferralHipsTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralHipsTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyPricingGroupReferralHipsTable.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteByTreatyPricingGroupReferralId(int treatyPricingGroupReferralId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [TreatyPricingGroupReferralHipsTables] WHERE [TreatyPricingGroupReferralId] = {0}", treatyPricingGroupReferralId);
                db.SaveChanges();

                return trails;
            }
        }

        public static List<int?> GetDistinctSubCategory()
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralHipsTables.Where(q => q.HipsSubCategoryId.HasValue).Select(q => q.HipsSubCategoryId).Distinct().OrderBy(q => q).ToList();
            }
        }
    }
}
