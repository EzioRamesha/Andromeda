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
    [Table("FacMasterListingUpload")]
    public class FacMasterListingUpload
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

        public FacMasterListingUpload()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListingUpload.Any(q => q.Id == id);
            }
        }

        public static FacMasterListingUpload Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.FacMasterListingUpload.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.FacMasterListingUpload.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                FacMasterListingUpload facMasterListingUpload = FacMasterListingUpload.Find(Id);
                if (facMasterListingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(facMasterListingUpload, this);

                facMasterListingUpload.Status = Status;
                facMasterListingUpload.FileName = FileName;
                facMasterListingUpload.HashFileName = HashFileName;
                facMasterListingUpload.Errors = Errors;

                facMasterListingUpload.UpdatedAt = DateTime.Now;
                facMasterListingUpload.UpdatedById = UpdatedById ?? facMasterListingUpload.UpdatedById;

                db.Entry(facMasterListingUpload).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                FacMasterListingUpload facMasterListingUpload = db.FacMasterListingUpload.Where(q => q.Id == id).FirstOrDefault();
                if (facMasterListingUpload == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(facMasterListingUpload, true);

                db.Entry(facMasterListingUpload).State = EntityState.Deleted;
                db.FacMasterListingUpload.Remove(facMasterListingUpload);
                db.SaveChanges();

                return trail;
            }
        }
    }
}

