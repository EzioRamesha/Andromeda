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
    public class TreatyPricingMedicalTableUploadCellService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadCell)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableUploadCell.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTableUploadCell, TreatyPricingMedicalTableUploadCellBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableUploadCellBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableUploadColumnId = entity.TreatyPricingMedicalTableUploadColumnId,
                TreatyPricingMedicalTableUploadRowId = entity.TreatyPricingMedicalTableUploadRowId,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableUploadCellBo FormBo(TreatyPricingMedicalTableUploadCell entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableUploadCellBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableUploadColumnId = entity.TreatyPricingMedicalTableUploadColumnId,
                TreatyPricingMedicalTableUploadRowId = entity.TreatyPricingMedicalTableUploadRowId,
                Code = entity.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                TreatyPricingMedicalTableUploadColumnBo = TreatyPricingMedicalTableUploadColumnService.Find(entity.TreatyPricingMedicalTableUploadColumnId),
                TreatyPricingMedicalTableUploadRowBo = TreatyPricingMedicalTableUploadRowService.Find(entity.TreatyPricingMedicalTableUploadRowId),
            };
        }

        public static IList<TreatyPricingMedicalTableUploadCellBo> FormBos(IList<TreatyPricingMedicalTableUploadCell> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableUploadCellBo> bos = new List<TreatyPricingMedicalTableUploadCellBo>() { };
            foreach (TreatyPricingMedicalTableUploadCell entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableUploadCell FormEntity(TreatyPricingMedicalTableUploadCellBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableUploadCell
            {
                Id = bo.Id,
                TreatyPricingMedicalTableUploadColumnId = bo.TreatyPricingMedicalTableUploadColumnId,
                TreatyPricingMedicalTableUploadRowId = bo.TreatyPricingMedicalTableUploadRowId,
                Code = bo.Code,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTableUploadCell.IsExists(id);
        }

        public static TreatyPricingMedicalTableUploadCellBo Find(int id)
        {
            return FormBo(TreatyPricingMedicalTableUploadCell.Find(id));
        }

        public static IList<TreatyPricingMedicalTableUploadCellBo> GetByTreatyPricingMedicalTableUploadColumnId(int treatyPricingMedicalTableUploadColumnId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableUploadCells
                    .Where(q => q.TreatyPricingMedicalTableUploadColumnId == treatyPricingMedicalTableUploadColumnId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingMedicalTableUploadCellBo> GetByTreatyPricingMedicalTableUploadRowId(int treatyPricingMedicalTableUploadRowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableUploadCells
                    .Where(q => q.TreatyPricingMedicalTableUploadRowId == treatyPricingMedicalTableUploadRowId)
                    .ToList());
            }
        }

        public static TreatyPricingMedicalTableUploadCellBo GetByTreatyPricingMedicalTableUploadRowColumnId(int treatyPricingMedicalTableUploadRowId, int treatyPricingMedicalTableUploadColumnId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingMedicalTableUploadCells
                    .Where(q => q.TreatyPricingMedicalTableUploadRowId == treatyPricingMedicalTableUploadRowId &&
                    q.TreatyPricingMedicalTableUploadColumnId == treatyPricingMedicalTableUploadColumnId)
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableUploadCellBo bo)
        {
            if (!TreatyPricingMedicalTableUploadCell.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableUploadCellBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableUploadCell.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadCellBo bo)
        {
            TreatyPricingMedicalTableUploadCell entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableUploadCellBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadCellBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableUploadCell entity = TreatyPricingMedicalTableUploadCell.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingMedicalTableUploadColumnId = bo.TreatyPricingMedicalTableUploadColumnId;
                entity.TreatyPricingMedicalTableUploadRowId = bo.TreatyPricingMedicalTableUploadRowId;
                entity.Code = bo.Code;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableUploadCellBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableUploadCellBo bo)
        {
            TreatyPricingMedicalTableUploadCell.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableUploadCellBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableUploadCell.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableUploadColumnId(int treatyPricingMedicalTableUploadColumnId)
        {
            return TreatyPricingMedicalTableUploadCell.DeleteAllByTreatyPricingMedicalTableUploadColumnId(treatyPricingMedicalTableUploadColumnId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableUploadColumnId(int treatyPricingMedicalTableUploadColumnId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableUploadColumnId(treatyPricingMedicalTableUploadColumnId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadCell)));
                }
            }
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableUploadRowId(int treatyPricingMedicalTableUploadRowId)
        {
            return TreatyPricingMedicalTableUploadCell.DeleteAllByTreatyPricingMedicalTableUploadRowId(treatyPricingMedicalTableUploadRowId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableUploadRowId(int treatyPricingMedicalTableUploadRowId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableUploadRowId(treatyPricingMedicalTableUploadRowId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableUploadCell)));
                }
            }
        }
    }
}
