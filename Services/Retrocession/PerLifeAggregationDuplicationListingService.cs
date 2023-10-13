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
    public class PerLifeAggregationDuplicationListingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeAggregationDuplicationListing)),
                Controller = ModuleBo.ModuleController.PerLifeAggregationDuplicationListing.ToString()
            };
        }

        public static Expression<Func<PerLifeAggregationDuplicationListing, PerLifeAggregationDuplicationListingBo>> Expression()
        {
            return entity => new PerLifeAggregationDuplicationListingBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                InsuredName = entity.InsuredName,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                ReinsuranceEffectiveDate = entity.ReinsuranceEffectiveDate,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                CedantPlanCode = entity.CedantPlanCode,
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                TransactionTypePickListDetailId = entity.TransactionTypePickListDetailId,
                ProceedToAggregate = entity.ProceedToAggregate,
                DateUpdated = entity.DateUpdated,
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                Remarks = entity.Remarks,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeAggregationDuplicationListingBo FormBo(PerLifeAggregationDuplicationListing entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeAggregationDuplicationListingBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                CedantPlanCode = entity.CedantPlanCode,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredDateOfBirthStr = entity.InsuredDateOfBirth.HasValue ? entity.InsuredDateOfBirth.Value.ToString(Util.GetDateFormat()) : "",
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                MLReBenefitCodeId = entity.MLReBenefitCodeId,
                MLReBenefitCodeBo = BenefitService.Find(entity.MLReBenefitCodeId),
                CedingBenefitRiskCode = entity.CedingBenefitRiskCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = entity.ReinsuranceEffectiveDate,
                ReinsuranceEffecitveDateStr = entity.ReinsuranceEffectiveDate.HasValue ? entity.ReinsuranceEffectiveDate.Value.ToString(Util.GetDateFormat()) : "",
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),
                TransactionTypePickListDetailId = entity.TransactionTypePickListDetailId,
                TransactionTypePickListDetailBo = PickListDetailService.Find(entity.TransactionTypePickListDetailId),
                ProceedToAggregate = entity.ProceedToAggregate,
                DateUpdated = entity.DateUpdated,
                DateUpdatedStr = entity.DateUpdated.HasValue ? entity.DateUpdated.Value.ToString(Util.GetDateFormat()) : "",
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                ExceptionStatusPickListDetailBo = PickListDetailService.Find(entity.ExceptionStatusPickListDetailId),
                Remarks = entity.Remarks,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeAggregationDuplicationListingBo> FormBos(IList<PerLifeAggregationDuplicationListing> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeAggregationDuplicationListingBo> bos = new List<PerLifeAggregationDuplicationListingBo>() { };
            foreach (PerLifeAggregationDuplicationListing entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeAggregationDuplicationListing FormEntity(PerLifeAggregationDuplicationListingBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeAggregationDuplicationListing
            {
                Id = bo.Id,
                TreatyCodeId = bo.TreatyCodeId,
                CedantPlanCode = bo.CedantPlanCode,
                InsuredName = bo.InsuredName,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                PolicyNumber = bo.PolicyNumber,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                MLReBenefitCodeId = bo.MLReBenefitCodeId,
                CedingBenefitRiskCode = bo.CedingBenefitRiskCode,
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode,
                ReinsuranceEffectiveDate = bo.ReinsuranceEffectiveDate,
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId,
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId,
                TransactionTypePickListDetailId = bo.TransactionTypePickListDetailId,
                ProceedToAggregate = bo.ProceedToAggregate,
                DateUpdated = bo.DateUpdated,
                ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId,
                Remarks = bo.Remarks,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeAggregationDuplicationListing.IsExists(id);
        }

        public static PerLifeAggregationDuplicationListingBo Find(int? id)
        {
            return FormBo(PerLifeAggregationDuplicationListing.Find(id));
        }

        public static IList<PerLifeAggregationDuplicationListingBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeAggregationDuplicationListings.OrderBy(q => q.TreatyCodeId).ToList());
            }
        }

        public static Result Save(ref PerLifeAggregationDuplicationListingBo bo)
        {
            if (!PerLifeAggregationDuplicationListing.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeAggregationDuplicationListingBo bo, ref TrailObject trail)
        {
            if (!PerLifeAggregationDuplicationListing.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeAggregationDuplicationListingBo bo)
        {
            PerLifeAggregationDuplicationListing entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeAggregationDuplicationListingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDuplicationListingBo bo)
        {
            Result result = Result();

            PerLifeAggregationDuplicationListing entity = PerLifeAggregationDuplicationListing.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.CedantPlanCode = bo.CedantPlanCode;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.MLReBenefitCodeId = bo.MLReBenefitCodeId;
                entity.CedingBenefitRiskCode = bo.CedingBenefitRiskCode;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                entity.ReinsuranceEffectiveDate = bo.ReinsuranceEffectiveDate;
                entity.FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                entity.ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                entity.TransactionTypePickListDetailId = bo.TransactionTypePickListDetailId;
                entity.ProceedToAggregate = bo.ProceedToAggregate;
                entity.DateUpdated = bo.DateUpdated;
                entity.ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId;
                entity.Remarks = bo.Remarks;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeAggregationDuplicationListingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeAggregationDuplicationListingBo bo)
        {
            PerLifeAggregationDuplicationListing.Delete(bo.Id);
        }

        public static Result Delete(PerLifeAggregationDuplicationListingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeAggregationDuplicationListing.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
