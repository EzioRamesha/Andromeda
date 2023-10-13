using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.RiDatas
{
    [Table("ReferralRiData")]
    public class ReferralRiData
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int ReferralRiDataFileId { get; set; }
        [ForeignKey(nameof(ReferralRiDataFile))]
        [ExcludeTrail]
        public virtual ReferralRiDataFile ReferralRiDataFile { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string ReinsBasisCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string FundsAccountingTypeCode { get; set; }

        [MaxLength(10)]
        [Index]
        public string PremiumFrequencyCode { get; set; }

        [Index]
        public int? ReportPeriodMonth { get; set; }

        [Index]
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
        [Index]
        public DateTime? IssueDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? IssueDateBen { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? ReinsEffDatePol { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
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
        [Index]
        public string MlreBenefitCode { get; set; }

        [Index]
        public double? OriSumAssured { get; set; }

        [Index]
        public double? CurrSumAssured { get; set; }

        [Index]
        public double? AmountCededB4MlreShare { get; set; }

        [Index]
        public double? RetentionAmount { get; set; }

        [Index]
        public double? AarOri { get; set; }

        [Index]
        public double? Aar { get; set; }

        [Index]
        public double? AarSpecial1 { get; set; }

        [Index]
        public double? AarSpecial2 { get; set; }

        [Index]
        public double? AarSpecial3 { get; set; }

        [MaxLength(128)]
        [Index]
        public string InsuredName { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredGenderCode { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredTobaccoUse { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(5)]
        [Index]
        public string InsuredOccupationCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string InsuredRegisterNo { get; set; }

        [Index]
        public int? InsuredAttainedAge { get; set; }

        [MaxLength(15)]
        [Index]
        public string InsuredNewIcNumber { get; set; }

        [MaxLength(15)]
        [Index]
        public string InsuredOldIcNumber { get; set; }

        [MaxLength(128)]
        [Index]
        public string InsuredName2nd { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredGenderCode2nd { get; set; }

        [MaxLength(1)]
        [Index]
        public string InsuredTobaccoUse2nd { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? InsuredDateOfBirth2nd { get; set; }

        [Index]
        public int? InsuredAttainedAge2nd { get; set; }

        [MaxLength(15)]
        [Index]
        public string InsuredNewIcNumber2nd { get; set; }

        [MaxLength(15)]
        [Index]
        public string InsuredOldIcNumber2nd { get; set; }

        [Index]
        public int? ReinsuranceIssueAge { get; set; }

        [Index]
        public int? ReinsuranceIssueAge2nd { get; set; }

        [Index]
        public double? PolicyTerm { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? PolicyExpiryDate { get; set; }

        [Index]
        public double? DurationYear { get; set; }

        [Index]
        public double? DurationDay { get; set; }

        [Index]
        public double? DurationMonth { get; set; }

        [MaxLength(5)]
        [Index]
        public string PremiumCalType { get; set; }

        [Index]
        public double? CedantRiRate { get; set; }

        [MaxLength(50)]
        [Index]
        public string RateTable { get; set; }

        [Index]
        public int? AgeRatedUp { get; set; }

        [Index]
        public double? DiscountRate { get; set; }

        [MaxLength(15)]
        [Index]
        public string LoadingType { get; set; }

        [Index]
        public double? UnderwriterRating { get; set; }

        [Index]
        public double? UnderwriterRatingUnit { get; set; }

        [Index]
        public int? UnderwriterRatingTerm { get; set; }

        [Index]
        public double? UnderwriterRating2 { get; set; }

        [Index]
        public double? UnderwriterRatingUnit2 { get; set; }

        [Index]
        public int? UnderwriterRatingTerm2 { get; set; }

        [Index]
        public double? UnderwriterRating3 { get; set; }

        [Index]
        public double? UnderwriterRatingUnit3 { get; set; }

        [Index]
        public int? UnderwriterRatingTerm3 { get; set; }

        [Index]
        public double? FlatExtraAmount { get; set; }

        [Index]
        public double? FlatExtraUnit { get; set; }

        [Index]
        public int? FlatExtraTerm { get; set; }

        [Index]
        public double? FlatExtraAmount2 { get; set; }

        [Index]
        public int? FlatExtraTerm2 { get; set; }

        [Index]
        public double? StandardPremium { get; set; }

        [Index]
        public double? SubstandardPremium { get; set; }

        [Index]
        public double? FlatExtraPremium { get; set; }

        [Index]
        public double? GrossPremium { get; set; }

        [Index]
        public double? StandardDiscount { get; set; }

        [Index]
        public double? SubstandardDiscount { get; set; }

        [Index]
        public double? VitalityDiscount { get; set; }

        [Index]
        public double? TotalDiscount { get; set; }

        [Index]
        public double? NetPremium { get; set; }

        [Index]
        public double? AnnualRiPrem { get; set; }

        [Index]
        public double? RiCovPeriod { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? AdjBeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? AdjEndDate { get; set; }

        [MaxLength(150)]
        [Index]
        public string PolicyNumberOld { get; set; }

        [MaxLength(20)]
        [Index]
        public string PolicyStatusCode { get; set; }

        [Index]
        public double? PolicyGrossPremium { get; set; }

        [Index]
        public double? PolicyStandardPremium { get; set; }

        [Index]
        public double? PolicySubstandardPremium { get; set; }

        [Index]
        public double? PolicyTermRemain { get; set; }

        [Index]
        public double? PolicyAmountDeath { get; set; }

        [Index]
        public double? PolicyReserve { get; set; }

        [MaxLength(10)]
        [Index]
        public string PolicyPaymentMethod { get; set; }

        [Index]
        public int? PolicyLifeNumber { get; set; }

        [MaxLength(25)]
        [Index]
        public string FundCode { get; set; }

        [MaxLength(5)]
        [Index]
        public string LineOfBusiness { get; set; }

        [Index]
        public double? ApLoading { get; set; }

        [Index]
        public double? LoanInterestRate { get; set; }

        [Index]
        public int? DefermentPeriod { get; set; }

        [Index]
        public int? RiderNumber { get; set; }

        [MaxLength(10)]
        [Index]
        public string CampaignCode { get; set; }

        [MaxLength(20)]
        [Index]
        public string Nationality { get; set; }

        [MaxLength(20)]
        [Index]
        public string TerritoryOfIssueCode { get; set; }

        [MaxLength(3)]
        [Index]
        public string CurrencyCode { get; set; }

        [MaxLength(1)]
        [Index]
        public string StaffPlanIndicator { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingTreatyCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingPlanCodeOld { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBasicPlanCode { get; set; }

        [Index]
        public double? CedantSar { get; set; }

        [MaxLength(10)]
        [Index]
        public string CedantReinsurerCode { get; set; }

        [Index]
        public double? AmountCededB4MlreShare2 { get; set; }

        [MaxLength(10)]
        [Index]
        public string CessionCode { get; set; }

        [MaxLength(255)]
        public string CedantRemark { get; set; }

        [MaxLength(30)]
        [Index]
        public string GroupPolicyNumber { get; set; }

        [MaxLength(128)]
        [Index]
        public string GroupPolicyName { get; set; }

        [Index]
        public int? NoOfEmployee { get; set; }

        [Index]
        public int? PolicyTotalLive { get; set; }

        [MaxLength(128)]
        [Index]
        public string GroupSubsidiaryName { get; set; }

        [MaxLength(30)]
        [Index]
        public string GroupSubsidiaryNo { get; set; }

        [Index]
        public double? GroupEmployeeBasicSalary { get; set; }

        [MaxLength(10)]
        [Index]
        public string GroupEmployeeJobType { get; set; }

        [MaxLength(10)]
        [Index]
        public string GroupEmployeeJobCode { get; set; }

        [Index]
        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        [Index]
        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingPlanCode2 { get; set; }

        [MaxLength(2)]
        [Index]
        public string DependantIndicator { get; set; }

        [Index]
        public int? GhsRoomBoard { get; set; }

        [Index]
        public double? PolicyAmountSubstandard { get; set; }

        [Index]
        public double? Layer1RiShare { get; set; }

        [Index]
        public int? Layer1InsuredAttainedAge { get; set; }

        [Index]
        public int? Layer1InsuredAttainedAge2nd { get; set; }

        [Index]
        public double? Layer1StandardPremium { get; set; }

        [Index]
        public double? Layer1SubstandardPremium { get; set; }

        [Index]
        public double? Layer1GrossPremium { get; set; }

        [Index]
        public double? Layer1StandardDiscount { get; set; }

        [Index]
        public double? Layer1SubstandardDiscount { get; set; }

        [Index]
        public double? Layer1TotalDiscount { get; set; }

        [Index]
        public double? Layer1NetPremium { get; set; }

        [Index]
        public double? Layer1GrossPremiumAlt { get; set; }

        [Index]
        public double? Layer1TotalDiscountAlt { get; set; }

        [Index]
        public double? Layer1NetPremiumAlt { get; set; }

        public string SpecialIndicator1 { get; set; }

        public string SpecialIndicator2 { get; set; }

        public string SpecialIndicator3 { get; set; }

        [MaxLength(1)]
        [Index]
        public string IndicatorJointLife { get; set; }

        [Index]
        public double? TaxAmount { get; set; }

        [MaxLength(3)]
        [Index]
        public string GstIndicator { get; set; }

        [Index]
        public double? GstGrossPremium { get; set; }

        [Index]
        public double? GstTotalDiscount { get; set; }

        [Index]
        public double? GstVitality { get; set; }

        [Index]
        public double? GstAmount { get; set; }

        [MaxLength(5)]
        [Index]
        public string Mfrs17BasicRider { get; set; }

        [MaxLength(50)]
        [Index]
        public string Mfrs17CellName { get; set; }

        [MaxLength(25)]
        [Index]
        public string Mfrs17TreatyCode { get; set; }

        [MaxLength(20)]
        [Index]
        public string LoaCode { get; set; }

        [Index]
        public double? CurrencyRate { get; set; }

        [Index]
        public double? NoClaimBonus { get; set; }

        [Index]
        public double? SurrenderValue { get; set; }

        [Index]
        public double? DatabaseCommision { get; set; }

        [Index]
        public double? GrossPremiumAlt { get; set; }

        [Index]
        public double? NetPremiumAlt { get; set; }

        [Index]
        public double? Layer1FlatExtraPremium { get; set; }

        [Index]
        public double? TransactionPremium { get; set; }

        [Index]
        public double? OriginalPremium { get; set; }

        [Index]
        public double? TransactionDiscount { get; set; }

        [Index]
        public double? OriginalDiscount { get; set; }

        [Index]
        public double? BrokerageFee { get; set; }

        [Index]
        public double? MaxUwRating { get; set; }

        [Index]
        public double? RetentionCap { get; set; }

        [Index]
        public double? AarCap { get; set; }

        [Index]
        public double? RiRate { get; set; }

        [Index]
        public double? RiRate2 { get; set; }

        [Index]
        public double? AnnuityFactor { get; set; }

        [Index]
        public double? SumAssuredOffered { get; set; }

        [Index]
        public double? UwRatingOffered { get; set; }

        [Index]
        public double? FlatExtraAmountOffered { get; set; }

        [Index]
        public double? FlatExtraDuration { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? EffectiveDate { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? OfferLetterSentDate { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? RiskPeriodStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime? RiskPeriodEndDate { get; set; }

        [Index]
        public int? Mfrs17AnnualCohort { get; set; }

        [Index]
        public int? MaxExpiryAge { get; set; }

        [Index]
        public int? MinIssueAge { get; set; }

        [Index]
        public int? MaxIssueAge { get; set; }

        [Index]
        public double? MinAar { get; set; }

        [Index]
        public double? MaxAar { get; set; }

        [Index]
        public double? CorridorLimit { get; set; }

        [Index]
        public double? Abl { get; set; }

        [Index]
        public int? RatePerBasisUnit { get; set; }

        [Index]
        public double? RiDiscountRate { get; set; }

        [Index]
        public double? LargeSaDiscount { get; set; }

        [Index]
        public double? GroupSizeDiscount { get; set; }

        [Index]
        public int? EwarpNumber { get; set; }

        [MaxLength(10)]
        [Index]
        public string EwarpActionCode { get; set; }

        [Index]
        public double? RetentionShare { get; set; }

        [Index]
        public double? AarShare { get; set; }

        [MaxLength(1)]
        [Index]
        public string ProfitComm { get; set; }

        [Index]
        public double? TotalDirectRetroAar { get; set; }

        [Index]
        public double? TotalDirectRetroGrossPremium { get; set; }

        [Index]
        public double? TotalDirectRetroDiscount { get; set; }

        [Index]
        public double? TotalDirectRetroNetPremium { get; set; }

        [MaxLength(20)]
        [Index]
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
        [Index]
        public DateTime LastUpdatedDate { get; set; }

        [Index]
        public double? AarShare2 { get; set; }

        [Index]
        public double? AarCap2 { get; set; }

        [Index]
        public double? WakalahFeePercentage { get; set; }

        [MaxLength(128), Index]
        public string TreatyNumber { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        [Index]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Index]
        public int CreatedById { get; set; }

        [Index]
        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public ReferralRiData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static ReferralRiData Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.ReferralRiData.Where(q => q.Id == id).FirstOrDefault();
            }
        }
        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.ReferralRiData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                ReferralRiData entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyCode = TreatyCode;
                entity.ReinsBasisCode = ReinsBasisCode;
                entity.FundsAccountingTypeCode = FundsAccountingTypeCode;
                entity.PremiumFrequencyCode = PremiumFrequencyCode;
                entity.ReportPeriodMonth = ReportPeriodMonth;
                entity.ReportPeriodYear = ReportPeriodYear;
                entity.RiskPeriodMonth = RiskPeriodMonth;
                entity.RiskPeriodYear = RiskPeriodYear;
                entity.TransactionTypeCode = TransactionTypeCode;
                entity.PolicyNumber = PolicyNumber;
                entity.IssueDatePol = IssueDatePol;
                entity.IssueDateBen = IssueDateBen;
                entity.ReinsEffDatePol = ReinsEffDatePol;
                entity.ReinsEffDateBen = ReinsEffDateBen;
                entity.CedingPlanCode = CedingPlanCode;
                entity.CedingBenefitTypeCode = CedingBenefitTypeCode;
                entity.CedingBenefitRiskCode = CedingBenefitRiskCode;
                entity.MlreBenefitCode = MlreBenefitCode;
                entity.OriSumAssured = OriSumAssured;
                entity.CurrSumAssured = CurrSumAssured;
                entity.AmountCededB4MlreShare = AmountCededB4MlreShare;
                entity.RetentionAmount = RetentionAmount;
                entity.AarOri = AarOri;
                entity.Aar = Aar;
                entity.AarSpecial1 = AarSpecial1;
                entity.AarSpecial2 = AarSpecial2;
                entity.AarSpecial3 = AarSpecial3;
                entity.InsuredName = InsuredName;
                entity.InsuredGenderCode = InsuredGenderCode;
                entity.InsuredTobaccoUse = InsuredTobaccoUse;
                entity.InsuredDateOfBirth = InsuredDateOfBirth;
                entity.InsuredOccupationCode = InsuredOccupationCode;
                entity.InsuredRegisterNo = InsuredRegisterNo;
                entity.InsuredAttainedAge = InsuredAttainedAge;
                entity.InsuredNewIcNumber = InsuredNewIcNumber;
                entity.InsuredOldIcNumber = InsuredOldIcNumber;
                entity.InsuredName2nd = InsuredName2nd;
                entity.InsuredGenderCode2nd = InsuredGenderCode2nd;
                entity.InsuredTobaccoUse2nd = InsuredTobaccoUse2nd;
                entity.InsuredDateOfBirth2nd = InsuredDateOfBirth2nd;
                entity.InsuredAttainedAge2nd = InsuredAttainedAge2nd;
                entity.InsuredNewIcNumber2nd = InsuredNewIcNumber2nd;
                entity.InsuredOldIcNumber2nd = InsuredOldIcNumber2nd;
                entity.ReinsuranceIssueAge = ReinsuranceIssueAge;
                entity.ReinsuranceIssueAge2nd = ReinsuranceIssueAge2nd;
                entity.PolicyTerm = PolicyTerm;
                entity.PolicyExpiryDate = PolicyExpiryDate;
                entity.DurationYear = DurationYear;
                entity.DurationDay = DurationDay;
                entity.DurationMonth = DurationMonth;
                entity.PremiumCalType = PremiumCalType;
                entity.CedantRiRate = CedantRiRate;
                entity.RateTable = RateTable;
                entity.AgeRatedUp = AgeRatedUp;
                entity.DiscountRate = DiscountRate;
                entity.LoadingType = LoadingType;
                entity.UnderwriterRating = UnderwriterRating;
                entity.UnderwriterRatingUnit = UnderwriterRatingUnit;
                entity.UnderwriterRatingTerm = UnderwriterRatingTerm;
                entity.UnderwriterRating2 = UnderwriterRating2;
                entity.UnderwriterRatingUnit2 = UnderwriterRatingUnit2;
                entity.UnderwriterRatingTerm2 = UnderwriterRatingTerm2;
                entity.UnderwriterRating3 = UnderwriterRating3;
                entity.UnderwriterRatingUnit3 = UnderwriterRatingUnit3;
                entity.UnderwriterRatingTerm3 = UnderwriterRatingTerm3;
                entity.FlatExtraAmount = FlatExtraAmount;
                entity.FlatExtraUnit = FlatExtraUnit;
                entity.FlatExtraTerm = FlatExtraTerm;
                entity.FlatExtraAmount2 = FlatExtraAmount2;
                entity.FlatExtraTerm2 = FlatExtraTerm2;
                entity.StandardPremium = StandardPremium;
                entity.SubstandardPremium = SubstandardPremium;
                entity.FlatExtraPremium = FlatExtraPremium;
                entity.GrossPremium = GrossPremium;
                entity.StandardDiscount = StandardDiscount;
                entity.SubstandardDiscount = SubstandardDiscount;
                entity.VitalityDiscount = VitalityDiscount;
                entity.TotalDiscount = TotalDiscount;
                entity.NetPremium = NetPremium;
                entity.AnnualRiPrem = AnnualRiPrem;
                entity.RiCovPeriod = RiCovPeriod;
                entity.AdjBeginDate = AdjBeginDate;
                entity.AdjEndDate = AdjEndDate;
                entity.PolicyNumberOld = PolicyNumberOld;
                entity.PolicyStatusCode = PolicyStatusCode;
                entity.PolicyGrossPremium = PolicyGrossPremium;
                entity.PolicyStandardPremium = PolicyStandardPremium;
                entity.PolicySubstandardPremium = PolicySubstandardPremium;
                entity.PolicyTermRemain = PolicyTermRemain;
                entity.PolicyAmountDeath = PolicyAmountDeath;
                entity.PolicyReserve = PolicyReserve;
                entity.PolicyPaymentMethod = PolicyPaymentMethod;
                entity.PolicyLifeNumber = PolicyLifeNumber;
                entity.FundCode = FundCode;
                entity.LineOfBusiness = LineOfBusiness;
                entity.ApLoading = ApLoading;
                entity.LoanInterestRate = LoanInterestRate;
                entity.DefermentPeriod = DefermentPeriod;
                entity.RiderNumber = RiderNumber;
                entity.CampaignCode = CampaignCode;
                entity.Nationality = Nationality;
                entity.TerritoryOfIssueCode = TerritoryOfIssueCode;
                entity.CurrencyCode = CurrencyCode;
                entity.StaffPlanIndicator = StaffPlanIndicator;
                entity.CedingTreatyCode = CedingTreatyCode;
                entity.CedingPlanCodeOld = CedingPlanCodeOld;
                entity.CedingBasicPlanCode = CedingBasicPlanCode;
                entity.CedantSar = CedantSar;
                entity.CedantReinsurerCode = CedantReinsurerCode;
                entity.AmountCededB4MlreShare2 = AmountCededB4MlreShare2;
                entity.CessionCode = CessionCode;
                entity.CedantRemark = CedantRemark;
                entity.GroupPolicyNumber = GroupPolicyNumber;
                entity.GroupPolicyName = GroupPolicyName;
                entity.NoOfEmployee = NoOfEmployee;
                entity.PolicyTotalLive = PolicyTotalLive;
                entity.GroupSubsidiaryName = GroupSubsidiaryName;
                entity.GroupSubsidiaryNo = GroupSubsidiaryNo;
                entity.GroupEmployeeBasicSalary = GroupEmployeeBasicSalary;
                entity.GroupEmployeeJobType = GroupEmployeeJobType;
                entity.GroupEmployeeJobCode = GroupEmployeeJobCode;
                entity.GroupEmployeeBasicSalaryRevise = GroupEmployeeBasicSalaryRevise;
                entity.GroupEmployeeBasicSalaryMultiplier = GroupEmployeeBasicSalaryMultiplier;
                entity.CedingPlanCode2 = CedingPlanCode2;
                entity.DependantIndicator = DependantIndicator;
                entity.GhsRoomBoard = GhsRoomBoard;
                entity.PolicyAmountSubstandard = PolicyAmountSubstandard;
                entity.Layer1RiShare = Layer1RiShare;
                entity.Layer1InsuredAttainedAge = Layer1InsuredAttainedAge;
                entity.Layer1InsuredAttainedAge2nd = Layer1InsuredAttainedAge2nd;
                entity.Layer1StandardPremium = Layer1StandardPremium;
                entity.Layer1SubstandardPremium = Layer1SubstandardPremium;
                entity.Layer1GrossPremium = Layer1GrossPremium;
                entity.Layer1StandardDiscount = Layer1StandardDiscount;
                entity.Layer1SubstandardDiscount = Layer1SubstandardDiscount;
                entity.Layer1TotalDiscount = Layer1TotalDiscount;
                entity.Layer1NetPremium = Layer1NetPremium;
                entity.Layer1GrossPremiumAlt = Layer1GrossPremiumAlt;
                entity.Layer1TotalDiscountAlt = Layer1TotalDiscountAlt;
                entity.Layer1NetPremiumAlt = Layer1NetPremiumAlt;
                entity.SpecialIndicator1 = SpecialIndicator1;
                entity.SpecialIndicator2 = SpecialIndicator2;
                entity.SpecialIndicator3 = SpecialIndicator3;
                entity.IndicatorJointLife = IndicatorJointLife;
                entity.TaxAmount = TaxAmount;
                entity.GstIndicator = GstIndicator;
                entity.GstGrossPremium = GstGrossPremium;
                entity.GstTotalDiscount = GstTotalDiscount;
                entity.GstVitality = GstVitality;
                entity.GstAmount = GstAmount;
                entity.Mfrs17BasicRider = Mfrs17BasicRider;
                entity.Mfrs17CellName = Mfrs17CellName;
                entity.Mfrs17TreatyCode = Mfrs17TreatyCode;
                entity.LoaCode = LoaCode;
                entity.CurrencyRate = CurrencyRate;
                entity.NoClaimBonus = NoClaimBonus;
                entity.SurrenderValue = SurrenderValue;
                entity.DatabaseCommision = DatabaseCommision;
                entity.GrossPremiumAlt = GrossPremiumAlt;
                entity.NetPremiumAlt = NetPremiumAlt;
                entity.Layer1FlatExtraPremium = Layer1FlatExtraPremium;
                entity.TransactionPremium = TransactionPremium;
                entity.OriginalPremium = OriginalPremium;
                entity.TransactionDiscount = TransactionDiscount;
                entity.OriginalDiscount = OriginalDiscount;
                entity.BrokerageFee = BrokerageFee;
                entity.MaxUwRating = MaxUwRating;
                entity.RetentionCap = RetentionCap;
                entity.AarCap = AarCap;
                entity.RiRate = RiRate;
                entity.RiRate2 = RiRate2;
                entity.AnnuityFactor = AnnuityFactor;
                entity.SumAssuredOffered = SumAssuredOffered;
                entity.UwRatingOffered = UwRatingOffered;
                entity.FlatExtraAmountOffered = FlatExtraAmountOffered;
                entity.FlatExtraDuration = FlatExtraDuration;
                entity.EffectiveDate = EffectiveDate;
                entity.OfferLetterSentDate = OfferLetterSentDate;
                entity.RiskPeriodStartDate = RiskPeriodStartDate;
                entity.RiskPeriodEndDate = RiskPeriodEndDate;
                entity.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                entity.MaxExpiryAge = MaxExpiryAge;
                entity.MinIssueAge = MinIssueAge;
                entity.MaxIssueAge = MaxIssueAge;
                entity.MinAar = MinAar;
                entity.MaxAar = MaxAar;
                entity.CorridorLimit = CorridorLimit;
                entity.Abl = Abl;
                entity.RatePerBasisUnit = RatePerBasisUnit;
                entity.RiDiscountRate = RiDiscountRate;
                entity.LargeSaDiscount = LargeSaDiscount;
                entity.GroupSizeDiscount = GroupSizeDiscount;
                entity.EwarpNumber = EwarpNumber;
                entity.EwarpActionCode = EwarpActionCode;
                entity.RetentionShare = RetentionShare;
                entity.AarShare = AarShare;
                entity.ProfitComm = ProfitComm;
                entity.TotalDirectRetroAar = TotalDirectRetroAar;
                entity.TotalDirectRetroGrossPremium = TotalDirectRetroGrossPremium;
                entity.TotalDirectRetroDiscount = TotalDirectRetroDiscount;
                entity.TotalDirectRetroNetPremium = TotalDirectRetroNetPremium;
                entity.TreatyType = TreatyType;
                entity.MaxApLoading = MaxApLoading;
                entity.MlreInsuredAttainedAgeAtCurrentMonth = MlreInsuredAttainedAgeAtCurrentMonth;
                entity.MlreInsuredAttainedAgeAtPreviousMonth = MlreInsuredAttainedAgeAtPreviousMonth;
                entity.InsuredAttainedAgeCheck = InsuredAttainedAgeCheck;
                entity.MaxExpiryAgeCheck = MaxExpiryAgeCheck;
                entity.MlrePolicyIssueAge = MlrePolicyIssueAge;
                entity.PolicyIssueAgeCheck = PolicyIssueAgeCheck;
                entity.MinIssueAgeCheck = MinIssueAgeCheck;
                entity.MaxIssueAgeCheck = MaxIssueAgeCheck;
                entity.MaxUwRatingCheck = MaxUwRatingCheck;
                entity.ApLoadingCheck = ApLoadingCheck;
                entity.EffectiveDateCheck = EffectiveDateCheck;
                entity.MinAarCheck = MinAarCheck;
                entity.MaxAarCheck = MaxAarCheck;
                entity.CorridorLimitCheck = CorridorLimitCheck;
                entity.AblCheck = AblCheck;
                entity.RetentionCheck = RetentionCheck;
                entity.AarCheck = AarCheck;
                entity.MlreStandardPremium = MlreStandardPremium;
                entity.MlreSubstandardPremium = MlreSubstandardPremium;
                entity.MlreFlatExtraPremium = MlreFlatExtraPremium;
                entity.MlreGrossPremium = MlreGrossPremium;
                entity.MlreStandardDiscount = MlreStandardDiscount;
                entity.MlreSubstandardDiscount = MlreSubstandardDiscount;
                entity.MlreLargeSaDiscount = MlreLargeSaDiscount;
                entity.MlreGroupSizeDiscount = MlreGroupSizeDiscount;
                entity.MlreVitalityDiscount = MlreVitalityDiscount;
                entity.MlreTotalDiscount = MlreTotalDiscount;
                entity.MlreNetPremium = MlreNetPremium;
                entity.NetPremiumCheck = NetPremiumCheck;
                entity.ServiceFeePercentage = ServiceFeePercentage;
                entity.ServiceFee = ServiceFee;
                entity.MlreBrokerageFee = MlreBrokerageFee;
                entity.MlreDatabaseCommission = MlreDatabaseCommission;
                entity.ValidityDayCheck = ValidityDayCheck;
                entity.SumAssuredOfferedCheck = SumAssuredOfferedCheck;
                entity.UwRatingCheck = UwRatingCheck;
                entity.FlatExtraAmountCheck = FlatExtraAmountCheck;
                entity.FlatExtraDurationCheck = FlatExtraDurationCheck;
                entity.AarShare2 = AarShare2;
                entity.AarCap2 = AarCap2;
                entity.WakalahFeePercentage = WakalahFeePercentage;
                entity.TreatyNumber = TreatyNumber;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext(false))
            {
                ReferralRiData entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.ReferralRiData.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
