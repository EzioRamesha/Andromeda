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
    public class TreatyPricingMedicalTableUploadColumnService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadColumn)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableUploadColumn.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableUploadColumn, TreatyPricingMedicalTableUploadColumnBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableUploadColumnBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                MinimumAge = entity.MinimumAge,
                MaximumAge = entity.MaximumAge,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableUploadColumnBo FormBo(TreatyPricingMedicalTableUploadColumn entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableUploadColumnBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableVersionDetailId = entity.TreatyPricingMedicalTableVersionDetailId,
                TreatyPricingMedicalTableVersionDetailBo = TreatyPricingMedicalTableVersionDetailService.Find(entity.TreatyPricingMedicalTableVersionDetailId),
                MinimumAge = entity.MinimumAge,
                MaximumAge = entity.MaximumAge,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingMedicalTableUploadColumnBo> FormBos(IList<TreatyPricingMedicalTableUploadColumn> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableUploadColumnBo> bos = new List<TreatyPricingMedicalTableUploadColumnBo>() { };
            foreach (TreatyPricingMedicalTableUploadColumn entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableUploadColumn FormEntity(TreatyPricingMedicalTableUploadColumnBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableUploadColumn
            {
                Id = bo.Id,
                TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId,
                MinimumAge = bo.MinimumAge,
                MaximumAge = bo.MaximumAge,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTableUploadColumn.IsExists(id);
        }

        public static TreatyPricingMedicalTableUploadColumnBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableUploadColumn.Find(id));
        }

        public static IList<TreatyPricingMedicalTableUploadColumnBo> GetByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableUploadColumns
                    .Where(q => q.TreatyPricingMedicalTableVersionDetailId == treatyPricingMedicalTableVersionDetailId)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableUploadColumnBo bo)
        {
            if (!TreatyPricingMedicalTableUploadColumn.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableUploadColumnBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableUploadColumn.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadColumnBo bo)
        {
            TreatyPricingMedicalTableUploadColumn entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadColumnBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadColumnBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableUploadColumn entity = TreatyPricingMedicalTableUploadColumn.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableVersionDetailId = bo.TreatyPricingMedicalTableVersionDetailId;
                entity.MinimumAge = bo.MinimumAge;
                entity.MaximumAge = bo.MaximumAge;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadColumnBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableUploadColumnBo bo)
        {
            TreatyPricingMedicalTableUploadColumn.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableUploadColumnBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableUploadColumn.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId)
        {
            return TreatyPricingMedicalTableUploadColumn.DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableVersionDetailId(int treatyPricingMedicalTableVersionDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableVersionDetailId(treatyPricingMedicalTableVersionDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadColumn)));
                }
            }
        }
    }
}
