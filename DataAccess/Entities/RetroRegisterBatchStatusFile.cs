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

namespace DataAccess.Entities
{
    [Table("RetroRegisterBatchStatusFiles")]
    public class RetroRegisterBatchStatusFile
    {

        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual RetroRegisterBatch RetroRegisterBatch { get; set; }

        [Required, Index]
        public int StatusHistoryId { get; set; }

        [ExcludeTrail]
        public virtual StatusHistory StatusHistory { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RetroRegisterBatchStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchStatusFiles.Any(q => q.Id == id);
            }
        }

        public static RetroRegisterBatchStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RetroRegisterBatchStatusFile FindByRetroRegisterBatchIdStatusHistoryId(int retroRegisterBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchStatusFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault();
            }
        }

        public static IList<RetroRegisterBatchStatusFile> GetRetroRegisterBatchStatusFileByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchStatusFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroRegisterBatchStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroRegisterBatchStatusFile retroRegisterBatchStatusFile = Find(Id);
                if (retroRegisterBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroRegisterBatchStatusFile, this);

                retroRegisterBatchStatusFile.UpdatedAt = DateTime.Now;
                retroRegisterBatchStatusFile.UpdatedById = UpdatedById ?? retroRegisterBatchStatusFile.UpdatedById;

                db.Entry(retroRegisterBatchStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroRegisterBatchStatusFile retroRegisterBatchStatusFile = Find(id);
                if (retroRegisterBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroRegisterBatchStatusFile, true);

                db.Entry(retroRegisterBatchStatusFile).State = EntityState.Deleted;
                db.RetroRegisterBatchStatusFiles.Remove(retroRegisterBatchStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisterBatchStatusFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroRegisterBatchStatusFile retroRegisterBatchStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroRegisterBatchStatusFile, true);
                    trails.Add(trail);

                    db.Entry(retroRegisterBatchStatusFile).State = EntityState.Deleted;
                    db.RetroRegisterBatchStatusFiles.Remove(retroRegisterBatchStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
