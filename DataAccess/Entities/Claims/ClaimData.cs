using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
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

namespace DataAccess.Entities.Claims
{
    [Table("ClaimData")]
    public class ClaimData
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimDataBatchId { get; set; }
        [ExcludeTrail]
        public virtual ClaimDataBatch ClaimDataBatch { get; set; }

        [Index]
        public int? ClaimDataFileId { get; set; }
        [ExcludeTrail]
        public virtual ClaimDataFile ClaimDataFile { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimId { get; set; }
        [MaxLength(30)]
        [Index]
        public string ClaimCode { get; set; }

        public bool CopyAndOverwriteData { get; set; } = false;

        [Index]
        public int MappingStatus { get; set; }
        [Index]
        public int PreComputationStatus { get; set; }
        [Index]
        public int PreValidationStatus { get; set; }
        [Index]
        public int ReportingStatus { get; set; }


        public string Errors { get; set; }
        public string CustomField { get; set; }

        [MaxLength(150)]
        [Index]
        public string PolicyNumber { get; set; }

        [Index]
        public double? PolicyTerm { get; set; }

        [Index]
        public double? ClaimRecoveryAmt { get; set; }

        [MaxLength(30)]
        [Index]
        public string ClaimTransactionType { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyCode { get; set; }

        [MaxLength(35)]
        [Index]
        public string TreatyType { get; set; }

        [Index]
        public double? AarPayable { get; set; }

        [Index]
        public double? AnnualRiPrem { get; set; }

        [MaxLength(255)]
        [Index]
        public string CauseOfEvent { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedantClaimEventCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedantClaimType { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? CedantDateOfNotification { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string CedingClaimType { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingCompany { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string CedingEventCode { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string CedingPlanCode { get; set; }

        [Index]
        public double? CurrencyRate { get; set; }

        [MaxLength(3)]
        [Index]
        public string CurrencyCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateApproved { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfEvent { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string EntryNo { get; set; }

        [Index]
        public double? ExGratia { get; set; }

        [Index]
        public double? ForeignClaimRecoveryAmt { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string FundsAccountingTypeCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? InsuredDateOfBirth { get; set; }
        
        [MaxLength(1)]
        [Index]
        public string InsuredGenderCode { get; set; }
        
        [MaxLength(128)]
        [Index]
        public string InsuredName { get; set; }
        
        [MaxLength(1)]
        [Index]
        public string InsuredTobaccoUse { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? LastTransactionDate { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string LastTransactionQuarter { get; set; }

        [Index]
        public double? LateInterest { get; set; }

        [Index]
        public double? Layer1SumRein { get; set; }

        [Index]
        public int? Mfrs17AnnualCohort { get; set; }

        [MaxLength(30)]
        [Index]
        public string Mfrs17ContractCode { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string MlreBenefitCode { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string MlreEventCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? MlreInvoiceDate { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string MlreInvoiceNumber { get; set; }

        [Index]
        public double? MlreRetainAmount { get; set; }

        [Index]
        public double? MlreShare { get; set; }

        [Index]
        public int? PendingProvisionDay { get; set; }

        [Index]
        public int? PolicyDuration { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string ReinsBasisCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? ReinsEffDatePol { get; set; }
        
        [MaxLength(128)]
        [Index]
        public string RetroParty1 { get; set; }
        
        [MaxLength(128)]
        [Index]
        public string RetroParty2 { get; set; }

        [MaxLength(128)]
        [Index]
        public string RetroParty3 { get; set; }

        [Index]
        public double? RetroRecovery1 { get; set; }

        [Index]
        public double? RetroRecovery2 { get; set; }

        [Index]
        public double? RetroRecovery3 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate1 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? RetroStatementDate3 { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string RetroStatementId1 { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string RetroStatementId2 { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string RetroStatementId3 { get; set; }

        [Index]
        public int? RiskPeriodMonth { get; set; }

        [Index]
        public int? RiskPeriodYear { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string RiskQuarter { get; set; }

        [Index]
        public double? SaFactor { get; set; }
        
        [MaxLength(30)]
        [Index]
        public string SoaQuarter { get; set; }

        [Index]
        public double? SumIns { get; set; }

        [Index]
        public double? TempA1 { get; set; }

        [Index]
        public double? TempA2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TempD1 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TempD2 { get; set; }

        [Index]
        public int? TempI1 { get; set; }

        [Index]
        public int? TempI2 { get; set; }
        
        [MaxLength(150)]
        [Index]
        public string TempS1 { get; set; }

        [MaxLength(50)]
        [Index]
        public string TempS2 { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? TransactionDateWop { get; set; }

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

        [Column(TypeName = "datetime2"), Index]
        public DateTime? IssueDatePol { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? PolicyExpiryDate { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfReported { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingTreatyCode { get; set; }

        [MaxLength(10)]
        [Index]
        public string CampaignCode { get; set; }

        [Column(TypeName = "datetime2"), Index]
        public DateTime? DateOfIntimation { get; set; }

        public ClaimData()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData.Any(q => q.Id == id);
            }
        }

        public static ClaimData Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Count();
            }
        }

        public static int CountByClaimDataFileIdMappingStatus(int claimDataBatchId, int mappingStatus)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Where(q => q.MappingStatus == mappingStatus)
                    .Count();
            }
        }

        public static IList<ClaimData> GetByClaimDataBatchId(int claimDataBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public static IList<ClaimData> GetByClaimDataBatchIdClaimDataFileId(int claimDataBatchId, int claimDataFileId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData.Where(q => q.ClaimDataBatchId == claimDataBatchId && q.ClaimDataFileId == claimDataFileId).ToList();
            }
        }

        public static List<int> GetIdsByClaimDataBatchId(int claimDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimData.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.ClaimData.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimData claimData = Find(Id);
                if (claimData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimData, this);

                claimData.ClaimDataBatchId = ClaimDataBatchId;
                claimData.ClaimDataFileId = ClaimDataFileId;
                claimData.ClaimId = ClaimId;
                claimData.ClaimCode = ClaimCode;
                claimData.CopyAndOverwriteData = CopyAndOverwriteData;

                claimData.MappingStatus = MappingStatus;
                claimData.PreComputationStatus = PreComputationStatus;
                claimData.PreValidationStatus = PreValidationStatus;
                claimData.ReportingStatus = ReportingStatus;

                claimData.Errors = Errors;
                claimData.CustomField = CustomField;

                claimData.PolicyNumber = PolicyNumber;
                claimData.PolicyTerm = PolicyTerm;
                claimData.ClaimRecoveryAmt = ClaimRecoveryAmt;
                claimData.ClaimTransactionType = ClaimTransactionType;
                claimData.TreatyCode = TreatyCode;
                claimData.TreatyType = TreatyType;
                claimData.AarPayable = AarPayable;
                claimData.AnnualRiPrem = AnnualRiPrem;
                claimData.CauseOfEvent = CauseOfEvent;
                claimData.CedantClaimEventCode = CedantClaimEventCode;
                claimData.CedantClaimType = CedantClaimType;
                claimData.CedantDateOfNotification = CedantDateOfNotification;
                claimData.CedingBenefitRiskCode = CedingBenefitRiskCode;
                claimData.CedingBenefitTypeCode = CedingBenefitTypeCode;
                claimData.CedingClaimType = CedingClaimType;
                claimData.CedingCompany = CedingCompany;
                claimData.CedingEventCode = CedingEventCode;
                claimData.CedingPlanCode = CedingPlanCode;
                claimData.CurrencyRate = CurrencyRate;
                claimData.CurrencyCode = CurrencyCode;
                claimData.DateApproved = DateApproved;
                claimData.DateOfEvent = DateOfEvent;
                claimData.EntryNo = EntryNo;
                claimData.ExGratia = ExGratia;
                claimData.ForeignClaimRecoveryAmt = ForeignClaimRecoveryAmt;
                claimData.FundsAccountingTypeCode = FundsAccountingTypeCode;
                claimData.InsuredDateOfBirth = InsuredDateOfBirth;
                claimData.InsuredGenderCode = InsuredGenderCode;
                claimData.InsuredName = InsuredName;
                claimData.InsuredTobaccoUse = InsuredTobaccoUse;
                claimData.LastTransactionDate = LastTransactionDate;
                claimData.LastTransactionQuarter = LastTransactionQuarter;
                claimData.LateInterest = LateInterest;
                claimData.Layer1SumRein = Layer1SumRein;
                claimData.Mfrs17AnnualCohort = Mfrs17AnnualCohort;
                claimData.Mfrs17ContractCode = Mfrs17ContractCode;
                claimData.MlreBenefitCode = MlreBenefitCode;
                claimData.MlreEventCode = MlreEventCode;
                claimData.MlreInvoiceDate = MlreInvoiceDate;
                claimData.MlreInvoiceNumber = MlreInvoiceNumber;
                claimData.MlreRetainAmount = MlreRetainAmount;
                claimData.MlreShare = MlreShare;
                claimData.PendingProvisionDay = PendingProvisionDay;
                claimData.PolicyDuration = PolicyDuration;
                claimData.ReinsBasisCode = ReinsBasisCode;
                claimData.ReinsEffDatePol = ReinsEffDatePol;
                claimData.RetroParty1 = RetroParty1;
                claimData.RetroParty2 = RetroParty2;
                claimData.RetroParty3 = RetroParty3;
                claimData.RetroRecovery1 = RetroRecovery1;
                claimData.RetroRecovery2 = RetroRecovery2;
                claimData.RetroRecovery3 = RetroRecovery3;
                claimData.RetroStatementDate1 = RetroStatementDate1;
                claimData.RetroStatementDate2 = RetroStatementDate2;
                claimData.RetroStatementDate3 = RetroStatementDate3;
                claimData.RetroStatementId1 = RetroStatementId1;
                claimData.RetroStatementId2 = RetroStatementId2;
                claimData.RetroStatementId3 = RetroStatementId3;
                claimData.RiskPeriodMonth = RiskPeriodMonth;
                claimData.RiskPeriodYear = RiskPeriodYear;
                claimData.RiskQuarter = RiskQuarter;
                claimData.SaFactor = SaFactor;
                claimData.SoaQuarter = SoaQuarter;
                claimData.SumIns = SumIns;
                claimData.TempA1 = TempA1;
                claimData.TempA2 = TempA2;
                claimData.TempD1 = TempD1;
                claimData.TempD2 = TempD2;
                claimData.TempI1 = TempI1;
                claimData.TempI2 = TempI2;
                claimData.TempS1 = TempS1;
                claimData.TempS2 = TempS2;
                claimData.TransactionDateWop = TransactionDateWop;
                claimData.IssueDatePol = IssueDatePol;
                claimData.PolicyExpiryDate = PolicyExpiryDate;
                claimData.DateOfReported = DateOfReported;
                claimData.CedingTreatyCode = CedingTreatyCode;
                claimData.CampaignCode = CampaignCode;
                claimData.DateOfIntimation = DateOfIntimation;

                claimData.UpdatedAt = DateTime.Now;
                claimData.UpdatedById = UpdatedById ?? claimData.UpdatedById;

                db.Entry(claimData).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimData claimData = db.ClaimData.Where(q => q.Id == id).FirstOrDefault();
                if (claimData == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimData, true);

                db.Entry(claimData).State = EntityState.Deleted;
                db.ClaimData.Remove(claimData);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
