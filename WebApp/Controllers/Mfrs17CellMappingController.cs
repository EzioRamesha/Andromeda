using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class Mfrs17CellMappingController : BaseController
    {
        public const string Controller = "Mfrs17CellMapping";

        // GET: Mfrs17CellMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string TreatyCode,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            int? ReinsBasisCodePickListDetailId,
            string CedingPlanCode,
            string BenefitCode,
            int? ProfitCommPickListDetailId,
            string RateTable,
            int? BasicRiderPickListDetailId,
            string CellName,
            //string Mfrs17TreatyCode,
            int? Mfrs17ContractCodeDetailId,
            string LoaCode,
            string SortOrder,
            int? Page
        )
        {
            DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDate);
            DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCode"] = TreatyCode,
                ["ReinsEffDatePolStartDate"] = start.HasValue ? ReinsEffDatePolStartDate : null,
                ["ReinsEffDatePolEndDate"] = end.HasValue ? ReinsEffDatePolEndDate : null,
                ["ReinsBasisCodePickListDetailId"] = ReinsBasisCodePickListDetailId,
                ["CedingPlanCode"] = CedingPlanCode,
                ["BenefitCode"] = BenefitCode,
                ["ProfitCommPickListDetailId"] = ProfitCommPickListDetailId,
                ["RateTable"] = RateTable,
                ["BasicRiderPickListDetailId"] = BasicRiderPickListDetailId,
                ["CellName"] = CellName,
                ["Mfrs17ContractCodeDetailId"] = Mfrs17ContractCodeDetailId,
                ["LoaCode"] = LoaCode,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortProfitCommPickListDetailId = GetSortParam("ProfitCommPickListDetailId");
            ViewBag.SortRateTable = GetSortParam("RateTable");
            ViewBag.SortCellName = GetSortParam("CellName");
            ViewBag.SortMfrs17ContractCodeDetailId = GetSortParam("Mfrs17ContractCodeDetailId");
            ViewBag.SortLoaCode = GetSortParam("LoaCode");

            var query = _db.Mfrs17CellMappings.Select(Mfrs17CellMappingViewModel.Expression());
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode.Contains(TreatyCode));

            if (start.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= start);
            if (end.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= end);

            if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.Mfrs17CellMappingDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
            if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => q.Mfrs17CellMappingDetails.Any(d => d.BenefitCode == BenefitCode));
            if (ProfitCommPickListDetailId.HasValue) query = query.Where(q => q.ProfitCommPickListDetailId == ProfitCommPickListDetailId);
            if (!string.IsNullOrEmpty(RateTable)) query = query.Where(q => q.RateTable == RateTable);
            if (BasicRiderPickListDetailId.HasValue) query = query.Where(q => q.BasicRiderPickListDetailId == BasicRiderPickListDetailId);
            if (!string.IsNullOrEmpty(CellName)) query = query.Where(q => q.CellName.Contains(CellName));
            if (Mfrs17ContractCodeDetailId.HasValue) query = query.Where(q => q.Mfrs17ContractCodeDetailId == Mfrs17ContractCodeDetailId);
            if (!string.IsNullOrEmpty(LoaCode)) query = query.Where(q => q.LoaCode.Contains(LoaCode));

            if (SortOrder == Html.GetSortAsc("ProfitCommPickListDetailId")) query = query.OrderBy(q => q.ProfitCommPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ProfitCommPickListDetailId")) query = query.OrderByDescending(q => q.ProfitCommPickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("RateTable")) query = query.OrderBy(q => q.RateTable);
            else if (SortOrder == Html.GetSortDsc("RateTable")) query = query.OrderByDescending(q => q.RateTable);
            else if (SortOrder == Html.GetSortAsc("CellName")) query = query.OrderBy(q => q.CellName);
            else if (SortOrder == Html.GetSortDsc("CellName")) query = query.OrderByDescending(q => q.CellName);
            else if (SortOrder == Html.GetSortAsc("Mfrs17ContractCodeDetailId")) query = query.OrderBy(q => q.Mfrs17ContractCodeDetail.ContractCode);
            else if (SortOrder == Html.GetSortDsc("Mfrs17ContractCodeDetailId")) query = query.OrderByDescending(q => q.Mfrs17ContractCodeDetail.ContractCode);
            else if (SortOrder == Html.GetSortAsc("LoaCode")) query = query.OrderBy(q => q.LoaCode);
            else if (SortOrder == Html.GetSortDsc("LoaCode")) query = query.OrderByDescending(q => q.LoaCode);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: Mfrs17CellMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new Mfrs17CellMappingViewModel());
        }

        // POST: Mfrs17CellMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(Mfrs17CellMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = RateTableService.Result();
                var mappingResult = Mfrs17CellMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = Mfrs17CellMappingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        Mfrs17CellMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create MFRS17 Cell Mapping"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: Mfrs17CellMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = Mfrs17CellMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(bo);
            return View(new Mfrs17CellMappingViewModel(bo));
        }

        // POST: Mfrs17CellMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, Mfrs17CellMappingViewModel model)
        {
            var dbBo = Mfrs17CellMappingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                var trail = GetNewTrailObject();
                Result = RateTableService.Result();
                var mappingResult = Mfrs17CellMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = Mfrs17CellMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        Mfrs17CellMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            dbBo.Id,
                            "Update MFRS17 Cell Mapping"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                AddResult(Result);
            }
            LoadPage(dbBo);
            return View(model);
        }

        // GET: Mfrs17CellMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = Mfrs17CellMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new Mfrs17CellMappingViewModel(bo));
        }

        // POST: Mfrs17CellMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, Mfrs17CellMappingViewModel model)
        {
            var bo = Mfrs17CellMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = Mfrs17CellMappingService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete MFRS17 Cell Mapping"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = AccessMatrixBo.PowerUpload, ReturnController = Controller)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessMfrs17CellMapping()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int create = process.GetProcessCount("Create");
                    int update = process.GetProcessCount("Update");
                    int delete = process.GetProcessCount("Delete");

                    SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            string TreatyCode,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            int? ReinsBasisCodePickListDetailId,
            string CedingPlanCode,
            string BenefitCode,
            int? ProfitCommPickListDetailId,
            string RateTable,
            int? BasicRiderPickListDetailId,
            string CellName,
            //string Mfrs17TreatyCode,
            int? Mfrs17ContractCodeDetailId,
            string LoaCode
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.Mfrs17CellMappings.Select(Mfrs17CellMappingViewModel.Expression());
            if (type == 2) // filtered dowload
            {
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => q.TreatyCode.Contains(TreatyCode));

                DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDate);
                DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDate);
                if (start.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= start);
                if (end.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= end);

                if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.Mfrs17CellMappingDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
                if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => q.Mfrs17CellMappingDetails.Any(d => d.BenefitCode == BenefitCode));
                if (ProfitCommPickListDetailId.HasValue) query = query.Where(q => q.ProfitCommPickListDetailId == ProfitCommPickListDetailId);
                if (!string.IsNullOrEmpty(RateTable)) query = query.Where(q => q.RateTable == RateTable);
                if (BasicRiderPickListDetailId.HasValue) query = query.Where(q => q.BasicRiderPickListDetailId == BasicRiderPickListDetailId);
                if (!string.IsNullOrEmpty(CellName)) query = query.Where(q => q.CellName.Contains(CellName));
                if (Mfrs17ContractCodeDetailId.HasValue) query = query.Where(q => q.Mfrs17ContractCodeDetailId == Mfrs17ContractCodeDetailId);
                if (!string.IsNullOrEmpty(LoaCode)) query = query.Where(q => q.LoaCode.Contains(LoaCode));
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportMfrs17CellMapping();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new Mfrs17CellMappingBo
                {
                    Id = x.Id,
                    TreatyCode = x.TreatyCode,
                    ReinsEffDatePolStartDate = x.ReinsEffDatePolStartDate,
                    ReinsEffDatePolEndDate = x.ReinsEffDatePolEndDate,
                    ReinsBasisCodePickListDetailId = x.ReinsBasisCodePickListDetailId,
                    ReinsBasisCode = x.ReinsBasisCodePickListDetail.Code,
                    CedingPlanCode = x.CedingPlanCode,
                    BenefitCode = x.BenefitCode,
                    ProfitCommPickListDetailId = x.ProfitCommPickListDetailId,
                    ProfitComm = x.ProfitCommPickListDetail.Code,
                    RateTable = x.RateTable,
                    BasicRiderPickListDetailId = x.BasicRiderPickListDetailId,
                    BasicRider = x.BasicRiderPickListDetail.Code,
                    CellName = x.CellName,
                    Mfrs17ContractCode = x.Mfrs17ContractCodeDetail.ContractCode,
                    LoaCode = x.LoaCode,
                }).AsQueryable());

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownReinsBasisCode();
            DropDownMfrs17BasicRider();
            //DropDownYN();
            DropDownProfitComm();
            DropDownMfrs17ContractCodeDetail();
            SetViewBagMessage();
        }

        public void LoadPage(Mfrs17CellMappingBo mfrs17CellMappingBo = null)
        {
            DropDownReinsBasisCode();
            DropDownMfrs17BasicRider();
            GetTreatyCodes(foreign: false);
            GetBenefitCodes();
            //DropDownYN();
            DropDownProfitComm();
            DropDownMfrs17ContractCodeDetail();

            var entity = new Mfrs17CellMapping();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CellName");
            ViewBag.CellNameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 50;

            //maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Mfrs17TreatyCode");
            //ViewBag.Mfrs17TreatyCodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 25;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("LoaCode");
            ViewBag.LoaCodeMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 20;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("RateTable");
            ViewBag.RateTableMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            if (mfrs17CellMappingBo == null)
            {
                // Create
            }
            else
            {
                // Edit
                if (!string.IsNullOrEmpty(mfrs17CellMappingBo.BenefitCode))
                {
                    string[] benefitCodes = mfrs17CellMappingBo.BenefitCode.ToArraySplitTrim();
                    foreach (string benefitCodeStr in benefitCodes)
                    {
                        var benefitCode = BenefitService.FindByCode(benefitCodeStr, false);
                        if (benefitCode != null)
                        {
                            if (benefitCode.Status == BenefitBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.BenefitStatusInactiveWithCode, benefitCodeStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.BenefitCodeNotFound, benefitCodeStr));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(mfrs17CellMappingBo.TreatyCode))
                {
                    string[] treatyCodes = mfrs17CellMappingBo.TreatyCode.ToArraySplitTrim();
                    foreach (string treatyCodeStr in treatyCodes)
                    {
                        var treatyCode = TreatyCodeService.FindByCode(treatyCodeStr, false);
                        if (treatyCode != null)
                        {
                            if (treatyCode.Status == TreatyCodeBo.StatusInactive)
                            {
                                AddErrorMsg(string.Format(MessageBag.TreatyCodeStatusInactiveWithCode, treatyCodeStr));
                            }
                        }
                        else
                        {
                            AddErrorMsg(string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr));
                        }
                    }
                }
            }
            SetViewBagMessage();
        }
    }
}
