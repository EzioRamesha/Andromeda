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
    public class TreatyPricingDefinitionAndExclusionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingDefinitionAndExclusion)),
                Controller = ModuleBo.ModuleController.TreatyPricingDefinitionAndExclusion.ToString()
            };
        }

        public static Expression<Func<TreatyPricingDefinitionAndExclusion, TreatyPricingDefinitionAndExclusionBo>> Expression()
        {
            return entity => new TreatyPricingDefinitionAndExclusionBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Status = entity.Status,
                StatusName = TreatyPricingDefinitionAndExclusionBo.GetStatusName(entity.Status),
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                AdditionalRemarks = entity.AdditionalRemarks,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingDefinitionAndExclusionBo FormBo(TreatyPricingDefinitionAndExclusion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingDefinitionAndExclusionBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Status = entity.Status,
                StatusName = TreatyPricingDefinitionAndExclusionBo.GetStatusName(entity.Status),
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                AdditionalRemarks = entity.AdditionalRemarks,
                Errors = entity.Errors,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                TreatyPricingDefinitionAndExclusionVersionBos = foreign ? TreatyPricingDefinitionAndExclusionVersionService.GetByTreatyPricingDefinitionAndExclusionId(entity.Id) : null,
            };
        }

        public static IList<TreatyPricingDefinitionAndExclusionBo> FormBos(IList<TreatyPricingDefinitionAndExclusion> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingDefinitionAndExclusionBo> bos = new List<TreatyPricingDefinitionAndExclusionBo>() { };
            foreach (TreatyPricingDefinitionAndExclusion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingDefinitionAndExclusion FormEntity(TreatyPricingDefinitionAndExclusionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingDefinitionAndExclusion
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Status = bo.Status,
                Name = bo.Name,
                Description = bo.Description,
                BenefitCode = bo.BenefitCode,
                AdditionalRemarks = bo.AdditionalRemarks,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingDefinitionAndExclusion.IsExists(id);
        }

        public static TreatyPricingDefinitionAndExclusionBo Find(int? id)
        {
            return FormBo(TreatyPricingDefinitionAndExclusion.Find(id));
        }

        public static IList<TreatyPricingDefinitionAndExclusionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingDefinitionAndExclusions.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingDefinitionAndExclusionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingDefinitionAndExclusions.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static IList<TreatyPricingDefinitionAndExclusionBo> GetByTreatyPricingCedantId(int? treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingDefinitionAndExclusions.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList());
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_DE_{1}_", cedantCode, year);

                var entity = db.TreatyPricingDefinitionAndExclusions.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.Code;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        public static int CountByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingDefinitionAndExclusions.OrderBy(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static Result Save(ref TreatyPricingDefinitionAndExclusionBo bo)
        {
            if (!TreatyPricingDefinitionAndExclusion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingDefinitionAndExclusionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingDefinitionAndExclusion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingDefinitionAndExclusion treatyPricingDefinitionAndExclusion)
        {
            return treatyPricingDefinitionAndExclusion.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingDefinitionAndExclusionBo bo)
        {
            TreatyPricingDefinitionAndExclusion entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingDefinitionAndExclusionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingDefinitionAndExclusionBo bo)
        {
            Result result = Result();

            TreatyPricingDefinitionAndExclusion entity = TreatyPricingDefinitionAndExclusion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.Code = bo.Code;
                entity.Status = bo.Status;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.BenefitCode = bo.BenefitCode;
                entity.AdditionalRemarks = bo.AdditionalRemarks;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result; 
        }

        public static Result Update(ref TreatyPricingDefinitionAndExclusionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingDefinitionAndExclusionBo bo)
        {
            TreatyPricingDefinitionAndExclusion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingDefinitionAndExclusionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingDefinitionAndExclusion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingDefinitionAndExclusionId.HasValue)
                    .GroupBy(q => q.TreatyPricingDefinitionAndExclusion.Id)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingDefinitionAndExclusion.Code)
                    .ToList();
            }
        }

        public static IList<int> GetVersionByDefinitionAndExclusionId(int? definitionAndExclusionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingDefinitionAndExclusionVersions.Where(a => a.TreatyPricingDefinitionAndExclusionId == definitionAndExclusionId).Select(a => a.Version).ToList();
            }
        }

        public static TreatyPricingDefinitionAndExclusionBo FindByDefinitionAndExclusionCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingDefinitionAndExclusions.Where(a => a.Code == code).FirstOrDefault());
            }
        }
    }
}
