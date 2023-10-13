using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterBatchRemarkService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchRemark)),
                Controller = ModuleBo.ModuleController.InvoiceRegisterBatchRemark.ToString()
            };
        }

        public static InvoiceRegisterBatchRemarkBo FormBo(InvoiceRegisterBatchRemark entity = null)
        {
            if (entity == null)
                return null;

            var createdBy = UserService.Find(entity.CreatedById);
            return new InvoiceRegisterBatchRemarkBo
            {
                Id = entity.Id,
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                InvoiceRegisterBatchBo = InvoiceRegisterBatchService.Find(entity.InvoiceRegisterBatchId),
                Status = entity.Status,
                StatusName = InvoiceRegisterBatchBo.GetStatusName(entity.Status),
                RemarkPermission = entity.RemarkPermission,
                Content = entity.Content,
                FilePermission = entity.FilePermission,
                FollowUp = entity.FollowUp,
                FollowUpStatus = entity.FollowUpStatus,
                FollowUpDate = entity.FollowUpDate,
                FollowUpDateStr = entity.FollowUpDate != null ? entity.FollowUpDate.Value.ToString(Util.GetDateTimeFormat()) : "",
                FollowUpUserId = entity.FollowUpUserId,
                FollowUpUserBo = UserService.Find(entity.FollowUpUserId),
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<InvoiceRegisterBatchRemarkBo> FormBos(IList<InvoiceRegisterBatchRemark> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchRemarkBo> bos = new List<InvoiceRegisterBatchRemarkBo>() { };
            foreach (InvoiceRegisterBatchRemark entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatchRemark FormEntity(InvoiceRegisterBatchRemarkBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatchRemark
            {
                Id = bo.Id,
                InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId,
                Status = bo.Status,
                RemarkPermission = bo.RemarkPermission,
                Content = bo.Content,
                FilePermission = bo.FilePermission,
                FollowUp = bo.FollowUp,
                FollowUpStatus = bo.FollowUpStatus,
                FollowUpDate = bo.FollowUpDate,
                FollowUpUserId = bo.FollowUpUserId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegisterBatchRemark.IsExists(id);
        }

        public static InvoiceRegisterBatchRemarkBo Find(int id)
        {
            return FormBo(InvoiceRegisterBatchRemark.Find(id));
        }

        public static IList<InvoiceRegisterBatchRemarkBo> Get()
        {
            return FormBos(InvoiceRegisterBatchRemark.Get());
        }

        public static IList<InvoiceRegisterBatchRemarkBo> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            return FormBos(InvoiceRegisterBatchRemark.GetByInvoiceRegisterBatchId(invoiceRegisterBatchId));
        }

        public static Result Save(ref InvoiceRegisterBatchRemarkBo bo)
        {
            if (!InvoiceRegisterBatchRemark.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref InvoiceRegisterBatchRemarkBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterBatchRemark.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBatchRemarkBo bo)
        {
            InvoiceRegisterBatchRemark entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBatchRemarkBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchRemarkBo bo)
        {
            Result result = Result();

            InvoiceRegisterBatchRemark entity = InvoiceRegisterBatchRemark.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId;
                entity.Status = bo.Status;
                entity.RemarkPermission = bo.RemarkPermission;
                entity.Content = bo.Content;
                entity.FilePermission = bo.FilePermission;
                entity.FollowUp = bo.FollowUp;
                entity.FollowUpStatus = bo.FollowUpStatus;
                entity.FollowUpDate = bo.FollowUpDate;
                entity.FollowUpUserId = bo.FollowUpUserId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchRemarkBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterBatchRemarkBo bo)
        {
            InvoiceRegisterBatchRemark.Delete(bo.Id);
        }

        public static Result Delete(InvoiceRegisterBatchRemarkBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = InvoiceRegisterBatchRemark.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            return InvoiceRegisterBatchRemark.DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchId);
        }

        public static void DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchRemark)));
                }
            }
        }
    }
}
