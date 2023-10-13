using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.SoaDatas
{
    public class SoaDataFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataFile)),
                Controller = ModuleBo.ModuleController.SoaDataFile.ToString(),
            };
        }

        public static SoaDataFileBo FormBo(SoaDataFile entity = null)
        {
            if (entity == null)
                return null;
            return new SoaDataFileBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId),
                RawFileId = entity.RawFileId,
                RawFileBo = RawFileService.Find(entity.RawFileId),
                TreatyId = entity.TreatyId,
                TreatyBo = TreatyService.Find(entity.TreatyId),
                Mode = entity.Mode,
                Status = entity.Status,
                Errors = entity.Errors,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SoaDataFileBo> FormBos(IList<SoaDataFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<SoaDataFileBo> bos = new List<SoaDataFileBo>() { };
            foreach (SoaDataFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SoaDataFile FormEntity(SoaDataFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataFile
            {
                Id = bo.Id,
                SoaDataBatchId = bo.SoaDataBatchId,
                RawFileId = bo.RawFileId,
                TreatyId = bo.TreatyId,
                Mode = bo.Mode,
                Status = bo.Status,
                Errors = bo.Errors,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SoaDataFile.IsExists(id);
        }

        public static SoaDataFileBo Find(int? id)
        {
            return FormBo(SoaDataFile.Find(id));
        }

        public static SoaDataFileBo FindByStatus(int status = 0)
        {
            return FormBo(SoaDataFile.FindByStatus(status));
        }

        public static IList<SoaDataFileBo> GetBySoaDataBatchId(int soaDataBatchId)
        {
            return FormBos(SoaDataFile.GetBySoaDataBatchId(soaDataBatchId));
        }

        public static IList<SoaDataFileBo> GetBySoaDataBatchIdMode(int soaDataBatchId, int Mode)
        {
            return FormBos(SoaDataFile.GetBySoaDataBatchIdMode(soaDataBatchId, Mode));
        }

        public static IList<SoaDataFileBo> GetBySoaDataBatchIdExcept(int soaDataBatchId, List<int> ids)
        {
            return FormBos(SoaDataFile.GetBySoaDataBatchIdExcept(soaDataBatchId, ids));
        }

        public static Result Save(ref SoaDataFileBo bo)
        {
            if (!SoaDataFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SoaDataFileBo bo, ref TrailObject trail)
        {
            if (!SoaDataFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataFileBo bo)
        {
            SoaDataFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SoaDataFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataFileBo bo)
        {
            Result result = Result();

            SoaDataFile entity = SoaDataFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.RawFileId = bo.RawFileId;
                entity.TreatyId = bo.TreatyId;
                entity.Mode = bo.Mode;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataFileBo bo)
        {
            SoaDataFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllBySoaDataBatchId(int soaDataBatchId)
        {
            return SoaDataFile.DeleteAllBySoaDataBatchId(soaDataBatchId);
        }

        public static void DeleteAllBySoaDataBatchId(int soaDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllBySoaDataBatchId(soaDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(SoaDataFile)));
                }
            }
        }
    }
}
