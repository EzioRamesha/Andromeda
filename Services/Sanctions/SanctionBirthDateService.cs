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
    public class SanctionBirthDateService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionBirthDate)),
                Controller = ModuleBo.ModuleController.SanctionBirthDate.ToString()
            };
        }

        public static Expression<Func<SanctionBirthDate, SanctionBirthDateBo>> Expression()
        {
            return entity => new SanctionBirthDateBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                DateOfBirth = entity.DateOfBirth,
                YearOfBirth = entity.YearOfBirth,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionBirthDateBo FormBo(SanctionBirthDate entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionBirthDateBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                DateOfBirth = entity.DateOfBirth,
                DateOfBirthStr = entity.DateOfBirth?.ToString(Util.GetDateFormat()),
                YearOfBirth = entity.YearOfBirth,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionBirthDateBo> FormBos(IList<SanctionBirthDate> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionBirthDateBo> bos = new List<SanctionBirthDateBo>() { };
            foreach (SanctionBirthDate entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionBirthDate FormEntity(SanctionBirthDateBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionBirthDate
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                DateOfBirth = bo.DateOfBirth,
                YearOfBirth = bo.YearOfBirth,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionBirthDate.IsExists(id);
        }

        public static SanctionBirthDateBo Find(int id)
        {
            return FormBo(SanctionBirthDate.Find(id));
        }

        public static IList<SanctionBirthDateBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionBirthDates.ToList());
            }
        }

        public static IList<SanctionBirthDateBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionBirthDates.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static List<int> GetSanctionIdByBirthDateSource(DateTime dateOfBirth)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBirthDates.Where(q => q.DateOfBirth == dateOfBirth).Select(q => q.SanctionId).ToList();
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionBirthDates
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionBirthDateBo bo)
        {
            if (!SanctionBirthDate.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionBirthDateBo bo, ref TrailObject trail)
        {
            if (!SanctionBirthDate.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionBirthDateBo bo)
        {
            SanctionBirthDate entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionBirthDateBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionBirthDateBo bo)
        {
            Result result = Result();

            SanctionBirthDate entity = SanctionBirthDate.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.DateOfBirth = bo.DateOfBirth;
                entity.YearOfBirth = bo.YearOfBirth;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionBirthDateBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionBirthDateBo bo)
        {
            SanctionBirthDate.Delete(bo.Id);
        }

        public static Result Delete(SanctionBirthDateBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionBirthDate.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionBirthDate.DeleteBySanctionId(sanctionId);
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
            return SanctionBirthDate.DeleteBySanctionBatchId(sanctionBatchId);
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
