using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterBatchStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchStatusFile)),
                Controller = ModuleBo.ModuleController.InvoiceRegisterBatchStatusFile.ToString(),
            };
        }

        public static InvoiceRegisterBatchStatusFileBo FormBo(InvoiceRegisterBatchStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterBatchStatusFileBo
            {
                Id = entity.Id,
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                InvoiceRegisterBatchBo = InvoiceRegisterBatchService.Find(entity.InvoiceRegisterBatchId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<InvoiceRegisterBatchStatusFileBo> FormBos(IList<InvoiceRegisterBatchStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchStatusFileBo> bos = new List<InvoiceRegisterBatchStatusFileBo>() { };
            foreach (InvoiceRegisterBatchStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatchStatusFile FormEntity(InvoiceRegisterBatchStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatchStatusFile
            {
                Id = bo.Id,
                InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static InvoiceRegisterBatchStatusFileBo Find(int id)
        {
            return FormBo(InvoiceRegisterBatchStatusFile.Find(id));
        }

        public static InvoiceRegisterBatchStatusFileBo FindByInvoiceRegisterBatchIdStatusHistoryId(int InvoiceRegisterBatchId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.InvoiceRegisterBatchStatusFiles.Where(q => q.InvoiceRegisterBatchId == InvoiceRegisterBatchId && q.StatusHistoryId == statusHistoryId).FirstOrDefault());
            }
        }

        public static IList<InvoiceRegisterBatchStatusFileBo> GetByInvoiceRegisterBatchId(int InvoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.InvoiceRegisterBatchStatusFiles.Where(q => q.InvoiceRegisterBatchId == InvoiceRegisterBatchId).OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static Result Save(ref InvoiceRegisterBatchStatusFileBo bo)
        {
            if (!InvoiceRegisterBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref InvoiceRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterBatchStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBatchStatusFileBo bo)
        {
            InvoiceRegisterBatchStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchStatusFileBo bo)
        {
            Result result = Result();

            InvoiceRegisterBatchStatusFile entity = InvoiceRegisterBatchStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterBatchStatusFileBo bo)
        {
            InvoiceRegisterBatchStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteByInvoiceRegisterBatchId(int InvoiceRegisterBatchId)
        {
            return InvoiceRegisterBatchStatusFile.DeleteAllByInvoiceRegisterBatchId(InvoiceRegisterBatchId);
        }

        public static void DeleteByInvoiceRegisterBatchId(int InvoiceRegisterBatchId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByInvoiceRegisterBatchId(InvoiceRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
