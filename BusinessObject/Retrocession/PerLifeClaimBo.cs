using BusinessObject.Identity;
using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeClaimBo
    {
        public int Id { get; set; }

        public int CutOffId { get; set; }

        public CutOffBo CutOffBo { get; set; }

        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public int Status { get; set; }

        public string SoaQuarter { get; set; }

        public int? PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }
        public DateTime? ProcessingDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusProcessingSuccess = 4;
        public const int StatusProcessingFailed = 5;
        public const int StatusSubmitForValidation = 6;
        public const int StatusValidating = 7;
        public const int StatusValidationSuccess = 8;
        public const int StatusValidationFailed = 9;
        public const int StatusSubmitForRetroRecovery = 10;
        public const int StatusProcessingRetroRecovery = 11;
        public const int StatusProcessingRetroRecoverySuccess = 12;
        public const int StatusProcessingRetroRecoveryFailed = 13;
        public const int StatusFinalised = 14;

        public const int StatusMax = 14;

        //Active Tab
        public const int ActiveTabClaimRegisterData = 1;
        public const int ActiveTabException = 2;
        public const int ActiveTabClaimRetroData = 3;
        public const int ActiveTabSummary = 4;
        public const int ActiveTabSummaryPendingClaims = 5;
        public const int ActiveTabSummaryClaimsRemoved = 6;

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
                case StatusProcessingSuccess:
                    return "Processing Success";
                case StatusProcessingFailed:
                    return "Processing Failed";
                case StatusSubmitForValidation:
                    return "Submit For Validation";
                case StatusValidating:
                    return "Validating";
                case StatusValidationSuccess:
                    return "Validation Success";
                case StatusValidationFailed:
                    return "Validation Failed";
                case StatusSubmitForRetroRecovery:
                    return "Submit For Retro Recovery";
                case StatusProcessingRetroRecovery:
                    return "Processing Retro Recovery";
                case StatusProcessingRetroRecoverySuccess:
                    return "Processing Retro Recovery Success";
                case StatusProcessingRetroRecoveryFailed:
                    return "Processing Retro Recovery Failed";
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
                case StatusProcessingSuccess:
                    return "status-success-badge";
                case StatusProcessingFailed:
                    return "status-fail-badge";
                case StatusSubmitForValidation:
                    return "status-submitprocess-badge";
                case StatusValidating:
                    return "status-processing-badge";
                case StatusValidationSuccess:
                    return "status-success-badge";
                case StatusValidationFailed:
                    return "status-fail-badge";
                case StatusSubmitForRetroRecovery:
                    return "status-submitprocess-badge";
                case StatusProcessingRetroRecovery:
                    return "status-processing-badge";
                case StatusProcessingRetroRecoverySuccess:
                    return "status-success-badge";
                case StatusProcessingRetroRecoveryFailed:
                    return "status-fail-badge";
                case StatusFinalised:
                    return "status-finalize-badge";
                default:
                    return "";
            }
        }
    }
}
