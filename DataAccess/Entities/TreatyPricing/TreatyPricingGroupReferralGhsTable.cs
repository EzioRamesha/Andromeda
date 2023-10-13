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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferralGhsTables")]
    public class TreatyPricingGroupReferralGhsTable
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

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EventDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClaimListDate { get; set; }

        [MaxLength(255)]
        public string ClaimantsName { get; set; }

        [MaxLength(128)]
        public string CauseOfClaim { get; set; }

        public string RbCovered { get; set; }

        public string AolCovered { get; set; }

        public string Relationship { get; set; }

        [MaxLength(255)]
        public string HospitalCovered { get; set; }

        public string GrossClaimIncurred { get; set; }

        public string GrossClaimPaid { get; set; }

        public string GrossClaimPaidIbnr { get; set; }

        public string RiClaimPaid { get; set; }

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

        public TreatyPricingGroupReferralGhsTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGhsTables.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralGhsTable Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralGhsTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.TreatyPricingGroupReferralGhsTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingGroupReferralGhsTables.Add(this);
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

                entity.CoverageStartDate = CoverageStartDate;
                entity.EventDate = EventDate;
                entity.ClaimListDate = ClaimListDate;
                entity.ClaimantsName = ClaimantsName;
                entity.CauseOfClaim = CauseOfClaim;
                entity.RbCovered = RbCovered;
                entity.AolCovered = AolCovered;
                entity.Relationship = Relationship;
                entity.HospitalCovered = HospitalCovered;
                entity.GrossClaimIncurred = GrossClaimIncurred;
                entity.GrossClaimPaid = GrossClaimPaid;
                entity.GrossClaimPaidIbnr = GrossClaimPaidIbnr;
                entity.RiClaimPaid = RiClaimPaid;
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
                db.TreatyPricingGroupReferralGhsTables.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
