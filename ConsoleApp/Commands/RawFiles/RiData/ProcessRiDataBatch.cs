using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.RawFiles.Sanction;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class ProcessRiDataBatch : Command
    {
        public bool Test { get; set; } = false;
        public int TestEndRow { get; set; } = 3;
        public int MaxEmptyRows { get; set; }
        public int MaxDebugRows { get; set; }
        public int TotalDebugRows { get; set; } = 0;
        public int Take { get; set; }
        public int DetailWidth { get; set; }
        public bool EndProcess { get; set; }
        public bool IsWithinDataRow { get; set; }
        public bool IsPostProcessing { get; set; }
        public int ProcessBatchStatus { get; set; }
        public string Cedants { get; set; }
        public bool IsExcelReader { get; set; }
        public bool EnabledConflictCheck { get; set; }
        public List<string> DbConnectionErrorKeywords { get; set; }

        public LogRiDataBatch LogRiDataBatch { get; set; }
        public LogRiDataFile LogRiDataFile { get; set; }
        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public int? RiDataBatchId { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public RiDataFileBo RiDataFileBo { get; set; }
        public List<RiDataFileBo> RiDataFileBos { get; set; }

        public RiDataBatchStatusFileBo RiDataBatchStatusFileBo { get; set; }
        public RiDataConfigBo RiDataConfigBo { get; set; }
        public RiDataFileConfig RiDataFileConfigFound { get; set; }

        public List<RiDataMappingBo> RiDataMappingBos { get; set; }
        public List<RiDataComputationBo> RiDataComputationBos { get; set; }
        public List<RiDataPreValidationBo> RiDataPreValidationBos { get; set; }

        public CacheService CacheService { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public List<Row> Rows { get; set; }
        public List<LogRiData> LogRiData { get; set; }
        public IProcessFile DataFile { get; set; }

        public List<ProcessRowRiData> ProcessRowRiDatas1 { get; set; } = new List<ProcessRowRiData> { };
        public List<ProcessRowRiData> ProcessRowRiDatas2 { get; set; } = new List<ProcessRowRiData> { };

        public List<RiDataBo> RiDataBos1 { get; set; } = new List<RiDataBo> { };
        public List<RiDataBo> RiDataBos2 { get; set; } = new List<RiDataBo> { };

        public SanctionVerificationChecking SanctionVerificationChecking { get; set; }

        public Logger multiThreadLog = Logger.GetLogger();

        public ProcessRiDataBatch()
        {
            Title = "ProcessRiDataBatch";
            Description = "To process RI Data Batch";
            Options = new string[] {
                "--f|filePath= : Enter File Path",
                "--t|test : Test process data",
                "--R|testMaxEndRow= : Test max end row",
                "--riDataBatchId= : Process by Batch Id",
                "--p|post : Post Processing",
                "--c|cedants= : Cedants",
                "--r|reader= : Excel Reader",
            };
            DetailWidth = StandardOutputBo.GetMaxLengthPropertyName();
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            TestEndRow = OptionInteger("testMaxEndRow", 3);
            MaxEmptyRows = Util.GetConfigInteger("ProcessRiDataMaxEmptyRows", 3);
            MaxDebugRows = Util.GetConfigInteger("ProcessRiDataMaxDebugRows", 0);
            RiDataBatchId = OptionIntegerNullable("riDataBatchId");
            Take = Util.GetConfigInteger("ProcessRiDataExcelRowRead", 50);
            IsPostProcessing = IsOption("post");
            ProcessBatchStatus = IsPostProcessing ? RiDataBatchBo.StatusSubmitForPostProcessing : RiDataBatchBo.StatusSubmitForPreProcessing;
            Cedants = Option("cedants");
            IsExcelReader = IsOption("reader");
            DbConnectionErrorKeywords = Util.GetConfig("DbConnectionErrorKeywords").Split(',').Select(q => q.Trim()).ToList();
            EnabledConflictCheck = Util.GetConfigBoolean("EnabledConflictCheck");
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
                return false;
            }
            return base.Validate();
        }

        public override void Run()
        {
            #region Checking for jobs with status 'Pre-Processing' or 'Post-Processing'
            var failedBos = RiDataBatchService.GetProcessingFailedByHours();
            RiDataBatchBo failedBo;

            if (failedBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing ProcessRiDataBatch stucked");
                foreach (RiDataBatchBo eachBo in failedBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();
                    if (eachBo.Status == RiDataBatchBo.StatusPreProcessing)
                        eachBo.Status = RiDataBatchBo.StatusPreFailed;
                    else
                        eachBo.Status = RiDataBatchBo.StatusPostFailed;

                    failedBo = eachBo;

                    RiDataBatchService.Update(ref failedBo);
                }
            }
            #endregion

            try
            {
                if (RiDataBatchId.HasValue)
                {
                    GlobalProcessRiDataConnectionStrategy.SetRiDataBatchId(RiDataBatchId.Value);
                    RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                    if (RiDataBatchBo != null && RiDataBatchBo.Status != ProcessBatchStatus)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoBatchPendingProcess);
                        return;
                    }
                }
                else if (RiDataBatchService.CountByStatus(ProcessBatchStatus) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }
                PrintStarting();

                CacheService = new CacheService();
                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());

                if (!string.IsNullOrEmpty(Cedants) && RiDataBatchService.CountByStatusAndCedantList(ProcessBatchStatus, Cedants) > 0)
                {
                    while (LoadRiDataBatchBoByCedants() != null)
                    {
                        GlobalProcessRiDataConnectionStrategy.SetRiDataBatchId(RiDataBatchBo.Id);

                        if (GetProcessCount("Batch") > 0)
                            PrintProcessCount();
                        SetProcessCount("Batch");

                        MaxDebugRows = Util.GetConfigInteger("ProcessRiDataMaxDebugRows", 0);
                        TotalDebugRows = 0;
                        CacheService.Load();

                        // Reset
                        SetProcessCount("Processed", 0);
                        SetProcessCount("Saved", 0);
                        SetProcessCount("File", 0);

                        PrintOutputTitle(string.Format("Process Batch Id: {0}", RiDataBatchBo.Id));

                        var processBatchSuccess = true;
                        LogRiDataBatch = new LogRiDataBatch(RiDataBatchBo);

                        if (IsPostProcessing)
                            UpdateBatchStatusWithConcurrencyChecking(RiDataBatchBo.StatusPostProcessing, MessageBag.ProcessRiDataBatchPostProcessing, true);
                        else
                            UpdateBatchStatusWithConcurrencyChecking(RiDataBatchBo.StatusPreProcessing, MessageBag.ProcessRiDataBatchPreProcessing, true);
                        CreateStatusLogFile();

                        if (!IsPostProcessing)
                            DeleteRiData();

                        LogRiData = new List<LogRiData> { };
                        if (!IsPostProcessing)
                        {
                            SanctionVerificationChecking = new SanctionVerificationChecking()
                            {
                                ModuleBo = ModuleBo,
                                BatchId = RiDataBatchBo.Id,
                                IsRiData = true
                            };
                        }

                        foreach (var file in RiDataFileBos)
                        {
                            LoadRiDataFile(file);

                            LogRiDataFile.SwFile.Start();
                            if (!IsPostProcessing)
                            {
                                if (ProcessFile() == false)
                                {
                                    PrintMessage(string.Format("This File Id: {0} has failed in Pre Processing", RiDataFileBo.Id));
                                    processBatchSuccess = false;
                                }
                            }
                            else
                            {
                                if (ProcessPostFile() == false)
                                {
                                    PrintMessage(string.Format("This File Id: {0} has failed in Post Processing.", RiDataFileBo.Id));
                                    processBatchSuccess = false;
                                }
                            }
                            LogRiDataFile.SwFile.Stop();

                            LogRiDataBatch.Add(LogRiDataFile);
                            WriteProcessFileSummary(LogRiDataFile);

                            if (Test)
                                break; // For testing only process one file
                        } // end of looping files

                        WriteDebugSummary();
                        WriteProcessBatchSummary(LogRiDataBatch);

                        // Conflict Check
                        if (!IsPostProcessing)
                        {
                            if (EnabledConflictCheck)
                            {
                                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("ProcessRiDataBatch");

                                using (var db = new AppDbContext(false))
                                {
                                    db.Database.CommandTimeout = 0;

                                    var script = RiDataBo.ConflictScriptGenderCountryWithinBatch(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender & country conflict check within batch...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(270);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender & country conflict check within batch");

                                    script = RiDataBo.ConflictScriptGenderCountryWithWarehouse(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender & country conflict check with warehouse...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(285);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender & country conflict check with warehouse");

                                    script = RiDataBo.ConflictScriptGenderWithinBatchCountryWithWarehouse(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender within batch & country with warehouse conflict check...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(299);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender within batch & country with warehouse conflict check");

                                    script = RiDataBo.ConflictScriptGenderWithWarehouseCountryWithinBatch(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender with warehouse & country within batch conflict check...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(312);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender with warehouse & country within batch conflict check");

                                    script = RiDataBo.ConflictScriptGenderWithinBatch(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender conflict check within batch...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(327);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender conflict check within batch");

                                    script = RiDataBo.ConflictScriptCountryWithinBatch(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for country conflict check within batch...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(342);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for country conflict check within batch");

                                    script = RiDataBo.ConflictScriptGenderWithWarehouse(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for gender conflict check with warehouse...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(355);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for gender conflict check with warehouse");

                                    script = RiDataBo.ConflictScriptCountryWithWarehouse(RiDataBatchBo.Id);
                                    PrintMessage();
                                    PrintMessage("Executing script for country conflict check with warehouse...");
                                    PrintMessage(script);

                                    connectionStrategy.Reset(369);
                                    connectionStrategy.Execute(() =>
                                    {
                                        db.Database.ExecuteSqlCommand(script);
                                        db.SaveChanges();
                                    });

                                    PrintMessage("Executed script for country conflict check with warehouse");
                                }
                            }
                        }

                        if (processBatchSuccess)
                        {
                            if (!IsPostProcessing)
                            {
                                if (SanctionVerificationChecking.IsFound)
                                    SanctionVerificationChecking.Save();

                                UpdateBatchStatus(RiDataBatchBo.StatusPreSuccess, MessageBag.ProcessRiDataBatchPreSuccess);
                            }
                            else
                            {
                                UpdateBatchStatus(RiDataBatchBo.StatusPostSuccess, MessageBag.ProcessRiDataBatchPostSuccess);
                            }
                        }
                        else
                        {
                            multiThreadLog.WriteLog();
                            if (IsPostProcessing)
                                UpdateBatchStatus(RiDataBatchBo.StatusPostFailed, MessageBag.ProcessRiDataBatchPostFailed);
                            else
                                UpdateBatchStatus(RiDataBatchBo.StatusPreFailed, MessageBag.ProcessRiDataBatchPreFailed);
                        }

                        if (Test)
                            break; // For testing only process one batch
                    }
                }

                while (LoadRiDataBatchBo() != null)
                {
                    GlobalProcessRiDataConnectionStrategy.SetRiDataBatchId(RiDataBatchBo.Id);

                    if (GetProcessCount("Batch") > 0)
                        PrintProcessCount();
                    SetProcessCount("Batch");

                    MaxDebugRows = Util.GetConfigInteger("ProcessRiDataMaxDebugRows", 0);
                    TotalDebugRows = 0;
                    CacheService.Load();

                    // Reset
                    SetProcessCount("Processed", 0);
                    SetProcessCount("Saved", 0);
                    SetProcessCount("File", 0);

                    PrintOutputTitle(string.Format("Process Batch Id: {0}", RiDataBatchBo.Id));

                    var processBatchSuccess = true;
                    LogRiDataBatch = new LogRiDataBatch(RiDataBatchBo);

                    if (IsPostProcessing)
                        UpdateBatchStatusWithConcurrencyChecking(RiDataBatchBo.StatusPostProcessing, MessageBag.ProcessRiDataBatchPostProcessing, true);
                    else
                        UpdateBatchStatusWithConcurrencyChecking(RiDataBatchBo.StatusPreProcessing, MessageBag.ProcessRiDataBatchPreProcessing, true);
                    CreateStatusLogFile();

                    if (!IsPostProcessing)
                        DeleteRiData();

                    LogRiData = new List<LogRiData> { };
                    if (!IsPostProcessing)
                    {
                        SanctionVerificationChecking = new SanctionVerificationChecking()
                        {
                            ModuleBo = ModuleBo,
                            BatchId = RiDataBatchBo.Id,
                            IsRiData = true
                        };
                    }

                    foreach (var file in RiDataFileBos)
                    {
                        LoadRiDataFile(file);

                        LogRiDataFile.SwFile.Start();
                        if (!IsPostProcessing)
                        {
                            if (ProcessFile() == false)
                            {
                                processBatchSuccess = false;
                                PrintMessage(string.Format("This File Id: {0} has failed in Pre Processing", RiDataFileBo.Id));
                            }
                        }
                        else
                        {
                            if (ProcessPostFile() == false)
                            {
                                processBatchSuccess = false;
                                PrintMessage(string.Format("This File Id: {0} has failed in Post Processing.", RiDataFileBo.Id));
                            }
                        }
                        LogRiDataFile.SwFile.Stop();

                        LogRiDataBatch.Add(LogRiDataFile);
                        WriteProcessFileSummary(LogRiDataFile);

                        if (Test)
                            break; // For testing only process one file
                    } // end of looping files

                    WriteDebugSummary();
                    WriteProcessBatchSummary(LogRiDataBatch);

                    // Conflict Check
                    if (!IsPostProcessing)
                    {
                        if (EnabledConflictCheck)
                        {
                            using (var db = new AppDbContext(false))
                            {
                                db.Database.CommandTimeout = 0;

                                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(506, "ProcessRiDataBatch");

                                var script = RiDataBo.ConflictScriptGenderCountryWithinBatch(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender & country conflict check within batch...");
                                PrintMessage(script);

                                connectionStrategy.Reset(507);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender & country conflict check within batch");

                                script = RiDataBo.ConflictScriptGenderCountryWithWarehouse(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender & country conflict check with warehouse...");
                                PrintMessage(script);

                                connectionStrategy.Reset(515);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender & country conflict check with warehouse");

                                script = RiDataBo.ConflictScriptGenderWithinBatchCountryWithWarehouse(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender within batch & country with warehouse conflict check...");
                                PrintMessage(script);

                                connectionStrategy.Reset(529);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender within batch & country with warehouse conflict check");

                                script = RiDataBo.ConflictScriptGenderWithWarehouseCountryWithinBatch(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender with warehouse & country within batch conflict check...");
                                PrintMessage(script);

                                connectionStrategy.Reset(543);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender with warehouse & country within batch conflict check");

                                script = RiDataBo.ConflictScriptGenderWithinBatch(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender conflict check within batch...");
                                PrintMessage(script);

                                connectionStrategy.Reset(557);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender conflict check within batch");

                                script = RiDataBo.ConflictScriptCountryWithinBatch(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for country conflict check within batch...");
                                PrintMessage(script);

                                connectionStrategy.Reset(571);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for country conflict check within batch");

                                script = RiDataBo.ConflictScriptGenderWithWarehouse(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for gender conflict check with warehouse...");
                                PrintMessage(script);

                                connectionStrategy.Reset(585);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for gender conflict check with warehouse");

                                script = RiDataBo.ConflictScriptCountryWithWarehouse(RiDataBatchBo.Id);
                                PrintMessage();
                                PrintMessage("Executing script for country conflict check with warehouse...");
                                PrintMessage(script);

                                connectionStrategy.Reset(599);
                                connectionStrategy.Execute(() =>
                                {
                                    db.Database.ExecuteSqlCommand(script);
                                    db.SaveChanges();
                                });

                                PrintMessage("Executed script for country conflict check with warehouse");
                            }
                        }
                    }

                    if (processBatchSuccess)
                    {
                        multiThreadLog.WriteLog();

                        if (!IsPostProcessing)
                        {
                            if (SanctionVerificationChecking.IsFound)
                                SanctionVerificationChecking.Save();

                            UpdateBatchStatus(RiDataBatchBo.StatusPreSuccess, MessageBag.ProcessRiDataBatchPreSuccess);
                        }
                        else
                        {
                            UpdateBatchStatus(RiDataBatchBo.StatusPostSuccess, MessageBag.ProcessRiDataBatchPostSuccess);
                        }
                    }
                    else
                    {
                        multiThreadLog.WriteLog();

                        if (IsPostProcessing)
                            UpdateBatchStatus(RiDataBatchBo.StatusPostFailed, MessageBag.ProcessRiDataBatchPostFailed);
                        else
                            UpdateBatchStatus(RiDataBatchBo.StatusPreFailed, MessageBag.ProcessRiDataBatchPreFailed);
                    }

                    if (Test)
                        break; // For testing only process one batch
                }

                PrintEnding();
            }
            catch (Exception e)
            {
                if (e is RetryLimitExceededException dex)
                {
                    PrintError(dex.ToString());
                }
                else
                {
                    PrintError(e.ToString());
                }
            }
        }

        public RiDataBatchBo LoadRiDataBatchBo()
        {
            RiDataBatchBo = null;
            if (RiDataBatchId.HasValue)
            {
                RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                if (RiDataBatchBo != null && RiDataBatchBo.Status != ProcessBatchStatus)
                    RiDataBatchBo = null;
            }
            else
                RiDataBatchBo = RiDataBatchService.FindByStatus(ProcessBatchStatus);

            if (RiDataBatchBo != null)
                RiDataFileBos = (List<RiDataFileBo>)RiDataFileService.GetByRiDataBatchIdMode(RiDataBatchBo.Id, RiDataFileBo.ModeInclude);
            return RiDataBatchBo;
        }

        public RiDataBatchBo LoadRiDataBatchBoByCedants()
        {
            RiDataBatchBo = RiDataBatchService.FindByStatusAndCedantList(ProcessBatchStatus, Cedants);

            if (RiDataBatchBo != null)
                RiDataFileBos = (List<RiDataFileBo>)RiDataFileService.GetByRiDataBatchIdMode(RiDataBatchBo.Id, RiDataFileBo.ModeInclude);
            return RiDataBatchBo;
        }

        public void LoadRiDataFile(RiDataFileBo file)
        {
            DataFile = null;
            RiDataFileBo = file;
            RiDataFileBo.Errors = null; // clear errors
            RiDataFileConfigFound = RiDataFileBo.RiDataFileConfig;
            LoadRiDataConfigDetails(RiDataFileBo.RiDataConfigId);
            LogRiDataFile = new LogRiDataFile(RiDataFileBo);
        }

        public void LoadRiDataConfigDetails(int? riDataConfigId)
        {
            RiDataConfigBo = RiDataConfigService.Find(riDataConfigId);
            if (RiDataConfigBo != null)
            {
                RiDataMappingBos = (List<RiDataMappingBo>)RiDataMappingService.GetByRiDataConfigId(RiDataConfigBo.Id);
                UpdateMappingBos();
                RiDataComputationBos = (List<RiDataComputationBo>)RiDataComputationService.GetByRiDataConfigId(RiDataConfigBo.Id);
                RiDataPreValidationBos = (List<RiDataPreValidationBo>)RiDataPreValidationService.GetByRiDataConfigId(RiDataConfigBo.Id);
            }
        }

        public void UpdateBatchStatus(int status, string description, bool setProcessingStatus = false)
        {
            ProcessingStatusHistoryBo = null;
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
            batch.Status = status;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataProcessingInstance(731, "ProcessRiDataBatch");
            connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext(false))
                {
                    RiDataBatchService.CountTotalFailed(ref batch, db);
                }
            });

            var result = RiDataBatchService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                RiDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setProcessingStatus)
                ProcessingStatusHistoryBo = statusBo;
        }

        // Count total failed -> Update RiDatabatch -> Create Status History -> Create user trail 
        public void UpdateBatchStatusWithConcurrencyChecking(int status, string description, bool setProcessingStatus = false)
        {
            ProcessingStatusHistoryBo = null;
            if (Test)
                return;

            var trail = new TrailObject();
            var batch = RiDataBatchBo;
            batch.Status = status;

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("ProcessRiDataBatch", 760);
            connectionStrategy.Execute(() =>
            {
                using (var db = new AppDbContext(false))
                {
                    RiDataBatchService.CountTotalFailed(ref batch, db);
                }
            });

            var result = RiDataBatchService.Update(ref batch, ref trail, true);
            var statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = RiDataBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var userTrailBo = new UserTrailBo(
                RiDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setProcessingStatus)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void CreateStatusLogFile()
        {
            if (Test)
                return;
            if (RiDataBatchBo == null)
                return;
            if (ProcessingStatusHistoryBo == null)
                return;

            var trail = new TrailObject();
            RiDataBatchStatusFileBo = new RiDataBatchStatusFileBo
            {
                RiDataBatchId = RiDataBatchBo.Id,
                StatusHistoryId = ProcessingStatusHistoryBo.Id,
                StatusHistoryBo = ProcessingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var fileBo = RiDataBatchStatusFileBo;
            var result = RiDataBatchStatusFileService.Create(ref fileBo, ref trail);

            var userTrailBo = new UserTrailBo(
                fileBo.Id,
                "Create RI Data Batch Status File",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            var path = fileBo.GetFilePath();
            Util.MakeDir(path);

            if (File.Exists(path))
                File.Delete(path);

            path = fileBo.GetDebugSummaryFilePath();
            Util.MakeDir(path);

            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteRiData()
        {
            if (Test)
                return;

            // DELETE ALL RI DATA BEFORE PROCESS
            PrintMessage("Deleting RI Data records...", true, false);
            RiDataService.DeleteByRiDataBatchId(RiDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted RI Data records", true, false);
        }

        public void UpdateFileStatus(int status, string description)
        {
            if (Test)
                return;

            var riDataFile = RiDataFileBo;
            var rawFile = RiDataFileBo.RawFileBo;

            riDataFile.Status = status;
            rawFile.Status = status;

            // Update configs from RiDataConfig
            riDataFile.UpdateConfigFromRiDataConfig(RiDataConfigBo);

            var trail = new TrailObject();
            var result = RiDataFileService.Update(ref riDataFile, ref trail);
            RawFileService.Update(ref rawFile, ref trail);

            var userTrailBo = new UserTrailBo(
                riDataFile.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateMappingBos()
        {
            if (RiDataMappingBos == null)
                return;
            if (RiDataMappingBos.Count == 0)
                return;

            foreach (RiDataMappingBo mapping in RiDataMappingBos)
            {
                if (string.IsNullOrEmpty(mapping.RawColumnName))
                    continue;

                if (int.TryParse(mapping.RawColumnName, out int col))
                {
                    mapping.Col = col;
                }
                else
                {
                    mapping.RawColumnName = mapping.RawColumnName.RemoveNewLines(); // remove new lines
                    mapping.RawColumnName = mapping.RawColumnName.Trim().ToLower(); // trim and to lower case
                }
            }
        }

        public bool ProcessFile()
        {
            if (RiDataFileBo == null)
                return false;

            EndProcess = false;
            IsWithinDataRow = false;

            PrintOutputTitle(string.Format("Process File Id: {0}", RiDataFileBo.Id));

            UpdateFileStatus(RiDataFileBo.StatusProcessing, MessageBag.ProcessRiDataFileProcessing);
            if (RiDataFileBo.Mode == RiDataFileBo.ModeExclude)
            {
                SetProcessCount("File Excluded");
                UpdateFileStatus(RiDataFileBo.StatusCompleted, MessageBag.ProcessRiDataFileCompletedExcluded);
                return true;
            }
            SetProcessCount("File");

            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                var maxRow = Test ? TestEndRow : RiDataFileConfigFound.EndRow;
                var maxCol = RiDataFileConfigFound.EndColumn;
                switch (RiDataConfigBo.FileType)
                {
                    case RiDataConfigBo.FileTypeExcel:
                        if (IsExcelReader)
                        {
                            PrintMessage("Using ExcelReader (new) library to process");
                            DataFile = new ExcelReader(GetFilePath(), worksheet: RiDataFileConfigFound.Worksheet, rowRead: Take)
                            {
                                MaxRow = maxRow,
                                MaxCol = maxCol,
                            };
                        }
                        else
                        {
                            PrintMessage("Using Excel (old) library to process");
                            DataFile = new Excel(GetFilePath(), worksheet: RiDataFileConfigFound.Worksheet, rowRead: Take)
                            {
                                MaxRow = maxRow,
                                MaxCol = maxCol,
                            };
                        }
                        break;
                    case RiDataConfigBo.FileTypePlainText:
                        DataFile = new TextFile(GetFilePath(), RiDataConfigBo.GetDelimiterChar(RiDataFileConfigFound.Delimiter))
                        {
                            MaxRow = maxRow,
                            MaxCol = maxCol,
                        };

                        if (RiDataFileConfigFound.Delimiter == RiDataConfigBo.DelimiterFixedLength)
                        {
                            DataFile.ResetColLengths();
                            foreach (RiDataMappingBo mapping in RiDataMappingBos)
                            {
                                if (mapping.Length == null)
                                    throw new Exception(string.Format(MessageBag.DelimiterFixedLengthIsNull, mapping.StandardOutputBo.TypeName));
                                if (mapping.Length == 0)
                                    throw new Exception(string.Format(MessageBag.DelimiterFixedLengthIsZero, mapping.StandardOutputBo.TypeName));
                                DataFile.AddColLengths(mapping.Length.Value);
                            }
                        }
                        break;
                }

                if (DataFile == null)
                    throw new Exception(string.Format(MessageBag.FileNotSupport, filePath));

                while (GetNextRows())
                {
                    MappingRows1(Rows); // Mapping Value
                    ProcessRows1(); // FixedValue, OverrideProperties, RiDataCorrection, RiDataComputation, RiDataPreValidation
                    AddRiData1(); // add the count and elapsed time
                    //SaveRiData1(); // already saved commit in ProcessRows1()

                    foreach (ProcessRowRiData row in ProcessRowRiDatas1)
                    {
                        if (row.SanctionVerificationChecking != null && row.SanctionVerificationChecking.IsFound)
                        {
                            SanctionVerificationChecking.IsFound = true;
                            SanctionVerificationChecking.Merge(row.SanctionVerificationChecking);
                        }
                    }

                    PrintProcessCount();

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = RiDataBatchBo;
                    RiDataBatchService.Update(ref boToUpdate);
                }

                if (DataFile != null)
                    DataFile.Close();

                // if no RiData created
                if (LogRiDataFile.RiDataCount == 0)
                    throw new Exception(RiDataFileConfigFound.GetNoDataMessage());
            }
            catch (Exception e)
            {
                if (DataFile != null)
                    DataFile.Close();

                var errors = new List<string> { };
                var message = "Pre Processing Error: " + e.Message;

                if (e.InnerException != null)
                {
                    //message += " " + e.InnerException;

                    if (e.InnerException is DbEntityValidationException idbEx)
                        PrintError(Util.CatchDbEntityValidationException(idbEx).ToString());
                }

                if (e is DbEntityValidationException dbEx)
                {
                    message = "Pre Processing Error: " + Util.CatchDbEntityValidationException(dbEx).ToString();
                }
                if (e is RetryLimitExceededException rex)
                {
                    message = $"Retry limit error: {rex}";
                }

                else if (e is COMException)
                {
                    message = "Pre Processing Error: " + "Excel Data Processing Error: " + message;
                }

                if (e.StackTrace.Length > 0)
                {
                    PrintError(message);
                }

                if (DbConnectionErrorKeywords.Any(e.ToString().Contains))
                {
                    message += " A database connection error occured.";
                }

                if (message.Contains("One or more errors occurred. "))
                {
                    message += " Please refer to your system administrator for more details.";
                }

                LogRiDataFile.FileError = message;

                errors.Add(message);
                PrintMessage("Error shown in error triangle: " + message);
                PrintMessage("Error: " + e.ToString());

                RiDataFileBo.Errors = JsonConvert.SerializeObject(errors);
                UpdateFileStatus(RiDataFileBo.StatusCompletedFailed, MessageBag.ProcessRiDataFileCompletedFailed);
                return false;
            }

            if (LogRiDataFile.GetTotalErrorCount() > 0)
            {
                //UpdateFileStatus(RiDataFileBo.StatusCompletedFailed, MessageBag.ProcessRiDataFileCompletedFailed);
                return false;
            }

            UpdateFileStatus(RiDataFileBo.StatusCompleted, MessageBag.ProcessRiDataFileCompleted);
            return true;
        }

        // Use to get RI Data by file id only
        public bool ProcessPostFile()
        {
            if (RiDataFileBo == null)
            {
                PrintMessage("RiDataFile is not found for File Id: " + RiDataFileBo.Id);
                return false;
            }

            PrintOutputTitle(string.Format("Process File Id: {0}", RiDataFileBo.Id));

            if (RiDataFileBo.Mode == RiDataFileBo.ModeExclude)
            {
                SetProcessCount("File Excluded");
                return true;
            }
            SetProcessCount("File");

            try
            {
                int total = RiDataService.CountByRiDataFileId(RiDataFileBo.Id);
                for (int skip = 0; skip < (total + Take); skip += Take)
                {
                    if (skip >= total)
                        break;

                    LogRiDataFile.SwMapping.Start();
                    ProcessRowRiDatas1 = new List<ProcessRowRiData> { };
                    foreach (var riDataBo in RiDataService.GetByRiDataFileId(RiDataFileBo.Id, skip, Take))
                    {
                        MappingRiData mappingDataRow = new MappingRiData(this, LogRiDataFile, new List<RiDataBo> { riDataBo });
                        ProcessRowRiDatas1.Add(new ProcessRowRiData(this, mappingDataRow));
                    }
                    LogRiDataFile.SwMapping.Stop();

                    ProcessRows1();
                    AddRiData1();

                    PrintProcessCount();

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = RiDataBatchBo;
                    RiDataBatchService.Update(ref boToUpdate);
                }
            }
            catch (Exception e)
            {
                var errors = new List<string> { };
                var message = "Post Processing Error: " + e.Message;

                if (e.InnerException != null)
                {
                    //message += " " + e.InnerException;

                    if (e.InnerException is DbEntityValidationException idbEx)
                        PrintError(Util.CatchDbEntityValidationException(idbEx).ToString());

                    PrintError("InnerException.Message: " + e.InnerException.Message);
                }

                if (e is DbEntityValidationException dbEx)
                {
                    message = "Post Processing Error: " + Util.CatchDbEntityValidationException(dbEx).ToString();
                }
                else if (e is COMException)
                {
                    message = "Post Processing Error: " + "Excel Data Processing Error: " + message;
                }
                else if (e is RetryLimitExceededException rex)
                {
                    message = "Post Processing Error: " + "Retry exceed limit Error: " + message + rex.StackTrace.ToString();
                }

                if (e.StackTrace.Length > 0)
                {
                    PrintError(message);
                    PrintError("StackTrace: " + e.StackTrace);
                }

                if (DbConnectionErrorKeywords.Any(e.ToString().Contains))
                {
                    message += " A database connection error occured.";
                }

                if (message.Contains("One or more errors occurred. "))
                {
                    message += " Please refer to your system administrator for more details.";
                }

                errors.Add(message);
                PrintMessage("Error shown in error triangle: " + message);
                PrintMessage("Error: " + e.ToString());

                RiDataFileBo.Errors = JsonConvert.SerializeObject(errors);
                UpdateFileStatus(RiDataFileBo.StatusCompletedFailed, MessageBag.ProcessRiDataFileCompletedFailed);
                return false;

            }

            if (LogRiDataFile.GetTotalErrorCount() > 0)
            {
                PrintMessage(string.Format("RiDataFile total error count = {0} for File Id: {1}", LogRiDataFile.GetTotalErrorCount(), RiDataFileBo.Id));
                PrintMessage("Mapping Error Count: " + LogRiDataFile.GetMappingErrorCount());
                PrintMessage("Pre Computation 1 Error Count: " + LogRiDataFile.GetPreComputation1ErrorCount());
                PrintMessage("Pre Computation 2 Error Count: " + LogRiDataFile.GetPreComputation2ErrorCount());
                PrintMessage("Pre Validation Error Count: " + LogRiDataFile.GetPreComputation2ErrorCount());
                PrintMessage("Post Computation Error Count: " + LogRiDataFile.GetPostComputationErrorCount());
                PrintMessage("Post Validation Error Count: " + LogRiDataFile.GetPostValidationErrorCount());
                return false;
            }

            RiDataFileBo.Errors = null;
            UpdateFileStatus(RiDataFileBo.StatusCompleted, MessageBag.ProcessRiDataFileCompleted);

            return true;
        }

        public bool GetNextRows()
        {
            if (EndProcess)
                return false;

            LogRiDataFile.SwRead.Start();
            Rows = DataFile.GetNextRows(Take);
            LogRiDataFile.SwRead.Stop();

            if (Rows.IsNullOrEmpty())
                return false;

            if (Rows.Where(q => !q.IsEmpty).Count() > 0)
                return true;

            return false;
        }

        public void ProcessHeader(Column col)
        {
            int? colIndex = col.ColIndex;
            object value = col.Value;
            //object value2 = col.Value2; // not using for now

            if (value != null)
            {
                // The RawColumnName already removed new lines, trim, and convert to lower case (refer to UpdateMappingBos() method in this class)
                // Thus, the value from cell or text file should remove the new lines, trim, and convert to lower case
                var header = value.ToString().Trim().ToLower().RemoveNewLines();

                // It is allow to map single column for multiple mapping fields
                // Thus, we should get all mappings using ToList() instead of one mapping using FirstOrDefault()
                var mappings = RiDataMappingBos.Where(q => q.RawColumnName == header).ToList();
                if (mappings != null && mappings.Count > 0)
                {
                    foreach (var mapping in mappings)
                    {
                        mapping.Col = colIndex;
                    }
                }
                else
                {
                    // Add custom field header
                    RiDataMappingBos.Add(RiDataMappingService.GetCustomFieldMapping(value.ToString(), colIndex));
                }
            }
        }

        public List<MappingRiData> MappingRows(List<Row> rows)
        {
            if (rows.IsNullOrEmpty())
                return null;

            var mappingDataRows = new List<MappingRiData> { };
            int countRowEmpty = 0;
            foreach (Row row in rows)
            {
                if (row.IsEnd)
                    continue;

                SetProcessCount();

                if (row.IsEmpty)
                {
                    if (IsWithinDataRow)
                    {
                        countRowEmpty++;
                        if (countRowEmpty >= MaxEmptyRows)
                        {
                            EndProcess = true;
                            break;
                        }
                    }
                }
                else
                {
                    countRowEmpty = 0;

                    int rowIndex = row.RowIndex;
                    int rowType = RiDataFileBo.RiDataFileConfig.DefineRowType(rowIndex);

                    switch (rowType)
                    {
                        case RiDataFileConfig.ROW_TYPE_HEADER:
                            foreach (var col in row.Columns)
                                ProcessHeader(col);
                            List<string> headerNames = new List<string> { };
                            foreach (var headerName in RiDataMappingBos.Where(q => !string.IsNullOrEmpty(q.RawColumnName)).Where(q => !q.Col.HasValue).Select(q => q.RawColumnName).ToList())
                            {
                                headerNames.Add(headerName);
                            }
                            if (!headerNames.IsNullOrEmpty())
                            {
                                string names = string.Join(", ", headerNames);
                                throw new Exception(string.Format("Incorrect/Missing header(s): \"{0}\" in uploaded file", names));
                            }
                            break;
                        case RiDataFileConfig.ROW_TYPE_DATA:
                            IsWithinDataRow = true;
                            mappingDataRows.Add(new MappingRiData(this, row));
                            break;
                    }
                }
            }

            if (mappingDataRows.Count > 0)
                Parallel.ForEach(mappingDataRows, mappingDataRow => mappingDataRow.MappingRow());

            return mappingDataRows;
        }

        public void MappingRows1(List<Row> rows)
        {
            LogRiDataFile.SwMapping.Start();

            ProcessRowRiDatas1 = new List<ProcessRowRiData> { };
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    LogRiDataFile.AddCount(mappingDataRow.LogRiDataFile);
                    LogRiDataFile.AddElapsedDetails(mappingDataRow.LogRiDataFile);
                    ProcessRowRiDatas1.Add(new ProcessRowRiData(this, mappingDataRow));
                }
            }

            LogRiDataFile.SwMapping.Stop();
        }

        public void MappingRows2(List<Row> rows)
        {
            LogRiDataFile.SwMapping.Start();

            ProcessRowRiDatas2 = new List<ProcessRowRiData> { };
            var mappingDataRows = MappingRows(rows);
            if (!mappingDataRows.IsNullOrEmpty())
            {
                foreach (var mappingDataRow in mappingDataRows)
                {
                    LogRiDataFile.AddCount(mappingDataRow.LogRiDataFile);
                    LogRiDataFile.AddElapsedDetails(mappingDataRow.LogRiDataFile);
                    ProcessRowRiDatas2.Add(new ProcessRowRiData(this, mappingDataRow));
                }
            }

            LogRiDataFile.SwMapping.Stop();
        }

        public void ProcessRows1()
        {
            LogRiDataFile.SwProcess.Start();
            if (!IsPostProcessing)
                Parallel.ForEach(ProcessRowRiDatas1, row =>
                {
                    try
                    {
                        row.Process();
                    }
                    catch (Exception e)
                    {
                        //PrintMessage("Error when ProcessRowRiDatas1 Process(): " + e.ToString());
                        if (e is RetryLimitExceededException dex)
                        {
                            throw dex;
                        }
                        else
                        {
                            throw e;
                        }
                    }
                });
            else
                Parallel.ForEach(ProcessRowRiDatas1, row =>
                {
                    try
                    {
                        row.ProcessPost();
                    }
                    catch (Exception e)
                    {
                        //PrintMessage("Error when ProcessRowRiDatas1 ProcessPost(): " + e.ToString());
                        if (e is RetryLimitExceededException dex)
                        {
                            throw dex;
                        }
                        else
                        {
                            throw e;
                        }
                    }
                });
            LogRiDataFile.SwProcess.Stop();
        }

        public void ProcessRows2()
        {
            LogRiDataFile.SwProcess.Start();
            if (!IsPostProcessing)
                Parallel.ForEach(ProcessRowRiDatas2, row => row.Process());
            else
                Parallel.ForEach(ProcessRowRiDatas2, row => row.ProcessPost());
            LogRiDataFile.SwProcess.Stop();
        }

        public List<RiDataBo> AddRiData(List<ProcessRowRiData> processRowRiDatas)
        {
            if (processRowRiDatas.IsNullOrEmpty())
                return null;

            var riDataBos = new List<RiDataBo> { };
            foreach (var processedRow in processRowRiDatas)
            {
                if (processedRow.RiDataBos.IsNullOrEmpty())
                    continue;

                // already save committed in parallel process
                //riDataBos.AddRange(processedRow.RiDataBos);

                string processName = IsPostProcessing ? "Updated" : "Saved";
                SetProcessCount(processName, processedRow.RiDataBos.Count, true);

                if (MaxDebugRows > 0)
                {
                    TotalDebugRows++;
                    if (!processedRow.RiDataBos.IsNullOrEmpty() && TotalDebugRows <= MaxDebugRows)
                        foreach (var riDataBo in processedRow.RiDataBos)
                            LogRiData.Add(riDataBo.Log);

                    if (TotalDebugRows >= MaxDebugRows)
                        MaxDebugRows = 0;
                }

                if (processedRow.LogRiDataFile != null)
                {
                    LogRiDataFile.AddCount(processedRow.LogRiDataFile);
                    LogRiDataFile.AddElapsedDetails(processedRow.LogRiDataFile);
                }
            }

            return riDataBos;
        }

        public void AddRiData1()
        {
            var riDataBos = AddRiData(ProcessRowRiDatas1);
            if (!riDataBos.IsNullOrEmpty())
                RiDataBos1.AddRange(riDataBos);
        }

        public void AddRiData2()
        {
            var riDataBos = AddRiData(ProcessRowRiDatas2);
            if (!riDataBos.IsNullOrEmpty())
                RiDataBos2.AddRange(riDataBos);
        }

        public void SaveRiData(List<RiDataBo> riDataBos)
        {
            if (riDataBos.IsNullOrEmpty())
                return;

            using (var db = new AppDbContext())
            {
                var transaction = db.Database.BeginTransaction();
                //var trail = new TrailObject();
                //var result = null;
                foreach (var riDataBo in riDataBos)
                {
                    if (IsCommitBuffer("Saved"))
                    {
                        db.SaveChanges();
                        transaction.Commit();

                        transaction = db.Database.BeginTransaction();
                    }

                    var riData = riDataBo;
                    riData.RiDataBatchId = RiDataFileBo.RiDataBatchId;
                    riData.RiDataFileId = RiDataFileBo.Id;
                    riData.CreatedById = RiDataFileBo.CreatedById;
                    riData.UpdatedById = RiDataFileBo.UpdatedById;
                    RiDataService.Create(ref riData, db);
                    //result = RiDataService.Create(ref riData, ref trail);

                    SetProcessCount("Saved");
                }
                db.SaveChanges();
                transaction.Commit();

                transaction.Dispose();

                /*
                UserTrailBo userTrailBo = new UserTrailBo(
                    RiDataFileBo.Id,
                    "Create RI Data",
                    result,
                    trail,
                    RiDataFileBo.CreatedById,
                    ignoreNull: true // RiData too many columns so json format should ignore the null columns
                );
                UserTrailService.Create(ref userTrailBo);
                */
            }
        }

        public void SaveRiData1()
        {
            if (Test)
                return;
            if (RiDataBos1.IsNullOrEmpty())
                return;

            LogRiDataFile.SwSave.Start();

            SaveRiData(RiDataBos1);

            RiDataBos1 = new List<RiDataBo> { }; // reset for next bulk records
            LogRiDataFile.SwSave.Stop();
        }

        public void SaveRiData2()
        {
            if (Test)
                return;
            if (RiDataBos2.IsNullOrEmpty())
                return;

            LogRiDataFile.SwSave.Start();

            SaveRiData(RiDataBos2);

            RiDataBos2 = new List<RiDataBo> { }; // reset for next bulk records
            LogRiDataFile.SwSave.Stop();
        }

        public string GetFilePath()
        {
            if (RiDataFileBo != null && RiDataFileBo.RawFileBo != null)
                return RiDataFileBo.RawFileBo.GetLocalPath();
            return null;
        }

        public void WriteProcessBatchSummary(LogRiDataBatch summary, bool print = false)
        {
            if (summary == null)
                return;
            if (print)
            {
                foreach (var line in summary.GetDetails())
                    PrintMessage(line);
                PrintMessage();
            }
            if (RiDataBatchStatusFileBo == null)
                return;

            string path = RiDataBatchStatusFileBo.GetFilePath();
            if (string.IsNullOrEmpty(path))
                return;

            if (summary != null)
            {
                using (var summaryLogFile = new TextFile(path, true, true))
                {
                    foreach (var line in summary.GetDetails())
                        summaryLogFile.WriteLine(line);
                    summaryLogFile.WriteLine("");
                }
            }
        }

        public void WriteProcessFileSummary(LogRiDataFile summary, bool print = false)
        {
            if (summary == null)
                return;
            if (print)
            {
                foreach (var line in summary.GetDetails())
                    PrintMessage(line);
                PrintMessage();
            }
            if (RiDataBatchStatusFileBo == null)
                return;

            string path = RiDataBatchStatusFileBo.GetFilePath();
            if (string.IsNullOrEmpty(path))
                return;

            if (summary != null)
            {
                using (var summaryLogFile = new TextFile(path, true, true))
                {
                    foreach (var line in summary.GetDetails())
                        summaryLogFile.WriteLine(line);
                    summaryLogFile.WriteLine("");
                }
            }
        }

        public void WriteDebugSummary(bool print = false)
        {
            if (RiDataBatchStatusFileBo == null)
                return;
            if (LogRiData.IsNullOrEmpty())
                return;
            var output = JsonConvert.SerializeObject(LogRiData, Formatting.Indented);
            if (print)
                Console.WriteLine(output);
            string path = RiDataBatchStatusFileBo.GetDebugSummaryFilePath();
            if (string.IsNullOrEmpty(path))
                return;
            using (var summaryLogFile = new TextFile(path, true, true))
                summaryLogFile.WriteLine(output);
        }

        public string FormatRiDataByType(int type, RiDataBo riData)
        {
            string property = StandardOutputBo.GetPropertyNameByType(type);
            return Util.FormatDetail(property, riData.GetPropertyValue(property), width: DetailWidth);
        }

        public void PrintRiData(RiDataBo riData)
        {
            PrintDetail("MappingStatus", RiDataBo.GetMappingStatusName(riData.MappingStatus));
            PrintDetail("PreComputation1Status", RiDataBo.GetPreComputation1StatusName(riData.PreComputation1Status));
            PrintDetail("PreComputation2Status", RiDataBo.GetPreComputation1StatusName(riData.PreComputation2Status));
            PrintDetail("PreValidationStatus", RiDataBo.GetPreValidationStatusName(riData.PreValidationStatus));
            PrintDetail("PostValidationStatus", RiDataBo.GetPostValidationStatusName(riData.PostValidationStatus));
            PrintDetail("FinaliseStatus", RiDataBo.GetFinaliseStatusName(riData.FinaliseStatus));
            PrintDetail("Errors", riData.Errors);
            //PrintDetail("CustomField", riData.CustomField);

            PrintMessage();

            PrintRiDataByTypes(new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypePolicyNumber,
                StandardOutputBo.TypePolicyExpiryDate,
                StandardOutputBo.TypeInsuredRegisterNo,
                StandardOutputBo.TypeInsuredName,
                StandardOutputBo.TypeInsuredGenderCode,
                StandardOutputBo.TypeInsuredDateOfBirth,
                StandardOutputBo.TypePremiumFrequencyCode,
                StandardOutputBo.TypeMlreBenefitCode,
                StandardOutputBo.TypeReinsEffDatePol,
                StandardOutputBo.TypeReinsBasisCode,

                //StandardOutputBo.TypeReportPeriodMonth,
                //StandardOutputBo.TypeReportPeriodYear,
                //StandardOutputBo.TypeRiskPeriodMonth,
                //StandardOutputBo.TypeRiskPeriodYear,
                //StandardOutputBo.TypeTransactionTypeCode,
                //StandardOutputBo.TypeCurrencyCode,
                //StandardOutputBo.TypeStandardPremium,
                //StandardOutputBo.TypeNetPremium,
                //StandardOutputBo.TypeTempD1,
                //StandardOutputBo.TypeTempD2,
                //StandardOutputBo.TypeTempD3,
                //StandardOutputBo.TypeTempD4,
                //StandardOutputBo.TypeTempI1,
                //StandardOutputBo.TypeTempI2,
                //StandardOutputBo.TypeTempA1,
                //StandardOutputBo.TypeTempA2,
                //StandardOutputBo.TypeTempA3,
                    
                //StandardOutputBo.TypeCedingBenefitTypeCode,
                //StandardOutputBo.TypeCedingBenefitRiskCode,
                //StandardOutputBo.TypeInsuredAttainedAge,
            }, riData);

            PrintMessage();
        }

        public void PrintRiDataByType(int type, RiDataBo riData)
        {
            PrintMessage(FormatRiDataByType(type, riData));
        }

        public void PrintRiDataByTypes(List<int> types, RiDataBo riData)
        {
            foreach (int type in types)
            {
                PrintRiDataByType(type, riData);
            }
        }
    }
}