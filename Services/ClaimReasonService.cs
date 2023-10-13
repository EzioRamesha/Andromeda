using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
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
    public class ClaimReasonService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimReason)),
                Controller = ModuleBo.ModuleController.ClaimReason.ToString()
            };
        }

        public static Expression<Func<ClaimReason, ClaimReasonBo>> Expression()
        {
            return entity => new ClaimReasonBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Reason = entity.Reason,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimReasonBo FormBo(ClaimReason entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimReasonBo
            {
                Id = entity.Id,
                Type = entity.Type,
                Reason = entity.Reason,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimReasonBo> FormBos(IList<ClaimReason> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimReasonBo> bos = new List<ClaimReasonBo>() { };
            foreach (ClaimReason entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimReason FormEntity(ClaimReasonBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimReason
            {
                Id = bo.Id,
                Type = bo.Type,
                Reason = bo.Reason?.Trim(),
                Remark = bo.Remark?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimReason.IsExists(id);
        }

        public static ClaimReasonBo Find(int id)
        {
            return FormBo(ClaimReason.Find(id));
        }
        public static ClaimReasonBo Find(int? id)
        {
            if (id.HasValue)
                return FormBo(ClaimReason.Find(id.Value));
            return null;
        }

        public static ClaimReasonBo FindByReason(int type, string reason)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimReasons
                    .Where(q => q.Type == type)
                    .Where(q => q.Reason.Trim() == reason.Trim());

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<ClaimReasonBo> Get()
        {
            return FormBos(ClaimReason.Get());
        }

        public static IList<ClaimReasonBo> GetByType(int type)
        {
            return FormBos(ClaimReason.GetByType(type));
        }

        public static Result Save(ref ClaimReasonBo bo)
        {
            if (!ClaimReason.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimReasonBo bo, ref TrailObject trail)
        {
            if (!ClaimReason.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateReason(ClaimReason claimReason)
        {
            return claimReason.IsDuplicateReason();
        }

        public static Result Create(ref ClaimReasonBo bo)
        {
            ClaimReason entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateReason(entity))
            {
                result.AddTakenError("Reason", bo.Reason);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimReasonBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimReasonBo bo)
        {
            Result result = Result();

            ClaimReason entity = ClaimReason.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateReason(FormEntity(bo)))
            {
                result.AddTakenError("Reason", bo.Reason);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Type = bo.Type;
                entity.Reason = bo.Reason;
                entity.Remark = bo.Remark;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimReasonBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimReasonBo bo)
        {
            ClaimReason.Delete(bo.Id);
        }

        public static Result Delete(ClaimReasonBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (ReferralClaim.CountByBenefitCode(bo.Id) > 0 ||
                ClaimRegister.CountByBenefitCode(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed(); 
            }

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = ClaimReason.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
