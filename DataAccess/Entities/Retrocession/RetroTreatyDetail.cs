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

namespace DataAccess.Entities.Retrocession
{
    [Table("RetroTreatyDetails")]
    public class RetroTreatyDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RetroTreatyId { get; set; }

        [ForeignKey(nameof(RetroTreatyId))]
        [ExcludeTrail]
        public virtual RetroTreaty RetroTreaty { get; set; }

        [Required, Index]
        public int PerLifeRetroConfigurationTreatyId { get; set; }

        [ForeignKey(nameof(PerLifeRetroConfigurationTreatyId))]
        [ExcludeTrail]
        public virtual PerLifeRetroConfigurationTreaty PerLifeRetroConfigurationTreaty { get; set; }

        [Index]
        public int? PremiumSpreadTableId { get; set; }

        [ForeignKey(nameof(PremiumSpreadTableId))]
        [ExcludeTrail]
        public virtual PremiumSpreadTable PremiumSpreadTable { get; set; }

        [Index]
        public int? TreatyDiscountTableId { get; set; }

        [ForeignKey(nameof(TreatyDiscountTableId))]
        [ExcludeTrail]
        public virtual TreatyDiscountTable TreatyDiscountTable { get; set; }

        [Index]
        public double MlreShare { get; set; }

        [Column(TypeName = "ntext")]
        public string GrossRetroPremium { get; set; }

        [Column(TypeName = "ntext")]
        public string TreatyDiscount { get; set; }

        [Column(TypeName = "ntext")]
        public string NetRetroPremium { get; set; }

        [Column(TypeName = "ntext")]
        public string Remark { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }
        [ExcludeTrail]
        public virtual User CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        [ExcludeTrail]
        public virtual User UpdatedBy { get; set; }

        public RetroTreatyDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroTreatyDetails.Any(q => q.Id == id);
            }
        }

        public static RetroTreatyDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroTreatyDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RetroTreatyDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RetroTreatyDetail entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.RetroTreatyId = RetroTreatyId;
                entity.PerLifeRetroConfigurationTreatyId = PerLifeRetroConfigurationTreatyId;
                entity.PremiumSpreadTableId = PremiumSpreadTableId;
                entity.TreatyDiscountTableId = TreatyDiscountTableId;
                entity.MlreShare = MlreShare;
                entity.GrossRetroPremium = GrossRetroPremium;
                entity.TreatyDiscount = TreatyDiscount;
                entity.NetRetroPremium = NetRetroPremium;
                entity.Remark = Remark;
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
                RetroTreatyDetail entity = db.RetroTreatyDetails.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.RetroTreatyDetails.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
