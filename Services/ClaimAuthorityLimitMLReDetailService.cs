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
    public class ClaimAuthorityLimitMLReDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimAuthorityLimitMLReDetail)),
            };
        }

        public static ClaimAuthorityLimitMLReDetailBo FormBo(ClaimAuthorityLimitMLReDetail entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimAuthorityLimitMLReDetailBo
            {
                Id = entity.Id,
                ClaimAuthorityLimitMLReId = entity.ClaimAuthorityLimitMLReId,
                ClaimAuthorityLimitMLReBo = ClaimAuthorityLimitMLReService.Find(entity.ClaimAuthorityLimitMLReId),
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(entity.ClaimCodeId),
                ClaimAuthorityLimitValue = entity.ClaimAuthorityLimitValue,
                CreatedById = entity.CreatedById,

                ClaimAuthorityLimitValueStr = Util.DoubleToString(entity.ClaimAuthorityLimitValue, 2),
            };
        }

        public static IList<ClaimAuthorityLimitMLReDetailBo> FormBos(IList<ClaimAuthorityLimitMLReDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimAuthorityLimitMLReDetailBo> bos = new List<ClaimAuthorityLimitMLReDetailBo>() { };
            foreach (ClaimAuthorityLimitMLReDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimAuthorityLimitMLReDetail FormEntity(ClaimAuthorityLimitMLReDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimAuthorityLimitMLReDetail
            {
                Id = bo.Id,
                ClaimAuthorityLimitMLReId = bo.ClaimAuthorityLimitMLReId,
                ClaimCodeId = bo.ClaimCodeId,
                ClaimAuthorityLimitValue = bo.ClaimAuthorityLimitValue,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimAuthorityLimitMLReDetail.IsExists(id);
        }

        public static ClaimAuthorityLimitMLReDetailBo Find(int id)
        {
            return FormBo(ClaimAuthorityLimitMLReDetail.Find(id));
        }

        public static ClaimAuthorityLimitMLReDetailBo FindByClaimAuthorityLimitMLReIdClaimCode(int calMLReId, string claimCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId).Where(q => q.ClaimCode.Code == claimCode).FirstOrDefault());
            }
        }

        public static IList<ClaimAuthorityLimitMLReDetailBo> GetByClaimAuthorityLimitMLReId(int calMLReId)
        {
            return FormBos(ClaimAuthorityLimitMLReDetail.GetByClaimAuthorityLimitMLReId(calMLReId));
        }

        public static List<string> GetDistinctClaimCodeForClaimAuthorityLimitMLRe()
        {
            return ClaimAuthorityLimitMLReDetail.GetDistinctClaimCodeForClaimAuthorityLimitMLRe();
        }

        public static Dictionary<string, double> GetLimitsByClaimAuthorityLimitMlreId(int calMLReId)
        {
            using (var db = new AppDbContext())
            {
                return db.ClaimAuthorityLimitMLReDetails.Where(q => q.ClaimAuthorityLimitMLReId == calMLReId).ToDictionary(q => q.ClaimCode.Code, q => q.ClaimAuthorityLimitValue);
            }
        }

        public static List<int> GetAuthorisedUserByMultipleLimits(Dictionary<string, double> limits)
        {
            using (var db = new AppDbContext())
            {
                List<int> userIds = new List<int>();
                
                foreach (var limit in limits)
                {
                    List<int> currUserIds = db.ClaimAuthorityLimitMLReDetails
                        .Where(q => q.ClaimCode.Code == limit.Key && q.ClaimAuthorityLimitValue >= limit.Value)
                        .Select(q => q.ClaimAuthorityLimitMLRe.UserId)
                        .ToList();

                    if (userIds.Count == 0)
                        userIds = currUserIds;
                    else
                        userIds = userIds.Intersect(currUserIds).ToList();

                    if (userIds.Count == 0)
                        break;
                }

                List<int> allowOverwriteUserIds = db.ClaimAuthorityLimitMLRe.Where(q => q.IsAllowOverwriteApproval).Select(q => q.UserId).ToList();
                userIds = userIds.Union(allowOverwriteUserIds).ToList();

                return userIds;
            }
        }

        public static Result Save(ClaimAuthorityLimitMLReDetailBo bo)
        {
            if (!ClaimAuthorityLimitMLReDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(ClaimAuthorityLimitMLReDetailBo bo, ref TrailObject trail)
        {
            if (!ClaimAuthorityLimitMLReDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(ClaimAuthorityLimitMLReDetailBo bo)
        {
            ClaimAuthorityLimitMLReDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ClaimAuthorityLimitMLReDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ClaimAuthorityLimitMLReDetailBo bo)
        {
            Result result = Result();

            ClaimAuthorityLimitMLReDetail entity = ClaimAuthorityLimitMLReDetail.Find(bo.Id);
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
                entity.ClaimAuthorityLimitMLReId = bo.ClaimAuthorityLimitMLReId;
                entity.ClaimCodeId = bo.ClaimCodeId;
                entity.ClaimAuthorityLimitValue = bo.ClaimAuthorityLimitValue;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ClaimAuthorityLimitMLReDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimAuthorityLimitMLReDetailBo bo)
        {
            ClaimAuthorityLimitMLReDetail.Delete(bo.Id);
        }

        public static Result Delete(ClaimAuthorityLimitMLReDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimAuthorityLimitMLReDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimAuthorityLimitMLReId(int calMLReId)
        {
            return ClaimAuthorityLimitMLReDetail.DeleteAllByClaimAuthorityLimitMLReId(calMLReId);
        }

        public static void DeleteAllByBenefitId(int calMLReId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimAuthorityLimitMLReId(calMLReId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(BenefitDetail)));
                }
            }
        }

        public static Result DeleteByClaimAuthorityLimitMLReIdExcept(int calMLReId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimAuthorityLimitMLReDetail> calMLReDetails = ClaimAuthorityLimitMLReDetail.GetByClaimAuthorityLimitMLReIdExcept(calMLReId, saveIds);
            foreach (ClaimAuthorityLimitMLReDetail calMLReDetail in calMLReDetails)
            {
                DataTrail dataTrail = ClaimAuthorityLimitMLReDetail.Delete(calMLReDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Validate(IList<ClaimAuthorityLimitMLReDetailBo> bos)
        {
            Result result = Result();
            int row = 1;
            foreach (ClaimAuthorityLimitMLReDetailBo bo in bos)
            {
                List<string> errors = bo.Validate();

                if (bo.ClaimCodeId != 0)
                {
                    if (bos.Where(q => q.ClaimCodeId == bo.ClaimCodeId).Count() >1)
                    {
                        errors.Add("Duplicate found for limit");
                    }
                }

                foreach ( string error in errors)
                {
                    result.AddError(string.Format("{0} at row #{1}", error, row));
                }

                row++;
            }
            return result;
        }
    }
}
