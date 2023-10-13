using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TreatyDiscountTableDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyDiscountTableDetail)),
                Controller = ModuleBo.ModuleController.TreatyDiscountTableDetail.ToString()
            };
        }

        public static Expression<Func<TreatyDiscountTableDetail, TreatyDiscountTableDetailBo>> Expression()
        {
            return entity => new TreatyDiscountTableDetailBo
            {
                Id = entity.Id,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                AgeFrom = entity.AgeFrom,
                AgeTo = entity.AgeTo,
                AARFrom = entity.AARFrom,
                AARTo = entity.AARTo,
                Discount = entity.Discount,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyDiscountTableDetailBo FormBo(TreatyDiscountTableDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyDiscountTableDetailBo
            {
                Id = entity.Id,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                TreatyDiscountTableBo = TreatyDiscountTableService.Find(entity.TreatyDiscountTableId),
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                BenefitBos = BenefitService.GetByBenefitCode(entity.BenefitCode),
                AgeFrom = entity.AgeFrom,
                AgeFromStr = entity.AgeFrom.ToString(),
                AgeTo = entity.AgeTo,
                AgeToStr = entity.AgeTo.ToString(),
                AARFrom = entity.AARFrom,
                AARFromStr = entity.AARFrom.ToString(),
                AARTo = entity.AARTo,
                AARToStr = entity.AARTo.ToString(),
                Discount = entity.Discount,
                DiscountStr = Util.DoubleToString(entity.Discount),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyDiscountTableDetailBo> FormBos(IList<TreatyDiscountTableDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyDiscountTableDetailBo> bos = new List<TreatyDiscountTableDetailBo>() { };
            foreach (TreatyDiscountTableDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyDiscountTableDetail FormEntity(TreatyDiscountTableDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyDiscountTableDetail
            {
                Id = bo.Id,
                TreatyDiscountTableId = bo.TreatyDiscountTableId,
                CedingPlanCode = bo.CedingPlanCode,
                BenefitCode = bo.BenefitCode,
                AgeFrom = bo.AgeFrom,
                AgeTo = bo.AgeTo,
                AARFrom = bo.AARFrom,
                AARTo = bo.AARTo,
                Discount = bo.Discount,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyDiscountTableDetail.IsExists(id);
        }

        public static TreatyDiscountTableDetailBo Find(int? id)
        {
            return FormBo(TreatyDiscountTableDetail.Find(id));
        }

        public static IList<TreatyDiscountTableDetailBo> Get()
        {
            return FormBos(TreatyDiscountTableDetail.Get());
        }

        public static IList<TreatyDiscountTableDetailBo> GetByTreatyDiscountTableId(int treatyDiscountTableId)
        {
            return FormBos(TreatyDiscountTableDetail.GetByTreatyDiscountTableId(treatyDiscountTableId));
        }

        public static IList<TreatyDiscountTableDetailBo> GetByTreatyDiscountTableIdExcludeId(int treatyDiscountTableId, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyDiscountTableDetails.Where(q => q.TreatyDiscountTableId == treatyDiscountTableId);

                if (id.HasValue)
                    query = query.Where(q => q.Id != id);

                return FormBos(query.ToList());
            }
        }

        public static double? GetDiscountByParams(int treatyDiscountTableId, string cedingPlanCode, string mlreBenefitCode, double? aar)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.TreatyDiscountTableDetails
                    .Where(q => q.TreatyDiscountTableId == treatyDiscountTableId);
                    //.Where(q => q.BenefitCode.Contains(mlreBenefitCode));

                if (aar.HasValue)
                {
                    query = query.Where(q => (q.AARFrom <= aar && q.AARTo >= aar) || (q.AARFrom == null && q.AARTo == null));
                }
                else
                {
                    query = query.Where(q => q.AARFrom == null && q.AARTo == null);
                }

                var entities = query.ToList();
                if (entities.IsNullOrEmpty())
                    return null;

                var entity = entities.Where(q => string.IsNullOrEmpty(q.BenefitCode) || q.BenefitCode.Split(',').Select(r => r.Trim()).ToArray().Contains(mlreBenefitCode))
                    .Where(q => q.CedingPlanCode.Split(',').Select(r => r.Trim()).ToArray().Contains(cedingPlanCode)).FirstOrDefault();

                return entity?.Discount;
            }
        }

        public static Result Save(ref TreatyDiscountTableDetailBo bo)
        {
            if (!TreatyDiscountTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyDiscountTableDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyDiscountTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyDiscountTableDetailBo bo)
        {
            TreatyDiscountTableDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyDiscountTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyDiscountTableDetailBo bo)
        {
            Result result = Result();

            TreatyDiscountTableDetail entity = TreatyDiscountTableDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.TreatyDiscountTableId = bo.TreatyDiscountTableId;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.BenefitCode = bo.BenefitCode;
                entity.AgeFrom = bo.AgeFrom;
                entity.AgeTo = bo.AgeTo;
                entity.AARFrom = bo.AARFrom;
                entity.AARTo = bo.AARTo;
                entity.Discount = bo.Discount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref TreatyDiscountTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyDiscountTableDetailBo bo)
        {
            TreatyDiscountTableDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyDiscountTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = TreatyDiscountTableDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByTreatyDiscountTableIdExcept(int treatyDiscountTableId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyDiscountTableDetail> treatyDiscountTableDetails = TreatyDiscountTableDetail.GetByTreatyDiscountTableIdExcept(treatyDiscountTableId, saveIds);
            foreach (TreatyDiscountTableDetail treatyDiscountTableDetail in treatyDiscountTableDetails)
            {
                DataTrail dataTrail = TreatyDiscountTableDetail.Delete(treatyDiscountTableDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByTreatyDiscountTableId(int treatyDiscountTableId)
        {
            return TreatyDiscountTableDetail.DeleteByTreatyDiscountTableId(treatyDiscountTableId);
        }

        public static void DeleteByTreatyDiscountTableId(int treatyDiscountTableId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByTreatyDiscountTableId(treatyDiscountTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
