using BusinessObject;
using BusinessObject.Identity;
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
    public class UpdateMfrs17Reporting : Command
    {
        public Mfrs17ReportingBo Mfrs17ReportingBo { get; set; }

        public Mfrs17ReportingDetailBo Mfrs17ReportingDetailBo { get; set; }

        // Mfrs17 Reporting Detail Info
        public int DetailStatus { get; set; }

        public int CedantId { get; set; }

        public string TreatyCode { get; set; }

        public PickListDetailBo PremiumFrequencyCodePickListDetailBo { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public string Mfrs17TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public DateTime DataStartDate { get; set; }

        public DateTime DataEndDate { get; set; }

        public RiskQuarterDate RiskQuarterDate { get; set; }

        // Others
        public ModuleBo ModuleBo { get; set; }

        public UpdateMfrs17Reporting()
        {
            Title = "UpdateMfrs17Reporting";
            Description = "To update RI Data for MFRS17 Reporting";
        }

        public override void Initial()
        {
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.Mfrs17Reporting.ToString());
            CommitLimit = 500;
        }

        public override void Run()
        {
            try
            {
                if (Mfrs17ReportingService.CountByStatus(Mfrs17ReportingBo.StatusPendingUpdate) == 0)
                {
                    PrintMessage(MessageBag.NoMfrs17reportingPendingUpdate);
                    return;
                }

                List<int> processStatus = new List<int>() { Mfrs17ReportingDetailBo.StatusPending, Mfrs17ReportingDetailBo.StatusReprocess };

                PrintStarting();

                while (LoadMfrs17ReportingBo() != null)
                {
                    try
                    {
                        UpdateStatus(Mfrs17ReportingBo.StatusUpdating, "Updating MFRS17 Reporting");
                        DeletePendingDeleteData();

                        IList<Mfrs17ReportingDetailBo> mfrs17ReportingDetailBos = Mfrs17ReportingDetailService.GetByMfrs17ReportingIdStatus(Mfrs17ReportingBo.Id, processStatus);
                        foreach (Mfrs17ReportingDetailBo bo in mfrs17ReportingDetailBos)
                        {
                            Mfrs17ReportingDetailRiDataService.DeleteAllByMfrs17ReportingDetailId(bo.Id); // Do not trail
                            LoadMfrs17ReportingDetailBo(bo);

                            List<string> mfrs17TreatyCodes = new List<string>();
                            if (DetailStatus == Mfrs17ReportingDetailBo.StatusReprocess)
                            {
                                mfrs17TreatyCodes.Add(Mfrs17TreatyCode);
                            }
                            else
                            {
                                mfrs17TreatyCodes = RiDataWarehouseHistoryService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.CutOffId, TreatyCode, PremiumFrequencyCode, RiskQuarterDate.EndDate.Year, RiskQuarterDate.StartDate.Month, RiskQuarterDate.EndDate.Month, CedingPlanCode);
                            }

                            foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
                            {
                                var riDataWarehouseHistoryIds = RiDataWarehouseHistoryService.GetIdsForMfrs17Reporting(Mfrs17ReportingBo.CutOffId, TreatyCode, PremiumFrequencyCode, mfrs17TreatyCode, RiskQuarterDate.EndDate.Year, RiskQuarterDate.StartDate.Month, RiskQuarterDate.EndDate.Month, CedingPlanCode);
                                int record = riDataWarehouseHistoryIds.Count();
                                if (record > 0)
                                {
                                    if (DetailStatus == Mfrs17ReportingDetailBo.StatusPending)
                                    {
                                        if (string.IsNullOrEmpty(Mfrs17TreatyCode))
                                        {
                                            Mfrs17TreatyCode = mfrs17TreatyCode;
                                            Mfrs17ReportingDetailBo.Mfrs17TreatyCode = mfrs17TreatyCode;
                                        }
                                        else
                                        {
                                            AddMfrs17ReportingDetailBo(mfrs17TreatyCode);
                                        }
                                    }

                                    AddRiData(riDataWarehouseHistoryIds);
                                }

                                if (record > 0 || DetailStatus == Mfrs17ReportingDetailBo.StatusReprocess)
                                {
                                    Mfrs17ReportingDetailBo.Record = record;
                                    UpdateDetailStatus(Mfrs17ReportingDetailBo.StatusProcessed, "Updated MFRS17 Reporting Detail");
                                }
                                Mfrs17ReportingDetailBo = null;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        UpdateStatus(Mfrs17ReportingBo.StatusFailed, "Failed to Update MFRS17 Reporting");
                        if (e is RetryLimitExceededException dex)
                        {
                            PrintMessage(dex.Message, log: true);
                        }
                        else
                        {
                            PrintMessage(e.Message, log: true);
                        }
                    }
                    CountTotalRecord();
                    UpdateStatus(Mfrs17ReportingBo.StatusSuccess, "Successfully Processed MFRS17 Reporting");
                }
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
            }
        }

        public void AddRiData(List<(int, int)> riDataWarehouseHistoryIds)
        {
            using (var db = new AppDbContext())
            {
                var transaction = db.Database.BeginTransaction();

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("UpdateMfrs17Reporting");

                foreach ((int id, int cutOffId) in riDataWarehouseHistoryIds)
                {
                    if (IsCommitBuffer("Saved"))
                    {
                        connectionStrategy.Reset();
                        connectionStrategy.Execute(() =>
                        {
                            db.SaveChanges();
                        });

                        transaction.Commit();
                        transaction = db.Database.BeginTransaction();
                    }

                    Mfrs17ReportingDetailRiDataBo mfrs17ReportingDetailRiDataBo = new Mfrs17ReportingDetailRiDataBo
                    {
                        Mfrs17ReportingDetailId = Mfrs17ReportingDetailBo.Id,
                        RiDataWarehouseId = id,
                        CutOffId = Mfrs17ReportingBo.CutOffId,
                    };
                    Mfrs17ReportingDetailRiDataService.Create(ref mfrs17ReportingDetailRiDataBo, db);

                    SetProcessCount("Saved");
                }

                connectionStrategy.Reset();
                connectionStrategy.Execute(() =>
                {
                    db.SaveChanges();
                    transaction.Commit();
                });

                transaction.Dispose();
            }
        }

        public void DeletePendingDeleteData()
        {
            try
            {
                IList<Mfrs17ReportingDetailBo> mfrs17ReportingDetailBos = Mfrs17ReportingDetailService.GetByMfrs17ReportingIdStatus(Mfrs17ReportingBo.Id, Mfrs17ReportingDetailBo.StatusPendingDelete);
                foreach (Mfrs17ReportingDetailBo bo in mfrs17ReportingDetailBos)
                {
                    Mfrs17ReportingDetailRiDataService.DeleteAllByMfrs17ReportingDetailId(bo.Id); // Do not trail

                    Mfrs17ReportingDetailBo = bo;
                    UpdateDetailStatus(Mfrs17ReportingDetailBo.StatusDeleted, "Delete MFRS17 Reporting Detail");
                }
            }
            catch (Exception e)
            {
                if (e is RetryLimitExceededException dex)
                {
                    PrintMessage(dex.ToString());
                    throw dex;
                }
                else
                {
                    PrintMessage(e.Message);
                    throw e;
                }
            }
        }

        public Mfrs17ReportingBo LoadMfrs17ReportingBo()
        {
            Mfrs17ReportingBo = Mfrs17ReportingService.FindByStatus(Mfrs17ReportingBo.StatusPendingUpdate);
            return Mfrs17ReportingBo;
        }

        public void CountTotalRecord()
        {
            Mfrs17ReportingBo.TotalRecord = Mfrs17ReportingDetailService.SumRecordsByMfrs17ReportingId(Mfrs17ReportingBo.Id);
        }

        public void LoadMfrs17ReportingDetailBo(Mfrs17ReportingDetailBo bo)
        {
            Mfrs17ReportingDetailBo = bo;
            DetailStatus = bo.Status.Value;
            CedantId = bo.CedantId;
            TreatyCode = bo.TreatyCode;
            PremiumFrequencyCodePickListDetailBo = bo.PremiumFrequencyCodePickListDetailBo;
            PremiumFrequencyCode = PremiumFrequencyCodePickListDetailBo.Code;
            Mfrs17TreatyCode = bo.Mfrs17TreatyCode;
            CedingPlanCode = bo.CedingPlanCode;

            RiskQuarterDate = RiskQuarterDate.GetRiskQuarterDate(bo.RiskQuarter, PremiumFrequencyCodePickListDetailBo.Code);

            if (PremiumFrequencyCodePickListDetailBo.Code == "M")
            {
                RiskQuarterDate.StartDate = bo.LatestDataStartDate;
                RiskQuarterDate.EndDate = bo.LatestDataEndDate;
            }

            RiskQuarterDate.DataStartDate = bo.LatestDataStartDate;
            RiskQuarterDate.DataEndDate = bo.LatestDataEndDate;
        }
        
        public void AddMfrs17ReportingDetailBo(string mfrs17TreatyCode)
        {
            TrailObject trail = new TrailObject();
            Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
            {
                Mfrs17ReportingId = Mfrs17ReportingBo.Id,
                Status = Mfrs17ReportingDetailBo.StatusPending,
                CedantId = CedantId,
                TreatyCode = TreatyCode,
                Mfrs17TreatyCode = mfrs17TreatyCode,
                PremiumFrequencyCodePickListDetailId = PremiumFrequencyCodePickListDetailBo.Id,
                RiskQuarter = RiskQuarterDate.RiskQuarter,
                LatestDataStartDate = RiskQuarterDate.DataStartDate,
                LatestDataEndDate = RiskQuarterDate.DataEndDate,
                Record = 0,
                CreatedById = User.DefaultSuperUserId,
            };
            Result result = Mfrs17ReportingDetailService.Create(ref mfrs17ReportingDetailBo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                mfrs17ReportingDetailBo.Id,
                "Create MFRS17 Reporting Detail",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            Mfrs17ReportingDetailBo = mfrs17ReportingDetailBo;
            Mfrs17TreatyCode = mfrs17TreatyCode;
        }

        public void UpdateStatus(int status, string description)
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

            var reporting = Mfrs17ReportingBo;
            Mfrs17ReportingBo.Status = status;
            Mfrs17ReportingBo.UpdatedById = User.DefaultSuperUserId;
            Result result = Mfrs17ReportingService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                Mfrs17ReportingBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateDetailStatus(int status, string description)
        {
            TrailObject trail = new TrailObject();

            Mfrs17ReportingDetailBo.Status = status;
            Mfrs17ReportingDetailBo.IsModified = true;
            Mfrs17ReportingDetailBo.UpdatedById = User.DefaultSuperUserId;
            var reportingDetail = Mfrs17ReportingDetailBo;

            Result result = Mfrs17ReportingDetailService.Update(ref reportingDetail, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                reportingDetail.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
