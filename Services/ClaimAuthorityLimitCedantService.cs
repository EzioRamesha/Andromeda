using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ClaimAuthorityLimitCedantService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimAuthorityLimitCedant)),
                Controller = ModuleBo.ModuleController.ClaimAuthorityLimitCedant.ToString()
            };
        }

        public static ClaimAuthorityLimitCedantBo FormBo(ClaimAuthorityLimitCedant entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimAuthorityLimitCedantBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimAuthorityLimitCedantBo> FormBos(IList<ClaimAuthorityLimitCedant> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimAuthorityLimitCedantBo> bos = new List<ClaimAuthorityLimitCedantBo>() { };
            foreach (ClaimAuthorityLimitCedant entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimAuthorityLimitCedant FormEntity(ClaimAuthorityLimitCedantBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimAuthorityLimitCedant
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                Remarks = bo.Remarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(ClaimAuthorityLimitCedant calCedant)
        {
            using (var db = new AppDbContext())
            {
                if (calCedant.CedantId != 0)
                {
                    var query = db.ClaimAuthorityLimitCedants.Where(q => q.CedantId == calCedant.CedantId);
                    if (calCedant.Id != 0)
                    {
                        query = query.Where(q => q.Id != calCedant.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return ClaimAuthorityLimitCedant.IsExists(id);
        }

        public static ClaimAuthorityLimitCedantBo Find(int id)
        {
            return FormBo(ClaimAuthorityLimitCedant.Find(id));
        }

        public static ClaimAuthorityLimitCedantBo FindByCedant(int cedantId)
        {
            return FormBo(ClaimAuthorityLimitCedant.FindByCedant(cedantId));
        }

        public static Result Save(ref ClaimAuthorityLimitCedantBo bo)
        {
            if (!ClaimAuthorityLimitCedant.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimAuthorityLimitCedantBo bo, ref TrailObject trail)
        {
            if (!ClaimAuthorityLimitCedant.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimAuthorityLimitCedantBo bo)
        {
            ClaimAuthorityLimitCedant entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Cedant", bo.CedantId.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimAuthorityLimitCedantBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimAuthorityLimitCedantBo bo)
        {
            Result result = Result();

            ClaimAuthorityLimitCedant entity = ClaimAuthorityLimitCedant.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Cedant", bo.CedantId.ToString());
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.Remarks = bo.Remarks;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimAuthorityLimitCedantBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimAuthorityLimitCedantBo bo)
        {
            ClaimAuthorityLimitCedantDetail.DeleteAllByClaimAuthorityLimitCedantId(bo.Id);
            ClaimAuthorityLimitCedant.Delete(bo.Id);
        }

        public static Result Delete(ClaimAuthorityLimitCedantBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                ClaimAuthorityLimitCedantDetail.DeleteAllByClaimAuthorityLimitCedantId(bo.Id);

                DataTrail dataTrail = ClaimAuthorityLimitCedant.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
