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
    public class Mfrs17CellMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17CellMappingDetail)),
            };
        }

        public static Mfrs17CellMappingDetailBo FormBo(Mfrs17CellMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17CellMappingDetailBo
            {
                Id = entity.Id,
                Mfrs17CellMappingId = entity.Mfrs17CellMappingId,
                Mfrs17CellMappingBo = Mfrs17CellMappingService.Find(entity.Mfrs17CellMappingId),
                Combination = entity.Combination,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,

                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                TreatyCode = entity.TreatyCode,
            };
        }

        public static IList<Mfrs17CellMappingDetailBo> FormBos(IList<Mfrs17CellMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17CellMappingDetailBo> bos = new List<Mfrs17CellMappingDetailBo>() { };
            foreach (Mfrs17CellMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Mfrs17CellMappingDetail FormEntity(Mfrs17CellMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17CellMappingDetail
            {
                Id = bo.Id,
                Mfrs17CellMappingId = bo.Mfrs17CellMappingId,
                Combination = bo.Combination?.Trim(),
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,

                CedingPlanCode = bo.CedingPlanCode?.Trim(),
                BenefitCode = bo.BenefitCode?.Trim(),
                TreatyCode = bo.TreatyCode?.Trim(),
            };
        }

        public static bool IsExists(int id)
        {
            return Mfrs17CellMappingDetail.IsExists(id);
        }

        public static Mfrs17CellMappingDetailBo Find(int id)
        {
            return FormBo(Mfrs17CellMappingDetail.Find(id));
        }

        public static Mfrs17CellMappingDetailBo FindByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? ReinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            return FormBo(Mfrs17CellMappingDetail.FindByCombination(
                combination,
                reinsEffDatePolStartDate,
                ReinsEffDatePolEndDate,
                mfrs17CellMappingId
            ));
        }

        public static Mfrs17CellMappingDetailBo FindByCombination(string combination, Mfrs17CellMappingBo mfrs17CellMappingBo)
        {
            return FindByCombination(
                combination,
                mfrs17CellMappingBo.ReinsEffDatePolStartDate,
                mfrs17CellMappingBo.ReinsEffDatePolEndDate,
                mfrs17CellMappingBo.Id
            );
        }

        public static Mfrs17CellMappingDetailBo FindByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            return FormBo(Mfrs17CellMappingDetail.FindByParams(
                treatyCode,
                profitComm,
                reinsBasisCodeId,
                cedingPlanCode,
                mlreBenefitCode,
                reinsEffDatePol,
                rateTable,
                groupById
            ));
        }

        public static Mfrs17CellMappingDetailBo FindByParams(
            RiDataBo riData,
            CacheService cache,
            bool groupById = false
        )
        {
            return FindByParams(
                riData.TreatyCode,
                riData.ProfitComm,
                cache.GetReinsBasisCodeId(riData),
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.RateTable,
                groupById
            );
        }

        public static int CountByCombination(string combination, Mfrs17CellMappingBo mfrs17CellMappingBo)
        {
            return CountByCombination(
                combination,
                mfrs17CellMappingBo.ReinsEffDatePolStartDate,
                mfrs17CellMappingBo.ReinsEffDatePolEndDate,
                mfrs17CellMappingBo.Id
            );
        }

        public static int CountByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate,
            DateTime? ReinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            return Mfrs17CellMappingDetail.CountByCombination(
                combination,
                //treatyCodeId,
                reinsEffDatePolStartDate,
                ReinsEffDatePolEndDate,
                mfrs17CellMappingId
            );
        }

        public static int CountByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            return Mfrs17CellMappingDetail.CountByParams(
                treatyCode,
                profitComm,
                reinsBasisCodeId,
                cedingPlanCode,
                mlreBenefitCode,
                reinsEffDatePol,
                rateTable,
                groupById
            );
        }

        public static int CountByParams(
            RiDataBo riData,
            CacheService cache,
            bool groupById = false
        )
        {
            return CountByParams(
                riData.TreatyCode,
                riData.ProfitComm,
                cache.GetReinsBasisCodeId(riData),
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.RateTable,
                groupById
            );
        }

        public static IList<Mfrs17CellMappingDetailBo> GetByParams(
            string treatyCode,
            string profitComm,
            int? reinsBasisCodeId,
            string cedingPlanCode = null,
            string mlreBenefitCode = null,
            DateTime? reinsEffDatePol = null,
            string rateTable = null,
            bool groupById = false
        )
        {
            return FormBos(Mfrs17CellMappingDetail.GetByParams(
                treatyCode,
                profitComm,
                reinsBasisCodeId,
                cedingPlanCode,
                mlreBenefitCode,
                reinsEffDatePol,
                rateTable,
                groupById
            ));
        }

        public static IList<Mfrs17CellMappingDetailBo> GetByParams(
            RiDataBo riData,
            CacheService cache,
            bool groupById = false
        )
        {
            return GetByParams(
                riData.TreatyCode,
                riData.ProfitComm,
                cache.GetReinsBasisCodeId(riData),
                riData.CedingPlanCode,
                riData.MlreBenefitCode,
                riData.ReinsEffDatePol,
                riData.RateTable,
                groupById
            );
        }
        
        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return Mfrs17CellMappingDetail.CountByTreatyCodeId(treatyCodeId);
        }

        public static int CountDuplicateByParams(Mfrs17CellMappingBo mfrs17CellMappingBo, Mfrs17CellMappingDetailBo mfrs17CellMappingDetailBo)
        {
            return CountDuplicateByParams(
                mfrs17CellMappingDetailBo.CedingPlanCode,
                mfrs17CellMappingDetailBo.BenefitCode,
                mfrs17CellMappingBo.ReinsBasisCodePickListDetailId,
                mfrs17CellMappingDetailBo.TreatyCode,
                mfrs17CellMappingBo.ProfitCommPickListDetailId,
                mfrs17CellMappingBo.RateTable,
                mfrs17CellMappingBo.ReinsEffDatePolStartDate,
                mfrs17CellMappingBo.ReinsEffDatePolEndDate,
                mfrs17CellMappingBo.Id
            );
        }

        public static int CountDuplicateByParams(
            string cedingPlanCode,
            string benefitCode,
            int? reinsBasisCodePickListId,
            string treatyCode,
            int? profitCommPickListDetailId,
            string rateTable,
            DateTime? reinsEffDatePolStartDate,
            DateTime? ReinsEffDatePolEndDate,
            int? mfrs17CellMappingId = null
        )
        {
            return Mfrs17CellMappingDetail.CountDuplicateByParams(
                cedingPlanCode,
                benefitCode,
                reinsBasisCodePickListId,
                treatyCode,
                profitCommPickListDetailId,
                rateTable,
                reinsEffDatePolStartDate,
                ReinsEffDatePolEndDate,
                mfrs17CellMappingId
            );
        }

        public static Result Save(ref Mfrs17CellMappingDetailBo bo)
        {
            if (!Mfrs17CellMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref Mfrs17CellMappingDetailBo bo, ref TrailObject trail)
        {
            if (!Mfrs17CellMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17CellMappingDetailBo bo)
        {
            Mfrs17CellMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref Mfrs17CellMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17CellMappingDetailBo bo)
        {
            Result result = Result();

            Mfrs17CellMappingDetail entity = Mfrs17CellMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Mfrs17CellMappingId = bo.Mfrs17CellMappingId;
                entity.Combination = bo.Combination;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.BenefitCode = bo.BenefitCode;
                entity.TreatyCode = bo.TreatyCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17CellMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(Mfrs17CellMappingDetailBo bo)
        {
            Mfrs17CellMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(Mfrs17CellMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = Mfrs17CellMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByMfrs17CellMappingId(int mfrs17CellMappingId)
        {
            return Mfrs17CellMappingDetail.DeleteByMfrs17CellMappingId(mfrs17CellMappingId);
        }

        public static void DeleteByMfrs17CellMappingId(int mfrs17CellMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByMfrs17CellMappingId(mfrs17CellMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(Mfrs17CellMappingDetail)));
                }
            }
        }
    }
}
