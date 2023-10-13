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
    [Table("RiDataConfigs")]
    public class RiDataConfig
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

        public RiDataConfig()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataConfigs.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var query = db.RiDataConfigs.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static RiDataConfig Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(110, "RiDataConfig");

                return connectionStrategy.Execute(() =>
                {
                    return db.RiDataConfigs.Where(q => q.Id == id).FirstOrDefault();
                });
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataConfigs.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataConfigs.Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static IList<RiDataConfig> GetByCedantIdStatus(int cedantId, int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataConfigs.Where(q => q.CedantId == cedantId);
                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }
                return query.OrderBy(q => q.Code).ThenBy(q => q.Name).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataConfigs.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataConfig riDataConfig = Find(Id);
                if (riDataConfig == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataConfig, this);

                riDataConfig.CedantId = CedantId;
                riDataConfig.TreatyId = TreatyId;
                riDataConfig.Status = Status;
                riDataConfig.Code = Code;
                riDataConfig.Name = Name;
                riDataConfig.FileType = FileType;
                riDataConfig.Configs = Configs;
                riDataConfig.UpdatedAt = DateTime.Now;
                riDataConfig.UpdatedById = UpdatedById ?? riDataConfig.UpdatedById;

                db.Entry(riDataConfig).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataConfig riDataConfig = db.RiDataConfigs.Where(q => q.Id == id).FirstOrDefault();
                if (riDataConfig == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataConfig, true);

                db.Entry(riDataConfig).State = EntityState.Deleted;
                db.RiDataConfigs.Remove(riDataConfig);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
