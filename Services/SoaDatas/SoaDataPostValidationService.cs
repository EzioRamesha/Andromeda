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
    public class SoaDataPostValidationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataPostValidation)),
            };
        }

        public static Expression<Func<SoaDataPostValidation, SoaDataPostValidationBo>> Expression()
        {
            return entity => new SoaDataPostValidationBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                Type = entity.Type,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                RiskMonth = entity.RiskMonth,

                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,

                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,
                TotalDiscount = entity.TotalDiscount,

                NoClaimBonus = entity.NoClaimBonus,
                Claim = entity.Claim,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                NetTotalAmount = entity.NetTotalAmount,

                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,

                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,

                NbSar = entity.NbSar,
                RnSar = entity.RnSar,
                AltSar = entity.AltSar,

                DTH = entity.DTH,
                TPA = entity.TPA,
                TPS = entity.TPS,
                PPD = entity.PPD,
                CCA = entity.CCA,
                CCS = entity.CCS,
                PA = entity.PA,
                HS = entity.HS,
                TPD = entity.TPD,
                CI = entity.CI,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SoaDataPostValidationBo FormBo(SoaDataPostValidation entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataPostValidationBo
            {
                Id = entity.Id,

                SoaDataBatchId = entity.SoaDataBatchId,

                Type = entity.Type,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                RiskMonth = entity.RiskMonth,

                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,

                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,

                TotalDiscount = entity.TotalDiscount,

                NoClaimBonus = entity.NoClaimBonus,
                Claim = entity.Claim,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                NetTotalAmount = entity.NetTotalAmount,

                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,

                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,

                NbSar = entity.NbSar,
                RnSar = entity.RnSar,
                AltSar = entity.AltSar,

                DTH = entity.DTH,
                TPA = entity.TPA,
                TPS = entity.TPS,
                PPD = entity.PPD,
                CCA = entity.CCA,
                CCS = entity.CCS,
                PA = entity.PA,
                HS = entity.HS,
                TPD = entity.TPD,
                CI = entity.CI,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
            }

            return bo;
        }

        public static SoaDataPostValidation FormEntity(SoaDataPostValidationBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataPostValidation
            {
                Id = bo.Id,

                SoaDataBatchId = bo.SoaDataBatchId,

                Type = bo.Type,

                BusinessCode = bo.BusinessCode,
                TreatyCode = bo.TreatyCode,
                SoaQuarter = bo.SoaQuarter,
                RiskQuarter = bo.RiskQuarter,
                RiskMonth = bo.RiskMonth,

                NbPremium = bo.NbPremium,
                RnPremium = bo.RnPremium,
                AltPremium = bo.AltPremium,
                GrossPremium = bo.GrossPremium,

                NbDiscount = bo.NbDiscount,
                RnDiscount = bo.RnDiscount,
                AltDiscount = bo.AltDiscount,

                TotalDiscount = bo.TotalDiscount,

                NoClaimBonus = bo.NoClaimBonus,
                Claim = bo.Claim,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                NetTotalAmount = bo.NetTotalAmount,

                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,

                NbCession = bo.NbCession,
                RnCession = bo.RnCession,
                AltCession = bo.AltCession,

                NbSar = bo.NbSar,
                RnSar = bo.RnSar,
                AltSar = bo.AltSar,

                DTH = bo.DTH,
                TPA = bo.TPA,
                TPS = bo.TPS,
                PPD = bo.PPD,
                CCA = bo.CCA,
                CCS = bo.CCS,
                PA = bo.PA,
                HS = bo.HS,
                TPD = bo.TPD,
                CI = bo.CI,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<SoaDataPostValidationBo> FormBos(IList<SoaDataPostValidation> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<SoaDataPostValidationBo> bos = new List<SoaDataPostValidationBo>() { };
            foreach (SoaDataPostValidation entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaDataPostValidation.IsExists(id);
        }

        public static SoaDataPostValidationBo Find(int id)
        {
            return FormBo(SoaDataPostValidation.Find(id));
        }

        public static IList<SoaDataPostValidationBo> GetBySoaDataBatchIdTypeCurrencyCode(int soaDataBatchId, int type, bool originalCurrency)
        {
            return FormBos(SoaDataPostValidation.GetBySoaDataBatchIdTypeCurrencyCode(soaDataBatchId, type, originalCurrency), false);
        }

        public static int CountBySoaDataBatchIdType(int soaDataBatchId, int type)
        {
            using (var db = new AppDbContext())
            {
                return db.SoaDataPostValidations.Where(q => q.SoaDataBatchId == soaDataBatchId && q.Type == type).Count();
            }
        }

        public static Result Save(ref SoaDataPostValidationBo bo)
        {
            if (!SoaDataPostValidation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataPostValidationBo bo, ref TrailObject trail)
        {
            if (!SoaDataPostValidation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataPostValidationBo bo)
        {
            SoaDataPostValidation entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref SoaDataPostValidationBo bo, AppDbContext db)
        {
            SoaDataPostValidation entity = FormEntity(bo);
            entity.Create();
            bo.Id = entity.Id;
        }

        public static Result Create(ref SoaDataPostValidationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataPostValidationBo bo)
        {
            Result result = Result();

            SoaDataPostValidation entity = SoaDataPostValidation.Find(bo.Id);
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

                entity.NbPremium = bo.NbPremium;
                entity.RnPremium = bo.RnPremium;
                entity.AltPremium = bo.AltPremium;
                entity.GrossPremium = bo.GrossPremium;

                entity.NbDiscount = bo.NbDiscount;
                entity.RnDiscount = bo.RnDiscount;
                entity.AltDiscount = bo.AltDiscount;

                entity.TotalDiscount = bo.TotalDiscount;

                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.Claim = bo.Claim;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.Gst = bo.Gst;
                entity.NetTotalAmount = bo.NetTotalAmount;

                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;

                entity.NbCession = bo.NbCession;
                entity.RnCession = bo.RnCession;
                entity.AltCession = bo.AltCession;

                entity.NbSar = bo.NbSar;
                entity.RnSar = bo.RnSar;
                entity.AltSar = bo.AltSar;

                entity.DTH = bo.DTH;
                entity.TPA = bo.TPA;
                entity.TPS = bo.TPS;
                entity.PPD = bo.PPD;
                entity.CCA = bo.CCA;
                entity.CCS = bo.CCS;
                entity.PA = bo.PA;
                entity.HS = bo.HS;
                entity.TPD = bo.TPD;
                entity.CI = bo.CI;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataPostValidationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataPostValidationBo bo)
        {
            SoaDataPostValidation.Delete(bo.Id);
        }

        public static Result Delete(SoaDataPostValidationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = SoaDataPostValidation.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                //var query = db.SoaDataPostValidations.Where(q => q.SoaDataBatchId == soaDataBatchId);
                // DO NOT TRAIL since this is mass data
                /*
                foreach (var entity in query.ToList())
                {
                    var trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SoaDataPostValidations.Remove(entity);
                }
                */

                //db.SoaDataPostValidations.RemoveRange(query);

                db.Database.ExecuteSqlCommand("DELETE FROM [SoaDataPostValidations] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
