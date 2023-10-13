using BusinessObject.Identity;
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
    [Table("AccessMatrices")]
    public class AccessMatrix
    {
        [Key, Column(Order = 0)]
        public int AccessGroupId { get; set; }

        [Key, Column(Order = 1)]
        public int ModuleId { get; set; }

        [MaxLength(128), Index]
        public string Power { get; set; }

        [ExcludeTrail]
        public virtual AccessGroup AccessGroup { get; set; }

        [ExcludeTrail]
        public virtual Module Module { get; set; }

        public AccessMatrix() { }

        public AccessMatrix(AccessMatrixBo accessMatrixBo)
        {
            AccessGroupId = accessMatrixBo.AccessGroupId;
            ModuleId = accessMatrixBo.ModuleId;
            Power = accessMatrixBo.Power;
        }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", ModuleId, AccessGroupId);
        }

        public static bool IsExists(int moduleId, int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessMatrices
                    .Any(q => q.ModuleId == moduleId && q.AccessGroupId == accessGroupId);
            }
        }

        public static AccessMatrix Find(int moduleId, int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessMatrices
                    .Where(q => q.ModuleId == moduleId && q.AccessGroupId == accessGroupId)
                    .FirstOrDefault();
            }
        }

        public static AccessMatrix FindByController(string controller, int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessMatrices
                    .Where(q => q.Module.Controller == controller)
                    .Where(q => q.AccessGroupId == accessGroupId)
                    .FirstOrDefault();
            }
        }
        
        public static List<AccessMatrix> GetByModule(int moduleId)
        {
            using (var db = new AppDbContext())
            {
                return db.AccessMatrices
                    .Where(q => q.ModuleId == moduleId)
                    .ToList();
            }
        }

        public DataTrail Save()
        {
            if (IsExists(ModuleId, AccessGroupId))
            {
                return Update();
            }
            return Create();
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext())
            {
                db.AccessMatrices.Add(this);
                db.SaveChanges();

                return new DataTrail(this);
            }
        }

        public DataTrail Update()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(ModuleId, AccessGroupId);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, this);

                entity.Power = Power;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return trail;
            }
        }

        public DataTrail Delete()
        {
            using (var db = new AppDbContext())
            {
                var entity = Find(ModuleId, AccessGroupId);
                if (entity == null)
                    throw new Exception(MessageBag.NoRecordFound);

                var trail = new DataTrail(entity, true);

                db.Entry(entity).State = EntityState.Deleted;
                db.AccessMatrices.Remove(entity);
                db.SaveChanges();

                return trail;
            }
        }

        public static void DeleteAllByAccessGroupId(int accessGroupId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccessMatrices.Where(q => q.AccessGroupId == accessGroupId);

                foreach (AccessMatrix am in query.ToList())
                {
                    var dataTrail = new DataTrail(am, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AccessMatrix)), am.PrimaryKey());

                    db.Entry(am).State = EntityState.Deleted;
                    db.AccessMatrices.Remove(am);
                }

                // Mehtod 1
                //db.AccessMatrices.RemoveRange(query);

                // Mehtod 2
                //db.Database.ExecuteSqlCommand("DELETE FROM AccessMatrices WHERE AccessGroupId = {0}", accessGroupId);
                db.SaveChanges();
            }
        }

        public static void DeleteAllByModuleId(int moduleId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccessMatrices.Where(q => q.ModuleId == moduleId);

                foreach (AccessMatrix am in query.ToList())
                {
                    var dataTrail = new DataTrail(am, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AccessMatrix)), am.PrimaryKey());

                    db.Entry(am).State = EntityState.Deleted;
                    db.AccessMatrices.Remove(am);
                }

                // Mehtod 1
                //db.AccessMatrices.RemoveRange(query);

                // Mehtod 2
                //db.Database.ExecuteSqlCommand("DELETE FROM AccessMatrices WHERE ModuleId = {0}", moduleId);

                db.SaveChanges();
            }
        }

        public static void SeedSuperAccessMatrices(AccessGroup superAccessGroup)
        {
            using (var db = new AppDbContext())
            {
                var table = UtilAttribute.GetTableName(typeof(AccessMatrix));
                var trail = new TrailObject();

                var modules = db.Modules.ToList();
                foreach (Module module in modules)
                {
                    // to prevent super user from being listed in the Checklist PIC dropdown
                    bool defaultPowerAdditional = true;
                    if ((module.Controller == BusinessObject.ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString() || module.Controller == BusinessObject.ModuleBo.ModuleController.TreatyPricingQuotationWorkflow.ToString()) && superAccessGroup.Id == User.DefaultSuperUserId)
                        defaultPowerAdditional = false;

                    var bo = new AccessMatrixBo
                    {
                        AccessGroupId = superAccessGroup.Id,
                        ModuleId = module.Id,
                        Power = module.GetFormattedPower(defaultPowerAdditional),
                    };

                    var entity = new AccessMatrix(bo);

                    var dataTrail = entity.Save();
                    dataTrail.Merge(ref trail, table, entity.PrimaryKey());
                }

                if (trail.IsValid())
                {
                    var userTrailBo = new UserTrailBo(
                        superAccessGroup.Id,
                        "Update Super Access Matrices",
                        AccessGroup.Result(),
                        trail,
                        superAccessGroup.CreatedById
                    );

                    var userTrail = new UserTrail(userTrailBo);
                    userTrail.Create();
                }
            }
        }
    }
}
