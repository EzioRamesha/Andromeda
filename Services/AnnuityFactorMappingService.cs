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
    public class AnnuityFactorMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AnnuityFactorMapping)),
            };
        }

        public static AnnuityFactorMappingBo FormBo(AnnuityFactorMapping entity = null)
        {
            if (entity == null)
                return null;
            return new AnnuityFactorMappingBo
            {
                Id = entity.Id,
                AnnuityFactorId = entity.AnnuityFactorId,
                AnnuityFactorBo = AnnuityFactorService.Find(entity.AnnuityFactorId),
                Combination = entity.Combination,
                CedingPlanCode = entity.CedingPlanCode,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<AnnuityFactorMappingBo> FormBos(IList<AnnuityFactorMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<AnnuityFactorMappingBo> bos = new List<AnnuityFactorMappingBo>() { };
            foreach (AnnuityFactorMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AnnuityFactorMapping FormEntity(AnnuityFactorMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new AnnuityFactorMapping
            {
                Id = bo.Id,
                AnnuityFactorId = bo.AnnuityFactorId,
                Combination = bo.Combination,
                CedingPlanCode = bo.CedingPlanCode,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return AnnuityFactorMapping.IsExists(id);
        }

        public static AnnuityFactorMappingBo Find(int id)
        {
            return FormBo(AnnuityFactorMapping.Find(id));
        }

        public static AnnuityFactorMappingBo FindByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate = null,
            DateTime? reinsEffDatePolEndDate = null,
            int? annuityFactorId = null
        )
        {
            return FormBo(AnnuityFactorMapping.FindByCombination(
                combination,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                annuityFactorId
            ));
        }

        public static AnnuityFactorMappingBo FindByCombination(string combination, RiDataBo riData)
        {
            return FindByCombination(
                combination,
                riData.ReinsEffDatePol,
                riData.ReinsEffDatePol
            );
        }

        public static AnnuityFactorMappingBo FindByCombination(string combination, AnnuityFactorBo annuityFactorBo)
        {
            return FindByCombination(
                combination,
                annuityFactorBo.ReinsEffDatePolStartDate,
                annuityFactorBo.ReinsEffDatePolEndDate,
                annuityFactorBo.Id
            );
        }

        public static AnnuityFactorMappingBo FindByParams(
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            return FormBo(AnnuityFactorMapping.FindByParams(
                cedingPlanCode,
                reinsEffDatePol,
                groupById
            ));
        }

        public static AnnuityFactorMappingBo FindByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return FindByParams(
                riData.CedingPlanCode,
                riData.ReinsEffDatePol,
                groupById
            );
        }

        public static int CountByCombination(
            string combination,
            DateTime? reinsEffDatePolStartDate = null,
            DateTime? reinsEffDatePolEndDate = null,
            int? annuityFactorId = null
        )
        {
            return AnnuityFactorMapping.CountByCombination(
                combination,
                reinsEffDatePolStartDate,
                reinsEffDatePolEndDate,
                annuityFactorId
            );
        }

        public static int CountByCombination(string combination, AnnuityFactorBo annuityFactorBo)
        {
            return CountByCombination(
                combination,
                annuityFactorBo.ReinsEffDatePolStartDate,
                annuityFactorBo.ReinsEffDatePolEndDate,
                annuityFactorBo.Id
            );
        }

        public static int CountByParams(
            string cedingPlanCode,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            return AnnuityFactorMapping.CountByParams(
                cedingPlanCode,
                reinsEffDatePol,
                groupById
            );
        }

        public static int CountByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return CountByParams(
                riData.CedingPlanCode,
                riData.ReinsEffDatePol,
                groupById
            );
        }

        public static IList<AnnuityFactorMappingBo> GetByParams(
            string cedingPlanCode = null,
            DateTime? reinsEffDatePol = null,
            bool groupById = false
        )
        {
            return FormBos(AnnuityFactorMapping.GetByParams(
                cedingPlanCode,
                reinsEffDatePol,
                groupById
            ));
        }

        public static IList<AnnuityFactorMappingBo> GetByParams(
            RiDataBo riData,
            bool groupById = false
        )
        {
            return GetByParams(
                riData.CedingPlanCode,
                riData.ReinsEffDatePol,
                groupById
            );
        }

        public static Result Save(ref AnnuityFactorMappingBo bo)
        {
            if (!AnnuityFactorMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AnnuityFactorMappingBo bo, ref TrailObject trail)
        {
            if (!AnnuityFactorMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AnnuityFactorMappingBo bo)
        {
            AnnuityFactorMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AnnuityFactorMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorMappingBo bo)
        {
            Result result = Result();

            AnnuityFactorMapping entity = AnnuityFactorMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.AnnuityFactorId = bo.AnnuityFactorId;
                entity.Combination = bo.Combination;
                entity.CedingPlanCode = bo.CedingPlanCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AnnuityFactorMappingBo bo)
        {
            AnnuityFactorMapping.Delete(bo.Id);
        }

        public static Result Delete(AnnuityFactorMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = AnnuityFactorMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByAnnuityFactorId(int annuityFactorId)
        {
            return AnnuityFactorMapping.DeleteByAnnuityFactorId(annuityFactorId);
        }

        public static void DeleteByAnnuityFactorId(int annuityFactorId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByAnnuityFactorId(annuityFactorId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AnnuityFactorMapping)));
                }
            }
        }
    }
}
