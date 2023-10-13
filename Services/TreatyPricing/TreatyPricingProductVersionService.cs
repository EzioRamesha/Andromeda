using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TreatyPricingProductVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProductVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingProductVersion.ToString()
            };
        }

        public static TreatyPricingProductVersionBo FormBo(TreatyPricingProductVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            string medicalTableSelect = null;
            if (entity.TreatyPricingMedicalTableId.HasValue && entity.TreatyPricingMedicalTableVersionId.HasValue)
            {
                medicalTableSelect = string.Format("{0}|{1}", entity.TreatyPricingMedicalTableVersionId, entity.TreatyPricingMedicalTableId);
            }

            string financialTableSelect = null;
            if (entity.TreatyPricingFinancialTableId.HasValue && entity.TreatyPricingFinancialTableVersionId.HasValue)
            {
                financialTableSelect = string.Format("{0}|{1}", entity.TreatyPricingFinancialTableVersionId, entity.TreatyPricingFinancialTableId);
            }

            string uwQuestionnaireSelect = null;
            if (entity.TreatyPricingUwQuestionnaireId.HasValue && entity.TreatyPricingUwQuestionnaireVersionId.HasValue)
            {
                uwQuestionnaireSelect = string.Format("{0}|{1}", entity.TreatyPricingUwQuestionnaireVersionId, entity.TreatyPricingUwQuestionnaireId);
            }

            string advantageProgramSelect = null;
            if (entity.TreatyPricingAdvantageProgramId.HasValue && entity.TreatyPricingAdvantageProgramVersionId.HasValue)
            {
                advantageProgramSelect = string.Format("{0}|{1}", entity.TreatyPricingAdvantageProgramVersionId, entity.TreatyPricingAdvantageProgramId);
            }

            string profitCommissionSelect = null;
            if (entity.TreatyPricingProfitCommissionId.HasValue && entity.TreatyPricingProfitCommissionVersionId.HasValue)
            {
                profitCommissionSelect = string.Format("{0}|{1}", entity.TreatyPricingProfitCommissionVersionId, entity.TreatyPricingProfitCommissionId);
            }


            return new TreatyPricingProductVersionBo
            {
                Id = entity.Id,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                TreatyPricingProductBo = foreign ? TreatyPricingProductService.Find(entity.TreatyPricingProductId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                ProductTypePickListDetailId = entity.ProductTypePickListDetailId,
                ProductTypePickListDetailBo = foreign ? PickListDetailService.Find(entity.ProductTypePickListDetailId) : null,
                TargetSegment = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.TargetSegment),
                DistributionChannel = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.DistributionChannel),
                CessionType = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.CessionType),
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetailBo = foreign ? PickListDetailService.Find(entity.BusinessOriginPickListDetailId) : null,
                BusinessTypePickListDetailId = entity.BusinessTypePickListDetailId,
                BusinessTypePickListDetailBo = foreign ? PickListDetailService.Find(entity.BusinessTypePickListDetailId) : null,
                ProductLine = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.ProductLine),
                ReinsuranceArrangementPickListDetailId = entity.ReinsuranceArrangementPickListDetailId,
                ReinsuranceArrangementPickListDetailBo = foreign ? PickListDetailService.Find(entity.ReinsuranceArrangementPickListDetailId) : null,
                ExpectedAverageSumAssured = entity.ExpectedAverageSumAssured,
                ExpectedRiPremium = entity.ExpectedRiPremium,
                ExpectedPolicyNo = entity.ExpectedPolicyNo,
                TreatyPricingMedicalTableId = entity.TreatyPricingMedicalTableId,
                TreatyPricingMedicalTableBo = entity.TreatyPricingMedicalTableId.HasValue ? (foreign ? TreatyPricingMedicalTableService.Find(entity.TreatyPricingMedicalTableId.Value) : null) : null,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingMedicalTableVersionBo = entity.TreatyPricingMedicalTableVersionId.HasValue ? (foreign ? TreatyPricingMedicalTableVersionService.Find(entity.TreatyPricingMedicalTableVersionId.Value) : null) : null,
                TreatyPricingMedicalTableSelect = medicalTableSelect,
                TreatyPricingFinancialTableId = entity.TreatyPricingFinancialTableId,
                TreatyPricingFinancialTableBo = entity.TreatyPricingFinancialTableId.HasValue ? (foreign ? TreatyPricingFinancialTableService.Find(entity.TreatyPricingFinancialTableId.Value) : null) : null,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingFinancialTableVersionBo = entity.TreatyPricingFinancialTableVersionId.HasValue ? (foreign ? TreatyPricingFinancialTableVersionService.Find(entity.TreatyPricingFinancialTableVersionId.Value) : null) : null,
                TreatyPricingFinancialTableSelect = financialTableSelect,
                TreatyPricingUwQuestionnaireId = entity.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireBo = entity.TreatyPricingUwQuestionnaireId.HasValue ? (foreign ? TreatyPricingUwQuestionnaireService.Find(entity.TreatyPricingUwQuestionnaireId.Value) : null) : null,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingUwQuestionnaireVersionBo = entity.TreatyPricingUwQuestionnaireVersionId.HasValue ? (foreign ? TreatyPricingUwQuestionnaireVersionService.Find(entity.TreatyPricingUwQuestionnaireVersionId.Value) : null) : null,
                TreatyPricingUwQuestionnaireSelect = uwQuestionnaireSelect,
                TreatyPricingAdvantageProgramId = entity.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramBo = entity.TreatyPricingAdvantageProgramId.HasValue ? (foreign ? TreatyPricingAdvantageProgramService.Find(entity.TreatyPricingAdvantageProgramId.Value) : null) : null,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                TreatyPricingAdvantageProgramVersionBo = entity.TreatyPricingAdvantageProgramVersionId.HasValue ? (foreign ? TreatyPricingAdvantageProgramVersionService.Find(entity.TreatyPricingAdvantageProgramVersionId.Value) : null) : null,
                TreatyPricingAdvantageProgramSelect = advantageProgramSelect,
                JuvenileLien = TreatyPricingProductDetailService.GetJsonByParentType(entity.Id, TreatyPricingProductDetailBo.TypeJuvenileLien),
                JumboLimit = entity.JumboLimit,
                SpecialLien = TreatyPricingProductDetailService.GetJsonByParentType(entity.Id, TreatyPricingProductDetailBo.TypeSpecialLien),
                UnderwritingAdditionalRemark = entity.UnderwritingAdditionalRemark,
                WaitingPeriod = entity.WaitingPeriod,
                SurvivalPeriod = entity.SurvivalPeriod,
                TreatyPricingProfitCommissionId = entity.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionBo = entity.TreatyPricingProfitCommissionId.HasValue ? (foreign ? TreatyPricingProfitCommissionService.Find(entity.TreatyPricingProfitCommissionId.Value) : null) : null,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,
                TreatyPricingProfitCommissionVersionBo = entity.TreatyPricingProfitCommissionVersionId.HasValue ? (foreign ? TreatyPricingProfitCommissionVersionService.Find(entity.TreatyPricingProfitCommissionVersionId.Value) : null) : null,
                TreatyPricingProfitCommissionSelect = profitCommissionSelect,
                ReinsurancePremiumPaymentPickListDetailId = entity.ReinsurancePremiumPaymentPickListDetailId,
                ReinsurancePremiumPaymentPickListDetailBo = foreign ? PickListDetailService.Find(entity.ReinsurancePremiumPaymentPickListDetailId) : null,
                UnearnedPremiumRefundPickListDetailId = entity.UnearnedPremiumRefundPickListDetailId,
                UnearnedPremiumRefundPickListDetailBo = foreign ? PickListDetailService.Find(entity.UnearnedPremiumRefundPickListDetailId) : null,
                TerminationClause = entity.TerminationClause,
                RecaptureClause = entity.RecaptureClause,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = foreign ? PickListDetailService.Find(entity.TerritoryOfIssueCodePickListDetailId) : null,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                GroupFreeCoverLimitNonCi = Util.EncodeString(entity.GroupFreeCoverLimitNonCi),
                GroupFreeCoverLimitAgeNonCi = entity.GroupFreeCoverLimitAgeNonCi,
                GroupFreeCoverLimitCi = Util.EncodeString(entity.GroupFreeCoverLimitCi),
                GroupFreeCoverLimitAgeCi = entity.GroupFreeCoverLimitAgeCi,
                GroupProfitCommission = Util.EncodeString(entity.GroupProfitCommission),
                OccupationalClassification = entity.OccupationalClassification,
                IsDirectRetro = entity.IsDirectRetro,
                DirectRetroProfitCommission = entity.DirectRetroProfitCommission,
                DirectRetroTerminationClause = entity.DirectRetroTerminationClause,
                DirectRetroRecaptureClause = entity.DirectRetroRecaptureClause,
                DirectRetroQuarterlyRiskPremium = entity.DirectRetroQuarterlyRiskPremium,
                IsInwardRetro = entity.IsInwardRetro,
                InwardRetroProfitCommission = entity.InwardRetroProfitCommission,
                InwardRetroTerminationClause = entity.InwardRetroTerminationClause,
                InwardRetroRecaptureClause = entity.InwardRetroRecaptureClause,
                InwardRetroQuarterlyRiskPremium = entity.InwardRetroQuarterlyRiskPremium,
                IsRetakafulService = entity.IsRetakafulService,
                InvestmentProfitSharing = entity.InvestmentProfitSharing,
                RetakafulModel = entity.RetakafulModel,
                TreatyPricingProductBenefit = TreatyPricingProductBenefitService.GetJsonByVersionId(entity.Id),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                #region Product & Benefit Comparison
                IsDirectRetroStr = entity.IsDirectRetro ? "Yes" : "No",
                IsInwardRetroStr = entity.IsInwardRetro ? "Yes" : "No",
                IsRetakafulServiceStr = entity.IsRetakafulService ? "Yes" : "No",
                #endregion
            };
        }

        public static TreatyPricingProductVersionBo FormBoForProductComparisonReport(TreatyPricingProductVersion entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProductVersionBo
            {
                Id = entity.Id,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                Version = entity.Version,
                TargetSegment = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.TargetSegment),
                DistributionChannel = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.DistributionChannel),
                CessionType = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.CessionType),
                ProductLine = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.ProductLine),
                ExpectedAverageSumAssured = entity.ExpectedAverageSumAssured,
                ExpectedRiPremium = entity.ExpectedRiPremium,
                ExpectedPolicyNo = entity.ExpectedPolicyNo,
                JumboLimit = entity.JumboLimit,
                UnderwritingAdditionalRemark = entity.UnderwritingAdditionalRemark,
                WaitingPeriod = entity.WaitingPeriod,
                SurvivalPeriod = entity.SurvivalPeriod,
                TerminationClause = entity.TerminationClause,
                RecaptureClause = entity.RecaptureClause,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                IsDirectRetro = entity.IsDirectRetro,
                DirectRetroProfitCommission = entity.DirectRetroProfitCommission,
                DirectRetroTerminationClause = entity.DirectRetroTerminationClause,
                DirectRetroRecaptureClause = entity.DirectRetroRecaptureClause,
                DirectRetroQuarterlyRiskPremium = entity.DirectRetroQuarterlyRiskPremium,
                IsInwardRetro = entity.IsInwardRetro,
                InwardRetroProfitCommission = entity.InwardRetroProfitCommission,
                InwardRetroTerminationClause = entity.InwardRetroTerminationClause,
                InwardRetroRecaptureClause = entity.InwardRetroRecaptureClause,
                InwardRetroQuarterlyRiskPremium = entity.InwardRetroQuarterlyRiskPremium,
                IsRetakafulService = entity.IsRetakafulService,
                InvestmentProfitSharing = entity.InvestmentProfitSharing,
                RetakafulModel = entity.RetakafulModel,

                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                #region Product & Benefit Comparison
                IsDirectRetroStr = entity.IsDirectRetro ? "Yes" : "No",
                IsInwardRetroStr = entity.IsInwardRetro ? "Yes" : "No",
                IsRetakafulServiceStr = entity.IsRetakafulService ? "Yes" : "No",
                ProductTypeStr = entity.ProductTypePickListDetailId == null ? "" : PickListDetailService.Find(entity.ProductTypePickListDetailId).Description,
                BusinessOriginStr = entity.BusinessOriginPickListDetailId == null ? "" : PickListDetailService.Find(entity.BusinessOriginPickListDetailId).Description,
                BusinessTypeStr = entity.BusinessTypePickListDetailId == null ? "" : PickListDetailService.Find(entity.BusinessTypePickListDetailId).Description,
                ReinsuranceArrangementStr = entity.ReinsuranceArrangementPickListDetailId == null ? "" : PickListDetailService.Find(entity.ReinsuranceArrangementPickListDetailId).Description,
                ReinsurancePremiumPaymentStr = entity.ReinsurancePremiumPaymentPickListDetailId == null ? "" : PickListDetailService.Find(entity.ReinsurancePremiumPaymentPickListDetailId).Description,
                UnearnedPremiumRefundStr = entity.UnearnedPremiumRefundPickListDetailId == null ? "" : PickListDetailService.Find(entity.UnearnedPremiumRefundPickListDetailId).Description,
                #endregion
            };
        }

        public static TreatyPricingProductVersionBo LinkFormBo(TreatyPricingProductVersion entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProductVersionBo
            {
                Id = entity.Id,
                TreatyPricingProductId = entity.TreatyPricingProductId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                ProductTypePickListDetailId = entity.ProductTypePickListDetailId,
                ProductTypePickListDetailBo = PickListDetailService.Find(entity.ProductTypePickListDetailId),
                TargetSegment = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.TargetSegment),
                DistributionChannel = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.DistributionChannel),
                CessionType = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.CessionType),
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessTypePickListDetailId = entity.BusinessTypePickListDetailId,
                ProductLine = TreatyPricingPickListDetailService.GetJoinCodeByObjectPickList(TreatyPricingCedantBo.ObjectProductVersion, entity.Id, PickListBo.ProductLine),
                ReinsuranceArrangementPickListDetailId = entity.ReinsuranceArrangementPickListDetailId,
                ExpectedAverageSumAssured = entity.ExpectedAverageSumAssured,
                ExpectedRiPremium = entity.ExpectedRiPremium,
                ExpectedPolicyNo = entity.ExpectedPolicyNo,
                TreatyPricingMedicalTableId = entity.TreatyPricingMedicalTableId,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingFinancialTableId = entity.TreatyPricingFinancialTableId,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingUwQuestionnaireId = entity.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingAdvantageProgramId = entity.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramVersionId = entity.TreatyPricingAdvantageProgramVersionId,
                JumboLimit = entity.JumboLimit,
                UnderwritingAdditionalRemark = entity.UnderwritingAdditionalRemark,
                WaitingPeriod = entity.WaitingPeriod,
                SurvivalPeriod = entity.SurvivalPeriod,
                TreatyPricingProfitCommissionId = entity.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,
                ReinsurancePremiumPaymentPickListDetailId = entity.ReinsurancePremiumPaymentPickListDetailId,
                UnearnedPremiumRefundPickListDetailId = entity.UnearnedPremiumRefundPickListDetailId,
                TerminationClause = entity.TerminationClause,
                RecaptureClause = entity.RecaptureClause,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                GroupFreeCoverLimitNonCi = Util.EncodeString(entity.GroupFreeCoverLimitNonCi),
                GroupFreeCoverLimitAgeNonCi = entity.GroupFreeCoverLimitAgeNonCi,
                GroupFreeCoverLimitCi = Util.EncodeString(entity.GroupFreeCoverLimitCi),
                GroupFreeCoverLimitAgeCi = entity.GroupFreeCoverLimitAgeCi,
                GroupProfitCommission = Util.EncodeString(entity.GroupProfitCommission),
                OccupationalClassification = entity.OccupationalClassification,
                IsDirectRetro = entity.IsDirectRetro,
                DirectRetroProfitCommission = entity.DirectRetroProfitCommission,
                DirectRetroTerminationClause = entity.DirectRetroTerminationClause,
                DirectRetroRecaptureClause = entity.DirectRetroRecaptureClause,
                DirectRetroQuarterlyRiskPremium = entity.DirectRetroQuarterlyRiskPremium,
                IsInwardRetro = entity.IsInwardRetro,
                InwardRetroProfitCommission = entity.InwardRetroProfitCommission,
                InwardRetroTerminationClause = entity.InwardRetroTerminationClause,
                InwardRetroRecaptureClause = entity.InwardRetroRecaptureClause,
                InwardRetroQuarterlyRiskPremium = entity.InwardRetroQuarterlyRiskPremium,
                IsRetakafulService = entity.IsRetakafulService,
                InvestmentProfitSharing = entity.InvestmentProfitSharing,
                RetakafulModel = entity.RetakafulModel,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingProductVersionBo> FormBos(IList<TreatyPricingProductVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProductVersionBo> bos = new List<TreatyPricingProductVersionBo>() { };
            foreach (TreatyPricingProductVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingProductVersion FormEntity(TreatyPricingProductVersionBo bo = null, TreatyPricingProductVersion entity = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProductVersion
            {
                Id = bo.Id,
                TreatyPricingProductId = bo.TreatyPricingProductId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                ProductTypePickListDetailId = bo.ProductTypePickListDetailId,
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId,
                BusinessTypePickListDetailId = bo.BusinessTypePickListDetailId,
                ReinsuranceArrangementPickListDetailId = bo.ReinsuranceArrangementPickListDetailId,
                ExpectedAverageSumAssured = bo.ExpectedAverageSumAssured,
                ExpectedRiPremium = bo.ExpectedRiPremium,
                ExpectedPolicyNo = bo.ExpectedPolicyNo,
                TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId,
                TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId,
                TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId,
                TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId,
                TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId,
                TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId,
                JumboLimit = bo.JumboLimit,
                UnderwritingAdditionalRemark = bo.UnderwritingAdditionalRemark,
                WaitingPeriod = bo.WaitingPeriod,
                SurvivalPeriod = bo.SurvivalPeriod,
                TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId,
                TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId,
                ReinsurancePremiumPaymentPickListDetailId = bo.ReinsurancePremiumPaymentPickListDetailId,
                UnearnedPremiumRefundPickListDetailId = bo.UnearnedPremiumRefundPickListDetailId,
                TerminationClause = bo.TerminationClause,
                RecaptureClause = bo.RecaptureClause,
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId,
                QuarterlyRiskPremium = bo.QuarterlyRiskPremium,
                GroupFreeCoverLimitNonCi = Util.DecodeString(bo.GroupFreeCoverLimitNonCi),
                GroupFreeCoverLimitAgeNonCi = bo.GroupFreeCoverLimitAgeNonCi,
                GroupFreeCoverLimitCi = Util.DecodeString(bo.GroupFreeCoverLimitCi),
                GroupFreeCoverLimitAgeCi = bo.GroupFreeCoverLimitAgeCi,
                GroupProfitCommission = Util.DecodeString(bo.GroupProfitCommission),
                OccupationalClassification = bo.OccupationalClassification,
                IsDirectRetro = bo.IsDirectRetro,
                DirectRetroProfitCommission = bo.DirectRetroProfitCommission,
                DirectRetroTerminationClause = bo.DirectRetroTerminationClause,
                DirectRetroRecaptureClause = bo.DirectRetroRecaptureClause,
                DirectRetroQuarterlyRiskPremium = bo.DirectRetroQuarterlyRiskPremium,
                IsInwardRetro = bo.IsInwardRetro,
                InwardRetroProfitCommission = bo.InwardRetroProfitCommission,
                InwardRetroTerminationClause = bo.InwardRetroTerminationClause,
                InwardRetroRecaptureClause = bo.InwardRetroRecaptureClause,
                InwardRetroQuarterlyRiskPremium = bo.InwardRetroQuarterlyRiskPremium,
                IsRetakafulService = bo.IsRetakafulService,
                InvestmentProfitSharing = bo.InvestmentProfitSharing,
                RetakafulModel = bo.RetakafulModel,

                CreatedAt = entity != null ? entity.CreatedAt : DateTime.Now,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProductVersion.IsExists(id);
        }

        public static TreatyPricingProductVersionBo Find(int? id)
        {
            return FormBo(TreatyPricingProductVersion.Find(id));
        }

        public static TreatyPricingProductVersionBo FindForProductComparisonReport(int? id)
        {
            return FormBoForProductComparisonReport(TreatyPricingProductVersion.Find(id));
        }

        public static IList<TreatyPricingProductVersionBo> GetByTreatyPricingProductId(int? treatyPricingProductId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions.Where(q => q.TreatyPricingProductId == treatyPricingProductId).ToList());
            }
        }

        public static TreatyPricingProductVersionBo GetLatestByTreatyPricingProductId(int? treatyPricingProductId, bool isLink = false)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProductId == treatyPricingProductId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault();

                return isLink ? LinkFormBo(entity) : FormBo(entity);
            }
        }

        public static IList<TreatyPricingProductVersionBo> GetByTreatyPricingCedantId(List<int> treatyPricingCedantIds, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => treatyPricingCedantIds.Contains(q.TreatyPricingProduct.TreatyPricingCedantId))
                    .OrderBy(q => q.TreatyPricingProductId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static IList<TreatyPricingProductVersionBo> GetByTreatyPricingMedicalTableVersionId(int? treatyPricingMedicalTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingMedicalTableVersionId == treatyPricingMedicalTableVersionId)
                    .ToList(), true);
            }
        }

        public static IList<TreatyPricingProductVersionBo> GetByTreatyPricingFinancialTableVersionId(int? treatyPricingFinancialTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingFinancialTableVersionId == treatyPricingFinancialTableVersionId)
                    .ToList(), true);
            }
        }

        public static IList<PickListDetailBo> GetDistinctProductTypeByIds(List<int> productIds)
        {
            using (var db = new AppDbContext())
            {
                List<int> ids = db.TreatyPricingProductVersions
                    .Where(q => productIds.Contains(q.TreatyPricingProductId))
                    .Where(q => q.ProductTypePickListDetailId.HasValue)
                    .GroupBy(q => q.ProductTypePickListDetailId)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.ProductTypePickListDetailId)
                    .Select(q => q.ProductTypePickListDetailId.Value)
                    .ToList();
                return PickListDetailService.FormBos(db.PickListDetails.Where(q => ids.Contains(q.Id)).ToList());
            }
        }

        public static List<int> GetIdByProductIdsProductType(List<int> productIds, int productType)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => productIds.Contains(q.TreatyPricingProductId))
                    .Where(q => q.ProductTypePickListDetailId.HasValue)
                    .Where(q => q.ProductTypePickListDetailId == productType)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByProductVersionIdsProductType(List<int> productVersionIds, int productType)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => productVersionIds.Contains(q.Id))
                    .Where(q => q.ProductTypePickListDetailId.HasValue)
                    .Where(q => q.ProductTypePickListDetailId == productType)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByIdsProductName(List<int> ids, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => ids.Contains(q.Id))
                    .Where(q => q.TreatyPricingProduct.Name == productName)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByProductIdsProductName(List<int> productIds, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => productIds.Contains(q.TreatyPricingProductId))
                    .Where(q => q.TreatyPricingProduct.Name == productName)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<string> GetDistinctProductNameByIds(List<int> Ids)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => Ids.Contains(q.Id))
                    .GroupBy(q => q.TreatyPricingProduct.Name)
                    .Select(r => r.FirstOrDefault())
                    .OrderBy(q => q.TreatyPricingProduct.Name)
                    .Select(q => q.TreatyPricingProduct.Name)
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList();
            }
        }

        public static List<int> GetIdByIdsProductType(List<int> productVersionIds, int productType)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => productVersionIds.Contains(q.Id))
                    .Where(q => q.ProductTypePickListDetailId.HasValue)
                    .Where(q => q.ProductTypePickListDetailId == productType)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static IList<TreatyPricingProductVersionBo> GetByVersionIds(List<int> productVersionIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingProductVersions
                    .Where(q => productVersionIds.Contains(q.Id))
                    .ToList(), true);
            }
        }

        public static string GetLinkedProductsByAdvantageProgramVersionId(int versionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingAdvantageProgramVersionId == versionId)
                    .Select(q => q.TreatyPricingProductId);

                string linkedProducts = "";
                List<string> linkedProductsList = new List<string>();
                foreach (int productId in query.ToList())
                {
                    var productBo = TreatyPricingProductService.Find(productId);
                    linkedProductsList.Add(productBo.Code + " - " + productBo.Name);
                }

                linkedProducts = string.Join(",", linkedProductsList.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct());

                return linkedProducts;
            }
        }

        public static IList<TreatyPricingProductVersionBo> GetBySearchParams(int? cedantId, int? treatyPricingCedantId, string quotationName, string underwritingMethods = "", string distributionChannels = "", string targetSegments = "", int? productType = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProductVersions.AsQueryable();

                if (cedantId.HasValue)
                    query = query.Where(q => q.TreatyPricingProduct.TreatyPricingCedant.CedantId == cedantId);
                if (treatyPricingCedantId.HasValue)
                    query = query.Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId);
                if (!string.IsNullOrEmpty(quotationName))
                    query = query.Where(q => q.TreatyPricingProduct.QuotationName == quotationName);
                if (productType.HasValue)
                    query = query.Where(q => q.ProductTypePickListDetailId == productType);

                if (!string.IsNullOrEmpty(underwritingMethods))
                {
                    var underwritingMethodList = underwritingMethods?.Split(',').ToList();
                    var ids = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProduct && b.PickListId == PickListBo.UnderwritingMethod)
                        .Where(b => underwritingMethodList.Contains(b.PickListDetailCode)).GroupBy(b => b.ObjectId).Select(b => b.Key).ToList();

                    query = query.Where(q => ids.Contains(q.TreatyPricingProductId));
                }

                if (!string.IsNullOrEmpty(distributionChannels))
                {
                    var distributionChannelList = distributionChannels?.Split(',').ToList();
                    var ids = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProductVersion && b.PickListId == PickListBo.DistributionChannel)
                        .Where(b => distributionChannelList.Contains(b.PickListDetailCode)).GroupBy(b => b.ObjectId).Select(b => b.Key).ToList();

                    query = query.Where(q => ids.Contains(q.Id));
                }

                if (!string.IsNullOrEmpty(targetSegments))
                {
                    var targetSegmentList = targetSegments?.Split(',').ToList();
                    var ids = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProductVersion && b.PickListId == PickListBo.TargetSegment)
                        .Where(b => targetSegmentList.Contains(b.PickListDetailCode)).GroupBy(b => b.ObjectId).Select(b => b.Key).ToList();

                    query = query.Where(q => ids.Contains(q.Id));
                }

                return FormBos(query.ToList());
            }
        }

        public static IQueryable<int> QueryByParam(AppDbContext db, int? treatyPricingCedantId, int? productTypeId, string targetSegments, string distributionChannels, string underwritingMethods, string productIdName, string quotationName)
        {
            var subQuery = db.TreatyPricingProductVersions.AsQueryable();

            if (treatyPricingCedantId.HasValue)
                subQuery = subQuery.Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId);

            if (productTypeId.HasValue)
                subQuery = subQuery.Where(q => q.ProductTypePickListDetailId == productTypeId);

            if (!string.IsNullOrEmpty(underwritingMethods))
            {
                var underwritingMethodList = underwritingMethods?.Split(',').ToList();
                var uwmQuery = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProduct && b.PickListId == PickListBo.UnderwritingMethod)
                    .Where(b => underwritingMethodList.Contains(b.PickListDetailCode)).Select(b => b.ObjectId);

                subQuery = subQuery.Where(q => uwmQuery.Contains(q.TreatyPricingProductId));
            }

            if (!string.IsNullOrEmpty(distributionChannels))
            {
                var distributionChannelList = distributionChannels?.Split(',').ToList();
                var dcQuery = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProductVersion && b.PickListId == PickListBo.DistributionChannel)
                    .Where(b => distributionChannelList.Contains(b.PickListDetailCode)).Select(b => b.ObjectId);

                subQuery = subQuery.Where(q => dcQuery.Contains(q.Id));
            }

            if (!string.IsNullOrEmpty(targetSegments))
            {
                var targetSegmentList = targetSegments?.Split(',').ToList();
                var tsQuery = db.TreatyPricingPickListDetails.Where(b => b.ObjectType == TreatyPricingCedantBo.ObjectProductVersion && b.PickListId == PickListBo.TargetSegment)
                    .Where(b => targetSegmentList.Contains(b.PickListDetailCode)).Select(b => b.ObjectId);

                subQuery = subQuery.Where(q => tsQuery.Contains(q.Id));
            }

            if (!string.IsNullOrEmpty(productIdName))
                subQuery = subQuery.Where(q => q.TreatyPricingProduct.Code.Contains(productIdName) || q.TreatyPricingProduct.Name.Contains(productIdName));

            if (!string.IsNullOrEmpty(quotationName))
                subQuery = subQuery.Where(q => q.TreatyPricingProduct.QuotationName.Contains(quotationName));

            return subQuery.Select(q => q.TreatyPricingProductId);
        }

        public static Result Save(ref TreatyPricingProductVersionBo bo)
        {
            if (!TreatyPricingProductVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingProductVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingProductVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProductVersionBo bo)
        {
            TreatyPricingProductVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProductVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductVersionBo bo)
        {
            Result result = Result();

            TreatyPricingProductVersion entity = TreatyPricingProductVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo, entity);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProductVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProductVersionBo bo)
        {
            TreatyPricingProductVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProductVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProductVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
