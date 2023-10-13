using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
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
    public class DocumentService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Document)),
                Controller = "Document",
            };
        }

        public static DocumentBo FormBo(Document entity = null)
        {
            if (entity == null)
                return null;

            UserBo createdBy = UserService.Find(entity.CreatedById);
            ObjectPermissionBo permissionBo = ObjectPermissionService.Find(ObjectPermissionBo.TypeDocument, entity.Id);
            return new DocumentBo
            {
                Id = entity.Id,
                ModuleId = entity.ModuleId,
                ModuleBo = ModuleService.Find(entity.ModuleId),
                ObjectId = entity.ObjectId,
                RemarkId = entity.RemarkId,
                Type = entity.Type,
                TypeName = DocumentBo.GetTypeName(entity.Type),
                IsPrivate = permissionBo != null,
                PermissionName = ObjectPermissionBo.GetPermissionName(permissionBo != null),
                ObjectPermissionBo = permissionBo,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString("dd MMM yyyy hh:mm:ss tt"),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
            };
        }

        public static IList<DocumentBo> FormBos(IList<Document> entities = null)
        {
            if (entities == null)
                return null;
            IList<DocumentBo> bos = new List<DocumentBo>() { };
            foreach (Document entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Document FormEntity(DocumentBo bo = null)
        {
            if (bo == null)
                return null;
            return new Document
            {
                Id = bo.Id,
                ModuleId = bo.ModuleId,
                ObjectId = bo.ObjectId,
                SubModuleController = bo.SubModuleController,
                SubObjectId = bo.SubObjectId,
                RemarkId = bo.RemarkId,
                Type = bo.Type,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Document.IsExists(id);
        }

        public static DocumentBo Find(int id)
        {
            return FormBo(Document.Find(id));
        }

        public static IList<DocumentBo> GetByModuleIdObjectId(int moduleId, int objectId, bool checkPrivacy = false, int? departmentId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Documents.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId && q.SubModuleController == null);
                var bos = FormBos(query.ToList());

                if (!checkPrivacy)
                    return bos;

                return bos.Where(q => !q.IsPrivate || (q.ObjectPermissionBo != null && q.ObjectPermissionBo.DepartmentId == departmentId)).ToList();
            }
        }

        public static IList<DocumentBo> GetBySubModule(int moduleId, int objectId, string subModuleController = null, int? subObjectId = null, bool excludeRemark = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Documents.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId);

                if (!string.IsNullOrEmpty(subModuleController))
                {
                    query = query.Where(q => q.SubModuleController == subModuleController);
                    if (subObjectId.HasValue)
                    {
                        query = query.Where(q => q.SubObjectId == subObjectId);
                    }
                }

                if (excludeRemark)
                {
                    query = query.Where(q => !q.RemarkId.HasValue);
                }

                return FormBos(query.OrderByDescending(q => q.CreatedAt).ToList());
            }
        }

        public static DocumentBo GetLatestByModuleIdObjectId(int moduleId, int objectId, string subModuleController = null, int? subObjectId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Documents.Where(q => q.ModuleId == moduleId && q.ObjectId == objectId && q.SubModuleController == subModuleController && q.SubObjectId == subObjectId)
                    .OrderByDescending(q => q.Id).FirstOrDefault();
                var bo = FormBo(query);

                return bo;
            }
        }

        public static Result Save(ref DocumentBo bo)
        {
            if (!Document.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref DocumentBo bo, ref TrailObject trail)
        {
            if (!Document.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DocumentBo bo)
        {
            Document entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
                bo.CreatedAt = entity.CreatedAt;
            }

            return result;
        }

        public static Result Create(ref DocumentBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DocumentBo bo)
        {
            Result result = Result();

            Document entity = Document.Find(bo.Id);
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
                entity.RemarkId = bo.RemarkId;
                entity.Type = bo.Type;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DocumentBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DocumentBo bo)
        {
            Document.Delete(bo.Id);
        }

        public static Result Delete(DocumentBo bo, ref TrailObject trail)
        {
            Result result = Result();

            Util.DeleteFiles(bo.GetDirectoryPath(), bo.HashFileName);
            DataTrail dataTrail = Document.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteExcept(int moduleId, int objectId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<Document> documents = Document.GetByModuleIdObjectIdExcept(moduleId, objectId, ids);
            foreach (Document document in documents)
            {
                DocumentBo documentBo = FormBo(document);
                Util.DeleteFiles(documentBo.GetDirectoryPath(), documentBo.HashFileName);
                DataTrail dataTrail = Document.Delete(document.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
