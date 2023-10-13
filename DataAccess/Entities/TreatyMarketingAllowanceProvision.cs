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
    [Table("TreatyMarketingAllowanceProvisions")]
    public class TreatyMarketingAllowanceProvision
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TreatyCodeId { get; set; }

        [ForeignKey(nameof(TreatyCodeId))]
        [ExcludeTrail]
        public virtual TreatyCode TreatyCode { get; set; }

        [Required, MaxLength(50), Index]
        public string Code { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LaunchAt { get; set; }

        public double? Year1stPremiumRate  { get; set; }

        public double? RenewalPremiumRate { get; set; }

        public int? Type { get; set; }

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

        public TreatyMarketingAllowanceProvision()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyMarketingAllowanceProvisions.Any(q => q.Id == id);
            }
        }

        public static TreatyMarketingAllowanceProvision Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyMarketingAllowanceProvisions.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyMarketingAllowanceProvisions.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.TreatyCodeId = TreatyCodeId;
                entity.Code = Code;
                entity.Name = Name;
                entity.Year1stPremiumRate = Year1stPremiumRate;
                entity.RenewalPremiumRate = RenewalPremiumRate;
                entity.Type = Type;
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
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyMarketingAllowanceProvisions.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
