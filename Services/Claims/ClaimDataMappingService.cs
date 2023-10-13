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
    public class ClaimDataMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimDataMapping)),
                Controller = ModuleBo.ModuleController.ClaimDataMapping.ToString(),
            };
        }

        public static ClaimDataMappingBo FormBo(ClaimDataMapping entity = null)
        {
            if (entity == null)
                return null;

            StandardClaimDataOutputBo standardClaimDataOutputBo = StandardClaimDataOutputService.Find(entity.StandardClaimDataOutputId);

            ClaimDataMappingBo claimDataMappingBo = new ClaimDataMappingBo
            {
                Id = entity.Id,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfigBo = ClaimDataConfigService.Find(entity.ClaimDataConfigId),
                StandardClaimDataOutputId = entity.StandardClaimDataOutputId,
                StandardClaimDataOutputBo = standardClaimDataOutputBo,
                StandardClaimDataOutputCode = standardClaimDataOutputBo?.Code,
                SortIndex = entity.SortIndex,
                Row = entity.Row,
                Length = entity.Length,
                RawColumnName = entity.RawColumnName,
                TransformFormula = entity.TransformFormula,
                TransformFormulaName = ClaimDataMappingBo.GetTransformFormulaName(entity.TransformFormula),
                DefaultValue = entity.DefaultValue,
                DefaultObjectId = entity.DefaultObjectId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                ClaimDataMappingDetailBos = ClaimDataMappingDetailService.GetByClaimDataMappingId(entity.Id)
            };

            claimDataMappingBo.GetDefaultValueType();
            return claimDataMappingBo;
        }

        public static IList<ClaimDataMappingBo> FormBos(IList<ClaimDataMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimDataMappingBo> bos = new List<ClaimDataMappingBo>() { };
            foreach (ClaimDataMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimDataMapping FormEntity(ClaimDataMappingBo bo = null)
        {
            if (bo == null)
                return null;

            return new ClaimDataMapping
            {
                Id = bo.Id,
                ClaimDataConfigId = bo.ClaimDataConfigId,
                StandardClaimDataOutputId = bo.StandardClaimDataOutputId,
                SortIndex = bo.SortIndex,
                Row = bo.Row,
                Length = bo.Length,
                RawColumnName = bo.RawColumnName,
                TransformFormula = bo.TransformFormula,
                DefaultValue = bo.DefaultValue,
                DefaultObjectId = bo.DefaultObjectId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimDataMapping.IsExists(id);
        }

        public static ClaimDataMappingBo Find(int id)
        {
            return FormBo(ClaimDataMapping.Find(id));
        }

        public static ClaimDataMappingBo GetCustomFieldMapping(string rawColumnName, int? col)
        {
            int id = StandardClaimDataOutputBo.TypeCustomField;
            return new ClaimDataMappingBo
            {
                StandardClaimDataOutputId = id,
                StandardClaimDataOutputBo = new StandardClaimDataOutputBo()
                {
                    Id = id,
                    Type = id,
                    DataType = StandardClaimDataOutputBo.GetDataTypeByType(id),
                    Code = StandardClaimDataOutputBo.GetCodeByType(id),
                    Name = StandardClaimDataOutputBo.GetTypeName(id),
                },
                RawColumnName = rawColumnName,
                Col = col,
                TransformFormula = ClaimDataMappingBo.TransformFormulaNoTransform,
            };
        }

        public static IList<ClaimDataMappingBo> GetByClaimDataConfigId(int claimDataConfigId)
        {
            return FormBos(ClaimDataMapping.GetByClaimDataConfigId(claimDataConfigId));
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            return ClaimDataMapping.CountByPickListDetailId(pickListDetailId);
        }

        public static int CountByBenefitId(int benefitId)
        {
            return ClaimDataMapping.CountByBenefitId(benefitId);
        }

        public static Result Save(ref ClaimDataMappingBo bo)
        {
            if (!ClaimDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimDataMappingBo bo, ref TrailObject trail)
        {
            if (!ClaimDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimDataMappingBo bo)
        {
            ClaimDataMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimDataMappingBo bo)
        {
            Result result = Result();

            ClaimDataMapping entity = ClaimDataMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ClaimDataConfigId = bo.ClaimDataConfigId;
                entity.StandardClaimDataOutputId = bo.StandardClaimDataOutputId;
                entity.SortIndex = bo.SortIndex;
                entity.Row = bo.Row;
                entity.Length = bo.Length;
                entity.RawColumnName = bo.RawColumnName;
                entity.TransformFormula = bo.TransformFormula;
                entity.DefaultValue = bo.DefaultValue;
                entity.DefaultObjectId = bo.DefaultObjectId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimDataMappingBo bo)
        {
            ClaimDataMapping.Delete(bo.Id);
        }

        public static Result Delete(ClaimDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = ClaimDataMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int claimDataConfigId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimDataMapping> claimDataMappings = ClaimDataMapping.GetByClaimDataConfigIdExcept(claimDataConfigId, ids);
            foreach (ClaimDataMapping claimDataMapping in claimDataMappings)
            {
                IList<DataTrail> dataTrails = ClaimDataMappingDetail.DeleteAllByClaimDataMappingId(claimDataMapping.Id);
                if (dataTrails.Count > 0)
                {
                    foreach (DataTrail detailDataTrail in dataTrails)
                    {
                        detailDataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataMapping)));
                    }
                }

                DataTrail dataTrail = ClaimDataMapping.Delete(claimDataMapping.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByClaimDataConfigId(int claimDataConfigId)
        {
            return ClaimDataMapping.DeleteAllByClaimDataConfigId(claimDataConfigId);
        }

        public static void DeleteAllByClaimDataConfigId(int claimDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByClaimDataConfigId(claimDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimDataMapping)));
                }
            }
        }
    }
}
