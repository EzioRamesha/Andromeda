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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthorizationLimitService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AuthorizationLimit)),
                Controller = ModuleBo.ModuleController.AuthorizationLimit.ToString()
            };
        }

        public static Expression<Func<AuthorizationLimit, AuthorizationLimitBo>> Expression()
        {
            return entity => new AuthorizationLimitBo
            {
                Id = entity.Id,
                AccessGroupId = entity.AccessGroupId,
                PositiveAmountFrom = entity.PositiveAmountFrom,
                PositiveAmountTo = entity.PositiveAmountTo,
                NegativeAmountFrom = entity.NegativeAmountFrom,
                NegativeAmountTo = entity.NegativeAmountTo,
                Percentage = entity.Percentage,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static AuthorizationLimitBo FormBo(AuthorizationLimit entity = null)
        {
            if (entity == null)
                return null;
            return new AuthorizationLimitBo
            {
                Id = entity.Id,
                AccessGroupId = entity.AccessGroupId,
                PositiveAmountFrom = entity.PositiveAmountFrom,
                PositiveAmountTo = entity.PositiveAmountTo,
                NegativeAmountFrom = entity.NegativeAmountFrom,
                NegativeAmountTo = entity.NegativeAmountTo,
                Percentage = entity.Percentage,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<AuthorizationLimitBo> FormBos(IList<AuthorizationLimit> entities = null)
        {
            if (entities == null)
                return null;
            IList<AuthorizationLimitBo> bos = new List<AuthorizationLimitBo>() { };
            foreach (AuthorizationLimit entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AuthorizationLimit FormEntity(AuthorizationLimitBo bo = null)
        {
            if (bo == null)
                return null;
            return new AuthorizationLimit
            {
                Id = bo.Id,
                AccessGroupId = bo.AccessGroupId,
                PositiveAmountFrom = bo.PositiveAmountFrom,
                PositiveAmountTo = bo.PositiveAmountTo,
                NegativeAmountFrom = bo.NegativeAmountFrom,
                NegativeAmountTo = bo.NegativeAmountTo,
                Percentage = bo.Percentage,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return AuthorizationLimit.IsExists(id);
        }
        
        public static bool IsDuplicateAccessGroup(AuthorizationLimit authorizationLimit)
        {
            return AuthorizationLimit.IsDuplicateAccessGroup(authorizationLimit);
        }

        public static AuthorizationLimitBo Find(int id)
        {
            return FormBo(AuthorizationLimit.Find(id));
        }

        public static IList<AuthorizationLimitBo> Get()
        {
            return FormBos(AuthorizationLimit.Get());
        }

        public static AuthorizationLimitBo GetByAccessGroupId(int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.AuthorizationLimits.Where(q => q.AccessGroupId == accessGroupId).FirstOrDefault());
            }
        }

        public static int CountByAccessGroupId(int accessGroupId)
        {
            using (var db = new AppDbContext())
            {
                return db.AuthorizationLimits.Where(q => q.AccessGroupId == accessGroupId).Count();
            }
        }

        public static Result Save(ref AuthorizationLimitBo bo)
        {
            if (!AuthorizationLimit.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AuthorizationLimitBo bo, ref TrailObject trail)
        {
            if (!AuthorizationLimit.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AuthorizationLimitBo bo)
        {
            AuthorizationLimit entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateAccessGroup(entity))
            {
                AccessGroup accessGroup = AccessGroup.Find(entity.AccessGroupId);
                result.AddError(string.Format(MessageBag.DuplicateAccessGroup, accessGroup.Name));
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AuthorizationLimitBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AuthorizationLimitBo bo)
        {
            Result result = Result();

            AuthorizationLimit entity = AuthorizationLimit.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateAccessGroup(FormEntity(bo)))
            {
                AccessGroup accessGroup = AccessGroup.Find(entity.AccessGroupId);
                result.AddError(string.Format(MessageBag.DuplicateAccessGroup, accessGroup.Name));
            }

            if (result.Valid)
            {
                entity.AccessGroupId = bo.AccessGroupId;
                entity.PositiveAmountFrom = bo.PositiveAmountFrom;
                entity.PositiveAmountTo = bo.PositiveAmountTo;
                entity.NegativeAmountFrom = bo.NegativeAmountFrom;
                entity.NegativeAmountTo = bo.NegativeAmountTo;
                entity.Percentage = bo.Percentage;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AuthorizationLimitBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Delete(AuthorizationLimitBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = AuthorizationLimit.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
