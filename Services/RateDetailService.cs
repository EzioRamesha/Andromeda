using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
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

namespace Services
{
    public class RateDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RateDetail)),
                Controller = ModuleBo.ModuleController.RateDetail.ToString()
            };
        }

        public static Expression<Func<RateDetail, RateDetailBo>> Expression()
        {
            return entity => new RateDetailBo
            {
                Id = entity.Id,
                RateId = entity.RateId,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCode = entity.InsuredGenderCodePickListDetail.Code,
                CedingTobaccoUsePickListDetailId = entity.CedingTobaccoUsePickListDetailId,
                CedingTobaccoUse = entity.CedingTobaccoUsePickListDetail.Code,
                CedingOccupationCodePickListDetailId = entity.CedingOccupationCodePickListDetailId,
                CedingOccupationCode = entity.CedingOccupationCodePickListDetail.Code,
                AttainedAge = entity.AttainedAge,
                IssueAge = entity.IssueAge,
                PolicyTerm = entity.PolicyTerm,
                PolicyTermRemain = entity.PolicyTermRemain,
                RateValue = entity.RateValue,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RateDetailBo FormBo(RateDetail entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            RateDetailBo bo = new RateDetailBo
            {
                Id = entity.Id,
                RateId = entity.RateId,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                CedingTobaccoUsePickListDetailId = entity.CedingTobaccoUsePickListDetailId,
                CedingOccupationCodePickListDetailId = entity.CedingOccupationCodePickListDetailId,
                AttainedAge = entity.AttainedAge,
                IssueAge = entity.IssueAge,
                PolicyTerm = entity.PolicyTerm,
                PolicyTermRemain = entity.PolicyTermRemain,
                RateValue = entity.RateValue,
                RateValueStr = Util.DoubleToString(entity.RateValue),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId);
                bo.CedingTobaccoUsePickListDetailBo = PickListDetailService.Find(entity.CedingTobaccoUsePickListDetailId);
                bo.CedingOccupationCodePickListDetailBo = PickListDetailService.Find(entity.CedingOccupationCodePickListDetailId);
            }

            bo.InsuredGenderCode = bo.InsuredGenderCodePickListDetailBo?.ToString();
            bo.CedingTobaccoUse = bo.CedingTobaccoUsePickListDetailBo?.ToString();
            bo.CedingOccupationCode = bo.CedingOccupationCodePickListDetailBo?.ToString();

            return bo;
        }

        public static IList<RateDetailBo> FormBos(IList<RateDetail> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<RateDetailBo> bos = new List<RateDetailBo>() { };
            foreach (RateDetail entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static RateDetail FormEntity(RateDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RateDetail
            {
                Id = bo.Id,
                RateId = bo.RateId,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                CedingTobaccoUsePickListDetailId = bo.CedingTobaccoUsePickListDetailId,
                CedingOccupationCodePickListDetailId = bo.CedingOccupationCodePickListDetailId,
                AttainedAge = bo.AttainedAge,
                IssueAge = bo.IssueAge,
                PolicyTerm = bo.PolicyTerm,
                PolicyTermRemain = bo.PolicyTermRemain,
                RateValue = bo.RateValue,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RateDetail.IsExists(id);
        }

        public static RateDetailBo Find(int id)
        {
            return FormBo(RateDetail.Find(id));
        }

        public static int CountByRateIdByParams(
            int rateId,
            RiDataBo riData,
            CacheService cache,
            bool isCurrentAge = false
        )
        {
            if (isCurrentAge)
            {
                return RateDetail.CountByParams(
                    rateId,
                    cache.GetInsuredGenderCodesId(riData),
                    cache.GetInsuredTobaccoUseId(riData),
                    cache.GetInsuredOccupationCodeId(riData),
                    riData.MlreInsuredAttainedAgeAtCurrentMonth,
                    riData.MlrePolicyIssueAge,
                    riData.PolicyTerm,
                    riData.PolicyTermRemain
                );
            }
            else
            {
                return RateDetail.CountByParams(
                    rateId,
                    cache.GetInsuredGenderCodesId(riData),
                    cache.GetInsuredTobaccoUseId(riData),
                    cache.GetInsuredOccupationCodeId(riData),
                    riData.MlreInsuredAttainedAgeAtPreviousMonth,
                    riData.MlrePolicyIssueAge,
                    riData.PolicyTerm,
                    riData.PolicyTermRemain
                );
            }
        }

        public static RateDetailBo FindByRateIdByParams(
            int rateId,
            RiDataBo riData,
            CacheService pickListCache,
            bool isCurrentAge = false
        )
        {
            if (isCurrentAge)
            {
                return FormBo(RateDetail.FindByParams(
                    rateId,
                    pickListCache.GetInsuredGenderCodesId(riData),
                    pickListCache.GetInsuredTobaccoUseId(riData),
                    pickListCache.GetInsuredOccupationCodeId(riData),
                    riData.MlreInsuredAttainedAgeAtCurrentMonth,
                    riData.MlrePolicyIssueAge,
                    riData.PolicyTerm,
                    riData.PolicyTermRemain
                ));
            }
            else
            {
                return FormBo(RateDetail.FindByParams(
                       rateId,
                       pickListCache.GetInsuredGenderCodesId(riData),
                       pickListCache.GetInsuredTobaccoUseId(riData),
                       pickListCache.GetInsuredOccupationCodeId(riData),
                       riData.MlreInsuredAttainedAgeAtPreviousMonth,
                       riData.MlrePolicyIssueAge,
                       riData.PolicyTerm,
                       riData.PolicyTermRemain
                   ));
            }
        }

        public static IList<RateDetailBo> GetByRateId(int rateId, bool foreign = true)
        {
            return FormBos(RateDetail.GetByRateId(rateId), foreign);
        }

        public static IList<RateDetailBo> GetNextByRateId(int rateId, int skip, int take)
        {
            return FormBos(RateDetail.GetNextByRateId(rateId, skip, take));
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            return RateDetail.CountByInsuredGenderCodePickListDetailId(insuredGenderCodePickListDetailId);
        }

        public static int CountByCedingTobaccoUsePickListDetailId(int cedingTobaccoUsePickListDetailId)
        {
            return RateDetail.CountByCedingTobaccoUsePickListDetailId(cedingTobaccoUsePickListDetailId);
        }

        public static int CountByCedingOccupationCodePickListDetailId(int cedingOccupationCodePickListDetailId)
        {
            return RateDetail.CountByCedingOccupationCodePickListDetailId(cedingOccupationCodePickListDetailId);
        }

        public static int CountByRateId(int rateId)
        {
            return RateDetail.CountByRateId(rateId);
        }

        public static Result Save(ref RateDetailBo bo)
        {
            if (!RateDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RateDetailBo bo, ref TrailObject trail)
        {
            if (!RateDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RateDetailBo bo)
        {
            RateDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RateDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RateDetailBo bo)
        {
            Result result = Result();

            RateDetail entity = RateDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RateId = bo.RateId;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.CedingTobaccoUsePickListDetailId = bo.CedingTobaccoUsePickListDetailId;
                entity.CedingOccupationCodePickListDetailId = bo.CedingOccupationCodePickListDetailId;
                entity.AttainedAge = bo.AttainedAge;
                entity.IssueAge = bo.IssueAge;
                entity.PolicyTerm = bo.PolicyTerm;
                entity.PolicyTermRemain = bo.PolicyTermRemain;
                entity.RateValue = bo.RateValue;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RateDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RateDetailBo bo)
        {
            RateDetail.Delete(bo.Id);
        }

        public static Result Delete(RateDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RateDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByRateDetailIdExcept(int rateId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<RateDetail> rateDetails = RateDetail.GetByRateIdExcept(rateId, saveIds);
            foreach (RateDetail rateDetail in rateDetails)
            {
                DataTrail dataTrail = RateDetail.Delete(rateDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByRateId(int rateId)
        {
            return RateDetail.DeleteByRateId(rateId);
        }

        public static void DeleteByRateId(int rateId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRateId(rateId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RateDetail)));
                }
            }
        }

        public static Result ValidateMapping(RateDetailBo bo, int rateId)
        {
            Result result = new Result();
            var list = new Dictionary<string, List<string>> { };

            var detailBos = GetByRateId(rateId, false);
            if (detailBos.Any())
            {
                var exist = detailBos.Where(q => q.InsuredGenderCodePickListDetailId == bo.InsuredGenderCodePickListDetailId)
                    .Where(q => q.CedingTobaccoUsePickListDetailId == bo.CedingTobaccoUsePickListDetailId)
                    .Where(q => q.CedingOccupationCodePickListDetailId == bo.CedingOccupationCodePickListDetailId)
                    .Where(q => q.AttainedAge == bo.AttainedAge)
                    .Where(q => q.IssueAge == bo.IssueAge)
                    .Where(q => q.PolicyTerm == bo.PolicyTerm)
                    .Where(q => q.PolicyTermRemain == bo.PolicyTermRemain);

                if (bo.Id != 0)
                    exist = exist.Where(q => q.Id != bo.Id);

                if (exist.Any())
                {
                    result.AddError("Duplicate Data Found");
                }
            }

            return result;
        }
    }
}
