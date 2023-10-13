using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services.Identity;
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
    public class InvoiceRegisterBatchFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchFile)),
            };
        }

        public static InvoiceRegisterBatchFileBo FormBo(InvoiceRegisterBatchFile entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterBatchFileBo
            {
                Id = entity.Id,
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                InvoiceRegisterBatchBo = InvoiceRegisterBatchService.Find(entity.InvoiceRegisterBatchId),
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Type = entity.Type,
                Status = entity.Status,
                Errors = entity.Errors,
                DataUpdate = entity.DataUpdate,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByBo = UserService.Find(entity.CreatedById),
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<InvoiceRegisterBatchFileBo> FormBos(IList<InvoiceRegisterBatchFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchFileBo> bos = new List<InvoiceRegisterBatchFileBo>() { };
            foreach (InvoiceRegisterBatchFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatchFile FormEntity(InvoiceRegisterBatchFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatchFile
            {
                Id = bo.Id,
                InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Type = bo.Type,
                Status = bo.Status,
                Errors = bo.Errors,
                DataUpdate = bo.DataUpdate,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegisterBatchFile.IsExists(id);
        }

        public static InvoiceRegisterBatchFileBo Find(int id)
        {
            return FormBo(InvoiceRegisterBatchFile.Find(id));
        }

        public static InvoiceRegisterBatchFileBo FindByStatus(int status = 0)
        {
            return FormBo(InvoiceRegisterBatchFile.FindByStatus(status));
        }

        public static int CountBInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            return InvoiceRegisterBatchFile.CountBInvoiceRegisterBatchId(invoiceRegisterBatchId);
        }

        public static IList<InvoiceRegisterBatchFileBo> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId, bool dataUpdate)
        {
            return FormBos(InvoiceRegisterBatchFile.GetByInvoiceRegisterBatchId(invoiceRegisterBatchId, dataUpdate));
        }

        public static IList<InvoiceRegisterBatchFileBo> GetByInvoiceRegisterBatchIdStatus(int invoiceRegisterBatchId, int status, bool dataUpdate)
        {
            return FormBos(InvoiceRegisterBatchFile.GetByInvoiceRegisterBatchIdStatus(invoiceRegisterBatchId, status, dataUpdate));
        }

        public static IList<InvoiceRegisterBatchFileBo> GetAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            return FormBos(InvoiceRegisterBatchFile.GetAllByInvoiceRegisterBatchId(invoiceRegisterBatchId));
        }

        public static Result Save(ref InvoiceRegisterBatchFileBo bo)
        {
            if (!InvoiceRegisterBatchFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref InvoiceRegisterBatchFileBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterBatchFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterBatchFileBo bo)
        {
            InvoiceRegisterBatchFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterBatchFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchFileBo bo)
        {
            Result result = Result();

            InvoiceRegisterBatchFile entity = InvoiceRegisterBatchFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Type = bo.Type;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.DataUpdate = bo.DataUpdate;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterBatchFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterBatchFileBo bo)
        {
            InvoiceRegisterBatchFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteExceptDataUpdateByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [InvoiceRegisterBatchFiles] WHERE [InvoiceRegisterBatchId] = {0} AND [DataUpdate] = {1}", invoiceRegisterBatchId, false);
                db.SaveChanges();

                return trails;
            }
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            return InvoiceRegisterBatchFile.DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchId);
        }

        public static void DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchFile)));
                }
            }
        }
    }
}
