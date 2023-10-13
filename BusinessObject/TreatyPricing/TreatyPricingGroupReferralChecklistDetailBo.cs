using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralChecklistDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralVersionId { get; set; }
        public virtual TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }
        public TreatyPricingGroupReferralChecklistBo TreatyPricingGroupReferralChecklistBo { get; set; }

        public int InternalItem { get; set; }

        public bool Underwriting { get; set; }
        public bool Health { get; set; }
        public bool Claim { get; set; }
        public bool BD { get; set; }
        public bool CnR { get; set; }
        public int? UltimateApprover { get; set; }
        public bool GroupTeamApprover { get; set; }
        public bool ReviewerApprover { get; set; }
        public bool HODApprover { get; set; }
        public bool CEOApprover { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingGroupReferralChecklistDetailBo()
        {
            Underwriting = false;
            Health = false;
            Claim = false;
            BD = false;
            CnR = false;
            GroupTeamApprover = false;
            ReviewerApprover = false;
            HODApprover = false;
            CEOApprover = false;
        }

        public const int ItemInternalMargin = 1;
        public const int ItemFCL = 2;
        public const int ItemOverseasLife = 3;
        public const int ItemReferredRisk = 4;
        public const int ItemAbl = 5;
        public const int ItemMaxSaPerLife = 6;
        public const int ItemWaiverOfUw = 7;
        public const int ItemAgeLimit = 8;
        public const int ItemExceedTreatyParams = 9;
        public const int ItemHips = 10;
        public const int ItemGrossPrem = 11;
        public const int ItemAffinity = 12;
        public const int ItemOthers = 13;
        public const int ItemSanctionRisk = 14;
        public const int MaxItem = 14;

        public const int UltimateApproverGroup = 1;
        public const int UltimateApproverReviewer = 2;
        public const int UltimateApproverHOD = 3;
        public const int UltimateApproverCEO = 4;
        public const int MaxUltimateApprover = 4;

        public static string GetItemPovName(int key)
        {
            switch (key)
            {
                case ItemInternalMargin:
                    return "Internal Margin";
                case ItemFCL:
                    return "FCL";
                case ItemOverseasLife:
                    return "Overseas Life";
                case ItemReferredRisk:
                    return "Referred Risk";
                case ItemAbl:
                    return "ABL";
                case ItemMaxSaPerLife:
                    return "Max SA Per Life";
                case ItemWaiverOfUw:
                    return "Waiver of UW";
                case ItemAgeLimit:
                    return "Age Limit";
                case ItemExceedTreatyParams:
                    return "Exceed Treaty Parameter";
                case ItemHips:
                    return "HIPS";
                case ItemGrossPrem:
                    return "Gross Prem";
                case ItemAffinity:
                    return "Affinity Group";
                case ItemOthers:
                    return "Others";
                case ItemSanctionRisk:
                    return "Sanction Risk";
                default:
                    return "";
            }
        }

        public static List<int> GetItems()
        {
            return new List<int>
            {
                ItemInternalMargin,
                ItemFCL,
                ItemOverseasLife,
                ItemReferredRisk,
                ItemAbl,
                ItemMaxSaPerLife,
                ItemWaiverOfUw,
                ItemAgeLimit,
                ItemExceedTreatyParams,
                ItemHips,
                ItemGrossPrem,
                ItemAffinity,
                ItemOthers,
                ItemSanctionRisk,
            };
        }

        public static string GetUltimateApproverName(int key)
        {
            switch (key)
            {
                case UltimateApproverCEO:
                    return "VII - CEO";
                case UltimateApproverHOD:
                    return "III - HOD, T&P";
                case UltimateApproverReviewer:
                    return "II - Reviewer";
                case UltimateApproverGroup:
                    return "I - Group Team";
                default:
                    return "";
            }
        }

        public static List<TreatyPricingGroupReferralChecklistDetailBo> GetDefaultRow(int parentId)
        {
            List<TreatyPricingGroupReferralChecklistDetailBo> bos = new List<TreatyPricingGroupReferralChecklistDetailBo> { };
            foreach (var i in Enumerable.Range(1, MaxItem))
            {
                bos.Add(new TreatyPricingGroupReferralChecklistDetailBo
                {
                    TreatyPricingGroupReferralVersionId = parentId,
                    InternalItem = i,
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
