using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.Retrocession;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.Retrocession
{
    public class ProcessPerLifeDataValidation : Command
    {
        public int? PerLifeAggregationId { get; set; }
        public PerLifeAggregationBo PerLifeAggregationBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeDataValidation()
        {
            Title = "ProcessPerLifeDataValidation";
            Description = "To process Per Life Data Validation";
            Options = new string[] {
                "--t|test : Test process data",
                "--perLifeAggregationId= : Process by Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeAggregation.ToString());
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
            if (PerLifeAggregationId.HasValue)
            {
                PerLifeAggregationBo = PerLifeAggregationService.Find(PerLifeAggregationId.Value);
                if (PerLifeAggregationBo != null && PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusSubmitForProcessing)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeAggregationPendingProcess, true, false);
                    return;
                }
            }
            if (PerLifeAggregationService.CountByStatus(PerLifeAggregationBo.StatusSubmitForProcessing) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeAggregationPendingProcess, true, false);
                return;
            }
            if (!SetStoredProcedure())
            {
                Log = false;
                PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                return;
            }
            PrintStarting();

            while (LoadPerLifeAggregationBo() != null)
            {
                SetProcessCount("PerLifeAggregation");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Aggregation Id: {0}", PerLifeAggregationBo.Id));

                UpdateStatus(PerLifeAggregationBo.StatusProcessing, MessageBag.ProcessPerLifeAggregationProcessing);

                if (PerLifeAggregationService.IsDataInUse(PerLifeAggregationBo.Id))
                {
                    UpdateError(MessageBag.UnableProcessPerLifeAggregation);
                    continue;
                }

                //bool success = true;
                try
                {
                    StoredProcedure.AddParameter("PerLifeAggregationId", PerLifeAggregationBo.Id);
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

                // If no Risk Quarter Record set to success
                if (PerLifeAggregationDetailService.CountByPerLifeAggregationId(PerLifeAggregationBo.Id) == 0)
                {
                    UpdateStatus(PerLifeAggregationBo.StatusSuccess, MessageBag.ProcessPerLifeAggregationSuccess);
                }
                else
                {
                    UpdateStatus(PerLifeAggregationBo.StatusPendingRiskQuarterProcessing, MessageBag.ProcessPerLifeAggregationSuccess);
                }

                //if (success)
                //    UpdateStatus(PerLifeAggregationBo.StatusSuccess, MessageBag.ProcessPerLifeAggregationSuccess);
                //else
                //    UpdateStatus(PerLifeAggregationBo.StatusFailed, MessageBag.ProcessPerLifeAggregationFailed);

                if (Test)
                    break; // For testing only process one batch
            }

            PrintEnding();
        }

        public void UpdateError(string error)
        {
            PerLifeAggregationBo.Errors = error;
            UpdateStatus(PerLifeAggregationBo.StatusFailed, MessageBag.ProcessPerLifeAggregationFailed);
        }

        public void UpdateStatus(int status, string description)
        {
            if (Test)
                return;

            var trail = new TrailObject();

            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = PerLifeAggregationBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var perLifeAggregation = PerLifeAggregationBo;
            PerLifeAggregationBo.Status = status;
            if (status == PerLifeAggregationBo.StatusProcessing)
            {
                PerLifeAggregationBo.ProcessingDate = DateTime.Now;
                PerLifeAggregationBo.Errors = null;
            }

            var result = PerLifeAggregationService.Update(ref perLifeAggregation, ref trail);
            var userTrailBo = new UserTrailBo(
                PerLifeAggregationBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public PerLifeAggregationBo LoadPerLifeAggregationBo()
        {
            PerLifeAggregationBo = null;
            if (PerLifeAggregationId.HasValue)
            {
                PerLifeAggregationBo = PerLifeAggregationService.Find(PerLifeAggregationId.Value);
                if (PerLifeAggregationBo != null && PerLifeAggregationBo.Status != PerLifeAggregationBo.StatusSubmitForProcessing)
                    PerLifeAggregationBo = null;
            }
            else
            {
                PerLifeAggregationBo = PerLifeAggregationService.FindByStatus(PerLifeAggregationBo.StatusSubmitForProcessing);
            }

            return PerLifeAggregationBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeDataValidation);
            return StoredProcedure.IsExists();
        }
    }
}
