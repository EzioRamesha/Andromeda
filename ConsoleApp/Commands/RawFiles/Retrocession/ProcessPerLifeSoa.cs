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
    public class ProcessPerLifeSoa : Command
    {
        public int? PerLifeSoaId { get; set; }
        public PerLifeSoaBo PerLifeSoaBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeSoa()
        {
            Title = "ProcessPerLifeSoa";
            Description = "To process Per Life SOA Data";
            Options = new string[] {
                "--t|test : Test process data",
                "--PerLifeSoaId= : Process by Id",
            };
        }

        public override void Run()
        {
            if (PerLifeSoaId.HasValue)
            {
                PerLifeSoaBo = PerLifeSoaService.Find(PerLifeSoaId.Value);
                if (PerLifeSoaBo != null && PerLifeSoaBo.Status != PerLifeSoaBo.StatusSubmitForProcessing)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeSoaPendingProcess, true, false);
                    return;
                }
            }
            if (PerLifeSoaService.CountByStatus(PerLifeSoaBo.StatusSubmitForProcessing) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeSoaPendingProcess, true, false);
                return;
            }
            if (!SetStoredProcedure())
            {
                Log = false;
                PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                return;
            }
            PrintStarting();

            while (LoadPerLifeSoaBo() != null)
            {
                SetProcessCount("PerLifeSoa");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Claim Id: {0}", PerLifeSoaBo.Id));

                UpdateStatus(PerLifeSoaBo.StatusProcessing, MessageBag.ProcessPerLifeSoaProcessing);

                bool success = true;
                try
                {
                    StoredProcedure.AddParameter("PerLifeSoaId", PerLifeSoaBo.Id);
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
                    UpdateStatus(PerLifeSoaBo.StatusProcessingSuccess, MessageBag.ProcessPerLifeSoaSuccess);
                else
                    UpdateStatus(PerLifeSoaBo.StatusProcessingFailed, MessageBag.ProcessPerLifeSoaFailed);

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

            var perLifeSoa = PerLifeSoaService.Find(PerLifeSoaBo.Id);
            perLifeSoa.Status = status;

            var result = PerLifeSoaService.Update(ref perLifeSoa, ref trail);
            var userTrailBo = new UserTrailBo(
                perLifeSoa.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public PerLifeSoaBo LoadPerLifeSoaBo()
        {
            PerLifeSoaBo = null;
            if (PerLifeSoaId.HasValue)
            {
                PerLifeSoaBo = PerLifeSoaService.Find(PerLifeSoaId.Value);
                if (PerLifeSoaBo != null && PerLifeSoaBo.Status != PerLifeSoaBo.StatusSubmitForProcessing)
                    PerLifeSoaBo = null;
            }
            else
            {
                PerLifeSoaBo = PerLifeSoaService.FindByStatus(PerLifeSoaBo.StatusSubmitForProcessing);
            }

            return PerLifeSoaBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeSoaProcessing);
            return StoredProcedure.IsExists();
        }
    }
}
