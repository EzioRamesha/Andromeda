using BusinessObject;
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
    [Table("RateTables")]
    public class RateTable
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? RateTableMappingUploadId { get; set; }

        [ForeignKey(nameof(RateTableMappingUploadId))]
        [ExcludeTrail]
        public virtual RateTableMappingUpload RateTableMappingUpload { get; set; }

        [MaxLength(50), Index]
        public string RateTableCode { get; set; }

        [Required]
        public string TreatyCode { get; set; }

        [Index]
        public int? BenefitId { get; set; }

        [ForeignKey(nameof(BenefitId))]
        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        public string CedingPlanCode { get; set; }

        [Index]
        public int? PremiumFrequencyCodePickListDetailId { get; set; }

        [ForeignKey(nameof(PremiumFrequencyCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail PremiumFrequencyCodePickListDetail { get; set; }

        [Index]
        public double? PolicyAmountFrom { get; set; }

        [Index]
        public double? PolicyAmountTo { get; set; }

        [Index]
        public int? AttainedAgeFrom { get; set; }

        [Index]
        public int? AttainedAgeTo { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolStartDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsEffDatePolEndDate { get; set; }

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

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string GroupPolicyNumber { get; set; }

        [Index]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        [ForeignKey(nameof(ReinsBasisCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [Index]
        public double? AarFrom { get; set; }

        [Index]
        public double? AarTo { get; set; }

        // Phase 2
        [Index]
        public double? PolicyTermFrom { get; set; }

        [Index]
        public double? PolicyTermTo { get; set; }

        [Index]
        public double? PolicyDurationFrom { get; set; }

        [Index]
        public double? PolicyDurationTo { get; set; }

        [Index]
        public int? RateId { get; set; }

        [ForeignKey(nameof(RateId))]
        [ExcludeTrail]
        public virtual Rate Rate { get; set; }

        [Index]
        public int? CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int? RiDiscountId { get; set; }

        public int? LargeDiscountId { get; set; }

        public int? GroupDiscountId { get; set; }

        [MaxLength(30), Index]
        public string RiDiscountCode { get; set; }

        [MaxLength(30), Index]
        public string LargeDiscountCode { get; set; }

        [MaxLength(30), Index]
        public string GroupDiscountCode { get; set; }

        // Requested Fields (2020-03-11)
        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReportingStartDate { get; set; }

        [Index, Column(TypeName = "datetime2")]
        public DateTime? ReportingEndDate { get; set; }

        [ExcludeTrail]
        public virtual ICollection<RateTableDetail> RateTableDetails { get; set; }

        public RateTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public RateTable(RateTableBo rateTableBo) : this()
        {
            Id = rateTableBo.Id;
            TreatyCode = rateTableBo.TreatyCode;
            CedingTreatyCode = rateTableBo.CedingTreatyCode;
            BenefitId = rateTableBo.BenefitId;
            CedingPlanCode = rateTableBo.CedingPlanCode;
            CedingPlanCode2 = rateTableBo.CedingPlanCode2;
            CedingBenefitTypeCode = rateTableBo.CedingBenefitTypeCode;
            CedingBenefitRiskCode = rateTableBo.CedingBenefitRiskCode;
            GroupPolicyNumber = rateTableBo.GroupPolicyNumber;
            PremiumFrequencyCodePickListDetailId = rateTableBo.PremiumFrequencyCodePickListDetailId;
            ReinsBasisCodePickListDetailId = rateTableBo.ReinsBasisCodePickListDetailId;
            PolicyAmountFrom = rateTableBo.PolicyAmountFrom;
            PolicyAmountTo = rateTableBo.PolicyAmountTo;
            AttainedAgeFrom = rateTableBo.AttainedAgeFrom;
            AttainedAgeTo = rateTableBo.AttainedAgeTo;
            AarFrom = rateTableBo.AarFrom;
            AarFrom = rateTableBo.AarTo;
            ReinsEffDatePolStartDate = rateTableBo.ReinsEffDatePolStartDate;
            ReinsEffDatePolEndDate = rateTableBo.ReinsEffDatePolEndDate;

            // Phase 2
            PolicyTermFrom = rateTableBo.PolicyTermFrom;
            PolicyTermTo = rateTableBo.PolicyTermTo;
            PolicyDurationFrom = rateTableBo.PolicyDurationFrom;
            PolicyDurationTo = rateTableBo.PolicyDurationTo;
            RateId = rateTableBo.RateId;
            CedantId = rateTableBo.CedantId;
            RiDiscountCode = rateTableBo.RiDiscountCode;
            LargeDiscountCode = rateTableBo.LargeDiscountCode;
            GroupDiscountCode = rateTableBo.GroupDiscountCode;

            ReportingStartDate = rateTableBo.ReportingStartDate;
            ReportingEndDate = rateTableBo.ReportingEndDate;

            CreatedById = rateTableBo.CreatedById;
            UpdatedById = rateTableBo.UpdatedById;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Any(q => q.Id == id);
            }
        }

        public static RateTable Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RateTable");
                return connectionStrategy.Execute(() => db.RateTables.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Where(q => q.BenefitId == benefitId).Count();
            }
        }

        public static int CountByPremiumFrequencyCodePickListDetailId(int premiumFrequencyCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Where(q => q.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodePickListDetailId).Count();
            }
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Where(q => q.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId).Count();
            }
        }

        public static int CountByRateId(int rateId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Where(q => q.RateId == rateId).Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.RateTables.Where(q => q.CedantId == cedantId).Count();
            }
        }

        //public static int CountByRiDiscountId(int riDiscountId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.RateTables.Where(q => q.RiDiscountId == riDiscountId).Count();
        //    }
        //}

        //public static int CountByLargeDiscountId(int largeDiscountId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.RateTables.Where(q => q.LargeDiscountId == largeDiscountId).Count();
        //    }
        //}

        //public static int CountByGroupDiscountId(int groupDiscountId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.RateTables.Where(q => q.GroupDiscountId == groupDiscountId).Count();
        //    }
        //}

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RateTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RateTable rateTable = RateTable.Find(Id);
                if (rateTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateTable, this);

                rateTable.TreatyCode = TreatyCode;
                rateTable.CedingTreatyCode = CedingTreatyCode;
                rateTable.BenefitId = BenefitId;
                rateTable.CedingPlanCode = CedingPlanCode;
                rateTable.CedingPlanCode2 = CedingPlanCode2;
                rateTable.CedingBenefitTypeCode = CedingBenefitTypeCode;
                rateTable.CedingBenefitRiskCode = CedingBenefitRiskCode;
                rateTable.GroupPolicyNumber = GroupPolicyNumber;
                rateTable.PremiumFrequencyCodePickListDetailId = PremiumFrequencyCodePickListDetailId;
                rateTable.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;
                rateTable.PolicyAmountFrom = PolicyAmountFrom;
                rateTable.PolicyAmountTo = PolicyAmountTo;
                rateTable.AttainedAgeFrom = AttainedAgeFrom;
                rateTable.AttainedAgeTo = AttainedAgeTo;
                rateTable.AarFrom = AarFrom;
                rateTable.AarTo = AarTo;
                rateTable.ReinsEffDatePolStartDate = ReinsEffDatePolStartDate;
                rateTable.ReinsEffDatePolEndDate = ReinsEffDatePolEndDate;
                rateTable.UpdatedAt = DateTime.Now;
                rateTable.UpdatedById = UpdatedById ?? rateTable.UpdatedById;

                // Phase 2
                rateTable.PolicyTermFrom = PolicyTermFrom;
                rateTable.PolicyTermTo = PolicyTermTo;
                rateTable.PolicyDurationFrom = PolicyDurationFrom;
                rateTable.PolicyDurationTo = PolicyDurationTo;
                rateTable.RateId = RateId;
                rateTable.CedantId = CedantId;
                rateTable.RiDiscountCode = RiDiscountCode;
                rateTable.LargeDiscountCode = LargeDiscountCode;
                rateTable.GroupDiscountCode = GroupDiscountCode;

                rateTable.ReportingStartDate = ReportingStartDate;
                rateTable.ReportingEndDate = ReportingEndDate;

                db.Entry(rateTable).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RateTable rateTable = db.RateTables.Where(q => q.Id == id).FirstOrDefault();
                if (rateTable == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(rateTable, true);

                db.Entry(rateTable).State = EntityState.Deleted;
                db.RateTables.Remove(rateTable);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
