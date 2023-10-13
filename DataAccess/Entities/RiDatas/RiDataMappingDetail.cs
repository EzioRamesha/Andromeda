using BusinessObject.RiDatas;
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
    [Table("RiDataMappingDetails")]
    public class RiDataMappingDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int RiDataMappingId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(RiDataMappingId))]
        public virtual RiDataMapping RiDataMapping { get; set; }

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

        public RiDataMappingDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public RiDataMappingDetail(RiDataMappingDetailBo riDataMappingDetailBo) : this()
        {
            Id = riDataMappingDetailBo.Id;
            RiDataMappingId = riDataMappingDetailBo.RiDataMappingId;
            PickListDetailId = riDataMappingDetailBo.PickListDetailId;
            RawValue = riDataMappingDetailBo.RawValue;
            IsPickDetailIdEmpty = riDataMappingDetailBo.IsPickDetailIdEmpty;
            IsRawValueEmpty = riDataMappingDetailBo.IsRawValueEmpty;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappingDetails.Any(q => q.Id == id);
            }
        }

        public static RiDataMappingDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappingDetails.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappingDetails.Where(q => q.PickListDetailId == pickListDetailId).Count();
            }
        }

        public static IList<RiDataMappingDetail> GetByRiDataMappingId(int riDataMappingId)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappingDetails.Where(q => q.RiDataMappingId == riDataMappingId).ToList();
            }
        }

        public static IList<RiDataMappingDetail> GetByRiDataMappingIdExcept(int riDataMappingId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.RiDataMappingDetails.Where(q => q.RiDataMappingId == riDataMappingId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.RiDataMappingDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                RiDataMappingDetail riDataMappingDetail = Find(Id);
                if (riDataMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataMappingDetail, this);

                riDataMappingDetail.RiDataMappingId = RiDataMappingId;
                riDataMappingDetail.PickListDetailId = PickListDetailId;
                riDataMappingDetail.RawValue = RawValue;
                riDataMappingDetail.IsPickDetailIdEmpty = IsPickDetailIdEmpty;
                riDataMappingDetail.IsRawValueEmpty = IsRawValueEmpty;
                riDataMappingDetail.UpdatedAt = DateTime.Now;
                riDataMappingDetail.UpdatedById = UpdatedById ?? riDataMappingDetail.UpdatedById;

                db.Entry(riDataMappingDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                RiDataMappingDetail riDataMappingDetail = db.RiDataMappingDetails.Where(q => q.Id == id).FirstOrDefault();
                if (riDataMappingDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(riDataMappingDetail, true);

                db.Entry(riDataMappingDetail).State = EntityState.Deleted;
                db.RiDataMappingDetails.Remove(riDataMappingDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByRiDataMappingId(int riDataMappingId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataMappingDetails.Where(q => q.RiDataMappingId == riDataMappingId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataMappingDetail riDataMappingDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(riDataMappingDetail).State = EntityState.Deleted;
                    db.RiDataMappingDetails.Remove(riDataMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
        
        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDataMappingDetails.Where(q => q.RiDataMapping.RiDataConfigId == riDataConfigId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (RiDataMappingDetail riDataMappingDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(riDataMappingDetail, true);
                    trails.Add(trail);

                    db.Entry(riDataMappingDetail).State = EntityState.Deleted;
                    db.RiDataMappingDetails.Remove(riDataMappingDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }
    }
}
