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
    public class TreatyPricingUwQuestionnaireVersionDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersionDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwQuestionnaireVersionDetail.ToString()
            };
        }

        public static Expression<Func<TreatyPricingUwQuestionnaireVersionDetail, TreatyPricingUwQuestionnaireVersionDetailBo>> Expression()
        {
            return entity => new TreatyPricingUwQuestionnaireVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                UwQuestionnaireCategoryId = entity.UwQuestionnaireCategoryId,
                Question = entity.Question,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingUwQuestionnaireVersionDetailBo FormBo(TreatyPricingUwQuestionnaireVersionDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireVersionId = entity.TreatyPricingUwQuestionnaireVersionId,
                TreatyPricingUwQuestionnaireVersionBo = TreatyPricingUwQuestionnaireVersionService.Find(entity.TreatyPricingUwQuestionnaireVersionId),
                UwQuestionnaireCategoryId = entity.UwQuestionnaireCategoryId,
                UwQuestionnaireCategoryBo = UwQuestionnaireCategoryService.Find(entity.UwQuestionnaireCategoryId),
                Question = entity.Question,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingUwQuestionnaireVersionDetailBo> FormBos(IList<TreatyPricingUwQuestionnaireVersionDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwQuestionnaireVersionDetailBo> bos = new List<TreatyPricingUwQuestionnaireVersionDetailBo>() { };
            foreach (TreatyPricingUwQuestionnaireVersionDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingUwQuestionnaireVersionDetail FormEntity(TreatyPricingUwQuestionnaireVersionDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersionDetail
            {
                Id = bo.Id,
                TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId,
                UwQuestionnaireCategoryId = bo.UwQuestionnaireCategoryId,
                Question = bo.Question,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwQuestionnaireVersionDetail.IsExists(id);
        }

        public static TreatyPricingUwQuestionnaireVersionDetailBo Find(int id)
        {
            return FormBo(TreatyPricingUwQuestionnaireVersionDetail.Find(id));
        }

        public static IList<TreatyPricingUwQuestionnaireVersionDetailBo> GetByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaireVersionDetails
                    .Where(q => q.TreatyPricingUwQuestionnaireVersionId == treatyPricingUwQuestionnaireVersionId)
                    .ToList());
            }
        }

        public static int CountByUwQuestionnaireCategoryId(int uwQuestionnaireCategoryId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaireVersionDetails
                    .Where(q => q.UwQuestionnaireCategoryId == uwQuestionnaireCategoryId)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingUwQuestionnaireVersionDetailBo bo)
        {
            if (!TreatyPricingUwQuestionnaireVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwQuestionnaireVersionDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwQuestionnaireVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionDetailBo bo)
        {
            TreatyPricingUwQuestionnaireVersionDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionDetailBo bo)
        {
            Result result = Result();

            TreatyPricingUwQuestionnaireVersionDetail entity = TreatyPricingUwQuestionnaireVersionDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId;
                entity.UwQuestionnaireCategoryId = bo.UwQuestionnaireCategoryId;
                entity.Question = bo.Question;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwQuestionnaireVersionDetailBo bo)
        {
            TreatyPricingUwQuestionnaireVersionDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwQuestionnaireVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwQuestionnaireVersionDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId)
        {
            return TreatyPricingUwQuestionnaireVersionDetail.DeleteAllByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);
        }

        public static void DeleteAllByTreatyPricingUwQuestionnaireVersionId(int treatyPricingUwQuestionnaireVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingUwQuestionnaireVersionId(treatyPricingUwQuestionnaireVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersionDetail)));
                }
            }
        }
    }
}
