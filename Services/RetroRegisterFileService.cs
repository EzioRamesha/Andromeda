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
    public class RetroRegisterFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegisterFile)),
            };
        }

        public static RetroRegisterFileBo FormBo(RetroRegisterFile entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterFileBo
            {
                Id = entity.Id,
                RetroRegisterId = entity.RetroRegisterId,
                RetroRegisterBo = RetroRegisterService.Find(entity.RetroRegisterId),
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

        public static IList<RetroRegisterFileBo> FormBos(IList<RetroRegisterFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterFileBo> bos = new List<RetroRegisterFileBo>() { };
            foreach (RetroRegisterFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroRegisterFile FormEntity(RetroRegisterFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegisterFile
            {
                Id = bo.Id,
                RetroRegisterId = bo.RetroRegisterId,
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
            return RetroRegisterFile.IsExists(id);
        }

        public static RetroRegisterFileBo Find(int id)
        {
            return FormBo(RetroRegisterFile.Find(id));
        }

        public static RetroRegisterFileBo FindByStatus(int status = 0)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroRegisterFiles.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static int CountRetroRegisterId(int retroRegisterId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroRegisterFiles.Where(q => q.RetroRegisterId == retroRegisterId).Count();
            }
        }

        public static IList<RetroRegisterFileBo> GetByRetroRegisterIdStatus(int retroRegisterId, int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisterFiles.Where(q => q.RetroRegisterId == retroRegisterId && q.Status == status)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToList());
            }
        }

        public static IList<RetroRegisterFileBo> GetByRetroRegisterId(int retroRegisterId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroRegisterFiles.Where(q => q.RetroRegisterId == retroRegisterId)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToList());
            }
        }

        public static Result Save(ref RetroRegisterFileBo bo)
        {
            if (!RetroRegisterFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroRegisterFileBo bo, ref TrailObject trail)
        {
            if (!RetroRegisterFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroRegisterFileBo bo)
        {
            RetroRegisterFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroRegisterFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroRegisterFileBo bo)
        {
            Result result = Result();

            RetroRegisterFile entity = RetroRegisterFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RetroRegisterId = bo.RetroRegisterId;
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

        public static Result Update(ref RetroRegisterFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroRegisterFileBo bo)
        {
            RetroRegisterFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterId(int retroRegisterId)
        {
            return RetroRegisterFile.DeleteAllByRetroRegisterId(retroRegisterId);
        }

        public static void DeleteAllByRetroRegisterId(int retroRegisterId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRetroRegisterId(retroRegisterId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroRegisterFile)));
                }
            }
        }
    }
}
