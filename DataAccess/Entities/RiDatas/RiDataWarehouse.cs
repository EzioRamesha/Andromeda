using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataWarehouse")]
    public class RiDataWarehouse
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int? EndingPolicyStatus { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail EndingPolicyStatusPickListDetail { get; set; }

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

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

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

        // Direct Retro
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

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        [MaxLength(128)]
        public string TreatyNumber { get; set; }

        [Index]
        public int ConflictType { get; set; }

        public RiDataWarehouse()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiDataWarehouse.Any(q => q.Id == id);
            }
        }

        public static RiDataWarehouse Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(711, "RiDataWarehouse");

                return connectionStrategy.Execute(() =>
                {
                    return db.RiDataWarehouse.Where(q => q.Id == id).FirstOrDefault();
                });
            }
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
            using (var db = new AppDbContext())
            {
                return QueryByLookupParams(
                    db,
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
                ).Count();
            }
        }

        public static RiDataWarehouse FindByLookupParams(
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
            using (var db = new AppDbContext())
            {
                return QueryByLookupParams(
                    db,
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
                ).FirstOrDefault();
            }
        }

        public static IQueryable<RiDataWarehouse> QueryByLookupParams(
            AppDbContext db,
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
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(818, "RiDataWarehouse");

            return connectionStrategy.Execute(() =>
            {
                var query = db.RiDataWarehouse
                    .Where(q => q.PolicyNumber == policyNumber)
                    .Where(q => q.CedingPlanCode == cedingPlanCode)
                    .Where(q => q.MlreBenefitCode == mlreBenefitCode)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodMonth == riskPeriodMonth)
                    .Where(q => q.RiskPeriodYear == riskPeriodYear)
                    .Where(q => q.InsuredName == insuredName)
                    .Where(q => q.CedingBenefitTypeCode == cedingBenefitTypeCode)
                    .Where(q => q.CedingBenefitRiskCode == cedingBenefitRiskCode)
                    .Where(q => q.CedingPlanCode2 == cedingPlanCode2)
                    .Where(q => q.CessionCode == cessionCode);

                if (riderNumber.HasValue)
                {
                    connectionStrategy.Reset(828);
                    query = query.Where(q => q.RiderNumber == riderNumber);
                }

                return query;
            });
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.RiDataWarehouse.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                RiDataWarehouse riDataWarehouse = Find(Id);
                if (riDataWarehouse == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataWarehouse, this);

                riDataWarehouse.EndingPolicyStatus = EndingPolicyStatus;
                riDataWarehouse.RecordType = RecordType;
                riDataWarehouse.Quarter = Quarter;
                riDataWarehouse.TreatyCode = TreatyCode;
                riDataWarehouse.ReinsBasisCode = ReinsBasisCode;
                riDataWarehouse.FundsAccountingTypeCode = FundsAccountingTypeCode;
                riDataWarehouse.PremiumFrequencyCode = PremiumFrequencyCode;
                riDataWarehouse.ReportPeriodMonth = ReportPeriodMonth;
                riDataWarehouse.ReportPeriodYear = ReportPeriodYear;
                riDataWarehouse.RiskPeriodMonth = RiskPeriodMonth;
                riDataWarehouse.RiskPeriodYear = RiskPeriodYear;
                riDataWarehouse.TransactionTypeCode = TransactionTypeCode;
                riDataWarehouse.PolicyNumber = PolicyNumber;
                riDataWarehouse.IssueDatePol = IssueDatePol;
                riDataWarehouse.IssueDateBen = IssueDateBen;
                riDataWarehouse.ReinsEffDatePol = ReinsEffDatePol;
                riDataWarehouse.ReinsEffDateBen = ReinsEffDateBen;
                riDataWarehouse.CedingPlanCode = CedingPlanCode;
                riDataWarehouse.CedingBenefitTypeCode = CedingBenefitTypeCode;
                riDataWarehouse.CedingBenefitRiskCode = CedingBenefitRiskCode;
                riDataWarehouse.MlreBenefitCode = MlreBenefitCode;
                riDataWarehouse.OriSumAssured = OriSumAssured;
                riDataWarehouse.CurrSumAssured = CurrSumAssured;
                riDataWarehouse.AmountCededB4MlreShare = AmountCededB4MlreShare;
                riDataWarehouse.RetentionAmount = RetentionAmount;
                riDataWarehouse.AarOri = AarOri;
                riDataWarehouse.Aar = Aar;
                riDataWarehouse.AarSpecial1 = AarSpecial1;
                riDataWarehouse.AarSpecial2 = AarSpecial2;
                riDataWarehouse.AarSpecial3 = AarSpecial3;
                riDataWarehouse.InsuredName = InsuredName;
                riDataWarehouse.InsuredGenderCode = InsuredGenderCode;
                riDataWarehouse.InsuredTobaccoUse = InsuredTobaccoUse;
                riDataWarehouse.InsuredDateOfBirth = InsuredDateOfBirth;
                riDataWarehouse.InsuredOccupationCode = InsuredOccupationCode;
                riDataWarehouse.InsuredRegisterNo = InsuredRegisterNo;
                riDataWarehouse.InsuredAttainedAge = InsuredAttainedAge;
                riDataWarehouse.InsuredNewIcNumber = InsuredNewIcNumber;
                riDataWarehouse.InsuredOldIcNumber = InsuredOldIcNumber;
                riDataWarehouse.InsuredName2nd = InsuredName2nd;
                riDataWarehouse.InsuredGenderCode2nd = InsuredGenderCode2nd;
                riDataWarehouse.InsuredTobaccoUse2nd = InsuredTobaccoUse2nd;
                riDataWarehouse.InsuredDateOfBirth2nd = InsuredDateOfBirth2nd;
                riDataWarehouse.InsuredAttainedAge2nd = InsuredAttainedAge2nd;
                riDataWarehouse.InsuredNewIcNumber2nd = InsuredNewIcNumber2nd;
                riDataWarehouse.InsuredOldIcNumber2nd = InsuredOldIcNumber2nd;
                riDataWarehouse.ReinsuranceIssueAge = ReinsuranceIssueAge;
                riDataWarehouse.ReinsuranceIssueAge2nd = ReinsuranceIssueAge2nd;
                riDataWarehouse.PolicyTerm = PolicyTerm;
                riDataWarehouse.PolicyExpiryDate = PolicyExpiryDate;
                riDataWarehouse.DurationYear = DurationYear;
                riDataWarehouse.DurationDay = DurationDay;
                riDataWarehouse.DurationMonth = DurationMonth;
                riDataWarehouse.PremiumCalType = PremiumCalType;
                riDataWarehouse.CedantRiRate = CedantRiRate;
                riDataWarehouse.RateTable = RateTable;
                riDataWarehouse.AgeRatedUp = AgeRatedUp;
                riDataWarehouse.DiscountRate = DiscountRate;
                riDataWarehouse.LoadingType = LoadingType;
                riDataWarehouse.UnderwriterRating = UnderwriterRating;
                riDataWarehouse.UnderwriterRatingUnit = UnderwriterRatingUnit;
                riDataWarehouse.UnderwriterRatingTerm = UnderwriterRatingTerm;
                riDataWarehouse.UnderwriterRating2 = UnderwriterRating2;
                riDataWarehouse.UnderwriterRatingUnit2 = UnderwriterRatingUnit2;
                riDataWarehouse.UnderwriterRatingTerm2 = UnderwriterRatingTerm2;
                riDataWarehouse.UnderwriterRating3 = UnderwriterRating3;
                riDataWarehouse.UnderwriterRatingUnit3 = UnderwriterRatingUnit3;
                riDataWarehouse.UnderwriterRatingTerm3 = UnderwriterRatingTerm3;
                riDataWarehouse.FlatExtraAmount = FlatExtraAmount;
                riDataWarehouse.FlatExtraUnit = FlatExtraUnit;
                riDataWarehouse.FlatExtraTerm = FlatExtraTerm;
                riDataWarehouse.FlatExtraAmount2 = FlatExtraAmount2;
                riDataWarehouse.FlatExtraTerm2 = FlatExtraTerm2;
                riDataWarehouse.StandardPremium = StandardPremium;
                riDataWarehouse.SubstandardPremium = SubstandardPremium;
                riDataWarehouse.FlatExtraPremium = FlatExtraPremium;
                riDataWarehouse.GrossPremium = GrossPremium;
                riDataWarehouse.StandardDiscount = StandardDiscount;
                riDataWarehouse.SubstandardDiscount = SubstandardDiscount;
                riDataWarehouse.VitalityDiscount = VitalityDiscount;
                riDataWarehouse.TotalDiscount = TotalDiscount;
                riDataWarehouse.NetPremium = NetPremium;
                riDataWarehouse.AnnualRiPrem = AnnualRiPrem;
                riDataWarehouse.RiCovPeriod = RiCovPeriod;
                riDataWarehouse.AdjBeginDate = AdjBeginDate;
                riDataWarehouse.AdjEndDate = AdjEndDate;
                riDataWarehouse.PolicyNumberOld = PolicyNumberOld;
                riDataWarehouse.PolicyStatusCode = PolicyStatusCode;
                riDataWarehouse.PolicyGrossPremium = PolicyGrossPremium;
                riDataWarehouse.PolicyStandardPremium = PolicyStandardPremium;
                riDataWarehouse.PolicySubstandardPremium = PolicySubstandardPremium;
                riDataWarehouse.PolicyTermRemain = PolicyTermRemain;
                riDataWarehouse.PolicyAmountDeath = PolicyAmountDeath;
                riDataWarehouse.PolicyReserve = PolicyReserve;
                riDataWarehouse.PolicyPaymentMethod = PolicyPaymentMethod;
                riDataWarehouse.PolicyLifeNumber = PolicyLifeNumber;
                riDataWarehouse.FundCode = FundCode;
                riDataWarehouse.LineOfBusiness = LineOfBusiness;
                riDataWarehouse.ApLoading = ApLoading;
                riDataWarehouse.LoanInterestRate = LoanInterestRate;
                riDataWarehouse.DefermentPeriod = DefermentPeriod;
                riDataWarehouse.RiderNumber = RiderNumber;
                riDataWarehouse.CampaignCode = CampaignCode;
                riDataWarehouse.Nationality = Nationality;
                riDataWarehouse.TerritoryOfIssueCode = TerritoryOfIssueCode;
                riDataWarehouse.CurrencyCode = CurrencyCode;
                riDataWarehouse.StaffPlanIndicator = StaffPlanIndicator;
                riDataWarehouse.CedingTreatyCode = CedingTreatyCode;
                riDataWarehouse.CedingPlanCodeOld = CedingPlanCodeOld;
                riDataWarehouse.CedingBasicPlanCode = CedingBasicPlanCode;
                riDataWarehouse.CedantSar = CedantSar;
                riDataWarehouse.CedantReinsurerCode = CedantReinsurerCode;
                riDataWarehouse.AmountCededB4MlreShare2 = AmountCededB4MlreShare2;
                riDataWarehouse.CessionCode = CessionCode;
                riDataWarehouse.CedantRemark = CedantRemark;
                riDataWarehouse.GroupPolicyNumber = GroupPolicyNumber;
                riDataWarehouse.GroupPolicyName = GroupPolicyName;
                riDataWarehouse.NoOfEmployee = NoOfEmployee;
                riDataWarehouse.PolicyTotalLive = PolicyTotalLive;
                riDataWarehouse.GroupSubsidiaryName = GroupSubsidiaryName;
                riDataWarehouse.GroupSubsidiaryNo = GroupSubsidiaryNo;
                riDataWarehouse.GroupEmployeeBasicSalary = GroupEmployeeBasicSalary;
                riDataWarehouse.GroupEmployeeJobType = GroupEmployeeJobType;
                riDataWarehouse.GroupEmployeeJobCode = GroupEmployeeJobCode;
                riDataWarehouse.GroupEmployeeBasicSalaryRevise = GroupEmployeeBasicSalaryRevise;
                riDataWarehouse.GroupEmployeeBasicSalaryMultiplier = GroupEmployeeBasicSalaryMultiplier;
                riDataWarehouse.CedingPlanCode2 = CedingPlanCode2;
                riDataWarehouse.DependantIndicator = DependantIndicator;
                riDataWarehouse.GhsRoomBoard = GhsRoomBoard;
                riDataWarehouse.PolicyAmountSubstandard = PolicyAmountSubstandard;
                riDataWarehouse.Layer1RiShare = Layer1RiShare;
                riDataWarehouse.Layer1InsuredAttainedAge = Layer1InsuredAttainedAge;
                riDataWarehouse.Layer1InsuredAttainedAge2nd = Layer1InsuredAttainedAge2nd;
                riDataWarehouse.Layer1StandardPremium = Layer1StandardPremium;
                riDataWarehouse.Layer1SubstandardPremium = Layer1SubstandardPremium;
                riDataWarehouse.Layer1GrossPremium = Layer1GrossPremium;
                riDataWarehouse.Layer1StandardDiscount = Layer1StandardDiscount;
                riDataWarehouse.Layer1SubstandardDiscount = Layer1SubstandardDiscount;
                riDataWarehouse.Layer1TotalDiscount = Layer1TotalDiscount;
                riDataWarehouse.Layer1NetPremium = Layer1NetPremium;
                riDataWarehouse.Layer1GrossPremiumAlt = Layer1GrossPremiumAlt;
                riDataWarehouse.Layer1TotalDiscountAlt = Layer1TotalDiscountAlt;
                riDataWarehouse.Layer1NetPremiumAlt = Layer1NetPremiumAlt;
                riDataWarehouse.SpecialIndicator1 = SpecialIndicator1;
                riDataWarehouse.SpecialIndicator2 = SpecialIndicator2;
                riDataWarehouse.SpecialIndicator3 = SpecialIndicator3;
                riDataWarehouse.IndicatorJointLife = IndicatorJointLife;
                riDataWarehouse.TaxAmount = TaxAmount;
                riDataWarehouse.GstIndicator = GstIndicator;
                riDataWarehouse.GstGrossPremium = GstGrossPremium;
                riDataWarehouse.GstTotalDiscount = GstTotalDiscount;
                riDataWarehouse.GstVitality = GstVitality;
                riDataWarehouse.GstAmount = GstAmount;
                riDataWarehouse.Mfrs17BasicRider = Mfrs17BasicRider;
                riDataWarehouse.Mfrs17CellName = Mfrs17CellName;
                riDataWarehouse.Mfrs17TreatyCode = Mfrs17TreatyCode;
                riDataWarehouse.LoaCode = LoaCode;
                riDataWarehouse.CurrencyRate = CurrencyRate;
                riDataWarehouse.NoClaimBonus = NoClaimBonus;
                riDataWarehouse.SurrenderValue = SurrenderValue;
                riDataWarehouse.DatabaseCommision = DatabaseCommision;
                riDataWarehouse.GrossPremiumAlt = GrossPremiumAlt;
                riDataWarehouse.NetPremiumAlt = NetPremiumAlt;
                riDataWarehouse.Layer1FlatExtraPremium = Layer1FlatExtraPremium;
                riDataWarehouse.TransactionPremium = TransactionPremium;
                riDataWarehouse.OriginalPremium = OriginalPremium;
                riDataWarehouse.TransactionDiscount = TransactionDiscount;
                riDataWarehouse.OriginalDiscount = OriginalDiscount;
                riDataWarehouse.BrokerageFee = BrokerageFee;
                riDataWarehouse.MaxUwRating = MaxUwRating;
                riDataWarehouse.RetentionCap = RetentionCap;
                riDataWarehouse.AarCap = AarCap;
                riDataWarehouse.RiRate = RiRate;
                riDataWarehouse.RiRate2 = RiRate2;
                riDataWarehouse.AnnuityFactor = AnnuityFactor;
                riDataWarehouse.SumAssuredOffered = SumAssuredOffered;
                riDataWarehouse.UwRatingOffered = UwRatingOffered;
                riDataWarehouse.FlatExtraAmountOffered = FlatExtraAmountOffered;
                riDataWarehouse.FlatExtraDuration = FlatExtraDuration;
                riDataWarehouse.EffectiveDate = EffectiveDate;
                riDataWarehouse.OfferLetterSentDate = OfferLetterSentDate;
                riDataWarehouse.RiskPeriodStartDate = RiskPeriodStartDate;
                riDataWarehouse.RiskPeriodEndDate = RiskPeriodEndDate;
                riDataWarehouse.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                riDataWarehouse.MaxExpiryAge = MaxExpiryAge;
                riDataWarehouse.MinIssueAge = MinIssueAge;
                riDataWarehouse.MaxIssueAge = MaxIssueAge;
                riDataWarehouse.MinAar = MinAar;
                riDataWarehouse.MaxAar = MaxAar;
                riDataWarehouse.CorridorLimit = CorridorLimit;
                riDataWarehouse.Abl = Abl;
                riDataWarehouse.RatePerBasisUnit = RatePerBasisUnit;
                riDataWarehouse.RiDiscountRate = RiDiscountRate;
                riDataWarehouse.LargeSaDiscount = LargeSaDiscount;
                riDataWarehouse.GroupSizeDiscount = GroupSizeDiscount;
                riDataWarehouse.EwarpNumber = EwarpNumber;
                riDataWarehouse.EwarpActionCode = EwarpActionCode;
                riDataWarehouse.RetentionShare = RetentionShare;
                riDataWarehouse.AarShare = AarShare;
                riDataWarehouse.ProfitComm = ProfitComm;
                riDataWarehouse.TotalDirectRetroAar = TotalDirectRetroAar;
                riDataWarehouse.TotalDirectRetroGrossPremium = TotalDirectRetroGrossPremium;
                riDataWarehouse.TotalDirectRetroDiscount = TotalDirectRetroDiscount;
                riDataWarehouse.TotalDirectRetroNetPremium = TotalDirectRetroNetPremium;
                riDataWarehouse.TotalDirectRetroNoClaimBonus = TotalDirectRetroNoClaimBonus;
                riDataWarehouse.TotalDirectRetroDatabaseCommission = TotalDirectRetroDatabaseCommission;
                riDataWarehouse.TreatyType = TreatyType;
                riDataWarehouse.MaxApLoading = MaxApLoading;
                riDataWarehouse.MlreInsuredAttainedAgeAtCurrentMonth = MlreInsuredAttainedAgeAtCurrentMonth;
                riDataWarehouse.MlreInsuredAttainedAgeAtPreviousMonth = MlreInsuredAttainedAgeAtPreviousMonth;
                riDataWarehouse.InsuredAttainedAgeCheck = InsuredAttainedAgeCheck;
                riDataWarehouse.MaxExpiryAgeCheck = MaxExpiryAgeCheck;
                riDataWarehouse.MlrePolicyIssueAge = MlrePolicyIssueAge;
                riDataWarehouse.PolicyIssueAgeCheck = PolicyIssueAgeCheck;
                riDataWarehouse.MinIssueAgeCheck = MinIssueAgeCheck;
                riDataWarehouse.MaxIssueAgeCheck = MaxIssueAgeCheck;
                riDataWarehouse.MaxUwRatingCheck = MaxUwRatingCheck;
                riDataWarehouse.ApLoadingCheck = ApLoadingCheck;
                riDataWarehouse.EffectiveDateCheck = EffectiveDateCheck;
                riDataWarehouse.MinAarCheck = MinAarCheck;
                riDataWarehouse.MaxAarCheck = MaxAarCheck;
                riDataWarehouse.CorridorLimitCheck = CorridorLimitCheck;
                riDataWarehouse.AblCheck = AblCheck;
                riDataWarehouse.RetentionCheck = RetentionCheck;
                riDataWarehouse.AarCheck = AarCheck;
                riDataWarehouse.MlreStandardPremium = MlreStandardPremium;
                riDataWarehouse.MlreSubstandardPremium = MlreSubstandardPremium;
                riDataWarehouse.MlreFlatExtraPremium = MlreFlatExtraPremium;
                riDataWarehouse.MlreGrossPremium = MlreGrossPremium;
                riDataWarehouse.MlreStandardDiscount = MlreStandardDiscount;
                riDataWarehouse.MlreSubstandardDiscount = MlreSubstandardDiscount;
                riDataWarehouse.MlreLargeSaDiscount = MlreLargeSaDiscount;
                riDataWarehouse.MlreGroupSizeDiscount = MlreGroupSizeDiscount;
                riDataWarehouse.MlreVitalityDiscount = MlreVitalityDiscount;
                riDataWarehouse.MlreTotalDiscount = MlreTotalDiscount;
                riDataWarehouse.MlreNetPremium = MlreNetPremium;
                riDataWarehouse.NetPremiumCheck = NetPremiumCheck;
                riDataWarehouse.ServiceFeePercentage = ServiceFeePercentage;
                riDataWarehouse.ServiceFee = ServiceFee;
                riDataWarehouse.MlreBrokerageFee = MlreBrokerageFee;
                riDataWarehouse.MlreDatabaseCommission = MlreDatabaseCommission;
                riDataWarehouse.ValidityDayCheck = ValidityDayCheck;
                riDataWarehouse.SumAssuredOfferedCheck = SumAssuredOfferedCheck;
                riDataWarehouse.UwRatingCheck = UwRatingCheck;
                riDataWarehouse.FlatExtraAmountCheck = FlatExtraAmountCheck;
                riDataWarehouse.FlatExtraDurationCheck = FlatExtraDurationCheck;
                riDataWarehouse.AarShare2 = AarShare2;
                riDataWarehouse.AarCap2 = AarCap2;
                riDataWarehouse.WakalahFeePercentage = WakalahFeePercentage;
                riDataWarehouse.TreatyNumber = TreatyNumber;
                riDataWarehouse.ConflictType = ConflictType;

                // Direct Retro
                riDataWarehouse.RetroParty1 = RetroParty1;
                riDataWarehouse.RetroParty2 = RetroParty2;
                riDataWarehouse.RetroParty3 = RetroParty3;
                riDataWarehouse.RetroShare1 = RetroShare1;
                riDataWarehouse.RetroShare2 = RetroShare2;
                riDataWarehouse.RetroShare3 = RetroShare3;
                riDataWarehouse.RetroPremiumSpread1 = RetroPremiumSpread1;
                riDataWarehouse.RetroPremiumSpread2 = RetroPremiumSpread2;
                riDataWarehouse.RetroPremiumSpread3 = RetroPremiumSpread3;
                riDataWarehouse.RetroAar1 = RetroAar1;
                riDataWarehouse.RetroAar2 = RetroAar2;
                riDataWarehouse.RetroAar3 = RetroAar3;
                riDataWarehouse.RetroReinsurancePremium1 = RetroReinsurancePremium1;
                riDataWarehouse.RetroReinsurancePremium2 = RetroReinsurancePremium2;
                riDataWarehouse.RetroReinsurancePremium3 = RetroReinsurancePremium3;
                riDataWarehouse.RetroDiscount1 = RetroDiscount1;
                riDataWarehouse.RetroDiscount2 = RetroDiscount2;
                riDataWarehouse.RetroDiscount3 = RetroDiscount3;
                riDataWarehouse.RetroNetPremium1 = RetroNetPremium1;
                riDataWarehouse.RetroNetPremium2 = RetroNetPremium2;
                riDataWarehouse.RetroNetPremium3 = RetroNetPremium3;
                riDataWarehouse.RetroNoClaimBonus1 = RetroNoClaimBonus1;
                riDataWarehouse.RetroNoClaimBonus2 = RetroNoClaimBonus2;
                riDataWarehouse.RetroNoClaimBonus3 = RetroNoClaimBonus3;
                riDataWarehouse.RetroDatabaseCommission1 = RetroDatabaseCommission1;
                riDataWarehouse.RetroDatabaseCommission2 = RetroDatabaseCommission2;
                riDataWarehouse.RetroDatabaseCommission3 = RetroDatabaseCommission3;

                riDataWarehouse.LastUpdatedDate = LastUpdatedDate;
                riDataWarehouse.UpdatedAt = DateTime.Now;
                riDataWarehouse.UpdatedById = UpdatedById ?? riDataWarehouse.UpdatedById;

                db.Entry(riDataWarehouse).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext(false))
            {
                RiDataWarehouse riDataWarehouse = Find(id);
                if (riDataWarehouse == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataWarehouse, true);

                db.Entry(riDataWarehouse).State = EntityState.Deleted;
                db.RiDataWarehouse.Remove(riDataWarehouse);
                db.SaveChanges();

                return trail;
            }
        }

        public static int? LookUpRiDataWarehouseIdForPostValidation(RiDataBo bo)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0; // execution timeout

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(1198, "RiDataWarehouse");

                return connectionStrategy.Execute(() => db.RiDataWarehouse
                    .Where(q => q.PolicyNumber == bo.PolicyNumber)
                    .Where(q => q.CedingPlanCode == bo.CedingPlanCode)
                    .Where(q => q.RiskPeriodMonth == bo.RiskPeriodMonth)
                    .Where(q => q.RiskPeriodYear == bo.RiskPeriodYear)
                    .Where(q => q.MlreBenefitCode == bo.MlreBenefitCode)
                    .Where(q => q.TreatyCode == bo.TreatyCode)
                    .Where(q => q.RiderNumber == bo.RiderNumber)
                    .Where(q => q.InsuredName == bo.InsuredName)
                    .Where(q => q.CedingBenefitTypeCode == bo.CedingBenefitTypeCode)
                    .Where(q => q.CedingBenefitRiskCode == bo.CedingBenefitRiskCode)
                    .Where(q => q.CedingPlanCode2 == bo.CedingPlanCode2)
                    .Where(q => q.CessionCode == bo.CessionCode)
                    .Select(q => q.Id)
                    .FirstOrDefault());
            }
        }
    }
}
