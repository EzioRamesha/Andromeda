using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeSoaService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeSoa)),
                Controller = ModuleBo.ModuleController.PerLifeSoa.ToString()
            };
        }

        public static Expression<Func<PerLifeSoa, PerLifeSoaBo>> Expression()
        {
            return entity => new PerLifeSoaBo
            {
                Id = entity.Id,
                RetroPartyId = entity.RetroPartyId,
                RetroTreatyId = entity.RetroTreatyId,
                Status = entity.Status,
                SoaQuarter = entity.SoaQuarter,
                InvoiceStatus = entity.InvoiceStatus,
                PersonInChargeId = entity.PersonInChargeId,
                ProcessingDate = entity.ProcessingDate,
                IsProfitCommissionData = entity.IsProfitCommissionData,
                PerLifeAggregationId = entity.PerLifeAggregationId,
            };
        }

        public static PerLifeSoaBo FormBo(PerLifeSoa entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeSoaBo
            {
                Id = entity.Id,
                RetroPartyId = entity.RetroPartyId,
                RetroTreatyId = entity.RetroTreatyId,
                Status = entity.Status,
                SoaQuarter = entity.SoaQuarter,
                InvoiceStatus = entity.InvoiceStatus,
                PersonInChargeId = entity.PersonInChargeId,
                ProcessingDate = entity.ProcessingDate,
                IsProfitCommissionData = entity.IsProfitCommissionData,
                PerLifeAggregationId = entity.PerLifeAggregationId,

                ProcessingDateStr = entity.ProcessingDate?.ToString(Util.GetDateFormat()),

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            bo.GetQuarterObject();
            if (foreign)
            {
                bo.RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId);
                bo.RetroTreatyBo = RetroTreatyService.Find(entity.RetroTreatyId);
                bo.PersonInChargeBo = UserService.Find(entity.PersonInChargeId);
            }
            return bo;
        }

        public static IList<PerLifeSoaBo> FormBos(IList<PerLifeSoa> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeSoaBo> bos = new List<PerLifeSoaBo>() { };
            foreach (PerLifeSoa entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeSoa FormEntity(PerLifeSoaBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeSoa
            {
                Id = bo.Id,
                RetroPartyId = bo.RetroPartyId,
                RetroTreatyId = bo.RetroTreatyId,
                Status = bo.Status,
                SoaQuarter = bo.SoaQuarter,
                InvoiceStatus = bo.InvoiceStatus,
                PersonInChargeId = bo.PersonInChargeId,
                ProcessingDate = bo.ProcessingDate,
                IsProfitCommissionData = bo.IsProfitCommissionData,
                PerLifeAggregationId = bo.PerLifeAggregationId,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeSoa.IsExists(id);
        }

        public static PerLifeSoaBo Find(int? id)
        {
            return FormBo(PerLifeSoa.Find(id));
        }

        public static PerLifeSoaBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeSoa
                    .Where(q => q.Status == status)
                    .FirstOrDefault());
            }
        }

        public static IList<PerLifeSoaBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoa.OrderBy(q => q.Id).ToList());
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoa.Where(q => q.Status == status).Count();
            }
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoa.Where(q => q.RetroPartyId == retroPartyId).Count();
            }
        }

        public static int CountByRetroTreatyId(int retroTreatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeSoa.Where(q => q.RetroTreatyId == retroTreatyId).Count();
            }
        }

        public static Result Save(ref PerLifeSoaBo bo)
        {
            if (!PerLifeSoa.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeSoaBo bo, ref TrailObject trail)
        {
            if (!PerLifeSoa.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeSoaBo bo)
        {
            PerLifeSoa entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeSoaBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeSoaBo bo)
        {
            Result result = Result();

            PerLifeSoa entity = PerLifeSoa.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.RetroPartyId = bo.RetroPartyId;
                entity.RetroTreatyId = bo.RetroTreatyId;
                entity.Status = bo.Status;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.InvoiceStatus = bo.InvoiceStatus;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.ProcessingDate = bo.ProcessingDate;
                entity.IsProfitCommissionData = bo.IsProfitCommissionData;
                entity.PerLifeAggregationId = bo.PerLifeAggregationId;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeSoaBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeSoaBo bo)
        {
            PerLifeSoa.Delete(bo.Id);
        }

        public static Result Delete(PerLifeSoaBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeSoa.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
