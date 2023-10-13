using BusinessObject;
using BusinessObject.Retrocession;
using BusinessObject.SoaDatas;
using Services;
using Services.Retrocession;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class RetrocessionDashboardController : BaseController
    {
        public const string Controller = "RetrocessionDashboard";

        // GET: RetrocessionDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            IndexPage();
            return View();
        }

        public void IndexPage()
        {
            DropDownYear();

            var retroStatusItems = new List<string>();
            for (int i = 0; i <= DirectRetroBo.RetroStatusMax; i++)
            {
                retroStatusItems.Add(DirectRetroBo.GetRetroStatusName(i));
            }
            ViewBag.RetroStatusList = retroStatusItems;

            ViewBag.PerLifeProcessingStatues = null;
            ViewBag.DirectRetroStatues = null;
            ViewBag.MaxTask = RetrocessionDashboardBo.TaskMax;
        }

        [HttpPost]
        public JsonResult RefreshDirectRetroStatues(int year)
        {
            _db.Database.CommandTimeout = 0;

            var bos = _db.SoaDataBatches
                .Where(q => q.Quarter.Contains(year.ToString()))
                .GroupBy(
                    q => new { q.CedantId, q.Quarter },
                    (key, DetailData) => new RetrocessionDashboardBo
                    {
                        CedantId = DetailData.Select(q => q.CedantId).FirstOrDefault(),
                        CedingCompany = DetailData.Select(q => q.Cedant.Code).FirstOrDefault(),
                        SoaQuarter = DetailData.Select(q => q.Quarter).FirstOrDefault(),
                        NoOfCompletedSOA = DetailData.Where(q => q.Status == SoaDataBatchBo.StatusApproved).Count(),
                        NoOfIncompleteRetroStatement = _db.RetroStatements
                        .Where(r => _db.DirectRetro.Where(d => d.CedantId == DetailData.Select(q => q.CedantId).FirstOrDefault()).Where(d => d.SoaQuarter == DetailData.Select(q => q.Quarter).FirstOrDefault()).Select(d => d.Id).Contains(r.DirectRetroId))
                        .Where(r => r.Status != RetroStatementBo.StatusFinalised).Count()
                    }
                )
                .OrderBy(q => q.CedingCompany)
                .ThenBy(q => q.SoaQuarter)
                .ToList();

            return Json(new { DirectRetroStatues = bos });
        }

        [HttpPost]
        public JsonResult GetDirectRetroStatusDetail(int cedantId, string SoaQuarter)
        {
            _db.Database.CommandTimeout = 0;

            var bos = _db.DirectRetro
                .Where(q => q.CedantId == cedantId)
                .Where(q => q.SoaQuarter == SoaQuarter)
                .Select(q => new RetrocessionDashboardBo
                {
                    DirectRetroId = q.Id,
                    TreatyCode = q.TreatyCode.Code,
                    DirectRetroStatus = q.RetroStatus
                });

            return Json(new { Detail = bos });
        }

        [HttpPost]
        public JsonResult RefreshPerLifeProcessingStatues(int year)
        {
            _db.Database.CommandTimeout = 0;

            var taskList = RetrocessionDashboardBo.GetTaskList();
            var retroTreatyBos = RetroTreatyService.GetByStatus();
            var perLifeProcessingStatues = new List<RetrocessionDashboardBo> { };

            foreach (var retroTreaty in retroTreatyBos)
            {
                var query = _db.PerLifeRetroConfigurationTreaties.Where(q => _db.RetroTreatyDetails.Where(s => s.RetroTreatyId == retroTreaty.Id).Select(s => s.PerLifeRetroConfigurationTreatyId).Contains(q.Id));

                var treatyQuery = _db.PerLifeAggregationDetailTreaties.Where(q => query.Select(s => s.TreatyCode.Code).Contains(q.TreatyCode));

                var perLifeAggregations = _db.PerLifeAggregations.Where(q => treatyQuery.Select(s => s.PerLifeAggregationDetail.PerLifeAggregationId).Contains(q.Id)).Where(q => q.SoaQuarter.Contains(year.ToString())).ToList();

                var retroTreatyQuery = _db.PerLifeClaimData.Where(q => _db.PerLifeClaimRetroData.Where(s => s.RetroTreatyId == retroTreaty.Id).Select(ss => ss.PerLifeClaimDataId).Contains(q.Id));

                var perLifeClaims = _db.PerLifeClaims.Where(q => retroTreatyQuery.Select(s => s.PerLifeClaimId).Contains(q.Id)).Where(q => q.SoaQuarter.Contains(year.ToString())).ToList();

                var perLifeSoa = _db.PerLifeSoa.Where(q => q.RetroTreatyId == retroTreaty.Id);

                var q1PerLifeAggregation = perLifeAggregations.Where(q => q.SoaQuarter == year.ToString() + " Q1").FirstOrDefault();
                var q2PerLifeAggregation = perLifeAggregations.Where(q => q.SoaQuarter == year.ToString() + " Q2").FirstOrDefault();
                var q3PerLifeAggregation = perLifeAggregations.Where(q => q.SoaQuarter == year.ToString() + " Q3").FirstOrDefault();
                var q4PerLifeAggregation = perLifeAggregations.Where(q => q.SoaQuarter == year.ToString() + " Q4").FirstOrDefault();

                var q1PerLifeClaim = perLifeClaims.Where(q => q.SoaQuarter == year.ToString() + " Q1").FirstOrDefault();
                var q2PerLifeClaim = perLifeClaims.Where(q => q.SoaQuarter == year.ToString() + " Q2").FirstOrDefault();
                var q3PerLifeClaim = perLifeClaims.Where(q => q.SoaQuarter == year.ToString() + " Q3").FirstOrDefault();
                var q4PerLifeClaim = perLifeClaims.Where(q => q.SoaQuarter == year.ToString() + " Q4").FirstOrDefault();

                var q1PerLifeSoa = perLifeSoa.Where(q => q.SoaQuarter == year.ToString() + " Q1").FirstOrDefault();
                var q2PerLifeSoa = perLifeSoa.Where(q => q.SoaQuarter == year.ToString() + " Q2").FirstOrDefault();
                var q3PerLifeSoa = perLifeSoa.Where(q => q.SoaQuarter == year.ToString() + " Q3").FirstOrDefault();
                var q4PerLifeSoa = perLifeSoa.Where(q => q.SoaQuarter == year.ToString() + " Q4").FirstOrDefault();

                var q1RetroRegisterBatch = q1PerLifeSoa != null ? _db.RetroRegisterBatches
                    .Where(q => q.Type == RetroRegisterBatchBo.TypePerLifeRetro)
                    .Where(q => _db.RetroRegisters.Where(s => s.RetroPartyId == q1PerLifeSoa.RetroPartyId).Where(s => s.OriginalSoaQuarter == q1PerLifeSoa.SoaQuarter).Select(s => s.RetroRegisterBatchId).Contains(q.Id))
                    .FirstOrDefault() : null;

                var q2RetroRegisterBatch = q2PerLifeSoa != null ? _db.RetroRegisterBatches
                    .Where(q => q.Type == RetroRegisterBatchBo.TypePerLifeRetro)
                    .Where(q => _db.RetroRegisters.Where(s => s.RetroPartyId == q2PerLifeSoa.RetroPartyId).Where(s => s.OriginalSoaQuarter == q2PerLifeSoa.SoaQuarter).Select(s => s.RetroRegisterBatchId).Contains(q.Id))
                    .FirstOrDefault() : null;

                var q3RetroRegisterBatch = q3PerLifeSoa != null ? _db.RetroRegisterBatches
                    .Where(q => q.Type == RetroRegisterBatchBo.TypePerLifeRetro)
                    .Where(q => _db.RetroRegisters.Where(s => s.RetroPartyId == q3PerLifeSoa.RetroPartyId).Where(s => s.OriginalSoaQuarter == q3PerLifeSoa.SoaQuarter).Select(s => s.RetroRegisterBatchId).Contains(q.Id))
                    .FirstOrDefault() : null;

                var q4RetroRegisterBatch = q4PerLifeSoa != null ? _db.RetroRegisterBatches
                    .Where(q => q.Type == RetroRegisterBatchBo.TypePerLifeRetro)
                    .Where(q => _db.RetroRegisters.Where(s => s.RetroPartyId == q4PerLifeSoa.RetroPartyId).Where(s => s.OriginalSoaQuarter == q4PerLifeSoa.SoaQuarter).Select(s => s.RetroRegisterBatchId).Contains(q.Id))
                    .FirstOrDefault() : null;

                foreach (var task in taskList)
                {
                    var bo = new RetrocessionDashboardBo(retroTreaty.Code, task);

                    switch (task)
                    {
                        case RetrocessionDashboardBo.TaskRiDataWarehouseSnapshot:
                            bo.Q1 = q1PerLifeAggregation?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q2 = q2PerLifeAggregation?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q3 = q3PerLifeAggregation?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q4 = q4PerLifeAggregation?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            break;
                        case RetrocessionDashboardBo.TaskClaimRegisterSnapshot:
                            bo.Q1 = q1PerLifeClaim?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q2 = q2PerLifeClaim?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q3 = q3PerLifeClaim?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            bo.Q4 = q4PerLifeClaim?.CutOff.CutOffDateTime?.ToString(Util.GetDateFormat()) ?? "-";
                            break;
                        case RetrocessionDashboardBo.TaskPremiumAggregation:
                            bo.Q1 = q1PerLifeAggregation?.Status == null ? "-" : PerLifeAggregationBo.GetStatusName(q1PerLifeAggregation.Status);
                            bo.Q2 = q2PerLifeAggregation?.Status == null ? "-" : PerLifeAggregationBo.GetStatusName(q2PerLifeAggregation.Status);
                            bo.Q3 = q3PerLifeAggregation?.Status == null ? "-" : PerLifeAggregationBo.GetStatusName(q3PerLifeAggregation.Status);
                            bo.Q4 = q4PerLifeAggregation?.Status == null ? "-" : PerLifeAggregationBo.GetStatusName(q4PerLifeAggregation.Status);
                            break;
                        case RetrocessionDashboardBo.TaskClaimProcessing:
                            bo.Q1 = q1PerLifeClaim?.Status == null ? "-" : PerLifeClaimBo.GetStatusName(q1PerLifeClaim.Status);
                            bo.Q2 = q2PerLifeClaim?.Status == null ? "-" : PerLifeClaimBo.GetStatusName(q2PerLifeClaim.Status);
                            bo.Q3 = q3PerLifeClaim?.Status == null ? "-" : PerLifeClaimBo.GetStatusName(q3PerLifeClaim.Status);
                            bo.Q4 = q4PerLifeClaim?.Status == null ? "-" : PerLifeClaimBo.GetStatusName(q4PerLifeClaim.Status);
                            break;
                        case RetrocessionDashboardBo.TaskSoa:
                            bo.Q1 = q1PerLifeSoa?.Status == null ? "-" : PerLifeSoaBo.GetStatusName(q1PerLifeSoa.Status);
                            bo.Q2 = q2PerLifeSoa?.Status == null ? "-" : PerLifeSoaBo.GetStatusName(q2PerLifeSoa.Status);
                            bo.Q3 = q3PerLifeSoa?.Status == null ? "-" : PerLifeSoaBo.GetStatusName(q3PerLifeSoa.Status);
                            bo.Q4 = q4PerLifeSoa?.Status == null ? "-" : PerLifeSoaBo.GetStatusName(q4PerLifeSoa.Status);
                            break;
                        case RetrocessionDashboardBo.TaskRetroRegister:
                            bo.Q1 = q1RetroRegisterBatch?.Status == null ? "-" : RetroRegisterBatchBo.GetStatusName(q1RetroRegisterBatch.Status);
                            bo.Q2 = q2RetroRegisterBatch?.Status == null ? "-" : RetroRegisterBatchBo.GetStatusName(q2RetroRegisterBatch.Status);
                            bo.Q3 = q3RetroRegisterBatch?.Status == null ? "-" : RetroRegisterBatchBo.GetStatusName(q3RetroRegisterBatch.Status);
                            bo.Q4 = q4RetroRegisterBatch?.Status == null ? "-" : RetroRegisterBatchBo.GetStatusName(q4RetroRegisterBatch.Status);
                            break;
                        default:
                            break;
                    }
                    perLifeProcessingStatues.Add(bo);
                }
            }

            return Json(new { PerLifeProcessingStatues = perLifeProcessingStatues });
        }
    }
}