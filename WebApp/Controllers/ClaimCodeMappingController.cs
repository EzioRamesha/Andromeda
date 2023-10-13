using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessObject;
using ConsoleApp.Commands.ProcessDatas;
using DataAccess.EntityFramework;
using PagedList;
using Services;
using Shared;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimCodeMappingController : BaseController
    {
        public const string Controller = "ClaimCodeMapping";

        // GET: ClaimCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string MlreEventCode,
            string MlreBenefitCode,
            int? ClaimCodeId,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["MlreEventCode"] = MlreEventCode,
                ["MlreBenefitCode"] = MlreBenefitCode,
                ["ClaimCodeId"] = ClaimCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortClaimCodeId = GetSortParam("ClaimCodeId");

            var query = _db.ClaimCodeMappings.Select(ClaimCodeMappingViewModel.Expression());

            if (!string.IsNullOrEmpty(MlreEventCode)) query = query.Where(q => q.ClaimCodeMappingDetails.Any(d => d.MlreEventCode == MlreEventCode));
            if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.ClaimCodeMappingDetails.Any(d => d.MlreBenefitCode == MlreBenefitCode));
            if (ClaimCodeId.HasValue) query = query.Where(q => q.ClaimCodeId == ClaimCodeId);

            if (SortOrder == Html.GetSortAsc("ClaimCodeId")) query = query.OrderBy(q => q.ClaimCode.Code);
            else if (SortOrder == Html.GetSortDsc("ClaimCodeId")) query = query.OrderByDescending(q => q.ClaimCode.Code);
            query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ClaimCodeMappingViewModel());
        }

        // POST: ClaimCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(ClaimCodeMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = ClaimCodeMappingService.Result();
                var mappingResult = ClaimCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = ClaimCodeMappingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        ClaimCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create Claim Code Mapping"
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

        // GET: ClaimCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ClaimCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(bo);
            return View(new ClaimCodeMappingViewModel(bo));
        }

        // POST: ClaimCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, ClaimCodeMappingViewModel model)
        {
            var dbBo = ClaimCodeMappingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = ClaimCodeMappingService.Result();
                var mappingResult = ClaimCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = ClaimCodeMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        ClaimCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Update Claim Code Mapping"
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

        // GET: ClaimCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ClaimCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new ClaimCodeMappingViewModel(bo));
        }

        // POST: ClaimCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ClaimCodeMappingViewModel model)
        {
            var bo = ClaimCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ClaimCodeMappingService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Claim Code Mapping"
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
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessClaimCodeMapping()
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
            int type,
            string MlreEventCode,
            string MlreBenefitCode,
            int? ClaimCodeId
        )
        {
            var process = new ProcessClaimCodeMapping();
            IEnumerable<ClaimCodeMappingBo> template = null;

            if (type != 3)
            {
                var query = _db.ClaimCodeMappings.Select(ClaimCodeMappingViewModel.Expression());
                if (type == 2) // filtered dowload
                {

                    if (!string.IsNullOrEmpty(MlreEventCode)) query = query.Where(q => q.ClaimCodeMappingDetails.Any(d => d.MlreEventCode == MlreEventCode));
                    if (!string.IsNullOrEmpty(MlreBenefitCode)) query = query.Where(q => q.ClaimCodeMappingDetails.Any(d => d.MlreBenefitCode == MlreBenefitCode));
                    if (ClaimCodeId.HasValue) query = query.Where(q => q.ClaimCodeId == ClaimCodeId);
                }
                process.ExportToCsv(query.Select(x => new ClaimCodeMappingBo
                {
                    Id = x.Id,
                    MlreEventCode = x.MlreEventCode,
                    MlreBenefitCode = x.MlreBenefitCode,
                    ClaimCodeId = x.ClaimCodeId,
                    ClaimCode = x.ClaimCode.Code,
                }).AsEnumerable<ClaimCodeMappingBo>());
            }
            else
            {
                process.ExportToCsv(template);
            }

            return File(process.FilePath, "text/csv", Path.GetFileName(process.FilePath));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownClaimCode();
            SetViewBagMessage();
        }

        public void LoadPage(ClaimCodeMappingBo claimCodeMappingBo = null)
        {
            GetBenefitCodes();
            GetMlreEventCode();

            if (claimCodeMappingBo == null)
            {
                // Create
                DropDownClaimCode(ClaimCodeBo.StatusActive);
            }
            else
            {
                DropDownClaimCode(ClaimCodeBo.StatusActive, claimCodeMappingBo.ClaimCodeId);

                if (claimCodeMappingBo.ClaimCodeBo != null && claimCodeMappingBo.ClaimCodeBo.Status == ClaimCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.ClaimCodeStatusInactive);
                }

                string[] benefitCodes = claimCodeMappingBo.MlreBenefitCode.ToArraySplitTrim();
                foreach (string benefitCodeStr in benefitCodes)
                {
                    var benefitCode = BenefitService.FindByCode(benefitCodeStr);
                    if (benefitCode != null)
                    {
                        if (benefitCode.Status == BenefitBo.StatusInactive)
                        {
                            AddErrorMsg(string.Format(MessageBag.BenefitStatusInactiveWithCode, benefitCode));
                        }
                    }
                    else
                    {
                        AddErrorMsg(string.Format(MessageBag.BenefitCodeNotFound, benefitCode));
                    }
                }

                string[] eventCodes = claimCodeMappingBo.MlreEventCode.ToArraySplitTrim();
                foreach (string eventCodeStr in eventCodes)
                {
                    var eventCode = EventCodeService.FindByCode(eventCodeStr);
                    if (eventCode != null)
                    {
                        if (eventCode.Status == EventCodeBo.StatusInactive)
                        {
                            AddErrorMsg(string.Format(MessageBag.EventCodeStatusInactiveWithCode, eventCode));
                        }
                    }
                    else
                    {
                        AddErrorMsg(string.Format(MessageBag.EventCodeNotFound, eventCode));
                    }
                }

            }
            SetViewBagMessage();
        }
    }
}
