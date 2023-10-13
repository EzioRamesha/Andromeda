using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeRetroGenderService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeRetroGender)),
                Controller = ModuleBo.ModuleController.PerLifeRetroGender.ToString()
            };
        }

        public static Expression<Func<PerLifeRetroGender, PerLifeRetroGenderBo>> Expression()
        {
            return entity => new PerLifeRetroGenderBo
            {
                Id = entity.Id,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCode = entity.InsuredGenderCodePickListDetail.Code,
                Gender = entity.Gender,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeRetroGenderBo FormBo(PerLifeRetroGender entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeRetroGenderBo
            {
                Id = entity.Id,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                Gender = entity.Gender,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeRetroGenderBo> FormBos(IList<PerLifeRetroGender> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeRetroGenderBo> bos = new List<PerLifeRetroGenderBo>() { };
            foreach (PerLifeRetroGender entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeRetroGender FormEntity(PerLifeRetroGenderBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeRetroGender
            {
                Id = bo.Id,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                Gender = bo.Gender,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeRetroGender.IsExists(id);
        }

        public static PerLifeRetroGenderBo Find(int? id)
        {
            return FormBo(PerLifeRetroGender.Find(id));
        }

        public static PerLifeRetroGenderBo FindByInsuredGenderCodePickListDetailId(int? insuredGenderCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroGenders.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodePickListDetailId).FirstOrDefault());
            }
        }

        public static PerLifeRetroGenderBo FindByInsuredGenderCode(string insuredGenderCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroGenders.Where(q => q.InsuredGenderCodePickListDetail.Code == insuredGenderCode).FirstOrDefault());
            }
        }

        public static PerLifeRetroGenderBo FindByGender(string gender)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeRetroGenders.Where(q => q.Gender == gender).FirstOrDefault());
            }
        }

        public static IList<PerLifeRetroGenderBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeRetroGenders.OrderBy(q => q.Gender).ToList());
            }
        }

        public static Result Save(ref PerLifeRetroGenderBo bo)
        {
            if (!PerLifeRetroGender.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeRetroGenderBo bo, ref TrailObject trail)
        {
            if (!PerLifeRetroGender.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeRetroGender perLifeRetroGender)
        {
            return perLifeRetroGender.IsDuplicate();
        }

        public static Result Create(ref PerLifeRetroGenderBo bo)
        {
            PerLifeRetroGender entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Per Life Retro Gender's record found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeRetroGenderBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroGenderBo bo)
        {
            Result result = Result();

            PerLifeRetroGender entity = PerLifeRetroGender.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Per Life Retro Gender's record found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.Gender = bo.Gender;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeRetroGenderBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeRetroGenderBo bo)
        {
            PerLifeRetroGender.Delete(bo.Id);
        }

        public static Result Delete(PerLifeRetroGenderBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (PerLifeDataCorrectionService.CountByPerLifeRetroGenderId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeRetroGender.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
