using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.Sanctions;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessSanctionVerification : Command
    {
        public int? SanctionVerificationId { get; set; }

        public bool IsSkipFormatName { get; set; }

        public SanctionVerificationBo SanctionVerificationBo { get; set; }

        public StoredProcedure StoredProcedure { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public ProcessSanctionVerification()
        {
            Title = "ProcessSanctionVerification";
            Description = "To process Sanction Verification";
            Options = new string[] {
                "--sanctionVerificationId= : Process by Id",
                "--skipFormatName= : Skip Format Sanction Name",
            };
        }

        public override void Initial()
        {
            base.Initial();
            SanctionVerificationId = OptionIntegerNullable("sanctionVerificationId");
            IsSkipFormatName = IsOption("skipFormatName");
        }

        public override void Run()
        {
            if (SanctionVerificationId.HasValue)
            {
                SanctionVerificationBo = SanctionVerificationService.Find(SanctionVerificationId.Value);
                if (SanctionVerificationBo != null && SanctionVerificationBo.Status != SanctionVerificationBo.StatusPending)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoSanctionVerificationPendingProcess);
                    return;
                }
            }
            else if (SanctionVerificationService.CountByStatus(SanctionVerificationBo.StatusPending) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoSanctionVerificationPendingProcess);
                return;
            }

            if (!SetStoredProcedure())
            {
                PrintMessage("Sanction Verification Stored Procedure does not exist");
                return;
            }

            StoredProcedure.PrintFunction = PrintMessage;

            PrintStarting();

            if (!IsSkipFormatName)
            {
                PrintOutputTitle("Process Sanction Name");
                ProcessSanctionNameBatch process = new ProcessSanctionNameBatch()
                {
                    Title = Title,
                    Take = Util.GetConfigInteger("ProcessSanctionNameRow", 1000),
                    PrintStartEnd = false,
                    ProcessName = "Processed Sanction",
                    LogIndex = 0
                };
                process.Run();

                PrintMessage();
            }

            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.SanctionVerification.ToString());

            while (LoadSanctionVerificationBo())
            {
                try
                {
                    UpdateSanctionVerificationStatus(SanctionVerificationBo.StatusProcessing, "Processing Sanction Verification");

                    PrintMessage(string.Format("Processing Sanction Verification: {0}", SanctionVerificationBo.Id));

                    SanctionVerificationBo.ProcessStartAt = DateTime.Now;

                    StoredProcedure.AddParameter("SanctionVerificationId", SanctionVerificationBo.Id);
                    StoredProcedure.Execute(true);


                    SanctionVerificationBo.ProcessEndAt = DateTime.Now;

                    if (!string.IsNullOrEmpty(StoredProcedure.Result))
                    {
                        var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(StoredProcedure.Result);
                        foreach (var r in result)
                        {
                            SetProcessCount(r.Key, r.Value);
                            if (r.Key == "Total")
                            {
                                if (r.Value == 0)
                                {
                                    PrintMessage("No Data Found");
                                }
                                SanctionVerificationBo.Record = r.Value;
                                SanctionVerificationBo.UnprocessedRecords = r.Value;
                            }
                        }
                        PrintProcessCount();
                    }

                    UpdateSanctionVerificationStatus(SanctionVerificationBo.StatusCompleted, "Process Sanction Verification Completed");
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                    UpdateSanctionVerificationStatus(SanctionVerificationBo.StatusFailed, "Process Sanction Verification Failed");
                }
            }

            PrintEnding();
        }

        public void UpdateSanctionVerificationStatus(int status, string description)
        {
            var trail = new TrailObject();

            var sanctionVerification = SanctionVerificationBo;
            sanctionVerification.Status = status;

            var result = SanctionVerificationService.Update(ref sanctionVerification, ref trail);
            if (result.Valid)
            {
                var statusBo = new StatusHistoryBo
                {
                    ModuleId = ModuleBo.Id,
                    ObjectId = SanctionVerificationBo.Id,
                    Status = status,
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };
                StatusHistoryService.Create(ref statusBo, ref trail);

                var userTrailBo = new UserTrailBo(
                    SanctionVerificationBo.Id,
                    description,
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }

        public bool LoadSanctionVerificationBo()
        {
            SanctionVerificationBo = null;
            if (SanctionVerificationId.HasValue)
            {
                SanctionVerificationBo = SanctionVerificationService.Find(SanctionVerificationId.Value);
                if (SanctionVerificationBo != null && SanctionVerificationBo.Status != SanctionVerificationBo.StatusPending)
                    return false;
            }
            else
            {
                SanctionVerificationBo = SanctionVerificationService.FindByStatus(SanctionVerificationBo.StatusPending);
            }

            return SanctionVerificationBo != null;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.SanctionVerification);
            return StoredProcedure.IsExists();
        }
    }
}
