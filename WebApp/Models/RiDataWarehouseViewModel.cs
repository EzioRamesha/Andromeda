using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class RiDataWarehouseViewModel
    {
        public int Id { get; set; }

        public bool IsSnapShotVersion { get; set; }

        public int? CutOffId { get; set; }

        public string Quarter { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public int RecordType { get; set; }

        [Display(Name = "Treaty Code")]
        public string TreatyCode { get; set; }

        public string ReinsBasisCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string TransactionTypeCode { get; set; }

        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Policy Issue Date")]
        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        [Display(Name = "Reinsurance Effective Date")]
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

        public double? Aar { get; set; }

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

        [Display(Name = "Policy Term")]
        public double? PolicyTerm { get; set; }

        [Display(Name = "Policy Expiry Date")]
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

        [Display(Name = "MFRS17 Basis Rider")]
        public string Mfrs17BasicRider { get; set; }

        [Display(Name = "MFRS17 Cell Name")]
        public string Mfrs17CellName { get; set; }

        [Display(Name = "MFRS17 Contract Code")]
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

        public double? TotalDirectRetroNoClaimBonus { get; set; }

        public double? TotalDirectRetroDatabaseCommission { get; set; }

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

        public double? RetroPremiumSpread1 { get; set; }

        public double? RetroPremiumSpread2 { get; set; }

        public double? RetroPremiumSpread3 { get; set; }

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

        public double? RetroNoClaimBonus1 { get; set; }

        public double? RetroNoClaimBonus2 { get; set; }

        public double? RetroNoClaimBonus3 { get; set; }

        public double? RetroDatabaseCommission1 { get; set; }

        public double? RetroDatabaseCommission2 { get; set; }

        public double? RetroDatabaseCommission3 { get; set; }

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        public int ConflictType { get; set; }

        public RiDataWarehouseViewModel() { }

        public RiDataWarehouseViewModel(RiDataWarehouseBo riDataWarehouseBo)
        {
            Set(riDataWarehouseBo);
        }

        public RiDataWarehouseViewModel(RiDataWarehouseHistoryBo riDataWarehouseHistoryBo)
        {
            Set(riDataWarehouseHistoryBo);
        }

        public void Set(RiDataWarehouseBo riDataWarehouseBo)
        {
            Id = riDataWarehouseBo.Id;

            Quarter = riDataWarehouseBo.Quarter;
            EndingPolicyStatus = riDataWarehouseBo.EndingPolicyStatus;
            RecordType = riDataWarehouseBo.RecordType;
            TreatyCode = riDataWarehouseBo.TreatyCode;
            ReinsBasisCode = riDataWarehouseBo.ReinsBasisCode;
            FundsAccountingTypeCode = riDataWarehouseBo.FundsAccountingTypeCode;
            PremiumFrequencyCode = riDataWarehouseBo.PremiumFrequencyCode;
            ReportPeriodMonth = riDataWarehouseBo.ReportPeriodMonth;
            ReportPeriodYear = riDataWarehouseBo.ReportPeriodYear;
            RiskPeriodMonth = riDataWarehouseBo.RiskPeriodMonth;
            RiskPeriodYear = riDataWarehouseBo.RiskPeriodYear;
            TransactionTypeCode = riDataWarehouseBo.TransactionTypeCode;
            PolicyNumber = riDataWarehouseBo.PolicyNumber;
            IssueDatePol = riDataWarehouseBo.IssueDatePol;
            IssueDateBen = riDataWarehouseBo.IssueDateBen;
            ReinsEffDatePol = riDataWarehouseBo.ReinsEffDatePol;
            ReinsEffDateBen = riDataWarehouseBo.ReinsEffDateBen;
            CedingPlanCode = riDataWarehouseBo.CedingPlanCode;
            CedingBenefitTypeCode = riDataWarehouseBo.CedingBenefitTypeCode;
            CedingBenefitRiskCode = riDataWarehouseBo.CedingBenefitRiskCode;
            MlreBenefitCode = riDataWarehouseBo.MlreBenefitCode;
            OriSumAssured = riDataWarehouseBo.OriSumAssured;
            CurrSumAssured = riDataWarehouseBo.CurrSumAssured;
            AmountCededB4MlreShare = riDataWarehouseBo.AmountCededB4MlreShare;
            RetentionAmount = riDataWarehouseBo.RetentionAmount;
            AarOri = riDataWarehouseBo.AarOri;
            Aar = riDataWarehouseBo.Aar;
            AarSpecial1 = riDataWarehouseBo.AarSpecial1;
            AarSpecial2 = riDataWarehouseBo.AarSpecial2;
            AarSpecial3 = riDataWarehouseBo.AarSpecial3;
            InsuredName = riDataWarehouseBo.InsuredName;
            InsuredGenderCode = riDataWarehouseBo.InsuredGenderCode;
            InsuredTobaccoUse = riDataWarehouseBo.InsuredTobaccoUse;
            InsuredDateOfBirth = riDataWarehouseBo.InsuredDateOfBirth;
            InsuredOccupationCode = riDataWarehouseBo.InsuredOccupationCode;
            InsuredRegisterNo = riDataWarehouseBo.InsuredRegisterNo;
            InsuredAttainedAge = riDataWarehouseBo.InsuredAttainedAge;
            InsuredNewIcNumber = riDataWarehouseBo.InsuredNewIcNumber;
            InsuredOldIcNumber = riDataWarehouseBo.InsuredOldIcNumber;
            InsuredName2nd = riDataWarehouseBo.InsuredName2nd;
            InsuredGenderCode2nd = riDataWarehouseBo.InsuredGenderCode2nd;
            InsuredTobaccoUse2nd = riDataWarehouseBo.InsuredTobaccoUse2nd;
            InsuredDateOfBirth2nd = riDataWarehouseBo.InsuredDateOfBirth2nd;
            InsuredAttainedAge2nd = riDataWarehouseBo.InsuredAttainedAge2nd;
            InsuredNewIcNumber2nd = riDataWarehouseBo.InsuredNewIcNumber2nd;
            InsuredOldIcNumber2nd = riDataWarehouseBo.InsuredOldIcNumber2nd;
            ReinsuranceIssueAge = riDataWarehouseBo.ReinsuranceIssueAge;
            ReinsuranceIssueAge2nd = riDataWarehouseBo.ReinsuranceIssueAge2nd;
            PolicyTerm = riDataWarehouseBo.PolicyTerm;
            PolicyExpiryDate = riDataWarehouseBo.PolicyExpiryDate;
            DurationYear = riDataWarehouseBo.DurationYear;
            DurationDay = riDataWarehouseBo.DurationDay;
            DurationMonth = riDataWarehouseBo.DurationMonth;
            PremiumCalType = riDataWarehouseBo.PremiumCalType;
            CedantRiRate = riDataWarehouseBo.CedantRiRate;
            RateTable = riDataWarehouseBo.RateTable;
            AgeRatedUp = riDataWarehouseBo.AgeRatedUp;
            DiscountRate = riDataWarehouseBo.DiscountRate;
            LoadingType = riDataWarehouseBo.LoadingType;
            UnderwriterRating = riDataWarehouseBo.UnderwriterRating;
            UnderwriterRatingUnit = riDataWarehouseBo.UnderwriterRatingUnit;
            UnderwriterRatingTerm = riDataWarehouseBo.UnderwriterRatingTerm;
            UnderwriterRating2 = riDataWarehouseBo.UnderwriterRating2;
            UnderwriterRatingUnit2 = riDataWarehouseBo.UnderwriterRatingUnit2;
            UnderwriterRatingTerm2 = riDataWarehouseBo.UnderwriterRatingTerm2;
            UnderwriterRating3 = riDataWarehouseBo.UnderwriterRating3;
            UnderwriterRatingUnit3 = riDataWarehouseBo.UnderwriterRatingUnit3;
            UnderwriterRatingTerm3 = riDataWarehouseBo.UnderwriterRatingTerm3;
            FlatExtraAmount = riDataWarehouseBo.FlatExtraAmount;
            FlatExtraUnit = riDataWarehouseBo.FlatExtraUnit;
            FlatExtraTerm = riDataWarehouseBo.FlatExtraTerm;
            FlatExtraAmount2 = riDataWarehouseBo.FlatExtraAmount2;
            FlatExtraTerm2 = riDataWarehouseBo.FlatExtraTerm2;
            StandardPremium = riDataWarehouseBo.StandardPremium;
            SubstandardPremium = riDataWarehouseBo.SubstandardPremium;
            FlatExtraPremium = riDataWarehouseBo.FlatExtraPremium;
            GrossPremium = riDataWarehouseBo.GrossPremium;
            StandardDiscount = riDataWarehouseBo.StandardDiscount;
            SubstandardDiscount = riDataWarehouseBo.SubstandardDiscount;
            VitalityDiscount = riDataWarehouseBo.VitalityDiscount;
            TotalDiscount = riDataWarehouseBo.TotalDiscount;
            NetPremium = riDataWarehouseBo.NetPremium;
            AnnualRiPrem = riDataWarehouseBo.AnnualRiPrem;
            RiCovPeriod = riDataWarehouseBo.RiCovPeriod;
            AdjBeginDate = riDataWarehouseBo.AdjBeginDate;
            AdjEndDate = riDataWarehouseBo.AdjEndDate;
            PolicyNumberOld = riDataWarehouseBo.PolicyNumberOld;
            PolicyStatusCode = riDataWarehouseBo.PolicyStatusCode;
            PolicyGrossPremium = riDataWarehouseBo.PolicyGrossPremium;
            PolicyStandardPremium = riDataWarehouseBo.PolicyStandardPremium;
            PolicySubstandardPremium = riDataWarehouseBo.PolicySubstandardPremium;
            PolicyTermRemain = riDataWarehouseBo.PolicyTermRemain;
            PolicyAmountDeath = riDataWarehouseBo.PolicyAmountDeath;
            PolicyReserve = riDataWarehouseBo.PolicyReserve;
            PolicyPaymentMethod = riDataWarehouseBo.PolicyPaymentMethod;
            PolicyLifeNumber = riDataWarehouseBo.PolicyLifeNumber;
            FundCode = riDataWarehouseBo.FundCode;
            LineOfBusiness = riDataWarehouseBo.LineOfBusiness;
            ApLoading = riDataWarehouseBo.ApLoading;
            LoanInterestRate = riDataWarehouseBo.LoanInterestRate;
            DefermentPeriod = riDataWarehouseBo.DefermentPeriod;
            RiderNumber = riDataWarehouseBo.RiderNumber;
            CampaignCode = riDataWarehouseBo.CampaignCode;
            Nationality = riDataWarehouseBo.Nationality;
            TerritoryOfIssueCode = riDataWarehouseBo.TerritoryOfIssueCode;
            CurrencyRate = riDataWarehouseBo.CurrencyRate;
            StaffPlanIndicator = riDataWarehouseBo.StaffPlanIndicator;
            CedingTreatyCode = riDataWarehouseBo.CedingTreatyCode;
            CedingPlanCodeOld = riDataWarehouseBo.CedingPlanCodeOld;
            CedingBasicPlanCode = riDataWarehouseBo.CedingBasicPlanCode;
            CedantSar = riDataWarehouseBo.CedantSar;
            CedantReinsurerCode = riDataWarehouseBo.CedantReinsurerCode;
            AmountCededB4MlreShare2 = riDataWarehouseBo.AmountCededB4MlreShare2;
            CessionCode = riDataWarehouseBo.CessionCode;
            CedantRemark = riDataWarehouseBo.CedantRemark;
            GroupPolicyNumber = riDataWarehouseBo.GroupPolicyNumber;
            GroupPolicyName = riDataWarehouseBo.GroupPolicyName;
            NoOfEmployee = riDataWarehouseBo.NoOfEmployee;
            PolicyTotalLive = riDataWarehouseBo.PolicyTotalLive;
            GroupSubsidiaryName = riDataWarehouseBo.GroupSubsidiaryName;
            GroupSubsidiaryNo = riDataWarehouseBo.GroupSubsidiaryNo;
            GroupEmployeeBasicSalary = riDataWarehouseBo.GroupEmployeeBasicSalary;
            GroupEmployeeJobType = riDataWarehouseBo.GroupEmployeeJobType;
            GroupEmployeeJobCode = riDataWarehouseBo.GroupEmployeeJobCode;
            GroupEmployeeBasicSalaryRevise = riDataWarehouseBo.GroupEmployeeBasicSalaryRevise;
            GroupEmployeeBasicSalaryMultiplier = riDataWarehouseBo.GroupEmployeeBasicSalaryMultiplier;
            CedingPlanCode2 = riDataWarehouseBo.CedingPlanCode2;
            DependantIndicator = riDataWarehouseBo.DependantIndicator;
            GhsRoomBoard = riDataWarehouseBo.GhsRoomBoard;
            PolicyAmountSubstandard = riDataWarehouseBo.PolicyAmountSubstandard;
            Layer1RiShare = riDataWarehouseBo.Layer1RiShare;
            Layer1InsuredAttainedAge = riDataWarehouseBo.Layer1InsuredAttainedAge;
            Layer1InsuredAttainedAge2nd = riDataWarehouseBo.Layer1InsuredAttainedAge2nd;
            Layer1StandardPremium = riDataWarehouseBo.Layer1StandardPremium;
            Layer1SubstandardPremium = riDataWarehouseBo.Layer1SubstandardPremium;
            Layer1GrossPremium = riDataWarehouseBo.Layer1GrossPremium;
            Layer1StandardDiscount = riDataWarehouseBo.Layer1StandardDiscount;
            Layer1SubstandardDiscount = riDataWarehouseBo.Layer1SubstandardDiscount;
            Layer1TotalDiscount = riDataWarehouseBo.Layer1TotalDiscount;
            Layer1NetPremium = riDataWarehouseBo.Layer1NetPremium;
            Layer1GrossPremiumAlt = riDataWarehouseBo.Layer1GrossPremiumAlt;
            Layer1TotalDiscountAlt = riDataWarehouseBo.Layer1TotalDiscountAlt;
            Layer1NetPremiumAlt = riDataWarehouseBo.Layer1NetPremiumAlt;
            SpecialIndicator1 = riDataWarehouseBo.SpecialIndicator1;
            SpecialIndicator2 = riDataWarehouseBo.SpecialIndicator2;
            SpecialIndicator3 = riDataWarehouseBo.SpecialIndicator3;
            IndicatorJointLife = riDataWarehouseBo.IndicatorJointLife;
            TaxAmount = riDataWarehouseBo.TaxAmount;
            GstIndicator = riDataWarehouseBo.GstIndicator;
            GstGrossPremium = riDataWarehouseBo.GstGrossPremium;
            GstTotalDiscount = riDataWarehouseBo.GstTotalDiscount;
            GstVitality = riDataWarehouseBo.GstVitality;
            GstAmount = riDataWarehouseBo.GstAmount;
            Mfrs17BasicRider = riDataWarehouseBo.Mfrs17BasicRider;
            Mfrs17CellName = riDataWarehouseBo.Mfrs17CellName;
            Mfrs17TreatyCode = riDataWarehouseBo.Mfrs17TreatyCode;
            LoaCode = riDataWarehouseBo.LoaCode;
            CurrencyCode = riDataWarehouseBo.CurrencyCode;
            NoClaimBonus = riDataWarehouseBo.NoClaimBonus;
            SurrenderValue = riDataWarehouseBo.SurrenderValue;
            DatabaseCommision = riDataWarehouseBo.DatabaseCommision;
            GrossPremiumAlt = riDataWarehouseBo.GrossPremiumAlt;
            NetPremiumAlt = riDataWarehouseBo.NetPremiumAlt;
            Layer1FlatExtraPremium = riDataWarehouseBo.Layer1FlatExtraPremium;
            TransactionPremium = riDataWarehouseBo.TransactionPremium;
            OriginalPremium = riDataWarehouseBo.OriginalPremium;
            TransactionDiscount = riDataWarehouseBo.TransactionDiscount;
            OriginalDiscount = riDataWarehouseBo.OriginalDiscount;
            BrokerageFee = riDataWarehouseBo.BrokerageFee;
            MaxUwRating = riDataWarehouseBo.MaxUwRating;
            RetentionCap = riDataWarehouseBo.RetentionCap;
            AarCap = riDataWarehouseBo.AarCap;
            RiRate = riDataWarehouseBo.RiRate;
            RiRate2 = riDataWarehouseBo.RiRate2;
            AnnuityFactor = riDataWarehouseBo.AnnuityFactor;
            SumAssuredOffered = riDataWarehouseBo.SumAssuredOffered;
            UwRatingOffered = riDataWarehouseBo.UwRatingOffered;
            FlatExtraAmountOffered = riDataWarehouseBo.FlatExtraAmountOffered;
            FlatExtraDuration = riDataWarehouseBo.FlatExtraDuration;
            EffectiveDate = riDataWarehouseBo.EffectiveDate;
            OfferLetterSentDate = riDataWarehouseBo.OfferLetterSentDate;
            RiskPeriodStartDate = riDataWarehouseBo.RiskPeriodStartDate;
            RiskPeriodEndDate = riDataWarehouseBo.RiskPeriodEndDate;
            Mfrs17AnnualCohort = riDataWarehouseBo.Mfrs17AnnualCohort;
            MaxExpiryAge = riDataWarehouseBo.MaxExpiryAge;
            MinIssueAge = riDataWarehouseBo.MinIssueAge;
            MaxIssueAge = riDataWarehouseBo.MaxIssueAge;
            MinAar = riDataWarehouseBo.MinAar;
            MaxAar = riDataWarehouseBo.MaxAar;
            CorridorLimit = riDataWarehouseBo.CorridorLimit;
            Abl = riDataWarehouseBo.Abl;
            RatePerBasisUnit = riDataWarehouseBo.RatePerBasisUnit;
            RiDiscountRate = riDataWarehouseBo.RiDiscountRate;
            LargeSaDiscount = riDataWarehouseBo.LargeSaDiscount;
            GroupSizeDiscount = riDataWarehouseBo.GroupSizeDiscount;
            EwarpNumber = riDataWarehouseBo.EwarpNumber;
            EwarpActionCode = riDataWarehouseBo.EwarpActionCode;
            RetentionShare = riDataWarehouseBo.RetentionShare;
            AarShare = riDataWarehouseBo.AarShare;
            ProfitComm = riDataWarehouseBo.ProfitComm;
            TotalDirectRetroAar = riDataWarehouseBo.TotalDirectRetroAar;
            TotalDirectRetroGrossPremium = riDataWarehouseBo.TotalDirectRetroGrossPremium;
            TotalDirectRetroDiscount = riDataWarehouseBo.TotalDirectRetroDiscount;
            TotalDirectRetroNetPremium = riDataWarehouseBo.TotalDirectRetroNetPremium;
            TotalDirectRetroNoClaimBonus = riDataWarehouseBo.TotalDirectRetroNoClaimBonus;
            TotalDirectRetroDatabaseCommission = riDataWarehouseBo.TotalDirectRetroDatabaseCommission;
            TreatyType = riDataWarehouseBo.TreatyType;
            MaxApLoading = riDataWarehouseBo.MaxApLoading;
            MlreInsuredAttainedAgeAtCurrentMonth = riDataWarehouseBo.MlreInsuredAttainedAgeAtCurrentMonth;
            MlreInsuredAttainedAgeAtPreviousMonth = riDataWarehouseBo.MlreInsuredAttainedAgeAtPreviousMonth;
            InsuredAttainedAgeCheck = riDataWarehouseBo.InsuredAttainedAgeCheck;
            MaxExpiryAgeCheck = riDataWarehouseBo.MaxExpiryAgeCheck;
            MlrePolicyIssueAge = riDataWarehouseBo.MlrePolicyIssueAge;
            PolicyIssueAgeCheck = riDataWarehouseBo.PolicyIssueAgeCheck;
            MinIssueAgeCheck = riDataWarehouseBo.MinIssueAgeCheck;
            MaxIssueAgeCheck = riDataWarehouseBo.MaxIssueAgeCheck;
            MaxUwRatingCheck = riDataWarehouseBo.MaxUwRatingCheck;
            ApLoadingCheck = riDataWarehouseBo.ApLoadingCheck;
            EffectiveDateCheck = riDataWarehouseBo.EffectiveDateCheck;
            MinAarCheck = riDataWarehouseBo.MinAarCheck;
            MaxAarCheck = riDataWarehouseBo.MaxAarCheck;
            CorridorLimitCheck = riDataWarehouseBo.CorridorLimitCheck;
            AblCheck = riDataWarehouseBo.AblCheck;
            RetentionCheck = riDataWarehouseBo.RetentionCheck;
            AarCheck = riDataWarehouseBo.AarCheck;
            MlreStandardPremium = riDataWarehouseBo.MlreStandardPremium;
            MlreSubstandardPremium = riDataWarehouseBo.MlreSubstandardPremium;
            MlreFlatExtraPremium = riDataWarehouseBo.MlreFlatExtraPremium;
            MlreGrossPremium = riDataWarehouseBo.MlreGrossPremium;
            MlreStandardDiscount = riDataWarehouseBo.MlreStandardDiscount;
            MlreSubstandardDiscount = riDataWarehouseBo.MlreSubstandardDiscount;
            MlreLargeSaDiscount = riDataWarehouseBo.MlreLargeSaDiscount;
            MlreGroupSizeDiscount = riDataWarehouseBo.MlreGroupSizeDiscount;
            MlreVitalityDiscount = riDataWarehouseBo.MlreVitalityDiscount;
            MlreTotalDiscount = riDataWarehouseBo.MlreTotalDiscount;
            MlreNetPremium = riDataWarehouseBo.MlreNetPremium;
            NetPremiumCheck = riDataWarehouseBo.NetPremiumCheck;
            ServiceFeePercentage = riDataWarehouseBo.ServiceFeePercentage;
            ServiceFee = riDataWarehouseBo.ServiceFee;
            MlreBrokerageFee = riDataWarehouseBo.MlreBrokerageFee;
            MlreDatabaseCommission = riDataWarehouseBo.MlreDatabaseCommission;
            ValidityDayCheck = riDataWarehouseBo.ValidityDayCheck;
            SumAssuredOfferedCheck = riDataWarehouseBo.SumAssuredOfferedCheck;
            UwRatingCheck = riDataWarehouseBo.UwRatingCheck;
            FlatExtraAmountCheck = riDataWarehouseBo.FlatExtraAmountCheck;
            FlatExtraDurationCheck = riDataWarehouseBo.FlatExtraDurationCheck;
            LastUpdatedDate = riDataWarehouseBo.LastUpdatedDate;
            AarShare2 = riDataWarehouseBo.AarShare2;
            AarCap2 = riDataWarehouseBo.AarCap2;
            WakalahFeePercentage = riDataWarehouseBo.WakalahFeePercentage;
            TreatyNumber = riDataWarehouseBo.TreatyNumber;
            ConflictType = riDataWarehouseBo.ConflictType;

            // Direct Retro
            RetroParty1 = riDataWarehouseBo.RetroParty1;
            RetroParty2 = riDataWarehouseBo.RetroParty2;
            RetroParty3 = riDataWarehouseBo.RetroParty3;
            RetroShare1 = riDataWarehouseBo.RetroShare1;
            RetroShare2 = riDataWarehouseBo.RetroShare2;
            RetroShare3 = riDataWarehouseBo.RetroShare3;
            RetroPremiumSpread1 = riDataWarehouseBo.RetroPremiumSpread1;
            RetroPremiumSpread2 = riDataWarehouseBo.RetroPremiumSpread2;
            RetroPremiumSpread3 = riDataWarehouseBo.RetroPremiumSpread3;
            RetroAar1 = riDataWarehouseBo.RetroAar1;
            RetroAar2 = riDataWarehouseBo.RetroAar2;
            RetroAar3 = riDataWarehouseBo.RetroAar3;
            RetroReinsurancePremium1 = riDataWarehouseBo.RetroReinsurancePremium1;
            RetroReinsurancePremium2 = riDataWarehouseBo.RetroReinsurancePremium2;
            RetroReinsurancePremium3 = riDataWarehouseBo.RetroReinsurancePremium3;
            RetroDiscount1 = riDataWarehouseBo.RetroDiscount1;
            RetroDiscount2 = riDataWarehouseBo.RetroDiscount2;
            RetroDiscount3 = riDataWarehouseBo.RetroDiscount3;
            RetroNetPremium1 = riDataWarehouseBo.RetroNetPremium1;
            RetroNetPremium2 = riDataWarehouseBo.RetroNetPremium2;
            RetroNetPremium3 = riDataWarehouseBo.RetroNetPremium3;
            RetroNoClaimBonus1 = riDataWarehouseBo.RetroNoClaimBonus1;
            RetroNoClaimBonus2 = riDataWarehouseBo.RetroNoClaimBonus2;
            RetroNoClaimBonus3 = riDataWarehouseBo.RetroNoClaimBonus3;
            RetroDatabaseCommission1 = riDataWarehouseBo.RetroDatabaseCommission1;
            RetroDatabaseCommission2 = riDataWarehouseBo.RetroDatabaseCommission2;
            RetroDatabaseCommission3 = riDataWarehouseBo.RetroDatabaseCommission3;
        }

        public void Set(RiDataWarehouseHistoryBo riDataWarehouseHistoryBo)
        {
            Id = riDataWarehouseHistoryBo.Id;

            Quarter = riDataWarehouseHistoryBo.Quarter;
            EndingPolicyStatus = riDataWarehouseHistoryBo.EndingPolicyStatus;
            RecordType = riDataWarehouseHistoryBo.RecordType;
            TreatyCode = riDataWarehouseHistoryBo.TreatyCode;
            ReinsBasisCode = riDataWarehouseHistoryBo.ReinsBasisCode;
            FundsAccountingTypeCode = riDataWarehouseHistoryBo.FundsAccountingTypeCode;
            PremiumFrequencyCode = riDataWarehouseHistoryBo.PremiumFrequencyCode;
            ReportPeriodMonth = riDataWarehouseHistoryBo.ReportPeriodMonth;
            ReportPeriodYear = riDataWarehouseHistoryBo.ReportPeriodYear;
            RiskPeriodMonth = riDataWarehouseHistoryBo.RiskPeriodMonth;
            RiskPeriodYear = riDataWarehouseHistoryBo.RiskPeriodYear;
            TransactionTypeCode = riDataWarehouseHistoryBo.TransactionTypeCode;
            PolicyNumber = riDataWarehouseHistoryBo.PolicyNumber;
            IssueDatePol = riDataWarehouseHistoryBo.IssueDatePol;
            IssueDateBen = riDataWarehouseHistoryBo.IssueDateBen;
            ReinsEffDatePol = riDataWarehouseHistoryBo.ReinsEffDatePol;
            ReinsEffDateBen = riDataWarehouseHistoryBo.ReinsEffDateBen;
            CedingPlanCode = riDataWarehouseHistoryBo.CedingPlanCode;
            CedingBenefitTypeCode = riDataWarehouseHistoryBo.CedingBenefitTypeCode;
            CedingBenefitRiskCode = riDataWarehouseHistoryBo.CedingBenefitRiskCode;
            MlreBenefitCode = riDataWarehouseHistoryBo.MlreBenefitCode;
            OriSumAssured = riDataWarehouseHistoryBo.OriSumAssured;
            CurrSumAssured = riDataWarehouseHistoryBo.CurrSumAssured;
            AmountCededB4MlreShare = riDataWarehouseHistoryBo.AmountCededB4MlreShare;
            RetentionAmount = riDataWarehouseHistoryBo.RetentionAmount;
            AarOri = riDataWarehouseHistoryBo.AarOri;
            Aar = riDataWarehouseHistoryBo.Aar;
            AarSpecial1 = riDataWarehouseHistoryBo.AarSpecial1;
            AarSpecial2 = riDataWarehouseHistoryBo.AarSpecial2;
            AarSpecial3 = riDataWarehouseHistoryBo.AarSpecial3;
            InsuredName = riDataWarehouseHistoryBo.InsuredName;
            InsuredGenderCode = riDataWarehouseHistoryBo.InsuredGenderCode;
            InsuredTobaccoUse = riDataWarehouseHistoryBo.InsuredTobaccoUse;
            InsuredDateOfBirth = riDataWarehouseHistoryBo.InsuredDateOfBirth;
            InsuredOccupationCode = riDataWarehouseHistoryBo.InsuredOccupationCode;
            InsuredRegisterNo = riDataWarehouseHistoryBo.InsuredRegisterNo;
            InsuredAttainedAge = riDataWarehouseHistoryBo.InsuredAttainedAge;
            InsuredNewIcNumber = riDataWarehouseHistoryBo.InsuredNewIcNumber;
            InsuredOldIcNumber = riDataWarehouseHistoryBo.InsuredOldIcNumber;
            InsuredName2nd = riDataWarehouseHistoryBo.InsuredName2nd;
            InsuredGenderCode2nd = riDataWarehouseHistoryBo.InsuredGenderCode2nd;
            InsuredTobaccoUse2nd = riDataWarehouseHistoryBo.InsuredTobaccoUse2nd;
            InsuredDateOfBirth2nd = riDataWarehouseHistoryBo.InsuredDateOfBirth2nd;
            InsuredAttainedAge2nd = riDataWarehouseHistoryBo.InsuredAttainedAge2nd;
            InsuredNewIcNumber2nd = riDataWarehouseHistoryBo.InsuredNewIcNumber2nd;
            InsuredOldIcNumber2nd = riDataWarehouseHistoryBo.InsuredOldIcNumber2nd;
            ReinsuranceIssueAge = riDataWarehouseHistoryBo.ReinsuranceIssueAge;
            ReinsuranceIssueAge2nd = riDataWarehouseHistoryBo.ReinsuranceIssueAge2nd;
            PolicyTerm = riDataWarehouseHistoryBo.PolicyTerm;
            PolicyExpiryDate = riDataWarehouseHistoryBo.PolicyExpiryDate;
            DurationYear = riDataWarehouseHistoryBo.DurationYear;
            DurationDay = riDataWarehouseHistoryBo.DurationDay;
            DurationMonth = riDataWarehouseHistoryBo.DurationMonth;
            PremiumCalType = riDataWarehouseHistoryBo.PremiumCalType;
            CedantRiRate = riDataWarehouseHistoryBo.CedantRiRate;
            RateTable = riDataWarehouseHistoryBo.RateTable;
            AgeRatedUp = riDataWarehouseHistoryBo.AgeRatedUp;
            DiscountRate = riDataWarehouseHistoryBo.DiscountRate;
            LoadingType = riDataWarehouseHistoryBo.LoadingType;
            UnderwriterRating = riDataWarehouseHistoryBo.UnderwriterRating;
            UnderwriterRatingUnit = riDataWarehouseHistoryBo.UnderwriterRatingUnit;
            UnderwriterRatingTerm = riDataWarehouseHistoryBo.UnderwriterRatingTerm;
            UnderwriterRating2 = riDataWarehouseHistoryBo.UnderwriterRating2;
            UnderwriterRatingUnit2 = riDataWarehouseHistoryBo.UnderwriterRatingUnit2;
            UnderwriterRatingTerm2 = riDataWarehouseHistoryBo.UnderwriterRatingTerm2;
            UnderwriterRating3 = riDataWarehouseHistoryBo.UnderwriterRating3;
            UnderwriterRatingUnit3 = riDataWarehouseHistoryBo.UnderwriterRatingUnit3;
            UnderwriterRatingTerm3 = riDataWarehouseHistoryBo.UnderwriterRatingTerm3;
            FlatExtraAmount = riDataWarehouseHistoryBo.FlatExtraAmount;
            FlatExtraUnit = riDataWarehouseHistoryBo.FlatExtraUnit;
            FlatExtraTerm = riDataWarehouseHistoryBo.FlatExtraTerm;
            FlatExtraAmount2 = riDataWarehouseHistoryBo.FlatExtraAmount2;
            FlatExtraTerm2 = riDataWarehouseHistoryBo.FlatExtraTerm2;
            StandardPremium = riDataWarehouseHistoryBo.StandardPremium;
            SubstandardPremium = riDataWarehouseHistoryBo.SubstandardPremium;
            FlatExtraPremium = riDataWarehouseHistoryBo.FlatExtraPremium;
            GrossPremium = riDataWarehouseHistoryBo.GrossPremium;
            StandardDiscount = riDataWarehouseHistoryBo.StandardDiscount;
            SubstandardDiscount = riDataWarehouseHistoryBo.SubstandardDiscount;
            VitalityDiscount = riDataWarehouseHistoryBo.VitalityDiscount;
            TotalDiscount = riDataWarehouseHistoryBo.TotalDiscount;
            NetPremium = riDataWarehouseHistoryBo.NetPremium;
            AnnualRiPrem = riDataWarehouseHistoryBo.AnnualRiPrem;
            RiCovPeriod = riDataWarehouseHistoryBo.RiCovPeriod;
            AdjBeginDate = riDataWarehouseHistoryBo.AdjBeginDate;
            AdjEndDate = riDataWarehouseHistoryBo.AdjEndDate;
            PolicyNumberOld = riDataWarehouseHistoryBo.PolicyNumberOld;
            PolicyStatusCode = riDataWarehouseHistoryBo.PolicyStatusCode;
            PolicyGrossPremium = riDataWarehouseHistoryBo.PolicyGrossPremium;
            PolicyStandardPremium = riDataWarehouseHistoryBo.PolicyStandardPremium;
            PolicySubstandardPremium = riDataWarehouseHistoryBo.PolicySubstandardPremium;
            PolicyTermRemain = riDataWarehouseHistoryBo.PolicyTermRemain;
            PolicyAmountDeath = riDataWarehouseHistoryBo.PolicyAmountDeath;
            PolicyReserve = riDataWarehouseHistoryBo.PolicyReserve;
            PolicyPaymentMethod = riDataWarehouseHistoryBo.PolicyPaymentMethod;
            PolicyLifeNumber = riDataWarehouseHistoryBo.PolicyLifeNumber;
            FundCode = riDataWarehouseHistoryBo.FundCode;
            LineOfBusiness = riDataWarehouseHistoryBo.LineOfBusiness;
            ApLoading = riDataWarehouseHistoryBo.ApLoading;
            LoanInterestRate = riDataWarehouseHistoryBo.LoanInterestRate;
            DefermentPeriod = riDataWarehouseHistoryBo.DefermentPeriod;
            RiderNumber = riDataWarehouseHistoryBo.RiderNumber;
            CampaignCode = riDataWarehouseHistoryBo.CampaignCode;
            Nationality = riDataWarehouseHistoryBo.Nationality;
            TerritoryOfIssueCode = riDataWarehouseHistoryBo.TerritoryOfIssueCode;
            CurrencyRate = riDataWarehouseHistoryBo.CurrencyRate;
            StaffPlanIndicator = riDataWarehouseHistoryBo.StaffPlanIndicator;
            CedingTreatyCode = riDataWarehouseHistoryBo.CedingTreatyCode;
            CedingPlanCodeOld = riDataWarehouseHistoryBo.CedingPlanCodeOld;
            CedingBasicPlanCode = riDataWarehouseHistoryBo.CedingBasicPlanCode;
            CedantSar = riDataWarehouseHistoryBo.CedantSar;
            CedantReinsurerCode = riDataWarehouseHistoryBo.CedantReinsurerCode;
            AmountCededB4MlreShare2 = riDataWarehouseHistoryBo.AmountCededB4MlreShare2;
            CessionCode = riDataWarehouseHistoryBo.CessionCode;
            CedantRemark = riDataWarehouseHistoryBo.CedantRemark;
            GroupPolicyNumber = riDataWarehouseHistoryBo.GroupPolicyNumber;
            GroupPolicyName = riDataWarehouseHistoryBo.GroupPolicyName;
            NoOfEmployee = riDataWarehouseHistoryBo.NoOfEmployee;
            PolicyTotalLive = riDataWarehouseHistoryBo.PolicyTotalLive;
            GroupSubsidiaryName = riDataWarehouseHistoryBo.GroupSubsidiaryName;
            GroupSubsidiaryNo = riDataWarehouseHistoryBo.GroupSubsidiaryNo;
            GroupEmployeeBasicSalary = riDataWarehouseHistoryBo.GroupEmployeeBasicSalary;
            GroupEmployeeJobType = riDataWarehouseHistoryBo.GroupEmployeeJobType;
            GroupEmployeeJobCode = riDataWarehouseHistoryBo.GroupEmployeeJobCode;
            GroupEmployeeBasicSalaryRevise = riDataWarehouseHistoryBo.GroupEmployeeBasicSalaryRevise;
            GroupEmployeeBasicSalaryMultiplier = riDataWarehouseHistoryBo.GroupEmployeeBasicSalaryMultiplier;
            CedingPlanCode2 = riDataWarehouseHistoryBo.CedingPlanCode2;
            DependantIndicator = riDataWarehouseHistoryBo.DependantIndicator;
            GhsRoomBoard = riDataWarehouseHistoryBo.GhsRoomBoard;
            PolicyAmountSubstandard = riDataWarehouseHistoryBo.PolicyAmountSubstandard;
            Layer1RiShare = riDataWarehouseHistoryBo.Layer1RiShare;
            Layer1InsuredAttainedAge = riDataWarehouseHistoryBo.Layer1InsuredAttainedAge;
            Layer1InsuredAttainedAge2nd = riDataWarehouseHistoryBo.Layer1InsuredAttainedAge2nd;
            Layer1StandardPremium = riDataWarehouseHistoryBo.Layer1StandardPremium;
            Layer1SubstandardPremium = riDataWarehouseHistoryBo.Layer1SubstandardPremium;
            Layer1GrossPremium = riDataWarehouseHistoryBo.Layer1GrossPremium;
            Layer1StandardDiscount = riDataWarehouseHistoryBo.Layer1StandardDiscount;
            Layer1SubstandardDiscount = riDataWarehouseHistoryBo.Layer1SubstandardDiscount;
            Layer1TotalDiscount = riDataWarehouseHistoryBo.Layer1TotalDiscount;
            Layer1NetPremium = riDataWarehouseHistoryBo.Layer1NetPremium;
            Layer1GrossPremiumAlt = riDataWarehouseHistoryBo.Layer1GrossPremiumAlt;
            Layer1TotalDiscountAlt = riDataWarehouseHistoryBo.Layer1TotalDiscountAlt;
            Layer1NetPremiumAlt = riDataWarehouseHistoryBo.Layer1NetPremiumAlt;
            SpecialIndicator1 = riDataWarehouseHistoryBo.SpecialIndicator1;
            SpecialIndicator2 = riDataWarehouseHistoryBo.SpecialIndicator2;
            SpecialIndicator3 = riDataWarehouseHistoryBo.SpecialIndicator3;
            IndicatorJointLife = riDataWarehouseHistoryBo.IndicatorJointLife;
            TaxAmount = riDataWarehouseHistoryBo.TaxAmount;
            GstIndicator = riDataWarehouseHistoryBo.GstIndicator;
            GstGrossPremium = riDataWarehouseHistoryBo.GstGrossPremium;
            GstTotalDiscount = riDataWarehouseHistoryBo.GstTotalDiscount;
            GstVitality = riDataWarehouseHistoryBo.GstVitality;
            GstAmount = riDataWarehouseHistoryBo.GstAmount;
            Mfrs17BasicRider = riDataWarehouseHistoryBo.Mfrs17BasicRider;
            Mfrs17CellName = riDataWarehouseHistoryBo.Mfrs17CellName;
            Mfrs17TreatyCode = riDataWarehouseHistoryBo.Mfrs17TreatyCode;
            LoaCode = riDataWarehouseHistoryBo.LoaCode;
            CurrencyCode = riDataWarehouseHistoryBo.CurrencyCode;
            NoClaimBonus = riDataWarehouseHistoryBo.NoClaimBonus;
            SurrenderValue = riDataWarehouseHistoryBo.SurrenderValue;
            DatabaseCommision = riDataWarehouseHistoryBo.DatabaseCommision;
            GrossPremiumAlt = riDataWarehouseHistoryBo.GrossPremiumAlt;
            NetPremiumAlt = riDataWarehouseHistoryBo.NetPremiumAlt;
            Layer1FlatExtraPremium = riDataWarehouseHistoryBo.Layer1FlatExtraPremium;
            TransactionPremium = riDataWarehouseHistoryBo.TransactionPremium;
            OriginalPremium = riDataWarehouseHistoryBo.OriginalPremium;
            TransactionDiscount = riDataWarehouseHistoryBo.TransactionDiscount;
            OriginalDiscount = riDataWarehouseHistoryBo.OriginalDiscount;
            BrokerageFee = riDataWarehouseHistoryBo.BrokerageFee;
            MaxUwRating = riDataWarehouseHistoryBo.MaxUwRating;
            RetentionCap = riDataWarehouseHistoryBo.RetentionCap;
            AarCap = riDataWarehouseHistoryBo.AarCap;
            RiRate = riDataWarehouseHistoryBo.RiRate;
            RiRate2 = riDataWarehouseHistoryBo.RiRate2;
            AnnuityFactor = riDataWarehouseHistoryBo.AnnuityFactor;
            SumAssuredOffered = riDataWarehouseHistoryBo.SumAssuredOffered;
            UwRatingOffered = riDataWarehouseHistoryBo.UwRatingOffered;
            FlatExtraAmountOffered = riDataWarehouseHistoryBo.FlatExtraAmountOffered;
            FlatExtraDuration = riDataWarehouseHistoryBo.FlatExtraDuration;
            EffectiveDate = riDataWarehouseHistoryBo.EffectiveDate;
            OfferLetterSentDate = riDataWarehouseHistoryBo.OfferLetterSentDate;
            RiskPeriodStartDate = riDataWarehouseHistoryBo.RiskPeriodStartDate;
            RiskPeriodEndDate = riDataWarehouseHistoryBo.RiskPeriodEndDate;
            Mfrs17AnnualCohort = riDataWarehouseHistoryBo.Mfrs17AnnualCohort;
            MaxExpiryAge = riDataWarehouseHistoryBo.MaxExpiryAge;
            MinIssueAge = riDataWarehouseHistoryBo.MinIssueAge;
            MaxIssueAge = riDataWarehouseHistoryBo.MaxIssueAge;
            MinAar = riDataWarehouseHistoryBo.MinAar;
            MaxAar = riDataWarehouseHistoryBo.MaxAar;
            CorridorLimit = riDataWarehouseHistoryBo.CorridorLimit;
            Abl = riDataWarehouseHistoryBo.Abl;
            RatePerBasisUnit = riDataWarehouseHistoryBo.RatePerBasisUnit;
            RiDiscountRate = riDataWarehouseHistoryBo.RiDiscountRate;
            LargeSaDiscount = riDataWarehouseHistoryBo.LargeSaDiscount;
            GroupSizeDiscount = riDataWarehouseHistoryBo.GroupSizeDiscount;
            EwarpNumber = riDataWarehouseHistoryBo.EwarpNumber;
            EwarpActionCode = riDataWarehouseHistoryBo.EwarpActionCode;
            RetentionShare = riDataWarehouseHistoryBo.RetentionShare;
            AarShare = riDataWarehouseHistoryBo.AarShare;
            ProfitComm = riDataWarehouseHistoryBo.ProfitComm;
            TotalDirectRetroAar = riDataWarehouseHistoryBo.TotalDirectRetroAar;
            TotalDirectRetroGrossPremium = riDataWarehouseHistoryBo.TotalDirectRetroGrossPremium;
            TotalDirectRetroDiscount = riDataWarehouseHistoryBo.TotalDirectRetroDiscount;
            TotalDirectRetroNetPremium = riDataWarehouseHistoryBo.TotalDirectRetroNetPremium;
            TotalDirectRetroNoClaimBonus = riDataWarehouseHistoryBo.TotalDirectRetroNoClaimBonus;
            TotalDirectRetroDatabaseCommission = riDataWarehouseHistoryBo.TotalDirectRetroDatabaseCommission;
            TreatyType = riDataWarehouseHistoryBo.TreatyType;
            MaxApLoading = riDataWarehouseHistoryBo.MaxApLoading;
            MlreInsuredAttainedAgeAtCurrentMonth = riDataWarehouseHistoryBo.MlreInsuredAttainedAgeAtCurrentMonth;
            MlreInsuredAttainedAgeAtPreviousMonth = riDataWarehouseHistoryBo.MlreInsuredAttainedAgeAtPreviousMonth;
            InsuredAttainedAgeCheck = riDataWarehouseHistoryBo.InsuredAttainedAgeCheck;
            MaxExpiryAgeCheck = riDataWarehouseHistoryBo.MaxExpiryAgeCheck;
            MlrePolicyIssueAge = riDataWarehouseHistoryBo.MlrePolicyIssueAge;
            PolicyIssueAgeCheck = riDataWarehouseHistoryBo.PolicyIssueAgeCheck;
            MinIssueAgeCheck = riDataWarehouseHistoryBo.MinIssueAgeCheck;
            MaxIssueAgeCheck = riDataWarehouseHistoryBo.MaxIssueAgeCheck;
            MaxUwRatingCheck = riDataWarehouseHistoryBo.MaxUwRatingCheck;
            ApLoadingCheck = riDataWarehouseHistoryBo.ApLoadingCheck;
            EffectiveDateCheck = riDataWarehouseHistoryBo.EffectiveDateCheck;
            MinAarCheck = riDataWarehouseHistoryBo.MinAarCheck;
            MaxAarCheck = riDataWarehouseHistoryBo.MaxAarCheck;
            CorridorLimitCheck = riDataWarehouseHistoryBo.CorridorLimitCheck;
            AblCheck = riDataWarehouseHistoryBo.AblCheck;
            RetentionCheck = riDataWarehouseHistoryBo.RetentionCheck;
            AarCheck = riDataWarehouseHistoryBo.AarCheck;
            MlreStandardPremium = riDataWarehouseHistoryBo.MlreStandardPremium;
            MlreSubstandardPremium = riDataWarehouseHistoryBo.MlreSubstandardPremium;
            MlreFlatExtraPremium = riDataWarehouseHistoryBo.MlreFlatExtraPremium;
            MlreGrossPremium = riDataWarehouseHistoryBo.MlreGrossPremium;
            MlreStandardDiscount = riDataWarehouseHistoryBo.MlreStandardDiscount;
            MlreSubstandardDiscount = riDataWarehouseHistoryBo.MlreSubstandardDiscount;
            MlreLargeSaDiscount = riDataWarehouseHistoryBo.MlreLargeSaDiscount;
            MlreGroupSizeDiscount = riDataWarehouseHistoryBo.MlreGroupSizeDiscount;
            MlreVitalityDiscount = riDataWarehouseHistoryBo.MlreVitalityDiscount;
            MlreTotalDiscount = riDataWarehouseHistoryBo.MlreTotalDiscount;
            MlreNetPremium = riDataWarehouseHistoryBo.MlreNetPremium;
            NetPremiumCheck = riDataWarehouseHistoryBo.NetPremiumCheck;
            ServiceFeePercentage = riDataWarehouseHistoryBo.ServiceFeePercentage;
            ServiceFee = riDataWarehouseHistoryBo.ServiceFee;
            MlreBrokerageFee = riDataWarehouseHistoryBo.MlreBrokerageFee;
            MlreDatabaseCommission = riDataWarehouseHistoryBo.MlreDatabaseCommission;
            ValidityDayCheck = riDataWarehouseHistoryBo.ValidityDayCheck;
            SumAssuredOfferedCheck = riDataWarehouseHistoryBo.SumAssuredOfferedCheck;
            UwRatingCheck = riDataWarehouseHistoryBo.UwRatingCheck;
            FlatExtraAmountCheck = riDataWarehouseHistoryBo.FlatExtraAmountCheck;
            FlatExtraDurationCheck = riDataWarehouseHistoryBo.FlatExtraDurationCheck;
            LastUpdatedDate = riDataWarehouseHistoryBo.LastUpdatedDate;
            AarShare2 = riDataWarehouseHistoryBo.AarShare2;
            AarCap2 = riDataWarehouseHistoryBo.AarCap2;
            WakalahFeePercentage = riDataWarehouseHistoryBo.WakalahFeePercentage;
            TreatyNumber = riDataWarehouseHistoryBo.TreatyNumber;
            ConflictType = riDataWarehouseHistoryBo.ConflictType;

            // Direct Retro
            RetroParty1 = riDataWarehouseHistoryBo.RetroParty1;
            RetroParty2 = riDataWarehouseHistoryBo.RetroParty2;
            RetroParty3 = riDataWarehouseHistoryBo.RetroParty3;
            RetroShare1 = riDataWarehouseHistoryBo.RetroShare1;
            RetroShare2 = riDataWarehouseHistoryBo.RetroShare2;
            RetroShare3 = riDataWarehouseHistoryBo.RetroShare3;
            RetroPremiumSpread1 = riDataWarehouseHistoryBo.RetroPremiumSpread1;
            RetroPremiumSpread2 = riDataWarehouseHistoryBo.RetroPremiumSpread2;
            RetroPremiumSpread3 = riDataWarehouseHistoryBo.RetroPremiumSpread3;
            RetroAar1 = riDataWarehouseHistoryBo.RetroAar1;
            RetroAar2 = riDataWarehouseHistoryBo.RetroAar2;
            RetroAar3 = riDataWarehouseHistoryBo.RetroAar3;
            RetroReinsurancePremium1 = riDataWarehouseHistoryBo.RetroReinsurancePremium1;
            RetroReinsurancePremium2 = riDataWarehouseHistoryBo.RetroReinsurancePremium2;
            RetroReinsurancePremium3 = riDataWarehouseHistoryBo.RetroReinsurancePremium3;
            RetroDiscount1 = riDataWarehouseHistoryBo.RetroDiscount1;
            RetroDiscount2 = riDataWarehouseHistoryBo.RetroDiscount2;
            RetroDiscount3 = riDataWarehouseHistoryBo.RetroDiscount3;
            RetroNetPremium1 = riDataWarehouseHistoryBo.RetroNetPremium1;
            RetroNetPremium2 = riDataWarehouseHistoryBo.RetroNetPremium2;
            RetroNetPremium3 = riDataWarehouseHistoryBo.RetroNetPremium3;
            RetroNoClaimBonus1 = riDataWarehouseHistoryBo.RetroNoClaimBonus1;
            RetroNoClaimBonus2 = riDataWarehouseHistoryBo.RetroNoClaimBonus2;
            RetroNoClaimBonus3 = riDataWarehouseHistoryBo.RetroNoClaimBonus3;
            RetroDatabaseCommission1 = riDataWarehouseHistoryBo.RetroDatabaseCommission1;
            RetroDatabaseCommission2 = riDataWarehouseHistoryBo.RetroDatabaseCommission2;
            RetroDatabaseCommission3 = riDataWarehouseHistoryBo.RetroDatabaseCommission3;
        }

        public static Expression<Func<RiDataWarehouse, RiDataWarehouseViewModel>> Expression()
        {
            return entity => new RiDataWarehouseViewModel
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                Quarter = entity.Quarter,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                CedingPlanCode = entity.CedingPlanCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
            };
        }

        public static Expression<Func<RiDataWarehouseHistory, RiDataWarehouseViewModel>> HistoryExpression()
        {
            return entity => new RiDataWarehouseViewModel
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                TreatyCode = entity.TreatyCode,
                Quarter = entity.Quarter,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                CedingPlanCode = entity.CedingPlanCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                FundsAccountingTypeCode = entity.FundsAccountingTypeCode,
                PremiumFrequencyCode = entity.PremiumFrequencyCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                TransactionTypeCode = entity.TransactionTypeCode,
                PolicyNumber = entity.PolicyNumber,
                IssueDatePol = entity.IssueDatePol,
                IssueDateBen = entity.IssueDateBen,
                ReinsEffDatePol = entity.ReinsEffDatePol,
            };
        }
    }
}