using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services.Identity;
using Services.Retrocession;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.Retrocession
{
    public class ProcessPerLifeClaims : Command
    {
        public int? PerLifeClaimId { get; set; }
        public PerLifeClaimBo PerLifeClaimBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeClaims()
        {
            Title = "ProcessPerLifeClaims";
            Description = "To process Per Life Claims Data";
            Options = new string[] {
                "--t|test : Test process data",
                "--perLifeClaimId= : Process by Id",
            };
        }

        public override void Run()
        {
            if (PerLifeClaimId.HasValue)
            {
                PerLifeClaimBo = PerLifeClaimService.Find(PerLifeClaimId.Value);
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForProcessing)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeClaimPendingProcess, true, false);
                    return;
                }
            }
            if (PerLifeClaimService.CountByStatus(PerLifeClaimBo.StatusSubmitForProcessing) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeClaimPendingProcess, true, false);
                return;
            }
            if (!SetStoredProcedure())
            {
                Log = false;
                PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                return;
            }
            PrintStarting();

            while (LoadPerLifeClaimBo() != null)
            {
                SetProcessCount("PerLifeClaim");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Claim Id: {0}", PerLifeClaimBo.Id));

                UpdateStatus(PerLifeClaimBo.StatusProcessing, MessageBag.ProcessPerLifeClaimProcessing);

                bool success = true;
                try
                {
                    StoredProcedure.AddParameter("PerLifeClaimId", PerLifeClaimBo.Id);
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
                    PrintError(e.Message);
                }

                if (success)
                    UpdateStatus(PerLifeClaimBo.StatusProcessingSuccess, MessageBag.ProcessPerLifeClaimSuccess);
                else
                    UpdateStatus(PerLifeClaimBo.StatusProcessingFailed, MessageBag.ProcessPerLifeClaimFailed);

                if (Test)
                    break; // For testing only process one batch
            }

            PrintEnding();
        }

        public void UpdateStatus(int status, string description)
        {
            if (Test)
                return;

            var trail = new TrailObject();

            var perLifeClaim = PerLifeClaimService.Find(PerLifeClaimBo.Id);
            perLifeClaim.Status = status;

            var result = PerLifeClaimService.Update(ref perLifeClaim, ref trail);
            var userTrailBo = new UserTrailBo(
                perLifeClaim.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public PerLifeClaimBo LoadPerLifeClaimBo()
        {
            PerLifeClaimBo = null;
            if (PerLifeClaimId.HasValue)
            {
                PerLifeClaimBo = PerLifeClaimService.Find(PerLifeClaimId.Value);
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForProcessing)
                    PerLifeClaimBo = null;
            }
            else
            {
                PerLifeClaimBo = PerLifeClaimService.FindByStatus(PerLifeClaimBo.StatusSubmitForProcessing);
            }

            return PerLifeClaimBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeClaimsProcessing);
            return StoredProcedure.IsExists();
        }
    }
}
