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

namespace Services.Retrocession
{
    public class RetroTreatyDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroTreatyDetail)),
                Controller = ModuleBo.ModuleController.RetroTreatyDetail.ToString()
            };
        }

        public static RetroTreatyDetailBo FormBo(RetroTreatyDetail entity = null, bool loadParent = false)
        {
            if (entity == null)
                return null;
            return new RetroTreatyDetailBo
            {
                Id = entity.Id,
                RetroTreatyId = entity.RetroTreatyId,
                RetroTreatyBo = loadParent ? RetroTreatyService.Find(entity.RetroTreatyId) : null,
                PerLifeRetroConfigurationTreatyId = entity.PerLifeRetroConfigurationTreatyId,
                PerLifeRetroConfigurationTreatyBo = PerLifeRetroConfigurationTreatyService.Find(entity.PerLifeRetroConfigurationTreatyId),
                PremiumSpreadTableId = entity.PremiumSpreadTableId,
                PremiumSpreadTableBo = PremiumSpreadTableService.Find(entity.PremiumSpreadTableId),
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                TreatyDiscountTableBo = TreatyDiscountTableService.Find(entity.TreatyDiscountTableId),
                MlreShare = entity.MlreShare,
                MlreShareStr = Util.DoubleToString(entity.MlreShare),
                GrossRetroPremium = entity.GrossRetroPremium,
                TreatyDiscount = entity.TreatyDiscount,
                NetRetroPremium = entity.NetRetroPremium,
                Remark = entity.Remark,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroTreatyDetailBo> FormBos(IList<RetroTreatyDetail> entities = null, bool loadParent = false)
        {
            if (entities == null)
                return null;
            IList<RetroTreatyDetailBo> bos = new List<RetroTreatyDetailBo>() { };
            foreach (RetroTreatyDetail entity in entities)
            {
                bos.Add(FormBo(entity, loadParent));
            }
            return bos;
        }

        public static RetroTreatyDetail FormEntity(RetroTreatyDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroTreatyDetail
            {
                Id = bo.Id,
                RetroTreatyId = bo.RetroTreatyId,
                PerLifeRetroConfigurationTreatyId = bo.PerLifeRetroConfigurationTreatyId,
                PremiumSpreadTableId = bo.PremiumSpreadTableId,
                TreatyDiscountTableId = bo.TreatyDiscountTableId,
                MlreShare = bo.MlreShare,
                GrossRetroPremium = bo.GrossRetroPremium,
                TreatyDiscount = bo.TreatyDiscount,
                NetRetroPremium = bo.NetRetroPremium,
                Remark = bo.Remark,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroTreatyDetail.IsExists(id);
        }

        public static bool IsExists(int retroTreatyId, int perLifeRetroConfigurationTreatyId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreatyDetails
                    .Where(q => q.RetroTreatyId == retroTreatyId)
                    .Where(q => q.PerLifeRetroConfigurationTreatyId == perLifeRetroConfigurationTreatyId);

                return query.Any();
            }
        }

        public static RetroTreatyDetailBo Find(int? id)
        {
            if (!id.HasValue)
                return null;
            return FormBo(RetroTreatyDetail.Find(id));
        }

        public static int CountByRetroTreatyIdExcludeId(int retroTreatyId, int id)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroTreatyDetails.Where(q => q.RetroTreatyId == retroTreatyId).Where(q => q.Id != id).Count();
            }
        }

        public static IList<RetroTreatyDetailBo> GetByParams(DateTime startDate, DateTime endDate)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreatyDetails
                    .Where(q => q.RetroTreaty.EffectiveStartDate <= startDate)
                    .Where(q => q.RetroTreaty.EffectiveEndDate >= endDate);

                return FormBos(query.ToList(), true);
            }
        }

        public static IList<RetroTreatyDetailBo> GetByRetroTreatyId(int retroTreatyId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreatyDetails.Where(q => q.RetroTreatyId == retroTreatyId);

                return FormBos(query.ToList());
            }
        }

        public static int CountByPerLifeRetroConfigurationTreatyId(int perLifeRetroConfigurationTreatyId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreatyDetails
                    .Where(q => q.PerLifeRetroConfigurationTreatyId == perLifeRetroConfigurationTreatyId);

                return query.Count();
            }
        }

        public static int CountByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreatyDetails
                    .Where(q => q.PremiumSpreadTableId == premiumSpreadTableId);

                return query.Count();
            }
        }

        public static Result Save(ref RetroTreatyDetailBo bo)
        {
            if (!RetroTreatyDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroTreatyDetailBo bo, ref TrailObject trail)
        {
            if (!RetroTreaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroTreatyDetailBo bo)
        {
            RetroTreatyDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroTreatyDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroTreatyDetailBo bo)
        {
            Result result = Result();

            RetroTreatyDetail entity = RetroTreatyDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            entity = FormEntity(bo);
            if (result.Valid)
            {
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroTreatyDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroTreatyDetailBo bo)
        {
            RetroTreaty.Delete(bo.Id);
        }

        public static Result Delete(RetroTreatyDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = RetroTreatyDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result DeleteByRetroTreatyId(int retroTreatyId, ref TrailObject trail)
        {
            Result result = Result();
            foreach (var detailBo in GetByRetroTreatyId(retroTreatyId))
            {
                DataTrail dataTrail = RetroTreatyDetail.Delete(detailBo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
