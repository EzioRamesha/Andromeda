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
    public class SanctionKeywordService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionKeyword)),
                Controller = ModuleBo.ModuleController.SanctionKeyword.ToString()
            };
        }

        public static Expression<Func<SanctionKeyword, SanctionKeywordBo>> Expression()
        {
            return entity => new SanctionKeywordBo
            {
                Id = entity.Id,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionKeywordBo FormBo(SanctionKeyword entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionKeywordBo
            {
                Id = entity.Id,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionKeywordBo> FormBos(IList<SanctionKeyword> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionKeywordBo> bos = new List<SanctionKeywordBo>() { };
            foreach (SanctionKeyword entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionKeyword FormEntity(SanctionKeywordBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionKeyword
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionKeyword.IsExists(id);
        }

        public static SanctionKeywordBo Find(int id)
        {
            return FormBo(SanctionKeyword.Find(id));
        }

        public static IList<SanctionKeywordBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionKeywords.ToList());
            }
        }

        public static bool IsDuplicateCode(SanctionKeyword sanctionKeyword)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(sanctionKeyword.Code))
                {
                    var query = db.SanctionKeywords.Where(q => q.Code.Trim().Equals(sanctionKeyword.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (sanctionKeyword.Id != 0)
                    {
                        query = query.Where(q => q.Id != sanctionKeyword.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Result Save(ref SanctionKeywordBo bo)
        {
            if (!SanctionKeyword.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SanctionKeywordBo bo, ref TrailObject trail)
        {
            if (!SanctionKeyword.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionKeywordBo bo)
        {
            SanctionKeyword entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Group", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionKeywordBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionKeywordBo bo)
        {
            Result result = Result();

            SanctionKeyword entity = SanctionKeyword.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Group", bo.Code);
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionKeywordBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionKeywordBo bo)
        {
            SanctionKeywordDetailService.DeleteBySanctionKeywordId(bo.Id);
            SanctionKeyword.Delete(bo.Id);
        }

        public static Result Delete(SanctionKeywordBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                SanctionKeywordDetailService.DeleteBySanctionKeywordId(bo.Id, ref trail);
                DataTrail dataTrail = SanctionKeyword.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
