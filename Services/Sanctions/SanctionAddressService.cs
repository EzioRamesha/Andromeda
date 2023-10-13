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

namespace Services.Sanctions
{
    public class SanctionAddressService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionAddress)),
                Controller = ModuleBo.ModuleController.SanctionAddress.ToString()
            };
        }

        public static Expression<Func<SanctionAddress, SanctionAddressBo>> Expression()
        {
            return entity => new SanctionAddressBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Address = entity.Address,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionAddressBo FormBo(SanctionAddress entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionAddressBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Address = entity.Address,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionAddressBo> FormBos(IList<SanctionAddress> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionAddressBo> bos = new List<SanctionAddressBo>() { };
            foreach (SanctionAddress entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionAddress FormEntity(SanctionAddressBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionAddress
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                Address = bo.Address,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionAddress.IsExists(id);
        }

        public static SanctionAddressBo Find(int id)
        {
            return FormBo(SanctionAddress.Find(id));
        }

        public static IList<SanctionAddressBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionAddresses.ToList());
            }
        }

        public static IList<SanctionAddressBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionAddresses.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionAddresses
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionAddressBo bo)
        {
            if (!SanctionAddress.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionAddressBo bo, ref TrailObject trail)
        {
            if (!SanctionAddress.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionAddressBo bo)
        {
            SanctionAddress entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionAddressBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionAddressBo bo)
        {
            Result result = Result();

            SanctionAddress entity = SanctionAddress.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.Address = bo.Address;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionAddressBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionAddressBo bo)
        {
            SanctionAddress.Delete(bo.Id);
        }

        public static Result Delete(SanctionAddressBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionAddress.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionAddress.DeleteBySanctionId(sanctionId);
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
            return SanctionAddress.DeleteBySanctionBatchId(sanctionBatchId);
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
