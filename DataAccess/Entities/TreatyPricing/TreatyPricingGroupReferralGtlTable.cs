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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferralGtlTables")]
    public class TreatyPricingGroupReferralGtlTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferral TreatyPricingGroupReferral { get; set; }

        [Index]
        public int? TreatyPricingGroupReferralFileId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralFile TreatyPricingGroupReferralFile { get; set; }

        [Index]
        public int Type { get; set; }

        [MaxLength(128)]
        public string BenefitCode { get; set; }

        [MaxLength(128)]
        public string Designation { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageEndDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EventDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClaimListDate { get; set; }

        [MaxLength(255)]
        public string ClaimantsName { get; set; }

        [MaxLength(128)]
        public string CauseOfClaim { get; set; }

        [MaxLength(128)]
        public string ClaimType { get; set; }

        [MaxLength(128)]
        public string AgeBandRange { get; set; }

        [MaxLength(128)]
        public string BasisOfSA { get; set; }

        public string GrossClaim { get; set; }

        public string RiClaim { get; set; }

        public string RiskRate { get; set; }

        public string GrossRate { get; set; }

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

        public TreatyPricingGroupReferralGtlTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGtlTables.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralGtlTable Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGtlTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.TreatyPricingGroupReferralGtlTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingGroupReferralGtlTables.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyPricingGroupReferralId = TreatyPricingGroupReferralId;
                entity.TreatyPricingGroupReferralFileId = TreatyPricingGroupReferralFileId;

                entity.BenefitCode = BenefitCode;
                entity.Designation = Designation;
                entity.CoverageStartDate = CoverageStartDate;
                entity.CoverageEndDate = CoverageEndDate;
                entity.EventDate = EventDate;
                entity.ClaimListDate = ClaimListDate;
                entity.ClaimantsName = ClaimantsName;
                entity.CauseOfClaim = CauseOfClaim;
                entity.ClaimType = ClaimType;
                entity.AgeBandRange = AgeBandRange;
                entity.BasisOfSA = BasisOfSA;
                entity.GrossClaim = GrossClaim;
                entity.RiClaim = RiClaim;
                entity.RiskRate = RiskRate;
                entity.GrossRate = GrossRate;
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
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingGroupReferralGtlTables.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
