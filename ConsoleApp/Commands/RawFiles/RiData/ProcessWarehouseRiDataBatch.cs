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
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class ProcessWarehouseRiDataBatch : Command
    {
        public bool Test { get; set; } = false;
        public bool EnableStoredProcedure { get; set; }
        public int? RiDataBatchId { get; set; }
        public int? MaxFinaliseItems { get; set; }

        public ModuleBo ModuleBo { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }

        public int Total { get; set; } = 0;
        public int Take { get; set; } = 100;
        public int Skip { get; set; } = 0;
        public IList<RiDataBo> RiDataBos { get; set; } = new List<RiDataBo> { };
        public IList<ProcessWarehouseRiData> ProcessWarehouseRiDatas { get; set; } = new List<ProcessWarehouseRiData> { };

        public List<int> PendingProcessWarehouseStatuses { get; set; } = new List<int> { RiDataBatchBo.ProcessWarehouseStatusPending, RiDataBatchBo.ProcessWarehouseStatusProcessFailed };
        public IList<string> PropertyNames { get; set; }

        public Dictionary<string, int> PolicyStatusCodes { get; set; }

        public ProcessWarehouseRiDataBatch()
        {
            Title = "ProcessWarehouseRiDataBatch";
            Description = "To process the latest RI Data to RI Data Warehouse";
            Options = new string[] {
                "--t|test : Test process data",
                "--riDataBatchId= : Process by Batch Id",
            };
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            RiDataBatchId = OptionIntegerNullable("riDataBatchId");

            Take = Util.GetConfigInteger("CompileRiDataItems", 100);
        }

        public override void Run()
        {
            if (CutOffService.IsCutOffProcessing())
            {
                Log = false;
                PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                return;
            }
            if (RiDataBatchId.HasValue)
            {
                RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                if (RiDataBatchBo != null && !PendingProcessWarehouseStatuses.Contains(RiDataBatchBo.ProcessWarehouseStatus))
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingCompile, true, false);
                    return;
                }
            }
            if (RiDataBatchService.CountByProcessWarehouseStatuses(PendingProcessWarehouseStatuses) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoBatchPendingCompile, true, false);
                return;
            }
            
            PrintStarting();

            SetStoredProcedure();

            GetPropertyNames();
            GetPolicyStatusCodes();
            while (LoadRiDataBatchBo() != null)
            {
                SetProcessCount("Batch");
                // Reset
                SetProcessCount("Processed", 0);

                PrintOutputTitle(string.Format("Compile Batch Id: {0}", RiDataBatchBo.Id));
                if (!Test)
                {
                    UpdateBatchCompileStatus(RiDataBatchBo.ProcessWarehouseStatusProcessing, "Compiling RI Data Batch");
                }

                bool success = true;
                if (EnableStoredProcedure)
                {
                    try
                    {
                        StoredProcedure.AddParameter("RiDataBatchId", RiDataBatchBo.Id);
                        StoredProcedure.Execute(true);

                        if (!string.IsNullOrEmpty(StoredProcedure.Result))
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
                    }
                    catch (Exception e)
                    {
                        success = false;
                        PrintError(e.Message);
                    }
                }
                else
                {
                    success = ProcessToWarehouse();
                }

                if (!Test)
                {
                    if (success)
                    {
                        UpdateBatchCompileStatus(RiDataBatchBo.ProcessWarehouseStatusSuccess, MessageBag.ProcessedWarehouseRiDataBatch);
                    }
                    else
                    {
                        UpdateBatchCompileStatus(RiDataBatchBo.ProcessWarehouseStatusFailed, MessageBag.ProcessWarehouseRiDataBatchFailed);
                    }
                }
                else
                {
                    // test one batch only
                    break;
                }
            }
            PrintEnding();
        }

        public bool ProcessToWarehouse()
        {
            // Reset after next batch
            Skip = 0;
            bool success = true;
            while (GetNextBulkRiData())
            {
                Parallel.ForEach(ProcessWarehouseRiDatas, f => f.ProcessToWarehouse());

                foreach (var w in ProcessWarehouseRiDatas)
                {
                    if (w.Success)
                    {
                        SetProcessCount("Success");
                    }
                    else
                    {
                        SetProcessCount("Failed");
                        success = false;
                    }
                }

                PrintProcessCount();
            }

            return success;
        }

        public bool GetNextBulkRiData()
        {
            RiDataBos = new List<RiDataBo> { };
            ProcessWarehouseRiDatas = new List<ProcessWarehouseRiData> { };

            if (RiDataBatchBo.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessFailed)
                Total = RiDataService.CountByRiDataBatchIdProcessWarehouseStatesFailed(RiDataBatchBo.Id);
            else
                Total = RiDataService.CountByRiDataBatchId(RiDataBatchBo.Id);

            if (Skip >= Total)
                return false;
            if (Skip >= MaxFinaliseItems && MaxFinaliseItems.HasValue)
                return false;

            if (RiDataBatchBo.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessFailed)
                RiDataBos = RiDataService.GetByRiDataBatchIdProcessWarehouseStatus(RiDataBatchBo.Id, RiDataBo.ProcessWarehouseStatusFailed, Skip, Take);
            else
                RiDataBos = RiDataService.GetByRiDataBatchId(RiDataBatchBo.Id, Skip, Take);

            foreach (var riData in RiDataBos)
            {
                ProcessWarehouseRiDatas.Add(new ProcessWarehouseRiData(this, riData));
            }

            Skip += Take;
            return true;
        }

        public void GetPropertyNames()
        {
            PropertyNames = new List<string>() { "RecordType" };
            List<int> excludedTypes = StandardOutputBo.GetWarehouseExcludedTypes();
            for (int i = 1; i <= StandardOutputBo.TypeMax; i++)
            {
                if (excludedTypes.Contains(i))
                    continue;

                string propertyName = StandardOutputBo.GetPropertyNameByType(i);
                PropertyNames.Add(propertyName);
            }
        }

        public void GetPolicyStatusCodes()
        {
            PolicyStatusCodes = new Dictionary<string, int>();
            foreach (PickListDetailBo bo in PickListDetailService.GetByPickListId(PickListBo.PolicyStatusCode))
            {
                PolicyStatusCodes[bo.Code] = bo.Id;
            }
        }

        public void UpdateBatchCompileStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();

            var batch = RiDataBatchBo;
            RiDataBatchBo.ProcessWarehouseStatus = status;

            using (var db = new AppDbContext(false))
            {
                RiDataBatchService.CountTotalFailed(ref batch, db);
            }

            Result result = RiDataBatchService.Update(ref batch, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                RiDataBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public RiDataBatchBo LoadRiDataBatchBo()
        {
            RiDataBatchBo = null;
            if (CutOffService.IsCutOffProcessing())
                return RiDataBatchBo;

            if (RiDataBatchId.HasValue)
            {
                RiDataBatchBo = RiDataBatchService.Find(RiDataBatchId.Value);
                if (RiDataBatchBo != null && !PendingProcessWarehouseStatuses.Contains(RiDataBatchBo.ProcessWarehouseStatus))
                {
                    RiDataBatchBo = null;
                }
            }
            else
            {
                RiDataBatchBo = RiDataBatchService.FindByProcessWarehouseStatuses(PendingProcessWarehouseStatuses);
            }
            return RiDataBatchBo;
        }

        public void SetStoredProcedure()
        {
            EnableStoredProcedure = Util.GetConfigBoolean("EnableStoredProcedure");
            if (EnableStoredProcedure)
            {
                StoredProcedure = new StoredProcedure(StoredProcedure.RiDataMigrateWarehouse);
                EnableStoredProcedure = StoredProcedure.IsExists();
            }

            PrintMessage(string.Format("Using Stored Procedure: {0}", EnableStoredProcedure ? "Yes" : "No"));
        }
    }
}
