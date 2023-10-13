using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
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
    public class RiDataCorrectionController : BaseController
    {
        public const string Controller = "RiDataCorrection";

        // GET: RiDataCorrection
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CedantId,
            string TreatyCodeId,
            string PolicyNumber,
            string InsuredRegisterNo,
            string CampaignCode,
            int? ReinsBasisCodeId,
            int? InsuredGenderCodeId,
            string InsuredDateOfBirth,
            string InsuredName,
            string ApLoading,
            string SortOrder,
            int? Page
        )
        {
            DateTime? insuredDOB = Util.GetParseDateTime(InsuredDateOfBirth);
            double? apLoading = Util.StringToDouble(ApLoading);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["TreatyCodeId"] = TreatyCodeId,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredRegisterNo"] = InsuredRegisterNo,
                ["CampaignCode"] = CampaignCode,
                ["ReinsBasisCodeId"] = ReinsBasisCodeId,
                ["InsuredGenderCodeId"] = InsuredGenderCodeId,
                ["InsuredDateOfBirth"] = insuredDOB.HasValue ? InsuredDateOfBirth : null,
                ["InsuredName"] = InsuredName,
                ["ApLoading"] = apLoading.HasValue ? apLoading : null,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortInsuredRegisterNo = GetSortParam("InsuredRegisterNo");
            ViewBag.SortCampaignCode = GetSortParam("CampaignCode");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortApLoading = GetSortParam("ApLoading");

            var query = _db.RiDataCorrections.Select(RiDataCorrectionViewModel.Expression());
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId.Value);
            if (!string.IsNullOrEmpty(TreatyCodeId)) {
                string[] TreatyCodeIds = TreatyCodeId.Split(',');
                query = query.Where(q => TreatyCodeIds.Contains(q.TreatyCodeId.ToString())); 
            }
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(InsuredRegisterNo)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredRegisterNo) && q.InsuredRegisterNo.Contains(InsuredRegisterNo));
            if (!string.IsNullOrEmpty(CampaignCode)) query = query.Where(q => !string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Contains(CampaignCode));
            if (ReinsBasisCodeId != null) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodeId);
            if (InsuredGenderCodeId != null) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodeId);
            if (insuredDOB.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDOB);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
            if (apLoading.HasValue) query = query.Where(q => q.ApLoading == apLoading);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("InsuredRegisterNo")) query = query.OrderBy(q => q.InsuredRegisterNo);
            else if (SortOrder == Html.GetSortDsc("InsuredRegisterNo")) query = query.OrderByDescending(q => q.InsuredRegisterNo);
            else if (SortOrder == Html.GetSortAsc("CampaignCode")) query = query.OrderBy(q => q.CampaignCode);
            else if (SortOrder == Html.GetSortDsc("CampaignCode")) query = query.OrderByDescending(q => q.CampaignCode);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("ApLoading")) query = query.OrderBy(q => q.ApLoading);
            else if (SortOrder == Html.GetSortDsc("ApLoading")) query = query.OrderByDescending(q => q.ApLoading);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: RiDataCorrection/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new RiDataCorrectionViewModel());
        }

        // POST: RiDataCorrection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RiDataCorrectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result childResult = new Result();
                model.GetMappedValues(model, ref childResult);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage();
                    return View(model);
                }

                if (model.InsuredDateOfBirthStr != null)
                {
                    model.InsuredDateOfBirth = DateTime.Parse(model.InsuredDateOfBirthStr);
                }

                var riDataCorrectionBo = new RiDataCorrectionBo
                {
                    CedantId = model.CedantId,
                    TreatyCodeId = model.TreatyCodeId,
                    PolicyNumber = model.PolicyNumber?.Trim(),
                    InsuredRegisterNo = model.InsuredRegisterNo?.Trim(),
                    InsuredGenderCodePickListDetailId = model.InsuredGenderCodePickListDetailId,
                    InsuredDateOfBirth = model.InsuredDateOfBirth,
                    InsuredName = model.InsuredName?.Trim(),
                    CampaignCode = model.CampaignCode?.Trim(),
                    ReinsBasisCodePickListDetailId = model.ReinsBasisCodePickListDetailId,
                    ApLoading = Util.StringToDouble(model.ApLoadingStr),
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = RiDataCorrectionService.Create(ref riDataCorrectionBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        riDataCorrectionBo.Id,
                        "Create RI Data Correction"
                    );

                    model.Id = riDataCorrectionBo.Id;

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = riDataCorrectionBo.Id });
                }
                AddResult(Result);
            }
            LoadPage();
            return View(model);
        }

        // GET: RiDataCorrection/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();
            RiDataCorrectionBo riDataCorrectionBo = RiDataCorrectionService.Find(id);
            if (riDataCorrectionBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(riDataCorrectionBo);
            return View(new RiDataCorrectionViewModel(riDataCorrectionBo));
        }

        // POST: RiDataCorrection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RiDataCorrectionViewModel model)
        {
            RiDataCorrectionBo riDataCorrectionBo = RiDataCorrectionService.Find(id);
            if (riDataCorrectionBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                Result childResult = new Result();
                model.GetMappedValues(model, ref childResult);
                if (!childResult.Valid)
                {
                    AddResult(childResult);
                    LoadPage();
                    return View(model);
                }

                if (model.InsuredDateOfBirthStr != null)
                {
                    model.InsuredDateOfBirth = DateTime.Parse(model.InsuredDateOfBirthStr);
                }

                riDataCorrectionBo.CedantId = model.CedantId;
                riDataCorrectionBo.TreatyCodeId = model.TreatyCodeId;
                riDataCorrectionBo.PolicyNumber = model.PolicyNumber?.Trim();
                riDataCorrectionBo.InsuredRegisterNo = model.InsuredRegisterNo?.Trim();
                riDataCorrectionBo.InsuredGenderCodePickListDetailId = model.InsuredGenderCodePickListDetailId;
                riDataCorrectionBo.InsuredDateOfBirth = model.InsuredDateOfBirth;
                riDataCorrectionBo.InsuredName = model.InsuredName?.Trim();
                riDataCorrectionBo.CampaignCode = model.CampaignCode?.Trim();
                riDataCorrectionBo.ReinsBasisCodePickListDetailId = model.ReinsBasisCodePickListDetailId;
                riDataCorrectionBo.ApLoading = Util.StringToDouble(model.ApLoadingStr);
                riDataCorrectionBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = RiDataCorrectionService.Update(ref riDataCorrectionBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        riDataCorrectionBo.Id,
                        "Update RI Data Correction"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = riDataCorrectionBo.Id });
                }
                AddResult(Result);
            }
            LoadPage(riDataCorrectionBo);
            SetViewBagMessage();
            return View(model);
        }

        // GET: RiDataCorrection/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            RiDataCorrectionBo riDataCorrectionBo = RiDataCorrectionService.Find(id);
            if (riDataCorrectionBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new RiDataCorrectionViewModel(riDataCorrectionBo));
        }

        // POST: RiDataCorrection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            RiDataCorrectionBo riDataCorrectionBo = RiDataCorrectionService.Find(id);
            if (riDataCorrectionBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = RiDataCorrectionService.Delete(riDataCorrectionBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    riDataCorrectionBo.Id,
                    "Delete RI Data Correction"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = riDataCorrectionBo.Id });
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
                    var process = new ProcessRiDataCorrection()
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

                    if (create != 0 || update != 0 || delete != 0)
                    {
                        SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? CedantId,
            string TreatyCodeId,
            string PolicyNumber,
            string InsuredRegisterNo,
            string CampaignCode,
            int? ReinsBasisCodeId,
            int? InsuredGenderCodeId,
            string InsuredDateOfBirth,
            string InsuredName,
            string ApLoading
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RiDataCorrections.Select(RiDataCorrectionService.Expression());
            if (type == 2) // filtered dowload
            {
                var insuredDOB = Util.GetParseDateTime(InsuredDateOfBirth);
                var apLoading = Util.StringToDouble(ApLoading);

                if (CedantId != null) query = query.Where(q => q.CedantId == CedantId.Value);
                if (!string.IsNullOrEmpty(TreatyCodeId))
                {
                    string[] TreatyCodeIds = TreatyCodeId.Split(',');
                    query = query.Where(q => TreatyCodeIds.Contains(q.TreatyCodeId.ToString()));
                }
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (!string.IsNullOrEmpty(InsuredRegisterNo)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredRegisterNo) && q.InsuredRegisterNo.Contains(InsuredRegisterNo));
                if (!string.IsNullOrEmpty(CampaignCode)) query = query.Where(q => !string.IsNullOrEmpty(q.CampaignCode) && q.CampaignCode.Contains(CampaignCode));
                if (ReinsBasisCodeId != null) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodeId);
                if (InsuredGenderCodeId != null) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodeId);
                if (insuredDOB.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDOB);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (apLoading.HasValue) query = query.Where(q => q.ApLoading == apLoading);
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportRiDataCorrection();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            //DropDownTreatyCode();
            DropDownReinsBasisCode();
            DropDownInsuredGenderCode();
            SetViewBagMessage();
        }

        public void LoadPage(RiDataCorrectionBo riDataCorrectionBo = null)
        {
            DropDownEmpty();
            DropDownReinsBasisCode();
            DropDownInsuredGenderCode();

            if (riDataCorrectionBo == null)
            {
                // Create
                DropDownCedant(CedantBo.StatusActive);
                DropDownTreatyCode(TreatyCodeBo.StatusActive, foreign: false);
            }
            else
            {
                // Edit
                DropDownCedant(CedantBo.StatusActive, riDataCorrectionBo.CedantId);
                DropDownTreatyCode(TreatyCodeBo.StatusActive, riDataCorrectionBo.TreatyCodeId, foreign: false);

                if (riDataCorrectionBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
                if (riDataCorrectionBo.TreatyCodeId != null)
                {
                    if (riDataCorrectionBo.TreatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                    {
                        AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                    }
                }
            }
            SetViewBagMessage();
        }
    }
}