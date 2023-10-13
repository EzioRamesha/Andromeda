using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Claims;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Claims
{
    public class ClaimDataComputationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataComputation)),
                Controller = ModuleBo.ModuleController.ClaimDataComputation.ToString(),
            };
        }

        public static ClaimDataComputationBo FormBo(ClaimDataComputation entity = null)
        {
            if (entity == null)
                return null;

            StandardClaimDataOutputBo standardClaimDataOutputBo = StandardClaimDataOutputService.Find(entity.StandardClaimDataOutputId);
            return new ClaimDataComputationBo
            {
                Id = entity.Id,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId),
                Step = entity.Step,
                SortIndex = entity.SortIndex,
                Description = entity.Description,
                Condition = entity.Condition,
                StandardClaimDataOutputId = entity.StandardClaimDataOutputId,
                StandardClaimDataOutputBo = standardClaimDataOutputBo,
                StandardClaimDataOutputCode = standardClaimDataOutputBo?.Code,
                Mode = entity.Mode,
                CalculationFormula = entity.CalculationFormula,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimDataComputationBo> FormBos(IList<ClaimDataComputation> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataComputationBo> bos = new List<ClaimDataComputationBo>() { };
            foreach (ClaimDataComputation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataComputation FormEntity(ClaimDataComputationBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataComputation
            {
                Id = bo.Id,
                ClaimDataConfigId = bo.ClaimDataConfigId,
                Step = bo.Step,
                SortIndex = bo.SortIndex,
                Description = bo.Description,
                Condition = bo.Condition,
                StandardClaimDataOutputId = bo.StandardClaimDataOutputId,
                Mode = bo.Mode,
                CalculationFormula = bo.CalculationFormula,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimDataComputation.IsExists(id);
        }

        public static ClaimDataComputationBo Find(int id)
        {
            return FormBo(ClaimDataComputation.Find(id));
        }

        public static IList<ClaimDataComputationBo> GetByClaimDataConfigId(int claimDataConfigId, int? step = null)
        {
            return FormBos(ClaimDataComputation.GetByClaimDataConfigId(claimDataConfigId, step));
        }

        public static Result Save(ref ClaimDataComputationBo bo)
        {
            if (!ClaimDataComputation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataComputationBo bo, ref TrailObject trail)
        {
            if (!ClaimDataComputation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataComputationBo bo)
        {
            ClaimDataComputation entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataComputationBo bo)
        {
            Result result = Result();

            ClaimDataComputation entity = ClaimDataComputation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimDataConfigId = bo.ClaimDataConfigId;
                entity.Step = bo.Step;
                entity.SortIndex = bo.SortIndex;
                entity.Description = bo.Description;
                entity.Condition = bo.Condition;
                entity.StandardClaimDataOutputId = bo.StandardClaimDataOutputId;
                entity.Mode = bo.Mode;
                entity.CalculationFormula = bo.CalculationFormula;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataComputationBo bo)
        {
            ClaimDataComputation.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimDataComputation.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int claimDataConfigId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimDataComputation> claimDataComputations = ClaimDataComputation.GetByClaimDataConfigIdExcept(claimDataConfigId, ids);
            foreach (ClaimDataComputation claimDataComputation in claimDataComputations)
            {
                DataTrail dataTrail = ClaimDataComputation.Delete(claimDataComputation.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            return ClaimDataComputation.DeleteAllByClaimDataConfigId(claimDataConfigId);
        }

        public static void DeleteAllByClaimDataConfigId(int claimDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataConfigId(claimDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataComputation)));
                }
            }
        }
    }
}
