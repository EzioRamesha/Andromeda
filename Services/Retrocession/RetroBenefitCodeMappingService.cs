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
    public class RetroBenefitCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroBenefitCodeMapping)),
                Controller = ModuleBo.ModuleController.RetroBenefitCodeMapping.ToString()
            };
        }

        public static Expression<Func<RetroBenefitCodeMapping, RetroBenefitCodeMappingBo>> Expression()
        {
            return entity => new RetroBenefitCodeMappingBo
            {
                Id = entity.Id,
                BenefitId = entity.BenefitId,
                IsPerAnnum = entity.IsPerAnnum,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static RetroBenefitCodeMappingBo FormBo(RetroBenefitCodeMapping entity = null)
        {
            if (entity == null)
                return null;
            return new RetroBenefitCodeMappingBo
            {
                Id = entity.Id,
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                IsPerAnnum = entity.IsPerAnnum,
                TreatyCode = string.Join(",", RetroBenefitCodeMappingTreatyService.GetTreatyCodeByRetroBenefitCodeMappingId(entity.Id)),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroBenefitCodeMappingBo> FormBos(IList<RetroBenefitCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroBenefitCodeMappingBo> bos = new List<RetroBenefitCodeMappingBo>() { };
            foreach (RetroBenefitCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroBenefitCodeMapping FormEntity(RetroBenefitCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroBenefitCodeMapping
            {
                Id = bo.Id,
                BenefitId = bo.BenefitId,
                IsPerAnnum = bo.IsPerAnnum,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return RetroBenefitCodeMapping.IsExists(id);
        }

        public static RetroBenefitCodeMappingBo Find(int? id)
        {
            return FormBo(RetroBenefitCodeMapping.Find(id));
        }

        public static IList<RetroBenefitCodeMappingBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.RetroBenefitCodeMappings.OrderBy(q => q.Benefit.Code).ToList());
            }
        }

        public static int CountByBenefitId(int benefitId)
        {
            using (var db = new AppDbContext())
            {
                return db.RetroBenefitCodeMappings.Where(q => q.BenefitId == benefitId).Count();
            }
        }

        public static Result Save(ref RetroBenefitCodeMappingBo bo)
        {
            if (!RetroBenefitCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            if (!RetroBenefitCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(RetroBenefitCodeMapping retroBenefitCodeMapping)
        {
            return retroBenefitCodeMapping.IsDuplicate();
        }

        public static Result Create(ref RetroBenefitCodeMappingBo bo)
        {
            RetroBenefitCodeMapping entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Retro Benefit Code Mapping's record found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingBo bo)
        {
            Result result = Result();

            RetroBenefitCodeMapping entity = RetroBenefitCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Retro Benefit Code Mapping's record found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.BenefitId = bo.BenefitId;
                entity.IsPerAnnum = bo.IsPerAnnum;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroBenefitCodeMappingBo bo)
        {
            RetroBenefitCodeMappingTreatyService.DeleteByRetroBenefitCodeMappingId(bo.Id);
            RetroBenefitCodeMappingDetailService.DeleteByRetroBenefitCodeMappingId(bo.Id);
            RetroBenefitCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(RetroBenefitCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                RetroBenefitCodeMappingTreatyService.DeleteByRetroBenefitCodeMappingId(bo.Id);
                RetroBenefitCodeMappingDetailService.DeleteByRetroBenefitCodeMappingId(bo.Id);
                DataTrail dataTrail = RetroBenefitCodeMapping.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
