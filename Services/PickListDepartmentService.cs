using BusinessObject;
using BusinessObject.Identity;
using Services.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;

namespace Services
{
    public class PickListDepartmentService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PickListDepartmentBo)),
            };
        }

        public static PickListDepartmentBo FormBo(PickListDepartment entity = null, bool loadPickList = true, bool loadDepartment = false)
        {
            if (entity == null)
                return null;
            return new PickListDepartmentBo
            {
                PickListId = entity.PickListId,
                PickListBo = loadPickList ? PickListService.Find(entity.PickListId) : null,
                DepartmentId = entity.DepartmentId,
                DepartmentBo = loadDepartment ? DepartmentService.Find(entity.DepartmentId) : null,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PickListDepartmentBo FormSimplifiedBo(PickListDepartment entity = null)
        {
            if (entity == null)
                return null;
            return new PickListDepartmentBo
            {
                PickListId = entity.PickListId,
                DepartmentId = entity.DepartmentId
            };

        }

        public static IList<PickListDepartmentBo> FormBos(IList<PickListDepartment> entities, bool loadPicklist = true, bool loadDepartment = false)
        {
            if (entities == null)
                return null;
            IList<PickListDepartmentBo> bos = new List<PickListDepartmentBo>() { };
            foreach (PickListDepartment entity in entities)
            {
                bos.Add(FormBo(entity, loadPicklist, loadDepartment));
            }
            return bos;
        }

        public static IList<PickListDepartmentBo> FormSimplifiedBos(IList<PickListDepartment> entities = null)
        {
            if (entities == null)
                return null;
            IList<PickListDepartmentBo> bos = new List<PickListDepartmentBo>() { };
            foreach (PickListDepartment entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static PickListDepartment FormEntity(PickListDepartmentBo bo = null)
        {
            if (bo == null)
                return null;
            return new PickListDepartment
            {
                PickListId = bo.PickListId,
                DepartmentId = bo.DepartmentId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static PickListDepartmentBo Find(int pickListId, int departmentId)
        {
            return FormBo(PickListDepartment.Find(pickListId, departmentId));
        }

        public static IList<PickListDepartmentBo> GetByPickListId(int pickListId, bool loadPickList = true, bool loadDepartment = false)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.PickListDepartments
                    .Where(q => q.PickListId == pickListId)
                    .OrderBy(q => q.PickListId)
                    .ToList(), loadPickList, loadDepartment);
            }
        }

        public static Result Create(PickListDepartmentBo bo)
        {
            PickListDepartment entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static Result Create(ref PickListDepartmentBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByPickListId(int pickListId)
        {
            return PickListDepartment.DeleteAllByPickListId(pickListId);
        }

        public static void DeleteAllByPickListId(int pickListId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByPickListId(pickListId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PickListDepartment)));
                }
            }
        }

    }
}