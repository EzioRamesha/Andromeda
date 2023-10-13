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
    [Table("ValidDuplicationLists")]
    public class ValidDuplicationList
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TreatyCodeId { get; set; } // Get treatyId, TreatyCode, treatyType, LOB
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [MaxLength(255), Index]
        public string CedantPlanCode { get; set; }

        [MaxLength(255), Index]
        public string InsuredName { get; set; }

        [Index]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(50), Index]
        public string PolicyNumber { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Index]
        public int? MLReBenefitCodeId { get; set; }
        [ExcludeTrail]
        public virtual Benefit MLReBenefitCode { get; set; }

        [MaxLength(100)]
        [Index]
        public string CedingBenefitRiskCode { get; set; }

        [MaxLength(100)]
        [Index]
        public string CedingBenefitTypeCode { get; set; }

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

        [Index]
        public int? TransactionTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail TransactionTypePickListDetail { get; set; }

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

        public ValidDuplicationList()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ValidDuplicationLists.Any(q => q.Id == id);
            }
        }

        public static ValidDuplicationList Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ValidDuplicationLists.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ValidDuplicationLists.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ValidDuplicationList validDuplicationList = Find(Id);
                if (validDuplicationList == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(validDuplicationList, this);

                validDuplicationList.TreatyCodeId = TreatyCodeId;
                validDuplicationList.CedantPlanCode = CedantPlanCode;
                validDuplicationList.InsuredName = InsuredName;
                validDuplicationList.InsuredDateOfBirth = InsuredDateOfBirth;
                validDuplicationList.PolicyNumber = PolicyNumber;
                validDuplicationList.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                validDuplicationList.MLReBenefitCodeId = MLReBenefitCodeId;
                validDuplicationList.CedingBenefitRiskCode = CedingBenefitRiskCode;
                validDuplicationList.CedingBenefitTypeCode = CedingBenefitTypeCode;
                validDuplicationList.ReinsuranceEffectiveDate = ReinsuranceEffectiveDate;
                validDuplicationList.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
                validDuplicationList.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;
                validDuplicationList.TransactionTypePickListDetailId = TransactionTypePickListDetailId;


                validDuplicationList.UpdatedAt = DateTime.Now;
                validDuplicationList.UpdatedById = UpdatedById ?? validDuplicationList.UpdatedById;

                db.Entry(validDuplicationList).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ValidDuplicationList ValidDuplicationList = db.ValidDuplicationLists.Where(q => q.Id == id).FirstOrDefault();
                if (ValidDuplicationList == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(ValidDuplicationList, true);

                db.Entry(ValidDuplicationList).State = EntityState.Deleted;
                db.ValidDuplicationLists.Remove(ValidDuplicationList);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
