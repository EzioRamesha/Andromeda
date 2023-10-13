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
    public class RiDataMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RiDataMappingDetail)),
                Controller = ModuleBo.ModuleController.RiDataMappingDetail.ToString(),
            };
        }

        public static RiDataMappingDetailBo FormBo(RiDataMappingDetail entity = null)
        {
            if (entity == null)
                return null;

            PickListDetailBo pickListDetailBo = PickListDetailService.Find(entity.PickListDetailId);
            return new RiDataMappingDetailBo
            {
                Id = entity.Id,
                RiDataMappingId = entity.RiDataMappingId,
                //RiDataMappingBo = RiDataMappingService.Find(entity.RiDataMappingId), // DO NOT LOAD THIS. IT WILL CAUSE INFINITE LOOP.
                PickListDetailId = entity.PickListDetailId,
                PickListDetailBo = pickListDetailBo,
                PickListDetailCode = pickListDetailBo?.Code,
                RawValue = entity.RawValue,
                IsPickDetailIdEmpty = entity.IsPickDetailIdEmpty,
                IsRawValueEmpty = entity.IsRawValueEmpty,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RiDataMappingDetailBo> FormBos(IList<RiDataMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<RiDataMappingDetailBo> bos = new List<RiDataMappingDetailBo>() { };
            foreach (RiDataMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RiDataMappingDetail FormEntity(RiDataMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new RiDataMappingDetail
            {
                Id = bo.Id,
                RiDataMappingId = bo.RiDataMappingId,
                PickListDetailId = bo.PickListDetailId,
                RawValue = bo.RawValue,
                IsPickDetailIdEmpty = bo.IsPickDetailIdEmpty,
                IsRawValueEmpty = bo.IsRawValueEmpty,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static RiDataMappingDetailBo Find(int id)
        {
            return FormBo(RiDataMappingDetail.Find(id));
        }

        public static int CountByPickListDetailId(int pickListDetailId)
        {
            return RiDataMappingDetail.CountByPickListDetailId(pickListDetailId);
        }

        public static IList<RiDataMappingDetailBo> GetByRiDataMappingId(int riDataMappingId)
        {
            return FormBos(RiDataMappingDetail.GetByRiDataMappingId(riDataMappingId));
        }

        public static Result Save(ref RiDataMappingDetailBo bo)
        {
            if (!RiDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RiDataMappingDetailBo bo, ref TrailObject trail)
        {
            if (!RiDataMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RiDataMappingDetailBo bo)
        {
            RiDataMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RiDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RiDataMappingDetailBo bo)
        {
            Result result = Result();

            RiDataMappingDetail entity = RiDataMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RiDataMappingId = bo.RiDataMappingId;
                entity.PickListDetailId = bo.PickListDetailId;
                entity.RawValue = bo.RawValue;
                entity.IsPickDetailIdEmpty = bo.IsPickDetailIdEmpty;
                entity.IsRawValueEmpty = bo.IsRawValueEmpty;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RiDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RiDataMappingDetailBo bo)
        {
            RiDataMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(RiDataMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();
            DataTrail dataTrail = RiDataMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static Result DeleteByDataMappingIdExcept(int riDataMappingId, List<int> ids, ref TrailObject trail)
        {
            Result result = Result();
            IList<RiDataMappingDetail> riDataMappingDetails = RiDataMappingDetail.GetByRiDataMappingIdExcept(riDataMappingId, ids);
            foreach (RiDataMappingDetail riDataMappingDetail in riDataMappingDetails)
            {
                DataTrail dataTrail = RiDataMappingDetail.Delete(riDataMappingDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByRiDataMappingId(int riDataMappingId)
        {
            return RiDataMappingDetail.DeleteAllByRiDataMappingId(riDataMappingId);
        }

        public static void DeleteAllByRiDataMappingId(int riDataMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataMappingId(riDataMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataMappingDetail)));
                }
            }
        }
        
        public static IList<DataTrail> DeleteAllByRiDataConfigId(int riDataConfigId)
        {
            return RiDataMappingDetail.DeleteAllByRiDataConfigId(riDataConfigId);
        }

        public static void DeleteAllByRiDataConfigId(int riDataConfigId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRiDataConfigId(riDataConfigId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RiDataMappingDetail)));
                }
            }
        }
    }
}
