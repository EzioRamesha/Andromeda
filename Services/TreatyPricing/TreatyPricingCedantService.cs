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
    public class TreatyPricingCedantService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingCedant)),
                Controller = ModuleBo.ModuleController.TreatyPricingCedant.ToString()
            };
        }

        public static Expression<Func<TreatyPricingCedant, TreatyPricingCedantBo>> Expression()
        {
            return entity => new TreatyPricingCedantBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                Code = entity.Code,
                NoOfProduct = entity.NoOfProduct,
                NoOfDocument = entity.NoOfDocument,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingCedantBo FormBo(TreatyPricingCedant entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingCedantBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                ReinsuranceTypePickListDetailBo = PickListDetailService.Find(entity.ReinsuranceTypePickListDetailId),
                Code = entity.Code,
                NoOfProduct = entity.NoOfProduct,
                NoOfDocument = entity.NoOfDocument,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingCedantBo> FormBos(IList<TreatyPricingCedant> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingCedantBo> bos = new List<TreatyPricingCedantBo>() { };
            foreach (TreatyPricingCedant entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingCedant FormEntity(TreatyPricingCedantBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingCedant
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId,
                Code = bo.Code,
                NoOfProduct = bo.NoOfProduct,
                NoOfDocument = bo.NoOfDocument,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingCedant.IsExists(id);
        }

        public static TreatyPricingCedantBo Find(int? id)
        {
            return FormBo(TreatyPricingCedant.Find(id));
        }

        public static TreatyPricingCedantBo FindByCode(string code)
        {
            return FormBo(TreatyPricingCedant.FindByCode(code));
        }

        public static TreatyPricingCedantBo FindByCedantIdReinsuranceType(int cedantId, int reinsuranceTypePickListDetailId)
        {
            return FormBo(TreatyPricingCedant.FindByCedantIdReinsuranceType(cedantId, reinsuranceTypePickListDetailId));
        }

        public static IList<TreatyPricingCedantBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCedants.OrderBy(q => q.Cedant.Code).ToList());
            }
        }

        public static IList<TreatyPricingCedantBo> GetByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingCedants.Where(q => q.CedantId == cedantId).ToList());
            }
        }

        public static int CountByReinsuranceTypePickListDetailId(int reinsuranceTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingCedants.Where(q => q.ReinsuranceTypePickListDetailId == reinsuranceTypePickListDetailId).Count();
            }
        }

        public static Result Save(ref TreatyPricingCedantBo bo)
        {
            if (!TreatyPricingCedant.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingCedantBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingCedant.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(TreatyPricingCedant treatyPricingCedant)
        {
            return treatyPricingCedant.IsDuplicate();
        }

        public static Result Create(ref TreatyPricingCedantBo bo)
        {
            TreatyPricingCedant entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Duplicate Cedant found with same Reinsurance Type");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingCedantBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCedantBo bo)
        {
            Result result = Result();

            TreatyPricingCedant entity = TreatyPricingCedant.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Duplicate Cedant found with same Reinsurance Type");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.CedantId = bo.CedantId;
                entity.ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId;
                entity.Code = bo.Code;
                entity.NoOfProduct = bo.NoOfProduct;
                entity.NoOfDocument = bo.NoOfDocument;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingCedantBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingCedantBo bo)
        {
            TreatyPricingCedant.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingCedantBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingCedant.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
