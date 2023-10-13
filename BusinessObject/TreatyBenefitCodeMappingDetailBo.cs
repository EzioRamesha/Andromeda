using System;

namespace BusinessObject
{
    public class TreatyBenefitCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int TreatyBenefitCodeMappingId { get; set; }

        public virtual TreatyBenefitCodeMappingBo TreatyBenefitCodeMappingBo { get; set; }

        public string Combination { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CampaignCode { get; set; }

        public TreatyBenefitCodeMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }

        public const int CombinationTypeTreaty = 1;
        public const int CombinationTypeBenefit = 2;
        public const int CombinationTypeProductFeature = 3;

        public static string GetCombinationTypeName(int key)
        {
            switch (key)
            {
                case CombinationTypeTreaty:
                    return "Treaty";
                case CombinationTypeBenefit:
                    return "Benefit";
                case CombinationTypeProductFeature:
                    return "Product Feature";
                default:
                    return "";
            }
        }
    }
}
