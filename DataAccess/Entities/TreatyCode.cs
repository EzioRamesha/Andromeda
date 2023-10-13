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
    [Table("TreatyCodes")]
    public class TreatyCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TreatyId { get; set; }

        [ForeignKey(nameof(TreatyId))]
        [ExcludeTrail]
        public virtual Treaty Treaty { get; set; }

        [Required, MaxLength(35), Index]
        public string Code { get; set; }

        public int? OldTreatyCodeId { get; set; }

        public string AccountFor { get; set; }

        [Index]
        public int? TreatyTypePickListDetailId { get; set; }

        [ForeignKey(nameof(TreatyTypePickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail TreatyTypePickListDetail { get; set; }

        [Index]
        public int? TreatyStatusPickListDetailId { get; set; }

        [ForeignKey(nameof(TreatyStatusPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail TreatyStatusPickListDetail { get; set; }

        [MaxLength(128), Index]
        public string TreatyNo { get; set; }

        [MaxLength(255), Index]
        public string Description { get; set; }

        public int Status { get; set; }

        public int? LineOfBusinessPickListDetailId { get; set; }

        [ForeignKey(nameof(LineOfBusinessPickListDetailId))]
        [ExcludeTrail]
        public virtual PickListDetail LineOfBusinessPickListDetail { get; set; }

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

        public TreatyCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Any(q => q.Id == id);
            }
        }

        public static TreatyCode Find(int id)
        {
            using (var db = new AppDbContext())
            {
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("TreatyCode", 102);

                return connectionStrategy.Execute(() =>
                {
                    return db.TreatyCodes.Where(q => q.Id == id).FirstOrDefault();
                });
            }
        }

        public static TreatyCode Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static TreatyCode FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(125, "TreatyCode");

                return connectionStrategy.Execute(() => { return db.TreatyCodes.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault(); });
            }
        }

        public static TreatyCode FindByTreatyIdCode(int treatyId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Treaty.Id == treatyId).Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static TreatyCode FindByCedantIdCode(int cedantId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountDistinctByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("TreatyCode");

                return connectionStrategy.Execute(() =>
                {
                    return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).GroupBy(q => q.Code).Select(q => q.FirstOrDefault()).Count();
                });
            }
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.TreatyId == treatyId).Count();
            }
        }

        public static int CountByCodeStatus(string code, int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes.Where(q => q.Code.Trim() == code.Trim());
                if (status != null)
                    query = query.Where(q => q.Status == status);
                return query.Count();
            }
        }

        public static int CountByCedantIdCodeStatus(int cedantId, string code, int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).Where(q => q.Code.Trim() == code.Trim());
                if (status != null)
                    query = query.Where(q => q.Status == status);
                return query.Count();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Code.Trim() == code.Trim()).Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Status == status).Count();
            }
        }

        public static int CountByTreatyTypePickListDetailId(int treatyTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.TreatyTypePickListDetailId == treatyTypePickListDetailId).Count();
            }
        }

        public static int CountByTreatyStatusPickListDetailId(int treatyStatusPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.TreatyStatusPickListDetailId == treatyStatusPickListDetailId).Count();
            }
        }

        public static IList<TreatyCode> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCode> GetByStatus(int? status = null, int? selectedId = null, bool isUniqueCode = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                if (isUniqueCode)
                    return query.GroupBy(q => q.Code).Select(q => q.FirstOrDefault()).OrderBy(q => q.Code).ToList();

                return query.OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCode> GetByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.TreatyId == treatyId).OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCode> GetDistinctByCedantId(int cedantId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0; // execution timeout

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("TreatyCode");

                return connectionStrategy.Execute(() =>
                {
                    return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).GroupBy(q => q.Code).Select(q => q.FirstOrDefault()).OrderBy(q => q.Code).Skip(skip).Take(take).ToList();
                });
            }
        }

        public static IList<TreatyCode> GetByCedantId(int? cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCode> GetByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.Code.Trim() == code.Trim()).ToList();
            }
        }

        public static IList<TreatyCode> GetIndexByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                if (cedantId == 0)
                {
                    return db.TreatyCodes.OrderBy(q => q.Code).ToList();
                }
                return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCode> GetByTreatyIdExcept(int treatyId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.TreatyId == treatyId && !ids.Contains(q.Id)).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.TreatyCodes.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                TreatyCode treatyCode = TreatyCode.Find(Id);
                if (treatyCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyCode, this);

                treatyCode.TreatyId = TreatyId;
                treatyCode.Code = Code;
                treatyCode.OldTreatyCodeId = OldTreatyCodeId;
                treatyCode.AccountFor = AccountFor;
                treatyCode.TreatyTypePickListDetailId = TreatyTypePickListDetailId;
                treatyCode.TreatyStatusPickListDetailId = TreatyStatusPickListDetailId;
                treatyCode.TreatyNo = TreatyNo;
                treatyCode.Description = Description;
                treatyCode.Status = Status;
                treatyCode.LineOfBusinessPickListDetailId = LineOfBusinessPickListDetailId;
                treatyCode.UpdatedAt = DateTime.Now;
                treatyCode.UpdatedById = UpdatedById ?? treatyCode.UpdatedById;

                db.Entry(treatyCode).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                TreatyCode treatyCode = db.TreatyCodes.Where(q => q.Id == id).FirstOrDefault();
                if (treatyCode == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(treatyCode, true);

                db.Entry(treatyCode).State = EntityState.Deleted;
                db.TreatyCodes.Remove(treatyCode);
                db.SaveChanges();

                return trail;
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes.Where(q => q.TreatyId == treatyId);

                List<DataTrail> trails = new List<DataTrail>();
                foreach (TreatyCode treatyCode in query.ToList())
                {
                    DataTrail trail = new DataTrail(treatyCode, true);
                    trails.Add(trail);

                    db.Entry(treatyCode).State = EntityState.Deleted;
                    db.TreatyCodes.Remove(treatyCode);
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
