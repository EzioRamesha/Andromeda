using Shared;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductBenefitBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int TreatyPricingProductVersionId { get; set; }
        public virtual TreatyPricingProductVersionBo TreatyPricingProductVersionBo { get; set; }

        [DisplayName("Benefit")]
        public int BenefitId { get; set; }
        public virtual BenefitBo BenefitBo { get; set; }
        public string BenefitCode { get; set; }

        [MaxLength(255), DisplayName("Benefit Marketing Name")]
        public string Name { get; set; }

        [DisplayName("Basic / Rider")]
        public int? BasicRiderPickListDetailId { get; set; }
        public virtual PickListDetailBo BasicRiderPickListDetailBo { get; set; }

        // General Tab

        [DisplayName("Payout Type")]
        public int? PayoutTypePickListDetailId { get; set; }
        public virtual PickListDetailBo PayoutTypePickListDetailBo { get; set; }

        [MaxLength(128), DisplayName("Rider Attachment Ratio")]
        public string RiderAttachmentRatio { get; set; }

        [DisplayName("Age Basis")]
        public int? AgeBasisPickListDetailId { get; set; }
        public virtual PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        [MaxLength(128), DisplayName("Minimum Entry Age")]
        public string MinimumEntryAge { get; set; }

        [MaxLength(128), DisplayName("Maximum Entry Age")]
        public string MaximumEntryAge { get; set; }

        [MaxLength(128), DisplayName("Maximum Expiry Age")]
        public string MaximumExpiryAge { get; set; }

        [MaxLength(128), DisplayName("Minimum Policy Term")]
        public string MinimumPolicyTerm { get; set; }

        [MaxLength(128), DisplayName("Maximum Policy Term")]
        public string MaximumPolicyTerm { get; set; }

        [MaxLength(128), DisplayName("Premium Paying Term")]
        public string PremiumPayingTerm { get; set; }

        [MaxLength(256), DisplayName("Minimum Sum Assured")]
        public string MinimumSumAssured { get; set; }

        [MaxLength(256), DisplayName("Maximum Sum Assured")]
        public string MaximumSumAssured { get; set; }

        public int? TreatyPricingUwLimitId { get; set; }
        public TreatyPricingUwLimitBo TreatyPricingUwLimitBo { get; set; }

        public int? TreatyPricingUwLimitVersionId { get; set; }
        public TreatyPricingUwLimitVersionBo TreatyPricingUwLimitVersionBo { get; set; }

        public string TreatyPricingUwLimitSelect { get; set; } // For select only

        [MaxLength(128), DisplayName("Others")]
        public string Others { get; set; }

        public int? TreatyPricingClaimApprovalLimitId { get; set; }
        public TreatyPricingClaimApprovalLimitBo TreatyPricingClaimApprovalLimitBo { get; set; }

        public int? TreatyPricingClaimApprovalLimitVersionId { get; set; }
        public TreatyPricingClaimApprovalLimitVersionBo TreatyPricingClaimApprovalLimitVersionBo { get; set; }

        public string TreatyPricingClaimApprovalLimitSelect { get; set; } // For select only

        [MaxLength(128), DisplayName("If Others")]
        public string IfOthers1 { get; set; }

        public int? TreatyPricingDefinitionAndExclusionId { get; set; }
        public TreatyPricingDefinitionAndExclusionBo TreatyPricingDefinitionAndExclusionBo { get; set; }

        public int? TreatyPricingDefinitionAndExclusionVersionId { get; set; }
        public TreatyPricingDefinitionAndExclusionVersionBo TreatyPricingDefinitionAndExclusionVersionBo { get; set; }

        public string TreatyPricingDefinitionAndExclusionSelect { get; set; } // For select only

        [MaxLength(128), DisplayName("If Others")]
        public string IfOthers2 { get; set; }

        [MaxLength(128), DisplayName("Refund of Premium")]
        public string RefundOfPremium { get; set; }

        [MaxLength(128), DisplayName("Pre-Existing Conditions")]
        public string PreExistingCondition { get; set; }

        // Pricing Tab

        [DisplayName("Reinsurance Type of Arrangement")]
        public int? PricingArrangementReinsuranceTypePickListDetailId { get; set; }

        public PickListDetailBo PricingArrangementReinsuranceTypePickListDetailBo { get; set; }

        [MaxLength(256), DisplayName("Benefit Payout")]
        public string BenefitPayout { get; set; }

        [MaxLength(256), DisplayName("Cedant's Retention")]
        public string CedantRetention { get; set; }

        [MaxLength(256), DisplayName("Reinsurance's Share")]
        public string ReinsuranceShare { get; set; }

        [MaxLength(128), DisplayName("Coinsurance Cedant's Retention")]
        public string CoinsuranceCedantRetention { get; set; }

        [MaxLength(128), DisplayName("Coinsurance Reinsurance's Share")]
        public string CoinsuranceReinsuranceShare { get; set; }

        [MaxLength(128), DisplayName("Coinsurance RI Discount")]
        public string RequestedCoinsuranceRiDiscount { get; set; }

        [MaxLength(128), DisplayName("Profit Margin")]
        public string ProfitMargin { get; set; }

        [MaxLength(128), DisplayName("Expense Margin")]
        public string ExpenseMargin { get; set; }

        [MaxLength(128), DisplayName("Commission Margin")]
        public string CommissionMargin { get; set; }

        [MaxLength(128), DisplayName("Group Profit Commission Loading")]
        public string GroupProfitCommissionLoading { get; set; }

        [MaxLength(128), DisplayName("Tabarru Loading")]
        public string TabarruLoading { get; set; }

        [DisplayName("Sum at Risk Pattern")]
        public int? RiskPatternSumPickListDetailId { get; set; }

        public PickListDetailBo RiskPatternSumPickListDetailBo { get; set; }

        public string PricingUploadFileName { get; set; }

        public string PricingUploadHashFileName { get; set; }

        public bool CanDownloadFile { get; set; }

        public bool IsProfitCommission { get; set; }

        public bool IsAdvantageProgram { get; set; }

        public int? TreatyPricingRateTableId { get; set; }

        public TreatyPricingRateTableBo TreatyPricingRateTableBo { get; set; }

        public int? TreatyPricingRateTableVersionId { get; set; }

        public TreatyPricingRateTableVersionBo TreatyPricingRateTableVersionBo { get; set; }

        public string TreatyPricingRateTableSelect { get; set; } // For select only

        [MaxLength(128), DisplayName("Requested Rate Guarantee")]
        public string RequestedRateGuarantee { get; set; }

        [MaxLength(128), DisplayName("Requested Reinsurance Discount")]
        public string RequestedReinsuranceDiscount { get; set; }

        // Direct Retro Tab

        [IsJsonProperty("RetroPartyId")]
        public IList<TreatyPricingProductBenefitDirectRetroBo> TreatyPricingProductBenefitDirectRetroBos { get; set; }

        // Inward Retro Tab

        [MaxLength(128), DisplayName("Inward Retro Party")]
        public string InwardRetroParty { get; set; }

        public int? InwardRetroArrangementReinsuranceTypePickListDetailId { get; set; }

        public virtual PickListDetailBo InwardRetroArrangementReinsuranceTypePickListDetailBo { get; set; }

        [MaxLength(128), DisplayName("Inward Retro's Retention")]
        public string InwardRetroRetention { get; set; }

        [MaxLength(128), DisplayName("MLRe's Share")]
        public string MlreShare { get; set; }

        public bool IsRetrocessionProfitCommission { get; set; }

        public bool IsRetrocessionAdvantageProgram { get; set; }

        [MaxLength(128), DisplayName("Retrocession Rate Table")]
        public string RetrocessionRateTable { get; set; }

        [MaxLength(128), DisplayName("Rate Guarantee for New Business")]
        public string NewBusinessRateGuarantee { get; set; }

        [MaxLength(128), DisplayName("Rate Guarantee for Renewal Business")]
        public string RenewalBusinessRateGuarantee { get; set; }

        [MaxLength(128), DisplayName("Retrocession Discount")]
        public string RetrocessionDiscount { get; set; }

        [MaxLength(128), DisplayName("Additional Discount")]
        public string AdditionalDiscount { get; set; }

        [MaxLength(128), DisplayName("Additional Loading")]
        public string AdditionalLoading { get; set; }

        // Retakaful Service Tab

        [MaxLength(128), DisplayName("Wakalah Fee")]
        public string WakalahFee { get; set; }

        [MaxLength(128), DisplayName("MLRe's Service Fee")]
        public string MlreServiceFee { get; set; }

        // Others

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        #region Product & Benefit Comparison
        //General
        public string BasicRider { get; set; }

        public string PayoutType { get; set; }

        public string AgeBasis { get; set; }

        public string UwLimitInfo { get; set; }

        public string ClaimApprovalLimitInfo { get; set; }

        public string DefinitionExclusionInfo { get; set; }

        //Pricing
        public string PricingArrangementReinsuranceType { get; set; }

        public string RiskPatternSum { get; set; }

        public string IsProfitCommissionStr { get; set; }

        public string IsAdvantageProgramStr { get; set; }

        public string RateTableInfo { get; set; }

        //Direct Retro
        public TreatyPricingProductBenefitDirectRetroBo DirectRetroBo { get; set; }

        //Inward Retro
        public string InwardRetroArrangementReinsuranceType { get; set; }

        public string IsRetrocessionProfitCommissionStr { get; set; }

        public string IsRetrocessionAdvantageProgramStr { get; set; }
        #endregion

        public void SetSelectValues()
        {
            SetSelectValue("TreatyPricingUwLimit");
            SetSelectValue("TreatyPricingClaimApprovalLimit");
            SetSelectValue("TreatyPricingRateTable");
            SetSelectValue("TreatyPricingDefinitionAndExclusion");
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
                "Name",
                "BasicRiderPickListDetailId",
                "PayoutTypePickListDetailId",
                "AgeBasisPickListDetailId",
                "MinimumEntryAge",
                "MaximumEntryAge",
                "MaximumExpiryAge",
                "MinimumSumAssured",
                "MaximumSumAssured",
                "PricingArrangementReinsuranceTypePickListDetailId",
                "BenefitPayout",
                "RiskPatternSumPickListDetailId",
            };
        }

        public static List<string> GetMaxLengthStringProperties()
        {

            return new List<string>()
            {
                "Name",
                "RiderAttachmentRatio",
                "MinimumEntryAge",
                "MaximumEntryAge",
                "MaximumExpiryAge",
                "MinimumPolicyTerm",
                "MaximumPolicyTerm",
                "PremiumPayingTerm",
                "MinimumSumAssured",
                "MaximumSumAssured",
                "Others",
                "IfOthers1",
                "IfOthers2",
                "RefundOfPremium",
                "PreExistingCondition",
                "BenefitPayout",
                "CedantRetention",
                "ReinsuranceShare",
                "CoinsuranceCedantRetention",
                "CoinsuranceReinsuranceShare",
                "RequestedCoinsuranceRiDiscount",
                "ProfitMargin",
                "ExpenseMargin",
                "CommissionMargin",
                "GroupProfitCommissionLoading",
                "TabarruLoading",
                "RequestedRateGuarantee",
                "RequestedReinsuranceDiscount",
                "InwardRetroParty",
                "InwardRetroRetention",
                "MlreShare",
                "RetrocessionRateTable",
                "NewBusinessRateGuarantee",
                "RenewalBusinessRateGuarantee",
                "RetrocessionDiscount",
                "AdditionalDiscount",
                "AdditionalLoading",
                "WakalahFee",
                "MlreServiceFee",
            };
        }

        public static string GetTempFolderPath(string subFolder = null)
        {
            return Util.GetTemporaryPath(subFolder);
        }

        public string GetTempPath(string subFolder = null)
        {
            return Path.Combine(GetTempFolderPath(subFolder), PricingUploadHashFileName);
        }

        public string GetLocalPath()
        {
            return string.Format("{0}/{1}", Util.GetUploadPath("TreatyPricingProductBenefitPricing"), PricingUploadHashFileName);
        }
    }
}
