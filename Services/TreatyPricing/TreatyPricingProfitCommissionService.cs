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
    public class TreatyPricingProfitCommissionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProfitCommission)),
                Controller = ModuleBo.ModuleController.TreatyPricingProfitCommission.ToString()
            };
        }

        public static Expression<Func<TreatyPricingProfitCommission, TreatyPricingProfitCommissionBo>> Expression()
        {
            return entity => new TreatyPricingProfitCommissionBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                BenefitCode = entity.BenefitCode,
                Description = entity.Description,
                Status = entity.Status,
                EffectiveDate = entity.EffectiveDate,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Entitlement = entity.Entitlement,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingProfitCommissionBo FormBo(TreatyPricingProfitCommission entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingProfitCommissionBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                BenefitCode = entity.BenefitCode,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingProfitCommissionBo.GetStatusName(entity.Status),
                EffectiveDate = entity.EffectiveDate,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Entitlement = entity.Entitlement,
                Remark = entity.Remark,
                TreatyPricingProfitCommissionVersionBos = foreign ? TreatyPricingProfitCommissionVersionService.GetByTreatyPricingProfitCommissionId(entity.Id) : null,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingProfitCommissionBo> FormBos(IList<TreatyPricingProfitCommission> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProfitCommissionBo> bos = new List<TreatyPricingProfitCommissionBo>() { };
            foreach (TreatyPricingProfitCommission entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingProfitCommission FormEntity(TreatyPricingProfitCommissionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProfitCommission
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                BenefitCode = bo.BenefitCode,
                Description = bo.Description,
                Status = bo.Status,
                EffectiveDate = bo.EffectiveDate,
                StartDate = bo.StartDate,
                EndDate = bo.EndDate,
                Entitlement = bo.Entitlement,
                Remark = bo.Remark,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProfitCommission.IsExists(id);
        }

        public static TreatyPricingProfitCommissionBo Find(int? id)
        {
            return FormBo(TreatyPricingProfitCommission.Find(id));
        }

        public static TreatyPricingProfitCommissionBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingProfitCommissions.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingProfitCommissionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissions.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingProfitCommissionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissions.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static IList<TreatyPricingProfitCommissionBo> GetByTreatyPricingCedantIds(List<int> treatyPricingCedantIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProfitCommissions.Where(q => treatyPricingCedantIds.Contains(q.TreatyPricingCedantId)).ToList(), false);
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingProfitCommissionId.HasValue)
                    .GroupBy(q => q.TreatyPricingProfitCommissionId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingProfitCommission.Code)
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

                string prefix = string.Format("{0}_PC_{1}_", cedantCode, year);

                var entity = db.TreatyPricingProfitCommissions.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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
                return db.TreatyPricingProfitCommissions.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissions.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingProfitCommissionBo bo)
        {
            if (!TreatyPricingProfitCommission.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingProfitCommissionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingProfitCommission.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingProfitCommission treatyPricingProfitCommission)
        {
            return treatyPricingProfitCommission.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingProfitCommissionBo bo)
        {
            TreatyPricingProfitCommission entity = FormEntity(bo);

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

        public static Result Create(ref TreatyPricingProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionBo bo)
        {
            Result result = Result();

            TreatyPricingProfitCommission entity = TreatyPricingProfitCommission.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Profit Commission ID", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.BenefitCode = bo.BenefitCode;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.EffectiveDate = bo.EffectiveDate;
                entity.StartDate = bo.StartDate;
                entity.EndDate = bo.EndDate;
                entity.Entitlement = bo.Entitlement;
                entity.Remark = bo.Remark;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProfitCommissionBo bo)
        {
            TreatyPricingProfitCommission.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProfitCommissionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProfitCommission.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
