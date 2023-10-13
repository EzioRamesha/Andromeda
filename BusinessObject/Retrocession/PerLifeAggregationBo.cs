using BusinessObject.Identity;
using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationBo
    {
        public int Id { get; set; }

        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public int CutOffId { get; set; }

        public CutOffBo CutOffBo { get; set; }

        public string SoaQuarter { get; set; }

        public DateTime? ProcessingDate { get; set; }

        public int Status { get; set; }

        public int? PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusPendingRiskQuarterProcessing = 4;
        public const int StatusSuccess = 5;
        public const int StatusFailed = 6;
        public const int StatusFinalised = 7;

        public const int StatusMax = 7;

        public const int StatusPendingDelete = 255;

        // Active Tab
        public const int ActiveTabRetroProcessing = 1;
        public const int ActiveTabExcludedRecord = 2;
        public const int ActiveTabRetroRecord = 3;

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
                case StatusPendingRiskQuarterProcessing:
                    return "Pending Risk Quarter Processing";
                case StatusSuccess:
                    return "Success";
                case StatusFailed:
                    return "Failed";
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
                case StatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusPendingRiskQuarterProcessing:
                    return "status-pending-badge";
                case StatusSuccess:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                case StatusFinalised:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }
    }
}
