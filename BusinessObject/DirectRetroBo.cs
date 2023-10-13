using BusinessObject.SoaDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DirectRetroBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string SoaQuarter { get; set; }

        public int SoaDataBatchId { get; set; }

        public SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int RetroStatus { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int RetroStatusPending = 1;
        public const int RetroStatusSubmitForProcessing = 2;
        public const int RetroStatusProcessing = 3;
        public const int RetroStatusCompleted = 4;
        public const int RetroStatusFailed = 5;
        public const int RetroStatusPendingApproval = 6;
        public const int RetroStatusApproved = 7;
        public const int RetroStatusStatementIssuing = 8;
        public const int RetroStatusStatementIssued = 9;

        public const int RetroStatusMax = 9;

        public const int FailedTypeValidation = 1;
        public const int FailedTypeNoRetroParty = 2;
        public const int FailedTypeExceedRetroParty = 3;
        public const int FailedTypePremiumSpread = 4;
        public const int FailedTypePremiumSpreadDetail = 5;

        public const int FailedTypeMax = 5;

        public static string GetRetroStatusName(int key)
        {
            switch (key)
            {
                case RetroStatusPending:
                    return "Pending";
                case RetroStatusSubmitForProcessing:
                    return "Submit for Processing";
                case RetroStatusProcessing:
                    return "Processing";
                case RetroStatusCompleted:
                    return "Completed";
                case RetroStatusFailed:
                    return "Failed";
                case RetroStatusPendingApproval:
                    return "Pending Approval";
                case RetroStatusApproved:
                    return "Approved";
                case RetroStatusStatementIssuing:
                    return "Statement Issuing";
                case RetroStatusStatementIssued:
                    return "Statement Issued";
                default:
                    return "";
            }
        }

        public static string GetRetroStatusClass(int key)
        {
            switch (key)
            {
                case RetroStatusPending:
                    return "status-pending-badge";
                case RetroStatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case RetroStatusProcessing:
                    return "status-processing-badge";
                case RetroStatusCompleted:
                    return "status-success-badge";
                case RetroStatusFailed:
                    return "status-fail-badge";
                case RetroStatusPendingApproval:
                    return "status-processing-badge";
                case RetroStatusApproved:
                    return "status-success-badge";
                case RetroStatusStatementIssuing:
                    return "status-finalising-badge";
                case RetroStatusStatementIssued:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static string GetFailedTypeName(int key)
        {
            switch (key)
            {
                case FailedTypeValidation:
                    return "Total Required fields validation failed";
                case FailedTypeNoRetroParty:
                    return "Total Retro Party not found";
                case FailedTypeExceedRetroParty:
                    return "Total Retro Party exceeded 3";
                case FailedTypePremiumSpread:
                    return "Total Premium Spread not exist in Direct Retro Config";
                case FailedTypePremiumSpreadDetail:
                    return "Total Premium Spread Detail not found with the params";
                default:
                    return "";
            }
        }
    }
}
