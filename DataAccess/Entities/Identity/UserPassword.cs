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

namespace DataAccess.Entities.Identity
{
    [Table("UserPasswords")]
    public class UserPassword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Key, Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string PasswordHash { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        // DO NOT FOREIGN
        //[ExcludeTrail]
        //public virtual User CreatedBy { get; set; }

        public UserPassword()
        {
            CreatedAt = DateTime.Now;
        }

        public static IList<UserPassword> GetByUserId(int userId, int skip = 0)
        {
            using (var db = new AppDbContext())
            {
                return db.UserPasswords.Where(up => up.UserId == userId).OrderByDescending(up => up.CreatedAt).Skip(skip).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.UserPasswords.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                UserPassword userPassword = db.UserPasswords.Where(q => q.Id == id).FirstOrDefault();
                if (userPassword == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(userPassword, true);

                db.Entry(userPassword).State = EntityState.Deleted;
                db.UserPasswords.Remove(userPassword);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.UserPasswords.Where(q => q.UserId == userId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (UserPassword up in query.ToList())
                {
                    DataTrail trail = new DataTrail(up, true);
                    trails.Add(trail);

                    db.Entry(up).State = EntityState.Deleted;
                    db.UserPasswords.Remove(up);
                }
                db.SaveChanges();

                return trails;
            }
        }
    }
}
