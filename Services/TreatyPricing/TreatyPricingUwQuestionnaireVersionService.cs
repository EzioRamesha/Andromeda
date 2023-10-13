using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingUwQuestionnaireVersion.ToString()
            };
        }

        public static Expression<Func<TreatyPricingUwQuestionnaireVersion, TreatyPricingUwQuestionnaireVersionBo>> Expression()
        {
            return entity => new TreatyPricingUwQuestionnaireVersionBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireId = entity.TreatyPricingUwQuestionnaireId,
                Version = entity.Version,
                EffectiveAt = entity.EffectiveAt,
                Type = entity.Type,
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingUwQuestionnaireVersionBo FormBo(TreatyPricingUwQuestionnaireVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersionBo
            {
                Id = entity.Id,
                TreatyPricingUwQuestionnaireId = entity.TreatyPricingUwQuestionnaireId,
                TreatyPricingUwQuestionnaireBo = foreign ? TreatyPricingUwQuestionnaireService.Find(entity.TreatyPricingUwQuestionnaireId) : null,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeBo = UserService.Find(entity.PersonInChargeId),
                EffectiveAt = entity.EffectiveAt,
                Type = entity.Type,
                Remarks = entity.Remarks,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                EffectiveAtStr = entity.EffectiveAt?.ToString(Util.GetDateFormat()),
                TypeName = TreatyPricingUwQuestionnaireVersionBo.GetTypeName(entity.Type),
            };
        }

        public static IList<TreatyPricingUwQuestionnaireVersionBo> FormBos(IList<TreatyPricingUwQuestionnaireVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingUwQuestionnaireVersionBo> bos = new List<TreatyPricingUwQuestionnaireVersionBo>() { };
            foreach (TreatyPricingUwQuestionnaireVersion entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingUwQuestionnaireVersion FormEntity(TreatyPricingUwQuestionnaireVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingUwQuestionnaireVersion
            {
                Id = bo.Id,
                TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                Type = bo.Type,
                Remarks = bo.Remarks,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingUwQuestionnaireVersion.IsExists(id);
        }

        public static TreatyPricingUwQuestionnaireVersionBo Find(int? id, bool foreign = false)
        {
            return FormBo(TreatyPricingUwQuestionnaireVersion.Find(id), foreign);
        }

        public static TreatyPricingUwQuestionnaireVersionBo GetLatestVersionByTreatyPricingUwQuestionnaireId(int treatyPricingUwQuestionnaireId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingUwQuestionnaireVersions
                    .Where(q => q.TreatyPricingUwQuestionnaireId == treatyPricingUwQuestionnaireId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static int GetVersionId(int uwQuestionnaireId, int uwQuestionnaireVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingUwQuestionnaireVersions
                    .FirstOrDefault(q => q.TreatyPricingUwQuestionnaireId == uwQuestionnaireId
                        && q.Version == uwQuestionnaireVersion).Id;
            }
        }

        public static IList<TreatyPricingUwQuestionnaireVersionBo> GetByTreatyPricingUwQuestionnaireId(int treatyPricingUwQuestionnaireId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaireVersions
                    .Where(q => q.TreatyPricingUwQuestionnaireId == treatyPricingUwQuestionnaireId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingUwQuestionnaireVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingUwQuestionnaireVersions
                    .Where(q => q.TreatyPricingUwQuestionnaire.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingUwQuestionnaireId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static Result Save(ref TreatyPricingUwQuestionnaireVersionBo bo)
        {
            if (!TreatyPricingUwQuestionnaireVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingUwQuestionnaireVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingUwQuestionnaireVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionBo bo)
        {
            TreatyPricingUwQuestionnaireVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingUwQuestionnaireVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionBo bo)
        {
            Result result = Result();

            TreatyPricingUwQuestionnaireVersion entity = TreatyPricingUwQuestionnaireVersion.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingUwQuestionnaireVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingUwQuestionnaireVersionBo bo)
        {
            TreatyPricingUwQuestionnaireVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingUwQuestionnaireVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingUwQuestionnaireVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingUwQuestionnaireId(int treatyPricingUwQuestionnaireId)
        {
            return TreatyPricingUwQuestionnaireVersion.DeleteAllByTreatyPricingUwQuestionnaireId(treatyPricingUwQuestionnaireId);
        }

        public static void DeleteAllByTreatyPricingUwQuestionnaireId(int treatyPricingUwQuestionnaireId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingUwQuestionnaireId(treatyPricingUwQuestionnaireId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingUwQuestionnaireVersion)));
                }
            }
        }
    }
}
