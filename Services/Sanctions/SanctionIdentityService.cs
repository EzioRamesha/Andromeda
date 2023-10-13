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
    public class SanctionIdentityService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionIdentity)),
                Controller = ModuleBo.ModuleController.SanctionIdentity.ToString()
            };
        }

        public static Expression<Func<SanctionIdentity, SanctionIdentityBo>> Expression()
        {
            return entity => new SanctionIdentityBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                IdType = entity.IdType,
                IdNumber = entity.IdNumber,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionIdentityBo FormBo(SanctionIdentity entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionIdentityBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                IdType = entity.IdType,
                IdNumber = entity.IdNumber,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionIdentityBo> FormBos(IList<SanctionIdentity> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionIdentityBo> bos = new List<SanctionIdentityBo>() { };
            foreach (SanctionIdentity entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionIdentity FormEntity(SanctionIdentityBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionIdentity
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                IdType = bo.IdType,
                IdNumber = bo.IdNumber,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionIdentity.IsExists(id);
        }

        public static SanctionIdentityBo Find(int id)
        {
            return FormBo(SanctionIdentity.Find(id));
        }

        public static IList<SanctionIdentityBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionIdentities.ToList());
            }
        }

        public static IList<SanctionIdentityBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionIdentities.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static List<int> GetSanctionIdByIdNumber(string idNumber)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionIdentities.Where(q => q.IdNumber.Trim() == idNumber.Trim()).Select(q => q.SanctionId).ToList();
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionIdentities
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionIdentityBo bo)
        {
            if (!SanctionIdentity.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionIdentityBo bo, ref TrailObject trail)
        {
            if (!SanctionIdentity.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionIdentityBo bo)
        {
            SanctionIdentity entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionIdentityBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionIdentityBo bo)
        {
            Result result = Result();

            SanctionIdentity entity = SanctionIdentity.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.IdType = bo.IdType;
                entity.IdNumber = bo.IdNumber;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionIdentityBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionIdentityBo bo)
        {
            SanctionIdentity.Delete(bo.Id);
        }

        public static Result Delete(SanctionIdentityBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionIdentity.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionIdentity.DeleteBySanctionId(sanctionId);
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
            return SanctionIdentity.DeleteBySanctionBatchId(sanctionBatchId);
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
