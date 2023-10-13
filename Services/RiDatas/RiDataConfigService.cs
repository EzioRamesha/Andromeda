using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.RiDatas
{
    public class RiDataConfigService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataConfig)),
                Controller = ModuleBo.ModuleController.RiDataConfig.ToString()
            };
        }

        public static RiDataConfigBo FormBo(RiDataConfig entity = null)
        {
            if (entity == null)
                return null;

            CedantBo cedantBo = CedantService.Find(entity.CedantId);
            TreatyBo treatyBo = TreatyService.Find(entity.TreatyId);
            RiDataConfigBo riDataConfigBo = new RiDataConfigBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = cedantBo,
                CedantName = cedantBo?.Name,
                TreatyId = entity.TreatyId,
                TreatyBo = treatyBo,
                TreatyIdCode = treatyBo?.TreatyIdCode,
                Status = entity.Status,
                Code = entity.Code,
                Name = entity.Name,
                FileType = entity.FileType,
                FileTypeName = RiDataConfigBo.GetFileTypeName(entity.FileType),
                Configs = entity.Configs,
                RiDataFileConfig = JsonConvert.DeserializeObject<RiDataFileConfig>(entity.Configs),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            riDataConfigBo.RiDataFileConfig.DelimiterName = RiDataConfigBo.GetDelimiterName(riDataConfigBo.RiDataFileConfig.Delimiter);

            return riDataConfigBo;
        }

        public static IList<RiDataConfigBo> FormBos(IList<RiDataConfig> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataConfigBo> bos = new List<RiDataConfigBo>() { };
            foreach (RiDataConfig entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataConfig FormEntity(RiDataConfigBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataConfig
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyId = bo.TreatyId,
                Status = bo.Status,
                Code = bo.Code?.Trim(),
                Name = bo.Name?.Trim(),
                FileType = bo.FileType,
                Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(RiDataConfig riDataConfig)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(riDataConfig.Code))
                {
                    var query = db.RiDataConfigs.Where(q => q.Code.Trim().Equals(riDataConfig.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (riDataConfig.Id != 0)
                    {
                        query = query.Where(q => q.Id != riDataConfig.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return RiDataConfig.IsExists(id);
        }

        public static RiDataConfigBo Find(int? id)
        {
            return FormBo(RiDataConfig.Find(id));
        }

        public static int CountByTreatyId(int treatyId)
        {
            return RiDataConfig.CountByTreatyId(treatyId);
        }

        public static IList<RiDataConfigBo> GetByCedantIdStatus(int cedantId, int? status = null, int? selectedId = null)
        {
            return FormBos(RiDataConfig.GetByCedantIdStatus(cedantId, status, selectedId));
        }

        public static Result Save(ref RiDataConfigBo bo)
        {
            if (!RiDataConfig.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RiDataConfigBo bo, ref TrailObject trail)
        {
            if (!RiDataConfig.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataConfigBo bo)
        {
            RiDataConfig entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataConfigBo bo)
        {
            Result result = Result();

            RiDataConfig entity = RiDataConfig.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyId = bo.TreatyId;
                entity.Status = bo.Status;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.FileType = bo.FileType;
                entity.Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataConfigBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiDataConfig.ToString());

            RiDataComputationService.DeleteAllByRiDataConfigId(bo.Id);
            RiDataMappingDetailService.DeleteAllByRiDataConfigId(bo.Id);
            RiDataMappingService.DeleteAllByRiDataConfigId(bo.Id);
            RiDataPreValidationService.DeleteAllByRiDataConfigId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RiDataConfig.Delete(bo.Id);
        }

        public static Result Delete(RiDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                RiDataBatch.CountByRiDataConfigId(bo.Id, true) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiDataConfig.ToString());

                RiDataMappingDetailService.DeleteAllByRiDataConfigId(bo.Id, ref trail);
                RiDataMappingService.DeleteAllByRiDataConfigId(bo.Id, ref trail);
                RiDataComputationService.DeleteAllByRiDataConfigId(bo.Id, ref trail);
                RiDataPreValidationService.DeleteAllByRiDataConfigId(bo.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

                DataTrail dataTrail = RiDataConfig.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
