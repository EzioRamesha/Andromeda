using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class RawFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RawFile)),
                Controller = "RawFile",
            };
        }

        public static RawFileBo FormBo(RawFile entity = null)
        {
            if (entity == null)
                return null;
            return new RawFileBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Status = entity.Status,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RawFileBo> FormBos(IList<RawFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<RawFileBo> bos = new List<RawFileBo>() { };
            foreach (RawFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RawFile FormEntity(RawFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RawFile
            {
                Id = bo.Id,
                Type = bo.Type,
                Status = bo.Status,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RawFile.IsExists(id);
        }

        public static RawFileBo Find(int id)
        {
            return FormBo(RawFile.Find(id));
        }

        public static RawFileBo FindByTypeStatus(int type, int status = 0)
        {
            return FormBo(RawFile.FindByTypeStatus(type, status));
        }

        public static Result Save(ref RawFileBo bo)
        {
            if (!RawFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RawFileBo bo, ref TrailObject trail)
        {
            if (!RawFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RawFileBo bo)
        {
            RawFile entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RawFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RawFileBo bo)
        {
            Result result = Result();

            RawFile entity = RawFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Type = bo.Type;
                entity.Status = bo.Status;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RawFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RawFileBo bo)
        {
            RawFile.Delete(bo.Id);
        }

        public static Result Delete(RawFileBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RawFile.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static void DeleteByIds(List<int> ids)
        {
            foreach (int id in ids)
            {
                RawFile.Delete(id);
            }
        }

        public static Result DeleteByIds(List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            foreach (int id in ids)
            {
                DataTrail dataTrail = RawFile.Delete(id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
