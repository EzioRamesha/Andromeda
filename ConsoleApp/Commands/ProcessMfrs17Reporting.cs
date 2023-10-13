using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class ProcessMfrs17Reporting : Command
    {
        public Mfrs17ReportingBo Mfrs17ReportingBo { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public IList<PickListDetailBo> PremiumFrequencyPickListDetailBos { get; set; }

        public DateTime? TreatyCodeQuarterEndDate { get; set; }

        public RiskQuarterDate MonthlyRiskQuarterDate { get; set; }

        public RiskQuarterDate QuarterlyRiskQuarterDate { get; set; }

        public List<RiskQuarterDate> SemiAnnualRiskQuarterDates { get; set; }

        public List<RiskQuarterDate> AnnualRiskQuarterDates { get; set; }

        public int TotalRecord { get; set; }

        public DateTime? ActualMonthlyEndDate { get; set; }

        public ProcessMfrs17Reporting()
        {
            Title = "ProcessMfrs17Reporting";
            Description = "To retrieve RI Data for MFRS17 Reporting";
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                return false;
            }
            return base.Validate();
        }

        public override void Initial()
        {
            PremiumFrequencyPickListDetailBos = PickListDetailService.GetByStandardOutputId(StandardOutputBo.TypePremiumFrequencyCode);
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.Mfrs17Reporting.ToString());
            CommitLimit = 500;
        }

        public override void Run()
        {
            try
            {
                if (Mfrs17ReportingService.CountByStatus(Mfrs17ReportingBo.StatusSubmitForProcessing) == 0)
                {
                    PrintMessage(MessageBag.NoMfrs17reportingPendingProcess);
                    return;
                }

                if (PremiumFrequencyPickListDetailBos == null || PremiumFrequencyPickListDetailBos.Count == 0)
                {
                    PrintMessage(MessageBag.NoPremiumFrequencyFound);
                    return;
                }

                PrintStarting();

                while (LoadMfrs17ReportingBo() != null)
                {
                    try
                    {
                        TotalRecord = 0;

                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(Mfrs17ReportingBo.StatusProcessing, "Processing MFRS17 Reporting");
                        DeleteExistingData();

                        if (Mfrs17ReportingBo != null && Mfrs17ReportingBo.QuarterObject != null && Mfrs17ReportingBo.QuarterObject.EndDate.HasValue)
                        {
                            int total = CedantService.Count();
                            int take = 1;
                            for (int skip = 0; skip < (total + take); skip += take)
                            {
                                if (skip >= total)
                                    break;

                                foreach (var cedantBo in CedantService.Get(skip, take))
                                {
                                    int totalTreatyCode = TreatyCodeService.CountDistinctByCedantId(cedantBo.Id);
                                    int takeTreatyCode = 1;
                                    for (int skipTreatyCode = 0; skipTreatyCode < (totalTreatyCode + takeTreatyCode); skipTreatyCode += takeTreatyCode)
                                    {
                                        if (skipTreatyCode >= totalTreatyCode)
                                            break;

                                        foreach (var treatyCodeBo in TreatyCodeService.GetDistinctByCedantId(cedantBo.Id, skipTreatyCode, takeTreatyCode))
                                        {
                                            GetTreatyCodeQuarterEndDate(treatyCodeBo.Code);
                                            if (TreatyCodeQuarterEndDate.HasValue)
                                            {
                                                MonthlyRiskQuarterDate = RiskQuarterDate.GetRiskQuarterDate(ActualMonthlyEndDate.Value, RiskQuarterDate.TypeMonthly);
                                                QuarterlyRiskQuarterDate = RiskQuarterDate.GetRiskQuarterDate(TreatyCodeQuarterEndDate.Value, RiskQuarterDate.TypeQuarterly);
                                                SemiAnnualRiskQuarterDates = RiskQuarterDate.GetRiskQuarterDates(TreatyCodeQuarterEndDate.Value, RiskQuarterDate.TypeSemiAnnual);
                                                AnnualRiskQuarterDates = RiskQuarterDate.GetRiskQuarterDates(TreatyCodeQuarterEndDate.Value, RiskQuarterDate.TypeAnnual);

                                                foreach (PickListDetailBo bo in PremiumFrequencyPickListDetailBos)
                                                {
                                                    List<string> mfrs17TreatyCodes;
                                                    List<(int, int)> riDataWarehouseHistoryIds;

                                                    switch (bo.Code)
                                                    {
                                                        case PickListDetailBo.PremiumFrequencyCodeMonthly:
                                                            mfrs17TreatyCodes = RiDataWarehouseHistoryService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, MonthlyRiskQuarterDate.EndDate.Year, MonthlyRiskQuarterDate.EndDate.Month);
                                                            foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
                                                            {
                                                                riDataWarehouseHistoryIds = RiDataWarehouseHistoryService.GetIdsForMfrs17Reporting(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, mfrs17TreatyCode, MonthlyRiskQuarterDate.EndDate.Year, MonthlyRiskQuarterDate.EndDate.Month);
                                                                int monthlyRecord = riDataWarehouseHistoryIds.Count();
                                                                if (monthlyRecord > 0)
                                                                {
                                                                    SetDetail(cedantBo.Id, treatyCodeBo.Code, bo.Id, MonthlyRiskQuarterDate.RiskQuarter, MonthlyRiskQuarterDate.DataStartDate, MonthlyRiskQuarterDate.DataEndDate, monthlyRecord, mfrs17TreatyCode, riDataWarehouseHistoryIds);
                                                                    TotalRecord += monthlyRecord;
                                                                }
                                                            }
                                                            break;
                                                        case PickListDetailBo.PremiumFrequencyCodeQuarter:
                                                            mfrs17TreatyCodes = RiDataWarehouseHistoryService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, QuarterlyRiskQuarterDate.EndDate.Year, QuarterlyRiskQuarterDate.StartDate.Month, QuarterlyRiskQuarterDate.EndDate.Month);
                                                            foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
                                                            {
                                                                riDataWarehouseHistoryIds = RiDataWarehouseHistoryService.GetIdsForMfrs17Reporting(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, mfrs17TreatyCode, QuarterlyRiskQuarterDate.EndDate.Year, QuarterlyRiskQuarterDate.StartDate.Month, QuarterlyRiskQuarterDate.EndDate.Month);
                                                                int quarterRecord = riDataWarehouseHistoryIds.Count();
                                                                if (quarterRecord != 0)
                                                                {
                                                                    SetDetail(cedantBo.Id, treatyCodeBo.Code, bo.Id, QuarterlyRiskQuarterDate.RiskQuarter, QuarterlyRiskQuarterDate.DataStartDate, QuarterlyRiskQuarterDate.DataEndDate, quarterRecord, mfrs17TreatyCode, riDataWarehouseHistoryIds);
                                                                    TotalRecord += quarterRecord;
                                                                }
                                                            }
                                                            break;
                                                        case PickListDetailBo.PremiumFrequencyCodeSemiAnnual:
                                                            foreach (RiskQuarterDate riskQuarterDate in SemiAnnualRiskQuarterDates)
                                                            {
                                                                mfrs17TreatyCodes = RiDataWarehouseHistoryService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, riskQuarterDate.EndDate.Year, riskQuarterDate.StartDate.Month, riskQuarterDate.EndDate.Month);
                                                                foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
                                                                {
                                                                    riDataWarehouseHistoryIds = RiDataWarehouseHistoryService.GetIdsForMfrs17Reporting(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, mfrs17TreatyCode, riskQuarterDate.EndDate.Year, riskQuarterDate.StartDate.Month, riskQuarterDate.EndDate.Month);
                                                                    int semiAnnualRecord = riDataWarehouseHistoryIds.Count();
                                                                    if (semiAnnualRecord != 0)
                                                                    {
                                                                        SetDetail(cedantBo.Id, treatyCodeBo.Code, bo.Id, riskQuarterDate.RiskQuarter, riskQuarterDate.DataStartDate, riskQuarterDate.DataEndDate, semiAnnualRecord, mfrs17TreatyCode, riDataWarehouseHistoryIds);
                                                                        TotalRecord += semiAnnualRecord;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        case PickListDetailBo.PremiumFrequencyCodeAnnual:
                                                            foreach (RiskQuarterDate riskQuarterDate in AnnualRiskQuarterDates)
                                                            {
                                                                mfrs17TreatyCodes = RiDataWarehouseHistoryService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, riskQuarterDate.EndDate.Year, riskQuarterDate.StartDate.Month, riskQuarterDate.EndDate.Month);
                                                                foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
                                                                {
                                                                    riDataWarehouseHistoryIds = RiDataWarehouseHistoryService.GetIdsForMfrs17Reporting(Mfrs17ReportingBo.CutOffId, treatyCodeBo.Code, bo.Code, mfrs17TreatyCode, riskQuarterDate.EndDate.Year, riskQuarterDate.StartDate.Month, riskQuarterDate.EndDate.Month);
                                                                    int annualRecord = riDataWarehouseHistoryIds.Count();
                                                                    if (annualRecord != 0)
                                                                    {
                                                                        SetDetail(cedantBo.Id, treatyCodeBo.Code, bo.Id, riskQuarterDate.RiskQuarter, riskQuarterDate.DataStartDate, riskQuarterDate.DataEndDate, annualRecord, mfrs17TreatyCode, riDataWarehouseHistoryIds);
                                                                        TotalRecord += annualRecord;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            SetTotalRecord();
                            UpdateStatus(Mfrs17ReportingBo.StatusSuccess, "Successfully Processed MFRS17 Reporting");
                        }
                        else
                        {
                            throw new Exception(string.Format(MessageBag.InvalidQuarterFormat, Mfrs17ReportingBo.Quarter));
                        }
                    }
                    catch (Exception e)
                    {
                        SetTotalRecord();
                        UpdateStatus(Mfrs17ReportingBo.StatusFailed, "Failed to Process MFRS17 Reporting");
                        if (e is RetryLimitExceededException dex)
                        {
                            PrintError(dex.Message);
                            PrintMessage(dex.ToString(), log: true);
                        }
                        else
                        {
                            PrintError(e.Message);
                            PrintMessage(e.ToString(), log: true);
                        }
                    }
                }
                if (GetProcessCount("Process") > 0)
                    PrintProcessCount();

                PrintEnding();
            }
            catch (Exception e)
            {
                if (e is RetryLimitExceededException dex)
                {
                    PrintError(dex.Message);
                    PrintMessage(dex.ToString(), log: true);

                }
                else
                {
                    PrintError(e.Message);
                    PrintMessage(e.ToString(), log: true);
                }
            }
        }

        public Mfrs17ReportingBo LoadMfrs17ReportingBo()
        {
            Mfrs17ReportingBo = Mfrs17ReportingService.FindByStatus(Mfrs17ReportingBo.StatusSubmitForProcessing);
            return Mfrs17ReportingBo;
        }

        public void DeleteExistingData()
        {
            IList<Mfrs17ReportingDetailBo> mfrs17ReportingDetailBos = Mfrs17ReportingDetailService.GetByMfrs17ReportingId(Mfrs17ReportingBo.Id);
            foreach (Mfrs17ReportingDetailBo bo in mfrs17ReportingDetailBos)
            {
                TrailObject trail = new TrailObject();
                Mfrs17ReportingDetailRiDataService.DeleteAllByMfrs17ReportingDetailId(bo.Id); // Do not trail
                Result result = Mfrs17ReportingDetailService.Delete(bo, ref trail);
                if (result.Valid)
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        bo.Id,
                        "Delete MFRS17 Reporting Detail",
                        result,
                        trail,
                        User.DefaultSuperUserId
                    );
                    UserTrailService.Create(ref userTrailBo);
                }
            }
        }

        public void GetTreatyCodeQuarterEndDate(string treatyCode)
        {
            ActualMonthlyEndDate = null;
            TreatyCodeQuarterEndDate = null;
            if (Mfrs17ReportingBo == null)
                return;
            if (Mfrs17ReportingBo.QuarterObject == null)
                return;

            int countData = RiDataWarehouseHistoryService.CountByCutOffIdsTreatyCodeYearMonth(Mfrs17ReportingBo.CutOffId, treatyCode, Mfrs17ReportingBo.QuarterObject.EndDate.Value.Year, Mfrs17ReportingBo.QuarterObject.EndDate.Value.Month);
            int? maxYear = null;
            int? maxMonth = null;

            if (countData > 0)
            {
                maxYear = Mfrs17ReportingBo.QuarterObject.EndDate.Value.Year;
                maxMonth = RiDataWarehouseHistoryService.GetMaxMonthByMonthYear(Mfrs17ReportingBo.CutOffId, treatyCode, Mfrs17ReportingBo.QuarterObject.EndDate.Value.Month, maxYear);
            }
            else
            {
                maxYear = RiDataWarehouseHistoryService.GetMaxYearByCutOffIdsTreatyCodeYear(Mfrs17ReportingBo.CutOffId, treatyCode, Mfrs17ReportingBo.QuarterObject.EndDate.Value.Year);
                maxMonth = RiDataWarehouseHistoryService.GetMaxMonthByYear(Mfrs17ReportingBo.CutOffId, treatyCode, maxYear);
            }

            if (maxMonth.HasValue && maxYear.HasValue)
            {
                var quarterObject = new QuarterObject(maxMonth.Value, maxYear.Value);
                TreatyCodeQuarterEndDate = quarterObject.EndDate;

                ActualMonthlyEndDate = new DateTime(maxYear.Value, maxMonth.Value, DateTime.DaysInMonth(maxYear.Value, maxMonth.Value));
            }
        }

        public void SetDetail(int cedantId, string treatyCode, int premiumFrequencyCodePickListDetailId, string riskQuarter, DateTime DataStartDate, DateTime DataEndDate, int record, string mfrs17TreatyCode, List<(int, int)> riDataWarehouseHistoryIds)
        {
            TrailObject trail = new TrailObject();
            Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
            {
                Mfrs17ReportingId = Mfrs17ReportingBo.Id,
                Status = Mfrs17ReportingDetailBo.StatusProcessed,
                CedantId = cedantId,
                TreatyCode = treatyCode,
                PremiumFrequencyCodePickListDetailId = premiumFrequencyCodePickListDetailId,
                RiskQuarter = riskQuarter,
                LatestDataStartDate = DataStartDate,
                LatestDataEndDate = DataEndDate,
                Record = record,
                Mfrs17TreatyCode = mfrs17TreatyCode,
                CreatedById = User.DefaultSuperUserId
            };
            Result result = Mfrs17ReportingDetailService.Create(ref mfrs17ReportingDetailBo, ref trail);

            using (var db = new AppDbContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("ProcessMfrs17Reporting");

                var transaction = db.Database.BeginTransaction();
                List<Mfrs17ReportingDetailRiData> mfrs17ReportingDetailRiDataBos = new List<Mfrs17ReportingDetailRiData>();
                foreach ((int id, int cutoffId) in riDataWarehouseHistoryIds)
                {
                    if (IsCommitBuffer("Saved"))
                    {
                        db.Mfrs17ReportingDetailRiDatas.AddRange(mfrs17ReportingDetailRiDataBos);
                        db.ChangeTracker.DetectChanges();

                        connectionStrategy.Reset();
                        connectionStrategy.Execute(() =>
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        });

                        transaction = db.Database.BeginTransaction();
                        mfrs17ReportingDetailRiDataBos = new List<Mfrs17ReportingDetailRiData>();
                    }

                    Mfrs17ReportingDetailRiData mfrs17ReportingDetailRiDataBo = new Mfrs17ReportingDetailRiData
                    {
                        Mfrs17ReportingDetailId = mfrs17ReportingDetailBo.Id,
                        RiDataWarehouseId = id,
                        CutOffId = cutoffId,
                    };
                    mfrs17ReportingDetailRiDataBos.Add(mfrs17ReportingDetailRiDataBo);

                    SetProcessCount("Saved");
                }
                db.Mfrs17ReportingDetailRiDatas.AddRange(mfrs17ReportingDetailRiDataBos);
                db.ChangeTracker.DetectChanges();

                connectionStrategy.Reset();
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                });

                transaction.Commit();
                transaction.Dispose();
            }

            UserTrailBo userTrailBo = new UserTrailBo(
               mfrs17ReportingDetailBo.Id,
               "Create MFRS17 Reporting Detail",
               result,
               trail,
               User.DefaultSuperUserId
           );
            UserTrailService.Create(ref userTrailBo);
        }

        public void SetTotalRecord()
        {
            TrailObject trail = new TrailObject();
            var reporting = Mfrs17ReportingBo;
            Mfrs17ReportingBo.TotalRecord = TotalRecord;
            Result result = Mfrs17ReportingService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                Mfrs17ReportingBo.Id,
                "Update MFRS17 Reporting",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = Mfrs17ReportingBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            Mfrs17ReportingBo.Status = status;
            var reporting = Mfrs17ReportingBo;

            Result result = Mfrs17ReportingService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                Mfrs17ReportingBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
