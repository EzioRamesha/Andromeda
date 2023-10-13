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

namespace DataAccess.Entities
{
    [Table("ObjectLocks")]
    public class ObjectLock
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ModuleId { get; set; }
        [ExcludeTrail]
        public virtual Module Module { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        [Required, Index]
        public int LockedById { get; set; }
        [ExcludeTrail]
        public virtual User LockedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ExpiresAt { get; set; }

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

        public ObjectLock()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ObjectLocks.Any(q => q.Id == id);
            }
        }

        public static ObjectLock Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return Find(id, db);
            }
        }

        public static ObjectLock Find(int? id, AppDbContext db)
        {
            return db.ObjectLocks.Where(q => q.Id == id).FirstOrDefault();
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ObjectLocks.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.ExpiresAt = ExpiresAt;
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
                var entity = Find(id, db);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ObjectLocks.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
