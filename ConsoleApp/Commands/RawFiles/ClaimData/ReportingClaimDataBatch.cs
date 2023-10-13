using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.Claims;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class ReportingClaimDataBatch : Command
    {
        public int? ClaimDataBatchId { get; set; }
        public bool Test { get; set; }

        public ModuleBo ModuleBo { get; set; }
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }
        public IList<ClaimDataFileBo> ClaimDataFileBos { get; set; }
        public StatusHistoryBo ReportingStatusHistoryBo { get; set; }
        public LogReportClaimData LogReportClaimData { get; set; }
        public string SummaryFilePath { get; set; }

        public int Total { get; set; } = 0;
        public int Take { get; set; } = 100;
        public int Skip { get; set; } = 0;
        public bool Success { get; set; }
        public IList<ClaimDataBo> ClaimDataBos { get; set; } = new List<ClaimDataBo> { };
        public IList<ReportingClaimData> ReportingClaimDatas { get; set; } = new List<ReportingClaimData> { };
        public IList<ClaimRegisterBo> ClaimRegisterBos { get; set; } = new List<ClaimRegisterBo> { };

        public ReportingClaimDataBatch()
        {
            Title = "ReportingClaimDataBatch";
            Description = "To report Claim Data to Claim Register";
            Options = new string[] {
                "--i|claimDataBatchId= : Enter the ClaimDataBatchId",
                "--t|test : Test process data",
            };
        }

        public override void Initial()
        {
            base.Initial();

            ClaimDataBatchId = OptionIntegerNullable("claimDataBatchId");
            Test = IsOption("test");

            Take = Util.GetConfigInteger("ReportClaimDataItems", 100);
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
            if (CutOffService.IsCutOffProcessing())
            {
                Log = false;
                PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                return;
            }
            if (ClaimDataBatchService.CountByStatus(ClaimDataBatchBo.StatusSubmitForReportClaim) == 0)
            {
                Log = false;
                PrintMessage("No Claim Data Batch pending to report", true, false);
                return;
            }

            PrintStarting();

            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimData.ToString());

            while (LoadClaimDataBatch() != null)
            {
                SetProcessCount("Batch");

                if (!Test)
                {
                    ReportingStatusHistoryBo = UpdateBatchStatus(ClaimDataBatchBo.StatusReportingClaim, "Reporting Claim Data Batch");
                    CreateClaimDataBatchStatusFile();
                }

                Report();

                if (!Test)
                {
                    if (Success)
                    {
                        UpdateBatchStatus(ClaimDataBatchBo.StatusReportedClaim, MessageBag.ReportedClaimDataBatch);
                    }
                    else 
                    {
                        UpdateBatchStatus(ClaimDataBatchBo.StatusReportingClaimFailed, MessageBag.ReportingClaimDataBatchFailed);
                    }
                    WriteSummary();
                }
                else
                {
                    // test one batch only
                    break;
                }
            }

            PrintEnding();
        }

        public void Report()
        {
            Skip = 0;
            LogReportClaimData = new LogReportClaimData(ClaimDataBatchBo);
            LogReportClaimData.SwReport.Start();

            while (GetNextBulkClaimData())
            {
                Parallel.ForEach(ReportingClaimDatas, r => r.Report());

                if (!Test)
                {
                    foreach (var r in ReportingClaimDatas)
                    {
                        if (!r.Valid)
                            Success = false;

                        if (r.ClaimRegisterBo == null)
                            continue;

                        ClaimRegisterBos.Add(r.ClaimRegisterBo);
                    }
                }

                SetProcessCount(number: ReportingClaimDatas.Count, acc: true);
                LogReportClaimData.Total += ReportingClaimDatas.Count;

                PrintProcessCount();
            }

            if (Success)
            {
                var claimRegisterModule = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
                foreach (var bo in ClaimRegisterBos)
                {
                    TrailObject trail = new TrailObject();

                    var claimRegisterBo = bo;
                    claimRegisterBo.EntryNo = ClaimRegisterService.GetNextEntryNo();
                    claimRegisterBo.LastTransactionDate = DateTime.Today;
                    claimRegisterBo.LastTransactionQuarter = Util.GetCurrentQuarter();
                    Result result = ClaimRegisterService.Create(ref claimRegisterBo, ref trail);
                    if (result.Valid)
                    {
                        StatusHistoryBo statusBo = new StatusHistoryBo
                        {
                            ModuleId = claimRegisterModule.Id,
                            ObjectId = claimRegisterBo.Id,
                            Status = claimRegisterBo.ClaimStatus,
                            CreatedById = User.DefaultSuperUserId,
                            UpdatedById = User.DefaultSuperUserId,
                        };
                        StatusHistoryService.Create(ref statusBo, ref trail);

                        UserTrailBo userTrailBo = new UserTrailBo(
                            claimRegisterBo.Id,
                            "Create Claim Register",
                            result,
                            trail,
                            User.DefaultSuperUserId
                        );
                        UserTrailService.Create(ref userTrailBo);
                    }
                }

                //var provisionClaimRegister = new ProvisionClaimRegisterBatch()
                //{
                //    Title = Title,
                //    ClaimDataBatchId = ClaimDataBatchBo.Id,
                //    IsGenerateE3E4 = false
                //};
                //provisionClaimRegister.Run();
            }

            LogReportClaimData.SwReport.Stop();
        }

        public bool GetNextBulkClaimData()
        {
            ClaimDataBos = new List<ClaimDataBo> { };
            ReportingClaimDatas = new List<ReportingClaimData> { };
            Total = ClaimDataService.CountByClaimDataBatchId(ClaimDataBatchBo.Id);
            if (Skip >= Total)
                return false;

            ClaimDataBos = ClaimDataService.GetByClaimDataBatchId(ClaimDataBatchBo.Id, Skip, Take);
            foreach (var claimDataBo in ClaimDataBos)
            {
                ReportingClaimDatas.Add(new ReportingClaimData(this, claimDataBo));
            }

            Skip += Take;
            return true;
        }

        public void CreateClaimDataBatchStatusFile()
        {
            if (ClaimDataBatchBo == null)
                return;
            if (ReportingStatusHistoryBo == null)
                return;

            TrailObject trail = new TrailObject();
            var claimDataBatchStatusFileBo = new ClaimDataBatchStatusFileBo
            {
                ClaimDataBatchId = ClaimDataBatchBo.Id,
                StatusHistoryId = ReportingStatusHistoryBo.Id,
                StatusHistoryBo = ReportingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var result = ClaimDataBatchStatusFileService.Create(ref claimDataBatchStatusFileBo, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                claimDataBatchStatusFileBo.Id,
                "Reporting Claim Data Batch",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            SummaryFilePath = claimDataBatchStatusFileBo.GetFilePath();
            Util.MakeDir(SummaryFilePath);
            if (File.Exists(SummaryFilePath))
                File.Delete(SummaryFilePath);
        }

        public void WriteSummaryLine(object line)
        {
            using (var file = new TextFile(SummaryFilePath, true, true))
            {
                file.WriteLine(line);
            }
        }

        public void WriteSummary()
        {
            foreach (string line in LogReportClaimData.GetDetails())
            {
                WriteSummaryLine(line);
            }
        }

        public ClaimDataBatchBo LoadClaimDataBatch()
        {
            if (CutOffService.IsCutOffProcessing())
                return null;
            if (ClaimDataBatchId != null)
                ClaimDataBatchBo = ClaimDataBatchService.Find(ClaimDataBatchId.Value);
            else
                ClaimDataBatchBo = ClaimDataBatchService.FindByStatus(ClaimDataBatchBo.StatusSubmitForReportClaim);
            LoadClaimDataFiles();
            ClaimRegisterBos = new List<ClaimRegisterBo>();
            Success = true;
            return ClaimDataBatchBo;
        }

        public void LoadClaimDataFiles()
        {
            ClaimDataFileBos = new List<ClaimDataFileBo> { };
            if (ClaimDataBatchBo == null)
                return;

            ClaimDataFileBos = ClaimDataFileService.GetConfigIdByClaimDataBatchId(ClaimDataBatchBo.Id);
        }

        public StatusHistoryBo UpdateBatchStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = ClaimDataBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = ClaimDataBatchBo;
            ClaimDataBatchBo.Status = status;

            using (var db = new AppDbContext(false))
            {
                ClaimDataBatchService.CountTotalFailed(ref batch, db);
            }

            Result result = ClaimDataBatchService.Update(ref batch, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                ClaimDataBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            return statusBo;
        }
    }
}
