using BusinessObject.RiDatas;
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
    [Table("RiDataPreValidations")]
    public class RiDataPreValidation
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

        public RiDataPreValidation()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public RiDataPreValidation(RiDataPreValidationBo riDataPreValidationBo) : this()
        {
            Id = riDataPreValidationBo.Id;
            RiDataConfigId = riDataPreValidationBo.RiDataConfigId;
            Step = riDataPreValidationBo.Step;
            SortIndex = riDataPreValidationBo.SortIndex;
            Description = riDataPreValidationBo.Description;
            Condition = riDataPreValidationBo.Condition;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataPreValidations.Any(q => q.Id == id);
            }
        }

        public static RiDataPreValidation Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataPreValidations.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<RiDataPreValidation> GetByRiDataConfigId(int riDataConfigId, int? step = null)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(101, "RiDataPreValidation");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.RiDataPreValidations.Where(q => q.RiDataConfigId == riDataConfigId);
                    if (step != null)
                    {
                        query = query.Where(q => q.Step == step);
                    }

                    return query.OrderBy(q => q.SortIndex).ToList();
                });
            }
        }

        public static IList<RiDataPreValidation> GetByRiDataConfigIdExcept(int riDataConfigId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataPreValidations.Where(q => q.RiDataConfigId == riDataConfigId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataPreValidations.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataPreValidation riDataPreValidation = Find(Id);
                if (riDataPreValidation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataPreValidation, this);

                riDataPreValidation.RiDataConfigId = RiDataConfigId;
                riDataPreValidation.Step = Step;
                riDataPreValidation.SortIndex = SortIndex;
                riDataPreValidation.Description = Description;
                riDataPreValidation.Condition = Condition;
                riDataPreValidation.UpdatedAt = DateTime.Now;
                riDataPreValidation.UpdatedById = UpdatedById ?? riDataPreValidation.UpdatedById;

                db.Entry(riDataPreValidation).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataPreValidation riDataPreValidation = db.RiDataPreValidations.Where(q => q.Id == id).FirstOrDefault();
                if (riDataPreValidation == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataPreValidation, true);

                db.Entry(riDataPreValidation).State = EntityState.Deleted;
                db.RiDataPreValidations.Remove(riDataPreValidation);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataPreValidations.Where(q => q.RiDataConfigId == riDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataPreValidation riDataPreValidation in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataPreValidation, true);
                    trails.Add(trail);

                    db.Entry(riDataPreValidation).State = EntityState.Deleted;
                    db.RiDataPreValidations.Remove(riDataPreValidation);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
