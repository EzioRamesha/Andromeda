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
    public class SoaDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SoaData)),
                Controller = ModuleBo.ModuleController.SoaData.ToString(),
            };
        }

        public static Expression<Func<SoaData, SoaDataBo>> Expression()
        {
            return entity => new SoaDataBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataFileId = entity.SoaDataFileId,

                MappingStatus = entity.MappingStatus,
                Errors = entity.Errors,

                CompanyName = entity.CompanyName,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                TreatyMode = entity.TreatyMode,
                TreatyType = entity.TreatyType,
                PlanBlock = entity.PlanBlock,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                RiDeposit = entity.RiDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,
                TotalCommission = entity.TotalCommission,
                NetTotalAmount = entity.NetTotalAmount,
                SoaReceivedDate = entity.SoaReceivedDate,
                BordereauxReceivedDate = entity.BordereauxReceivedDate,
                StatementStatus = entity.StatementStatus,
                Remarks1 = entity.Remarks1,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaStatus = entity.SoaStatus,
                ConfirmationDate = entity.ConfirmationDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SoaDataBo FormBo(SoaData entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new SoaDataBo
            {
                Id = entity.Id,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataFileId = entity.SoaDataFileId,

                MappingStatus = entity.MappingStatus,
                Errors = entity.Errors,

                CompanyName = entity.CompanyName,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                TreatyMode = entity.TreatyMode,
                TreatyType = entity.TreatyType,
                PlanBlock = entity.PlanBlock,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                RiDeposit = entity.RiDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,
                TotalCommission = entity.TotalCommission,
                NetTotalAmount = entity.NetTotalAmount,
                SoaReceivedDate = entity.SoaReceivedDate,
                BordereauxReceivedDate = entity.BordereauxReceivedDate,
                StatementStatus = entity.StatementStatus,
                Remarks1 = entity.Remarks1,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaStatus = entity.SoaStatus,
                ConfirmationDate = entity.ConfirmationDate,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId);
                bo.SoaDataFileBo = SoaDataFileService.Find(entity.SoaDataFileId);
            }

            return bo;
        }

        public static SoaData FormEntity(SoaDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new SoaData
            {
                Id = bo.Id,
                SoaDataBatchId = bo.SoaDataBatchId,
                SoaDataFileId = bo.SoaDataFileId,

                MappingStatus = bo.MappingStatus,
                Errors = bo.Errors,

                CompanyName = bo.CompanyName,
                BusinessCode = bo.BusinessCode,
                TreatyId = bo.TreatyId,
                TreatyCode = bo.TreatyCode,
                TreatyMode = bo.TreatyMode,
                TreatyType = bo.TreatyType,
                PlanBlock = bo.PlanBlock,
                RiskMonth = bo.RiskMonth,
                SoaQuarter = bo.SoaQuarter,
                RiskQuarter = bo.RiskQuarter,
                NbPremium = bo.NbPremium,
                RnPremium = bo.RnPremium,
                AltPremium = bo.AltPremium,
                GrossPremium = bo.GrossPremium,
                TotalDiscount = bo.TotalDiscount,
                RiskPremium = bo.RiskPremium,
                NoClaimBonus = bo.NoClaimBonus,
                Levy = bo.Levy,
                Claim = bo.Claim,
                ProfitComm = bo.ProfitComm,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                ModcoReserveIncome = bo.ModcoReserveIncome,
                RiDeposit = bo.RiDeposit,
                DatabaseCommission = bo.DatabaseCommission,
                AdministrationContribution = bo.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = bo.RecaptureFee,
                CreditCardCharges = bo.CreditCardCharges,
                BrokerageFee = bo.BrokerageFee,
                TotalCommission = bo.TotalCommission,
                NetTotalAmount = bo.NetTotalAmount,
                SoaReceivedDate = bo.SoaReceivedDate,
                BordereauxReceivedDate = bo.BordereauxReceivedDate,
                StatementStatus = bo.StatementStatus,
                Remarks1 = bo.Remarks1,
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                SoaStatus = bo.SoaStatus,
                ConfirmationDate = bo.ConfirmationDate,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<SoaDataBo> FormBos(IList<SoaData> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<SoaDataBo> bos = new List<SoaDataBo>() { };
            foreach (SoaData entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return SoaData.IsExists(id);
        }

        public static SoaDataBo Find(int id)
        {
            return FormBo(SoaData.Find(id));
        }

        public static int CountBySoaDataBatchIdMappingStatusFailed(int soaDataBatchId, AppDbContext db)
        {
            return db.SoaData
                .Where(q => q.SoaDataBatchId == soaDataBatchId)
                .Where(q => q.MappingStatus == SoaDataBo.MappingStatusFailed)
                .Count();
        }

        public static double? TotalRiskPremiumBySoaDataBatchId(int soaDataBatchId, bool retakaful = false, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId);

                if (retakaful)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);
                else
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => !string.IsNullOrEmpty(q.CurrencyCode) && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);
                else
                    query = query.Where(q => string.IsNullOrEmpty(q.CurrencyCode) || q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr);

                return query.Sum(q => q.RiskPremium) ?? 0;
            }                
        }

        public static IList<SoaDataBo> GetBySoaDataBatchIdBusinessCode(int soaDataBatchId, int type, bool originalCurrency = false)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId);

                if (type == 1)
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
                else if (type == 2)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);

                if (originalCurrency)
                    query = query.Where(q => q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr);

                return FormBos(query.ToList());
            }
        }

        public static IList<SoaDataBo> QuerySoaDataGroupBy(int soaDataBatchId, int type)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaData
                    .Where(q => q.SoaDataBatchId == soaDataBatchId);

                if (type == 1)
                    query = query.Where(q => q.BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee);
                else if (type == 2)
                    query = query.Where(q => q.BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee);

                var bos = FormBos(query.ToList());
                return bos
                    .GroupBy(q => new { q.TreatyId, q.SoaQuarter })
                    .Select(q => new SoaDataBo
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

        #region Target Planning
        public static List<string> GetDistinctForTargetPlanning()
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.SoaData.Where(q => q.TreatyCode != null).ToList())
                    .Where(q => q.SoaDataBatchBo != null)
                    .Select(q => q.SoaDataBatchBo.CedantId.ToString() + "|" +
                    q.SoaDataBatchBo.TreatyId.ToString() + "|" +
                    q.SoaDataBatchBo.CreatedById.ToString() + "|" +
                    q.SoaDataBatchBo.CedantBo.Code + " - " + q.SoaDataBatchBo.CedantBo.Name + "|" +
                    q.SoaDataBatchBo.TreatyBo.TreatyIdCode + "|" +
                    q.SoaDataBatchBo.CreatedByBo.FullName + "|" +
                    q.TreatyCode)
                    .Distinct().ToList();
            }
        }
        
        public static IList<SoaDataBo> SoaDataReportParams()
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.SoaData
                    .Where(q => q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                    .Where(q => q.SoaDataBatch.InvoiceStatus == 0)
                    .Where(q => q.SoaDataBatch.Status != SoaDataBatchBo.StatusPendingDelete);

                return FormBos(query.ToList());
            }        
        }

        public static string GetTargetPlanningStatementTrackingValue(string treatyCode, string quarter)
        {
            using (var db = new AppDbContext(false))
            {
                var soaData = FormBos(db.SoaData.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == quarter).ToList())
                    .Where(q => q.SoaDataBatchBo != null)
                    .FirstOrDefault();

                if (soaData != null)
                {
                    if (soaData.SoaDataBatchBo.Status == SoaDataBatchBo.StatusProcessing)
                        return "Processing RI/Claim Data";

                    if (soaData.SoaDataBatchBo.Status == SoaDataBatchBo.StatusSubmitForApproval)
                        return "Pending Approval";

                    if (soaData.SoaDataBatchBo.InvoiceStatus == SoaDataBatchBo.InvoiceStatusInvoiced)
                        return "Invoiced";
                }

                return "";
            }
        }

        public static string GetTargetPlanningPCStatementTrackingValue(string treatyCode, int year)
        {
            string yearStr = year.ToString();

            using (var db = new AppDbContext(false))
            {
                var soaData = FormBos(db.SoaData.Where(q => q.TreatyCode == treatyCode && ("20" + q.SoaQuarter.Substring(0, 2)) == yearStr).ToList())
                    .Where(q => q.SoaDataBatchBo != null)
                    .FirstOrDefault();

                if (soaData != null)
                {
                    if (soaData.SoaDataBatchBo.IsProfitCommissionData)
                    {
                        return "Booked";
                    }
                }

                return "None";
            }
        }
        #endregion

        public static Result Save(ref SoaDataBo bo)
        {
            if (!SoaData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SoaDataBo bo, ref TrailObject trail)
        {
            if (!SoaData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SoaDataBo bo)
        {
            SoaData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static void Create(ref SoaDataBo bo, AppDbContext db)
        {
            SoaData entity = FormEntity(bo);
            entity.Create(db);
            bo.Id = entity.Id;
        }

        public static Result Create(ref SoaDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SoaDataBo bo)
        {
            Result result = Result();

            SoaData entity = SoaData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.SoaDataFileId = bo.SoaDataFileId;

                entity.MappingStatus = bo.MappingStatus;
                entity.Errors = bo.Errors;

                entity.CompanyName = bo.CompanyName;
                entity.BusinessCode = bo.BusinessCode;
                entity.TreatyId = bo.TreatyId;
                entity.TreatyCode = bo.TreatyCode;
                entity.TreatyMode = bo.TreatyMode;
                entity.TreatyType = bo.TreatyType;
                entity.PlanBlock = bo.PlanBlock;
                entity.RiskMonth = bo.RiskMonth;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.RiskQuarter = bo.RiskQuarter;
                entity.NbPremium = bo.NbPremium;
                entity.RnPremium = bo.RnPremium;
                entity.AltPremium = bo.AltPremium;
                entity.GrossPremium = bo.GrossPremium;
                entity.TotalDiscount = bo.TotalDiscount;
                entity.RiskPremium = bo.RiskPremium;
                entity.NoClaimBonus = bo.NoClaimBonus;
                entity.Levy = bo.Levy;
                entity.Claim = bo.Claim;
                entity.ProfitComm = bo.ProfitComm;
                entity.SurrenderValue = bo.SurrenderValue;
                entity.Gst = bo.Gst;
                entity.ModcoReserveIncome = bo.ModcoReserveIncome;
                entity.RiDeposit = bo.RiDeposit;
                entity.DatabaseCommission = bo.DatabaseCommission;
                entity.AdministrationContribution = bo.AdministrationContribution;
                entity.ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession;
                entity.RecaptureFee = bo.RecaptureFee;
                entity.CreditCardCharges = bo.CreditCardCharges;
                entity.BrokerageFee = bo.BrokerageFee;
                entity.TotalCommission = bo.TotalCommission;
                entity.NetTotalAmount = bo.NetTotalAmount;
                entity.SoaReceivedDate = bo.SoaReceivedDate;
                entity.BordereauxReceivedDate = bo.BordereauxReceivedDate;
                entity.StatementStatus = bo.StatementStatus;
                entity.Remarks1 = bo.Remarks1;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.CurrencyRate = bo.CurrencyRate;
                entity.SoaStatus = bo.SoaStatus;
                entity.ConfirmationDate = bo.ConfirmationDate;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SoaDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SoaDataBo bo)
        {
            SoaData.Delete(bo.Id);
        }

        public static Result Delete(SoaDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = SoaData.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static IList<DataTrail> DeleteBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                var trails = new List<DataTrail>();
                //var query = db.SoaData.Where(q => q.SoaDataBatchId == soaDataBatchId);
                // DO NOT TRAIL since this is mass data
                /*
                foreach (var soaData in query.ToList())
                {
                    var trail = new DataTrail(soaData, true);
                    trails.Add(trail);

                    db.Entry(soaData).State = EntityState.Deleted;
                    db.SoaData.Remove(soaData);
                }
                */
                //db.SoaData.RemoveRange(query);

                db.Database.ExecuteSqlCommand("DELETE FROM [SoaData] WHERE [SoaDataBatchId] = {0}", soaDataBatchId);
                db.SaveChanges();

                return trails;
            }
        }
    }
}
