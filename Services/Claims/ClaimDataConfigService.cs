using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Claims
{
    public class ClaimDataConfigService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataConfig)),
                Controller = ModuleBo.ModuleController.ClaimDataConfig.ToString()
            };
        }

        public static ClaimDataConfigBo FormBo(ClaimDataConfig entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;

            ClaimDataConfigBo claimDataConfigBo = new ClaimDataConfigBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                TreatyId = entity.TreatyId,
                Status = entity.Status,
                Code = entity.Code,
                Name = entity.Name,
                FileType = entity.FileType,
                FileTypeName = ClaimDataConfigBo.GetFileTypeName(entity.FileType),
                Configs = entity.Configs,
                ClaimDataFileConfig = JsonConvert.DeserializeObject<ClaimDataFileConfig>(entity.Configs),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            claimDataConfigBo.ClaimDataFileConfig.DelimiterName = ClaimDataConfigBo.GetDelimiterName(claimDataConfigBo.ClaimDataFileConfig.Delimiter);

            if (foreign)
            {
                claimDataConfigBo.CedantBo = CedantService.Find(entity.CedantId);
                claimDataConfigBo.CedantName = claimDataConfigBo.CedantBo?.Name;
                claimDataConfigBo.TreatyBo = TreatyService.Find(entity.TreatyId);
                claimDataConfigBo.TreatyIdCode = claimDataConfigBo.TreatyBo?.TreatyIdCode;
            }

            return claimDataConfigBo;
        }

        public static IList<ClaimDataConfigBo> FormBos(IList<ClaimDataConfig> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataConfigBo> bos = new List<ClaimDataConfigBo>() { };
            foreach (ClaimDataConfig entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataConfig FormEntity(ClaimDataConfigBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataConfig
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyId = bo.TreatyId,
                Status = bo.Status,
                Code = bo.Code?.Trim(),
                Name = bo.Name?.Trim(),
                FileType = bo.FileType,
                Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(ClaimDataConfig claimDataConfig)
        {
            return claimDataConfig.IsDuplicateCode();
        }

        public static bool IsExists(int id)
        {
            return ClaimDataConfig.IsExists(id);
        }

        public static ClaimDataConfigBo Find(int? id, bool foreign = true)
        {
            return FormBo(ClaimDataConfig.Find(id), foreign);
        }

        public static int CountByTreatyId(int treatyId)
        {
            return ClaimDataConfig.CountByTreatyId(treatyId);
        }

        public static IList<ClaimDataConfigBo> GetByCedantIdStatus(int cedantId, int? status = null, int? selectedId = null)
        {
            return FormBos(ClaimDataConfig.GetByCedantIdStatus(cedantId, status, selectedId));
        }

        public static IList<ClaimDataConfigBo> GetByCedantCodeStatus(string cedantCode, int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataConfigs.Where(q => q.Cedant.Code == cedantCode);
                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }
                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref ClaimDataConfigBo bo)
        {
            if (!ClaimDataConfig.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimDataConfigBo bo, ref TrailObject trail)
        {
            if (!ClaimDataConfig.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataConfigBo bo)
        {
            ClaimDataConfig entity = FormEntity(bo);

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

        public static Result Create(ref ClaimDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataConfigBo bo)
        {
            Result result = Result();

            ClaimDataConfig entity = ClaimDataConfig.Find(bo.Id);
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
                entity.Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataConfigBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimDataConfig.ToString());

            ClaimDataComputationService.DeleteAllByClaimDataConfigId(bo.Id);
            ClaimDataMappingDetailService.DeleteAllByClaimDataConfigId(bo.Id);
            ClaimDataMappingService.DeleteAllByClaimDataConfigId(bo.Id);
            ClaimDataValidationService.DeleteAllByClaimDataConfigId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            ClaimDataConfig.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataConfigBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (ClaimDataBatch.CountByClaimDataConfigId(bo.Id, true) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiDataConfig.ToString());

                ClaimDataMappingDetailService.DeleteAllByClaimDataConfigId(bo.Id, ref trail);
                ClaimDataMappingService.DeleteAllByClaimDataConfigId(bo.Id, ref trail);
                ClaimDataComputationService.DeleteAllByClaimDataConfigId(bo.Id, ref trail);
                ClaimDataValidationService.DeleteAllByClaimDataConfigId(bo.Id, ref trail);
                StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
                RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

                DataTrail dataTrail = ClaimDataConfig.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
