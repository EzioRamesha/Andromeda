using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FinanceProvisioningTransactionBo
    {
        public int Id { get; set; }

        public int ClaimRegisterId { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public int? FinanceProvisioningId { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public int Status { get; set; }

        public int SortIndex { get; set; }

        public bool IsLatestProvision { get; set; }

        public string ClaimId { get; set; }

        public string PolicyNumber { get; set; }

        public string CedingCompany { get; set; }

        public string EntryNo { get; set; }

        public string Quarter { get; set; }

        public double SumReinsured { get; set; }

        public string SumReinsuredStr { get; set; }

        public double ClaimRecoveryAmount { get; set; }

        public string ClaimRecoveryAmountStr { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyType { get; set; }

        public string ClaimCode { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public string LastTransactionQuarter { get; set; }

        public DateTime? DateOfEvent { get; set; }

        public string RiskQuarterProvisioning { get; set; } //Risk Quarter

        public int? RiskPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public string MlreBenefitCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroRecovery1 { get; set; }

        public double? RetroRecovery2 { get; set; }

        public double? RetroRecovery3 { get; set; }

        public string ReinsBasisCode { get; set; }
        public DateTime? ReinsEffDatePol { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedAtStr { get; set; }

        // Claim Register variable
        public string RiskQuarter { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public string Mfrs17ContractCode { get; set; }

        // E3E4
        public string BusinessOrigin { get; set; }

        public const int StatusPending = 1;
        public const int StatusProvisioned = 2;
    }
}
