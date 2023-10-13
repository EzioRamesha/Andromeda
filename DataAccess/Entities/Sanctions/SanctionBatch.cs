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

namespace DataAccess.Entities.Sanctions
{
    [Table("SanctionBatches")]
    public class SanctionBatch
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SourceId { get; set; }
        [ExcludeTrail]
        public virtual Source Source { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Required, Index]
        public int Method { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int Record { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UploadedAt { get; set; }

        public string Errors { get; set; }

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

        public SanctionBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBatches.Any(q => q.Id == id);
            }
        }

        public static SanctionBatch Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBatches.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionBatches.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SanctionBatch entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.SourceId = SourceId;
                entity.FileName = FileName;
                entity.HashFileName = HashFileName;
                entity.Record = Record;
                entity.Method = Method;
                entity.Status = Status;
                entity.UploadedAt = UploadedAt;
                entity.Errors = Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SanctionBatch entity = db.SanctionBatches.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionBatches.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
