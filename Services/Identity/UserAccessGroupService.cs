using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;

namespace Services.Identity
{
    public class UserAccessGroupService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(UserAccessGroup)),
            };
        }

        public static UserAccessGroupBo FormBo(UserAccessGroup entity = null)
        {
            if (entity == null)
                return null;
            return new UserAccessGroupBo
            {
                UserId = entity.UserId,
                AccessGroupId = entity.AccessGroupId,
                AccessGroupBo = AccessGroupService.Find(entity.AccessGroupId),
            };
        }

        public static IList<UserAccessGroupBo> FormBos(IList<UserAccessGroup> entities = null)
        {
            if (entities == null)
                return null;
            IList<UserAccessGroupBo> bos = new List<UserAccessGroupBo>() { };
            foreach (UserAccessGroup entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static UserAccessGroup FormEntity(UserAccessGroupBo bo = null)
        {
            if (bo == null)
                return null;
            return new UserAccessGroup
            {
                UserId = bo.UserId,
                AccessGroupId = bo.AccessGroupId,
            };
        }

        public static UserAccessGroupBo Find(int userId, int accessGroupId)
        {
            return FormBo(UserAccessGroup.Find(userId, accessGroupId));
        }

        public static IList<UserAccessGroupBo> GetByUserId(int userId)
        {
            return FormBos(UserAccessGroup.GetByUserId(userId));
        }

        public static void DeleteAllByUserId(int userId)
        {
            var trail = new TrailObject();
            UserAccessGroup.DeleteAllByUserId(userId, ref trail);
        }

        public static void DeleteAllByUserId(int userId, ref TrailObject trail)
        {
            UserAccessGroup.DeleteAllByUserId(userId, ref trail);
        }
    }
}
