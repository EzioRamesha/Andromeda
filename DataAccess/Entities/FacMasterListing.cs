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
    [Table("FacMasterListings")]
    public class FacMasterListing
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255), Index]
        public string UniqueId { get; set; }

        [Index]
        public int? EwarpNumber { get; set; }

        [MaxLength(128)]
        public string InsuredName { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredGenderCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public int? CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public string PolicyNumber { get; set; }

        [Index]
        public double? FlatExtraAmountOffered { get; set; }

        [Index]
        public int? FlatExtraDuration { get; set; }

        public string BenefitCode { get; set; }

        [Index]
        public double? SumAssuredOffered { get; set; }

        [MaxLength(10), Index]
        public string EwarpActionCode { get; set; }

        [Index]
        public double? UwRatingOffered { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OfferLetterSentDate { get; set; }

        public string UwOpinion { get; set; }

        public string Remark { get; set; }

        public string CedingBenefitTypeCode { get; set; }

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
        public virtual ICollection<FacMasterListingDetail> FacMasterListingDetails { get; set; }

        public FacMasterListing()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateUniqueId()
        {
            using (var db = new AppDbContext())
            {
                var query = db.FacMasterListings.Where(q => q.UniqueId.Trim().Equals(UniqueId.Trim(), StringComparison.OrdinalIgnoreCase));
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static FacMasterListing Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static FacMasterListing Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<FacMasterListing> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.ToList();
            }
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodePickListDetailId).Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByBenefitCode(string benefitCode)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListings.Where(q => q.FacMasterListingDetails.Any(d => d.BenefitCode.Trim() == benefitCode.Trim())).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.FacMasterListings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                FacMasterListing facMasterListing = Find(Id);
                if (facMasterListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(facMasterListing, this);

                facMasterListing.UniqueId = UniqueId;
                facMasterListing.EwarpNumber = EwarpNumber;
                facMasterListing.InsuredName = InsuredName;
                facMasterListing.InsuredDateOfBirth = InsuredDateOfBirth;
                facMasterListing.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                facMasterListing.CedantId = CedantId;
                facMasterListing.PolicyNumber = PolicyNumber;
                facMasterListing.FlatExtraAmountOffered = FlatExtraAmountOffered;
                facMasterListing.FlatExtraDuration = FlatExtraDuration;
                facMasterListing.BenefitCode = BenefitCode;
                facMasterListing.SumAssuredOffered = SumAssuredOffered;
                facMasterListing.EwarpActionCode = EwarpActionCode;
                facMasterListing.UwRatingOffered = UwRatingOffered;
                facMasterListing.OfferLetterSentDate = OfferLetterSentDate;
                facMasterListing.UwOpinion = UwOpinion;
                facMasterListing.Remark = Remark;
                facMasterListing.CedingBenefitTypeCode = CedingBenefitTypeCode;
                facMasterListing.UpdatedAt = DateTime.Now;
                facMasterListing.UpdatedById = UpdatedById ?? facMasterListing.UpdatedById;

                db.Entry(facMasterListing).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                FacMasterListing facMasterListing = db.FacMasterListings.Where(q => q.Id == id).FirstOrDefault();
                if (facMasterListing == null)
                {
                    throw new Exception(MessageBag.NoRecordFound + ": " + id);
                }

                DataTrail trail = new DataTrail(facMasterListing, true);

                db.Entry(facMasterListing).State = EntityState.Deleted;
                db.FacMasterListings.Remove(facMasterListing);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
