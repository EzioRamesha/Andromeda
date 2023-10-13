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
    public class TreatyPricingMedicalTableVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTableVersion.ToString()
            };
        }

        public static TreatyPricingMedicalTableVersionBo FormBo(TreatyPricingMedicalTableVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingMedicalTableVersionBo
            {
                Id = entity.Id,
                TreatyPricingMedicalTableId = entity.TreatyPricingMedicalTableId,
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
                bo.TreatyPricingMedicalTableBo = TreatyPricingMedicalTableService.Find(entity.TreatyPricingMedicalTableId);
            }

            return bo;
        }

        public static IList<TreatyPricingMedicalTableVersionBo> FormBos(IList<TreatyPricingMedicalTableVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableVersionBo> bos = new List<TreatyPricingMedicalTableVersionBo>() { };
            foreach (TreatyPricingMedicalTableVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingMedicalTableVersion FormEntity(TreatyPricingMedicalTableVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTableVersion
            {
                Id = bo.Id,
                TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId,
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
            return TreatyPricingMedicalTableVersion.IsExists(id);
        }

        public static TreatyPricingMedicalTableVersionBo Find(int id, bool foreign = false)
        {
            return FormBo(TreatyPricingMedicalTableVersion.Find(id), foreign);
        }

        public static IList<TreatyPricingMedicalTableVersionBo> GetByTreatyPricingMedicalTableId(int treatyPricingMedicalTableId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableVersions
                    .Where(q => q.TreatyPricingMedicalTableId == treatyPricingMedicalTableId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }
        public static IList<TreatyPricingMedicalTableVersionBo> GetByTreatyPricingCedantId(int treatyPricingCedantId, bool foreign = false)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTableVersions
                    .Where(q => q.TreatyPricingMedicalTable.TreatyPricingCedantId == treatyPricingCedantId)
                    .OrderBy(q => q.TreatyPricingMedicalTableId).ThenBy(q => q.Version)
                    .ToList(), foreign);
            }
        }

        public static int GetVersionId(int medicalTableId, int medicalTableVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableVersions
                    .FirstOrDefault(q => q.TreatyPricingMedicalTableId == medicalTableId
                        && q.Version == medicalTableVersion).Id;
            }
        }

        public static List<TreatyPricingMedicalTableVersion> GetVersionByMedicalTableVersionIdsMedicalTableId(int medicalTableId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTableVersions
                    .Where(q => q.TreatyPricingMedicalTableId == medicalTableId)
                    .Select(q => q)
                    .ToList();
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableVersionBo bo)
        {
            if (!TreatyPricingMedicalTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTableVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionBo bo)
        {
            TreatyPricingMedicalTableVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableVersionBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTableVersion entity = TreatyPricingMedicalTableVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingMedicalTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableVersionBo bo)
        {
            TreatyPricingMedicalTableVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTableVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingMedicalTableId(int treatyPricingMedicalTableId)
        {
            return TreatyPricingMedicalTableVersion.DeleteAllByTreatyPricingMedicalTableId(treatyPricingMedicalTableId);
        }

        public static void DeleteAllByTreatyPricingMedicalTableId(int treatyPricingMedicalTableId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingMedicalTableId(treatyPricingMedicalTableId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTableVersion)));
                }
            }
        }
    }
}
