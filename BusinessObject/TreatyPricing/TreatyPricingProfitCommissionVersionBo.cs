using BusinessObject.Identity;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProfitCommissionVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingProfitCommissionId { get; set; }

        public TreatyPricingProfitCommissionBo TreatyPricingProfitCommissionBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public int? ProfitSharing { get; set; }

        public string ProfitDescription { get; set; }

        public double? NetProfitPercentage { get; set; }

        public string NetProfitPercentageStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        [IsJsonProperty("SortIndex")]
        public string ProfitCommissionDetail { get; set; }

        public string TierProfitCommission { get; set; }

        public TreatyPricingProfitCommissionVersionBo() { }

        public TreatyPricingProfitCommissionVersionBo(TreatyPricingProfitCommissionVersionBo bo)
        {
            TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            ProfitSharing = bo.ProfitSharing;
            ProfitDescription = bo.ProfitDescription;
            NetProfitPercentage = bo.NetProfitPercentage;
            NetProfitPercentageStr = bo.NetProfitPercentageStr;
            TierProfitCommission = bo.TierProfitCommission;
            ProfitCommissionDetail = bo.ProfitCommissionDetail;
        }

        public const int ProfitSharingFlat = 1;
        public const int ProfitSharingTier = 2;

        public const int ProfitSharingMax = 2;

        public static string GetProfitSharingName(int key)
        {
            switch (key)
            {
                case ProfitSharingFlat:
                    return "Flat Profit Commission";
                case ProfitSharingTier:
                    return "Tier Profit Commission";
                default:
                    return "";
            }
        }
    }
}
