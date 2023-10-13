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
    public class TreatyPricingReportGenerationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingReportGeneration)),
                Controller = ModuleBo.ModuleController.TreatyPricingReportGeneration.ToString()
            };
        }

        public static Expression<Func<TreatyPricingReportGeneration, TreatyPricingReportGenerationBo>> Expression()
        {
            return entity => new TreatyPricingReportGenerationBo
            {
                Id = entity.Id,
                ReportName = entity.ReportName,
                ReportParams = entity.ReportParams,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingReportGenerationBo FormBo(TreatyPricingReportGeneration entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingReportGenerationBo
            {
                Id = entity.Id,
                ReportName = entity.ReportName,
                ReportParams = entity.ReportParams,
                FileName = entity.FileName,
                HashFileName = entity.HashFileName,
                Errors = entity.Errors,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                StatusName = TreatyPricingReportGenerationBo.GetStatusName(entity.Status),
                CreatedByName = UserService.Find(entity.CreatedById).FullName,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
            };
        }

        public static IList<TreatyPricingReportGenerationBo> FormBos(IList<TreatyPricingReportGeneration> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingReportGenerationBo> bos = new List<TreatyPricingReportGenerationBo>() { };
            foreach (TreatyPricingReportGeneration entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingReportGeneration FormEntity(TreatyPricingReportGenerationBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingReportGeneration
            {
                Id = bo.Id,
                ReportName = bo.ReportName,
                ReportParams = bo.ReportParams,
                FileName = bo.FileName,
                HashFileName = bo.HashFileName,
                Errors = bo.Errors,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingReportGeneration.IsExists(id);
        }

        public static TreatyPricingReportGenerationBo Find(int id)
        {
            return FormBo(TreatyPricingReportGeneration.Find(id));
        }

        public static TreatyPricingReportGenerationBo FindByStatus(int status)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.TreatyPricingReportGenerations.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingReportGenerationBo> GetByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingReportGenerations.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingReportGenerationBo> GetByReportName(string reportName)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingReportGenerations.Where(q => q.ReportName == reportName).ToList());
            }
        }

        public static List<string> GetDistinctReportName()
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingReportGenerations
                    .OrderBy(q => q.ReportName)
                    .Select(q => q.ReportName)
                    .Distinct()
                    .ToList();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingReportGenerations.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingReportGenerationBo bo)
        {
            if (!TreatyPricingReportGeneration.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingReportGenerationBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingReportGeneration.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingReportGenerationBo bo)
        {
            TreatyPricingReportGeneration entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingReportGenerationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingReportGenerationBo bo)
        {
            Result result = Result();

            TreatyPricingReportGeneration entity = TreatyPricingReportGeneration.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.ReportName = bo.ReportName;
                entity.ReportParams = bo.ReportParams;
                entity.FileName = bo.FileName;
                entity.HashFileName = bo.HashFileName;
                entity.Status = bo.Status;
                entity.Errors = bo.Errors;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingReportGenerationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingReportGenerationBo bo)
        {
            TreatyPricingReportGeneration.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingReportGenerationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingReportGeneration.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
