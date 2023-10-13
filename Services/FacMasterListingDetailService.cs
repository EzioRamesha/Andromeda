using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class FacMasterListingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(FacMasterListingDetail)),
            };
        }

        public static FacMasterListingDetailBo FormBo(FacMasterListingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new FacMasterListingDetailBo
            {
                Id = entity.Id,
                FacMasterListingId = entity.FacMasterListingId,
                FacMasterListingBo = FacMasterListingService.Find(entity.FacMasterListingId),
                PolicyNumber = entity.PolicyNumber,
                BenefitCode = entity.BenefitCode,
                CedingBenefitTypeCode = entity.CedingBenefitTypeCode,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<FacMasterListingDetailBo> FormBos(IList<FacMasterListingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<FacMasterListingDetailBo> bos = new List<FacMasterListingDetailBo>() { };
            foreach (FacMasterListingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static FacMasterListingDetail FormEntity(FacMasterListingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new FacMasterListingDetail
            {
                Id = bo.Id,
                FacMasterListingId = bo.FacMasterListingId,
                PolicyNumber = bo.PolicyNumber?.Trim(),
                BenefitCode = bo.BenefitCode?.Trim(),
                CedingBenefitTypeCode = bo.CedingBenefitTypeCode?.Trim(),
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return FacMasterListingDetail.IsExists(id);
        }

        public static FacMasterListingDetailBo Find(int id)
        {
            return FormBo(FacMasterListingDetail.Find(id));
        }

        public static FacMasterListingDetailBo FindByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            return FormBo(FacMasterListingDetail.FindByParams(
                insuredName,
                policyNumber,
                benefitCode,
                cedingBenefitTypeCode,
                groupById
            ));
        }

        public static FacMasterListingDetailBo FindByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return FindByParams(
                riData.InsuredName,
                riData.PolicyNumber,
                riData.MlreBenefitCode,
                riData.CedingBenefitTypeCode,
                groupById
            );
        }

        public static int CountByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            return FacMasterListingDetail.CountByParams(
                insuredName,
                policyNumber,
                benefitCode,
                cedingBenefitTypeCode,
                groupById
            );
        }

        public static int CountByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return CountByParams(
                riData.InsuredName,
                riData.PolicyNumber,
                riData.MlreBenefitCode,
                riData.CedingBenefitTypeCode,
                groupById
            );
        }

        public static IList<FacMasterListingDetailBo> GetByParams(
            string insuredName,
            string policyNumber,
            string benefitCode,
            string cedingBenefitTypeCode,
            bool groupById = false
        )
        {
            return FormBos(FacMasterListingDetail.GetByParams(
                insuredName,
                policyNumber,
                benefitCode,
                cedingBenefitTypeCode,
                groupById
            ));
        }

        public static IList<FacMasterListingDetailBo> GetByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return GetByParams(
                riData.InsuredName,
                riData.PolicyNumber,
                riData.MlreBenefitCode,
                riData.CedingBenefitTypeCode,
                groupById
            );
        }

        public static Result Save(ref FacMasterListingDetailBo bo)
        {
            if (!FacMasterListingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref FacMasterListingDetailBo bo, ref TrailObject trail)
        {
            if (!FacMasterListingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref FacMasterListingDetailBo bo)
        {
            FacMasterListingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref FacMasterListingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref FacMasterListingDetailBo bo)
        {
            Result result = Result();

            FacMasterListingDetail entity = FacMasterListingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.FacMasterListingId = bo.FacMasterListingId;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.BenefitCode = bo.BenefitCode;
                entity.CedingBenefitTypeCode = bo.CedingBenefitTypeCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref FacMasterListingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(FacMasterListingDetailBo bo)
        {
            FacMasterListingDetail.Delete(bo.Id);
        }

        public static Result Delete(FacMasterListingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = FacMasterListingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByFacMasterListingId(int facMasterListingId)
        {
            return FacMasterListingDetail.DeleteByFacMasterListingId(facMasterListingId);
        }

        public static void DeleteByFacMasterListingId(int facMasterListingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByFacMasterListingId(facMasterListingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(FacMasterListingDetail)));
                }
            }
        }
    }
}
