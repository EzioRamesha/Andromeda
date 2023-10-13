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
    public class TreatyPricingMedicalTableUploadLegendService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadLegend)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableUploadLegend.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableUploadLegend, TreatyPricingMedicalTableUploadLegendBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableUploadLegendBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableUploadLegendBo FormBo(TreatyPricingMedicalTableUploadLegend entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableUploadLegendBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                TreatyPricingMedicalTableVersionDetailBo = TreatyPricingMedicalTableVersionDetailService.Find(entity.TreatyPricingMedicalTableVersionDetailId),
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingMedicalTableUploadLegendBo> FormBos(IList<TreatyPricingMedicalTableUploadLegend> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableUploadLegendBo> bos = new List<TreatyPricingMedicalTableUploadLegendBo>() { };
            foreach (TreatyPricingMedicalTableUploadLegend entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableUploadLegend FormEntity(TreatyPricingMedicalTableUploadLegendBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableUploadLegend
            {
                Id = bo.Id,
                TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId,
                Code = bo.Code,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTableUploadLegend.IsExists(id);
        }

        public static TreatyPricingMedicalTableUploadLegendBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableUploadLegend.Find(id));
        }

        public static IList<TreatyPricingMedicalTableUploadLegendBo> GetByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableUploadLegends
                    .Where(q => q.TreatyPricingMedicalTableVersionDetailId == treatyPricingMedicalTableVersionDetailId)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableUploadLegendBo bo)
        {
            if (!TreatyPricingMedicalTableUploadLegend.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableUploadLegendBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableUploadLegend.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadLegendBo bo)
        {
            TreatyPricingMedicalTableUploadLegend entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadLegendBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableUploadLegend entity = TreatyPricingMedicalTableUploadLegend.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId;
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableUploadLegendBo bo)
        {
            TreatyPricingMedicalTableUploadLegend.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableUploadLegendBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableUploadLegend.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            return TreatyPricingMedicalTableUploadLegend.DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadLegend)));
                }
            }
        }
    }
}
