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
    public class GroupDiscountService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(GroupDiscount)),
                Controller = ModuleBo.ModuleController.GroupDiscount.ToString()
            };
        }

        public static Expression<Func<GroupDiscount, GroupDiscountBo>> Expression()
        {
            return entity => new GroupDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                GroupSizeFrom = entity.GroupSizeFrom,
                GroupSizeTo = entity.GroupSizeTo,
                Discount = entity.Discount,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static GroupDiscountBo FormBo(GroupDiscount entity = null)
        {
            if (entity == null)
                return null;
            return new GroupDiscountBo
            {
                Id = entity.Id,
                DiscountTableId = entity.DiscountTableId,
                DiscountTableBo = DiscountTableService.Find(entity.DiscountTableId),
                DiscountCode = entity.DiscountCode,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveStartDateStr = entity.EffectiveStartDate?.ToString(Util.GetDateFormat()),
                EffectiveEndDate = entity.EffectiveEndDate,
                EffectiveEndDateStr = entity.EffectiveEndDate?.ToString(Util.GetDateFormat()),
                GroupSizeFrom = entity.GroupSizeFrom,
                GroupSizeFromStr = entity.GroupSizeFrom.ToString(),
                GroupSizeTo = entity.GroupSizeTo,
                GroupSizeToStr = entity.GroupSizeTo.ToString(),
                Discount = entity.Discount,
                DiscountStr = Util.DoubleToString(entity.Discount),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<GroupDiscountBo> FormBos(IList<GroupDiscount> entities = null)
        {
            if (entities == null)
                return null;
            IList<GroupDiscountBo> bos = new List<GroupDiscountBo>() { };
            foreach (GroupDiscount entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static GroupDiscount FormEntity(GroupDiscountBo bo = null)
        {
            if (bo == null)
                return null;
            return new GroupDiscount
            {
                Id = bo.Id,
                DiscountTableId = bo.DiscountTableId,
                DiscountCode = bo.DiscountCode?.Trim(),
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                GroupSizeFrom = bo.GroupSizeFrom,
                GroupSizeTo = bo.GroupSizeTo,
                Discount = bo.Discount,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return GroupDiscount.IsExists(id);
        }

        public static GroupDiscountBo Find(int id)
        {
            return FormBo(GroupDiscount.Find(id));
        }

        public static GroupDiscountBo Find(int? id)
        {
            return FormBo(GroupDiscount.Find(id));
        }
        public static GroupDiscountBo FindByDiscountCode(string discountCode)
        {
            return FormBo(GroupDiscount.FindByDiscountCode(discountCode));
        }

        public static GroupDiscountBo FindByDiscountCodeCedantId(string discountCode, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.GroupDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.DiscountTable.CedantId == cedantId);

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<GroupDiscountBo> Get()
        {
            return FormBos(GroupDiscount.Get());
        }

        public static IList<GroupDiscountBo> GetByDiscountTableId(int discountTableId)
        {
            return FormBos(GroupDiscount.GetByDiscountTableId(discountTableId));
        }

        public static IList<GroupDiscountBo> GetByCedantId(int cedantId, bool isDistinctCode = false)
        {
            return FormBos(GroupDiscount.GetByCedantId(cedantId, isDistinctCode));
        }

        public static int CountByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("GroupDiscountService");

                var query = connectionStrategy.Execute(() => db.GroupDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.GroupSizeFrom <= riDataBo.PolicyTotalLive && q.GroupSizeTo >= riDataBo.PolicyTotalLive || q.GroupSizeFrom <= riDataBo.PolicyTotalLive && q.GroupSizeTo >= riDataBo.PolicyTotalLive));

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

                return query.Count();
            }
        }

        public static GroupDiscountBo FindByParams(RiDataBo riDataBo, string discountCode)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("GroupDiscountService");

                var query = connectionStrategy.Execute(() => db.GroupDiscounts
                    .Where(q => q.DiscountCode.Trim() == discountCode.Trim())
                    .Where(q => q.GroupSizeFrom <= riDataBo.PolicyTotalLive && q.GroupSizeTo >= riDataBo.PolicyTotalLive || q.GroupSizeFrom <= riDataBo.PolicyTotalLive && q.GroupSizeTo >= riDataBo.PolicyTotalLive));

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

        public static Result Save(ref GroupDiscountBo bo)
        {
            if (!GroupDiscount.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref GroupDiscountBo bo, ref TrailObject trail)
        {
            if (!GroupDiscount.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref GroupDiscountBo bo)
        {
            GroupDiscount entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref GroupDiscountBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref GroupDiscountBo bo)
        {
            Result result = Result();

            GroupDiscount entity = GroupDiscount.Find(bo.Id);
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
                entity.GroupSizeFrom = bo.GroupSizeFrom;
                entity.GroupSizeTo = bo.GroupSizeTo;
                entity.Discount = bo.Discount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref GroupDiscountBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(GroupDiscountBo bo)
        {
            GroupDiscount.Delete(bo.Id);
        }

        public static Result Delete(GroupDiscountBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = GroupDiscount.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDiscountTableIdExcept(int discountTableId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<GroupDiscount> groupDiscounts = GroupDiscount.GetByDiscountTableIdExcept(discountTableId, saveIds);
            foreach (GroupDiscount groupDiscount in groupDiscounts)
            {
                DataTrail dataTrail = GroupDiscount.Delete(groupDiscount.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByDiscountTableId(int discountTableId)
        {
            return GroupDiscount.DeleteAllByDiscountTableId(discountTableId);
        }

        public static void DeleteAllByDiscountTableId(int discountTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByDiscountTableId(discountTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(GroupDiscount)));
                }
            }
        }
    }
}
