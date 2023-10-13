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

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataComputations")]
    public class ClaimDataComputation
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

        [Required, MaxLength(128), Index]
        public string Description { get; set; }

        [Required, MaxLength(512)]
        public string Condition { get; set; }

        [Required, Index]
        public int StandardClaimDataOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardClaimDataOutputId))]
        public virtual StandardClaimDataOutput StandardClaimDataOutput { get; set; }

        [Required]
        public int Mode { get; set; }

        [Required, MaxLength(512)]
        public string CalculationFormula { get; set; }

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

        public ClaimDataComputation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataComputations.Any(q => q.Id == id);
            }
        }

        public static ClaimDataComputation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataComputations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimDataComputation> GetByClaimDataConfigId(int claimDataConfigId, int? step = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataComputations.Where(q => q.ClaimDataConfigId == claimDataConfigId);

                if (step != null)
                {
                    query = query.Where(q => q.Step == step);
                }

                return query.OrderBy(q => q.SortIndex).ToList();
            }
        }

        public static IList<ClaimDataComputation> GetByClaimDataConfigIdExcept(int claimDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataComputations.Where(q => q.ClaimDataConfigId == claimDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataComputations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataComputation claimDataComputation = Find(Id);
                if (claimDataComputation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataComputation, this);

                claimDataComputation.ClaimDataConfigId = ClaimDataConfigId;
                claimDataComputation.Step = Step;
                claimDataComputation.SortIndex = SortIndex;
                claimDataComputation.Description = Description;
                claimDataComputation.Condition = Condition;
                claimDataComputation.StandardClaimDataOutputId = StandardClaimDataOutputId;
                claimDataComputation.Mode = Mode;
                claimDataComputation.CalculationFormula = CalculationFormula;
                claimDataComputation.UpdatedAt = DateTime.Now;
                claimDataComputation.UpdatedById = UpdatedById ?? claimDataComputation.UpdatedById;

                db.Entry(claimDataComputation).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataComputation claimDataComputation = db.ClaimDataComputations.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataComputation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataComputation, true);

                db.Entry(claimDataComputation).State = EntityState.Deleted;
                db.ClaimDataComputations.Remove(claimDataComputation);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataComputations.Where(q => q.ClaimDataConfigId == claimDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataComputation claimDataComputation in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataComputation, true);
                    trails.Add(trail);

                    db.Entry(claimDataComputation).State = EntityState.Deleted;
                    db.ClaimDataComputations.Remove(claimDataComputation);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
