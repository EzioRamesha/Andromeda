using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities;
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

namespace Services
{
    public class RetroRegisterBatchFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegisterBatchFile)),
            };
        }

        public static RetroRegisterBatchFileBo FormBo(RetroRegisterBatchFile entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterBatchFileBo
            {
                Id = entity.Id,
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                RetroRegisterBatchBo = RetroRegisterBatchService.Find(entity.RetroRegisterBatchId),
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Status = entity.Status,
                Errors = entity.Errors,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByBo = UserService.Find(entity.CreatedById),
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroRegisterBatchFileBo> FormBos(IList<RetroRegisterBatchFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBatchFileBo> bos = new List<RetroRegisterBatchFileBo>() { };
            foreach (RetroRegisterBatchFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroRegisterBatchFile FormEntity(RetroRegisterBatchFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegisterBatchFile
            {
                Id = bo.Id,
                RetroRegisterBatchId = bo.RetroRegisterBatchId,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Status = bo.Status,
                Errors = bo.Errors,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroRegisterBatchFile.IsExists(id);
        }

        public static RetroRegisterBatchFileBo Find(int id)
        {
            return FormBo(RetroRegisterBatchFile.Find(id));
        }

        public static RetroRegisterBatchFileBo FindByStatus(int status = 0)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroRegisterBatchFiles.Where(q => q.Status == status).FirstOrDefault());
            }        
        }

        public static int CountRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterBatchFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId).Count();
            }
        }

        public static IList<RetroRegisterBatchFileBo> GetByRetroRegisterBatchIdStatus(int retroRegisterBatchId, int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisterBatchFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId && q.Status == status)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToList());
            }
        }

        public static IList<RetroRegisterBatchFileBo> GetByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisterBatchFiles.Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToList());
            }
        }

        public static Result Save(ref RetroRegisterBatchFileBo bo)
        {
            if (!RetroRegisterBatchFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroRegisterBatchFileBo bo, ref TrailObject trail)
        {
            if (!RetroRegisterBatchFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroRegisterBatchFileBo bo)
        {
            RetroRegisterBatchFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroRegisterBatchFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchFileBo bo)
        {
            Result result = Result();

            RetroRegisterBatchFile entity = RetroRegisterBatchFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RetroRegisterBatchId = bo.RetroRegisterBatchId;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroRegisterBatchFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroRegisterBatchFileBo bo)
        {
            RetroRegisterBatchFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            return RetroRegisterBatchFile.DeleteAllByRetroRegisterBatchId(retroRegisterBatchId);
        }

        public static void DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRetroRegisterBatchId(retroRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroRegisterBatchFile)));
                }
            }
        }
    }
}
