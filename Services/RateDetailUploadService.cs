using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class RateDetailUploadService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RateDetailUpload)),
                Controller = ModuleBo.ModuleController.RateDetailUpload.ToString()
            };
        }

        public static Expression<Func<RateDetailUpload, RateDetailUploadBo>> Expression()
        {
            return entity => new RateDetailUploadBo
            {
                Id = entity.Id,
                RateId = entity.RateId,

                Status = entity.Status,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RateDetailUploadBo FormBo(RateDetailUpload entity = null)
        {
            if (entity == null)
                return null;

            var createdBy = UserService.Find(entity.CreatedById);

            return new RateDetailUploadBo
            {
                Id = entity.Id,
                RateId = entity.RateId,

                Status = entity.Status,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                UpdatedAt = entity.UpdatedAt,

                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedByName = createdBy != null ? createdBy.FullName : "",

                StatusName = RateDetailUploadBo.GetStatusName(entity.Status),
                StatusClass = RateDetailUploadBo.GetStatusClass(entity.Status),
            };
        }

        public static IList<RateDetailUploadBo> FormBos(IList<RateDetailUpload> entities = null)
        {
            if (entities == null)
                return null;
            IList<RateDetailUploadBo> bos = new List<RateDetailUploadBo>() { };
            foreach (RateDetailUpload entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RateDetailUpload FormEntity(RateDetailUploadBo bo = null)
        {
            if (bo == null)
                return null;
            return new RateDetailUpload
            {
                Id = bo.Id,
                RateId = bo.RateId,

                Status = bo.Status,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RateDetailUpload.IsExists(id);
        }

        public static RateDetailUploadBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(RateDetailUpload.Find(id.Value));
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetailUpload.Where(q => q.Status == status).Count();
            }
        }

        public static IList<RateDetailUploadBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RateDetailUpload.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<RateDetailUploadBo> GetByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RateDetailUpload.Where(q => q.RateId == rateId).ToList());
            }
        }

        public static Result Save(ref RateDetailUploadBo bo)
        {
            if (!RateDetailUpload.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RateDetailUploadBo bo, ref TrailObject trail)
        {
            if (!RateDetailUpload.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RateDetailUploadBo bo)
        {
            RateDetailUpload entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RateDetailUploadBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateDetailUploadBo bo)
        {
            Result result = Result();

            RateDetailUpload entity = RateDetailUpload.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RateId = bo.RateId;
                entity.Status = bo.Status;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RateDetailUploadBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateDetailUploadBo bo)
        {
            RateDetailUpload.Delete(bo.Id);
        }

        public static Result Delete(RateDetailUploadBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RateDetailUpload.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByRateId(int rateId)
        {
            return RateDetailUpload.DeleteByRateId(rateId);
        }

        public static void DeleteByRateId(int rateId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRateId(rateId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RateDetail)));
                }
            }
        }
    }
}
