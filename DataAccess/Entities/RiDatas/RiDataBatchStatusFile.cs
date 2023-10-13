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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataBatchStatusFiles")]
    public class RiDataBatchStatusFile
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RiDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual RiDataBatch RiDataBatch { get; set; }

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

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RiDataBatchStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataBatchStatusFiles.Any(q => q.Id == id);
            }
        }

        public static RiDataBatchStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataBatchStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RiDataBatchStatusFile FindByRiDataBatchIdStatusHistoryId(int riDataBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataBatchStatusFiles.Where(q => q.RiDataBatchId == riDataBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault();
            }
        }

        public static IList<RiDataBatchStatusFile> GetRiDataBatchStatusFileByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataBatchStatusFiles.Where(q => q.RiDataBatchId == riDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataBatchStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataBatchStatusFile riDataBatchStatusFile = Find(Id);
                if (riDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataBatchStatusFile, this);

                riDataBatchStatusFile.UpdatedAt = DateTime.Now;
                riDataBatchStatusFile.UpdatedById = UpdatedById ?? riDataBatchStatusFile.UpdatedById;

                db.Entry(riDataBatchStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataBatchStatusFile riDataBatchStatusFile = Find(id);
                if (riDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataBatchStatusFile, true);

                db.Entry(riDataBatchStatusFile).State = EntityState.Deleted;
                db.RiDataBatchStatusFiles.Remove(riDataBatchStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataBatchStatusFiles.Where(q => q.RiDataBatchId == riDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataBatchStatusFile riDataBatchStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataBatchStatusFile, true);
                    trails.Add(trail);

                    db.Entry(riDataBatchStatusFile).State = EntityState.Deleted;
                    db.RiDataBatchStatusFiles.Remove(riDataBatchStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
