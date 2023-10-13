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
    [Table("TreatyDiscountTableDetails")]
    public class TreatyDiscountTableDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int TreatyDiscountTableId { get; set; }
        [ExcludeTrail]
        public virtual TreatyDiscountTable TreatyDiscountTable { get; set; }

        [Required]
        public string CedingPlanCode { get; set; }

        [Column(TypeName = "ntext")]
        public string BenefitCode { get; set; }

        [Index]
        public int? AgeFrom { get; set; }

        [Index]
        public int? AgeTo { get; set; }
        
        [Index]
        public double? AARFrom { get; set; }

        [Index]
        public double? AARTo { get; set; }

        [Required, Index]
        public double Discount { get; set; }

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

        public TreatyDiscountTableDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTableDetails.Any(q => q.Id == id);
            }
        }

        public static TreatyDiscountTableDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTableDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<TreatyDiscountTableDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTableDetails.ToList();
            }
        }

        public static IList<TreatyDiscountTableDetail> GetByTreatyDiscountTableId(int treatyDiscountTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTableDetails.Where(q => q.TreatyDiscountTableId == treatyDiscountTableId).ToList();
            }
        }

        public static IList<TreatyDiscountTableDetail> GetByTreatyDiscountTableIdExcept(int treatyDiscountTableId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyDiscountTableDetails.Where(q => q.TreatyDiscountTableId == treatyDiscountTableId && !ids.Contains(q.Id)).ToList();
            }
        }

        public static int CountByBenefitId(string benefitCode)
        {
            using (var db = new AppDbContext())
            {
                var count = 0;

                foreach (var bc in db.TreatyDiscountTableDetails.Where(q => q.BenefitCode.Contains(benefitCode)))
                {
                    count++;
                }
                return count;
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyDiscountTableDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyDiscountTableDetail treatyDiscountTableDetail = Find(Id);
                if (treatyDiscountTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyDiscountTableDetail, this);

                treatyDiscountTableDetail.TreatyDiscountTableId = TreatyDiscountTableId;
                treatyDiscountTableDetail.CedingPlanCode = CedingPlanCode;
                treatyDiscountTableDetail.BenefitCode = BenefitCode;
                treatyDiscountTableDetail.AgeFrom = AgeFrom;
                treatyDiscountTableDetail.AgeTo = AgeTo;
                treatyDiscountTableDetail.AARFrom = AARFrom;
                treatyDiscountTableDetail.AARTo = AARTo;
                treatyDiscountTableDetail.Discount = Discount;
                treatyDiscountTableDetail.UpdatedAt = DateTime.Now;
                treatyDiscountTableDetail.UpdatedById = UpdatedById ?? treatyDiscountTableDetail.UpdatedById;

                db.Entry(treatyDiscountTableDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyDiscountTableDetail treatyDiscountTableDetail = db.TreatyDiscountTableDetails.Where(q => q.Id == id).FirstOrDefault();
                if (treatyDiscountTableDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyDiscountTableDetail, true);

                db.Entry(treatyDiscountTableDetail).State = EntityState.Deleted;
                db.TreatyDiscountTableDetails.Remove(treatyDiscountTableDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByTreatyDiscountTableId(int treatyDiscountTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyDiscountTableDetails.Where(q => q.TreatyDiscountTableId == treatyDiscountTableId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyDiscountTableDetail treatyDiscountTableDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(treatyDiscountTableDetail, true);
                    trails.Add(trail);

                    db.Entry(treatyDiscountTableDetail).State = EntityState.Deleted;
                    db.TreatyDiscountTableDetails.Remove(treatyDiscountTableDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
