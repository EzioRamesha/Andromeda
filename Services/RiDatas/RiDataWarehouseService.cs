using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.RiDatas
{
    public class RiDataWarehouseService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataWarehouse)),
                Controller = ModuleBo.ModuleController.RiDataWarehouse.ToString(),
            };
        }

        public static Expression<Func<RiDataWarehouse, RiDataWarehouseBo>> Expression()
        {
            return entity => new RiDataWarehouseBo
            {
                Id = entity.Id,
                EndingPolicyStatus = entity.EndingPolicyStatus,
                EndingPolicyStatusCode = entity.EndingPolicyStatusPickListDetail.Code,
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

        public static RiDataWarehouseBo FormBo(RiDataWarehouse entity = null, bool formatOutput = false, int? precision = null)
        {
            if (entity == null)
                return null;
            var bo = new RiDataWarehouseBo
            {
                Id = entity.Id,
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

                bo.DurationDayStr = Util.DoubleToString(entity.DurationDay, precision);
                bo.DurationMonthStr = Util.DoubleToString(entity.DurationMonth, precision);
                bo.RiCovPeriodStr = Util.DoubleToString(entity.RiCovPeriod, precision);
                bo.OriSumAssuredStr = Util.DoubleToString(entity.OriSumAssured, precision);
                bo.CurrSumAssuredStr = Util.DoubleToString(entity.CurrSumAssured, precision);
                bo.AmountCededB4MlreShareStr = Util.DoubleToString(entity.AmountCededB4MlreShare, precision);
                bo.RetentionAmountStr = Util.DoubleToString(entity.RetentionAmount, precision);
                bo.AarOriStr = Util.DoubleToString(entity.AarOri, precision);
                bo.AarStr = Util.DoubleToString(entity.Aar, precision);
                bo.AarSpecial1Str = Util.DoubleToString(entity.AarSpecial1, precision);
                bo.AarSpecial2Str = Util.DoubleToString(entity.AarSpecial2, precision);
                bo.AarSpecial3Str = Util.DoubleToString(entity.AarSpecial3, precision);
                bo.DurationYearStr = Util.DoubleToString(entity.DurationYear, precision);
                bo.CedantRiRateStr = Util.DoubleToString(entity.CedantRiRate, precision);
                bo.DiscountRateStr = Util.DoubleToString(entity.DiscountRate, precision);
                bo.UnderwriterRatingStr = Util.DoubleToString(entity.UnderwriterRating, precision);
                bo.UnderwriterRatingUnitStr = Util.DoubleToString(entity.UnderwriterRatingUnit, precision);
                bo.UnderwriterRating2Str = Util.DoubleToString(entity.UnderwriterRating2, precision);
                bo.UnderwriterRatingUnit2Str = Util.DoubleToString(entity.UnderwriterRatingUnit2, precision);
                bo.UnderwriterRating3Str = Util.DoubleToString(entity.UnderwriterRating3, precision);
                bo.UnderwriterRatingUnit3Str = Util.DoubleToString(entity.UnderwriterRatingUnit3, precision);
                bo.FlatExtraAmountStr = Util.DoubleToString(entity.FlatExtraAmount, precision);
                bo.FlatExtraUnitStr = Util.DoubleToString(entity.FlatExtraUnit, precision);
                bo.FlatExtraAmount2Str = Util.DoubleToString(entity.FlatExtraAmount2, precision);
                bo.StandardPremiumStr = Util.DoubleToString(entity.StandardPremium, precision);
                bo.SubstandardPremiumStr = Util.DoubleToString(entity.SubstandardPremium, precision);
                bo.FlatExtraPremiumStr = Util.DoubleToString(entity.FlatExtraPremium, precision);
                bo.GrossPremiumStr = Util.DoubleToString(entity.GrossPremium, precision);
                bo.StandardDiscountStr = Util.DoubleToString(entity.StandardDiscount, precision);
                bo.SubstandardDiscountStr = Util.DoubleToString(entity.SubstandardDiscount, precision);
                bo.VitalityDiscountStr = Util.DoubleToString(entity.VitalityDiscount, precision);
                bo.TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount, precision);
                bo.NetPremiumStr = Util.DoubleToString(entity.NetPremium, precision);
                bo.AnnualRiPremStr = Util.DoubleToString(entity.AnnualRiPrem, precision);
                bo.PolicyGrossPremiumStr = Util.DoubleToString(entity.PolicyGrossPremium, precision);
                bo.PolicyStandardPremiumStr = Util.DoubleToString(entity.PolicyStandardPremium, precision);
                bo.PolicySubstandardPremiumStr = Util.DoubleToString(entity.PolicySubstandardPremium, precision);
                bo.PolicyAmountDeathStr = Util.DoubleToString(entity.PolicyAmountDeath, precision);
                bo.PolicyReserveStr = Util.DoubleToString(entity.PolicyReserve, precision);
                bo.ApLoadingStr = Util.DoubleToString(entity.ApLoading, precision);
                bo.LoanInterestRateStr = Util.DoubleToString(entity.LoanInterestRate, precision);
                bo.CedantSarStr = Util.DoubleToString(entity.CedantSar, precision);
                bo.AmountCededB4MlreShare2Str = Util.DoubleToString(entity.AmountCededB4MlreShare2, precision);
                bo.GroupEmployeeBasicSalaryStr = Util.DoubleToString(entity.GroupEmployeeBasicSalary, precision);
                bo.GroupEmployeeBasicSalaryReviseStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryRevise, precision);
                bo.GroupEmployeeBasicSalaryMultiplierStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryMultiplier, precision);
                bo.PolicyAmountSubstandardStr = Util.DoubleToString(entity.PolicyAmountSubstandard, precision);
                bo.Layer1RiShareStr = Util.DoubleToString(entity.Layer1RiShare, precision);
                bo.Layer1StandardPremiumStr = Util.DoubleToString(entity.Layer1StandardPremium, precision);
                bo.Layer1SubstandardPremiumStr = Util.DoubleToString(entity.Layer1SubstandardPremium, precision);
                bo.Layer1GrossPremiumStr = Util.DoubleToString(entity.Layer1GrossPremium, precision);
                bo.Layer1StandardDiscountStr = Util.DoubleToString(entity.Layer1StandardDiscount, precision);
                bo.Layer1SubstandardDiscountStr = Util.DoubleToString(entity.Layer1SubstandardDiscount, precision);
                bo.Layer1TotalDiscountStr = Util.DoubleToString(entity.Layer1TotalDiscount, precision);
                bo.Layer1NetPremiumStr = Util.DoubleToString(entity.Layer1NetPremium, precision);
                bo.Layer1GrossPremiumAltStr = Util.DoubleToString(entity.Layer1GrossPremiumAlt, precision);
                bo.Layer1TotalDiscountAltStr = Util.DoubleToString(entity.Layer1TotalDiscountAlt, precision);
                bo.Layer1NetPremiumAltStr = Util.DoubleToString(entity.Layer1NetPremiumAlt, precision);
                bo.TaxAmountStr = Util.DoubleToString(entity.TaxAmount, precision);
                bo.GstGrossPremiumStr = Util.DoubleToString(entity.GstGrossPremium, precision);
                bo.GstTotalDiscountStr = Util.DoubleToString(entity.GstTotalDiscount, precision);
                bo.GstVitalityStr = Util.DoubleToString(entity.GstVitality, precision);
                bo.GstAmountStr = Util.DoubleToString(entity.GstAmount, precision);
                bo.CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate);
                bo.NoClaimBonusStr = Util.DoubleToString(entity.NoClaimBonus, precision);
                bo.SurrenderValueStr = Util.DoubleToString(entity.SurrenderValue, precision);
                bo.DatabaseCommisionStr = Util.DoubleToString(entity.DatabaseCommision, precision);
                bo.GrossPremiumAltStr = Util.DoubleToString(entity.GrossPremiumAlt, precision);
                bo.NetPremiumAltStr = Util.DoubleToString(entity.NetPremiumAlt, precision);
                bo.Layer1FlatExtraPremiumStr = Util.DoubleToString(entity.Layer1FlatExtraPremium, precision);
                bo.TransactionPremiumStr = Util.DoubleToString(entity.TransactionPremium, precision);
                bo.OriginalPremiumStr = Util.DoubleToString(entity.OriginalPremium, precision);
                bo.TransactionDiscountStr = Util.DoubleToString(entity.TransactionDiscount, precision);
                bo.OriginalDiscountStr = Util.DoubleToString(entity.OriginalDiscount, precision);
                bo.BrokerageFeeStr = Util.DoubleToString(entity.BrokerageFee, precision);
                bo.MaxUwRatingStr = Util.DoubleToString(entity.MaxUwRating, precision);
                bo.RetentionCapStr = Util.DoubleToString(entity.RetentionCap, precision);
                bo.AarCapStr = Util.DoubleToString(entity.AarCap, precision);
                bo.RiRateStr = Util.DoubleToString(entity.RiRate, precision);
                bo.RiRate2Str = Util.DoubleToString(entity.RiRate2, precision);
                bo.AnnuityFactorStr = Util.DoubleToString(entity.AnnuityFactor, precision);
                bo.SumAssuredOfferedStr = Util.DoubleToString(entity.SumAssuredOffered, precision);
                bo.UwRatingOfferedStr = Util.DoubleToString(entity.UwRatingOffered, precision);
                bo.FlatExtraAmountOfferedStr = Util.DoubleToString(entity.FlatExtraAmountOffered, precision);
                bo.FlatExtraDurationStr = Util.DoubleToString(entity.FlatExtraDuration, precision);
                bo.RetentionShareStr = Util.DoubleToString(entity.RetentionShare, precision);
                bo.AarShareStr = Util.DoubleToString(entity.AarShare, precision);
                bo.TotalDirectRetroAarStr = Util.DoubleToString(entity.TotalDirectRetroAar, precision);
                bo.TotalDirectRetroGrossPremiumStr = Util.DoubleToString(entity.TotalDirectRetroGrossPremium, precision);
                bo.TotalDirectRetroDiscountStr = Util.DoubleToString(entity.TotalDirectRetroDiscount, precision);
                bo.TotalDirectRetroNetPremiumStr = Util.DoubleToString(entity.TotalDirectRetroNetPremium, precision);
                bo.TotalDirectRetroNoClaimBonusStr = Util.DoubleToString(entity.TotalDirectRetroNoClaimBonus, precision);
                bo.TotalDirectRetroDatabaseCommissionStr = Util.DoubleToString(entity.TotalDirectRetroDatabaseCommission, precision);
                bo.MinAarStr = Util.DoubleToString(entity.MinAar, precision);
                bo.MaxAarStr = Util.DoubleToString(entity.MaxAar, precision);
                bo.CorridorLimitStr = Util.DoubleToString(entity.CorridorLimit, precision);
                bo.AblStr = Util.DoubleToString(entity.Abl, precision);
                bo.RiDiscountRateStr = Util.DoubleToString(entity.RiDiscountRate, precision);
                bo.LargeSaDiscountStr = Util.DoubleToString(entity.LargeSaDiscount, precision);
                bo.GroupSizeDiscountStr = Util.DoubleToString(entity.GroupSizeDiscount, precision);
                bo.MaxApLoadingStr = Util.DoubleToString(entity.MaxApLoading, precision);
                bo.MlreStandardPremiumStr = Util.DoubleToString(entity.MlreStandardPremium, precision);
                bo.MlreSubstandardPremiumStr = Util.DoubleToString(entity.MlreSubstandardPremium, precision);
                bo.MlreFlatExtraPremiumStr = Util.DoubleToString(entity.MlreFlatExtraPremium, precision);
                bo.MlreGrossPremiumStr = Util.DoubleToString(entity.MlreGrossPremium, precision);
                bo.MlreStandardDiscountStr = Util.DoubleToString(entity.MlreStandardDiscount, precision);
                bo.MlreSubstandardDiscountStr = Util.DoubleToString(entity.MlreSubstandardDiscount, precision);
                bo.MlreLargeSaDiscountStr = Util.DoubleToString(entity.MlreLargeSaDiscount, precision);
                bo.MlreGroupSizeDiscountStr = Util.DoubleToString(entity.MlreGroupSizeDiscount, precision);
                bo.MlreVitalityDiscountStr = Util.DoubleToString(entity.MlreVitalityDiscount, precision);
                bo.MlreTotalDiscountStr = Util.DoubleToString(entity.MlreTotalDiscount, precision);
                bo.MlreNetPremiumStr = Util.DoubleToString(entity.MlreNetPremium, precision);
                bo.NetPremiumCheckStr = Util.DoubleToString(entity.NetPremiumCheck, precision);
                bo.ServiceFeePercentageStr = Util.DoubleToString(entity.ServiceFeePercentage, precision);
                bo.ServiceFeeStr = Util.DoubleToString(entity.ServiceFee, precision);
                bo.MlreBrokerageFeeStr = Util.DoubleToString(entity.MlreBrokerageFee, precision);
                bo.MlreDatabaseCommissionStr = Util.DoubleToString(entity.MlreDatabaseCommission, precision);
                bo.AarShare2Str = Util.DoubleToString(entity.AarShare2, precision);
                bo.AarCap2Str = Util.DoubleToString(entity.GroupSizeDiscount, precision);
                bo.WakalahFeePercentageStr = Util.DoubleToString(entity.WakalahFeePercentage, precision);

                // Direct Retro
                bo.RetroShare1Str = Util.DoubleToString(entity.RetroShare1, precision);
                bo.RetroShare2Str = Util.DoubleToString(entity.RetroShare2, precision);
                bo.RetroShare3Str = Util.DoubleToString(entity.RetroShare3, precision);
                bo.RetroPremiumSpread1Str = Util.DoubleToString(entity.RetroPremiumSpread1, precision);
                bo.RetroPremiumSpread2Str = Util.DoubleToString(entity.RetroPremiumSpread2, precision);
                bo.RetroPremiumSpread3Str = Util.DoubleToString(entity.RetroPremiumSpread3, precision);
                bo.RetroAar1Str = Util.DoubleToString(entity.RetroAar1, precision);
                bo.RetroAar2Str = Util.DoubleToString(entity.RetroAar2, precision);
                bo.RetroAar3Str = Util.DoubleToString(entity.RetroAar3, precision);
                bo.RetroReinsurancePremium1Str = Util.DoubleToString(entity.RetroReinsurancePremium1, precision);
                bo.RetroReinsurancePremium2Str = Util.DoubleToString(entity.RetroReinsurancePremium2, precision);
                bo.RetroReinsurancePremium3Str = Util.DoubleToString(entity.RetroReinsurancePremium3, precision);
                bo.RetroDiscount1Str = Util.DoubleToString(entity.RetroDiscount1, precision);
                bo.RetroDiscount2Str = Util.DoubleToString(entity.RetroDiscount2, precision);
                bo.RetroDiscount3Str = Util.DoubleToString(entity.RetroDiscount3, precision);
                bo.RetroNetPremium1Str = Util.DoubleToString(entity.RetroNetPremium1, precision);
                bo.RetroNetPremium2Str = Util.DoubleToString(entity.RetroNetPremium2, precision);
                bo.RetroNetPremium3Str = Util.DoubleToString(entity.RetroNetPremium3, precision);
                bo.RetroNoClaimBonus1Str = Util.DoubleToString(entity.RetroNoClaimBonus1, precision);
                bo.RetroNoClaimBonus2Str = Util.DoubleToString(entity.RetroNoClaimBonus2, precision);
                bo.RetroNoClaimBonus3Str = Util.DoubleToString(entity.RetroNoClaimBonus3, precision);
                bo.RetroDatabaseCommission1Str = Util.DoubleToString(entity.RetroDatabaseCommission1, precision);
                bo.RetroDatabaseCommission2Str = Util.DoubleToString(entity.RetroDatabaseCommission2, precision);
                bo.RetroDatabaseCommission3Str = Util.DoubleToString(entity.RetroDatabaseCommission3, precision);

                bo.RecordTypeStr = RiDataBatchBo.GetRecordTypeName(entity.RecordType);
            }

            if (bo.Aar.HasValue && bo.CurrSumAssured.HasValue)
            {
                double mlreShare = bo.Aar.Value / bo.CurrSumAssured.Value;
                bo.MlreShareStr = Util.DoubleToString(mlreShare, precision);
            }

            return bo;
        }

        public static RiDataWarehouseBo FormBo(ReferralRiData entity = null, bool formatOutput = false, int? precision = null)
        {
            if (entity == null)
                return null;
            var bo = new RiDataWarehouseBo
            {
                Id = entity.Id,
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
                //TotalDirectRetroNoClaimBonus = entity.TotalDirectRetroNoClaimBonus,
                //TotalDirectRetroDatabaseCommission = entity.TotalDirectRetroDatabaseCommission,
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
                //ConflictType = entity.ConflictType,

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

                bo.DurationDayStr = Util.DoubleToString(entity.DurationDay, precision);
                bo.DurationMonthStr = Util.DoubleToString(entity.DurationMonth, precision);
                bo.RiCovPeriodStr = Util.DoubleToString(entity.RiCovPeriod, precision);
                bo.OriSumAssuredStr = Util.DoubleToString(entity.OriSumAssured, precision);
                bo.CurrSumAssuredStr = Util.DoubleToString(entity.CurrSumAssured, precision);
                bo.AmountCededB4MlreShareStr = Util.DoubleToString(entity.AmountCededB4MlreShare, precision);
                bo.RetentionAmountStr = Util.DoubleToString(entity.RetentionAmount, precision);
                bo.AarOriStr = Util.DoubleToString(entity.AarOri, precision);
                bo.AarStr = Util.DoubleToString(entity.Aar, precision);
                bo.AarSpecial1Str = Util.DoubleToString(entity.AarSpecial1, precision);
                bo.AarSpecial2Str = Util.DoubleToString(entity.AarSpecial2, precision);
                bo.AarSpecial3Str = Util.DoubleToString(entity.AarSpecial3, precision);
                bo.DurationYearStr = Util.DoubleToString(entity.DurationYear, precision);
                bo.CedantRiRateStr = Util.DoubleToString(entity.CedantRiRate, precision);
                bo.DiscountRateStr = Util.DoubleToString(entity.DiscountRate, precision);
                bo.UnderwriterRatingStr = Util.DoubleToString(entity.UnderwriterRating, precision);
                bo.UnderwriterRatingUnitStr = Util.DoubleToString(entity.UnderwriterRatingUnit, precision);
                bo.UnderwriterRating2Str = Util.DoubleToString(entity.UnderwriterRating2, precision);
                bo.UnderwriterRatingUnit2Str = Util.DoubleToString(entity.UnderwriterRatingUnit2, precision);
                bo.UnderwriterRating3Str = Util.DoubleToString(entity.UnderwriterRating3, precision);
                bo.UnderwriterRatingUnit3Str = Util.DoubleToString(entity.UnderwriterRatingUnit3, precision);
                bo.FlatExtraAmountStr = Util.DoubleToString(entity.FlatExtraAmount, precision);
                bo.FlatExtraUnitStr = Util.DoubleToString(entity.FlatExtraUnit, precision);
                bo.FlatExtraAmount2Str = Util.DoubleToString(entity.FlatExtraAmount2, precision);
                bo.StandardPremiumStr = Util.DoubleToString(entity.StandardPremium, precision);
                bo.SubstandardPremiumStr = Util.DoubleToString(entity.SubstandardPremium, precision);
                bo.FlatExtraPremiumStr = Util.DoubleToString(entity.FlatExtraPremium, precision);
                bo.GrossPremiumStr = Util.DoubleToString(entity.GrossPremium, precision);
                bo.StandardDiscountStr = Util.DoubleToString(entity.StandardDiscount, precision);
                bo.SubstandardDiscountStr = Util.DoubleToString(entity.SubstandardDiscount, precision);
                bo.VitalityDiscountStr = Util.DoubleToString(entity.VitalityDiscount, precision);
                bo.TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount, precision);
                bo.NetPremiumStr = Util.DoubleToString(entity.NetPremium, precision);
                bo.AnnualRiPremStr = Util.DoubleToString(entity.AnnualRiPrem, precision);
                bo.PolicyGrossPremiumStr = Util.DoubleToString(entity.PolicyGrossPremium, precision);
                bo.PolicyStandardPremiumStr = Util.DoubleToString(entity.PolicyStandardPremium, precision);
                bo.PolicySubstandardPremiumStr = Util.DoubleToString(entity.PolicySubstandardPremium, precision);
                bo.PolicyAmountDeathStr = Util.DoubleToString(entity.PolicyAmountDeath, precision);
                bo.PolicyReserveStr = Util.DoubleToString(entity.PolicyReserve, precision);
                bo.ApLoadingStr = Util.DoubleToString(entity.ApLoading, precision);
                bo.LoanInterestRateStr = Util.DoubleToString(entity.LoanInterestRate, precision);
                bo.CedantSarStr = Util.DoubleToString(entity.CedantSar, precision);
                bo.AmountCededB4MlreShare2Str = Util.DoubleToString(entity.AmountCededB4MlreShare2, precision);
                bo.GroupEmployeeBasicSalaryStr = Util.DoubleToString(entity.GroupEmployeeBasicSalary, precision);
                bo.GroupEmployeeBasicSalaryReviseStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryRevise, precision);
                bo.GroupEmployeeBasicSalaryMultiplierStr = Util.DoubleToString(entity.GroupEmployeeBasicSalaryMultiplier, precision);
                bo.PolicyAmountSubstandardStr = Util.DoubleToString(entity.PolicyAmountSubstandard, precision);
                bo.Layer1RiShareStr = Util.DoubleToString(entity.Layer1RiShare, precision);
                bo.Layer1StandardPremiumStr = Util.DoubleToString(entity.Layer1StandardPremium, precision);
                bo.Layer1SubstandardPremiumStr = Util.DoubleToString(entity.Layer1SubstandardPremium, precision);
                bo.Layer1GrossPremiumStr = Util.DoubleToString(entity.Layer1GrossPremium, precision);
                bo.Layer1StandardDiscountStr = Util.DoubleToString(entity.Layer1StandardDiscount, precision);
                bo.Layer1SubstandardDiscountStr = Util.DoubleToString(entity.Layer1SubstandardDiscount, precision);
                bo.Layer1TotalDiscountStr = Util.DoubleToString(entity.Layer1TotalDiscount, precision);
                bo.Layer1NetPremiumStr = Util.DoubleToString(entity.Layer1NetPremium, precision);
                bo.Layer1GrossPremiumAltStr = Util.DoubleToString(entity.Layer1GrossPremiumAlt, precision);
                bo.Layer1TotalDiscountAltStr = Util.DoubleToString(entity.Layer1TotalDiscountAlt, precision);
                bo.Layer1NetPremiumAltStr = Util.DoubleToString(entity.Layer1NetPremiumAlt, precision);
                bo.TaxAmountStr = Util.DoubleToString(entity.TaxAmount, precision);
                bo.GstGrossPremiumStr = Util.DoubleToString(entity.GstGrossPremium, precision);
                bo.GstTotalDiscountStr = Util.DoubleToString(entity.GstTotalDiscount, precision);
                bo.GstVitalityStr = Util.DoubleToString(entity.GstVitality, precision);
                bo.GstAmountStr = Util.DoubleToString(entity.GstAmount, precision);
                bo.CurrencyRateStr = Util.DoubleToString(entity.CurrencyRate);
                bo.NoClaimBonusStr = Util.DoubleToString(entity.NoClaimBonus, precision);
                bo.SurrenderValueStr = Util.DoubleToString(entity.SurrenderValue, precision);
                bo.DatabaseCommisionStr = Util.DoubleToString(entity.DatabaseCommision, precision);
                bo.GrossPremiumAltStr = Util.DoubleToString(entity.GrossPremiumAlt, precision);
                bo.NetPremiumAltStr = Util.DoubleToString(entity.NetPremiumAlt, precision);
                bo.Layer1FlatExtraPremiumStr = Util.DoubleToString(entity.Layer1FlatExtraPremium, precision);
                bo.TransactionPremiumStr = Util.DoubleToString(entity.TransactionPremium, precision);
                bo.OriginalPremiumStr = Util.DoubleToString(entity.OriginalPremium, precision);
                bo.TransactionDiscountStr = Util.DoubleToString(entity.TransactionDiscount, precision);
                bo.OriginalDiscountStr = Util.DoubleToString(entity.OriginalDiscount, precision);
                bo.BrokerageFeeStr = Util.DoubleToString(entity.BrokerageFee, precision);
                bo.MaxUwRatingStr = Util.DoubleToString(entity.MaxUwRating, precision);
                bo.RetentionCapStr = Util.DoubleToString(entity.RetentionCap, precision);
                bo.AarCapStr = Util.DoubleToString(entity.AarCap, precision);
                bo.RiRateStr = Util.DoubleToString(entity.RiRate, precision);
                bo.RiRate2Str = Util.DoubleToString(entity.RiRate2, precision);
                bo.AnnuityFactorStr = Util.DoubleToString(entity.AnnuityFactor, precision);
                bo.SumAssuredOfferedStr = Util.DoubleToString(entity.SumAssuredOffered, precision);
                bo.UwRatingOfferedStr = Util.DoubleToString(entity.UwRatingOffered, precision);
                bo.FlatExtraAmountOfferedStr = Util.DoubleToString(entity.FlatExtraAmountOffered, precision);
                bo.FlatExtraDurationStr = Util.DoubleToString(entity.FlatExtraDuration, precision);
                bo.RetentionShareStr = Util.DoubleToString(entity.RetentionShare, precision);
                bo.AarShareStr = Util.DoubleToString(entity.AarShare, precision);
                bo.TotalDirectRetroAarStr = Util.DoubleToString(entity.TotalDirectRetroAar, precision);
                bo.TotalDirectRetroGrossPremiumStr = Util.DoubleToString(entity.TotalDirectRetroGrossPremium, precision);
                bo.TotalDirectRetroDiscountStr = Util.DoubleToString(entity.TotalDirectRetroDiscount, precision);
                bo.TotalDirectRetroNetPremiumStr = Util.DoubleToString(entity.TotalDirectRetroNetPremium, precision);
                //bo.TotalDirectRetroNoClaimBonusStr = Util.DoubleToString(entity.TotalDirectRetroNoClaimBonus, precision);
                //bo.TotalDirectRetroDatabaseCommissionStr = Util.DoubleToString(entity.TotalDirectRetroDatabaseCommission, precision);
                bo.MinAarStr = Util.DoubleToString(entity.MinAar, precision);
                bo.MaxAarStr = Util.DoubleToString(entity.MaxAar, precision);
                bo.CorridorLimitStr = Util.DoubleToString(entity.CorridorLimit, precision);
                bo.AblStr = Util.DoubleToString(entity.Abl, precision);
                bo.RiDiscountRateStr = Util.DoubleToString(entity.RiDiscountRate, precision);
                bo.LargeSaDiscountStr = Util.DoubleToString(entity.LargeSaDiscount, precision);
                bo.GroupSizeDiscountStr = Util.DoubleToString(entity.GroupSizeDiscount, precision);
                bo.MaxApLoadingStr = Util.DoubleToString(entity.MaxApLoading, precision);
                bo.MlreStandardPremiumStr = Util.DoubleToString(entity.MlreStandardPremium, precision);
                bo.MlreSubstandardPremiumStr = Util.DoubleToString(entity.MlreSubstandardPremium, precision);
                bo.MlreFlatExtraPremiumStr = Util.DoubleToString(entity.MlreFlatExtraPremium, precision);
                bo.MlreGrossPremiumStr = Util.DoubleToString(entity.MlreGrossPremium, precision);
                bo.MlreStandardDiscountStr = Util.DoubleToString(entity.MlreStandardDiscount, precision);
                bo.MlreSubstandardDiscountStr = Util.DoubleToString(entity.MlreSubstandardDiscount, precision);
                bo.MlreLargeSaDiscountStr = Util.DoubleToString(entity.MlreLargeSaDiscount, precision);
                bo.MlreGroupSizeDiscountStr = Util.DoubleToString(entity.MlreGroupSizeDiscount, precision);
                bo.MlreVitalityDiscountStr = Util.DoubleToString(entity.MlreVitalityDiscount, precision);
                bo.MlreTotalDiscountStr = Util.DoubleToString(entity.MlreTotalDiscount, precision);
                bo.MlreNetPremiumStr = Util.DoubleToString(entity.MlreNetPremium, precision);
                bo.NetPremiumCheckStr = Util.DoubleToString(entity.NetPremiumCheck, precision);
                bo.ServiceFeePercentageStr = Util.DoubleToString(entity.ServiceFeePercentage, precision);
                bo.ServiceFeeStr = Util.DoubleToString(entity.ServiceFee, precision);
                bo.MlreBrokerageFeeStr = Util.DoubleToString(entity.MlreBrokerageFee, precision);
                bo.MlreDatabaseCommissionStr = Util.DoubleToString(entity.MlreDatabaseCommission, precision);
                bo.AarShare2Str = Util.DoubleToString(entity.AarShare2, precision);
                bo.AarCap2Str = Util.DoubleToString(entity.GroupSizeDiscount, precision);
                bo.WakalahFeePercentageStr = Util.DoubleToString(entity.WakalahFeePercentage, precision);
            }
            return bo;
        }

        public static RiDataWarehouse FormEntity(RiDataWarehouseBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataWarehouse
            {
                Id = bo.Id,
                EndingPolicyStatus = bo.EndingPolicyStatus,
                RecordType = bo.RecordType,
                Quarter = bo.Quarter,
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
                TotalDirectRetroNoClaimBonus = bo.TotalDirectRetroNoClaimBonus,
                TotalDirectRetroDatabaseCommission = bo.TotalDirectRetroDatabaseCommission,
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
                ConflictType = bo.ConflictType,

                // Direct Retro
                RetroParty1 = bo.RetroParty1,
                RetroParty2 = bo.RetroParty2,
                RetroParty3 = bo.RetroParty3,
                RetroShare1 = bo.RetroShare1,
                RetroShare2 = bo.RetroShare2,
                RetroShare3 = bo.RetroShare3,
                RetroPremiumSpread1 = bo.RetroPremiumSpread1,
                RetroPremiumSpread2 = bo.RetroPremiumSpread2,
                RetroPremiumSpread3 = bo.RetroPremiumSpread3,
                RetroAar1 = bo.RetroAar1,
                RetroAar2 = bo.RetroAar2,
                RetroAar3 = bo.RetroAar3,
                RetroReinsurancePremium1 = bo.RetroReinsurancePremium1,
                RetroReinsurancePremium2 = bo.RetroReinsurancePremium2,
                RetroReinsurancePremium3 = bo.RetroReinsurancePremium3,
                RetroDiscount1 = bo.RetroDiscount1,
                RetroDiscount2 = bo.RetroDiscount2,
                RetroDiscount3 = bo.RetroDiscount3,
                RetroNetPremium1 = bo.RetroNetPremium1,
                RetroNetPremium2 = bo.RetroNetPremium2,
                RetroNetPremium3 = bo.RetroNetPremium3,
                RetroNoClaimBonus1 = bo.RetroNoClaimBonus1,
                RetroNoClaimBonus2 = bo.RetroNoClaimBonus2,
                RetroNoClaimBonus3 = bo.RetroNoClaimBonus3,
                RetroDatabaseCommission1 = bo.RetroDatabaseCommission1,
                RetroDatabaseCommission2 = bo.RetroDatabaseCommission2,
                RetroDatabaseCommission3 = bo.RetroDatabaseCommission3,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<RiDataWarehouseBo> FormBos(IList<RiDataWarehouse> entities = null, bool formatOutput = false, int? precision = null)
        {
            if (entities == null)
                return null;
            IList<RiDataWarehouseBo> bos = new List<RiDataWarehouseBo>() { };
            foreach (RiDataWarehouse entity in entities)
            {
                bos.Add(FormBo(entity, formatOutput));
            }
            return bos;
        }

        public static IList<RiDataWarehouseBo> FormBos(IList<ReferralRiData> entities = null, bool formatOutput = false, int? precision = null)
        {
            if (entities == null)
                return null;
            IList<RiDataWarehouseBo> bos = new List<RiDataWarehouseBo>() { };
            foreach (ReferralRiData entity in entities)
            {
                bos.Add(FormBo(entity, formatOutput));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return RiDataWarehouse.IsExists(id);
        }

        public static int CountForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                    .Where(q => q.RiskPeriodYear == year);

                if (monthEnd != null)
                {
                    query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                }
                else
                {
                    query = query.Where(q => q.RiskPeriodMonth == monthStart);
                }

                return query.Count();
            }
        }

        public static RiDataWarehouseBo Find(int? id, bool formatOutput = false)
        {
            if (!id.HasValue)
                return null;

            return FormBo(RiDataWarehouse.Find(id.Value), formatOutput);
        }

        public static RiDataWarehouseBo Find(int id)
        {
            return FormBo(RiDataWarehouse.Find(id));
        }

        public static RiDataWarehouseBo Find(int? riDataWarehouseId, int? referralRiDataId)
        {
            if (riDataWarehouseId.HasValue)
            {
                return FormBo(RiDataWarehouse.Find(riDataWarehouseId.Value), true);
            }
            else if (referralRiDataId.HasValue)
            {
                return FormBo(ReferralRiData.Find(referralRiDataId.Value), true);
            }

            return null;
        }

        public static RiDataWarehouseBo FindByRiData(RiDataBo riDataBo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataWarehouse.Where(q => q.PolicyNumber == riDataBo.PolicyNumber)
                    .Where(q => q.CedingPlanCode == riDataBo.CedingPlanCode)
                    .Where(q => q.RiskPeriodMonth == riDataBo.RiskPeriodMonth)
                    .Where(q => q.RiskPeriodYear == riDataBo.RiskPeriodYear)
                    .Where(q => q.MlreBenefitCode == riDataBo.MlreBenefitCode)
                    .Where(q => q.TreatyCode == riDataBo.TreatyCode)
                    .Where(q => q.RiderNumber == riDataBo.RiderNumber)
                    .Where(q => q.CedingBenefitTypeCode == riDataBo.CedingBenefitTypeCode)
                    .Where(q => q.CedingBenefitRiskCode == riDataBo.CedingBenefitRiskCode)
                    .Where(q => q.CedingPlanCode2 == riDataBo.CedingPlanCode2)
                    .Where(q => q.CessionCode == riDataBo.CessionCode);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static RiDataWarehouseBo FindByGroupParam(ClaimRegisterBo claimRegisterBo, List<string> benefitCodes, int step = 1)
        {
            using (var db = new AppDbContext())
            {
                foreach (string benefitCode in benefitCodes)
                {
                    var query = db.RiDataWarehouse.Where(q => q.TreatyCode == claimRegisterBo.TreatyCode)
                        .Where(q => q.MlreBenefitCode == benefitCode)
                        .Where(q => DbFunctions.TruncateTime(q.RiskPeriodStartDate) <= DbFunctions.TruncateTime(claimRegisterBo.DateOfEvent)
                                && DbFunctions.TruncateTime(q.RiskPeriodEndDate) >= DbFunctions.TruncateTime(claimRegisterBo.DateOfEvent));

                    if (!string.IsNullOrEmpty(claimRegisterBo.CedingPlanCode))
                    {
                        query = query.Where(q => q.CedingPlanCode == claimRegisterBo.CedingPlanCode);
                    }

                    if (step == 2)
                    {
                        query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.GroupPolicyNumber == claimRegisterBo.PolicyNumber) || q.InsuredName == claimRegisterBo.InsuredName);
                    }
                    else
                    {
                        query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.GroupPolicyNumber == claimRegisterBo.PolicyNumber) && q.InsuredName == claimRegisterBo.InsuredName);
                    }

                    if (query.Any())
                        return FormBo(query.FirstOrDefault(), false);
                }
                return null;
            }
        }

        public static RiDataWarehouseBo FindByIndividualParam(ClaimRegisterBo claimRegisterBo, List<string> benefitCodes, int step = 1)
        {
            using (var db = new AppDbContext())
            {
                foreach (string benefitCode in benefitCodes)
                {
                    var query = db.RiDataWarehouse.Where(q => q.TreatyCode == claimRegisterBo.TreatyCode)
                        .Where(q => q.MlreBenefitCode == benefitCode)
                        .Where(q => DbFunctions.TruncateTime(q.RiskPeriodStartDate) <= DbFunctions.TruncateTime(claimRegisterBo.DateOfEvent)
                                && DbFunctions.TruncateTime(q.RiskPeriodEndDate) >= DbFunctions.TruncateTime(claimRegisterBo.DateOfEvent));

                    if (!string.IsNullOrEmpty(claimRegisterBo.CedingPlanCode))
                    {
                        query = query.Where(q => q.CedingPlanCode == claimRegisterBo.CedingPlanCode);
                    }

                    if (step == 2)
                    {
                        query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumberOld) && q.PolicyNumberOld == claimRegisterBo.PolicyNumber) || q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.InsuredName == claimRegisterBo.InsuredName);
                    }
                    else
                    {
                        query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumberOld) && q.PolicyNumberOld == claimRegisterBo.PolicyNumber) || q.PolicyNumber == claimRegisterBo.PolicyNumber) && q.InsuredName == claimRegisterBo.InsuredName);
                    }

                    if (query.Any())
                        return FormBo(query.FirstOrDefault(), false);
                }
                return null;
            }
        }

        public static IList<RiDataWarehouseBo> GetByClaimRegisterParam(string policyNumber, string cedingPlanCode, int? riskYear, int? riskMonth, string soaQuarter, string mlreBenefitCode, string cedingBenefitRiskCode, string treatyCode, DateTime? dateOfEvent)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataWarehouse
                    .Where(q => q.PolicyNumber == policyNumber);

                if (!string.IsNullOrEmpty(cedingPlanCode))
                    query = query.Where(q => q.CedingPlanCode == cedingPlanCode);

                if (riskYear.HasValue)
                    query = query.Where(q => q.RiskPeriodYear == riskYear);

                if (riskMonth.HasValue)
                    query = query.Where(q => q.RiskPeriodMonth == riskMonth);

                //if (!string.IsNullOrEmpty(soaQuarter))
                //    query = query.Where(q => q.Quarter == soaQuarter);

                if (!string.IsNullOrEmpty(soaQuarter))
                {
                    string[] quarterStr = soaQuarter.Split(' ');
                    List<int> months = new List<int> { };

                    switch (quarterStr[1])
                    {
                        case "Q1": months = new List<int> { 1, 2, 3 }; break;
                        case "Q2": months = new List<int> { 4, 5, 6 }; break;
                        case "Q3": months = new List<int> { 7, 8, 9 }; break;
                        case "Q4": months = new List<int> { 10, 11, 12 }; break;
                    }

                    int quarterYear = Convert.ToInt32(quarterStr[0]);
                    query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                }

                if (!string.IsNullOrEmpty(mlreBenefitCode))
                    query = query.Where(q => q.MlreBenefitCode == mlreBenefitCode);

                if (!string.IsNullOrEmpty(cedingBenefitRiskCode))
                    query = query.Where(q => q.CedingBenefitRiskCode == cedingBenefitRiskCode);

                if (!string.IsNullOrEmpty(treatyCode))
                    query = query.Where(q => q.TreatyCode == treatyCode);

                if (dateOfEvent.HasValue)
                {
                    query = query.Where(q => DbFunctions.TruncateTime(q.RiskPeriodStartDate) <= DbFunctions.TruncateTime(dateOfEvent)
                            && DbFunctions.TruncateTime(q.RiskPeriodEndDate) >= DbFunctions.TruncateTime(dateOfEvent));
                }

                return FormBos(query.ToList(), true);
            }
        }

        public static IList<RiDataWarehouseBo> GetByReferralClaimParam(string insuredName, string policyNumber, string treatyCode = null, string cedingPlanCode = null, DateTime? dateOfBirth = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataWarehouse
                    .Where(q => q.InsuredName == insuredName)
                    .Where(q => q.PolicyNumber == policyNumber);

                if (!string.IsNullOrEmpty(treatyCode))
                    query = query.Where(q => q.TreatyCode == treatyCode);

                if (!string.IsNullOrEmpty(cedingPlanCode))
                    query = query.Where(q => q.CedingPlanCode == cedingPlanCode);

                if (dateOfBirth.HasValue)
                    query = query.Where(q => q.InsuredDateOfBirth == dateOfBirth);

                return FormBos(query.ToList(), true, 2);
            }
        }

        public static IList<RiDataWarehouseBo> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.RiDataWarehouse.Where(q => ids.Contains(q.Id)).ToList());
            }
        }

        public static int GetMinId()
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouse.OrderBy(q => q.Id).Select(q => q.Id).FirstOrDefault();
            }
        }

        public static int GetMaxId()
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouse.OrderByDescending(q => q.Id).Select(q => q.Id).FirstOrDefault();
            }
        }

        public static int GetCount()
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouse.Count();
            }
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                    .Where(q => q.RiskPeriodYear == year);

                if (monthEnd != null)
                {
                    query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                }
                else
                {
                    query = query.Where(q => q.RiskPeriodMonth == monthStart);
                }

                return query.GroupBy(q => q.Mfrs17TreatyCode).Select(q => q.FirstOrDefault().Mfrs17TreatyCode).ToList();
            }
        }

        public static List<int> GetIdsForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, string mfrs17TreatyCode, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                    .Where(q => q.Mfrs17TreatyCode == mfrs17TreatyCode)
                    .Where(q => q.RiskPeriodYear == year);

                if (monthEnd != null)
                {
                    query = query.Where(q => q.RiskPeriodMonth >= monthStart).Where(q => q.RiskPeriodMonth <= monthEnd);
                }
                else
                {
                    query = query.Where(q => q.RiskPeriodMonth == monthStart);
                }
                return query.Select(q => q.Id).ToList();
            }
        }

        public static int? GetMaxYearByTreatyCodeYear(string treatyCode, int year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouse
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear <= year)
                    .OrderByDescending(q => q.RiskPeriodYear)
                    .Select(q => q.RiskPeriodYear)
                    .FirstOrDefault();
            }
        }

        public static int? GetMaxMonthByTreatyCodeMonthYear(string treatyCode, int? month, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear <= year);

                if (month != null)
                    query = query.Where(q => q.RiskPeriodMonth <= month);

                return query.OrderByDescending(q => q.RiskPeriodMonth)
                    .Select(q => q.RiskPeriodMonth)
                    .FirstOrDefault();
            }
        }

        public static int CountByTreatyCodeByQuarter(string treatyCode, string quarter, List<string> transactionTypes = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode).Where(q => q.Quarter == quarter);
                if (transactionTypes != null)
                {
                    query.Where(q => transactionTypes.Contains(q.TransactionTypeCode));
                }
                return query.Count();
            }
        }

        public static IList<RiDataWarehouseBo> GetByTreatyCodeByQuarter(string treatyCode, string quarter, List<string> transactionTypes = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode).Where(q => q.Quarter == quarter);
                if (transactionTypes != null)
                {
                    query.Where(q => transactionTypes.Contains(q.TransactionTypeCode));
                }
                return FormBos(query.ToList(), true);
            }
        }

        public static IList<RiDataWarehouseBo> GetByTreatyCodeByQuarter(string treatyCode, string quarter, int skip, int take, List<string> transactionTypes = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.Where(q => q.TreatyCode == treatyCode).Where(q => q.Quarter == quarter);
                if (transactionTypes != null)
                {
                    query.Where(q => transactionTypes.Contains(q.TransactionTypeCode));
                }
                return FormBos(query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList());
            }
        }

        public static IList<RiDataWarehouseBo> GetByRiDataParam(string policyNumber, string planCode, string quarter, string riskQuarter, string mlreBenefitCode, string cedingBenefitTypeCode, int? riskPeriodMonth, int? riskPeriodYear, string treatyCode, int? riderNumber)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiDataWarehouse.AsQueryable();

                if (!string.IsNullOrEmpty(policyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == policyNumber);
                if (!string.IsNullOrEmpty(planCode)) query = query.Where(q => !string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode == planCode);
                if (!string.IsNullOrEmpty(quarter))
                {
                    string[] quarterStr = quarter.Split(' ');
                    List<int> months = new List<int> { };

                    switch (quarterStr[1])
                    {
                        case "Q1": months = new List<int> { 1, 2, 3 }; break;
                        case "Q2": months = new List<int> { 4, 5, 6 }; break;
                        case "Q3": months = new List<int> { 7, 8, 9 }; break;
                        case "Q4": months = new List<int> { 10, 11, 12 }; break;
                    }

                    int quarterYear = Convert.ToInt32(quarterStr[0]);
                    query = query.Where(q => months.Contains(q.ReportPeriodMonth.Value) && q.ReportPeriodYear == quarterYear);
                }
                if (!string.IsNullOrEmpty(mlreBenefitCode)) query = query.Where(q => !string.IsNullOrEmpty(q.MlreBenefitCode) && q.MlreBenefitCode == mlreBenefitCode);
                if (!string.IsNullOrEmpty(cedingBenefitTypeCode)) query = query.Where(q => !string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode == cedingBenefitTypeCode);
                if (riskPeriodMonth.HasValue) query = query.Where(q => q.RiskPeriodMonth == riskPeriodMonth);
                if (riskPeriodYear.HasValue) query = query.Where(q => q.RiskPeriodYear == riskPeriodYear);
                if (!string.IsNullOrEmpty(treatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode == treatyCode);
                if (riderNumber.HasValue) query = query.Where(q => q.RiderNumber == riderNumber);

                return FormBos(query.ToList(), true);
            }
        }

        public static int CountByLookupParams(RiDataBo riData)
        {
            return CountByLookupParams(
                riData.PolicyNumber,
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.TreatyCode,
                riData.InsuredName,
                riData.CedingBenefitTypeCode,
                riData.CedingBenefitRiskCode,
                riData.CedingPlanCode2,
                riData.CessionCode,
                riData.RiskPeriodMonth,
                riData.RiskPeriodYear,
                riData.RiderNumber
            );
        }

        public static int CountByLookupParams(
            string policyNumber,
            string cedingPlanCode,
            string mlreBenefitCode,
            string treatyCode,
            string insuredName,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingPlanCode2,
            string cessionCode,
            int? riskPeriodMonth = null,
            int? riskPeriodYear = null,
            int? riderNumber = null
        )
        {
            return RiDataWarehouse.CountByLookupParams(
                policyNumber,
                cedingPlanCode,
                mlreBenefitCode,
                treatyCode,
                insuredName,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                cedingPlanCode2,
                cessionCode,
                riskPeriodMonth,
                riskPeriodYear,
                riderNumber
            );
        }

        public static RiDataWarehouseBo FindByLookupParams(RiDataBo riData)
        {
            return FindByLookupParams(
                riData.PolicyNumber,
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.TreatyCode,
                riData.InsuredName,
                riData.CedingBenefitTypeCode,
                riData.CedingBenefitRiskCode,
                riData.CedingPlanCode2,
                riData.CessionCode,
                riData.RiskPeriodMonth,
                riData.RiskPeriodYear,
                riData.RiderNumber
            );
        }

        public static RiDataWarehouseBo FindByLookupParams(
            string policyNumber,
            string cedingPlanCode,
            string mlreBenefitCode,
            string treatyCode,
            string insuredName,
            string cedingBenefitTypeCode,
            string cedingBenefitRiskCode,
            string cedingPlanCode2,
            string cessionCode,
            int? riskPeriodMonth = null,
            int? riskPeriodYear = null,
            int? riderNumber = null
        )
        {
            return FormBo(RiDataWarehouse.FindByLookupParams(
                policyNumber,
                cedingPlanCode,
                mlreBenefitCode,
                treatyCode,
                insuredName,
                cedingBenefitTypeCode,
                cedingBenefitRiskCode,
                cedingPlanCode2,
                cessionCode,
                riskPeriodMonth,
                riskPeriodYear,
                riderNumber
            ), false);
        }

        public static Result Save(ref RiDataWarehouseBo bo)
        {
            if (!RiDataWarehouse.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RiDataWarehouseBo bo, ref TrailObject trail)
        {
            if (!RiDataWarehouse.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataWarehouseBo bo)
        {
            RiDataWarehouse entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataWarehouseBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataWarehouseBo bo)
        {
            Result result = Result();

            RiDataWarehouse entity = RiDataWarehouse.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            entity.EndingPolicyStatus = bo.EndingPolicyStatus;
            entity.RecordType = bo.RecordType;
            entity.Quarter = bo.Quarter;
            entity.TreatyCode = bo.TreatyCode;
            entity.ReinsBasisCode = bo.ReinsBasisCode;
            entity.FundsAccountingTypeCode = bo.FundsAccountingTypeCode;
            entity.PremiumFrequencyCode = bo.PremiumFrequencyCode;
            entity.ReportPeriodMonth = bo.ReportPeriodMonth;
            entity.ReportPeriodYear = bo.ReportPeriodYear;
            entity.RiskPeriodMonth = bo.RiskPeriodMonth;
            entity.RiskPeriodYear = bo.RiskPeriodYear;
            entity.TransactionTypeCode = bo.TransactionTypeCode;
            entity.PolicyNumber = bo.PolicyNumber;
            entity.IssueDatePol = bo.IssueDatePol;
            entity.IssueDateBen = bo.IssueDateBen;
            entity.ReinsEffDatePol = bo.ReinsEffDatePol;
            entity.ReinsEffDateBen = bo.ReinsEffDateBen;
            entity.CedingPlanCode = bo.CedingPlanCode;
            entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
            entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
            entity.MlreBenefitCode = bo.MlreBenefitCode;
            entity.OriSumAssured = bo.OriSumAssured;
            entity.CurrSumAssured = bo.CurrSumAssured;
            entity.AmountCededB4MlreShare = bo.AmountCededB4MlreShare;
            entity.RetentionAmount = bo.RetentionAmount;
            entity.AarOri = bo.AarOri;
            entity.Aar = bo.Aar;
            entity.AarSpecial1 = bo.AarSpecial1;
            entity.AarSpecial2 = bo.AarSpecial2;
            entity.AarSpecial3 = bo.AarSpecial3;
            entity.InsuredName = bo.InsuredName;
            entity.InsuredGenderCode = bo.InsuredGenderCode;
            entity.InsuredTobaccoUse = bo.InsuredTobaccoUse;
            entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
            entity.InsuredOccupationCode = bo.InsuredOccupationCode;
            entity.InsuredRegisterNo = bo.InsuredRegisterNo;
            entity.InsuredAttainedAge = bo.InsuredAttainedAge;
            entity.InsuredNewIcNumber = bo.InsuredNewIcNumber;
            entity.InsuredOldIcNumber = bo.InsuredOldIcNumber;
            entity.InsuredName2nd = bo.InsuredName2nd;
            entity.InsuredGenderCode2nd = bo.InsuredGenderCode2nd;
            entity.InsuredTobaccoUse2nd = bo.InsuredTobaccoUse2nd;
            entity.InsuredDateOfBirth2nd = bo.InsuredDateOfBirth2nd;
            entity.InsuredAttainedAge2nd = bo.InsuredAttainedAge2nd;
            entity.InsuredNewIcNumber2nd = bo.InsuredNewIcNumber2nd;
            entity.InsuredOldIcNumber2nd = bo.InsuredOldIcNumber2nd;
            entity.ReinsuranceIssueAge = bo.ReinsuranceIssueAge;
            entity.ReinsuranceIssueAge2nd = bo.ReinsuranceIssueAge2nd;
            entity.PolicyTerm = bo.PolicyTerm;
            entity.PolicyExpiryDate = bo.PolicyExpiryDate;
            entity.DurationYear = bo.DurationYear;
            entity.DurationDay = bo.DurationDay;
            entity.DurationMonth = bo.DurationMonth;
            entity.PremiumCalType = bo.PremiumCalType;
            entity.CedantRiRate = bo.CedantRiRate;
            entity.RateTable = bo.RateTable;
            entity.AgeRatedUp = bo.AgeRatedUp;
            entity.DiscountRate = bo.DiscountRate;
            entity.LoadingType = bo.LoadingType;
            entity.UnderwriterRating = bo.UnderwriterRating;
            entity.UnderwriterRatingUnit = bo.UnderwriterRatingUnit;
            entity.UnderwriterRatingTerm = bo.UnderwriterRatingTerm;
            entity.UnderwriterRating2 = bo.UnderwriterRating2;
            entity.UnderwriterRatingUnit2 = bo.UnderwriterRatingUnit2;
            entity.UnderwriterRatingTerm2 = bo.UnderwriterRatingTerm2;
            entity.UnderwriterRating3 = bo.UnderwriterRating3;
            entity.UnderwriterRatingUnit3 = bo.UnderwriterRatingUnit3;
            entity.UnderwriterRatingTerm3 = bo.UnderwriterRatingTerm3;
            entity.FlatExtraAmount = bo.FlatExtraAmount;
            entity.FlatExtraUnit = bo.FlatExtraUnit;
            entity.FlatExtraTerm = bo.FlatExtraTerm;
            entity.FlatExtraAmount2 = bo.FlatExtraAmount2;
            entity.FlatExtraTerm2 = bo.FlatExtraTerm2;
            entity.StandardPremium = bo.StandardPremium;
            entity.SubstandardPremium = bo.SubstandardPremium;
            entity.FlatExtraPremium = bo.FlatExtraPremium;
            entity.GrossPremium = bo.GrossPremium;
            entity.StandardDiscount = bo.StandardDiscount;
            entity.SubstandardDiscount = bo.SubstandardDiscount;
            entity.VitalityDiscount = bo.VitalityDiscount;
            entity.TotalDiscount = bo.TotalDiscount;
            entity.NetPremium = bo.NetPremium;
            entity.AnnualRiPrem = bo.AnnualRiPrem;
            entity.RiCovPeriod = bo.RiCovPeriod;
            entity.AdjBeginDate = bo.AdjBeginDate;
            entity.AdjEndDate = bo.AdjEndDate;
            entity.PolicyNumberOld = bo.PolicyNumberOld;
            entity.PolicyStatusCode = bo.PolicyStatusCode;
            entity.PolicyGrossPremium = bo.PolicyGrossPremium;
            entity.PolicyStandardPremium = bo.PolicyStandardPremium;
            entity.PolicySubstandardPremium = bo.PolicySubstandardPremium;
            entity.PolicyTermRemain = bo.PolicyTermRemain;
            entity.PolicyAmountDeath = bo.PolicyAmountDeath;
            entity.PolicyReserve = bo.PolicyReserve;
            entity.PolicyPaymentMethod = bo.PolicyPaymentMethod;
            entity.PolicyLifeNumber = bo.PolicyLifeNumber;
            entity.FundCode = bo.FundCode;
            entity.LineOfBusiness = bo.LineOfBusiness;
            entity.ApLoading = bo.ApLoading;
            entity.LoanInterestRate = bo.LoanInterestRate;
            entity.DefermentPeriod = bo.DefermentPeriod;
            entity.RiderNumber = bo.RiderNumber;
            entity.CampaignCode = bo.CampaignCode;
            entity.Nationality = bo.Nationality;
            entity.TerritoryOfIssueCode = bo.TerritoryOfIssueCode;
            entity.CurrencyCode = bo.CurrencyCode;
            entity.StaffPlanIndicator = bo.StaffPlanIndicator;
            entity.CedingTreatyCode = bo.CedingTreatyCode;
            entity.CedingPlanCodeOld = bo.CedingPlanCodeOld;
            entity.CedingBasicPlanCode = bo.CedingBasicPlanCode;
            entity.CedantSar = bo.CedantSar;
            entity.CedantReinsurerCode = bo.CedantReinsurerCode;
            entity.AmountCededB4MlreShare2 = bo.AmountCededB4MlreShare2;
            entity.CessionCode = bo.CessionCode;
            entity.CedantRemark = bo.CedantRemark;
            entity.GroupPolicyNumber = bo.GroupPolicyNumber;
            entity.GroupPolicyName = bo.GroupPolicyName;
            entity.NoOfEmployee = bo.NoOfEmployee;
            entity.PolicyTotalLive = bo.PolicyTotalLive;
            entity.GroupSubsidiaryName = bo.GroupSubsidiaryName;
            entity.GroupSubsidiaryNo = bo.GroupSubsidiaryNo;
            entity.GroupEmployeeBasicSalary = bo.GroupEmployeeBasicSalary;
            entity.GroupEmployeeJobType = bo.GroupEmployeeJobType;
            entity.GroupEmployeeJobCode = bo.GroupEmployeeJobCode;
            entity.GroupEmployeeBasicSalaryRevise = bo.GroupEmployeeBasicSalaryRevise;
            entity.GroupEmployeeBasicSalaryMultiplier = bo.GroupEmployeeBasicSalaryMultiplier;
            entity.CedingPlanCode2 = bo.CedingPlanCode2;
            entity.DependantIndicator = bo.DependantIndicator;
            entity.GhsRoomBoard = bo.GhsRoomBoard;
            entity.PolicyAmountSubstandard = bo.PolicyAmountSubstandard;
            entity.Layer1RiShare = bo.Layer1RiShare;
            entity.Layer1InsuredAttainedAge = bo.Layer1InsuredAttainedAge;
            entity.Layer1InsuredAttainedAge2nd = bo.Layer1InsuredAttainedAge2nd;
            entity.Layer1StandardPremium = bo.Layer1StandardPremium;
            entity.Layer1SubstandardPremium = bo.Layer1SubstandardPremium;
            entity.Layer1GrossPremium = bo.Layer1GrossPremium;
            entity.Layer1StandardDiscount = bo.Layer1StandardDiscount;
            entity.Layer1SubstandardDiscount = bo.Layer1SubstandardDiscount;
            entity.Layer1TotalDiscount = bo.Layer1TotalDiscount;
            entity.Layer1NetPremium = bo.Layer1NetPremium;
            entity.Layer1GrossPremiumAlt = bo.Layer1GrossPremiumAlt;
            entity.Layer1TotalDiscountAlt = bo.Layer1TotalDiscountAlt;
            entity.Layer1NetPremiumAlt = bo.Layer1NetPremiumAlt;
            entity.SpecialIndicator1 = bo.SpecialIndicator1;
            entity.SpecialIndicator2 = bo.SpecialIndicator2;
            entity.SpecialIndicator3 = bo.SpecialIndicator3;
            entity.IndicatorJointLife = bo.IndicatorJointLife;
            entity.TaxAmount = bo.TaxAmount;
            entity.GstIndicator = bo.GstIndicator;
            entity.GstGrossPremium = bo.GstGrossPremium;
            entity.GstTotalDiscount = bo.GstTotalDiscount;
            entity.GstVitality = bo.GstVitality;
            entity.GstAmount = bo.GstAmount;
            entity.Mfrs17BasicRider = bo.Mfrs17BasicRider;
            entity.Mfrs17CellName = bo.Mfrs17CellName;
            entity.Mfrs17TreatyCode = bo.Mfrs17TreatyCode;
            entity.LoaCode = bo.LoaCode;
            entity.CurrencyRate = bo.CurrencyRate;
            entity.NoClaimBonus = bo.NoClaimBonus;
            entity.SurrenderValue = bo.SurrenderValue;
            entity.DatabaseCommision = bo.DatabaseCommision;
            entity.GrossPremiumAlt = bo.GrossPremiumAlt;
            entity.NetPremiumAlt = bo.NetPremiumAlt;
            entity.Layer1FlatExtraPremium = bo.Layer1FlatExtraPremium;
            entity.TransactionPremium = bo.TransactionPremium;
            entity.OriginalPremium = bo.OriginalPremium;
            entity.TransactionDiscount = bo.TransactionDiscount;
            entity.OriginalDiscount = bo.OriginalDiscount;
            entity.BrokerageFee = bo.BrokerageFee;
            entity.MaxUwRating = bo.MaxUwRating;
            entity.RetentionCap = bo.RetentionCap;
            entity.AarCap = bo.AarCap;
            entity.RiRate = bo.RiRate;
            entity.RiRate2 = bo.RiRate2;
            entity.AnnuityFactor = bo.AnnuityFactor;
            entity.SumAssuredOffered = bo.SumAssuredOffered;
            entity.UwRatingOffered = bo.UwRatingOffered;
            entity.FlatExtraAmountOffered = bo.FlatExtraAmountOffered;
            entity.FlatExtraDuration = bo.FlatExtraDuration;
            entity.EffectiveDate = bo.EffectiveDate;
            entity.OfferLetterSentDate = bo.OfferLetterSentDate;
            entity.RiskPeriodStartDate = bo.RiskPeriodStartDate;
            entity.RiskPeriodEndDate = bo.RiskPeriodEndDate;
            entity.Mfrs17AnnualCohort = bo.Mfrs17AnnualCohort;
            entity.MaxExpiryAge = bo.MaxExpiryAge;
            entity.MinIssueAge = bo.MinIssueAge;
            entity.MaxIssueAge = bo.MaxIssueAge;
            entity.MinAar = bo.MinAar;
            entity.MaxAar = bo.MaxAar;
            entity.CorridorLimit = bo.CorridorLimit;
            entity.Abl = bo.Abl;
            entity.RatePerBasisUnit = bo.RatePerBasisUnit;
            entity.RiDiscountRate = bo.RiDiscountRate;
            entity.LargeSaDiscount = bo.LargeSaDiscount;
            entity.GroupSizeDiscount = bo.GroupSizeDiscount;
            entity.EwarpNumber = bo.EwarpNumber;
            entity.EwarpActionCode = bo.EwarpActionCode;
            entity.RetentionShare = bo.RetentionShare;
            entity.AarShare = bo.AarShare;
            entity.ProfitComm = bo.ProfitComm;
            entity.TotalDirectRetroAar = bo.TotalDirectRetroAar;
            entity.TotalDirectRetroGrossPremium = bo.TotalDirectRetroGrossPremium;
            entity.TotalDirectRetroDiscount = bo.TotalDirectRetroDiscount;
            entity.TotalDirectRetroNetPremium = bo.TotalDirectRetroNetPremium;
            entity.TotalDirectRetroNoClaimBonus = bo.TotalDirectRetroNoClaimBonus;
            entity.TotalDirectRetroDatabaseCommission = bo.TotalDirectRetroDatabaseCommission;
            entity.TreatyType = bo.TreatyType;
            entity.MaxApLoading = bo.MaxApLoading;
            entity.MlreInsuredAttainedAgeAtCurrentMonth = bo.MlreInsuredAttainedAgeAtCurrentMonth;
            entity.MlreInsuredAttainedAgeAtPreviousMonth = bo.MlreInsuredAttainedAgeAtPreviousMonth;
            entity.InsuredAttainedAgeCheck = bo.InsuredAttainedAgeCheck;
            entity.MaxExpiryAgeCheck = bo.MaxExpiryAgeCheck;
            entity.MlrePolicyIssueAge = bo.MlrePolicyIssueAge;
            entity.PolicyIssueAgeCheck = bo.PolicyIssueAgeCheck;
            entity.MinIssueAgeCheck = bo.MinIssueAgeCheck;
            entity.MaxIssueAgeCheck = bo.MaxIssueAgeCheck;
            entity.MaxUwRatingCheck = bo.MaxUwRatingCheck;
            entity.ApLoadingCheck = bo.ApLoadingCheck;
            entity.EffectiveDateCheck = bo.EffectiveDateCheck;
            entity.MinAarCheck = bo.MinAarCheck;
            entity.MaxAarCheck = bo.MaxAarCheck;
            entity.CorridorLimitCheck = bo.CorridorLimitCheck;
            entity.AblCheck = bo.AblCheck;
            entity.RetentionCheck = bo.RetentionCheck;
            entity.AarCheck = bo.AarCheck;
            entity.MlreStandardPremium = bo.MlreStandardPremium;
            entity.MlreSubstandardPremium = bo.MlreSubstandardPremium;
            entity.MlreFlatExtraPremium = bo.MlreFlatExtraPremium;
            entity.MlreGrossPremium = bo.MlreGrossPremium;
            entity.MlreStandardDiscount = bo.MlreStandardDiscount;
            entity.MlreSubstandardDiscount = bo.MlreSubstandardDiscount;
            entity.MlreLargeSaDiscount = bo.MlreLargeSaDiscount;
            entity.MlreGroupSizeDiscount = bo.MlreGroupSizeDiscount;
            entity.MlreVitalityDiscount = bo.MlreVitalityDiscount;
            entity.MlreTotalDiscount = bo.MlreTotalDiscount;
            entity.MlreNetPremium = bo.MlreNetPremium;
            entity.NetPremiumCheck = bo.NetPremiumCheck;
            entity.ServiceFeePercentage = bo.ServiceFeePercentage;
            entity.ServiceFee = bo.ServiceFee;
            entity.MlreBrokerageFee = bo.MlreBrokerageFee;
            entity.MlreDatabaseCommission = bo.MlreDatabaseCommission;
            entity.ValidityDayCheck = bo.ValidityDayCheck;
            entity.SumAssuredOfferedCheck = bo.SumAssuredOfferedCheck;
            entity.UwRatingCheck = bo.UwRatingCheck;
            entity.FlatExtraAmountCheck = bo.FlatExtraAmountCheck;
            entity.FlatExtraDurationCheck = bo.FlatExtraDurationCheck;
            entity.AarShare2 = bo.AarShare2;
            entity.AarCap2 = bo.AarCap2;
            entity.WakalahFeePercentage = bo.WakalahFeePercentage;
            entity.TreatyNumber = bo.TreatyNumber;
            entity.ConflictType = bo.ConflictType;

            // Direct Retro
            entity.RetroParty1 = bo.RetroParty1;
            entity.RetroParty2 = bo.RetroParty2;
            entity.RetroParty3 = bo.RetroParty3;
            entity.RetroShare1 = bo.RetroShare1;
            entity.RetroShare2 = bo.RetroShare2;
            entity.RetroShare3 = bo.RetroShare3;
            entity.RetroPremiumSpread1 = bo.RetroPremiumSpread1;
            entity.RetroPremiumSpread2 = bo.RetroPremiumSpread2;
            entity.RetroPremiumSpread3 = bo.RetroPremiumSpread3;
            entity.RetroAar1 = bo.RetroAar1;
            entity.RetroAar2 = bo.RetroAar2;
            entity.RetroAar3 = bo.RetroAar3;
            entity.RetroReinsurancePremium1 = bo.RetroReinsurancePremium1;
            entity.RetroReinsurancePremium2 = bo.RetroReinsurancePremium2;
            entity.RetroReinsurancePremium3 = bo.RetroReinsurancePremium3;
            entity.RetroDiscount1 = bo.RetroDiscount1;
            entity.RetroDiscount2 = bo.RetroDiscount2;
            entity.RetroDiscount3 = bo.RetroDiscount3;
            entity.RetroNetPremium1 = bo.RetroNetPremium1;
            entity.RetroNetPremium2 = bo.RetroNetPremium2;
            entity.RetroNetPremium3 = bo.RetroNetPremium3;
            entity.RetroNoClaimBonus1 = bo.RetroNoClaimBonus1;
            entity.RetroNoClaimBonus2 = bo.RetroNoClaimBonus2;
            entity.RetroNoClaimBonus3 = bo.RetroNoClaimBonus3;
            entity.RetroDatabaseCommission1 = bo.RetroDatabaseCommission1;
            entity.RetroDatabaseCommission2 = bo.RetroDatabaseCommission2;
            entity.RetroDatabaseCommission3 = bo.RetroDatabaseCommission3;

            entity.LastUpdatedDate = bo.LastUpdatedDate;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
            result.DataTrail = entity.Update();

            return result;
        }

        public static int? LookUpRiDataWarehouseIdForPostValidation(RiDataBo bo)
        {
            return RiDataWarehouse.LookUpRiDataWarehouseIdForPostValidation(bo);
        }

        public static Result Update(ref RiDataWarehouseBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void UpdateEndingPolicyStatusByPolicyNumber(string policyNumber, string cedingPlanCode, string mlreBenefitCode, int exceptId, int policyStatus, int authUserId)
        {
            using (var db = new AppDbContext(false))
            {
                var list = db.RiDataWarehouse.Where(q => q.PolicyNumber == policyNumber && q.CedingPlanCode == cedingPlanCode && q.MlreBenefitCode == mlreBenefitCode && q.Id != exceptId).ToList();
                foreach (var entity in list)
                {
                    entity.EndingPolicyStatus = policyStatus;
                    entity.UpdatedById = authUserId;
                    entity.LastUpdatedDate = DateTime.Today;
                    entity.UpdatedAt = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public static void Delete(RiDataWarehouseBo bo)
        {
            RiDataWarehouse.Delete(bo.Id);
        }

        public static Result Delete(RiDataWarehouseBo bo, ref TrailObject trail)
        {
            Result result = Result();


            DataTrail dataTrail = RiDataWarehouse.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
