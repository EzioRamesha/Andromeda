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
    public class ProcessPerLifeClaimsRetroRecovery : Command
    {
        public int? PerLifeClaimId { get; set; }
        public PerLifeClaimBo PerLifeClaimBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeClaimsRetroRecovery()
        {
            Title = "ProcessPerLifeClaimsRetroRecovery";
            Description = "To process Per Life Claims Retro Recovery";
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
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForRetroRecovery)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeClaimRetroRecoveryPendingProcess, true, false);
                    return;
                }
            }
            if (PerLifeClaimService.CountByStatus(PerLifeClaimBo.StatusSubmitForRetroRecovery) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeClaimRetroRecoveryPendingProcess, true, false);
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

                UpdateStatus(PerLifeClaimBo.StatusProcessingRetroRecovery, MessageBag.ProcessPerLifeClaimRetroRecoveryProcessing);

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
                    UpdateStatus(PerLifeClaimBo.StatusProcessingRetroRecoverySuccess, MessageBag.ProcessPerLifeClaimRetroRecoverySuccess);
                else
                    UpdateStatus(PerLifeClaimBo.StatusProcessingRetroRecoveryFailed, MessageBag.ProcessPerLifeClaimRetroRecoveryFailed);

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

            var perLifeClaim = PerLifeClaimBo;
            PerLifeClaimBo.Status = status;

            var result = PerLifeClaimService.Update(ref perLifeClaim, ref trail);
            var userTrailBo = new UserTrailBo(
                PerLifeClaimBo.Id,
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
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForRetroRecovery)
                    PerLifeClaimBo = null;
            }
            else
            {
                PerLifeClaimBo = PerLifeClaimService.FindByStatus(PerLifeClaimBo.StatusSubmitForRetroRecovery);
            }

            return PerLifeClaimBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeClaimsRetroRecoveryProcess);
            return StoredProcedure.IsExists();
        }
    }
}
