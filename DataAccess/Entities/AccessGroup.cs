using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
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
    [Table("AccessGroups")]
    public class AccessGroup
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DepartmentId { get; set; }

        [ExcludeTrail]
        public virtual Department Department { get; set; }

        [Required, MaxLength(20), Index]
        public string Code { get; set; }

        [Required, MaxLength(64), Index]
        public string Name { get; set; }

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

        public static string DefaultSuperCode = "SUPER";
        public static string DefaultSuperName = "Super Access Group";

        public AccessGroup()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Any(q => q.Id == id);
            }
        }

        public static AccessGroup Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static AccessGroup FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
            }
        }

        public static int CountByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.Code.Trim() == code.Trim()).Count();
            }
        }

        public static int CountByDepartmentId(int departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.DepartmentId == departmentId).Count();
            }
        }

        public static AccessGroup GetSuperAccessGroup()
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.Code == DefaultSuperCode).FirstOrDefault();
            }
        }

        public static IList<AccessGroup> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.ToList();
            }
        }
        
        public static IList<AccessGroup> GetByDepartmentId(int? departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessGroups.Where(q => q.DepartmentId == departmentId).ToList();
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccessGroup)),
                Controller = ModuleBo.ModuleController.AccessGroup.ToString()
            };
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AccessGroups.Add(this);
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
                entity.Name = Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = UpdatedById != 0 ? UpdatedById : entity.UpdatedById;

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
                db.AccessGroups.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static AccessGroup SeedSuperAccessGroup(User superUser)
        {
            var entity = GetSuperAccessGroup();
            if (entity == null)
            {
                entity = new AccessGroup()
                {
                    DepartmentId = DepartmentBo.DepartmentIT,
                    Code = DefaultSuperCode,
                    Name = DefaultSuperName,
                    CreatedById = superUser.Id,
                    UpdatedById = superUser.Id,
                };

                var table = UtilAttribute.GetTableName(typeof(AccessGroup));
                var dataTrail = entity.Create();
                var trail = new TrailObject();
                dataTrail.Merge(ref trail, table);

                dataTrail = superUser.AddToAccessGroup(entity.Id, out UserAccessGroup uag);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserAccessGroup)), uag.PrimaryKey());

                var userTrailBo = new UserTrailBo(
                    entity.Id,
                    "Insert Super Access Group",
                    Result(),
                    trail,
                    superUser.Id
                );

                var userTrail = new UserTrail(userTrailBo);
                userTrail.Create();
            }

            return entity;
        }
    }
}
