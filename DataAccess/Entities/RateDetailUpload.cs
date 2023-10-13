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
    [Table("RateDetailUpload")]
    public class RateDetailUpload
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RateId { get; set; }

        [ForeignKey(nameof(RateId))]
        [ExcludeTrail]
        public virtual Rate Rate { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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

        public RateDetailUpload()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetailUpload.Any(q => q.Id == id);
            }
        }

        public static RateDetailUpload Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateDetailUpload.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RateDetailUpload.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RateDetailUpload rateDetailUpload = RateDetailUpload.Find(Id);
                if (rateDetailUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateDetailUpload, this);

                rateDetailUpload.RateId = RateId;
                rateDetailUpload.Status = Status;
                rateDetailUpload.FileName = FileName;
                rateDetailUpload.HashFileName = HashFileName;
                rateDetailUpload.Errors = Errors;

                rateDetailUpload.UpdatedAt = DateTime.Now;
                rateDetailUpload.UpdatedById = UpdatedById ?? rateDetailUpload.UpdatedById;

                db.Entry(rateDetailUpload).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RateDetailUpload rateDetailUpload = db.RateDetailUpload.Where(q => q.Id == id).FirstOrDefault();
                if (rateDetailUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateDetailUpload, true);

                db.Entry(rateDetailUpload).State = EntityState.Deleted;
                db.RateDetailUpload.Remove(rateDetailUpload);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RateDetailUpload.Where(q => q.RateId == rateId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RateDetailUpload rateDetailUpload in query.ToList())
                {
                    DataTrail trail = new DataTrail(rateDetailUpload, true);
                    trails.Add(trail);

                    db.Entry(rateDetailUpload).State = EntityState.Deleted;
                    db.RateDetailUpload.Remove(rateDetailUpload);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
