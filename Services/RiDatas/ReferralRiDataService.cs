using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RiDatas
{
    public class ReferralRiDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ReferralRiData)),
                Controller = ModuleBo.ModuleController.ReferralRiData.ToString(),
            };
        }

        public static ReferralRiDataBo FormBo(ReferralRiData entity = null)
        {
            if (entity == null)
                return null;
            var bo = new ReferralRiDataBo
            {
                Id = entity.Id,
                ReferralRiDataFileId = entity.ReferralRiDataFileId,
                TreatyCode = entity.TreatyCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                ReinsEffDateBen = entity.ReinsEffDateBen,
                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                OriSumAssured = entity.OriSumAssured,
                CurrSumAssured = entity.CurrSumAssured,
                AmountCededB4MlreShare = entity.AmountCededB4MlreShare,
                RetentionAmount = entity.RetentionAmount,
                AarOri = entity.AarOri,
                Aar = entity.Aar,
                AarSpecial1 = entity.AarSpecial1,
                AarSpecial2 = entity.AarSpecial2,
                AarSpecial3 = entity.AarSpecial3,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredOccupationCode = entity.InsuredOccupationCode,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                InsuredNewIcNumber = entity.InsuredNewIcNumber,
                InsuredOldIcNumber = entity.InsuredOldIcNumber,
                InsuredName2nd = entity.InsuredName2nd,
                InsuredGenderCode2nd = entity.InsuredGenderCode2nd,
                InsuredTobaccoUse2nd = entity.InsuredTobaccoUse2nd,
                InsuredDateOfBirth2nd = entity.InsuredDateOfBirth2nd,
                InsuredAttainedAge2nd = entity.InsuredAttainedAge2nd,
                InsuredNewIcNumber2nd = entity.InsuredNewIcNumber2nd,
                InsuredOldIcNumber2nd = entity.InsuredOldIcNumber2nd,
                ReinsuranceIssueAge = entity.ReinsuranceIssueAge,
                ReinsuranceIssueAge2nd = entity.ReinsuranceIssueAge2nd,
                PolicyTerm = entity.PolicyTerm,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                DurationYear = entity.DurationYear,
                DurationDay = entity.DurationDay,
                DurationMonth = entity.DurationMonth,
                PremiumCalType = entity.PremiumCalType,
                CedantRiRate = entity.CedantRiRate,
                RateTable = entity.RateTable,
                AgeRatedUp = entity.AgeRatedUp,
                DiscountRate = entity.DiscountRate,
                LoadingType = entity.LoadingType,
                UnderwriterRating = entity.UnderwriterRating,
                UnderwriterRatingUnit = entity.UnderwriterRatingUnit,
                UnderwriterRatingTerm = entity.UnderwriterRatingTerm,
                UnderwriterRating2 = entity.UnderwriterRating2,
                UnderwriterRatingUnit2 = entity.UnderwriterRatingUnit2,
                UnderwriterRatingTerm2 = entity.UnderwriterRatingTerm2,
                UnderwriterRating3 = entity.UnderwriterRating3,
                UnderwriterRatingUnit3 = entity.UnderwriterRatingUnit3,
                UnderwriterRatingTerm3 = entity.UnderwriterRatingTerm3,
                FlatExtraAmount = entity.FlatExtraAmount,
                FlatExtraUnit = entity.FlatExtraUnit,
                FlatExtraTerm = entity.FlatExtraTerm,
                FlatExtraAmount2 = entity.FlatExtraAmount2,
                FlatExtraTerm2 = entity.FlatExtraTerm2,
                StandardPremium = entity.StandardPremium,
                SubstandardPremium = entity.SubstandardPremium,
                FlatExtraPremium = entity.FlatExtraPremium,
                GrossPremium = entity.GrossPremium,
                StandardDiscount = entity.StandardDiscount,
                SubstandardDiscount = entity.SubstandardDiscount,
                VitalityDiscount = entity.VitalityDiscount,
                TotalDiscount = entity.TotalDiscount,
                NetPremium = entity.NetPremium,
                AnnualRiPrem = entity.AnnualRiPrem,
                RiCovPeriod = entity.RiCovPeriod,
                AdjBeginDate = entity.AdjBeginDate,
                AdjEndDate = entity.AdjEndDate,
                PolicyNumberOld = entity.PolicyNumberOld,
                PolicyStatusCode = entity.PolicyStatusCode,
                PolicyGrossPremium = entity.PolicyGrossPremium,
                PolicyStandardPremium = entity.PolicyStandardPremium,
                PolicySubstandardPremium = entity.PolicySubstandardPremium,
                PolicyTermRemain = entity.PolicyTermRemain,
                PolicyAmountDeath = entity.PolicyAmountDeath,
                PolicyReserve = entity.PolicyReserve,
                PolicyPaymentMethod = entity.PolicyPaymentMethod,
                PolicyLifeNumber = entity.PolicyLifeNumber,
                FundCode = entity.FundCode,
                LineOfBusiness = entity.LineOfBusiness,
                ApLoading = entity.ApLoading,
                LoanInterestRate = entity.LoanInterestRate,
                DefermentPeriod = entity.DefermentPeriod,
                RiderNumber = entity.RiderNumber,
                CampaignCode = entity.CampaignCode,
                Nationality = entity.Nationality,
                TerritoryOfIssueCode = entity.TerritoryOfIssueCode,
                CurrencyCode = entity.CurrencyCode,
                StaffPlanIndicator = entity.StaffPlanIndicator,
                CedingTreatyCode = entity.CedingTreatyCode,
                CedingPlanCodeOld = entity.CedingPlanCodeOld,
                CedingBasicPlanCode = entity.CedingBasicPlanCode,
                CedantSar = entity.CedantSar,
                CedantReinsurerCode = entity.CedantReinsurerCode,
                AmountCededB4MlreShare2 = entity.AmountCededB4MlreShare2,
                CessionCode = entity.CessionCode,
                CedantRemark = entity.CedantRemark,
                GroupPolicyNumber = entity.GroupPolicyNumber,
                GroupPolicyName = entity.GroupPolicyName,
                NoOfEmployee = entity.NoOfEmployee,
                PolicyTotalLive = entity.PolicyTotalLive,
                GroupSubsidiaryName = entity.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.GroupSubsidiaryNo,
                GroupEmployeeBasicSalary = entity.GroupEmployeeBasicSalary,
                GroupEmployeeJobType = entity.GroupEmployeeJobType,
                GroupEmployeeJobCode = entity.GroupEmployeeJobCode,
                GroupEmployeeBasicSalaryRevise = entity.GroupEmployeeBasicSalaryRevise,
                GroupEmployeeBasicSalaryMultiplier = entity.GroupEmployeeBasicSalaryMultiplier,
                CedingPlanCode2 = entity.CedingPlanCode2,
                DependantIndicator = entity.DependantIndicator,
                GhsRoomBoard = entity.GhsRoomBoard,
                PolicyAmountSubstandard = entity.PolicyAmountSubstandard,
                Layer1RiShare = entity.Layer1RiShare,
                Layer1InsuredAttainedAge = entity.Layer1InsuredAttainedAge,
                Layer1InsuredAttainedAge2nd = entity.Layer1InsuredAttainedAge2nd,
                Layer1StandardPremium = entity.Layer1StandardPremium,
                Layer1SubstandardPremium = entity.Layer1SubstandardPremium,
                Layer1GrossPremium = entity.Layer1GrossPremium,
                Layer1StandardDiscount = entity.Layer1StandardDiscount,
                Layer1SubstandardDiscount = entity.Layer1SubstandardDiscount,
                Layer1TotalDiscount = entity.Layer1TotalDiscount,
                Layer1NetPremium = entity.Layer1NetPremium,
                Layer1GrossPremiumAlt = entity.Layer1GrossPremiumAlt,
                Layer1TotalDiscountAlt = entity.Layer1TotalDiscountAlt,
                Layer1NetPremiumAlt = entity.Layer1NetPremiumAlt,
                SpecialIndicator1 = entity.SpecialIndicator1,
                SpecialIndicator2 = entity.SpecialIndicator2,
                SpecialIndicator3 = entity.SpecialIndicator3,
                IndicatorJointLife = entity.IndicatorJointLife,
                TaxAmount = entity.TaxAmount,
                GstIndicator = entity.GstIndicator,
                GstGrossPremium = entity.GstGrossPremium,
                GstTotalDiscount = entity.GstTotalDiscount,
                GstVitality = entity.GstVitality,
                GstAmount = entity.GstAmount,
                Mfrs17BasicRider = entity.Mfrs17BasicRider,
                Mfrs17CellName = entity.Mfrs17CellName,
                Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                LoaCode = entity.LoaCode,
                CurrencyRate = entity.CurrencyRate,
                NoClaimBonus = entity.NoClaimBonus,
                SurrenderValue = entity.SurrenderValue,
                DatabaseCommision = entity.DatabaseCommision,
                GrossPremiumAlt = entity.GrossPremiumAlt,
                NetPremiumAlt = entity.NetPremiumAlt,
                Layer1FlatExtraPremium = entity.Layer1FlatExtraPremium,
                TransactionPremium = entity.TransactionPremium,
                OriginalPremium = entity.OriginalPremium,
                TransactionDiscount = entity.TransactionDiscount,
                OriginalDiscount = entity.OriginalDiscount,
                BrokerageFee = entity.BrokerageFee,
                MaxUwRating = entity.MaxUwRating,
                RetentionCap = entity.RetentionCap,
                AarCap = entity.AarCap,
                RiRate = entity.RiRate,
                RiRate2 = entity.RiRate2,
                AnnuityFactor = entity.AnnuityFactor,
                SumAssuredOffered = entity.SumAssuredOffered,
                UwRatingOffered = entity.UwRatingOffered,
                FlatExtraAmountOffered = entity.FlatExtraAmountOffered,
                FlatExtraDuration = entity.FlatExtraDuration,
                EffectiveDate = entity.EffectiveDate,
                OfferLetterSentDate = entity.OfferLetterSentDate,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                Mfrs17AnnualCohort = entity.Mfrs17AnnualCohort,
                MaxExpiryAge = entity.MaxExpiryAge,
                MinIssueAge = entity.MinIssueAge,
                MaxIssueAge = entity.MaxIssueAge,
                MinAar = entity.MinAar,
                MaxAar = entity.MaxAar,
                CorridorLimit = entity.CorridorLimit,
                Abl = entity.Abl,
                RatePerBasisUnit = entity.RatePerBasisUnit,
                RiDiscountRate = entity.RiDiscountRate,
                LargeSaDiscount = entity.LargeSaDiscount,
                GroupSizeDiscount = entity.GroupSizeDiscount,
                EwarpNumber = entity.EwarpNumber,
                EwarpActionCode = entity.EwarpActionCode,
                RetentionShare = entity.RetentionShare,
                AarShare = entity.AarShare,
                ProfitComm = entity.ProfitComm,
                TotalDirectRetroAar = entity.TotalDirectRetroAar,
                TotalDirectRetroGrossPremium = entity.TotalDirectRetroGrossPremium,
                TotalDirectRetroDiscount = entity.TotalDirectRetroDiscount,
                TotalDirectRetroNetPremium = entity.TotalDirectRetroNetPremium,
                TreatyType = entity.TreatyType,
                MaxApLoading = entity.MaxApLoading,
                MlreInsuredAttainedAgeAtCurrentMonth = entity.MlreInsuredAttainedAgeAtCurrentMonth,
                MlreInsuredAttainedAgeAtPreviousMonth = entity.MlreInsuredAttainedAgeAtPreviousMonth,
                InsuredAttainedAgeCheck = entity.InsuredAttainedAgeCheck,
                MaxExpiryAgeCheck = entity.MaxExpiryAgeCheck,
                MlrePolicyIssueAge = entity.MlrePolicyIssueAge,
                PolicyIssueAgeCheck = entity.PolicyIssueAgeCheck,
                MinIssueAgeCheck = entity.MinIssueAgeCheck,
                MaxIssueAgeCheck = entity.MaxIssueAgeCheck,
                MaxUwRatingCheck = entity.MaxUwRatingCheck,
                ApLoadingCheck = entity.ApLoadingCheck,
                EffectiveDateCheck = entity.EffectiveDateCheck,
                MinAarCheck = entity.MinAarCheck,
                MaxAarCheck = entity.MaxAarCheck,
                CorridorLimitCheck = entity.CorridorLimitCheck,
                AblCheck = entity.AblCheck,
                RetentionCheck = entity.RetentionCheck,
                AarCheck = entity.AarCheck,
                MlreStandardPremium = entity.MlreStandardPremium,
                MlreSubstandardPremium = entity.MlreSubstandardPremium,
                MlreFlatExtraPremium = entity.MlreFlatExtraPremium,
                MlreGrossPremium = entity.MlreGrossPremium,
                MlreStandardDiscount = entity.MlreStandardDiscount,
                MlreSubstandardDiscount = entity.MlreSubstandardDiscount,
                MlreLargeSaDiscount = entity.MlreLargeSaDiscount,
                MlreGroupSizeDiscount = entity.MlreGroupSizeDiscount,
                MlreVitalityDiscount = entity.MlreVitalityDiscount,
                MlreTotalDiscount = entity.MlreTotalDiscount,
                MlreNetPremium = entity.MlreNetPremium,
                NetPremiumCheck = entity.NetPremiumCheck,
                ServiceFeePercentage = entity.ServiceFeePercentage,
                ServiceFee = entity.ServiceFee,
                MlreBrokerageFee = entity.MlreBrokerageFee,
                MlreDatabaseCommission = entity.MlreDatabaseCommission,
                ValidityDayCheck = entity.ValidityDayCheck,
                SumAssuredOfferedCheck = entity.SumAssuredOfferedCheck,
                UwRatingCheck = entity.UwRatingCheck,
                FlatExtraAmountCheck = entity.FlatExtraAmountCheck,
                FlatExtraDurationCheck = entity.FlatExtraDurationCheck,
                LastUpdatedDate = entity.LastUpdatedDate,
                AarShare2 = entity.AarShare2,
                AarCap2 = entity.AarCap2,
                WakalahFeePercentage = entity.WakalahFeePercentage,
                TreatyNumber = entity.TreatyNumber,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            return bo;
        }

        public static ReferralRiData FormEntity(ReferralRiDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new ReferralRiData
            {
                Id = bo.Id,
                ReferralRiDataFileId = bo.ReferralRiDataFileId,
                TreatyCode = bo.TreatyCode,
                ReinsBasisCode = bo.ReinsBasisCode,
                FundsAccountingTypeCode = bo.FundsAccountingTypeCode,
                PremiumFrequencyCode = bo.PremiumFrequencyCode,
                ReportPeriodMonth = bo.ReportPeriodMonth,
                ReportPeriodYear = bo.ReportPeriodYear,
                RiskPeriodMonth = bo.RiskPeriodMonth,
                RiskPeriodYear = bo.RiskPeriodYear,
                TransactionTypeCode = bo.TransactionTypeCode,
                PolicyNumber = bo.PolicyNumber,
                IssueDatePol = bo.IssueDatePol,
                IssueDateBen = bo.IssueDateBen,
                ReinsEffDatePol = bo.ReinsEffDatePol,
                ReinsEffDateBen = bo.ReinsEffDateBen,
                CedingPlanCode = bo.CedingPlanCode,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                OriSumAssured = bo.OriSumAssured,
                CurrSumAssured = bo.CurrSumAssured,
                AmountCededB4MlreShare = bo.AmountCededB4MlreShare,
                RetentionAmount = bo.RetentionAmount,
                AarOri = bo.AarOri,
                Aar = bo.Aar,
                AarSpecial1 = bo.AarSpecial1,
                AarSpecial2 = bo.AarSpecial2,
                AarSpecial3 = bo.AarSpecial3,
                InsuredName = bo.InsuredName,
                InsuredGenderCode = bo.InsuredGenderCode,
                InsuredTobaccoUse = bo.InsuredTobaccoUse,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredOccupationCode = bo.InsuredOccupationCode,
                InsuredRegisterNo = bo.InsuredRegisterNo,
                InsuredAttainedAge = bo.InsuredAttainedAge,
                InsuredNewIcNumber = bo.InsuredNewIcNumber,
                InsuredOldIcNumber = bo.InsuredOldIcNumber,
                InsuredName2nd = bo.InsuredName2nd,
                InsuredGenderCode2nd = bo.InsuredGenderCode2nd,
                InsuredTobaccoUse2nd = bo.InsuredTobaccoUse2nd,
                InsuredDateOfBirth2nd = bo.InsuredDateOfBirth2nd,
                InsuredAttainedAge2nd = bo.InsuredAttainedAge2nd,
                InsuredNewIcNumber2nd = bo.InsuredNewIcNumber2nd,
                InsuredOldIcNumber2nd = bo.InsuredOldIcNumber2nd,
                ReinsuranceIssueAge = bo.ReinsuranceIssueAge,
                ReinsuranceIssueAge2nd = bo.ReinsuranceIssueAge2nd,
                PolicyTerm = bo.PolicyTerm,
                PolicyExpiryDate = bo.PolicyExpiryDate,
                DurationYear = bo.DurationYear,
                DurationDay = bo.DurationDay,
                DurationMonth = bo.DurationMonth,
                PremiumCalType = bo.PremiumCalType,
                CedantRiRate = bo.CedantRiRate,
                RateTable = bo.RateTable,
                AgeRatedUp = bo.AgeRatedUp,
                DiscountRate = bo.DiscountRate,
                LoadingType = bo.LoadingType,
                UnderwriterRating = bo.UnderwriterRating,
                UnderwriterRatingUnit = bo.UnderwriterRatingUnit,
                UnderwriterRatingTerm = bo.UnderwriterRatingTerm,
                UnderwriterRating2 = bo.UnderwriterRating2,
                UnderwriterRatingUnit2 = bo.UnderwriterRatingUnit2,
                UnderwriterRatingTerm2 = bo.UnderwriterRatingTerm2,
                UnderwriterRating3 = bo.UnderwriterRating3,
                UnderwriterRatingUnit3 = bo.UnderwriterRatingUnit3,
                UnderwriterRatingTerm3 = bo.UnderwriterRatingTerm3,
                FlatExtraAmount = bo.FlatExtraAmount,
                FlatExtraUnit = bo.FlatExtraUnit,
                FlatExtraTerm = bo.FlatExtraTerm,
                FlatExtraAmount2 = bo.FlatExtraAmount2,
                FlatExtraTerm2 = bo.FlatExtraTerm2,
                StandardPremium = bo.StandardPremium,
                SubstandardPremium = bo.SubstandardPremium,
                FlatExtraPremium = bo.FlatExtraPremium,
                GrossPremium = bo.GrossPremium,
                StandardDiscount = bo.StandardDiscount,
                SubstandardDiscount = bo.SubstandardDiscount,
                VitalityDiscount = bo.VitalityDiscount,
                TotalDiscount = bo.TotalDiscount,
                NetPremium = bo.NetPremium,
                AnnualRiPrem = bo.AnnualRiPrem,
                RiCovPeriod = bo.RiCovPeriod,
                AdjBeginDate = bo.AdjBeginDate,
                AdjEndDate = bo.AdjEndDate,
                PolicyNumberOld = bo.PolicyNumberOld,
                PolicyStatusCode = bo.PolicyStatusCode,
                PolicyGrossPremium = bo.PolicyGrossPremium,
                PolicyStandardPremium = bo.PolicyStandardPremium,
                PolicySubstandardPremium = bo.PolicySubstandardPremium,
                PolicyTermRemain = bo.PolicyTermRemain,
                PolicyAmountDeath = bo.PolicyAmountDeath,
                PolicyReserve = bo.PolicyReserve,
                PolicyPaymentMethod = bo.PolicyPaymentMethod,
                PolicyLifeNumber = bo.PolicyLifeNumber,
                FundCode = bo.FundCode,
                LineOfBusiness = bo.LineOfBusiness,
                ApLoading = bo.ApLoading,
                LoanInterestRate = bo.LoanInterestRate,
                DefermentPeriod = bo.DefermentPeriod,
                RiderNumber = bo.RiderNumber,
                CampaignCode = bo.CampaignCode,
                Nationality = bo.Nationality,
                TerritoryOfIssueCode = bo.TerritoryOfIssueCode,
                CurrencyCode = bo.CurrencyCode,
                StaffPlanIndicator = bo.StaffPlanIndicator,
                CedingTreatyCode = bo.CedingTreatyCode,
                CedingPlanCodeOld = bo.CedingPlanCodeOld,
                CedingBasicPlanCode = bo.CedingBasicPlanCode,
                CedantSar = bo.CedantSar,
                CedantReinsurerCode = bo.CedantReinsurerCode,
                AmountCededB4MlreShare2 = bo.AmountCededB4MlreShare2,
                CessionCode = bo.CessionCode,
                CedantRemark = bo.CedantRemark,
                GroupPolicyNumber = bo.GroupPolicyNumber,
                GroupPolicyName = bo.GroupPolicyName,
                NoOfEmployee = bo.NoOfEmployee,
                PolicyTotalLive = bo.PolicyTotalLive,
                GroupSubsidiaryName = bo.GroupSubsidiaryName,
                GroupSubsidiaryNo = bo.GroupSubsidiaryNo,
                GroupEmployeeBasicSalary = bo.GroupEmployeeBasicSalary,
                GroupEmployeeJobType = bo.GroupEmployeeJobType,
                GroupEmployeeJobCode = bo.GroupEmployeeJobCode,
                GroupEmployeeBasicSalaryRevise = bo.GroupEmployeeBasicSalaryRevise,
                GroupEmployeeBasicSalaryMultiplier = bo.GroupEmployeeBasicSalaryMultiplier,
                CedingPlanCode2 = bo.CedingPlanCode2,
                DependantIndicator = bo.DependantIndicator,
                GhsRoomBoard = bo.GhsRoomBoard,
                PolicyAmountSubstandard = bo.PolicyAmountSubstandard,
                Layer1RiShare = bo.Layer1RiShare,
                Layer1InsuredAttainedAge = bo.Layer1InsuredAttainedAge,
                Layer1InsuredAttainedAge2nd = bo.Layer1InsuredAttainedAge2nd,
                Layer1StandardPremium = bo.Layer1StandardPremium,
                Layer1SubstandardPremium = bo.Layer1SubstandardPremium,
                Layer1GrossPremium = bo.Layer1GrossPremium,
                Layer1StandardDiscount = bo.Layer1StandardDiscount,
                Layer1SubstandardDiscount = bo.Layer1SubstandardDiscount,
                Layer1TotalDiscount = bo.Layer1TotalDiscount,
                Layer1NetPremium = bo.Layer1NetPremium,
                Layer1GrossPremiumAlt = bo.Layer1GrossPremiumAlt,
                Layer1TotalDiscountAlt = bo.Layer1TotalDiscountAlt,
                Layer1NetPremiumAlt = bo.Layer1NetPremiumAlt,
                SpecialIndicator1 = bo.SpecialIndicator1,
                SpecialIndicator2 = bo.SpecialIndicator2,
                SpecialIndicator3 = bo.SpecialIndicator3,
                IndicatorJointLife = bo.IndicatorJointLife,
                TaxAmount = bo.TaxAmount,
                GstIndicator = bo.GstIndicator,
                GstGrossPremium = bo.GstGrossPremium,
                GstTotalDiscount = bo.GstTotalDiscount,
                GstVitality = bo.GstVitality,
                GstAmount = bo.GstAmount,
                Mfrs17BasicRider = bo.Mfrs17BasicRider,
                Mfrs17CellName = bo.Mfrs17CellName,
                Mfrs17TreatyCode = bo.Mfrs17TreatyCode,
                LoaCode = bo.LoaCode,
                CurrencyRate = bo.CurrencyRate,
                NoClaimBonus = bo.NoClaimBonus,
                SurrenderValue = bo.SurrenderValue,
                DatabaseCommision = bo.DatabaseCommision,
                GrossPremiumAlt = bo.GrossPremiumAlt,
                NetPremiumAlt = bo.NetPremiumAlt,
                Layer1FlatExtraPremium = bo.Layer1FlatExtraPremium,
                TransactionPremium = bo.TransactionPremium,
                OriginalPremium = bo.OriginalPremium,
                TransactionDiscount = bo.TransactionDiscount,
                OriginalDiscount = bo.OriginalDiscount,
                BrokerageFee = bo.BrokerageFee,
                MaxUwRating = bo.MaxUwRating,
                RetentionCap = bo.RetentionCap,
                AarCap = bo.AarCap,
                RiRate = bo.RiRate,
                RiRate2 = bo.RiRate2,
                AnnuityFactor = bo.AnnuityFactor,
                SumAssuredOffered = bo.SumAssuredOffered,
                UwRatingOffered = bo.UwRatingOffered,
                FlatExtraAmountOffered = bo.FlatExtraAmountOffered,
                FlatExtraDuration = bo.FlatExtraDuration,
                EffectiveDate = bo.EffectiveDate,
                OfferLetterSentDate = bo.OfferLetterSentDate,
                RiskPeriodStartDate = bo.RiskPeriodStartDate,
                RiskPeriodEndDate = bo.RiskPeriodEndDate,
                Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort,
                MaxExpiryAge = bo.MaxExpiryAge,
                MinIssueAge = bo.MinIssueAge,
                MaxIssueAge = bo.MaxIssueAge,
                MinAar = bo.MinAar,
                MaxAar = bo.MaxAar,
                CorridorLimit = bo.CorridorLimit,
                Abl = bo.Abl,
                RatePerBasisUnit = bo.RatePerBasisUnit,
                RiDiscountRate = bo.RiDiscountRate,
                LargeSaDiscount = bo.LargeSaDiscount,
                GroupSizeDiscount = bo.GroupSizeDiscount,
                EwarpNumber = bo.EwarpNumber,
                EwarpActionCode = bo.EwarpActionCode,
                RetentionShare = bo.RetentionShare,
                AarShare = bo.AarShare,
                ProfitComm = bo.ProfitComm,
                TotalDirectRetroAar = bo.TotalDirectRetroAar,
                TotalDirectRetroGrossPremium = bo.TotalDirectRetroGrossPremium,
                TotalDirectRetroDiscount = bo.TotalDirectRetroDiscount,
                TotalDirectRetroNetPremium = bo.TotalDirectRetroNetPremium,
                TreatyType = bo.TreatyType,
                MaxApLoading = bo.MaxApLoading,
                MlreInsuredAttainedAgeAtCurrentMonth = bo.MlreInsuredAttainedAgeAtCurrentMonth,
                MlreInsuredAttainedAgeAtPreviousMonth = bo.MlreInsuredAttainedAgeAtPreviousMonth,
                InsuredAttainedAgeCheck = bo.InsuredAttainedAgeCheck,
                MaxExpiryAgeCheck = bo.MaxExpiryAgeCheck,
                MlrePolicyIssueAge = bo.MlrePolicyIssueAge,
                PolicyIssueAgeCheck = bo.PolicyIssueAgeCheck,
                MinIssueAgeCheck = bo.MinIssueAgeCheck,
                MaxIssueAgeCheck = bo.MaxIssueAgeCheck,
                MaxUwRatingCheck = bo.MaxUwRatingCheck,
                ApLoadingCheck = bo.ApLoadingCheck,
                EffectiveDateCheck = bo.EffectiveDateCheck,
                MinAarCheck = bo.MinAarCheck,
                MaxAarCheck = bo.MaxAarCheck,
                CorridorLimitCheck = bo.CorridorLimitCheck,
                AblCheck = bo.AblCheck,
                RetentionCheck = bo.RetentionCheck,
                AarCheck = bo.AarCheck,
                MlreStandardPremium = bo.MlreStandardPremium,
                MlreSubstandardPremium = bo.MlreSubstandardPremium,
                MlreFlatExtraPremium = bo.MlreFlatExtraPremium,
                MlreGrossPremium = bo.MlreGrossPremium,
                MlreStandardDiscount = bo.MlreStandardDiscount,
                MlreSubstandardDiscount = bo.MlreSubstandardDiscount,
                MlreLargeSaDiscount = bo.MlreLargeSaDiscount,
                MlreGroupSizeDiscount = bo.MlreGroupSizeDiscount,
                MlreVitalityDiscount = bo.MlreVitalityDiscount,
                MlreTotalDiscount = bo.MlreTotalDiscount,
                MlreNetPremium = bo.MlreNetPremium,
                NetPremiumCheck = bo.NetPremiumCheck,
                ServiceFeePercentage = bo.ServiceFeePercentage,
                ServiceFee = bo.ServiceFee,
                MlreBrokerageFee = bo.MlreBrokerageFee,
                MlreDatabaseCommission = bo.MlreDatabaseCommission,
                ValidityDayCheck = bo.ValidityDayCheck,
                SumAssuredOfferedCheck = bo.SumAssuredOfferedCheck,
                UwRatingCheck = bo.UwRatingCheck,
                FlatExtraAmountCheck = bo.FlatExtraAmountCheck,
                FlatExtraDurationCheck = bo.FlatExtraDurationCheck,
                LastUpdatedDate = bo.LastUpdatedDate,
                AarShare2 = bo.AarShare2,
                AarCap2 = bo.AarCap2,
                WakalahFeePercentage = bo.WakalahFeePercentage,
                TreatyNumber = bo.TreatyNumber,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<ReferralRiDataBo> FormBos(IList<ReferralRiData> entities = null, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<ReferralRiDataBo> bos = new List<ReferralRiDataBo>() { };
            foreach (ReferralRiData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ReferralRiDataBo Find(int? id = null)
        {
            if (!id.HasValue)
                return null;
            return FormBo(ReferralRiData.Find(id.Value));
        }

        public static RiDataWarehouseBo FindToWarehouse(int? id = null)
        {
            if (!id.HasValue)
                return null;
            return RiDataWarehouseService.FormBo(ReferralRiData.Find(id.Value), true);
        }

        public static Result Create(ref ReferralRiDataBo bo)
        {
            ReferralRiData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ReferralRiDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ReferralRiDataBo bo)
        {
            Result result = Result();

            ReferralRiData entity = ReferralRiData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (!result.Valid)
                return result;

            entity = FormEntity(bo);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

            result.DataTrail = entity.Update();
            return result;
        }

        public static Result Update(ref ReferralRiDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ReferralRiDataBo bo)
        {
            ReferralRiData.Delete(bo.Id);
        }

        public static Result Delete(ReferralRiDataBo bo, ref TrailObject trail)
        {
            Result result = Result();


            DataTrail dataTrail = ReferralRiData.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
