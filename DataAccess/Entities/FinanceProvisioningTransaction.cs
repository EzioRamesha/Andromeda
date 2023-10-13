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
    [Table("FinanceProvisioningTransactions")]
    public class FinanceProvisioningTransaction
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimRegisterId { get; set; }
        [ExcludeTrail]
        public virtual ClaimRegister ClaimRegister { get; set; }

        [Index]
        public int? FinanceProvisioningId { get; set; }
        [ExcludeTrail]
        public virtual FinanceProvisioning FinanceProvisioning { get; set; }

        public int SortIndex { get; set; }

        public bool IsLatestProvision { get; set; }

        public string ClaimId { get; set; }

        public string PolicyNumber { get; set; }

        public string CedingCompany { get; set; }

        public string EntryNo { get; set; }

        public string Quarter { get; set; }

        public double SumReinsured { get; set; }

        public double ClaimRecoveryAmount { get; set; }

        // Added For Account Code Mapping
        [MaxLength(35)]
        public string TreatyCode { get; set; }

        [MaxLength(35)]
        public string TreatyType { get; set; }

        [MaxLength(30)]
        public string ClaimCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastTransactionDate { get; set; }

        [MaxLength(30)]
        public string LastTransactionQuarter { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateOfEvent { get; set; }

        [MaxLength(30)]
        public string RiskQuarter { get; set; }

        public int? RiskPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        [MaxLength(30)]
        public string MlreBenefitCode { get; set; }

        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        [MaxLength(128)]
        public string RetroParty1 { get; set; }

        [MaxLength(128)]
        public string RetroParty2 { get; set; }

        [MaxLength(128)]
        public string RetroParty3 { get; set; }

        public double? RetroRecovery1 { get; set; }

        public double? RetroRecovery2 { get; set; }

        public double? RetroRecovery3 { get; set; }

        public string ReinsBasisCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePol { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        [MaxLength(30)]
        public string Mfrs17ContractCode { get; set; }

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

        public FinanceProvisioningTransaction()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FinanceProvisioningTransactions.Any(q => q.Id == id);
            }
        }

        public static FinanceProvisioningTransaction Find(AppDbContext db, int id)
        {
            return db.FinanceProvisioningTransactions.Where(q => q.Id == id).FirstOrDefault();
        }

        public static FinanceProvisioningTransaction Find(int id)
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
                db.FinanceProvisioningTransactions.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(db, Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.ClaimRegisterId = ClaimRegisterId;
                entity.FinanceProvisioningId = FinanceProvisioningId;
                entity.SortIndex = SortIndex;
                entity.IsLatestProvision = IsLatestProvision;
                entity.ClaimId = ClaimId;
                entity.PolicyNumber = PolicyNumber;
                entity.CedingCompany = CedingCompany;
                entity.EntryNo = EntryNo;
                entity.Quarter = Quarter;
                entity.SumReinsured = SumReinsured;
                entity.ClaimRecoveryAmount = ClaimRecoveryAmount;

                entity.TreatyCode = TreatyCode;
                entity.TreatyType = TreatyType;
                entity.ClaimCode = ClaimCode;
                entity.LastTransactionDate = LastTransactionDate;
                entity.LastTransactionQuarter = LastTransactionQuarter;
                entity.DateOfEvent = DateOfEvent;
                entity.RiskQuarter = RiskQuarter;
                entity.RiskPeriodYear = RiskPeriodYear;
                entity.RiskPeriodMonth = RiskPeriodMonth;
                entity.FundsAccountingTypeCode = FundsAccountingTypeCode;
                entity.MlreBenefitCode = MlreBenefitCode;
                entity.RetroParty1 = RetroParty1;
                entity.RetroParty2 = RetroParty2;
                entity.RetroParty3 = RetroParty3;
                entity.RetroRecovery1 = RetroRecovery1;
                entity.RetroRecovery2 = RetroRecovery2;
                entity.RetroRecovery3 = RetroRecovery3;
                entity.ReinsEffDatePol = ReinsEffDatePol;
                entity.ReinsBasisCode = ReinsBasisCode;
                entity.Mfrs17ContractCode = Mfrs17ContractCode;
                entity.Mfrs17AnnualCohort = Mfrs17AnnualCohort;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }
    }
}
