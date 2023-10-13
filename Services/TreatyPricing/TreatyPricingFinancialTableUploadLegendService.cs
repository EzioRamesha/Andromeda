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
    public class TreatyPricingFinancialTableUploadLegendService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableUploadLegend)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTableUploadLegend.ToString()
            };
        }

        public static Expression<Func<TreatyPricingFinancialTableUploadLegend, TreatyPricingFinancialTableUploadLegendBo>> Expression()
        {
            return entity => new TreatyPricingFinancialTableUploadLegendBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionDetailId = entity.TreatyPricingFinancialTableVersionDetailId,
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingFinancialTableUploadLegendBo FormBo(TreatyPricingFinancialTableUploadLegend entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingFinancialTableUploadLegendBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableVersionDetailId = entity.TreatyPricingFinancialTableVersionDetailId,
                TreatyPricingFinancialTableVersionDetailBo = TreatyPricingFinancialTableVersionDetailService.Find(entity.TreatyPricingFinancialTableVersionDetailId),
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingFinancialTableUploadLegendBo> FormBos(IList<TreatyPricingFinancialTableUploadLegend> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableUploadLegendBo> bos = new List<TreatyPricingFinancialTableUploadLegendBo>() { };
            foreach (TreatyPricingFinancialTableUploadLegend entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingFinancialTableUploadLegend FormEntity(TreatyPricingFinancialTableUploadLegendBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTableUploadLegend
            {
                Id = bo.Id,
                TreatyPricingFinancialTableVersionDetailId = bo.TreatyPricingFinancialTableVersionDetailId,
                Code = bo.Code,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingFinancialTableUploadLegend.IsExists(id);
        }

        public static TreatyPricingFinancialTableUploadLegendBo Find(int id)
        {
            return FormBo(TreatyPricingFinancialTableUploadLegend.Find(id));
        }

        public static IList<TreatyPricingFinancialTableUploadLegendBo> GetByTreatyPricingFinancialTableVersionDetailId(int treatyPricingFinancialTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableUploadLegends
                    .Where(q => q.TreatyPricingFinancialTableVersionDetailId == treatyPricingFinancialTableVersionDetailId)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableUploadLegendBo bo)
        {
            if (!TreatyPricingFinancialTableUploadLegend.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableUploadLegendBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTableUploadLegend.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableUploadLegendBo bo)
        {
            TreatyPricingFinancialTableUploadLegend entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableUploadLegendBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTableUploadLegend entity = TreatyPricingFinancialTableUploadLegend.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingFinancialTableVersionDetailId = bo.TreatyPricingFinancialTableVersionDetailId;
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableUploadLegendBo bo)
        {
            TreatyPricingFinancialTableUploadLegend.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTableUploadLegend.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableVersionDetailId(int treatyPricingFinancialTableVersionDetailId)
        {
            return TreatyPricingFinancialTableUploadLegend.DeleteAllByTreatyPricingFinancialTableVersionDetailId(treatyPricingFinancialTableVersionDetailId);
        }

        public static void DeleteAllByTreatyPricingFinancialTableVersionDetailId(int treatyPricingFinancialTableVersionDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingFinancialTableVersionDetailId(treatyPricingFinancialTableVersionDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableUploadLegend)));
                }
            }
        }
    }
}
