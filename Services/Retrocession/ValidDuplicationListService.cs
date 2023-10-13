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
    public class ValidDuplicationListService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ValidDuplicationList)),
                Controller = ModuleBo.ModuleController.ValidDuplicationList.ToString()
            };
        }

        public static Expression<Func<ValidDuplicationList, ValidDuplicationListBo>> Expression()
        {
            return entity => new ValidDuplicationListBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                CedantPlanCode = entity.CedantPlanCode,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ValidDuplicationListBo FormBo(ValidDuplicationList entity = null)
        {
            if (entity == null)
                return null;
            return new ValidDuplicationListBo
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
                ReinsuranceEffectiveDateStr = entity.ReinsuranceEffectiveDate.HasValue ? entity.ReinsuranceEffectiveDate.Value.ToString(Util.GetDateFormat()) : "",
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),
                TransactionTypePickListDetailId = entity.TransactionTypePickListDetailId,
                TransactionTypePickListDetailBo = PickListDetailService.Find(entity.TransactionTypePickListDetailId),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ValidDuplicationListBo> FormBos(IList<ValidDuplicationList> entities = null)
        {
            if (entities == null)
                return null;
            IList<ValidDuplicationListBo> bos = new List<ValidDuplicationListBo>() { };
            foreach (ValidDuplicationList entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ValidDuplicationList FormEntity(ValidDuplicationListBo bo = null)
        {
            if (bo == null)
                return null;
            return new ValidDuplicationList
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


                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ValidDuplicationList.IsExists(id);
        }

        public static bool IsExistsByParam(
            string treatyCode,
            string cedingPlanCode,
            string insuredName,
            DateTime? insuredDateOfBirth,
            string insuredGenderCode,
            string mlreBenefitCode,
            string cedingBenefitRiskCode,
            string cedingBenefitTypeCode,
            DateTime? reinsuranceEffectiveDate,
            string fundsAccountingType,
            string reinsBasisCode,
            string transactionType
        )
        {
            using (var db = new AppDbContext())
            {
                return db.ValidDuplicationLists
                    .Where(q => q.TreatyCode.Code == treatyCode)
                    .Where(q => q.CedantPlanCode == cedingPlanCode)
                    .Where(q => q.InsuredName == insuredName)
                    .Where(q => q.InsuredDateOfBirth == insuredDateOfBirth)
                    .Where(q => q.InsuredGenderCodePickListDetail.Code == insuredGenderCode)
                    .Where(q => q.MLReBenefitCode.Code == mlreBenefitCode)
                    .Where(q => q.CedingBenefitRiskCode == cedingBenefitRiskCode)
                    .Where(q => q.CedingBenefitTypeCode == cedingBenefitTypeCode)
                    .Where(q => q.ReinsuranceEffectiveDate == reinsuranceEffectiveDate)
                    .Where(q => q.FundsAccountingTypePickListDetail.Code == fundsAccountingType)
                    .Where(q => q.ReinsBasisCodePickListDetail.Code == reinsBasisCode)
                    .Where(q => q.TransactionTypePickListDetail.Code == transactionType)
                    .Any();
            }
        }

        public static ValidDuplicationListBo Find(int? id)
        {
            return FormBo(ValidDuplicationList.Find(id));
        }

        public static IList<ValidDuplicationListBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.ValidDuplicationLists.OrderBy(q => q.TreatyCodeId).ToList());
            }
        }

        public static Result Save(ref ValidDuplicationListBo bo)
        {
            if (!ValidDuplicationList.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ValidDuplicationListBo bo, ref TrailObject trail)
        {
            if (!ValidDuplicationList.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ValidDuplicationListBo bo)
        {
            ValidDuplicationList entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ValidDuplicationListBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ValidDuplicationListBo bo)
        {
            Result result = Result();

            ValidDuplicationList entity = ValidDuplicationList.Find(bo.Id);
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

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ValidDuplicationListBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ValidDuplicationListBo bo)
        {
            ValidDuplicationList.Delete(bo.Id);
        }

        public static Result Delete(ValidDuplicationListBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = ValidDuplicationList.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
