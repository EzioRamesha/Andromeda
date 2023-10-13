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
    public class TreatyPricingFinancialTableVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTableVersion.ToString()
            };
        }

        public static TreatyPricingFinancialTableVersionBo FormBo(TreatyPricingFinancialTableVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingFinancialTableVersionBo
            {
                Id = entity.Id,
                TreatyPricingFinancialTableId = entity.TreatyPricingFinancialTableId,
                Version = entity.Version,
                PersonInChargeId = entity.PersonInChargeId,
                EffectiveAt = entity.EffectiveAt,
                Remarks = entity.Remarks,
                AggregationNote = entity.AggregationNote,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                PersonInChargeName = UserService.Find(entity.PersonInChargeId).FullName,
            };
            if (bo.EffectiveAt.HasValue)
                bo.EffectiveAtStr = bo.EffectiveAt.Value.ToString(Util.GetDateFormat());
            
            if (foreign)
            {
                bo.TreatyPricingFinancialTableBo = TreatyPricingFinancialTableService.Find(entity.TreatyPricingFinancialTableId);
            }

            return bo;
        }

        public static IList<TreatyPricingFinancialTableVersionBo> FormBos(IList<TreatyPricingFinancialTableVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableVersionBo> bos = new List<TreatyPricingFinancialTableVersionBo>() { };
            foreach (TreatyPricingFinancialTableVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingFinancialTableVersion FormEntity(TreatyPricingFinancialTableVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTableVersion
            {
                Id = bo.Id,
                TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId,
                Version = bo.Version,
                PersonInChargeId = bo.PersonInChargeId,
                EffectiveAt = bo.EffectiveAt,
                Remarks = bo.Remarks,
                AggregationNote = bo.AggregationNote,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingFinancialTableVersion.IsExists(id);
        }

        public static TreatyPricingFinancialTableVersionBo Find(int id, bool foreign = false)
        {
            return FormBo(TreatyPricingFinancialTableVersion.Find(id), foreign);
        }

        public static IList<TreatyPricingFinancialTableVersionBo> GetByTreatyPricingFinancialTableId(int treatyPricingFinancialTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableVersions
                    .Where(q => q.TreatyPricingFinancialTableId == treatyPricingFinancialTableId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static IList<TreatyPricingFinancialTableVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTableVersions
                    .Where(q => q.TreatyPricingFinancialTable.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingFinancialTableId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static int GetVersionId(int medicalTableId, int medicalTableVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableVersions
                    .FirstOrDefault(q => q.TreatyPricingFinancialTableId == medicalTableId
                        && q.Version == medicalTableVersion).Id;
            }
        }

        public static List<TreatyPricingFinancialTableVersion> GetVersionByFinancialTableVersionIdsFinancialTableId(int financialTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTableVersions
                    .Where(q => q.TreatyPricingFinancialTableId == financialTableId)
                    .Select(q => q)
                    .ToList();
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableVersionBo bo)
        {
            if (!TreatyPricingFinancialTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionBo bo)
        {
            TreatyPricingFinancialTableVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableVersionBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTableVersion entity = TreatyPricingFinancialTableVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingFinancialTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableVersionBo bo)
        {
            TreatyPricingFinancialTableVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTableVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingFinancialTableId(int treatyPricingFinancialTableId)
        {
            return TreatyPricingFinancialTableVersion.DeleteAllByTreatyPricingFinancialTableId(treatyPricingFinancialTableId);
        }

        public static void DeleteAllByTreatyPricingFinancialTableId(int treatyPricingFinancialTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingFinancialTableId(treatyPricingFinancialTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTableVersion)));
                }
            }
        }
    }
}
