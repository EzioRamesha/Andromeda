using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.RiDatas
{
    public class RiDataMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataMapping)),
                Controller = ModuleBo.ModuleController.RiDataMapping.ToString(),
            };
        }

        public static RiDataMappingBo FormBo(RiDataMapping entity = null)
        {
            if (entity == null)
                return null;

            StandardOutputBo standardOutputBo = StandardOutputService.Find(entity.StandardOutputId);

            RiDataMappingBo riDataMappingBo = new RiDataMappingBo
            {
                Id = entity.Id,
                RiDataConfigId = entity.RiDataConfigId,
                RiDataConfigBo = RiDataConfigService.Find(entity.RiDataConfigId),
                StandardOutputId = entity.StandardOutputId,
                StandardOutputBo = standardOutputBo,
                StandardOutputCode = standardOutputBo?.Code,
                SortIndex = entity.SortIndex,
                Row = entity.Row,
                Length = entity.Length,
                RawColumnName = entity.RawColumnName,
                TransformFormula = entity.TransformFormula,
                TransformFormulaName = RiDataMappingBo.GetTransformFormulaName(entity.TransformFormula),
                DefaultValue = entity.DefaultValue,
                DefaultObjectId = entity.DefaultObjectId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                RiDataMappingDetailBos = RiDataMappingDetailService.GetByRiDataMappingId(entity.Id)
            };

            riDataMappingBo.GetDefaultValueType();
            return riDataMappingBo;
        }

        public static IList<RiDataMappingBo> FormBos(IList<RiDataMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataMappingBo> bos = new List<RiDataMappingBo>() { };
            foreach (RiDataMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataMapping FormEntity(RiDataMappingBo bo = null)
        {
            if (bo == null)
                return null;

            return new RiDataMapping
            {
                Id = bo.Id,
                RiDataConfigId = bo.RiDataConfigId,
                StandardOutputId = bo.StandardOutputId,
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
            return RiDataMapping.IsExists(id);
        }

        public static RiDataMappingBo Find(int id)
        {
            return FormBo(RiDataMapping.Find(id));
        }

        public static RiDataMappingBo GetCustomFieldMapping(string rawColumnName, int? col)
        {
            int id = StandardOutputBo.TypeCustomField;
            return new RiDataMappingBo
            {
                StandardOutputId = id,
                StandardOutputBo = new StandardOutputBo()
                {
                    Id = id,
                    Type = id,
                    DataType = StandardOutputBo.GetDataTypeByType(id),
                    Code = StandardOutputBo.GetCodeByType(id),
                    Name = StandardOutputBo.GetTypeName(id),
                },
                RawColumnName = rawColumnName,
                Col = col,
                TransformFormula = RiDataMappingBo.TransformFormulaNoTransform,
            };
        }

        public static IList<RiDataMappingBo> GetByRiDataConfigId(int riDataConfigId)
        {
            return FormBos(RiDataMapping.GetByRiDataConfigId(riDataConfigId));
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            return RiDataMapping.CountByPickListDetailId(pickListDetailId);
        }

        public static int CountByBenefitId(int benefitId)
        {
            return RiDataMapping.CountByBenefitId(benefitId);
        }

        public static Result Save(ref RiDataMappingBo bo)
        {
            if (!RiDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataMappingBo bo, ref TrailObject trail)
        {
            if (!RiDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataMappingBo bo)
        {
            RiDataMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataMappingBo bo)
        {
            Result result = Result();

            RiDataMapping entity = RiDataMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataConfigId = bo.RiDataConfigId;
                entity.StandardOutputId = bo.StandardOutputId;
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

        public static Result Update(ref RiDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataMappingBo bo)
        {
            RiDataMapping.Delete(bo.Id);
        }

        public static Result Delete(RiDataMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = RiDataMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static Result DeleteByDataConfigIdExcept(int riDataConfigId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<RiDataMapping> riDataMappings = RiDataMapping.GetByRiDataConfigIdExcept(riDataConfigId, ids);
            foreach (RiDataMapping riDataMapping in riDataMappings)
            {
                IList<DataTrail> dataTrails = RiDataMappingDetail.DeleteAllByRiDataMappingId(riDataMapping.Id);
                if (dataTrails.Count > 0)
                {
                    foreach (DataTrail detailDataTrail in dataTrails)
                    {
                        detailDataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataMapping)));
                    }
                }

                DataTrail dataTrail = RiDataMapping.Delete(riDataMapping.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            return RiDataMapping.DeleteAllByRiDataConfigId(riDataConfigId);
        }

        public static void DeleteAllByRiDataConfigId(int riDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataConfigId(riDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataMapping)));
                }
            }
        }
    }
}
