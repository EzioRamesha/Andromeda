using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.InvoiceRegisters
{
    [Table("InvoiceRegisterHistories")]
    public class InvoiceRegisterHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Required, Index]
        public int InvoiceRegisterId { get; set; }

        [Required, Index]
        public int InvoiceRegisterBatchId { get; set; }

        [Index]
        public int? SoaDataBatchId { get; set; }

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

        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[InvoiceRegisterHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},[Id],{1} FROM [dbo].[InvoiceRegister]", cutOffId, string.Join(",", fields.Skip(2).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[InvoiceRegisterId]",
                "[InvoiceRegisterBatchId]",
                "[InvoiceReference]",
                "[TotalPaid]",
                "[PaymentReference]",
                "[PaymentAmount]",
                "[PaymentReceivedDate]",
                "[Year1st]",
                "[Renewal]",
                "[Gross1st]",
                "[GrossRenewal]",
                "[AltPremium]",
                "[Discount1st]",
                "[DiscountRen]",
                "[DiscountAlt]",
                "[NBCession]",
                "[NBSumReins]",
                "[RNCession]",
                "[RNSumReins]",
                "[AltCession]",
                "[AltSumReins]",
                "[DTH]",
                "[TPA]",
                "[TPS]",
                "[PPD]",
                "[CCA]",
                "[CCS]",
                "[PA]",
                "[HS]",
                "[Frequency]",
                "[PreparedById]",
                "[ValuationGross1st]",
                "[ValuationGrossRen]",
                "[ValuationDiscount1st]",
                "[ValuationDiscountRen]",
                "[ValuationCom1st]",
                "[ValuationComRen]",
                "[ValuationClaims]",
                "[ValuationProfitCom]",
                "[ValuationMode]",
                "[ValuationRiskPremium]",
                "[Remark]",
                "[CreatedAt]",
                "[UpdatedAt]",
                "[CreatedById]",
                "[UpdatedById]",
                "[InvoiceType]",
                "[InvoiceNumber]",
                "[InvoiceDate]",
                "[StatementReceivedDate]",
                "[CedantId]",
                "[RiskQuarter]",
                "[TreatyCodeId]",
                "[AccountDescription]",
                "[RiskPremium]",
                "[Claim]",
                "[ProfitComm]",
                "[Levy]",
                "[SurrenderValue]",
                "[ModcoReserveIncome]",
                "[ReinsDeposit]",
                "[NoClaimBonus]",
                "[DatabaseCommission]",
                "[AdministrationContribution]",
                "[ShareOfRiCommissionFromCompulsoryCession]",
                "[RecaptureFee]",
                "[CreditCardCharges]",
                "[BrokerageFee]",
                "[PlanName]",
                "[CurrencyCode]",
                "[CurrencyRate]",
                "[SoaQuarter]",
                "[ReasonOfAdjustment1]",
                "[ReasonOfAdjustment2]",
                "[InvoiceNumber1]",
                "[InvoiceDate1]",
                "[Amount1]",
                "[InvoiceNumber2]",
                "[InvoiceDate2]",
                "[Amount2]",
                "[SoaDataBatchId]",
                "[Gst]",
                "[ContractCode]",
                "[AnnualCohort]",
                "[TPD]",
                "[CI]",
                "[ReportingType]",
                "[Mfrs17CellName]"
            };
        }

        public static int CountByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterHistories.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).Count();
            }
        }
    }
}
