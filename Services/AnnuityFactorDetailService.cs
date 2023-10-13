using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
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
    public class AnnuityFactorDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AnnuityFactorDetail)),
                Controller = ModuleBo.ModuleController.AnnuityFactorDetail.ToString()
            };
        }

        public static Expression<Func<AnnuityFactorDetail, AnnuityFactorDetailBo>> Expression()
        {
            return entity => new AnnuityFactorDetailBo
            {
                Id = entity.Id,
                AnnuityFactorId = entity.AnnuityFactorId,
                PolicyTermRemain = entity.PolicyTermRemain,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCode = entity.InsuredGenderCodePickListDetail.Code,
                InsuredTobaccoUsePickListDetailId = entity.InsuredTobaccoUsePickListDetailId,
                InsuredTobaccoUse = entity.InsuredTobaccoUsePickListDetail.Code,
                InsuredAttainedAge = entity.InsuredAttainedAge,
                PolicyTerm = entity.PolicyTerm,
                AnnuityFactorValue = entity.AnnuityFactorValue,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static AnnuityFactorDetailBo FormBo(AnnuityFactorDetail entity = null)
        {
            if (entity == null)
                return null;
            return new AnnuityFactorDetailBo
            {
                Id = entity.Id,
                AnnuityFactorId = entity.AnnuityFactorId,
                PolicyTermRemain = entity.PolicyTermRemain,
                PolicyTermRemainStr = entity.PolicyTermRemain.ToString(),
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                InsuredTobaccoUsePickListDetailId = entity.InsuredTobaccoUsePickListDetailId,
                InsuredTobaccoUsePickListDetailBo = PickListDetailService.Find(entity.InsuredTobaccoUsePickListDetailId),
                InsuredAttainedAge = entity.InsuredAttainedAge,
                PolicyTerm = entity.PolicyTerm,
                PolicyTermStr = Util.DoubleToString(entity.PolicyTerm),
                AnnuityFactorValue = entity.AnnuityFactorValue,
                AnnuityFactorValueStr = Util.DoubleToString(entity.AnnuityFactorValue),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<AnnuityFactorDetailBo> FormBos(IList<AnnuityFactorDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<AnnuityFactorDetailBo> bos = new List<AnnuityFactorDetailBo>() { };
            foreach (AnnuityFactorDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AnnuityFactorDetail FormEntity(AnnuityFactorDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new AnnuityFactorDetail
            {
                Id = bo.Id,
                AnnuityFactorId = bo.AnnuityFactorId,
                PolicyTermRemain = bo.PolicyTermRemain,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                InsuredTobaccoUsePickListDetailId = bo.InsuredTobaccoUsePickListDetailId,
                InsuredAttainedAge = bo.InsuredAttainedAge,
                PolicyTerm = bo.PolicyTerm,
                AnnuityFactorValue = bo.AnnuityFactorValue,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return AnnuityFactorDetail.IsExists(id);
        }

        public static AnnuityFactorDetailBo Find(int id)
        {
            return FormBo(AnnuityFactorDetail.Find(id));
        }

        public static int CountByAnnuityFactorIdByParams(int annuityFactorId, RiDataBo riData, CacheService cache)
        {
            return CountByAnnuityFactorIdByParams(
                annuityFactorId, 
                riData.PolicyTermRemain,
                cache.GetInsuredGenderCodesId(riData),
                cache.GetInsuredTobaccoUseId(riData),
                riData.InsuredAttainedAge,
                riData.PolicyTerm
            );
        }

        public static int CountByAnnuityFactorIdByParams(int annuityFactorId, double? policyTermRemain, int? insuredGenderCodeId, int? insuredTobaccoUseId, int? insuredAttainedAge, double? policyTerm)
        {
            return AnnuityFactorDetail.CountByAnnuityFactorIdByParams(annuityFactorId, policyTermRemain, insuredGenderCodeId, insuredTobaccoUseId, insuredAttainedAge, policyTerm);
        }

        public static AnnuityFactorDetailBo FindByAnnuityFactorIdByParams(int annuityFactorId, RiDataBo riData, CacheService cache)
        {
            return FindByAnnuityFactorIdByParams(
                annuityFactorId,
                riData.PolicyTermRemain,
                cache.GetInsuredGenderCodesId(riData),
                cache.GetInsuredTobaccoUseId(riData),
                riData.InsuredAttainedAge,
                riData.PolicyTerm
            );
        }

        public static AnnuityFactorDetailBo FindByAnnuityFactorIdByParams(int annuityFactorId, double? policyTermRemain, int? insuredGenderCodeId, int? insuredTobaccoUseId, int? insuredAttainedAge, double? policyTerm)
        {
            return FormBo(AnnuityFactorDetail.FindByAnnuityFactorIdByParams(annuityFactorId, policyTermRemain, insuredGenderCodeId, insuredTobaccoUseId, insuredAttainedAge, policyTerm));
        }

        public static IList<AnnuityFactorDetailBo> GetByAnnuityFactorId(int annuityFactorId)
        {
            return FormBos(AnnuityFactorDetail.GetByAnnuityFactorId(annuityFactorId));
        }

        public static Result Save(ref AnnuityFactorDetailBo bo)
        {
            if (!AnnuityFactorDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref AnnuityFactorDetailBo bo, ref TrailObject trail)
        {
            if (!AnnuityFactorDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AnnuityFactorDetailBo bo)
        {
            AnnuityFactorDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AnnuityFactorDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorDetailBo bo)
        {
            Result result = Result();

            AnnuityFactorDetail entity = AnnuityFactorDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.AnnuityFactorId = bo.AnnuityFactorId;
                entity.PolicyTermRemain = bo.PolicyTermRemain;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.InsuredTobaccoUsePickListDetailId = bo.InsuredTobaccoUsePickListDetailId;
                entity.InsuredAttainedAge = bo.InsuredAttainedAge;
                entity.PolicyTerm = bo.PolicyTerm;
                entity.AnnuityFactorValue = bo.AnnuityFactorValue;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AnnuityFactorDetailBo bo)
        {
            AnnuityFactorDetail.Delete(bo.Id);
        }

        public static Result Delete(AnnuityFactorDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = AnnuityFactorDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByAnnuityFactorIdExcept(int annuityFactorId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<AnnuityFactorDetail> annuityFactorDetails = AnnuityFactorDetail.GetByAnnuityFactorIdExcept(annuityFactorId, saveIds);
            foreach (AnnuityFactorDetail annuityFactorDetail in annuityFactorDetails)
            {
                DataTrail dataTrail = AnnuityFactorDetail.Delete(annuityFactorDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByAnnuityFactorId(int annuityFactorId)
        {
            return AnnuityFactorDetail.DeleteByAnnuityFactorId(annuityFactorId);
        }

        public static void DeleteByAnnuityFactorId(int annuityFactorId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByAnnuityFactorId(annuityFactorId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AnnuityFactorDetail)));
                }
            }
        }
    }
}
