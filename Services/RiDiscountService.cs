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
    public class RiDiscountService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDiscount)),
                Controller = ModuleBo.ModuleController.RiDiscount.ToString()
            };
        }

        public static Expression<Func<RiDiscount, RiDiscountBo>> Expression()
        {
            return entity => new RiDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                DurationFrom = entity.DurationFrom,
                DurationTo = entity.DurationTo,
                Discount = entity.Discount,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RiDiscountBo FormBo(RiDiscount entity = null)
        {
            if (entity == null)
                return null;
            return new RiDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountTableBo = DiscountTableService.Find(entity.DiscountTableId),
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveStartDateStr = entity.EffectiveStartDate?.ToString(Util.GetDateFormat()),
                EffectiveEndDate = entity.EffectiveEndDate,
                EffectiveEndDateStr = entity.EffectiveEndDate?.ToString(Util.GetDateFormat()),
                DurationFrom = entity.DurationFrom,
                DurationFromStr = Util.DoubleToString(entity.DurationFrom),
                DurationTo = entity.DurationTo,
                DurationToStr = Util.DoubleToString(entity.DurationTo),
                Discount = entity.Discount,
                DiscountStr = Util.DoubleToString(entity.Discount),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDiscountBo> FormBos(IList<RiDiscount> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDiscountBo> bos = new List<RiDiscountBo>() { };
            foreach (RiDiscount entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDiscount FormEntity(RiDiscountBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDiscount
            {
                Id = bo.Id,
                DiscountTableId = bo.DiscountTableId,
                DiscountCode = bo.DiscountCode?.Trim(),
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                DurationFrom = bo.DurationFrom,
                DurationTo = bo.DurationTo,
                Discount = bo.Discount,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDiscount.IsExists(id);
        }

        public static RiDiscountBo Find(int id)
        {
            return FormBo(RiDiscount.Find(id));
        }

        public static RiDiscountBo Find(int? id)
        {
            return FormBo(RiDiscount.Find(id));
        }

        public static RiDiscountBo FindByDiscountCode(string discountCode)
        {
            return FormBo(RiDiscount.FindByDiscountCode(discountCode));
        }

        public static RiDiscountBo FindByDiscountCodeCedantId(string discountCode, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RiDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.DiscountTable.CedantId == cedantId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<RiDiscountBo> Get()
        {
            return FormBos(RiDiscount.Get());
        }

        public static IList<RiDiscountBo> GetByDiscountTableId(int discountTableId)
        {
            return FormBos(RiDiscount.GetByDiscountTableId(discountTableId));
        }

        public static IList<RiDiscountBo> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            return FormBos(RiDiscount.GetByCedantId(cedantId, isDistinctCode));
        }

        public static int CountByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDiscountService");

                var query = connectionStrategy.Execute(() => db.RiDiscounts.Where(q => q.DiscountCode.Trim() == discountCode.Trim()));

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

                if (riDataBo.DurationMonth.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                (
                                    q.DurationFrom <= riDataBo.DurationMonth && q.DurationTo >= riDataBo.DurationMonth
                                    ||
                                    q.DurationFrom <= riDataBo.DurationMonth && q.DurationTo >= riDataBo.DurationMonth
                                )
                                ||
                                (q.DurationFrom == null && q.DurationTo == null)
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.DurationFrom == null && q.DurationTo == null));
                }

                return query.Count();
            }
        }

        public static RiDiscountBo FindByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("RiDiscountService");

                var query = connectionStrategy.Execute(() => db.RiDiscounts.Where(q => q.DiscountCode.Trim() == discountCode.Trim()));

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

                if (riDataBo.DurationMonth.HasValue)
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q =>
                            (
                                (
                                    q.DurationFrom <= riDataBo.DurationMonth && q.DurationTo >= riDataBo.DurationMonth
                                    ||
                                    q.DurationFrom <= riDataBo.DurationMonth && q.DurationTo >= riDataBo.DurationMonth
                                )
                                ||
                                (q.DurationFrom == null && q.DurationTo == null)
                            )
                        ));
                }
                else
                {
                    connectionStrategy.Reset();
                    query = connectionStrategy.Execute(() => query
                        .Where(q => q.DurationFrom == null && q.DurationTo == null));
                }

                return FormBo(query.FirstOrDefault());
            }
        }

        public static Result Save(ref RiDiscountBo bo)
        {
            if (!RiDiscount.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDiscountBo bo, ref TrailObject trail)
        {
            if (!RiDiscount.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDiscountBo bo)
        {
            RiDiscount entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDiscountBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDiscountBo bo)
        {
            Result result = Result();

            RiDiscount entity = RiDiscount.Find(bo.Id);
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
                entity.DurationFrom = bo.DurationFrom;
                entity.DurationTo = bo.DurationTo;
                entity.Discount = bo.Discount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref RiDiscountBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDiscountBo bo)
        {
            RiDiscount.Delete(bo.Id);
        }

        public static Result Delete(RiDiscountBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RiDiscount.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDiscountTableIdExcept(int discountTableId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<RiDiscount> riDiscounts = RiDiscount.GetByDiscountTableIdExcept(discountTableId, saveIds);
            foreach (RiDiscount riDiscount in riDiscounts)
            {
                DataTrail dataTrail = RiDiscount.Delete(riDiscount.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            return RiDiscount.DeleteAllByDiscountTableId(discountTableId);
        }

        public static void DeleteAllByDiscountTableId(int discountTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByDiscountTableId(discountTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDiscount)));
                }
            }
        }
    }
}
