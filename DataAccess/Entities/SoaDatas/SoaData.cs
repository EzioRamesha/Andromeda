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
    [Table("SoaData")]
    public class SoaData
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        [Index]
        public int? SoaDataFileId { get; set; }
        [ExcludeTrail]
        public virtual SoaDataFile SoaDataFile { get; set; }


        [Index]
        public int MappingStatus { get; set; }


        public string Errors { get; set; }


        [MaxLength(255)]
        public string CompanyName { get; set; }


        [MaxLength(12), Index]
        public string BusinessCode { get; set; }
        [MaxLength(30)]
        public string TreatyId { get; set; }
        [MaxLength(35), Index]
        public string TreatyCode { get; set; }
        [MaxLength(1)]
        public string TreatyMode { get; set; }
        [MaxLength(10)]
        public string TreatyType { get; set; }

        [MaxLength(35)]
        public string PlanBlock { get; set; }


        public int? RiskMonth { get; set; }
        [MaxLength(32), Index]
        public string SoaQuarter { get; set; }
        [MaxLength(32), Index]
        public string RiskQuarter { get; set; }


        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

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

        public double? TotalCommission { get; set; }

        public double? NetTotalAmount { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? SoaReceivedDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? BordereauxReceivedDate { get; set; }

        [MaxLength(60)]
        public string StatementStatus { get; set; }

        [MaxLength(60)]
        public string Remarks1 { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        [MaxLength(30)]
        public string SoaStatus { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ConfirmationDate { get; set; }


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


        public SoaData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData.Any(q => q.Id == id);
            }
        }

        public static SoaData Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Count();
            }
        }

        public static IList<SoaData> GetBySoaDataBatchId(int soaDataBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public static IList<SoaData> GetBySoaDataBatchIdSoaDataFileId(int soaDataBatchId, int soaDataFileId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData.Where(q => q.SoaDataBatchId == soaDataBatchId && q.SoaDataFileId == soaDataFileId).ToList();
            }
        }

        public static List<int> GetAllBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.SoaData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.SoaData.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                SoaData soaData = Find(Id);
                if (soaData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaData, this);

                soaData.SoaDataBatchId = SoaDataBatchId;
                soaData.SoaDataFileId = SoaDataFileId;

                soaData.MappingStatus = MappingStatus;

                soaData.Errors = Errors;
                soaData.CompanyName = CompanyName;
                soaData.BusinessCode = BusinessCode;
                soaData.TreatyId = TreatyId;
                soaData.TreatyCode = TreatyCode;
                soaData.TreatyMode = TreatyMode;
                soaData.TreatyType = TreatyType;
                soaData.PlanBlock = PlanBlock;
                soaData.RiskMonth = RiskMonth;
                soaData.SoaQuarter = SoaQuarter;
                soaData.RiskQuarter = RiskQuarter;
                soaData.NbPremium = NbPremium;
                soaData.RnPremium = RnPremium;
                soaData.AltPremium = AltPremium;
                soaData.GrossPremium = GrossPremium;
                soaData.TotalDiscount = TotalDiscount;
                soaData.RiskPremium = RiskPremium;
                soaData.NoClaimBonus = NoClaimBonus;
                soaData.Levy = Levy;
                soaData.Claim = Claim;
                soaData.ProfitComm = ProfitComm;
                soaData.SurrenderValue = SurrenderValue;
                soaData.Gst = Gst;
                soaData.ModcoReserveIncome = ModcoReserveIncome;
                soaData.RiDeposit = RiDeposit;
                soaData.AdministrationContribution = AdministrationContribution;
                soaData.DatabaseCommission = DatabaseCommission;
                soaData.ShareOfRiCommissionFromCompulsoryCession = ShareOfRiCommissionFromCompulsoryCession;
                soaData.RecaptureFee = RecaptureFee;
                soaData.CreditCardCharges = CreditCardCharges;
                soaData.BrokerageFee = BrokerageFee;
                soaData.NetTotalAmount = NetTotalAmount;
                soaData.SoaReceivedDate = SoaReceivedDate;
                soaData.BordereauxReceivedDate = BordereauxReceivedDate;
                soaData.StatementStatus = StatementStatus;
                soaData.Remarks1 = Remarks1;
                soaData.CurrencyCode = CurrencyCode;
                soaData.CurrencyRate = CurrencyRate;
                soaData.SoaStatus = SoaStatus;
                soaData.ConfirmationDate = ConfirmationDate;
                soaData.UpdatedAt = DateTime.Now;
                soaData.UpdatedById = UpdatedById ?? soaData.UpdatedById;

                db.Entry(soaData).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext(false))
            {
                SoaData soaData = db.SoaData.Where(q => q.Id == id).FirstOrDefault();
                if (soaData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(soaData, true);

                db.Entry(soaData).State = EntityState.Deleted;
                db.SoaData.Remove(soaData);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
