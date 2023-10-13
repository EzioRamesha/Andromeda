using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataBatches")]
    public class RiDataBatch
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int? TreatyCodeId { get; set; }

        [Index]
        public int? TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Required, Index]
        public int RiDataConfigId { get; set; }

        [ForeignKey(nameof(RiDataConfigId))]
        [ExcludeTrail]
        public virtual RiDataConfig RiDataConfig { get; set; }

        public string Configs { get; set; }

        public string OverrideProperties { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int ProcessWarehouseStatus { get; set; }

        [MaxLength(64), Index]
        public string Quarter { get; set; }

        public int TotalMappingFailedStatus { get; set; }
        public int TotalPreComputation1FailedStatus { get; set; }
        public int TotalPreComputation2FailedStatus { get; set; }
        public int TotalPreValidationFailedStatus { get; set; }
        public int TotalPostComputationFailedStatus { get; set; }
        public int TotalPostValidationFailedStatus { get; set; }
        public int TotalFinaliseFailedStatus { get; set; }
        public int TotalProcessWarehouseFailedStatus { get; set; }
        public int TotalConflict { get; set; }


        [Required, Index]
        public int RecordType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ReceivedAt { get; set; }

        [Index]
        public int? SoaDataBatchId { get; set; }

        [ForeignKey(nameof(SoaDataBatchId))]
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FinalisedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RiDataBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Any(q => q.Id == id);
            }
        }

        public static RiDataBatch Find(int? id)
        {
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(130, "RiDataBatch");
            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    return db.GetRiDataBatches().Where(q => q.Id == id).FirstOrDefault();
                }
            });
        }

        public static RiDataBatch FindByStatus(int status)
        {
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataBatch", 143);
            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    return db.GetRiDataBatches().Where(q => q.Status == status).FirstOrDefault();
                }
            });
        }

        public static RiDataBatch FindByStatusAndCedantList(int status, string cedants)
        {
            List<string> cedantList = cedants.Split(',').ToList();

            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches()
                    .Where(q => q.Status == status)
                    .Where(q => cedantList.Contains(q.Cedant.Code))
                    .FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Count();
            }
        }

        public static int CountByStatus(int status)
        {
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(174, "RiDataBatch");
            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    return db.GetRiDataBatches().Where(q => q.Status == status).Count();
                }
            });
        }

        public static int CountByStatusAndCedantList(int status, string cedants)
        {
            List<string> cedantList = cedants.Split(',').ToList();

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(189, "RiDataBatch");
            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    return db.GetRiDataBatches()
                        .Where(q => q.Status == status)
                        .Where(q => cedantList.Contains(q.Cedant.Code))
                        .Count();
                }
            });
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches().Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static int CountByRiDataConfigId(int riDataConfigId, bool deleted = false)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches(deleted).Where(q => q.RiDataConfigId == riDataConfigId).Count();
            }
        }

        public static IList<RiDataBatch> GetByParam(int? cedantId, int? treatyId, string quarter)
        {
            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches()
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.TreatyId == treatyId)
                    .Where(q => q.Quarter == quarter)
                    .ToList();
            }
        }

        public static IList<RiDataBatch> GetProcessingFailedByHours()
        {
            var hoursToCheck = Util.GetConfigInteger("ProcessRiDataBatchFailCheckHours");
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataBatch", 243);

            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    return db.GetRiDataBatches()
                    .Where(q => q.Status == RiDataBatchBo.StatusPreProcessing || q.Status == RiDataBatchBo.StatusPostProcessing)
                    .Where(q => DbFunctions.DiffHours(q.UpdatedAt, DateTime.Now) >= hoursToCheck)
                    .ToList();
                }
            });
        }

        public static IList<RiDataBatch> GetFinaliseFailedByHours()
        {
            var hoursToCheck = Util.GetConfigInteger("FinaliseRiDataBatchFailCheckHours");

            using (var db = new AppDbContext())
            {
                return db.GetRiDataBatches()
                    .Where(q => q.Status == RiDataBatchBo.StatusFinalising)
                    .Where(q => DbFunctions.DiffHours(q.UpdatedAt, DateTime.Now) >= hoursToCheck)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataBatches.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update(bool concurrentChecking = false)
        {
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataBatch", 320);
            int resultCount = 0;

            return connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext())
                {
                    db.Database.CommandTimeout = 0;
                    RiDataBatch riDataBatch = concurrentChecking ? db.RiDataBatches.FirstOrDefault(q => q.Id == Id) : Find(Id);
                    if (riDataBatch == null)
                    {
                        throw new Exception(MessageBag.NoRecordFound);
                    }

                    DataTrail trail = new DataTrail(riDataBatch, this);
                    riDataBatch.CedantId = CedantId;
                    riDataBatch.TreatyId = TreatyId;
                    riDataBatch.RiDataConfigId = RiDataConfigId;
                    riDataBatch.Status = Status;
                    riDataBatch.ProcessWarehouseStatus = ProcessWarehouseStatus;
                    riDataBatch.Quarter = Quarter;
                    riDataBatch.Configs = Configs;
                    riDataBatch.OverrideProperties = OverrideProperties;
                    riDataBatch.TotalMappingFailedStatus = TotalMappingFailedStatus;
                    riDataBatch.TotalPreComputation1FailedStatus = TotalPreComputation1FailedStatus;
                    riDataBatch.TotalPreValidationFailedStatus = TotalPreValidationFailedStatus;
                    riDataBatch.TotalFinaliseFailedStatus = TotalFinaliseFailedStatus;
                    riDataBatch.TotalPreComputation2FailedStatus = TotalPreComputation2FailedStatus;
                    riDataBatch.TotalPostComputationFailedStatus = TotalPostComputationFailedStatus;
                    riDataBatch.TotalPostValidationFailedStatus = TotalPostValidationFailedStatus;
                    riDataBatch.TotalProcessWarehouseFailedStatus = TotalProcessWarehouseFailedStatus;
                    riDataBatch.TotalConflict = TotalConflict;
                    riDataBatch.RecordType = RecordType;
                    riDataBatch.ReceivedAt = ReceivedAt;
                    riDataBatch.SoaDataBatchId = SoaDataBatchId;
                    riDataBatch.FinalisedAt = FinalisedAt;

                    db.Configuration.AutoDetectChangesEnabled = true;
                    DateTime currentPendingDate = riDataBatch.UpdatedAt;

                    if (!concurrentChecking)
                    {
                        riDataBatch.UpdatedAt = DateTime.Now;
                        riDataBatch.UpdatedById = UpdatedById ?? riDataBatch.UpdatedById;
                        db.Entry(riDataBatch).State = EntityState.Modified;
                    }
                    resultCount = db.SaveChanges();

                    #region Concurrency Logic
                    if (concurrentChecking && resultCount > 0)
                    {
                        DateTime updatedDate = DateTime.Now;
                        riDataBatch.UpdatedAt = updatedDate;
                        riDataBatch.UpdatedById = UpdatedById ?? riDataBatch.UpdatedById;
                        db.Entry(riDataBatch).State = EntityState.Modified;

                        connectionStrategy.LineNumber = 335;
                        var checkByPendingDate = db.RiDataBatches.Where(q => q.Id == Id && q.Status == Status && q.UpdatedAt == currentPendingDate).Any();
                        if (!checkByPendingDate)
                            throw new Exception(string.Format(MessageBag.ConcurrentProcess, RiDataBatchBo.GetStatusName(riDataBatch.Status)));

                        connectionStrategy.LineNumber = 345;
                        db.SaveChanges();

                        connectionStrategy.LineNumber = 343;
                        var checkByUpdatedDate = db.RiDataBatches.Where(q => q.Id == Id && q.Status == Status && q.UpdatedAt == updatedDate).Any();
                        if (!checkByUpdatedDate)
                            throw new Exception(string.Format(MessageBag.ConcurrentProcess, RiDataBatchBo.GetStatusName(riDataBatch.Status)));
                    }
                    else if (concurrentChecking && resultCount <= 0)
                    {
                        throw new Exception(string.Format(MessageBag.ConcurrentProcess, RiDataBatchBo.GetStatusName(riDataBatch.Status)));
                    }
                    #endregion
                    return trail;
                }
            });
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataBatch riDataBatch = db.RiDataBatches.Where(q => q.Id == id).FirstOrDefault();
                if (riDataBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataBatch, true);

                db.Entry(riDataBatch).State = EntityState.Deleted;
                db.RiDataBatches.Remove(riDataBatch);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
