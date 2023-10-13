using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataWarehouseHistories")]
    public class RiDataWarehouseHistory
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Index]
        public int RiDataWarehouseId { get; set; }
        [ExcludeTrail]
        public virtual RiDataWarehouse RiDataWarehouse { get; set; }

        [Index]
        public int? EndingPolicyStatus { get; set; }

        //[ExcludeTrail]
        //public virtual PickListDetail EndingPolicyStatusPickListDetail { get; set; }

        [Index]
        public int RecordType { get; set; }

        [MaxLength(64), Index]
        public string Quarter { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyCode { get; set; }

        [MaxLength(30)]
        public string ReinsBasisCode { get; set; }

        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        [MaxLength(10)]
        [Index]
        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        [Index]
        public int? RiskPeriodMonth { get; set; }

        [Index]
        public int? RiskPeriodYear { get; set; }

        [MaxLength(2)]
        [Index]
        public string TransactionTypeCode { get; set; }

        [MaxLength(150)]
        [Index]
        public string PolicyNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? IssueDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? IssueDateBen { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDateBen { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingPlanCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(50)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(10)]
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

        [MaxLength(128)]
        [Index]
        public string InsuredName { get; set; }

        [MaxLength(1)]
        public string InsuredGenderCode { get; set; }

        [MaxLength(1)]
        public string InsuredTobaccoUse { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(5)]
        public string InsuredOccupationCode { get; set; }

        [MaxLength(30)]
        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        [MaxLength(15)]
        public string InsuredNewIcNumber { get; set; }

        [MaxLength(15)]
        public string InsuredOldIcNumber { get; set; }

        [MaxLength(128)]
        public string InsuredName2nd { get; set; }

        [MaxLength(1)]
        public string InsuredGenderCode2nd { get; set; }

        [MaxLength(1)]
        public string InsuredTobaccoUse2nd { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth2nd { get; set; }

        public int? InsuredAttainedAge2nd { get; set; }

        [MaxLength(15)]
        public string InsuredNewIcNumber2nd { get; set; }

        [MaxLength(15)]
        public string InsuredOldIcNumber2nd { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public int? ReinsuranceIssueAge2nd { get; set; }

        public double? PolicyTerm { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PolicyExpiryDate { get; set; }

        public double? DurationYear { get; set; }

        public double? DurationDay { get; set; }

        public double? DurationMonth { get; set; }

        [MaxLength(5)]
        public string PremiumCalType { get; set; }

        public double? CedantRiRate { get; set; }

        [MaxLength(128)]
        public string RateTable { get; set; }

        public int? AgeRatedUp { get; set; }

        public double? DiscountRate { get; set; }

        [MaxLength(15)]
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

        [Column(TypeName = "datetime2")]
        public DateTime? AdjBeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AdjEndDate { get; set; }

        [MaxLength(150)]
        public string PolicyNumberOld { get; set; }

        [MaxLength(20)]
        public string PolicyStatusCode { get; set; }

        public double? PolicyGrossPremium { get; set; }

        public double? PolicyStandardPremium { get; set; }

        public double? PolicySubstandardPremium { get; set; }

        public double? PolicyTermRemain { get; set; }

        public double? PolicyAmountDeath { get; set; }

        public double? PolicyReserve { get; set; }

        [MaxLength(10)]
        public string PolicyPaymentMethod { get; set; }

        public int? PolicyLifeNumber { get; set; }

        [MaxLength(25)]
        public string FundCode { get; set; }

        [MaxLength(5)]
        public string LineOfBusiness { get; set; }

        public double? ApLoading { get; set; }

        public double? LoanInterestRate { get; set; }

        public int? DefermentPeriod { get; set; }

        public int? RiderNumber { get; set; }

        [MaxLength(10)]
        public string CampaignCode { get; set; }

        [MaxLength(20)]
        public string Nationality { get; set; }

        [MaxLength(20)]
        public string TerritoryOfIssueCode { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        [MaxLength(1)]
        public string StaffPlanIndicator { get; set; }

        [MaxLength(30)]
        public string CedingTreatyCode { get; set; }

        [MaxLength(30)]
        public string CedingPlanCodeOld { get; set; }

        [MaxLength(30)]
        public string CedingBasicPlanCode { get; set; }

        public double? CedantSar { get; set; }

        [MaxLength(10)]
        public string CedantReinsurerCode { get; set; }

        public double? AmountCededB4MlreShare2 { get; set; }

        [MaxLength(10)]
        public string CessionCode { get; set; }

        [MaxLength(255)]
        public string CedantRemark { get; set; }

        [MaxLength(30)]
        public string GroupPolicyNumber { get; set; }

        [MaxLength(128)]
        public string GroupPolicyName { get; set; }

        public int? NoOfEmployee { get; set; }

        public int? PolicyTotalLive { get; set; }

        [MaxLength(128)]
        public string GroupSubsidiaryName { get; set; }

        [MaxLength(30)]
        public string GroupSubsidiaryNo { get; set; }

        public double? GroupEmployeeBasicSalary { get; set; }

        [MaxLength(10)]
        public string GroupEmployeeJobType { get; set; }

        [MaxLength(10)]
        public string GroupEmployeeJobCode { get; set; }

        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        [MaxLength(30)]
        public string CedingPlanCode2 { get; set; }

        [MaxLength(2)]
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

        [MaxLength(1)]
        public string IndicatorJointLife { get; set; }

        public double? TaxAmount { get; set; }

        [MaxLength(3)]
        public string GstIndicator { get; set; }

        public double? GstGrossPremium { get; set; }

        public double? GstTotalDiscount { get; set; }

        public double? GstVitality { get; set; }

        public double? GstAmount { get; set; }

        [MaxLength(5)]
        public string Mfrs17BasicRider { get; set; }

        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [MaxLength(25)]
        public string Mfrs17TreatyCode { get; set; }

        [MaxLength(20)]
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

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OfferLetterSentDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskPeriodStartDate { get; set; }

        [Column(TypeName = "datetime2")]
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

        [MaxLength(10)]
        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        [MaxLength(1)]
        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

        public double? TotalDirectRetroNoClaimBonus { get; set; }

        public double? TotalDirectRetroDatabaseCommission { get; set; }

        [MaxLength(20)]
        public string TreatyType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

        [MaxLength(128), Index]
        public string RetroParty1 { get; set; }

        [MaxLength(128), Index]
        public string RetroParty2 { get; set; }

        [MaxLength(128), Index]
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

        public double? MaxApLoading { get; set; }

        public int? MlreInsuredAttainedAgeAtCurrentMonth { get; set; }

        public int? MlreInsuredAttainedAgeAtPreviousMonth { get; set; }

        public bool? InsuredAttainedAgeCheck { get; set; } = null;

        public bool? MaxExpiryAgeCheck { get; set; } = null;

        public int? MlrePolicyIssueAge { get; set; }

        public bool? PolicyIssueAgeCheck { get; set; } = null;

        public bool? MinIssueAgeCheck { get; set; } = null;

        public bool? MaxIssueAgeCheck { get; set; } = null;

        public bool? MaxUwRatingCheck { get; set; } = null;

        public bool? ApLoadingCheck { get; set; } = null;

        public bool? EffectiveDateCheck { get; set; } = null;

        public bool? MinAarCheck { get; set; } = null;

        public bool? MaxAarCheck { get; set; } = null;

        public bool? CorridorLimitCheck { get; set; } = null;

        public bool? AblCheck { get; set; } = null;

        public bool? RetentionCheck { get; set; } = null;

        public bool? AarCheck { get; set; } = null;

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

        public bool? ValidityDayCheck { get; set; } = null;

        public bool? SumAssuredOfferedCheck { get; set; } = null;

        public bool? UwRatingCheck { get; set; } = null;

        public bool? FlatExtraAmountCheck { get; set; } = null;

        public bool? FlatExtraDurationCheck { get; set; } = null;

        //[Index]
        public double? AarShare2 { get; set; }

        //[Index]
        public double? AarCap2 { get; set; }

        //[Index]
        public double? WakalahFeePercentage { get; set; }

        [MaxLength(128)] //, Index
        public string TreatyNumber { get; set; }

        [Index]
        public int ConflictType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public static RiDataWarehouseHistory Find(int id, int cutOffId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouseHistories.Where(q => q.Id == id && q.CutOffId == cutOffId).FirstOrDefault();
            }
        }

        public static string Script(int cutOffId, int startId, int take)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[RiDataWarehouseHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},[Id],{1} FROM [dbo].[RiDataWarehouse]", cutOffId, string.Join(",", fields.Skip(2).ToArray()));
            string where = string.Format("WHERE Id >= {0} AND Id < {1}", startId, (startId + take));

            return string.Format("{0}\n{1}\n{2}\n{3};", script, field, select, where);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[RiDataWarehouseId]",
                "[EndingPolicyStatus]",
                "[RecordType]",
                "[Quarter]",
                "[TreatyCode]",
                "[ReinsBasisCode]",
                "[FundsAccountingTypeCode]",
                "[PremiumFrequencyCode]",
                "[ReportPeriodMonth]",
                "[ReportPeriodYear]",
                "[RiskPeriodMonth]",
                "[RiskPeriodYear]",
                "[TransactionTypeCode]",
                "[PolicyNumber]",
                "[IssueDatePol]",
                "[IssueDateBen]",
                "[ReinsEffDatePol]",
                "[ReinsEffDateBen]",
                "[CedingPlanCode]",
                "[CedingBenefitTypeCode]",
                "[CedingBenefitRiskCode]",
                "[MlreBenefitCode]",
                "[OriSumAssured]",
                "[CurrSumAssured]",
                "[AmountCededB4MlreShare]",
                "[RetentionAmount]",
                "[AarOri]",
                "[Aar]",
                "[AarSpecial1]",
                "[AarSpecial2]",
                "[AarSpecial3]",
                "[InsuredName]",
                "[InsuredGenderCode]",
                "[InsuredTobaccoUse]",
                "[InsuredDateOfBirth]",
                "[InsuredOccupationCode]",
                "[InsuredRegisterNo]",
                "[InsuredAttainedAge]",
                "[InsuredNewIcNumber]",
                "[InsuredOldIcNumber]",
                "[InsuredName2nd]",
                "[InsuredGenderCode2nd]",
                "[InsuredTobaccoUse2nd]",
                "[InsuredDateOfBirth2nd]",
                "[InsuredAttainedAge2nd]",
                "[InsuredNewIcNumber2nd]",
                "[InsuredOldIcNumber2nd]",
                "[ReinsuranceIssueAge]",
                "[ReinsuranceIssueAge2nd]",
                "[PolicyTerm]",
                "[PolicyExpiryDate]",
                "[DurationYear]",
                "[DurationDay]",
                "[DurationMonth]",
                "[PremiumCalType]",
                "[CedantRiRate]",
                "[RateTable]",
                "[AgeRatedUp]",
                "[DiscountRate]",
                "[LoadingType]",
                "[UnderwriterRating]",
                "[UnderwriterRatingUnit]",
                "[UnderwriterRatingTerm]",
                "[UnderwriterRating2]",
                "[UnderwriterRatingUnit2]",
                "[UnderwriterRatingTerm2]",
                "[UnderwriterRating3]",
                "[UnderwriterRatingUnit3]",
                "[UnderwriterRatingTerm3]",
                "[FlatExtraAmount]",
                "[FlatExtraUnit]",
                "[FlatExtraTerm]",
                "[FlatExtraAmount2]",
                "[FlatExtraTerm2]",
                "[StandardPremium]",
                "[SubstandardPremium]",
                "[FlatExtraPremium]",
                "[GrossPremium]",
                "[StandardDiscount]",
                "[SubstandardDiscount]",
                "[VitalityDiscount]",
                "[TotalDiscount]",
                "[NetPremium]",
                "[AnnualRiPrem]",
                "[RiCovPeriod]",
                "[AdjBeginDate]",
                "[AdjEndDate]",
                "[PolicyNumberOld]",
                "[PolicyStatusCode]",
                "[PolicyGrossPremium]",
                "[PolicyStandardPremium]",
                "[PolicySubstandardPremium]",
                "[PolicyTermRemain]",
                "[PolicyAmountDeath]",
                "[PolicyReserve]",
                "[PolicyPaymentMethod]",
                "[PolicyLifeNumber]",
                "[FundCode]",
                "[LineOfBusiness]",
                "[ApLoading]",
                "[LoanInterestRate]",
                "[DefermentPeriod]",
                "[RiderNumber]",
                "[CampaignCode]",
                "[Nationality]",
                "[TerritoryOfIssueCode]",
                "[CurrencyCode]",
                "[StaffPlanIndicator]",
                "[CedingTreatyCode]",
                "[CedingPlanCodeOld]",
                "[CedingBasicPlanCode]",
                "[CedantSar]",
                "[CedantReinsurerCode]",
                "[AmountCededB4MlreShare2]",
                "[CessionCode]",
                "[CedantRemark]",
                "[GroupPolicyNumber]",
                "[GroupPolicyName]",
                "[NoOfEmployee]",
                "[PolicyTotalLive]",
                "[GroupSubsidiaryName]",
                "[GroupSubsidiaryNo]",
                "[GroupEmployeeBasicSalary]",
                "[GroupEmployeeJobType]",
                "[GroupEmployeeJobCode]",
                "[GroupEmployeeBasicSalaryRevise]",
                "[GroupEmployeeBasicSalaryMultiplier]",
                "[CedingPlanCode2]",
                "[DependantIndicator]",
                "[GhsRoomBoard]",
                "[PolicyAmountSubstandard]",
                "[Layer1RiShare]",
                "[Layer1InsuredAttainedAge]",
                "[Layer1InsuredAttainedAge2nd]",
                "[Layer1StandardPremium]",
                "[Layer1SubstandardPremium]",
                "[Layer1GrossPremium]",
                "[Layer1StandardDiscount]",
                "[Layer1SubstandardDiscount]",
                "[Layer1TotalDiscount]",
                "[Layer1NetPremium]",
                "[Layer1GrossPremiumAlt]",
                "[Layer1TotalDiscountAlt]",
                "[Layer1NetPremiumAlt]",
                "[SpecialIndicator1]",
                "[SpecialIndicator2]",
                "[SpecialIndicator3]",
                "[IndicatorJointLife]",
                "[TaxAmount]",
                "[GstIndicator]",
                "[GstGrossPremium]",
                "[GstTotalDiscount]",
                "[GstVitality]",
                "[GstAmount]",
                "[Mfrs17BasicRider]",
                "[Mfrs17CellName]",
                "[Mfrs17TreatyCode]",
                "[LoaCode]",
                "[CurrencyRate]",
                "[NoClaimBonus]",
                "[SurrenderValue]",
                "[DatabaseCommision]",
                "[GrossPremiumAlt]",
                "[NetPremiumAlt]",
                "[Layer1FlatExtraPremium]",
                "[TransactionPremium]",
                "[OriginalPremium]",
                "[TransactionDiscount]",
                "[OriginalDiscount]",
                "[BrokerageFee]",
                "[MaxUwRating]",
                "[RetentionCap]",
                "[AarCap]",
                "[RiRate]",
                "[RiRate2]",
                "[AnnuityFactor]",
                "[SumAssuredOffered]",
                "[UwRatingOffered]",
                "[FlatExtraAmountOffered]",
                "[FlatExtraDuration]",
                "[EffectiveDate]",
                "[OfferLetterSentDate]",
                "[RiskPeriodStartDate]",
                "[RiskPeriodEndDate]",
                "[Mfrs17AnnualCohort]",
                "[MaxExpiryAge]",
                "[MinIssueAge]",
                "[MaxIssueAge]",
                "[MinAar]",
                "[MaxAar]",
                "[CorridorLimit]",
                "[Abl]",
                "[RatePerBasisUnit]",
                "[RiDiscountRate]",
                "[LargeSaDiscount]",
                "[GroupSizeDiscount]",
                "[EwarpNumber]",
                "[EwarpActionCode]",
                "[RetentionShare]",
                "[AarShare]",
                "[ProfitComm]",
                "[TotalDirectRetroAar]",
                "[TotalDirectRetroGrossPremium]",
                "[TotalDirectRetroDiscount]",
                "[TotalDirectRetroNetPremium]",
                "[TotalDirectRetroNoClaimBonus]",
                "[TotalDirectRetroDatabaseCommission]",
                "[TreatyType]",
                "[LastUpdatedDate]",
                "[RetroParty1]",
                "[RetroParty2]",
                "[RetroParty3]",
                "[RetroShare1]",
                "[RetroShare2]",
                "[RetroShare3]",
                "[RetroPremiumSpread1]",
                "[RetroPremiumSpread2]",
                "[RetroPremiumSpread3]",
                "[RetroAar1]",
                "[RetroAar2]",
                "[RetroAar3]",
                "[RetroReinsurancePremium1]",
                "[RetroReinsurancePremium2]",
                "[RetroReinsurancePremium3]",
                "[RetroDiscount1]",
                "[RetroDiscount2]",
                "[RetroDiscount3]",
                "[RetroNetPremium1]",
                "[RetroNetPremium2]",
                "[RetroNetPremium3]",
                "[RetroNoClaimBonus1]",
                "[RetroNoClaimBonus2]",
                "[RetroNoClaimBonus3]",
                "[RetroDatabaseCommission1]",
                "[RetroDatabaseCommission2]",
                "[RetroDatabaseCommission3]",
                "[MaxApLoading]",
                "[MlreInsuredAttainedAgeAtCurrentMonth]",
                "[MlreInsuredAttainedAgeAtPreviousMonth]",
                "[InsuredAttainedAgeCheck]",
                "[MaxExpiryAgeCheck]",
                "[MlrePolicyIssueAge]",
                "[PolicyIssueAgeCheck]",
                "[MinIssueAgeCheck]",
                "[MaxIssueAgeCheck]",
                "[MaxUwRatingCheck]",
                "[ApLoadingCheck]",
                "[EffectiveDateCheck]",
                "[MinAarCheck]",
                "[MaxAarCheck]",
                "[CorridorLimitCheck]",
                "[AblCheck]",
                "[RetentionCheck]",
                "[AarCheck]",
                "[MlreStandardPremium]",
                "[MlreSubstandardPremium]",
                "[MlreFlatExtraPremium]",
                "[MlreGrossPremium]",
                "[MlreStandardDiscount]",
                "[MlreSubstandardDiscount]",
                "[MlreLargeSaDiscount]",
                "[MlreGroupSizeDiscount]",
                "[MlreVitalityDiscount]",
                "[MlreTotalDiscount]",
                "[MlreNetPremium]",
                "[NetPremiumCheck]",
                "[ServiceFeePercentage]",
                "[ServiceFee]",
                "[MlreBrokerageFee]",
                "[MlreDatabaseCommission]",
                "[ValidityDayCheck]",
                "[SumAssuredOfferedCheck]",
                "[UwRatingCheck]",
                "[FlatExtraAmountCheck]",
                "[FlatExtraDurationCheck]",
                "[AarShare2]",
                "[AarCap2]",
                "[WakalahFeePercentage]",
                "[TreatyNumber]",
                "[ConflictType]",
                "[CreatedAt]",
                "[UpdatedAt]",
                "[CreatedById]",
                "[UpdatedById]",
            };
        }
    }
}
