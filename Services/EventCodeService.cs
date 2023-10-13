using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EventCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(EventCode)),
                Controller = ModuleBo.ModuleController.EventCode.ToString()
            };
        }

        public static EventCodeBo FormBo(EventCode entity = null)
        {
            if (entity == null)
                return null;
            return new EventCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<EventCodeBo> FormBos(IList<EventCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<EventCodeBo> bos = new List<EventCodeBo>() { };
            foreach (EventCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static EventCode FormEntity(EventCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new EventCode
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                Description = bo.Description?.Trim(),
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(EventCode EventCode)
        {
            using (var db = new AppDbContext())
            {
                if (!String.IsNullOrEmpty(EventCode.Code?.Trim()))
                {
                    var query = db.EventCodes.Where(q => q.Code.Trim().Equals(EventCode.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (EventCode.Id != 0)
                    {
                        query = query.Where(q => q.Id != EventCode.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return EventCode.IsExists(id);
        }

        public static EventCodeBo Find(int? id)
        {
            return FormBo(EventCode.Find(id));
        }

        public static EventCodeBo FindByCode(string code)
        {
            return FormBo(EventCode.FindByCode(code));
        }

        public static int Count()
        {
            return EventCode.Count();
        }

        public static IList<EventCodeBo> Get()
        {
            return FormBos(EventCode.Get());
        }

        public static IList<EventCodeBo> GetByStatus(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.EventCodes.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.Trim() == selectedCode.Trim());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static Result Save(ref EventCodeBo bo)
        {
            if (!EventCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref EventCodeBo bo, ref TrailObject trail)
        {
            if (!EventCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref EventCodeBo bo)
        {
            EventCode entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref EventCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref EventCodeBo bo)
        {
            Result result = Result();

            EventCode entity = EventCode.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref EventCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(EventCodeBo bo)
        {
            EventCode.Delete(bo.Id);
        }

        public static Result Delete(EventCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                BenefitDetail.CountByEventCode(bo.Id) > 0 ||
                EventClaimCodeMapping.CountByEventCode(bo.Id) > 0 ||
                ClaimCodeMappingDetail.CountByEventCodeId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = EventCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
