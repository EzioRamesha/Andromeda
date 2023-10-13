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
    public class TreatyPricingFinancialTableVersionFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersionFile)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTableVersionFile.ToString()
            };
        }

        public static Expression<Func<TreatyPricingFinancialTableVersionFile, TreatyPricingFinancialTableVersionFileBo>> Expression()
        {
            return entity => new TreatyPricingFinancialTableVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
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

        public static TreatyPricingFinancialTableVersionFileBo FormBo(TreatyPricingFinancialTableVersionFile entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingFinancialTableVersionFileBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionId = entity.TreatyPricingFinancialTableVersionId,
                TreatyPricingFinancialTableVersionBo = TreatyPricingFinancialTableVersionService.Find(entity.TreatyPricingFinancialTableVersionId),
                DistributionTierPickListDetailId = entity.DistributionTierPickListDetailId,
                Description = entity.Description,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingFinancialTableVersionFileBo.GetStatusName(entity.Status),
                CreatedByName = UserService.Find(entity.CreatedById).FullName,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),

                DistributionTier = entity.GetDistributionTier(entity.DistributionTierPickListDetailId),
            };
        }

        public static IList<TreatyPricingFinancialTableVersionFileBo> FormBos(IList<TreatyPricingFinancialTableVersionFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableVersionFileBo> bos = new List<TreatyPricingFinancialTableVersionFileBo>() { };
            foreach (TreatyPricingFinancialTableVersionFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingFinancialTableVersionFile FormEntity(TreatyPricingFinancialTableVersionFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTableVersionFile
            {
                Id = bo.Id,
                TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId,
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
            return TreatyPricingFinancialTableVersionFile.IsExists(id);
        }

        public static TreatyPricingFinancialTableVersionFileBo Find(int id)
        {
            return FormBo(TreatyPricingFinancialTableVersionFile.Find(id));
        }

        public static TreatyPricingFinancialTableVersionFileBo FindByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingFinancialTableVersionFiles.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingFinancialTableVersionFileBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableVersionFiles.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingFinancialTableVersionFileBo> GetByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableVersionFiles
                    .Where(q => q.TreatyPricingFinancialTableVersionId == treatyPricingFinancialTableVersionId)
                    .ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableVersionFiles.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableVersionFileBo bo)
        {
            if (!TreatyPricingFinancialTableVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableVersionFileBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTableVersionFile.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionFileBo bo)
        {
            TreatyPricingFinancialTableVersionFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableVersionFileBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTableVersionFile entity = TreatyPricingFinancialTableVersionFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId;
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

        public static Result Update(ref TreatyPricingFinancialTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableVersionFileBo bo)
        {
            TreatyPricingFinancialTableVersionFile.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableVersionFileBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTableVersionFile.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId)
        {
            return TreatyPricingFinancialTableVersionFile.DeleteAllByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);
        }

        public static void DeleteAllByTreatyPricingFinancialTableVersionId(int treatyPricingFinancialTableVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingFinancialTableVersionId(treatyPricingFinancialTableVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersionFile)));
                }
            }
        }
    }
}
