using BusinessObject.Identity;
using DataAccess.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.Identity
{
    [Table("UserTrails")]
    public class UserTrail
    {
        public UserTrail()
        {
            Type = 1;
            CreatedAt = DateTime.Now;
        }

        public UserTrail(UserTrailBo bo) : this()
        {
            Type = bo.Type;
            Controller = bo.Controller;
            Description = bo.Description;
            ObjectId = bo.ObjectId;
            Data = bo.Data;
            CreatedById = bo.CreatedById;
        }

        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [Required, Index, MaxLength(64)]
        public string Controller { get; set; }

        [Index]
        public int ObjectId { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(128)]
        public string IpAddress { get; set; }

        public string Data { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        // DO NOT FOREIGN
        //[ExcludeTrail]
        //public virtual User CreatedBy { get; set; }

        public static UserTrail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.UserTrails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public void Create()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("UserTrail");
                connectionStrategy.Execute(() =>
                {
                    db.UserTrails.Add(this);
                    db.SaveChanges();
                });
            }
        }

        public void Create(AppDbContext db, bool save = true)
        {
            db.UserTrails.Add(this);
            if (save)
                db.SaveChanges();
        }
    }
}
