using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataBatchBo
    {
        public int Id { get; set; }

        public int Status { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public int? TreatyId { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public int ClaimDataConfigId { get; set; }

        public virtual ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int? SoaDataBatchId { get; set; }

        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int? ClaimTransactionTypePickListDetailId { get; set; }

        public virtual PickListDetailBo ClaimTransactionTypePickListDetailBo { get; set; }

        public string Configs { get; set; }

        public ClaimDataFileConfig ClaimDataFileConfig { get; set; }

        public string OverrideProperties { get; set; }

        public string Quarter { get; set; }

        public int TotalMappingFailedStatus { get; set; }
        public int TotalPreComputationFailedStatus { get; set; }
        public int TotalPreValidationFailedStatus { get; set; }

        public DateTime ReceivedAt { get; set; }

        public int CreatedById { get; set; }
        public UserBo CreatedByBo { get; set; }

        public int? UpdatedById { get; set; }

        // Operational Dashboard
        public int NoOfCase { get; set; }

        public string StatusName { get; set; }

        public const int StatusPending = 1;                 // save, process
        public const int StatusSubmitForProcessing = 2;     // disabled    
        public const int StatusProcessing = 3;              // disabled
        public const int StatusSuccess = 4;                 // save, process, finalise
        public const int StatusFailed = 5;                  // save, process
        public const int StatusSubmitForReportClaim = 6;    // disabled
        public const int StatusReportingClaim = 7;          // disabled
        public const int StatusReportedClaim = 8;           // disabled
        public const int StatusReportingClaimFailed = 9;    // save, process

        public const int MaxStatus = 8;

        public const int StatusPendingDelete = 255;

        public ClaimDataBatchBo()
        {
            ClaimDataFileConfig = new ClaimDataFileConfig();
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";

                case StatusSubmitForProcessing:
                    return "Submit For Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusSuccess:
                    return "Success";
                case StatusFailed:
                    return "Failed";
                case StatusSubmitForReportClaim:
                    return "Submit For Report Claim";
                case StatusReportingClaim:
                    return "Reporting Claim";
                case StatusReportedClaim:
                    return "Reported";
                case StatusReportingClaimFailed:
                    return "Reporting Claim Failed";
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

                case StatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusSuccess:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";

                case StatusSubmitForReportClaim:
                    return "status-submitfinalise-badge";
                case StatusReportingClaim:
                    return "status-finalising-badge";
                case StatusReportedClaim:
                    return "status-finalize-badge";
                case StatusReportingClaimFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public bool CanUploadFile
        {
            get
            {
                return CanUpload(Status);
            }
        }
        
        public bool CanUpdateDataFile
        {
            get
            {
                return CanProcess(Status);
            }
        }

        public static bool CanProcess(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed, StatusReportingClaimFailed };
            return statuses.Contains(status);
        }
        
        public static bool CanReportClaim(int status)
        {
            int[] statuses = { StatusSuccess, StatusReportingClaimFailed };
            return statuses.Contains(status);
        }

        public static bool CanUpload(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed, StatusReportingClaimFailed };
            return statuses.Contains(status);
        }

        public static bool CanDelete(int status)
        {
            int[] statuses = { StatusSubmitForReportClaim, StatusReportingClaim, StatusReportedClaim };
            return !statuses.Contains(status);
        }

        public static bool IsReported(int status)
        {
            int[] statuses = { StatusSubmitForReportClaim, StatusReportingClaim, StatusReportedClaim };
            return statuses.Contains(status);
        }

        public static List<int> RequiredOverrideProperties()
        {
            return new List<int> {
                StandardClaimDataOutputBo.TypeCedantDateOfNotification,
                StandardClaimDataOutputBo.TypeCurrencyCode
            };
        }
    }
}
