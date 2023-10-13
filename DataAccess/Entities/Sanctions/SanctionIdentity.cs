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
    [Table("SanctionIdentities")]
    public class SanctionIdentity
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SanctionId { get; set; }
        [ExcludeTrail]
        public virtual Sanction Sanction { get; set; }

        [MaxLength(128), Index]
        public string IdType { get; set; }

        [MaxLength(128), Index]
        public string IdNumber { get; set; }

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

        public SanctionIdentity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionIdentities.Any(q => q.Id == id);
            }
        }

        public static SanctionIdentity Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionIdentities.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SanctionIdentities.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.SanctionId = SanctionId;
                entity.IdType = IdType;
                entity.IdNumber = IdNumber;
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
                var entity = db.SanctionIdentities.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionIdentities.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionIdentities.Where(q => q.SanctionId == sanctionId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionIdentities.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionIdentities.Where(q => q.Sanction.SanctionBatchId == sanctionBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (var entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionIdentities.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
