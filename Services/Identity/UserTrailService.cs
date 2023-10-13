using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Services.Identity
{
    public class UserTrailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(UserTrail))
            };
        }

        public static UserTrailBo FormBo(UserTrail entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            var bo = new UserTrailBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Controller = entity.Controller,
                ObjectId = entity.ObjectId,
                Description = entity.Description,
                IpAddress = entity.IpAddress,
                Data = entity.Data,
                CreatedById = entity.CreatedById,
            };

            if (foreign)
            {
                bo.CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat());
                bo.CreatedByBo = UserService.Find(entity.CreatedById, true);
            }

            return bo;
        }

        public static IList<UserTrailBo> FormBos(IList<UserTrail> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<UserTrailBo> bos = new List<UserTrailBo>() { };
            foreach (UserTrail entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static UserTrail FormEntity(UserTrailBo bo = null)
        {
            if (bo == null)
                return null;
            return new UserTrail
            {
                Id = bo.Id,
                Type = bo.Type,
                Controller = bo.Controller,
                ObjectId = bo.ObjectId,
                Description = bo.Description,
                IpAddress = bo.IpAddress,
                Data = bo.Data,
                CreatedById = bo.CreatedById,
            };
        }

        public static UserTrailBo Find(int id)
        {
            return FormBo(UserTrail.Find(id));
        }

        public static IList<UserTrailBo> GetByControllerObject(string controller, int objectId, bool latestFirst = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.UserTrails.Where(q => q.Controller == controller && q.ObjectId == objectId);

                if (latestFirst)
                {
                    query = query.OrderByDescending(q => q.CreatedAt);
                }

                return FormBos(query.ToList(), true);
            }
        }

        public static void Create(ref UserTrailBo bo)
        {
            UserTrail entity = FormEntity(bo);
            entity.Create();
            bo.Id = entity.Id;
            bo.CreatedAt = entity.CreatedAt;
        }

        public static void Create(ref UserTrailBo bo, AppDbContext db, bool save = true)
        {
            UserTrail entity = FormEntity(bo);
            entity.Create(db, save);
            bo.Id = entity.Id;
        }

        public static void CreateLoginTrail(User user, string ipAddress)
        {
            CreateUserActionTrail(user, UserTrailBo.TypeLogin, ipAddress);
        }

        public static void CreateLogoutTrail(User user, string ipAddress)
        {
            CreateUserActionTrail(user, UserTrailBo.TypeLogout, ipAddress);
        }

        public static void CreateUserActionTrail(User user, int type, string ipAddress)
        {
            Result result = UserService.Result();

            TrailObject trail = new TrailObject();
            DataTrail dataTrail = new DataTrail(user, trail: true, ignoreFields: User.IgnoreFields());
            dataTrail.Merge(ref trail, result.Table);

            UserTrail entity = new UserTrail
            {
                Type = type,
                Controller = result.Controller,
                ObjectId = user.Id,
                Description = string.Format("{0} - {1}", result.Table, UserTrailBo.GetTypeName(type)),
                IpAddress = ipAddress,
                Data = trail.ToString(),
                CreatedById = user.Id,
            };
            entity.Create();
        }
    }
}
