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
    class ProcessPerLifeDetailDataValidation : Command
    {
        public int? PerLifeAggregationDetailId { get; set; }
        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public ProcessPerLifeDetailDataValidation()
        {
            Title = "ProcessPerLifeDetailDataValidation";
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
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForValidation)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingValidation, true, false);
                    return;
                }
            }
            if (PerLifeAggregationDetailService.CountByStatus(PerLifeAggregationDetailBo.StatusSubmitForValidation) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingValidation, true, false);
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

                UpdateStatus(PerLifeAggregationDetailBo.StatusValidating, MessageBag.ProcessPerLifeAggregationDetailValidating);



                bool success = true;
                try
                {
                    StoredProcedure.AddParameter("PerLifeAggregationDetailId", PerLifeAggregationDetailBo.Id);
                    StoredProcedure.Execute(true);
                    if (!string.IsNullOrEmpty(StoredProcedure.Result))
                    {
                        string jsonResult = "{" + StoredProcedure.Result + "}";
                        var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonResult);
                        foreach (var r in result)
                        {
                            SetProcessCount(r.Key, r.Value);
                            //if (r.Key == "Failed" && r.Value > 0)
                            //{
                            //    success = false;
                            //}
                        }
                        PrintProcessCount();
                    }
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                }

                if (success)
                    UpdateStatus(PerLifeAggregationDetailBo.StatusValidationSuccess, MessageBag.ProcessPerLifeAggregationDetailValidationSuccess);
                else
                    UpdateStatus(PerLifeAggregationDetailBo.StatusValidationFailed, MessageBag.ProcessPerLifeAggregationDetailValidationFailed);

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
            //if (status == PerLifeAggregationDetailBo.StatusProcessing)
            //{
            //    PerLifeAggregationDetailBo.ProcessingDate = DateTime.Now;
            //}

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
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForValidation)
                    PerLifeAggregationDetailBo = null;
            }
            else
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.FindByStatus(PerLifeAggregationDetailBo.StatusSubmitForValidation);
            }

            return PerLifeAggregationDetailBo;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeDetailDataValidation);
            return StoredProcedure.IsExists();
        }
    }
}
