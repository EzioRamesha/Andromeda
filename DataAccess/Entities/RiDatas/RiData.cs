using BusinessObject.RiDatas;
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
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccess.Entities.RiDatas
{
    [Table("RiData")]
    public class RiData
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Index]
        public int? RiDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual RiDataBatch RiDataBatch { get; set; }

        [Index]
        public int? RiDataFileId { get; set; }

        [ExcludeTrail]
        public virtual RiDataFile RiDataFile { get; set; }

        public int RecordType { get; set; }

        public int? OriginalEntryId { get; set; }

        [Index]
        public bool IgnoreFinalise { get; set; } = false;

        [Index]
        public int MappingStatus { get; set; }
        [Index]
        public int PreComputation1Status { get; set; }
        [Index]
        public int PreComputation2Status { get; set; }
        [Index]
        public int PreValidationStatus { get; set; }
        [Index]
        public int PostComputationStatus { get; set; }
        [Index]
        public int PostValidationStatus { get; set; }
        [Index]
        public int FinaliseStatus { get; set; }
        [Index]
        public int ProcessWarehouseStatus { get; set; }


        public string Errors { get; set; }
        public string CustomField { get; set; }


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

        [Index, MaxLength(10)]
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

        [Index]
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

        [Column(TypeName = "datetime2")]
        public DateTime? TempD1 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD2 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD3 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD4 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TempD5 { get; set; }

        [MaxLength(50)]
        public string TempS1 { get; set; }

        [MaxLength(50)]
        public string TempS2 { get; set; }

        [MaxLength(50)]
        public string TempS3 { get; set; }

        [MaxLength(50)]
        public string TempS4 { get; set; }

        [MaxLength(50)]
        public string TempS5 { get; set; }

        public int? TempI1 { get; set; }

        public int? TempI2 { get; set; }

        public int? TempI3 { get; set; }

        public int? TempI4 { get; set; }

        public int? TempI5 { get; set; }

        public double? TempA1 { get; set; }

        public double? TempA2 { get; set; }

        public double? TempA3 { get; set; }

        public double? TempA4 { get; set; }

        public double? TempA5 { get; set; }

        public double? TempA6 { get; set; }

        public double? TempA7 { get; set; }

        public double? TempA8 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

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

        public int? FlatExtraDuration { get; set; }

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

        public RiData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData.Any(q => q.Id == id);
            }
        }

        public static RiData Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiData");
                return connectionStrategy.Execute(() => db.RiData.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static RiData FindSimplified(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RiData> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData.Where(q => ids.Contains(q.Id)).ToList();
            }
        }

        public static int? GetMaxYearByTreatyCodeYear(string treatyCode, int year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear < year)
                    .OrderByDescending(q => q.RiskPeriodYear)
                    .Select(q => q.RiskPeriodYear)
                    .FirstOrDefault();
            }
        }

        public static int? GetMaxMonthByTreatyCodeYear(string treatyCode, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodYear <= year)
                    .OrderByDescending(q => q.RiskPeriodMonth)
                    .Select(q => q.RiskPeriodMonth)
                    .FirstOrDefault();
            }
        }

        public static int? GetMaxMonthByTreatyCodeMonthYear(string treatyCode, int? month, int? year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiskPeriodMonth <= month)
                    .Where(q => q.RiskPeriodYear <= year)
                    .OrderByDescending(q => q.RiskPeriodMonth)
                    .Select(q => q.RiskPeriodMonth)
                    .FirstOrDefault();
            }
        }

        public static int CountByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Count();
            }
        }

        public static int CountByRiDataFileIdMappingStatus(int riDataFileId, int mappingStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .Where(q => q.MappingStatus == mappingStatus)
                    .Count();
            }
        }

        public static int CountByRiDataFileIdPreComputation1Status(int riDataFileId, int preComputation1Status)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .Where(q => q.PreComputation1Status == preComputation1Status)
                    .Count();
            }
        }

        public static int CountByRiDataFileIdPreValidationStatus(int riDataFileId, int preValidationStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .Where(q => q.PreValidationStatus == preValidationStatus)
                    .Count();
            }
        }

        public static int CountByRiDataFileIdFinaliseStatus(int riDataFileId, int finaliseStatus)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataFileId == riDataFileId)
                    .Where(q => q.FinaliseStatus == finaliseStatus)
                    .Count();
            }
        }

        public static int CountRecordForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, int month, int year)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
                    .Where(q => q.PremiumFrequencyCode == premiumFrequencyCode)
                    .Where(q => q.RiskPeriodMonth == month)
                    .Where(q => q.RiskPeriodYear == year)
                    .Count();
            }
        }

        public static int CountByRiDataBatchIdMappingStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(954);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.MappingStatus == RiDataBo.MappingStatusFailed)
                .Count());
        }

        public static int CountByRiDataBatchIdMappingStatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.MappingStatus == RiDataBo.MappingStatusFailed)
                    .Count();
            }
        }

        public static int CountByRiDataBatchIdPreComputation1StatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(975);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PreComputation1Status == RiDataBo.PreComputation1StatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdPreComputation1StatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(986);
                return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PreComputation1Status == RiDataBo.PreComputation1StatusFailed)
                .Count());
            }
        }

        public static int CountByRiDataBatchIdPreComputation2StatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(997);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PreComputation2Status == RiDataBo.PreComputation1StatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdPreComputation2StatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1008);
                return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PreComputation2Status == RiDataBo.PreComputation1StatusFailed)
                .Count());
            }
        }

        public static int CountByRiDataBatchIdPreValidationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1019);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PreValidationStatus == RiDataBo.PreValidationStatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdPreValidationStatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1030);
                return connectionStrategy.Execute(() => db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.PreValidationStatus == RiDataBo.PreValidationStatusFailed)
                    .Count());
            }
        }

        public static int CountByRiDataBatchIdPostComputationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1041);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PostComputationStatus == RiDataBo.PostComputationStatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdPostComputationStatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1052);
                return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PostComputationStatus == RiDataBo.PostComputationStatusFailed)
                .Count());
            }
        }

        public static int CountByRiDataBatchIdPostValidationStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1063);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PostValidationStatus == RiDataBo.PostValidationStatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdPostValidationStatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.PostValidationStatus == RiDataBo.PostValidationStatusFailed)
                .Count();
            }
        }

        public static int CountByRiDataBatchIdFinaliseStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1083);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.FinaliseStatus == RiDataBo.FinaliseStatusFailed)
                .Count());
        }
        public static int CountByRiDataBatchIdFinaliseStatusFailed(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.FinaliseStatus == RiDataBo.FinaliseStatusFailed)
                    .Count();
            }
        }

        public static int CountByRiDataBatchIdProcessWarehouseStatusFailed(int riDataBatchId, AppDbContext db)
        {
            db.Database.CommandTimeout = 0;
            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1103);
            return connectionStrategy.Execute(() => db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.ProcessWarehouseStatus == RiDataBo.ProcessWarehouseStatusFailed)
                .Count());
        }

        public static int CountByRiDataBatchIdIsConflict(int riDataBatchId, AppDbContext db)
        {
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.ConflictType > 0)
                .Count();
        }

        public static IList<RiData> GetByRiDataBatchId(int riDataBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public static IList<RiData> GetByRiDataBatchIdRiDataFileId(int riDataBatchId, int riDataFileId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData.Where(q => q.RiDataBatchId == riDataBatchId && q.RiDataFileId == riDataFileId).ToList();
            }
        }

        public static List<int> GetAllByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdsOfIgnoreFinalise(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.IgnoreFinalise == true)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<string> GetDistinctMfrs17TreatyCodes(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
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
                var query = db.RiData.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
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

        public static int CountForMfrs17Reporting(string treatyCode, string premiumFrequencyCode, int year, int monthStart, int? monthEnd = null)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.RiData.Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised)
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

        public static IList<RiData> GetByOriginalMatchCombination(int id, string policyNo, string planCode, string quarter, string riskQuarter)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiData.Where(q => q.Id != id);

                if (!string.IsNullOrEmpty(policyNo)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber == policyNo);
                if (!string.IsNullOrEmpty(planCode)) query = query.Where(q => !string.IsNullOrEmpty(q.CedingPlanCode) && q.CedingPlanCode == planCode);
                if (!string.IsNullOrEmpty(quarter)) query = query.Where(q => q.RiDataBatch.Quarter == quarter);
                if (!string.IsNullOrEmpty(riskQuarter))
                {
                    int riskQuarterYear = int.Parse(riskQuarter.Substring(0, 4));
                    int riskQuarterNo = int.Parse(riskQuarter.Substring(6, 1));

                    var riskMonthByQuarter = Enumerable.Range(1, 12).Select(month => month).GroupBy(month => (month - 1) / 3).ToList();
                    List<int> riskMonths = riskMonthByQuarter.Where(o => o.Key == (riskQuarterNo - 1)).Select(o => o.ToList()).FirstOrDefault();

                    query = query.Where(q => riskMonths.Contains(q.RiskPeriodMonth.Value) && q.RiskPeriodYear == riskQuarterYear);
                }

                return query.ToList();
            }
        }

        public static IList<RiData> GetByClaimRegisterParam(string policyNumber, string cedingPlanCode, int riskYear, int riskMonth, string soaQuarter, string cedingBenefitTypeCode, string cedingBenefitRiskCode)
        {
            using (var db = new AppDbContext())
            {
                return db.RiData
                    .Where(q => q.PolicyNumber == policyNumber)
                    .Where(q => q.CedingPlanCode == cedingPlanCode)
                    .Where(q => q.RiskPeriodYear == riskYear)
                    .Where(q => q.RiskPeriodMonth == riskMonth)
                    .Where(q => q.RiDataBatch.Quarter == soaQuarter)
                    .Where(q => q.CedingBenefitTypeCode == cedingBenefitTypeCode)
                    .Where(q => q.CedingBenefitRiskCode == cedingBenefitRiskCode)
                    .ToList();
            }
        }

        public static double SumDiscountForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Sum(q => q.TotalDiscount) ?? 0;
            }
        }

        public static double SumGrossForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Select(q => new { GrossTotal = q.StandardPremium.Value + q.SubstandardPremium.Value + q.FlatExtraPremium.Value })
                    .Sum(q => (double?)q.GrossTotal) ?? 0;
            }
        }

        public static double SumReinsForInvoice(int riDataBatchId, string transactionTypeCode)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Sum(q => q.Aar) ?? 0;
            }
        }

        public static double CountReinsForInvoice(
            int riDataBatchId,
            string transactionTypeCode,
            string quarterlyMode,
            string annualMode,
            string monthlyMode,
            List<int> quarterMonths)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId)
                    .Where(q => q.TransactionTypeCode == transactionTypeCode)
                    .Where(q => q.PremiumFrequencyCode == quarterlyMode)
                    .Where(q => quarterMonths.Contains(q.RiskPeriodMonth.Value) && q.PremiumFrequencyCode == monthlyMode)
                    .Count();
            }
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
            using (var db = new AppDbContext())
            {
                return QueryByLookupParams(
                    db,
                    policyNumber,
                    cedingPlanCode,
                    mlreBenefitCode,
                    treatyCode,
                    riskPeriodMonth,
                    riskPeriodYear,
                    riderNumber
                ).Count();
            }
        }

        public static RiData FindByLookupParams(
            string policyNumber,
            string cedingPlanCode,
            string mlreBenefitCode,
            string treatyCode,
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
                    riskPeriodMonth,
                    riskPeriodYear,
                    riderNumber
                ).FirstOrDefault();
            }
        }

        public static IQueryable<RiData> QueryByLookupParams(
            AppDbContext db,
            string policyNumber,
            string cedingPlanCode,
            string mlreBenefitCode,
            string treatyCode,
            int? riskPeriodMonth = null,
            int? riskPeriodYear = null,
            int? riderNumber = null
        )
        {
            var query = db.RiData
                .Where(q => q.PolicyNumber == policyNumber)
                .Where(q => q.CedingPlanCode == cedingPlanCode)
                .Where(q => q.MlreBenefitCode == mlreBenefitCode)
                .Where(q => q.TreatyCode == treatyCode)
                .Where(q => q.RiskPeriodMonth == riskPeriodMonth)
                .Where(q => q.RiskPeriodYear == riskPeriodYear);

            if (riderNumber.HasValue)
            {
                query = query.Where(q => q.RiderNumber == riderNumber);
            }

            return query.Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised);
        }

        public static bool IsDuplicate(RiDataBo riData)
        {
            //TREATY_CODE
            //RISK_PERIOD_MONTH
            //RISK_PERIOD_YEAR
            //TRANSACTION_TYPE_CODE
            //POLICY_NUMBER
            //CEDING_PLAN_CODE
            //CEDING_PLAN_CODE_2
            //CEDING_BENEFIT_TYPE_CODE
            //CEDING_BENEFIT_RISK_CODE
            //INSURED_NAME
            //CESSION_CODE
            using (var db = new AppDbContext(false))
            {
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiData");
                connectionStrategy.retryCount = 0;
                return connectionStrategy.Execute(() =>
                {
                    var query = db.RiData
                        .Where(q => q.TreatyCode == riData.TreatyCode)
                        .Where(q => q.RiskPeriodMonth == riData.RiskPeriodMonth)
                        .Where(q => q.RiskPeriodYear == riData.RiskPeriodYear)
                        .Where(q => q.TransactionTypeCode == riData.TransactionTypeCode)
                        .Where(q => q.PolicyNumber == riData.PolicyNumber)
                        .Where(q => q.CedingPlanCode == riData.CedingPlanCode)
                        .Where(q => q.CedingPlanCode2 == riData.CedingPlanCode2)
                        .Where(q => q.CedingBenefitTypeCode == riData.CedingBenefitTypeCode)
                        .Where(q => q.CedingBenefitRiskCode == riData.CedingBenefitRiskCode)
                        .Where(q => q.InsuredName == riData.InsuredName)
                        .Where(q => q.NetPremium == riData.NetPremium)
                        .Where(q => q.CessionCode == riData.CessionCode)
                        .Where(q => q.RiderNumber == riData.RiderNumber)
                        .Where(q => q.MlreBenefitCode == riData.MlreBenefitCode)
                        .Where(q => q.CampaignCode == riData.CampaignCode);

                    int countNotInSameBatch = query.Where(q => q.RiDataBatchId != riData.RiDataBatchId).Where(q => q.RiDataBatch.Status == RiDataBatchBo.StatusFinalised).Count();
                    int countInSameBatch = query.Where(q => q.RiDataBatchId == riData.RiDataBatchId).Where(q => q.Id != riData.Id).Count();

                    return (countNotInSameBatch + countInSameBatch) > 0;
                });
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                var saved = db.RiData.Add(this);

                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(saved.Id);
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(1452);
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.RiData.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                RiData riData = RiData.Find(Id);
                if (riData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riData, this);

                riData.RiDataBatchId = RiDataBatchId;
                riData.RiDataFileId = RiDataFileId;
                riData.RecordType = RecordType;
                riData.OriginalEntryId = OriginalEntryId;
                riData.IgnoreFinalise = IgnoreFinalise;
                riData.MappingStatus = MappingStatus;
                riData.PreComputation1Status = PreComputation1Status;
                riData.PreComputation2Status = PreComputation2Status;
                riData.PreValidationStatus = PreValidationStatus;
                riData.PostComputationStatus = PostComputationStatus;
                riData.PostValidationStatus = PostValidationStatus;
                riData.FinaliseStatus = FinaliseStatus;
                riData.ProcessWarehouseStatus = ProcessWarehouseStatus;
                riData.Errors = Errors;

                riData.CustomField = CustomField;
                riData.TreatyCode = TreatyCode;
                riData.ReinsBasisCode = ReinsBasisCode;
                riData.FundsAccountingTypeCode = FundsAccountingTypeCode;
                riData.PremiumFrequencyCode = PremiumFrequencyCode;
                riData.ReportPeriodMonth = ReportPeriodMonth;
                riData.ReportPeriodYear = ReportPeriodYear;
                riData.RiskPeriodMonth = RiskPeriodMonth;
                riData.RiskPeriodYear = RiskPeriodYear;
                riData.TransactionTypeCode = TransactionTypeCode;
                riData.PolicyNumber = PolicyNumber;
                riData.IssueDatePol = IssueDatePol;
                riData.IssueDateBen = IssueDateBen;
                riData.ReinsEffDatePol = ReinsEffDatePol;
                riData.ReinsEffDateBen = ReinsEffDateBen;
                riData.CedingPlanCode = CedingPlanCode;
                riData.CedingBenefitTypeCode = CedingBenefitTypeCode;
                riData.CedingBenefitRiskCode = CedingBenefitRiskCode;
                riData.MlreBenefitCode = MlreBenefitCode;
                riData.OriSumAssured = OriSumAssured;
                riData.CurrSumAssured = CurrSumAssured;
                riData.AmountCededB4MlreShare = AmountCededB4MlreShare;
                riData.RetentionAmount = RetentionAmount;
                riData.AarOri = AarOri;
                riData.Aar = Aar;
                riData.AarSpecial1 = AarSpecial1;
                riData.AarSpecial2 = AarSpecial2;
                riData.AarSpecial3 = AarSpecial3;
                riData.InsuredName = InsuredName;
                riData.InsuredGenderCode = InsuredGenderCode;
                riData.InsuredTobaccoUse = InsuredTobaccoUse;
                riData.InsuredDateOfBirth = InsuredDateOfBirth;
                riData.InsuredOccupationCode = InsuredOccupationCode;
                riData.InsuredRegisterNo = InsuredRegisterNo;
                riData.InsuredAttainedAge = InsuredAttainedAge;
                riData.InsuredNewIcNumber = InsuredNewIcNumber;
                riData.InsuredOldIcNumber = InsuredOldIcNumber;
                riData.InsuredName2nd = InsuredName2nd;
                riData.InsuredGenderCode2nd = InsuredGenderCode2nd;
                riData.InsuredTobaccoUse2nd = InsuredTobaccoUse2nd;
                riData.InsuredDateOfBirth2nd = InsuredDateOfBirth2nd;
                riData.InsuredAttainedAge2nd = InsuredAttainedAge2nd;
                riData.InsuredNewIcNumber2nd = InsuredNewIcNumber2nd;
                riData.InsuredOldIcNumber2nd = InsuredOldIcNumber2nd;
                riData.ReinsuranceIssueAge = ReinsuranceIssueAge;
                riData.ReinsuranceIssueAge2nd = ReinsuranceIssueAge2nd;
                riData.PolicyTerm = PolicyTerm;
                riData.PolicyExpiryDate = PolicyExpiryDate;
                riData.DurationYear = DurationYear;
                riData.DurationDay = DurationDay;
                riData.DurationMonth = DurationMonth;
                riData.PremiumCalType = PremiumCalType;
                riData.CedantRiRate = CedantRiRate;
                riData.RateTable = RateTable;
                riData.AgeRatedUp = AgeRatedUp;
                riData.DiscountRate = DiscountRate;
                riData.LoadingType = LoadingType;
                riData.UnderwriterRating = UnderwriterRating;
                riData.UnderwriterRatingUnit = UnderwriterRatingUnit;
                riData.UnderwriterRatingTerm = UnderwriterRatingTerm;
                riData.UnderwriterRating2 = UnderwriterRating2;
                riData.UnderwriterRatingUnit2 = UnderwriterRatingUnit2;
                riData.UnderwriterRatingTerm2 = UnderwriterRatingTerm2;
                riData.UnderwriterRating3 = UnderwriterRating3;
                riData.UnderwriterRatingUnit3 = UnderwriterRatingUnit3;
                riData.UnderwriterRatingTerm3 = UnderwriterRatingTerm3;
                riData.FlatExtraAmount = FlatExtraAmount;
                riData.FlatExtraUnit = FlatExtraUnit;
                riData.FlatExtraTerm = FlatExtraTerm;
                riData.FlatExtraAmount2 = FlatExtraAmount2;
                riData.FlatExtraTerm2 = FlatExtraTerm2;
                riData.StandardPremium = StandardPremium;
                riData.SubstandardPremium = SubstandardPremium;
                riData.FlatExtraPremium = FlatExtraPremium;
                riData.GrossPremium = GrossPremium;
                riData.StandardDiscount = StandardDiscount;
                riData.SubstandardDiscount = SubstandardDiscount;
                riData.VitalityDiscount = VitalityDiscount;
                riData.TotalDiscount = TotalDiscount;
                riData.NetPremium = NetPremium;
                riData.AnnualRiPrem = AnnualRiPrem;
                riData.RiCovPeriod = RiCovPeriod;
                riData.AdjBeginDate = AdjBeginDate;
                riData.AdjEndDate = AdjEndDate;
                riData.PolicyNumberOld = PolicyNumberOld;
                riData.PolicyStatusCode = PolicyStatusCode;
                riData.PolicyGrossPremium = PolicyGrossPremium;
                riData.PolicyStandardPremium = PolicyStandardPremium;
                riData.PolicySubstandardPremium = PolicySubstandardPremium;
                riData.PolicyTermRemain = PolicyTermRemain;
                riData.PolicyAmountDeath = PolicyAmountDeath;
                riData.PolicyReserve = PolicyReserve;
                riData.PolicyPaymentMethod = PolicyPaymentMethod;
                riData.PolicyLifeNumber = PolicyLifeNumber;
                riData.FundCode = FundCode;
                riData.LineOfBusiness = LineOfBusiness;
                riData.ApLoading = ApLoading;
                riData.LoanInterestRate = LoanInterestRate;
                riData.DefermentPeriod = DefermentPeriod;
                riData.RiderNumber = RiderNumber;
                riData.CampaignCode = CampaignCode;
                riData.Nationality = Nationality;
                riData.TerritoryOfIssueCode = TerritoryOfIssueCode;
                riData.CurrencyCode = CurrencyCode;
                riData.StaffPlanIndicator = StaffPlanIndicator;
                riData.CedingTreatyCode = CedingTreatyCode;
                riData.CedingPlanCodeOld = CedingPlanCodeOld;
                riData.CedingBasicPlanCode = CedingBasicPlanCode;
                riData.CedantSar = CedantSar;
                riData.CedantReinsurerCode = CedantReinsurerCode;
                riData.AmountCededB4MlreShare2 = AmountCededB4MlreShare2;
                riData.CessionCode = CessionCode;
                riData.CedantRemark = CedantRemark;
                riData.GroupPolicyNumber = GroupPolicyNumber;
                riData.GroupPolicyName = GroupPolicyName;
                riData.NoOfEmployee = NoOfEmployee;
                riData.PolicyTotalLive = PolicyTotalLive;
                riData.GroupSubsidiaryName = GroupSubsidiaryName;
                riData.GroupSubsidiaryNo = GroupSubsidiaryNo;
                riData.GroupEmployeeBasicSalary = GroupEmployeeBasicSalary;
                riData.GroupEmployeeJobType = GroupEmployeeJobType;
                riData.GroupEmployeeJobCode = GroupEmployeeJobCode;
                riData.GroupEmployeeBasicSalaryRevise = GroupEmployeeBasicSalaryRevise;
                riData.GroupEmployeeBasicSalaryMultiplier = GroupEmployeeBasicSalaryMultiplier;
                riData.CedingPlanCode2 = CedingPlanCode2;
                riData.DependantIndicator = DependantIndicator;
                riData.GhsRoomBoard = GhsRoomBoard;
                riData.PolicyAmountSubstandard = PolicyAmountSubstandard;
                riData.Layer1RiShare = Layer1RiShare;
                riData.Layer1InsuredAttainedAge = Layer1InsuredAttainedAge;
                riData.Layer1InsuredAttainedAge2nd = Layer1InsuredAttainedAge2nd;
                riData.Layer1StandardPremium = Layer1StandardPremium;
                riData.Layer1SubstandardPremium = Layer1SubstandardPremium;
                riData.Layer1GrossPremium = Layer1GrossPremium;
                riData.Layer1StandardDiscount = Layer1StandardDiscount;
                riData.Layer1SubstandardDiscount = Layer1SubstandardDiscount;
                riData.Layer1TotalDiscount = Layer1TotalDiscount;
                riData.Layer1NetPremium = Layer1NetPremium;
                riData.Layer1GrossPremiumAlt = Layer1GrossPremiumAlt;
                riData.Layer1TotalDiscountAlt = Layer1TotalDiscountAlt;
                riData.Layer1NetPremiumAlt = Layer1NetPremiumAlt;
                riData.SpecialIndicator1 = SpecialIndicator1;
                riData.SpecialIndicator2 = SpecialIndicator2;
                riData.SpecialIndicator3 = SpecialIndicator3;
                riData.IndicatorJointLife = IndicatorJointLife;
                riData.TaxAmount = TaxAmount;
                riData.GstIndicator = GstIndicator;
                riData.GstGrossPremium = GstGrossPremium;
                riData.GstTotalDiscount = GstTotalDiscount;
                riData.GstVitality = GstVitality;
                riData.GstAmount = GstAmount;
                riData.Mfrs17BasicRider = Mfrs17BasicRider;
                riData.Mfrs17CellName = Mfrs17CellName;
                riData.Mfrs17TreatyCode = Mfrs17TreatyCode;
                riData.LoaCode = LoaCode;
                riData.TempD1 = TempD1;
                riData.TempD2 = TempD2;
                riData.TempD3 = TempD3;
                riData.TempD4 = TempD4;
                riData.TempD5 = TempD5;
                riData.TempS1 = TempS1;
                riData.TempS2 = TempS2;
                riData.TempS3 = TempS3;
                riData.TempS4 = TempS4;
                riData.TempS5 = TempS5;
                riData.TempI1 = TempI1;
                riData.TempI2 = TempI2;
                riData.TempI3 = TempI3;
                riData.TempI4 = TempI4;
                riData.TempI5 = TempI5;
                riData.TempA1 = TempA1;
                riData.TempA2 = TempA2;
                riData.TempA3 = TempA3;
                riData.TempA4 = TempA4;
                riData.TempA5 = TempA5;
                riData.TempA6 = TempA6;
                riData.TempA7 = TempA7;
                riData.TempA8 = TempA8;

                //Phase 2
                riData.CurrencyRate = CurrencyRate;
                riData.NoClaimBonus = NoClaimBonus;
                riData.SurrenderValue = SurrenderValue;
                riData.DatabaseCommision = DatabaseCommision;
                riData.GrossPremiumAlt = GrossPremiumAlt;
                riData.NetPremiumAlt = NetPremiumAlt;
                riData.Layer1FlatExtraPremium = Layer1FlatExtraPremium;
                riData.TransactionPremium = TransactionPremium;
                riData.OriginalPremium = OriginalPremium;
                riData.TransactionDiscount = TransactionDiscount;
                riData.OriginalDiscount = OriginalDiscount;
                riData.BrokerageFee = BrokerageFee;
                riData.MaxUwRating = MaxUwRating;
                riData.RetentionCap = RetentionCap;
                riData.AarCap = AarCap;
                riData.RiRate = RiRate;
                riData.RiRate2 = RiRate2;
                riData.AnnuityFactor = AnnuityFactor;
                riData.SumAssuredOffered = SumAssuredOffered;
                riData.UwRatingOffered = UwRatingOffered;
                riData.FlatExtraAmountOffered = FlatExtraAmountOffered;
                riData.FlatExtraDuration = FlatExtraDuration;
                riData.EffectiveDate = EffectiveDate;
                riData.OfferLetterSentDate = OfferLetterSentDate;
                riData.RiskPeriodStartDate = RiskPeriodStartDate;
                riData.RiskPeriodEndDate = RiskPeriodEndDate;
                riData.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                riData.MaxExpiryAge = MaxExpiryAge;
                riData.MinIssueAge = MinIssueAge;
                riData.MaxIssueAge = MaxIssueAge;
                riData.MinAar = MinAar;
                riData.MaxAar = MaxAar;
                riData.CorridorLimit = CorridorLimit;
                riData.Abl = Abl;
                riData.RatePerBasisUnit = RatePerBasisUnit;
                riData.RiDiscountRate = RiDiscountRate;
                riData.LargeSaDiscount = LargeSaDiscount;
                riData.GroupSizeDiscount = GroupSizeDiscount;
                riData.EwarpNumber = EwarpNumber;
                riData.EwarpActionCode = EwarpActionCode;
                riData.RetentionShare = RetentionShare;
                riData.AarShare = AarShare;
                riData.ProfitComm = ProfitComm;
                riData.TotalDirectRetroAar = TotalDirectRetroAar;
                riData.TotalDirectRetroGrossPremium = TotalDirectRetroGrossPremium;
                riData.TotalDirectRetroDiscount = TotalDirectRetroDiscount;
                riData.TotalDirectRetroNetPremium = TotalDirectRetroNetPremium;
                riData.TotalDirectRetroNoClaimBonus = TotalDirectRetroNoClaimBonus;
                riData.TotalDirectRetroDatabaseCommission = TotalDirectRetroDatabaseCommission;
                riData.TreatyType = TreatyType;
                riData.MaxApLoading = MaxApLoading;
                riData.MlreInsuredAttainedAgeAtCurrentMonth = MlreInsuredAttainedAgeAtCurrentMonth;
                riData.MlreInsuredAttainedAgeAtPreviousMonth = MlreInsuredAttainedAgeAtPreviousMonth;
                riData.InsuredAttainedAgeCheck = InsuredAttainedAgeCheck;
                riData.MaxExpiryAgeCheck = MaxExpiryAgeCheck;
                riData.MlrePolicyIssueAge = MlrePolicyIssueAge;
                riData.PolicyIssueAgeCheck = PolicyIssueAgeCheck;
                riData.MinIssueAgeCheck = MinIssueAgeCheck;
                riData.MaxIssueAgeCheck = MaxIssueAgeCheck;
                riData.MaxUwRatingCheck = MaxUwRatingCheck;
                riData.ApLoadingCheck = ApLoadingCheck;
                riData.EffectiveDateCheck = EffectiveDateCheck;
                riData.MinAarCheck = MinAarCheck;
                riData.MaxAarCheck = MaxAarCheck;
                riData.CorridorLimitCheck = CorridorLimitCheck;
                riData.AblCheck = AblCheck;
                riData.RetentionCheck = RetentionCheck;
                riData.AarCheck = AarCheck;
                riData.MlreStandardPremium = MlreStandardPremium;
                riData.MlreSubstandardPremium = MlreSubstandardPremium;
                riData.MlreFlatExtraPremium = MlreFlatExtraPremium;
                riData.MlreGrossPremium = MlreGrossPremium;
                riData.MlreStandardDiscount = MlreStandardDiscount;
                riData.MlreSubstandardDiscount = MlreSubstandardDiscount;
                riData.MlreLargeSaDiscount = MlreLargeSaDiscount;
                riData.MlreGroupSizeDiscount = MlreGroupSizeDiscount;
                riData.MlreVitalityDiscount = MlreVitalityDiscount;
                riData.MlreTotalDiscount = MlreTotalDiscount;
                riData.MlreNetPremium = MlreNetPremium;
                riData.NetPremiumCheck = NetPremiumCheck;
                riData.ServiceFeePercentage = ServiceFeePercentage;
                riData.ServiceFee = ServiceFee;
                riData.MlreBrokerageFee = MlreBrokerageFee;
                riData.MlreDatabaseCommission = MlreDatabaseCommission;
                riData.ValidityDayCheck = ValidityDayCheck;
                riData.SumAssuredOfferedCheck = SumAssuredOfferedCheck;
                riData.UwRatingCheck = UwRatingCheck;
                riData.FlatExtraAmountCheck = FlatExtraAmountCheck;
                riData.FlatExtraDurationCheck = FlatExtraDurationCheck;
                riData.AarShare2 = AarShare2;
                riData.AarCap2 = AarCap2;
                riData.WakalahFeePercentage = WakalahFeePercentage;
                riData.TreatyNumber = TreatyNumber;
                riData.ConflictType = ConflictType;

                // Direct Retro
                riData.RetroParty1 = RetroParty1;
                riData.RetroParty2 = RetroParty2;
                riData.RetroParty3 = RetroParty3;
                riData.RetroShare1 = RetroShare1;
                riData.RetroShare2 = RetroShare2;
                riData.RetroShare3 = RetroShare3;
                riData.RetroPremiumSpread1 = RetroPremiumSpread1;
                riData.RetroPremiumSpread2 = RetroPremiumSpread2;
                riData.RetroPremiumSpread3 = RetroPremiumSpread3;
                riData.RetroAar1 = RetroAar1;
                riData.RetroAar2 = RetroAar2;
                riData.RetroAar3 = RetroAar3;
                riData.RetroReinsurancePremium1 = RetroReinsurancePremium1;
                riData.RetroReinsurancePremium2 = RetroReinsurancePremium2;
                riData.RetroReinsurancePremium3 = RetroReinsurancePremium3;
                riData.RetroDiscount1 = RetroDiscount1;
                riData.RetroDiscount2 = RetroDiscount2;
                riData.RetroDiscount3 = RetroDiscount3;
                riData.RetroNetPremium1 = RetroNetPremium1;
                riData.RetroNetPremium2 = RetroNetPremium2;
                riData.RetroNetPremium3 = RetroNetPremium3;
                riData.RetroNoClaimBonus1 = RetroNoClaimBonus1;
                riData.RetroNoClaimBonus2 = RetroNoClaimBonus2;
                riData.RetroNoClaimBonus3 = RetroNoClaimBonus3;
                riData.RetroDatabaseCommission1 = RetroDatabaseCommission1;
                riData.RetroDatabaseCommission2 = RetroDatabaseCommission2;
                riData.RetroDatabaseCommission3 = RetroDatabaseCommission3;
                riData.UpdatedAt = DateTime.Now;
                riData.UpdatedById = UpdatedById ?? riData.UpdatedById;
                db.Entry(riData).State = EntityState.Modified;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(1805);
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext(false))
            {
                RiData riData = db.RiData.Where(q => q.Id == id).FirstOrDefault();
                if (riData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riData, true);

                db.Entry(riData).State = EntityState.Deleted;
                db.RiData.Remove(riData);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataBatchId(int riDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                db.Database.CommandTimeout = 0;
                //var query = db.RiData.Where(q => q.RiDataBatchId == riDataBatchId);
                List<DataTrail> trails = new List<DataTrail>();

                // DO NOT TRAIL since this is mass data
                /*
                foreach (RiData riData in query.ToList())
                {
                    DataTrail trail = new DataTrail(riData, true);
                    trails.Add(trail);

                    db.Entry(riData).State = EntityState.Deleted;
                    db.RiData.Remove(riData);
                }
                */

                //db.RiData.RemoveRange(query);

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(1852);
                db.Database.ExecuteSqlCommand("DELETE FROM [RiData] WHERE [RiDataBatchId] = {0}", riDataBatchId);
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });

                return trails;
            }
        }

        public static void DeleteAllByRiDataBatchId(AppDbContext db, int riDataBatchId)
        {
            //var query = db.RiData.Where(q => q.RiDataBatchId == riDataBatchId);

            // DO NOT TRAIL since this is mass data
            /*
            foreach (RiData riData in query.ToList())
            {
                DataTrail trail = new DataTrail(riData, true);
                trails.Add(trail);

                db.Entry(riData).State = EntityState.Deleted;
                db.RiData.Remove(riData);
            }
            */

            //db.RiData.RemoveRange(query);

            db.Database.ExecuteSqlCommand("DELETE FROM [RiData] WHERE [RiDataBatchId] = {0}", riDataBatchId);
            db.SaveChanges();
        }
    }
}
