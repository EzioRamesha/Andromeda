using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class PremiumSpreadTableDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PremiumSpreadTableDetail)),
                Controller = ModuleBo.ModuleController.PremiumSpreadTableDetail.ToString()
            };
        }

        public static Expression<Func<PremiumSpreadTableDetail, PremiumSpreadTableDetailBo>> Expression()
        {
            return entity => new PremiumSpreadTableDetailBo
            {
                Id = entity.Id,
                PremiumSpreadTableId = entity.PremiumSpreadTableId,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                AgeFrom = entity.AgeFrom,
                AgeTo = entity.AgeTo,
                PremiumSpread = entity.PremiumSpread,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PremiumSpreadTableDetailBo FormBo(PremiumSpreadTableDetail entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            PremiumSpreadTableDetailBo premiumSpreadTableDetailBo = new PremiumSpreadTableDetailBo
            {
                Id = entity.Id,
                PremiumSpreadTableId = entity.PremiumSpreadTableId,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                AgeFrom = entity.AgeFrom,
                AgeFromStr = entity.AgeFrom.ToString(),
                AgeTo = entity.AgeTo,
                AgeToStr = entity.AgeTo.ToString(),
                PremiumSpread = entity.PremiumSpread,
                PremiumSpreadStr = Util.DoubleToString(entity.PremiumSpread),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                premiumSpreadTableDetailBo.PremiumSpreadTableBo = PremiumSpreadTableService.Find(entity.PremiumSpreadTableId);
            }

            return premiumSpreadTableDetailBo;
        }

        public static IList<PremiumSpreadTableDetailBo> FormBos(IList<PremiumSpreadTableDetail> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<PremiumSpreadTableDetailBo> bos = new List<PremiumSpreadTableDetailBo>() { };
            foreach (PremiumSpreadTableDetail entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static PremiumSpreadTableDetail FormEntity(PremiumSpreadTableDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new PremiumSpreadTableDetail
            {
                Id = bo.Id,
                PremiumSpreadTableId = bo.PremiumSpreadTableId,
                CedingPlanCode = bo.CedingPlanCode,
                BenefitCode = bo.BenefitCode,
                AgeFrom = bo.AgeFrom,
                AgeTo = bo.AgeTo,
                PremiumSpread = bo.PremiumSpread,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PremiumSpreadTableDetail.IsExists(id);
        }

        public static PremiumSpreadTableDetailBo Find(int? id)
        {
            return FormBo(PremiumSpreadTableDetail.Find(id));
        }

        public static double? GetPremiumSpreadByParams(int premiumSpreadTableId, string cedingPlanCode, string mlreBenefitCode, int? age)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PremiumSpreadTableDetails
                    .Where(q => q.PremiumSpreadTableId == premiumSpreadTableId);

                if (age.HasValue)
                {
                    query = query.Where(q => (q.AgeFrom <= age && q.AgeTo >= age) || (q.AgeFrom == null && q.AgeTo == null));
                }
                else
                {
                    query = query.Where(q => q.AgeFrom == null && q.AgeTo == null);
                }

                var entities = query.ToList();
                if (entities.IsNullOrEmpty())
                    return null;

                var entity = entities.Where(q => string.IsNullOrEmpty(q.BenefitCode) || q.BenefitCode.Split(',').Select(r => r.Trim()).ToArray().Contains(mlreBenefitCode))
                    .Where(q => q.CedingPlanCode.Split(',').Select(r => r.Trim()).ToArray().Contains(cedingPlanCode))
                    .FirstOrDefault();

                return entity?.PremiumSpread;
            }
        }

        public static PremiumSpreadTableDetailBo GetByPremiumSpreadTableIdByParam(int premiumSpreadTableId, string cedingPlanCode, string mlreBenefitCode, int? age, bool foreign = true)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.PremiumSpreadTableDetails
                    .Where(q => q.PremiumSpreadTableId == premiumSpreadTableId);

                if (age.HasValue)
                {
                    query = query
                        .Where(q =>
                            (
                                q.AgeFrom <= age && q.AgeTo >= age
                                ||
                                q.AgeFrom <= age && q.AgeTo >= age
                            )
                            || (q.AgeFrom == null && q.AgeTo == null)
                        );
                }
                else
                {
                    query = query
                        .Where(q => q.AgeFrom == null && q.AgeTo == null);
                }

                var entities = query.ToList();
                if (entities.IsNullOrEmpty())
                    return null;

                var entity = entities.Where(q => string.IsNullOrEmpty(q.BenefitCode) || q.BenefitCode.Split(',').Select(r => r.Trim()).ToArray().Contains(mlreBenefitCode))
                    .Where(q => q.CedingPlanCode.Split(',').Select(r => r.Trim()).ToArray().Contains(cedingPlanCode))
                    .FirstOrDefault();

                return FormBo(entity, foreign);
            }
        }

        public static IList<PremiumSpreadTableDetailBo> Get()
        {
            return FormBos(PremiumSpreadTableDetail.Get());
        }

        public static IList<PremiumSpreadTableDetailBo> GetByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            return FormBos(PremiumSpreadTableDetail.GetByPremiumSpreadTableId(premiumSpreadTableId));
        }

        public static IList<PremiumSpreadTableDetailBo> GetByPremiumSpreadTableIdExcludeId(int premiumSpreadTableId, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PremiumSpreadTableDetails.Where(q => q.PremiumSpreadTableId == premiumSpreadTableId);

                if (id.HasValue)
                    query = query.Where(q => q.Id != id);

                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref PremiumSpreadTableDetailBo bo)
        {
            if (!PremiumSpreadTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref PremiumSpreadTableDetailBo bo, ref TrailObject trail)
        {
            if (!PremiumSpreadTableDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PremiumSpreadTableDetailBo bo)
        {
            PremiumSpreadTableDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PremiumSpreadTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PremiumSpreadTableDetailBo bo)
        {
            Result result = Result();

            PremiumSpreadTableDetail entity = PremiumSpreadTableDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.PremiumSpreadTableId = bo.PremiumSpreadTableId;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.BenefitCode = bo.BenefitCode;
                entity.AgeFrom = bo.AgeFrom;
                entity.AgeTo = bo.AgeTo;
                entity.PremiumSpread = bo.PremiumSpread;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref PremiumSpreadTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PremiumSpreadTableDetailBo bo)
        {
            PremiumSpreadTableDetail.Delete(bo.Id);
        }

        public static Result Delete(PremiumSpreadTableDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = PremiumSpreadTableDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByPremiumSpreadTableIdExcept(int premiumSpreadTableId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<PremiumSpreadTableDetail> premiumSpreadTableDetails = PremiumSpreadTableDetail.GetByPremiumSpreadTableIdExcept(premiumSpreadTableId, saveIds);
            foreach (PremiumSpreadTableDetail premiumSpreadTableDetail in premiumSpreadTableDetails)
            {
                DataTrail dataTrail = PremiumSpreadTableDetail.Delete(premiumSpreadTableDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            return PremiumSpreadTableDetail.DeleteByPremiumSpreadTableId(premiumSpreadTableId);
        }

        public static void DeleteByPremiumSpreadTableId(int premiumSpreadTableId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByPremiumSpreadTableId(premiumSpreadTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
