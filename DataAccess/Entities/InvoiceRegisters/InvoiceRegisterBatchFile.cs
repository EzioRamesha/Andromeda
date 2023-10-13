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
    [Table("InvoiceRegisterBatchFiles")]
    public class InvoiceRegisterBatchFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatch InvoiceRegisterBatch { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        public int Type { get; set; }

        [Required, Index]
        public int Status { get; set; }

        public string Errors { get; set; }

        public bool DataUpdate { get; set; } = false;

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

        public InvoiceRegisterBatchFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegisterBatchFile Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static InvoiceRegisterBatchFile FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.Status == status).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterBatchFile> GetAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<InvoiceRegisterBatchFile> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId, bool dataUpdate = false)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId && q.DataUpdate == dataUpdate).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<InvoiceRegisterBatchFile> GetByInvoiceRegisterBatchIdStatus(int invoiceRegisterBatchId, int status, bool dataUpdate = false)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId && q.Status == status && q.DataUpdate == dataUpdate).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static int CountBInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterBatchFiles.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchFile invoiveDataFile = Find(Id);
                if (invoiveDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiveDataFile, this);

                invoiveDataFile.InvoiceRegisterBatchId = InvoiceRegisterBatchId;
                invoiveDataFile.FileName = FileName;
                invoiveDataFile.HashFileName = HashFileName;
                invoiveDataFile.Type = Type;
                invoiveDataFile.Status = Status;
                invoiveDataFile.Errors = Errors;
                invoiveDataFile.DataUpdate = DataUpdate;
                invoiveDataFile.UpdatedAt = DateTime.Now;
                invoiveDataFile.UpdatedById = UpdatedById ?? invoiveDataFile.UpdatedById;

                db.Entry(invoiveDataFile).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchFile invoiveDataFile = Find(id);
                if (invoiveDataFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiveDataFile, true);

                db.Entry(invoiveDataFile).State = EntityState.Deleted;
                db.InvoiceRegisterBatchFiles.Remove(invoiveDataFile);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisterBatchFiles.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (InvoiceRegisterBatchFile invoiveDataFile in query.ToList())
                {
                    DataTrail trail = new DataTrail(invoiveDataFile, true);
                    trails.Add(trail);

                    db.Entry(invoiveDataFile).State = EntityState.Deleted;
                    db.InvoiceRegisterBatchFiles.Remove(invoiveDataFile);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
