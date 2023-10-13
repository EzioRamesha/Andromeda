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
    [Table("Modules")]
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int DepartmentId { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [Required, Index]
        public int Type { get; set; }

        [Required, MaxLength(64), Index]
        public string Controller { get; set; }

        [MaxLength(32)]
        public string Power { get; set; }

        [MaxLength(128)]
        public string PowerAdditional { get; set; }

        public bool Editable { get; set; }

        [Required, MaxLength(64), Index]
        public string Name { get; set; }

        [MaxLength(64)]
        public string ReportPath { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; }

        [ExcludeTrail]
        [ForeignKey(nameof(UpdatedById))]
        public virtual User UpdatedBy { get; set; }

        public int Index { get; set; }

        public bool HideParameters { get; set; }

        public Module()
        {
            Editable = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Module(ModuleBo moduleBo) : this()
        {
            DepartmentId = moduleBo.DepartmentId;
            Type = moduleBo.Type;
            Controller = moduleBo.Controller;
            Power = moduleBo.Power;
            PowerAdditional = moduleBo.PowerAdditional;
            Editable = moduleBo.Editable;
            Name = moduleBo.Name;
            ReportPath = moduleBo.ReportPath;
            CreatedById = moduleBo.CreatedById;
            UpdatedById = moduleBo.UpdatedById;
            Index = moduleBo.Index;
            HideParameters = moduleBo.HideParameters;
        }

        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Module)),
                Controller = ModuleBo.ModuleController.Module.ToString()
            };
        }

        public static bool IsExists(string controller)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Any(q => q.Controller == controller);
            }
        }

        public static Module Find(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.Id == id).FirstOrDefault();
            }
        }

        public static Module FindByController(string controller)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.Controller == controller).FirstOrDefault();
            }
        }

        public static int CountByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.Type == type).Count();
            }
        }

        public static int CountByDepartmentId(int departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.DepartmentId == departmentId).Count();
            }
        }

        public static IList<Module> GetByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.Type == type).ToList();
            }
        }

        public static IList<Module> GetByDepartmentId(int departmentId, int type = 0)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Modules.Where(q => q.DepartmentId == departmentId);
                if (type != 0)
                    return query.Where(q => q.Type == type).OrderBy(q => q.Index).ToList();
                return query.OrderBy(q => q.Type == ModuleBo.TypeReport).ThenBy(q => q.Type).ThenBy(n => n.Index).ToList();
            }
        }

        public string GetFormattedPower(bool superPowerAdditionalDefault = true)
        {
            if (string.IsNullOrEmpty(PowerAdditional) || !superPowerAdditionalDefault)
                return Power;
            return string.Format("{0},{1}", Power, PowerAdditional);
        }

        public DataTrail Save()
        {
            if (IsExists(Controller))
            {
                return Update();
            }
            return Create();
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.Modules.Add(this);
                db.SaveChanges();

                DataTrail trail = new DataTrail(this);
                return trail;
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                Module module = Module.Find(Id);
                if (module == null)
                {
                    module = Module.FindByController(Controller);
                }
                if (module == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(module, this);

                module.DepartmentId = DepartmentId;
                module.Type = Type;
                module.Controller = Controller;
                module.Power = Power;
                module.PowerAdditional = PowerAdditional;
                module.Editable = Editable;
                module.Name = Name;
                module.ReportPath = ReportPath;
                module.UpdatedAt = DateTime.Now;
                module.UpdatedById = UpdatedById != 0 ? UpdatedById : module.UpdatedById;
                module.Index = Index;
                module.HideParameters = HideParameters;

                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public static DataTrail Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                Module module = db.Modules.Where(q => q.Id == id).FirstOrDefault();
                if (module == null)
                {
                    throw new Exception(MessageBag.NoRecordFound);
                }

                DataTrail trail = new DataTrail(module, true);

                db.Entry(module).State = EntityState.Deleted;
                db.Modules.Remove(module);
                db.SaveChanges();

                return trail;
            }
        }

        public static void SeedModules(User superUser)
        {
            TrailObject trail = new TrailObject();
            string table = UtilAttribute.GetTableName(typeof(Module));

            foreach (ModuleBo moduleBo in ModuleBo.GetModules())
            {
                trail = new TrailObject();
                DataTrail dataTrail = null;

                Module module = FindByController(moduleBo.Controller);
                if (module == null)
                {
                    module = new Module(moduleBo)
                    {
                        CreatedById = superUser.Id,
                        UpdatedById = superUser.Id
                    };
                    dataTrail = module.Create();
                }
                else
                {
                    if (!moduleBo.Editable)
                    {
                        module.DepartmentId = moduleBo.DepartmentId;
                        module.Type = moduleBo.Type;
                        module.Controller = moduleBo.Controller;
                        module.Power = moduleBo.Power;
                        module.PowerAdditional = moduleBo.PowerAdditional;
                        module.Editable = moduleBo.Editable;
                        module.Name = moduleBo.Name;
                        module.ReportPath = moduleBo.ReportPath;
                        module.Index = moduleBo.Index;
                        module.HideParameters = moduleBo.HideParameters;

                        dataTrail = module.Update();
                    }
                }

                if (dataTrail != null)
                    dataTrail.Merge(ref trail, table);

                if (trail.IsValid())
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        module.Id,
                        "Create/Update Module",
                        Result(),
                        trail,
                        module.CreatedById
                    );

                    UserTrail userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }
            }
        }
    }
}
