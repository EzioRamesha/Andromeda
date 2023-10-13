using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.RiDatas
{
    public class RiDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiData)),
                Controller = ModuleBo.ModuleController.RiData.ToString(),
            };
        }

        public static Expression<Func<RiData, RiDataBo>> Expression()
        {
            return entity => new RiDataBo
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                Quarter = entity.RiDataBatch.Quarter,
                RiDataBatchStatus = entity.RiDataBatch.Status,
                SoaDataBatchId = entity.RiDataBatch.SoaDataBatchId,
                RiDataFileId = entity.RiDataFileId,
                RecordType = entity.RecordType,
                OriginalEntryId = entity.OriginalEntryId,
                IgnoreFinalise = entity.IgnoreFinalise,
                MappingStatus = entity.MappingStatus,
                PreComputation1Status = entity.PreComputation1Status,
                PreComputation2Status = entity.PreComputation2Status,
                PreValidationStatus = entity.PreValidationStatus,
                PostComputationStatus = entity.PostComputationStatus,
                PostValidationStatus = entity.PostValidationStatus,
                FinaliseStatus = entity.FinaliseStatus,
                ProcessWarehouseStatus = entity.ProcessWarehouseStatus,
                CustomField = entity.CustomField,
                Errors = entity.Errors,
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
                TempD1 = entity.TempD1,
                TempD2 = entity.TempD2,
                TempD3 = entity.TempD3,
                TempD4 = entity.TempD4,
                TempD5 = entity.TempD5,
                TempS1 = entity.TempS1,
                TempS2 = entity.TempS2,
                TempS3 = entity.TempS3,
                TempS4 = entity.TempS4,
                TempS5 = entity.TempS5,
                TempI1 = entity.TempI1,
                TempI2 = entity.TempI2,
                TempI3 = entity.TempI3,
                TempI4 = entity.TempI4,
                TempI5 = entity.TempI5,
                TempA1 = entity.TempA1,
                TempA2 = entity.TempA2,
                TempA3 = entity.TempA3,
                TempA4 = entity.TempA4,
                TempA5 = entity.TempA5,
                TempA6 = entity.TempA6,
                TempA7 = entity.TempA7,
                TempA8 = entity.TempA8,

                // Phase 2
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

        public static RiDataBo FormBo(RiData entity = null, bool foreign = true, bool formatOutput = false)
        {
            if (entity == null)
                return null;
            var bo = new RiDataBo
            {
                Id = entity.Id,
                RiDataBatchId = entity.RiDataBatchId,
                RiDataFileId = entity.RiDataFileId,
                RecordType = entity.RecordType,
                OriginalEntryId = entity.OriginalEntryId,
                IgnoreFinalise = entity.IgnoreFinalise,
                MappingStatus = entity.MappingStatus,
                PreComputation1Status = entity.PreComputation1Status,
                PreComputation2Status = entity.PreComputation2Status,
                PreValidationStatus = entity.PreValidationStatus,
                PostComputationStatus = entity.PostComputationStatus,
                PostValidationStatus = entity.PostValidationStatus,
                FinaliseStatus = entity.FinaliseStatus,
                ProcessWarehouseStatus = entity.ProcessWarehouseStatus,
                CustomField = entity.CustomField,
                Errors = entity.Errors,
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
                TempD1 = entity.TempD1,
                TempD2 = entity.TempD2,
                TempD3 = entity.TempD3,
                TempD4 = entity.TempD4,
                TempD5 = entity.TempD5,
                TempS1 = entity.TempS1,
                TempS2 = entity.TempS2,
                TempS3 = entity.TempS3,
                TempS4 = entity.TempS4,
                TempS5 = entity.TempS5,
                TempI1 = entity.TempI1,
                TempI2 = entity.TempI2,
                TempI3 = entity.TempI3,
                TempI4 = entity.TempI4,
                TempI5 = entity.TempI5,
                TempA1 = entity.TempA1,
                TempA2 = entity.TempA2,
                TempA3 = entity.TempA3,
                TempA4 = entity.TempA4,
                TempA5 = entity.TempA5,
                TempA6 = entity.TempA6,
                TempA7 = entity.TempA7,
                TempA8 = entity.TempA8,

                // Phase 2
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

            if (foreign)
            {
                bo.RiDataBatchBo = RiDataBatchService.Find(entity.RiDataBatchId);
                bo.RiDataFileBo = RiDataFileService.Find(entity.RiDataFileId);
            }

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
                bo.TempD1Str = entity.TempD1?.ToString(Util.GetDateFormat());
                bo.TempD2Str = entity.TempD2?.ToString(Util.GetDateFormat());
                bo.TempD3Str = entity.TempD3?.ToString(Util.GetDateFormat());
                bo.TempD4Str = entity.TempD4?.ToString(Util.GetDateFormat());
                bo.TempD5Str = entity.TempD5?.ToString(Util.GetDateFormat());

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
                bo.TempA1Str = Util.DoubleToString(entity.TempA1);
                bo.TempA2Str = Util.DoubleToString(entity.TempA2);
                bo.TempA3Str = Util.DoubleToString(entity.TempA3);
                bo.TempA4Str = Util.DoubleToString(entity.TempA4);
                bo.TempA5Str = Util.DoubleToString(entity.TempA5);
                bo.TempA6Str = Util.DoubleToString(entity.TempA6);
                bo.TempA7Str = Util.DoubleToString(entity.TempA7);
                bo.TempA8Str = Util.DoubleToString(entity.TempA8);

                // Phase 2
                bo.EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat());
                bo.OfferLetterSentDateStr = entity.OfferLetterSentDate?.ToString(Util.GetDateFormat());
                bo.RiskPeriodStartDateStr = entity.RiskPeriodStartDate?.ToString(Util.GetDateFormat());
                bo.RiskPeriodEndDateStr = entity.RiskPeriodEndDate?.ToString(Util.GetDateFormat());

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
                bo.MaxApLoadingStr = Util.DoubleToString(entity.MaxApLoading);
                bo.MlreStandardPremiumStr = Util.DoubleToString(entity.MlreStandardPremium);
                bo.MlreSubstandardPremiumStr = Util.DoubleToString(entity.MlreSubstandardPremium);
                bo.MlreFlatExtraPremiumStr = Util.DoubleToString(entity.MlreFlatExtraPremium);
                bo.MlreGrossPremiumStr = Util.DoubleToString(entity.MlreGrossPremium);
                bo.MlreStandardDiscountStr = Util.DoubleToString(entity.MlreStandardDiscount);
                bo.MlreSubstandardDiscountStr = Util.DoubleToString(entity.MlreSubstandardDiscount);
                bo.MlreLargeSaDiscountStr = Util.DoubleToString(entity.MlreLargeSaDiscount);
                bo.MlreGroupSizeDiscountStr = Util.DoubleToString(entity.MlreGroupSizeDiscount);
                bo.MlreVitalityDiscountStr = Util.DoubleToString(entity.MlreVitalityDiscount);
                bo.MlreTotalDiscountStr = Util.DoubleToString(entity.MlreTotalDiscount);
                bo.MlreNetPremiumStr = Util.DoubleToString(entity.MlreNetPremium);
                bo.NetPremiumCheckStr = Util.DoubleToString(entity.NetPremiumCheck);
                bo.ServiceFeePercentageStr = Util.DoubleToString(entity.ServiceFeePercentage);
                bo.ServiceFeeStr = Util.DoubleToString(entity.ServiceFee);
                bo.MlreBrokerageFeeStr = Util.DoubleToString(entity.MlreBrokerageFee);
                bo.MlreDatabaseCommissionStr = Util.DoubleToString(entity.MlreDatabaseCommission);
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

                // Type & Status Name
                bo.RecordTypeStr = RiDataBo.GetRecordTypeName(entity.RecordType);
            }

            return bo;
        }

        public static RiDataBo FormSimplifiedBo(RiData entity = null)
        {
            if (entity == null)
                return null;
            return new RiDataBo
            {
                Id = entity.Id,
                //RiDataBatchId = entity.RiDataBatchId,
                //RiDataFileId = entity.RiDataFileId,
                RecordType = entity.RecordType,
                //OriginalEntryId = entity.OriginalEntryId,
                //IgnoreFinalise = entity.IgnoreFinalise,
                //MappingStatus = entity.MappingStatus,
                //PreComputation1Status = entity.PreComputation1Status,
                //PreComputation2Status = entity.PreComputation2Status,
                //PreValidationStatus = entity.PreValidationStatus,
                //PostComputationStatus = entity.PostComputationStatus,
                //PostValidationStatus = entity.PostValidationStatus,
                //FinaliseStatus = entity.FinaliseStatus,
                //CustomField = entity.CustomField,
                //Errors = entity.Errors,
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
                //IssueDatePol = entity.IssueDatePol,
                //IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                //ReinsEffDateBen = entity.ReinsEffDateBen,
                CedingPlanCode = entity.CedingPlanCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                OriSumAssured = entity.OriSumAssured,
                CurrSumAssured = entity.CurrSumAssured,
                AmountCededB4MlreShare = entity.AmountCededB4MlreShare,
                //RetentionAmount = entity.RetentionAmount,
                //AarOri = entity.AarOri,
                Aar = entity.Aar,
                AarSpecial1 = entity.AarSpecial1,
                AarSpecial2 = entity.AarSpecial2,
                AarSpecial3 = entity.AarSpecial3,
                //InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                InsuredTobaccoUse = entity.InsuredTobaccoUse,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredOccupationCode = entity.InsuredOccupationCode,
                //InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                //InsuredNewIcNumber = entity.InsuredNewIcNumber,
                //InsuredOldIcNumber = entity.InsuredOldIcNumber,
                //InsuredName2nd = entity.InsuredName2nd,
                InsuredGenderCode2nd = entity.InsuredGenderCode2nd,
                InsuredTobaccoUse2nd = entity.InsuredTobaccoUse2nd,
                InsuredDateOfBirth2nd = entity.InsuredDateOfBirth2nd,
                InsuredAttainedAge2nd = entity.InsuredAttainedAge2nd,
                //InsuredNewIcNumber2nd = entity.InsuredNewIcNumber2nd,
                //InsuredOldIcNumber2nd = entity.InsuredOldIcNumber2nd,
                ReinsuranceIssueAge = entity.ReinsuranceIssueAge,
                ReinsuranceIssueAge2nd = entity.ReinsuranceIssueAge2nd,
                PolicyTerm = entity.PolicyTerm,
                PolicyExpiryDate = entity.PolicyExpiryDate,
                DurationYear = entity.DurationYear,
                DurationDay = entity.DurationDay,
                DurationMonth = entity.DurationMonth,
                PremiumCalType = entity.PremiumCalType,
                //CedantRiRate = entity.CedantRiRate,
                RateTable = entity.RateTable,
                AgeRatedUp = entity.AgeRatedUp,
                //DiscountRate = entity.DiscountRate,
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
                //AnnualRiPrem = entity.AnnualRiPrem,
                RiCovPeriod = entity.RiCovPeriod,
                AdjBeginDate = entity.AdjBeginDate,
                AdjEndDate = entity.AdjEndDate,
                //PolicyNumberOld = entity.PolicyNumberOld,
                PolicyStatusCode = entity.PolicyStatusCode,
                //PolicyGrossPremium = entity.PolicyGrossPremium,
                PolicyStandardPremium = entity.PolicyStandardPremium,
                PolicySubstandardPremium = entity.PolicySubstandardPremium,
                //PolicyTermRemain = entity.PolicyTermRemain,
                //PolicyAmountDeath = entity.PolicyAmountDeath,
                //PolicyReserve = entity.PolicyReserve,
                //PolicyPaymentMethod = entity.PolicyPaymentMethod,
                //PolicyLifeNumber = entity.PolicyLifeNumber,
                //FundCode = entity.FundCode,
                LineOfBusiness = entity.LineOfBusiness,
                //ApLoading = entity.ApLoading,
                LoanInterestRate = entity.LoanInterestRate,
                DefermentPeriod = entity.DefermentPeriod,
                //RiderNumber = entity.RiderNumber,
                CampaignCode = entity.CampaignCode,
                //Nationality = entity.Nationality,
                TerritoryOfIssueCode = entity.TerritoryOfIssueCode,
                //CurrencyCode = entity.CurrencyCode,
                //StaffPlanIndicator = entity.StaffPlanIndicator,
                CedingTreatyCode = entity.CedingTreatyCode,
                CedingPlanCodeOld = entity.CedingPlanCodeOld,
                //CedingBasicPlanCode = entity.CedingBasicPlanCode,
                //CedantSar = entity.CedantSar,
                CedantReinsurerCode = entity.CedantReinsurerCode,
                AmountCededB4MlreShare2 = entity.AmountCededB4MlreShare2,
                //CessionCode = entity.CessionCode,
                //CedantRemark = entity.CedantRemark,
                GroupPolicyNumber = entity.GroupPolicyNumber,
                GroupPolicyName = entity.GroupPolicyName,
                NoOfEmployee = entity.NoOfEmployee,
                PolicyTotalLive = entity.PolicyTotalLive,
                GroupSubsidiaryName = entity.GroupSubsidiaryName,
                GroupSubsidiaryNo = entity.GroupSubsidiaryNo,
                //GroupEmployeeBasicSalary = entity.GroupEmployeeBasicSalary,
                //GroupEmployeeJobType = entity.GroupEmployeeJobType,
                //GroupEmployeeJobCode = entity.GroupEmployeeJobCode,
                //GroupEmployeeBasicSalaryRevise = entity.GroupEmployeeBasicSalaryRevise,
                //GroupEmployeeBasicSalaryMultiplier = entity.GroupEmployeeBasicSalaryMultiplier,
                CedingPlanCode2 = entity.CedingPlanCode2,
                DependantIndicator = entity.DependantIndicator,
                GhsRoomBoard = entity.GhsRoomBoard,
                //PolicyAmountSubstandard = entity.PolicyAmountSubstandard,
                //Layer1RiShare = entity.Layer1RiShare,
                //Layer1InsuredAttainedAge = entity.Layer1InsuredAttainedAge,
                //Layer1InsuredAttainedAge2nd = entity.Layer1InsuredAttainedAge2nd,
                //Layer1StandardPremium = entity.Layer1StandardPremium,
                //Layer1SubstandardPremium = entity.Layer1SubstandardPremium,
                //Layer1GrossPremium = entity.Layer1GrossPremium,
                //Layer1StandardDiscount = entity.Layer1StandardDiscount,
                //Layer1SubstandardDiscount = entity.Layer1SubstandardDiscount,
                //Layer1TotalDiscount = entity.Layer1TotalDiscount,
                //Layer1NetPremium = entity.Layer1NetPremium,
                //Layer1GrossPremiumAlt = entity.Layer1GrossPremiumAlt,
                //Layer1TotalDiscountAlt = entity.Layer1TotalDiscountAlt,
                //Layer1NetPremiumAlt = entity.Layer1NetPremiumAlt,
                SpecialIndicator1 = entity.SpecialIndicator1,
                SpecialIndicator2 = entity.SpecialIndicator2,
                SpecialIndicator3 = entity.SpecialIndicator3,
                IndicatorJointLife = entity.IndicatorJointLife,
                //TaxAmount = entity.TaxAmount,
                GstIndicator = entity.GstIndicator,
                //GstGrossPremium = entity.GstGrossPremium,
                //GstTotalDiscount = entity.GstTotalDiscount,
                //GstVitality = entity.GstVitality,
                //GstAmount = entity.GstAmount,
                Mfrs17BasicRider = entity.Mfrs17BasicRider,
                Mfrs17CellName = entity.Mfrs17CellName,
                Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                //LoaCode = entity.LoaCode,
                //TempD1 = entity.TempD1,
                //TempD2 = entity.TempD2,
                //TempD3 = entity.TempD3,
                //TempD4 = entity.TempD4,
                //TempD5 = entity.TempD5,
                //TempS1 = entity.TempS1,
                //TempS2 = entity.TempS2,
                //TempS3 = entity.TempS3,
                //TempS4 = entity.TempS4,
                //TempS5 = entity.TempS5,
                //TempI1 = entity.TempI1,
                //TempI2 = entity.TempI2,
                //TempI3 = entity.TempI3,
                //TempI4 = entity.TempI4,
                //TempI5 = entity.TempI5,
                //TempA1 = entity.TempA1,
                //TempA2 = entity.TempA2,
                //TempA3 = entity.TempA3,
                //TempA4 = entity.TempA4,
                //TempA5 = entity.TempA5,
                //TempA6 = entity.TempA6,
                //TempA7 = entity.TempA7,
                //TempA8 = entity.TempA8,

                // Phase 2
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

                //CreatedById = entity.CreatedById,
                //UpdatedById = entity.UpdatedById,
            };
        }

        public static RiData FormEntity(RiDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiData
            {
                Id = bo.Id,
                RiDataBatchId = bo.RiDataBatchId,
                RiDataFileId = bo.RiDataFileId,
                RecordType = bo.RecordType,
                OriginalEntryId = bo.OriginalEntryId,
                MappingStatus = bo.MappingStatus,
                PreComputation1Status = bo.PreComputation1Status,
                PreComputation2Status = bo.PreComputation2Status,
                PreValidationStatus = bo.PreValidationStatus,
                PostComputationStatus = bo.PostComputationStatus,
                PostValidationStatus = bo.PostValidationStatus,
                FinaliseStatus = bo.FinaliseStatus,
                ProcessWarehouseStatus = bo.ProcessWarehouseStatus,
                CustomField = bo.CustomField,
                Errors = bo.Errors,
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
                TempD1 = bo.TempD1,
                TempD2 = bo.TempD2,
                TempD3 = bo.TempD3,
                TempD4 = bo.TempD4,
                TempD5 = bo.TempD5,
                TempS1 = bo.TempS1,
                TempS2 = bo.TempS2,
                TempS3 = bo.TempS3,
                TempS4 = bo.TempS4,
                TempS5 = bo.TempS5,
                TempI1 = bo.TempI1,
                TempI2 = bo.TempI2,
                TempI3 = bo.TempI3,
                TempI4 = bo.TempI4,
                TempI5 = bo.TempI5,
                TempA1 = bo.TempA1,
                TempA2 = bo.TempA2,
                TempA3 = bo.TempA3,
                TempA4 = bo.TempA4,
                TempA5 = bo.TempA5,
                TempA6 = bo.TempA6,
                TempA7 = bo.TempA7,
                TempA8 = bo.TempA8,

                // Phase 2
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

        public static IList<RiDataBo> FormBos(IList<RiData> entities = null, bool foreign = true, bool formatOutput = false)
        {
            if (entities == null)
                return null;
            IList<RiDataBo> bos = new List<RiDataBo>() { };
            foreach (RiData entity in entities)
            {
                bos.Add(FormBo(entity, foreign, formatOutput));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return RiData.IsExists(id);
        }

        public static bool IsDeleted(int id)
        {
            using (var db = new AppDbContext(false))
            {
                var entity = db.RiData.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                    return true;

                if (entity.RiDataBatch.Status == RiDataBatchBo.StatusPendingDelete)
                    return true;

                return false;
            }
        }

        public static RiDataBo Find(int id)
        {
            return FormBo(RiData.Find(id));
        }

        public static RiDataBo FindWithFormattedOutput(int id)
        {
            return FormBo(RiData.Find(id), true, true);
        }

        public static RiDataBo FindSimplified(int id)
        {
            return FormBo(RiData.FindSimplified(id), false);
        }

        public static RiDataBo FindSimplifiedWithFormattedOutput(int? id)
        {
            if (id.HasValue)
                return FormBo(RiData.FindSimplified(id.Value), false, true);
            return null;
        }

        public static int CountByRiDataBatchId(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchId(riDataBatchId);
        }

        public static int CountByRiDataFileId(int riDataFileId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .Count();
            }
        }

        public static int CountByRiDataFileIdMappingStatus(int riDataFileId, int mappingStatus)
        {
            return RiData.CountByRiDataFileIdMappingStatus(riDataFileId, mappingStatus);
        }

        public static int CountByRiDataFileIdPreComputation1Status(int riDataFileId, int computationStatus)
        {
            return RiData.CountByRiDataFileIdPreComputation1Status(riDataFileId, computationStatus);
        }

        public static int CountByRiDataFileIdPreValidationStatus(int riDataFileId, int preValidationStatus)
        {
            return RiData.CountByRiDataFileIdPreValidationStatus(riDataFileId, preValidationStatus);
        }

        public static int CountByRiDataFileIdFinaliseStatus(int riDataFileId, int finaliseStatus)
        {
            return RiData.CountByRiDataFileIdFinaliseStatus(riDataFileId, finaliseStatus);
        }

        public static int CountRecordForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, int month, int year)
        {
            return RiData.CountRecordForMfrs17Reporting(treatyCode, premiumFrequencyCode, month, year);
        }

        public static int CountByRiDataBatchIdMappingStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdMappingStatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdMappingStatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdMappingStatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdPreComputation1StatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdPreComputation1StatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdPreComputation1StatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdPreComputation1StatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdPreComputation2StatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdPreComputation2StatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdPreComputation2StatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdPreComputation2StatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdPreValidationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdPreValidationStatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdPreValidationStatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdPreValidationStatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdPostComputationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdPostComputationStatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdPostComputationStatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdPostComputationStatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdPostValidationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdPostValidationStatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdPostValidationStatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdPostValidationStatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdFinaliseStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdFinaliseStatusFailed(riDataBatchId, db);
        }
        public static int CountByRiDataBatchIdFinaliseStatusFailed(int riDataBatchId)
        {
            return RiData.CountByRiDataBatchIdFinaliseStatusFailed(riDataBatchId);
        }

        public static int CountByRiDataBatchIdProcessWarehouseStatusFailed(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdProcessWarehouseStatusFailed(riDataBatchId, db);
        }

        public static int CountByRiDataBatchIdIsConflict(int riDataBatchId, AppDbContext db)
        {
            return RiData.CountByRiDataBatchIdIsConflict(riDataBatchId, db);
        }

        public static int CountByTreatyCodeYearMonth(string treatyCode, int year, int month)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear == year)
                    .Where(q => q.RiskPeriodMonth <= month)
                    .Count();
            }
        }

        public static int? GetMaxMonthByMonthYear(string treatyCode, int? month, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear == year);

                if (month != null)
                    query = query.Where(q => q.RiskPeriodMonth <= month);

                return query.OrderByDescending(q => q.RiskPeriodMonth)
                    .Select(q => q.RiskPeriodMonth)
                    .FirstOrDefault();
            }
        }

        public static int? GetMaxMonthByYear(string treatyCode, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear == year)
                    .OrderByDescending(q => q.RiskPeriodMonth)
                    .Select(q => q.RiskPeriodMonth)
                    .FirstOrDefault();
            }
        }

        public static int CountByRiDataBatchIdProcessWarehouseStatesFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.ProcessWarehouseStatus == RiDataBo.ProcessWarehouseStatusFailed)
                    .Count();
            }
        }

        public static int CountNoOfPolicyByRetroParty(DirectRetroBo directRetroBo, string transactionTypeCode, string retroPartyCode, int monthStart, int monthEnd, int year, string Mfrs17TreatyCode = "", int? Mfrs17AnnualCohort = null)
        {
            int policy = 0;
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == directRetroBo.SoaDataBatchId)
                    .Where(q => q.TreatyCode == directRetroBo.TreatyCodeBo.Code)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Where(q => q.RiskPeriodYear == year)
                    .Where(q => q.RiskPeriodMonth >= monthStart)
                    .Where(q => q.RiskPeriodMonth <= monthEnd);

                if (!string.IsNullOrEmpty(Mfrs17TreatyCode)) query = query.Where(q => q.Mfrs17TreatyCode == Mfrs17TreatyCode);
                if (Mfrs17AnnualCohort.HasValue) query = query.Where(q => q.Mfrs17AnnualCohort == Mfrs17AnnualCohort);

                policy += query.Where(q => q.RetroParty1 == retroPartyCode).Count();
                policy += query.Where(q => q.RetroParty2 == retroPartyCode).Count();
                policy += query.Where(q => q.RetroParty3 == retroPartyCode).Count();

                return policy;
            }
        }

        public static double CountSumRiAmountByRetroParty(DirectRetroBo directRetroBo, string transactionTypeCode, string retroPartyCode, int monthStart, int monthEnd, int year, string Mfrs17TreatyCode = "", int? Mfrs17AnnualCohort = null)
        {
            double retroAar = 0;
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == directRetroBo.SoaDataBatchId)
                    .Where(q => q.TreatyCode == directRetroBo.TreatyCodeBo.Code)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Where(q => q.RiskPeriodYear == year)
                    .Where(q => q.RiskPeriodMonth >= monthStart)
                    .Where(q => q.RiskPeriodMonth <= monthEnd);

                if (!string.IsNullOrEmpty(Mfrs17TreatyCode)) query = query.Where(q => q.Mfrs17TreatyCode == Mfrs17TreatyCode);
                if (Mfrs17AnnualCohort.HasValue) query = query.Where(q => q.Mfrs17AnnualCohort == Mfrs17AnnualCohort);

                retroAar += query.Where(q => q.RetroParty1 == retroPartyCode).Sum(q => q.RetroAar1) ?? 0;
                retroAar += query.Where(q => q.RetroParty2 == retroPartyCode).Sum(q => q.RetroAar2) ?? 0;
                retroAar += query.Where(q => q.RetroParty3 == retroPartyCode).Sum(q => q.RetroAar3) ?? 0;

                return retroAar;
            }
        }

        public static double CountTotalNoClaimBonus(DirectRetroBo directRetroBo, string retroPartyCode, int monthStart, int monthEnd, int year, string Mfrs17TreatyCode = "", int? Mfrs17AnnualCohort = null)
        {
            double noClaimBonus = 0;
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == directRetroBo.SoaDataBatchId)
                    .Where(q => q.TreatyCode == directRetroBo.TreatyCodeBo.Code)
                    .Where(q => q.RiskPeriodYear == year)
                    .Where(q => q.RiskPeriodMonth >= monthStart)
                    .Where(q => q.RiskPeriodMonth <= monthEnd);

                if (!string.IsNullOrEmpty(Mfrs17TreatyCode)) query = query.Where(q => q.Mfrs17TreatyCode == Mfrs17TreatyCode);
                if (Mfrs17AnnualCohort.HasValue) query = query.Where(q => q.Mfrs17AnnualCohort == Mfrs17AnnualCohort);

                noClaimBonus += query.Where(q => q.RetroParty1 == retroPartyCode).Sum(q => q.RetroNoClaimBonus1) ?? 0;
                noClaimBonus += query.Where(q => q.RetroParty2 == retroPartyCode).Sum(q => q.RetroNoClaimBonus2) ?? 0;
                noClaimBonus += query.Where(q => q.RetroParty3 == retroPartyCode).Sum(q => q.RetroNoClaimBonus3) ?? 0;

                return noClaimBonus;
            }
        }

        public static double CountTotalDatabaseCommission(DirectRetroBo directRetroBo, string retroPartyCode, int monthStart, int monthEnd, int year, string Mfrs17TreatyCode = "", int? Mfrs17AnnualCohort = null)
        {
            double databaseCommission = 0;
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == directRetroBo.SoaDataBatchId)
                    .Where(q => q.TreatyCode == directRetroBo.TreatyCodeBo.Code)
                    .Where(q => q.RiskPeriodYear == year)
                    .Where(q => q.RiskPeriodMonth >= monthStart)
                    .Where(q => q.RiskPeriodMonth <= monthEnd);

                if (!string.IsNullOrEmpty(Mfrs17TreatyCode)) query = query.Where(q => q.Mfrs17TreatyCode == Mfrs17TreatyCode);
                if (Mfrs17AnnualCohort.HasValue) query = query.Where(q => q.Mfrs17AnnualCohort == Mfrs17AnnualCohort);

                databaseCommission += query.Where(q => q.RetroParty1 == retroPartyCode).Sum(q => q.RetroDatabaseCommission1) ?? 0;
                databaseCommission += query.Where(q => q.RetroParty2 == retroPartyCode).Sum(q => q.RetroDatabaseCommission2) ?? 0;
                databaseCommission += query.Where(q => q.RetroParty3 == retroPartyCode).Sum(q => q.RetroDatabaseCommission3) ?? 0;

                return databaseCommission;
            }
        }

        public static int CountByLookupParams(RiDataBo riData)
        {
            return CountByLookupParams(
                riData.PolicyNumber,
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.TreatyCode,
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
            int? riskPeriodMonth = null,
            int? riskPeriodYear = null,
            int? riderNumber = null
        )
        {
            return RiData.CountByLookupParams(
                policyNumber,
                cedingPlanCode,
                mlreBenefitCode,
                treatyCode,
                riskPeriodMonth,
                riskPeriodYear,
                riderNumber
            );
        }

        public static RiDataBo FindByLookupParams(RiDataBo riData)
        {
            return FindByLookupParams(
                riData.PolicyNumber,
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.TreatyCode,
                riData.RiskPeriodMonth,
                riData.RiskPeriodYear,
                riData.RiderNumber
            );
        }

        public static RiDataBo FindByLookupParams(
            string policyNumber,
            string cedingPlanCode,
            string mlreBenefitCode,
            string treatyCode,
            int? riskPeriodMonth = null,
            int? riskPeriodYear = null,
            int? riderNumber = null
        )
        {
            return FormBo(RiData.FindByLookupParams(
                policyNumber,
                cedingPlanCode,
                mlreBenefitCode,
                treatyCode,
                riskPeriodMonth,
                riskPeriodYear,
                riderNumber
            ), false);
        }

        public static IEnumerable<PickListDetailBo> GetDropDowns()
        {
            var pickListDetailBos = new List<PickListDetailBo> { };
            var dropDowns = StandardOutputBo.GetDropDownTypes();
            foreach (int type in dropDowns)
            {
                var bos = PickListDetailService.GetByStandardOutputId(type);
                pickListDetailBos.AddRange(bos);
            }
            return pickListDetailBos;
        }

        public static IList<RiDataBo> GetByIds(List<int> ids, bool foreign = false)
        {
            return FormBos(RiData.GetByIds(ids), foreign);
        }

        public static int? GetMaxYearByTreatyCodeYear(string treatyCode, int year)
        {
            return RiData.GetMaxYearByTreatyCodeYear(treatyCode, year);
        }

        public static int? GetMaxMonthByTreatyCodeYear(string treatyCode, int? year)
        {
            return RiData.GetMaxMonthByTreatyCodeYear(treatyCode, year);
        }

        public static int? GetMaxMonthByTreatyCodeMonthYear(string treatyCode, int? month, int? year)
        {
            return RiData.GetMaxMonthByTreatyCodeMonthYear(treatyCode, month, year);
        }

        public static IList<RiDataBo> GetByRiDataBatchId(int riDataBatchId, int skip, int take, bool foreign = false)
        {
            return FormBos(RiData.GetByRiDataBatchId(riDataBatchId, skip, take), foreign);
        }

        public static IList<RiDataBo> GetByRiDataFileId(int riDataFileId, int skip = 0, int take = 50, bool foreign = false)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList(), foreign);
            }
        }

        public static IList<RiDataBo> GetByRiDataBatchIdRiDataFileId(int riDataBatchId, int riDataFileId)
        {
            return FormBos(RiData.GetByRiDataBatchIdRiDataFileId(riDataBatchId, riDataFileId));
        }

        public static IList<RiDataBo> GetByRiDataBatchIdProcessWarehouseStatus(int riDataBatchId, int status, int skip, int take, bool foreign = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.ProcessWarehouseStatus == status)
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take);

                return FormBos(query.ToList(), foreign);
            }
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            return RiData.GetDistinctMfrs17TreatyCodes(treatyCode, premiumFrequencyCode, year, monthStart, monthEnd);
        }

        public static List<int> GetIdsForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, string mfrs17TreatyCode, int year, int monthStart, int? monthEnd = null)
        {
            return RiData.GetIdsForMfrs17Reporting(treatyCode, premiumFrequencyCode, mfrs17TreatyCode, year, monthStart, monthEnd);
        }

        public static List<int> GetIdsOfIgnoreFinalise(int riDataBatchId)
        {
            return RiData.GetIdsOfIgnoreFinalise(riDataBatchId);
        }

        public static int CountForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            return RiData.CountForMfrs17Reporting(treatyCode, premiumFrequencyCode, year, monthStart, monthEnd);
        }

        public static IList<RiDataBo> GetByOriginalMatchCombination(int id, string policyNo, string planCode, string quarter, string riskQuarter)
        {
            return FormBos(RiData.GetByOriginalMatchCombination(id, policyNo, planCode, quarter, riskQuarter));
        }

        public static IList<RiDataBo> GetByClaimRegisterParam(string policyNumber, string cedingPlanCode, int riskYear, int riskMonth, string soaQuarter, string cedingBenefitTypeCode, string cedingBenefitRiskCode)
        {
            return FormBos(RiData.GetByClaimRegisterParam(policyNumber, cedingPlanCode, riskYear, riskMonth, soaQuarter, cedingBenefitTypeCode, cedingBenefitRiskCode), false, true);
        }

        public static RiDataBo FindByGroupParam(ClaimRegisterBo claimRegisterBo, int step = 1)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiData.Where(q => q.TreatyCode == claimRegisterBo.TreatyCode)
                    .Where(q => q.CedingPlanCode == claimRegisterBo.CedingPlanCode);

                if (step == 2)
                {
                    query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.GroupPolicyNumber == claimRegisterBo.PolicyNumber) || q.InsuredName == claimRegisterBo.InsuredName);
                }
                else
                {
                    query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.GroupPolicyNumber == claimRegisterBo.PolicyNumber) && q.InsuredName == claimRegisterBo.InsuredName);
                }

                return FormBo(query.FirstOrDefault(), false);
            }
        }

        public static RiDataBo FindByIndividualParam(ClaimRegisterBo claimRegisterBo, int step = 1)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiData.Where(q => q.TreatyCode == claimRegisterBo.TreatyCode)
                    .Where(q => q.CedingPlanCode == claimRegisterBo.CedingPlanCode);

                if (step == 2)
                {
                    query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumberOld) && q.PolicyNumberOld == claimRegisterBo.PolicyNumber) || q.PolicyNumber == claimRegisterBo.PolicyNumber) || q.InsuredName == claimRegisterBo.InsuredName);
                }
                else
                {
                    query = query.Where(q => ((!string.IsNullOrEmpty(q.PolicyNumberOld) && q.PolicyNumberOld == claimRegisterBo.PolicyNumber) || q.PolicyNumber == claimRegisterBo.PolicyNumber) && q.InsuredName == claimRegisterBo.InsuredName);
                }

                return FormBo(query.FirstOrDefault(), false);
            }
        }

        public static double SumDiscountForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            return RiData.SumDiscountForInvoice(riDataBatchId, transactionTypeCode);
        }

        public static double SumGrossForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            return RiData.SumGrossForInvoice(riDataBatchId, transactionTypeCode);
        }

        public static double SumReinsForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            return RiData.SumReinsForInvoice(riDataBatchId, transactionTypeCode);
        }

        public static int CountBySoaDataBatchIdByTreatyCode(int soaDataBatchId, string treatyCode, List<string> transactionTypes = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.TreatyCode == treatyCode);
                if (transactionTypes != null)
                {
                    query.Where(q => transactionTypes.Contains(q.TransactionTypeCode));
                }
                return query.Count();
            }
        }

        public static IList<RiDataBo> GetBySoaDataBatchIdByTreatyCode(int soaDataBatchId, string treatyCode, int skip, int take, List<string> transactionTypes = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData
                    .Where(q => q.RiDataBatch.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.TreatyCode == treatyCode);
                if (transactionTypes != null)
                {
                    query.Where(q => transactionTypes.Contains(q.TransactionTypeCode));
                }
                return FormBos(query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList(), false);
            }
        }

        public static List<string> GetDistinctTransactionTypeCodes(int? soaDataBatchId, string treatyCode)
        {
            using (var db = new AppDbContext(false))
            {
                List<string> transactionTypes = new List<string> { PickListDetailBo.TransactionTypeCodeNewBusiness, PickListDetailBo.TransactionTypeCodeRenewal };

                var query = db.RiData.AsNoTracking().Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiDataBatch.SoaDataBatchId == soaDataBatchId)
                    .Where(q => transactionTypes.Contains(q.TransactionTypeCode));

                return query.GroupBy(q => q.TransactionTypeCode).Select(q => q.FirstOrDefault().TransactionTypeCode).ToList();
            }
        }

        public static bool IsDuplicate(RiDataBo riData)
        {
            return RiData.IsDuplicate(riData);
        }

        public static bool IsExistByTreatyCode(int treatyCodeId)
        {
            using (var db = new AppDbContext(false))
            {
                var treatyCode = db.TreatyCodes.Where(q => q.Id == treatyCodeId).Select(q => q.Code).FirstOrDefault();
                if (!string.IsNullOrEmpty(treatyCode))
                {
                    return db.RiData
                        .Where(q => q.TreatyCode.Trim() == treatyCode.Trim())
                        .Any();
                }
                return false;
            }
        }

        public static Result Save(ref RiDataBo bo)
        {
            if (!RiData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RiDataBo bo, ref TrailObject trail)
        {
            if (!RiData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataBo bo)
        {
            RiData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref RiDataBo bo, AppDbContext db)
        {
            RiData entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref RiDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataBo bo)
        {
            Result result = Result();

            RiData entity = RiData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataBatchId = bo.RiDataBatchId;
                entity.RiDataFileId = bo.RiDataFileId;
                entity.RecordType = bo.RecordType;
                entity.OriginalEntryId = bo.OriginalEntryId;
                entity.IgnoreFinalise = bo.IgnoreFinalise;
                entity.MappingStatus = bo.MappingStatus;
                entity.PreComputation1Status = bo.PreComputation1Status;
                entity.PreComputation2Status = bo.PreComputation2Status;
                entity.PreValidationStatus = bo.PreValidationStatus;
                entity.PostComputationStatus = bo.PostComputationStatus;
                entity.PostValidationStatus = bo.PostValidationStatus;
                entity.FinaliseStatus = bo.FinaliseStatus;
                entity.ProcessWarehouseStatus = bo.ProcessWarehouseStatus;
                entity.Errors = bo.Errors;

                entity.CustomField = bo.CustomField;
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
                entity.TempD1 = bo.TempD1;
                entity.TempD2 = bo.TempD2;
                entity.TempD3 = bo.TempD3;
                entity.TempD4 = bo.TempD4;
                entity.TempD5 = bo.TempD5;
                entity.TempS1 = bo.TempS1;
                entity.TempS2 = bo.TempS2;
                entity.TempS3 = bo.TempS3;
                entity.TempS4 = bo.TempS4;
                entity.TempS5 = bo.TempS5;
                entity.TempI1 = bo.TempI1;
                entity.TempI2 = bo.TempI2;
                entity.TempI3 = bo.TempI3;
                entity.TempI4 = bo.TempI4;
                entity.TempI5 = bo.TempI5;
                entity.TempA1 = bo.TempA1;
                entity.TempA2 = bo.TempA2;
                entity.TempA3 = bo.TempA3;
                entity.TempA4 = bo.TempA4;
                entity.TempA5 = bo.TempA5;
                entity.TempA6 = bo.TempA6;
                entity.TempA7 = bo.TempA7;
                entity.TempA8 = bo.TempA8;

                // Phase 2
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

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataBo bo)
        {
            RiData.Delete(bo.Id);
        }

        public static Result Delete(RiDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RiData.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByRiDataBatchId(int riDataBatchId)
        {
            return RiData.DeleteAllByRiDataBatchId(riDataBatchId);
        }

        public static void DeleteByRiDataBatchId(int riDataBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRiDataBatchId(riDataBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiData)));
                }
            }
        }

        public static List<string> ValidateDropDownCodes(string title, List<int> types, RiDataBo riData)
        {
            var errors = new List<string> { };
            foreach (var type in types)
            {
                string property = StandardOutputBo.GetPropertyNameByType(type);
                string code = (string)riData.GetPropertyValue(property);

                if (string.IsNullOrEmpty(code))
                    continue;

                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riData.Id);
                if (PickListDetailService.CountByStandardOutputIdCode(type, code.ToString()) == 0)
                {
                    errors.Add(riData.FormatDropDownError(title, type, string.Format("Record not found in Pick List: {0}", code)));
                }
            }
            return errors;
        }
    }
}
