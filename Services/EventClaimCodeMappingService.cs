using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EventClaimCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(EventClaimCodeMapping)),
                Controller = ModuleBo.ModuleController.EventClaimCodeMapping.ToString()
            };
        }

        public static Expression<Func<EventClaimCodeMapping, EventClaimCodeMappingBo>> Expression()
        {
            return entity => new EventClaimCodeMappingBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,
                EventCodeId = entity.EventCodeId,
                MLReEventCode = entity.EventCode.Code,
                CedingEventCode = entity.CedingEventCode,
                CedingClaimType = entity.CedingClaimType,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static EventClaimCodeMappingBo FormBo(EventClaimCodeMapping entity = null)
        {
            if (entity == null)
                return null;
            return new EventClaimCodeMappingBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                EventCodeId = entity.EventCodeId,
                EventCodeBo = EventCodeService.Find(entity.EventCodeId),
                CedingEventCode = entity.CedingEventCode,
                CedingClaimType = entity.CedingClaimType,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<EventClaimCodeMappingBo> FormBos(IList<EventClaimCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<EventClaimCodeMappingBo> bos = new List<EventClaimCodeMappingBo>() { };
            foreach (EventClaimCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static EventClaimCodeMapping FormEntity(EventClaimCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new EventClaimCodeMapping
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                EventCodeId = bo.EventCodeId,
                CedingEventCode = bo.CedingEventCode,
                CedingClaimType = bo.CedingClaimType,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return EventClaimCodeMapping.IsExists(id);
        }

        public static bool IsDuplicate(EventClaimCodeMapping eventClaimCodeMapping)
        {
            return eventClaimCodeMapping.IsDuplicate();
        }

        public static EventClaimCodeMappingBo Find(int id)
        {
            return FormBo(EventClaimCodeMapping.Find(id));
        }

        public static IList<EventClaimCodeMappingBo> GetByCedantId(int cedantId)
        {
            return FormBos(EventClaimCodeMapping.GetByCedantId(cedantId));
        }

        public static IList<EventClaimCodeMappingBo> GetByCedantIdEventCodeId(int cedantId, int eventCodeId)
        {
            return FormBos(EventClaimCodeMapping.GetByCedantIdEventCodeId(cedantId, eventCodeId));
        }

        public static Result Save(ref EventClaimCodeMappingBo bo)
        {
            if (!EventClaimCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref EventClaimCodeMappingBo bo, ref TrailObject trail)
        {
            if (!EventClaimCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref EventClaimCodeMappingBo bo)
        {
            EventClaimCodeMapping entity = FormEntity(bo);

            Result result = Result();
            //if (IsDuplicate(entity))
            //{
            //    result.AddError(string.Format("Combination {0} '{1}' and {2} '{3}' is already taken.",
            //        "Ceding Company", bo.CedantBo.Code, "EventCode", bo.EventCodeBo.Code));
            //}

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref EventClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref EventClaimCodeMappingBo bo)
        {
            Result result = Result();

            EventClaimCodeMapping entity = EventClaimCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            //if (IsDuplicate(FormEntity(bo)))
            //{
            //    result.AddError(string.Format("Combination {0} '{1}' and {2} '{3}' is already taken.",
            //        "Ceding Company", bo.CedantBo.Code, "EventCode", bo.EventCodeBo.Code));
            //}

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.EventCodeId = bo.EventCodeId;
                entity.CedingEventCode = bo.CedingEventCode;
                entity.CedingClaimType = bo.CedingClaimType;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref EventClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(EventClaimCodeMappingBo bo)
        {
            EventClaimCodeMappingDetailService.DeleteByEventClaimCodeMappingId(bo.Id);
            EventClaimCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(EventClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            EventClaimCodeMappingDetailService.DeleteByEventClaimCodeMappingId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = EventClaimCodeMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMapping(EventClaimCodeMappingBo bo)
        {
            Result result = new Result();
            var list = new Dictionary<string, List<string>> { };

            var details = CreateDetails(bo);
            //var cedant = details.Where(q => q.EventClaimCodeMappingBo.CedantId == bo.CedantId).ToList();

            foreach (var detail in details)
            {
                var d = detail;
                TrimMaxLength(ref d, ref list);
                if (list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        result.AddError(string.Format("Exceeded Max Length: {0}", prop.Key));
                    }
                    break;
                }

                if (EventClaimCodeMappingDetailService.CountByCombination(detail.Combination, bo) > 0)
                {
                    result.AddError("Existing Event Claim Code Mapping Combination Found");
                    break;
                }
            }

            //foreach (var detail in cedant)
            //{
            //    var count = EventClaimCodeMappingDetailService.CountByCombination(detail.Combination, bo);
            //    if (count > 0)
            //    {
            //        result.AddError("Existing Event Claim Code Mapping Combination Found");
            //        break;
            //    }
            //}
            return result;
        }

        public static IList<EventClaimCodeMappingDetailBo> CreateDetails(EventClaimCodeMappingBo bo, int createdById = 0)
        {
            var details = new List<EventClaimCodeMappingDetailBo> { };
            CartesianProduct<string> cellMappings = new CartesianProduct<string>(
                bo.CedingEventCode.ToArraySplitTrim(),
                bo.CedingClaimType.ToArraySplitTrim()
            );
            foreach (var item in cellMappings.Get())
            {
                var cedingEventCode = item[0];
                var cedingClaimType = item[1];
                var items = new List<string>
                {
                    cedingEventCode,
                    cedingClaimType,
                };
                details.Add(new EventClaimCodeMappingDetailBo
                {
                    EventClaimCodeMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CedingEventCode = string.IsNullOrEmpty(cedingEventCode) ? null : cedingEventCode,
                    CedingClaimType = string.IsNullOrEmpty(cedingClaimType) ? null : cedingClaimType,                    
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref EventClaimCodeMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new EventClaimCodeMappingDetail();
            foreach (var property in (typeof(EventClaimCodeMappingDetailBo)).GetProperties())
            {
                var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>(property.Name);
                if (maxLengthAttr != null)
                {
                    var value = property.GetValue(detailBo, null);
                    if (value != null && value is string @string && !string.IsNullOrEmpty(@string))
                    {
                        if (@string.Length > maxLengthAttr.Length)
                        {
                            string propName = string.Format("{0}({1})", property.Name, maxLengthAttr.Length);

                            if (!list.ContainsKey(propName))
                                list.Add(propName, new List<string> { });

                            var oldValue = @string;
                            var newValue = @string.Substring(0, maxLengthAttr.Length);
                            var formatValue = string.Format("{0}|{1}", oldValue, newValue);

                            if (!list[propName].Contains(formatValue))
                                list[propName].Add(formatValue);

                            property.SetValue(detailBo, newValue);
                        }
                    }
                }
            }
        }

        public static void ProcessMappingDetail(EventClaimCodeMappingBo bo, int authUserId)
        {
            EventClaimCodeMappingDetailService.DeleteByEventClaimCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                EventClaimCodeMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(EventClaimCodeMappingBo bo, int authUserId, ref TrailObject trail)
        {
            EventClaimCodeMappingDetailService.DeleteByEventClaimCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                EventClaimCodeMappingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
