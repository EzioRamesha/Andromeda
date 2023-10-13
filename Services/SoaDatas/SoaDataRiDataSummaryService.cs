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
    public class SoaDataRiDataSummaryService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaDataRiDataSummary)),
            };
        }

        public static Expression<Func<SoaDataRiDataSummary, SoaDataRiDataSummaryBo>> Expression()
        {
            return entity => new SoaDataRiDataSummaryBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,

                Type = entity.Type,

                BusinessCode = entity.BusinessCode,
                TreatyCode = entity.TreatyCode,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,  
                
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,

                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,
                TotalDiscount = entity.TotalDiscount,

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

                NoClaimBonus = entity.NoClaimBonus,
                Claim = entity.Claim,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                NetTotalAmount = entity.NetTotalAmount,

                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,

                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                Frequency = entity.Frequency,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                TreatyId = entity.SoaDataBatch.Treaty.TreatyIdCode,
            };
        }

        public static SoaDataRiDataSummaryBo FormBo(SoaDataRiDataSummary entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataRiDataSummaryBo
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
                TotalDiscount = entity.TotalDiscount,
                Claim = entity.Claim,
                SurrenderValue = entity.SurrenderValue,
                NoClaimBonus = entity.NoClaimBonus,
                Gst = entity.Gst,
                NetTotalAmount = entity.NetTotalAmount,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,
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
                ContractCode = entity.ContractCode,
                AnnualCohort = entity.AnnualCohort,
                Mfrs17CellName = entity.Mfrs17CellName,
                Frequency = entity.Frequency,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TreatyId = entity.SoaDataBatch?.Treaty?.TreatyIdCode,
            };

            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
            }

            return bo;
        }

        public static SoaDataRiDataSummary FormEntity(SoaDataRiDataSummaryBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaDataRiDataSummary
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
                TotalDiscount = bo.TotalDiscount,
                Claim = bo.Claim,
                NoClaimBonus = bo.NoClaimBonus,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                NetTotalAmount = bo.NetTotalAmount,
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                NbDiscount = bo.NbDiscount,
                RnDiscount = bo.RnDiscount,
                AltDiscount = bo.AltDiscount,
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
                ContractCode = bo.ContractCode,
                AnnualCohort = bo.AnnualCohort,
                Mfrs17CellName = bo.Mfrs17CellName,
                Frequency = bo.Frequency,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<SoaDataRiDataSummaryBo> FormBos(IList<SoaDataRiDataSummary> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<SoaDataRiDataSummaryBo> bos = new List<SoaDataRiDataSummaryBo>() { };
            foreach (SoaDataRiDataSummary entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaDataRiDataSummary.IsExists(id);
        }

        public static SoaDataRiDataSummaryBo Find(int id)
        {
            return FormBo(SoaDataRiDataSummary.Find(id));
        }

        public static IList<SoaDataRiDataSummaryBo> GetBySoaDataBatchIdBusinessCode(int soaDataBatchId, int type, int businessCode, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataRiDataSummaries
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.Type == type);

                if (businessCode == 1)
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
                else if (businessCode == 2)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList(), false);
            }
        }

        public static IList<SoaDataRiDataSummaryBo> QueryRiDataSummaryGroupBy(int soaDataBatchId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaDataRiDataSummaries
                    .Where(q => q.SoaDataBatchId == soaDataBatchId);

                if (type == 1)
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
                else if (type == 2)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);

                var bos = FormBos(query.ToList());
                return bos
                    .GroupBy(q => new { q.TreatyId, q.SoaQuarter })
                    .Select(q => new SoaDataRiDataSummaryBo
                    {
                        TreatyId = q.Key.TreatyId,
                        SoaQuarter = q.Key.SoaQuarter,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),
                        GrossPremium = q.Sum(d => d.GrossPremium),
                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        Claim = q.Sum(d => d.Claim),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        Gst = q.Sum(d => d.Gst),
                        NetTotalAmount = q.Sum(d => d.NetTotalAmount),
                    })
                    .ToList();
            }
        }

        public static Result Save(ref SoaDataRiDataSummaryBo bo)
        {
            if (!SoaDataRiDataSummary.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataRiDataSummaryBo bo, ref TrailObject trail)
        {
            if (!SoaDataRiDataSummary.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataRiDataSummaryBo bo)
        {
            SoaDataRiDataSummary entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref SoaDataRiDataSummaryBo bo, AppDbContext db)
        {
            SoaDataRiDataSummary entity = FormEntity(bo);
            entity.Create();
            bo.Id = entity.Id;
        }

        public static Result Create(ref SoaDataRiDataSummaryBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataRiDataSummaryBo bo)
        {
            Result result = Result();

            SoaDataRiDataSummary entity = SoaDataRiDataSummary.Find(bo.Id);
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
                entity.RiskQuarter = bo.RiskQuarter;
                entity.RiskMonth = bo.RiskMonth;
                entity.NbPremium = bo.NbPremium;
                entity.RnPremium = bo.RnPremium;
                entity.AltPremium = bo.AltPremium;
                entity.GrossPremium = bo.GrossPremium;
                entity.TotalDiscount = bo.TotalDiscount;
                entity.Claim = bo.Claim;
                entity.Gst = bo.Gst;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.DatabaseCommission = bo.DatabaseCommission;
                entity.BrokerageFee = bo.BrokerageFee;
                entity.ServiceFee = bo.ServiceFee;
                entity.NetTotalAmount = bo.NetTotalAmount;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.NbDiscount = bo.NbDiscount;
                entity.RnDiscount = bo.RnDiscount;
                entity.AltDiscount = bo.AltDiscount;
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
                entity.ContractCode = bo.ContractCode;
                entity.AnnualCohort = bo.AnnualCohort;
                entity.Mfrs17CellName = bo.Mfrs17CellName;
                entity.Frequency = bo.Frequency;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataRiDataSummaryBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataRiDataSummaryBo bo)
        {
            SoaDataRiDataSummary.Delete(bo.Id);
        }

        public static Result Delete(SoaDataRiDataSummaryBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = SoaDataRiDataSummary.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                //var query = db.SoaDataRiDataSummaries.Where(q => q.SoaDataBatchId == soaDataBatchId);
                // DO NOT TRAIL since this is mass data
                /*
                foreach (var entity in query.ToList())
                {
                    var trail = new DataTrail(entity, true);
                    trails.Add(trail);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.SoaDataRiDataSummaries.Remove(entity);
                }
                */

                //db.SoaDataRiDataSummaries.RemoveRange(query);

                db.Database.ExecuteSqlCommand("DELETE FROM [SoaDataRiDataSummaries] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
