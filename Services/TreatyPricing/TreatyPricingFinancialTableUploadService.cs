
using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingFinancialTableUploadService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableUpload)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTableUpload.ToString()
            };
        }

        public static Expression<Func<TreatyPricingFinancialTableUpload, TreatyPricingFinancialTableUploadBo>> Expression()
        {
            return entity => new TreatyPricingFinancialTableUploadBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionDetailId = entity.TreatyPricingFinancialTableVersionDetailId,
                MinimumSumAssured = entity.MinimumSumAssured,
                MaximumSumAssured = entity.MaximumSumAssured,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingFinancialTableUploadBo FormBo(TreatyPricingFinancialTableUpload entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingFinancialTableUploadBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionDetailId = entity.TreatyPricingFinancialTableVersionDetailId,
                TreatyPricingFinancialTableVersionDetailBo = TreatyPricingFinancialTableVersionDetailService.Find(entity.TreatyPricingFinancialTableVersionDetailId),
                MinimumSumAssured = entity.MinimumSumAssured,
                MaximumSumAssured = entity.MaximumSumAssured,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingFinancialTableUploadBo> FormBos(IList<TreatyPricingFinancialTableUpload> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableUploadBo> bos = new List<TreatyPricingFinancialTableUploadBo>() { };
            foreach (TreatyPricingFinancialTableUpload entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingFinancialTableUpload FormEntity(TreatyPricingFinancialTableUploadBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTableUpload
            {
                Id = bo.Id,
                TreatyPricingFinancialTableVersionDetailId = bo.TreatyPricingFinancialTableVersionDetailId,
                MinimumSumAssured = bo.MinimumSumAssured,
                MaximumSumAssured = bo.MaximumSumAssured,
                Code = bo.Code,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingFinancialTableUpload.IsExists(id);
        }

        public static TreatyPricingFinancialTableUploadBo Find(int id)
        {
            return FormBo(TreatyPricingFinancialTableUpload.Find(id));
        }

        public static IList<TreatyPricingFinancialTableUploadBo> GetByTreatyPricingFinancialTableVersionDetailId(int TreatyPricingFinancialTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableUploads
                    .Where(q => q.TreatyPricingFinancialTableVersionDetailId == TreatyPricingFinancialTableVersionDetailId)
                    .ToList());
            }
        }

        public static TreatyPricingFinancialTableUploadBo GetByTreatyPricingFinancialTableVersionDetailIdForComparison(int treatyPricingFinancialTableVersionDetailId, int minimumSumAssured, int maximumSumAssured)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingFinancialTableUploads
                    .Where(q => q.TreatyPricingFinancialTableVersionDetailId == treatyPricingFinancialTableVersionDetailId
                    && ((minimumSumAssured >= q.MinimumSumAssured && minimumSumAssured <= q.MaximumSumAssured) || (maximumSumAssured >= q.MinimumSumAssured && maximumSumAssured <= q.MaximumSumAssured)))
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableUploadBo bo)
        {
            if (!TreatyPricingFinancialTableUpload.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableUploadBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTableUpload.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableUploadBo bo)
        {
            TreatyPricingFinancialTableUpload entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableUploadBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableUploadBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTableUpload entity = TreatyPricingFinancialTableUpload.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingFinancialTableVersionDetailId = bo.TreatyPricingFinancialTableVersionDetailId;
                entity.MinimumSumAssured = bo.MinimumSumAssured;
                entity.MaximumSumAssured = bo.MaximumSumAssured;
                entity.Code = bo.Code;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableUploadBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableUploadBo bo)
        {
            TreatyPricingFinancialTableUpload.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableUploadBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTableUpload.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionDetailId(int TreatyPricingFinancialTableVersionDetailId)
        {
            return TreatyPricingFinancialTableUpload.DeleteAllByTreatyPricingFinancialTableVersionDetailId(TreatyPricingFinancialTableVersionDetailId);
        }

        public static void DeleteAllByTreatyPricingFinancialTableVersionDetailId(int TreatyPricingFinancialTableVersionDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingFinancialTableVersionDetailId(TreatyPricingFinancialTableVersionDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableUpload)));
                }
            }
        }
    }
}
