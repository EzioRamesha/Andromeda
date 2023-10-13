using BusinessObject;
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
    [Table("Cedants")]
    public class Cedant
    {
        [Key]
        public int Id { get; set; }

        public int? CedingCompanyTypePickListDetailId { get; set; }

        [ExcludeTrail]
        public virtual PickListDetail CedingCompanyTypePickListDetail { get; set; }

        [Required, MaxLength(255), Index]
        public string Name { get; set; }

        [Required, MaxLength(10), Index]
        public string Code { get; set; }

        [Required, MaxLength(30), Index]
        public string PartyCode { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }

        [MaxLength(10), Index]
        public string AccountCode { get; set; }

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

        public Cedant()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.Any(q => q.Id == id);
            }
        }

        public static Cedant Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Cedant", 81);
                return connectionStrategy.Execute(() =>
                {
                    return db.Cedants.Where(q => q.Id == id).FirstOrDefault();
                });
            }
        }

        public static Cedant FindByName(string name)
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.Where(q => q.Name == name).FirstOrDefault();
            }
        }

        public static Cedant FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Cedant");
                return connectionStrategy.Execute(() =>
                {
                    return db.Cedants.Count();
                });
            }
        }

        public static int CountByCedingCompanyTypePickListDetailId(int CedingCompanyTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.Where(q => q.CedingCompanyTypePickListDetailId == CedingCompanyTypePickListDetailId).Count();
            }
        }

        public static IList<Cedant> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Cedants.OrderBy(q => q.Code).ToList();
            }
        }

        public static IList<Cedant> Get(int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Cedant");
                return connectionStrategy.Execute(() =>
                {
                    return db.Cedants.OrderBy(q => q.Code).Skip(skip).Take(take).ToList();
                });
            }
        }

        public static IList<Cedant> GetNotInCedantWorkgroup(int cedantWorkgroupId)
        {
            using (var db = new AppDbContext())
            {
                var subQuery = db.CedantWorkgroupCedants.Where(c => c.CedantWorkgroupId != cedantWorkgroupId).Select(c => c.CedantId);
                return db.Cedants.Where(q => !subQuery.Contains(q.Id) && q.Status != CedantBo.StatusInactive).OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Cedants.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Cedant cedants = Cedant.Find(Id);
                if (cedants == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedants, this);

                cedants.CedingCompanyTypePickListDetailId = CedingCompanyTypePickListDetailId;
                cedants.Name = Name;
                cedants.Code = Code;
                cedants.PartyCode = PartyCode;
                cedants.Status = Status;
                cedants.Remarks = Remarks;
                cedants.AccountCode = AccountCode;
                cedants.UpdatedAt = DateTime.Now;
                cedants.UpdatedById = UpdatedById ?? cedants.UpdatedById;

                db.Entry(cedants).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Cedant cedants = db.Cedants.Where(q => q.Id == id).FirstOrDefault();
                if (cedants == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(cedants, true);

                db.Entry(cedants).State = EntityState.Deleted;
                db.Cedants.Remove(cedants);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Name);
        }
    }
}
