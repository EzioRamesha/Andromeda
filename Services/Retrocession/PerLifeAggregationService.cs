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
    public class PerLifeAggregationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregation)),
                Controller = ModuleBo.ModuleController.PerLifeAggregation.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregation, PerLifeAggregationBo>> Expression()
        {
            return entity => new PerLifeAggregationBo
            {
                Id = entity.Id,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                CutOffId = entity.CutOffId,
                SoaQuarter = entity.SoaQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,
                PersonInChargeId = entity.PersonInChargeId,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationBo FormBo(PerLifeAggregation entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationBo
            {
                Id = entity.Id,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                CutOffId = entity.CutOffId,
                CutOffBo = CutOffService.Find(entity.CutOffId),
                SoaQuarter = entity.SoaQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationBo> FormBos(IList<PerLifeAggregation> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationBo> bos = new List<PerLifeAggregationBo>() { };
            foreach (PerLifeAggregation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregation FormEntity(PerLifeAggregationBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregation
            {
                Id = bo.Id,
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId,
                CutOffId = bo.CutOffId,
                SoaQuarter = bo.SoaQuarter,
                ProcessingDate = bo.ProcessingDate,
                Status = bo.Status,
                PersonInChargeId = bo.PersonInChargeId,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregation.IsExists(id);
        }

        public static PerLifeAggregationBo Find(int? id)
        {
            return FormBo(PerLifeAggregation.Find(id));
        }

        public static PerLifeAggregationBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.GetPerLifeAggregations()
                    .Where(q => q.Status == status)
                    .FirstOrDefault());
            }
        }

        public static IList<PerLifeAggregationBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.GetPerLifeAggregations().ToList());
            }
        }

        public static int CountByCutOffId(int cutOffId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetPerLifeAggregations().Where(q => q.CutOffId == cutOffId).Count();
            }
        }

        public static int CountByFundsAccountingTypePickListDetailId(int fundsAccountingTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.GetPerLifeAggregations().Where(q => q.FundsAccountingTypePickListDetailId == fundsAccountingTypePickListDetailId).Count();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.GetPerLifeAggregations().Where(q => q.Status == status).Count();
            }
        }

        public static bool IsDataInUse(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeAggregationDetailData
                    .Where(q => q.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.PerLifeAggregationId == id)
                    .Any(q => db.PerLifeClaimData.Select(c => c.PerLifeAggregationDetailDataId).Contains(q.Id) ||
                        db.PerLifeSoaData.Select(c => c.PerLifeAggregationDetailDataId).Contains(q.Id) 
                    );
            }
        }

        public static Result Save(ref PerLifeAggregationBo bo)
        {
            if (!PerLifeAggregation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeAggregation perLifeAggregation)
        {
            return perLifeAggregation.IsDuplicate();
        }

        public static Result Create(ref PerLifeAggregationBo bo)
        {
            PerLifeAggregation entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Per Life Aggregation found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationBo bo)
        {
            Result result = Result();

            PerLifeAggregation entity = PerLifeAggregation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Per Life Aggregation found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                entity.CutOffId = bo.CutOffId;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.ProcessingDate = bo.ProcessingDate;
                entity.Status = bo.Status;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationBo bo)
        {
            PerLifeAggregationDetailService.DeleteByPerLifeAggregationId(bo.Id);
            PerLifeAggregation.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (bo.Status == PerLifeAggregationBo.StatusSubmitForProcessing || 
                bo.Status == PerLifeAggregationBo.StatusProcessing || 
                bo.Status == PerLifeAggregationBo.StatusFinalised)
            {
                result.AddError("Unable to delete Processing or Finalised data");
            }

            if (result.Valid)
            {
                PerLifeAggregationDetailService.DeleteByPerLifeAggregationId(bo.Id, ref trail);
                DataTrail dataTrail = PerLifeAggregation.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
