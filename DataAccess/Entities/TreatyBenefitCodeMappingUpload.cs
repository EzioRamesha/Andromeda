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

namespace DataAccess.Entities
{
    [Table("TreatyBenefitCodeMappingUpload")]
    public class TreatyBenefitCodeMappingUpload
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int Status { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Column(TypeName = "ntext")]
        public string Errors { get; set; }

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

        public TreatyBenefitCodeMappingUpload()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappingUpload.Any(q => q.Id == id);
            }
        }

        public static TreatyBenefitCodeMappingUpload Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyBenefitCodeMappingUpload.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyBenefitCodeMappingUpload.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyBenefitCodeMappingUpload treatyBenefitCodeMappingUpload = TreatyBenefitCodeMappingUpload.Find(Id);
                if (treatyBenefitCodeMappingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyBenefitCodeMappingUpload, this);

                treatyBenefitCodeMappingUpload.Status = Status;
                treatyBenefitCodeMappingUpload.FileName = FileName;
                treatyBenefitCodeMappingUpload.HashFileName = HashFileName;
                treatyBenefitCodeMappingUpload.Errors = Errors;

                treatyBenefitCodeMappingUpload.UpdatedAt = DateTime.Now;
                treatyBenefitCodeMappingUpload.UpdatedById = UpdatedById ?? treatyBenefitCodeMappingUpload.UpdatedById;

                db.Entry(treatyBenefitCodeMappingUpload).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyBenefitCodeMappingUpload treatyBenefitCodeMappingUpload = db.TreatyBenefitCodeMappingUpload.Where(q => q.Id == id).FirstOrDefault();
                if (treatyBenefitCodeMappingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyBenefitCodeMappingUpload, true);

                db.Entry(treatyBenefitCodeMappingUpload).State = EntityState.Deleted;
                db.TreatyBenefitCodeMappingUpload.Remove(treatyBenefitCodeMappingUpload);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
