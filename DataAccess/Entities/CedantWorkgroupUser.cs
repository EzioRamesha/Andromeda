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
    [Table("CedantWorkgroupUsers")]
    public class CedantWorkgroupUser
    {
        [Key, Column(Order = 0), Required]
        public int CedantWorkgroupId { get; set; }

        [ExcludeTrail]
        public virtual CedantWorkgroup CedantWorkgroup { get; set; }

        [Key, Column(Order = 1), Required]
        public int UserId { get; set; }

        [ExcludeTrail]
        public virtual User User { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", CedantWorkgroupId, UserId);
        }

        public static bool IsExists(int cedantWorkgroupId, int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Any(q => q.CedantWorkgroupId == cedantWorkgroupId && q.UserId == userId);
            }
        }
        
        public static CedantWorkgroupUser Find(int cedantWorkgroupId, int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.CedantWorkgroupId == cedantWorkgroupId && q.UserId == userId).FirstOrDefault();
            }
        }

        public static IList<CedantWorkgroupUser> GetByCedantWorkgroupId(int cedantWorkgroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.CedantWorkgroupId == cedantWorkgroupId).ToList();
            }
        }

        public static IList<CedantWorkgroupUser> GetByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.UserId == userId).ToList();
            }
        }
        
        public static IList<CedantWorkgroupUser> GetByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> userIds)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.CedantWorkgroupId == cedantWorkgroupId && !userIds.Contains(q.UserId)).ToList();
            }
        }

        public static int CountByCedantWorkgroupId(int cedantWorkgroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.CedantWorkgroupId == cedantWorkgroupId).Count();
            }
        }

        public static int CountByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Where(q => q.UserId == userId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                DataTrail trail = new DataTrail(this);

                db.CedantWorkgroupUsers.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public DataTrail Delete()
        {
            using (var db = new AppDbContext())
            {
                CedantWorkgroupUser cedantWorkgroupUser = Find(CedantWorkgroupId, UserId);
                if (cedantWorkgroupUser == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedantWorkgroupUser, true);

                db.Entry(cedantWorkgroupUser).State = EntityState.Deleted;
                db.CedantWorkgroupUsers.Remove(cedantWorkgroupUser);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
