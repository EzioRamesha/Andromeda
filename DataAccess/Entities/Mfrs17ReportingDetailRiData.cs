using DataAccess.Entities.RiDatas;
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
    [Table("Mfrs17ReportingDetailRiDatas")]
    public class Mfrs17ReportingDetailRiData
    {
        [Key, Column(Order = 0)]
        [Index]
        public int Mfrs17ReportingDetailId { get; set; }

        [ForeignKey(nameof(Mfrs17ReportingDetailId))]
        [ExcludeTrail]
        public virtual Mfrs17ReportingDetail Mfrs17ReportingDetail { get; set; }

        [Column(Order = 1)]
        public int? RiDataId { get; set; }

        [Key, Column(Order = 2)]
        [Index]
        public int? RiDataWarehouseId { get; set; }

        [Column(Order = 3)]
        public int RiDataWarehouseHistoryId { get; set; }

        //[ForeignKey(nameof(RiDataWarehouseHistoryId))]
        [ExcludeTrail]
        public virtual RiDataWarehouseHistory RiDataWarehouseHistory { get; set; }

        [Key, Column(Order = 4)]
        [Index]
        public int? CutOffId { get; set; }

        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}|{2}", Mfrs17ReportingDetailId, CutOffId, RiDataWarehouseId);
        }

        public static bool IsExists(int mfrs17ReportingDetailId, int cutOffId, int riDataWarehouseId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                return db.Mfrs17ReportingDetailRiDatas
                    .Any(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId && q.CutOffId == cutOffId && q.RiDataWarehouseId == riDataWarehouseId);
            }
        }

        public static Mfrs17ReportingDetailRiData Find(int mfrs17ReportingDetailId, int cutOffId, int riDataWarehouseId)
        {
            if (mfrs17ReportingDetailId == 0 || cutOffId == 0 || riDataWarehouseId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas
                        .Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId && q.CutOffId == cutOffId && q.RiDataWarehouseId == riDataWarehouseId).FirstOrDefault();
                });
            }
        }

        public static int CountByMfrs17ReportingDetailId(int mfrs17ReportingDetailId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas.Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId).Count();
                });
            }
        }

        public static List<int> GetIdsByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas
                       .Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId)
                       .OrderBy(q => q.RiDataWarehouseId.Value)
                       .Skip(skip)
                       .Take(take)
                       .Select(q => q.RiDataWarehouseId.Value)
                       .ToList();
                });
            }
        }

        public static IList<Mfrs17ReportingDetailRiData> GetByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas
                        .Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId)
                        .OrderBy(q => q.RiDataWarehouseId.Value)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
                });
            }
        }

        public static int CountByMfrs17ReportingIdMfrs17TreatyCode(int mfrs17ReportingDetailId, string mfrs17TreatyCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas
                        .Where(q => q.Mfrs17ReportingDetail.Mfrs17ReportingId == mfrs17ReportingDetailId)
                        .Where(q => q.Mfrs17ReportingDetail.Mfrs17TreatyCode == mfrs17TreatyCode)
                        .Count();
                });
            }
        }

        public static IList<Mfrs17ReportingDetailRiData> GetByMfrs17ReportingIdMfrs17TreatyCode(int mfrs17ReportingDetailId, string mfrs17TreatyCode, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                return connectionStrategy.Execute(() =>
                {
                    return db.Mfrs17ReportingDetailRiDatas
                       .Where(q => q.Mfrs17ReportingDetail.Mfrs17ReportingId == mfrs17ReportingDetailId)
                       .Where(q => q.Mfrs17ReportingDetail.Mfrs17TreatyCode == mfrs17TreatyCode)
                       .OrderBy(q => q.RiDataWarehouseId.Value)
                       .Skip(skip)
                       .Take(take)
                       .ToList();
                });
            }
        }

        public static List<int> GetIdsByMfrs17ReportingDetailIdPage(int mfrs17ReportingDetailId, int skip = 0, int take = 50, int? page = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.Mfrs17ReportingDetailRiDatas
                    .Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId)
                    .Take((page.GetValueOrDefault(0) + 1) * take);

                return query
                    .OrderBy(x => x.RiDataWarehouseHistoryId)
                    .Skip(page.GetValueOrDefault(0) * take)
                    .Select(q => q.RiDataWarehouseHistoryId)
                    .ToList();
            }
        }

        public static IList<Mfrs17ReportingDetailRiData> GetByMfrs17ReportingIdMfrs17TreatyCodePage(int mfrs17ReportingDetailId, string mfrs17TreatyCode, int skip = 0, int take = 50, int? page = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.Mfrs17ReportingDetailRiDatas
                    .Where(q => q.Mfrs17ReportingDetail.Mfrs17ReportingId == mfrs17ReportingDetailId)
                    .Where(q => q.Mfrs17ReportingDetail.Mfrs17TreatyCode == mfrs17TreatyCode)
                    .Take((page.GetValueOrDefault(0) + 1) * take);

                // Now skip to the required page. 
                return query
                    .OrderBy(x => x.RiDataWarehouseHistoryId)
                    .Skip(page.GetValueOrDefault(0) * take)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                DataTrail trail = new DataTrail(this);

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                connectionStrategy.Execute(() =>
                {
                    db.Mfrs17ReportingDetailRiDatas.Add(this);
                    db.SaveChanges();
                });
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.Mfrs17ReportingDetailRiDatas.Add(this);
        }

        public static IList<DataTrail> DeleteAllByMfrs17ReportingDetailId(int mfrs17ReportingDetailId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                List<DataTrail> trails = new List<DataTrail>();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");

                connectionStrategy.Execute(() =>
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [Mfrs17ReportingDetailRiDatas] WHERE [Mfrs17ReportingDetailId] = {0}", mfrs17ReportingDetailId);
                    db.SaveChanges();
                });

                return trails;
            }
        }

        public static void DeleteAllByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                var query = db.Mfrs17ReportingDetailRiDatas.Where(q => q.Mfrs17ReportingDetailId == mfrs17ReportingDetailId);

                foreach (Mfrs17ReportingDetailRiData mfrs17ReportingDetailRiData in query.ToList())
                {
                    var dataTrail = new DataTrail(mfrs17ReportingDetailRiData, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(Mfrs17ReportingDetail)), mfrs17ReportingDetailRiData.PrimaryKey());

                    db.Entry(mfrs17ReportingDetailRiData).State = EntityState.Deleted;
                    db.Mfrs17ReportingDetailRiDatas.Remove(mfrs17ReportingDetailRiData);
                }

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingDetailRiData");
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });
            }
        }
    }
}
