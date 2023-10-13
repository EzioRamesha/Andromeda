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
    public class SanctionBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionBatch)),
                Controller = ModuleBo.ModuleController.SanctionBatch.ToString()
            };
        }

        public static Expression<Func<SanctionBatch, SanctionBatchBo>> Expression()
        {
            return entity => new SanctionBatchBo
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Method = entity.Method,
                Status = entity.Status,
                Record = entity.Record,
                UploadedAt = entity.UploadedAt,
                Errors = entity.Errors,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionBatchBo FormBo(SanctionBatch entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionBatchBo
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                SourceBo = SourceService.Find(entity.SourceId),
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Method = entity.Method,
                Status = entity.Status,
                Record = entity.Record,
                UploadedAt = entity.UploadedAt,
                Errors = entity.Errors,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionBatchBo> FormBos(IList<SanctionBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionBatchBo> bos = new List<SanctionBatchBo>() { };
            foreach (SanctionBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionBatch FormEntity(SanctionBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionBatch
            {
                Id = bo.Id,
                SourceId = bo.SourceId,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Method = bo.Method,
                Status = bo.Status,
                Record = bo.Record,
                UploadedAt = bo.UploadedAt,
                Errors = bo.Errors,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionBatch.IsExists(id);
        }

        public static SanctionBatchBo Find(int id)
        {
            return FormBo(SanctionBatch.Find(id));
        }

        public static SanctionBatchBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.SanctionBatches.Where(q => q.Status == status).OrderByDescending(q => q.UploadedAt).FirstOrDefault());
            }
        }

        public static IList<SanctionBatchBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionBatches.ToList());
            }
        }

        public static IList<SanctionBatchBo> GetBySourceByStatuses(int sourceId, List<int> statuses = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionBatches.Where(q => q.SourceId == sourceId);

                if (statuses != null && statuses.Count > 0)
                {
                    query = query.Where(q => statuses.Contains(q.Status));
                }

                return FormBos(query.ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBatches.Where(q => q.Status == status).Count();
            }
        }

        public static int CountBySourceId(int sourceId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBatches.Where(q => q.SourceId == sourceId).Count();
            }
        }

        public static Result Save(ref SanctionBatchBo bo)
        {
            if (!SanctionBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SanctionBatchBo bo, ref TrailObject trail)
        {
            if (!SanctionBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionBatchBo bo)
        {
            SanctionBatch entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionBatchBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionBatchBo bo)
        {
            Result result = Result();

            SanctionBatch entity = SanctionBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SourceId = bo.SourceId;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Method = bo.Method;
                entity.Status = bo.Status;
                entity.Record = bo.Record;
                entity.UploadedAt = bo.UploadedAt;
                entity.Errors = bo.Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionBatchBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionBatchBo bo)
        {
            SanctionNameService.DeleteBySanctionBatchId(bo.Id);
            SanctionService.DeleteBySanctionBatchId(bo.Id);
            SanctionBatch.Delete(bo.Id);
        }

        public static Result Delete(SanctionBatchBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                SanctionFormatNameService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionNameService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionAddressService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionBirthDateService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionCommentService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionCountryService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionIdentityService.DeleteBySanctionBatchId(bo.Id, ref trail);
                SanctionService.DeleteBySanctionBatchId(bo.Id, ref trail);
                DataTrail dataTrail = SanctionBatch.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result Replace(ref SanctionBatchBo bo, ref TrailObject trail, int take = 100)
        {
            using (var db = new AppDbContext())
            {
                int sanctionBatchId = bo.Id;
                var query = db.Sanctions.Where(q => q.SanctionBatchId == sanctionBatchId).OrderBy(q => q.Id).Skip(0).Take(take);

                while (query.Count() > 0)
                {
                    foreach (var sanctionId in query.Select(q => q.Id).ToList())
                    {
                        SanctionFormatNameService.DeleteBySanctionId(sanctionId);
                        SanctionNameService.DeleteBySanctionId(sanctionId);
                        SanctionAddressService.DeleteBySanctionId(sanctionId);
                        SanctionBirthDateService.DeleteBySanctionId(sanctionId);
                        SanctionCommentService.DeleteBySanctionId(sanctionId);
                        SanctionCountryService.DeleteBySanctionId(sanctionId);
                        SanctionIdentityService.DeleteBySanctionId(sanctionId);
                        Sanction.Delete(sanctionId);
                    }
                }
            }

            return Update(ref bo, ref trail);
        }
    }
}
