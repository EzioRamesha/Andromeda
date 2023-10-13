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
    [Table("InvoiceRegisterBatchStatusFiles")]
    public class InvoiceRegisterBatchStatusFile
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int InvoiceRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatch InvoiceRegisterBatch { get; set; }

        [Required, Index]
        public int StatusHistoryId { get; set; }

        [ExcludeTrail]
        public virtual StatusHistory StatusHistory { get; set; }

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

        public InvoiceRegisterBatchStatusFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchStatusFiles.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegisterBatchStatusFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchStatusFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static InvoiceRegisterBatchStatusFile FindByInvoiceRegisterBatchIdStatusHistoryId(int invoiceRegisterBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchStatusFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterBatchStatusFile> GetInvoiceRegisterBatchStatusFileByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchStatusFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterBatchStatusFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchStatusFile InvoiceRegisterBatchStatusFile = Find(Id);
                if (InvoiceRegisterBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(InvoiceRegisterBatchStatusFile, this);

                InvoiceRegisterBatchStatusFile.UpdatedAt = DateTime.Now;
                InvoiceRegisterBatchStatusFile.UpdatedById = UpdatedById ?? InvoiceRegisterBatchStatusFile.UpdatedById;

                db.Entry(InvoiceRegisterBatchStatusFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchStatusFile InvoiceRegisterBatchStatusFile = Find(id);
                if (InvoiceRegisterBatchStatusFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(InvoiceRegisterBatchStatusFile, true);

                db.Entry(InvoiceRegisterBatchStatusFile).State = EntityState.Deleted;
                db.InvoiceRegisterBatchStatusFiles.Remove(InvoiceRegisterBatchStatusFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisterBatchStatusFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (InvoiceRegisterBatchStatusFile InvoiceRegisterBatchStatusFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(InvoiceRegisterBatchStatusFile, true);
                    trails.Add(trail);

                    db.Entry(InvoiceRegisterBatchStatusFile).State = EntityState.Deleted;
                    db.InvoiceRegisterBatchStatusFiles.Remove(InvoiceRegisterBatchStatusFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
