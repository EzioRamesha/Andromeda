using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.RiDatas
{
    public class RiDataComputationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataComputation)),
                Controller = ModuleBo.ModuleController.RiDataComputation.ToString(),
            };
        }

        public static RiDataComputationBo FormBo(RiDataComputation entity = null)
        {
            if (entity == null)
                return null;

            StandardOutputBo standardOutputBo = StandardOutputService.Find(entity.StandardOutputId);
            return new RiDataComputationBo
            {
                Id = entity.Id,
                RiDataConfigId = entity.RiDataConfigId,
                RiDataConfigBo = RiDataConfigService.Find(entity.RiDataConfigId),
                Step = entity.Step,
                SortIndex = entity.SortIndex,
                Description = entity.Description,
                Condition = entity.Condition,
                StandardOutputId = entity.StandardOutputId,
                StandardOutputBo = standardOutputBo,
                StandardOutputCode = standardOutputBo?.Code,
                Mode = entity.Mode,
                CalculationFormula = entity.CalculationFormula,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataComputationBo> FormBos(IList<RiDataComputation> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataComputationBo> bos = new List<RiDataComputationBo>() { };
            foreach (RiDataComputation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

            public static RiDataComputation FormEntity(RiDataComputationBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataComputation
            {
                Id = bo.Id,
                RiDataConfigId = bo.RiDataConfigId,
                Step = bo.Step,
                SortIndex = bo.SortIndex,
                Description = bo.Description,
                Condition = bo.Condition,
                StandardOutputId = bo.StandardOutputId,
                Mode = bo.Mode,
                CalculationFormula = bo.CalculationFormula,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDataComputation.IsExists(id);
        }

        public static RiDataComputationBo Find(int id)
        {
            return FormBo(RiDataComputation.Find(id));
        }

        public static IList<RiDataComputationBo> GetByRiDataConfigId(int riDataConfigId, int? step = null)
        {
            return FormBos(RiDataComputation.GetByRiDataConfigId(riDataConfigId, step));
        }

        public static Result Save(ref RiDataComputationBo bo)
        {
            if (!RiDataComputation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataComputationBo bo, ref TrailObject trail)
        {
            if (!RiDataComputation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataComputationBo bo)
        {
            RiDataComputation entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataComputationBo bo)
        {
            Result result = Result();

            RiDataComputation entity = RiDataComputation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataConfigId = bo.RiDataConfigId;
                entity.Step = bo.Step;
                entity.SortIndex = bo.SortIndex;
                entity.Description = bo.Description;
                entity.Condition = bo.Condition;
                entity.StandardOutputId = bo.StandardOutputId;
                entity.Mode = bo.Mode;
                entity.CalculationFormula = bo.CalculationFormula;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataComputationBo bo)
        {
            RiDataComputation.Delete(bo.Id);
        }

        public static Result Delete(RiDataComputationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RiDataComputation.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int riDataConfigId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<RiDataComputation> riDataComputations = RiDataComputation.GetByRiDataConfigIdExcept(riDataConfigId, ids);
            foreach (RiDataComputation riDataComputation in riDataComputations)
            {
                DataTrail dataTrail = RiDataComputation.Delete(riDataComputation.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            return RiDataComputation.DeleteAllByRiDataConfigId(riDataConfigId);
        }

        public static void DeleteAllByRiDataConfigId(int riDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataConfigId(riDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataComputation)));
                }
            }
        }
    }
}
