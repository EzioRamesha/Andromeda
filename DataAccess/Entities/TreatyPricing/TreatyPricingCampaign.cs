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
    [Table("TreatyPricingCampaigns")]
    public class TreatyPricingCampaign
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingCedantId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingCedant TreatyPricingCedant { get; set; }

        [Required, MaxLength(60), Index]
        public string Code { get; set; }

        [MaxLength(255), Index]
        public string Name { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "ntext")]
        public string Type { get; set; }

        public string Purpose { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PeriodStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PeriodEndDate { get; set; }

        public string Duration { get; set; }

        [MaxLength(255)]
        public string TargetTakeUpRate { get; set; }

        public string AverageSumAssured { get; set; }

        public string RiPremiumReceivable{ get; set; }

        public string NoOfPolicy { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

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

        public TreatyPricingCampaign()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaigns.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingCampaign Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCampaigns.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyPricingCampaigns.Add(this);
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

                entity.TreatyPricingCedantId = TreatyPricingCedantId;
                entity.Code = Code;
                entity.Name = Name;
                entity.Status = Status;
                entity.Type = Type;
                entity.Purpose = Purpose;
                entity.PeriodStartDate = PeriodStartDate;
                entity.PeriodEndDate = PeriodEndDate;
                entity.Duration = Duration;
                entity.TargetTakeUpRate = TargetTakeUpRate;
                entity.AverageSumAssured = AverageSumAssured;
                entity.RiPremiumReceivable = RiPremiumReceivable;
                entity.NoOfPolicy = NoOfPolicy;
                entity.Remarks = Remarks;
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
                db.TreatyPricingCampaigns.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
