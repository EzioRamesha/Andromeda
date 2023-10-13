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
    [Table("ClaimChecklistDetails")]
    public class ClaimChecklistDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimChecklistId { get; set; }

        [ForeignKey(nameof(ClaimChecklistId))]
        [ExcludeTrail]
        public virtual ClaimChecklist ClaimChecklist { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Remark { get; set; }

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

        public ClaimChecklistDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklistDetails.Any(q => q.Id == id);
            }
        }

        public static ClaimChecklistDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklistDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<ClaimChecklistDetail> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklistDetails.ToList();
            }
        }

        public static IList<ClaimChecklistDetail> GetByClaimChecklistId(int claimChecklistId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklistDetails.Where(q => q.ClaimChecklistId == claimChecklistId).ToList();
            }
        }

        public static IList<ClaimChecklistDetail> GetByClaimChecklistIdExcept(int claimChecklist, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimChecklistDetails.Where(q => q.ClaimChecklistId == claimChecklist && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimChecklistDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimChecklistDetail claimChecklistDetail = Find(Id);
                if (claimChecklistDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimChecklistDetail, this);

                claimChecklistDetail.ClaimChecklistId = ClaimChecklistId;
                claimChecklistDetail.Name = Name;
                claimChecklistDetail.Remark = Remark;
                claimChecklistDetail.UpdatedAt = DateTime.Now;
                claimChecklistDetail.UpdatedById = UpdatedById ?? claimChecklistDetail.UpdatedById;

                db.Entry(claimChecklistDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimChecklistDetail claimChecklistDetail = db.ClaimChecklistDetails.Where(q => q.Id == id).FirstOrDefault();
                if (claimChecklistDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimChecklistDetail, true);

                db.Entry(claimChecklistDetail).State = EntityState.Deleted;
                db.ClaimChecklistDetails.Remove(claimChecklistDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteByClaimChecklistId(int claimChecklistId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimChecklistDetails.Where(q => q.ClaimChecklistId == claimChecklistId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimChecklistDetail claimChecklistDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimChecklistDetail, true);
                    trails.Add(trail);

                    db.Entry(claimChecklistDetail).State = EntityState.Deleted;
                    db.ClaimChecklistDetails.Remove(claimChecklistDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
