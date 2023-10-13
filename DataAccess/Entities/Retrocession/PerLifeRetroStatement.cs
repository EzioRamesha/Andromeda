using DataAccess.Entities.Identity;
using DataAccess.Entities.Retrocession;
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
    [Table("PerLifeRetroStatements")]
    public class PerLifeRetroStatement
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeSoaId { get; set; }

        [ExcludeTrail]
        public virtual PerLifeSoa PerLifeSoa { get; set; }

        [Required, Index]
        public int RetroPartyId { get; set; }

        [ExcludeTrail]
        public virtual RetroParty RetroParty { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(128)]
        public string MlreRef { get; set; }

        [MaxLength(255)]
        public string CedingCompany { get; set; }

        [MaxLength(35)]
        public string TreatyCode { get; set; }

        [MaxLength(128)]
        public string TreatyNo { get; set; }

        [MaxLength(20)]
        public string Schedule { get; set; }

        [MaxLength(128)]
        public string TreatyType { get; set; }

        [MaxLength(128)]
        public string FromMlreTo { get; set; }

        [Column(TypeName = "ntext")]
        public string AccountsFor { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateReportCompleted { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateSendToRetro { get; set; }

        // Data Set 1
        [MaxLength(10)]
        public string AccountingPeriod { get; set; }

        public double? ReserveCededBegin { get; set; }

        public double? ReserveCededEnd { get; set; }

        public double? RiskChargeCededBegin { get; set; }

        public double? RiskChargeCededEnd { get; set; }

        public double? AverageReserveCeded { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB { get; set; }

        public double? RiPremiumRN { get; set; }

        public double? RiPremiumALT { get; set; }

        public double? QuarterlyRiskPremium { get; set; }

        public double? RetrocessionMarketingFee { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB { get; set; }

        public double? RiDiscountRN { get; set; }

        public double? RiDiscountALT { get; set; }

        public double? AgreedDatabaseComm { get; set; }

        public double? GstPayable { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Claims { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? PaymentToTheReinsurer { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB { get; set; }

        public int? TotalNoOfPolicyRN { get; set; }

        public int? TotalNoOfPolicyALT { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB { get; set; }

        public double? TotalSumReinsuredRN { get; set; }

        public double? TotalSumReinsuredALT { get; set; }

        // Date Set 2
        [MaxLength(10)]
        public string AccountingPeriod2 { get; set; }

        public double? ReserveCededBegin2 { get; set; }

        public double? ReserveCededEnd2 { get; set; }

        public double? RiskChargeCededBegin2 { get; set; }

        public double? RiskChargeCededEnd2 { get; set; }

        public double? AverageReserveCeded2 { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB2 { get; set; }

        public double? RiPremiumRN2 { get; set; }

        public double? RiPremiumALT2 { get; set; }

        public double? QuarterlyRiskPremium2 { get; set; }

        public double? RetrocessionMarketingFee2 { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB2 { get; set; }

        public double? RiDiscountRN2 { get; set; }

        public double? RiDiscountALT2 { get; set; }

        public double? AgreedDatabaseComm2 { get; set; }

        public double? GstPayable2 { get; set; }

        public double? NoClaimBonus2 { get; set; }

        public double? Claims2 { get; set; }

        public double? ProfitComm2 { get; set; }

        public double? SurrenderValue2 { get; set; }

        public double? PaymentToTheReinsurer2 { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB2 { get; set; }

        public int? TotalNoOfPolicyRN2 { get; set; }

        public int? TotalNoOfPolicyALT2 { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB2 { get; set; }

        public double? TotalSumReinsuredRN2 { get; set; }

        public double? TotalSumReinsuredALT2 { get; set; }

        // Date Set 3
        [MaxLength(10)]
        public string AccountingPeriod3 { get; set; }

        public double? ReserveCededBegin3 { get; set; }

        public double? ReserveCededEnd3 { get; set; }

        public double? RiskChargeCededBegin3 { get; set; }

        public double? RiskChargeCededEnd3 { get; set; }

        public double? AverageReserveCeded3 { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB3 { get; set; }

        public double? RiPremiumRN3 { get; set; }

        public double? RiPremiumALT3 { get; set; }

        public double? QuarterlyRiskPremium3 { get; set; }

        public double? RetrocessionMarketingFee3 { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB3 { get; set; }

        public double? RiDiscountRN3 { get; set; }

        public double? RiDiscountALT3 { get; set; }

        public double? AgreedDatabaseComm3 { get; set; }

        public double? GstPayable3 { get; set; }

        public double? NoClaimBonus3 { get; set; }

        public double? Claims3 { get; set; }

        public double? ProfitComm3 { get; set; }

        public double? SurrenderValue3 { get; set; }

        public double? PaymentToTheReinsurer3 { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB3 { get; set; }

        public int? TotalNoOfPolicyRN3 { get; set; }

        public int? TotalNoOfPolicyALT3 { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB3 { get; set; }

        public double? TotalSumReinsuredRN3 { get; set; }

        public double? TotalSumReinsuredALT3 { get; set; }

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

        public PerLifeRetroStatement()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroStatements.Any(q => q.Id == id);
            }
        }

        public static PerLifeRetroStatement Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroStatements.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeRetroStatements.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroStatement perLifeRetroStatement = Find(Id);
                if (perLifeRetroStatement == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroStatement, this);

                perLifeRetroStatement.PerLifeSoaId = PerLifeSoaId;
                perLifeRetroStatement.RetroPartyId = RetroPartyId;
                perLifeRetroStatement.Status = Status;
                perLifeRetroStatement.MlreRef = MlreRef;
                perLifeRetroStatement.CedingCompany = CedingCompany;
                perLifeRetroStatement.TreatyCode = TreatyCode;
                perLifeRetroStatement.TreatyNo = TreatyNo;
                perLifeRetroStatement.Schedule = Schedule;
                perLifeRetroStatement.TreatyType = TreatyType;
                perLifeRetroStatement.FromMlreTo = FromMlreTo;
                perLifeRetroStatement.AccountsFor = AccountsFor;
                perLifeRetroStatement.DateReportCompleted = DateReportCompleted;
                perLifeRetroStatement.DateSendToRetro = DateSendToRetro;

                // Data Set 1
                perLifeRetroStatement.AccountingPeriod = AccountingPeriod;
                perLifeRetroStatement.ReserveCededBegin = ReserveCededBegin;
                perLifeRetroStatement.ReserveCededEnd = ReserveCededEnd;
                perLifeRetroStatement.RiskChargeCededBegin = RiskChargeCededBegin;
                perLifeRetroStatement.RiskChargeCededEnd = RiskChargeCededEnd;
                perLifeRetroStatement.AverageReserveCeded = AverageReserveCeded;
                perLifeRetroStatement.RiPremiumNB = RiPremiumNB;
                perLifeRetroStatement.RiPremiumRN = RiPremiumRN;
                perLifeRetroStatement.RiPremiumALT = RiPremiumALT;
                perLifeRetroStatement.QuarterlyRiskPremium = QuarterlyRiskPremium;
                perLifeRetroStatement.RetrocessionMarketingFee = RetrocessionMarketingFee;
                perLifeRetroStatement.RiDiscountNB = RiDiscountNB;
                perLifeRetroStatement.RiDiscountRN = RiDiscountRN;
                perLifeRetroStatement.RiDiscountALT = RiDiscountALT;
                perLifeRetroStatement.AgreedDatabaseComm = AgreedDatabaseComm;
                perLifeRetroStatement.GstPayable = GstPayable;
                perLifeRetroStatement.NoClaimBonus = NoClaimBonus;
                perLifeRetroStatement.Claims = Claims;
                perLifeRetroStatement.ProfitComm = ProfitComm;
                perLifeRetroStatement.SurrenderValue = SurrenderValue;
                perLifeRetroStatement.PaymentToTheReinsurer = PaymentToTheReinsurer;
                perLifeRetroStatement.TotalNoOfPolicyNB = TotalNoOfPolicyNB;
                perLifeRetroStatement.TotalNoOfPolicyRN = TotalNoOfPolicyRN;
                perLifeRetroStatement.TotalNoOfPolicyALT = TotalNoOfPolicyALT;
                perLifeRetroStatement.TotalSumReinsuredNB = TotalSumReinsuredNB;
                perLifeRetroStatement.TotalSumReinsuredRN = TotalSumReinsuredRN;
                perLifeRetroStatement.TotalSumReinsuredALT = TotalSumReinsuredALT;

                // Data Set 2
                perLifeRetroStatement.AccountingPeriod2 = AccountingPeriod2;
                perLifeRetroStatement.ReserveCededBegin2 = ReserveCededBegin2;
                perLifeRetroStatement.ReserveCededEnd2 = ReserveCededEnd2;
                perLifeRetroStatement.RiskChargeCededBegin2 = RiskChargeCededBegin2;
                perLifeRetroStatement.RiskChargeCededEnd2 = RiskChargeCededEnd2;
                perLifeRetroStatement.AverageReserveCeded2 = AverageReserveCeded2;
                perLifeRetroStatement.RiPremiumNB2 = RiPremiumNB2;
                perLifeRetroStatement.RiPremiumRN2 = RiPremiumRN2;
                perLifeRetroStatement.RiPremiumALT2 = RiPremiumALT2;
                perLifeRetroStatement.QuarterlyRiskPremium2 = QuarterlyRiskPremium2;
                perLifeRetroStatement.RetrocessionMarketingFee2 = RetrocessionMarketingFee2;
                perLifeRetroStatement.RiDiscountNB2 = RiDiscountNB2;
                perLifeRetroStatement.RiDiscountRN2 = RiDiscountRN2;
                perLifeRetroStatement.RiDiscountALT2 = RiDiscountALT2;
                perLifeRetroStatement.AgreedDatabaseComm2 = AgreedDatabaseComm2;
                perLifeRetroStatement.GstPayable2 = GstPayable2;
                perLifeRetroStatement.NoClaimBonus2 = NoClaimBonus2;
                perLifeRetroStatement.Claims2 = Claims2;
                perLifeRetroStatement.ProfitComm2 = ProfitComm2;
                perLifeRetroStatement.SurrenderValue2 = SurrenderValue2;
                perLifeRetroStatement.PaymentToTheReinsurer2 = PaymentToTheReinsurer2;
                perLifeRetroStatement.TotalNoOfPolicyNB2 = TotalNoOfPolicyNB2;
                perLifeRetroStatement.TotalNoOfPolicyRN2 = TotalNoOfPolicyRN2;
                perLifeRetroStatement.TotalNoOfPolicyALT2 = TotalNoOfPolicyALT2;
                perLifeRetroStatement.TotalSumReinsuredNB2 = TotalSumReinsuredNB2;
                perLifeRetroStatement.TotalSumReinsuredRN2 = TotalSumReinsuredRN2;
                perLifeRetroStatement.TotalSumReinsuredALT2 = TotalSumReinsuredALT2;

                // Data Set 3
                perLifeRetroStatement.AccountingPeriod3 = AccountingPeriod3;
                perLifeRetroStatement.ReserveCededBegin3 = ReserveCededBegin3;
                perLifeRetroStatement.ReserveCededEnd3 = ReserveCededEnd3;
                perLifeRetroStatement.RiskChargeCededBegin3 = RiskChargeCededBegin3;
                perLifeRetroStatement.RiskChargeCededEnd3 = RiskChargeCededEnd3;
                perLifeRetroStatement.AverageReserveCeded3 = AverageReserveCeded3;
                perLifeRetroStatement.RiPremiumNB3 = RiPremiumNB3;
                perLifeRetroStatement.RiPremiumRN3 = RiPremiumRN3;
                perLifeRetroStatement.RiPremiumALT3 = RiPremiumALT3;
                perLifeRetroStatement.QuarterlyRiskPremium3 = QuarterlyRiskPremium3;
                perLifeRetroStatement.RetrocessionMarketingFee3 = RetrocessionMarketingFee3;
                perLifeRetroStatement.RiDiscountNB3 = RiDiscountNB3;
                perLifeRetroStatement.RiDiscountRN3 = RiDiscountRN3;
                perLifeRetroStatement.RiDiscountALT3 = RiDiscountALT3;
                perLifeRetroStatement.AgreedDatabaseComm3 = AgreedDatabaseComm3;
                perLifeRetroStatement.GstPayable3 = GstPayable3;
                perLifeRetroStatement.NoClaimBonus3 = NoClaimBonus3;
                perLifeRetroStatement.Claims3 = Claims3;
                perLifeRetroStatement.ProfitComm3 = ProfitComm3;
                perLifeRetroStatement.SurrenderValue3 = SurrenderValue3;
                perLifeRetroStatement.PaymentToTheReinsurer3 = PaymentToTheReinsurer3;
                perLifeRetroStatement.TotalNoOfPolicyNB3 = TotalNoOfPolicyNB3;
                perLifeRetroStatement.TotalNoOfPolicyRN3 = TotalNoOfPolicyRN3;
                perLifeRetroStatement.TotalNoOfPolicyALT3 = TotalNoOfPolicyALT3;
                perLifeRetroStatement.TotalSumReinsuredNB3 = TotalSumReinsuredNB3;
                perLifeRetroStatement.TotalSumReinsuredRN3 = TotalSumReinsuredRN3;
                perLifeRetroStatement.TotalSumReinsuredALT3 = TotalSumReinsuredALT3;

                perLifeRetroStatement.UpdatedAt = DateTime.Now;
                perLifeRetroStatement.UpdatedById = UpdatedById ?? perLifeRetroStatement.UpdatedById;

                db.Entry(perLifeRetroStatement).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroStatement perLifeRetroStatement = db.PerLifeRetroStatements.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeRetroStatement == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroStatement, true);

                db.Entry(perLifeRetroStatement).State = EntityState.Deleted;
                db.PerLifeRetroStatements.Remove(perLifeRetroStatement);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeRetroStatements.Where(q => q.PerLifeSoaId == perLifeSoaId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeRetroStatement perLifeRetroStatement in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeRetroStatement, true);
                    trails.Add(trail);

                    db.Entry(perLifeRetroStatement).State = EntityState.Deleted;
                    db.PerLifeRetroStatements.Remove(perLifeRetroStatement);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
