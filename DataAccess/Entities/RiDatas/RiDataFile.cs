using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataFiles")]
    public class RiDataFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RiDataBatchId { get; set; }

        [ForeignKey(nameof(RiDataBatchId))]
        [ExcludeTrail]
        public virtual RiDataBatch RiDataBatch { get; set; }

        [Required]
        public int RawFileId { get; set; }

        [ForeignKey(nameof(RawFileId))]
        [ExcludeTrail]
        public virtual RawFile RawFile { get; set; }

        public int? TreatyCodeId { get; set; }

        public int? TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Index]
        public int? RiDataConfigId { get; set; }

        [ForeignKey(nameof(RiDataConfigId))]
        [ExcludeTrail]
        public virtual RiDataConfig RiDataConfig { get; set; }

        public string Configs { get; set; }

        public string OverrideProperties { get; set; }

        [Required]
        public int Mode { get; set; }

        [Required, Index]
        public int Status { get; set; }

        public string Errors { get; set; }

        [Required, Index]
        public int RecordType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RiDataFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RawFiles.Any(q => q.Id == id);
            }
        }

        public static RiDataFile Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RiDataFile FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataFiles.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<RiDataFile> GetByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataFiles.Where(q => q.RiDataBatchId == riDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<RiDataFile> GetByRiDataBatchIdMode(int riDataBatchId, int Mode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(131, "RiDataComputation");
                GlobalProcessRiDataConnectionStrategy.SetRiDataBatchId(riDataBatchId);

                return connectionStrategy.Execute(() => db.RiDataFiles.Where(q => q.RiDataBatchId == riDataBatchId && q.Mode == Mode).OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static int CountByRiDataConfigIdStatus(int riDataConfigId, int[] status)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataFiles.Where(q => q.RiDataConfigId == riDataConfigId && status.Contains(q.RiDataBatch.Status)).Count();
            }
        }

        public static IList<RiDataFile> GetByRiDataBatchIdExcept(int riDataBatchId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataFiles.Where(q => q.RiDataBatchId == riDataBatchId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataFile riDataFile = Find(Id);
                if (riDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataFile, this);

                riDataFile.RiDataBatchId = RiDataBatchId;
                riDataFile.RawFileId = RawFileId;
                riDataFile.TreatyId = TreatyId;
                riDataFile.Configs = Configs;
                riDataFile.RiDataConfigId = RiDataConfigId;
                riDataFile.OverrideProperties = OverrideProperties;
                riDataFile.Mode = Mode;
                riDataFile.Status = Status;
                riDataFile.Errors = Errors;
                riDataFile.RecordType = RecordType;
                riDataFile.UpdatedAt = DateTime.Now;
                riDataFile.UpdatedById = UpdatedById ?? riDataFile.UpdatedById;

                db.Entry(riDataFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataFile riDataFile = Find(id);
                if (riDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataFile, true);

                db.Entry(riDataFile).State = EntityState.Deleted;
                db.RiDataFiles.Remove(riDataFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataFiles.Where(q => q.RiDataBatchId == riDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataFile riDataFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataFile, true);
                    trails.Add(trail);

                    db.Entry(riDataFile).State = EntityState.Deleted;
                    db.RiDataFiles.Remove(riDataFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
