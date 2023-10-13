using Shared;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralVersionBenefitBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralVersionId { get; set; }
        public virtual TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }
        public  TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        [DisplayName("Benefit")]
        public int BenefitId { get; set; }
        public BenefitBo BenefitBo { get; set; }
        public string BenefitCode { get; set; }

        [DisplayName("RI Arrangement")]
        public int? ReinsuranceArrangementPickListDetailId { get; set; }
        public PickListDetailBo ReinsuranceArrangementPickListDetailBo { get; set; }

        public int? AgeBasisPickListDetailId { get; set; }
        public PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        public string MinimumEntryAge { get; set; }

        public string MaximumEntryAge { get; set; }

        public string MaximumExpiryAge { get; set; }

        public int? TreatyPricingUwLimitId { get; set; }
        public TreatyPricingUwLimitBo TreatyPricingUwLimitBo { get; set; }
        public int? TreatyPricingUwLimitVersionId { get; set; }
        public TreatyPricingUwLimitVersionBo TreatyPricingUwLimitVersionBo { get; set; }
        public string TreatyPricingUwLimitSelect { get; set; }

        public string GroupFreeCoverLimitNonCI { get; set; }

        public string RequestedFreeCoverLimitNonCI { get; set; }

        public string GroupFreeCoverLimitCI { get; set; }

        public string RequestedFreeCoverLimitCI { get; set; }

        public string GroupFreeCoverLimitAgeNonCI { get; set; }

        public string GroupFreeCoverLimitAgeCI { get; set; }

        [DisplayName("Other Special RI Arrangement")]
        public int? OtherSpecialReinsuranceArrangement { get; set; }
        public string OtherSpecialReinsurnaceArrangementStr { get; set; }

        public string OtherSpecialTerms { get; set; }

        [DisplayName("Profit Margin")]
        public string ProfitMargin { get; set; }

        [DisplayName("Expense Margin")]
        public string ExpenseMargin { get; set; }

        [DisplayName("Commission Margin")]
        public string CommissionMargin { get; set; }

        public string ProfitCommissionLoading { get; set; }

        public string AdditionalLoading { get; set; }

        public string CoinsuranceRiDiscount { get; set; }

        public string CoinsuranceCedantRetention { get; set; }

        public string CoinsuranceReinsurerShare { get; set; }

        public bool HasProfitCommission { get; set; }

        public bool HasGroupProfitCommission { get; set; }
        public string HasGroupProfitCommissionStr { get; set; }

        public bool IsOverwriteGroupProfitCommission { get; set; }
        public string IsOverwriteGroupProfitCommissionStr { get; set; }
        public string OverwriteGroupProfitCommissionRemarks { get; set; }

        public string GroupProfitCommission { get; set; }

        public string AdditionalLoadingYRTLayer { get; set; }

        public string RiDiscount { get; set; }

        public string CedantRetention { get; set; }

        public string ReinsuranceShare { get; set; }

        public string TabarruLoading { get; set; }

        //display on comparison only
        public string AutoBindingLimit { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int OtherSpecialReinsuranceArrangementNormal = 1;
        public const int OtherSpecialReinsuranceArrangementMaxisPooling = 2;
        public const int OtherSpecialReinsuranceArrangementMax = 2;

        public static string GetOtherSpecialReinsuranceArrangementName(int? key)
        {
            switch (key)
            {
                case OtherSpecialReinsuranceArrangementNormal:
                    return "Normal";
                case OtherSpecialReinsuranceArrangementMaxisPooling:
                    return "Maxis Pooling";
                default:
                    return "";
            }
        }

        public void SetSelectValues()
        {
            SetSelectValue("TreatyPricingUwLimit");
        }

        public void SetSelectValue(string property)
        {
            string objectProperty = property + "Id";
            string versionProperty = property + "VersionId";
            string selectProperty = property + "Select";

            this.SetPropertyValue(objectProperty, null);
            this.SetPropertyValue(versionProperty, null);

            object value = this.GetPropertyValue(selectProperty);
            if (value == null)
                return;

            string[] values = value.ToString().Split('|');
            if (values.Length == 2)
            {
                int? versionId = Util.GetParseInt(values[0]);
                int? objectId = Util.GetParseInt(values[1]);

                if (versionId.HasValue && objectId.HasValue)
                {
                    this.SetPropertyValue(objectProperty, objectId);
                    this.SetPropertyValue(versionProperty, versionId);
                }
            }
        }

        public static List<string> GetRequiredProperties()
        {

            return new List<string>()
            {
                "BenefitId",
                "ReinsuranceArrangementPickListDetailId",
                "OtherSpecialReinsuranceArrangement",
                "ProfitMargin",
                "ExpenseMargin",
                "CommissionMargin",
            };
        }
    }
}
