using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class UwQuestionnaireCategoryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(UwQuestionnaireCategory)),
                Controller = ModuleBo.ModuleController.UwQuestionnaireCategory.ToString()
            };
        }

        public static Expression<Func<UwQuestionnaireCategory, UwQuestionnaireCategoryBo>> Expression()
        {
            return entity => new UwQuestionnaireCategoryBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static UwQuestionnaireCategoryBo FormBo(UwQuestionnaireCategory entity = null)
        {
            if (entity == null)
                return null;
            return new UwQuestionnaireCategoryBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<UwQuestionnaireCategoryBo> FormBos(IList<UwQuestionnaireCategory> entities = null)
        {
            if (entities == null)
                return null;
            IList<UwQuestionnaireCategoryBo> bos = new List<UwQuestionnaireCategoryBo>() { };
            foreach (UwQuestionnaireCategory entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static UwQuestionnaireCategory FormEntity(UwQuestionnaireCategoryBo bo = null)
        {
            if (bo == null)
                return null;
            return new UwQuestionnaireCategory
            {
                Id = bo.Id,
                Code = bo.Code,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return UwQuestionnaireCategory.IsExists(id);
        }

        public static UwQuestionnaireCategoryBo Find(int? id)
        {
            return FormBo(UwQuestionnaireCategory.Find(id));
        }

        public static IList<UwQuestionnaireCategoryBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.UwQuestionnaireCategories.OrderBy(q => q.Code).ToList());
            }
        }

        public static UwQuestionnaireCategoryBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.UwQuestionnaireCategories.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static Result Save(ref UwQuestionnaireCategoryBo bo)
        {
            if (!UwQuestionnaireCategory.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref UwQuestionnaireCategoryBo bo, ref TrailObject trail)
        {
            if (!UwQuestionnaireCategory.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(UwQuestionnaireCategory uwQuestionnaireCategory)
        {
            return uwQuestionnaireCategory.IsDuplicateCode();
        }

        public static bool IsDuplicateName(UwQuestionnaireCategory uwQuestionnaireCategory)
        {
            return uwQuestionnaireCategory.IsDuplicateName();
        }

        public static Result Create(ref UwQuestionnaireCategoryBo bo)
        {
            UwQuestionnaireCategory entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (IsDuplicateName(entity))
            {
                result.AddTakenError("Name", bo.Name);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref UwQuestionnaireCategoryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref UwQuestionnaireCategoryBo bo)
        {
            Result result = Result();

            UwQuestionnaireCategory entity = UwQuestionnaireCategory.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (IsDuplicateName(FormEntity(bo)))
            {
                result.AddTakenError("Name", bo.Name);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref UwQuestionnaireCategoryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(UwQuestionnaireCategoryBo bo)
        {
            UwQuestionnaireCategory.Delete(bo.Id);
        }

        public static Result Delete(UwQuestionnaireCategoryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (TreatyPricingUwQuestionnaireVersionDetailService.CountByUwQuestionnaireCategoryId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = UwQuestionnaireCategory.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
