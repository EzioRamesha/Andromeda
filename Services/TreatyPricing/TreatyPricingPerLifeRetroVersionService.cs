using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingPerLifeRetroVersion.ToString()
            };
        }

        public static TreatyPricingPerLifeRetroVersionBo FormBo(TreatyPricingPerLifeRetroVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingPerLifeRetroVersionBo
            {
                Id = entity.Id,
                TreatyPricingPerLifeRetroId = entity.TreatyPricingPerLifeRetroId,
                TreatyPricingPerLifeRetroBo = foreign ? TreatyPricingPerLifeRetroService.Find(entity.TreatyPricingPerLifeRetroId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                EffectiveDate = entity.EffectiveDate,
                RetrocessionaireRetroPartyId = entity.RetrocessionaireRetroPartyId,
                RefundofUnearnedPremium = entity.RefundofUnearnedPremium,
                TerminationPeriod = entity.TerminationPeriod,
                ResidenceCountry = entity.ResidenceCountry,
                PaymentRetrocessionairePremiumPickListDetailId = entity.PaymentRetrocessionairePremiumPickListDetailId,
                JumboLimitCurrencyCodePickListDetailId = entity.JumboLimitCurrencyCodePickListDetailId,
                JumboLimit = entity.JumboLimit,
                Remarks = entity.Remarks,
                ProfitSharing = entity.ProfitSharing,
                ProfitDescription = entity.ProfitDescription,
                NetProfitPercentage = entity.NetProfitPercentage,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                ProfitCommissionDetail = TreatyPricingPerLifeRetroVersionDetailService.GetJsonByParent(entity.Id),
                TierProfitCommission = TreatyPricingPerLifeRetroVersionDetailService.GetJsonByParent(entity.Id),
                Benefits = TreatyPricingPerLifeRetroVersionBenefitService.GetJsonByVersionId(entity.Id),

                EffectiveDateStr = entity.EffectiveDate?.ToString(Util.GetDateFormat()),
                JumboLimitStr = Util.DoubleToString(entity.JumboLimit, 2),
                NetProfitPercentageStr = Util.DoubleToString(entity.NetProfitPercentage),
            };
        }

        public static IList<TreatyPricingPerLifeRetroVersionBo> FormBos(IList<TreatyPricingPerLifeRetroVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroVersionBo> bos = new List<TreatyPricingPerLifeRetroVersionBo>() { };
            foreach (TreatyPricingPerLifeRetroVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetroVersion FormEntity(TreatyPricingPerLifeRetroVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetroVersion
            {
                Id = bo.Id,
                TreatyPricingPerLifeRetroId = bo.TreatyPricingPerLifeRetroId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveDate = bo.EffectiveDate,
                JumboLimitCurrencyCodePickListDetailId = bo.JumboLimitCurrencyCodePickListDetailId,
                JumboLimit = bo.JumboLimit,
                RetrocessionaireRetroPartyId = bo.RetrocessionaireRetroPartyId,
                RefundofUnearnedPremium = bo.RefundofUnearnedPremium,
                TerminationPeriod = bo.TerminationPeriod,
                ResidenceCountry = bo.ResidenceCountry,
                PaymentRetrocessionairePremiumPickListDetailId = bo.PaymentRetrocessionairePremiumPickListDetailId,
                Remarks = bo.Remarks,
                ProfitSharing = bo.ProfitSharing,
                ProfitDescription = bo.ProfitDescription,
                NetProfitPercentage = bo.NetProfitPercentage,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingPerLifeRetroVersion.IsExists(id);
        }

        public static TreatyPricingPerLifeRetroVersionBo Find(int? id)
        {
            return FormBo(TreatyPricingPerLifeRetroVersion.Find(id));
        }

        public static TreatyPricingPerLifeRetroVersionBo FindLatestByTreatyPricingPerLifeRetroId(int treatyPricingPerLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingPerLifeRetroVersions.Where(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId).OrderByDescending(q => q.CreatedAt).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingPerLifeRetroVersionBo> GetByTreatyPricingPerLifeRetroId(int? treatyPricingPerLifeRetroId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingPerLifeRetroVersions
                    .Where(q => q.TreatyPricingPerLifeRetroId == treatyPricingPerLifeRetroId)
                    //.OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingPerLifeRetroVersionBo> GetByTreatyPricingPerLifeRetroVersion(int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingPerLifeRetroVersions.Where(q => q.Id == version).ToList());
            }
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionBo bo)
        {
            if (!TreatyPricingPerLifeRetroVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingPerLifeRetroVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingPerLifeRetroVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionBo bo)
        {
            TreatyPricingPerLifeRetroVersion entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionBo bo)
        {
            Result result = Result();

            TreatyPricingPerLifeRetroVersion entity = TreatyPricingPerLifeRetroVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPerLifeRetroVersionBo bo)
        {
            TreatyPricingPerLifeRetroVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPerLifeRetroVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPerLifeRetroVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
