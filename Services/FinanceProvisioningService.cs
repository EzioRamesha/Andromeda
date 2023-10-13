using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class FinanceProvisioningService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(FinanceProvisioning)),
                Controller = ModuleBo.ModuleController.FinanceProvisioning.ToString()
            };
        }

        public static Expression<Func<FinanceProvisioning, FinanceProvisioningBo>> Expression()
        {
            return entity => new FinanceProvisioningBo
            {
                Id = entity.Id,
                Date = entity.Date,
                Status = entity.Status,
                ClaimsProvisionRecord = entity.ClaimsProvisionRecord,
                DrProvisionRecord = entity.DrProvisionRecord,
                ClaimsProvisionAmount = entity.ClaimsProvisionAmount,
                DrProvisionAmount = entity.DrProvisionAmount,
                ProvisionAt = entity.ProvisionAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static FinanceProvisioningBo FormBo(FinanceProvisioning entity = null)
        {
            if (entity == null)
                return null;
            return new FinanceProvisioningBo
            {
                Id = entity.Id,
                Date = entity.Date,
                Status = entity.Status,
                ClaimsProvisionRecord = entity.ClaimsProvisionRecord,
                DrProvisionRecord = entity.DrProvisionRecord,
                ClaimsProvisionAmount = entity.ClaimsProvisionAmount,
                DrProvisionAmount = entity.DrProvisionAmount,
                ProvisionAt = entity.ProvisionAt,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<FinanceProvisioningBo> FormBos(IList<FinanceProvisioning> entities = null)
        {
            if (entities == null)
                return null;
            IList<FinanceProvisioningBo> bos = new List<FinanceProvisioningBo>() { };
            foreach (FinanceProvisioning entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static FinanceProvisioning FormEntity(FinanceProvisioningBo bo = null)
        {
            if (bo == null)
                return null;
            return new FinanceProvisioning
            {
                Id = bo.Id,
                Date = bo.Date,
                Status = bo.Status,
                ClaimsProvisionRecord = bo.ClaimsProvisionRecord,
                DrProvisionRecord = bo.DrProvisionRecord,
                ClaimsProvisionAmount = bo.ClaimsProvisionAmount,
                DrProvisionAmount = bo.DrProvisionAmount,
                ProvisionAt = bo.ProvisionAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return FinanceProvisioning.IsExists(id);
        }

        public static FinanceProvisioningBo Find(int? id)
        {
            return FormBo(FinanceProvisioning.Find(id));
        }

        public static FinanceProvisioningBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.FinanceProvisionings.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.FinanceProvisionings.Where(q => q.Status == status).Count();
            }
        }

        public static IList<FinanceProvisioningBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.FinanceProvisionings.ToList());
            }
        }

        public static Result Save(ref FinanceProvisioningBo bo)
        {
            if (!FinanceProvisioning.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref FinanceProvisioningBo bo, ref TrailObject trail)
        {
            if (!FinanceProvisioning.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref FinanceProvisioningBo bo)
        {
            FinanceProvisioning entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref FinanceProvisioningBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void SumAmount(ref FinanceProvisioningBo bo)
        {
            int id = bo.Id;
            using (var db = new AppDbContext())
            {
                var claimQuery = db.FinanceProvisioningTransactions.Where(q => q.FinanceProvisioningId == id);
                var drQuery = db.DirectRetroProvisioningTransactions.Where(q => q.FinanceProvisioningId == id);

                bo.ClaimsProvisionAmount = claimQuery.Any() ? claimQuery.Sum(q => q.ClaimRecoveryAmount) : 0;
                bo.DrProvisionAmount = drQuery.Any() ? drQuery.Sum(q => q.RetroRecovery) : 0;
            }
        }

        public static Result Update(ref FinanceProvisioningBo bo)
        {
            Result result = Result();

            FinanceProvisioning entity = FinanceProvisioning.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Date = bo.Date;
                entity.Status = bo.Status;
                entity.ClaimsProvisionRecord = bo.ClaimsProvisionRecord;
                entity.DrProvisionRecord = bo.DrProvisionRecord;
                entity.ClaimsProvisionAmount = bo.ClaimsProvisionAmount;
                entity.DrProvisionAmount = bo.DrProvisionAmount;
                entity.ProvisionAt = bo.ProvisionAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref FinanceProvisioningBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(FinanceProvisioningBo bo)
        {
            FinanceProvisioning.Delete(bo.Id);
        }

        public static Result Delete(FinanceProvisioningBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = FinanceProvisioning.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
