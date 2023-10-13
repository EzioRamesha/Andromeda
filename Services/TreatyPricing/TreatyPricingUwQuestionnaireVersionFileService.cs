using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireVersionFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersionFile)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwQuestionnaireVersionFile.ToString()
            };
        }

        public static Expression<Func<TreatyPricingUwQuestionnaireVersionFile, TreatyPricingUwQuestionnaireVersionFileBo>> Expression()
        {
            return entity => new TreatyPricingUwQuestionnaireVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingUwQuestionnaireVersionFileBo FormBo(TreatyPricingUwQuestionnaireVersionFile entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingUwQuestionnaireVersionBo = TreatyPricingUwQuestionnaireVersionService.Find(entity.TreatyPricingUwQuestionnaireVersionId),
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingUwQuestionnaireVersionFileBo.GetStatusName(entity.Status),
                CreatedByName = UserService.Find(entity.CreatedById).FullName,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
            };
        }

        public static IList<TreatyPricingUwQuestionnaireVersionFileBo> FormBos(IList<TreatyPricingUwQuestionnaireVersionFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwQuestionnaireVersionFileBo> bos = new List<TreatyPricingUwQuestionnaireVersionFileBo>() { };
            foreach (TreatyPricingUwQuestionnaireVersionFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingUwQuestionnaireVersionFile FormEntity(TreatyPricingUwQuestionnaireVersionFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersionFile
            {
                Id = bo.Id,
                TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Errors = bo.Errors,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwQuestionnaireVersionFile.IsExists(id);
        }

        public static TreatyPricingUwQuestionnaireVersionFileBo Find(int id)
        {
            return FormBo(TreatyPricingUwQuestionnaireVersionFile.Find(id));
        }

        public static TreatyPricingUwQuestionnaireVersionFileBo FindByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingUwQuestionnaireVersionFiles.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingUwQuestionnaireVersionFileBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaireVersionFiles.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingUwQuestionnaireVersionFileBo> GetByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaireVersionFiles
                    .Where(q => q.TreatyPricingUwQuestionnaireVersionId == treatyPricingUwQuestionnaireVersionId)
                    .ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaireVersionFiles.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingUwQuestionnaireVersionFileBo bo)
        {
            if (!TreatyPricingUwQuestionnaireVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwQuestionnaireVersionFileBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwQuestionnaireVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionFileBo bo)
        {
            TreatyPricingUwQuestionnaireVersionFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionFileBo bo)
        {
            Result result = Result();

            TreatyPricingUwQuestionnaireVersionFile entity = TreatyPricingUwQuestionnaireVersionFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId;
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

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwQuestionnaireVersionFileBo bo)
        {
            TreatyPricingUwQuestionnaireVersionFile.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwQuestionnaireVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwQuestionnaireVersionFile.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId)
        {
            return TreatyPricingUwQuestionnaireVersionFile.DeleteAllByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);
        }

        public static void DeleteAllByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersionFile)));
                }
            }
        }
    }
}
