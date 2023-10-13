using BusinessObject.Identity;
using Shared;
using System;
using System.Linq;

namespace BusinessObject.Retrocession
{
    public class PerLifeSoaBo
    {
        public int Id { get; set; }

        public int RetroPartyId { get; set; }
        public RetroPartyBo RetroPartyBo { get; set; }

        public int RetroTreatyId { get; set; }
        public RetroTreatyBo RetroTreatyBo { get; set; }

        public int Status { get; set; }

        public string SoaQuarter { get; set; }
        public QuarterObject QuarterObject { get; set; }

        public int InvoiceStatus { get; set; }

        public int? PersonInChargeId { get; set; }
        public UserBo PersonInChargeBo { get; set; }

        public DateTime? ProcessingDate { get; set; }
        public string ProcessingDateStr { get; set; }

        public bool IsProfitCommissionData { get; set; } = false;

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int? PerLifeAggregationId { get; set; }
        public PerLifeAggregationBo PerLifeAggregationBo { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusProcessingSuccess = 4;
        public const int StatusProcessingFailed = 5;
        public const int StatusPendingApproval = 6;
        public const int StatusApproved = 7;
        public const int StatusRejected = 8;
        public const int StatusSubmitForIssuance = 9;
        public const int StatusPendingIssuance = 10;
        public const int StatusStatementIssued = 11;
        public const int StatusMax = 11;

        public const int InvoiceStatusInvoicing = 1;
        public const int InvoiceStatusInvoiced = 2;
        public const int InvoiceStatusMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusSubmitForProcessing:
                    return "Submit for Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusProcessingSuccess:
                    return "Processing Sucess";
                case StatusProcessingFailed:
                    return "Processing Failed";
                case StatusPendingApproval:
                    return "Pending Approval";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                case StatusSubmitForIssuance:
                    return "Submit for Issuance";
                case StatusPendingIssuance:
                    return "Pending Retro Statement Issuance";
                case StatusStatementIssued:
                    return "Retro Statement Issued";
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
                case StatusSubmitForIssuance:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                case StatusPendingIssuance:
                    return "status-processing-badge";
                case StatusProcessingSuccess:
                case StatusPendingApproval:
                case StatusApproved:
                    return "status-success-badge";
                case StatusProcessingFailed:
                case StatusRejected:
                    return "status-fail-badge";
                case StatusStatementIssued:
                    return "status-finalize-badge";
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

        public void GetQuarterObject()
        {
            QuarterObject = new QuarterObject(SoaQuarter);
        }

        public static bool CanSubmitForProcessing(int status)
        {
            int[] statuses = { StatusPending, StatusProcessingSuccess, StatusProcessingFailed, StatusPendingApproval, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanSubmitForApproval(int status)
        {
            int[] statuses = { StatusProcessingSuccess, StatusRejected };
            return statuses.Contains(status);
        }

        public static bool CanApprove(int status)
        {
            int[] statuses = { StatusPendingApproval };
            return statuses.Contains(status);
        }

        public static bool CanSave(int status)
        {
            int[] statuses = { StatusPending, StatusPendingApproval, StatusRejected, StatusPendingIssuance };
            return statuses.Contains(status);
        }
    }
}
