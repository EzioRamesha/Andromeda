using BusinessObject.Claims;
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

namespace DataAccess.Entities.Claims
{
    [Table("ClaimDataMappingDetails")]
    public class ClaimDataMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int ClaimDataMappingId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(ClaimDataMappingId))]
        public virtual ClaimDataMapping ClaimDataMapping { get; set; }

        [Index]
        public int? PickListDetailId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(PickListDetailId))]
        public virtual PickListDetail PickListDetail { get; set; }

        [MaxLength(128)]
        public string RawValue { get; set; }

        public bool IsPickDetailIdEmpty { get; set; } = false;

        public bool IsRawValueEmpty { get; set; } = false;

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

        public ClaimDataMappingDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ClaimDataMappingDetail(ClaimDataMappingDetailBo claimDataMappingDetailBo) : this()
        {
            Id = claimDataMappingDetailBo.Id;
            ClaimDataMappingId = claimDataMappingDetailBo.ClaimDataMappingId;
            PickListDetailId = claimDataMappingDetailBo.PickListDetailId;
            RawValue = claimDataMappingDetailBo.RawValue;
            IsPickDetailIdEmpty = claimDataMappingDetailBo.IsPickDetailIdEmpty;
            IsRawValueEmpty = claimDataMappingDetailBo.IsRawValueEmpty;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappingDetails.Any(q => q.Id == id);
            }
        }

        public static ClaimDataMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappingDetails.Where(q => q.PickListDetailId == pickListDetailId).Count();
            }
        }

        public static IList<ClaimDataMappingDetail> GetByClaimDataMappingId(int claimDataMappingId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappingDetails.Where(q => q.ClaimDataMappingId == claimDataMappingId).ToList();
            }
        }

        public static IList<ClaimDataMappingDetail> GetByClaimDataMappingIdExcept(int claimDataMappingId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimDataMappingDetails.Where(q => q.ClaimDataMappingId == claimDataMappingId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.ClaimDataMappingDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                ClaimDataMappingDetail claimDataMappingDetail = Find(Id);
                if (claimDataMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataMappingDetail, this);

                claimDataMappingDetail.ClaimDataMappingId = ClaimDataMappingId;
                claimDataMappingDetail.PickListDetailId = PickListDetailId;
                claimDataMappingDetail.RawValue = RawValue;
                claimDataMappingDetail.IsPickDetailIdEmpty = IsPickDetailIdEmpty;
                claimDataMappingDetail.IsRawValueEmpty = IsRawValueEmpty;
                claimDataMappingDetail.UpdatedAt = DateTime.Now;
                claimDataMappingDetail.UpdatedById = UpdatedById ?? claimDataMappingDetail.UpdatedById;

                db.Entry(claimDataMappingDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                ClaimDataMappingDetail claimDataMappingDetail = db.ClaimDataMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (claimDataMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(claimDataMappingDetail, true);

                db.Entry(claimDataMappingDetail).State = EntityState.Deleted;
                db.ClaimDataMappingDetails.Remove(claimDataMappingDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataMappingId(int claimDataMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataMappingDetails.Where(q => q.ClaimDataMappingId == claimDataMappingId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataMappingDetail claimDataMappingDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(claimDataMappingDetail).State = EntityState.Deleted;
                    db.ClaimDataMappingDetails.Remove(claimDataMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimDataMappingDetails.Where(q => q.ClaimDataMapping.ClaimDataConfigId == claimDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (ClaimDataMappingDetail claimDataMappingDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(claimDataMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(claimDataMappingDetail).State = EntityState.Deleted;
                    db.ClaimDataMappingDetails.Remove(claimDataMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
