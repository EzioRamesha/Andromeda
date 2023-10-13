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

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataCorrections")]
    public class  RiDataCorrection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]
        public virtual Cedant Cedant { get; set; }

        public int? TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, Index, MaxLength(150)]
        public string PolicyNumber { get; set; }

        [Index, MaxLength(30)]
        public string InsuredRegisterNo { get; set; }

        public int? InsuredGenderCodePickListDetailId { get; set; }

        [ForeignKey(nameof(InsuredGenderCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [MaxLength(128)]
        public string InsuredName { get; set; }

        [MaxLength(10)]
        public string CampaignCode { get; set; }

        [Index]
        public int? ReinsBasisCodePickListDetailId { get; set; }

        [ForeignKey(nameof(ReinsBasisCodePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail ReinsBasisCodePickListDetail { get; set; }

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

        [Index]
        public double? ApLoading { get; set; }

        public RiDataCorrection()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Any(q => q.Id == id);
            }
        }

        public static RiDataCorrection Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static RiDataCorrection FindByCedantIdTreatyCodeIdPolicyRegNo(int cedantId, string policyNumber, string registerNo = null, int? treadyCodeId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataCorrections
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.PolicyNumber.Trim() == policyNumber.Trim());
                if (!string.IsNullOrEmpty(registerNo))
                    query.Where(q => q.InsuredRegisterNo.Trim() == registerNo.Trim());
                if (treadyCodeId != null && treadyCodeId != 0)
                    query.Where(q => q.TreatyCodeId == treadyCodeId);
                return query.FirstOrDefault();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodePickListDetailId).Count();
            }
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataCorrections.Where(q => q.ReinsBasisCodePickListDetailId == reinsBasisCodePickListDetailId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataCorrections.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataCorrection riDataCorrections = Find(Id);
                if (riDataCorrections == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataCorrections, this);

                riDataCorrections.CedantId = CedantId;
                riDataCorrections.TreatyCodeId = TreatyCodeId;
                riDataCorrections.PolicyNumber = PolicyNumber;
                riDataCorrections.InsuredRegisterNo = InsuredRegisterNo;
                riDataCorrections.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                riDataCorrections.InsuredDateOfBirth = InsuredDateOfBirth;
                riDataCorrections.InsuredName = InsuredName;
                riDataCorrections.CampaignCode = CampaignCode;
                riDataCorrections.ReinsBasisCodePickListDetailId = ReinsBasisCodePickListDetailId;
                riDataCorrections.ApLoading = ApLoading;
                riDataCorrections.UpdatedAt = DateTime.Now;
                riDataCorrections.UpdatedById = UpdatedById ?? riDataCorrections.UpdatedById;

                db.Entry(riDataCorrections).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataCorrection riDataCorrections = db.RiDataCorrections.Where(q => q.Id == id).FirstOrDefault();
                if (riDataCorrections == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataCorrections, true);


                db.Entry(riDataCorrections).State = EntityState.Deleted;
                db.RiDataCorrections.Remove(riDataCorrections);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
