using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeClaimService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeClaim)),
                Controller = ModuleBo.ModuleController.PerLifeClaim.ToString()
            };
        }

        public static Expression<Func<PerLifeClaim, PerLifeClaimBo>> Expression()
        {
            return entity => new PerLifeClaimBo
            {
                Id = entity.Id,
                CutOffId = entity.CutOffId,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                Status = entity.Status,
                SoaQuarter = entity.SoaQuarter,
                PersonInChargeId = entity.PersonInChargeId,
                ProcessingDate = entity.ProcessingDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeClaimBo FormBo(PerLifeClaim entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeClaimBo
            {
                Id = entity.Id,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                CutOffId = entity.CutOffId,
                CutOffBo = CutOffService.Find(entity.CutOffId),
                Status = entity.Status,
                SoaQuarter = entity.SoaQuarter,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                ProcessingDate = entity.ProcessingDate,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeClaimBo> FormBos(IList<PerLifeClaim> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeClaimBo> bos = new List<PerLifeClaimBo>() { };
            foreach (PerLifeClaim entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeClaim FormEntity(PerLifeClaimBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeClaim
            {
                Id = bo.Id,
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId,
                CutOffId = bo.CutOffId,
                Status = bo.Status,
                SoaQuarter = bo.SoaQuarter,
                PersonInChargeId = bo.PersonInChargeId,
                ProcessingDate = bo.ProcessingDate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeClaim.IsExists(id);
        }

        public static PerLifeClaimBo Find(int? id)
        {
            return FormBo(PerLifeClaim.Find(id));
        }

        public static PerLifeClaimBo FindBySoaQuarter(string soaQuarter)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeClaims
                    .Where(q => q.SoaQuarter == soaQuarter)
                    .FirstOrDefault());
            }
        }

        public static PerLifeClaimBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeClaims
                    .Where(q => q.Status == status)
                    .FirstOrDefault());
            }
        }

        public static IList<PerLifeClaimBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeClaims.ToList());
            }
        }

        public static int CountByCutOffId(int cutOffId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaims.Where(q => q.CutOffId == cutOffId).Count();
            }
        }

        public static int CountByFundsAccountingTypePickListDetailId(int fundsAccountingTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaims.Where(q => q.FundsAccountingTypePickListDetailId == fundsAccountingTypePickListDetailId).Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaims.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref PerLifeClaimBo bo)
        {
            if (!PerLifeClaim.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeClaimBo bo, ref TrailObject trail)
        {
            if (!PerLifeClaim.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeClaimBo bo)
        {
            PerLifeClaim entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeClaimBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimBo bo)
        {
            Result result = Result();

            PerLifeClaim entity = PerLifeClaim.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                entity.CutOffId = bo.CutOffId;
                entity.Status = bo.Status;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.ProcessingDate = bo.ProcessingDate;


                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeClaimBo bo)
        {
            PerLifeClaimDataService.DeleteAllByPerLifeClaimId(bo.Id);
            PerLifeClaim.Delete(bo.Id);
        }

        public static Result Delete(PerLifeClaimBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (bo.Status == PerLifeClaimBo.StatusSubmitForProcessing || 
                bo.Status == PerLifeClaimBo.StatusProcessing || 
                bo.Status == PerLifeClaimBo.StatusFinalised)
            {
                result.AddError("Unable to delete Processing or Finalised data");
            }

            if (result.Valid)
            {
                PerLifeClaimDataService.DeleteAllByPerLifeClaimId(bo.Id, ref trail);
                DataTrail dataTrail = PerLifeClaim.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
