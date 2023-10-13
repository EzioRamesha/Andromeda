using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Services.Retrocession;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeAggregationMonthlyDataViewModel
    {
        public int Id { get; set; }

        [DisplayName("Per Life Aggregation Detail Data ID")]
        public int PerLifeAggregationDetailDataId { get; set; }

        public PerLifeAggregationDetailData PerLifeAggregationDetailData { get; set; }

        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }

        [DisplayName("Risk Year")]
        public int RiskYear { get; set; }

        [DisplayName("Risk Month")]
        public int RiskMonth { get; set; }

        public string UniqueKeyPerLife { get; set; }

        public string RetroPremFreq { get; set; }

        public double Aar { get; set; }

        public double? SumOfAar { get; set; }

        public double NetPremium { get; set; }

        public double? SumOfNetPremium { get; set; }

        public double? RetentionLimit { get; set; }

        public double? DistributedRetentionLimit { get; set; }

        public double? RetroAmount { get; set; }

        public double? DistributedRetroAmount { get; set; }

        public double? AccumulativeRetainAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public bool RetroIndicator { get; set; }

        public string Errors { get; set; }

        [DisplayName("Retro Benefit Code")]
        public string RetroBenefitCode { get; set; }

        public virtual ICollection<PerLifeAggregationMonthlyRetroData> PerLifeAggregationMonthlyRetroData { get; set; }


        // RI Data Warehouse History
        public int RiDataWarehouseHistoryId { get; set; }

        public string Quarter { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public int RecordType { get; set; }

        public string TreatyCode { get; set; }

        public string ReinsBasisCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string TransactionTypeCode { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        public DateTime? ReinsEffDatePol { get; set; }

        public DateTime? ReinsEffDateBen { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? RetentionAmount { get; set; }

        public double? AarOri { get; set; }

        //public double? Aar { get; set; }

        public double? AarSpecial1 { get; set; }

        public double? AarSpecial2 { get; set; }

        public double? AarSpecial3 { get; set; }

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredOccupationCode { get; set; }

        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        public string InsuredNewIcNumber { get; set; }

        public string InsuredOldIcNumber { get; set; }

        public string InsuredName2nd { get; set; }

        public string InsuredGenderCode2nd { get; set; }

        public string InsuredTobaccoUse2nd { get; set; }

        public DateTime? InsuredDateOfBirth2nd { get; set; }

        public int? InsuredAttainedAge2nd { get; set; }

        public string InsuredNewIcNumber2nd { get; set; }

        public string InsuredOldIcNumber2nd { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public int? ReinsuranceIssueAge2nd { get; set; }

        public double? PolicyTerm { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public double? DurationYear { get; set; }

        public double? DurationDay { get; set; }

        public double? DurationMonth { get; set; }

        public string PremiumCalType { get; set; }

        public double? CedantRiRate { get; set; }

        public string RateTable { get; set; }

        public int? AgeRatedUp { get; set; }

        public double? DiscountRate { get; set; }

        public string LoadingType { get; set; }

        public double? UnderwriterRating { get; set; }

        public double? UnderwriterRatingUnit { get; set; }

        public int? UnderwriterRatingTerm { get; set; }

        public double? UnderwriterRating2 { get; set; }

        public double? UnderwriterRatingUnit2 { get; set; }

        public int? UnderwriterRatingTerm2 { get; set; }

        public double? UnderwriterRating3 { get; set; }

        public double? UnderwriterRatingUnit3 { get; set; }

        public int? UnderwriterRatingTerm3 { get; set; }

        public double? FlatExtraAmount { get; set; }

        public double? FlatExtraUnit { get; set; }

        public int? FlatExtraTerm { get; set; }

        public double? FlatExtraAmount2 { get; set; }

        public int? FlatExtraTerm2 { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? StandardDiscount { get; set; }

        public double? SubstandardDiscount { get; set; }

        public double? VitalityDiscount { get; set; }

        public double? TotalDiscount { get; set; }

        //public double? NetPremium { get; set; }

        public double? AnnualRiPrem { get; set; }

        public double? RiCovPeriod { get; set; }

        public DateTime? AdjBeginDate { get; set; }

        public DateTime? AdjEndDate { get; set; }

        public string PolicyNumberOld { get; set; }

        public string PolicyStatusCode { get; set; }

        public double? PolicyGrossPremium { get; set; }

        public double? PolicyStandardPremium { get; set; }

        public double? PolicySubstandardPremium { get; set; }

        public double? PolicyTermRemain { get; set; }

        public double? PolicyAmountDeath { get; set; }

        public double? PolicyReserve { get; set; }

        public string PolicyPaymentMethod { get; set; }

        public int? PolicyLifeNumber { get; set; }

        public string FundCode { get; set; }

        public string LineOfBusiness { get; set; }

        public double? ApLoading { get; set; }

        public double? LoanInterestRate { get; set; }

        public int? DefermentPeriod { get; set; }

        public int? RiderNumber { get; set; }

        public string CampaignCode { get; set; }

        public string Nationality { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public string CurrencyCode { get; set; }

        public string StaffPlanIndicator { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCodeOld { get; set; }

        public string CedingBasicPlanCode { get; set; }

        public double? CedantSar { get; set; }

        public string CedantReinsurerCode { get; set; }

        public double? AmountCededB4MlreShare2 { get; set; }

        public string CessionCode { get; set; }

        public string CedantRemark { get; set; }

        public string GroupPolicyNumber { get; set; }

        public string GroupPolicyName { get; set; }

        public int? NoOfEmployee { get; set; }

        public int? PolicyTotalLive { get; set; }

        public string GroupSubsidiaryName { get; set; }

        public string GroupSubsidiaryNo { get; set; }

        public double? GroupEmployeeBasicSalary { get; set; }

        public string GroupEmployeeJobType { get; set; }

        public string GroupEmployeeJobCode { get; set; }

        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string DependantIndicator { get; set; }

        public int? GhsRoomBoard { get; set; }

        public double? PolicyAmountSubstandard { get; set; }

        public double? Layer1RiShare { get; set; }

        public int? Layer1InsuredAttainedAge { get; set; }

        public int? Layer1InsuredAttainedAge2nd { get; set; }

        public double? Layer1StandardPremium { get; set; }

        public double? Layer1SubstandardPremium { get; set; }

        public double? Layer1GrossPremium { get; set; }

        public double? Layer1StandardDiscount { get; set; }

        public double? Layer1SubstandardDiscount { get; set; }

        public double? Layer1TotalDiscount { get; set; }

        public double? Layer1NetPremium { get; set; }

        public double? Layer1GrossPremiumAlt { get; set; }

        public double? Layer1TotalDiscountAlt { get; set; }

        public double? Layer1NetPremiumAlt { get; set; }

        public string SpecialIndicator1 { get; set; }

        public string SpecialIndicator2 { get; set; }

        public string SpecialIndicator3 { get; set; }

        public string IndicatorJointLife { get; set; }

        public double? TaxAmount { get; set; }

        public string GstIndicator { get; set; }

        public double? GstGrossPremium { get; set; }

        public double? GstTotalDiscount { get; set; }

        public double? GstVitality { get; set; }

        public double? GstAmount { get; set; }

        public string Mfrs17BasicRider { get; set; }

        public string Mfrs17CellName { get; set; }

        public string Mfrs17TreatyCode { get; set; }

        public string LoaCode { get; set; }

        public double? CurrencyRate { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? SurrenderValue { get; set; }

        public double? DatabaseCommision { get; set; }

        public double? GrossPremiumAlt { get; set; }

        public double? NetPremiumAlt { get; set; }

        public double? Layer1FlatExtraPremium { get; set; }

        public double? TransactionPremium { get; set; }

        public double? OriginalPremium { get; set; }

        public double? TransactionDiscount { get; set; }

        public double? OriginalDiscount { get; set; }

        public double? BrokerageFee { get; set; }

        public double? MaxUwRating { get; set; }

        public double? RetentionCap { get; set; }

        public double? AarCap { get; set; }

        public double? RiRate { get; set; }

        public double? RiRate2 { get; set; }

        public double? AnnuityFactor { get; set; }

        public double? SumAssuredOffered { get; set; }

        public double? UwRatingOffered { get; set; }

        public double? FlatExtraAmountOffered { get; set; }

        public double? FlatExtraDuration { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? OfferLetterSentDate { get; set; }

        public DateTime? RiskPeriodStartDate { get; set; }

        public DateTime? RiskPeriodEndDate { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public int? MaxExpiryAge { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public double? MinAar { get; set; }

        public double? MaxAar { get; set; }

        public double? CorridorLimit { get; set; }

        public double? Abl { get; set; }

        public int? RatePerBasisUnit { get; set; }

        public double? RiDiscountRate { get; set; }

        public double? LargeSaDiscount { get; set; }

        public double? GroupSizeDiscount { get; set; }

        public int? EwarpNumber { get; set; }

        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

        public string TreatyType { get; set; }

        public double? MaxApLoading { get; set; }

        public int? MlreInsuredAttainedAgeAtCurrentMonth { get; set; }

        public int? MlreInsuredAttainedAgeAtPreviousMonth { get; set; }

        public bool? InsuredAttainedAgeCheck { get; set; }

        public bool? MaxExpiryAgeCheck { get; set; }

        public int? MlrePolicyIssueAge { get; set; }

        public bool? PolicyIssueAgeCheck { get; set; }

        public bool? MinIssueAgeCheck { get; set; }

        public bool? MaxIssueAgeCheck { get; set; }

        public bool? MaxUwRatingCheck { get; set; }

        public bool? ApLoadingCheck { get; set; }

        public bool? EffectiveDateCheck { get; set; }

        public bool? MinAarCheck { get; set; }

        public bool? MaxAarCheck { get; set; }

        public bool? CorridorLimitCheck { get; set; }

        public bool? AblCheck { get; set; }

        public bool? RetentionCheck { get; set; }

        public bool? AarCheck { get; set; }

        public double? MlreStandardPremium { get; set; }

        public double? MlreSubstandardPremium { get; set; }

        public double? MlreFlatExtraPremium { get; set; }

        public double? MlreGrossPremium { get; set; }

        public double? MlreStandardDiscount { get; set; }

        public double? MlreSubstandardDiscount { get; set; }

        public double? MlreLargeSaDiscount { get; set; }

        public double? MlreGroupSizeDiscount { get; set; }

        public double? MlreVitalityDiscount { get; set; }

        public double? MlreTotalDiscount { get; set; }

        public double? MlreNetPremium { get; set; }

        public double? NetPremiumCheck { get; set; }

        public double? ServiceFeePercentage { get; set; }

        public double? ServiceFee { get; set; }

        public double? MlreBrokerageFee { get; set; }

        public double? MlreDatabaseCommission { get; set; }

        public bool? ValidityDayCheck { get; set; }

        public bool? SumAssuredOfferedCheck { get; set; }

        public bool? UwRatingCheck { get; set; }

        public bool? FlatExtraAmountCheck { get; set; }

        public bool? FlatExtraDurationCheck { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

        public double? RetroAar1 { get; set; }

        public double? RetroAar2 { get; set; }

        public double? RetroAar3 { get; set; }

        public double? RetroReinsurancePremium1 { get; set; }

        public double? RetroReinsurancePremium2 { get; set; }

        public double? RetroReinsurancePremium3 { get; set; }

        public double? RetroDiscount1 { get; set; }

        public double? RetroDiscount2 { get; set; }

        public double? RetroDiscount3 { get; set; }

        public double? RetroNetPremium1 { get; set; }

        public double? RetroNetPremium2 { get; set; }

        public double? RetroNetPremium3 { get; set; }

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        public PerLifeAggregationMonthlyDataViewModel() { }

        public PerLifeAggregationMonthlyDataViewModel(PerLifeAggregationMonthlyDataBo perLifeAggregationMonthlyDataBo)
        {
            Set(perLifeAggregationMonthlyDataBo);
        }

        public void Set(PerLifeAggregationMonthlyDataBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId;
                PerLifeAggregationDetailDataBo = bo.PerLifeAggregationDetailDataBo;
                RiskYear = bo.RiskYear;
                RiskMonth = bo.RiskMonth;
                UniqueKeyPerLife = UniqueKeyPerLife;
                RetroPremFreq = RetroPremFreq;
                Aar = Aar;
                SumOfAar = SumOfAar;
                NetPremium = NetPremium;
                SumOfNetPremium = SumOfNetPremium;
                RetentionLimit = RetentionLimit;
                DistributedRetentionLimit = DistributedRetentionLimit;
                RetroAmount = RetroAmount;
                DistributedRetroAmount = DistributedRetroAmount;
                AccumulativeRetainAmount = AccumulativeRetainAmount;
                RetroGrossPremium = RetroGrossPremium;
                RetroNetPremium = RetroNetPremium;
                RetroDiscount = RetroDiscount;
                Errors = Errors;
            }
        }

        public PerLifeAggregationMonthlyDataBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeAggregationMonthlyDataBo
            {
                Id = Id,
                PerLifeAggregationDetailDataId = PerLifeAggregationDetailDataId,
                PerLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(PerLifeAggregationDetailDataId),
                RiskYear = RiskYear,
                RiskMonth = RiskMonth,
                UniqueKeyPerLife = UniqueKeyPerLife,
                RetroPremFreq = RetroPremFreq,
                Aar = Aar,
                SumOfAar = SumOfAar,
                NetPremium = NetPremium,
                SumOfNetPremium = SumOfNetPremium,
                RetentionLimit = RetentionLimit,
                DistributedRetentionLimit = DistributedRetentionLimit,
                RetroAmount = RetroAmount,
                DistributedRetroAmount = DistributedRetroAmount,
                AccumulativeRetainAmount = AccumulativeRetainAmount,
                RetroGrossPremium = RetroGrossPremium,
                RetroNetPremium = RetroNetPremium,
                RetroDiscount = RetroDiscount,
                RetroIndicator = RetroIndicator,
                Errors = Errors,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeAggregationMonthlyData, PerLifeAggregationMonthlyDataViewModel>> Expression()
        {
            return entity => new PerLifeAggregationMonthlyDataViewModel
            {
                Id = entity.Id,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                PerLifeAggregationDetailData = entity.PerLifeAggregationDetailData,
                RiskYear = entity.RiskYear,
                RiskMonth = entity.RiskMonth,
                UniqueKeyPerLife = entity.UniqueKeyPerLife,
                RetroPremFreq = entity.RetroPremFreq,
                Aar = entity.Aar,
                SumOfAar = entity.SumOfAar,
                NetPremium = entity.NetPremium,
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
                RetroBenefitCode = entity.PerLifeAggregationDetailData.RetroBenefitCode, //TODO: To be confirm
                PerLifeAggregationMonthlyRetroData = entity.PerLifeAggregationMonthlyRetroData,

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
                //Aar = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.Aar ?? 0,
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
                //NetPremium = entity.PerLifeAggregationDetailData.RiDataWarehouseHistory.NetPremium ?? 0,
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
    }
}