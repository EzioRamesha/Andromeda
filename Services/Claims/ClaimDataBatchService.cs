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
    public class ClaimDataBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataBatch)),
                Controller = ModuleBo.ModuleController.ClaimDataBatch.ToString()
            };
        }

        public static ClaimDataBatchBo FormBo(ClaimDataBatch entity = null)
        {
            if (entity == null)
                return null;
            var riDataBatchBo = new ClaimDataBatchBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                TreatyId = entity.TreatyId,
                TreatyBo = TreatyService.Find(entity.TreatyId),
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId),
                SoaDataBatchId = entity.SoaDataBatchId,
                ClaimTransactionTypePickListDetailId = entity.ClaimTransactionTypePickListDetailId,
                ClaimTransactionTypePickListDetailBo = PickListDetailService.Find(entity.ClaimTransactionTypePickListDetailId),
                Configs = entity.Configs,
                ClaimDataFileConfig = JsonConvert.DeserializeObject<ClaimDataFileConfig>(entity.Configs),
                OverrideProperties = entity.OverrideProperties,
                Status = entity.Status,
                Quarter = entity.Quarter,
                ReceivedAt = entity.ReceivedAt,
                TotalMappingFailedStatus = entity.TotalMappingFailedStatus,
                TotalPreComputationFailedStatus = entity.TotalPreComputationFailedStatus,
                TotalPreValidationFailedStatus = entity.TotalPreValidationFailedStatus,
                CreatedById = entity.CreatedById,
                CreatedByBo = UserService.Find(entity.CreatedById),
                UpdatedById = entity.UpdatedById,
            };

            riDataBatchBo.ClaimDataFileConfig.DelimiterName = ClaimDataConfigBo.GetDelimiterName(riDataBatchBo.ClaimDataFileConfig.Delimiter);

            return riDataBatchBo;
        }

        public static IList<ClaimDataBatchBo> FormBos(IList<ClaimDataBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataBatchBo> bos = new List<ClaimDataBatchBo>() { };
            foreach (ClaimDataBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataBatch FormEntity(ClaimDataBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataBatch
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyId = bo.TreatyId,
                ClaimDataConfigId = bo.ClaimDataConfigId,
                SoaDataBatchId = bo.SoaDataBatchId,
                ClaimTransactionTypePickListDetailId = bo.ClaimTransactionTypePickListDetailId,
                Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig),
                OverrideProperties = bo.OverrideProperties,
                Status = bo.Status,
                Quarter = bo.Quarter,
                ReceivedAt = bo.ReceivedAt,
                TotalMappingFailedStatus = bo.TotalMappingFailedStatus,
                TotalPreComputationFailedStatus = bo.TotalPreComputationFailedStatus,
                TotalPreValidationFailedStatus = bo.TotalPreValidationFailedStatus,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimDataBatch.IsExists(id);
        }

        public static ClaimDataBatchBo Find(int id)
        {
            return FormBo(ClaimDataBatch.Find(id));
        }

        public static ClaimDataBatchBo FindByStatus(int status)
        {
            return FormBo(ClaimDataBatch.FindByStatus(status));
        }

        public static int CountByStatus(int status)
        {
            return ClaimDataBatch.CountByStatus(status);
        }

        public static int CountByTreatyId(int treatyId)
        {
            return ClaimDataBatch.CountByTreatyId(treatyId);
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetClaimDataBatches().Where(q => q.SoaDataBatchId == soaDataBatchId).Count();
            }
        }

        public static void CountTotalFailed(ref ClaimDataBatchBo bo, AppDbContext db)
        {
            bo.TotalMappingFailedStatus = ClaimDataService.CountByClaimDataBatchIdMappingStatusFailed(bo.Id, db);
            bo.TotalPreComputationFailedStatus = ClaimDataService.CountByClaimDataBatchIdPreComputationStatusFailed(bo.Id, db);
            bo.TotalPreValidationFailedStatus = ClaimDataService.CountByClaimDataBatchIdPreValidationStatusFailed(bo.Id, db);
        }

        public static IList<ClaimDataBatchBo> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            return FormBos(ClaimDataBatch.GetByParam(cedantId, treatyId, quarter));
        }

        // Claim Data Overview
        public static IList<ClaimDataBatchBo> GetTotalCaseGroupByCedantId()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.ClaimDataBatches
                    .GroupBy(q => new { q.CedantId })
                    .Select(r => new ClaimDataBatchBo
                    {
                        CedantId = r.Key.CedantId,
                        NoOfCase = r.Count(),
                    })
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.CedantBo = CedantService.Find(bo.CedantId);
                }

                return bos;
            }
        }

        public static IList<ClaimDataBatchBo> GetDetailByCedantIdGroupByStatusByTreatyId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = db.ClaimDataBatches
                    .Where(q => q.CedantId == cedantId)
                    .GroupBy(q => new { q.Status, q.TreatyId, q.SoaDataBatch.Quarter })
                    .Select(r => new ClaimDataBatchBo
                    {
                        Status = r.Key.Status,
                        TreatyId = r.Key.TreatyId,
                        Quarter = r.Key.Quarter,
                        NoOfCase = r.Count(),
                    })
                    .OrderBy(q => q.TreatyId)
                    .ThenBy(q => q.Status)
                    .ToList();

                foreach (var bo in bos)
                {
                    bo.TreatyBo = TreatyService.Find(bo.TreatyId);
                    bo.StatusName = ClaimDataBatchBo.GetStatusName(bo.Status);
                }

                return bos;
            }
        }


        public static Result Save(ref ClaimDataBatchBo bo)
        {
            if (!ClaimDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataBatchBo bo, ref TrailObject trail)
        {
            if (!ClaimDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataBatchBo bo)
        {
            ClaimDataBatch entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataBatchBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataBatchBo bo)
        {
            Result result = Result();

            ClaimDataBatch entity = ClaimDataBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (bo.Status == ClaimDataBatchBo.StatusPendingDelete)
            {
                List<int> riDataIds = ClaimData.GetIdsByClaimDataBatchId(bo.Id);
                if (ClaimRegisterService.CountByClaimDataIds(riDataIds) > 0)
                {
                    result.AddErrorRecordInUsed();
                }
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyId = bo.TreatyId;
                entity.ClaimDataConfigId = bo.ClaimDataConfigId;
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.ClaimTransactionTypePickListDetailId = bo.ClaimTransactionTypePickListDetailId;
                entity.Configs = JsonConvert.SerializeObject(bo.ClaimDataFileConfig);
                entity.OverrideProperties = bo.OverrideProperties;
                entity.Status = bo.Status;
                entity.Quarter = bo.Quarter;
                entity.ReceivedAt = bo.ReceivedAt;
                entity.TotalMappingFailedStatus = bo.TotalMappingFailedStatus;
                entity.TotalPreComputationFailedStatus = bo.TotalPreComputationFailedStatus;
                entity.TotalPreValidationFailedStatus = bo.TotalPreValidationFailedStatus;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataBatchBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<ClaimDataBatchBo> GetProcessingFailedByHours()
        {
            return FormBos(ClaimDataBatch.GetProcessingFailedByHours());
        }
    }
}
