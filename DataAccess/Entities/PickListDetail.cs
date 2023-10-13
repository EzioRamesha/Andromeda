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
    [Table("PickListDetails")]
    public class PickListDetail
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int PickListId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(PickListId))]
        public virtual PickList PickList { get; set; }

        public int SortIndex { get; set; }

        [MaxLength(64), Index]
        public string Code { get; set; }

        [Required, MaxLength(128), Index]
        public string Description { get; set; }

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

        public PickListDetail()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Any(q => q.Id == id);
            }
        }

        public static PickListDetail Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("PickLIstDetail");
                return connectionStrategy.Execute(() => db.PickListDetails.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static PickListDetail FindByPickListIdCode(int pickListId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails
                    .Where(q => q.PickListId == pickListId)
                    .Where(q => q.Code.Trim() == code.Trim())
                    .FirstOrDefault();
            }
        }

        public static PickListDetail FindByStandardOutputIdCode(int standardOutputId, string code = null)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails
                    .Where(q => q.PickList.StandardOutputId == standardOutputId)
                    .Where(q => q.Code.Trim() == code.Trim())
                    .OrderBy(q => q.SortIndex).FirstOrDefault();
            }
        }

        public static PickListDetail FindByStandardClaimDataOutputIdCode(int standardClaimDataOutputId, string code = null)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails
                    .Where(q => q.PickList.StandardClaimDataOutputId == standardClaimDataOutputId)
                    .Where(q => q.Code.Trim() == code.Trim())
                    .OrderBy(q => q.SortIndex).FirstOrDefault();
            }
        }

        public static PickListDetail FindByPickListIdSortIndex(int pickListId, int sortIndex)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickListId == pickListId && q.SortIndex == sortIndex).FirstOrDefault();
            }
        }

        public static int CountByStandardOutputIdCode(int standardOutputId, string code)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(128, "PickListDetail");

                return connectionStrategy.Execute(() =>
                {
                    return db.PickListDetails
                     .Where(q => q.PickList.StandardOutputId == standardOutputId)
                     .Where(q => q.Code.Trim() == code.Trim())
                     .Count();
                });

            }
        }

        public static int CountByPickListIdSortIndex(int pickListId, int sortIndex)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickListId == pickListId && q.SortIndex == sortIndex).Count();
            }
        }

        public static IList<PickListDetail> GetByPickListId(int pickListId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("PickLIstDetail");
                return connectionStrategy.Execute(() => db.PickListDetails.Where(q => q.PickListId == pickListId).OrderBy(q => q.SortIndex).ToList());
            }
        }

        public static IList<PickListDetail> GetByStandardOutputId(int standardOutputId)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickList.StandardOutputId == standardOutputId).OrderBy(q => q.SortIndex).ToList();
            }
        }

        public static IList<PickListDetail> GetByStandardClaimDataOutputId(int standardClaimDataOutputId)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickList.StandardClaimDataOutputId == standardClaimDataOutputId).OrderBy(q => q.SortIndex).ToList();
            }
        }

        public static IList<PickListDetail> GetByPickListIdExcept(int pickListId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickListId == pickListId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.PickListDetails.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                PickListDetail pickListDetail = PickListDetail.Find(Id);
                if (pickListDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(pickListDetail, this);

                pickListDetail.PickListId = PickListId;
                pickListDetail.SortIndex = SortIndex;
                pickListDetail.Code = Code;
                pickListDetail.Description = Description;
                pickListDetail.UpdatedAt = DateTime.Now;
                pickListDetail.UpdatedById = UpdatedById ?? pickListDetail.UpdatedById;

                db.Entry(pickListDetail).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                PickListDetail pickListDetail = db.PickListDetails.Where(q => q.Id == id).FirstOrDefault();
                if (pickListDetail == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(pickListDetail, true);

                db.Entry(pickListDetail).State = EntityState.Deleted;
                db.PickListDetails.Remove(pickListDetail);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByPickListId(int pickListId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PickListDetails.Where(q => q.PickListId == pickListId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (PickListDetail pickListDetail in query.ToList())
                {
                    DataTrail trail = new DataTrail(pickListDetail, true);
                    trails.Add(trail);

                    db.Entry(pickListDetail).State = EntityState.Deleted;
                    db.PickListDetails.Remove(pickListDetail);
                }

                db.SaveChanges();
                return trails;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
