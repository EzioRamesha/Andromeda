using BusinessObject;
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
    public class RetroBenefitCodeMappingTreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitCodeMappingTreaty)),
                Controller = ModuleBo.ModuleController.RetroBenefitCodeMappingTreaty.ToString()
            };
        }

        public static Expression<Func<RetroBenefitCodeMappingTreaty, RetroBenefitCodeMappingTreatyBo>> Expression()
        {
            return entity => new RetroBenefitCodeMappingTreatyBo
            {
                Id = entity.Id,
                RetroBenefitCodeMappingId = entity.RetroBenefitCodeMappingId,
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroBenefitCodeMappingTreatyBo FormBo(RetroBenefitCodeMappingTreaty entity = null)
        {
            if (entity == null)
                return null;

            return new RetroBenefitCodeMappingTreatyBo
            {
                Id = entity.Id,
                RetroBenefitCodeMappingId = entity.RetroBenefitCodeMappingId,
                //RetroBenefitCodeMappingBo = RetroBenefitCodeMappingService.Find(entity.RetroBenefitCodeMappingId),
                TreatyCode = entity.TreatyCode,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitCodeMappingTreatyBo> FormBos(IList<RetroBenefitCodeMappingTreaty> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitCodeMappingTreatyBo> bos = new List<RetroBenefitCodeMappingTreatyBo>() { };
            foreach (RetroBenefitCodeMappingTreaty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitCodeMappingTreaty FormEntity(RetroBenefitCodeMappingTreatyBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitCodeMappingTreaty
            {
                Id = bo.Id,
                RetroBenefitCodeMappingId = bo.RetroBenefitCodeMappingId,
                TreatyCode = bo.TreatyCode,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitCodeMappingTreaty.IsExists(id);
        }

        public static RetroBenefitCodeMappingTreatyBo Find(int id)
        {
            return FormBo(RetroBenefitCodeMappingTreaty.Find(id));
        }

        public static IList<RetroBenefitCodeMappingTreatyBo> GetByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitCodeMappingTreaties.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId).ToList());
            }
        }

        public static List<string> GetTreatyCodeByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappingTreaties.Where(q => q.RetroBenefitCodeMappingId == retroBenefitCodeMappingId).Select(q => q.TreatyCode).ToList();
            }
        }

        public static Result Save(ref RetroBenefitCodeMappingTreatyBo bo)
        {
            if (!RetroBenefitCodeMappingTreaty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref RetroBenefitCodeMappingTreatyBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitCodeMappingTreaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroBenefitCodeMappingTreatyBo bo)
        {
            RetroBenefitCodeMappingTreaty entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroBenefitCodeMappingTreatyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingTreatyBo bo)
        {
            Result result = Result();

            RetroBenefitCodeMappingTreaty entity = RetroBenefitCodeMappingTreaty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RetroBenefitCodeMappingId = bo.RetroBenefitCodeMappingId;
                entity.TreatyCode = bo.TreatyCode;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingTreatyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitCodeMappingTreatyBo bo)
        {
            RetroBenefitCodeMappingTreaty.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitCodeMappingTreatyBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = RetroBenefitCodeMappingTreaty.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId)
        {
            return RetroBenefitCodeMappingTreaty.DeleteByRetroBenefitCodeMappingId(retroBenefitCodeMappingId);
        }

        public static void DeleteByRetroBenefitCodeMappingId(int retroBenefitCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByRetroBenefitCodeMappingId(retroBenefitCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroBenefitCodeMappingTreaty)));
                }
            }
        }
    }
}
