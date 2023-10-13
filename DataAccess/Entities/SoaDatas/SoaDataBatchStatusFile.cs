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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataBatchStatusFiles")]
    public class SoaDataBatchStatusFile
    {

        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

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

        public SoaDataBatchStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataBatchStatusFiles.Any(q => q.Id == id);
            }
        }

        public static SoaDataBatchStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataBatchStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static SoaDataBatchStatusFile FindBySoaDataBatchIdStatusHistoryId(int soaDataBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataBatchStatusFiles.Where(q => q.SoaDataBatchId == soaDataBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault();
            }
        }

        public static IList<SoaDataBatchStatusFile> GetSoaDataBatchStatusFileBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataBatchStatusFiles.Where(q => q.SoaDataBatchId == soaDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataBatchStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SoaDataBatchStatusFile soaDataBatchStatusFile = Find(Id);
                if (soaDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataBatchStatusFile, this);

                soaDataBatchStatusFile.UpdatedAt = DateTime.Now;
                soaDataBatchStatusFile.UpdatedById = UpdatedById ?? soaDataBatchStatusFile.UpdatedById;

                db.Entry(soaDataBatchStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SoaDataBatchStatusFile soaDataBatchStatusFile = Find(id);
                if (soaDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataBatchStatusFile, true);

                db.Entry(soaDataBatchStatusFile).State = EntityState.Deleted;
                db.SoaDataBatchStatusFiles.Remove(soaDataBatchStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataBatchStatusFiles.Where(q => q.SoaDataBatchId == soaDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (SoaDataBatchStatusFile soaDataBatchStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(soaDataBatchStatusFile, true);
                    trails.Add(trail);

                    db.Entry(soaDataBatchStatusFile).State = EntityState.Deleted;
                    db.SoaDataBatchStatusFiles.Remove(soaDataBatchStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
