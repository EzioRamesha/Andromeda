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

namespace DataAccess.Entities
{
    [Table("ClaimCodeMappings")]
    public class ClaimCodeMapping
    {
        [Key]
        public int Id { get; set; }

        public string MlreEventCode { get; set; }

        public string MlreBenefitCode { get; set; }

        [Required, Index]
        public int ClaimCodeId { get; set; }

        [ForeignKey(nameof(ClaimCodeId))]
        [ExcludeTrail]
        public virtual ClaimCode ClaimCode { get; set; }

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

        [ExcludeTrail]
        public virtual ICollection<ClaimCodeMappingDetail> ClaimCodeMappingDetails { get; set; }

        public ClaimCodeMapping()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodeMappings.Any(q => q.Id == id);
            }
        }

        public static ClaimCodeMapping Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodeMappings.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimCodeMappings.Where(q => q.ClaimCodeId == claimCodeId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimCodeMappings.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimCodeMapping claimCodeMapping = ClaimCodeMapping.Find(Id);
                if (claimCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCodeMapping, this);

                claimCodeMapping.MlreEventCode = MlreEventCode;
                claimCodeMapping.MlreBenefitCode = MlreBenefitCode;
                claimCodeMapping.ClaimCodeId = ClaimCodeId;
                claimCodeMapping.UpdatedAt = DateTime.Now;
                claimCodeMapping.UpdatedById = UpdatedById ?? claimCodeMapping.UpdatedById;

                db.Entry(claimCodeMapping).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimCodeMapping claimCodeMapping = db.ClaimCodeMappings.Where(q => q.Id == id).FirstOrDefault();
                if (claimCodeMapping == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimCodeMapping, true);

                db.Entry(claimCodeMapping).State = EntityState.Deleted;
                db.ClaimCodeMappings.Remove(claimCodeMapping);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
