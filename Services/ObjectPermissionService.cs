using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;

namespace Services
{
    public class ObjectPermissionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ObjectPermission)),
                Controller = ModuleBo.ModuleController.ObjectPermission.ToString()
            };
        }

        public static ObjectPermissionBo FormBo(ObjectPermission entity = null)
        {
            if (entity == null)
                return null;

            return new ObjectPermissionBo
            {
                Id = entity.Id,
                ObjectId = entity.ObjectId,
                Type = entity.Type,
                DepartmentId = entity.DepartmentId,
                CreatedById = entity.CreatedById,
            };
        }

        public static IList<ObjectPermissionBo> FormBos(IList<ObjectPermission> entities = null)
        {
            if (entities == null)
                return null;
            IList<ObjectPermissionBo> bos = new List<ObjectPermissionBo>() { };
            foreach (ObjectPermission entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ObjectPermission FormEntity(ObjectPermissionBo bo = null)
        {
            if (bo == null)
                return null;
            return new ObjectPermission
            {
                Id = bo.Id,
                ObjectId = bo.ObjectId,
                Type = bo.Type,
                DepartmentId = bo.DepartmentId,
                CreatedById = bo.CreatedById,
            };
        }

        public static ObjectPermissionBo Find(int id)
        {
            return FormBo(ObjectPermission.Find(id));
        }

        public static ObjectPermissionBo Find(int type, int objectId)
        {
            return FormBo(ObjectPermission.Find(type, objectId));
        }

        public static Result Create(ref ObjectPermissionBo bo)
        {
            ObjectPermission entity = FormEntity(bo);
            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ObjectPermissionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ObjectPermissionBo bo)
        {
            ObjectPermission.Delete(bo.Id);
        }

        public static Result Delete(ObjectPermissionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ObjectPermission.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
