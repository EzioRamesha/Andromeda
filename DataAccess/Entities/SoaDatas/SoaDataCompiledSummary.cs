using BusinessObject;
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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataCompiledSummaries")]
    public class SoaDataCompiledSummary
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }


        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }


        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? StatementReceivedDate { get; set; }


        [MaxLength(12), Index]
        public string BusinessCode { get; set; }
        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }
        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }


        [MaxLength(128)]
        public string AccountDescription { get; set; }


        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }


        public double? TotalDiscount { get; set; }
        public double? RiskPremium { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? Levy { get; set; }
        public double? Claim { get; set; }
        public double? ProfitComm { get; set; }
        public double? SurrenderValue { get; set; }
        public double? Gst { get; set; }
        public double? ModcoReserveIncome { get; set; }
        public double? RiDeposit { get; set; }
        public double? DatabaseCommission { get; set; }
        public double? AdministrationContribution { get; set; }
        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }
        public double? RecaptureFee { get; set; }
        public double? CreditCardCharges { get; set; }
        public double? BrokerageFee { get; set; }
        public double? NetTotalAmount { get; set; }


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
        [MaxLength(128)]
        public string FilingReference { get; set; }
        public double? ServiceFeePercentage { get; set; }
        public double? ServiceFee { get; set; }
        public double? Sst { get; set; }
        public double? TotalAmount { get; set; }


        public double? NbDiscount { get; set; }
        public double? RnDiscount { get; set; }
        public double? AltDiscount { get; set; }


        public int? NbCession { get; set; }
        public int? RnCession { get; set; }
        public int? AltCession { get; set; }


        public double? NbSar { get; set; }
        public double? RnSar { get; set; }
        public double? AltSar { get; set; }


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
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        [MaxLength(35)]
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        [MaxLength(3)]
        public string Frequency { get; set; }
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
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }


        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }


        public SoaDataCompiledSummary()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataCompiledSummaries.Any(q => q.Id == id);
            }
        }

        public static SoaDataCompiledSummary Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataCompiledSummaries.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataCompiledSummaries.Add(this);
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
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.SoaDataBatchId = SoaDataBatchId;
                entity.InvoiceType = InvoiceType;
                entity.InvoiceDate = InvoiceDate;
                entity.StatementReceivedDate = StatementReceivedDate;

                entity.BusinessCode = BusinessCode;
                entity.TreatyCode = TreatyCode;
                entity.SoaQuarter = SoaQuarter;
                entity.RiskQuarter = RiskQuarter;
                entity.AccountDescription = AccountDescription;

                entity.NbPremium = NbPremium;
                entity.RnPremium = RnPremium;
                entity.AltPremium = AltPremium;

                entity.TotalDiscount = TotalDiscount;
                entity.RiskPremium = RiskPremium;
                entity.NoClaimBonus = NoClaimBonus;
                entity.Levy = Levy;
                entity.Claim = Claim;
                entity.ProfitComm = ProfitComm;
                entity.SurrenderValue = SurrenderValue;
                entity.Gst = Gst;
                entity.ModcoReserveIncome = ModcoReserveIncome;
                entity.RiDeposit = RiDeposit;
                entity.DatabaseCommission = DatabaseCommission;
                entity.AdministrationContribution = AdministrationContribution;
                entity.ShareOfRiCommissionFromCompulsoryCession = ShareOfRiCommissionFromCompulsoryCession;
                entity.RecaptureFee = RecaptureFee;
                entity.CreditCardCharges = CreditCardCharges;
                entity.BrokerageFee = BrokerageFee;
                entity.NetTotalAmount = NetTotalAmount;

                entity.ReasonOfAdjustment1 = ReasonOfAdjustment1;
                entity.ReasonOfAdjustment2 = ReasonOfAdjustment2;
                entity.InvoiceNumber1 = InvoiceNumber1;
                entity.InvoiceDate1 = InvoiceDate1;
                entity.Amount1 = Amount1;
                entity.InvoiceNumber2 = InvoiceNumber2;
                entity.InvoiceDate2 = InvoiceDate2;
                entity.Amount2 = Amount2;
                entity.FilingReference = FilingReference;
                entity.ServiceFeePercentage = ServiceFeePercentage;
                entity.ServiceFee = ServiceFee;
                entity.Sst = Sst;
                entity.TotalAmount = TotalAmount;

                entity.NbDiscount = NbDiscount;
                entity.RnDiscount = RnDiscount;
                entity.AltDiscount = AltDiscount;

                entity.NbCession = NbCession;
                entity.RnCession = RnCession;
                entity.AltCession = AltCession;

                entity.NbSar = NbSar;
                entity.RnSar = RnSar;
                entity.AltSar = AltSar;

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

                entity.CurrencyCode = CurrencyCode;
                entity.CurrencyRate = CurrencyRate;

                entity.ContractCode = ContractCode;
                entity.AnnualCohort = AnnualCohort;
                entity.Frequency = Frequency;
                entity.Mfrs17CellName = Mfrs17CellName;

                entity.ReportingType = ReportingType;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SoaDataCompiledSummary soaDataCompiledSummarys = db.SoaDataCompiledSummaries.Where(q => q.Id == id).FirstOrDefault();
                if (soaDataCompiledSummarys == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataCompiledSummarys, true);

                db.Entry(soaDataCompiledSummarys).State = EntityState.Deleted;
                db.SoaDataCompiledSummaries.Remove(soaDataCompiledSummarys);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
