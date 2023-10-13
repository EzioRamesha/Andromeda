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

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataBatchStatusFiles")]
    public class ClaimDataBatchStatusFile
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimDataBatchId { get; set; }

        [ForeignKey(nameof(ClaimDataBatchId))]
        [ExcludeTrail]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        [Required, Index]
        public int StatusHistoryId { get; set; }

        [ForeignKey(nameof(StatusHistoryId))]
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

        public ClaimDataBatchStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataBatchStatusFiles.Any(q => q.Id == id);
            }
        }

        public static ClaimDataBatchStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataBatchStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ClaimDataBatchStatusFile FindByClaimDataBatchIdStatusHistoryId(int claimDataBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataBatchStatusFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault();
            }
        }

        public static IList<ClaimDataBatchStatusFile> GetByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataBatchStatusFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataBatchStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataBatchStatusFile claimDataBatchStatusFile = Find(Id);
                if (claimDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataBatchStatusFile, this);

                claimDataBatchStatusFile.UpdatedAt = DateTime.Now;
                claimDataBatchStatusFile.UpdatedById = UpdatedById ?? claimDataBatchStatusFile.UpdatedById;

                db.Entry(claimDataBatchStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataBatchStatusFile claimDataBatchStatusFile = Find(id);
                if (claimDataBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataBatchStatusFile, true);

                db.Entry(claimDataBatchStatusFile).State = EntityState.Deleted;
                db.ClaimDataBatchStatusFiles.Remove(claimDataBatchStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataBatchStatusFiles.Where(q => q.ClaimDataBatchId == claimDataBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataBatchStatusFile claimDataBatchStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataBatchStatusFile, true);
                    trails.Add(trail);

                    db.Entry(claimDataBatchStatusFile).State = EntityState.Deleted;
                    db.ClaimDataBatchStatusFiles.Remove(claimDataBatchStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
