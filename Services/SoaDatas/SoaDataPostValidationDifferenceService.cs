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
using System.Linq.Expressions;

namespace Services.SoaDatas
{
    public class SoaDataPostValidationDifferenceService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataPostValidationDifference)),
            };
        }

        public static Expression<Func<SoaDataPostValidationDifference, SoaDataPostValidationDifferenceBo>> Expression()
        {
            return entity => new SoaDataPostValidationDifferenceBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                Type = entity.Type,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                RiskMonth = entity.RiskMonth,

                GrossPremium = entity.GrossPremium,
                DifferenceNetTotalAmount = entity.DifferenceNetTotalAmount,
                DifferencePercetage = entity.DifferencePercetage,

                Remark = entity.Remark,
                Check = entity.Check,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SoaDataPostValidationDifferenceBo FormBo(SoaDataPostValidationDifference entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataPostValidationDifferenceBo
            {
                Id = entity.Id,

                SoaDataBatchId = entity.SoaDataBatchId,

                Type = entity.Type,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                RiskMonth = entity.RiskMonth,

                GrossPremium = entity.GrossPremium,
                DifferenceNetTotalAmount = entity.DifferenceNetTotalAmount,
                DifferencePercetage = entity.DifferencePercetage,

                Remark = entity.Remark,
                Check = entity.Check,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
            }

            return bo;
        }

        public static SoaDataPostValidationDifference FormEntity(SoaDataPostValidationDifferenceBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataPostValidationDifference
            {
                Id = bo.Id,

                SoaDataBatchId = bo.SoaDataBatchId,

                Type = bo.Type,

                BusinessCode = bo.BusinessCode,
                TreatyCode = bo.TreatyCode,
                SoaQuarter = bo.SoaQuarter,
                RiskQuarter = bo.RiskQuarter,
                RiskMonth = bo.RiskMonth,

                GrossPremium = bo.GrossPremium,
                DifferenceNetTotalAmount = bo.DifferenceNetTotalAmount,
                DifferencePercetage = bo.DifferencePercetage,

                Remark = bo.Remark,
                Check = bo.Check,

                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<SoaDataPostValidationDifferenceBo> FormBos(IList<SoaDataPostValidationDifference> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            var bos = new List<SoaDataPostValidationDifferenceBo>() { };
            foreach (var entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaDataPostValidationDifference.IsExists(id);
        }

        public static SoaDataPostValidationDifferenceBo Find(int id)
        {
            return FormBo(SoaDataPostValidationDifference.Find(id));
        }

        public static IList<SoaDataPostValidationDifferenceBo> GetBySoaDataBatchIdType(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataPostValidationDifferences
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.Type == type);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList(), false);
            }
        }

        public static IList<SoaDataPostValidationDifferenceBo> GetBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SoaDataPostValidationDifferences.Where(q => q.SoaDataBatchId == soaDataBatchId).ToList());
            }
        }

        public static int CountBySoaDataBatchIdType(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SoaDataPostValidationDifferences.Where(q => q.SoaDataBatchId == soaDataBatchId && q.Type == type);
                if (originalCurrency)
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return query.Count();
            }
        }

        public static Result Save(ref SoaDataPostValidationDifferenceBo bo)
        {
            if (!SoaDataPostValidationDifference.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataPostValidationDifferenceBo bo, ref TrailObject trail)
        {
            if (!SoaDataPostValidationDifference.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataPostValidationDifferenceBo bo)
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

        public static void Create(ref SoaDataPostValidationDifferenceBo bo, AppDbContext db)
        {
            var entity = FormEntity(bo);
            entity.Create();
            bo.Id = entity.Id;
        }

        public static Result Create(ref SoaDataPostValidationDifferenceBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataPostValidationDifferenceBo bo)
        {
            var result = Result();
            var entity = SoaDataPostValidationDifference.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;

                entity.Type = bo.Type;

                entity.BusinessCode = bo.BusinessCode;
                entity.TreatyCode = bo.TreatyCode;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.RiskMonth = bo.RiskMonth;

                entity.GrossPremium = bo.GrossPremium;
                entity.DifferenceNetTotalAmount = bo.DifferenceNetTotalAmount;
                entity.DifferencePercetage = bo.DifferencePercetage;

                entity.Remark = bo.Remark;
                entity.Check = bo.Check;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataPostValidationDifferenceBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataPostValidationDifferenceBo bo)
        {
            SoaDataPostValidationDifference.Delete(bo.Id);
        }

        public static Result Delete(SoaDataPostValidationDifferenceBo bo, ref TrailObject trail)
        {
            var result = Result();
            var dataTrail = SoaDataPostValidationDifference.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                //var query = db.SoaDataPostValidationDifferences.Where(q => q.SoaDataBatchId == soaDataBatchId);
                // DO NOT TRAIL since this is mass data
                /*
                foreach (var entity in query.ToList())
                {
                    var trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SoaDataPostValidationDifferences.Remove(entity);
                }
                */

                //db.SoaDataPostValidationDifferences.RemoveRange(query);

                db.Database.ExecuteSqlCommand("DELETE FROM [SoaDataPostValidationDifferences] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
