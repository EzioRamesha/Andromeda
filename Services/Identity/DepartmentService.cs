using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Identity
{
    public class DepartmentService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Department)),
                Controller = ModuleBo.ModuleController.Department.ToString()
            };
        }

        public static DepartmentBo FormBo(Department entity = null)
        {
            if (entity == null)
                return null;
            return new DepartmentBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                HodUserId = entity.HodUserId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DepartmentBo> FormBos(IList<Department> entities = null)
        {
            if (entities == null)
                return null;
            IList<DepartmentBo> bos = new List<DepartmentBo>() { };
            foreach (Department entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Department FormEntity(DepartmentBo bo = null)
        {
            if (bo == null)
                return null;
            return new Department
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                Name = bo.Name?.Trim(),
                HodUserId = bo.HodUserId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Department department)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(department.Code?.Trim()))
                {
                    var query = db.Departments.Where(q => q.Code.Trim().Equals(department.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (department.Id != 0)
                    {
                        query = query.Where(q => q.Id != department.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static IList<DepartmentBo> Get()
        {
            return FormBos(Department.Get());
        }
        
        public static IList<DepartmentBo> GetExceptShared()
        {
            return FormBos(Department.GetExceptShared());
        }

        public static DepartmentBo Find(int id)
        {
            return FormBo(Department.Find(id));
        }

        public static DepartmentBo Find(int? id)
        {
            return FormBo(Department.Find(id));
        }

        public static int CountByHodUser(int userId)
        {
            return Department.CountByHodUser(userId);
        }

        public static Result Save(ref DepartmentBo bo)
        {
            if (!Department.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DepartmentBo bo, ref TrailObject trail)
        {
            if (!Department.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DepartmentBo bo)
        {
            Department entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DepartmentBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DepartmentBo bo)
        {
            Result result = Result();

            Department entity = Department.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.HodUserId = bo.HodUserId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DepartmentBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DepartmentBo bo)
        {
            Department.Delete(bo.Id);
        }

        public static Result Delete(DepartmentBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                Module.CountByDepartmentId(bo.Id) > 0 ||
                AccessGroup.CountByDepartmentId(bo.Id) > 0 ||
                User.CountByDepartmentId(bo.Id) > 0
            ) {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = Department.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
