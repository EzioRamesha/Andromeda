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
using System.Text.RegularExpressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingClaimApprovalLimitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingClaimApprovalLimit)),
                Controller = ModuleBo.ModuleController.TreatyPricingClaimApprovalLimit.ToString()
            };
        }

        public static Expression<Func<TreatyPricingClaimApprovalLimit, TreatyPricingClaimApprovalLimitBo>> Expression()
        {
            return entity => new TreatyPricingClaimApprovalLimitBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                BenefitCode = entity.BenefitCode,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingClaimApprovalLimitBo.GetStatusName(entity.Status),
                Remarks = entity.Remarks,
                Errors = entity.Errors,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingClaimApprovalLimitBo FormBo(TreatyPricingClaimApprovalLimit entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingClaimApprovalLimitBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingClaimApprovalLimitBo.GetStatusName(entity.Status),
                BenefitCode = entity.BenefitCode,
                Remarks = entity.Remarks,
                Amount = Util.StringToNumber(GetAmountFromVersionById(entity.Id)),
                Errors = entity.Errors,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                TreatyPricingClaimApprovalLimitVersionBos = foreign ? TreatyPricingClaimApprovalLimitVersionService.GetByTreatyPricingClaimApprovalLimitId(entity.Id) : null,

            };
        }

        public static string GetAmountFromVersionById(int id)
        {
            using (var db = new AppDbContext())
            {
                var amt = db.TreatyPricingClaimApprovalLimitVersions.Where(q => q.TreatyPricingClaimApprovalLimitId == id).OrderByDescending(q => q.Id).FirstOrDefault();
                return amt.Amount;
            }
        }

        public static IList<TreatyPricingClaimApprovalLimitBo> FormBos(IList<TreatyPricingClaimApprovalLimit> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingClaimApprovalLimitBo> bos = new List<TreatyPricingClaimApprovalLimitBo>() { };
            foreach (TreatyPricingClaimApprovalLimit entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingClaimApprovalLimit FormEntity(TreatyPricingClaimApprovalLimitBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingClaimApprovalLimit
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                BenefitCode = bo.BenefitCode,
                Description = bo.Description,
                Status = bo.Status,
                Remarks = bo.Remarks,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingClaimApprovalLimit.IsExists(id);
        }

        public static TreatyPricingClaimApprovalLimitBo Find(int? id, bool foreign = true)
        {
            return FormBo(TreatyPricingClaimApprovalLimit.Find(id), foreign);
        }

        public static IList<TreatyPricingClaimApprovalLimitBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingClaimApprovalLimits.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingClaimApprovalLimitBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingClaimApprovalLimits.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_CL_{1}_", cedantCode, year);

                var entity = db.TreatyPricingClaimApprovalLimits.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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
                return db.TreatyPricingClaimApprovalLimits.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static Result Save(ref TreatyPricingClaimApprovalLimitBo bo)
        {
            if (!TreatyPricingClaimApprovalLimit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingClaimApprovalLimitBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingClaimApprovalLimit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingClaimApprovalLimit treatyPricingClaimApprovalLimit)
        {
            return treatyPricingClaimApprovalLimit.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingClaimApprovalLimitBo bo)
        {
            TreatyPricingClaimApprovalLimit entity = FormEntity(bo);

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

        public static Result Create(ref TreatyPricingClaimApprovalLimitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingClaimApprovalLimitBo bo)
        {
            Result result = Result();

            TreatyPricingClaimApprovalLimit entity = TreatyPricingClaimApprovalLimit.Find(bo.Id);
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
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.BenefitCode = bo.BenefitCode;
                entity.Remarks = bo.Remarks;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }
        public static Result Update(ref TreatyPricingClaimApprovalLimitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingClaimApprovalLimitBo bo)
        {
            TreatyPricingClaimApprovalLimit.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingClaimApprovalLimitBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingClaimApprovalLimit.Delete(bo.Id);
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
                    .Where(q => q.TreatyPricingClaimApprovalLimitId.HasValue)
                    .GroupBy(q => q.TreatyPricingClaimApprovalLimit.Id)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingClaimApprovalLimit.Code)
                    .ToList();
            }
        }
    }
}
