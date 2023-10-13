using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("RetroRegisterBatchDirectRetros")]
    public class RetroRegisterBatchDirectRetro
    {
        [Key, Column(Order = 0)]
        public int RetroRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual RetroRegisterBatch RetroRegisterBatch { get; set; }

        [Key, Column(Order = 1)]
        public int DirectRetroId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetro DirectRetro { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", RetroRegisterBatchId, DirectRetroId);
        }

        public static bool IsExists(int retroRegisterBatchId, int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchDirectRetros
                    .Any(q => q.RetroRegisterBatchId == retroRegisterBatchId && q.DirectRetroId == directRetroId);
            }
        }

        public static RetroRegisterBatchDirectRetro Find(int retroRegisterBatchId, int directRetroId)
        {
            if (retroRegisterBatchId == 0 || directRetroId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchDirectRetros
                    .Where(q => q.RetroRegisterBatchId == retroRegisterBatchId && q.DirectRetroId == directRetroId)
                    .FirstOrDefault();
            }
        }

        public static int CountByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RetroRegisterBatchDirectRetros.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId).Count();
            }
        }

        public static List<int> GetIdsByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RetroRegisterBatchDirectRetros
                    .Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .OrderBy(q => q.DirectRetroId)
                    .Select(q => q.DirectRetroId)
                    .ToList();
            }
        }

        public static IList<RetroRegisterBatchDirectRetro> GetByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RetroRegisterBatchDirectRetros
                    .Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .OrderBy(q => q.DirectRetroId)
                    .ToList();
            }
        }

        public static IList<RetroRegisterBatchDirectRetro> GetByRetroRegisterBatchId(int retroRegisterBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RetroRegisterBatchDirectRetros
                    .Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .OrderBy(q => q.DirectRetroId)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public static int CountByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchDirectRetros.Where(q => q.DirectRetroId == directRetroId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.RetroRegisterBatchDirectRetros.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.RetroRegisterBatchDirectRetros.Add(this);
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [RetroRegisterBatchDirectRetros] WHERE [RetroRegisterBatchId] = {0}", retroRegisterBatchId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroRegisterBatchDirectRetros.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId);

                foreach (RetroRegisterBatchDirectRetro retroDirectData in query.ToList())
                {
                    var dataTrail = new DataTrail(retroDirectData, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroRegisterBatch)), retroDirectData.PrimaryKey());

                    db.Entry(retroDirectData).State = EntityState.Deleted;
                    db.RetroRegisterBatchDirectRetros.Remove(retroDirectData);
                }

                db.SaveChanges();
            }
        }
    }
}
