using BusinessObject.Identity;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.Identity
{
    public class AccessMatrixService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccessMatrix)),
            };
        }

        public static AccessMatrixBo FormBo(AccessMatrix entity = null)
        {
            if (entity == null)
                return null;

            return new AccessMatrixBo
            {
                AccessGroupId = entity.AccessGroupId,
                ModuleId = entity.ModuleId,
                Power = entity.Power,
            };
        }

        public static IList<AccessMatrixBo> FormBos(IList<AccessMatrix> entities = null)
        {
            if (entities == null)
                return null;
            IList<AccessMatrixBo> bos = new List<AccessMatrixBo>() { };
            foreach (AccessMatrix entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AccessMatrix FormEntity(AccessMatrixBo entity = null)
        {
            if (entity == null)
                return null;
            return new AccessMatrix
            {
                AccessGroupId = entity.AccessGroupId,
                ModuleId = entity.ModuleId,
                Power = entity.Power,
            };
        }

        public static bool IsExists(int moduleId, int accessGroupId)
        {
            return AccessMatrix.IsExists(moduleId, accessGroupId);
        }

        public static AccessMatrixBo Find(int moduleId, int accessGroupId)
        {
            return FormBo(AccessMatrix.Find(moduleId, accessGroupId));
        }

        public static AccessMatrixBo FindByController(string controller, int accessGroupId)
        {
            return FormBo(AccessMatrix.FindByController(controller, accessGroupId));
        }
        
        public static IList<AccessMatrixBo> GetByModule(int moduleId)
        {

            return FormBos(AccessMatrix.GetByModule(moduleId));
        }

        public static Result Save(AccessMatrixBo bo)
        {
            if (!AccessMatrix.IsExists(bo.ModuleId, bo.AccessGroupId))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(AccessMatrixBo bo, ref TrailObject trail)
        {
            if (!AccessMatrix.IsExists(bo.ModuleId, bo.AccessGroupId))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(AccessMatrixBo bo)
        {
            AccessMatrix entity = FormEntity(bo);
            Result result = new Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static Result Create(AccessMatrixBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            result.Table = UtilAttribute.GetTableName(typeof(AccessMatrix));
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static Result Update(AccessMatrixBo bo)
        {
            Result result = new Result();

            var entity = AccessMatrix.Find(bo.ModuleId, bo.AccessGroupId);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.Power = bo.Power;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(AccessMatrixBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            result.Table = UtilAttribute.GetTableName(typeof(AccessMatrix));
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static DataTrail Delete(AccessMatrixBo bo)
        {
            AccessMatrix entity = FormEntity(bo);
            return entity.Delete();
        }

        public static void Delete(AccessMatrixBo bo, ref TrailObject trail)
        {
            DataTrail dataTrail = Delete(bo);
            dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AccessMatrix)), bo.PrimaryKey());
        }

        public static void DeleteAllByAccessGroupId(int accessGroupId)
        {
            var trail = new TrailObject();
            AccessMatrix.DeleteAllByAccessGroupId(accessGroupId, ref trail);
        }

        public static void DeleteAllByAccessGroupId(int accessGroupId, ref TrailObject trail)
        {
            AccessMatrix.DeleteAllByAccessGroupId(accessGroupId, ref trail);
        }

        public static void DeleteAllByModuleId(int moduleId)
        {
            var trail = new TrailObject();
            AccessMatrix.DeleteAllByModuleId(moduleId, ref trail);
        }

        public static void DeleteAllByModuleId(int moduleId, ref TrailObject trail)
        {
            AccessMatrix.DeleteAllByModuleId(moduleId, ref trail);
        }
    }
}
