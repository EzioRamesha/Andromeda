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
    public class RiDataPreValidationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataPreValidation)),
                Controller = ModuleBo.ModuleController.RiDataPreValidation.ToString(),
            };
        }

        public static RiDataPreValidationBo FormBo(RiDataPreValidation entity = null)
        {
            if (entity == null)
                return null;
            return new RiDataPreValidationBo
            {
                Id = entity.Id,
                RiDataConfigId = entity.RiDataConfigId,
                RiDataConfigBo = RiDataConfigService.Find(entity.RiDataConfigId),
                Step = entity.Step,
                SortIndex = entity.SortIndex,
                Description = entity.Description,
                Condition = entity.Condition,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataPreValidationBo> FormBos(IList<RiDataPreValidation> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataPreValidationBo> bos = new List<RiDataPreValidationBo>() { };
            foreach (RiDataPreValidation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataPreValidation FormEntity(RiDataPreValidationBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataPreValidation
            {
                Id = bo.Id,
                RiDataConfigId = bo.RiDataConfigId,
                Step = bo.Step,
                SortIndex = bo.SortIndex,
                Description = bo.Description,
                Condition = bo.Condition,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RiDataPreValidation.IsExists(id);
        }

        public static RiDataPreValidationBo Find(int id)
        {
            return FormBo(RiDataPreValidation.Find(id));
        }

        public static IList<RiDataPreValidationBo> GetByRiDataConfigId(int riDataConfigId, int? step = null)
        {
            return FormBos(RiDataPreValidation.GetByRiDataConfigId(riDataConfigId, step));
        }

        public static Result Save(ref RiDataPreValidationBo bo)
        {
            if (!RiDataPreValidation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataPreValidationBo bo, ref TrailObject trail)
        {
            if (!RiDataPreValidation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataPreValidationBo bo)
        {
            RiDataPreValidation entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataPreValidationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataPreValidationBo bo)
        {
            Result result = Result();

            RiDataPreValidation entity = RiDataPreValidation.Find(bo.Id);
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
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataPreValidationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataPreValidationBo bo)
        {
            RiDataPreValidation.Delete(bo.Id);
        }

        public static Result Delete(RiDataPreValidationBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = RiDataPreValidation.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int riDataConfigId, List<int> mappingIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<RiDataPreValidation> riDataPreValidations = RiDataPreValidation.GetByRiDataConfigIdExcept(riDataConfigId, mappingIds);
            foreach (RiDataPreValidation riDataPreValidation in riDataPreValidations)
            {
                DataTrail dataTrail = RiDataPreValidation.Delete(riDataPreValidation.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            return RiDataPreValidation.DeleteAllByRiDataConfigId(riDataConfigId);
        }

        public static void DeleteAllByRiDataConfigId(int riDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataConfigId(riDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataPreValidation)));
                }
            }
        }
    }
}
