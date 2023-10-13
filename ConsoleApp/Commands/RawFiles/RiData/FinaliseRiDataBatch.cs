using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class FinaliseRiDataBatch : Command
    {
        public bool Test { get; set; } = false;
        public int? RiDataBatchId { get; set; }
        public int? MaxFinaliseItems { get; set; }
        public bool EnableStoredProcedure { get; set; }

        public ModuleBo ModuleBo { get; set; }
        public CacheService CacheService { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public StatusHistoryBo FinalisingStatusHistoryBo { get; set; }
        public LogFinaliseRiData LogFinaliseRiData { get; set; }
        public StoredProcedure StoredProcedure { get; set; }

        public string SummaryFilePath { get; set; }

        public int Total { get; set; } = 0;
        public int Take { get; set; } = 100;
        public int Skip { get; set; } = 0;
        public IList<RiDataBo> RiDataBos { get; set; } = new List<RiDataBo> { };
        public IList<FinaliseRiData> FinaliseRiDatas { get; set; } = new List<FinaliseRiData> { };

        public FinaliseRiDataBatch()
        {
            Title = "FinaliseRiDataBatch";
            Description = "To finalise the RI Data from batch";
            Options = new string[] {
                "--t|test : Test process data",
                "--riDataBatchId= : Process by Batch Id",
                "--maxFinaliseItems= : Enter the number of max finalise items",
            };
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            RiDataBatchId = OptionIntegerNullable("riDataBatchId");
            MaxFinaliseItems = OptionIntegerNullable("maxFinaliseItems");

            Take = Util.GetConfigInteger("FinaliseRiDataItems", 100);
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

        public override void Run()
        {
            #region Checking for jobs with status 'Finalising'
            var failedBos = RiDataBatchService.GetFinaliseFailedByHours();
            RiDataBatchBo failedBo;

            if (failedBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing FinaliseRiDataBatch stucked");
                foreach (RiDataBatchBo eachBo in failedBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();
                    eachBo.Status = RiDataBatchBo.StatusFinaliseFailed;

                    failedBo = eachBo;

                    RiDataBatchService.Update(ref failedBo);
                }
            }
            #endregion

            if (RiDataBatchId.HasValue)
            {
                RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                if (RiDataBatchBo != null && RiDataBatchBo.Status != RiDataBatchBo.StatusSubmitForFinalise)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingFinalise, true, false);
                    return;
                }
            }
            if (RiDataBatchService.CountByStatus(RiDataBatchBo.StatusSubmitForFinalise) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoBatchPendingFinalise, true, false);
                return;
            }
            PrintStarting();

            SetStoredProcedure();

            CacheService = new CacheService();
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());

            while (LoadRiDataBatchBo() != null)
            {
                CacheService.LoadForRiData(RiDataBatchBo.CedantId);

                SetProcessCount("Batch");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Finalise Batch Id: {0}", RiDataBatchBo.Id));

                UpdateBatchStatus(RiDataBatchBo.StatusFinalising, MessageBag.FinalisingRiDataBatch, true);
                CreateRiDataBatchStatusFile();

                bool success = true;
                if (EnableStoredProcedure)
                {
                    try
                    {
                        StoredProcedure.AddParameter("RiDataBatchId", RiDataBatchBo.Id);
                        StoredProcedure.Execute(true);
                        if (!string.IsNullOrEmpty(StoredProcedure.Result))
                        {
                            if (StoredProcedure.Success)
                            {
                                var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(StoredProcedure.Result);
                                foreach (var r in result)
                                {
                                    SetProcessCount(r.Key, r.Value);
                                    if (r.Key == "Failed" && r.Value > 0)
                                    {
                                        success = false;
                                    }
                                }
                                PrintProcessCount();
                            }
                            else
                            {
                                success = false;
                                PrintError(StoredProcedure.Result);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        success = false;
                        PrintError(e.Message);
                    }
                }
                else
                {
                    success = Finalise();
                }

                if (success)
                    UpdateBatchStatus(RiDataBatchBo.StatusFinalised, MessageBag.FinalisedRiDataBatch);
                else
                    UpdateBatchStatus(RiDataBatchBo.StatusFinaliseFailed, MessageBag.FinalisedRiDataBatchFailed);

                if (!EnableStoredProcedure)
                    WriteSummary();

                if (Test)
                    break; // For testing only process one batch
            }

            PrintEnding();
        }

        public void UpdateBatchStatus(int status, string description, bool setFinalisingStatus = false)
        {
            FinalisingStatusHistoryBo = null;
            if (Test)
                return;

            var trail = new TrailObject();
            var statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = RiDataBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = RiDataBatchBo;
            RiDataBatchBo.Status = status;
            if (status == RiDataBatchBo.StatusFinalised)
            {
                RiDataBatchBo.ProcessWarehouseStatus = RiDataBatchBo.ProcessWarehouseStatusPending;
                RiDataBatchBo.FinalisedAt = DateTime.Now;
            }

            using (var db = new AppDbContext(false))
            {
                RiDataBatchService.CountTotalFailed(ref batch, db);
            }

            var result = RiDataBatchService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                RiDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setFinalisingStatus)
                FinalisingStatusHistoryBo = statusBo;
        }

        public bool Finalise()
        {
            LogFinaliseRiData = new LogFinaliseRiData(RiDataBatchBo);
            LogFinaliseRiData.SwFinalise.Start();

            // Reset after next batch
            Skip = 0;

            while (GetNextBulkRiData())
            {
                //PrintMessageOnly(string.Format("Taking {0} records", RiDataBos.Count));
                //PrintMessageOnly(string.Format("Between {0} - {1} records", Skip + 1, Skip + Take));

                Parallel.ForEach(FinaliseRiDatas, f => f.Finalise());

                foreach (var f in FinaliseRiDatas)
                {
                    if (f.Ignore)
                    {
                        SetProcessCount("Ignored");
                        LogFinaliseRiData.Ignored++;
                    }

                    if (f.Success)
                    {
                        SetProcessCount("Success");
                        LogFinaliseRiData.Success++;
                    }
                    else
                    {
                        SetProcessCount("Failed");
                        LogFinaliseRiData.Failed++;
                    }

                    LogFinaliseRiData.Total++;
                }

                PrintProcessCount();

                //Added update for UpdatedAt for fail checking
                var boToUpdate = RiDataBatchBo;
                RiDataBatchService.Update(ref boToUpdate);
            }

            LogFinaliseRiData.SwFinalise.Stop();
            return LogFinaliseRiData.Failed == 0;
        }

        public bool GetNextBulkRiData()
        {
            RiDataBos = new List<RiDataBo> { };
            FinaliseRiDatas = new List<FinaliseRiData> { };
            Total = RiDataService.CountByRiDataBatchId(RiDataBatchBo.Id);
            if (Skip >= Total)
                return false;
            if (Skip >= MaxFinaliseItems && MaxFinaliseItems.HasValue)
                return false;

            RiDataBos = RiDataService.GetByRiDataBatchId(RiDataBatchBo.Id, Skip, Take);
            foreach (var riDataBo in RiDataBos)
                FinaliseRiDatas.Add(new FinaliseRiData(this, riDataBo));

            Skip += Take;
            return true;
        }

        public RiDataBatchBo LoadRiDataBatchBo()
        {
            RiDataBatchBo = null;
            if (RiDataBatchId.HasValue)
            {
                RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                if (RiDataBatchBo != null && RiDataBatchBo.Status != RiDataBatchBo.StatusSubmitForFinalise)
                    RiDataBatchBo = null;
            }
            else
                RiDataBatchBo = RiDataBatchService.FindByStatus(RiDataBatchBo.StatusSubmitForFinalise);
            return RiDataBatchBo;
        }

        public void CreateRiDataBatchStatusFile()
        {
            if (RiDataBatchBo == null)
                return;
            if (FinalisingStatusHistoryBo == null)
                return;

            var trail = new TrailObject();
            var riDataBatchStatusFileBo = new RiDataBatchStatusFileBo
            {
                RiDataBatchId = RiDataBatchBo.Id,
                StatusHistoryId = FinalisingStatusHistoryBo.Id,
                StatusHistoryBo = FinalisingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var result = RiDataBatchStatusFileService.Create(ref riDataBatchStatusFileBo, ref trail);
            var userTrailBo = new UserTrailBo(
                riDataBatchStatusFileBo.Id,
                "Finalise RI Data",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            SummaryFilePath = riDataBatchStatusFileBo.GetFilePath();
            Util.MakeDir(SummaryFilePath);
            if (File.Exists(SummaryFilePath))
                File.Delete(SummaryFilePath);
        }

        public void WriteSummaryLine(object line)
        {
            if (Test)
                return;
            using (var file = new TextFile(SummaryFilePath, true, true))
            {
                file.WriteLine(line);
            }
        }

        public void WriteSummary()
        {
            if (Test)
                return;
            foreach (string line in LogFinaliseRiData.GetDetails())
            {
                WriteSummaryLine(line);
            }
        }

        public void SetStoredProcedure()
        {
            EnableStoredProcedure = Util.GetConfigBoolean("EnableStoredProcedure");
            if (EnableStoredProcedure)
            {
                StoredProcedure = new StoredProcedure(StoredProcedure.RiDataFinalise);
                EnableStoredProcedure = StoredProcedure.IsExists();
            }

            PrintMessage(string.Format("Using Stored Procedure: {0}", EnableStoredProcedure ? "Yes" : "No"));
        }
    }
}