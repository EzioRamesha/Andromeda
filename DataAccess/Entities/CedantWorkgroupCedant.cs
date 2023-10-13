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
    [Table("CedantWorkgroupCedants")]
    public class CedantWorkgroupCedant
    {
        [Key, Column(Order = 0), Required]
        public int CedantWorkgroupId { get; set; }
        [ExcludeTrail]
        public virtual CedantWorkgroup CedantWorkgroup { get; set; }

        [Key, Column(Order = 1), Required]
        public int CedantId { get; set; }
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", CedantWorkgroupId, CedantId);
        }

        public static bool IsExists(int cedantWorkgroupId, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Any(q => q.CedantWorkgroupId == cedantWorkgroupId && q.CedantId == cedantId);
            }
        }
        
        public static bool IsDuplicate(int cedantWorkgroupId, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Any(q => q.CedantWorkgroupId != cedantWorkgroupId && q.CedantId == cedantId);
            }
        }

        public static CedantWorkgroupCedant Find(int cedantWorkgroupId, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantWorkgroupId == cedantWorkgroupId && q.CedantId == cedantId).FirstOrDefault();
            }
        }

        public static IList<CedantWorkgroupCedant> GetByCedantWorkgroupId(int cedantWorkgroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantWorkgroupId == cedantWorkgroupId).ToList();
            }
        }
        
        public static IList<CedantWorkgroupCedant> GetByCedantCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantId == cedantId).ToList();
            }
        }

        public static IList<CedantWorkgroupCedant> GetByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> cedantIds)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantWorkgroupId == cedantWorkgroupId && !cedantIds.Contains(q.CedantId)).ToList();
            }
        }

        public static int CountByCedantWorkgroupId(int cedantWorkgroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantWorkgroupId == cedantWorkgroupId).Count();
            }
        }
        
        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupCedants.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                DataTrail trail = new DataTrail(this);

                db.CedantWorkgroupCedants.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public DataTrail Delete()
        {
            using (var db = new AppDbContext())
            {
                CedantWorkgroupCedant cedantWorkgroupCedant = Find(CedantWorkgroupId, CedantId);
                if (cedantWorkgroupCedant == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedantWorkgroupCedant, true);

                db.Entry(cedantWorkgroupCedant).State = EntityState.Deleted;
                db.CedantWorkgroupCedants.Remove(cedantWorkgroupCedant);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
