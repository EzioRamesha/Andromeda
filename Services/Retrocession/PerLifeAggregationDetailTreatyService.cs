using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeAggregationDetailTreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationDetailTreaty)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationDetailTreaty.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationDetailTreaty, PerLifeAggregationDetailTreatyBo>> Expression()
        {
            return entity => new PerLifeAggregationDetailTreatyBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailId = entity.PerLifeAggregationDetailId,
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationDetailTreatyBo FormBo(PerLifeAggregationDetailTreaty entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationDetailTreatyBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailId = entity.PerLifeAggregationDetailId,
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(entity.PerLifeAggregationDetailId),
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationDetailTreatyBo> FormBos(IList<PerLifeAggregationDetailTreaty> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationDetailTreatyBo> bos = new List<PerLifeAggregationDetailTreatyBo>() { };
            foreach (PerLifeAggregationDetailTreaty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregationDetailTreaty FormEntity(PerLifeAggregationDetailTreatyBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationDetailTreaty
            {
                Id = bo.Id,
                PerLifeAggregationDetailId = bo.PerLifeAggregationDetailId,
                TreatyCode = bo.TreatyCode,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationDetailTreaty.IsExists(id);
        }

        public static PerLifeAggregationDetailTreatyBo Find(int? id)
        {
            return FormBo(PerLifeAggregationDetailTreaty.Find(id));
        }

        public static IList<PerLifeAggregationDetailTreatyBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetailTreaties.ToList());
            }
        }

        public static IList<PerLifeAggregationDetailTreatyBo> GetByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetailTreaties
                    .Where(q => q.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                    .ToList());
            }
        }

        public static Result Save(ref PerLifeAggregationDetailTreatyBo bo)
        {
            if (!PerLifeAggregationDetailTreaty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationDetailTreatyBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationDetailTreaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationDetailTreatyBo bo)
        {
            PerLifeAggregationDetailTreaty entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationDetailTreatyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailTreatyBo bo)
        {
            Result result = Result();

            PerLifeAggregationDetailTreaty entity = PerLifeAggregationDetailTreaty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationDetailId = bo.PerLifeAggregationDetailId;
                entity.TreatyCode = bo.TreatyCode;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailTreatyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationDetailTreatyBo bo)
        {
            PerLifeAggregationDetailTreaty.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationDetailTreatyBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationDetailTreaty.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            return PerLifeAggregationDetailTreaty.DeleteByPerLifeAggregationDetailId(perLifeAggregationDetailId);
        }

        public static void DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationDetailId(perLifeAggregationDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregationDetailTreaty)));
                }
            }
        }
    }
}
