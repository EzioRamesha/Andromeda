using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class PublicHolidayService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PublicHoliday)),
                Controller = ModuleBo.ModuleController.PublicHoliday.ToString()
            };
        }

        public static PublicHolidayBo FormBo(PublicHoliday entity = null)
        {
            if (entity == null)
                return null;
            return new PublicHolidayBo
            {
                Id = entity.Id,
                Year = entity.Year,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PublicHolidayBo> FormBos(IList<PublicHoliday> entities = null)
        {
            if (entities == null)
                return null;
            IList<PublicHolidayBo> bos = new List<PublicHolidayBo>() { };
            foreach (PublicHoliday entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PublicHoliday FormEntity(PublicHolidayBo bo = null)
        {
            if (bo == null)
                return null;
            return new PublicHoliday
            {
                Id = bo.Id,
                Year = bo.Year,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateYear(PublicHoliday publicHolidays)
        {
            return publicHolidays.IsDuplicateYear();
        }

        public static bool IsExists(int id)
        {
            return PublicHoliday.IsExists(id);
        }

        public static PublicHolidayBo Find(int id)
        {
            return FormBo(PublicHoliday.Find(id));
        }

        public static PublicHolidayBo FindByYear(int year)
        {
            return FormBo(PublicHoliday.FindByYear(year));
        }

        public static Result Save(ref PublicHolidayBo bo)
        {
            if (!PublicHoliday.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PublicHolidayBo bo, ref TrailObject trail)
        {
            if (!PublicHoliday.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PublicHolidayBo bo)
        {
            PublicHoliday entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateYear(entity))
            {
                result.AddTakenError("Year", bo.Year.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PublicHolidayBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PublicHolidayBo bo)
        {
            Result result = Result();

            PublicHoliday entity = PublicHoliday.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateYear(FormEntity(bo)))
            {
                result.AddTakenError("Year", bo.Year.ToString());
            }

            if (result.Valid)
            {
                entity.Year = bo.Year;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PublicHolidayBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PublicHolidayBo bo)
        {
            PublicHoliday.Delete(bo.Id);
        }

        public static Result Delete(PublicHolidayBo bo, ref TrailObject trail)
        {
            Result result = Result();

            //if (ItemCodeMapping.CountByItemCodeId(bo.Id) > 0)
            //{
            //    result.AddErrorRecordInUsed();
            //}

            if (result.Valid)
            {
                DataTrail dataTrail = PublicHoliday.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
