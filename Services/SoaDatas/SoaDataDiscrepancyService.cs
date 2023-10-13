using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.SoaDatas
{

    public class SoaDataDiscrepancyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataDiscrepancy)),
            };
        }

        public static SoaDataDiscrepancyBo FormBo(SoaDataDiscrepancy entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataDiscrepancyBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                Type = entity.Type,
                TreatyCode = entity.TreatyCode,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CedingPlanCode = entity.CedingPlanCode,
                CedantAmount = entity.CedantAmount,
                MlreChecking = entity.MlreChecking,
                Discrepancy = entity.Discrepancy,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
            }

            return bo;
        }

        public static SoaDataDiscrepancy FormEntity(SoaDataDiscrepancyBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataDiscrepancy
            {
                Id = bo.Id,
                SoaDataBatchId = bo.SoaDataBatchId,
                Type = bo.Type,
                TreatyCode = bo.TreatyCode,
                CedingPlanCode = bo.CedingPlanCode,
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                CedantAmount = bo.CedantAmount,
                MlreChecking = bo.MlreChecking,
                Discrepancy = bo.Discrepancy,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<SoaDataDiscrepancyBo> FormBos(IList<SoaDataDiscrepancy> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            var bos = new List<SoaDataDiscrepancyBo>() { };
            foreach (var entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaDataDiscrepancy.IsExists(id);
        }

        public static SoaDataDiscrepancyBo Find(int id)
        {
            return FormBo(SoaDataDiscrepancy.Find(id));
        }

        public static IList<SoaDataDiscrepancyBo> GetBySoaDataBatchIdType(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataDiscrepancies
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.Type == type);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList(), false);
            }
        }

        public static IList<SoaDataDiscrepancyBo> GetBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SoaDataDiscrepancies.Where(q => q.SoaDataBatchId == soaDataBatchId).ToList());
            }
        }

        public static Result Save(ref SoaDataDiscrepancyBo bo)
        {
            if (!SoaDataDiscrepancy.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataDiscrepancyBo bo, ref TrailObject trail)
        {
            if (!SoaDataDiscrepancy.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataDiscrepancyBo bo)
        {
            var entity = FormEntity(bo);
            var result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static void Create(ref SoaDataDiscrepancyBo bo, AppDbContext db)
        {
            var entity = FormEntity(bo);
            entity.Create();
            bo.Id = entity.Id;
        }

        public static Result Create(ref SoaDataDiscrepancyBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataDiscrepancyBo bo)
        {
            var result = Result();
            var entity = SoaDataDiscrepancy.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.Type = bo.Type;
                entity.TreatyCode = bo.TreatyCode;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.CedantAmount = bo.CedantAmount;
                entity.MlreChecking = bo.MlreChecking;
                entity.Discrepancy = bo.Discrepancy;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataDiscrepancyBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataDiscrepancyBo bo)
        {
            SoaDataDiscrepancy.Delete(bo.Id);
        }

        public static Result Delete(SoaDataDiscrepancyBo bo, ref TrailObject trail)
        {
            var result = Result();
            var dataTrail = SoaDataDiscrepancy.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                db.Database.ExecuteSqlCommand("DELETE FROM [SoaDataDiscrepancies] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
