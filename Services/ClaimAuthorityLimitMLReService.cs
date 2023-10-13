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

namespace Services
{
    public class ClaimAuthorityLimitMLReService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimAuthorityLimitMLRe)),
                Controller = ModuleBo.ModuleController.ClaimAuthorityLimitMLRe.ToString()
            };
        }

        public static ClaimAuthorityLimitMLReBo FormBo(ClaimAuthorityLimitMLRe entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimAuthorityLimitMLReBo
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                UserId = entity.UserId,
                Position = entity.Position,
                IsAllowOverwriteApproval = entity.IsAllowOverwriteApproval,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                DepartmentBo = DepartmentService.Find(entity.DepartmentId),
                UserBo = UserService.Find(entity.UserId),
            };
        }

        public static IList<ClaimAuthorityLimitMLReBo> FormBos(IList<ClaimAuthorityLimitMLRe> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimAuthorityLimitMLReBo> bos = new List<ClaimAuthorityLimitMLReBo>() { };
            foreach (ClaimAuthorityLimitMLRe entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimAuthorityLimitMLRe FormEntity(ClaimAuthorityLimitMLReBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimAuthorityLimitMLRe
            {
                Id = bo.Id,
                DepartmentId = bo.DepartmentId,
                UserId = bo.UserId,
                Position = bo.Position,
                IsAllowOverwriteApproval = bo.IsAllowOverwriteApproval,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        //public static bool IsDuplicateCode(ClaimAuthorityLimitMLRe calCedant)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        if (calCedant.CedantId != 0)
        //        {
        //            var query = db.ClaimAuthorityLimitMLRe.Where(q => q.CedantId == calCedant.CedantId);
        //            if (calCedant.Id != 0)
        //            {
        //                query = query.Where(q => q.Id != calCedant.Id);
        //            }
        //            return query.Count() > 0;
        //        }
        //        return false;
        //    }
        //}

        public static bool IsExists(int id)
        {
            return ClaimAuthorityLimitMLRe.IsExists(id);
        }

        public static ClaimAuthorityLimitMLReBo Find(int id)
        {
            return FormBo(ClaimAuthorityLimitMLRe.Find(id));
        }

        public static ClaimAuthorityLimitMLReBo FindByUserId(int userId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ClaimAuthorityLimitMLRe.Where(q => q.UserId == userId).FirstOrDefault());
            }
        }

        public static Result Save(ref ClaimAuthorityLimitMLReBo bo)
        {
            if (!ClaimAuthorityLimitMLRe.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimAuthorityLimitMLReBo bo, ref TrailObject trail)
        {
            if (!ClaimAuthorityLimitMLRe.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimAuthorityLimitMLReBo bo)
        {
            ClaimAuthorityLimitMLRe entity = FormEntity(bo);

            Result result = Result();
            //if (IsDuplicateCode(entity))
            //{
            //    result.AddTakenError("Cedant", bo.CedantId.ToString());
            //}

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimAuthorityLimitMLReBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimAuthorityLimitMLReBo bo)
        {
            Result result = Result();

            ClaimAuthorityLimitMLRe entity = ClaimAuthorityLimitMLRe.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            //if (IsDuplicateCode(FormEntity(bo)))
            //{
            //    result.AddTakenError("Cedant", bo.CedantId.ToString());
            //}

            if (result.Valid)
            {
                entity.DepartmentId = bo.DepartmentId;
                entity.UserId = bo.UserId;
                entity.Position = bo.Position;
                entity.IsAllowOverwriteApproval = bo.IsAllowOverwriteApproval;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimAuthorityLimitMLReBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimAuthorityLimitMLReBo bo)
        {
            ClaimAuthorityLimitMLReDetail.DeleteAllByClaimAuthorityLimitMLReId(bo.Id);
            ClaimAuthorityLimitMLRe.Delete(bo.Id);
        }

        public static Result Delete(ClaimAuthorityLimitMLReBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                ClaimAuthorityLimitMLReDetail.DeleteAllByClaimAuthorityLimitMLReId(bo.Id);

                DataTrail dataTrail = ClaimAuthorityLimitMLRe.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
