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
    [Table("TreatyPricingGroupReferralHipsTables")]
    public class TreatyPricingGroupReferralHipsTable
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyPricingGroupReferralId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferral TreatyPricingGroupReferral { get; set; }

        [Index]
        public int? TreatyPricingGroupReferralFileId { get; set; }
        [ExcludeTrail]
        public virtual TreatyPricingGroupReferralFile TreatyPricingGroupReferralFile { get; set; }        

        [Index]
        public int? HipsCategoryId { get; set; }
        [ExcludeTrail]
        public virtual HipsCategory HipsCategory { get; set; }

        [Index]
        public int? HipsSubCategoryId { get; set; }
        [ExcludeTrail]
        public virtual HipsCategoryDetail HipsSubCategory { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CoverageStartDate { get; set; }

        public string Description { get; set; }

        public string PlanA { get; set; }
        public string PlanB { get; set; }
        public string PlanC { get; set; }
        public string PlanD { get; set; }
        public string PlanE { get; set; }
        public string PlanF { get; set; }
        public string PlanG { get; set; }
        public string PlanH { get; set; }
        public string PlanI { get; set; }
        public string PlanJ { get; set; }
        public string PlanK { get; set; }
        public string PlanL { get; set; }
        public string PlanM { get; set; }
        public string PlanN { get; set; }
        public string PlanO { get; set; }
        public string PlanP { get; set; }
        public string PlanQ { get; set; }
        public string PlanR { get; set; }
        public string PlanS { get; set; }
        public string PlanT { get; set; }
        public string PlanU { get; set; }
        public string PlanV { get; set; }
        public string PlanW { get; set; }
        public string PlanX { get; set; }
        public string PlanY { get; set; }
        public string PlanZ { get; set; }

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

        public TreatyPricingGroupReferralHipsTable()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralHipsTables.Any(q => q.Id == id);
            }
        }

        public static TreatyPricingGroupReferralHipsTable Find(int id)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyPricingGroupReferralHipsTables.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                db.TreatyPricingGroupReferralHipsTables.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.TreatyPricingGroupReferralHipsTables.Add(this);
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext(false))
            {
                var entity = Find(Id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, this);

                entity.TreatyPricingGroupReferralId = TreatyPricingGroupReferralId;
                entity.TreatyPricingGroupReferralFileId = TreatyPricingGroupReferralFileId;

                entity.HipsCategoryId = HipsCategoryId;
                entity.HipsSubCategoryId = HipsSubCategoryId;
                entity.CoverageStartDate = CoverageStartDate;
                entity.Description = Description;
                entity.PlanA = PlanA;
                entity.PlanB = PlanB;
                entity.PlanC = PlanC;
                entity.PlanD = PlanD;
                entity.PlanE = PlanE;
                entity.PlanF = PlanF;
                entity.PlanG = PlanG;
                entity.PlanH = PlanH;
                entity.PlanI = PlanI;
                entity.PlanJ = PlanJ;
                entity.PlanK = PlanK;
                entity.PlanL = PlanL;
                entity.PlanM = PlanM;
                entity.PlanN = PlanN;
                entity.PlanO = PlanO;
                entity.PlanP = PlanP;
                entity.PlanQ = PlanQ;
                entity.PlanR = PlanR;
                entity.PlanS = PlanS;
                entity.PlanT = PlanT;
                entity.PlanU = PlanU;
                entity.PlanV = PlanV;
                entity.PlanW = PlanW;
                entity.PlanX = PlanX;
                entity.PlanY = PlanY;
                entity.PlanZ = PlanZ;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext(false))
            {
                var entity = Find(id);
                if (entity == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.TreatyPricingGroupReferralHipsTables.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
