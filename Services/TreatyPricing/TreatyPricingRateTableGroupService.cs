using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingRateTableGroupService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingRateTableGroup)),
                Controller = ModuleBo.ModuleController.TreatyPricingRateTableGroup.ToString()
            };
        }

        public static Expression<Func<TreatyPricingRateTableGroup, TreatyPricingRateTableGroupBo>> Expression()
        {
            return entity => new TreatyPricingRateTableGroupBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                NoOfRateTable = entity.NoOfRateTable,
                Status = entity.Status,
                Errors = entity.Errors,

                UploadedAt = entity.UploadedAt,
                UploadedById = entity.UploadedById,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingRateTableGroupBo FormBo(TreatyPricingRateTableGroup entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingRateTableGroupBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                NoOfRateTable = entity.NoOfRateTable,
                Status = entity.Status,
                StatusName = TreatyPricingRateTableGroupBo.GetStatusName(entity.Status),
                Errors = entity.Errors,
                FormattedErrors = !string.IsNullOrEmpty(entity.Errors) ? string.Join("\n", JsonConvert.DeserializeObject<List<string>>(entity.Errors).ToArray()) : "",

                UploadedAt = entity.UploadedAt,
                UploadedAtStr = entity.UploadedAt.ToString(Util.GetDateTimeFormat()),

                UploadedById = entity.UploadedById,
                UploadedByBo = UserService.Find(entity.UploadedById),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingRateTableGroupBo> FormBos(IList<TreatyPricingRateTableGroup> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingRateTableGroupBo> bos = new List<TreatyPricingRateTableGroupBo>() { };
            foreach (TreatyPricingRateTableGroup entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingRateTableGroup FormEntity(TreatyPricingRateTableGroupBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingRateTableGroup
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                Code = bo.Code,
                Name = bo.Name,
                Description = bo.Description,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                NoOfRateTable = bo.NoOfRateTable,
                Status = bo.Status,
                Errors = bo.Errors,

                UploadedAt = bo.UploadedAt,
                UploadedById = bo.UploadedById,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingRateTableGroup.IsExists(id);
        }

        public static TreatyPricingRateTableGroupBo Find(int? id)
        {
            return FormBo(TreatyPricingRateTableGroup.Find(id));
        }

        public static TreatyPricingRateTableGroupBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingRateTableGroups.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingRateTableGroupBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableGroups.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyPricingRateTableGroupBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableGroups.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).ToList(), false);
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductBenefits
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProductVersion.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingRateTableId.HasValue)
                    .GroupBy(q => q.TreatyPricingRateTable.TreatyPricingRateTableGroupId)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingRateTable.TreatyPricingRateTableGroup.Code)
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

                string prefix = string.Format("{0}_GRT_{1}_", cedantCode, year);

                var entity = db.TreatyPricingRateTableGroups.Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix)).OrderByDescending(q => q.Code).FirstOrDefault();

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
                return db.TreatyPricingRateTableGroups.Where(q => q.TreatyPricingCedantId == treatyPricingCedantId).Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableGroups.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingRateTableGroupBo bo)
        {
            if (!TreatyPricingRateTableGroup.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingRateTableGroupBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingRateTableGroup.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(TreatyPricingRateTableGroup treatyPricingRateTableGroup)
        {
            return treatyPricingRateTableGroup.IsDuplicateCode();
        }

        public static Result Create(ref TreatyPricingRateTableGroupBo bo)
        {
            TreatyPricingRateTableGroup entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Rate Table Group ID", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingRateTableGroupBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableGroupBo bo)
        {
            Result result = Result();

            TreatyPricingRateTableGroup entity = TreatyPricingRateTableGroup.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Rate Table Group ID", bo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.NoOfRateTable = bo.NoOfRateTable;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;

                entity.UploadedAt = bo.UploadedAt;
                entity.UploadedById = bo.UploadedById;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableGroupBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingRateTableGroupBo bo)
        {
            TreatyPricingRateTableGroup.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingRateTableGroupBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingRateTableGroup.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
