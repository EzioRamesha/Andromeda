using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClaimCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimCodeMappingDetail)),
            };
        }

        public static ClaimCodeMappingDetailBo FormBo(ClaimCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimCodeMappingDetailBo
            {
                Id = entity.Id,
                ClaimCodeMappingId = entity.ClaimCodeMappingId,
                ClaimCodeMappingBo = ClaimCodeMappingService.Find(entity.ClaimCodeMappingId),
                Combination = entity.Combination,
                MlreEventCode = entity.MlreEventCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<ClaimCodeMappingDetailBo> FormBos(IList<ClaimCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimCodeMappingDetailBo> bos = new List<ClaimCodeMappingDetailBo>() { };
            foreach (ClaimCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimCodeMappingDetail FormEntity(ClaimCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimCodeMappingDetail
            {
                Id = bo.Id,
                ClaimCodeMappingId = bo.ClaimCodeMappingId,
                Combination = bo.Combination,
                MlreEventCode = bo.MlreEventCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimCodeMappingDetail.IsExists(id);
        }

        public static ClaimCodeMappingDetailBo Find(int id)
        {
            return FormBo(ClaimCodeMappingDetail.Find(id));
        }

        public static ClaimCodeMappingDetailBo FindByCombination(
            string combination,
            int? claimCodeMappingId = null
        )
        {
            return FormBo(ClaimCodeMappingDetail.FindByCombination(
                combination,
                claimCodeMappingId
            ));
        }

        public static ClaimCodeMappingDetailBo FindByCombination(string combination)
        {
            return FindByCombination(
                combination
            );
        }

        public static ClaimCodeMappingDetailBo FindByCombination(string combination, ClaimCodeMappingBo claimCodeMappingBo)
        {
            return FindByCombination(
                combination,
                claimCodeMappingBo.Id
            );
        }

        public static ClaimCodeMappingDetailBo FindByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            return FormBo(ClaimCodeMappingDetail.FindByParams(
                mlreEventCode,
                mlreBenefitCode,
                groupById
            ));
        }

        // Reminder: To update first param to MLRE Event Code
        public static ClaimCodeMappingDetailBo FindByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return FindByParams(
                riData.MlreBenefitCode, //MLRE Event Code
                riData.MlreBenefitCode,
                groupById
            );
        }

        public static int CountByCombination(
            string combination,
            int? claimCodeMappingId = null
        )
        {
            return ClaimCodeMappingDetail.CountByCombination(
                combination,
                claimCodeMappingId
            );
        }

        public static int CountByCombination(string combination, ClaimCodeMappingBo claimCodeMappingBo)
        {
            return CountByCombination(
                combination,
                claimCodeMappingBo.Id
            );
        }

        public static int CountByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            return ClaimCodeMappingDetail.CountByParams(
                mlreEventCode,
                mlreBenefitCode,
                groupById
            );
        }

        // Reminder: To update first param to MLRE Event Code
        public static int CountByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return CountByParams(
                riData.MlreBenefitCode, //MLRE Event Code
                riData.MlreBenefitCode,
                groupById
            );
        }

        public static IList<ClaimCodeMappingDetailBo> GetByParams(
            string mlreEventCode = null,
            string mlreBenefitCode = null,
            bool groupById = false
        )
        {
            return FormBos(ClaimCodeMappingDetail.GetByParams(
                mlreEventCode,
                mlreBenefitCode,
                groupById
            ));
        }

        // Reminder: To update first param to MLRE Event Code
        public static IList<ClaimCodeMappingDetailBo> GetByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return GetByParams(
                riData.MlreBenefitCode, //MLRE Event Code
                riData.MlreBenefitCode,
                groupById
            );
        }

        public static Result Save(ref ClaimCodeMappingDetailBo bo)
        {
            if (!ClaimCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!ClaimCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimCodeMappingDetailBo bo)
        {
            ClaimCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimCodeMappingDetailBo bo)
        {
            Result result = Result();

            ClaimCodeMappingDetail entity = ClaimCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimCodeMappingId = bo.ClaimCodeMappingId;
                entity.Combination = bo.Combination;
                entity.MlreEventCode = bo.MlreEventCode;
                entity.MlreBenefitCode = bo.MlreBenefitCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimCodeMappingDetailBo bo)
        {
            ClaimCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(ClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByClaimCodeMappingId(int claimCodeMappingId)
        {
            return ClaimCodeMappingDetail.DeleteByClaimCodeMappingId(claimCodeMappingId);
        }

        public static void DeleteByClaimCodeMappingId(int claimCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByClaimCodeMappingId(claimCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimCodeMappingDetail)));
                }
            }
        }
    }
}
