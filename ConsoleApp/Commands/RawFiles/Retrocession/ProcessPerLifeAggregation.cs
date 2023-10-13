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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Retrocession
{
    public class ProcessPerLifeAggregation : Command
    {
        public int? PerLifeAggregationDetailId { get; set; }
        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }
        public StoredProcedure StoredProcedure { get; set; }
        public bool Test { get; set; } = false;

        public IList<ProcessPerLifeAggregationDetail> ProcessPerLifeAggregationDetails { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public IList<RetroTreatyDetailBo> RetroTreatyDetailBos { get; set; }
        public IList<GstMaintenanceBo> GstMaintenanceBos { get; set; }
        public ModuleBo PerLifeAggregationModuleBo { get; set; }

        public ProcessPerLifeAggregation()
        {
            Title = "ProcessPerLifeAggregation";
            Description = "To process Per Life Aggregation";
            Options = new string[] {
                "--t|test : Test process data",
                "--perLifeAggregationId= : Process by Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            Take = Util.GetConfigInteger("ProcessPerLifeAggregation", 1000);
            Test = IsOption("test");
            PerLifeAggregationModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeAggregation.ToString());
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
            if(PerLifeAggregationDetailId.HasValue)
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(PerLifeAggregationDetailId.Value);
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForAggregation)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingAggregation, true, false);
                    return;
                }
            }
            if (PerLifeAggregationDetailService.CountByStatus(PerLifeAggregationDetailBo.StatusSubmitForAggregation) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoPerLifeAggregationDetailPendingAggregation, true, false);
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
                SetProcessCount("PerLifeAggregation");
                // Reset
                SetProcessCount("Processed", 0);
                SetProcessCount("Ignored", 0);
                SetProcessCount("Success", 0);
                SetProcessCount("Failed", 0);

                PrintOutputTitle(string.Format("Processing Per Life Aggregation Detail Id: {0}", PerLifeAggregationDetailBo.Id));

                if (!Test)
                {
                    UpdateStatus(PerLifeAggregationDetailBo.StatusAggregating, MessageBag.ProcessPerLifeAggregationProcessing);
                }

                Skip = 0;
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

                    LoadConfigurations();
                    while(GetNextBulkMonthlyData())
                    {
                        Parallel.ForEach(ProcessPerLifeAggregationDetails, f => f.ComputeRetroPeremium());
                        if (ProcessPerLifeAggregationDetails.Any(q => !q.Success))
                            success = false;

                        if (Test)
                            break;
                    }
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                }

                if (!Test)
                {
                    if (success)
                        UpdateStatus(PerLifeAggregationDetailBo.StatusAggregationSuccess, MessageBag.ProcessPerLifeAggregationDetailAggregationSuccess, true);
                    else
                        UpdateStatus(PerLifeAggregationDetailBo.StatusAggregationFailed, MessageBag.ProcessPerLifeAggregationDetailAggregationFailed, true);
                }

                if (Test)
                    break; // For testing only process one batch
            }

            PrintEnding();
        }

        public void UpdateStatus(int status, string description, bool isEnd = false)
        {
            if (Test)
                return;

            var trail = new TrailObject();

            var perLifeAggregation = PerLifeAggregationDetailBo;
            PerLifeAggregationDetailBo.Status = status;
            //if (status == PerLifeAggregationDetailBo.StatusProcessing)
            //{
            //    PerLifeAggregationDetailBo.ProcessingDate = DateTime.Now;
            //}

            var result = PerLifeAggregationDetailService.Update(ref perLifeAggregation, ref trail);
            var userTrailBo = new UserTrailBo(
                PerLifeAggregationDetailBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (isEnd)
            {
                if (PerLifeAggregationDetailService.IsNotStatusesPerLifeAggregationId(new List<int> { PerLifeAggregationDetailBo.StatusAggregationSuccess, PerLifeAggregationDetailBo.StatusFinalised }, perLifeAggregation.PerLifeAggregationId))
                    return;

                var perLifeAggregationBo = PerLifeAggregationService.Find(perLifeAggregation.PerLifeAggregationId);
                if (perLifeAggregationBo != null)
                {
                    StatusHistoryBo statusBo = new StatusHistoryBo
                    {
                        ModuleId = PerLifeAggregationModuleBo.Id,
                        ObjectId = perLifeAggregationBo.Id,
                        Status = PerLifeAggregationBo.StatusSuccess,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    StatusHistoryService.Create(ref statusBo, ref trail);

                    perLifeAggregationBo.Status = PerLifeAggregationBo.StatusSuccess;
                    perLifeAggregationBo.UpdatedById = User.DefaultSuperUserId;

                    trail = new TrailObject();
                    result = PerLifeAggregationService.Update(ref perLifeAggregationBo, ref trail);
                    userTrailBo = new UserTrailBo(
                        perLifeAggregationBo.Id,
                        "Update Per Life Aggregation",
                        result,
                        trail,
                        User.DefaultSuperUserId
                    );
                    UserTrailService.Create(ref userTrailBo);
                }
            }
        }

        public PerLifeAggregationDetailBo LoadPerLifeAggregationDetailBo()
        {
            PerLifeAggregationDetailBo = null;
            if (PerLifeAggregationDetailId.HasValue)
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(PerLifeAggregationDetailId.Value);
                if (PerLifeAggregationDetailBo != null && PerLifeAggregationDetailBo.Status != PerLifeAggregationDetailBo.StatusSubmitForAggregation)
                    PerLifeAggregationDetailBo = null;
            }
            else
            {
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.FindByStatus(PerLifeAggregationDetailBo.StatusSubmitForAggregation);
            }

            return PerLifeAggregationDetailBo;
        }

        public bool GetNextBulkMonthlyData()
        {
            ProcessPerLifeAggregationDetails = new List<ProcessPerLifeAggregationDetail>();

            IList<PerLifeAggregationMonthlyDataBo> bos = PerLifeAggregationMonthlyDataService.GetByPerLifeAggregationDetailId(PerLifeAggregationDetailBo.Id, Skip, Take);
            int count = bos.Count();
            if (count == 0)
                return false;

            Skip += count;
            foreach (var bo in bos)
            {
                ProcessPerLifeAggregationDetails.Add(new ProcessPerLifeAggregationDetail(this, bo));
            }

            return true;
        }

        public void LoadConfigurations()
        {
            RetroTreatyDetailBos = new List<RetroTreatyDetailBo>();
            GstMaintenanceBos = new List<GstMaintenanceBo>();

            int year = 0;
            int? startMonth = null;
            int? endMonth = null;
            if (Util.GetStartEndMonthFromQuarter(PerLifeAggregationDetailBo.RiskQuarter, ref year, ref startMonth, ref endMonth))
            {
                DateTime riskStartDate = new DateTime(year, startMonth.Value, 1);
                DateTime riskEndDate = new DateTime(year, endMonth.Value, 1);

                RetroTreatyDetailBos = RetroTreatyDetailService.GetByParams(riskStartDate, riskEndDate);
                GstMaintenanceBos = GstMaintenanceService.GetByParams(riskStartDate, riskEndDate);
            }
        }

        public void LoadGst()
        {
            GstMaintenanceBos = new List<GstMaintenanceBo>();

            int year = 0;
            int? startMonth = null;
            int? endMonth = null;
            if (Util.GetStartEndMonthFromQuarter(PerLifeAggregationDetailBo.RiskQuarter, ref year, ref startMonth, ref endMonth))
            {
                DateTime riskStartDate = new DateTime(year, startMonth.Value, 1);
                DateTime riskEndDate = new DateTime(year, endMonth.Value, 1);

                RetroTreatyDetailBos = RetroTreatyDetailService.GetByParams(riskStartDate, riskEndDate);
            }
        }

        public IList<RetroTreatyDetailBo> GetRetroTreatyDetails(string reinsBasisCode, string treatyCode, string treatyType, DateTime? reinsEffDatePol, DateTime riskDate)
        {
            var query = RetroTreatyDetailBos.Where(q => q.PerLifeRetroConfigurationTreatyBo.TreatyTypePickListDetailBo.Code == treatyType)
                .Where(q => q.PerLifeRetroConfigurationTreatyBo.TreatyCodeBo.Code == treatyCode)
                .Where(q => q.PerLifeRetroConfigurationTreatyBo.RiskQuarterStartDate <= riskDate
                   && q.PerLifeRetroConfigurationTreatyBo.RiskQuarterEndDate >= riskDate);

            if (reinsEffDatePol.HasValue)
            {
                query = query.Where(q => q.PerLifeRetroConfigurationTreatyBo.ReinsEffectiveStartDate <= reinsEffDatePol
                   && q.PerLifeRetroConfigurationTreatyBo.ReinsEffectiveEndDate >= reinsEffDatePol);
            }

            switch(reinsBasisCode)
            {
                case PickListDetailBo.ReinsBasisCodeAutomatic:
                    query = query.Where(q => q.RetroTreatyBo.IsLobAutomatic);
                    break;
                case PickListDetailBo.ReinsBasisCodeFacultative:
                    query = query.Where(q => q.RetroTreatyBo.IsLobFacultative);
                    break;
                case PickListDetailBo.ReinsBasisCodeAdvantageProgram:
                    query = query.Where(q => q.RetroTreatyBo.IsLobAdvantageProgram);
                    break;
                default:
                    break;
            }

            return query.ToList();
        }

        public double? GetGst(DateTime? reinsEffDatePol, DateTime riskDate)
        {
            var query = GstMaintenanceBos.Where(q => q.RiskEffectiveStartDate <= riskDate && q.RiskEffectiveEndDate >= riskDate);
                

            if (reinsEffDatePol.HasValue)
            {
                query = query.Where(q => q.EffectiveStartDate <= reinsEffDatePol && q.EffectiveEndDate >= reinsEffDatePol);
            }

            return query.FirstOrDefault()?.Rate;
        }

        public bool SetStoredProcedure()
        {
            StoredProcedure = new StoredProcedure(StoredProcedure.PerLifeAggregate);
            return StoredProcedure.IsExists();
        }
    }
}
