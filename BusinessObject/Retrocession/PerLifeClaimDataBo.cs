using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeClaimDataBo
    {
        public int Id { get; set; }

        public int PerLifeClaimId { get; set; }

        public PerLifeClaimBo PerLifeClaimBo { get; set; }

        public int ClaimRegisterHistoryId { get; set; }

        public ClaimRegisterHistoryBo ClaimRegisterHistoryBo { get; set; }

        public int? PerLifeAggregationDetailDataId { get; set; }

        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }

        public bool IsException { get; set; }

        public int? ClaimCategory { get; set; }

        public string ClaimCategoryStr { get; set; }

        public bool IsExcludePerformClaimRecovery { get; set; }

        public int? ClaimRecoveryStatus { get; set; }

        public string ClaimRecoveryStatusStr { get; set; } 

        public string ClaimRecoveryStatusRecoveredStr { get; set; }

        public int? ClaimRecoveryDecision { get; set; }

        public string ClaimRecoveryDecisionStr { get; set; }

        public int? MovementType { get; set; }

        public string PerLifeRetro { get; set; }

        public int? RetroOutputId { get; set; }

        public int? RetainPoolId { get; set; }

        public int? NoOfRetroTreaty { get; set; }

        public int? RetroRecoveryId { get; set; }

        public bool IsLateInterestShare { get; set; }

        public bool IsExGratiaShare { get; set; }

        public string Errors { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public int ClaimRegisterOffsetStatus { get; set; }

        public string ClaimRegisterOffsetStatusStr { get; set; }

        public const int ClaimCategoryPaidClaim = 1;
        public const int ClaimCategoryPendingClaim = 2;
        public const int ClaimCategoryReversed = 3;
        public const int ClaimCategoryRetainClaim = 4;

        public const int ClaimCategoryStatusMax = 4;

        public const int ClaimRecoveryStatusPending = 1;
        public const int ClaimRecoveryStatusSubmitForProcessing = 2;
        public const int ClaimRecoveryStatusProcessing = 3;
        public const int ClaimRecoveryStatusProcessingSuccess = 4;
        public const int ClaimRecoveryStatusProcessingFailed = 5;

        public const int ClaimRecoveryStatusMax = 5;

        public const int ClaimRecoveryDecisionPending = 1;
        public const int ClaimRecoveryDecisionPaid = 2;
        public const int ClaimRecoveryDecisionRemoved = 3;

        public const int ClaimRecoveryDecisionMax = 3;


        public static string GetClaimRetroDataRecovered(int category, string soaQuarter)
        {
            if (category == ClaimCategoryRetainClaim)
            {
                return "Won't Recover";
            }
            else if (category == ClaimCategoryPaidClaim)
            {
                return soaQuarter;
            }
            else if (category == ClaimCategoryReversed)
            {
                return soaQuarter;
            }
            else 
            {
                return "Won't Recover";
            }
        }

        public static string GetClaimCategoryName(int? key)
        {
            switch (key)
            {
                case ClaimCategoryPaidClaim:
                    return "Paid Claim";
                case ClaimCategoryPendingClaim:
                    return "Pending Claim";
                case ClaimCategoryReversed:
                    return "Reversed";
                case ClaimCategoryRetainClaim:
                    return "Retain Claim";
                default:
                    return "";
            }
        }

        public static string GetClaimRecoveryStatusName(int? key)
        {
            switch (key)
            {
                case ClaimRecoveryStatusPending:
                    return "Pending";
                case ClaimRecoveryStatusSubmitForProcessing:
                    return "Submit for Processing";
                case ClaimRecoveryStatusProcessing:
                    return "Processing";
                case ClaimRecoveryStatusProcessingSuccess:
                    return "Processing Success";
                case ClaimRecoveryStatusProcessingFailed:
                    return "Processing Failed";
                default:
                    return "";
            }
        }

        public static string GetClaimRecoveryDecisionName(int? key)
        {
            switch (key)
            {
                case ClaimRecoveryDecisionPending:
                    return "Pending";
                case ClaimRecoveryDecisionPaid:
                    return "Paid";
                case ClaimRecoveryDecisionRemoved:
                    return "Removed";
                default:
                    return "";
            }
        }
    }
}
