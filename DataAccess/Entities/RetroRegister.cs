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

namespace DataAccess.Entities
{
    [Table("RetroRegister")]
    public class RetroRegister
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int Type { get; set; }

        public int? RetroRegisterBatchId { get; set; }
        [ExcludeTrail]
        public virtual RetroRegisterBatch RetroRegisterBatch { get; set; }

        [Index]
        public int RetroStatementType { get; set; }

        [MaxLength(30), Index]
        public string RetroStatementNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RetroStatementDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReportCompletedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SendToRetroDate { get; set; }

        public int? RetroPartyId { get; set; }

        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        public int? CedantId { get; set; }

        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int? TreatyCodeId { get; set; }

        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [MaxLength(10)]
        public string RiskQuarter { get; set; }

        [MaxLength(128)]
        public string TreatyNumber { get; set; }

        [MaxLength(50)]
        public string Schedule { get; set; }

        [MaxLength(50)]
        public string TreatyType { get; set; }

        [MaxLength(50)]
        public string LOB { get; set; }

        [MaxLength(255)]
        public string AccountFor { get; set; }

        public double? Year1st { get; set; }

        public double? Renewal { get; set; }

        public double? ReserveCededBegin { get; set; }

        public double? ReserveCededEnd { get; set; }

        public double? RiskChargeCededBegin { get; set; }

        public double? RiskChargeCededEnd { get; set; }

        public double? AverageReserveCeded { get; set; }

        public double? Gross1st { get; set; }

        public double? GrossRen { get; set; }

        public double? AltPremium { get; set; }

        public double? Discount1st { get; set; }

        public double? DiscountRen { get; set; }

        public double? DiscountAlt { get; set; }

        public double? RiskPremium { get; set; }

        public double? Claims { get; set; }

        public double? ProfitCommission { get; set; }

        public double? SurrenderVal { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? RetrocessionMarketingFee { get; set; }

        public double? AgreedDBCommission { get; set; }

        public double? GstPayable { get; set; }

        public double? NetTotalAmount { get; set; }

        public int? NbCession { get; set; }

        public double? NbSumReins { get; set; }

        public int? RnCession { get; set; }

        public double? RnSumReins { get; set; }

        public int? AltCession { get; set; }

        public double? AltSumReins { get; set; }

        [MaxLength(3)]
        public string Frequency { get; set; }

        public int? PreparedById { get; set; }

        [ExcludeTrail]
        public virtual User PreparedBy { get; set; }

        [MaxLength(10)]
        public string OriginalSoaQuarter { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RetroConfirmationDate { get; set; }

        public double? ValuationGross1st { get; set; }

        public double? ValuationGrossRen { get; set; }

        public double? ValuationDiscount1st { get; set; }

        public double? ValuationDiscountRen { get; set; }

        public double? ValuationCom1st { get; set; }

        public double? ValuationComRen { get; set; }

        public string Remark { get; set; }

        [Index]
        public int? Status { get; set; }

        [MaxLength(35)]
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }

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

        [Index]
        public int? DirectRetroId { get; set; }
        [ExcludeTrail]
        public virtual DirectRetro DirectRetro { get; set; }


        public RetroRegister()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisters.Any(q => q.Id == id);
            }
        }

        public static RetroRegister Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisters.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RetroRegister> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisters.OrderBy(q => q.RetroStatementNo).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroRegisters.Add(this);
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

                entity.Type = Type;
                entity.RetroRegisterBatchId = RetroRegisterBatchId;
                entity.RetroStatementType = RetroStatementType;
                entity.RetroStatementNo = RetroStatementNo;
                entity.RetroStatementDate = RetroStatementDate;
                entity.ReportCompletedDate = ReportCompletedDate;
                entity.SendToRetroDate = SendToRetroDate;
                entity.RetroPartyId = RetroPartyId;
                entity.RiskQuarter = RiskQuarter;
                entity.CedantId = CedantId;
                entity.TreatyCodeId = TreatyCodeId;
                entity.TreatyNumber = TreatyNumber;
                entity.Schedule = Schedule;
                entity.TreatyType = TreatyType;
                entity.LOB = LOB;
                entity.AccountFor = AccountFor;
                entity.Year1st = Year1st;
                entity.Renewal = Renewal;
                entity.ReserveCededBegin = ReserveCededBegin;
                entity.ReserveCededEnd = ReserveCededEnd;
                entity.RiskChargeCededBegin = RiskChargeCededBegin;
                entity.RiskChargeCededEnd = RiskChargeCededEnd;
                entity.AverageReserveCeded = AverageReserveCeded;
                entity.Gross1st = Gross1st;
                entity.GrossRen = GrossRen;
                entity.AltPremium = AltPremium;
                entity.Discount1st = Discount1st;
                entity.DiscountRen = DiscountRen;
                entity.DiscountAlt = DiscountAlt;
                entity.RiskPremium = RiskPremium;
                entity.Claims = Claims;
                entity.ProfitCommission = ProfitCommission;
                entity.SurrenderVal = SurrenderVal;
                entity.NoClaimBonus = NoClaimBonus;
                entity.RetrocessionMarketingFee = RetrocessionMarketingFee;
                entity.AgreedDBCommission = AgreedDBCommission;
                entity.GstPayable = GstPayable;
                entity.NetTotalAmount = NetTotalAmount;
                entity.NbCession = NbCession;
                entity.NbSumReins = NbSumReins;
                entity.RnCession = RnCession;
                entity.RnSumReins = RnSumReins;
                entity.AltCession = AltCession;
                entity.AltSumReins = AltSumReins;
                entity.Frequency = Frequency;
                entity.PreparedById = PreparedById;
                entity.OriginalSoaQuarter = OriginalSoaQuarter;
                entity.RetroConfirmationDate = RetroConfirmationDate;
                entity.ValuationGross1st = ValuationGross1st;
                entity.ValuationGrossRen = ValuationGrossRen;
                entity.ValuationDiscount1st = ValuationDiscount1st;
                entity.ValuationDiscountRen = ValuationDiscountRen;
                entity.ValuationCom1st = ValuationCom1st;
                entity.ValuationComRen = ValuationComRen;
                entity.Remark = Remark;
                entity.Status = Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;
                entity.DirectRetroId = DirectRetroId;
                entity.ContractCode = ContractCode;
                entity.AnnualCohort = AnnualCohort;
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
                db.RetroRegisters.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
