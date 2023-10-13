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

namespace DataAccess.Entities.Retrocession
{
    [Table("PerLifeDuplicationChecks")]
    public class PerLifeDuplicationCheck
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30), Index]
        public string ConfigurationCode { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        [Index]
        public bool Inclusion { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsuranceEffectiveStartDate { get; set; }

        [Index]
        [Column(TypeName = "datetime2")]
        public DateTime? ReinsuranceEffectiveEndDate { get; set; }

        public string TreatyCode { get; set; }
        public int NoOfTreatyCode { get; set; }

        [Index]
        public bool EnableReinsuranceBasisCodeCheck { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<PerLifeDuplicationCheckDetail> PerLifeDuplicationCheckDetails { get; set; }

        public PerLifeDuplicationCheck()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDuplicationChecks.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeDuplicationChecks.Where(q => q.ConfigurationCode == ConfigurationCode);
                if (Id != 0)
                {
                    query = query.Where(q => q.Id != Id);
                }
                return query.Count() > 0;
            }
        }

        public static PerLifeDuplicationCheck Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDuplicationChecks.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PerLifeDuplicationChecks.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PerLifeDuplicationCheck perLifeDuplicationChecks = Find(Id);
                if (perLifeDuplicationChecks == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeDuplicationChecks, this);

                perLifeDuplicationChecks.ConfigurationCode = ConfigurationCode;
                perLifeDuplicationChecks.Description = Description;
                perLifeDuplicationChecks.Inclusion = Inclusion;
                perLifeDuplicationChecks.ReinsuranceEffectiveStartDate = ReinsuranceEffectiveStartDate;
                perLifeDuplicationChecks.ReinsuranceEffectiveEndDate = ReinsuranceEffectiveEndDate;
                perLifeDuplicationChecks.TreatyCode = TreatyCode;
                perLifeDuplicationChecks.NoOfTreatyCode = NoOfTreatyCode;
                perLifeDuplicationChecks.EnableReinsuranceBasisCodeCheck = EnableReinsuranceBasisCodeCheck;

                perLifeDuplicationChecks.UpdatedAt = DateTime.Now;
                perLifeDuplicationChecks.UpdatedById = UpdatedById ?? perLifeDuplicationChecks.UpdatedById;

                db.Entry(perLifeDuplicationChecks).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PerLifeDuplicationCheck perLifeDuplicationCheck = db.PerLifeDuplicationChecks.Where(q => q.Id == id).FirstOrDefault();
                if (perLifeDuplicationCheck == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(perLifeDuplicationCheck, true);

                db.Entry(perLifeDuplicationCheck).State = EntityState.Deleted;
                db.PerLifeDuplicationChecks.Remove(perLifeDuplicationCheck);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
