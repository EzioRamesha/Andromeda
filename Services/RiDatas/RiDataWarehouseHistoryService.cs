using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.RiDatas
{
    public class RiDataWarehouseHistoryService
    {
        public static Expression<Func<RiDataWarehouseHistory, RiDataWarehouseHistoryBo>> Expression()
        {
            return entity => new RiDataWarehouseHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                EndingPolicyStatus = entity.EndingPolicyStatus,
                //EndingPolicyStatusCode = entity.EndingPolicyStatusPickListDetail.Code,
                RecordType = entity.RecordType,
                Quarter = entity.Quarter,
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
                TotalDirectRetroNoClaimBonus = entity.TotalDirectRetroNoClaimBonus,
                TotalDirectRetroDatabaseCommission = entity.TotalDirectRetroDatabaseCommission,
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
                ConflictType = entity.ConflictType,

                // Direct Retro
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RetroPremiumSpread1 = entity.RetroPremiumSpread1,
                RetroPremiumSpread2 = entity.RetroPremiumSpread2,
                RetroPremiumSpread3 = entity.RetroPremiumSpread3,
                RetroAar1 = entity.RetroAar1,
                RetroAar2 = entity.RetroAar2,
                RetroAar3 = entity.RetroAar3,
                RetroReinsurancePremium1 = entity.RetroReinsurancePremium1,
                RetroReinsurancePremium2 = entity.RetroReinsurancePremium2,
                RetroReinsurancePremium3 = entity.RetroReinsurancePremium3,
                RetroDiscount1 = entity.RetroDiscount1,
                RetroDiscount2 = entity.RetroDiscount2,
                RetroDiscount3 = entity.RetroDiscount3,
                RetroNetPremium1 = entity.RetroNetPremium1,
                RetroNetPremium2 = entity.RetroNetPremium2,
                RetroNetPremium3 = entity.RetroNetPremium3,
                RetroNoClaimBonus1 = entity.RetroNoClaimBonus1,
                RetroNoClaimBonus2 = entity.RetroNoClaimBonus2,
                RetroNoClaimBonus3 = entity.RetroNoClaimBonus3,
                RetroDatabaseCommission1 = entity.RetroDatabaseCommission1,
                RetroDatabaseCommission2 = entity.RetroDatabaseCommission2,
                RetroDatabaseCommission3 = entity.RetroDatabaseCommission3,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };


        }

        public static RiDataWarehouseHistoryBo FormBo(RiDataWarehouseHistory entity = null, bool formatOutput = false)
        {
            if (entity == null)
                return null;
            var bo = new RiDataWarehouseHistoryBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                EndingPolicyStatus = entity.EndingPolicyStatus,
                RecordType = entity.RecordType,
                Quarter = entity.Quarter,
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
                TotalDirectRetroNoClaimBonus = entity.TotalDirectRetroNoClaimBonus,
                TotalDirectRetroDatabaseCommission = entity.TotalDirectRetroDatabaseCommission,
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
                ConflictType = entity.ConflictType,

                // Direct Retro
                RetroParty1 = entity.RetroParty1,
                RetroParty2 = entity.RetroParty2,
                RetroParty3 = entity.RetroParty3,
                RetroShare1 = entity.RetroShare1,
                RetroShare2 = entity.RetroShare2,
                RetroShare3 = entity.RetroShare3,
                RetroPremiumSpread1 = entity.RetroPremiumSpread1,
                RetroPremiumSpread2 = entity.RetroPremiumSpread2,
                RetroPremiumSpread3 = entity.RetroPremiumSpread3,
                RetroAar1 = entity.RetroAar1,
                RetroAar2 = entity.RetroAar2,
                RetroAar3 = entity.RetroAar3,
                RetroReinsurancePremium1 = entity.RetroReinsurancePremium1,
                RetroReinsurancePremium2 = entity.RetroReinsurancePremium2,
                RetroReinsurancePremium3 = entity.RetroReinsurancePremium3,
                RetroDiscount1 = entity.RetroDiscount1,
                RetroDiscount2 = entity.RetroDiscount2,
                RetroDiscount3 = entity.RetroDiscount3,
                RetroNetPremium1 = entity.RetroNetPremium1,
                RetroNetPremium2 = entity.RetroNetPremium2,
                RetroNetPremium3 = entity.RetroNetPremium3,
                RetroNoClaimBonus1 = entity.RetroNoClaimBonus1,
                RetroNoClaimBonus2 = entity.RetroNoClaimBonus2,
                RetroNoClaimBonus3 = entity.RetroNoClaimBonus3,
                RetroDatabaseCommission1 = entity.RetroDatabaseCommission1,
                RetroDatabaseCommission2 = entity.RetroDatabaseCommission2,
                RetroDatabaseCommission3 = entity.RetroDatabaseCommission3,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (formatOutput)
            {
                bo.IssueDatePolStr = entity.IssueDatePol?.ToString(Util.GetDateFormat());
                bo.IssueDateBenStr = entity.IssueDateBen?.ToString(Util.GetDateFormat());
                bo.ReinsEffDatePolStr = entity.ReinsEffDatePol?.ToString(Util.GetDateFormat());
                bo.ReinsEffDateBenStr = entity.ReinsEffDateBen?.ToString(Util.GetDateFormat());
                bo.InsuredDateOfBirthStr = entity.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                bo.InsuredDateOfBirth2ndStr = entity.InsuredDateOfBirth2nd?.ToString(Util.GetDateFormat());
                bo.PolicyExpiryDateStr = entity.PolicyExpiryDate?.ToString(Util.GetDateFormat());
                bo.AdjBeginDateStr = entity.AdjBeginDate?.ToString(Util.GetDateFormat());
                bo.AdjEndDateStr = entity.AdjEndDate?.ToString(Util.GetDateFormat());
                bo.EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat());
                bo.OfferLetterSentDateStr = entity.OfferLetterSentDate?.ToString(Util.GetDateFormat());
                bo.RiskPeriodStartDateStr = entity.RiskPeriodStartDate?.ToString(Util.GetDateFormat());
                bo.RiskPeriodEndDateStr = entity.RiskPeriodEndDate?.ToString(Util.GetDateFormat());

                bo.DurationDayStr = Util.DoubleToString(entity.DurationDay);
                bo.DurationMonthStr = Util.DoubleToString(entity.DurationMonth);
                bo.RiCovPeriodStr = Util.DoubleToString(entity.RiCovPeriod);
                bo.OriSumAssuredStr = Util.DoubleToString(entity.OriSumAssured);
                bo.CurrSumAssuredStr = Util.DoubleToString(entity.CurrSumAssured);
                bo.AmountCededB4MlreShareStr = Util.DoubleToString(entity.AmountCededB4MlreShare);
                bo.RetentionAmountStr = Util.DoubleToString(entity.RetentionAmount);
                bo.AarOriStr = Util.DoubleToString(entity.AarOri);
                bo.AarStr = Util.DoubleToString(entity.Aar);
                bo.AarSpecial1Str = Util.DoubleToString(entity.AarSpecial1);
                bo.AarSpecial2Str = Util.DoubleToString(entity.AarSpecial2);
                bo.AarSpecial3Str = Util.DoubleToString(entity.AarSpecial3);
                bo.DurationYearStr = Util.DoubleToString(entity.DurationYear);
                bo.CedantRiRateStr = Util.DoubleToString(entity.CedantRiRate);
                bo.DiscountRateStr = Util.DoubleToString(entity.DiscountRate);
                bo.UnderwriterRatingStr = Util.DoubleToString(entity.UnderwriterRating);
                bo.UnderwriterRatingUnitStr = Util.DoubleToString(entity.UnderwriterRatingUnit);
                bo.UnderwriterRating2Str = Util.DoubleToString(entity.UnderwriterRating2);
                bo.UnderwriterRatingUnit2Str = Util.DoubleToString(entity.UnderwriterRatingUnit2);
                bo.UnderwriterRating3Str = Util.DoubleToString(entity.UnderwriterRating3);
                bo.UnderwriterRatingUnit3Str = Util.DoubleToString(entity.UnderwriterRatingUnit3);
                bo.FlatExtraAmountStr = Util.DoubleToString(entity.FlatExtraAmount);
                bo.FlatExtraUnitStr = Util.DoubleToString(entity.FlatExtraUnit);
                bo.FlatExtraAmount2Str = Util.DoubleToString(entity.FlatExtraAmount2);
                bo.StandardPremiumStr = Util.DoubleToString(entity.StandardPremium);
                bo.SubstandardPremiumStr = Util.DoubleToString(entity.SubstandardPremium);
                bo.FlatExtraPremiumStr = Util.DoubleToString(entity.FlatExtraPremium);
                bo.GrossPremiumStr = Util.DoubleToString(entity.GrossPremium);
                bo.StandardDiscountStr = Util.DoubleToString(entity.StandardDiscount);
                bo.SubstandardDiscountStr = Util.DoubleToString(entity.SubstandardDiscount);
                bo.VitalityDiscountStr = Util.DoubleToString(entity.VitalityDiscount);
                bo.TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount);
                bo.NetPremiumStr = Util.DoubleToString(entity.NetPremium);
                bo.AnnualRiPremStr = Util.DoubleToString(entity.AnnualRiPrem);
                bo.PolicyGrossPremiumStr = Util.DoubleToString(entity.PolicyGrossPremium);
                bo.PolicyStandardPremiumStr = Util.DoubleToString(entity.PolicyStandardPremium);
                bo.PolicySubstandardPremiumStr = Util.DoubleToString(entity.PolicySubstandardPremium);
                bo.PolicyAmountDeathStr = Util.DoubleToString(entity.PolicyAmountDeath);
                bo.PolicyReserveStr = Util.DoubleToString(entity.PolicyReserve);
                bo.ApLoadingStr = Util.DoubleToString(entity.ApLoading);
                bo.LoanInterestRateStr = Util.DoubleToString(entity.LoanInterestRate);
                bo.CedantSarStr = Util.DoubleToString(entity.CedantSar);
                bo.AmountCededB4MlreShare2Str = Util.DoubleToString(entity.AmountCededB4MlreShare2);
                bo.GroupEmployeeBasicSalaryStr = Util.DoubleToString(entity.GroupEmployeeBasicSalary);
                bo.GroupEmployeeBasicSalaryReviseStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryRevise);
                bo.GroupEmployeeBasicSalaryMultiplierStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryMultiplier);
                bo.PolicyAmountSubstandardStr = Util.DoubleToString(entity.PolicyAmountSubstandard);
                bo.Layer1RiShareStr = Util.DoubleToString(entity.Layer1RiShare);
                bo.Layer1StandardPremiumStr = Util.DoubleToString(entity.Layer1StandardPremium);
                bo.Layer1SubstandardPremiumStr = Util.DoubleToString(entity.Layer1SubstandardPremium);
                bo.Layer1GrossPremiumStr = Util.DoubleToString(entity.Layer1GrossPremium);
                bo.Layer1StandardDiscountStr = Util.DoubleToString(entity.Layer1StandardDiscount);
                bo.Layer1SubstandardDiscountStr = Util.DoubleToString(entity.Layer1SubstandardDiscount);
                bo.Layer1TotalDiscountStr = Util.DoubleToString(entity.Layer1TotalDiscount);
                bo.Layer1NetPremiumStr = Util.DoubleToString(entity.Layer1NetPremium);
                bo.Layer1GrossPremiumAltStr = Util.DoubleToString(entity.Layer1GrossPremiumAlt);
                bo.Layer1TotalDiscountAltStr = Util.DoubleToString(entity.Layer1TotalDiscountAlt);
                bo.Layer1NetPremiumAltStr = Util.DoubleToString(entity.Layer1NetPremiumAlt);
                bo.TaxAmountStr = Util.DoubleToString(entity.TaxAmount);
                bo.GstGrossPremiumStr = Util.DoubleToString(entity.GstGrossPremium);
                bo.GstTotalDiscountStr = Util.DoubleToString(entity.GstTotalDiscount);
                bo.GstVitalityStr = Util.DoubleToString(entity.GstVitality);
                bo.GstAmountStr = Util.DoubleToString(entity.GstAmount);
                bo.CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate);
                bo.NoClaimBonusStr = Util.DoubleToString(entity.NoClaimBonus);
                bo.SurrenderValueStr = Util.DoubleToString(entity.SurrenderValue);
                bo.DatabaseCommisionStr = Util.DoubleToString(entity.DatabaseCommision);
                bo.GrossPremiumAltStr = Util.DoubleToString(entity.GrossPremiumAlt);
                bo.NetPremiumAltStr = Util.DoubleToString(entity.NetPremiumAlt);
                bo.Layer1FlatExtraPremiumStr = Util.DoubleToString(entity.Layer1FlatExtraPremium);
                bo.TransactionPremiumStr = Util.DoubleToString(entity.TransactionPremium);
                bo.OriginalPremiumStr = Util.DoubleToString(entity.OriginalPremium);
                bo.TransactionDiscountStr = Util.DoubleToString(entity.TransactionDiscount);
                bo.OriginalDiscountStr = Util.DoubleToString(entity.OriginalDiscount);
                bo.BrokerageFeeStr = Util.DoubleToString(entity.BrokerageFee);
                bo.MaxUwRatingStr = Util.DoubleToString(entity.MaxUwRating);
                bo.RetentionCapStr = Util.DoubleToString(entity.RetentionCap);
                bo.AarCapStr = Util.DoubleToString(entity.AarCap);
                bo.RiRateStr = Util.DoubleToString(entity.RiRate);
                bo.RiRate2Str = Util.DoubleToString(entity.RiRate2);
                bo.AnnuityFactorStr = Util.DoubleToString(entity.AnnuityFactor);
                bo.SumAssuredOfferedStr = Util.DoubleToString(entity.SumAssuredOffered);
                bo.UwRatingOfferedStr = Util.DoubleToString(entity.UwRatingOffered);
                bo.FlatExtraAmountOfferedStr = Util.DoubleToString(entity.FlatExtraAmountOffered);
                bo.FlatExtraDurationStr = Util.DoubleToString(entity.FlatExtraDuration);
                bo.RetentionShareStr = Util.DoubleToString(entity.RetentionShare);
                bo.AarShareStr = Util.DoubleToString(entity.AarShare);
                bo.TotalDirectRetroAarStr = Util.DoubleToString(entity.TotalDirectRetroAar);
                bo.TotalDirectRetroGrossPremiumStr = Util.DoubleToString(entity.TotalDirectRetroGrossPremium);
                bo.TotalDirectRetroDiscountStr = Util.DoubleToString(entity.TotalDirectRetroDiscount);
                bo.TotalDirectRetroNetPremiumStr = Util.DoubleToString(entity.TotalDirectRetroNetPremium);
                bo.TotalDirectRetroNoClaimBonusStr = Util.DoubleToString(entity.TotalDirectRetroNoClaimBonus);
                bo.TotalDirectRetroDatabaseCommissionStr = Util.DoubleToString(entity.TotalDirectRetroDatabaseCommission);
                bo.MinAarStr = Util.DoubleToString(entity.MinAar);
                bo.MaxAarStr = Util.DoubleToString(entity.MaxAar);
                bo.CorridorLimitStr = Util.DoubleToString(entity.CorridorLimit);
                bo.AblStr = Util.DoubleToString(entity.Abl);
                bo.RiDiscountRateStr = Util.DoubleToString(entity.RiDiscountRate);
                bo.LargeSaDiscountStr = Util.DoubleToString(entity.LargeSaDiscount);
                bo.GroupSizeDiscountStr = Util.DoubleToString(entity.GroupSizeDiscount);
                bo.AarShare2Str = Util.DoubleToString(entity.AarShare2);
                bo.AarCap2Str = Util.DoubleToString(entity.AarCap2);
                bo.WakalahFeePercentageStr = Util.DoubleToString(entity.WakalahFeePercentage);

                // Direct Retro
                bo.RetroShare1Str = Util.DoubleToString(entity.RetroShare1);
                bo.RetroShare2Str = Util.DoubleToString(entity.RetroShare2);
                bo.RetroShare3Str = Util.DoubleToString(entity.RetroShare3);
                bo.RetroPremiumSpread1Str = Util.DoubleToString(entity.RetroPremiumSpread1);
                bo.RetroPremiumSpread2Str = Util.DoubleToString(entity.RetroPremiumSpread2);
                bo.RetroPremiumSpread3Str = Util.DoubleToString(entity.RetroPremiumSpread3);
                bo.RetroAar1Str = Util.DoubleToString(entity.RetroAar1);
                bo.RetroAar2Str = Util.DoubleToString(entity.RetroAar2);
                bo.RetroAar3Str = Util.DoubleToString(entity.RetroAar3);
                bo.RetroReinsurancePremium1Str = Util.DoubleToString(entity.RetroReinsurancePremium1);
                bo.RetroReinsurancePremium2Str = Util.DoubleToString(entity.RetroReinsurancePremium2);
                bo.RetroReinsurancePremium3Str = Util.DoubleToString(entity.RetroReinsurancePremium3);
                bo.RetroDiscount1Str = Util.DoubleToString(entity.RetroDiscount1);
                bo.RetroDiscount2Str = Util.DoubleToString(entity.RetroDiscount2);
                bo.RetroDiscount3Str = Util.DoubleToString(entity.RetroDiscount3);
                bo.RetroNetPremium1Str = Util.DoubleToString(entity.RetroNetPremium1);
                bo.RetroNetPremium2Str = Util.DoubleToString(entity.RetroNetPremium2);
                bo.RetroNetPremium3Str = Util.DoubleToString(entity.RetroNetPremium3);
                bo.RetroNoClaimBonus1Str = Util.DoubleToString(entity.RetroNoClaimBonus1);
                bo.RetroNoClaimBonus2Str = Util.DoubleToString(entity.RetroNoClaimBonus2);
                bo.RetroNoClaimBonus3Str = Util.DoubleToString(entity.RetroNoClaimBonus3);
                bo.RetroDatabaseCommission1Str = Util.DoubleToString(entity.RetroDatabaseCommission1);
                bo.RetroDatabaseCommission2Str = Util.DoubleToString(entity.RetroDatabaseCommission2);
                bo.RetroDatabaseCommission3Str = Util.DoubleToString(entity.RetroDatabaseCommission3);
            }

            return bo;
        }

        public static IList<RiDataWarehouseHistoryBo> FormBos(IList<RiDataWarehouseHistory> entities = null, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<RiDataWarehouseHistoryBo> bos = new List<RiDataWarehouseHistoryBo>() { };
            foreach (RiDataWarehouseHistory entity in entities)
            {
                bos.Add(FormBo(entity, formatOutput));
            }
            return bos;
        }

        public static RiDataWarehouseHistoryBo Find(int? id, int? cutOffId, bool formatOutput = false)
        {
            if (!id.HasValue || !cutOffId.HasValue)
                return null;

            return FormBo(RiDataWarehouseHistory.Find(id.Value, cutOffId.Value), formatOutput);
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(int cutOffId, string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null, string cedingPlanCode = null)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                       .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                       .Where(q => list.Contains(q.Code.Trim()));

                    var query = db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value));

                    if (monthEnd != null)
                    {
                        query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                    }
                    else
                    {
                        query = query.Where(q => q.RiskPeriodMonth == monthStart);
                    }

                    if (!string.IsNullOrEmpty(cedingPlanCode))
                    {
                        query = query.Where(q => q.CedingPlanCode == cedingPlanCode);
                    }

                    return query.GroupBy(q => q.Mfrs17TreatyCode).Select(q => q.FirstOrDefault().Mfrs17TreatyCode).ToList();
                });
            }
        }

        public static List<(int, int)> GetIdsForMfrs17Reporting(int cutOffId, string treatyCode, string premiumFrequencyCode, string mfrs17TreatyCode, int year, int monthStart, int? monthEnd = null, string cedingPlanCode = null)

        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                    .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                    .Where(q => list.Contains(q.Code.Trim()));

                    var query = db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                        .Where(q => q.Mfrs17TreatyCode == mfrs17TreatyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value));

                    if (monthEnd != null)
                    {
                        query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                    }
                    else
                    {
                        query = query.Where(q => q.RiskPeriodMonth == monthStart);
                    }

                    if (!string.IsNullOrEmpty(cedingPlanCode))
                    {
                        query = query.Where(q => q.CedingPlanCode == cedingPlanCode);
                    }

                    return query.Select(q => new { q.RiDataWarehouseId, q.CutOffId }).AsEnumerable().Select(c => (c.RiDataWarehouseId, c.CutOffId)).ToList();
                });
            }
        }

        public static IList<RiDataWarehouseHistoryBo> GetByIds(List<int> ids, int cutOffId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    return FormBos(db.RiDataWarehouseHistories.Where(q => ids.Contains(q.Id))
                        .Where(q => q.CutOffId == cutOffId)
                        .ToList());
                });
            }
        }

        public static int CountByCutOffIdsTreatyCodeYearMonth(int cutOffId, string treatyCode, int year, int month)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                        .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                        .Where(q => list.Contains(q.Code.Trim()));

                    return db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.RiskPeriodMonth <= month)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value))
                        .Count();
                });
            }
        }

        public static int? GetMaxYearByCutOffIdsTreatyCodeYear(int cutOffId, string treatyCode, int year)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                       .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                       .Where(q => list.Contains(q.Code.Trim()));

                    return db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.RiskPeriodYear < year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value))
                        .OrderByDescending(q => q.RiskPeriodYear)
                        .Select(q => q.RiskPeriodYear)
                        .FirstOrDefault();
                });
            }
        }

        public static int? GetMaxMonthByMonthYear(int cutOffId, string treatyCode, int? month, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                       .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                       .Where(q => list.Contains(q.Code.Trim()));

                    var query = db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value));

                    if (month != null)
                        query = query.Where(q => q.RiskPeriodMonth <= month);

                    return query.OrderByDescending(q => q.RiskPeriodMonth)
                        .Select(q => q.RiskPeriodMonth)
                        .FirstOrDefault();
                });
            }
        }

        public static int? GetMaxMonthByYear(int cutOffId, string treatyCode, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                       .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                       .Where(q => list.Contains(q.Code.Trim()));

                    return db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value))
                        .OrderByDescending(q => q.RiskPeriodMonth)
                        .Select(q => q.RiskPeriodMonth)
                        .FirstOrDefault();
                });
            }
        }

        public static Dictionary<string, int> CountForMfrs17Reporting(int cutOffId, string treatyCode, string premiumFrequencyCode, string cedingPlanCodes, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var list = PickListDetailBo.GetMfrs17EndingPolicyStatus();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.PickListDetails
                        .Where(q => q.PickList.StandardOutputId == StandardOutputBo.TypePolicyStatusCode)
                        .Where(q => list.Contains(q.Code.Trim()));

                    var query = db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => q.TreatyCode == treatyCode)
                        .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                        .Where(q => q.RiskPeriodYear == year)
                        .Where(q => q.EndingPolicyStatus.HasValue)
                        .Where(q => subQuery.Select(s => s.Id).Contains(q.EndingPolicyStatus.Value));

                    if (monthEnd != null)
                    {
                        query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                    }
                    else
                    {
                        query = query.Where(q => q.RiskPeriodMonth == monthStart);
                    }

                    if (!string.IsNullOrEmpty(cedingPlanCodes))
                    {
                        var listCedingPlanCode = cedingPlanCodes.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        query = query.Where(q => listCedingPlanCode.Contains(q.CedingPlanCode));
                    }

                    return query.GroupBy(q => q.CedingPlanCode).OrderBy(q => q.Key).ToDictionary(q => q.Key, q => q.Count());
                });
            }
        }

        public static int GetCountByCutOffId(int cutOffId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataWarehouseHistories.Where(q => q.CutOffId == cutOffId).Count();
            }
        }

        public static int GetMaxIdByCutOffId(int cutOffId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataWarehouseHistories.Where(q => q.CutOffId == cutOffId).Max(q => q.RiDataWarehouseId);
            }
        }

        public static List<string> GetDistinctCedingPlanCodes(int cutOffId, int cedantId, int treatyCodeId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDataWarehouseHistoryService");

                return connectionStrategy.Execute(() =>
                {
                    var subQuery = db.TreatyCodes
                       .Where(q => q.Id == treatyCodeId)
                       .Where(q => q.Treaty.CedantId == cedantId);

                    var query = db.RiDataWarehouseHistories
                        .Where(q => q.CutOffId == cutOffId)
                        .Where(q => subQuery.Select(s => s.Code).Contains(q.TreatyCode));

                    return query.GroupBy(q => q.CedingPlanCode).Select(q => q.FirstOrDefault().CedingPlanCode).ToList();
                });
            }
        }
    }
}
