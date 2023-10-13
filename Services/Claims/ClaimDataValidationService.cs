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
    public class ClaimDataValidationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataValidation)),
                Controller = ModuleBo.ModuleController.ClaimDataPreValidation.ToString(),
            };
        }

        public static ClaimDataValidationBo FormBo(ClaimDataValidation entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimDataValidationBo
            {
                Id = entity.Id,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId),
                Step = entity.Step,
                SortIndex = entity.SortIndex,
                Description = entity.Description,
                Condition = entity.Condition,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimDataValidationBo> FormBos(IList<ClaimDataValidation> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataValidationBo> bos = new List<ClaimDataValidationBo>() { };
            foreach (ClaimDataValidation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataValidation FormEntity(ClaimDataValidationBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimDataValidation
            {
                Id = bo.Id,
                ClaimDataConfigId = bo.ClaimDataConfigId,
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
            return ClaimDataValidation.IsExists(id);
        }

        public static ClaimDataValidationBo Find(int id)
        {
            return FormBo(ClaimDataValidation.Find(id));
        }

        public static IList<ClaimDataValidationBo> GetByClaimDataConfigId(int claimDataConfigId, int? step = null)
        {
            return FormBos(ClaimDataValidation.GetByClaimDataConfigId(claimDataConfigId, step));
        }

        public static Result Save(ref ClaimDataValidationBo bo)
        {
            if (!ClaimDataValidation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataValidationBo bo, ref TrailObject trail)
        {
            if (!ClaimDataValidation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataValidationBo bo)
        {
            ClaimDataValidation entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataValidationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataValidationBo bo)
        {
            Result result = Result();

            ClaimDataValidation entity = ClaimDataValidation.Find(bo.Id);
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
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataValidationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataValidationBo bo)
        {
            ClaimDataValidation.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataValidationBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = ClaimDataValidation.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int claimDataConfigId, List<int> mappingIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimDataValidation> claimDataPreValidations = ClaimDataValidation.GetByClaimDataConfigIdExcept(claimDataConfigId, mappingIds);
            foreach (ClaimDataValidation claimDataPreValidation in claimDataPreValidations)
            {
                DataTrail dataTrail = ClaimDataValidation.Delete(claimDataPreValidation.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            return ClaimDataValidation.DeleteAllByClaimDataConfigId(claimDataConfigId);
        }

        public static void DeleteAllByClaimDataConfigId(int claimDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataConfigId(claimDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataValidation)));
                }
            }
        }
    }
}
