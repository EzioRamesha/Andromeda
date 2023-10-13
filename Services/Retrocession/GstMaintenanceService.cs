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
    public class GstMaintenanceService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(GstMaintenance)),
                Controller = ModuleBo.ModuleController.GstMaintenance.ToString()
            };
        }

        public static Expression<Func<GstMaintenance, GstMaintenanceBo>> Expression()
        {
            return entity => new GstMaintenanceBo
            {
                Id = entity.Id,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                RiskEffectiveStartDate = entity.EffectiveStartDate,
                RiskEffectiveEndDate = entity.RiskEffectiveEndDate,
                Rate = entity.Rate
            };
        }

        public static GstMaintenanceBo FormBo(GstMaintenance entity = null)
        {
            if (entity == null)
                return null;
            return new GstMaintenanceBo
            {
                Id = entity.Id,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                RiskEffectiveStartDate = entity.RiskEffectiveStartDate,
                RiskEffectiveEndDate = entity.RiskEffectiveEndDate,
                Rate = entity.Rate,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<GstMaintenanceBo> FormBos(IList<GstMaintenance> entities = null)
        {
            if (entities == null)
                return null;
            IList<GstMaintenanceBo> bos = new List<GstMaintenanceBo>() { };
            foreach (GstMaintenance entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static GstMaintenance FormEntity(GstMaintenanceBo bo = null)
        {
            if (bo == null)
                return null;
            return new GstMaintenance
            {
                Id = bo.Id,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                RiskEffectiveStartDate = bo.RiskEffectiveStartDate,
                RiskEffectiveEndDate = bo.RiskEffectiveEndDate,
                Rate = bo.Rate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return GstMaintenance.IsExists(id);
        }

        public static GstMaintenanceBo Find(int? id)
        {
            return FormBo(GstMaintenance.Find(id));
        }

        public static IList<GstMaintenanceBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.GstMaintenances.OrderBy(q => q.Id).ToList());
            }
        }

        public static IList<GstMaintenanceBo> GetByParams(DateTime startDate, DateTime endDate)
        {
            using (var db = new AppDbContext())
            {
                var query = db.GstMaintenances
                    .Where(q => q.RiskEffectiveStartDate <= startDate)
                    .Where(q => q.RiskEffectiveEndDate >= endDate);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref GstMaintenanceBo bo)
        {
            if (!GstMaintenance.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref GstMaintenanceBo bo, ref TrailObject trail)
        {
            if (!GstMaintenance.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(GstMaintenance gstMaintenance)
        {
            return gstMaintenance.IsDuplicate();
        }

        public static Result Create(ref GstMaintenanceBo bo)
        {
            GstMaintenance entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing GST Maintenance's record found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref GstMaintenanceBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref GstMaintenanceBo bo)
        {
            Result result = Result();

            GstMaintenance entity = GstMaintenance.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing GST Maintenance's record found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.EffectiveStartDate = bo.EffectiveStartDate;
                entity.EffectiveEndDate = bo.EffectiveEndDate;
                entity.RiskEffectiveStartDate = bo.RiskEffectiveStartDate;
                entity.RiskEffectiveEndDate = bo.RiskEffectiveEndDate;
                entity.Rate = bo.Rate;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref GstMaintenanceBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(GstMaintenanceBo bo)
        {
            GstMaintenance.Delete(bo.Id);
        }

        public static Result Delete(GstMaintenanceBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = GstMaintenance.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
