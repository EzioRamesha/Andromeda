using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
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
    public class DirectRetroStatusFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetroStatusFile)),
                Controller = ModuleBo.ModuleController.DirectRetroStatusFile.ToString(),
            };
        }

        public static DirectRetroStatusFileBo FormBo(DirectRetroStatusFile entity = null)
        {
            if (entity == null)
                return null;
            return new DirectRetroStatusFileBo
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                DirectRetroBo = DirectRetroService.Find(entity.DirectRetroId),
                StatusHistoryId = entity.StatusHistoryId,
                StatusHistoryBo = StatusHistoryService.Find(entity.StatusHistoryId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DirectRetroStatusFileBo> FormBos(IList<DirectRetroStatusFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<DirectRetroStatusFileBo> bos = new List<DirectRetroStatusFileBo>() { };
            foreach (DirectRetroStatusFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static DirectRetroStatusFile FormEntity(DirectRetroStatusFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetroStatusFile
            {
                Id = bo.Id,
                DirectRetroId = bo.DirectRetroId,
                StatusHistoryId = bo.StatusHistoryId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static DirectRetroStatusFileBo Find(int id)
        {
            return FormBo(DirectRetroStatusFile.Find(id));
        }

        public static DirectRetroStatusFileBo FindByDirectRetroIdStatusHistoryId(int directRetroId, int statusHistoryId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.DirectRetroStatusFiles.Where(q => q.DirectRetroId == directRetroId && q.StatusHistoryId == statusHistoryId).FirstOrDefault());
            }
        }

        public static IList<DirectRetroStatusFileBo> GetByDirectRetroId(int directRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.DirectRetroStatusFiles.Where(q => q.DirectRetroId == directRetroId).OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static Result Save(ref DirectRetroStatusFileBo bo)
        {
            if (!DirectRetroStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref DirectRetroStatusFileBo bo, ref TrailObject trail)
        {
            if (!DirectRetroStatusFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DirectRetroStatusFileBo bo)
        {
            DirectRetroStatusFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroStatusFileBo bo)
        {
            Result result = Result();

            DirectRetroStatusFile entity = DirectRetroStatusFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.DirectRetroId = bo.DirectRetroId;
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DirectRetroStatusFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DirectRetroStatusFileBo bo)
        {
            DirectRetroStatusFile.Delete(bo.Id);
        }

        public static IList<DataTrail> DeleteByDirectRetroId(int directRetroId)
        {
            return DirectRetroStatusFile.DeleteByDirectRetroId(directRetroId);
        }

        public static void DeleteByDirectRetroId(int directRetroId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByDirectRetroId(directRetroId);
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
