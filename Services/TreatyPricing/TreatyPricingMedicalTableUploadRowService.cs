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
    public class TreatyPricingMedicalTableUploadRowService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadRow)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableUploadRow.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableUploadRow, TreatyPricingMedicalTableUploadRowBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableUploadRowBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                MinimumSumAssured = entity.MinimumSumAssured,
                MaximumSumAssured = entity.MaximumSumAssured,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableUploadRowBo FormBo(TreatyPricingMedicalTableUploadRow entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableUploadRowBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                TreatyPricingMedicalTableVersionDetailBo = TreatyPricingMedicalTableVersionDetailService.Find(entity.TreatyPricingMedicalTableVersionDetailId),
                MinimumSumAssured = entity.MinimumSumAssured,
                MaximumSumAssured = entity.MaximumSumAssured,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingMedicalTableUploadRowBo> FormBos(IList<TreatyPricingMedicalTableUploadRow> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableUploadRowBo> bos = new List<TreatyPricingMedicalTableUploadRowBo>() { };
            foreach (TreatyPricingMedicalTableUploadRow entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableUploadRow FormEntity(TreatyPricingMedicalTableUploadRowBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableUploadRow
            {
                Id = bo.Id,
                TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId,
                MinimumSumAssured = bo.MinimumSumAssured,
                MaximumSumAssured = bo.MaximumSumAssured,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTableUploadRow.IsExists(id);
        }

        public static TreatyPricingMedicalTableUploadRowBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableUploadRow.Find(id));
        }

        public static IList<TreatyPricingMedicalTableUploadRowBo> GetByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableUploadRows
                    .Where(q => q.TreatyPricingMedicalTableVersionDetailId == treatyPricingMedicalTableVersionDetailId)
                    .ToList());
            }
        }

        public static TreatyPricingMedicalTableUploadRowBo GetByTreatyPricingMedicalTableVersionDetailIdForComparison(int treatyPricingMedicalTableVersionDetailId, int minimumSumAssured, int maximumSumAssured)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingMedicalTableUploadRows
                    .Where(q => q.TreatyPricingMedicalTableVersionDetailId == treatyPricingMedicalTableVersionDetailId
                    && ((minimumSumAssured >= q.MinimumSumAssured && minimumSumAssured <= q.MaximumSumAssured) || (maximumSumAssured >= q.MinimumSumAssured && maximumSumAssured <= q.MaximumSumAssured)))
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableUploadRowBo bo)
        {
            if (!TreatyPricingMedicalTableUploadRow.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableUploadRowBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableUploadRow.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadRowBo bo)
        {
            TreatyPricingMedicalTableUploadRow entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadRowBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadRowBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableUploadRow entity = TreatyPricingMedicalTableUploadRow.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId;
                entity.MinimumSumAssured = bo.MinimumSumAssured;
                entity.MaximumSumAssured = bo.MaximumSumAssured;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadRowBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableUploadRowBo bo)
        {
            TreatyPricingMedicalTableUploadRow.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableUploadRowBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableUploadRow.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            return TreatyPricingMedicalTableUploadRow.DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadRow)));
                }
            }
        }
    }
}
