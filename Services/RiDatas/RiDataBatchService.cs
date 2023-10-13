using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.RiDatas
{
    public class RiDataBatchService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataBatch)),
                Controller = ModuleBo.ModuleController.RiDataBatch.ToString()
            };
        }

        public static RiDataBatchBo FormBo(RiDataBatch entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new RiDataBatchBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                TreatyId = entity.TreatyId,
                RiDataConfigId = entity.RiDataConfigId,
                Configs = entity.Configs,
                RiDataFileConfig = JsonConvert.DeserializeObject<RiDataFileConfig>(entity.Configs),
                OverrideProperties = entity.OverrideProperties,
                Status = entity.Status,
                ProcessWarehouseStatus = entity.ProcessWarehouseStatus,
                Quarter = entity.Quarter,
                TotalPreComputation1FailedStatus = entity.TotalPreComputation1FailedStatus,
                TotalPreComputation2FailedStatus = entity.TotalPreComputation2FailedStatus,
                TotalMappingFailedStatus = entity.TotalMappingFailedStatus,
                TotalPreValidationFailedStatus = entity.TotalPreValidationFailedStatus,
                TotalFinaliseFailedStatus = entity.TotalFinaliseFailedStatus,
                TotalPostComputationFailedStatus = entity.TotalPostComputationFailedStatus,
                TotalPostValidationFailedStatus = entity.TotalPostValidationFailedStatus,
                TotalProcessWarehouseFailedStatus = entity.TotalProcessWarehouseFailedStatus,
                TotalConflict = entity.TotalConflict,
                RecordType = entity.RecordType,
                ReceivedAt = entity.ReceivedAt,
                SoaDataBatchId = entity.SoaDataBatchId,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                FinalisedAt = entity.FinalisedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.CedantBo = CedantService.Find(entity.CedantId);
                bo.TreatyBo = TreatyService.Find(entity.TreatyId);
                bo.RiDataConfigBo = RiDataConfigService.Find(entity.RiDataConfigId);
                bo.CreatedByBo = UserService.Find(entity.CreatedById, deleted: true);
            }

            bo.RiDataFileConfig.DelimiterName = RiDataConfigBo.GetDelimiterName(bo.RiDataFileConfig.Delimiter);

            return bo;
        }

        public static IList<RiDataBatchBo> FormBos(IList<RiDataBatch> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataBatchBo> bos = new List<RiDataBatchBo>() { };
            foreach (RiDataBatch entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataBatch FormEntity(RiDataBatchBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataBatch
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyId = bo.TreatyId,
                RiDataConfigId = bo.RiDataConfigId,
                Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig),
                OverrideProperties = bo.OverrideProperties,
                Status = bo.Status,
                ProcessWarehouseStatus = bo.ProcessWarehouseStatus,
                Quarter = bo.Quarter,
                TotalPreComputation1FailedStatus = bo.TotalPreComputation1FailedStatus,
                TotalPreComputation2FailedStatus = bo.TotalPreComputation2FailedStatus,
                TotalFinaliseFailedStatus = bo.TotalFinaliseFailedStatus,
                TotalPreValidationFailedStatus = bo.TotalPreValidationFailedStatus,
                TotalMappingFailedStatus = bo.TotalMappingFailedStatus,
                TotalPostComputationFailedStatus = bo.TotalPostComputationFailedStatus,
                TotalPostValidationFailedStatus = bo.TotalPostValidationFailedStatus,
                TotalProcessWarehouseFailedStatus = bo.TotalProcessWarehouseFailedStatus,
                TotalConflict = bo.TotalConflict,
                RecordType = bo.RecordType,
                ReceivedAt = bo.ReceivedAt,
                SoaDataBatchId = bo.SoaDataBatchId,
                FinalisedAt = bo.FinalisedAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDataBatch.IsExists(id);
        }

        public static RiDataBatchBo Find(int? id)
        {
            return FormBo(RiDataBatch.Find(id));
        }

        public static RiDataBatchBo FindByStatus(int status)
        {
            return FormBo(RiDataBatch.FindByStatus(status));
        }

        public static RiDataBatchBo FindByStatusAndCedantList(int status, string cedants)
        {
            return FormBo(RiDataBatch.FindByStatusAndCedantList(status, cedants));
        }

        public static RiDataBatchBo FindByProcessWarehouseStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.GetRiDataBatches().Where(q => q.ProcessWarehouseStatus == status).OrderBy(q => q.FinalisedAt).FirstOrDefault());
            }
        }

        public static RiDataBatchBo FindByProcessWarehouseStatuses(List<int> statuses)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.GetRiDataBatches().Where(q => statuses.Contains(q.ProcessWarehouseStatus)).OrderBy(q => q.FinalisedAt).FirstOrDefault());
            }
        }

        public static int Count()
        {
            return RiDataBatch.Count();
        }

        public static int CountByStatus(int status)
        {
            return RiDataBatch.CountByStatus(status);
        }

        public static int CountByStatusAndCedantList(int status, string cedants)
        {
            return RiDataBatch.CountByStatusAndCedantList(status, cedants);
        }

        public static int CountByProcessWarehouseStatuses(List<int> statuses)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Where(q => statuses.Contains(q.ProcessWarehouseStatus)).Count();
            }
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Where(q => q.SoaDataBatchId == soaDataBatchId).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            return RiDataBatch.CountByTreatyId(treatyId);
        }

        public static void CountTotalFailed(ref RiDataBatchBo bo, AppDbContext db)
        {
            bo.TotalMappingFailedStatus = RiDataService.CountByRiDataBatchIdMappingStatusFailed(bo.Id, db);
            bo.TotalPreComputation1FailedStatus = RiDataService.CountByRiDataBatchIdPreComputation1StatusFailed(bo.Id, db);
            bo.TotalPreComputation2FailedStatus = RiDataService.CountByRiDataBatchIdPreComputation2StatusFailed(bo.Id, db);
            bo.TotalPreValidationFailedStatus = RiDataService.CountByRiDataBatchIdPreValidationStatusFailed(bo.Id, db);
            bo.TotalPostComputationFailedStatus = RiDataService.CountByRiDataBatchIdPostComputationStatusFailed(bo.Id, db);
            bo.TotalPostValidationFailedStatus = RiDataService.CountByRiDataBatchIdPostValidationStatusFailed(bo.Id, db);
            bo.TotalFinaliseFailedStatus = RiDataService.CountByRiDataBatchIdFinaliseStatusFailed(bo.Id, db);
            bo.TotalProcessWarehouseFailedStatus = RiDataService.CountByRiDataBatchIdProcessWarehouseStatusFailed(bo.Id, db);
            bo.TotalConflict = RiDataService.CountByRiDataBatchIdIsConflict(bo.Id, db);
        }

        public static IList<RiDataBatchBo> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            return FormBos(RiDataBatch.GetByParam(cedantId, treatyId, quarter));
        }

        public static IList<RiDataBatchBo> GetProcessingFailedByHours()
        {
            return FormBos(RiDataBatch.GetProcessingFailedByHours());
        }

        public static IList<RiDataBatchBo> GetFinaliseFailedByHours()
        {
            return FormBos(RiDataBatch.GetFinaliseFailedByHours());
        }

        // RI Data Overview
        public static IList<RiDataBatchBo> GetTotalCaseGroupByCedantId()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.RiDataBatches
                    .GroupBy(q => new { q.CedantId })
                    .Select(r => new RiDataBatchBo
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

        public static IList<RiDataBatchBo> GetDetailByCedantIdGroupByStatusByTreatyId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = db.RiDataBatches
                    .Where(q => q.CedantId == cedantId)
                    .GroupBy(q => new { q.Status, q.TreatyId, q.SoaDataBatch.Quarter })
                    .Select(r => new RiDataBatchBo
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
                    bo.StatusName = RiDataBatchBo.GetStatusName(bo.Status);
                }

                return bos;
            }
        }

        public static Result Save(ref RiDataBatchBo bo)
        {
            if (!RiDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataBatchBo bo, ref TrailObject trail)
        {
            if (!RiDataBatch.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataBatchBo bo)
        {
            RiDataBatch entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataBatchBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataBatchBo bo, bool concurrentChecking = false)
        {
            Result result = Result();

            RiDataBatch entity = RiDataBatch.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataConfigId = bo.RiDataConfigId;
                entity.CedantId = bo.CedantId;
                entity.TreatyId = bo.TreatyId;
                entity.RiDataConfigId = bo.RiDataConfigId;
                entity.Configs = JsonConvert.SerializeObject(bo.RiDataFileConfig);
                entity.OverrideProperties = bo.OverrideProperties;
                entity.Status = bo.Status;
                entity.ProcessWarehouseStatus = bo.ProcessWarehouseStatus;
                entity.Quarter = bo.Quarter;
                entity.TotalPreComputation1FailedStatus = bo.TotalPreComputation1FailedStatus;
                entity.TotalPreComputation2FailedStatus = bo.TotalPreComputation2FailedStatus;
                entity.TotalMappingFailedStatus = bo.TotalMappingFailedStatus;
                entity.TotalPreValidationFailedStatus = bo.TotalPreValidationFailedStatus;
                entity.TotalFinaliseFailedStatus = bo.TotalFinaliseFailedStatus;
                entity.TotalPostComputationFailedStatus = bo.TotalPostComputationFailedStatus;
                entity.TotalPostValidationFailedStatus = bo.TotalPostValidationFailedStatus;
                entity.TotalConflict = bo.TotalConflict;
                entity.RecordType = bo.RecordType;
                entity.ReceivedAt = bo.ReceivedAt;
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.FinalisedAt = bo.FinalisedAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update(concurrentChecking);
            }
            return result;
        }

        public static Result Update(ref RiDataBatchBo bo, ref TrailObject trail, bool concurrentChecking = false)
        {
            Result result = Update(ref bo, concurrentChecking);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataBatchBo bo)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());

            List<int> rawFileIds = new List<int> { };
            IList<RiDataFileBo> riDataFiles = RiDataFileService.GetByRiDataBatchId(bo.Id);
            foreach (RiDataFileBo riDataFile in riDataFiles)
            {
                if (File.Exists(riDataFile.RawFileBo.GetLocalPath()))
                {
                    File.Delete(riDataFile.RawFileBo.GetLocalPath());
                    rawFileIds.Add(riDataFile.RawFileId);
                }
            }

            // DO NOT delete RiData
            // Because it will catch SqlException: Execution Timeout Expired
            // When the total numbe of record more than 1k (estimated)
            // TODO: Have to find a way to delete RI Data if the Batch deleted
            //RiDataService.DeleteAllByRiDataBatchId(bo.Id);

            RiDataFileService.DeleteAllByRiDataBatchId(bo.Id);
            RawFileService.DeleteByIds(rawFileIds);
            RiDataBatchStatusFileService.DeleteAllByRiDataBatchId(bo.Id);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id);

            RiDataBatch.Delete(bo.Id);
        }

        public static Result Delete(RiDataBatchBo bo, ref TrailObject trail)
        {
            Result result = Result();
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());

            List<int> rawFileIds = new List<int> { };
            IList<RiDataFileBo> riDataFiles = RiDataFileService.GetByRiDataBatchId(bo.Id);
            foreach (RiDataFileBo riDataFile in riDataFiles)
            {
                if (File.Exists(riDataFile.RawFileBo.GetLocalPath()))
                {
                    File.Delete(riDataFile.RawFileBo.GetLocalPath());
                    rawFileIds.Add(riDataFile.RawFileId);
                }
            }

            // DO NOT delete RiData
            // Because it will catch SqlException: Execution Timeout Expired
            // When the total numbe of record more than 1k (estimated)
            // TODO: Have to find a way to delete RI Data if the Batch deleted
            //RiDataService.DeleteAllByRiDataBatchId(bo.Id); // DO NOT TRAIL

            RiDataFileService.DeleteAllByRiDataBatchId(bo.Id, ref trail);
            RawFileService.DeleteByIds(rawFileIds, ref trail);
            RiDataBatchStatusFileService.DeleteAllByRiDataBatchId(bo.Id, ref trail);
            StatusHistoryService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);
            RemarkService.DeleteAllByModuleIdObjectId(moduleBo.Id, bo.Id, ref trail);

            DataTrail dataTrail = RiDataBatch.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static List<string> GetDistinctTreatyCodesById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiData.Where(q => q.RiDataBatchId == id).Select(q => q.TreatyCode).Distinct().ToList();
            }
        }
    }
}
