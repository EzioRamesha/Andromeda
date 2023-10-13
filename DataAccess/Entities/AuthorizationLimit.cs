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
    [Table("AuthorizationLimits")]
    public class AuthorizationLimit
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int AccessGroupId { get; set; }
            
        [ExcludeTrail]
        public virtual AccessGroup AccessGroup { get; set; }

        [Index]
        public double? PositiveAmountFrom { get; set; }

        [Index]
        public double? PositiveAmountTo { get; set; }

        [Index]
        public double? NegativeAmountFrom { get; set; }

        [Index]
        public double? NegativeAmountTo { get; set; }
        
        [Index]
        public double? Percentage { get; set; }


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

        public AuthorizationLimit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.Any(q => q.Id == id);
            }
        }

        public static bool IsDuplicateAccessGroup(AuthorizationLimit authorizationLimit)
        {
            using (var db = new AppDbContext())
            {
                if (authorizationLimit.AccessGroupId != 0)
                {
                    var query = db.AuthorizationLimits.Where(q => q.AccessGroupId == authorizationLimit.AccessGroupId);
                    if (authorizationLimit.Id != 0)
                    {
                        query = query.Where(q => q.Id != authorizationLimit.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static AuthorizationLimit Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.Count();
            }
        }

        public static IList<AuthorizationLimit> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.ToList();
            }
        }

        public static IList<AuthorizationLimit> Get(int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.Skip(skip).Take(take).ToList();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AuthorizationLimits.Add(this);
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

                entity.AccessGroupId = AccessGroupId;
                entity.PositiveAmountFrom = PositiveAmountFrom;
                entity.PositiveAmountTo = PositiveAmountTo;
                entity.NegativeAmountFrom = NegativeAmountFrom;
                entity.NegativeAmountTo = NegativeAmountTo;
                entity.Percentage = Percentage;
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
                var entity = db.AuthorizationLimits.Where(q => q.Id == id).FirstOrDefault();
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.AuthorizationLimits.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }
    }
}
