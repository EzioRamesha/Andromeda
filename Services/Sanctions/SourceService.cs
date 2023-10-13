using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SourceService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Source)),
                Controller = ModuleBo.ModuleController.Source.ToString()
            };
        }

        public static Expression<Func<Source, SourceBo>> Expression()
        {
            return entity => new SourceBo
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SourceBo FormBo(Source entity = null)
        {
            if (entity == null)
                return null;
            return new SourceBo
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SourceBo> FormBos(IList<Source> entities = null)
        {
            if (entities == null)
                return null;
            IList<SourceBo> bos = new List<SourceBo>() { };
            foreach (Source entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Source FormEntity(SourceBo bo = null)
        {
            if (bo == null)
                return null;
            return new Source
            {
                Id = bo.Id,
                Name = bo.Name?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Source.IsExists(id);
        }

        public static SourceBo Find(int id)
        {
            return FormBo(Source.Find(id));
        }

        public static IList<SourceBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Sources.ToList());
            }
        }

        public static IList<SourceBo> GetForAutoSchedule()
        {
            using (var db = new AppDbContext())
            {
                var sourceIds = db.SanctionBatches
                    .Where(q => q.Status == SanctionBatchBo.StatusSuccess)
                    .Where(q => q.Record > 0)
                    .Select(q => q.SourceId)
                    .ToList();

                var query = db.Sources.Where(q => sourceIds.Contains(q.Id));

                return FormBos(query.ToList());
            }
        }

        public static bool IsDuplicateName(Source source)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(source.Name?.Trim()))
                {
                    var query = db.Sources.Where(q => q.Name.Trim().Equals(source.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (source.Id != 0)
                    {
                        query = query.Where(q => q.Id != source.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static Result Save(ref SourceBo bo)
        {
            if (!Source.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SourceBo bo, ref TrailObject trail)
        {
            if (!Source.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SourceBo bo)
        {
            Source entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateName(entity))
            {
                result.AddTakenError("Source", bo.Name);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SourceBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SourceBo bo)
        {
            Result result = Result();

            Source entity = Source.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateName(FormEntity(bo)))
            {
                result.AddTakenError("Source", bo.Name);
            }

            if (result.Valid)
            {
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SourceBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SourceBo bo)
        {
            Source.Delete(bo.Id);
        }

        public static Result Delete(SourceBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (bo.Id == SourceBo.TypeUN || bo.Id == SourceBo.TypeOFAC)
            {
                result.AddError("Preset Source cannot be Deleted");
            }
            else
            {
                if (SanctionBatchService.CountBySourceId(bo.Id) > 0 ||
               SanctionVerificationService.CountBySourceId(bo.Id) > 0)
                {
                    result.AddErrorRecordInUsed();
                }
            }

            if (result.Valid)
            {
                DataTrail dataTrail = Source.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
