using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.RiDatas
{
    public class RiDataFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataFile)),
                Controller = ModuleBo.ModuleController.RiDataFile.ToString(),
            };
        }

        public static RiDataFileBo FormBo(RiDataFile entity = null, bool foreign = true, bool loadConfigBo = true)
        {
            if (entity == null)
                return null;
            return new RiDataFileBo
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                RiDataBatchBo = foreign ? RiDataBatchService.Find(entity.RiDataBatchId) : null,
                RawFileId = entity.RawFileId,
                RawFileBo = RawFileService.Find(entity.RawFileId),
                TreatyId = entity.TreatyId,
                TreatyBo = foreign ? TreatyService.Find(entity.TreatyId) : null,
                RiDataConfigId = entity.RiDataConfigId,
                RiDataConfigBo = loadConfigBo ? RiDataConfigService.Find(entity.RiDataConfigId) : null,
                Configs = entity.Configs,
                RiDataFileConfig = JsonConvert.DeserializeObject<RiDataFileConfig>(entity.Configs),
                OverrideProperties = entity.OverrideProperties,
                Mode = entity.Mode,
                Status = entity.Status,
                Errors = entity.Errors,
                RecordType = entity.RecordType,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataFileBo> FormBos(IList<RiDataFile> entities = null, bool foreign = true, bool loadConfigBo = true)
        {
            if (entities == null)
                return null;
            IList<RiDataFileBo> bos = new List<RiDataFileBo>() { };
            foreach (RiDataFile entity in entities)
            {
                bos.Add(FormBo(entity, foreign, loadConfigBo));
            }
            return bos;
        }

        public static RiDataFile FormEntity(RiDataFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataFile
            {
                Id = bo.Id,
                RiDataBatchId = bo.RiDataBatchId,
                RawFileId = bo.RawFileId,
                TreatyId = bo.TreatyId,
                RiDataConfigId = bo.RiDataConfigId,
                Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig),
                OverrideProperties = bo.OverrideProperties,
                Mode = bo.Mode,
                Status = bo.Status,
                Errors = bo.Errors,
                RecordType = bo.RecordType,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDataFile.IsExists(id);
        }

        public static RiDataFileBo Find(int? id)
        {
            return FormBo(RiDataFile.Find(id));
        }

        public static RiDataFileBo FindByStatus(int status = 0)
        {
            return FormBo(RiDataFile.FindByStatus(status));
        }

        public static IList<RiDataFileBo> GetByRiDataBatchId(int riDataBatchId, bool foreign = true, bool loadConfigBo = true)
        {
            return FormBos(RiDataFile.GetByRiDataBatchId(riDataBatchId), foreign, loadConfigBo);
        }

        public static IList<RiDataFileBo> GetByRiDataBatchIdMode(int riDataBatchId, int Mode)
        {
            return FormBos(RiDataFile.GetByRiDataBatchIdMode(riDataBatchId, Mode));
        }

        public static IList<RiDataFileBo> GetByRiDataBatchIdExcept(int riDataBatchId, List<int> ids)
        {
            return FormBos(RiDataFile.GetByRiDataBatchIdExcept(riDataBatchId, ids));
        }

        public static int CountByRiDataConfigIdStatus(int riDataConfigId, int[] status)
        {
            return RiDataFile.CountByRiDataConfigIdStatus(riDataConfigId, status);
        }

        public static Result Save(ref RiDataFileBo bo)
        {
            if (!RiDataFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataFileBo bo, ref TrailObject trail)
        {
            if (!RiDataFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataFileBo bo)
        {
            RiDataFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataFileBo bo)
        {
            Result result = Result();

            RiDataFile entity = RiDataFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataBatchId = bo.RiDataBatchId;
                entity.RawFileId = bo.RawFileId;
                entity.TreatyId = bo.TreatyId;
                entity.RiDataConfigId = bo.RiDataConfigId;
                entity.Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig);
                entity.OverrideProperties = bo.OverrideProperties;
                entity.Mode = bo.Mode;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.RecordType = bo.RecordType;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataFileBo bo)
        {
            RiDataFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByRiDataBatchId(int riDataBatchId)
        {
            return RiDataFile.DeleteAllByRiDataBatchId(riDataBatchId);
        }

        public static void DeleteAllByRiDataBatchId(int riDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataBatchId(riDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataFile)));
                }
            }
        }
    }
}
