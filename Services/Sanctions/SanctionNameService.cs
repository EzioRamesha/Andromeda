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
    public class SanctionNameService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionName)),
                Controller = ModuleBo.ModuleController.SanctionName.ToString()
            };
        }

        public static Expression<Func<SanctionName, SanctionNameBo>> Expression()
        {
            return entity => new SanctionNameBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionNameBo FormBo(SanctionName entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionNameBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionNameBo> FormBos(IList<SanctionName> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionNameBo> bos = new List<SanctionNameBo>() { };
            foreach (SanctionName entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionName FormEntity(SanctionNameBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionName
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionName.IsExists(id);
        }

        public static SanctionNameBo Find(int id)
        {
            return FormBo(SanctionName.Find(id));
        }

        public static IList<SanctionNameBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionNames.ToList());
            }
        }

        public static IList<SanctionNameBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionNames.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static Dictionary<int, string> GetNameBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionNames.Where(q => q.SanctionId == sanctionId).ToDictionary(q => q.Id, q => q.Name.ToUpper());
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionNames
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionNameBo bo)
        {
            if (!SanctionName.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionNameBo bo, ref TrailObject trail)
        {
            if (!SanctionName.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionNameBo bo)
        {
            SanctionName entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionNameBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionNameBo bo)
        {
            Result result = Result();

            SanctionName entity = SanctionName.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionNameBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionNameBo bo)
        {
            SanctionName.Delete(bo.Id);
        }

        public static Result Delete(SanctionNameBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionName.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionName.DeleteBySanctionId(sanctionId);
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
            return SanctionName.DeleteBySanctionBatchId(sanctionBatchId);
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
