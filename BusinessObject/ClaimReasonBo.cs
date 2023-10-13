using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimReasonBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string Reason { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeClaimDeclinePending = 1;
        public const int TypeCedantReferral = 2;
        public const int TypeRetroReferral = 3;
        public const int TypeReferralDelay = 4;
        public const int MaxType = 4;

        public const string StaticReasonDuplicateClaim = "Duplicate Claim";

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeClaimDeclinePending:
                    return "Claim Decline / Pending";
                case TypeCedantReferral:
                    return "Cedants Referral";
                case TypeRetroReferral:
                    return "Retro Referral";
                case TypeReferralDelay:
                    return "Referral Delay";
                default:
                    return "";
            }
        }

        public static IList<ClaimReasonBo> GetClaimReasons()
        {
            return new List<ClaimReasonBo>()
            {
                GetStaticClaimReason(StaticReasonDuplicateClaim)
            };
        }

        public static ClaimReasonBo GetStaticClaimReason(string reason)
        {
            int type = 0;
            switch (reason)
            {
                case StaticReasonDuplicateClaim:
                    type = TypeClaimDeclinePending;
                    break;
                default:
                    break;
            }


            return new ClaimReasonBo()
            {
                Type = type,
                Reason = reason,
                Remark = ""
            };
        }
    }
}
