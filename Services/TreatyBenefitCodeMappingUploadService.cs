﻿using BusinessObject;
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
    public class TreatyBenefitCodeMappingUploadService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyBenefitCodeMappingUpload)),
                Controller = ModuleBo.ModuleController.TreatyBenefitCodeMappingUpload.ToString()
            };
        }

        public static Expression<Func<TreatyBenefitCodeMappingUpload, TreatyBenefitCodeMappingUploadBo>> Expression()
        {
            return entity => new TreatyBenefitCodeMappingUploadBo
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

        public static TreatyBenefitCodeMappingUploadBo FormBo(TreatyBenefitCodeMappingUpload entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyBenefitCodeMappingUploadBo
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

        public static IList<TreatyBenefitCodeMappingUploadBo> FormBos(IList<TreatyBenefitCodeMappingUpload> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyBenefitCodeMappingUploadBo> bos = new List<TreatyBenefitCodeMappingUploadBo>() { };
            foreach (TreatyBenefitCodeMappingUpload entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyBenefitCodeMappingUpload FormEntity(TreatyBenefitCodeMappingUploadBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyBenefitCodeMappingUpload
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
            return TreatyBenefitCodeMappingUpload.IsExists(id);
        }

        public static TreatyBenefitCodeMappingUploadBo Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return FormBo(TreatyBenefitCodeMappingUpload.Find(id.Value));
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappingUpload.Where(q => q.Status == status).Count();
            }
        }

        public static IList<TreatyBenefitCodeMappingUploadBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyBenefitCodeMappingUpload.Where(q => q.Status == status).ToList());
            }
        }

        public static Result Save(ref TreatyBenefitCodeMappingUploadBo bo)
        {
            if (!TreatyBenefitCodeMappingUpload.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyBenefitCodeMappingUploadBo bo, ref TrailObject trail)
        {
            if (!TreatyBenefitCodeMappingUpload.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyBenefitCodeMappingUploadBo bo)
        {
            TreatyBenefitCodeMappingUpload entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyBenefitCodeMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyBenefitCodeMappingUploadBo bo)
        {
            Result result = Result();

            TreatyBenefitCodeMappingUpload entity = TreatyBenefitCodeMappingUpload.Find(bo.Id);
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

        public static Result Update(ref TreatyBenefitCodeMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyBenefitCodeMappingUploadBo bo)
        {
            TreatyBenefitCodeMappingUpload.Delete(bo.Id);
        }

        public static Result Delete(TreatyBenefitCodeMappingUploadBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyBenefitCodeMappingUpload.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
