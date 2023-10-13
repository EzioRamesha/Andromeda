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
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20), Index]
        public string Code { get; set; }

        [Required, MaxLength(64), Index]
        public string Name { get; set; }

        [Index]
        public int? HodUserId { get; set; }

        [ForeignKey(nameof(HodUserId))]
        [ExcludeTrail]
        public virtual User HodUser { get; set; }

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

        public Department()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Department(DepartmentBo departmentBo) : this()
        {
            Id = departmentBo.Id;
            Code = departmentBo.Code;
            Name = departmentBo.Name;
            HodUserId = departmentBo.HodUserId;
            CreatedById = departmentBo.CreatedById;
            UpdatedById = departmentBo.UpdatedById;
        }

        public static bool IsExists(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Departments.Any(q => q.Id == id);
            }
        }

        public bool IsDuplicateCode()
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    var query = db.Departments.Where(q => q.Code.Trim().Equals(Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (Id != 0)
                    {
                        query = query.Where(q => q.Id != Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Department Find(int id)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Department");
                return connectionStrategy.Execute(() => db.Departments.Where(q => q.Id == id).FirstOrDefault());
            }
        }

        public static Department Find(int? id)
        {
            using (var db = new AppDbContext())
            {
                return db.Departments.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static IList<Department> Get()
        {
            using (var db = new AppDbContext())
            {
                return db.Departments.ToList();
            }
        }

        public static IList<Department> GetExceptShared()
        {
            using (var db = new AppDbContext())
            {
                return db.Departments.Where(q => q.Id != DepartmentBo.DepartmentShared).ToList();
            }
        }

        public static int CountByHodUser(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.Departments.Where(q => q.HodUserId == userId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Departments.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Department department = Department.Find(Id);
                if (department == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(department, this);

                department.Code = Code;
                department.Name = Name;
                department.HodUserId = HodUserId;
                department.UpdatedAt = DateTime.Now;
                department.UpdatedById = UpdatedById ?? department.UpdatedById;

                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Department department = db.Departments.Where(q => q.Id == id).FirstOrDefault();
                if (department == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(department, true);

                db.Entry(department).State = EntityState.Deleted;
                db.Departments.Remove(department);
                db.SaveChanges();

                return trail;
            }
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Department)),
                Controller = ModuleBo.ModuleController.Department.ToString()
            };
        }

        public static void SeedDepartments(User superUser)
        {
            TrailObject trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(Module));

            foreach (DepartmentBo departmentBo in DepartmentBo.GetDepartments())
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                Department department = Find(departmentBo.Id);
                if (department == null)
                {
                    department = new Department(departmentBo)
                    {
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id
                    };
                    dataTrail = department.Create();
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        department.Id,
                        "Create Department",
                        Result(),
                        trail,
                        department.CreatedById
                    );

                    UserTrail userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }
            }
        }
    }
}
