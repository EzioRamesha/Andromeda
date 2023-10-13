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
    [Table("RetroBenefitCodes")]
    public class RetroBenefitCode
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string Code { get; set; }

        [Required, MaxLength(255), Index]
        public string Description { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime CeaseDate { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [MaxLength(255), Column(TypeName = "ntext")]
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

        public RetroBenefitCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodes.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroBenefitCodes.Where(q => q.Code == Code);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static RetroBenefitCode Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroBenefitCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCode retroBenefitCode = Find(Id);
                if (retroBenefitCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCode, this);

                retroBenefitCode.Code = Code;
                retroBenefitCode.Description = Description;
                retroBenefitCode.EffectiveDate = EffectiveDate;
                retroBenefitCode.CeaseDate = CeaseDate;
                retroBenefitCode.Status = Status;
                retroBenefitCode.Remarks = Remarks;
                retroBenefitCode.UpdatedAt = DateTime.Now;
                retroBenefitCode.UpdatedById = UpdatedById ?? retroBenefitCode.UpdatedById;

                db.Entry(retroBenefitCode).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RetroBenefitCode retroBenefitCode = db.RetroBenefitCodes.Where(q => q.Id == id).FirstOrDefault();
                if (retroBenefitCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(retroBenefitCode, true);

                db.Entry(retroBenefitCode).State = EntityState.Deleted;
                db.RetroBenefitCodes.Remove(retroBenefitCode);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
