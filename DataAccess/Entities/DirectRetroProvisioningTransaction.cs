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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("DirectRetroProvisioningTransactions")]
    public class DirectRetroProvisioningTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimRegisterId { get; set; }
        [ForeignKey(nameof(ClaimRegisterId))]
        [ExcludeTrail]
        public virtual ClaimRegister ClaimRegister { get; set; }

        [Index]
        public int? FinanceProvisioningId { get; set; }
        [ExcludeTrail]
        public virtual FinanceProvisioning FinanceProvisioning { get; set; }

        [Index]
        public bool IsLatestProvision { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimId { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingCompany { get; set; }

        [MaxLength(30)]
        [Index]
        public string EntryNo { get; set; }

        [MaxLength(30)]
        [Index]
        public string Quarter { get; set; }

        [MaxLength(128)]
        [Index]
        public string RetroParty { get; set; }

        [Index]
        public double RetroRecovery { get; set; }

        [MaxLength(30)]
        [Index]
        public string RetroStatementId { get; set; }

        public DateTime? RetroStatementDate { get; set; }

        // Added For Account Code Mapping
        [MaxLength(35)]
        public string TreatyCode { get; set; }

        [MaxLength(35)]
        public string TreatyType { get; set; }

        [MaxLength(30)]
        public string ClaimCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public DirectRetroProvisioningTransaction()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static DirectRetroProvisioningTransaction Find(AppDbContext db, int id)
        {
            return db.DirectRetroProvisioningTransactions.Where(q => q.Id == id).FirstOrDefault();
        }

        public static DirectRetroProvisioningTransaction Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return Find(db, id);
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.DirectRetroProvisioningTransactions.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                DirectRetroProvisioningTransaction entity = Find(db, Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.ClaimRegisterId = ClaimRegisterId;
                entity.IsLatestProvision = IsLatestProvision;
                entity.ClaimId = ClaimId;
                entity.CedingCompany = CedingCompany;
                entity.EntryNo = EntryNo;
                entity.Quarter = Quarter;
                entity.RetroParty = RetroParty;
                entity.RetroRecovery = RetroRecovery;
                entity.RetroStatementId = RetroStatementId;
                entity.RetroStatementDate = RetroStatementDate;

                entity.TreatyCode = TreatyCode;
                entity.TreatyType = TreatyType;
                entity.ClaimCode = ClaimCode;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }
    }
}
