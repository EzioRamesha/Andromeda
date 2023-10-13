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
    public class PerLifeRetroCountryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeRetroCountry)),
                Controller = ModuleBo.ModuleController.PerLifeRetroCountry.ToString()
            };
        }

        public static Expression<Func<PerLifeRetroCountry, PerLifeRetroCountryBo>> Expression()
        {
            return entity => new PerLifeRetroCountryBo
            {
                Id = entity.Id,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCode = entity.TerritoryOfIssueCodePickListDetail.Code,
                Country = entity.Country,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeRetroCountryBo FormBo(PerLifeRetroCountry entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeRetroCountryBo
            {
                Id = entity.Id,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = PickListDetailService.Find(entity.TerritoryOfIssueCodePickListDetailId),
                Country = entity.Country,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeRetroCountryBo> FormBos(IList<PerLifeRetroCountry> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeRetroCountryBo> bos = new List<PerLifeRetroCountryBo>() { };
            foreach (PerLifeRetroCountry entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeRetroCountry FormEntity(PerLifeRetroCountryBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeRetroCountry
            {
                Id = bo.Id,
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId,
                Country = bo.Country,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeRetroCountry.IsExists(id);
        }

        public static PerLifeRetroCountryBo Find(int? id)
        {
            return FormBo(PerLifeRetroCountry.Find(id));
        }

        public static PerLifeRetroCountryBo FindByTerritoryOfIssueCodePickListDetailId(int? territoryOfIssueCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroCountries.Where(q => q.TerritoryOfIssueCodePickListDetailId == territoryOfIssueCodePickListDetailId).FirstOrDefault());
            }
        }

        public static PerLifeRetroCountryBo FindByTerritoryOfIssueCode(string territoryOfIssueCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroCountries.Where(q => q.TerritoryOfIssueCodePickListDetail.Code == territoryOfIssueCode).FirstOrDefault());
            }
        }

        public static PerLifeRetroCountryBo FindByCountry(string country)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroCountries.Where(q => q.Country == country).FirstOrDefault());
            }
        }

        public static IList<PerLifeRetroCountryBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeRetroCountries.OrderBy(q => q.Country).ToList());
            }
        }

        public static Result Save(ref PerLifeRetroCountryBo bo)
        {
            if (!PerLifeRetroCountry.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeRetroCountryBo bo, ref TrailObject trail)
        {
            if (!PerLifeRetroCountry.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeRetroCountry perLifeRetroCountry)
        {
            return perLifeRetroCountry.IsDuplicate();
        }

        public static Result Create(ref PerLifeRetroCountryBo bo)
        {
            PerLifeRetroCountry entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddTakenError("Country", bo.Country);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeRetroCountryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroCountryBo bo)
        {
            Result result = Result();

            PerLifeRetroCountry entity = PerLifeRetroCountry.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddTakenError("Country", bo.Country);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
                entity.Country = bo.Country;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroCountryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeRetroCountryBo bo)
        {
            PerLifeRetroCountry.Delete(bo.Id);
        }

        public static Result Delete(PerLifeRetroCountryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (PerLifeDataCorrectionService.CountByPerLifeRetroCountryId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeRetroCountry.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
