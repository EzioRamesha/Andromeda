using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralChecklistBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralVersionId { get; set; }
        public TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }

        public int InternalTeam { get; set; }

        public string InternalTeamPersonInCharge { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public bool DisableButtons { get; set; }

        public bool DisableRequest { get; set; }

        public bool DisablePersonInCharge { get; set; }

        public string StatusName { get; set; }

        public const int StatusNotRequired = 1;
        public const int StatusPendingReview = 2;
        public const int StatusCompleted = 3;
        public const int StatusPendingApproval = 4;
        public const int StatusApproved = 5;
        public const int StatusRejected = 6;

        public const int DefaultInternalTeamUnderwriting = 1;
        public const int DefaultInternalTeamHealth = 2;
        public const int DefaultInternalTeamClaims = 3;
        public const int DefaultInternalTeamBD = 4;
        public const int DefaultInternalTeamCR = 5;
        public const int DefaultInternalTeamApprover = 6;
        public const int DefaultInternalTeamGroup = 7;
        public const int DefaultInternalTeamReviewer = 8;
        public const int DefaultInternalTeamHOD = 9;
        public const int DefaultInternalTeamCEO = 10;
        public const int MaxDefaultInternalTeam = 10;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusNotRequired:
                    return "Not Required";
                case StatusPendingReview:
                    return "Pending Review";
                case StatusCompleted:
                    return "Completed";
                case StatusPendingApproval:
                    return "Pending Approval";
                case StatusApproved:
                    return "Approved";
                case StatusRejected:
                    return "Rejected";
                default:
                    return "";
            }
        }

        public static string GetDefaultInternalTeamName(int key)
        {
            switch (key)
            {
                case DefaultInternalTeamUnderwriting:
                    return "Underwriting";
                case DefaultInternalTeamHealth:
                    return "Health";
                case DefaultInternalTeamClaims:
                    return "Claim";
                case DefaultInternalTeamBD:
                    return "BD";
                case DefaultInternalTeamCR:
                    return "C&R";
                case DefaultInternalTeamApprover:
                    return "Ultimate Approver";
                case DefaultInternalTeamGroup:
                    return "I - Group Team";
                case DefaultInternalTeamReviewer:
                    return "II - Reviewer";
                case DefaultInternalTeamHOD:
                    return "III - HOD";
                case DefaultInternalTeamCEO:
                    return "IV - CEO";
                default:
                    return "";
            }
        }

        public static List<int> GetDefaultInternalTeams()
        {
            return new List<int>
            {
                DefaultInternalTeamUnderwriting,
                DefaultInternalTeamHealth,
                DefaultInternalTeamClaims,
                DefaultInternalTeamBD,
                DefaultInternalTeamCR,
                DefaultInternalTeamApprover,
                DefaultInternalTeamGroup,
                DefaultInternalTeamReviewer,
                DefaultInternalTeamHOD,
                DefaultInternalTeamCEO,
            };
        }

        public static List<TreatyPricingGroupReferralChecklistBo> GetDefaultRow(int parentId)
        {
            List<TreatyPricingGroupReferralChecklistBo> bos = new List<TreatyPricingGroupReferralChecklistBo> { };
            foreach (var i in Enumerable.Range(1, MaxDefaultInternalTeam))
            {
                bos.Add(new TreatyPricingGroupReferralChecklistBo
                {
                    TreatyPricingGroupReferralVersionId = parentId,
                    InternalTeam = i,
                    Status = StatusNotRequired,
                });
            }

            return bos;
        }

        public static string GetJsonDefaultRow(int parentId)
        {
            return JsonConvert.SerializeObject(GetDefaultRow(parentId));
        }
    }
}
