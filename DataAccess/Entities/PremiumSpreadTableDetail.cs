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
    [Table("PremiumSpreadTableDetails")]
    public class PremiumSpreadTableDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PremiumSpreadTableId { get; set; }

        [ExcludeTrail]
        public virtual PremiumSpreadTable PremiumSpreadTable { get; set; }

        [Required]
        public string CedingPlanCode { get; set; }

        [Index]
        public int? BenefitId { get; set; }

        [ExcludeTrail]
        public virtual Benefit Benefit { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        [Index]
        public int? AgeFrom { get; set; }

        [Index]
        public int? AgeTo { get; set; }

        [Required, Index]
        public double PremiumSpread { get; set; }

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

        public PremiumSpreadTableDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.Any(q => q.Id == id);
            }
        }

        public static PremiumSpreadTableDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<PremiumSpreadTableDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.ToList();
            }
        }

        public static IList<PremiumSpreadTableDetail> GetByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.Where(q => q.PremiumSpreadTableId == premiumSpreadTableId).ToList();
            }
        }

        public static IList<PremiumSpreadTableDetail> GetByPremiumSpreadTableIdExcept(int premiumSpreadTableId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.Where(q => q.PremiumSpreadTableId == premiumSpreadTableId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.PremiumSpreadTableDetails.Where(q => q.BenefitId == benefitId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PremiumSpreadTableDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PremiumSpreadTableDetail premiumSpreadTableDetail = Find(Id);
                if (premiumSpreadTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(premiumSpreadTableDetail, this);

                premiumSpreadTableDetail.PremiumSpreadTableId = PremiumSpreadTableId;
                premiumSpreadTableDetail.CedingPlanCode = CedingPlanCode;
                //premiumSpreadTableDetail.BenefitId = BenefitId;
                premiumSpreadTableDetail.BenefitCode = BenefitCode;
                premiumSpreadTableDetail.AgeFrom = AgeFrom;
                premiumSpreadTableDetail.AgeTo = AgeTo;
                premiumSpreadTableDetail.PremiumSpread = PremiumSpread;
                premiumSpreadTableDetail.UpdatedAt = DateTime.Now;
                premiumSpreadTableDetail.UpdatedById = UpdatedById ?? premiumSpreadTableDetail.UpdatedById;

                db.Entry(premiumSpreadTableDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PremiumSpreadTableDetail premiumSpreadTableDetail = db.PremiumSpreadTableDetails.Where(q => q.Id == id).FirstOrDefault();
                if (premiumSpreadTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(premiumSpreadTableDetail, true);

                db.Entry(premiumSpreadTableDetail).State = EntityState.Deleted;
                db.PremiumSpreadTableDetails.Remove(premiumSpreadTableDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PremiumSpreadTableDetails.Where(q => q.PremiumSpreadTableId == premiumSpreadTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PremiumSpreadTableDetail premiumSpreadTableDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(premiumSpreadTableDetail, true);
                    trails.Add(trail);

                    db.Entry(premiumSpreadTableDetail).State = EntityState.Deleted;
                    db.PremiumSpreadTableDetails.Remove(premiumSpreadTableDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
