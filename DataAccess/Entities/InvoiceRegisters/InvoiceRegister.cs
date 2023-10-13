using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
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
    [Table("InvoiceRegister")]
    public class InvoiceRegister
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int InvoiceRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatch InvoiceRegisterBatch { get; set; }

        [Index]
        public int? SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Required, Index]
        public int InvoiceType { get; set; }
        [Index, MaxLength(30)]
        public string InvoiceReference { get; set; }
        [Index, MaxLength(30)]
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? StatementReceivedDate { get; set; }

        [Index]
        public int? CedantId { get; set; }

        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }

        [Index]
        public int? TreatyCodeId { get; set; } // Get treatyId, TreatyCode, treatyType, LOB

        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [MaxLength(128)]
        public string AccountDescription { get; set; }

        public double? TotalPaid { get; set; }

        [MaxLength(50)]
        public string PaymentReference { get; set; }
        [MaxLength(255)]
        public string PaymentAmount { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? PaymentReceivedDate { get; set; }

        public double? Year1st { get; set; }
        public double? Renewal { get; set; }
        public double? Gross1st { get; set; }
        public double? GrossRenewal { get; set; }
        public double? AltPremium { get; set; }
        public double? Discount1st { get; set; }
        public double? DiscountRen { get; set; }
        public double? DiscountAlt { get; set; }

        public double? RiskPremium { get; set; }
        public double? Claim { get; set; }
        public double? ProfitComm { get; set; }
        public double? Levy { get; set; }
        public double? SurrenderValue { get; set; }
        public double? Gst { get; set; }
        public double? ModcoReserveIncome { get; set; }
        public double? ReinsDeposit { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? DatabaseCommission { get; set; }
        public double? AdministrationContribution { get; set; }
        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }
        public double? RecaptureFee { get; set; }
        public double? CreditCardCharges { get; set; }
        public double? BrokerageFee { get; set; }

        public int? NBCession { get; set; }
        public double? NBSumReins { get; set; }
        public int? RNCession { get; set; }
        public double? RNSumReins { get; set; }
        public int? AltCession { get; set; }
        public double? AltSumReins { get; set; }              

        public double? DTH { get; set; }
        public double? TPA { get; set; }
        public double? TPS { get; set; }
        public double? PPD { get; set; }
        public double? CCA { get; set; }
        public double? CCS { get; set; }
        public double? PA { get; set; }
        public double? HS { get; set; }
        public double? TPD { get; set; }
        public double? CI { get; set; }

        [MaxLength(3)]
        public string Frequency { get; set; }
        public int? PreparedById { get; set; }

        [ExcludeTrail]
        public virtual User PreparedBy { get; set; }

        public string PlanName { get; set; }

        public double? ValuationGross1st { get; set; }
        public double? ValuationGrossRen { get; set; }
        public double? ValuationDiscount1st { get; set; }
        public double? ValuationDiscountRen { get; set; }
        public double? ValuationCom1st { get; set; }
        public double? ValuationComRen { get; set; }
        public double? ValuationClaims { get; set; }
        public double? ValuationProfitCom { get; set; }
        [MaxLength(3)]
        public string ValuationMode { get; set; }
        public double? ValuationRiskPremium { get; set; }

        public string Remark { get; set; }
        [MaxLength(32)]
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }

        [MaxLength(128)]
        public string ReasonOfAdjustment1 { get; set; }
        [MaxLength(128)]
        public string ReasonOfAdjustment2 { get; set; }
        [MaxLength(64), Index]
        public string InvoiceNumber1 { get; set; }
        public DateTime? InvoiceDate1 { get; set; }
        public double? Amount1 { get; set; }
        [MaxLength(64), Index]
        public string InvoiceNumber2 { get; set; }
        public DateTime? InvoiceDate2 { get; set; }
        public double? Amount2 { get; set; }

        [MaxLength(35)]
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }

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

        public InvoiceRegister()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.Any(q => q.Id == id);
            }
        }

        public static InvoiceRegister Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<InvoiceRegister> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.OrderBy(q => q.InvoiceReference).ToList();
            }
        }

        public static IList<InvoiceRegister> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisters.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.InvoiceRegisters.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.InvoiceRegisterBatchId = InvoiceRegisterBatchId;
                entity.InvoiceType = InvoiceType;
                entity.InvoiceReference = InvoiceReference;
                entity.InvoiceNumber = InvoiceNumber;
                entity.InvoiceDate = InvoiceDate;
                entity.StatementReceivedDate = StatementReceivedDate;
                entity.CedantId = CedantId;
                entity.RiskQuarter = RiskQuarter;
                entity.TreatyCodeId = TreatyCodeId;
                entity.AccountDescription = AccountDescription;
                entity.TotalPaid = TotalPaid;
                entity.PaymentReference = PaymentReference;
                entity.PaymentAmount = PaymentAmount;
                entity.PaymentReceivedDate = PaymentReceivedDate;

                entity.Year1st = Year1st;
                entity.Renewal = Renewal;
                entity.Gross1st = Gross1st;
                entity.GrossRenewal = GrossRenewal;
                entity.AltPremium = AltPremium;
                entity.Discount1st = Discount1st;
                entity.DiscountRen = DiscountRen;
                entity.DiscountAlt = DiscountAlt;

                entity.RiskPremium = RiskPremium;
                entity.NoClaimBonus = NoClaimBonus;
                entity.Levy = Levy;
                entity.Claim = Claim;
                entity.ProfitComm = ProfitComm;
                entity.SurrenderValue = SurrenderValue;
                entity.Gst = Gst;
                entity.ModcoReserveIncome = ModcoReserveIncome;
                entity.ReinsDeposit = ReinsDeposit;
                entity.DatabaseCommission = DatabaseCommission;
                entity.AdministrationContribution = AdministrationContribution;
                entity.ShareOfRiCommissionFromCompulsoryCession = ShareOfRiCommissionFromCompulsoryCession;
                entity.RecaptureFee = RecaptureFee;
                entity.CreditCardCharges = CreditCardCharges;
                entity.BrokerageFee = BrokerageFee;

                entity.NBCession = NBCession;
                entity.NBSumReins = NBSumReins;
                entity.RNCession = RNCession;
                entity.RNSumReins = RNSumReins;
                entity.AltCession = AltCession;
                entity.AltSumReins = AltSumReins;

                entity.DTH = DTH;
                entity.TPA = TPA;
                entity.TPS = TPS;
                entity.PPD = PPD;
                entity.CCA = CCA;
                entity.CCS = CCS;
                entity.PA = PA;
                entity.HS = HS;
                entity.TPD = TPD;
                entity.CI = CI;

                entity.Frequency = Frequency;
                entity.PreparedById = PreparedById;

                entity.ValuationGross1st = ValuationGross1st;
                entity.ValuationGrossRen = ValuationGrossRen;
                entity.ValuationDiscount1st = ValuationDiscount1st;
                entity.ValuationDiscountRen = ValuationDiscountRen;
                entity.ValuationCom1st = ValuationCom1st;
                entity.ValuationComRen = ValuationComRen;
                entity.ValuationClaims = ValuationClaims;
                entity.ValuationProfitCom = ValuationProfitCom;
                entity.ValuationMode = ValuationMode;
                entity.ValuationRiskPremium = ValuationRiskPremium;

                entity.Remark = Remark;
                entity.CurrencyCode = CurrencyCode;
                entity.CurrencyRate = CurrencyRate;
                entity.SoaQuarter = SoaQuarter;

                entity.ReasonOfAdjustment1 = ReasonOfAdjustment1;
                entity.ReasonOfAdjustment2 = ReasonOfAdjustment2;
                entity.InvoiceNumber1 = InvoiceNumber1;
                entity.InvoiceDate1 = InvoiceDate1;
                entity.Amount1 = Amount1;
                entity.InvoiceNumber2 = InvoiceNumber2;
                entity.InvoiceDate2 = InvoiceDate2;
                entity.Amount2 = Amount2;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;
                entity.SoaDataBatchId = SoaDataBatchId;

                entity.ContractCode = ContractCode;
                entity.AnnualCohort = AnnualCohort;
                entity.Mfrs17CellName = Mfrs17CellName;
                entity.ReportingType = ReportingType;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.InvoiceRegisters.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
