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

namespace DataAccess.Entities.RiDatas
{
    [Table("RiDataComputations")]
    public class RiDataComputation
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RiDataConfigId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(RiDataConfigId))]
        public virtual RiDataConfig RiDataConfig { get; set; }

        [Required, Index]
        public int Step { get; set; }

        public int SortIndex { get; set; }

        [Required, MaxLength(128), Index]
        public string Description { get; set; }

        [Required, MaxLength(512)]
        public string Condition { get; set; }

        [Index]
        public int? StandardOutputId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(StandardOutputId))]
        public virtual StandardOutput StandardOutput { get; set; }

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

        public RiDataComputation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataComputations.Any(q => q.Id == id);
            }
        }

        public static RiDataComputation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataComputations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RiDataComputation> GetByRiDataConfigId(int riDataConfigId, int? step = null)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(101, "RiDataComputation");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.RiDataComputations.Where(q => q.RiDataConfigId == riDataConfigId);
                    if (step != null)
                    {
                        query = query.Where(q => q.Step == step);
                    }

                    return query.OrderBy(q => q.SortIndex).ToList();
                });
            }
        }

        public static IList<RiDataComputation> GetByRiDataConfigIdExcept(int riDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataComputations.Where(q => q.RiDataConfigId == riDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataComputations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataComputation riDataComputation = Find(Id);
                if (riDataComputation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataComputation, this);

                riDataComputation.RiDataConfigId = RiDataConfigId;
                riDataComputation.Step = Step;
                riDataComputation.SortIndex = SortIndex;
                riDataComputation.Description = Description;
                riDataComputation.Condition = Condition;
                riDataComputation.StandardOutputId = StandardOutputId;
                riDataComputation.Mode = Mode;
                riDataComputation.CalculationFormula = CalculationFormula;
                riDataComputation.UpdatedAt = DateTime.Now;
                riDataComputation.UpdatedById = UpdatedById ?? riDataComputation.UpdatedById;

                db.Entry(riDataComputation).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataComputation riDataComputation = db.RiDataComputations.Where(q => q.Id == id).FirstOrDefault();
                if (riDataComputation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataComputation, true);

                db.Entry(riDataComputation).State = EntityState.Deleted;
                db.RiDataComputations.Remove(riDataComputation);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataComputations.Where(q => q.RiDataConfigId == riDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataComputation riDataComputation in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataComputation, true);
                    trails.Add(trail);

                    db.Entry(riDataComputation).State = EntityState.Deleted;
                    db.RiDataComputations.Remove(riDataComputation);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
