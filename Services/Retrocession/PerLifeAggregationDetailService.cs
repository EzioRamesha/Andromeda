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
    public class PerLifeAggregationDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationDetail)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationDetail.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationDetail, PerLifeAggregationDetailBo>> Expression()
        {
            return entity => new PerLifeAggregationDetailBo
            {
                Id = entity.Id,
                PerLifeAggregationId = entity.PerLifeAggregationId,
                RiskQuarter = entity.RiskQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationDetailBo FormBo(PerLifeAggregationDetail entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationDetailBo
            {
                Id = entity.Id,
                PerLifeAggregationId = entity.PerLifeAggregationId,
                PerLifeAggregationBo = PerLifeAggregationService.Find(entity.PerLifeAggregationId),
                RiskQuarter = entity.RiskQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationDetailBo> FormBos(IList<PerLifeAggregationDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationDetailBo> bos = new List<PerLifeAggregationDetailBo>() { };
            foreach (PerLifeAggregationDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregationDetail FormEntity(PerLifeAggregationDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationDetail
            {
                Id = bo.Id,
                PerLifeAggregationId = bo.PerLifeAggregationId,
                RiskQuarter = bo.RiskQuarter,
                ProcessingDate = bo.ProcessingDate,
                Status = bo.Status,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationDetail.IsExists(id);
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails.Where(q => q.Status == status).Count();
            }
        }

        public static PerLifeAggregationDetailBo Find(int? id)
        {
            return FormBo(PerLifeAggregationDetail.Find(id));
        }

        public static PerLifeAggregationDetailBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeAggregationDetails
                    .Where(q => q.Status == status)
                    .FirstOrDefault());
            }
        }

        public static IList<PerLifeAggregationDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetails.ToList());
            }
        }

        public static IList<PerLifeAggregationDetailBo> GetByPerLifeAggregationId(int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetails
                    .Where(q => q.PerLifeAggregationId == perLifeAggregationId)
                    .OrderBy(q => q.RiskQuarter)
                    .ToList());
            }
        }

        public static int CountByPerLifeAggregationId(int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails
                    .Where(q => q.PerLifeAggregationId == perLifeAggregationId)
                    .Count();
            }
        }

        public static bool IsNotStatusByPerLifeAggregationId(int status, int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails
                    .Where(q => q.PerLifeAggregationId == perLifeAggregationId)
                    .Where(q => q.Status != status)
                    .Any();
            }
        }

        public static bool IsNotStatusesPerLifeAggregationId(List<int> statuses, int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails
                    .Where(q => q.PerLifeAggregationId == perLifeAggregationId)
                    .Where(q => !statuses.Contains(q.Status))
                    .Any();
            }
        }

        public static bool IsStatusesPerLifeAggregationId(List<int> statuses, int perLifeAggregationId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetails
                    .Where(q => q.PerLifeAggregationId == perLifeAggregationId)
                    .Where(q => statuses.Contains(q.Status))
                    .Any();
            }
        }

        public static Result Save(ref PerLifeAggregationDetailBo bo)
        {
            if (!PerLifeAggregationDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationDetailBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationDetailBo bo)
        {
            PerLifeAggregationDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailBo bo)
        {
            Result result = Result();

            PerLifeAggregationDetail entity = PerLifeAggregationDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationId = bo.PerLifeAggregationId;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.ProcessingDate = bo.ProcessingDate;
                entity.Status = bo.Status;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationDetailBo bo)
        {
            PerLifeAggregationDetailTreatyService.DeleteByPerLifeAggregationDetailId(bo.Id);
            PerLifeAggregatedDataService.DeleteByPerLifeAggregationDetailId(bo.Id);
            PerLifeAggregationDetail.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                PerLifeAggregationDetailTreatyService.DeleteByPerLifeAggregationDetailId(bo.Id, ref trail);
                PerLifeAggregatedDataService.DeleteByPerLifeAggregationDetailId(bo.Id, ref trail);
                DataTrail dataTrail = PerLifeAggregationDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        // TODO: Add delete child function
        public static IList<DataTrail> DeleteByPerLifeAggregationId(int perLifeAggregationId)
        {
            return PerLifeAggregationDetail.DeleteByPerLifeAggregationId(perLifeAggregationId);
        }

        public static void DeleteByPerLifeAggregationId(int perLifeAggregationId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationId(perLifeAggregationId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregationDetail)));
                }
            }
        }
    }
}
