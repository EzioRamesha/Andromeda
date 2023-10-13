using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ModuleService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Module)),
                Controller = ModuleBo.ModuleController.Module.ToString()
            };
        }

        public static ModuleBo FormBo(Module entity = null, int accessGroupId = 0)
        {
            if (entity == null)
                return null;
            return new ModuleBo
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                Type = entity.Type,
                Controller = entity.Controller,
                Power = entity.Power,
                PowerAdditional = entity.PowerAdditional,
                Editable = entity.Editable,
                Name = entity.Name,
                ReportPath = entity.ReportPath,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                Index = entity.Index,
                HideParameters = entity.HideParameters,

                AccessMatrixBo = AccessMatrixService.Find(entity.Id, accessGroupId),
                DepartmentBo = DepartmentService.Find(entity.DepartmentId)
            };
        }

        public static IList<ModuleBo> FormBos(IList<Module> entities = null, int accessGroupId = 0)
        {
            if (entities == null)
                return null;
            IList<ModuleBo> bos = new List<ModuleBo>() { };
            foreach (Module entity in entities)
            {
                bos.Add(FormBo(entity, accessGroupId));
            }
            return bos;
        }

        public static Module FormEntity(ModuleBo bo = null)
        {
            if (bo == null)
                return null;
            return new Module
            {
                Id = bo.Id,
                DepartmentId = bo.DepartmentId,
                Type = bo.Type,
                Controller = bo.Controller,
                Power = bo.Power,
                PowerAdditional = bo.PowerAdditional,
                Editable = bo.Editable,
                Name = bo.Name,
                ReportPath = bo.ReportPath,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                Index = bo.Index,
                HideParameters = bo.HideParameters,
            };
        }

        public static bool IsExists(string controller)
        {
            return Module.IsExists(controller);
        }

        public static bool IsDuplicateController(Module module)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(module.Controller))
                {
                    var query = db.Modules.Where(q => q.Controller == module.Controller);
                    if (module.Id != 0)
                    {
                        query = query.Where(q => q.Id != module.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static ModuleBo Find(int id)
        {
            return FormBo(Module.Find(id));
        }

        public static ModuleBo FindByController(string controller)
        {
            return FormBo(Module.FindByController(controller));
        }

        public static int CountByType(int type)
        {
            return Module.CountByType(type);
        }

        public static int CountByDepartmentId(int departmentId)
        {
            return Module.CountByDepartmentId(departmentId);
        }

        public static IList<ModuleBo> GetByDepartmentId(int departmentId, int accessGroupId = 0, int type = 0)
        {
            return FormBos(Module.GetByDepartmentId(departmentId, type), accessGroupId);
        }

        public static IList<ModuleBo> GetByType(int type, int accessGroupId = 0)
        {
            return FormBos(Module.GetByType(type), accessGroupId);
        }

        public static int GetMaxIndexByDepartment(int departmentId)
        {
            using (var db = new AppDbContext())
            {
                return db.Modules.Where(q => q.DepartmentId == departmentId).Max(q => q.Index);
            }
        }

        public static Result Create(ref ModuleBo bo)
        {
            Module entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateController(entity))
            {
                result.AddTakenError("Controller", bo.Controller);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ModuleBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            result.Table = UtilAttribute.GetTableName(typeof(Module));
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);

                AccessGroup accessGroup = AccessGroup.GetSuperAccessGroup();
                if (AccessGroup.IsExists(accessGroup.Id))
                {
                    AccessMatrixBo accessMatrixBo = new AccessMatrixBo
                    {
                        AccessGroupId = accessGroup.Id,
                        ModuleId = bo.Id,
                        Power = bo.Power,
                    };
                    //Result accessMatrixResult = AccessMatrixService.Save(accessMatrixBo, ref trail);
                }
            }
            return result;
        }

        public static Result Update(ref ModuleBo bo)
        {
            Result result = Result();

            Module entity = Module.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateController(FormEntity(bo)))
            {
                result.AddTakenError("Controller", bo.Controller);
            }

            if (result.Valid)
            {
                entity.DepartmentId = bo.DepartmentId;
                entity.Type = bo.Type;
                entity.Controller = bo.Controller;
                entity.Power = bo.Power;
                entity.PowerAdditional = bo.PowerAdditional;
                entity.Editable = bo.Editable;
                entity.Name = bo.Name;
                entity.ReportPath = bo.ReportPath;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                entity.Index = bo.Index;
                entity.HideParameters = bo.HideParameters;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ModuleBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            result.Table = UtilAttribute.GetTableName(typeof(Module));
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ModuleBo bo)
        {
            AccessMatrixService.DeleteAllByModuleId(bo.Id);
            Module.Delete(bo.Id);
        }

        public static Result Delete(ModuleBo bo, ref TrailObject trail)
        {
            Result result = Result();

            AccessMatrixService.DeleteAllByModuleId(bo.Id, ref trail);
            DataTrail dataTrail = Module.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
