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
    [Table("PerLifeRetroCountries")]
    public class PerLifeRetroCountry
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail TerritoryOfIssueCodePickListDetail { get; set; }

        [MaxLength(50), Index]
        public string Country { get; set; }

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

        public PerLifeRetroCountry()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroCountries.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicate()
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeRetroCountries.Where(q => q.TerritoryOfIssueCodePickListDetailId == TerritoryOfIssueCodePickListDetailId);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeRetroCountry Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeRetroCountries.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeRetroCountries.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroCountry perLifeRetroCountry = Find(Id);
                if (perLifeRetroCountry == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroCountry, this);

                perLifeRetroCountry.TerritoryOfIssueCodePickListDetailId = TerritoryOfIssueCodePickListDetailId;
                perLifeRetroCountry.Country = Country;
                perLifeRetroCountry.EffectiveStartDate = EffectiveStartDate;
                perLifeRetroCountry.EffectiveEndDate = EffectiveEndDate;
                perLifeRetroCountry.UpdatedAt = DateTime.Now;
                perLifeRetroCountry.UpdatedById = UpdatedById ?? perLifeRetroCountry.UpdatedById;

                db.Entry(perLifeRetroCountry).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeRetroCountry perLifeRetroCountry = db.PerLifeRetroCountries.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeRetroCountry == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeRetroCountry, true);

                db.Entry(perLifeRetroCountry).State = EntityState.Deleted;
                db.PerLifeRetroCountries.Remove(perLifeRetroCountry);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (TerritoryOfIssueCodePickListDetail != null)
                return TerritoryOfIssueCodePickListDetail.ToString();

            return null;
        }
    }
}
