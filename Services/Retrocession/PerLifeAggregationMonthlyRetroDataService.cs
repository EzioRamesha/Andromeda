using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeAggregationMonthlyRetroDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationMonthlyRetroData)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationMonthlyRetroData.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationMonthlyRetroData, PerLifeAggregationMonthlyRetroDataBo>> Expression()
        {
            return entity => new PerLifeAggregationMonthlyRetroDataBo
            {
                Id = entity.Id,
                PerLifeAggregationMonthlyDataId = entity.PerLifeAggregationMonthlyDataId,
                RetroParty = entity.RetroParty,
                RetroAmount = entity.RetroAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroGst = entity.RetroGst,
                MlreShare = entity.MlreShare,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationMonthlyRetroDataBo FormBo(PerLifeAggregationMonthlyRetroData entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationMonthlyRetroDataBo
            {
                Id = entity.Id,
                PerLifeAggregationMonthlyDataId = entity.PerLifeAggregationMonthlyDataId,
                PerLifeAggregationMonthlyDataBo = PerLifeAggregationMonthlyDataService.Find(entity.PerLifeAggregationMonthlyDataId),
                RetroParty = entity.RetroParty,
                RetroAmount = entity.RetroAmount,
                RetroGrossPremium = entity.RetroGrossPremium,
                RetroNetPremium = entity.RetroNetPremium,
                RetroDiscount = entity.RetroDiscount,
                RetroGst = entity.RetroGst,
                MlreShare = entity.MlreShare,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationMonthlyRetroDataBo> FormBos(IList<PerLifeAggregationMonthlyRetroData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationMonthlyRetroDataBo> bos = new List<PerLifeAggregationMonthlyRetroDataBo>() { };
            foreach (PerLifeAggregationMonthlyRetroData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregationMonthlyRetroData FormEntity(PerLifeAggregationMonthlyRetroDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationMonthlyRetroData
            {
                Id = bo.Id,
                PerLifeAggregationMonthlyDataId = bo.PerLifeAggregationMonthlyDataId,
                RetroParty = bo.RetroParty,
                RetroAmount = bo.RetroAmount,
                RetroGrossPremium = bo.RetroGrossPremium,
                RetroNetPremium = bo.RetroNetPremium,
                RetroDiscount = bo.RetroDiscount,
                RetroGst = bo.RetroGst,
                MlreShare = bo.MlreShare,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationMonthlyRetroData.IsExists(id);
        }

        public static PerLifeAggregationMonthlyRetroDataBo Find(int? id)
        {
            return FormBo(PerLifeAggregationMonthlyRetroData.Find(id));
        }

        public static IList<PerLifeAggregationMonthlyRetroDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationMonthlyRetroData.ToList());
            }
        }

        public static IList<PerLifeAggregationMonthlyRetroDataBo> GetByPerLifeAggregationMonthlyDataId(int perLifeAggregationMonthlyDataId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationMonthlyRetroData
                    .Where(q => q.PerLifeAggregationMonthlyDataId == perLifeAggregationMonthlyDataId)
                    .ToList());
            }
        }

        public static List<string> GetDistinctRetroPartyByPerLifeAggregationDetailId(int? perLifeAggregationDetailId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.PerLifeAggregationMonthlyRetroData
                    .Where(q => q.PerLifeAggregationMonthlyData.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetailId == perLifeAggregationDetailId)
                    .GroupBy(q => q.RetroParty)
                    .Select(q => q.FirstOrDefault().RetroParty).ToList();
            }
        }

        public static List<string> GetDistinctRetroPartyByPerLifeAggregationId(int perLifeAggregationId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.PerLifeAggregationMonthlyRetroData
                    .Where(q => q.PerLifeAggregationMonthlyData.PerLifeAggregationDetailData.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == perLifeAggregationId)
                    .GroupBy(q => q.RetroParty)
                    .Select(q => q.FirstOrDefault().RetroParty).ToList();
            }
        }

        public static Result Save(ref PerLifeAggregationMonthlyRetroDataBo bo)
        {
            if (!PerLifeAggregationMonthlyRetroData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationMonthlyRetroDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationMonthlyRetroData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationMonthlyRetroDataBo bo)
        {
            PerLifeAggregationMonthlyRetroData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationMonthlyRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationMonthlyRetroDataBo bo)
        {
            Result result = Result();

            PerLifeAggregationMonthlyRetroData entity = PerLifeAggregationMonthlyRetroData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeAggregationMonthlyDataId = bo.PerLifeAggregationMonthlyDataId;
                entity.RetroParty = bo.RetroParty;
                entity.RetroAmount = bo.RetroAmount;
                entity.RetroGrossPremium = bo.RetroGrossPremium;
                entity.RetroNetPremium = bo.RetroNetPremium;
                entity.RetroDiscount = bo.RetroDiscount;
                entity.RetroGst = bo.RetroGst;
                entity.MlreShare = bo.MlreShare;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationMonthlyRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationMonthlyRetroDataBo bo)
        {
            PerLifeAggregationMonthlyRetroData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationMonthlyRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: Add validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationMonthlyRetroData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteByPerLifeAggregationMonthlyDataId(int perLifeAggregationMonthlyDataId)
        {
            return PerLifeAggregationMonthlyRetroData.DeleteByPerLifeAggregationMonthlyDataId(perLifeAggregationMonthlyDataId);
        }

        public static void DeleteByPerLifeAggregationMonthlyDataId(int perLifeAggregationMonthlyDataId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByPerLifeAggregationMonthlyDataId(perLifeAggregationMonthlyDataId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeAggregationMonthlyRetroData)));
                }
            }
        }
    }
}
