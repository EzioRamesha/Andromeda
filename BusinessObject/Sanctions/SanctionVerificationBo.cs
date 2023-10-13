using System;
using System.Collections.Generic;

namespace BusinessObject.Sanctions
{
    public class SanctionVerificationBo
    {
        public int Id { get; set; }

        public int SourceId { get; set; }

        public SourceBo SourceBo { get; set; }

        public bool IsRiData { get; set; }

        public bool IsClaimRegister { get; set; }

        public bool IsReferralClaim { get; set; }

        public int Type { get; set; }

        public int? BatchId { get; set; }

        public int Status { get; set; }

        public int Record { get; set; }

        public int UnprocessedRecords { get; set; }

        public DateTime? ProcessStartAt { get; set; }

        public DateTime? ProcessEndAt { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // For Sanction Verification Checking only
        public List<SanctionVerificationDetailBo> SanctionVerificationDetailBos { get; set; }

        // Dashboard
        public int DetailModuleId { get; set; }

        public int DataCount { get; set; }

        public int RiDataCount { get; set; }

        public int ClaimRegisterCount { get; set; }

        public int ReferralClaimCount { get; set; }

        public int TotalCount { get; set; }

        public int PotentialCount { get; set; }

        public int WhitelistCount { get; set; }

        public int ExactMatchCount { get; set; }

        public const int TypeAuto = 1;
        public const int TypeManual = 2;
        public const int TypeData = 3;

        public const int TypeMax = 3;

        public const int StatusPending = 1;
        public const int StatusProcessing = 2;
        public const int StatusCompleted = 3;
        public const int StatusFailed = 4;

        public const int StatusMax = 4;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeAuto:
                    return "Auto";
                case TypeManual:
                    return "Manual";
                case TypeData:
                    return "Data";
                default:
                    return "";
            }
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusProcessing:
                    return "Processing";
                case StatusCompleted:
                    return "Completed";
                case StatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-pending-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusCompleted:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }
    }
}
