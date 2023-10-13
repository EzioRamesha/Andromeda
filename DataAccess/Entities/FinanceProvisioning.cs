using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("FinanceProvisionings")]
    public class FinanceProvisioning
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        [Index]
        public int Status { get; set; }

        [Required, Index]
        public int ClaimsProvisionRecord { get; set; }

        [Required, Index]
        public double ClaimsProvisionAmount { get; set; }

        [Required, Index]
        public int DrProvisionRecord { get; set; }

        [Required, Index]
        public double DrProvisionAmount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProvisionAt { get; set; }

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

        public FinanceProvisioning()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FinanceProvisionings.Any(q => q.Id == id);
            }
        }

        public static FinanceProvisioning Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.FinanceProvisionings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.FinanceProvisionings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                FinanceProvisioning financeProvisioning = Find(Id);
                if (financeProvisioning == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(financeProvisioning, this);

                financeProvisioning.Date = Date;
                financeProvisioning.Status = Status;
                financeProvisioning.ClaimsProvisionRecord = ClaimsProvisionRecord;
                financeProvisioning.ClaimsProvisionAmount = ClaimsProvisionAmount;
                financeProvisioning.DrProvisionRecord = DrProvisionRecord;
                financeProvisioning.DrProvisionAmount = DrProvisionAmount;
                financeProvisioning.ProvisionAt = ProvisionAt;
                financeProvisioning.UpdatedAt = DateTime.Now;
                financeProvisioning.UpdatedById = UpdatedById ?? financeProvisioning.UpdatedById;

                db.Entry(financeProvisioning).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                FinanceProvisioning financeProvisioning = db.FinanceProvisionings.Where(q => q.Id == id).FirstOrDefault();
                if (financeProvisioning == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(financeProvisioning, true);

                db.Entry(financeProvisioning).State = EntityState.Deleted;
                db.FinanceProvisionings.Remove(financeProvisioning);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
