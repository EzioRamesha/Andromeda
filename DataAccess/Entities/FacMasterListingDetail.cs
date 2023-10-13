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
    [Table("FacMasterListingDetails")]
    public class FacMasterListingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int FacMasterListingId { get; set; }

        [ForeignKey(nameof(FacMasterListingId))]
        [ExcludeTrail]
        public virtual FacMasterListing FacMasterListing { get; set; }

        [MaxLength(150), Index]
        public string PolicyNumber { get; set; }

        [MaxLength(10), Index]
        public string BenefitCode { get; set; }

        [MaxLength(10), Index]
        public string CedingBenefitTypeCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public FacMasterListingDetail()
        {
            CreatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListingDetails.Any(q => q.Id == id);
            }
        }

        public static IQueryable<FacMasterListingDetail> QueryByParams(
            AppDbContext db,
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            db.Database.CommandTimeout = 0;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(73, "FacMasterListingDetail");

            return connectionStrategy.Execute(() =>
            {
                var query = db.FacMasterListingDetails
                 .Where(q => q.FacMasterListing.InsuredName.Trim() == insuredName.Trim())
                 .Where(q => q.PolicyNumber.Trim() == policyNumber.Trim())
                 .Where(q => q.BenefitCode.Trim() == benefitCode.Trim());

                if (!string.IsNullOrEmpty(cedingBenefitTypeCode))
                {
                    connectionStrategy.Reset(83);
                    query = query.Where(q =>
                            (!string.IsNullOrEmpty(q.CedingBenefitTypeCode) && q.CedingBenefitTypeCode.Trim() == cedingBenefitTypeCode.Trim())
                            ||
                            q.CedingBenefitTypeCode == null
                        );
                }
                else
                {
                    connectionStrategy.Reset(92);
                    query = query.Where(q => q.CedingBenefitTypeCode == null);
                }

                // NOTE: Group by should put at the end of query
                if (groupById)
                {
                    connectionStrategy.Reset(99);
                    query = query.GroupBy(q => q.FacMasterListingId).Select(q => q.FirstOrDefault());
                }

                return query;
            });

        }

        public static FacMasterListingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static FacMasterListingDetail FindByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    insuredName,
                    policyNumber,
                    benefitCode,
                    cedingBenefitTypeCode,
                    groupById
                ).FirstOrDefault();
            }
        }

        public static int CountByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    insuredName,
                    policyNumber,
                    benefitCode,
                    cedingBenefitTypeCode,
                    groupById
                ).Count();
            }
        }

        public static IList<FacMasterListingDetail> GetByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            using (var db = new AppDbContext())
            {
                return QueryByParams(
                    db,
                    insuredName,
                    policyNumber,
                    benefitCode,
                    cedingBenefitTypeCode,
                    groupById
                ).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.FacMasterListingDetails.Add(this);
                db.SaveChanges();

                var trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, this);

                entity.FacMasterListingId = FacMasterListingId;
                entity.PolicyNumber = PolicyNumber;
                entity.BenefitCode = BenefitCode;
                entity.CedingBenefitTypeCode = CedingBenefitTypeCode;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.FacMasterListingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.FacMasterListingDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByFacMasterListingId(int facMasterListingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.FacMasterListingDetails.Where(q => q.FacMasterListingId == facMasterListingId);

                var trails = new List<DataTrail>();
                foreach (FacMasterListingDetail facMasterListingDetail in query.ToList())
                {
                    var trail = new DataTrail(facMasterListingDetail, true);
                    trails.Add(trail);

                    db.Entry(facMasterListingDetail).State = EntityState.Deleted;
                    db.FacMasterListingDetails.Remove(facMasterListingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
