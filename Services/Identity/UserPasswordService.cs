using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Services.Identity
{
    public class UserPasswordService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(UserPassword)),
                Controller = ModuleBo.ModuleController.User.ToString()
            };
        }

        public static UserPasswordBo FormBo(UserPassword entity = null)
        {
            if (entity == null)
                return null;
            return new UserPasswordBo
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PasswordHash = entity.PasswordHash,
                CreatedById = entity.CreatedById,
            };
        }

        public static IList<UserPasswordBo> FormBos(IList<UserPassword> entities = null)
        {
            if (entities == null)
                return null;
            IList<UserPasswordBo> bos = new List<UserPasswordBo>() { };
            foreach (UserPassword entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static UserPassword FormEntity(UserPasswordBo bo = null)
        {
            if (bo == null)
                return null;
            return new UserPassword
            {
                Id = bo.Id,
                UserId = bo.UserId,
                PasswordHash = bo.PasswordHash,
                CreatedById = bo.CreatedById,
            };
        }

        public static IList<UserPasswordBo> GetByUserId(int userId, int skip = 0)
        {
            return FormBos(UserPassword.GetByUserId(userId, skip));
        }

        public static Result Create(ref UserPasswordBo bo)
        {
            Result result = Result();

            UserPassword entity = FormEntity(bo);

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref UserPasswordBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserPassword)));
            }
            return result;
        }

        public static Result Delete(UserPasswordBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = UserPassword.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByUserId(int userId)
        {
            return UserPassword.DeleteAllByUserId(userId);
        }

        public static void DeleteAllByUserId(int userId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByUserId(userId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(UserPassword)));
                }
            }
        }
    }
}
