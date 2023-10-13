using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataHistories")]
    public class SoaDataHistory
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

        [Index]
        public int? SoaDataFileId { get; set; }

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

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }

        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[SoaDataHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},{1} FROM [dbo].[SoaData]", cutOffId, string.Join(",", fields.Skip(1).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[SoaDataBatchId]",
                "[SoaDataFileId]",
                "[MappingStatus]",
                "[Errors]",
                "[CompanyName]",
                "[BusinessCode]",
                "[TreatyId]",
                "[TreatyCode]",
                "[TreatyMode]",
                "[TreatyType]",
                "[PlanBlock]",
                "[RiskMonth]",
                "[SoaQuarter]",
                "[RiskQuarter]",
                "[NbPremium]",
                "[RnPremium]",
                "[AltPremium]",
                "[GrossPremium]",
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
                "[TotalCommission]",
                "[NetTotalAmount]",
                "[SoaReceivedDate]",
                "[BordereauxReceivedDate]",
                "[StatementStatus]",
                "[Remarks1]",
                "[CurrencyCode]",
                "[CurrencyRate]",
                "[SoaStatus]",
                "[ConfirmationDate]",
                "[CreatedAt]",
                "[CreatedById]",
                "[UpdatedAt]",
                "[UpdatedById]",
            };
        }
    }
}
