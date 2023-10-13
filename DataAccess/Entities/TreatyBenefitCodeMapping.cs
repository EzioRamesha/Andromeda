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

namespace DataAccess.Entities
{
    [Table("TreatyBenefitCodeMappings")]
    public class TreatyBenefitCodeMapping
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TreatyBenefitCodeMappingUploadId { get; set; }

        [ForeignKey(nameof(TreatyBenefitCodeMappingUploadId))]
        [ExcludeTrail]
        public virtual TreatyBenefitCodeMappingUpload TreatyBenefitCodeMappingUpload { get; set; }

        [Required, Index]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, Index]
        public int BenefitId { get; set; }

        [ForeignKey(nameof(BenefitId))]
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [Required]
        public string CedingPlanCode { get; set; }

        public string Description { get; set; }

        [Required]
        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CampaignCode { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

        [Index]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        [ForeignKey(nameof(ReinsBasisCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [Index]
        public int? AttainedAgeFrom { get; set; }

        [Index]
        public int? AttainedAgeTo { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReportingStartDate { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReportingEndDate { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<TreatyBenefitCodeMappingDetail> TreatyBenefitCodeMappingDetails { get; set; }

        [MaxLength(1), Index]
        public string ProfitComm { get; set; }

        [Index]
        public int? ProfitCommPickListDetailId { get; set; }

        [ForeignKey(nameof(ProfitCommPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ProfitCommPickListDetail { get; set; }

        [Index]
        public int? MaxExpiryAge { get; set; }

        [Index]
        public int? MinIssueAge { get; set; }

        [Index]
        public int? MaxIssueAge { get; set; }

        [Index]
        public double? MaxUwRating { get; set; }

        [Index]
        public double? ApLoading { get; set; }

        [Index]
        public double? MinAar { get; set; }

        [Index]
        public double? MaxAar { get; set; }

        [Index]
        public double? AblAmount { get; set; }

        [Index]
        public double? RetentionShare { get; set; }

        [Index]
        public double? RetentionCap { get; set; }

        [Index]
        public double? RiShare { get; set; }

        [Index]
        public double? RiShareCap { get; set; }

        [Index]
        public double? ServiceFee { get; set; }

        [Index]
        public double? WakalahFee { get; set; }

        [Index]
        public double? UnderwriterRatingFrom { get; set; }

        [Index]
        public double? UnderwriterRatingTo { get; set; }

        [Index]
        public double? RiShare2 { get; set; }

        [Index]
        public double? RiShareCap2 { get; set; }

        [Index]
        public double? OriSumAssuredFrom { get; set; }

        [Index]
        public double? OriSumAssuredTo { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? EffectiveDate { get; set; }

        // New field added (2020-03-11)
        [Index]
        public int? ReinsuranceIssueAgeFrom { get; set; }

        [Index]
        public int? ReinsuranceIssueAgeTo { get; set; }

        public TreatyBenefitCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Any(q => q.Id == id);
            }
        }

        public static TreatyBenefitCodeMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.BenefitId == benefitId).Count();
            }
        }

        public static int CountByCedingBenefitTypeCode(string cedingBenefitTypeCode)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.CedingBenefitTypeCode.Contains(cedingBenefitTypeCode)).Count();
            }
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId).Count();
            }
        }

        public static IList<TreatyBenefitCodeMapping> GetByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappings.Where(q => q.CedantId == cedantId).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyBenefitCodeMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyBenefitCodeMapping treatyBenefitCodeMapping = TreatyBenefitCodeMapping.Find(Id);
                if (treatyBenefitCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyBenefitCodeMapping, this);

                treatyBenefitCodeMapping.CedantId = CedantId;
                treatyBenefitCodeMapping.BenefitId = BenefitId;
                treatyBenefitCodeMapping.TreatyCodeId = TreatyCodeId;

                treatyBenefitCodeMapping.CedingPlanCode = CedingPlanCode;
                treatyBenefitCodeMapping.Description = Description;
                treatyBenefitCodeMapping.CedingBenefitTypeCode = CedingBenefitTypeCode;
                treatyBenefitCodeMapping.CedingBenefitRiskCode = CedingBenefitRiskCode;
                treatyBenefitCodeMapping.CedingTreatyCode = CedingTreatyCode;
                treatyBenefitCodeMapping.CampaignCode = CampaignCode;

                treatyBenefitCodeMapping.ReinsEffDatePolStartDate = ReinsEffDatePolStartDate;
                treatyBenefitCodeMapping.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;

                treatyBenefitCodeMapping.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;

                treatyBenefitCodeMapping.AttainedAgeFrom = AttainedAgeFrom;
                treatyBenefitCodeMapping.AttainedAgeTo = AttainedAgeTo;

                treatyBenefitCodeMapping.ReportingStartDate = ReportingStartDate;
                treatyBenefitCodeMapping.ReportingEndDate = ReportingEndDate;

                treatyBenefitCodeMapping.UpdatedAt = DateTime.Now;
                treatyBenefitCodeMapping.UpdatedById = UpdatedById ?? treatyBenefitCodeMapping.UpdatedById;

                treatyBenefitCodeMapping.TreatyBenefitCodeMappingUploadId = TreatyBenefitCodeMappingUploadId ?? treatyBenefitCodeMapping.TreatyBenefitCodeMappingUploadId;

                // Phase 2
                treatyBenefitCodeMapping.ProfitComm = ProfitComm;
                treatyBenefitCodeMapping.ProfitCommPickListDetailId = ProfitCommPickListDetailId;
                treatyBenefitCodeMapping.MaxExpiryAge = MaxExpiryAge;
                treatyBenefitCodeMapping.MinIssueAge = MinIssueAge;
                treatyBenefitCodeMapping.MaxIssueAge = MaxIssueAge;
                treatyBenefitCodeMapping.MaxUwRating = MaxUwRating;
                treatyBenefitCodeMapping.ApLoading = ApLoading;
                treatyBenefitCodeMapping.MinAar = MinAar;
                treatyBenefitCodeMapping.MaxAar = MaxAar;
                treatyBenefitCodeMapping.AblAmount = AblAmount;
                treatyBenefitCodeMapping.RetentionShare = RetentionShare;
                treatyBenefitCodeMapping.RetentionCap = RetentionCap;
                treatyBenefitCodeMapping.RiShare = RiShare;
                treatyBenefitCodeMapping.RiShareCap = RiShareCap;
                treatyBenefitCodeMapping.ServiceFee = ServiceFee;
                treatyBenefitCodeMapping.WakalahFee = WakalahFee;
                treatyBenefitCodeMapping.UnderwriterRatingFrom = UnderwriterRatingFrom;
                treatyBenefitCodeMapping.UnderwriterRatingTo = UnderwriterRatingTo;
                treatyBenefitCodeMapping.RiShare2 = RiShare2;
                treatyBenefitCodeMapping.RiShareCap2 = RiShareCap2;

                treatyBenefitCodeMapping.OriSumAssuredFrom = OriSumAssuredFrom;
                treatyBenefitCodeMapping.OriSumAssuredTo = OriSumAssuredTo;

                treatyBenefitCodeMapping.EffectiveDate = EffectiveDate;

                treatyBenefitCodeMapping.ReinsuranceIssueAgeFrom = ReinsuranceIssueAgeFrom;
                treatyBenefitCodeMapping.ReinsuranceIssueAgeTo = ReinsuranceIssueAgeTo;

                db.Entry(treatyBenefitCodeMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyBenefitCodeMapping treatyBenefitCodeMapping = db.TreatyBenefitCodeMappings.Where(q => q.Id == id).FirstOrDefault();
                if (treatyBenefitCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyBenefitCodeMapping, true);

                db.Entry(treatyBenefitCodeMapping).State = EntityState.Deleted;
                db.TreatyBenefitCodeMappings.Remove(treatyBenefitCodeMapping);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
