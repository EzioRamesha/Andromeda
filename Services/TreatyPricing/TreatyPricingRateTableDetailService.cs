using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingRateTableDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingRateTableDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingRateTableDetail.ToString()
            };
        }

        public static Expression<Func<TreatyPricingRateTableDetail, TreatyPricingRateTableDetailBo>> Expression()
        {
            return entity => new TreatyPricingRateTableDetailBo
            {
                Id = entity.Id,
                TreatyPricingRateTableVersionId = entity.TreatyPricingRateTableVersionId,
                Type = entity.Type,
                Col1 = entity.Col1,
                Col2 = entity.Col2,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingRateTableDetailBo FormBo(TreatyPricingRateTableDetail entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingRateTableDetailBo
            {
                Id = entity.Id,
                TreatyPricingRateTableVersionId = entity.TreatyPricingRateTableVersionId,
                Type = entity.Type,
                Col1 = entity.Col1,
                Col2 = entity.Col2,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingRateTableDetailBo> FormBos(IList<TreatyPricingRateTableDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingRateTableDetailBo> bos = new List<TreatyPricingRateTableDetailBo>() { };
            foreach (TreatyPricingRateTableDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingRateTableDetail FormEntity(TreatyPricingRateTableDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingRateTableDetail
            {
                Id = bo.Id,
                TreatyPricingRateTableVersionId = bo.TreatyPricingRateTableVersionId,
                Type = bo.Type,
                Col1 = bo.Col1,
                Col2 = bo.Col2,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingRateTableDetail.IsExists(id);
        }

        public static TreatyPricingRateTableDetailBo Find(int? id)
        {
            return FormBo(TreatyPricingRateTableDetail.Find(id));
        }

        public static IList<TreatyPricingRateTableDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableDetails.ToList());
            }
        }

        public static IList<TreatyPricingRateTableDetailBo> GetByTreatyPricingRateTableVersionId(int treatyPricingRateTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableDetails.Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId).ToList());
            }
        }

        public static IList<TreatyPricingRateTableDetailBo> GetByTreatyPricingRateTableVersionIdType(int treatyPricingRateTableVersionId, int type, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingRateTableDetails
                    .Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId)
                    .Where(q => q.Type == type);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static string GetJsonByTreatyPricingRateTableVersionIdType(int treatyPricingRateTableVersionId, int type)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingRateTableVersionIdType(treatyPricingRateTableVersionId, type);

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByTreatyPricingRateTableVersionId(int treatyPricingRateTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableDetails.Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId).Count();
            }
        }

        public static void CopyBulk(int nextVersionId, int previousVersionId, int authUserId)
        {
            string script = "INSERT INTO [dbo].[TreatyPricingRateTableDetails]";
            string insertFields = string.Format("({0})", string.Join(",", TreatyPricingRateTableDetailBo.InsertFields()));
            string queryFields = string.Format("{0}", string.Join(",", TreatyPricingRateTableDetailBo.QueryFields()));
            string select = string.Format("SELECT {0},{1},GETDATE(),GETDATE(),{2},{2} FROM [dbo].[TreatyPricingRateTableDetails] WHERE TreatyPricingRateTableVersionId = {3}", nextVersionId, queryFields, authUserId, previousVersionId);
            string query = string.Format("{0}\n{1}\n{2};", script, insertFields, select);

            using (var db = new AppDbContext())
            {
                db.Database.ExecuteSqlCommand(query);
                db.SaveChanges();
            }
        }

        public static Result Save(ref TreatyPricingRateTableDetailBo bo)
        {
            if (!TreatyPricingRateTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingRateTableDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingRateTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingRateTableDetailBo bo)
        {
            TreatyPricingRateTableDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingRateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableDetailBo bo)
        {
            Result result = Result();

            TreatyPricingRateTableDetail entity = TreatyPricingRateTableDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingRateTableVersionId = bo.TreatyPricingRateTableVersionId;
                entity.Type = bo.Type;
                entity.Col1 = bo.Col1;
                entity.Col2 = bo.Col2;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingRateTableDetailBo bo)
        {
            TreatyPricingRateTableDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingRateTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingRateTableDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<TreatyPricingRateTableDetail> GetByTreatyPricingRateTableVersionIdExcept(int versionId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableDetails.Where(q => q.TreatyPricingRateTableVersionId == versionId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static void DeleteByTreatyPricingRateTableDetailIdExcept(int versionId, int type, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByTreatyPricingRateTableVersionIdType(versionId, type, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
