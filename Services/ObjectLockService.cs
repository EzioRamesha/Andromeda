using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ObjectLockService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ObjectLock)),
                Controller = ModuleBo.ModuleController.ObjectLock.ToString()
            };
        }

        public static ObjectLockBo FormBo(ObjectLock entity = null)
        {
            if (entity == null)
                return null;
            return new ObjectLockBo
            {
                Id = entity.Id,
                ModuleId = entity.ModuleId,
                ObjectId = entity.ObjectId,
                LockedById = entity.LockedById,
                LockedByBo = UserService.Find(entity.LockedById),
                ExpiresAt = entity.ExpiresAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<ObjectLockBo> FormBos(IList<ObjectLock> entities = null)
        {
            if (entities == null)
                return null;
            IList<ObjectLockBo> bos = new List<ObjectLockBo>() { };
            foreach (ObjectLock entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ObjectLock FormEntity(ObjectLockBo bo = null)
        {
            if (bo == null)
                return null;
            return new ObjectLock
            {
                Id = bo.Id,
                ModuleId = bo.ModuleId,
                ObjectId = bo.ObjectId,
                LockedById = bo.LockedById,
                ExpiresAt = bo.ExpiresAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ObjectLock.IsExists(id);
        }

        public static ObjectLockBo Find(int? id)
        {
            return FormBo(ObjectLock.Find(id));
        }

        public static ObjectLockBo Find(int moduleId, int objectId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ObjectLocks
                    .Where(q => q.ModuleId == moduleId)
                    .Where(q => q.ObjectId == objectId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static ObjectLockBo Find(string controller, int objectId, int lockedById)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ObjectLocks
                    .Where(q => q.Module.Controller == controller)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.LockedById == lockedById);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<ObjectLockBo> GetByLockedUser(int lockedById)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ObjectLocks
                    .Where(q => q.LockedById == lockedById);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref ObjectLockBo bo)
        {
            if (!ObjectLock.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ObjectLockBo bo, ref TrailObject trail)
        {
            if (!ObjectLock.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ObjectLockBo bo)
        {
            ObjectLock entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ObjectLockBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ObjectLockBo bo)
        {
            Result result = Result();

            ObjectLock entity = ObjectLock.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ModuleId = bo.ModuleId;
                entity.ObjectId = bo.ObjectId;
                entity.LockedById = bo.LockedById;
                entity.ExpiresAt = bo.ExpiresAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ObjectLockBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ObjectLockBo bo)
        {
            ObjectLock.Delete(bo.Id);
        }

        public static Result Delete(ObjectLockBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = ObjectLock.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
