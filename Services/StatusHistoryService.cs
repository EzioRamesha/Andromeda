using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class StatusHistoryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StatusHistory)),
                Controller = ModuleBo.ModuleController.StatusHistory.ToString(),
            };
        }

        public static StatusHistoryBo FormBo(StatusHistory entity = null)
        {
            if (entity == null)
                return null;

            var createdBy = UserService.Find(entity.CreatedById);
            var updatedBy = UserService.Find(entity.UpdatedById);

            ModuleBo moduleBo = ModuleService.Find(entity.ModuleId);
            return new StatusHistoryBo
            {
                Id = entity.Id,
                ModuleId = entity.ModuleId,
                ModuleBo = moduleBo,
                ObjectId = entity.ObjectId,
                SubModuleController = entity.SubModuleController,
                SubObjectId = entity.SubObjectId,
                Version = entity.Version,
                Status = entity.Status,
                StatusName = StatusHistoryBo.GetStatusName(moduleBo, entity.Status),
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                UpdatedAtStr = entity.UpdatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
                UpdatedByName = updatedBy != null ? updatedBy.FullName : "",
                UpdatedById = entity.UpdatedById,
                RemarkBo = RemarkService.FindByStatusHistoryId(entity.Id),
            };
        }

        public static IList<StatusHistoryBo> FormBos(IList<StatusHistory> entities = null)
        {
            if (entities == null)
                return null;
            IList<StatusHistoryBo> bos = new List<StatusHistoryBo>() { };
            foreach (StatusHistory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static StatusHistory FormEntity(StatusHistoryBo bo = null)
        {
            if (bo == null)
                return null;
            return new StatusHistory
            {
                Id = bo.Id,
                ModuleId = bo.ModuleId,
                ObjectId = bo.ObjectId,
                SubModuleController = bo.SubModuleController,
                SubObjectId = bo.SubObjectId,
                Version = bo.Version,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return StatusHistory.IsExists(id);
        }

        public static StatusHistoryBo Find(int id)
        {
            return FormBo(StatusHistory.Find(id));
        }

        public static StatusHistoryBo FindByModuleIdObjectIdStatus(int moduleId, int objectId, int status)
        {
            return FormBo(StatusHistory.FindByModuleIdObjectIdStatus(moduleId, objectId, status));
        }
        
        public static StatusHistoryBo FindLatestByModuleIdObjectId(int moduleId, int objectId)
        {
            return FormBo(StatusHistory.FindLatestByModuleIdObjectId(moduleId, objectId));
        }

        public static IList<StatusHistoryBo> GetStatusHistoriesByModuleIdObjectId(int moduleId, int objectId)
        {
            return FormBos(StatusHistory.GetStatusHistoriesByModuleIdObjectId(moduleId, objectId));
        }

        public static IList<StatusHistoryBo> GetBySubModule(int moduleId, int objectId, string subModuleController, int? subObjectId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.StatusHistories.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId);

                if (!string.IsNullOrEmpty(subModuleController))
                {
                    query = query.Where(q => q.SubModuleController == subModuleController);
                    if (subObjectId.HasValue)
                    {
                        query = query.Where(q => q.SubObjectId == subObjectId);
                    }
                }
                else
                {
                    query = query.Where(q => string.IsNullOrEmpty(q.SubModuleController));
                }

                return FormBos(query.OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static Result Save(ref StatusHistoryBo bo)
        {
            if (!StatusHistory.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref StatusHistoryBo bo, ref TrailObject trail)
        {
            if (!StatusHistory.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref StatusHistoryBo bo)
        {
            StatusHistory entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
                bo.CreatedAt = entity.CreatedAt;
            }

            return result;
        }

        public static Result Create(ref StatusHistoryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref StatusHistoryBo bo)
        {
            Result result = Result();

            StatusHistory entity = StatusHistory.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ModuleId = bo.ModuleId;
                entity.ObjectId = bo.ObjectId;
                entity.SubModuleController = bo.SubModuleController;
                entity.SubObjectId = bo.SubObjectId;
                entity.Version = bo.Version;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref StatusHistoryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(StatusHistoryBo bo)
        {
            StatusHistory.Delete(bo.Id);
        }

        public static Result Delete(StatusHistoryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = StatusHistory.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByModuleIdObjectId(int moduleId, int objectId)
        {
            return StatusHistory.DeleteAllByModuleIdObjectId(moduleId, objectId);
        }

        public static void DeleteAllByModuleIdObjectId(int moduleId, int objectId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByModuleIdObjectId(moduleId, objectId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(StatusHistory)));
                }
            }
        }
    }
}
