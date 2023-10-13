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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingMedicalTableUploadCells")]
    public class TreatyPricingMedicalTableUploadCell
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingMedicalTableUploadColumnId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTableUploadColumn TreatyPricingMedicalTableUploadColumn { get; set; }

        [Required, Index]
        public int TreatyPricingMedicalTableUploadRowId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingMedicalTableUploadRow TreatyPricingMedicalTableUploadRow { get; set; }

        [MaxLength(255), Index]
        public string Code { get; set; }

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

        public TreatyPricingMedicalTableUploadCell()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableUploadCells.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingMedicalTableUploadCell Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableUploadCells.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingMedicalTableUploadCells.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyPricingMedicalTableUploadColumnId = TreatyPricingMedicalTableUploadColumnId;
                entity.TreatyPricingMedicalTableUploadRowId = TreatyPricingMedicalTableUploadRowId;
                entity.Code = Code;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingMedicalTableUploadCells.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableUploadColumnId(int treatyPricingMedicalTableUploadColumnId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingMedicalTableUploadCells.Where(q => q.TreatyPricingMedicalTableUploadColumnId == treatyPricingMedicalTableUploadColumnId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingMedicalTableUploadCell entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingMedicalTableUploadCells.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableUploadRowId(int treatyPricingMedicalTableUploadRowId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingMedicalTableUploadCells.Where(q => q.TreatyPricingMedicalTableUploadRowId == treatyPricingMedicalTableUploadRowId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyPricingMedicalTableUploadCell entity in query.ToList())
                {
                    DataTrail trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.TreatyPricingMedicalTableUploadCells.Remove(entity);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
