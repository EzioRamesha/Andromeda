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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Retrocession
{
    public class ProcessPerLifeSplitData : Command
    {
        public int? PerLifeAggregationDetailId { get; set; }
        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeSplitData()
        {
            Title = "ProcessPerLifeSplitData";
            Description = "To process Per Life Split Data";
            Options = new string[] {
                "--t|test : Test process data",
                "--perLifeAggregationDetailId= : Process by Id",
            };
        }

        public override void Run()
        {
            if (PerLifeAggregationDetailId.HasValue)
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(PerLifeAggregationDetailId.Value);
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForProcessing)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingProcess, true, false);
                    return;
                }
            }
            if (PerLifeAggregationDetailService.CountByStatus(PerLifeAggregationDetailBo.StatusSubmitForProcessing) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingProcess, true, false);
                return;
            }
            if (!SetStoredProcedure())
            {
                Log = false;
                PrintMessage(MessageBag.StoredProcedureNotFound, true, false);
                return;
            }
            PrintStarting();

            while (LoadPerLifeAggregationDetailBo() != null)
            {
                SetProcessCount("PerLifeAggregationDetail");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Aggregation Detail Id: {0}", PerLifeAggregationDetailBo.Id));

                UpdateStatus(PerLifeAggregationDetailBo.StatusProcessing, MessageBag.ProcessPerLifeAggregationDetailProcessing);

                bool success = true;
                try
                {
                    StoredProcedure.AddParameter("PerLifeAggregationDetailId", PerLifeAggregationDetailBo.Id);
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
                    UpdateStatus(PerLifeAggregationDetailBo.StatusProcessSuccess, MessageBag.ProcessPerLifeAggregationDetailSuccess);
                else
                    UpdateStatus(PerLifeAggregationDetailBo.StatusProcessFailed, MessageBag.ProcessPerLifeAggregationDetailFailed);

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

            var perLifeAggregationDetail = PerLifeAggregationDetailBo;
            PerLifeAggregationDetailBo.Status = status;
            if (status == PerLifeAggregationDetailBo.StatusProcessing)
            {
                PerLifeAggregationDetailBo.ProcessingDate = DateTime.Now;
            }

            var result = PerLifeAggregationDetailService.Update(ref perLifeAggregationDetail, ref trail);
            var userTrailBo = new UserTrailBo(
                PerLifeAggregationDetailBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public PerLifeAggregationDetailBo LoadPerLifeAggregationDetailBo()
        {
            PerLifeAggregationDetailBo = null;
            if (PerLifeAggregationDetailId.HasValue)
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(PerLifeAggregationDetailId.Value);
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForProcessing)
                    PerLifeAggregationDetailBo = null;
            }
            else
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.FindByStatus(PerLifeAggregationDetailBo.StatusSubmitForProcessing);
            }

            return PerLifeAggregationDetailBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeSplitData);
            return StoredProcedure.IsExists();
        }
    }
}
