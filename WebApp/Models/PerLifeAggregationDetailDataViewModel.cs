using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.Entities.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class PerLifeAggregationDetailDataViewModel
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailTreatyId { get; set; }

        public PerLifeAggregationDetailTreaty PerLifeAggregationDetailTreaty { get; set; }

        public int RiDataWarehouseHistoryId { get; set; }

        public RiDataWarehouseHistory RiDataWarehouseHistory { get; set; }

        public string ExpectedGenderCode { get; set; }

        public string RetroBenefitCode { get; set; }

        public string ExpectedTerritoryOfIssueCode { get; set; }

        public int? FlagCode { get; set; }

        public int? ExceptionType { get; set; }

        public int? ExceptionErrorType { get; set; }

        public bool IsException { get; set; }

        public string Errors { get; set; }

        [DisplayName("Proceed to Aggregate")]
        public int ProceedStatus { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [DisplayName("Date Updated")]
        public string UpdatedAtStr { get; set; }

        // RI Data Warehouse History
        public string Quarter { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public int RecordType { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Reinsurance Risk Basis ID")]
        public string ReinsBasisCode { get; set; }

        [DisplayName("Funds Accounting Type")]
        public string FundsAccountingTypeCode { get; set; }

        [DisplayName("Premium Frequency Code")]
        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        [DisplayName("Risk Month")]
        public int? RiskPeriodMonth { get; set; }

        [DisplayName("Risk Year")]
        public int? RiskPeriodYear { get; set; }

        [DisplayName("Transaction Type Code")]
        public string TransactionTypeCode { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        public DateTime? ReinsEffDatePol { get; set; }

        public DateTime? ReinsEffDateBen { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Ceding Benefit Type Code")]
        public string CedingBenefitTypeCode { get; set; }

        [DisplayName("Ceding Benefit Risk Code")]
        public string CedingBenefitRiskCode { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? RetentionAmount { get; set; }

        public double? AarOri { get; set; }

        public double? Aar { get; set; }

        public double? AarSpecial1 { get; set; }

        public double? AarSpecial2 { get; set; }

        public double? AarSpecial3 { get; set; }

        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Insured Gender Code")]
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

        public double? NetPremium { get; set; }

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

        [DisplayName("Territory of Issue Code")]
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

        // RiDataWarehouseHistory Str
        [DisplayName("Insured Date of Birth")]
        public string InsuredDateOfBirthStr { get; set; }

        [DisplayName("Reinsurance Effective Date Policy")]
        public string ReinsEffDatePolStr { get; set; }

        [DisplayName("AAR")]
        public string AarStr { get; set; }

        [DisplayName("Gross Premium")]
        public string GrossPremiumStr { get; set; }

        [DisplayName("Net Premium")]
        public string NetPremiumStr { get; set; }

        public PerLifeAggregationDetailDataViewModel() { }

        public PerLifeAggregationDetailDataViewModel(PerLifeAggregationDetailDataBo perLifeAggregationDetailDataBo)
        {
            Set(perLifeAggregationDetailDataBo);
        }

        public void Set(PerLifeAggregationDetailDataBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                PerLifeAggregationDetailTreatyId = bo.PerLifeAggregationDetailTreatyId;
                RiDataWarehouseHistoryId = bo.RiDataWarehouseHistoryId;
                ExpectedGenderCode = bo.ExpectedGenderCode;
                RetroBenefitCode = bo.RetroBenefitCode;
                ExpectedTerritoryOfIssueCode = bo.ExpectedTerritoryOfIssueCode;
                FlagCode = bo.FlagCode;
                ExceptionType = bo.ExceptionType;
                ExceptionErrorType = bo.ExceptionErrorType;
                IsException = bo.IsException;
                Errors = bo.Errors;
                ProceedStatus = bo.ProceedStatus;
                Remarks = bo.Remarks;
                UpdatedAt = bo.UpdatedAt;
                UpdatedAtStr = bo.UpdatedAt?.ToString(Util.GetDateFormat());

                // RI Data Warehouse History
                Quarter = bo.RiDataWarehouseHistoryBo.Quarter;
                EndingPolicyStatus = bo.RiDataWarehouseHistoryBo.EndingPolicyStatus;
                RecordType = bo.RiDataWarehouseHistoryBo.RecordType;
                TreatyCode = bo.RiDataWarehouseHistoryBo.TreatyCode;
                ReinsBasisCode = bo.RiDataWarehouseHistoryBo.ReinsBasisCode;
                FundsAccountingTypeCode = bo.RiDataWarehouseHistoryBo.FundsAccountingTypeCode;
                PremiumFrequencyCode = bo.RiDataWarehouseHistoryBo.PremiumFrequencyCode;
                ReportPeriodMonth = bo.RiDataWarehouseHistoryBo.ReportPeriodMonth;
                ReportPeriodYear = bo.RiDataWarehouseHistoryBo.ReportPeriodYear;
                RiskPeriodMonth = bo.RiDataWarehouseHistoryBo.RiskPeriodMonth;
                RiskPeriodYear = bo.RiDataWarehouseHistoryBo.RiskPeriodYear;
                TransactionTypeCode = bo.RiDataWarehouseHistoryBo.TransactionTypeCode;
                PolicyNumber = bo.RiDataWarehouseHistoryBo.PolicyNumber;
                IssueDatePol = bo.RiDataWarehouseHistoryBo.IssueDatePol;
                IssueDateBen = bo.RiDataWarehouseHistoryBo.IssueDateBen;
                ReinsEffDatePol = bo.RiDataWarehouseHistoryBo.ReinsEffDatePol;
                ReinsEffDateBen = bo.RiDataWarehouseHistoryBo.ReinsEffDateBen;
                CedingPlanCode = bo.RiDataWarehouseHistoryBo.CedingPlanCode;
                CedingBenefitTypeCode = bo.RiDataWarehouseHistoryBo.CedingBenefitTypeCode;
                CedingBenefitRiskCode = bo.RiDataWarehouseHistoryBo.CedingBenefitRiskCode;
                MlreBenefitCode = bo.RiDataWarehouseHistoryBo.MlreBenefitCode;
                OriSumAssured = bo.RiDataWarehouseHistoryBo.OriSumAssured;
                CurrSumAssured = bo.RiDataWarehouseHistoryBo.CurrSumAssured;
                AmountCededB4MlreShare = bo.RiDataWarehouseHistoryBo.AmountCededB4MlreShare;
                RetentionAmount = bo.RiDataWarehouseHistoryBo.RetentionAmount;
                AarOri = bo.RiDataWarehouseHistoryBo.AarOri;
                Aar = bo.RiDataWarehouseHistoryBo.Aar;
                AarSpecial1 = bo.RiDataWarehouseHistoryBo.AarSpecial1;
                AarSpecial2 = bo.RiDataWarehouseHistoryBo.AarSpecial2;
                AarSpecial3 = bo.RiDataWarehouseHistoryBo.AarSpecial3;
                InsuredName = bo.RiDataWarehouseHistoryBo.InsuredName;
                InsuredGenderCode = bo.RiDataWarehouseHistoryBo.InsuredGenderCode;
                InsuredTobaccoUse = bo.RiDataWarehouseHistoryBo.InsuredTobaccoUse;
                InsuredDateOfBirth = bo.RiDataWarehouseHistoryBo.InsuredDateOfBirth;
                InsuredOccupationCode = bo.RiDataWarehouseHistoryBo.InsuredOccupationCode;
                InsuredRegisterNo = bo.RiDataWarehouseHistoryBo.InsuredRegisterNo;
                InsuredAttainedAge = bo.RiDataWarehouseHistoryBo.InsuredAttainedAge;
                InsuredNewIcNumber = bo.RiDataWarehouseHistoryBo.InsuredNewIcNumber;
                InsuredOldIcNumber = bo.RiDataWarehouseHistoryBo.InsuredOldIcNumber;
                InsuredName2nd = bo.RiDataWarehouseHistoryBo.InsuredName2nd;
                InsuredGenderCode2nd = bo.RiDataWarehouseHistoryBo.InsuredGenderCode2nd;
                InsuredTobaccoUse2nd = bo.RiDataWarehouseHistoryBo.InsuredTobaccoUse2nd;
                InsuredDateOfBirth2nd = bo.RiDataWarehouseHistoryBo.InsuredDateOfBirth2nd;
                InsuredAttainedAge2nd = bo.RiDataWarehouseHistoryBo.InsuredAttainedAge2nd;
                InsuredNewIcNumber2nd = bo.RiDataWarehouseHistoryBo.InsuredNewIcNumber2nd;
                InsuredOldIcNumber2nd = bo.RiDataWarehouseHistoryBo.InsuredOldIcNumber2nd;
                ReinsuranceIssueAge = bo.RiDataWarehouseHistoryBo.ReinsuranceIssueAge;
                ReinsuranceIssueAge2nd = bo.RiDataWarehouseHistoryBo.ReinsuranceIssueAge2nd;
                PolicyTerm = bo.RiDataWarehouseHistoryBo.PolicyTerm;
                PolicyExpiryDate = bo.RiDataWarehouseHistoryBo.PolicyExpiryDate;
                DurationYear = bo.RiDataWarehouseHistoryBo.DurationYear;
                DurationDay = bo.RiDataWarehouseHistoryBo.DurationDay;
                DurationMonth = bo.RiDataWarehouseHistoryBo.DurationMonth;
                PremiumCalType = bo.RiDataWarehouseHistoryBo.PremiumCalType;
                CedantRiRate = bo.RiDataWarehouseHistoryBo.CedantRiRate;
                RateTable = bo.RiDataWarehouseHistoryBo.RateTable;
                AgeRatedUp = bo.RiDataWarehouseHistoryBo.AgeRatedUp;
                DiscountRate = bo.RiDataWarehouseHistoryBo.DiscountRate;
                LoadingType = bo.RiDataWarehouseHistoryBo.LoadingType;
                UnderwriterRating = bo.RiDataWarehouseHistoryBo.UnderwriterRating;
                UnderwriterRatingUnit = bo.RiDataWarehouseHistoryBo.UnderwriterRatingUnit;
                UnderwriterRatingTerm = bo.RiDataWarehouseHistoryBo.UnderwriterRatingTerm;
                UnderwriterRating2 = bo.RiDataWarehouseHistoryBo.UnderwriterRating2;
                UnderwriterRatingUnit2 = bo.RiDataWarehouseHistoryBo.UnderwriterRatingUnit2;
                UnderwriterRatingTerm2 = bo.RiDataWarehouseHistoryBo.UnderwriterRatingTerm2;
                UnderwriterRating3 = bo.RiDataWarehouseHistoryBo.UnderwriterRating3;
                UnderwriterRatingUnit3 = bo.RiDataWarehouseHistoryBo.UnderwriterRatingUnit3;
                UnderwriterRatingTerm3 = bo.RiDataWarehouseHistoryBo.UnderwriterRatingTerm3;
                FlatExtraAmount = bo.RiDataWarehouseHistoryBo.FlatExtraAmount;
                FlatExtraUnit = bo.RiDataWarehouseHistoryBo.FlatExtraUnit;
                FlatExtraTerm = bo.RiDataWarehouseHistoryBo.FlatExtraTerm;
                FlatExtraAmount2 = bo.RiDataWarehouseHistoryBo.FlatExtraAmount2;
                FlatExtraTerm2 = bo.RiDataWarehouseHistoryBo.FlatExtraTerm2;
                StandardPremium = bo.RiDataWarehouseHistoryBo.StandardPremium;
                SubstandardPremium = bo.RiDataWarehouseHistoryBo.SubstandardPremium;
                FlatExtraPremium = bo.RiDataWarehouseHistoryBo.FlatExtraPremium;
                GrossPremium = bo.RiDataWarehouseHistoryBo.GrossPremium;
                StandardDiscount = bo.RiDataWarehouseHistoryBo.StandardDiscount;
                SubstandardDiscount = bo.RiDataWarehouseHistoryBo.SubstandardDiscount;
                VitalityDiscount = bo.RiDataWarehouseHistoryBo.VitalityDiscount;
                TotalDiscount = bo.RiDataWarehouseHistoryBo.TotalDiscount;
                NetPremium = bo.RiDataWarehouseHistoryBo.NetPremium;
                AnnualRiPrem = bo.RiDataWarehouseHistoryBo.AnnualRiPrem;
                RiCovPeriod = bo.RiDataWarehouseHistoryBo.RiCovPeriod;
                AdjBeginDate = bo.RiDataWarehouseHistoryBo.AdjBeginDate;
                AdjEndDate = bo.RiDataWarehouseHistoryBo.AdjEndDate;
                PolicyNumberOld = bo.RiDataWarehouseHistoryBo.PolicyNumberOld;
                PolicyStatusCode = bo.RiDataWarehouseHistoryBo.PolicyStatusCode;
                PolicyGrossPremium = bo.RiDataWarehouseHistoryBo.PolicyGrossPremium;
                PolicyStandardPremium = bo.RiDataWarehouseHistoryBo.PolicyStandardPremium;
                PolicySubstandardPremium = bo.RiDataWarehouseHistoryBo.PolicySubstandardPremium;
                PolicyTermRemain = bo.RiDataWarehouseHistoryBo.PolicyTermRemain;
                PolicyAmountDeath = bo.RiDataWarehouseHistoryBo.PolicyAmountDeath;
                PolicyReserve = bo.RiDataWarehouseHistoryBo.PolicyReserve;
                PolicyPaymentMethod = bo.RiDataWarehouseHistoryBo.PolicyPaymentMethod;
                PolicyLifeNumber = bo.RiDataWarehouseHistoryBo.PolicyLifeNumber;
                FundCode = bo.RiDataWarehouseHistoryBo.FundCode;
                LineOfBusiness = bo.RiDataWarehouseHistoryBo.LineOfBusiness;
                ApLoading = bo.RiDataWarehouseHistoryBo.ApLoading;
                LoanInterestRate = bo.RiDataWarehouseHistoryBo.LoanInterestRate;
                DefermentPeriod = bo.RiDataWarehouseHistoryBo.DefermentPeriod;
                RiderNumber = bo.RiDataWarehouseHistoryBo.RiderNumber;
                CampaignCode = bo.RiDataWarehouseHistoryBo.CampaignCode;
                Nationality = bo.RiDataWarehouseHistoryBo.Nationality;
                TerritoryOfIssueCode = bo.RiDataWarehouseHistoryBo.TerritoryOfIssueCode;
                CurrencyCode = bo.RiDataWarehouseHistoryBo.CurrencyCode;
                StaffPlanIndicator = bo.RiDataWarehouseHistoryBo.StaffPlanIndicator;
                CedingTreatyCode = bo.RiDataWarehouseHistoryBo.CedingTreatyCode;
                CedingPlanCodeOld = bo.RiDataWarehouseHistoryBo.CedingPlanCodeOld;
                CedingBasicPlanCode = bo.RiDataWarehouseHistoryBo.CedingBasicPlanCode;
                CedantSar = bo.RiDataWarehouseHistoryBo.CedantSar;
                CedantReinsurerCode = bo.RiDataWarehouseHistoryBo.CedantReinsurerCode;
                AmountCededB4MlreShare2 = bo.RiDataWarehouseHistoryBo.AmountCededB4MlreShare2;
                CessionCode = bo.RiDataWarehouseHistoryBo.CessionCode;
                CedantRemark = bo.RiDataWarehouseHistoryBo.CedantRemark;
                GroupPolicyNumber = bo.RiDataWarehouseHistoryBo.GroupPolicyNumber;
                GroupPolicyName = bo.RiDataWarehouseHistoryBo.GroupPolicyName;
                NoOfEmployee = bo.RiDataWarehouseHistoryBo.NoOfEmployee;
                PolicyTotalLive = bo.RiDataWarehouseHistoryBo.PolicyTotalLive;
                GroupSubsidiaryName = bo.RiDataWarehouseHistoryBo.GroupSubsidiaryName;
                GroupSubsidiaryNo = bo.RiDataWarehouseHistoryBo.GroupSubsidiaryNo;
                GroupEmployeeBasicSalary = bo.RiDataWarehouseHistoryBo.GroupEmployeeBasicSalary;
                GroupEmployeeJobType = bo.RiDataWarehouseHistoryBo.GroupEmployeeJobType;
                GroupEmployeeJobCode = bo.RiDataWarehouseHistoryBo.GroupEmployeeJobCode;
                GroupEmployeeBasicSalaryRevise = bo.RiDataWarehouseHistoryBo.GroupEmployeeBasicSalaryRevise;
                GroupEmployeeBasicSalaryMultiplier = bo.RiDataWarehouseHistoryBo.GroupEmployeeBasicSalaryMultiplier;
                CedingPlanCode2 = bo.RiDataWarehouseHistoryBo.CedingPlanCode2;
                DependantIndicator = bo.RiDataWarehouseHistoryBo.DependantIndicator;
                GhsRoomBoard = bo.RiDataWarehouseHistoryBo.GhsRoomBoard;
                PolicyAmountSubstandard = bo.RiDataWarehouseHistoryBo.PolicyAmountSubstandard;
                Layer1RiShare = bo.RiDataWarehouseHistoryBo.Layer1RiShare;
                Layer1InsuredAttainedAge = bo.RiDataWarehouseHistoryBo.Layer1InsuredAttainedAge;
                Layer1InsuredAttainedAge2nd = bo.RiDataWarehouseHistoryBo.Layer1InsuredAttainedAge2nd;
                Layer1StandardPremium = bo.RiDataWarehouseHistoryBo.Layer1StandardPremium;
                Layer1SubstandardPremium = bo.RiDataWarehouseHistoryBo.Layer1SubstandardPremium;
                Layer1GrossPremium = bo.RiDataWarehouseHistoryBo.Layer1GrossPremium;
                Layer1StandardDiscount = bo.RiDataWarehouseHistoryBo.Layer1StandardDiscount;
                Layer1SubstandardDiscount = bo.RiDataWarehouseHistoryBo.Layer1SubstandardDiscount;
                Layer1TotalDiscount = bo.RiDataWarehouseHistoryBo.Layer1TotalDiscount;
                Layer1NetPremium = bo.RiDataWarehouseHistoryBo.Layer1NetPremium;
                Layer1GrossPremiumAlt = bo.RiDataWarehouseHistoryBo.Layer1GrossPremiumAlt;
                Layer1TotalDiscountAlt = bo.RiDataWarehouseHistoryBo.Layer1TotalDiscountAlt;
                Layer1NetPremiumAlt = bo.RiDataWarehouseHistoryBo.Layer1NetPremiumAlt;
                SpecialIndicator1 = bo.RiDataWarehouseHistoryBo.SpecialIndicator1;
                SpecialIndicator2 = bo.RiDataWarehouseHistoryBo.SpecialIndicator2;
                SpecialIndicator3 = bo.RiDataWarehouseHistoryBo.SpecialIndicator3;
                IndicatorJointLife = bo.RiDataWarehouseHistoryBo.IndicatorJointLife;
                TaxAmount = bo.RiDataWarehouseHistoryBo.TaxAmount;
                GstIndicator = bo.RiDataWarehouseHistoryBo.GstIndicator;
                GstGrossPremium = bo.RiDataWarehouseHistoryBo.GstGrossPremium;
                GstTotalDiscount = bo.RiDataWarehouseHistoryBo.GstTotalDiscount;
                GstVitality = bo.RiDataWarehouseHistoryBo.GstVitality;
                GstAmount = bo.RiDataWarehouseHistoryBo.GstAmount;
                Mfrs17BasicRider = bo.RiDataWarehouseHistoryBo.Mfrs17BasicRider;
                Mfrs17CellName = bo.RiDataWarehouseHistoryBo.Mfrs17CellName;
                Mfrs17TreatyCode = bo.RiDataWarehouseHistoryBo.Mfrs17TreatyCode;
                LoaCode = bo.RiDataWarehouseHistoryBo.LoaCode;
                NoClaimBonus = bo.RiDataWarehouseHistoryBo.NoClaimBonus;
                SurrenderValue = bo.RiDataWarehouseHistoryBo.SurrenderValue;
                DatabaseCommision = bo.RiDataWarehouseHistoryBo.DatabaseCommision;
                GrossPremiumAlt = bo.RiDataWarehouseHistoryBo.GrossPremiumAlt;
                NetPremiumAlt = bo.RiDataWarehouseHistoryBo.NetPremiumAlt;
                Layer1FlatExtraPremium = bo.RiDataWarehouseHistoryBo.Layer1FlatExtraPremium;
                TransactionPremium = bo.RiDataWarehouseHistoryBo.TransactionPremium;
                OriginalPremium = bo.RiDataWarehouseHistoryBo.OriginalPremium;
                TransactionDiscount = bo.RiDataWarehouseHistoryBo.TransactionDiscount;
                OriginalDiscount = bo.RiDataWarehouseHistoryBo.OriginalDiscount;
                BrokerageFee = bo.RiDataWarehouseHistoryBo.BrokerageFee;
                MaxUwRating = bo.RiDataWarehouseHistoryBo.MaxUwRating;
                RetentionCap = bo.RiDataWarehouseHistoryBo.RetentionCap;
                AarCap = bo.RiDataWarehouseHistoryBo.AarCap;
                RiRate = bo.RiDataWarehouseHistoryBo.RiRate;
                RiRate2 = bo.RiDataWarehouseHistoryBo.RiRate2;
                AnnuityFactor = bo.RiDataWarehouseHistoryBo.AnnuityFactor;
                SumAssuredOffered = bo.RiDataWarehouseHistoryBo.SumAssuredOffered;
                UwRatingOffered = bo.RiDataWarehouseHistoryBo.UwRatingOffered;
                FlatExtraAmountOffered = bo.RiDataWarehouseHistoryBo.FlatExtraAmountOffered;
                FlatExtraDuration = bo.RiDataWarehouseHistoryBo.FlatExtraDuration;
                EffectiveDate = bo.RiDataWarehouseHistoryBo.EffectiveDate;
                OfferLetterSentDate = bo.RiDataWarehouseHistoryBo.OfferLetterSentDate;
                RiskPeriodStartDate = bo.RiDataWarehouseHistoryBo.RiskPeriodStartDate;
                RiskPeriodEndDate = bo.RiDataWarehouseHistoryBo.RiskPeriodEndDate;
                Mfrs17AnnualCohort = bo.RiDataWarehouseHistoryBo.Mfrs17AnnualCohort;
                MaxExpiryAge = bo.RiDataWarehouseHistoryBo.MaxExpiryAge;
                MinIssueAge = bo.RiDataWarehouseHistoryBo.MinIssueAge;
                MaxIssueAge = bo.RiDataWarehouseHistoryBo.MaxIssueAge;
                MinAar = bo.RiDataWarehouseHistoryBo.MinAar;
                MaxAar = bo.RiDataWarehouseHistoryBo.MaxAar;
                CorridorLimit = bo.RiDataWarehouseHistoryBo.CorridorLimit;
                Abl = bo.RiDataWarehouseHistoryBo.Abl;
                RatePerBasisUnit = bo.RiDataWarehouseHistoryBo.RatePerBasisUnit;
                RiDiscountRate = bo.RiDataWarehouseHistoryBo.RiDiscountRate;
                LargeSaDiscount = bo.RiDataWarehouseHistoryBo.LargeSaDiscount;
                GroupSizeDiscount = bo.RiDataWarehouseHistoryBo.GroupSizeDiscount;
                EwarpNumber = bo.RiDataWarehouseHistoryBo.EwarpNumber;
                EwarpActionCode = bo.RiDataWarehouseHistoryBo.EwarpActionCode;
                RetentionShare = bo.RiDataWarehouseHistoryBo.RetentionShare;
                AarShare = bo.RiDataWarehouseHistoryBo.AarShare;
                ProfitComm = bo.RiDataWarehouseHistoryBo.ProfitComm;
                TotalDirectRetroAar = bo.RiDataWarehouseHistoryBo.TotalDirectRetroAar;
                TotalDirectRetroGrossPremium = bo.RiDataWarehouseHistoryBo.TotalDirectRetroGrossPremium;
                TotalDirectRetroDiscount = bo.RiDataWarehouseHistoryBo.TotalDirectRetroDiscount;
                TotalDirectRetroNetPremium = bo.RiDataWarehouseHistoryBo.TotalDirectRetroNetPremium;
                TreatyType = bo.RiDataWarehouseHistoryBo.TreatyType;
                MaxApLoading = bo.RiDataWarehouseHistoryBo.MaxApLoading;
                MlreInsuredAttainedAgeAtCurrentMonth = bo.RiDataWarehouseHistoryBo.MlreInsuredAttainedAgeAtCurrentMonth;
                MlreInsuredAttainedAgeAtPreviousMonth = bo.RiDataWarehouseHistoryBo.MlreInsuredAttainedAgeAtPreviousMonth;
                InsuredAttainedAgeCheck = bo.RiDataWarehouseHistoryBo.InsuredAttainedAgeCheck;
                MaxExpiryAgeCheck = bo.RiDataWarehouseHistoryBo.MaxExpiryAgeCheck;
                MlrePolicyIssueAge = bo.RiDataWarehouseHistoryBo.MlrePolicyIssueAge;
                PolicyIssueAgeCheck = bo.RiDataWarehouseHistoryBo.PolicyIssueAgeCheck;
                MinIssueAgeCheck = bo.RiDataWarehouseHistoryBo.MinIssueAgeCheck;
                MaxIssueAgeCheck = bo.RiDataWarehouseHistoryBo.MaxIssueAgeCheck;
                MaxUwRatingCheck = bo.RiDataWarehouseHistoryBo.MaxUwRatingCheck;
                ApLoadingCheck = bo.RiDataWarehouseHistoryBo.ApLoadingCheck;
                EffectiveDateCheck = bo.RiDataWarehouseHistoryBo.EffectiveDateCheck;
                MinAarCheck = bo.RiDataWarehouseHistoryBo.MinAarCheck;
                MaxAarCheck = bo.RiDataWarehouseHistoryBo.MaxAarCheck;
                CorridorLimitCheck = bo.RiDataWarehouseHistoryBo.CorridorLimitCheck;
                AblCheck = bo.RiDataWarehouseHistoryBo.AblCheck;
                RetentionCheck = bo.RiDataWarehouseHistoryBo.RetentionCheck;
                AarCheck = bo.RiDataWarehouseHistoryBo.AarCheck;
                MlreStandardPremium = bo.RiDataWarehouseHistoryBo.MlreStandardPremium;
                MlreSubstandardPremium = bo.RiDataWarehouseHistoryBo.MlreSubstandardPremium;
                MlreFlatExtraPremium = bo.RiDataWarehouseHistoryBo.MlreFlatExtraPremium;
                MlreGrossPremium = bo.RiDataWarehouseHistoryBo.MlreGrossPremium;
                MlreStandardDiscount = bo.RiDataWarehouseHistoryBo.MlreStandardDiscount;
                MlreSubstandardDiscount = bo.RiDataWarehouseHistoryBo.MlreSubstandardDiscount;
                MlreLargeSaDiscount = bo.RiDataWarehouseHistoryBo.MlreLargeSaDiscount;
                MlreGroupSizeDiscount = bo.RiDataWarehouseHistoryBo.MlreGroupSizeDiscount;
                MlreVitalityDiscount = bo.RiDataWarehouseHistoryBo.MlreVitalityDiscount;
                MlreTotalDiscount = bo.RiDataWarehouseHistoryBo.MlreTotalDiscount;
                MlreNetPremium = bo.RiDataWarehouseHistoryBo.MlreNetPremium;
                NetPremiumCheck = bo.RiDataWarehouseHistoryBo.NetPremiumCheck;
                ServiceFeePercentage = bo.RiDataWarehouseHistoryBo.ServiceFeePercentage;
                ServiceFee = bo.RiDataWarehouseHistoryBo.ServiceFee;
                MlreBrokerageFee = bo.RiDataWarehouseHistoryBo.MlreBrokerageFee;
                MlreDatabaseCommission = bo.RiDataWarehouseHistoryBo.MlreDatabaseCommission;
                ValidityDayCheck = bo.RiDataWarehouseHistoryBo.ValidityDayCheck;
                SumAssuredOfferedCheck = bo.RiDataWarehouseHistoryBo.SumAssuredOfferedCheck;
                UwRatingCheck = bo.RiDataWarehouseHistoryBo.UwRatingCheck;
                FlatExtraAmountCheck = bo.RiDataWarehouseHistoryBo.FlatExtraAmountCheck;
                FlatExtraDurationCheck = bo.RiDataWarehouseHistoryBo.FlatExtraDurationCheck;
                LastUpdatedDate = bo.RiDataWarehouseHistoryBo.LastUpdatedDate;
                AarShare2 = bo.RiDataWarehouseHistoryBo.AarShare2;
                AarCap2 = bo.RiDataWarehouseHistoryBo.AarCap2;
                WakalahFeePercentage = bo.RiDataWarehouseHistoryBo.WakalahFeePercentage;
                TreatyNumber = bo.RiDataWarehouseHistoryBo.TreatyNumber;

                // Direct Retro
                RetroParty1 = bo.RiDataWarehouseHistoryBo.RetroParty1;
                RetroParty2 = bo.RiDataWarehouseHistoryBo.RetroParty2;
                RetroParty3 = bo.RiDataWarehouseHistoryBo.RetroParty3;
                RetroShare1 = bo.RiDataWarehouseHistoryBo.RetroShare1;
                RetroShare2 = bo.RiDataWarehouseHistoryBo.RetroShare2;
                RetroShare3 = bo.RiDataWarehouseHistoryBo.RetroShare3;
                RetroAar1 = bo.RiDataWarehouseHistoryBo.RetroAar1;
                RetroAar2 = bo.RiDataWarehouseHistoryBo.RetroAar2;
                RetroAar3 = bo.RiDataWarehouseHistoryBo.RetroAar3;
                RetroReinsurancePremium1 = bo.RiDataWarehouseHistoryBo.RetroReinsurancePremium1;
                RetroReinsurancePremium2 = bo.RiDataWarehouseHistoryBo.RetroReinsurancePremium2;
                RetroReinsurancePremium3 = bo.RiDataWarehouseHistoryBo.RetroReinsurancePremium3;
                RetroDiscount1 = bo.RiDataWarehouseHistoryBo.RetroDiscount1;
                RetroDiscount2 = bo.RiDataWarehouseHistoryBo.RetroDiscount2;
                RetroDiscount3 = bo.RiDataWarehouseHistoryBo.RetroDiscount3;
                RetroNetPremium1 = bo.RiDataWarehouseHistoryBo.RetroNetPremium1;
                RetroNetPremium2 = bo.RiDataWarehouseHistoryBo.RetroNetPremium2;
                RetroNetPremium3 = bo.RiDataWarehouseHistoryBo.RetroNetPremium3;

                // RiDataWarehouseHistory Str
                InsuredDateOfBirthStr = bo.RiDataWarehouseHistoryBo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                ReinsEffDatePolStr = bo.RiDataWarehouseHistoryBo.ReinsEffDatePol?.ToString(Util.GetDateFormat());

                AarStr = Util.DoubleToString(bo.RiDataWarehouseHistoryBo.Aar);
                GrossPremiumStr = Util.DoubleToString(bo.RiDataWarehouseHistoryBo.GrossPremium);
                NetPremiumStr = Util.DoubleToString(bo.RiDataWarehouseHistoryBo.NetPremium);
            }
        }

        public PerLifeAggregationDetailDataBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeAggregationDetailDataBo
            {
                Id = Id,
                PerLifeAggregationDetailTreatyId = PerLifeAggregationDetailTreatyId,
                RiDataWarehouseHistoryId = RiDataWarehouseHistoryId,
                ExpectedGenderCode = ExpectedGenderCode,
                RetroBenefitCode = RetroBenefitCode,
                ExpectedTerritoryOfIssueCode = ExpectedTerritoryOfIssueCode,
                FlagCode = FlagCode,
                ExceptionType = ExceptionType,
                ExceptionErrorType = ExceptionErrorType,
                IsException = IsException,
                Errors = Errors,
                ProceedStatus = ProceedStatus,
                Remarks = Remarks,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeAggregationDetailData, PerLifeAggregationDetailDataViewModel>> Expression()
        {
            return entity => new PerLifeAggregationDetailDataViewModel
            {
                Id = entity.Id,
                PerLifeAggregationDetailTreatyId = entity.PerLifeAggregationDetailTreatyId,
                PerLifeAggregationDetailTreaty = entity.PerLifeAggregationDetailTreaty,
                RiDataWarehouseHistoryId = entity.RiDataWarehouseHistoryId,
                RiDataWarehouseHistory = entity.RiDataWarehouseHistory,
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
            };
        }
    }
}