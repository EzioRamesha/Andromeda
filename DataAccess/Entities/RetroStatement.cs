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
    [Table("RetroStatements")]
    public class RetroStatement
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DirectRetroId { get; set; }

        [ExcludeTrail]
        public virtual DirectRetro DirectRetro { get; set; }

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

        public RetroStatement()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroStatements.Any(q => q.Id == id);
            }
        }

        public static RetroStatement Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroStatements.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroStatements.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroStatement retroStatement = Find(Id);
                if (retroStatement == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroStatement, this);

                retroStatement.DirectRetroId = DirectRetroId;
                retroStatement.RetroPartyId = RetroPartyId;
                retroStatement.Status = Status;
                retroStatement.MlreRef = MlreRef;
                retroStatement.CedingCompany = CedingCompany;
                retroStatement.TreatyCode = TreatyCode;
                retroStatement.TreatyNo = TreatyNo;
                retroStatement.Schedule = Schedule;
                retroStatement.TreatyType = TreatyType;
                retroStatement.FromMlreTo = FromMlreTo;
                retroStatement.AccountsFor = AccountsFor;
                retroStatement.DateReportCompleted = DateReportCompleted;
                retroStatement.DateSendToRetro = DateSendToRetro;

                // Data Set 1
                retroStatement.AccountingPeriod = AccountingPeriod;
                retroStatement.ReserveCededBegin = ReserveCededBegin;
                retroStatement.ReserveCededEnd = ReserveCededEnd;
                retroStatement.RiskChargeCededBegin = RiskChargeCededBegin;
                retroStatement.RiskChargeCededEnd = RiskChargeCededEnd;
                retroStatement.AverageReserveCeded = AverageReserveCeded;
                retroStatement.RiPremiumNB = RiPremiumNB;
                retroStatement.RiPremiumRN = RiPremiumRN;
                retroStatement.RiPremiumALT = RiPremiumALT;
                retroStatement.QuarterlyRiskPremium = QuarterlyRiskPremium;
                retroStatement.RetrocessionMarketingFee = RetrocessionMarketingFee;
                retroStatement.RiDiscountNB = RiDiscountNB;
                retroStatement.RiDiscountRN = RiDiscountRN;
                retroStatement.RiDiscountALT = RiDiscountALT;
                retroStatement.AgreedDatabaseComm = AgreedDatabaseComm;
                retroStatement.GstPayable = GstPayable;
                retroStatement.NoClaimBonus = NoClaimBonus;
                retroStatement.Claims = Claims;
                retroStatement.ProfitComm = ProfitComm;
                retroStatement.SurrenderValue = SurrenderValue;
                retroStatement.PaymentToTheReinsurer = PaymentToTheReinsurer;
                retroStatement.TotalNoOfPolicyNB = TotalNoOfPolicyNB;
                retroStatement.TotalNoOfPolicyRN = TotalNoOfPolicyRN;
                retroStatement.TotalNoOfPolicyALT = TotalNoOfPolicyALT;
                retroStatement.TotalSumReinsuredNB = TotalSumReinsuredNB;
                retroStatement.TotalSumReinsuredRN = TotalSumReinsuredRN;
                retroStatement.TotalSumReinsuredALT = TotalSumReinsuredALT;

                // Data Set 2
                retroStatement.AccountingPeriod2 = AccountingPeriod2;
                retroStatement.ReserveCededBegin2 = ReserveCededBegin2;
                retroStatement.ReserveCededEnd2 = ReserveCededEnd2;
                retroStatement.RiskChargeCededBegin2 = RiskChargeCededBegin2;
                retroStatement.RiskChargeCededEnd2 = RiskChargeCededEnd2;
                retroStatement.AverageReserveCeded2 = AverageReserveCeded2;
                retroStatement.RiPremiumNB2 = RiPremiumNB2;
                retroStatement.RiPremiumRN2 = RiPremiumRN2;
                retroStatement.RiPremiumALT2 = RiPremiumALT2;
                retroStatement.QuarterlyRiskPremium2 = QuarterlyRiskPremium2;
                retroStatement.RetrocessionMarketingFee2 = RetrocessionMarketingFee2;
                retroStatement.RiDiscountNB2 = RiDiscountNB2;
                retroStatement.RiDiscountRN2 = RiDiscountRN2;
                retroStatement.RiDiscountALT2 = RiDiscountALT2;
                retroStatement.AgreedDatabaseComm2 = AgreedDatabaseComm2;
                retroStatement.GstPayable2 = GstPayable2;
                retroStatement.NoClaimBonus2 = NoClaimBonus2;
                retroStatement.Claims2 = Claims2;
                retroStatement.ProfitComm2 = ProfitComm2;
                retroStatement.SurrenderValue2 = SurrenderValue2;
                retroStatement.PaymentToTheReinsurer2 = PaymentToTheReinsurer2;
                retroStatement.TotalNoOfPolicyNB2 = TotalNoOfPolicyNB2;
                retroStatement.TotalNoOfPolicyRN2 = TotalNoOfPolicyRN2;
                retroStatement.TotalNoOfPolicyALT2 = TotalNoOfPolicyALT2;
                retroStatement.TotalSumReinsuredNB2 = TotalSumReinsuredNB2;
                retroStatement.TotalSumReinsuredRN2 = TotalSumReinsuredRN2;
                retroStatement.TotalSumReinsuredALT2 = TotalSumReinsuredALT2;

                // Data Set 3
                retroStatement.AccountingPeriod3 = AccountingPeriod3;
                retroStatement.ReserveCededBegin3 = ReserveCededBegin3;
                retroStatement.ReserveCededEnd3 = ReserveCededEnd3;
                retroStatement.RiskChargeCededBegin3 = RiskChargeCededBegin3;
                retroStatement.RiskChargeCededEnd3 = RiskChargeCededEnd3;
                retroStatement.AverageReserveCeded3 = AverageReserveCeded3;
                retroStatement.RiPremiumNB3 = RiPremiumNB3;
                retroStatement.RiPremiumRN3 = RiPremiumRN3;
                retroStatement.RiPremiumALT3 = RiPremiumALT3;
                retroStatement.QuarterlyRiskPremium3 = QuarterlyRiskPremium3;
                retroStatement.RetrocessionMarketingFee3 = RetrocessionMarketingFee3;
                retroStatement.RiDiscountNB3 = RiDiscountNB3;
                retroStatement.RiDiscountRN3 = RiDiscountRN3;
                retroStatement.RiDiscountALT3 = RiDiscountALT3;
                retroStatement.AgreedDatabaseComm3 = AgreedDatabaseComm3;
                retroStatement.GstPayable3 = GstPayable3;
                retroStatement.NoClaimBonus3 = NoClaimBonus3;
                retroStatement.Claims3 = Claims3;
                retroStatement.ProfitComm3 = ProfitComm3;
                retroStatement.SurrenderValue3 = SurrenderValue3;
                retroStatement.PaymentToTheReinsurer3 = PaymentToTheReinsurer3;
                retroStatement.TotalNoOfPolicyNB3 = TotalNoOfPolicyNB3;
                retroStatement.TotalNoOfPolicyRN3 = TotalNoOfPolicyRN3;
                retroStatement.TotalNoOfPolicyALT3 = TotalNoOfPolicyALT3;
                retroStatement.TotalSumReinsuredNB3 = TotalSumReinsuredNB3;
                retroStatement.TotalSumReinsuredRN3 = TotalSumReinsuredRN3;
                retroStatement.TotalSumReinsuredALT3 = TotalSumReinsuredALT3;

                retroStatement.UpdatedAt = DateTime.Now;
                retroStatement.UpdatedById = UpdatedById ?? retroStatement.UpdatedById;

                db.Entry(retroStatement).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroStatement retroStatement = db.RetroStatements.Where(q => q.Id == id).FirstOrDefault();
                if (retroStatement == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroStatement, true);

                db.Entry(retroStatement).State = EntityState.Deleted;
                db.RetroStatements.Remove(retroStatement);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroStatements.Where(q => q.DirectRetroId == directRetroId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RetroStatement retroStatement in query.ToList())
                {
                    DataTrail trail = new DataTrail(retroStatement, true);
                    trails.Add(trail);

                    db.Entry(retroStatement).State = EntityState.Deleted;
                    db.RetroStatements.Remove(retroStatement);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
