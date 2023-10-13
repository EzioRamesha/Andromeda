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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("TreatyPricingGroupReferralFiles")]
    public class TreatyPricingGroupReferralFile
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int? TreatyPricingGroupReferralId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferral TreatyPricingGroupReferral { get; set; }

        [Index]
        public int? TableTypePickListDetailId { get; set; }
        [ExcludeTrail]
        public virtual PickListDetail TableTypePickListDetail { get; set; }

        [MaxLength(255), Index]
        public string FileName { get; set; }

        [MaxLength(255), Index]
        public string HashFileName { get; set; }

        [Index]
        public int UploadedType { get; set; }

        [Required, Index]
        public int Status { get; set; }

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

        public TreatyPricingGroupReferralFile()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralFiles.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralFile Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingGroupReferralFiles.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingGroupReferralFiles.Add(this);
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

                entity.TreatyPricingGroupReferralId = TreatyPricingGroupReferralId;
                entity.TableTypePickListDetailId = TableTypePickListDetailId;
                entity.UploadedType = UploadedType;
                entity.FileName = FileName;
                entity.HashFileName = HashFileName;
                entity.Status = Status;
                entity.Errors = Errors;
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
                TreatyPricingGroupReferralFile TreatyPricingGroupReferralFile = Find(id);
                if (TreatyPricingGroupReferralFile == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(TreatyPricingGroupReferralFile, true);

                db.Entry(TreatyPricingGroupReferralFile).State = EntityState.Deleted;
                db.TreatyPricingGroupReferralFiles.Remove(TreatyPricingGroupReferralFile);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
