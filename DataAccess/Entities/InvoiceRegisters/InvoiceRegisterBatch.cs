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

namespace DataAccess.Entities.InvoiceRegisters
{
    [Table("InvoiceRegisterBatches")]
    public class InvoiceRegisterBatch
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int BatchNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BatchDate { get; set; }

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

        public InvoiceRegisterBatch()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (BatchNo != 0)
                {
                    var query = db.InvoiceRegisterBatches.Where(q => q.BatchNo == BatchNo);
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static InvoiceRegisterBatch Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static InvoiceRegisterBatch FindByBatchNo(int batchNo)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Where(q => q.BatchNo == batchNo).FirstOrDefault();
            }
        }

        public static InvoiceRegisterBatch FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterBatch> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.OrderBy(q => q.BatchNo).ToList();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Where(q => q.Status == status).Count();
            }
        }

        public static int GetMaxId()
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatches.Max(q => (int?)q.Id) ?? 0;
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterBatches.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatch invoiceRegisterBatch = Find(Id);
                if (invoiceRegisterBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiceRegisterBatch, this);

                invoiceRegisterBatch.BatchNo = BatchNo;
                invoiceRegisterBatch.BatchDate = BatchDate;
                invoiceRegisterBatch.Status = Status;
                invoiceRegisterBatch.TotalInvoice = TotalInvoice;
                invoiceRegisterBatch.UpdatedAt = DateTime.Now;
                invoiceRegisterBatch.UpdatedById = UpdatedById ?? invoiceRegisterBatch.UpdatedById;

                db.Entry(invoiceRegisterBatch).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatch invoiceRegisterBatch = db.InvoiceRegisterBatches.Where(q => q.Id == id).FirstOrDefault();
                if (invoiceRegisterBatch == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiceRegisterBatch, true);

                db.Entry(invoiceRegisterBatch).State = EntityState.Deleted;
                db.InvoiceRegisterBatches.Remove(invoiceRegisterBatch);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
