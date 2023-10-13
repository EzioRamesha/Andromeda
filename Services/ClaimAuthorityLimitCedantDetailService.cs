using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ClaimAuthorityLimitCedantDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimAuthorityLimitCedantDetail)),
            };
        }

        public static ClaimAuthorityLimitCedantDetailBo FormBo(ClaimAuthorityLimitCedantDetail entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimAuthorityLimitCedantDetailBo
            {
                Id = entity.Id,
                ClaimAuthorityLimitCedantId = entity.ClaimAuthorityLimitCedantId,
                ClaimAuthorityLimitCedantBo = ClaimAuthorityLimitCedantService.Find(entity.ClaimAuthorityLimitCedantId),
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(entity.ClaimCodeId),
                Type = entity.Type,
                ClaimAuthorityLimitValue = entity.ClaimAuthorityLimitValue,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(entity.FundsAccountingTypePickListDetailId),
                EffectiveDate = entity.EffectiveDate,
                CreatedById = entity.CreatedById,

                ClaimAuthorityLimitValueStr = Util.DoubleToString(entity.ClaimAuthorityLimitValue, 2),
                EffectiveDateStr = entity.EffectiveDate.ToString(Util.GetDateFormat()),
            };
        }

        public static IList<ClaimAuthorityLimitCedantDetailBo> FormBos(IList<ClaimAuthorityLimitCedantDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimAuthorityLimitCedantDetailBo> bos = new List<ClaimAuthorityLimitCedantDetailBo>() { };
            foreach (ClaimAuthorityLimitCedantDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimAuthorityLimitCedantDetail FormEntity(ClaimAuthorityLimitCedantDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimAuthorityLimitCedantDetail
            {
                Id = bo.Id,
                ClaimAuthorityLimitCedantId = bo.ClaimAuthorityLimitCedantId,
                ClaimCodeId = bo.ClaimCodeId,
                Type = bo.Type,
                ClaimAuthorityLimitValue = bo.ClaimAuthorityLimitValue,
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId,
                EffectiveDate = bo.EffectiveDate,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimAuthorityLimitCedantDetail.IsExists(id);
        }

        public static ClaimAuthorityLimitCedantDetailBo Find(int id)
        {
            return FormBo(ClaimAuthorityLimitCedantDetail.Find(id));
        }

        public static ClaimAuthorityLimitCedantDetailBo FindByParams(bool isContestable, string cedingCompany, string claimCode, string fundsAccountingTypeCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ClaimAuthorityLimitCedantDetails
                    .Where(q => q.ClaimAuthorityLimitCedant.Cedant.Code == cedingCompany)
                    .Where(q => q.ClaimCode.Code == claimCode);

                if (!string.IsNullOrEmpty(fundsAccountingTypeCode))
                {
                    query = query.Where(q => q.FundsAccountingTypePickListDetail.Code == fundsAccountingTypeCode);
                }

                if (isContestable)
                {
                    query = query.Where(q => q.Type == ClaimAuthorityLimitCedantDetailBo.TypeBoth || q.Type == ClaimAuthorityLimitCedantDetailBo.TypeContestable);
                }
                else
                {
                    query = query.Where(q => q.Type == ClaimAuthorityLimitCedantDetailBo.TypeBoth || q.Type == ClaimAuthorityLimitCedantDetailBo.TypeNonContestable);
                }

                return FormBo(query.FirstOrDefault());
            }
        }

        public static IList<ClaimAuthorityLimitCedantDetailBo> GetByClaimAuthorityLimitCedantId(int calCedantId)
        {
            return FormBos(ClaimAuthorityLimitCedantDetail.GetByClaimAuthorityLimitCedantId(calCedantId));
        }

        public static List<string> GetDistinctClaimCodeForClaimAuthorityLimitCedant()
        {
            return ClaimAuthorityLimitCedantDetail.GetDistinctClaimCodeForClaimAuthorityLimitCedant();
        }

        public static Result Save(ClaimAuthorityLimitCedantDetailBo bo)
        {
            if (!ClaimAuthorityLimitCedantDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(ClaimAuthorityLimitCedantDetailBo bo, ref TrailObject trail)
        {
            if (!ClaimAuthorityLimitCedantDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(ClaimAuthorityLimitCedantDetailBo bo)
        {
            ClaimAuthorityLimitCedantDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ClaimAuthorityLimitCedantDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ClaimAuthorityLimitCedantDetailBo bo)
        {
            Result result = Result();

            ClaimAuthorityLimitCedantDetail entity = ClaimAuthorityLimitCedantDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            /*
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }
            */

            if (result.Valid)
            {
                entity.ClaimAuthorityLimitCedantId = bo.ClaimAuthorityLimitCedantId;
                entity.ClaimCodeId = bo.ClaimCodeId;
                entity.Type = bo.Type;
                entity.ClaimAuthorityLimitValue = bo.ClaimAuthorityLimitValue;
                entity.FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                entity.EffectiveDate = bo.EffectiveDate;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ClaimAuthorityLimitCedantDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimAuthorityLimitCedantDetailBo bo)
        {
            ClaimAuthorityLimitCedantDetail.Delete(bo.Id);
        }

        public static Result Delete(ClaimAuthorityLimitCedantDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimAuthorityLimitCedantDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimAuthorityLimitCedantId(int calCedantId)
        {
            return ClaimAuthorityLimitCedantDetail.DeleteAllByClaimAuthorityLimitCedantId(calCedantId);
        }

        public static void DeleteAllByBenefitId(int calCedantId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimAuthorityLimitCedantId(calCedantId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(BenefitDetail)));
                }
            }
        }

        public static Result DeleteByClaimAuthorityLimitCedantIdExcept(int calCedantId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimAuthorityLimitCedantDetail> calCedantDetails = ClaimAuthorityLimitCedantDetail.GetByClaimAuthorityLimitCedantIdExcept(calCedantId, saveIds);
            foreach (ClaimAuthorityLimitCedantDetail calCedantDetail in calCedantDetails)
            {
                DataTrail dataTrail = ClaimAuthorityLimitCedantDetail.Delete(calCedantDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Validate(IList<ClaimAuthorityLimitCedantDetailBo> bos)
        {
            Result result = Result();
            int row = 1;
            foreach (ClaimAuthorityLimitCedantDetailBo bo in bos)
            {
                List<string> errors = bo.Validate();

                if (bo.ClaimCodeId != 0 && bo.Type != 0)
                {
                    if (bos.Where(q => q.ClaimCodeId == bo.ClaimCodeId && q.Type == bo.Type && q.EffectiveDateStr == bo.EffectiveDateStr).Count() > 1)
                    {
                        errors.Add("Duplicate found for limit");
                    }
                }

                foreach (string error in errors)
                {
                    result.AddError(string.Format("{0} at row #{1}", error, row));
                }


                row++;
            }

            var bothTypeLimits = bos.Where(q => q.Type == ClaimAuthorityLimitCedantDetailBo.TypeBoth).ToList();
            foreach (var bo in bothTypeLimits)
            {
                if (bos.Where(q => q.ClaimCodeId == bo.ClaimCodeId && q.Type != bo.Type && q.EffectiveDateStr == bo.EffectiveDateStr).Count() > 0)
                {
                    string code = (ClaimCodeService.Find(bo.ClaimCodeId))?.Code;
                    result.AddError(string.Format("Invalid type for claim code {0}", code));
                }
            }

            return result;
        }
    }
}
