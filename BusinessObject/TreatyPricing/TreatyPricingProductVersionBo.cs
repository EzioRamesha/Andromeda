using BusinessObject.Identity;
using Shared;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingProductId { get; set; }

        public TreatyPricingProductBo TreatyPricingProductBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }

        public UserBo PersonInCharge { get; set; }

        public int? ProductTypePickListDetailId { get; set; }

        public PickListDetailBo ProductTypePickListDetailBo { get; set; }

        public string TargetSegment { get; set; }

        public string DistributionChannel { get; set; }

        public string CessionType { get; set; }

        public int? BusinessOriginPickListDetailId { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public int? BusinessTypePickListDetailId { get; set; }

        public PickListDetailBo BusinessTypePickListDetailBo { get; set; }

        public string ProductLine { get; set; }

        public int? ReinsuranceArrangementPickListDetailId { get; set; }

        public PickListDetailBo ReinsuranceArrangementPickListDetailBo { get; set; }

        public string ExpectedAverageSumAssured { get; set; }

        public string ExpectedRiPremium { get; set; }

        public string ExpectedPolicyNo { get; set; }

        public int? TreatyPricingMedicalTableId { get; set; }
        public TreatyPricingMedicalTableBo TreatyPricingMedicalTableBo { get; set; }

        public int? TreatyPricingMedicalTableVersionId { get; set; }
        public TreatyPricingMedicalTableVersionBo TreatyPricingMedicalTableVersionBo { get; set; }

        public string TreatyPricingMedicalTableSelect { get; set; }

        public int? TreatyPricingFinancialTableId { get; set; }
        public TreatyPricingFinancialTableBo TreatyPricingFinancialTableBo { get; set; }

        public int? TreatyPricingFinancialTableVersionId { get; set; }
        public TreatyPricingFinancialTableVersionBo TreatyPricingFinancialTableVersionBo { get; set; }

        public string TreatyPricingFinancialTableSelect { get; set; }

        public int? TreatyPricingUwQuestionnaireId { get; set; }
        public TreatyPricingUwQuestionnaireBo TreatyPricingUwQuestionnaireBo { get; set; }

        public int? TreatyPricingUwQuestionnaireVersionId { get; set; }
        public TreatyPricingUwQuestionnaireVersionBo TreatyPricingUwQuestionnaireVersionBo { get; set; }

        public string TreatyPricingUwQuestionnaireSelect { get; set; }

        public int? TreatyPricingAdvantageProgramId { get; set; }
        public TreatyPricingAdvantageProgramBo TreatyPricingAdvantageProgramBo { get; set; }

        public int? TreatyPricingAdvantageProgramVersionId { get; set; }
        public TreatyPricingAdvantageProgramVersionBo TreatyPricingAdvantageProgramVersionBo { get; set; }

        public string TreatyPricingAdvantageProgramSelect { get; set; }

        public string JuvenileLien { get; set; }

        public string JumboLimit { get; set; }

        public string SpecialLien { get; set; }

        public string UnderwritingAdditionalRemark { get; set; }

        public string WaitingPeriod { get; set; }

        public string SurvivalPeriod { get; set; }

        public int? TreatyPricingProfitCommissionId { get; set; }
        public TreatyPricingProfitCommissionBo TreatyPricingProfitCommissionBo { get; set; }

        public int? TreatyPricingProfitCommissionVersionId { get; set; }
        public TreatyPricingProfitCommissionVersionBo TreatyPricingProfitCommissionVersionBo { get; set; }

        public string TreatyPricingProfitCommissionSelect { get; set; }

        public int? ReinsurancePremiumPaymentPickListDetailId { get; set; }

        public PickListDetailBo ReinsurancePremiumPaymentPickListDetailBo { get; set; }

        public int? UnearnedPremiumRefundPickListDetailId { get; set; }

        public PickListDetailBo UnearnedPremiumRefundPickListDetailBo { get; set; }

        public string TerminationClause { get; set; }

        public string RecaptureClause { get; set; }

        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        public string QuarterlyRiskPremium { get; set; }

        public string GroupFreeCoverLimitNonCi { get; set; }

        public string GroupFreeCoverLimitAgeNonCi { get; set; }

        public string GroupFreeCoverLimitCi { get; set; }

        public string GroupFreeCoverLimitAgeCi { get; set; }

        public string GroupProfitCommission { get; set; }

        public string OccupationalClassification { get; set; }

        public bool IsDirectRetro { get; set; }

        public string DirectRetroProfitCommission { get; set; }

        public string DirectRetroTerminationClause { get; set; }

        public string DirectRetroRecaptureClause { get; set; }

        public string DirectRetroQuarterlyRiskPremium { get; set; }

        public bool IsInwardRetro { get; set; }

        public string InwardRetroProfitCommission { get; set; }

        public string InwardRetroTerminationClause { get; set; }

        public string InwardRetroRecaptureClause { get; set; }

        public string InwardRetroQuarterlyRiskPremium { get; set; }

        public bool IsRetakafulService { get; set; }

        public string InvestmentProfitSharing { get; set; }

        public string RetakafulModel { get; set; }

        [IsJsonProperty("BenefitId")]
        public string TreatyPricingProductBenefit { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        #region Product & Benefit Comparison
        public string IsDirectRetroStr { get; set; }

        public string IsInwardRetroStr { get; set; }

        public string IsRetakafulServiceStr { get; set; }

        public string ProductTypeStr { get; set; }

        public string BusinessOriginStr { get; set; }

        public string BusinessTypeStr { get; set; }

        public string ReinsuranceArrangementStr { get; set; }

        public string ReinsurancePremiumPaymentStr { get; set; }

        public string UnearnedPremiumRefundStr { get; set; }

        public string MedicalTableInfo { get; set; }

        public string FinancialTableInfo { get; set; }

        public string UwQuestionnaireInfo { get; set; }

        public string AdvantageProgramInfo { get; set; }

        public string ProfitCommInfo { get; set; }
        #endregion

        public TreatyPricingProductVersionBo()
        {
            IsDirectRetro = false;
            IsInwardRetro = false;
            IsRetakafulService = false;
        }

        public TreatyPricingProductVersionBo(TreatyPricingProductVersionBo bo)
        {
            TreatyPricingProductId = bo.TreatyPricingProductId;
            ProductTypePickListDetailId = bo.ProductTypePickListDetailId;
            TargetSegment = bo.TargetSegment;
            DistributionChannel = bo.DistributionChannel;
            CessionType = bo.CessionType;
            BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
            BusinessTypePickListDetailId = bo.BusinessTypePickListDetailId;
            ProductLine = bo.ProductLine;
            ReinsuranceArrangementPickListDetailId = bo.ReinsuranceArrangementPickListDetailId;
            ExpectedAverageSumAssured = bo.ExpectedAverageSumAssured;
            ExpectedRiPremium = bo.ExpectedRiPremium;
            ExpectedPolicyNo = bo.ExpectedPolicyNo;
            TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId;
            TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId;
            TreatyPricingMedicalTableSelect = bo.TreatyPricingMedicalTableSelect;
            TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId;
            TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId;
            TreatyPricingFinancialTableSelect = bo.TreatyPricingFinancialTableSelect;
            TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId;
            TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId;
            TreatyPricingUwQuestionnaireSelect = bo.TreatyPricingUwQuestionnaireSelect;
            TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId;
            TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId;
            TreatyPricingAdvantageProgramSelect = bo.TreatyPricingAdvantageProgramSelect;
            TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId;
            TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId;
            TreatyPricingProfitCommissionSelect = bo.TreatyPricingProfitCommissionSelect;
            JuvenileLien = bo.JuvenileLien;
            JumboLimit = bo.JumboLimit;
            SpecialLien = bo.SpecialLien;
            UnderwritingAdditionalRemark = bo.UnderwritingAdditionalRemark;
            WaitingPeriod = bo.WaitingPeriod;
            SurvivalPeriod = bo.SurvivalPeriod;
            ReinsurancePremiumPaymentPickListDetailId = bo.ReinsurancePremiumPaymentPickListDetailId;
            UnearnedPremiumRefundPickListDetailId = bo.UnearnedPremiumRefundPickListDetailId;
            TerminationClause = bo.TerminationClause;
            RecaptureClause = bo.RecaptureClause;
            TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
            QuarterlyRiskPremium = bo.QuarterlyRiskPremium;
            GroupFreeCoverLimitNonCi = bo.GroupFreeCoverLimitNonCi;
            GroupFreeCoverLimitAgeNonCi = bo.GroupFreeCoverLimitAgeNonCi;
            GroupFreeCoverLimitCi = bo.GroupFreeCoverLimitCi;
            GroupFreeCoverLimitAgeCi = bo.GroupFreeCoverLimitAgeCi;
            GroupProfitCommission = bo.GroupProfitCommission;
            IsDirectRetro = bo.IsDirectRetro;
            DirectRetroProfitCommission = bo.DirectRetroProfitCommission;
            DirectRetroTerminationClause = bo.DirectRetroTerminationClause;
            DirectRetroRecaptureClause = bo.DirectRetroRecaptureClause;
            DirectRetroQuarterlyRiskPremium = bo.DirectRetroQuarterlyRiskPremium;
            IsInwardRetro = bo.IsInwardRetro;
            InwardRetroProfitCommission = bo.InwardRetroProfitCommission;
            InwardRetroTerminationClause = bo.InwardRetroTerminationClause;
            InwardRetroRecaptureClause = bo.InwardRetroRecaptureClause;
            InwardRetroQuarterlyRiskPremium = bo.InwardRetroQuarterlyRiskPremium;
            IsRetakafulService = bo.IsRetakafulService;
            InvestmentProfitSharing = bo.InvestmentProfitSharing;
            RetakafulModel = bo.RetakafulModel;
            TreatyPricingProductBenefit = bo.TreatyPricingProductBenefit;
        }

        public void SetSelectValues()
        {
            SetSelectValue("TreatyPricingMedicalTable");
            SetSelectValue("TreatyPricingFinancialTable");
            SetSelectValue("TreatyPricingUwQuestionnaire");
            SetSelectValue("TreatyPricingAdvantageProgram");
            SetSelectValue("TreatyPricingProfitCommission");
            SetSelectValue("TreatyPricingClaimApprovalLimit");
            SetSelectValue("TreatyPricingDefinitionAndExclusion");
            SetSelectValue("TreatyPricingCustomOther");
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
    }
}
