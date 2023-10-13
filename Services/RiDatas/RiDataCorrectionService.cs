using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.RiDatas
{
    public class RiDataCorrectionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataCorrection)),
                Controller = ModuleBo.ModuleController.RiDataCorrection.ToString()
            };
        }

        public static Expression<Func<RiDataCorrection, RiDataCorrectionBo>> Expression()
        {
            return entity => new RiDataCorrectionBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode.Code,
                PolicyNumber = entity.PolicyNumber,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCode = entity.InsuredGenderCodePickListDetail.Code,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredName = entity.InsuredName,
                CampaignCode = entity.CampaignCode,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCode = entity.ReinsBasisCodePickListDetail.Code,
                ApLoading = entity.ApLoading,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RiDataCorrectionBo FormBo(RiDataCorrection entity = null)
        {
            if (entity == null)
                return null;
            return new RiDataCorrectionBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                PolicyNumber = entity.PolicyNumber,
                InsuredRegisterNo = entity.InsuredRegisterNo,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredName = entity.InsuredName,
                CampaignCode = entity.CampaignCode,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),
                ApLoading = entity.ApLoading,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataCorrectionBo> FormBos(IList<RiDataCorrection> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataCorrectionBo> bos = new List<RiDataCorrectionBo>() { };
            foreach (RiDataCorrection entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataCorrection FormEntity(RiDataCorrectionBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataCorrection
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyCodeId = bo.TreatyCodeId,
                PolicyNumber = bo.PolicyNumber?.Trim(),
                InsuredRegisterNo = bo.InsuredRegisterNo?.Trim(),
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                InsuredName = bo.InsuredName?.Trim(),
                CampaignCode = bo.CampaignCode?.Trim(),
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId,
                ApLoading = bo.ApLoading,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDataCorrection.IsExists(id);
        }

        public static RiDataCorrectionBo Find(int id)
        {
            return FormBo(RiDataCorrection.Find(id));
        }

        public static RiDataCorrectionBo FindByCedantIdTreatyCodeIdPolicyRegNo(int cedantId, string policyNumber, string registerNo = null, int? treadyCodeId = null)
        {
            return FormBo(RiDataCorrection.FindByCedantIdTreatyCodeIdPolicyRegNo(cedantId, policyNumber, registerNo, treadyCodeId));
        }

        public static RiDataCorrectionBo FindByCedantIdTreatyCodePolicyRegNo(int cedantId, string policyNumber, string registerNo = null, string treatyCode = null)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(136, "RiDataCorrectionService");

                return connectionStrategy.Execute(() =>
                {
                    var query = db.RiDataCorrections
                        .Where(q => q.CedantId == cedantId)
                        .Where(q => q.PolicyNumber.Trim() == policyNumber.Trim())
                        .Where(q => q.InsuredRegisterNo.Trim() == registerNo.Trim());

                    //if (!string.IsNullOrEmpty(registerNo))
                    //    query.Where(q => q.InsuredRegisterNo == registerNo);
                    if (!string.IsNullOrEmpty(treatyCode))
                    {
                        connectionStrategy.Reset(148);
                        query.Where(q => q.TreatyCodeId.HasValue && q.TreatyCode.Code.Trim() == treatyCode.Trim());
                    }
                    else
                    {
                        connectionStrategy.Reset(152);
                        query.Where(q => !q.TreatyCodeId.HasValue);
                    }

                    connectionStrategy.Reset(160);
                    return FormBo(query.FirstOrDefault());
                });
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return RiDataCorrection.CountByTreatyCodeId(treatyCodeId);
        }

        public static int CountByInsuredGenderCodePickListDetailId(int countByInsuredGenderCodePickListDetailId)
        {
            return RiDataCorrection.CountByInsuredGenderCodePickListDetailId(countByInsuredGenderCodePickListDetailId);
        }

        public static int CountByReinsBasisCodePickListDetailId(int countByReinsBasisCodePickListDetailId)
        {
            return RiDataCorrection.CountByReinsBasisCodePickListDetailId(countByReinsBasisCodePickListDetailId);
        }

        public static Result Save(ref RiDataCorrectionBo bo)
        {
            if (!RiDataCorrection.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RiDataCorrectionBo bo, ref TrailObject trail)
        {
            if (!RiDataCorrection.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataCorrectionBo bo)
        {
            RiDataCorrection entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataCorrectionBo bo)
        {
            Result result = Result();

            RiDataCorrection entity = RiDataCorrection.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredRegisterNo = bo.InsuredRegisterNo;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.InsuredName = bo.InsuredName;
                entity.CampaignCode = bo.CampaignCode;
                entity.ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                entity.ApLoading = bo.ApLoading;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataCorrectionBo bo)
        {
            RiDataCorrection.Delete(bo.Id);
        }

        public static Result Delete(RiDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RiDataCorrection.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMappedResult(RiDataCorrectionBo bo)
        {
            Result result = new Result();

            if (bo.InsuredGenderCodePickListDetailId == null
                && !bo.InsuredDateOfBirth.HasValue
                && string.IsNullOrEmpty(bo.InsuredName)
                && string.IsNullOrEmpty(bo.CampaignCode)
                && bo.ReinsBasisCodePickListDetailId == null
                && !bo.ApLoading.HasValue)
            {
                result.AddError("Please enter at least one mapped values");
            }
            return result;
        }
    }
}
