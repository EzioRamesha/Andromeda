using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("TreatyOldCodes")]
    public class TreatyOldCode
    {
        [Key, Column(Order = 0), Required]
        public int TreatyCodeId { get; set; }

        [Key, Column(Order = 1), Required]
        public int OldTreatyCodeId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(TreatyCodeId))]
        public virtual TreatyCode TreatyCode { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(OldTreatyCodeId))]
        public virtual TreatyCode OldTreatyCode { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", TreatyCodeId, OldTreatyCodeId);
        }

        public static bool IsExists(int treatyCodeId, int oldTreatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Any(q => q.TreatyCodeId == treatyCodeId && q.OldTreatyCodeId == oldTreatyCodeId);
            }
        }

        public static TreatyOldCode Find(int treatyCodeId, int oldTreatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId && q.OldTreatyCodeId == oldTreatyCodeId).FirstOrDefault();
            }
        }

        public static TreatyOldCode FindByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId).FirstOrDefault();
            }
        }

        public static int Count(int treatyCodeId, int oldTreatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId && q.OldTreatyCodeId == oldTreatyCodeId).Count();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static IList<TreatyOldCode> Get(int treatyCodeId, int oldTreatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId && q.OldTreatyCodeId == oldTreatyCodeId).ToList();
            }
        }

        public static IList<TreatyOldCode> GetByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId).ToList();
            }
        }

        public static IList<TreatyOldCode> GetByTreatyCodeIdExcept(int treatyCodeId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => q.TreatyCodeId == treatyCodeId && !ids.Contains(q.OldTreatyCodeId)).ToList();
            }
        }

        public static IList<TreatyOldCode> GetAllByTreatyCodeIdExcept(List<int> treatyCodeIds)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyOldCodes.Where(q => !treatyCodeIds.Contains(q.TreatyCodeId)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyOldCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyOldCode treatyOldCode = TreatyOldCode.Find(TreatyCodeId, OldTreatyCodeId);
                if (treatyOldCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyOldCode, this);

                treatyOldCode.TreatyCodeId = TreatyCodeId;
                treatyOldCode.OldTreatyCodeId = OldTreatyCodeId;

                db.Entry(treatyOldCode).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int treatyCodeId, int oldTreatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                var oldCode = Find(treatyCodeId, oldTreatyCodeId);
                if (oldCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(oldCode, true);

                db.Entry(oldCode).State = EntityState.Deleted;
                db.TreatyOldCodes.Remove(oldCode);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyCodeId(List<int> treatyCodeIds)
        {
            using (var db = new AppDbContext())
            {
                IList<TreatyOldCode> treatyOldCodes = GetAllByTreatyCodeIdExcept(treatyCodeIds);
                if (treatyOldCodes == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyOldCode treatyOldCode in treatyOldCodes)
                {
                    DataTrail trail = new DataTrail(treatyOldCode, true);
                    trails.Add(trail);

                    db.Entry(treatyOldCode).State = EntityState.Deleted;
                    db.TreatyOldCodes.Remove(treatyOldCode);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
