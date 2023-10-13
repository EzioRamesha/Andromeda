using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingRateTableRateService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingRateTableRate)),
                Controller = ModuleBo.ModuleController.TreatyPricingRateTableRate.ToString()
            };
        }

        public static Expression<Func<TreatyPricingRateTableRate, TreatyPricingRateTableRateBo>> Expression()
        {
            return entity => new TreatyPricingRateTableRateBo
            {
                Id = entity.Id,
                TreatyPricingRateTableVersionId = entity.TreatyPricingRateTableVersionId,
                Age = entity.Age,
                MaleNonSmoker = entity.MaleNonSmoker,
                MaleSmoker = entity.MaleSmoker,
                FemaleNonSmoker = entity.FemaleNonSmoker,
                FemaleSmoker = entity.FemaleSmoker,
                Male = entity.Male,
                Female = entity.Female,
                Unisex = entity.Unisex,
                UnitRate = entity.UnitRate,
                OccupationClass = entity.OccupationClass,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingRateTableRateBo FormBo(TreatyPricingRateTableRate entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingRateTableRateBo
            {
                Id = entity.Id,
                TreatyPricingRateTableVersionId = entity.TreatyPricingRateTableVersionId,
                //TreatyPricingRateTableVersionBo = TreatyPricingRateTableVersionService.Find(entity.TreatyPricingRateTableVersionId),
                Age = entity.Age,
                MaleNonSmoker = entity.MaleNonSmoker,
                MaleSmoker = entity.MaleSmoker,
                FemaleNonSmoker = entity.FemaleNonSmoker,
                FemaleSmoker = entity.FemaleSmoker,
                Male = entity.Male,
                Female = entity.Female,
                Unisex = entity.Unisex,
                UnitRate = entity.UnitRate,
                OccupationClass = entity.OccupationClass,

                MaleNonSmokerStr = Util.DoubleToString(entity.MaleNonSmoker, 2),
                MaleSmokerStr = Util.DoubleToString(entity.MaleSmoker, 2),
                FemaleNonSmokerStr = Util.DoubleToString(entity.FemaleNonSmoker, 2),
                FemaleSmokerStr = Util.DoubleToString(entity.FemaleSmoker, 2),
                MaleStr = Util.DoubleToString(entity.Male, 2),
                FemaleStr = Util.DoubleToString(entity.Female, 2),
                UnisexStr = Util.DoubleToString(entity.Unisex, 2),
                UnitRateStr = Util.DoubleToString(entity.UnitRate, 2),
                OccupationClassStr = Util.DoubleToString(entity.OccupationClass, 2),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingRateTableRateBo> FormBos(IList<TreatyPricingRateTableRate> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingRateTableRateBo> bos = new List<TreatyPricingRateTableRateBo>() { };
            foreach (TreatyPricingRateTableRate entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingRateTableRate FormEntity(TreatyPricingRateTableRateBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingRateTableRate
            {
                Id = bo.Id,
                TreatyPricingRateTableVersionId = bo.TreatyPricingRateTableVersionId,
                Age = bo.Age,
                MaleNonSmoker = bo.MaleNonSmoker,
                MaleSmoker = bo.MaleSmoker,
                FemaleNonSmoker = bo.FemaleNonSmoker,
                FemaleSmoker = bo.FemaleSmoker,
                Male = bo.Male,
                Female = bo.Female,
                Unisex = bo.Unisex,
                UnitRate = bo.UnitRate,
                OccupationClass = bo.OccupationClass,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingRateTableRate.IsExists(id);
        }

        public static TreatyPricingRateTableRateBo Find(int? id)
        {
            return FormBo(TreatyPricingRateTableRate.Find(id));
        }

        public static IList<TreatyPricingRateTableRateBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableRates.ToList());
            }
        }

        public static IList<TreatyPricingRateTableRateBo> GetByTreatyPricingRateTableVersionId(int? treatyPricingRateTableVersionId)
        {
            if (!treatyPricingRateTableVersionId.HasValue)
                return null;

            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableRates.Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId).ToList());
            }
        }

        public static IList<TreatyPricingRateTableRateBo> GetByTreatyPricingRateTableVersionIdSa(int? treatyPricingRateTableVersionId)
        {
            if (!treatyPricingRateTableVersionId.HasValue)
                return null;

            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableRates
                    .Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId)
                    .Where(q =>
                        q.MaleNonSmoker.HasValue ||
                        q.MaleSmoker.HasValue || 
                        q.FemaleNonSmoker.HasValue || 
                        q.FemaleSmoker.HasValue || 
                        q.Male.HasValue || 
                        q.Female.HasValue || 
                        q.Unisex.HasValue)
                    .ToList());
            }
        }

        public static IList<TreatyPricingRateTableRateBo> GetByTreatyPricingRateTableVersionIdPa(int? treatyPricingRateTableVersionId)
        {
            if (!treatyPricingRateTableVersionId.HasValue)
                return null;

            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableRates
                    .Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId)
                    .Where(q =>
                        q.UnitRate.HasValue ||
                        q.OccupationClass.HasValue)
                    .ToList());
            }
        }

        public static IList<TreatyPricingRateTableRateBo> GetByTreatyPricingRateTableIdVersion(int id, int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingRateTableRates.Where(q => q.TreatyPricingRateTableVersion.TreatyPricingRateTableId == id).Where(q => q.TreatyPricingRateTableVersion.Version == version).ToList());
            }
        }

        public static string GetJsonByTreatyPricingRateTableVersionId(int treatyPricingRateTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingRateTableVersionId(treatyPricingRateTableVersionId);

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByTreatyPricingRateTableVersionId(int treatyPricingRateTableVersionId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingRateTableRates.Where(q => q.TreatyPricingRateTableVersionId == treatyPricingRateTableVersionId).Count();
            }
        }

        public static void CopyBulk(int nextVersionId, int previousVersionId, int authUserId)
        {
            string script = "INSERT INTO [dbo].[TreatyPricingRateTableRates]";
            string insertFields = string.Format("({0})", string.Join(",", TreatyPricingRateTableRateBo.InsertFields()));
            string queryFields = string.Format("{0}", string.Join(",", TreatyPricingRateTableRateBo.QueryFields()));
            string select = string.Format("SELECT {0},{1},GETDATE(),GETDATE(),{2},{2} FROM [dbo].[TreatyPricingRateTableRates] WHERE TreatyPricingRateTableVersionId = {3}", nextVersionId, queryFields, authUserId, previousVersionId);
            string query = string.Format("{0}\n{1}\n{2};", script, insertFields, select);

            using (var db = new AppDbContext())
            {
                db.Database.ExecuteSqlCommand(query);
                db.SaveChanges();
            }
        }

        public static Result Save(ref TreatyPricingRateTableRateBo bo)
        {
            if (!TreatyPricingRateTableRate.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingRateTableRateBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingRateTableRate.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingRateTableRateBo bo)
        {
            TreatyPricingRateTableRate entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingRateTableRateBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableRateBo bo)
        {
            Result result = Result();

            TreatyPricingRateTableRate entity = TreatyPricingRateTableRate.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingRateTableVersionId = bo.TreatyPricingRateTableVersionId;
                entity.Age = bo.Age;
                entity.MaleNonSmoker = bo.MaleNonSmoker;
                entity.MaleSmoker = bo.MaleSmoker;
                entity.FemaleNonSmoker = bo.FemaleNonSmoker;
                entity.FemaleSmoker = bo.FemaleSmoker;
                entity.Male = bo.Male;
                entity.Female = bo.Female;
                entity.Unisex = bo.Unisex;
                entity.UnitRate = bo.UnitRate;
                entity.OccupationClass = bo.OccupationClass;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingRateTableRateBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingRateTableRateBo bo)
        {
            TreatyPricingRateTableRate.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingRateTableRateBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingRateTableRate.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
