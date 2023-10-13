using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using Shared;
using System;
using System.Linq;

namespace BusinessObject.SoaDatas
{
    public class SoaDataBatchBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public int? TreatyId { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public int? CurrencyCodePickListDetailId { get; set; }

        public PickListDetailBo CurrencyCodePickListDetailBo { get; set; }

        public double? CurrencyRate { get; set; }

        public int Status { get; set; }

        public int DataUpdateStatus { get; set; }

        public string Quarter { get; set; }
        public QuarterObject QuarterObject { get; set; }

        public int Type { get; set; }

        public DateTime StatementReceivedAt { get; set; }

        public int? RiDataBatchId { get; set; }
        public virtual RiDataBatchBo RiDataBatchBo { get; set; }

        public int? ClaimDataBatchId { get; set; }
        public virtual ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int DirectStatus { get; set; }
        public int InvoiceStatus { get; set; }

        public int TotalRecords { get; set; }
        public int TotalMappingFailedStatus { get; set; }

        public bool IsRiDataAutoCreate { get; set; } = false;

        public bool IsClaimDataAutoCreate { get; set; } = false;

        public bool IsProfitCommissionData { get; set; } = false;

        public int CreatedById { get; set; }

        public UserBo CreatedByBo { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Operational Dashboard
        public int NoOfCase { get; set; }

        public string StatusName { get; set; }

        public double TotalGrossPremium { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusSuccess = 4;
        public const int StatusFailed = 5;
        public const int StatusSubmitForApproval = 6;
        public const int StatusApproved = 7;
        public const int StatusRejected = 8;
        public const int StatusConditionalApproval = 9;
        public const int StatusProvisionalApproval = 10;
        public const int StatusMax = 10;

        public const int DataUpdateStatusPending = 101;
        public const int DataUpdateStatusSubmitForDataUpdate = 102;
        public const int DataUpdateStatusDataUpdating = 103;
        public const int DataUpdateStatusDataUpdateComplete = 104;
        public const int DataUpdateStatusDataUpdateFailed = 105;
        public const int DataUpdateStatusStart = 101;
        public const int DataUpdateStatusEnd = 104;
        public const int DataUpdateStatusMax = 5;

        public const int StatusPendingDelete = 255;

        public const int TypeReinsuranceData = 1;
        public const int TypeRetakafulData = 2;
        public const int TypeMax = 2;

        public const int DirectStatusIncomplete = 1;
        public const int DirectStatusCompleted = 2;
        public const int DirectStatusMax = 2;

        public const int InvoiceStatusInvoicing = 1;
        public const int InvoiceStatusInvoiced = 2;
        public const int InvoiceStatusMax = 2;

        public const int TabSoaValidationReinsuranceMyr = 1;
        public const int TabSoaValidationReinsuranceOri = 2;
        public const int TabSoaValidationRetakaful = 3;
        public const int TabSoaPostValidationMLReShareMyr = 4;
        public const int TabSoaPostValidationMLReShareOri = 5;
        public const int TabSoaPostValidationLayerShare = 6;
        public const int TabSoaPostValidationRetakaful = 7;
        public const int TabSoaCompiledSummaryIfrs4 = 8;
        public const int TabSoaCompiledSummaryIfrs17 = 9;

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
                case StatusSubmitForApproval:
                    return "Submit For Approval";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                case StatusConditionalApproval:
                    return "Conditional Approval";
                case StatusProvisionalApproval:
                    return "Provisional Approval";

                case DataUpdateStatusPending:
                    return "Pending";
                case DataUpdateStatusSubmitForDataUpdate:
                    return "Submit For Data Update";
                case DataUpdateStatusDataUpdating:
                    return "Data Updating";
                case DataUpdateStatusDataUpdateComplete:
                    return "Data Update Completed";
                case DataUpdateStatusDataUpdateFailed:
                    return "Data Update Failed";

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
                case StatusSubmitForApproval:
                    return "status-submitfinalise-badge";
                case StatusApproved:
                    return "status-success-badge";
                case StatusRejected:
                    return "status-fail-badge";
                case StatusConditionalApproval:
                    return "status-success-badge";
                case StatusProvisionalApproval:
                    return "status-success-badge";

                case DataUpdateStatusPending:
                    return "status-pending-badge";
                case DataUpdateStatusSubmitForDataUpdate:
                    return "status-submitprocess-badge";
                case DataUpdateStatusDataUpdating:
                    return "status-processing-badge";
                case DataUpdateStatusDataUpdateComplete:
                    return "status-success-badge";
                case DataUpdateStatusDataUpdateFailed:
                    return "status-fail-badge";

                default:
                    return "";
            }
        }

