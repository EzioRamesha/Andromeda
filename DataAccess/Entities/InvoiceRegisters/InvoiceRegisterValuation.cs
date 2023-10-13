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
    [Table("InvoiceRegisterValuations")]
    public class InvoiceRegisterValuation
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int InvoiceRegisterId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegister InvoiceRegister { get; set; }

        [Required, Index]
        public int ValuationBenefitCodeId { get; set; }

        [ExcludeTrail]
        public virtual Benefit ValuationBenefitCode { get; set; }

        public double? Amount { get; set; }

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

        public InvoiceRegisterValuation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterValuations.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegisterValuation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterValuations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegisterValuation> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterValuations.OrderBy(q => q.Id).ToList();
            }
        }

        public static IList<InvoiceRegisterValuation> Get(int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterValuations.OrderBy(q => q.Id).Skip(skip).Take(take).ToList();
            }
        }

        public static IList<InvoiceRegisterValuation> GetByInvoiceRegisterId(int invoiceRegisterId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterValuations.Where(q => q.InvoiceRegisterId == invoiceRegisterId).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisterValuations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterValuation invoiceRegisterValuations = Find(Id);
                if (invoiceRegisterValuations == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiceRegisterValuations, this);

                invoiceRegisterValuations.InvoiceRegisterId = InvoiceRegisterId;
                invoiceRegisterValuations.ValuationBenefitCodeId = ValuationBenefitCodeId;
                invoiceRegisterValuations.Amount = Amount;
                invoiceRegisterValuations.UpdatedAt = DateTime.Now;
                invoiceRegisterValuations.UpdatedById = UpdatedById ?? invoiceRegisterValuations.UpdatedById;

                db.Entry(invoiceRegisterValuations).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                InvoiceRegisterValuation invoiceRegisterValuations = db.InvoiceRegisterValuations.Where(q => q.Id == id).FirstOrDefault();
                if (invoiceRegisterValuations == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(invoiceRegisterValuations, true);

                db.Entry(invoiceRegisterValuations).State = EntityState.Deleted;
                db.InvoiceRegisterValuations.Remove(invoiceRegisterValuations);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterId(int invoiceRegisterId)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.InvoiceRegisterValuations.Where(q => q.InvoiceRegisterId == invoiceRegisterId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (InvoiceRegisterValuation valuation in query.ToList())
                {
                    DataTrail trail = new DataTrail(valuation, true);
                    trails.Add(trail);

                    db.Entry(valuation).State = EntityState.Deleted;
                    db.InvoiceRegisterValuations.Remove(valuation);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
