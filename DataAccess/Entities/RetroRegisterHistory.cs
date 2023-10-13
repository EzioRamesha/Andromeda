using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities
{
    [Table("RetroRegisterHistories")]
    public class RetroRegisterHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Index]
        public int RetroRegisterId { get; set; }

        public int? RetroRegisterBatchId { get; set; }

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

        public int? DirectRetroId { get; set; }

        [Index]
        public int Type { get; set; }

        [MaxLength(35)]
        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }


        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[RetroRegisterHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},[Id],{1} FROM [dbo].[RetroRegister]", cutOffId, string.Join(",", fields.Skip(2).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[RetroRegisterId]",
                "[RetroRegisterBatchId]",
                "[RetroStatementType]",
                "[RetroStatementNo]",
                "[RetroStatementDate]",
                "[ReportCompletedDate]",
                "[SendToRetroDate]",
                "[RetroPartyId]",
                "[CedantId]",
                "[TreatyCodeId]",
                "[RiskQuarter]",
                "[TreatyNumber]",
                "[Schedule]",
                "[TreatyType]",
                "[LOB]",
                "[AccountFor]",
                "[Year1st]",
                "[Renewal]",
                "[ReserveCededBegin]",
                "[ReserveCededEnd]",
                "[RiskChargeCededBegin]",
                "[RiskChargeCededEnd]",
                "[AverageReserveCeded]",
                "[Gross1st]",
                "[GrossRen]",
                "[AltPremium]",
                "[Discount1st]",
                "[DiscountRen]",
                "[DiscountAlt]",
                "[RiskPremium]",
                "[Claims]",
                "[ProfitCommission]",
                "[SurrenderVal]",
                "[NoClaimBonus]",
                "[RetrocessionMarketingFee]",
                "[AgreedDBCommission]",
                "[NetTotalAmount]",
                "[NbCession]",
                "[NbSumReins]",
                "[RnCession]",
                "[RnSumReins]",
                "[AltCession]",
                "[AltSumReins]",
                "[Frequency]",
                "[PreparedById]",
                "[OriginalSoaQuarter]",
                "[RetroConfirmationDate]",
                "[ValuationGross1st]",
                "[ValuationGrossRen]",
                "[ValuationDiscount1st]",
                "[ValuationDiscountRen]",
                "[ValuationCom1st]",
                "[ValuationComRen]",
                "[CreatedAt]",
                "[CreatedById]",
                "[UpdatedAt]",
                "[UpdatedById]",
                "[DirectRetroId]",
                "[Type]",
                "[GstPayable]",
                "[Remark]",
                "[Status]",
                "[ContractCode]",
                "[AnnualCohort]",
                "[ReportingType]",
            };
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterHistories.Where(q => q.RetroPartyId == retroPartyId).Count();
            }
        }
    }
}
