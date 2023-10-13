using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationDetailBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationId { get; set; }

        public PerLifeAggregationBo PerLifeAggregationBo { get; set; }

        public string RiskQuarter { get; set; }

        public DateTime? ProcessingDate { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Status
        public const int StatusPending = 1;
        public const int StatusSubmitForValidation = 2;
        public const int StatusValidating = 3;
        public const int StatusValidationSuccess = 4;
        public const int StatusValidationFailed = 5;
        public const int StatusSubmitForProcessing = 6;
        public const int StatusProcessing = 7;
        public const int StatusProcessSuccess = 8;
        public const int StatusProcessFailed = 9;
        public const int StatusSubmitForAggregation = 10;
        public const int StatusAggregating = 11;
        public const int StatusAggregationSuccess = 12;
        public const int StatusAggregationFailed = 13;
        public const int StatusFinalised = 14;

        public const int StatusMax = 14;

        // Active Tab
        public const int ActiveTabRiData = 1;
        public const int ActiveTabException = 2;
        public const int ActiveTabRetroRiData = 3;
        public const int ActiveTabRetentionPremium = 4;
        public const int ActiveTabSummary = 5;
        public const int ActiveTabTreatySummary = 6;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusSubmitForValidation:
                    return "Submit For Validation";
                case StatusValidating:
                    return "Validating";
                case StatusValidationSuccess:
                    return "Validation Success";
                case StatusValidationFailed:
                    return "Validation Failed";
                case StatusSubmitForProcessing:
                    return "Submit For Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusProcessSuccess:
                    return "Process Success";
                case StatusProcessFailed:
                    return "Process Failed";
                case StatusSubmitForAggregation:
                    return "Submit For Aggregation";
                case StatusAggregating:
                    return "Aggregating";
                case StatusAggregationSuccess:
                    return "Aggregation Success";
                case StatusAggregationFailed:
                    return "Aggregation Failed";
                case StatusFinalised:
                    return "Finalised";
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
                case StatusSubmitForValidation:
                    return "status-submitprocess-badge";
                case StatusValidating:
                    return "status-processing-badge";
                case StatusValidationSuccess:
                    return "status-success-badge";
                case StatusValidationFailed:
                    return "status-fail-badge";
                case StatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusProcessSuccess:
                    return "status-success-badge";
                case StatusProcessFailed:
                    return "status-fail-badge";
                case StatusSubmitForAggregation:
                    return "status-submitprocess-badge";
                case StatusAggregating:
                    return "status-processing-badge";
                case StatusAggregationSuccess:
                    return "status-success-badge";
                case StatusAggregationFailed:
                    return "status-fail-badge";
                case StatusFinalised:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }

        public bool CanProcess()
        {
            return new List<int>()
            {
                StatusValidationSuccess,
                StatusValidationFailed,
                StatusProcessSuccess,
                StatusProcessFailed,
                StatusAggregationSuccess,
                StatusAggregationFailed,
                StatusFinalised
            }.Contains(Status);
        }
    }
}
