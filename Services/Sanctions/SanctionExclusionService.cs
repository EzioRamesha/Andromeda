using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SanctionExclusionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionExclusion)),
                Controller = ModuleBo.ModuleController.SanctionExclusion.ToString()
            };
        }

        public static Expression<Func<SanctionExclusion, SanctionExclusionBo>> Expression()
        {
            return entity => new SanctionExclusionBo
            {
                Id = entity.Id,
                Keyword = entity.Keyword,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionExclusionBo FormBo(SanctionExclusion entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionExclusionBo
            {
                Id = entity.Id,
                Keyword = entity.Keyword,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionExclusionBo> FormBos(IList<SanctionExclusion> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionExclusionBo> bos = new List<SanctionExclusionBo>() { };
            foreach (SanctionExclusion entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionExclusion FormEntity(SanctionExclusionBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionExclusion
            {
                Id = bo.Id,
                Keyword = bo.Keyword?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionExclusion.IsExists(id);
        }

        public static bool IsExists(string value)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionExclusions.Any(q => q.Keyword.ToUpper() == value);
            }
        }

        public static List<string> FormatName(string[] splitNames)
        {
            List<string> formattedName = new List<string>();
            foreach (string splitName in splitNames)
            {
                if (!IsExists(splitName))
                {
                    formattedName.Add(splitName);
                }
            }

            return formattedName;
        }

        public static SanctionExclusionBo Find(int id)
        {
            return FormBo(SanctionExclusion.Find(id));
        }

        public static IList<SanctionExclusionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionExclusions.ToList());
            }
        }

        public static bool IsDuplicateName(SanctionExclusion sanctionExclusion)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(sanctionExclusion.Keyword?.Trim()))
                {
                    var query = db.SanctionExclusions.Where(q => q.Keyword.Trim().Equals(sanctionExclusion.Keyword.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (sanctionExclusion.Id != 0)
                    {
                        query = query.Where(q => q.Id != sanctionExclusion.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Result Save(ref SanctionExclusionBo bo)
        {
            if (!SanctionExclusion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SanctionExclusionBo bo, ref TrailObject trail)
        {
            if (!SanctionExclusion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionExclusionBo bo)
        {
            SanctionExclusion entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateName(entity))
            {
                result.AddTakenError("Keyword", bo.Keyword);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionExclusionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionExclusionBo bo)
        {
            Result result = Result();

            SanctionExclusion entity = SanctionExclusion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateName(FormEntity(bo)))
            {
                result.AddTakenError("Keyword", bo.Keyword);
            }

            if (result.Valid)
            {
                entity.Keyword = bo.Keyword;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionExclusionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionExclusionBo bo)
        {
            SanctionExclusion.Delete(bo.Id);
        }

        public static Result Delete(SanctionExclusionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionExclusion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
