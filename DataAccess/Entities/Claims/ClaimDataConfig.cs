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
    [Table("ClaimDataConfigs")]
    public class ClaimDataConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CedantId { get; set; }

        [ForeignKey(nameof(CedantId))]
        [ExcludeTrail]

        public virtual Cedant Cedant { get; set; }

        public int? TreatyCodeId { get; set; }

        public int? TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Required]
        public int Status { get; set; }

        [Required, MaxLength(64), Index]
        public string Code { get; set; }

        [Required, MaxLength(255), Index]
        public string Name { get; set; }

        [Required]
        public int FileType { get; set; }

        public string Configs { get; set; }

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

        public ClaimDataConfig()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataConfigs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var query = db.ClaimDataConfigs.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static ClaimDataConfig Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataConfigs.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataConfigs.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataConfigs.Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static IList<ClaimDataConfig> GetByCedantIdStatus(int cedantId, int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataConfigs.Where(q => q.CedantId == cedantId);
                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }
                return query.ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataConfigs.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataConfig claimDataConfig = Find(Id);
                if (claimDataConfig == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataConfig, this);

                claimDataConfig.CedantId = CedantId;
                claimDataConfig.TreatyId = TreatyId;
                claimDataConfig.Status = Status;
                claimDataConfig.Code = Code;
                claimDataConfig.Name = Name;
                claimDataConfig.FileType = FileType;
                claimDataConfig.Configs = Configs;
                claimDataConfig.UpdatedAt = DateTime.Now;
                claimDataConfig.UpdatedById = UpdatedById ?? claimDataConfig.UpdatedById;

                db.Entry(claimDataConfig).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataConfig claimDataConfig = db.ClaimDataConfigs.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataConfig == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataConfig, true);

                db.Entry(claimDataConfig).State = EntityState.Deleted;
                db.ClaimDataConfigs.Remove(claimDataConfig);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
