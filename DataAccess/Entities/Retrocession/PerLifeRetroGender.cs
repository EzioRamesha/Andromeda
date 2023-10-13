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
    [Table("PerLifeRetroGenders")]
    public class PerLifeRetroGender
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail InsuredGenderCodePickListDetail { get; set; }

        [MaxLength(15), Index]
        public string Gender { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveStartDate { get; set; }

        [Required, Index]
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveEndDate { get; set; }

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

        public PerLifeRetroGender()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroGenders.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeRetroGenders
                    .Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId);
                    //.Where(
                    //    q => DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(EffectiveStartDate)
                    //    && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(EffectiveStartDate)
                    //    ||
                    //    DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(EffectiveEndDate)
                    //    && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(EffectiveEndDate)
                    //);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeRetroGender Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroGenders.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeRetroGenders.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroGender perLifeRetroGender = Find(Id);
                if (perLifeRetroGender == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroGender, this);

                perLifeRetroGender.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                perLifeRetroGender.Gender = Gender;
                perLifeRetroGender.EffectiveStartDate = EffectiveStartDate;
                perLifeRetroGender.EffectiveEndDate = EffectiveEndDate;
                perLifeRetroGender.UpdatedAt = DateTime.Now;
                perLifeRetroGender.UpdatedById = UpdatedById ?? perLifeRetroGender.UpdatedById;

                db.Entry(perLifeRetroGender).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroGender perLifeRetroGender = db.PerLifeRetroGenders.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeRetroGender == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroGender, true);

                db.Entry(perLifeRetroGender).State = EntityState.Deleted;
                db.PerLifeRetroGenders.Remove(perLifeRetroGender);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (InsuredGenderCodePickListDetail != null)
                return InsuredGenderCodePickListDetail.ToString();

            return null;
        }
    }
}
