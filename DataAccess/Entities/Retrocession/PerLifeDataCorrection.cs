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
    [Table("PerLifeDataCorrections")]
    public class PerLifeDataCorrection
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, MaxLength(128)]
        [Index]
        public string InsuredName { get; set; }

        [Required, Column(TypeName = "datetime2")]
        [Index]
        public DateTime InsuredDateOfBirth { get; set; }

        [Required, MaxLength(150)]
        [Index]
        public string PolicyNumber { get; set; }

        [Required, Index]
        public int InsuredGenderCodePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredGenderCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Required, Index]
        public int TerritoryOfIssueCodePickListDetailId { get; set; }

        [ForeignKey(nameof(TerritoryOfIssueCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

        [Required, Index]
        public int PerLifeRetroGenderId { get; set; }

        [ForeignKey(nameof(PerLifeRetroGenderId))]
        [ExcludeTrail]
        public virtual PerLifeRetroGender PerLifeRetroGender { get; set; }

        [Required, Index]
        public int PerLifeRetroCountryId { get; set; }

        [ForeignKey(nameof(PerLifeRetroCountryId))]
        [ExcludeTrail]
        public virtual PerLifeRetroCountry PerLifeRetroCountry { get; set; }

        [Required, Column(TypeName = "datetime2")]
        [Index]
        public DateTime DateOfPolicyExist { get; set; }

        [Required, Index]
        public bool IsProceedToAggregate { get; set; } = false;

        [Required, Column(TypeName = "datetime2")]
        [Index]
        public DateTime DateOfExceptionDetected { get; set; }

        [Required, Index]
        public int ExceptionStatusPickListDetailId { get; set; }

        [ForeignKey(nameof(ExceptionStatusPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ExceptionStatusPickListDetail { get; set; }

        [Column(TypeName = "ntext")]
        public string Remark { get; set; }

        [Required, Column(TypeName = "datetime2")]
        [Index]
        public DateTime DateUpdated { get; set; }

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

        public PerLifeDataCorrection()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeDataCorrections
                    .Where(q => q.TreatyCodeId == TreatyCodeId)
                    .Where(q => q.InsuredName == InsuredName)
                    .Where(q => q.InsuredDateOfBirth == InsuredDateOfBirth)
                    .Where(q => q.PolicyNumber == PolicyNumber)
                    .Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId)
                    .Where(q => q.TerritoryOfIssueCodePickListDetailId == TerritoryOfIssueCodePickListDetailId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Any();
            }
        }

        public static PerLifeDataCorrection Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeDataCorrections.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeDataCorrection perLifeDataCorrection = Find(Id);
                if (perLifeDataCorrection == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeDataCorrection, this);

                perLifeDataCorrection.TreatyCodeId = TreatyCodeId;
                perLifeDataCorrection.InsuredName = InsuredName;
                perLifeDataCorrection.InsuredDateOfBirth = InsuredDateOfBirth;
                perLifeDataCorrection.PolicyNumber = PolicyNumber;
                perLifeDataCorrection.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                perLifeDataCorrection.TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId;
                perLifeDataCorrection.PerLifeRetroGenderId = PerLifeRetroGenderId;
                perLifeDataCorrection.PerLifeRetroCountryId = PerLifeRetroCountryId;
                perLifeDataCorrection.DateOfPolicyExist = DateOfPolicyExist;
                perLifeDataCorrection.IsProceedToAggregate = IsProceedToAggregate;
                perLifeDataCorrection.DateOfExceptionDetected = DateOfExceptionDetected;
                perLifeDataCorrection.ExceptionStatusPickListDetailId = ExceptionStatusPickListDetailId;
                perLifeDataCorrection.Remark = Remark;
                perLifeDataCorrection.DateUpdated = DateUpdated;
                perLifeDataCorrection.UpdatedAt = DateTime.Now;
                perLifeDataCorrection.UpdatedById = UpdatedById ?? perLifeDataCorrection.UpdatedById;

                db.Entry(perLifeDataCorrection).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeDataCorrection perLifeDataCorrection = db.PerLifeDataCorrections.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeDataCorrection == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeDataCorrection, true);

                db.Entry(perLifeDataCorrection).State = EntityState.Deleted;
                db.PerLifeDataCorrections.Remove(perLifeDataCorrection);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
