using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("ObjectPermissions")]
    public class ObjectPermission
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ObjectId { get; set; }

        [Index]
        public int Type { get; set; }

        [Required, Index]
        public int DepartmentId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }

        public ObjectPermission()
        {
            CreatedAt = DateTime.Now;
        }

        public static ObjectPermission Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ObjectPermissions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static ObjectPermission Find(int type, int objectId)
        {
            using (var db = new AppDbContext())
            {
                return db.ObjectPermissions.Where(q => q.Type == type).Where(q => q.ObjectId == objectId).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ObjectPermissions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.ObjectPermissions.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ObjectPermissions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
