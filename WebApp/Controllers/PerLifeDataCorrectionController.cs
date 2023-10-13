using BusinessObject;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using System;
using System.ComponentModel.DataAnnotations;
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
    public class PerLifeDataCorrectionController : BaseController
    {
        public const string Controller = "PerLifeDataCorrection";

        // GET: PerLifeDataCorrection
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? TreatyCodeId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            int? InsuredGenderCodePickListDetailId,
            int? TerritoryOfIssueCodePickListDetailId,
            int? PerLifeRetroGenderId,
            int? PerLifeRetroCountryId,
            string DateOfExceptionDetected,
            string DateOfPolicyExist,
            bool? IsProceedToAggregate,
            string DateUpdated,
            int? ExceptionStatusPickListDetailId,
            string SortOrder,
            int? Page)
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? dateOfExceptionDetected = Util.GetParseDateTime(DateOfExceptionDetected);
            DateTime? dateOfPolicyExist = Util.GetParseDateTime(DateOfPolicyExist);
            DateTime? dateUpdated = Util.GetParseDateTime(DateUpdated);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCodeId"] = TreatyCodeId,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["PolicyNumber"] = PolicyNumber,
                ["InsuredGenderCodePickListDetailId"] = InsuredGenderCodePickListDetailId,
                ["TerritoryOfIssueCodePickListDetailId"] = TerritoryOfIssueCodePickListDetailId,
                ["PerLifeRetroGenderId"] = PerLifeRetroGenderId,
                ["PerLifeRetroCountryId"] = PerLifeRetroCountryId,
                ["DateOfExceptionDetected"] = dateOfExceptionDetected.HasValue ? DateOfExceptionDetected : null,
                ["DateOfPolicyExist"] = dateOfPolicyExist.HasValue ? DateOfPolicyExist : null,
                ["IsProceedToAggregate"] = IsProceedToAggregate,
                ["DateUpdated"] = dateUpdated.HasValue ? DateUpdated : null,
                ["ExceptionStatusPickListDetailId"] = ExceptionStatusPickListDetailId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortInsuredGenderCodePickListDetailId = GetSortParam("InsuredGenderCodePickListDetailId");
            ViewBag.SortTerritoryOfIssueCodePickListDetailId = GetSortParam("TerritoryOfIssueCodePickListDetailId");
            ViewBag.SortPerLifeRetroGenderId = GetSortParam("PerLifeRetroGenderId");
            ViewBag.SortPerLifeRetroCountryId = GetSortParam("PerLifeRetroCountryId");
            ViewBag.SortDateOfExceptionDetected = GetSortParam("DateOfExceptionDetected");
            ViewBag.SortDateOfPolicyExist = GetSortParam("DateOfPolicyExist");
            ViewBag.SortIsProceedToAggregate = GetSortParam("IsProceedToAggregate");
            ViewBag.SortDateUpdated = GetSortParam("DateUpdated");
            ViewBag.SortExceptionStatusPickListDetailId = GetSortParam("ExceptionStatusPickListDetailId");

            var query = _db.PerLifeDataCorrections.Select(PerLifeDataCorrectionViewModel.Expression());

            if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
            if (InsuredGenderCodePickListDetailId.HasValue) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId);
            if (TerritoryOfIssueCodePickListDetailId.HasValue) query = query.Where(q => q.TerritoryOfIssueCodePickListDetailId == TerritoryOfIssueCodePickListDetailId);
            if (PerLifeRetroGenderId.HasValue) query = query.Where(q => q.PerLifeRetroGenderId == PerLifeRetroGenderId);
            if (PerLifeRetroCountryId.HasValue) query = query.Where(q => q.PerLifeRetroCountryId == PerLifeRetroCountryId);
            if (dateOfExceptionDetected.HasValue) query = query.Where(q => q.DateOfExceptionDetected == dateOfExceptionDetected);
            if (dateOfPolicyExist.HasValue) query = query.Where(q => q.DateOfPolicyExist == dateOfPolicyExist);
            if (IsProceedToAggregate.HasValue) query = query.Where(q => q.IsProceedToAggregate == IsProceedToAggregate);
            if (dateUpdated.HasValue) query = query.Where(q => q.DateUpdated == dateUpdated);
            if (ExceptionStatusPickListDetailId.HasValue) query = query.Where(q => q.ExceptionStatusPickListDetailId == ExceptionStatusPickListDetailId);

            if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("InsuredGenderCodePickListDetailId")) query = query.OrderBy(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("InsuredGenderCodePickListDetailId")) query = query.OrderByDescending(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("TerritoryOfIssueCodePickListDetailId")) query = query.OrderBy(q => q.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TerritoryOfIssueCodePickListDetailId")) query = query.OrderByDescending(q => q.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("PerLifeRetroGenderId")) query = query.OrderBy(q => q.PerLifeRetroGender.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("PerLifeRetroGenderId")) query = query.OrderByDescending(q => q.PerLifeRetroGender.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("PerLifeRetroCountryId")) query = query.OrderBy(q => q.PerLifeRetroCountry.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("PerLifeRetroCountryId")) query = query.OrderByDescending(q => q.PerLifeRetroCountry.TerritoryOfIssueCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("DateOfExceptionDetected")) query = query.OrderBy(q => q.DateOfExceptionDetected);
            else if (SortOrder == Html.GetSortDsc("DateOfExceptionDetected")) query = query.OrderByDescending(q => q.DateOfExceptionDetected);
            else if (SortOrder == Html.GetSortAsc("DateOfPolicyExist")) query = query.OrderBy(q => q.DateOfPolicyExist);
            else if (SortOrder == Html.GetSortDsc("DateOfPolicyExist")) query = query.OrderByDescending(q => q.DateOfPolicyExist);
            else if (SortOrder == Html.GetSortAsc("IsProceedToAggregate")) query = query.OrderBy(q => q.IsProceedToAggregate);
            else if (SortOrder == Html.GetSortDsc("IsProceedToAggregate")) query = query.OrderByDescending(q => q.IsProceedToAggregate);
            else if (SortOrder == Html.GetSortAsc("DateUpdated")) query = query.OrderBy(q => q.DateUpdated);
            else if (SortOrder == Html.GetSortDsc("DateUpdated")) query = query.OrderByDescending(q => q.DateUpdated);
            else if (SortOrder == Html.GetSortAsc("ExceptionStatusPickListDetailId")) query = query.OrderBy(q => q.ExceptionStatusPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ExceptionStatusPickListDetailId")) query = query.OrderByDescending(q => q.ExceptionStatusPickListDetail.Code);
            else query = query.OrderBy(q => q.TreatyCode.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PerLifeDataCorrection/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new PerLifeDataCorrectionViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: PerLifeDataCorrection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeDataCorrectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);



                var trail = GetNewTrailObject();
                Result = PerLifeDataCorrectionService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Data Correction"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model);
            return View(model);
        }

        // GET: PerLifeDataCorrection/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeDataCorrectionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PerLifeDataCorrectionViewModel(bo);
            LoadPage(model);
            return View(model);
        }

        // POST: PerLifeDataCorrection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeDataCorrectionViewModel model)
        {
            var dbBo = PerLifeDataCorrectionService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeDataCorrectionService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Data Correction"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model);
            return View(model);
        }

        // GET: PerLifeDataCorrection/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeDataCorrectionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new PerLifeDataCorrectionViewModel(bo));
        }

        // POST: PerLifeDataCorrection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeDataCorrectionViewModel model)
        {
            var bo = PerLifeDataCorrectionService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = PerLifeDataCorrectionService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Data Correction"
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
                    var process = new ProcessPerLifeDataCorrection()
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
            int? TreatyCodeId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            int? InsuredGenderCodePickListDetailId,
            int? TerritoryOfIssueCodePickListDetailId,
            int? PerLifeRetroGenderId,
            int? PerLifeRetroCountryId,
            string DateOfExceptionDetected,
            string DateOfPolicyExist,
            bool? IsProceedToAggregate,
            string DateUpdated,
            int? ExceptionStatusPickListDetailId
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PerLifeDataCorrections.Select(PerLifeDataCorrectionViewModel.Expression());
            if (type == 2) // filtered dowload
            {
                DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
                DateTime? dateOfExceptionDetected = Util.GetParseDateTime(DateOfExceptionDetected);
                DateTime? dateOfPolicyExist = Util.GetParseDateTime(DateOfPolicyExist);
                DateTime? dateUpdated = Util.GetParseDateTime(DateUpdated);

                if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber.Contains(PolicyNumber));
                if (InsuredGenderCodePickListDetailId.HasValue) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId);
                if (TerritoryOfIssueCodePickListDetailId.HasValue) query = query.Where(q => q.TerritoryOfIssueCodePickListDetailId == TerritoryOfIssueCodePickListDetailId);
                if (PerLifeRetroGenderId.HasValue) query = query.Where(q => q.PerLifeRetroGenderId == PerLifeRetroGenderId);
                if (PerLifeRetroCountryId.HasValue) query = query.Where(q => q.PerLifeRetroCountryId == PerLifeRetroCountryId);
                if (dateOfExceptionDetected.HasValue) query = query.Where(q => q.DateOfExceptionDetected == dateOfExceptionDetected);
                if (dateOfPolicyExist.HasValue) query = query.Where(q => q.DateOfPolicyExist == dateOfPolicyExist);
                if (IsProceedToAggregate.HasValue) query = query.Where(q => q.IsProceedToAggregate == IsProceedToAggregate);
                if (dateUpdated.HasValue) query = query.Where(q => q.DateUpdated == dateUpdated);
                if (ExceptionStatusPickListDetailId.HasValue) query = query.Where(q => q.ExceptionStatusPickListDetailId == ExceptionStatusPickListDetailId);
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportPerLifeDataCorrection();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new PerLifeDataCorrectionBo
                {
                    Id = x.Id,
                    TreatyCode = x.TreatyCode.Code,
                    InsuredName = x.InsuredName,
                    InsuredDateOfBirth = x.InsuredDateOfBirth.Value,
                    PolicyNumber = x.PolicyNumber,
                    InsuredGenderCode = x.InsuredGenderCodePickListDetail.Code,
                    TerritoryOfIssueCode = x.TerritoryOfIssueCodePickListDetail.Code,
                    PerLifeRetroGenderStr = x.PerLifeRetroGender.InsuredGenderCodePickListDetail.Code,
                    PerLifeRetroCountryStr = x.PerLifeRetroCountry.TerritoryOfIssueCodePickListDetail.Code,
                    DateOfExceptionDetected = x.DateOfExceptionDetected.Value,
                    DateOfPolicyExist = x.DateOfPolicyExist.Value,
                    IsProceedToAggregate = x.IsProceedToAggregate,
                    DateUpdated = x.DateUpdated.Value,
                    ExceptionStatus = x.ExceptionStatusPickListDetail.Code,
                    Remark = x.Remark,
                }).AsQueryable());

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownTreatyCode(foreign: false);
            DropDownInsuredGenderCode();
            DropDownTerritoryOfIssueCode();
            DropDownPerLifeRetroGender();
            DropDownPerLifeRetroCountry();
            DropDownExceptionStatus();
            DropDownYesNoWithSelect();
            SetViewBagMessage();
        }

        public void LoadPage(PerLifeDataCorrectionViewModel model)
        {
            DropDownInsuredGenderCode();
            DropDownTerritoryOfIssueCode();
            DropDownPerLifeRetroGender();
            DropDownPerLifeRetroCountry();
            DropDownExceptionStatus();

            var entity = new PerLifeDataCorrection();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("InsuredName");
            ViewBag.InsuredNameMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("PolicyNumber");
            ViewBag.PolicyNumberMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 150;

            if (model.Id == 0)
            {
                DropDownTreatyCode(TreatyCodeBo.StatusActive, foreign: false);
            }
            else
            {
                DropDownTreatyCode(TreatyCodeBo.StatusActive, model.TreatyCodeId, foreign: false);
                var treatyCodeBo = TreatyCodeService.Find(model.TreatyCodeId);
                if (treatyCodeBo != null && treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                }
            }

            SetViewBagMessage();
        }
    }
}