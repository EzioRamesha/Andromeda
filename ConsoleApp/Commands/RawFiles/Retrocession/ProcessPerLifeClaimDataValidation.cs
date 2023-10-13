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
    public class ProcessPerLifeClaimDataValidation : Command
    {
        public int? PerLifeClaimId { get; set; }
        public PerLifeClaimBo PerLifeClaimBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeClaimDataValidation()
        {
            Title = "ProcessPerLifeClaimDataValidation";
            Description = "To process Per Life Claims Data Validation";
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
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForValidation)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeClaimDataPendingValidation, true, false);
                    return;
                }
            }
            if (PerLifeClaimService.CountByStatus(PerLifeClaimBo.StatusSubmitForValidation) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeClaimDataPendingValidation, true, false);
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
                SetProcessCount("PerLifeClaimData");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Claim Id: {0}", PerLifeClaimBo.Id));

                UpdateStatus(PerLifeClaimBo.StatusValidating, MessageBag.ProcessPerLifeClaimDataValidating);

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
                    UpdateStatus(PerLifeClaimBo.StatusValidationSuccess, MessageBag.ProcessPerLifeClaimDataValidationSuccess);
                else
                    UpdateStatus(PerLifeClaimBo.StatusValidationFailed, MessageBag.ProcessPerLifeClaimDataValidationFailed);

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
                if (PerLifeClaimBo != null && PerLifeClaimBo.Status != PerLifeClaimBo.StatusSubmitForValidation)
                    PerLifeClaimBo = null;
            }
            else
            {
                PerLifeClaimBo = PerLifeClaimService.FindByStatus(PerLifeClaimBo.StatusSubmitForValidation);
            }

            return PerLifeClaimBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeClaimDataValidation);
            return StoredProcedure.IsExists();
        }
    }
}
