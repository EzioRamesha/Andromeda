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
    [Table("InvoiceRegisterBatchRemarkDocuments")]
    public class InvoiceRegisterBatchRemarkDocument
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int InvoiceRegisterBatchRemarkId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatchRemark InvoiceRegisterBatchRemark { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

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

        public InvoiceRegisterBatchRemarkDocument()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarkDocuments.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegisterBatchRemarkDocument Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarkDocuments.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterBatchRemarkDocument> GetByInvoiceRegisterBatchRemarkId(int invoiceRegisterBatchRemarkId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarkDocuments.Where(q => q.InvoiceRegisterBatchRemarkId == invoiceRegisterBatchRemarkId).ToList();
            }
        }

        public static IList<InvoiceRegisterBatchRemarkDocument> GetByInvoiceRegisterBatchRemarkIdExcept(int invoiceRegisterBatchRemarkId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarkDocuments.Where(q => q.InvoiceRegisterBatchRemarkId == invoiceRegisterBatchRemarkId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterBatchRemarkDocuments.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchRemarkDocument document = Find(Id);
                if (document == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(document, this);

                document.InvoiceRegisterBatchRemarkId = InvoiceRegisterBatchRemarkId;
                document.FileName = FileName;
                document.HashFileName = HashFileName;
                document.UpdatedAt = DateTime.Now;
                document.UpdatedById = UpdatedById ?? document.UpdatedById;

                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchRemarkDocument document = Find(id);
                if (document == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(document, true);

                db.Entry(document).State = EntityState.Deleted;
                db.InvoiceRegisterBatchRemarkDocuments.Remove(document);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
