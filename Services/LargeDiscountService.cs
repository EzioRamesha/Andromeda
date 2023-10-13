using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class LargeDiscountService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(LargeDiscount)),
                Controller = ModuleBo.ModuleController.LargeDiscount.ToString()
            };
        }

        public static Expression<Func<LargeDiscount, LargeDiscountBo>> Expression()
        {
            return entity => new LargeDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                AarFrom = entity.AarFrom,
                AarTo = entity.AarTo,
                Discount = entity.Discount,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static LargeDiscountBo FormBo(LargeDiscount entity = null)
        {
            if (entity == null)
                return null;
            return new LargeDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountTableBo = DiscountTableService.Find(entity.DiscountTableId),
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveStartDateStr = entity.EffectiveStartDate?.ToString(Util.GetDateFormat()),
                EffectiveEndDate = entity.EffectiveEndDate,
                EffectiveEndDateStr = entity.EffectiveEndDate?.ToString(Util.GetDateFormat()),
                AarFrom = entity.AarFrom,
                AarFromStr = Util.DoubleToString(entity.AarFrom),
                AarTo = entity.AarTo,
                AarToStr = Util.DoubleToString(entity.AarTo),
                Discount = entity.Discount,
                DiscountStr = Util.DoubleToString(entity.Discount),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<LargeDiscountBo> FormBos(IList<LargeDiscount> entities = null)
        {
            if (entities == null)
                return null;
            IList<LargeDiscountBo> bos = new List<LargeDiscountBo>() { };
            foreach (LargeDiscount entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static LargeDiscount FormEntity(LargeDiscountBo bo = null)
        {
            if (bo == null)
                return null;
            return new LargeDiscount
            {
                Id = bo.Id,
                DiscountTableId = bo.DiscountTableId,
                DiscountCode = bo.DiscountCode?.Trim(),
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                AarFrom = bo.AarFrom,
                AarTo = bo.AarTo,
                Discount = bo.Discount,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return LargeDiscount.IsExists(id);
        }

        public static LargeDiscountBo Find(int id)
        {
            return FormBo(LargeDiscount.Find(id));
        }

        public static LargeDiscountBo Find(int? id)
        {
            return FormBo(LargeDiscount.Find(id));
        }

        public static LargeDiscountBo FindByDiscountCode(string discountCode)
        {
            return FormBo(LargeDiscount.FindByDiscountCode(discountCode));
        }

        public static LargeDiscountBo FindByDiscountCodeCedantId(string discountCode, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.LargeDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.DiscountTable.CedantId == cedantId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<LargeDiscountBo> Get()
        {
            return FormBos(LargeDiscount.Get());
        }

        public static IList<LargeDiscountBo> GetByDiscountTableId(int discountTableId)
        {
            return FormBos(LargeDiscount.GetByDiscountTableId(discountTableId));
        }

        public static IList<LargeDiscountBo> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            return FormBos(LargeDiscount.GetByCedantId(cedantId, isDistinctCode));
        }

        public static int CountByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("LargeDiscountService");

                var query = connectionStrategy.Execute(() => db.LargeDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.AarFrom <= riDataBo.OriSumAssured && q.AarTo >= riDataBo.OriSumAssured || q.AarFrom <= riDataBo.OriSumAssured && q.AarTo >= riDataBo.OriSumAssured));

                if (riDataBo.ReinsEffDatePol.HasValue)
                {
                    connectionStrategy.retryCount = 0;
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                            )
                            ||
                            (q.EffectiveStartDate == null && q.EffectiveEndDate == null)
                        ));
                }
                else
                {
                    connectionStrategy.retryCount = 0;
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.EffectiveStartDate == null && q.EffectiveEndDate == null));
                }

                return query.Count();
            }
        }

        public static LargeDiscountBo FindByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("LargeDiscountService");

                var query = connectionStrategy.Execute(() => db.LargeDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.AarFrom <= riDataBo.OriSumAssured && q.AarTo >= riDataBo.OriSumAssured || q.AarFrom <= riDataBo.OriSumAssured && q.AarTo >= riDataBo.OriSumAssured));

                if (riDataBo.ReinsEffDatePol.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                ||
                                DbFunctions.TruncateTime(q.EffectiveStartDate) <= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                                && DbFunctions.TruncateTime(q.EffectiveEndDate) >= DbFunctions.TruncateTime(riDataBo.ReinsEffDatePol)
                            )
                            ||
                            (q.EffectiveStartDate == null && q.EffectiveEndDate == null)
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.EffectiveStartDate == null && q.EffectiveEndDate == null));
                }

                return FormBo(query.FirstOrDefault());
            }
        }

        public static Result Save(ref LargeDiscountBo bo)
        {
            if (!LargeDiscount.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref LargeDiscountBo bo, ref TrailObject trail)
        {
            if (!LargeDiscount.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref LargeDiscountBo bo)
        {
            LargeDiscount entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref LargeDiscountBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref LargeDiscountBo bo)
        {
            Result result = Result();

            LargeDiscount entity = LargeDiscount.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.DiscountTableId = bo.DiscountTableId;
                entity.DiscountCode = bo.DiscountCode;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;
                entity.AarFrom = bo.AarFrom;
                entity.AarTo = bo.AarTo;
                entity.Discount = bo.Discount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref LargeDiscountBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(LargeDiscountBo bo)
        {
            LargeDiscount.Delete(bo.Id);
        }

        public static Result Delete(LargeDiscountBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = LargeDiscount.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDiscountTableIdExcept(int discountTableId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<LargeDiscount> largeDiscounts = LargeDiscount.GetByDiscountTableIdExcept(discountTableId, saveIds);
            foreach (LargeDiscount largeDiscount in largeDiscounts)
            {
                DataTrail dataTrail = LargeDiscount.Delete(largeDiscount.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            return LargeDiscount.DeleteAllByDiscountTableId(discountTableId);
        }

        public static void DeleteAllByDiscountTableId(int discountTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByDiscountTableId(discountTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(LargeDiscount)));
                }
            }
        }
    }
}
