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
    public class PublicHolidayDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PublicHolidayDetail)),
            };
        }

        public static PublicHolidayDetailBo FormBo(PublicHolidayDetail entity = null)
        {
            if (entity == null)
                return null;
            return new PublicHolidayDetailBo
            {
                Id = entity.Id,
                PublicHolidayId = entity.PublicHolidayId,
                PublicHolidayBo = PublicHolidayService.Find(entity.PublicHolidayId),
                PublicHolidayDate = entity.PublicHolidayDate,
                PublicHolidayDateStr = entity.PublicHolidayDate.ToString(Util.GetDateFormat()),
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PublicHolidayDetailBo> FormBos(IList<PublicHolidayDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<PublicHolidayDetailBo> bos = new List<PublicHolidayDetailBo>() { };
            foreach (PublicHolidayDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PublicHolidayDetail FormEntity(PublicHolidayDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new PublicHolidayDetail
            {
                Id = bo.Id,
                PublicHolidayId = bo.PublicHolidayId,
                PublicHolidayDate = bo.PublicHolidayDate,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PublicHolidayDetail.IsExists(id);
        }

        public static bool IsExists(DateTime dateTime)
        {
            using (var db = new AppDbContext())
            {
                DateTime date = dateTime.Date;
                return db.PublicHolidayDetails.Any(q => q.PublicHolidayDate == date);
            }
        }

        public static PublicHolidayDetailBo Find(int id)
        {
            return FormBo(PublicHolidayDetail.Find(id));
        }

        public static IList<PublicHolidayDetailBo> GetByPublicHolidayId(int publicHolidayId)
        {
            return FormBos(PublicHolidayDetail.GetByPublicHolidayId(publicHolidayId));
        }

        public static IList<PublicHolidayDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PublicHolidayDetails.ToList());
            }
        }

        public static Result Save(PublicHolidayDetailBo bo)
        {
            if (!PublicHolidayDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(PublicHolidayDetailBo bo, ref TrailObject trail)
        {
            if (!PublicHolidayDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(PublicHolidayDetailBo bo)
        {
            PublicHolidayDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(PublicHolidayDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(PublicHolidayDetailBo bo)
        {
            Result result = Result();

            PublicHolidayDetail entity = PublicHolidayDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            /*
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }
            */

            if (result.Valid)
            {
                entity.PublicHolidayId = bo.PublicHolidayId;
                entity.PublicHolidayDate = bo.PublicHolidayDate;
                entity.Description = bo.Description;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(PublicHolidayDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PublicHolidayDetailBo bo)
        {
            PublicHolidayDetail.Delete(bo.Id);
        }

        public static Result Delete(PublicHolidayDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = PublicHolidayDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByPublicHolidayId(int publicHolidayId)
        {
            return PublicHolidayDetail.DeleteAllByPublicHolidayId(publicHolidayId);
        }

        public static void DeleteAllByPublicHolidayId(int publicHolidayId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByPublicHolidayId(publicHolidayId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(BenefitDetail)));
                }
            }
        }

        public static Result DeleteByPublicHolidayIdExcept(int publicHolidayId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<PublicHolidayDetail> calMLReDetails = PublicHolidayDetail.GetByPublicHolidayIdExcept(publicHolidayId, saveIds);
            foreach (PublicHolidayDetail calMLReDetail in calMLReDetails)
            {
                DataTrail dataTrail = PublicHolidayDetail.Delete(calMLReDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<PublicHolidayDetailBo> IsDuplicate(IList<PublicHolidayDetailBo> bos)
        {
            return bos.GroupBy(x => x.PublicHolidayDateStr)
                .Where(x => !string.IsNullOrEmpty(x.Key) && x.Skip(1).Any())
                .Select(x => new PublicHolidayDetailBo { PublicHolidayDateStr = x.Key })
                .ToList();
        }

        public static Result Validate(IList<PublicHolidayDetailBo> bos)
        {
            Result result = Result();

            List<string> errors = new List<string>();
            foreach (PublicHolidayDetailBo bo in bos)
            {
                errors = bo.Validate();
            }

            // check for Date duplication
            foreach (PublicHolidayDetailBo bo in IsDuplicate(bos))
            {
                errors.Add(string.Format(MessageBag.AlreadyTaken, "Date", bo.PublicHolidayDateStr));
            }

            foreach (string error in errors)
            {
                result.AddError(error);
            }
            return result;
        }
    }
}
