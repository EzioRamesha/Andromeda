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
    public class TreatyPricingCustomOtherService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCustomOther)),
                Controller = ModuleBo.ModuleController.TreatyPricingCustomOther.ToString()
            };
        }

        public static Expression<Func<TreatyPricingCustomOther, TreatyPricingCustomOtherBo>> Expression()
        {
            return entity => new TreatyPricingCustomOtherBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingCustomOtherBo.GetStatusName(entity.Status),
                Errors = entity.Errors,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingCustomOtherBo FormBo(TreatyPricingCustomOther entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCustomOtherBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                StatusName = TreatyPricingCustomOtherBo.GetStatusName(entity.Status),
                Errors = entity.Errors,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                TreatyPricingCustomOtherVersionBos = foreign ? TreatyPricingCustomOtherVersionService.GetByTreatyPricingCustomOtherId(entity.Id) : null,

            };
        }

        public static IList<TreatyPricingCustomOtherBo> FormBos(IList<TreatyPricingCustomOther> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCustomOtherBo> bos = new List<TreatyPricingCustomOtherBo>() { };
            foreach (TreatyPricingCustomOther entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingCustomOther FormEntity(TreatyPricingCustomOtherBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCustomOther
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingCustomOther.IsExists(id);
        }

        public static TreatyPricingCustomOtherBo Find(int? id)
        {
            return FormBo(TreatyPricingCustomOther.Find(id));
        }

        public static IList<TreatyPricingCustomOtherBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCustomOthers.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingCustomOtherBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCustomOthers.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_CS_{1}_", cedantCode, year);

                var entity = db.TreatyPricingCustomOthers.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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
                return db.TreatyPricingCustomOthers.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static Result Save(ref TreatyPricingCustomOtherBo bo)
        {
            if (!TreatyPricingCustomOther.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingCustomOtherBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingCustomOther.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingCustomOther treatyPricingCustomOther)
        {
            return treatyPricingCustomOther.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingCustomOtherBo bo)
        {
            TreatyPricingCustomOther entity = FormEntity(bo);

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

        public static Result Create(ref TreatyPricingCustomOtherBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCustomOtherBo bo)
        {
            Result result = Result();

            TreatyPricingCustomOther entity = TreatyPricingCustomOther.Find(bo.Id);
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
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }
        public static Result Update(ref TreatyPricingCustomOtherBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingCustomOtherBo bo)
        {
            TreatyPricingCustomOther.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingCustomOtherBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCustomOther.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
