using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DirectRetroProvisioningTransactionBo
    {
        public int Id { get; set; }

        public int ClaimRegisterId { get; set; }

        public  ClaimRegisterBo ClaimRegisterBo { get; set; }

        public int? FinanceProvisioningId { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public int FinanceProvisioningStatus { get; set; }

        public bool IsLatestProvision { get; set; }

        public string ClaimId { get; set; }

        public string CedingCompany { get; set; }

        public string EntryNo { get; set; }

        public string Quarter { get; set; }

        public string RetroParty { get; set; }

        public double RetroRecovery { get; set; }

        public string RetroRecoveryStr { get; set; }

        public string RetroStatementId { get; set; }

        public DateTime? RetroStatementDate { get; set; }

        public string RetroStatementDateStr { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyType { get; set; }

        public string ClaimCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Claim Register variable
        public string RiskQuarter { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public string Mfrs17ContractCode { get; set; }

        // E3E4
        public string BusinessOrigin { get; set; }
    }
}
