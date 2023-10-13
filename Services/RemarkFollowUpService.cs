using BusinessObject;
using DataAccess.Entities;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class RemarkFollowUpService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RemarkFollowUp)),
                Controller = ModuleBo.ModuleController.RemarkFollowUp.ToString()
            };
        }

        public static RemarkFollowUpBo FormBo(RemarkFollowUp entity = null)
        {
            if (entity == null)
                return null;

            return new RemarkFollowUpBo
            {
                Id = entity.Id,
                RemarkId = entity.RemarkId,
                Status = entity.Status,
                StatusName = RemarkFollowUpBo.GetStatusName(entity.Status),
                FollowUpAt = entity.FollowUpAt,
                FollowUpAtStr = entity.FollowUpAt.Value.ToString(Util.GetDateFormat()),
                FollowUpUserId = entity.FollowUpUserId,
                FollowUpUserBo = UserService.Find(entity.FollowUpUserId, true),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById
            };
        }

        public static IList<RemarkFollowUpBo> FormBos(IList<RemarkFollowUp> entities = null)
        {
            if (entities == null)
                return null;
            IList<RemarkFollowUpBo> bos = new List<RemarkFollowUpBo>() { };
            foreach (RemarkFollowUp entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RemarkFollowUp FormEntity(RemarkFollowUpBo bo = null)
        {
            if (bo == null)
                return null;
            return new RemarkFollowUp
            {
                Id = bo.Id,
                RemarkId = bo.RemarkId,
                Status = bo.Status,
                FollowUpAt = bo.FollowUpAt,
                FollowUpUserId = bo.FollowUpUserId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById
            };
        }

        public static RemarkFollowUpBo Find(int id)
        {
            return FormBo(RemarkFollowUp.Find(id));
        }

        public static RemarkFollowUpBo FindByRemarkId(int remarkId)
        {
            return FormBo(RemarkFollowUp.FindByRemarkId(remarkId));
        }

        public static Result Create(ref RemarkFollowUpBo bo)
        {
            RemarkFollowUp entity = FormEntity(bo);
            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RemarkFollowUpBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RemarkFollowUpBo bo)
        {
            Result result = Result();

            RemarkFollowUp entity = RemarkFollowUp.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RemarkId = bo.RemarkId;
                entity.Status = bo.Status;
                entity.FollowUpAt = bo.FollowUpAt;
                entity.FollowUpUserId = bo.FollowUpUserId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RemarkFollowUpBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RemarkFollowUpBo bo)
        {
            RemarkFollowUp.Delete(bo.Id);
        }

        public static Result Delete(RemarkFollowUpBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RemarkFollowUp.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
