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
    public class SanctionKeywordDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionKeywordDetail)),
                Controller = ModuleBo.ModuleController.SanctionKeywordDetail.ToString()
            };
        }

        public static Expression<Func<SanctionKeywordDetail, SanctionKeywordDetailBo>> Expression()
        {
            return entity => new SanctionKeywordDetailBo
            {
                Id = entity.Id,
                SanctionKeywordId = entity.SanctionKeywordId,
                Keyword = entity.Keyword,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionKeywordDetailBo FormBo(SanctionKeywordDetail entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionKeywordDetailBo
            {
                Id = entity.Id,
                Keyword = entity.Keyword,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionKeywordDetailBo> FormBos(IList<SanctionKeywordDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionKeywordDetailBo> bos = new List<SanctionKeywordDetailBo>() { };
            foreach (SanctionKeywordDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionKeywordDetail FormEntity(SanctionKeywordDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionKeywordDetail
            {
                Id = bo.Id,
                SanctionKeywordId = bo.SanctionKeywordId,
                Keyword = bo.Keyword?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionKeywordDetail.IsExists(id);
        }

        public static SanctionKeywordDetailBo Find(int id)
        {
            return FormBo(SanctionKeywordDetail.Find(id));
        }

        public static int CountByKeywordExceptSanctionKeywordId(string keyword, int sanctionKeywordId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywordDetails
                    .Where(q => q.Keyword == keyword)
                    .Where(q => q.SanctionKeywordId != sanctionKeywordId)
                    .Count();
            }
        }

        public static IList<SanctionKeywordDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionKeywordDetails.ToList());
            }
        }

        public static IList<SanctionKeywordDetailBo> GetBySanctionKeywordId(int sanctionKeywordId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionKeywordDetails.Where(q => q.SanctionKeywordId == sanctionKeywordId).ToList());
            }
        }

        public static IList<SanctionKeywordDetail> GetBySanctionKeywordIdExcept(int sanctionKeywordId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionKeywordDetails.Where(q => q.SanctionKeywordId == sanctionKeywordId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static IList<string> GetKeywords(string keyword)
        {
            using (var db = new AppDbContext())
            {
                var sanctionKeyword = db.SanctionKeywordDetails.Where(q => q.Keyword.ToUpper() == keyword).FirstOrDefault();
                if (sanctionKeyword == null)
                    return null;

                return db.SanctionKeywordDetails.Where(q => q.SanctionKeywordId == sanctionKeyword.SanctionKeywordId).Select(q => q.Keyword.ToUpper()).ToList();
            }
        }

        public static Result Save(ref SanctionKeywordDetailBo bo)
        {
            if (!SanctionKeywordDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionKeywordDetailBo bo, ref TrailObject trail)
        {
            if (!SanctionKeywordDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionKeywordDetailBo bo)
        {
            SanctionKeywordDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionKeywordDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionKeywordDetailBo bo)
        {
            Result result = Result();

            SanctionKeywordDetail entity = SanctionKeywordDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionKeywordId = bo.SanctionKeywordId;
                entity.Keyword = bo.Keyword;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionKeywordDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionKeywordDetailBo bo)
        {
            SanctionKeywordDetail.Delete(bo.Id);
        }

        public static Result Delete(SanctionKeywordDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionKeywordDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result DeleteBySanctionKeywordIdExcept(int sanctionKeywordId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<SanctionKeywordDetail> sanctionKeywordDetails = GetBySanctionKeywordIdExcept(sanctionKeywordId, saveIds);
            foreach (SanctionKeywordDetail sanctionKeywordDetail in sanctionKeywordDetails)
            {
                DataTrail dataTrail = SanctionKeywordDetail.Delete(sanctionKeywordDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteBySanctionKeywordId(int sanctionKeywordId)
        {
            return SanctionKeywordDetail.DeleteBySanctionKeywordId(sanctionKeywordId);
        }

        public static void DeleteBySanctionKeywordId(int sanctionKeywordId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionKeywordId(sanctionKeywordId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
