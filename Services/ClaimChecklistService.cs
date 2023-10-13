using BusinessObject;
using DataAccess.Entities;
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
    public class ClaimChecklistService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimChecklist)),
                Controller = ModuleBo.ModuleController.ClaimChecklist.ToString()
            };
        }

        public static Expression<Func<ClaimChecklist, ClaimChecklistBo>> Expression()
        {
            return entity => new ClaimChecklistBo
            {
                Id = entity.Id,
                ClaimCodeId = entity.ClaimCodeId,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimChecklistBo FormBo(ClaimChecklist entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimChecklistBo
            {
                Id = entity.Id,
                ClaimCodeId = entity.ClaimCodeId,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimChecklistBo> FormBos(IList<ClaimChecklist> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimChecklistBo> bos = new List<ClaimChecklistBo>() { };
            foreach (ClaimChecklist entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimChecklist FormEntity(ClaimChecklistBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimChecklist
            {
                Id = bo.Id,
                ClaimCodeId = bo.ClaimCodeId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimChecklist.IsExists(id);
        }

        public static ClaimChecklistBo Find(int id)
        {
            return FormBo(ClaimChecklist.Find(id));
        }

        public static IList<ClaimChecklistBo> Get()
        {
            return FormBos(ClaimChecklist.Get());
        }

        public static IList<ClaimChecklistBo> GetByClaimCodeId(int claimCodeId)
        {
            return FormBos(ClaimChecklist.GetByClaimCodeId(claimCodeId));
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            return ClaimChecklist.CountByClaimCodeId(claimCodeId);
        }

        public static Result Save(ref ClaimChecklistBo bo)
        {
            if (!ClaimChecklist.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimChecklistBo bo, ref TrailObject trail)
        {
            if (!ClaimChecklist.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateClaimCode(ClaimChecklist claimChecklist)
        {
            return claimChecklist.IsDuplicateClaimCode();
        }

        public static Result Create(ref ClaimChecklistBo bo)
        {
            ClaimChecklist entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateClaimCode(entity))
            {
                ClaimCodeBo claimCodeBo = ClaimCodeService.Find(entity.ClaimCodeId);
                result.AddTakenError("Claim Code", claimCodeBo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimChecklistBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimChecklistBo bo)
        {
            Result result = Result();

            ClaimChecklist entity = ClaimChecklist.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateClaimCode(FormEntity(bo)))
            {
                ClaimCodeBo claimCodeBo = ClaimCodeService.Find(bo.ClaimCodeId);
                result.AddTakenError("Claim Code", claimCodeBo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.ClaimCodeId = bo.ClaimCodeId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimChecklistBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimChecklistBo bo)
        {
            ClaimChecklistDetailService.DeleteByClaimChecklistId(bo.Id);
            ClaimChecklist.Delete(bo.Id);
        }

        public static Result Delete(ClaimChecklistBo bo, ref TrailObject trail)
        {
            Result result = Result();

            ClaimChecklistDetailService.DeleteByClaimChecklistId(bo.Id, ref trail);
            DataTrail dataTrail = ClaimChecklist.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
