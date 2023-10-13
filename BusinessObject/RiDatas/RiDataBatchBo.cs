using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using System;
using System.Collections.Generic;

namespace BusinessObject.RiDatas
{
    public class RiDataBatchBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public int? TreatyId { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public int RiDataConfigId { get; set; }

        public virtual RiDataConfigBo RiDataConfigBo { get; set; }

        public string Configs { get; set; }

        public RiDataFileConfig RiDataFileConfig { get; set; }

        public string OverrideProperties { get; set; }

        public int Status { get; set; }

        public int ProcessWarehouseStatus { get; set; }

        public string Quarter { get; set; }

        public int TotalMappingFailedStatus { get; set; }

        public int TotalPreComputation1FailedStatus { get; set; }

        public int TotalPreComputation2FailedStatus { get; set; }

        public int TotalPreValidationFailedStatus { get; set; }

        public int TotalPostComputationFailedStatus { get; set; }

        public int TotalPostValidationFailedStatus { get; set; }

        public int TotalFinaliseFailedStatus { get; set; }

        public int TotalProcessWarehouseFailedStatus { get; set; }

        public int TotalConflict { get; set; }

        public int RecordType { get; set; }

        public DateTime ReceivedAt { get; set; }

        public int? SoaDataBatchId { get; set; }

        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int CreatedById { get; set; }

        public UserBo CreatedByBo { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? FinalisedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Operational Dashboard
        public int NoOfCase { get; set; }

        public string StatusName { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForPreProcessing = 2;
        public const int StatusPreProcessing = 3;
        public const int StatusPreSuccess = 4;
        public const int StatusPreFailed = 5;
        public const int StatusSubmitForFinalise = 6;
        public const int StatusFinalising = 7;
        public const int StatusFinalised = 8;
        public const int StatusFinaliseFailed = 9;
        public const int StatusSubmitForPostProcessing = 10;
        public const int StatusPostProcessing = 11;
        public const int StatusPostSuccess = 12;
        public const int StatusPostFailed = 13;

        public const int StatusMax = 13;

        public const int StatusPendingDelete = 255;

        public const int ProcessWarehouseStatusNotApplicable = 1;
        public const int ProcessWarehouseStatusPending = 2;
        public const int ProcessWarehouseStatusProcessing = 3;
        public const int ProcessWarehouseStatusSuccess = 4;
        public const int ProcessWarehouseStatusFailed = 5;
        public const int ProcessWarehouseStatusProcessFailed = 6;
        public const int ProcessWarehouseStatusMax = 6;

        public const int RecordTypeNew = 1;
        public const int RecordTypeAdjustment = 2;
        public const int RecordTypeMax = 2;

        public RiDataBatchBo()
        {
            RiDataFileConfig = new RiDataFileConfig();
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusSubmitForPreProcessing:
                    return "Submit For Pre-Processing";
                case StatusPreProcessing:
                    return "Pre-Processing";
                case StatusPreSuccess:
                    return "Pre-Success";
                case StatusPreFailed:
                    return "Pre-Failed";
                case StatusSubmitForFinalise:
                    return "Submit For Finalise";
                case StatusFinalising:
                    return "Finalising";
                case StatusFinalised:
                    return "Finalised";
                case StatusFinaliseFailed:
                    return "Finalised Failed";
                case StatusSubmitForPostProcessing:
                    return "Submit For Post-Processing";
                case StatusPostProcessing:
                    return "Post-Processing";
                case StatusPostSuccess:
                    return "Post-Success";
                case StatusPostFailed:
                    return "Post-Failed";
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
                case StatusSubmitForPreProcessing:
                    return "status-submitprocess-badge";
                case StatusPreProcessing:
                    return "status-processing-badge";
                case StatusPreSuccess:
                    return "status-success-badge";
                case StatusPreFailed:
                    return "status-fail-badge";
                case StatusSubmitForFinalise:
                    return "status-submitfinalise-badge";
                case StatusFinalising:
                    return "status-finalising-badge";
                case StatusFinalised:
                    return "status-finalize-badge";
                case StatusFinaliseFailed:
                    return "status-fail-badge";
                case StatusSubmitForPostProcessing:
                    return "status-submitprocess-badge";
                case StatusPostProcessing:
                    return "status-processing-badge";
                case StatusPostSuccess:
                    return "status-success-badge";
                case StatusPostFailed:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetProcessWarehouseStatusName(int key)
        {
            switch (key)
            {
                case ProcessWarehouseStatusNotApplicable:
                    return "Not Applicable";
                case ProcessWarehouseStatusPending:
                    return "Process Warehouse Pending";
                case ProcessWarehouseStatusProcessing:
                    return "Process Warehouse Processing";
                case ProcessWarehouseStatusSuccess:
                    return "Process Warehouse Success";
                case ProcessWarehouseStatusFailed:
                    return "Process Warehouse Failed";
                case ProcessWarehouseStatusProcessFailed:
                    return "Process Warehouse Submit For Reprocess";
                default:
                    return "";
            }
        }

        public static string GetProcessWarehouseStatusClass(int key)
        {
            switch (key)
            {
                case ProcessWarehouseStatusPending:
                    return "status-pending-badge";
                case ProcessWarehouseStatusProcessing:
                    return "status-processing-badge";
                case ProcessWarehouseStatusSuccess:
                    return "status-success-badge";
                case ProcessWarehouseStatusFailed:
                    return "status-fail-badge";
                case ProcessWarehouseStatusProcessFailed:
                    return "status-submitprocess-badge";
                default:
                    return "";
            }
        }

        public static string GetRecordTypeName(int key)
        {
            switch (key)
            {
                case RecordTypeNew:
                    return "New";
                case RecordTypeAdjustment:
                    return "Adjustment";
                default:
                    return "";
            }
        }

        public static List<int> DropDownStatusOrder()
        {
            return new List<int>
            {
                StatusPending,
                StatusSubmitForPreProcessing,
                StatusPreProcessing,
                StatusPreSuccess,
                StatusPreFailed,
                StatusSubmitForPostProcessing,
                StatusPostProcessing,
                StatusPostSuccess,
                StatusPostFailed,
                StatusSubmitForFinalise,
                StatusFinalising,
                StatusFinalised,
                StatusFinaliseFailed,
            };
        }
    }
}
