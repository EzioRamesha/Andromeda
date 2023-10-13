using BusinessObject.Identity;
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
    public class InvoiceRegisterBatchRemarkDocumentService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchRemarkDocument)),
            };
        }

        public static InvoiceRegisterBatchRemarkDocumentBo FormBo(InvoiceRegisterBatchRemarkDocument entity = null)
        {
            if (entity == null)
                return null;

            UserBo createdBy = UserService.Find(entity.CreatedById);
            return new InvoiceRegisterBatchRemarkDocumentBo
            {
                Id = entity.Id,
                InvoiceRegisterBatchRemarkId = entity.InvoiceRegisterBatchRemarkId,
                InvoiceRegisterBatchRemarkBo = InvoiceRegisterBatchRemarkService.Find(entity.InvoiceRegisterBatchRemarkId),
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
            };
        }

        public static IList<InvoiceRegisterBatchRemarkDocumentBo> FormBos(IList<InvoiceRegisterBatchRemarkDocument> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchRemarkDocumentBo> bos = new List<InvoiceRegisterBatchRemarkDocumentBo>() { };
            foreach (InvoiceRegisterBatchRemarkDocument entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatchRemarkDocument FormEntity(InvoiceRegisterBatchRemarkDocumentBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatchRemarkDocument
            {
                Id = bo.Id,
                InvoiceRegisterBatchRemarkId = bo.InvoiceRegisterBatchRemarkId,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegisterBatchRemarkDocument.IsExists(id);
        }

        public static InvoiceRegisterBatchRemarkDocumentBo Find(int id)
        {
            return FormBo(InvoiceRegisterBatchRemarkDocument.Find(id));
        }

        public static IList<InvoiceRegisterBatchRemarkDocumentBo> GetByInvoiceRegisterBatchRemarkId(int invoiceRegisterBatchRemarkId)
        {
            return FormBos(InvoiceRegisterBatchRemarkDocument.GetByInvoiceRegisterBatchRemarkId(invoiceRegisterBatchRemarkId));
        }

        public static Result Save(ref InvoiceRegisterBatchRemarkDocumentBo bo)
        {
            if (!InvoiceRegisterBatchRemarkDocument.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref InvoiceRegisterBatchRemarkDocumentBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterBatchRemarkDocument.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBatchRemarkDocumentBo bo)
        {
            InvoiceRegisterBatchRemarkDocument entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBatchRemarkDocumentBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchRemarkDocumentBo bo)
        {
            Result result = Result();

            InvoiceRegisterBatchRemarkDocument entity = InvoiceRegisterBatchRemarkDocument.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterBatchRemarkId = bo.InvoiceRegisterBatchRemarkId;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchRemarkDocumentBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result DeleteExcept(int invoiceRegisterBatchRemarkId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<InvoiceRegisterBatchRemarkDocument> documents = InvoiceRegisterBatchRemarkDocument.GetByInvoiceRegisterBatchRemarkIdExcept(invoiceRegisterBatchRemarkId, ids);
            foreach (InvoiceRegisterBatchRemarkDocument document in documents)
            {
                InvoiceRegisterBatchRemarkDocumentBo documentBo = FormBo(document);
                Util.DeleteFiles(documentBo.GetDirectoryPath(), documentBo.HashFileName);
                DataTrail dataTrail = InvoiceRegisterBatchRemarkDocument.Delete(document.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
