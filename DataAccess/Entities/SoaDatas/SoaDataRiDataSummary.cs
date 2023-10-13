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

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataRiDataSummaries")]
    public class SoaDataRiDataSummary
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [MaxLength(12), Index]
        public string BusinessCode { get; set; }
        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }
        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }
        [Index]
        public int? RiskMonth { get; set; }


        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
        public double? GrossPremium { get; set; }


        public double? TotalDiscount { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? Claim { get; set; }
        public double? SurrenderValue { get; set; }
        public double? DatabaseCommission { get; set; }
        public double? BrokerageFee { get; set; }
        public double? ServiceFee { get; set; }
        public double? Gst { get; set; }
        public double? NetTotalAmount { get; set; }


        [MaxLength(3), Index]
        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }


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

        [MaxLength(35)]
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [MaxLength(3)]
        public string Frequency { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }


        public SoaDataRiDataSummary()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataRiDataSummaries.Any(q => q.Id == id);
            }
        }

        public static SoaDataRiDataSummary Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataRiDataSummaries.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.SoaDataRiDataSummaries.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                SoaDataRiDataSummary soaDataRiDataSummarys = Find(Id);
                if (soaDataRiDataSummarys == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataRiDataSummarys, this);

                soaDataRiDataSummarys.SoaDataBatchId = SoaDataBatchId;
                soaDataRiDataSummarys.Type = Type;
                soaDataRiDataSummarys.BusinessCode = BusinessCode;
                soaDataRiDataSummarys.TreatyCode = TreatyCode;
                soaDataRiDataSummarys.SoaQuarter = SoaQuarter;
                soaDataRiDataSummarys.RiskQuarter = RiskQuarter;
                soaDataRiDataSummarys.RiskMonth = RiskMonth;
                soaDataRiDataSummarys.NbPremium = NbPremium;
                soaDataRiDataSummarys.RnPremium = RnPremium;
                soaDataRiDataSummarys.AltPremium = AltPremium;
                soaDataRiDataSummarys.GrossPremium = GrossPremium;
                soaDataRiDataSummarys.TotalDiscount = TotalDiscount;
                soaDataRiDataSummarys.NoClaimBonus = NoClaimBonus;
                soaDataRiDataSummarys.Claim = Claim;
                soaDataRiDataSummarys.SurrenderValue = SurrenderValue;
                soaDataRiDataSummarys.Gst = Gst;
                soaDataRiDataSummarys.DatabaseCommission = DatabaseCommission;
                soaDataRiDataSummarys.BrokerageFee = BrokerageFee;
                soaDataRiDataSummarys.ServiceFee = ServiceFee;
                soaDataRiDataSummarys.NetTotalAmount = NetTotalAmount;
                soaDataRiDataSummarys.CurrencyCode = CurrencyCode;
                soaDataRiDataSummarys.CurrencyRate = CurrencyRate;
                soaDataRiDataSummarys.NbDiscount = NbDiscount;
                soaDataRiDataSummarys.RnDiscount = RnDiscount;
                soaDataRiDataSummarys.AltDiscount = AltDiscount;
                soaDataRiDataSummarys.NbCession = NbCession;
                soaDataRiDataSummarys.RnCession = RnCession;
                soaDataRiDataSummarys.AltCession = AltCession;
                soaDataRiDataSummarys.NbSar = NbSar;
                soaDataRiDataSummarys.RnSar = RnSar;
                soaDataRiDataSummarys.AltSar = AltSar;
                soaDataRiDataSummarys.DTH = DTH;
                soaDataRiDataSummarys.TPA = TPA;
                soaDataRiDataSummarys.TPS = TPS;
                soaDataRiDataSummarys.PPD = PPD;
                soaDataRiDataSummarys.CCA = CCA;
                soaDataRiDataSummarys.CCS = CCS;
                soaDataRiDataSummarys.PA = PA;
                soaDataRiDataSummarys.HS = HS;
                soaDataRiDataSummarys.TPD = TPD;
                soaDataRiDataSummarys.CI = CI;
                soaDataRiDataSummarys.ContractCode = ContractCode;
                soaDataRiDataSummarys.AnnualCohort = AnnualCohort;
                soaDataRiDataSummarys.Mfrs17CellName = Mfrs17CellName;
                soaDataRiDataSummarys.Frequency = Frequency;
                soaDataRiDataSummarys.UpdatedAt = DateTime.Now;
                soaDataRiDataSummarys.UpdatedById = UpdatedById ?? soaDataRiDataSummarys.UpdatedById;

                db.Entry(soaDataRiDataSummarys).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                SoaDataRiDataSummary soaDataRiDataSummarys = db.SoaDataRiDataSummaries.Where(q => q.Id == id).FirstOrDefault();
                if (soaDataRiDataSummarys == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaDataRiDataSummarys, true);

                db.Entry(soaDataRiDataSummarys).State = EntityState.Deleted;
                db.SoaDataRiDataSummaries.Remove(soaDataRiDataSummarys);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
