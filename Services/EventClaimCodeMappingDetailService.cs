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
    public class EventClaimCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(EventClaimCodeMappingDetail)),
            };
        }

        public static EventClaimCodeMappingDetailBo FormBo(EventClaimCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new EventClaimCodeMappingDetailBo
            {
                Id = entity.Id,
                EventClaimCodeMappingId = entity.EventClaimCodeMappingId,
                EventClaimCodeMappingBo = EventClaimCodeMappingService.Find(entity.EventClaimCodeMappingId),
                Combination = entity.Combination,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,

                CedingEventCode = entity.CedingEventCode,
                CedingClaimType = entity.CedingClaimType,
            };
        }

        public static IList<EventClaimCodeMappingDetailBo> FormBos(IList<EventClaimCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<EventClaimCodeMappingDetailBo> bos = new List<EventClaimCodeMappingDetailBo>() { };
            foreach (EventClaimCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static EventClaimCodeMappingDetail FormEntity(EventClaimCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new EventClaimCodeMappingDetail
            {
                Id = bo.Id,
                EventClaimCodeMappingId = bo.EventClaimCodeMappingId,
                Combination = bo.Combination,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,

                CedingEventCode = bo.CedingEventCode,
                CedingClaimType = bo.CedingClaimType,
            };
        }

        public static bool IsExists(int id)
        {
            return EventClaimCodeMappingDetail.IsExists(id);
        }

        public static EventClaimCodeMappingDetailBo Find(int id)
        {
            return FormBo(EventClaimCodeMappingDetail.Find(id));
        }

        public static EventClaimCodeMappingDetailBo FindByCombination(
            string combination,
            int cedantId,
            int? eventClaimCodeMappingId = null
        )
        {
            return FormBo(EventClaimCodeMappingDetail.FindByCombination(
                combination,
                cedantId,
                eventClaimCodeMappingId
            ));
        }

        public static EventClaimCodeMappingDetailBo FindByCombination(string combination, EventClaimCodeMappingBo eventClaimCodeMappingBo)
        {
            return FindByCombination(
                combination,
                eventClaimCodeMappingBo.CedantId,
                eventClaimCodeMappingBo.Id
            );
        }

        public static EventClaimCodeMappingDetailBo FindByParams(
            string cedingEventCode = null,
            string cedingClaimCode = null,
            bool groupById = false
        )
        {
            return FormBo(EventClaimCodeMappingDetail.FindByParams(
                cedingEventCode,
                cedingClaimCode,
                groupById
            ));
        }

        public static EventClaimCodeMappingDetailBo FindByCedantParams(
            int cedantId,
            string cedingEventCode,
            string cedingClaimType,
            bool groupById = false
        )
        {
            return FormBo(EventClaimCodeMappingDetail.FindByCedantParams(
                cedantId,
                cedingEventCode,
                cedingClaimType,
                groupById
            ));
        }

        public static int CountByCombination(string combination, EventClaimCodeMappingBo eventClaimCodeMappingBo)
        {
            return CountByCombination(
                combination,
                eventClaimCodeMappingBo.CedantId,
                eventClaimCodeMappingBo.Id
            );
        }

        public static int CountByCombination(
            string combination,
            int cedantId,
            int? eventClaimCodeMappingId = null
        )
        {
            return EventClaimCodeMappingDetail.CountByCombination(
                combination,
                cedantId,
                eventClaimCodeMappingId
            );
        }

        public static int CountByParams(
            string cedingEventCode = null,
            string cedingClaimType = null,
            bool groupById = false
        )
        {
            return EventClaimCodeMappingDetail.CountByParams(
                cedingEventCode,
                cedingClaimType,
                groupById
            );
        }
        
        public static int CountByCedantParams(
            int cedantId,
            string cedingEventCode,
            string cedingClaimType,
            bool groupById = false
        )
        {
            return EventClaimCodeMappingDetail.CountByCedantParams(
                cedantId,
                cedingEventCode,
                cedingClaimType,
                groupById
            );
        }

        public static IList<EventClaimCodeMappingDetailBo> GetByParams(
            string cedingEventCode = null,
            string cedingClaimType = null,
            bool groupById = false
        )
        {
            return FormBos(EventClaimCodeMappingDetail.GetByParams(
                cedingEventCode,
                cedingClaimType,
                groupById
            ));
        }

        public static Result Save(ref EventClaimCodeMappingDetailBo bo)
        {
            if (!EventClaimCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref EventClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!EventClaimCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref EventClaimCodeMappingDetailBo bo)
        {
            EventClaimCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref EventClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref EventClaimCodeMappingDetailBo bo)
        {
            Result result = Result();

            EventClaimCodeMappingDetail entity = EventClaimCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.EventClaimCodeMappingId = bo.EventClaimCodeMappingId;
                entity.Combination = bo.Combination;
                entity.CedingEventCode = bo.CedingEventCode;
                entity.CedingClaimType = bo.CedingClaimType;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref EventClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(EventClaimCodeMappingDetailBo bo)
        {
            EventClaimCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(EventClaimCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = EventClaimCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByEventClaimCodeMappingId(int eventClaimCodeMappingId)
        {
            return EventClaimCodeMappingDetail.DeleteByEventClaimCodeMappingId(eventClaimCodeMappingId);
        }

        public static void DeleteByEventClaimCodeMappingId(int eventClaimCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByEventClaimCodeMappingId(eventClaimCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(EventClaimCodeMappingDetail)));
                }
            }
        }
    }
}
