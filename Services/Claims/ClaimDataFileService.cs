using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Claims
{
    public class ClaimDataFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataFile)),
                Controller = ModuleBo.ModuleController.ClaimDataFile.ToString(),
            };
        }

        public static ClaimDataFileBo FormBo(ClaimDataFile entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimDataFileBo
            {
                Id = entity.Id,
                ClaimDataBatchId = entity.ClaimDataBatchId,
                ClaimDataBatchBo = ClaimDataBatchService.Find(entity.ClaimDataBatchId),
                RawFileId = entity.RawFileId,
                RawFileBo = RawFileService.Find(entity.RawFileId),
                TreatyId = entity.TreatyId,
                TreatyBo = TreatyService.Find(entity.TreatyId),
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId),
                CurrencyCodeId = entity.CurrencyCodeId,
                CurrencyCodeBo = PickListDetailService.Find(entity.CurrencyCodeId),
                CurrencyRate = entity.CurrencyRate,
                Configs = entity.Configs,
                ClaimDataFileConfig = JsonConvert.DeserializeObject<ClaimDataFileConfig>(entity.Configs),
                OverrideProperties = entity.OverrideProperties,
                Mode = entity.Mode,
                Status = entity.Status,
                Errors = entity.Errors,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByBo = UserService.Find(entity.CreatedById),
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimDataFileBo> FormBos(IList<ClaimDataFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataFileBo> bos = new List<ClaimDataFileBo>() { };
            foreach (ClaimDataFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataFile FormEntity(ClaimDataFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataFile
            {
                Id = bo.Id,
                ClaimDataBatchId = bo.ClaimDataBatchId,
                RawFileId = bo.RawFileId,
                TreatyId = bo.TreatyId,
                ClaimDataConfigId = bo.ClaimDataConfigId,
                CurrencyCodeId = bo.CurrencyCodeId,
                CurrencyRate = bo.CurrencyRate,
                Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig),
                OverrideProperties = bo.OverrideProperties,
                Mode = bo.Mode,
                Status = bo.Status,
                Errors = bo.Errors,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimDataFile.IsExists(id);
        }

        public static ClaimDataFileBo Find(int id)
        {
            return FormBo(ClaimDataFile.Find(id));
        }

        public static ClaimDataFileBo FindByStatus(int status = 0)
        {
            return FormBo(ClaimDataFile.FindByStatus(status));
        }

        public static int CountByClaimDataBatchId(int claimDataBatchId)
        {
            return ClaimDataFile.CountByClaimDataBatchId(claimDataBatchId);
        }

        public static int CountByClaimDataConfigIdStatus(int claimDataConfigId, int[] status)
        {
            return ClaimDataFile.CountByClaimDataConfigIdStatus(claimDataConfigId, status);
        }

        public static IList<ClaimDataFileBo> GetConfigIdByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId).Select(q => new ClaimDataFileBo
                {
                    Id = q.Id,
                    ClaimDataConfigId = q.ClaimDataConfigId,
                }).ToList();
            }
        }

        public static IList<ClaimDataFileBo> GetByClaimDataBatchId(int claimDataBatchId)
        {
            return FormBos(ClaimDataFile.GetByClaimDataBatchId(claimDataBatchId));
        }

        public static IList<ClaimDataFileBo> GetByClaimDataBatchIdMode(int claimDataBatchId, int Mode)
        {
            return FormBos(ClaimDataFile.GetByClaimDataBatchIdMode(claimDataBatchId, Mode));
        }

        public static IList<ClaimDataFileBo> GetByClaimDataBatchIdExcept(int claimDataBatchId, List<int> ids)
        {
            return FormBos(ClaimDataFile.GetByClaimDataBatchIdExcept(claimDataBatchId, ids));
        }

        public static Result Save(ref ClaimDataFileBo bo)
        {
            if (!ClaimDataFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataFileBo bo, ref TrailObject trail)
        {
            if (!ClaimDataFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataFileBo bo)
        {
            ClaimDataFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataFileBo bo)
        {
            Result result = Result();

            ClaimDataFile entity = ClaimDataFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimDataBatchId = bo.ClaimDataBatchId;
                entity.RawFileId = bo.RawFileId;
                entity.TreatyId = bo.TreatyId;
                entity.ClaimDataConfigId = bo.ClaimDataConfigId;
                entity.CurrencyCodeId = bo.CurrencyCodeId;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig);
                entity.OverrideProperties = bo.OverrideProperties;
                entity.Mode = bo.Mode;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataFileBo bo)
        {
            ClaimDataFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByClaimDataBatchId(int claimDataBatchId)
        {
            return ClaimDataFile.DeleteAllByClaimDataBatchId(claimDataBatchId);
        }

        public static void DeleteAllByClaimDataBatchId(int claimDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataBatchId(claimDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataFile)));
                }
            }
        }
    }
}
