using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Controllers;

namespace WebApp.Models
{
    public class TreatyPricingProductViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }

        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, StringLength(60), DisplayName("Product ID")]
        public string Code { get; set; }

        [DisplayName("Product Name")]
        public string Name { get; set; }

        [DisplayName("Product Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Product Effective Date")]
        public string EffectiveDateStr { get; set; }

        [Required, DisplayName("Product Summary")]
        public string Summary { get; set; }

        [DisplayName("Quotation Name")]
        public string QuotationName { get; set; }

        [Required, DisplayName("Underwriting Method (Separated by comma)")]
        [ValidateMultiplePickListDetailCode(PickListId = PickListBo.UnderwritingMethod)]
        public string UnderwritingMethod { get; set; }

        [DisplayName("Per Life Retro")]
        public bool HasPerLifeRetro { get; set; }

        [DisplayName("Per Life Treaty Code (Separated by comma)")]
        public string PerLifeRetroTreatyCode { get; set; }

        [RequiredVersion, DisplayName("Person In-Charge (Business Development)")]
        public int PersonInChargeId { get; set; }

        [RequiredVersion, DisplayName("Product Type")]
        public int? ProductTypePickListDetailId { get; set; }

        [RequiredVersion, DisplayName("Target Segment (Separated by comma)")]
        [ValidateMultiplePickListDetailCode(PickListId = PickListBo.TargetSegment)]
        public string TargetSegment { get; set; }

        [RequiredVersion, DisplayName("Distribution Channel (Separated by comma)")]
        [ValidateMultiplePickListDetailCode(PickListId = PickListBo.DistributionChannel)]
        public string DistributionChannel { get; set; }

        [RequiredVersion, DisplayName("Cession Type (Separated by comma)")]
        [ValidateMultiplePickListDetailCode(PickListId = PickListBo.CessionType)]
        public string CessionType { get; set; }

        [RequiredVersion, DisplayName("Business Origin")]
        public int? BusinessOriginPickListDetailId { get; set; }

        [RequiredVersion, DisplayName("Business Type")]
        public int? BusinessTypePickListDetailId { get; set; }

        [RequiredVersion, DisplayName("Product Line (Separated by comma)")]
        [ValidateMultiplePickListDetailCode(PickListId = PickListBo.ProductLine)]
        public string ProductLine { get; set; }

        [RequiredVersion, DisplayName("Reinsurance Arrangement")]
        public int? ReinsuranceArrangementPickListDetailId { get; set; }

        [DisplayName("Expected Average Sum Assured"), StringLength(128)]
        public string ExpectedAverageSumAssured { get; set; }

        [DisplayName("Expected RI Premium"), StringLength(128)]
        public string ExpectedRiPremium { get; set; }

        [DisplayName("Expected Number of Policy"), StringLength(128)]
        public string ExpectedPolicyNo { get; set; }

        [DisplayName("Medical Table")]
        public string TreatyPricingMedicalTableSelect { get; set; }

        [DisplayName("Financial Table")]
        public string TreatyPricingFinancialTableSelect { get; set; }

        [DisplayName("Underwriting Questionnaire")]
        public string TreatyPricingUwQuestionnaireSelect { get; set; }

        [DisplayName("Advantage Program")]
        public string TreatyPricingAdvantageProgramSelect { get; set; }

        [DisplayName("Juvenile Lien")]
        public string JuvenileLien { get; set; }

        [DisplayName("Jumbo Limit"), StringLength(128)]
        public string JumboLimit { get; set; }

        [DisplayName("Special Lien")]
        public string SpecialLien { get; set; }

        [DisplayName("Additional Remark for Underwriting")]
        public string UnderwritingAdditionalRemark { get; set; }

        [DisplayName("Waiting Period"), StringLength(256)]
        public string WaitingPeriod { get; set; }

        [DisplayName("Survival Period"), StringLength(128)]
        public string SurvivalPeriod { get; set; }

        [DisplayName("Profit Commission")]
        public string TreatyPricingProfitCommissionSelect { get; set; }

        [DisplayName("Payment of Reinsurance Premium")]
        public int? ReinsurancePremiumPaymentPickListDetailId { get; set; }

        [DisplayName("Unearned Premium Refund")]
        public int? UnearnedPremiumRefundPickListDetailId { get; set; }

        [DisplayName("Termination Clause"), StringLength(128)]
        public string TerminationClause { get; set; }

        [DisplayName("Recapture Clause"), StringLength(256)]
        public string RecaptureClause { get; set; }

        //[DisplayName("Country of Residence")]
        //public string ResidenceCountry { get; set; }

        [DisplayName("Territory of Issue Code")]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        [DisplayName("Quarterly Risk Premium (For SRT Only)")]
        public string QuarterlyRiskPremium { get; set; }

        [DisplayName("Group Free Cover Limit (Non-CI)")]
        public string GroupFreeCoverLimitNonCi { get; set; }

        [DisplayName("Group Free Cover Limit Age (Non-CI)"), StringLength(128)]
        public string GroupFreeCoverLimitAgeNonCi { get; set; }

        [DisplayName("Group Free Cover Limit (CI)")]
        public string GroupFreeCoverLimitCi { get; set; }

        [DisplayName("Group Free Cover Limit Age (CI)"), StringLength(128)]
        public string GroupFreeCoverLimitAgeCi { get; set; }

        [DisplayName("Group Profit Commission")]
        public string GroupProfitCommission { get; set; }

        [DisplayName("Occupational Classification")]
        public string OccupationalClassification { get; set; }

        [DisplayName("Direct Retro")]
        public bool IsDirectRetro { get; set; }

        [DisplayName("Profit Commission"), StringLength(128)]
        public string DirectRetroProfitCommission { get; set; }

        [DisplayName("Termination Clause"), StringLength(128)]
        public string DirectRetroTerminationClause { get; set; }

        [DisplayName("Recapture Clause"), StringLength(128)]
        public string DirectRetroRecaptureClause { get; set; }

        [DisplayName("Retrocession's Quarterly Risk Premium (For SRT Only)"), StringLength(128)]
        public string DirectRetroQuarterlyRiskPremium { get; set; }

        [DisplayName("Inward Retro")]
        public bool IsInwardRetro { get; set; }

        [DisplayName("Profit Commission"), StringLength(128)]
        public string InwardRetroProfitCommission { get; set; }

        [DisplayName("Termination Clause"), StringLength(128)]
        public string InwardRetroTerminationClause { get; set; }

        [DisplayName("Recapture Clause"), StringLength(128)]
        public string InwardRetroRecaptureClause { get; set; }

        [DisplayName("Retrocession's Quarterly Risk Premium (For SRT Only)"), StringLength(128)]
        public string InwardRetroQuarterlyRiskPremium { get; set; }

        [DisplayName("Retakaful Service")]
        public bool IsRetakafulService { get; set; }

        [DisplayName("Investment Profit Sharing"), StringLength(128)]
        public string InvestmentProfitSharing { get; set; }

        [DisplayName("Retakaful Model"), StringLength(128)]
        public string RetakafulModel { get; set; }

        public string TreatyPricingProductBenefit { get; set; }

        public int WorkflowId { get; set; }

        [DisplayName("Target Send Date")]
        public string TargetSendDate { get; set; }

        [DisplayName("Latest Revision Date")]
        public string LatestRevisionDate { get; set; }

        [DisplayName("Quotation Status")]
        public string QuotationStatus { get; set; }

        [DisplayName("Quotation Status Remark")]
        public string QuotationStatusRemark { get; set; }

        public TreatyPricingProductViewModel()
        {
            Set();
        }

        public TreatyPricingProductViewModel(TreatyPricingProductBo productBo)
        {
            Set(productBo);
            SetVersionObjects(productBo.TreatyPricingProductVersionBos);

            PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingProductBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            TreatyPricingCedantId = bo.TreatyPricingCedantId;
            TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
            Code = bo.Code;
            Name = bo.Name;
            EffectiveDate = bo.EffectiveDate;
            EffectiveDateStr = bo.EffectiveDateStr;
            Summary = bo.Summary;
            QuotationName = bo.QuotationName;
            UnderwritingMethod = bo.UnderwritingMethod;
            HasPerLifeRetro = bo.HasPerLifeRetro;
            PerLifeRetroTreatyCode = bo.PerLifeRetroTreatyCode;
        }

        public TreatyPricingProductBo FormBo(TreatyPricingProductBo bo)
        {
            bo.Code = Code;
            bo.Name = Name;
            bo.EffectiveDate = Util.GetParseDateTime(EffectiveDateStr);
            bo.Summary = Summary;
            bo.QuotationName = QuotationName;
            bo.UnderwritingMethod = UnderwritingMethod;
            bo.HasPerLifeRetro = HasPerLifeRetro;
            bo.PerLifeRetroTreatyCode = PerLifeRetroTreatyCode;
            return bo;
        }

        public TreatyPricingProductVersionBo GetVersionBo(TreatyPricingProductVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeId = PersonInChargeId;
            bo.ProductTypePickListDetailId = ProductTypePickListDetailId;
            bo.TargetSegment = TargetSegment;
            bo.DistributionChannel = DistributionChannel;
            bo.CessionType = CessionType;
            bo.BusinessOriginPickListDetailId = BusinessOriginPickListDetailId;
            bo.BusinessTypePickListDetailId = BusinessTypePickListDetailId;
            bo.ProductLine = ProductLine;
            bo.ReinsuranceArrangementPickListDetailId = ReinsuranceArrangementPickListDetailId;
            bo.ExpectedAverageSumAssured = ExpectedAverageSumAssured;
            bo.ExpectedRiPremium = ExpectedRiPremium;
            bo.ExpectedPolicyNo = ExpectedPolicyNo;
            bo.TreatyPricingMedicalTableSelect = TreatyPricingMedicalTableSelect;
            bo.TreatyPricingFinancialTableSelect = TreatyPricingFinancialTableSelect;
            bo.TreatyPricingUwQuestionnaireSelect = TreatyPricingUwQuestionnaireSelect;
            bo.TreatyPricingAdvantageProgramSelect = TreatyPricingAdvantageProgramSelect;
            bo.JuvenileLien = JuvenileLien;
            bo.JumboLimit = JumboLimit;
            bo.SpecialLien = SpecialLien;
            bo.UnderwritingAdditionalRemark = UnderwritingAdditionalRemark;
            bo.WaitingPeriod = WaitingPeriod;
            bo.SurvivalPeriod = SurvivalPeriod;
            bo.TreatyPricingProfitCommissionSelect = TreatyPricingProfitCommissionSelect;
            bo.ReinsurancePremiumPaymentPickListDetailId = ReinsurancePremiumPaymentPickListDetailId;
            bo.UnearnedPremiumRefundPickListDetailId = UnearnedPremiumRefundPickListDetailId;
            bo.TerminationClause = TerminationClause;
            bo.RecaptureClause = RecaptureClause;
            bo.TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId;
            bo.QuarterlyRiskPremium = QuarterlyRiskPremium;
            bo.GroupFreeCoverLimitNonCi = GroupFreeCoverLimitNonCi;
            bo.GroupFreeCoverLimitAgeNonCi = GroupFreeCoverLimitAgeNonCi;
            bo.GroupFreeCoverLimitCi = GroupFreeCoverLimitCi;
            bo.GroupFreeCoverLimitAgeCi = GroupFreeCoverLimitAgeCi;
            bo.GroupProfitCommission = GroupProfitCommission;
            bo.OccupationalClassification = OccupationalClassification;
            bo.IsDirectRetro = IsDirectRetro;
            bo.DirectRetroProfitCommission = IsDirectRetro ? DirectRetroProfitCommission : null;
            bo.DirectRetroTerminationClause = IsDirectRetro ? DirectRetroTerminationClause : null;
            bo.DirectRetroRecaptureClause = IsDirectRetro ? DirectRetroRecaptureClause : null;
            bo.DirectRetroQuarterlyRiskPremium = IsDirectRetro ? DirectRetroQuarterlyRiskPremium : null;
            bo.IsInwardRetro = IsInwardRetro;
            bo.InwardRetroProfitCommission = IsInwardRetro ? InwardRetroProfitCommission : null;
            bo.InwardRetroTerminationClause = IsInwardRetro ? InwardRetroTerminationClause : null;
            bo.InwardRetroRecaptureClause = IsInwardRetro ? InwardRetroRecaptureClause : null;
            bo.InwardRetroQuarterlyRiskPremium = IsInwardRetro ? InwardRetroQuarterlyRiskPremium : null;
            bo.IsRetakafulService = IsRetakafulService;
            bo.InvestmentProfitSharing = IsRetakafulService ? InvestmentProfitSharing : null;
            bo.RetakafulModel = IsRetakafulService ? RetakafulModel : null;
            bo.TreatyPricingProductBenefit = TreatyPricingProductBenefit;

            return bo;
        }
    }
}