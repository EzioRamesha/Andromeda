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

namespace DataAccess.Entities.TreatyPricing
{
    [Table("HipsCategoryDetails")]
    public class HipsCategoryDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int HipsCategoryId { get; set; }

        [ForeignKey(nameof(HipsCategoryId))]
        [ExcludeTrail]
        public virtual HipsCategory HipsCategory { get; set; }

        [Required, MaxLength(20), Index]
        public string Subcategory { get; set; }

        [Required, MaxLength(255), Index]
        public string Description { get; set; }

        [Required, Index]
        public int ItemType { get; set; }

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

        public HipsCategoryDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.HipsCategoryDetails.Any(q => q.Id == id);
            }
        }

        public static HipsCategoryDetail Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.HipsCategoryDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<HipsCategoryDetail> GetByHipsCategoryIdExcept(int hipsCategoryId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.HipsCategoryDetails.Where(q => q.HipsCategoryId == hipsCategoryId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.HipsCategoryDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                HipsCategoryDetail hipsCategoryDetail = Find(Id);
                if (hipsCategoryDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(hipsCategoryDetail, this);

                hipsCategoryDetail.HipsCategoryId = HipsCategoryId;
                hipsCategoryDetail.Subcategory = Subcategory;
                hipsCategoryDetail.Description = Description;
                hipsCategoryDetail.ItemType = ItemType;
                hipsCategoryDetail.UpdatedAt = DateTime.Now;
                hipsCategoryDetail.UpdatedById = UpdatedById ?? hipsCategoryDetail.UpdatedById;

                db.Entry(hipsCategoryDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                HipsCategoryDetail hipsCategoryDetail = db.HipsCategoryDetails.Where(q => q.Id == id).FirstOrDefault();
                if (hipsCategoryDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(hipsCategoryDetail, true);

                db.Entry(hipsCategoryDetail).State = EntityState.Deleted;
                db.HipsCategoryDetails.Remove(hipsCategoryDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByHipsCategoryId(int hipsCategoryId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.HipsCategoryDetails.Where(q => q.HipsCategoryId == hipsCategoryId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (HipsCategoryDetail hipsCategoryDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(hipsCategoryDetail, true);
                    trails.Add(trail);

                    db.Entry(hipsCategoryDetail).State = EntityState.Deleted;
                    db.HipsCategoryDetails.Remove(hipsCategoryDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
