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
    public class TreatyPricingMedicalTableVersionFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersionFile)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableVersionFile.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableVersionFile, TreatyPricingMedicalTableVersionFileBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableVersionFileBo FormBo(TreatyPricingMedicalTableVersionFile entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionId = entity.TreatyPricingMedicalTableVersionId,
                TreatyPricingMedicalTableVersionBo = TreatyPricingMedicalTableVersionService.Find(entity.TreatyPricingMedicalTableVersionId),
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingMedicalTableVersionFileBo.GetStatusName(entity.Status),
                CreatedByName = UserService.Find(entity.CreatedById).FullName,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),

                DistributionTier = entity.GetDistributionTier(entity.DistributionTierPickListDetailId),
            };
        }

        public static IList<TreatyPricingMedicalTableVersionFileBo> FormBos(IList<TreatyPricingMedicalTableVersionFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableVersionFileBo> bos = new List<TreatyPricingMedicalTableVersionFileBo>() { };
            foreach (TreatyPricingMedicalTableVersionFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableVersionFile FormEntity(TreatyPricingMedicalTableVersionFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableVersionFile
            {
                Id = bo.Id,
                TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId,
                DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId,
                Description = bo.Description,
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
            return TreatyPricingMedicalTableVersionFile.IsExists(id);
        }

        public static TreatyPricingMedicalTableVersionFileBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableVersionFile.Find(id));
        }

        public static TreatyPricingMedicalTableVersionFileBo FindByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingMedicalTableVersionFiles.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingMedicalTableVersionFileBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableVersionFiles.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingMedicalTableVersionFileBo> GetByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableVersionFiles
                    .Where(q => q.TreatyPricingMedicalTableVersionId == treatyPricingMedicalTableVersionId)
                    .ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableVersionFiles.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableVersionFileBo bo)
        {
            if (!TreatyPricingMedicalTableVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableVersionFileBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionFileBo bo)
        {
            TreatyPricingMedicalTableVersionFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableVersionFileBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableVersionFile entity = TreatyPricingMedicalTableVersionFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId;
                entity.DistributionTierPickListDetailId = bo.DistributionTierPickListDetailId;
                entity.Description = bo.Description;
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

        public static Result Update(ref TreatyPricingMedicalTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableVersionFileBo bo)
        {
            TreatyPricingMedicalTableVersionFile.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableVersionFile.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId)
        {
            return TreatyPricingMedicalTableVersionFile.DeleteAllByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableVersionId(int treatyPricingMedicalTableVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableVersionId(treatyPricingMedicalTableVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersionFile)));
                }
            }
        }
    }
}
