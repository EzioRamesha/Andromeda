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
    public class RemarkService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Remark)),
                Controller = ModuleBo.ModuleController.Remark.ToString()
            };
        }

        public static RemarkBo FormBo(Remark entity = null)
        {
            if (entity == null)
                return null;

            var createdBy = UserService.Find(entity.CreatedById);
            ModuleBo moduleBo = ModuleService.Find(entity.ModuleId);
            ObjectPermissionBo permissionBo = ObjectPermissionService.Find(ObjectPermissionBo.TypeRemark, entity.Id);
            RemarkFollowUpBo followUpBo = RemarkFollowUpService.FindByRemarkId(entity.Id);

            return new RemarkBo
            {
                Id = entity.Id,
                ModuleId = entity.ModuleId,
                ModuleBo = moduleBo,
                ObjectId = entity.ObjectId,
                StatusHistoryId = entity.StatusHistoryId,
                Status = entity.Status,
                StatusName = StatusHistoryBo.GetStatusName(moduleBo, entity.Status),
                Content = entity.Content,
                ShortContent = entity.Content.Length > 100 ? entity.Content.Substring(0, 97) + "..." : entity.Content,
                Subject = entity.Subject,
                Version = entity.Version,
                IsPrivate = permissionBo != null,
                PermissionName = ObjectPermissionBo.GetPermissionName(permissionBo != null),
                ObjectPermissionBo = permissionBo,
                HasFollowUp = followUpBo != null,
                RemarkFollowUpBo = followUpBo,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString("dd MMM yyyy hh:mm:ss tt"),
                CreatedById = entity.CreatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RemarkBo> FormBos(IList<Remark> entities = null)
        {
            if (entities == null)
                return null;
            IList<RemarkBo> bos = new List<RemarkBo>() { };
            foreach (Remark entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Remark FormEntity(RemarkBo bo = null)
        {
            if (bo == null)
                return null;
            return new Remark
            {
                Id = bo.Id,
                ModuleId = bo.ModuleId,
                ObjectId = bo.ObjectId,
                SubModuleController = bo.SubModuleController,
                SubObjectId = bo.SubObjectId,
                StatusHistoryId = bo.StatusHistoryId,
                Status = bo.Status,
                Content = bo.Content,
                Subject = bo.Subject,
                Version = bo.Version,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Remark.IsExists(id);
        }

        public static RemarkBo Find(int? id)
        {
            if (!id.HasValue)
                return null;
            return FormBo(Remark.Find(id.Value));
        }

        public static RemarkBo FindByStatusHistoryId(int? statusHistoryId)
        {
            if (!statusHistoryId.HasValue)
                return null;

            using (var db = new AppDbContext())
            {
                return FormBo(db.Remarks.Where(q => q.StatusHistoryId == statusHistoryId).FirstOrDefault());
            }
        }

        public static IList<RemarkBo> Get()
        {
            return FormBos(Remark.Get());
        }

        public static IList<RemarkBo> GetByModuleIdObjectId(int moduleId, int objectId, bool checkPrivacy = false, int? departmentId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Remarks.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId && q.SubModuleController == null);
                var bos = FormBos(query.OrderByDescending(q => q.CreatedAt).ToList());

                if (!checkPrivacy)
                    return bos;

                return bos.Where(q => !q.IsPrivate || (q.ObjectPermissionBo != null && q.ObjectPermissionBo.DepartmentId == departmentId)).ToList();
            }
        }

        public static IList<RemarkBo> GetBySubModule(int moduleId, int objectId, string subModuleController, int? subObjectId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Remarks.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId && !q.StatusHistoryId.HasValue);

                if (!string.IsNullOrEmpty(subModuleController))
                {
                    query = query.Where(q => q.SubModuleController == subModuleController);
                    if (subObjectId.HasValue)
                    {
                        query = query.Where(q => q.SubObjectId == subObjectId);
                    }
                }

                return FormBos(query.OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static IList<string> GetRemarkSubjects()
        {
            using (var db = new AppDbContext())
            {
                return db.Remarks.Where(q => !string.IsNullOrEmpty(q.Subject)).GroupBy(q => q.Subject).Select(g => g.Key).ToList();
            }
        }

        public static Result Save(ref RemarkBo bo)
        {
            if (!Remark.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RemarkBo bo, ref TrailObject trail)
        {
            if (!Remark.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RemarkBo bo)
        {
            Remark entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
                bo.CreatedAt = entity.CreatedAt;
            }

            return result;
        }

        public static Result Create(ref RemarkBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RemarkBo bo)
        {
            Result result = Result();

            Remark entity = Remark.Find(bo.Id);
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
                entity.StatusHistoryId = bo.StatusHistoryId;
                entity.Status = bo.Status;
                entity.Content = bo.Content;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RemarkBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RemarkBo bo)
        {
            Remark.Delete(bo.Id);
        }

        public static Result Delete(RemarkBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = Remark.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByModuleIdObjectId(int moduleId, int objectId)
        {
            return Remark.DeleteAllByModuleIdObjectId(moduleId, objectId);
        }

        public static void DeleteAllByModuleIdObjectId(int moduleId, int objectId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByModuleIdObjectId(moduleId, objectId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(Remark)));
                }
            }
        }
    }
}
