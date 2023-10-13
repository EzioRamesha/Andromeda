using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
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

namespace Services.Sanctions
{
    public class SanctionCountryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionCountry)),
                Controller = ModuleBo.ModuleController.SanctionCountry.ToString()
            };
        }

        public static Expression<Func<SanctionCountry, SanctionCountryBo>> Expression()
        {
            return entity => new SanctionCountryBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Country = entity.Country,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionCountryBo FormBo(SanctionCountry entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionCountryBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Country = entity.Country,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionCountryBo> FormBos(IList<SanctionCountry> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionCountryBo> bos = new List<SanctionCountryBo>() { };
            foreach (SanctionCountry entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionCountry FormEntity(SanctionCountryBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionCountry
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                Country = bo.Country,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionCountry.IsExists(id);
        }

        public static SanctionCountryBo Find(int id)
        {
            return FormBo(SanctionCountry.Find(id));
        }

        public static IList<SanctionCountryBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionCountries.ToList());
            }
        }

        public static IList<SanctionCountryBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionCountries.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionCountries
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionCountryBo bo)
        {
            if (!SanctionCountry.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionCountryBo bo, ref TrailObject trail)
        {
            if (!SanctionCountry.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionCountryBo bo)
        {
            SanctionCountry entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionCountryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionCountryBo bo)
        {
            Result result = Result();

            SanctionCountry entity = SanctionCountry.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.Country = bo.Country;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionCountryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionCountryBo bo)
        {
            SanctionCountry.Delete(bo.Id);
        }

        public static Result Delete(SanctionCountryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionCountry.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionCountry.DeleteBySanctionId(sanctionId);
        }

        public static void DeleteBySanctionId(int sanctionId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionId(sanctionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            return SanctionCountry.DeleteBySanctionBatchId(sanctionBatchId);
        }

        public static void DeleteBySanctionBatchId(int sanctionBatchId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionBatchId(sanctionBatchId);
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
