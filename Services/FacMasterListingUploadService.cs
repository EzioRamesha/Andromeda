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
    public class FacMasterListingUploadService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(FacMasterListingUpload)),
                Controller = ModuleBo.ModuleController.FacMasterListingUpload.ToString()
            };
        }

        public static Expression<Func<FacMasterListingUpload, FacMasterListingUploadBo>> Expression()
        {
            return entity => new FacMasterListingUploadBo
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

        public static FacMasterListingUploadBo FormBo(FacMasterListingUpload entity = null)
        {
            if (entity == null)
                return null;
            return new FacMasterListingUploadBo
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

        public static IList<FacMasterListingUploadBo> FormBos(IList<FacMasterListingUpload> entities = null)
        {
            if (entities == null)
                return null;
            IList<FacMasterListingUploadBo> bos = new List<FacMasterListingUploadBo>() { };
            foreach (FacMasterListingUpload entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static FacMasterListingUpload FormEntity(FacMasterListingUploadBo bo = null)
        {
            if (bo == null)
                return null;
            return new FacMasterListingUpload
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
            return FacMasterListingUpload.IsExists(id);
        }

        public static FacMasterListingUploadBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(FacMasterListingUpload.Find(id.Value));
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListingUpload.Where(q => q.Status == status).Count();
            }
        }

        public static IList<FacMasterListingUploadBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.FacMasterListingUpload.Where(q => q.Status == status).ToList());
            }
        }

        public static Result Save(ref FacMasterListingUploadBo bo)
        {
            if (!FacMasterListingUpload.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref FacMasterListingUploadBo bo, ref TrailObject trail)
        {
            if (!FacMasterListingUpload.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref FacMasterListingUploadBo bo)
        {
            FacMasterListingUpload entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref FacMasterListingUploadBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref FacMasterListingUploadBo bo)
        {
            Result result = Result();

            FacMasterListingUpload entity = FacMasterListingUpload.Find(bo.Id);
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

        public static Result Update(ref FacMasterListingUploadBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(FacMasterListingUploadBo bo)
        {
            FacMasterListingUpload.Delete(bo.Id);
        }

        public static Result Delete(FacMasterListingUploadBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = FacMasterListingUpload.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
