using BusinessObject.Claims;
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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataValidations")]
    public class ClaimDataValidation
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimDataConfigId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataConfigId))]
        public virtual ClaimDataConfig ClaimDataConfig { get; set; }

        public int Step { get; set; }

        public int SortIndex { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [Required, MaxLength(512)]
        public string Condition { get; set; }

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

        public const int StepPreValidation = 1;
        public const int StepPostValidation = 2;
        public const int MaxStep = 2;

        public static string GetStepName(int key)
        {
            switch (key)
            {
                case StepPreValidation:
                    return "Pre-Validation";
                case StepPostValidation:
                    return "Post-Validation";
                default:
                    return "";
            }
        }

        public ClaimDataValidation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ClaimDataValidation(ClaimDataValidationBo claimDataPreValidationBo) : this()
        {
            Id = claimDataPreValidationBo.Id;
            ClaimDataConfigId = claimDataPreValidationBo.ClaimDataConfigId;
            Step = claimDataPreValidationBo.Step;
            SortIndex = claimDataPreValidationBo.SortIndex;
            Description = claimDataPreValidationBo.Description;
            Condition = claimDataPreValidationBo.Condition;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataValidations.Any(q => q.Id == id);
            }
        }

        public static ClaimDataValidation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataValidations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimDataValidation> GetByClaimDataConfigId(int claimDataConfigId, int? step = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataValidations.Where(q => q.ClaimDataConfigId == claimDataConfigId);

                if (step != null)
                {
                    query = query.Where(q => q.Step == step);
                }

                return query.OrderBy(q => q.SortIndex).ToList();
            }
        }

        public static IList<ClaimDataValidation> GetByClaimDataConfigIdExcept(int claimDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataValidations.Where(q => q.ClaimDataConfigId == claimDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataValidations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataValidation claimDataPreValidation = Find(Id);
                if (claimDataPreValidation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataPreValidation, this);

                claimDataPreValidation.ClaimDataConfigId = ClaimDataConfigId;
                claimDataPreValidation.Step = Step;
                claimDataPreValidation.SortIndex = SortIndex;
                claimDataPreValidation.Description = Description;
                claimDataPreValidation.Condition = Condition;
                claimDataPreValidation.UpdatedAt = DateTime.Now;
                claimDataPreValidation.UpdatedById = UpdatedById ?? claimDataPreValidation.UpdatedById;

                db.Entry(claimDataPreValidation).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataValidation claimDataPreValidation = db.ClaimDataValidations.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataPreValidation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataPreValidation, true);

                db.Entry(claimDataPreValidation).State = EntityState.Deleted;
                db.ClaimDataValidations.Remove(claimDataPreValidation);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataValidations.Where(q => q.ClaimDataConfigId == claimDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataValidation claimDataPreValidation in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataPreValidation, true);
                    trails.Add(trail);

                    db.Entry(claimDataPreValidation).State = EntityState.Deleted;
                    db.ClaimDataValidations.Remove(claimDataPreValidation);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
