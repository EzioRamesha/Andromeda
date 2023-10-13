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

namespace DataAccess.Entities
{
    [Table("RetroRegisterBatches")]
    public class RetroRegisterBatch
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int BatchNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BatchDate { get; set; }

        public int Type { get; set; }

        public int TotalInvoice { get; set; }

        [Index]
        public int Status { get; set; }

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

        public RetroRegisterBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (BatchNo != 0)
                {
                    var query = db.RetroRegisterBatches.Where(q => q.BatchNo == BatchNo);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static RetroRegisterBatch Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RetroRegisterBatch FindByBatchNo(int batchNo)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Where(q => q.BatchNo == batchNo).FirstOrDefault();
            }
        }

        public static RetroRegisterBatch FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<RetroRegisterBatch> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.OrderBy(q => q.BatchNo).ToList();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Where(q => q.Status == status).Count();
            }
        }

        public static int GetMaxId()
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatches.Max(q => (int?)q.Id) ?? 0;
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroRegisterBatches.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var retroRegisterBatch = Find(Id);
                if (retroRegisterBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(retroRegisterBatch, this);

                retroRegisterBatch.BatchNo = BatchNo;
                retroRegisterBatch.BatchDate = BatchDate;
                retroRegisterBatch.Type = Type;
                retroRegisterBatch.Status = Status;
                retroRegisterBatch.TotalInvoice = TotalInvoice;
                retroRegisterBatch.UpdatedAt = DateTime.Now;
                retroRegisterBatch.UpdatedById = UpdatedById ?? retroRegisterBatch.UpdatedById;

                db.Entry(retroRegisterBatch).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var retroRegisterBatch = db.RetroRegisterBatches.Where(q => q.Id == id).FirstOrDefault();
                if (retroRegisterBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(retroRegisterBatch, true);

                db.Entry(retroRegisterBatch).State = EntityState.Deleted;
                db.RetroRegisterBatches.Remove(retroRegisterBatch);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