        public static string GetTypeName(int? key)
        {
            switch (key)
            {
                case TypeReinsuranceData:
                    return "Reinsurance Data";
                case TypeRetakafulData:
                    return "Retakaful Data";
                default:
                    return "";
            }
        }

        public static int GetType(string code)
        {
            switch (code)
            {
                case "WM":
                case "OM":
                    return TypeReinsuranceData;
                case "SF":
                    return TypeRetakafulData;
                default:
                    return 0;
            }
        }

        public static string GetDirectStatusName(int key)
        {
            switch (key)
            {
                case DirectStatusCompleted:
                    return "Complete";
                case DirectStatusIncomplete:
                    return "Incomplete";
                default:
                    return "";
            }
        }

        public static string GetInvoiceStatusClass(int key)
        {
            switch (key)
            {
                case InvoiceStatusInvoicing:
                    return "status-success-badge";
                case InvoiceStatusInvoiced:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public static string GetInvoiceStatusName(int key)
        {
            switch (key)
            {
                case InvoiceStatusInvoicing:
                    return "Invoicing";
                case InvoiceStatusInvoiced:
                    return "Invoiced";
                default:
                    return "";
            }
        }

        public static string GetDirectStatusClass(int key)
        {
            switch (key)
            {
                case DirectStatusCompleted:
                    return "status-success-badge";
                case DirectStatusIncomplete:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetTabName(int key)
        {
            switch (key)
            {
                case TabSoaValidationReinsuranceMyr:
                    return "SoaValidationReinsuranceMyr";
                case TabSoaValidationReinsuranceOri:
                    return "SoaValidationReinsuranceOri";
                case TabSoaValidationRetakaful:
                    return "SoaValidationRetakaful";
                case TabSoaPostValidationMLReShareMyr:
                    return "SoaPostValidationMLReShareMyr";
                case TabSoaPostValidationMLReShareOri:
                    return "SoaPostValidationMLReShareOri";
                case TabSoaPostValidationLayerShare:
                    return "SoaPostValidationLayerShare";
                case TabSoaPostValidationRetakaful:
                    return "SoaPostValidationRetakaful";
                case TabSoaCompiledSummaryIfrs4:
                    return "SoaCompiledSummaryfrs4";
                case TabSoaCompiledSummaryIfrs17:
                    return "SoaCompiledSummaryIfrs17";
                default:
                    return "";
            }
        }

        public void GetQuarterObject()
        {
            QuarterObject = new QuarterObject(Quarter);
        }

        public bool CanUploadFile
        {
            get
            {
                return CanUpload(Status);
            }
        }

        public bool CanDataUpdate
        {
            get
            {
                return CanUpdate(Status);
            }
        }

        public static bool CanUpload(int status)
        {
            int[] statuses = { StatusPending, StatusFailed, StatusSuccess, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanSubmitForProcessing(int status)
        {
            int[] statuses = { StatusPending, StatusFailed, StatusSuccess, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanSubmitForDataUpdate(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed, StatusRejected, StatusConditionalApproval, StatusProvisionalApproval };
            return statuses.Contains(status);
        }

        public static bool CanSubmitForApproval(int status)
        {
            int[] statuses = { StatusSuccess, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanApproveReject(int status)
        {
            int[] statuses = { StatusSubmitForApproval, StatusConditionalApproval, StatusProvisionalApproval };
            return statuses.Contains(status);
        }

        public static bool CanUpdate(int status)
        {
            int[] statuses = { DataUpdateStatusPending, DataUpdateStatusDataUpdateComplete, DataUpdateStatusDataUpdateFailed };
            return statuses.Contains(status);
        }

        public static bool CanProcess(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanSave(int status)
        {
            int[] statuses = { StatusPending, StatusSuccess, StatusFailed, StatusRejected, StatusConditionalApproval, StatusProvisionalApproval };
            return statuses.Contains(status);
        }
    }
}
