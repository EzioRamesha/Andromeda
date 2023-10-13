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
    public class TreatyPricingUwLimitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwLimit)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwLimit.ToString()
            };
        }

        public static Expression<Func<TreatyPricingUwLimit, TreatyPricingUwLimitBo>> Expression()
        {
            return entity => new TreatyPricingUwLimitBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                LimitId = entity.LimitId,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                BenefitCode = entity.BenefitCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingUwLimitBo FormBo(TreatyPricingUwLimit entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingUwLimitBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                LimitId = entity.LimitId,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                BenefitCode = entity.BenefitCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TreatyPricingUwLimitVersionBos = foreign ? TreatyPricingUwLimitVersionService.GetByTreatyPricingUwLimitId(entity.Id) : null,

                StatusName = TreatyPricingUwLimitBo.GetStatusName(entity.Status),
            };
        }

        public static IList<TreatyPricingUwLimitBo> FormBos(IList<TreatyPricingUwLimit> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwLimitBo> bos = new List<TreatyPricingUwLimitBo>() { };
            foreach (TreatyPricingUwLimit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingUwLimit FormEntity(TreatyPricingUwLimitBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwLimit
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                LimitId = bo.LimitId,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,
                BenefitCode = bo.BenefitCode,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwLimit.IsExists(id);
        }

        public static TreatyPricingUwLimitBo Find(int? id, bool foreign = true)
        {
            return FormBo(TreatyPricingUwLimit.Find(id), foreign);
        }

        public static IList<TreatyPricingUwLimitBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimits.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingUwLimitBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwLimits
                    .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
                    .ToList(), false);
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingUwLimitId.HasValue)
                    .GroupBy(q => q.TreatyPricingUwLimit.Id)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingUwLimit.Id.ToString())
                    .ToList();
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_UWL_{1}_", cedantCode, year);

                var entity = db.TreatyPricingUwLimits.Where(q => !string.IsNullOrEmpty(q.LimitId) && q.LimitId.StartsWith(prefix)).OrderByDescending(q => q.LimitId).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.LimitId;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        public static List<string> GetBenefitCodesById(int? id)
        {
            var bo = Find(id, false);
            var benefitCodes = new List<string>();

            if (bo != null)
            {
                if (!string.IsNullOrEmpty(bo.BenefitCode))
                {
                    if (bo.BenefitCode.Contains(','))
                        benefitCodes.AddRange(bo.BenefitCode.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        benefitCodes.Add(bo.BenefitCode);
                }
            }

            return benefitCodes;
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwLimits.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingUwLimitBo bo)
        {
            if (!TreatyPricingUwLimit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwLimitBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwLimit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwLimitBo bo)
        {
            TreatyPricingUwLimit entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwLimitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwLimitBo bo)
        {
            Result result = Result();

            TreatyPricingUwLimit entity = TreatyPricingUwLimit.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.LimitId = bo.LimitId;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.BenefitCode = bo.BenefitCode;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwLimitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwLimitBo bo)
        {
            TreatyPricingUwLimit.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwLimitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwLimit.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            return TreatyPricingUwLimit.DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
        }

        public static void DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwLimit)));
                }
            }
        }
    }
}
