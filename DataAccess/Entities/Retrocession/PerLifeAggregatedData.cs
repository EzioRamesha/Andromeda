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

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeAggregatedData")]
    public class PerLifeAggregatedData
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PerLifeAggregationDetailId { get; set; }

        [ForeignKey(nameof(PerLifeAggregationDetailId))]
        [ExcludeTrail]
        public virtual PerLifeAggregationDetail PerLifeAggregationDetail { get; set; }

        [MaxLength(35)]
        public string TreatyCode { get; set; }

        [MaxLength(30)]
        public string ReinsBasisCode { get; set; }

        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        [MaxLength(10)]
        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        [MaxLength(2)]
        public string TransactionTypeCode { get; set; }

        [MaxLength(150)]
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
        public string CedingPlanCode { get; set; }

        [MaxLength(30)]
        public string CedingBenefitTypeCode { get; set; }

        [MaxLength(50)]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(10)]
        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? AarOri { get; set; }

        public double? Aar { get; set; }

        [MaxLength(128)]
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

        public int? ReinsuranceIssueAge { get; set; }

        public double? PolicyTerm { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PolicyExpiryDate { get; set; }

        [MaxLength(15)]
        public string LoadingType { get; set; }

        public double? UnderwriterRating { get; set; }

        public double? FlatExtraAmount { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? StandardDiscount { get; set; }

        public double? SubstandardDiscount { get; set; }

        public double? NetPremium { get; set; }

        [MaxLength(150)]
        public string PolicyNumberOld { get; set; }

        public int? PolicyLifeNumber { get; set; }

        [MaxLength(25)]
        public string FundCode { get; set; }

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
        public string CedingPlanCodeOld { get; set; }

        [MaxLength(30)]
        public string CedingBasicPlanCode { get; set; }

        [MaxLength(30)]
        public string GroupPolicyNumber { get; set; }

        [MaxLength(128)]
        public string GroupPolicyName { get; set; }

        [MaxLength(128)]
        public string GroupSubsidiaryName { get; set; }

        [MaxLength(30)]
        public string GroupSubsidiaryNo { get; set; }

        [MaxLength(30)]
        public string CedingPlanCode2 { get; set; }

        [MaxLength(2)]
        public string DependantIndicator { get; set; }

        [MaxLength(5)]
        public string Mfrs17BasicRider { get; set; }

        [MaxLength(50)]
        public string Mfrs17CellName { get; set; }

        [MaxLength(25)]
        public string Mfrs17ContractCode { get; set; }

        [MaxLength(20)]
        public string LoaCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskPeriodStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RiskPeriodEndDate { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public double? BrokerageFee { get; set; }

        public double? ApLoading { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EffectiveDate { get; set; }

        public double? AnnuityFactor { get; set; }

        [MaxLength(20)]
        public string EndingPolicyStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastUpdatedDate { get; set; }

        [MaxLength(20)]
        public string TreatyType { get; set; }

        [MaxLength(128)]
        public string TreatyNumber { get; set; }

        [MaxLength(10)]
        public string RetroPremFreq { get; set; }

        public bool LifeBenefitFlag { get; set; } = false;

        [MaxLength(64)]
        public string RiskQuarter { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessingDate { get; set; }

        [MaxLength(150)]
        public string UniqueKeyPerLife { get; set; }

        [MaxLength(10)]
        public string RetroBenefitCode { get; set; }

        public double? RetroRatio { get; set; }

        public double? AccumulativeRetainAmount { get; set; }

        public double? RetroRetainAmount { get; set; }

        public double? RetroAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public double? RetroExtraPremium { get; set; }

        public double? RetroExtraComm { get; set; }

        public double? RetroGst { get; set; }

        [MaxLength(50)]
        public string RetroTreaty { get; set; }

        [MaxLength(30)]
        public string RetroClaimId { get; set; }

        [MaxLength(150)]
        public string Soa { get; set; }

        public bool RetroIndicator { get; set; } = false;

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

        public PerLifeAggregatedData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregatedData.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregatedData Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregatedData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregatedData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregatedData perLifeAggregatedData = Find(Id);
                if (perLifeAggregatedData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregatedData, this);

                perLifeAggregatedData.PerLifeAggregationDetailId = PerLifeAggregationDetailId;
                perLifeAggregatedData.TreatyCode = TreatyCode;
                perLifeAggregatedData.ReinsBasisCode = ReinsBasisCode;
                perLifeAggregatedData.FundsAccountingTypeCode = FundsAccountingTypeCode;
                perLifeAggregatedData.PremiumFrequencyCode = PremiumFrequencyCode;
                perLifeAggregatedData.ReportPeriodMonth = ReportPeriodMonth;
                perLifeAggregatedData.ReportPeriodYear = ReportPeriodYear;
                perLifeAggregatedData.RiskPeriodMonth = RiskPeriodMonth;
                perLifeAggregatedData.RiskPeriodYear = RiskPeriodYear;
                perLifeAggregatedData.TransactionTypeCode = TransactionTypeCode;
                perLifeAggregatedData.PolicyNumber = PolicyNumber;
                perLifeAggregatedData.IssueDatePol = IssueDatePol;
                perLifeAggregatedData.IssueDateBen = IssueDateBen;
                perLifeAggregatedData.ReinsEffDatePol = ReinsEffDatePol;
                perLifeAggregatedData.ReinsEffDateBen = ReinsEffDateBen;
                perLifeAggregatedData.CedingPlanCode = CedingPlanCode;
                perLifeAggregatedData.CedingBenefitTypeCode = CedingBenefitTypeCode;
                perLifeAggregatedData.CedingBenefitRiskCode = CedingBenefitRiskCode;
                perLifeAggregatedData.MlreBenefitCode = MlreBenefitCode;
                perLifeAggregatedData.OriSumAssured = OriSumAssured;
                perLifeAggregatedData.CurrSumAssured = CurrSumAssured;
                perLifeAggregatedData.AmountCededB4MlreShare = AmountCededB4MlreShare;
                perLifeAggregatedData.AarOri = AarOri;
                perLifeAggregatedData.Aar = Aar;
                perLifeAggregatedData.InsuredName = InsuredName;
                perLifeAggregatedData.InsuredGenderCode = InsuredGenderCode;
                perLifeAggregatedData.InsuredTobaccoUse = InsuredTobaccoUse;
                perLifeAggregatedData.InsuredDateOfBirth = InsuredDateOfBirth;
                perLifeAggregatedData.InsuredOccupationCode = InsuredOccupationCode;
                perLifeAggregatedData.InsuredRegisterNo = InsuredRegisterNo;
                perLifeAggregatedData.InsuredAttainedAge = InsuredAttainedAge;
                perLifeAggregatedData.InsuredNewIcNumber = InsuredNewIcNumber;
                perLifeAggregatedData.InsuredOldIcNumber = InsuredOldIcNumber;
                perLifeAggregatedData.ReinsuranceIssueAge = ReinsuranceIssueAge;
                perLifeAggregatedData.PolicyTerm = PolicyTerm;
                perLifeAggregatedData.PolicyExpiryDate = PolicyExpiryDate;
                perLifeAggregatedData.LoadingType = LoadingType;
                perLifeAggregatedData.UnderwriterRating = UnderwriterRating;
                perLifeAggregatedData.FlatExtraAmount = FlatExtraAmount;
                perLifeAggregatedData.StandardPremium = StandardPremium;
                perLifeAggregatedData.SubstandardPremium = SubstandardPremium;
                perLifeAggregatedData.FlatExtraPremium = FlatExtraPremium;
                perLifeAggregatedData.GrossPremium = GrossPremium;
                perLifeAggregatedData.StandardDiscount = StandardDiscount;
                perLifeAggregatedData.SubstandardDiscount = SubstandardDiscount;
                perLifeAggregatedData.NetPremium = NetPremium;
                perLifeAggregatedData.PolicyNumberOld = PolicyNumberOld;
                perLifeAggregatedData.PolicyLifeNumber = PolicyLifeNumber;
                perLifeAggregatedData.FundCode = FundCode;
                perLifeAggregatedData.RiderNumber = RiderNumber;
                perLifeAggregatedData.CampaignCode = CampaignCode;
                perLifeAggregatedData.Nationality = Nationality;
                perLifeAggregatedData.TerritoryOfIssueCode = TerritoryOfIssueCode;
                perLifeAggregatedData.CurrencyCode = CurrencyCode;
                perLifeAggregatedData.StaffPlanIndicator = StaffPlanIndicator;
                perLifeAggregatedData.CedingPlanCodeOld = CedingPlanCodeOld;
                perLifeAggregatedData.CedingBasicPlanCode = CedingBasicPlanCode;
                perLifeAggregatedData.GroupPolicyNumber = GroupPolicyNumber;
                perLifeAggregatedData.GroupPolicyName = GroupPolicyName;
                perLifeAggregatedData.GroupSubsidiaryName = GroupSubsidiaryName;
                perLifeAggregatedData.GroupSubsidiaryNo = GroupSubsidiaryNo;
                perLifeAggregatedData.CedingPlanCode2 = CedingPlanCode2;
                perLifeAggregatedData.DependantIndicator = DependantIndicator;
                perLifeAggregatedData.Mfrs17BasicRider = Mfrs17BasicRider;
                perLifeAggregatedData.Mfrs17CellName = Mfrs17CellName;
                perLifeAggregatedData.Mfrs17ContractCode = Mfrs17ContractCode;
                perLifeAggregatedData.LoaCode = LoaCode;
                perLifeAggregatedData.RiskPeriodStartDate = RiskPeriodStartDate;
                perLifeAggregatedData.RiskPeriodEndDate = RiskPeriodEndDate;
                perLifeAggregatedData.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                perLifeAggregatedData.BrokerageFee = BrokerageFee;
                perLifeAggregatedData.ApLoading = ApLoading;
                perLifeAggregatedData.EffectiveDate = EffectiveDate;
                perLifeAggregatedData.AnnuityFactor = AnnuityFactor;
                perLifeAggregatedData.EndingPolicyStatus = EndingPolicyStatus;
                perLifeAggregatedData.LastUpdatedDate = LastUpdatedDate;
                perLifeAggregatedData.TreatyType = TreatyType;
                perLifeAggregatedData.TreatyNumber = TreatyNumber;
                perLifeAggregatedData.RetroPremFreq = RetroPremFreq;
                perLifeAggregatedData.LifeBenefitFlag = LifeBenefitFlag;
                perLifeAggregatedData.RiskQuarter = RiskQuarter;
                perLifeAggregatedData.ProcessingDate = ProcessingDate;
                perLifeAggregatedData.UniqueKeyPerLife = UniqueKeyPerLife;
                perLifeAggregatedData.RetroBenefitCode = RetroBenefitCode;
                perLifeAggregatedData.RetroRatio = RetroRatio;
                perLifeAggregatedData.AccumulativeRetainAmount = AccumulativeRetainAmount;
                perLifeAggregatedData.RetroRetainAmount = RetroRetainAmount;
                perLifeAggregatedData.RetroAmount = RetroAmount;
                perLifeAggregatedData.RetroGrossPremium = RetroGrossPremium;
                perLifeAggregatedData.RetroNetPremium = RetroNetPremium;
                perLifeAggregatedData.RetroDiscount = RetroDiscount;
                perLifeAggregatedData.RetroExtraPremium = RetroExtraPremium;
                perLifeAggregatedData.RetroExtraComm = RetroExtraComm;
                perLifeAggregatedData.RetroGst = RetroGst;
                perLifeAggregatedData.RetroTreaty = RetroTreaty;
                perLifeAggregatedData.RetroClaimId = RetroClaimId;
                perLifeAggregatedData.Soa = Soa;
                perLifeAggregatedData.RetroIndicator = RetroIndicator;
                perLifeAggregatedData.UpdatedAt = DateTime.Now;
                perLifeAggregatedData.UpdatedById = UpdatedById ?? perLifeAggregatedData.UpdatedById;

                db.Entry(perLifeAggregatedData).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregatedData perLifeAggregatedData = db.PerLifeAggregatedData.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeAggregatedData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregatedData, true);

                db.Entry(perLifeAggregatedData).State = EntityState.Deleted;
                db.PerLifeAggregatedData.Remove(perLifeAggregatedData);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationDetailId(int perLifeAggregationDetailId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeAggregatedData.Where(q => q.PerLifeAggregationDetailId == perLifeAggregationDetailId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PerLifeAggregatedData perLifeAggregatedData in query.ToList())
                {
                    DataTrail trail = new DataTrail(perLifeAggregatedData, true);
                    trails.Add(trail);

                    db.Entry(perLifeAggregatedData).State = EntityState.Deleted;
                    db.PerLifeAggregatedData.Remove(perLifeAggregatedData);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
