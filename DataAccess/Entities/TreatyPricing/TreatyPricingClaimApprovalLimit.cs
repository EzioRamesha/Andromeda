using BusinessObject.TreatyPricing;
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
    [Table("TreatyPricingClaimApprovalLimits")]
    public class TreatyPricingClaimApprovalLimit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(255), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        public string Description { get; set; }

        [Index]
        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        public string Remarks { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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

        public TreatyPricingClaimApprovalLimit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingClaimApprovalLimits.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingClaimApprovalLimits.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static TreatyPricingClaimApprovalLimit Find (int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingClaimApprovalLimits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingClaimApprovalLimits.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingClaimApprovalLimit treatyPricingClaimApprovalLimit = Find(Id);
                if (treatyPricingClaimApprovalLimit == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingClaimApprovalLimit, this);

                treatyPricingClaimApprovalLimit.TreatyPricingCedantId = TreatyPricingCedantId;
                treatyPricingClaimApprovalLimit.Code = Code;
                treatyPricingClaimApprovalLimit.Name = Name;
                treatyPricingClaimApprovalLimit.BenefitCode = BenefitCode;
                treatyPricingClaimApprovalLimit.Description = Description;
                treatyPricingClaimApprovalLimit.Status = Status;
                treatyPricingClaimApprovalLimit.Remarks = Remarks;
                treatyPricingClaimApprovalLimit.UpdatedAt = DateTime.Now;
                treatyPricingClaimApprovalLimit.UpdatedById = UpdatedById ?? treatyPricingClaimApprovalLimit.UpdatedById;

                db.Entry(treatyPricingClaimApprovalLimit).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingClaimApprovalLimit treatyPricingClaimApprovalLimit = db.TreatyPricingClaimApprovalLimits.Where(q => q.Id == id).FirstOrDefault();
                if (treatyPricingClaimApprovalLimit == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyPricingClaimApprovalLimit, true);

                db.Entry(treatyPricingClaimApprovalLimit).State = EntityState.Deleted;
                db.TreatyPricingClaimApprovalLimits.Remove(treatyPricingClaimApprovalLimit);
                db.SaveChanges();

                return trail;
            }
        }

    }
}
