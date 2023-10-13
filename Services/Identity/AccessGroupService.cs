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
    public class AccessGroupService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccessGroup)),
                Controller = ModuleBo.ModuleController.AccessGroup.ToString()
            };
        }

        public static AccessGroupBo FormBo(AccessGroup entity = null)
        {
            if (entity == null)
                return null;
            return new AccessGroupBo
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                Code = entity.Code,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<AccessGroupBo> FormBos(IList<AccessGroup> entities = null)
        {
            if (entities == null)
                return null;
            IList<AccessGroupBo> bos = new List<AccessGroupBo>() { };
            foreach (AccessGroup entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AccessGroup FormEntity(AccessGroupBo entity = null)
        {
            if (entity == null)
                return null;
            return new AccessGroup
            {
                Id = entity.Id,
                Code = entity.Code?.Trim(),
                Name = entity.Name?.Trim(),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return AccessGroup.IsExists(id);
        }

        public static bool IsDuplicateCode(AccessGroup entity)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(entity.Code?.Trim()))
                {
                    var query = db.AccessGroups.Where(q => q.Code.Trim().Equals(entity.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (entity.Id != 0)
                    {
                        query = query.Where(q => q.Id != entity.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static AccessGroupBo Find(int id)
        {
            return FormBo(AccessGroup.Find(id));
        }

        public static int CountByDepartmentId(int departmentId)
        {
            return AccessGroup.CountByDepartmentId(departmentId);
        }

        public static IList<AccessGroupBo> Get()
        {
            return FormBos(AccessGroup.Get());
        }
        
        public static IList<AccessGroupBo> GetByDepartmentId(int? departmentId)
        {
            return FormBos(AccessGroup.GetByDepartmentId(departmentId));
        }

        public static Result Save(ref AccessGroupBo bo)
        {
            if (!AccessGroup.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AccessGroupBo bo, ref TrailObject trail)
        {
            if (!AccessGroup.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AccessGroupBo bo)
        {
            AccessGroup entity = new AccessGroup
            {
                DepartmentId = bo.DepartmentId,
                Code = bo.Code,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };

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

        public static Result Create(ref AccessGroupBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AccessGroupBo bo)
        {
            Result result = Result();

            var entity = AccessGroup.Find(bo.Id);
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
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }

            return result;
        }

        public static Result Update(ref AccessGroupBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Delete(AccessGroupBo bo)
        {
            AccessMatrixService.DeleteAllByAccessGroupId(bo.Id);
            AccessGroup.Delete(bo.Id);

            return Result();
        }

        public static Result Delete(AccessGroupBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if ( UserAccessGroup.CountByAccessGroupId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                AccessMatrixService.DeleteAllByAccessGroupId(bo.Id, ref trail);
                DataTrail dataTrail = AccessGroup.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            
            return result;
        }
    }
}
