using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeAggregationDetailDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationDetailData)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationDetailData.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationDetailData, PerLifeAggregationDetailDataBo>> Expression()
        {
            return entity => new PerLifeAggregationDetailDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailTreatyId = entity.PerLifeAggregationDetailTreatyId,
                RiDataWarehouseHistoryId = entity.RiDataWarehouseHistoryId,
                ExpectedGenderCode = entity.ExpectedGenderCode,
                RetroBenefitCode = entity.RetroBenefitCode,
                ExpectedTerritoryOfIssueCode = entity.ExpectedTerritoryOfIssueCode,
                FlagCode = entity.FlagCode,
                ExceptionType = entity.ExceptionType,
                ExceptionErrorType = entity.ExceptionErrorType,
                IsException = entity.IsException,
                Errors = entity.Errors,
                ProceedStatus = entity.ProceedStatus,
                Remarks = entity.Remarks,
                UpdatedAt = entity.UpdatedAt,

                // RI Data Warehouse History
                Quarter = entity.RiDataWarehouseHistory.Quarter,
                EndingPolicyStatus = entity.RiDataWarehouseHistory.EndingPolicyStatus,
                RecordType = entity.RiDataWarehouseHistory.RecordType,
                TreatyCode = entity.RiDataWarehouseHistory.TreatyCode,
                ReinsBasisCode = entity.RiDataWarehouseHistory.ReinsBasisCode,
                FundsAccountingTypeCode = entity.RiDataWarehouseHistory.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.RiDataWarehouseHistory.PremiumFrequencyCode,
                ReportPeriodMonth = entity.RiDataWarehouseHistory.ReportPeriodMonth,
                ReportPeriodYear = entity.RiDataWarehouseHistory.ReportPeriodYear,
                RiskPeriodMonth = entity.RiDataWarehouseHistory.RiskPeriodMonth,
                RiskPeriodYear = entity.RiDataWarehouseHistory.RiskPeriodYear,
                TransactionTypeCode = entity.RiDataWarehouseHistory.TransactionTypeCode,
                PolicyNumber = entity.RiDataWarehouseHistory.PolicyNumber,
                IssueDatePol = entity.RiDataWarehouseHistory.IssueDatePol,
                IssueDateBen = entity.RiDataWarehouseHistory.IssueDateBen,
                ReinsEffDatePol = entity.RiDataWarehouseHistory.ReinsEffDatePol,
                ReinsEffDateBen = entity.RiDataWarehouseHistory.ReinsEffDateBen,
                CedingPlanCode = entity.RiDataWarehouseHistory.CedingPlanCode,
                CedingBenefitTypeCode = entity.RiDataWarehouseHistory.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.RiDataWarehouseHistory.CedingBenefitRiskCode,
                MlreBenefitCode = entity.RiDataWarehouseHistory.MlreBenefitCode,
                OriSumAssured = entity.RiDataWarehouseHistory.OriSumAssured,
                CurrSumAssured = entity.RiDataWarehouseHistory.CurrSumAssured,
                AmountCededB4MlreShare = entity.RiDataWarehouseHistory.AmountCededB4MlreShare,
                RetentionAmount = entity.RiDataWarehouseHistory.RetentionAmount,
                AarOri = entity.RiDataWarehouseHistory.AarOri,
                Aar = entity.RiDataWarehouseHistory.Aar,
                AarSpecial1 = entity.RiDataWarehouseHistory.AarSpecial1,
                AarSpecial2 = entity.RiDataWarehouseHistory.AarSpecial2,
                AarSpecial3 = entity.RiDataWarehouseHistory.AarSpecial3,
                InsuredName = entity.RiDataWarehouseHistory.InsuredName,
                InsuredGenderCode = entity.RiDataWarehouseHistory.InsuredGenderCode,
                InsuredTobaccoUse = entity.RiDataWarehouseHistory.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.RiDataWarehouseHistory.InsuredDateOfBirth,
                InsuredOccupationCode = entity.RiDataWarehouseHistory.InsuredOccupationCode,
                InsuredRegisterNo = entity.RiDataWarehouseHistory.InsuredRegisterNo,
                InsuredAttainedAge = entity.RiDataWarehouseHistory.InsuredAttainedAge,
                InsuredNewIcNumber = entity.RiDataWarehouseHistory.InsuredNewIcNumber,
                InsuredOldIcNumber = entity.RiDataWarehouseHistory.InsuredOldIcNumber,
                InsuredName2nd = entity.RiDataWarehouseHistory.InsuredName2nd,
                InsuredGenderCode2nd = entity.RiDataWarehouseHistory.InsuredGenderCode2nd,
                InsuredTobaccoUse2nd = entity.RiDataWarehouseHistory.InsuredTobaccoUse2nd,
                InsuredDateOfBirth2nd = entity.RiDataWarehouseHistory.InsuredDateOfBirth2nd,
                InsuredAttainedAge2nd = entity.RiDataWarehouseHistory.InsuredAttainedAge2nd,
                InsuredNewIcNumber2nd = entity.RiDataWarehouseHistory.InsuredNewIcNumber2nd,
                InsuredOldIcNumber2nd = entity.RiDataWarehouseHistory.InsuredOldIcNumber2nd,
                ReinsuranceIssueAge = entity.RiDataWarehouseHistory.ReinsuranceIssueAge,
                ReinsuranceIssueAge2nd = entity.RiDataWarehouseHistory.ReinsuranceIssueAge2nd,
                PolicyTerm = entity.RiDataWarehouseHistory.PolicyTerm,
                PolicyExpiryDate = entity.RiDataWarehouseHistory.PolicyExpiryDate,
                DurationYear = entity.RiDataWarehouseHistory.DurationYear,
                DurationDay = entity.RiDataWarehouseHistory.DurationDay,
                DurationMonth = entity.RiDataWarehouseHistory.DurationMonth,
                PremiumCalType = entity.RiDataWarehouseHistory.PremiumCalType,
                CedantRiRate = entity.RiDataWarehouseHistory.CedantRiRate,
                RateTable = entity.RiDataWarehouseHistory.RateTable,
                AgeRatedUp = entity.RiDataWarehouseHistory.AgeRatedUp,
                DiscountRate = entity.RiDataWarehouseHistory.DiscountRate,
                LoadingType = entity.RiDataWarehouseHistory.LoadingType,
                UnderwriterRating = entity.RiDataWarehouseHistory.UnderwriterRating,
                UnderwriterRatingUnit = entity.RiDataWarehouseHistory.UnderwriterRatingUnit,
                UnderwriterRatingTerm = entity.RiDataWarehouseHistory.UnderwriterRatingTerm,
                UnderwriterRating2 = entity.RiDataWarehouseHistory.UnderwriterRating2,
                UnderwriterRatingUnit2 = entity.RiDataWarehouseHistory.UnderwriterRatingUnit2,
                UnderwriterRatingTerm2 = entity.RiDataWarehouseHistory.UnderwriterRatingTerm2,
                UnderwriterRating3 = entity.RiDataWarehouseHistory.UnderwriterRating3,
                UnderwriterRatingUnit3 = entity.RiDataWarehouseHistory.UnderwriterRatingUnit3,
                UnderwriterRatingTerm3 = entity.RiDataWarehouseHistory.UnderwriterRatingTerm3,
                FlatExtraAmount = entity.RiDataWarehouseHistory.FlatExtraAmount,
                FlatExtraUnit = entity.RiDataWarehouseHistory.FlatExtraUnit,
                FlatExtraTerm = entity.RiDataWarehouseHistory.FlatExtraTerm,
                FlatExtraAmount2 = entity.RiDataWarehouseHistory.FlatExtraAmount2,
                FlatExtraTerm2 = entity.RiDataWarehouseHistory.FlatExtraTerm2,
                StandardPremium = entity.RiDataWarehouseHistory.StandardPremium,
                SubstandardPremium = entity.RiDataWarehouseHistory.SubstandardPremium,
                FlatExtraPremium = entity.RiDataWarehouseHistory.FlatExtraPremium,
                GrossPremium = entity.RiDataWarehouseHistory.GrossPremium,
                StandardDiscount = entity.RiDataWarehouseHistory.StandardDiscount,
                SubstandardDiscount = entity.RiDataWarehouseHistory.SubstandardDiscount,
                VitalityDiscount = entity.RiDataWarehouseHistory.VitalityDiscount,
                TotalDiscount = entity.RiDataWarehouseHistory.TotalDiscount,
                NetPremium = entity.RiDataWarehouseHistory.NetPremium,
                AnnualRiPrem = entity.RiDataWarehouseHistory.AnnualRiPrem,
                RiCovPeriod = entity.RiDataWarehouseHistory.RiCovPeriod,
                AdjBeginDate = entity.RiDataWarehouseHistory.AdjBeginDate,
                AdjEndDate = entity.RiDataWarehouseHistory.AdjEndDate,
                PolicyNumberOld = entity.RiDataWarehouseHistory.PolicyNumberOld,
                PolicyStatusCode = entity.RiDataWarehouseHistory.PolicyStatusCode,
                PolicyGrossPremium = entity.RiDataWarehouseHistory.PolicyGrossPremium,
                PolicyStandardPremium = entity.RiDataWarehouseHistory.PolicyStandardPremium,
                PolicySubstandardPremium = entity.RiDataWarehouseHistory.PolicySubstandardPremium,
                PolicyTermRemain = entity.RiDataWarehouseHistory.PolicyTermRemain,
                PolicyAmountDeath = entity.RiDataWarehouseHistory.PolicyAmountDeath,
                PolicyReserve = entity.RiDataWarehouseHistory.PolicyReserve,
                PolicyPaymentMethod = entity.RiDataWarehouseHistory.PolicyPaymentMethod,
                PolicyLifeNumber = entity.RiDataWarehouseHistory.PolicyLifeNumber,
                FundCode = entity.RiDataWarehouseHistory.FundCode,
                LineOfBusiness = entity.RiDataWarehouseHistory.LineOfBusiness,
                ApLoading = entity.RiDataWarehouseHistory.ApLoading,
                LoanInterestRate = entity.RiDataWarehouseHistory.LoanInterestRate,
                DefermentPeriod = entity.RiDataWarehouseHistory.DefermentPeriod,
                RiderNumber = entity.RiDataWarehouseHistory.RiderNumber,
                CampaignCode = entity.RiDataWarehouseHistory.CampaignCode,
                Nationality = entity.RiDataWarehouseHistory.Nationality,
                TerritoryOfIssueCode = entity.RiDataWarehouseHistory.TerritoryOfIssueCode,
                CurrencyCode = entity.RiDataWarehouseHistory.CurrencyCode,
                StaffPlanIndicator = entity.RiDataWarehouseHistory.StaffPlanIndicator,
                CedingTreatyCode = entity.RiDataWarehouseHistory.CedingTreatyCode,
                CedingPlanCodeOld = entity.RiDataWarehouseHistory.CedingPlanCodeOld,
                CedingBasicPlanCode = entity.RiDataWarehouseHistory.CedingBasicPlanCode,
                CedantSar = entity.RiDataWarehouseHistory.CedantSar,
                CedantReinsurerCode = entity.RiDataWarehouseHistory.CedantReinsurerCode,
                AmountCededB4MlreShare2 = entity.RiDataWarehouseHistory.AmountCededB4MlreShare2,
                CessionCode = entity.RiDataWarehouseHistory.CessionCode,
                CedantRemark = entity.RiDataWarehouseHistory.CedantRemark,
                GroupPolicyNumber = entity.RiDataWarehouseHistory.GroupPolicyNumber,
                GroupPolicyName = entity.RiDataWarehouseHistory.GroupPolicyName,
                NoOfEmployee = entity.RiDataWarehouseHistory.NoOfEmployee,
                PolicyTotalLive = entity.RiDataWarehouseHistory.PolicyTotalLive,
                GroupSubsidiaryName = entity.RiDataWarehouseHistory.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.RiDataWarehouseHistory.GroupSubsidiaryNo,
                GroupEmployeeBasicSalary = entity.RiDataWarehouseHistory.GroupEmployeeBasicSalary,
                GroupEmployeeJobType = entity.RiDataWarehouseHistory.GroupEmployeeJobType,
                GroupEmployeeJobCode = entity.RiDataWarehouseHistory.GroupEmployeeJobCode,
                GroupEmployeeBasicSalaryRevise = entity.RiDataWarehouseHistory.GroupEmployeeBasicSalaryRevise,
                GroupEmployeeBasicSalaryMultiplier = entity.RiDataWarehouseHistory.GroupEmployeeBasicSalaryMultiplier,
                CedingPlanCode2 = entity.RiDataWarehouseHistory.CedingPlanCode2,
                DependantIndicator = entity.RiDataWarehouseHistory.DependantIndicator,
                GhsRoomBoard = entity.RiDataWarehouseHistory.GhsRoomBoard,
                PolicyAmountSubstandard = entity.RiDataWarehouseHistory.PolicyAmountSubstandard,
                Layer1RiShare = entity.RiDataWarehouseHistory.Layer1RiShare,
                Layer1InsuredAttainedAge = entity.RiDataWarehouseHistory.Layer1InsuredAttainedAge,
                Layer1InsuredAttainedAge2nd = entity.RiDataWarehouseHistory.Layer1InsuredAttainedAge2nd,
                Layer1StandardPremium = entity.RiDataWarehouseHistory.Layer1StandardPremium,
                Layer1SubstandardPremium = entity.RiDataWarehouseHistory.Layer1SubstandardPremium,
                Layer1GrossPremium = entity.RiDataWarehouseHistory.Layer1GrossPremium,
                Layer1StandardDiscount = entity.RiDataWarehouseHistory.Layer1StandardDiscount,
                Layer1SubstandardDiscount = entity.RiDataWarehouseHistory.Layer1SubstandardDiscount,
                Layer1TotalDiscount = entity.RiDataWarehouseHistory.Layer1TotalDiscount,
                Layer1NetPremium = entity.RiDataWarehouseHistory.Layer1NetPremium,
                Layer1GrossPremiumAlt = entity.RiDataWarehouseHistory.Layer1GrossPremiumAlt,
                Layer1TotalDiscountAlt = entity.RiDataWarehouseHistory.Layer1TotalDiscountAlt,
                Layer1NetPremiumAlt = entity.RiDataWarehouseHistory.Layer1NetPremiumAlt,
                SpecialIndicator1 = entity.RiDataWarehouseHistory.SpecialIndicator1,
                SpecialIndicator2 = entity.RiDataWarehouseHistory.SpecialIndicator2,
                SpecialIndicator3 = entity.RiDataWarehouseHistory.SpecialIndicator3,
                IndicatorJointLife = entity.RiDataWarehouseHistory.IndicatorJointLife,
                TaxAmount = entity.RiDataWarehouseHistory.TaxAmount,
                GstIndicator = entity.RiDataWarehouseHistory.GstIndicator,
                GstGrossPremium = entity.RiDataWarehouseHistory.GstGrossPremium,
                GstTotalDiscount = entity.RiDataWarehouseHistory.GstTotalDiscount,
                GstVitality = entity.RiDataWarehouseHistory.GstVitality,
                GstAmount = entity.RiDataWarehouseHistory.GstAmount,
                Mfrs17BasicRider = entity.RiDataWarehouseHistory.Mfrs17BasicRider,
                Mfrs17CellName = entity.RiDataWarehouseHistory.Mfrs17CellName,
                Mfrs17TreatyCode = entity.RiDataWarehouseHistory.Mfrs17TreatyCode,
                LoaCode = entity.RiDataWarehouseHistory.LoaCode,
                NoClaimBonus = entity.RiDataWarehouseHistory.NoClaimBonus,
                SurrenderValue = entity.RiDataWarehouseHistory.SurrenderValue,
                DatabaseCommision = entity.RiDataWarehouseHistory.DatabaseCommision,
                GrossPremiumAlt = entity.RiDataWarehouseHistory.GrossPremiumAlt,
                NetPremiumAlt = entity.RiDataWarehouseHistory.NetPremiumAlt,
                Layer1FlatExtraPremium = entity.RiDataWarehouseHistory.Layer1FlatExtraPremium,
                TransactionPremium = entity.RiDataWarehouseHistory.TransactionPremium,
                OriginalPremium = entity.RiDataWarehouseHistory.OriginalPremium,
                TransactionDiscount = entity.RiDataWarehouseHistory.TransactionDiscount,
                OriginalDiscount = entity.RiDataWarehouseHistory.OriginalDiscount,
                BrokerageFee = entity.RiDataWarehouseHistory.BrokerageFee,
                MaxUwRating = entity.RiDataWarehouseHistory.MaxUwRating,
                RetentionCap = entity.RiDataWarehouseHistory.RetentionCap,
                AarCap = entity.RiDataWarehouseHistory.AarCap,
                RiRate = entity.RiDataWarehouseHistory.RiRate,
                RiRate2 = entity.RiDataWarehouseHistory.RiRate2,
                AnnuityFactor = entity.RiDataWarehouseHistory.AnnuityFactor,
                SumAssuredOffered = entity.RiDataWarehouseHistory.SumAssuredOffered,
                UwRatingOffered = entity.RiDataWarehouseHistory.UwRatingOffered,
                FlatExtraAmountOffered = entity.RiDataWarehouseHistory.FlatExtraAmountOffered,
                FlatExtraDuration = entity.RiDataWarehouseHistory.FlatExtraDuration,
                EffectiveDate = entity.RiDataWarehouseHistory.EffectiveDate,
                OfferLetterSentDate = entity.RiDataWarehouseHistory.OfferLetterSentDate,
                RiskPeriodStartDate = entity.RiDataWarehouseHistory.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiDataWarehouseHistory.RiskPeriodEndDate,
                Mfrs17AnnualCohort = entity.RiDataWarehouseHistory.Mfrs17AnnualCohort,
                MaxExpiryAge = entity.RiDataWarehouseHistory.MaxExpiryAge,
                MinIssueAge = entity.RiDataWarehouseHistory.MinIssueAge,
                MaxIssueAge = entity.RiDataWarehouseHistory.MaxIssueAge,
                MinAar = entity.RiDataWarehouseHistory.MinAar,
                MaxAar = entity.RiDataWarehouseHistory.MaxAar,
                CorridorLimit = entity.RiDataWarehouseHistory.CorridorLimit,
                Abl = entity.RiDataWarehouseHistory.Abl,
                RatePerBasisUnit = entity.RiDataWarehouseHistory.RatePerBasisUnit,
                RiDiscountRate = entity.RiDataWarehouseHistory.RiDiscountRate,
                LargeSaDiscount = entity.RiDataWarehouseHistory.LargeSaDiscount,
                GroupSizeDiscount = entity.RiDataWarehouseHistory.GroupSizeDiscount,
                EwarpNumber = entity.RiDataWarehouseHistory.EwarpNumber,
                EwarpActionCode = entity.RiDataWarehouseHistory.EwarpActionCode,
                RetentionShare = entity.RiDataWarehouseHistory.RetentionShare,
                AarShare = entity.RiDataWarehouseHistory.AarShare,
                ProfitComm = entity.RiDataWarehouseHistory.ProfitComm,
                TotalDirectRetroAar = entity.RiDataWarehouseHistory.TotalDirectRetroAar,
                TotalDirectRetroGrossPremium = entity.RiDataWarehouseHistory.TotalDirectRetroGrossPremium,
                TotalDirectRetroDiscount = entity.RiDataWarehouseHistory.TotalDirectRetroDiscount,
                TotalDirectRetroNetPremium = entity.RiDataWarehouseHistory.TotalDirectRetroNetPremium,
                TreatyType = entity.RiDataWarehouseHistory.TreatyType,
                MaxApLoading = entity.RiDataWarehouseHistory.MaxApLoading,
                MlreInsuredAttainedAgeAtCurrentMonth = entity.RiDataWarehouseHistory.MlreInsuredAttainedAgeAtCurrentMonth,
                MlreInsuredAttainedAgeAtPreviousMonth = entity.RiDataWarehouseHistory.MlreInsuredAttainedAgeAtPreviousMonth,
                InsuredAttainedAgeCheck = entity.RiDataWarehouseHistory.InsuredAttainedAgeCheck,
                MaxExpiryAgeCheck = entity.RiDataWarehouseHistory.MaxExpiryAgeCheck,
                MlrePolicyIssueAge = entity.RiDataWarehouseHistory.MlrePolicyIssueAge,
                PolicyIssueAgeCheck = entity.RiDataWarehouseHistory.PolicyIssueAgeCheck,
                MinIssueAgeCheck = entity.RiDataWarehouseHistory.MinIssueAgeCheck,
                MaxIssueAgeCheck = entity.RiDataWarehouseHistory.MaxIssueAgeCheck,
                MaxUwRatingCheck = entity.RiDataWarehouseHistory.MaxUwRatingCheck,
                ApLoadingCheck = entity.RiDataWarehouseHistory.ApLoadingCheck,
                EffectiveDateCheck = entity.RiDataWarehouseHistory.EffectiveDateCheck,
                MinAarCheck = entity.RiDataWarehouseHistory.MinAarCheck,
                MaxAarCheck = entity.RiDataWarehouseHistory.MaxAarCheck,
                CorridorLimitCheck = entity.RiDataWarehouseHistory.CorridorLimitCheck,
                AblCheck = entity.RiDataWarehouseHistory.AblCheck,
                RetentionCheck = entity.RiDataWarehouseHistory.RetentionCheck,
                AarCheck = entity.RiDataWarehouseHistory.AarCheck,
                MlreStandardPremium = entity.RiDataWarehouseHistory.MlreStandardPremium,
                MlreSubstandardPremium = entity.RiDataWarehouseHistory.MlreSubstandardPremium,
                MlreFlatExtraPremium = entity.RiDataWarehouseHistory.MlreFlatExtraPremium,
                MlreGrossPremium = entity.RiDataWarehouseHistory.MlreGrossPremium,
                MlreStandardDiscount = entity.RiDataWarehouseHistory.MlreStandardDiscount,
                MlreSubstandardDiscount = entity.RiDataWarehouseHistory.MlreSubstandardDiscount,
                MlreLargeSaDiscount = entity.RiDataWarehouseHistory.MlreLargeSaDiscount,
                MlreGroupSizeDiscount = entity.RiDataWarehouseHistory.MlreGroupSizeDiscount,
                MlreVitalityDiscount = entity.RiDataWarehouseHistory.MlreVitalityDiscount,
                MlreTotalDiscount = entity.RiDataWarehouseHistory.MlreTotalDiscount,
                MlreNetPremium = entity.RiDataWarehouseHistory.MlreNetPremium,
                NetPremiumCheck = entity.RiDataWarehouseHistory.NetPremiumCheck,
                ServiceFeePercentage = entity.RiDataWarehouseHistory.ServiceFeePercentage,
                ServiceFee = entity.RiDataWarehouseHistory.ServiceFee,
                MlreBrokerageFee = entity.RiDataWarehouseHistory.MlreBrokerageFee,
                MlreDatabaseCommission = entity.RiDataWarehouseHistory.MlreDatabaseCommission,
                ValidityDayCheck = entity.RiDataWarehouseHistory.ValidityDayCheck,
                SumAssuredOfferedCheck = entity.RiDataWarehouseHistory.SumAssuredOfferedCheck,
                UwRatingCheck = entity.RiDataWarehouseHistory.UwRatingCheck,
                FlatExtraAmountCheck = entity.RiDataWarehouseHistory.FlatExtraAmountCheck,
                FlatExtraDurationCheck = entity.RiDataWarehouseHistory.FlatExtraDurationCheck,
                LastUpdatedDate = entity.RiDataWarehouseHistory.LastUpdatedDate,
                AarShare2 = entity.RiDataWarehouseHistory.AarShare2,
                AarCap2 = entity.RiDataWarehouseHistory.AarCap2,
                WakalahFeePercentage = entity.RiDataWarehouseHistory.WakalahFeePercentage,
                TreatyNumber = entity.RiDataWarehouseHistory.TreatyNumber,

                // Direct Retro
                RetroParty1 = entity.RiDataWarehouseHistory.RetroParty1,
                RetroParty2 = entity.RiDataWarehouseHistory.RetroParty2,
                RetroParty3 = entity.RiDataWarehouseHistory.RetroParty3,
                RetroShare1 = entity.RiDataWarehouseHistory.RetroShare1,
                RetroShare2 = entity.RiDataWarehouseHistory.RetroShare2,
                RetroShare3 = entity.RiDataWarehouseHistory.RetroShare3,
                RetroAar1 = entity.RiDataWarehouseHistory.RetroAar1,
                RetroAar2 = entity.RiDataWarehouseHistory.RetroAar2,
                RetroAar3 = entity.RiDataWarehouseHistory.RetroAar3,
                RetroReinsurancePremium1 = entity.RiDataWarehouseHistory.RetroReinsurancePremium1,
                RetroReinsurancePremium2 = entity.RiDataWarehouseHistory.RetroReinsurancePremium2,
                RetroReinsurancePremium3 = entity.RiDataWarehouseHistory.RetroReinsurancePremium3,
                RetroDiscount1 = entity.RiDataWarehouseHistory.RetroDiscount1,
                RetroDiscount2 = entity.RiDataWarehouseHistory.RetroDiscount2,
                RetroDiscount3 = entity.RiDataWarehouseHistory.RetroDiscount3,
                RetroNetPremium1 = entity.RiDataWarehouseHistory.RetroNetPremium1,
                RetroNetPremium2 = entity.RiDataWarehouseHistory.RetroNetPremium2,
                RetroNetPremium3 = entity.RiDataWarehouseHistory.RetroNetPremium3,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationDetailDataBo FormBo(PerLifeAggregationDetailData entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationDetailDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailTreatyId = entity.PerLifeAggregationDetailTreatyId,
                PerLifeAggregationDetailTreatyBo = PerLifeAggregationDetailTreatyService.Find(entity.PerLifeAggregationDetailTreatyId),
                RiDataWarehouseHistoryId = entity.RiDataWarehouseHistoryId,
                RiDataWarehouseHistoryBo = RiDataWarehouseHistoryService.Find(entity.RiDataWarehouseHistoryId, null),
                ExpectedGenderCode = entity.ExpectedGenderCode,
                RetroBenefitCode = entity.RetroBenefitCode,
                ExpectedTerritoryOfIssueCode = entity.ExpectedTerritoryOfIssueCode,
                FlagCode = entity.FlagCode,
                ExceptionType = entity.ExceptionType,
                ExceptionErrorType = entity.ExceptionErrorType,
                IsException = entity.IsException,
                Errors = entity.Errors,
                ProceedStatus = entity.ProceedStatus,
                Remarks = entity.Remarks,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static IList<PerLifeAggregationDetailDataBo> FormBos(IList<PerLifeAggregationDetailData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationDetailDataBo> bos = new List<PerLifeAggregationDetailDataBo>() { };
            foreach (PerLifeAggregationDetailData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregationDetailData FormEntity(PerLifeAggregationDetailDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationDetailData
            {
                Id = bo.Id,
                PerLifeAggregationDetailTreatyId = bo.PerLifeAggregationDetailTreatyId,
                RiDataWarehouseHistoryId = bo.RiDataWarehouseHistoryId,
                ExpectedGenderCode = bo.ExpectedGenderCode,
                RetroBenefitCode = bo.RetroBenefitCode,
                ExpectedTerritoryOfIssueCode = bo.ExpectedTerritoryOfIssueCode,
                FlagCode = bo.FlagCode,
                ExceptionType = bo.ExceptionType,
                ExceptionErrorType = bo.ExceptionErrorType,
                IsException = bo.IsException,
                Errors = bo.Errors,
                ProceedStatus = bo.ProceedStatus,
                Remarks = bo.Remarks,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationDetailData.IsExists(id);
        }

        public static PerLifeAggregationDetailDataBo Find(int? id)
        {
            return FormBo(PerLifeAggregationDetailData.Find(id));
        }

        public static IList<PerLifeAggregationDetailDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetailData.ToList());
            }
        }

        public static IList<PerLifeAggregationDetailDataBo> GetByPerLifeAggregationDetailTreatyId(int perLifeAggregationDetailTreatyId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDetailData
                    .Where(q => q.PerLifeAggregationDetailTreatyId == perLifeAggregationDetailTreatyId)
                    .ToList());
            }
        }

        public static IList<PerLifeAggregationDetailDataBo> GetByConflictCheckParams
        (
            string insuredName,
            DateTime? insuredDateOfBirth,
            string mlreBenefitCode,
            string transactionTypeCode,
            string insuredGenderCode,
            string territoryOfIssueCode
        )
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.PerLifeAggregationDetailData
                    .Where(q => q.RiDataWarehouseHistory.InsuredName == insuredName)
                    .Where(q => q.RiDataWarehouseHistory.InsuredDateOfBirth == insuredDateOfBirth)
                    .Where(q => q.RiDataWarehouseHistory.MlreBenefitCode == mlreBenefitCode)
                    .Where(q => q.RiDataWarehouseHistory.TransactionTypeCode == transactionTypeCode)
                    .Where(q => q.RiDataWarehouseHistory.InsuredGenderCode != insuredGenderCode)
                    .Where(q => q.RiDataWarehouseHistory.TerritoryOfIssueCode != territoryOfIssueCode);

                return FormBos(query.ToList());
            }
        }

        public static IList<PerLifeAggregationDetailDataBo> GetByDuplicationCheckParams
        (
            int perLifeAggregationDetailId,
            string insuredName,
            string policyNumber,
            string mlreBenefitCode,
            DateTime? insuredDateOfBirth,
            string transactionTypeCode,
            string cedingPlanCode,
            string treatyCode,
            string insuredGenderCode,
            DateTime? effectiveDate,
            DateTime? reinsEffDatePol
        )
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.PerLifeAggregationDetailData
                    .Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                    .Where(q => q.RiDataWarehouseHistory.InsuredName == insuredName)
                    .Where(q => q.RiDataWarehouseHistory.PolicyNumber == policyNumber)
                    .Where(q => q.RiDataWarehouseHistory.MlreBenefitCode == mlreBenefitCode)
                    .Where(q => q.RiDataWarehouseHistory.InsuredDateOfBirth == insuredDateOfBirth)
                    .Where(q => q.RiDataWarehouseHistory.TransactionTypeCode == transactionTypeCode)
                    .Where(q => q.RiDataWarehouseHistory.CedingPlanCode == cedingPlanCode)
                    .Where(q => q.RiDataWarehouseHistory.TreatyCode == treatyCode)
                    .Where(q => q.RiDataWarehouseHistory.InsuredGenderCode == insuredGenderCode)
                    .Where(q => q.RiDataWarehouseHistory.EffectiveDate == effectiveDate)
                    .Where(q => q.RiDataWarehouseHistory.ReinsEffDatePol == reinsEffDatePol);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref PerLifeAggregationDetailDataBo bo)
        {
            if (!PerLifeAggregationDetailData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationDetailDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationDetailData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationDetailDataBo bo)
        {
            PerLifeAggregationDetailData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationDetailDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailDataBo bo)
        {
            Result result = Result();

            PerLifeAggregationDetailData entity = PerLifeAggregationDetailData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationDetailTreatyId = bo.PerLifeAggregationDetailTreatyId;
                entity.RiDataWarehouseHistoryId = bo.RiDataWarehouseHistoryId;
                entity.ExpectedGenderCode = bo.ExpectedGenderCode;
                entity.RetroBenefitCode = bo.RetroBenefitCode;
                entity.ExpectedTerritoryOfIssueCode = bo.ExpectedTerritoryOfIssueCode;
                entity.FlagCode = bo.FlagCode;
                entity.ExceptionType = bo.ExceptionType;
                entity.ExceptionErrorType = bo.ExceptionErrorType;
                entity.IsException = bo.IsException;
                entity.Errors = bo.Errors;
                entity.ProceedStatus = bo.ProceedStatus;
                entity.Remarks = bo.Remarks;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDetailDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationDetailDataBo bo)
        {
            PerLifeAggregationDetailData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationDetailDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationDetailData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailTreatyId(int perLifeAggregationDetailTreatyId)
        {
            return PerLifeAggregationDetailData.DeleteByPerLifeAggregationDetailTreatyId(perLifeAggregationDetailTreatyId);
        }

        public static void DeleteByPerLifeAggregationDetailTreatyId(int perLifeAggregationDetailTreatyId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationDetailTreatyId(perLifeAggregationDetailTreatyId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregationDetailData)));
                }
            }
        }
    }
}
