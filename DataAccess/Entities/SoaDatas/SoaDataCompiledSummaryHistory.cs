using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataCompiledSummaryHistories")]
    public class SoaDataCompiledSummaryHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }

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
        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [MaxLength(3)]
        public string Frequency { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[SoaDataCompiledSummaryHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},{1} FROM [dbo].[SoaDataCompiledSummaries]", cutOffId, string.Join(",", fields.Skip(1).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[SoaDataBatchId]",
                "[InvoiceType]",
                "[InvoiceDate]",
                "[StatementReceivedDate]",
                "[BusinessCode]",
                "[TreatyCode]",
                "[SoaQuarter]",
                "[RiskQuarter]",
                "[AccountDescription]",
                "[NbPremium]",
                "[RnPremium]",
                "[AltPremium]",
                "[TotalDiscount]",
                "[RiskPremium]",
                "[NoClaimBonus]",
                "[Levy]",
                "[Claim]",
                "[ProfitComm]",
                "[SurrenderValue]",
                "[Gst]",
                "[ModcoReserveIncome]",
                "[RiDeposit]",
                "[DatabaseCommission]",
                "[AdministrationContribution]",
                "[ShareOfRiCommissionFromCompulsoryCession]",
                "[RecaptureFee]",
                "[CreditCardCharges]",
                "[BrokerageFee]",
                "[NetTotalAmount]",
                "[ReasonOfAdjustment1]",
                "[ReasonOfAdjustment2]",
                "[InvoiceNumber1]",
                "[InvoiceDate1]",
                "[Amount1]",
                "[InvoiceNumber2]",
                "[InvoiceDate2]",
                "[Amount2]",
                "[FilingReference]",
                "[ServiceFeePercentage]",
                "[ServiceFee]",
                "[Sst]",
                "[TotalAmount]",
                "[NbDiscount]",
                "[RnDiscount]",
                "[AltDiscount]",
                "[NbCession]",
                "[RnCession]",
                "[AltCession]",
                "[NbSar]",
                "[RnSar]",
                "[AltSar]",
                "[DTH]",
                "[TPA]",
                "[TPS]",
                "[PPD]",
                "[CCA]",
                "[CCS]",
                "[PA]",
                "[HS]",
                "[TPD]",
                "[CI]",
                "[CurrencyCode]",
                "[CurrencyRate]",
                "[ContractCode]",
                "[AnnualCohort]",
                "[CreatedAt]",
                "[CreatedById]",
                "[UpdatedAt]",
                "[UpdatedById]",
                "[Frequency]",
                "[ReportingType]",
                "[Mfrs17CellName]"
            };
        }
    }
}
