using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class RateTableMappingUploadService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RateTableMappingUpload)),
                Controller = ModuleBo.ModuleController.RateTableMappingUpload.ToString()
            };
        }

        public static Expression<Func<RateTableMappingUpload, RateTableMappingUploadBo>> Expression()
        {
            return entity => new RateTableMappingUploadBo
            {
                Id = entity.Id,

                Status = entity.Status,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RateTableMappingUploadBo FormBo(RateTableMappingUpload entity = null)
        {
            if (entity == null)
                return null;
            return new RateTableMappingUploadBo
            {
                Id = entity.Id,

                Status = entity.Status,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static IList<RateTableMappingUploadBo> FormBos(IList<RateTableMappingUpload> entities = null)
        {
            if (entities == null)
                return null;
            IList<RateTableMappingUploadBo> bos = new List<RateTableMappingUploadBo>() { };
            foreach (RateTableMappingUpload entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RateTableMappingUpload FormEntity(RateTableMappingUploadBo bo = null)
        {
            if (bo == null)
                return null;
            return new RateTableMappingUpload
            {
                Id = bo.Id,

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
            return RateTableMappingUpload.IsExists(id);
        }

        public static RateTableMappingUploadBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(RateTableMappingUpload.Find(id.Value));
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTableMappingUpload.Where(q => q.Status == status).Count();
            }
        }

        public static IList<RateTableMappingUploadBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RateTableMappingUpload.Where(q => q.Status == status).ToList());
            }
        }

        public static Result Save(ref RateTableMappingUploadBo bo)
        {
            if (!RateTableMappingUpload.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RateTableMappingUploadBo bo, ref TrailObject trail)
        {
            if (!RateTableMappingUpload.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RateTableMappingUploadBo bo)
        {
            RateTableMappingUpload entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RateTableMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateTableMappingUploadBo bo)
        {
            Result result = Result();

            RateTableMappingUpload entity = RateTableMappingUpload.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
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

        public static Result Update(ref RateTableMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateTableMappingUploadBo bo)
        {
            RateTableMappingUpload.Delete(bo.Id);
        }

        public static Result Delete(RateTableMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RateTableMappingUpload.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
