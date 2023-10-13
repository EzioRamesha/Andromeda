using BusinessObject.InvoiceRegisters;
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
    [Table("InvoiceRegisterBatchRemarks")]
    public class InvoiceRegisterBatchRemark
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int InvoiceRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatch InvoiceRegisterBatch { get; set; }

        public int Status { get; set; }

        public bool RemarkPermission { get; set; } = false;

        public string Content { get; set; }

        public bool FilePermission { get; set; } = false;

        public bool FollowUp { get; set; } = false;

        public int? FollowUpStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FollowUpDate { get; set; }

        public int? FollowUpUserId { get; set; }

        [ExcludeTrail]
        public virtual User FollowUpUser { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<InvoiceRegisterBatchRemarkDocument> InvoiceRegisterBatchRemarkDocuments { get; set; }

        public InvoiceRegisterBatchRemark()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public InvoiceRegisterBatchRemark(InvoiceRegisterBatchRemarkBo remarkBo) : this()
        {
            Id = remarkBo.Id;
            InvoiceRegisterBatchId = remarkBo.InvoiceRegisterBatchId;
            Status = remarkBo.Status;
            RemarkPermission = remarkBo.RemarkPermission;
            Content = remarkBo.Content;
            FilePermission = remarkBo.FilePermission;
            FollowUp = remarkBo.FollowUp;
            FollowUpStatus = remarkBo.FollowUpStatus;
            FollowUpDate = remarkBo.FollowUpDate;
            FollowUpUserId = remarkBo.FollowUpUserId;
            CreatedById = remarkBo.CreatedById;
            UpdatedById = remarkBo.UpdatedById;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarks.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegisterBatchRemark Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarks.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterBatchRemark> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarks.OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public static IList<InvoiceRegisterBatchRemark> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchRemarks.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterBatchRemarks.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchRemark remark = Find(Id);
                if (remark == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(remark, this);

                remark.InvoiceRegisterBatchId = InvoiceRegisterBatchId;
                remark.Status = Status;
                remark.RemarkPermission = RemarkPermission;
                remark.Content = Content;
                remark.FilePermission = FilePermission;
                remark.FollowUp = FollowUp;
                remark.FollowUpDate = FollowUpDate;
                remark.FollowUpStatus = FollowUpStatus;
                remark.FollowUpUserId = FollowUpUserId;
                remark.UpdatedAt = DateTime.Now;
                remark.UpdatedById = UpdatedById ?? remark.UpdatedById;

                db.Entry(remark).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterBatchRemark remark = db.InvoiceRegisterBatchRemarks.Where(q => q.Id == id).FirstOrDefault();
                if (remark == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(remark, true);

                db.Entry(remark).State = EntityState.Deleted;
                db.InvoiceRegisterBatchRemarks.Remove(remark);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisterBatchRemarks.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (InvoiceRegisterBatchRemark remark in query.ToList())
                {
                    DataTrail trail = new DataTrail(remark, true);
                    trails.Add(trail);

                    db.Entry(remark).State = EntityState.Deleted;
                    db.InvoiceRegisterBatchRemarks.Remove(remark);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
