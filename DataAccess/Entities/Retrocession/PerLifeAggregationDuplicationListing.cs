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

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeAggregationDuplicationListings")]
    public class PerLifeAggregationDuplicationListing
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TreatyCodeId { get; set; } // Get treatyId, TreatyCode, treatyType, LOB
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [MaxLength(100), Index]
        public string InsuredName { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(50), Index]
        public string PolicyNumber { get; set; }

        [Index]
        public DateTime? ReinsuranceEffectiveDate { get; set; }

        [Index]
        public int? FundsAccountingTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail FundsAccountingTypePickListDetail { get; set; }

        [Index]
        public int? ReinsBasisCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

        [MaxLength(255), Index]
        public string CedantPlanCode { get; set; }

        [Index]
        public int? MLReBenefitCodeId { get; set; }
        [ExcludeTrail]
        public virtual Benefit MLReBenefitCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(30)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }

        [Index]
        public int? TransactionTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail TransactionTypePickListDetail { get; set; }

        [Index]
        public int? ProceedToAggregate { get; set; } // Y and N

        [Index]
        public DateTime? DateUpdated { get; set; }

        [Index]
        public int? ExceptionStatusPickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail ExceptionStatusPickListDetail { get; set; }

        public string Remarks { get; set; }

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

        public PerLifeAggregationDuplicationListing()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDuplicationListings.Any(q => q.Id == id);
            }
        }

        public static PerLifeAggregationDuplicationListing Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDuplicationListings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeAggregationDuplicationListings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDuplicationListing perLifeAggregationDuplicationListing = Find(Id);
                if (perLifeAggregationDuplicationListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeAggregationDuplicationListing, this);

                perLifeAggregationDuplicationListing.TreatyCodeId = TreatyCodeId;
                perLifeAggregationDuplicationListing.InsuredName = InsuredName;
                perLifeAggregationDuplicationListing.InsuredDateOfBirth = InsuredDateOfBirth;
                perLifeAggregationDuplicationListing.PolicyNumber = PolicyNumber;
                perLifeAggregationDuplicationListing.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                perLifeAggregationDuplicationListing.MLReBenefitCodeId = MLReBenefitCodeId;
                perLifeAggregationDuplicationListing.CedingBenefitRiskCode = CedingBenefitRiskCode;
                perLifeAggregationDuplicationListing.CedingBenefitTypeCode = CedingBenefitTypeCode;
                perLifeAggregationDuplicationListing.ReinsuranceEffectiveDate = ReinsuranceEffectiveDate;
                perLifeAggregationDuplicationListing.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                perLifeAggregationDuplicationListing.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;
                perLifeAggregationDuplicationListing.TransactionTypePickListDetailId = TransactionTypePickListDetailId;
                perLifeAggregationDuplicationListing.ProceedToAggregate = ProceedToAggregate;
                perLifeAggregationDuplicationListing.DateUpdated = DateUpdated;
                perLifeAggregationDuplicationListing.ExceptionStatusPickListDetailId = ExceptionStatusPickListDetailId;
                perLifeAggregationDuplicationListing.Remarks = Remarks;



                perLifeAggregationDuplicationListing.UpdatedAt = DateTime.Now;
                perLifeAggregationDuplicationListing.UpdatedById = UpdatedById ?? perLifeAggregationDuplicationListing.UpdatedById;

                db.Entry(perLifeAggregationDuplicationListing).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeAggregationDuplicationListing PerLifeAggregationDuplicationListing = db.PerLifeAggregationDuplicationListings.Where(q => q.Id == id).FirstOrDefault();
                if (PerLifeAggregationDuplicationListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(PerLifeAggregationDuplicationListing, true);

                db.Entry(PerLifeAggregationDuplicationListing).State = EntityState.Deleted;
                db.PerLifeAggregationDuplicationListings.Remove(PerLifeAggregationDuplicationListing);
                db.SaveChanges();

                return trail;
            }
        }
    }
}