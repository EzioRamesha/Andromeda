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
    public class PerLifeAggregationMonthlyDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationMonthlyData)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationMonthlyData.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationMonthlyData, PerLifeAggregationMonthlyDataBo>> Expression()
        {
            return entity => new PerLifeAggregationMonthlyDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                RiskYear = entity.RiskYear,
                RiskMonth = entity.RiskMonth,
                UniqueKeyPerLife = entity.UniqueKeyPerLife,
                RetroPremFreq = entity.RetroPremFreq,
                Aar = entity.Aar,
                SumOfAar = entity.SumOfAar,
                NetPremium = entity.NetPremium,
                SumOfNetPremium = entity.SumOfNetPremium,
                RetroRatio = entity.RetroRatio,
                RetentionLimit = entity.RetentionLimit,
                DistributedRetentionLimit = entity.DistributedRetentionLimit,
                RetroAmount = entity.RetroAmount,
                DistributedRetroAmount = entity.DistributedRetroAmount,
                AccumulativeRetainAmount = entity.AccumulativeRetainAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroIndicator = entity.RetroIndicator,
                Errors = entity.Errors,
                RetroBenefitCode = entity.PerLifeAggregationDetailData.RetroBenefitCode, //TODO: To be confirm

                // RI Data Warehouse History
                Quarter = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Quarter,
                EndingPolicyStatus = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.EndingPolicyStatus,
                RecordType = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RecordType,
                TreatyCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TreatyCode,
                ReinsBasisCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReinsBasisCode,
                FundsAccountingTypeCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PremiumFrequencyCode,
                ReportPeriodMonth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReportPeriodMonth,
                ReportPeriodYear = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReportPeriodYear,
                RiskPeriodMonth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiskPeriodMonth,
                RiskPeriodYear = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiskPeriodYear,
                TransactionTypeCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TransactionTypeCode,
                PolicyNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyNumber,
                IssueDatePol = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.IssueDatePol,
                IssueDateBen = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.IssueDateBen,
                ReinsEffDatePol = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReinsEffDatePol,
                ReinsEffDateBen = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReinsEffDateBen,
                CedingPlanCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingPlanCode,
                CedingBenefitTypeCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingBenefitRiskCode,
                MlreBenefitCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreBenefitCode,
                OriSumAssured = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.OriSumAssured,
                CurrSumAssured = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CurrSumAssured,
                AmountCededB4MlreShare = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AmountCededB4MlreShare,
                RetentionAmount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetentionAmount,
                AarOri = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarOri,
                //Aar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Aar,
                AarSpecial1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarSpecial1,
                AarSpecial2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarSpecial2,
                AarSpecial3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarSpecial3,
                InsuredName = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredName,
                InsuredGenderCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredGenderCode,
                InsuredTobaccoUse = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredDateOfBirth,
                InsuredOccupationCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredOccupationCode,
                InsuredRegisterNo = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredRegisterNo,
                InsuredAttainedAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredAttainedAge,
                InsuredNewIcNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredNewIcNumber,
                InsuredOldIcNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredOldIcNumber,
                InsuredName2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredName2nd,
                InsuredGenderCode2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredGenderCode2nd,
                InsuredTobaccoUse2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredTobaccoUse2nd,
                InsuredDateOfBirth2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredDateOfBirth2nd,
                InsuredAttainedAge2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredAttainedAge2nd,
                InsuredNewIcNumber2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredNewIcNumber2nd,
                InsuredOldIcNumber2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredOldIcNumber2nd,
                ReinsuranceIssueAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReinsuranceIssueAge,
                ReinsuranceIssueAge2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ReinsuranceIssueAge2nd,
                PolicyTerm = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyTerm,
                PolicyExpiryDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyExpiryDate,
                DurationYear = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DurationYear,
                DurationDay = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DurationDay,
                DurationMonth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DurationMonth,
                PremiumCalType = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PremiumCalType,
                CedantRiRate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedantRiRate,
                RateTable = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RateTable,
                AgeRatedUp = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AgeRatedUp,
                DiscountRate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DiscountRate,
                LoadingType = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LoadingType,
                UnderwriterRating = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRating,
                UnderwriterRatingUnit = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingUnit,
                UnderwriterRatingTerm = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingTerm,
                UnderwriterRating2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRating2,
                UnderwriterRatingUnit2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingUnit2,
                UnderwriterRatingTerm2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingTerm2,
                UnderwriterRating3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRating3,
                UnderwriterRatingUnit3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingUnit3,
                UnderwriterRatingTerm3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UnderwriterRatingTerm3,
                FlatExtraAmount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraAmount,
                FlatExtraUnit = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraUnit,
                FlatExtraTerm = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraTerm,
                FlatExtraAmount2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraAmount2,
                FlatExtraTerm2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraTerm2,
                StandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.StandardPremium,
                SubstandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SubstandardPremium,
                FlatExtraPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraPremium,
                GrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GrossPremium,
                StandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.StandardDiscount,
                SubstandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SubstandardDiscount,
                VitalityDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.VitalityDiscount,
                TotalDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TotalDiscount,
                //NetPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NetPremium,
                AnnualRiPrem = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AnnualRiPrem,
                RiCovPeriod = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiCovPeriod,
                AdjBeginDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AdjBeginDate,
                AdjEndDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AdjEndDate,
                PolicyNumberOld = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyNumberOld,
                PolicyStatusCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyStatusCode,
                PolicyGrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyGrossPremium,
                PolicyStandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyStandardPremium,
                PolicySubstandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicySubstandardPremium,
                PolicyTermRemain = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyTermRemain,
                PolicyAmountDeath = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyAmountDeath,
                PolicyReserve = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyReserve,
                PolicyPaymentMethod = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyPaymentMethod,
                PolicyLifeNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyLifeNumber,
                FundCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FundCode,
                LineOfBusiness = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LineOfBusiness,
                ApLoading = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ApLoading,
                LoanInterestRate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LoanInterestRate,
                DefermentPeriod = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DefermentPeriod,
                RiderNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiderNumber,
                CampaignCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CampaignCode,
                Nationality = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Nationality,
                TerritoryOfIssueCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TerritoryOfIssueCode,
                CurrencyCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CurrencyCode,
                StaffPlanIndicator = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.StaffPlanIndicator,
                CedingTreatyCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingTreatyCode,
                CedingPlanCodeOld = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingPlanCodeOld,
                CedingBasicPlanCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingBasicPlanCode,
                CedantSar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedantSar,
                CedantReinsurerCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedantReinsurerCode,
                AmountCededB4MlreShare2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AmountCededB4MlreShare2,
                CessionCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CessionCode,
                CedantRemark = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedantRemark,
                GroupPolicyNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupPolicyNumber,
                GroupPolicyName = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupPolicyName,
                NoOfEmployee = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NoOfEmployee,
                PolicyTotalLive = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyTotalLive,
                GroupSubsidiaryName = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupSubsidiaryNo,
                GroupEmployeeBasicSalary = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupEmployeeBasicSalary,
                GroupEmployeeJobType = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupEmployeeJobType,
                GroupEmployeeJobCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupEmployeeJobCode,
                GroupEmployeeBasicSalaryRevise = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupEmployeeBasicSalaryRevise,
                GroupEmployeeBasicSalaryMultiplier = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupEmployeeBasicSalaryMultiplier,
                CedingPlanCode2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CedingPlanCode2,
                DependantIndicator = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DependantIndicator,
                GhsRoomBoard = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GhsRoomBoard,
                PolicyAmountSubstandard = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyAmountSubstandard,
                Layer1RiShare = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1RiShare,
                Layer1InsuredAttainedAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1InsuredAttainedAge,
                Layer1InsuredAttainedAge2nd = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1InsuredAttainedAge2nd,
                Layer1StandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1StandardPremium,
                Layer1SubstandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1SubstandardPremium,
                Layer1GrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1GrossPremium,
                Layer1StandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1StandardDiscount,
                Layer1SubstandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1SubstandardDiscount,
                Layer1TotalDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1TotalDiscount,
                Layer1NetPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1NetPremium,
                Layer1GrossPremiumAlt = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1GrossPremiumAlt,
                Layer1TotalDiscountAlt = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1TotalDiscountAlt,
                Layer1NetPremiumAlt = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1NetPremiumAlt,
                SpecialIndicator1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SpecialIndicator1,
                SpecialIndicator2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SpecialIndicator2,
                SpecialIndicator3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SpecialIndicator3,
                IndicatorJointLife = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.IndicatorJointLife,
                TaxAmount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TaxAmount,
                GstIndicator = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GstIndicator,
                GstGrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GstGrossPremium,
                GstTotalDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GstTotalDiscount,
                GstVitality = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GstVitality,
                GstAmount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GstAmount,
                Mfrs17BasicRider = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Mfrs17BasicRider,
                Mfrs17CellName = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Mfrs17CellName,
                Mfrs17TreatyCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Mfrs17TreatyCode,
                LoaCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LoaCode,
                NoClaimBonus = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NoClaimBonus,
                SurrenderValue = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SurrenderValue,
                DatabaseCommision = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.DatabaseCommision,
                GrossPremiumAlt = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GrossPremiumAlt,
                NetPremiumAlt = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NetPremiumAlt,
                Layer1FlatExtraPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Layer1FlatExtraPremium,
                TransactionPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TransactionPremium,
                OriginalPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.OriginalPremium,
                TransactionDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TransactionDiscount,
                OriginalDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.OriginalDiscount,
                BrokerageFee = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.BrokerageFee,
                MaxUwRating = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxUwRating,
                RetentionCap = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetentionCap,
                AarCap = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarCap,
                RiRate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiRate,
                RiRate2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiRate2,
                AnnuityFactor = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AnnuityFactor,
                SumAssuredOffered = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SumAssuredOffered,
                UwRatingOffered = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UwRatingOffered,
                FlatExtraAmountOffered = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraAmountOffered,
                FlatExtraDuration = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraDuration,
                EffectiveDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.EffectiveDate,
                OfferLetterSentDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.OfferLetterSentDate,
                RiskPeriodStartDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiskPeriodEndDate,
                Mfrs17AnnualCohort = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Mfrs17AnnualCohort,
                MaxExpiryAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxExpiryAge,
                MinIssueAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MinIssueAge,
                MaxIssueAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxIssueAge,
                MinAar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MinAar,
                MaxAar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxAar,
                CorridorLimit = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CorridorLimit,
                Abl = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Abl,
                RatePerBasisUnit = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RatePerBasisUnit,
                RiDiscountRate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RiDiscountRate,
                LargeSaDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LargeSaDiscount,
                GroupSizeDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.GroupSizeDiscount,
                EwarpNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.EwarpNumber,
                EwarpActionCode = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.EwarpActionCode,
                RetentionShare = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetentionShare,
                AarShare = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarShare,
                ProfitComm = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ProfitComm,
                TotalDirectRetroAar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TotalDirectRetroAar,
                TotalDirectRetroGrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TotalDirectRetroGrossPremium,
                TotalDirectRetroDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TotalDirectRetroDiscount,
                TotalDirectRetroNetPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TotalDirectRetroNetPremium,
                TreatyType = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TreatyType,
                MaxApLoading = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxApLoading,
                MlreInsuredAttainedAgeAtCurrentMonth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreInsuredAttainedAgeAtCurrentMonth,
                MlreInsuredAttainedAgeAtPreviousMonth = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreInsuredAttainedAgeAtPreviousMonth,
                InsuredAttainedAgeCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.InsuredAttainedAgeCheck,
                MaxExpiryAgeCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxExpiryAgeCheck,
                MlrePolicyIssueAge = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlrePolicyIssueAge,
                PolicyIssueAgeCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.PolicyIssueAgeCheck,
                MinIssueAgeCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MinIssueAgeCheck,
                MaxIssueAgeCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxIssueAgeCheck,
                MaxUwRatingCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxUwRatingCheck,
                ApLoadingCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ApLoadingCheck,
                EffectiveDateCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.EffectiveDateCheck,
                MinAarCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MinAarCheck,
                MaxAarCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MaxAarCheck,
                CorridorLimitCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.CorridorLimitCheck,
                AblCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AblCheck,
                RetentionCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetentionCheck,
                AarCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarCheck,
                MlreStandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreStandardPremium,
                MlreSubstandardPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreSubstandardPremium,
                MlreFlatExtraPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreFlatExtraPremium,
                MlreGrossPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreGrossPremium,
                MlreStandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreStandardDiscount,
                MlreSubstandardDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreSubstandardDiscount,
                MlreLargeSaDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreLargeSaDiscount,
                MlreGroupSizeDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreGroupSizeDiscount,
                MlreVitalityDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreVitalityDiscount,
                MlreTotalDiscount = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreTotalDiscount,
                MlreNetPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreNetPremium,
                NetPremiumCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NetPremiumCheck,
                ServiceFeePercentage = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ServiceFeePercentage,
                ServiceFee = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ServiceFee,
                MlreBrokerageFee = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreBrokerageFee,
                MlreDatabaseCommission = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.MlreDatabaseCommission,
                ValidityDayCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.ValidityDayCheck,
                SumAssuredOfferedCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.SumAssuredOfferedCheck,
                UwRatingCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.UwRatingCheck,
                FlatExtraAmountCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraAmountCheck,
                FlatExtraDurationCheck = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.FlatExtraDurationCheck,
                LastUpdatedDate = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.LastUpdatedDate,
                AarShare2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarShare2,
                AarCap2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.AarCap2,
                WakalahFeePercentage = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.WakalahFeePercentage,
                TreatyNumber = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.TreatyNumber,

                // Direct Retro
                RetroParty1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroParty1,
                RetroParty2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroParty2,
                RetroParty3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroParty3,
                RetroShare1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroShare1,
                RetroShare2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroShare2,
                RetroShare3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroShare3,
                RetroAar1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroAar1,
                RetroAar2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroAar2,
                RetroAar3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroAar3,
                RetroReinsurancePremium1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroReinsurancePremium1,
                RetroReinsurancePremium2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroReinsurancePremium2,
                RetroReinsurancePremium3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroReinsurancePremium3,
                RetroDiscount1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroDiscount1,
                RetroDiscount2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroDiscount2,
                RetroDiscount3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroDiscount3,
                RetroNetPremium1 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroNetPremium1,
                RetroNetPremium2 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroNetPremium2,
                RetroNetPremium3 = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.RetroNetPremium3,
            };
        }

        public static PerLifeAggregationMonthlyDataBo FormBo(PerLifeAggregationMonthlyData entity = null, bool loadWarehouseValues = false)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeAggregationMonthlyDataBo
            {
                Id = entity.Id,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                PerLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(entity.PerLifeAggregationDetailDataId),
                RiskYear = entity.RiskYear,
                RiskMonth = entity.RiskMonth,
                UniqueKeyPerLife = entity.UniqueKeyPerLife,
                RetroPremFreq = entity.RetroPremFreq,
                Aar = entity.Aar,
                SumOfAar = entity.SumOfAar,
                NetPremium = entity.NetPremium,
                RetroRatio = entity.RetroRatio,
                SumOfNetPremium = entity.SumOfNetPremium,
                RetentionLimit = entity.RetentionLimit,
                DistributedRetentionLimit = entity.DistributedRetentionLimit,
                RetroAmount = entity.RetroAmount,
                DistributedRetroAmount = entity.DistributedRetroAmount,
                AccumulativeRetainAmount = entity.AccumulativeRetainAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroIndicator = entity.RetroIndicator,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (loadWarehouseValues)
            {
                var warehouseBo = bo.PerLifeAggregationDetailDataBo.RiDataWarehouseHistoryBo;
                // RI Data Warehouse History
                bo.Quarter = warehouseBo.Quarter;
                bo.EndingPolicyStatus = warehouseBo.EndingPolicyStatus;
                bo.RecordType = warehouseBo.RecordType;
                bo.TreatyCode = warehouseBo.TreatyCode;
                bo.ReinsBasisCode = warehouseBo.ReinsBasisCode;
                bo.FundsAccountingTypeCode = warehouseBo.FundsAccountingTypeCode;
                bo.PremiumFrequencyCode = warehouseBo.PremiumFrequencyCode;
                bo.ReportPeriodMonth = warehouseBo.ReportPeriodMonth;
                bo.ReportPeriodYear = warehouseBo.ReportPeriodYear;
                bo.RiskPeriodMonth = warehouseBo.RiskPeriodMonth;
                bo.RiskPeriodYear = warehouseBo.RiskPeriodYear;
                bo.TransactionTypeCode = warehouseBo.TransactionTypeCode;
                bo.PolicyNumber = warehouseBo.PolicyNumber;
                bo.IssueDatePol = warehouseBo.IssueDatePol;
                bo.IssueDateBen = warehouseBo.IssueDateBen;
                bo.ReinsEffDatePol = warehouseBo.ReinsEffDatePol;
                bo.ReinsEffDateBen = warehouseBo.ReinsEffDateBen;
                bo.CedingPlanCode = warehouseBo.CedingPlanCode;
                bo.CedingBenefitTypeCode = warehouseBo.CedingBenefitTypeCode;
                bo.CedingBenefitRiskCode = warehouseBo.CedingBenefitRiskCode;
                bo.MlreBenefitCode = warehouseBo.MlreBenefitCode;
                bo.OriSumAssured = warehouseBo.OriSumAssured;
                bo.CurrSumAssured = warehouseBo.CurrSumAssured;
                bo.AmountCededB4MlreShare = warehouseBo.AmountCededB4MlreShare;
                bo.RetentionAmount = warehouseBo.RetentionAmount;
                bo.AarOri = warehouseBo.AarOri;
                bo.AarSpecial1 = warehouseBo.AarSpecial1;
                bo.AarSpecial2 = warehouseBo.AarSpecial2;
                bo.AarSpecial3 = warehouseBo.AarSpecial3;
                bo.InsuredName = warehouseBo.InsuredName;
                bo.InsuredGenderCode = warehouseBo.InsuredGenderCode;
                bo.InsuredTobaccoUse = warehouseBo.InsuredTobaccoUse;
                bo.InsuredDateOfBirth = warehouseBo.InsuredDateOfBirth;
                bo.InsuredOccupationCode = warehouseBo.InsuredOccupationCode;
                bo.InsuredRegisterNo = warehouseBo.InsuredRegisterNo;
                bo.InsuredAttainedAge = warehouseBo.InsuredAttainedAge;
                bo.InsuredNewIcNumber = warehouseBo.InsuredNewIcNumber;
                bo.InsuredOldIcNumber = warehouseBo.InsuredOldIcNumber;
                bo.InsuredName2nd = warehouseBo.InsuredName2nd;
                bo.InsuredGenderCode2nd = warehouseBo.InsuredGenderCode2nd;
                bo.InsuredTobaccoUse2nd = warehouseBo.InsuredTobaccoUse2nd;
                bo.InsuredDateOfBirth2nd = warehouseBo.InsuredDateOfBirth2nd;
                bo.InsuredAttainedAge2nd = warehouseBo.InsuredAttainedAge2nd;
                bo.InsuredNewIcNumber2nd = warehouseBo.InsuredNewIcNumber2nd;
                bo.InsuredOldIcNumber2nd = warehouseBo.InsuredOldIcNumber2nd;
                bo.ReinsuranceIssueAge = warehouseBo.ReinsuranceIssueAge;
                bo.ReinsuranceIssueAge2nd = warehouseBo.ReinsuranceIssueAge2nd;
                bo.PolicyTerm = warehouseBo.PolicyTerm;
                bo.PolicyExpiryDate = warehouseBo.PolicyExpiryDate;
                bo.DurationYear = warehouseBo.DurationYear;
                bo.DurationDay = warehouseBo.DurationDay;
                bo.DurationMonth = warehouseBo.DurationMonth;
                bo.PremiumCalType = warehouseBo.PremiumCalType;
                bo.CedantRiRate = warehouseBo.CedantRiRate;
                bo.RateTable = warehouseBo.RateTable;
                bo.AgeRatedUp = warehouseBo.AgeRatedUp;
                bo.DiscountRate = warehouseBo.DiscountRate;
                bo.LoadingType = warehouseBo.LoadingType;
                bo.UnderwriterRating = warehouseBo.UnderwriterRating;
                bo.UnderwriterRatingUnit = warehouseBo.UnderwriterRatingUnit;
                bo.UnderwriterRatingTerm = warehouseBo.UnderwriterRatingTerm;
                bo.UnderwriterRating2 = warehouseBo.UnderwriterRating2;
                bo.UnderwriterRatingUnit2 = warehouseBo.UnderwriterRatingUnit2;
                bo.UnderwriterRatingTerm2 = warehouseBo.UnderwriterRatingTerm2;
                bo.UnderwriterRating3 = warehouseBo.UnderwriterRating3;
                bo.UnderwriterRatingUnit3 = warehouseBo.UnderwriterRatingUnit3;
                bo.UnderwriterRatingTerm3 = warehouseBo.UnderwriterRatingTerm3;
                bo.FlatExtraAmount = warehouseBo.FlatExtraAmount;
                bo.FlatExtraUnit = warehouseBo.FlatExtraUnit;
                bo.FlatExtraTerm = warehouseBo.FlatExtraTerm;
                bo.FlatExtraAmount2 = warehouseBo.FlatExtraAmount2;
                bo.FlatExtraTerm2 = warehouseBo.FlatExtraTerm2;
                bo.StandardPremium = warehouseBo.StandardPremium;
                bo.SubstandardPremium = warehouseBo.SubstandardPremium;
                bo.FlatExtraPremium = warehouseBo.FlatExtraPremium;
                bo.GrossPremium = warehouseBo.GrossPremium;
                bo.StandardDiscount = warehouseBo.StandardDiscount;
                bo.SubstandardDiscount = warehouseBo.SubstandardDiscount;
                bo.VitalityDiscount = warehouseBo.VitalityDiscount;
                bo.TotalDiscount = warehouseBo.TotalDiscount;
                bo.AnnualRiPrem = warehouseBo.AnnualRiPrem;
                bo.RiCovPeriod = warehouseBo.RiCovPeriod;
                bo.AdjBeginDate = warehouseBo.AdjBeginDate;
                bo.AdjEndDate = warehouseBo.AdjEndDate;
                bo.PolicyNumberOld = warehouseBo.PolicyNumberOld;
                bo.PolicyStatusCode = warehouseBo.PolicyStatusCode;
                bo.PolicyGrossPremium = warehouseBo.PolicyGrossPremium;
                bo.PolicyStandardPremium = warehouseBo.PolicyStandardPremium;
                bo.PolicySubstandardPremium = warehouseBo.PolicySubstandardPremium;
                bo.PolicyTermRemain = warehouseBo.PolicyTermRemain;
                bo.PolicyAmountDeath = warehouseBo.PolicyAmountDeath;
                bo.PolicyReserve = warehouseBo.PolicyReserve;
                bo.PolicyPaymentMethod = warehouseBo.PolicyPaymentMethod;
                bo.PolicyLifeNumber = warehouseBo.PolicyLifeNumber;
                bo.FundCode = warehouseBo.FundCode;
                bo.LineOfBusiness = warehouseBo.LineOfBusiness;
                bo.ApLoading = warehouseBo.ApLoading;
                bo.LoanInterestRate = warehouseBo.LoanInterestRate;
                bo.DefermentPeriod = warehouseBo.DefermentPeriod;
                bo.RiderNumber = warehouseBo.RiderNumber;
                bo.CampaignCode = warehouseBo.CampaignCode;
                bo.Nationality = warehouseBo.Nationality;
                bo.TerritoryOfIssueCode = warehouseBo.TerritoryOfIssueCode;
                bo.CurrencyCode = warehouseBo.CurrencyCode;
                bo.StaffPlanIndicator = warehouseBo.StaffPlanIndicator;
                bo.CedingTreatyCode = warehouseBo.CedingTreatyCode;
                bo.CedingPlanCodeOld = warehouseBo.CedingPlanCodeOld;
                bo.CedingBasicPlanCode = warehouseBo.CedingBasicPlanCode;
                bo.CedantSar = warehouseBo.CedantSar;
                bo.CedantReinsurerCode = warehouseBo.CedantReinsurerCode;
                bo.AmountCededB4MlreShare2 = warehouseBo.AmountCededB4MlreShare2;
                bo.CessionCode = warehouseBo.CessionCode;
                bo.CedantRemark = warehouseBo.CedantRemark;
                bo.GroupPolicyNumber = warehouseBo.GroupPolicyNumber;
                bo.GroupPolicyName = warehouseBo.GroupPolicyName;
                bo.NoOfEmployee = warehouseBo.NoOfEmployee;
                bo.PolicyTotalLive = warehouseBo.PolicyTotalLive;
                bo.GroupSubsidiaryName = warehouseBo.GroupSubsidiaryName;
                bo.GroupSubsidiaryNo = warehouseBo.GroupSubsidiaryNo;
                bo.GroupEmployeeBasicSalary = warehouseBo.GroupEmployeeBasicSalary;
                bo.GroupEmployeeJobType = warehouseBo.GroupEmployeeJobType;
                bo.GroupEmployeeJobCode = warehouseBo.GroupEmployeeJobCode;
                bo.GroupEmployeeBasicSalaryRevise = warehouseBo.GroupEmployeeBasicSalaryRevise;
                bo.GroupEmployeeBasicSalaryMultiplier = warehouseBo.GroupEmployeeBasicSalaryMultiplier;
                bo.CedingPlanCode2 = warehouseBo.CedingPlanCode2;
                bo.DependantIndicator = warehouseBo.DependantIndicator;
                bo.GhsRoomBoard = warehouseBo.GhsRoomBoard;
                bo.PolicyAmountSubstandard = warehouseBo.PolicyAmountSubstandard;
                bo.Layer1RiShare = warehouseBo.Layer1RiShare;
                bo.Layer1InsuredAttainedAge = warehouseBo.Layer1InsuredAttainedAge;
                bo.Layer1InsuredAttainedAge2nd = warehouseBo.Layer1InsuredAttainedAge2nd;
                bo.Layer1StandardPremium = warehouseBo.Layer1StandardPremium;
                bo.Layer1SubstandardPremium = warehouseBo.Layer1SubstandardPremium;
                bo.Layer1GrossPremium = warehouseBo.Layer1GrossPremium;
                bo.Layer1StandardDiscount = warehouseBo.Layer1StandardDiscount;
                bo.Layer1SubstandardDiscount = warehouseBo.Layer1SubstandardDiscount;
                bo.Layer1TotalDiscount = warehouseBo.Layer1TotalDiscount;
                bo.Layer1NetPremium = warehouseBo.Layer1NetPremium;
                bo.Layer1GrossPremiumAlt = warehouseBo.Layer1GrossPremiumAlt;
                bo.Layer1TotalDiscountAlt = warehouseBo.Layer1TotalDiscountAlt;
                bo.Layer1NetPremiumAlt = warehouseBo.Layer1NetPremiumAlt;
                bo.SpecialIndicator1 = warehouseBo.SpecialIndicator1;
                bo.SpecialIndicator2 = warehouseBo.SpecialIndicator2;
                bo.SpecialIndicator3 = warehouseBo.SpecialIndicator3;
                bo.IndicatorJointLife = warehouseBo.IndicatorJointLife;
                bo.TaxAmount = warehouseBo.TaxAmount;
                bo.GstIndicator = warehouseBo.GstIndicator;
                bo.GstGrossPremium = warehouseBo.GstGrossPremium;
                bo.GstTotalDiscount = warehouseBo.GstTotalDiscount;
                bo.GstVitality = warehouseBo.GstVitality;
                bo.GstAmount = warehouseBo.GstAmount;
                bo.Mfrs17BasicRider = warehouseBo.Mfrs17BasicRider;
                bo.Mfrs17CellName = warehouseBo.Mfrs17CellName;
                bo.Mfrs17TreatyCode = warehouseBo.Mfrs17TreatyCode;
                bo.LoaCode = warehouseBo.LoaCode;
                bo.CurrencyRate = warehouseBo.CurrencyRate;
                bo.NoClaimBonus = warehouseBo.NoClaimBonus;
                bo.SurrenderValue = warehouseBo.SurrenderValue;
                bo.DatabaseCommision = warehouseBo.DatabaseCommision;
                bo.GrossPremiumAlt = warehouseBo.GrossPremiumAlt;
                bo.NetPremiumAlt = warehouseBo.NetPremiumAlt;
                bo.Layer1FlatExtraPremium = warehouseBo.Layer1FlatExtraPremium;
                bo.TransactionPremium = warehouseBo.TransactionPremium;
                bo.OriginalPremium = warehouseBo.OriginalPremium;
                bo.TransactionDiscount = warehouseBo.TransactionDiscount;
                bo.OriginalDiscount = warehouseBo.OriginalDiscount;
                bo.BrokerageFee = warehouseBo.BrokerageFee;
                bo.MaxUwRating = warehouseBo.MaxUwRating;
                bo.RetentionCap = warehouseBo.RetentionCap;
                bo.AarCap = warehouseBo.AarCap;
                bo.RiRate = warehouseBo.RiRate;
                bo.RiRate2 = warehouseBo.RiRate2;
                bo.AnnuityFactor = warehouseBo.AnnuityFactor;
                bo.SumAssuredOffered = warehouseBo.SumAssuredOffered;
                bo.UwRatingOffered = warehouseBo.UwRatingOffered;
                bo.FlatExtraAmountOffered = warehouseBo.FlatExtraAmountOffered;
                bo.FlatExtraDuration = warehouseBo.FlatExtraDuration;
                bo.EffectiveDate = warehouseBo.EffectiveDate;
                bo.OfferLetterSentDate = warehouseBo.OfferLetterSentDate;
                bo.RiskPeriodStartDate = warehouseBo.RiskPeriodStartDate;
                bo.RiskPeriodEndDate = warehouseBo.RiskPeriodEndDate;
                bo.Mfrs17AnnualCohort = warehouseBo.Mfrs17AnnualCohort;
                bo.MaxExpiryAge = warehouseBo.MaxExpiryAge;
                bo.MinIssueAge = warehouseBo.MinIssueAge;
                bo.MaxIssueAge = warehouseBo.MaxIssueAge;
                bo.MinAar = warehouseBo.MinAar;
                bo.MaxAar = warehouseBo.MaxAar;
                bo.CorridorLimit = warehouseBo.CorridorLimit;
                bo.Abl = warehouseBo.Abl;
                bo.RatePerBasisUnit = warehouseBo.RatePerBasisUnit;
                bo.RiDiscountRate = warehouseBo.RiDiscountRate;
                bo.LargeSaDiscount = warehouseBo.LargeSaDiscount;
                bo.GroupSizeDiscount = warehouseBo.GroupSizeDiscount;
                bo.EwarpNumber = warehouseBo.EwarpNumber;
                bo.EwarpActionCode = warehouseBo.EwarpActionCode;
                bo.RetentionShare = warehouseBo.RetentionShare;
                bo.AarShare = warehouseBo.AarShare;
                bo.ProfitComm = warehouseBo.ProfitComm;
                bo.TotalDirectRetroAar = warehouseBo.TotalDirectRetroAar;
                bo.TotalDirectRetroGrossPremium = warehouseBo.TotalDirectRetroGrossPremium;
                bo.TotalDirectRetroDiscount = warehouseBo.TotalDirectRetroDiscount;
                bo.TotalDirectRetroNetPremium = warehouseBo.TotalDirectRetroNetPremium;
                bo.TreatyType = warehouseBo.TreatyType;
                bo.MaxApLoading = warehouseBo.MaxApLoading;
                bo.MlreInsuredAttainedAgeAtCurrentMonth = warehouseBo.MlreInsuredAttainedAgeAtCurrentMonth;
                bo.MlreInsuredAttainedAgeAtPreviousMonth = warehouseBo.MlreInsuredAttainedAgeAtPreviousMonth;
                bo.InsuredAttainedAgeCheck = warehouseBo.InsuredAttainedAgeCheck;
                bo.MaxExpiryAgeCheck = warehouseBo.MaxExpiryAgeCheck;
                bo.MlrePolicyIssueAge = warehouseBo.MlrePolicyIssueAge;
                bo.PolicyIssueAgeCheck = warehouseBo.PolicyIssueAgeCheck;
                bo.MinIssueAgeCheck = warehouseBo.MinIssueAgeCheck;
                bo.MaxIssueAgeCheck = warehouseBo.MaxIssueAgeCheck;
                bo.MaxUwRatingCheck = warehouseBo.MaxUwRatingCheck;
                bo.ApLoadingCheck = warehouseBo.ApLoadingCheck;
                bo.EffectiveDateCheck = warehouseBo.EffectiveDateCheck;
                bo.MinAarCheck = warehouseBo.MinAarCheck;
                bo.MaxAarCheck = warehouseBo.MaxAarCheck;
                bo.CorridorLimitCheck = warehouseBo.CorridorLimitCheck;
                bo.AblCheck = warehouseBo.AblCheck;
                bo.RetentionCheck = warehouseBo.RetentionCheck;
                bo.AarCheck = warehouseBo.AarCheck;
                bo.MlreStandardPremium = warehouseBo.MlreStandardPremium;
                bo.MlreSubstandardPremium = warehouseBo.MlreSubstandardPremium;
                bo.MlreFlatExtraPremium = warehouseBo.MlreFlatExtraPremium;
                bo.MlreGrossPremium = warehouseBo.MlreGrossPremium;
                bo.MlreStandardDiscount = warehouseBo.MlreStandardDiscount;
                bo.MlreSubstandardDiscount = warehouseBo.MlreSubstandardDiscount;
                bo.MlreLargeSaDiscount = warehouseBo.MlreLargeSaDiscount;
                bo.MlreGroupSizeDiscount = warehouseBo.MlreGroupSizeDiscount;
                bo.MlreVitalityDiscount = warehouseBo.MlreVitalityDiscount;
                bo.MlreTotalDiscount = warehouseBo.MlreTotalDiscount;
                bo.MlreNetPremium = warehouseBo.MlreNetPremium;
                bo.NetPremiumCheck = warehouseBo.NetPremiumCheck;
                bo.ServiceFeePercentage = warehouseBo.ServiceFeePercentage;
                bo.ServiceFee = warehouseBo.ServiceFee;
                bo.MlreBrokerageFee = warehouseBo.MlreBrokerageFee;
                bo.MlreDatabaseCommission = warehouseBo.MlreDatabaseCommission;
                bo.ValidityDayCheck = warehouseBo.ValidityDayCheck;
                bo.SumAssuredOfferedCheck = warehouseBo.SumAssuredOfferedCheck;
                bo.UwRatingCheck = warehouseBo.UwRatingCheck;
                bo.FlatExtraAmountCheck = warehouseBo.FlatExtraAmountCheck;
                bo.FlatExtraDurationCheck = warehouseBo.FlatExtraDurationCheck;
                bo.LastUpdatedDate = warehouseBo.LastUpdatedDate;
                bo.AarShare2 = warehouseBo.AarShare2;
                bo.AarCap2 = warehouseBo.AarCap2;
                bo.WakalahFeePercentage = warehouseBo.WakalahFeePercentage;
                bo.TreatyNumber = warehouseBo.TreatyNumber;

                bo.RetroParty1 = warehouseBo.RetroParty1;
                bo.RetroParty2 = warehouseBo.RetroParty2;
                bo.RetroParty3 = warehouseBo.RetroParty3;
                bo.RetroShare1 = warehouseBo.RetroShare1;
                bo.RetroShare2 = warehouseBo.RetroShare2;
                bo.RetroShare3 = warehouseBo.RetroShare3;
                bo.RetroAar1 = warehouseBo.RetroAar1;
                bo.RetroAar2 = warehouseBo.RetroAar2;
                bo.RetroAar3 = warehouseBo.RetroAar3;
                bo.RetroReinsurancePremium1 = warehouseBo.RetroReinsurancePremium1;
                bo.RetroReinsurancePremium2 = warehouseBo.RetroReinsurancePremium2;
                bo.RetroReinsurancePremium3 = warehouseBo.RetroReinsurancePremium3;
                bo.RetroDiscount1 = warehouseBo.RetroDiscount1;
                bo.RetroDiscount2 = warehouseBo.RetroDiscount2;
                bo.RetroDiscount3 = warehouseBo.RetroDiscount3;
                bo.RetroNetPremium1 = warehouseBo.RetroNetPremium1;
                bo.RetroNetPremium2 = warehouseBo.RetroNetPremium2;
                bo.RetroNetPremium3 = warehouseBo.RetroNetPremium3;

            }
            return bo;
        }

        public static IList<PerLifeAggregationMonthlyDataBo> FormBos(IList<PerLifeAggregationMonthlyData> entities = null, bool loadWarehouseValues = false)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationMonthlyDataBo> bos = new List<PerLifeAggregationMonthlyDataBo>() { };
            foreach (PerLifeAggregationMonthlyData entity in entities)
            {
                bos.Add(FormBo(entity, loadWarehouseValues));
            }
            return bos;
        }

        public static PerLifeAggregationMonthlyData FormEntity(PerLifeAggregationMonthlyDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationMonthlyData
            {
                Id = bo.Id,
                PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId,
                RiskYear = bo.RiskYear,
                RiskMonth = bo.RiskMonth,
                UniqueKeyPerLife = bo.UniqueKeyPerLife,
                RetroPremFreq = bo.RetroPremFreq,
                Aar = bo.Aar,
                SumOfAar = bo.SumOfAar,
                NetPremium = bo.NetPremium,
                SumOfNetPremium = bo.SumOfNetPremium,
                RetroRatio = bo.RetroRatio,
                RetentionLimit = bo.RetentionLimit,
                DistributedRetentionLimit = bo.DistributedRetentionLimit,
                RetroAmount = bo.RetroAmount,
                DistributedRetroAmount = bo.DistributedRetroAmount,
                AccumulativeRetainAmount = bo.AccumulativeRetainAmount,
                RetroGrossPremium = bo.RetroGrossPremium,
                RetroNetPremium = bo.RetroNetPremium,
                RetroDiscount = bo.RetroDiscount,
                RetroIndicator = bo.RetroIndicator,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationMonthlyData.IsExists(id);
        }

        public static PerLifeAggregationMonthlyDataBo Find(int? id)
        {
            return FormBo(PerLifeAggregationMonthlyData.Find(id));
        }

        public static IList<PerLifeAggregationMonthlyDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationMonthlyData.ToList());
            }
        }

        public static IList<PerLifeAggregationMonthlyDataBo> GetByPerLifeAggregationDetailId(int perLifeAggregationDetailId, int? skip = null, int? take = null)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<PerLifeAggregationMonthlyData> query = db.PerLifeAggregationMonthlyData
                    .Where(q => q.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                    .Where(q => q.RetroIndicator)
                    .OrderBy(q => q.Id);

                if (skip.HasValue && take.HasValue)
                {
                    query = query.Skip(skip.Value).Take(take.Value);
                }

                return FormBos(query.ToList(), true);
            }
        }

        public static Result Save(ref PerLifeAggregationMonthlyDataBo bo)
        {
            if (!PerLifeAggregationMonthlyData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationMonthlyDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationMonthlyData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationMonthlyDataBo bo)
        {
            PerLifeAggregationMonthlyData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationMonthlyDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationMonthlyDataBo bo)
        {
            Result result = Result();

            PerLifeAggregationMonthlyData entity = PerLifeAggregationMonthlyData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId;
                entity.RiskYear = bo.RiskYear;
                entity.RiskMonth = bo.RiskMonth;
                entity.UniqueKeyPerLife = bo.UniqueKeyPerLife;
                entity.RetroPremFreq = bo.RetroPremFreq;
                entity.Aar = bo.Aar;
                entity.SumOfAar = bo.SumOfAar;
                entity.NetPremium = bo.NetPremium;
                entity.SumOfNetPremium = bo.SumOfNetPremium;
                entity.RetroRatio = bo.RetroRatio;
                entity.RetentionLimit = bo.RetentionLimit;
                entity.DistributedRetentionLimit = bo.DistributedRetentionLimit;
                entity.RetroAmount = bo.RetroAmount;
                entity.DistributedRetroAmount = bo.DistributedRetroAmount;
                entity.AccumulativeRetainAmount = bo.AccumulativeRetainAmount;
                entity.RetroGrossPremium = bo.RetroGrossPremium;
                entity.RetroNetPremium = bo.RetroNetPremium;
                entity.RetroDiscount = bo.RetroDiscount;
                entity.RetroIndicator = bo.RetroIndicator;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationMonthlyDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationMonthlyDataBo bo)
        {
            PerLifeAggregationDetail.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationMonthlyDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationMonthlyData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailDataId(int perLifeAggregationDetailDataId)
        {
            return PerLifeAggregationMonthlyData.DeleteByPerLifeAggregationDetailDataId(perLifeAggregationDetailDataId);
        }

        public static void DeleteByPerLifeAggregationDetailDataId(int perLifeAggregationDetailDataId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationDetailDataId(perLifeAggregationDetailDataId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregationMonthlyData)));
                }
            }
        }
    }
}
