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

namespace Services.TreatyPricing
{
    public class TreatyPricingPerLifeRetroService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetro)),
                Controller = ModuleBo.ModuleController.TreatyPricingPerLifeRetro.ToString()
            };
        }

        public static TreatyPricingPerLifeRetroBo FormBo(TreatyPricingPerLifeRetro entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingPerLifeRetroBo
            {
                Id = entity.Id,
                Code = entity.Code,
                RetroPartyId = entity.RetroPartyId,
                RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId),
                Type = entity.Type,
                RetrocessionaireShare = entity.RetrocessionaireShare,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                TreatyPricingPerLifeRetroVersionBos = TreatyPricingPerLifeRetroVersionService.GetByTreatyPricingPerLifeRetroId(entity.Id),
                RetrocessionaireShareStr = Util.DoubleToString(entity.RetrocessionaireShare),
            };
        }

        public static IList<TreatyPricingPerLifeRetroBo> FormBos(IList<TreatyPricingPerLifeRetro> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroBo> bos = new List<TreatyPricingPerLifeRetroBo>() { };
            foreach (TreatyPricingPerLifeRetro entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetro FormEntity(TreatyPricingPerLifeRetroBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetro
            {
                Id = bo.Id,
                Code = bo.Code,
                RetroPartyId = bo.RetroPartyId,
                Type = bo.Type,
                RetrocessionaireShare = bo.RetrocessionaireShare,
                Description = bo.Description,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingPerLifeRetro.IsExists(id);
        }

        public static TreatyPricingPerLifeRetroBo Find(int? id)
        {
            return FormBo(TreatyPricingPerLifeRetro.Find(id));
        }

        public static TreatyPricingPerLifeRetroBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingPerLifeRetro.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingPerLifeRetroBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingPerLifeRetro.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<string> GetCodes()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingPerLifeRetro.Select(q => q.Code).ToList();
            }
        }

        public static Result Create(ref TreatyPricingPerLifeRetroBo bo)
        {
            TreatyPricingPerLifeRetro entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPerLifeRetroBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroBo bo)
        {
            Result result = Result();

            TreatyPricingPerLifeRetro entity = TreatyPricingPerLifeRetro.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Code = bo.Code;
                entity.RetroPartyId = bo.RetroPartyId;
                entity.RetrocessionaireShare = bo.RetrocessionaireShare;
                entity.Type = bo.Type;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPerLifeRetroBo bo)
        {
            TreatyPricingPerLifeRetro.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPerLifeRetroBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPerLifeRetro.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
