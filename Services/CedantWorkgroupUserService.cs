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
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CedantWorkgroupUserService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(CedantWorkgroupUser)),
            };
        }

        public static CedantWorkgroupUserBo FormBo(CedantWorkgroupUser entity = null)
        {
            if (entity == null)
                return null;

            UserBo userBo = UserService.Find(entity.UserId);
            if (userBo == null)
                return null;

            return new CedantWorkgroupUserBo
            {
                CedantWorkgroupId = entity.CedantWorkgroupId,
                UserId = entity.UserId,
                UserName = userBo.UserName
            };
        }

        public static IList<CedantWorkgroupUserBo> FormBos(IList<CedantWorkgroupUser> entities = null)
        {
            if (entities == null)
                return null;
            IList<CedantWorkgroupUserBo> bos = new List<CedantWorkgroupUserBo>() { };
            foreach (CedantWorkgroupUser entity in entities)
            {
                var bo = FormBo(entity);
                if (bo == null)
                    continue;

                bos.Add(bo);
            }
            return bos;
        }

        public static CedantWorkgroupUser FormEntity(CedantWorkgroupUserBo entity = null)
        {
            if (entity == null)
                return null;
            return new CedantWorkgroupUser
            {
                CedantWorkgroupId = entity.CedantWorkgroupId,
                UserId = entity.UserId,
            };
        }

        public static bool IsExists(int cedantWorkgroupId, int userId)
        {
            return CedantWorkgroupUser.IsExists(cedantWorkgroupId, userId);
        }

        public static bool IsDuplicate(int cedantWorkgroupId, int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Any(q => q.CedantWorkgroupId != cedantWorkgroupId && q.UserId == userId);
            }
        }

        public static bool IsUserExists(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.CedantWorkgroupUsers.Any(q => q.UserId == userId);
            }
        }

        public static CedantWorkgroupUserBo Find(int cedantWorkgroupId, int userId)
        {
            return FormBo(CedantWorkgroupUser.Find(cedantWorkgroupId, userId));
        }

        public static IList<CedantWorkgroupUserBo> GetByCedantWorkgroupId(int cedantWorkgroupId)
        {
            return FormBos(CedantWorkgroupUser.GetByCedantWorkgroupId(cedantWorkgroupId));
        }

        public static IList<CedantWorkgroupUserBo> GetByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.CedantWorkgroupUsers.Where(q => q.UserId == userId).ToList());
            }
        }

        public static IList<CedantWorkgroupUserBo> GetByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> userIds)
        {
            return FormBos(CedantWorkgroupUser.GetByCedantWorkgroupIdExcept(cedantWorkgroupId, userIds));
        }

        public static Result Create(CedantWorkgroupUserBo bo)
        {
            CedantWorkgroupUser entity = FormEntity(bo);
            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static Result Create(CedantWorkgroupUserBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static DataTrail Delete(CedantWorkgroupUserBo bo)
        {
            CedantWorkgroupUser entity = FormEntity(bo);
            return entity.Delete();
        }

        public static void Delete(CedantWorkgroupUserBo bo, ref TrailObject trail)
        {
            DataTrail dataTrail = Delete(bo);
            dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupUser)), bo.PrimaryKey());
        }

        public static void DeleteAllByCedantWorkgroupId(int cedantWorkgroupId, ref TrailObject trail)
        {
            foreach (CedantWorkgroupUserBo bo in GetByCedantWorkgroupId(cedantWorkgroupId))
            {
                DataTrail dataTrail = Delete(bo);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupUser)), bo.PrimaryKey());
            }
        }

        public static void DeleteByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> userIds, ref TrailObject trail)
        {
            foreach (CedantWorkgroupUserBo bo in GetByCedantWorkgroupIdExcept(cedantWorkgroupId, userIds))
            {
                DataTrail dataTrail = Delete(bo);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupUser)), bo.PrimaryKey());
            }
        }

        public static void DeleteByUserId(int userId, ref TrailObject trail)
        {
            foreach (CedantWorkgroupUserBo bo in GetByUserId(userId))
            {
                DataTrail dataTrail = Delete(bo);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupUser)), bo.PrimaryKey());
            }
        }
    }
}
