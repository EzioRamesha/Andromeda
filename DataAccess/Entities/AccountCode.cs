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
    [Table("AccountCodes")]
    public class AccountCode
    {
        [Key]
        public int Id { get; set; }

        [Required, Index, MaxLength(30)]
        public string Code { get; set; }

        [Required, Index]
        public int ReportingType { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [Index]
        public int? Type { get; set; }

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

        public AccountCode()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodes.Any(q => q.Id == id);
            }
        }

        public static AccountCode Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AccountCode Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodes.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AccountCode FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodes.Where(q => q.Code == code).FirstOrDefault();
            }
        }

        public static IList<AccountCode> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AccountCodes.OrderBy(q => q.Code).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AccountCodes.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(Id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.Code = Code;
                entity.ReportingType = ReportingType;
                entity.Description = Description;
                entity.Type = Type;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById ?? entity.UpdatedById;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(id);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.AccountCodes.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
                return string.Format("{0} - {1}", Code, AccountCodeBo.GetReportingTypeName(ReportingType));
            return string.Format("{0} - {1} - {2}", Code, AccountCodeBo.GetReportingTypeName(ReportingType), Description);
        }
    }
}
