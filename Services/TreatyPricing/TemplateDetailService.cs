using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TreatyPricing
{
    public class TemplateDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TemplateDetail)),
            };
        }

        public static TemplateDetailBo FormBo(TemplateDetail entity = null)
        {
            if (entity == null)
                return null;

            UserBo createdBy = UserService.Find(entity.CreatedById);
            return new TemplateDetailBo
            {
                Id = entity.Id,
                TemplateId = entity.TemplateId,
                TemplateBo = TemplateService.Find(entity.TemplateId),
                TemplateVersion = entity.TemplateVersion,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByName = createdBy != null ? createdBy.FullName : "",
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TemplateDetailBo> FormBos(IList<TemplateDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TemplateDetailBo> bos = new List<TemplateDetailBo>() { };
            foreach (TemplateDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TemplateDetail FormEntity(TemplateDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TemplateDetail
            {
                Id = bo.Id,
                TemplateId = bo.TemplateId,
                TemplateVersion = bo.TemplateVersion,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                CreatedAt = bo.CreatedAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TemplateDetail.IsExists(id);
        }

        public static TemplateDetailBo Find(int id)
        {
            return FormBo(TemplateDetail.Find(id));
        }

        public static IList<TemplateDetailBo> GetByTemplateId(int templateId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TemplateDetails.Where(q => q.TemplateId == templateId)
                       .OrderBy(q => q.TemplateVersion).ToList());
            }
        }

        public static TemplateDetailBo GetLatestByTemplateId(int templateId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TemplateDetails.Where(q => q.TemplateId == templateId)
                       .OrderByDescending(q => q.TemplateVersion).FirstOrDefault());
            }
        }

        public static IList<TemplateDetailBo> GetByTemplateIdExcept(int templateId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TemplateDetails.Where(q => q.TemplateId == templateId && !ids.Contains(q.Id)).ToList());
            }
        }

        public static Result Save(TemplateDetailBo bo)
        {
            if (!TemplateDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(TemplateDetailBo bo, ref TrailObject trail)
        {
            if (!TemplateDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(TemplateDetailBo bo)
        {
            TemplateDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(TemplateDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(TemplateDetailBo bo)
        {
            Result result = Result();

            TemplateDetail entity = TemplateDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.TemplateId = bo.TemplateId;
                entity.TemplateVersion = bo.TemplateVersion;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(TemplateDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TemplateDetailBo bo)
        {
            TemplateDetail.Delete(bo.Id);
        }

        public static Result Delete(TemplateDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TemplateDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByTemplateId(int templateId)
        {
            return TemplateDetail.DeleteAllByTemplateId(templateId);
        }

        public static void DeleteAllByTemplateId(int templateId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTemplateId(templateId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TemplateDetail)));
                }
            }
        }

        public static Result DeleteByTemplateIdExcept(int templateId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            var templateDetails = GetByTemplateIdExcept(templateId, saveIds);
            foreach (TemplateDetailBo templateDetail in templateDetails)
            {
                if (File.Exists(templateDetail.GetLocalPath()))
                    File.Delete(templateDetail.GetLocalPath());

                DataTrail dataTrail = TemplateDetail.Delete(templateDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
