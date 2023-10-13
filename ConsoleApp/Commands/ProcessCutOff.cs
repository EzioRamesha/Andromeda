using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.InvoiceRegisters;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class ProcessCutOff : Command
    {
        public DateTime Now { get; set; }
        public QuarterObject QuarterObject { get; set; }
        public CutOffBo CutOffBo { get; set; }
        public ModuleBo ModuleBo { get; set; }

        public bool IsProcess { get; set; } = false;
        public bool IsRiDataWarehouse { get; set; } = false;
        public bool IsClaimRegister { get; set; } = false;
        public bool IsInvoiceRegister { get; set; } = false;
        public bool IsRetroRegister { get; set; } = false;
        public bool IsSoaData { get; set; } = false;
        public bool IsRecover { get; set; } = false;
        public StoredProcedure StoredProcedure { get; set; }

        public ProcessCutOff()
        {
            Title = "ProcessCutOff";
            Description = "To process Cut Off";
            Options = new string[] {
                "--p|process : To process 'Submit for Processing' status record",
                "--w|riDataWarehouse : Process RiDataWarehouse",
                "--c|claimRegister : Process ClaimRegister",
                "--i|invoiceRegister : Process InvoiceRegister",
                "--r|retroRegister : Process RetroRegister",
                "--s|soaData : Process SoaData",
                "--v|recover : Recover, continue from where it stopped"
            };
            Now = DateTime.Now;
            QuarterObject = new QuarterObject(Now.Month, Now.Year);
        }

        public override void Run()
        {
            IsProcess = IsOption("process");
            IsRiDataWarehouse = IsOption("riDataWarehouse");
            IsClaimRegister = IsOption("claimRegister");
            IsInvoiceRegister = IsOption("invoiceRegister");
            IsRetroRegister = IsOption("retroRegister");
            IsSoaData = IsOption("soaData");
            IsRecover = IsOption("recover");

            try
            {
                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.CutOff.ToString());

                if (IsProcess)
                {
                    LoadCutOffSubmitForProcessing();
                    if (CutOffBo == null)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoCutOffPendingToProcess);
                        return;
                    }

                    PrintStarting();
                    if (LoadCutOffSubmitForProcessing() != null)
                    {
                        Process();
                    }
                }
                else
                {
                    LoadCutOff();

                    var messages = new List<string> { };
                    if (CutOffBo == null)
                    {
                        // if no record create it
                        CreateCutOff();
                        messages.Add(string.Format("Created new Cut Off, Quarter: {0}", QuarterObject.Quarter));

                        // if new record is not end of quarter
                        if (QuarterObject.EndDate.Value.Month != Now.Month)
                        {
                            UpdateCutOffStatus(CutOffBo.StatusSubmitForProcessing, "Auto submit for processing");
                            messages.Add(string.Format("Auto submit for process for new Cut Off, Quarter: {0}", QuarterObject.Quarter));
                        }
                        messages.Add("");
                    }

                    if (CutOffBo.Status != CutOffBo.StatusSubmitForProcessing)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoCutOffPendingToProcess);
                        return;
                    }

                    PrintStarting();
                    PrintMessages(messages);

                    Process();
                }
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
            }
            PrintEnding();
        }

        public void Process()
        {
            if (CutOffBo == null)
                return;
            if (CutOffBo.Status != CutOffBo.StatusSubmitForProcessing)
                return;
            if (!ValidateCutOff())
            {
                PrintMessage("Cut Off cannot run as another process is currently running");
                PrintDepedencyError();
                return;
            }

            UpdateCutOffStatus(CutOffBo.StatusProcessing, "Processing");
            PrintMessage("Cut Off Id: " + CutOffBo.Id);
            PrintMessage("Processing Cut Off: " + CutOffBo.Quarter);

            using (var db = new AppDbContext(false))
            {
                string script;

                if (IsRiDataWarehouse)
                {
                    if (!IsRecover)
                    {
                        PrintMessage("Start Stored Procedure for RiDataWarehouseHistoryTable");
                        if (!SetRiDataWarehouseStoredProcedure())
                        {
                            Log = false;
                            PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                            return;
                        }

                        try
                        {
                            StoredProcedure.AddParameter("CutOffQuarter", CutOffBo.Quarter.Replace(" ", ""));
                            //StoredProcedure.AddParameter("Interval", Util.GetConfigInteger("ProcessCutOffRiDataWarehouseHistoryTake", 10000));
                            StoredProcedure.Execute(true);
                            if (StoredProcedure.ParseResult())
                            {
                                foreach (var r in StoredProcedure.ResultList)
                                {
                                    SetProcessCount(r.Key, r.Value);
                                }
                                PrintProcessCount();
                            }
                            else
                            {
                                PrintMessage(StoredProcedure.Result);
                            }
                        }
                        catch (Exception e)
                        {
                            PrintError(e.ToString());
                        }
                        PrintMessage("End Stored Procedure for RiDataWarehouseHistoryTable");
                    }
                    else
                    {
                        PrintMessage("Start Stored Procedure for RiDataWarehouseHistoryTable with Recover");
                        if (!SetRiDataWarehouseRecoverStoredProcedure())
                        {
                            Log = false;
                            PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                            return;
                        }

                        try
                        {
                            StoredProcedure.AddParameter("CutOffId", CutOffBo.Id);
                            StoredProcedure.AddParameter("Interval", Util.GetConfigInteger("ProcessCutOffRiDataWarehouseHistoryTake", 10000));
                            StoredProcedure.AddParameter("RecoverFrom", RiDataWarehouseHistoryService.GetMaxIdByCutOffId(CutOffBo.Id));
                            StoredProcedure.Execute(true);
                            if (StoredProcedure.ParseResult())
                            {
                                foreach (var r in StoredProcedure.ResultList)
                                {
                                    SetProcessCount(r.Key, r.Value);
                                }
                                PrintProcessCount();
                            }
                            else
                            {
                                PrintMessage(StoredProcedure.Result);
                            }
                        }
                        catch (Exception e)
                        {
                            PrintError(e.ToString());
                        }
                        PrintMessage("End Stored Procedure for RiDataWarehouseHistoryTable with Recover");
                    }

                    #region C# Coding for RiDataWarehouse CutOff
                    //PrintMessage();
                    //PrintMessage("Executing script for RiDataWarehouseHistory table...");
                    //int take = Util.GetConfigInteger("ProcessCutOffRiDataWarehouseHistoryTake", 10000);
                    //int riDataWarehouseMinId = RiDataWarehouseService.GetMinId();
                    //int riDataWarehouseMaxId = RiDataWarehouseService.GetMaxId();
                    //int riDataWarehouseCount = RiDataWarehouseService.GetCount();

                    //for (var i = riDataWarehouseMinId; i <= riDataWarehouseMaxId; i = i + take)
                    //{
                    //    script = RiDataWarehouseHistory.Script(CutOffBo.Id, i, take);
                    //    db.Database.ExecuteSqlCommand(script);
                    //    PrintMessage(string.Format("Migrated {0} / {1} records to RiDataWarehouseHistories table", RiDataWarehouseHistoryService.GetCountByCutOffId(CutOffBo.Id), riDataWarehouseCount));
                    //}

                    ////db.SaveChanges();

                    //PrintMessage("Executed script for RiDataWarehouseHistory table");
                    #endregion
                }

                if (IsClaimRegister)
                {
                    script = ClaimRegisterHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for ClaimRegisterHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for ClaimRegisterHistory table");
                }

                if (IsInvoiceRegister)
                {
                    script = InvoiceRegisterHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for InvoiceRegisterHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for InvoiceRegisterHistory table");
                }

                if (IsRetroRegister)
                {
                    script = RetroRegisterHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for RetroRegisterHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for RetroRegisterHistory table");
                }

                if (IsSoaData)
                {
                    script = SoaDataBatchHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for SoaDataBatchHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for SoaDataBatchHistory table");

                    script = SoaDataHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for SoaDataHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for SoaDataHistory table");

                    script = SoaDataCompiledSummaryHistory.Script(CutOffBo.Id);
                    PrintMessage();
                    PrintMessage("Executing script for SoaDataCompiledSummaryHistory table...");

                    db.Database.ExecuteSqlCommand(script);
                    db.SaveChanges();

                    PrintMessage("Executed script for SoaDataCompiledSummaryHistory table");
                }
            }

            PrintMessage();
            PrintMessage("Executing ProvisionClaimRegisterBatch Command...");
            var provisionClaimRegisterBatch = new ProvisionClaimRegisterBatch();
            provisionClaimRegisterBatch.Run();
            PrintMessage("Executed ProvisionClaimRegisterBatch Command");

            UpdateCutOffStatus(CutOffBo.StatusCompleted, "Completed");

            PrintMessage();
            PrintMessage("Completed Cut Off");

            return;
        }

        public CutOffBo LoadCutOff()
        {
            CutOffBo = CutOffService.FindByMonthYear(QuarterObject.Month, QuarterObject.Year);

            if (IsRecover)
            {
                var bo = CutOffService.FindByStatus(CutOffBo.StatusToRecover);
                if (bo != null)
                    CutOffBo = bo;
            }

            return CutOffBo;
        }

        public CutOffBo LoadCutOffSubmitForProcessing()
        {
            CutOffBo = CutOffService.FindByStatus(CutOffBo.StatusSubmitForProcessing);
            return CutOffBo;
        }

        public void CreateCutOff()
        {
            CutOffBo = new CutOffBo
            {
                Status = CutOffBo.StatusInitiated,
                Month = QuarterObject.Month,
                Year = QuarterObject.Year,
                Quarter = QuarterObject.Quarter,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var trail = new TrailObject();
            var cutOffBo = CutOffBo;
            var result = CutOffService.Create(ref cutOffBo, ref trail);

            var userTrailBo = new UserTrailBo(
                CutOffBo.Id,
                "",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            // stored procudure
            var enablePartitioning = Util.GetConfigBoolean("EnablePartitioning", true);
            if (enablePartitioning)
            {
                StoredProcedure storedProcedure = new StoredProcedure(StoredProcedure.AddRiDataWarehouseHistoryPartition);
                var filepath = Util.GetConfig("PartitioningFilePath");
                var databaseName = Util.GetConfig("DatabaseName");
                var fileCount = Util.GetConfigInteger("RiDataWarehouseHistoryFileCount", 8);
                try
                {
                    storedProcedure.AddParameter("DatabaseName", databaseName);
                    storedProcedure.AddParameter("CutOffId", CutOffBo.Id);
                    storedProcedure.AddParameter("FilePath", filepath);
                    storedProcedure.AddParameter("FileCount", fileCount);
                    storedProcedure.Execute(true);
                }
                catch (Exception e)
                {

                }
            }
        }

        public void UpdateCutOffStatus(int status, string description)
        {
            var trail = new TrailObject();
            //var statusBo = new StatusHistoryBo
            //{
            //    ModuleId = ModuleBo.Id,
            //    ObjectId = CutOffBo.Id,
            //    Status = status,
            //    CreatedById = User.DefaultSuperUserId,
            //    UpdatedById = User.DefaultSuperUserId,
            //};
            //StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = CutOffBo;
            batch.Status = status;

            if (status == CutOffBo.StatusCompleted)
            {
                batch.CutOffDateTime = DateTime.Now;
            }

            var result = CutOffService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                CutOffBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public bool ValidateCutOff()
        {
            using (var db = new AppDbContext(false))
            {
                if (db.GetRiDataBatches().Any(q => q.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessing))
                    return false;

                if (db.GetClaimDataBatches().Any(q => q.Status == ClaimDataBatchBo.StatusReportingClaim))
                    return false;

                if (db.ClaimRegister.Any(q => q.ClaimStatus == ClaimDataBatchBo.StatusProcessing))
                    return false;

                if (db.InvoiceRegisterBatches.Any(q => q.Status == InvoiceRegisterBatchBo.StatusProcessing))
                    return false;

                if (db.RetroRegisterBatches.Any(q => q.Status == RetroRegisterBatchBo.StatusProcessing))
                    return false;

                if (db.GetSoaDataBatches().Any(q => q.Status == SoaDataBatchBo.StatusProcessing || q.DataUpdateStatus == SoaDataBatchBo.DataUpdateStatusDataUpdating))
                    return false;
            }

            return true;
        }

        public void PrintDepedencyError()
        {
            using (var db = new AppDbContext(false))
            {
                PrintMessage();
                PrintMessage("Dependency: ");
                if (db.GetRiDataBatches().Any(q => q.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessing))
                    PrintMessage("There is Processing in RiDataBatches.ProcessWarehouseStatus");

                if (db.GetClaimDataBatches().Any(q => q.Status == ClaimDataBatchBo.StatusReportingClaim))
                    PrintMessage("There is ReportingClaim in ClaimDataBatches.Status");

                if (db.ClaimRegister.Any(q => q.ClaimStatus == ClaimDataBatchBo.StatusProcessing))
                    PrintMessage("There is Processing in ClaimRigister.ClaimStatus");

                if (db.InvoiceRegisterBatches.Any(q => q.Status == InvoiceRegisterBatchBo.StatusProcessing))
                    PrintMessage("There is Processing in InvoiceRegisterBatches.Status");

                if (db.RetroRegisterBatches.Any(q => q.Status == RetroRegisterBatchBo.StatusProcessing))
                    PrintMessage("There is Processing in RetroRegisterBatches.Status");

                if (db.GetSoaDataBatches().Any(q => q.Status == SoaDataBatchBo.StatusProcessing || q.DataUpdateStatus == SoaDataBatchBo.DataUpdateStatusDataUpdating))
                    PrintMessage("There is Processing in SoaDataBatches.Status / DataUpdateStatusDataUpdating in SoaDataBatches.DataUpdateStatus");
            }
        }

        public bool SetRiDataWarehouseStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.ProcessCutOffRiDataWarehouse);
            return StoredProcedure.IsExists();
        }

        public bool SetRiDataWarehouseRecoverStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.ProcessCutOffRiDataWarehouseRecover);
            return StoredProcedure.IsExists();
        }
    }
}
