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
    [Table("Emails")]
    public class Email
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? RecipientUserId { get; set; }
        [ExcludeTrail]
        public virtual User RecipientUser { get; set; }

        [MaxLength(64), Index]
        public string ModuleController { get; set; }

        [Index]
        public int? ObjectId { get; set; }

        [Index]
        public int Type { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(256), Index]
        public string EmailAddress { get; set; }

        public string Data { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public Email()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Emails.Any(q => q.Id == id);
            }
        }

        public static Email Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Emails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Emails.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.RecipientUserId = RecipientUserId;
                entity.ModuleController = ModuleController;
                entity.ObjectId = ObjectId;
                entity.Type = Type;
                entity.Status = Status;
                entity.Data = Data;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById != 0 ? UpdatedById : entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.Emails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
