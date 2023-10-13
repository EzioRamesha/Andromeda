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
    [Table("RateTableMappingUpload")]
    public class RateTableMappingUpload
    {
        [Key]
        public int Id { get; set; }

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

        public RateTableMappingUpload()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTableMappingUpload.Any(q => q.Id == id);
            }
        }

        public static RateTableMappingUpload Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTableMappingUpload.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RateTableMappingUpload.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RateTableMappingUpload rateTableMappingUpload = RateTableMappingUpload.Find(Id);
                if (rateTableMappingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateTableMappingUpload, this);

                rateTableMappingUpload.Status = Status;
                rateTableMappingUpload.FileName = FileName;
                rateTableMappingUpload.HashFileName = HashFileName;
                rateTableMappingUpload.Errors = Errors;

                rateTableMappingUpload.UpdatedAt = DateTime.Now;
                rateTableMappingUpload.UpdatedById = UpdatedById ?? rateTableMappingUpload.UpdatedById;

                db.Entry(rateTableMappingUpload).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RateTableMappingUpload rateTableMappingUpload = db.RateTableMappingUpload.Where(q => q.Id == id).FirstOrDefault();
                if (rateTableMappingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateTableMappingUpload, true);

                db.Entry(rateTableMappingUpload).State = EntityState.Deleted;
                db.RateTableMappingUpload.Remove(rateTableMappingUpload);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
