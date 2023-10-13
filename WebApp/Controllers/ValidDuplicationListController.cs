using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Retrocession;
using PagedList;
using Services.Retrocession;
using Shared;
using System;
using System.Collections.Generic;
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
    public class ValidDuplicationListController : BaseController
    {
        public const string Controller = "ValidDuplicationList";

        // GET: ValidDuplicationList
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? TreatyCodeId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string CedantPlanCode,
            int? MLReBenefitCodeId,
            string SortOrder,
            int? Page)
        {
            var insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["TreatyCodeId"] = TreatyCodeId,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? insuredDateOfBirth : null,
                ["PolicyNumber"] = PolicyNumber,
                ["CedantPlanCode"] = CedantPlanCode,
                ["MLReBenefitCodeId"] = MLReBenefitCodeId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortTreatyCode = GetSortParam("TreatyCodeId");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortCedantPlanCode = GetSortParam("CedantPlanCode");
            ViewBag.SortMLReBenefitCodeId = GetSortParam("MLReBenefitCodeId");

            var query = _db.ValidDuplicationLists.Select(ValidDuplicationListViewModel.Expression());

            if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
            if (!string.IsNullOrEmpty(CedantPlanCode)) query = query.Where(q => q.CedantPlanCode == CedantPlanCode);
            if (MLReBenefitCodeId.HasValue) query = query.Where(q => q.MLReBenefitCodeId == MLReBenefitCodeId);

            if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("CedantPlanCode")) query = query.OrderBy(q => q.CedantPlanCode);
            else if (SortOrder == Html.GetSortDsc("CedantPlanCode")) query = query.OrderByDescending(q => q.CedantPlanCode);
            else if (SortOrder == Html.GetSortAsc("MLReBenefitCodeId")) query = query.OrderBy(q => q.MLReBenefitCode.Code);
            else if (SortOrder == Html.GetSortDsc("MLReBenefitCodeId")) query = query.OrderByDescending(q => q.MLReBenefitCode.Code);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownTreatyCode(foreign: false);
            DropDownBenefit();

            SetViewBagMessage();
        }

        //// GET: ValidDuplicationList/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new ValidDuplicationListViewModel());
        }

        // Post: ValidDuplicationList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ValidDuplicationListViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();

                model.Id = bo.Id;
                Result = ValidDuplicationListService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Valid Duplication List"
                        );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }

                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }
            LoadPage();
            return View(model);
        }

        // GET:ValidDuplicationList/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = ValidDuplicationListService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage();
            return View(new ValidDuplicationListViewModel(bo));
        }

        // POST: ValidDuplicationList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ValidDuplicationListViewModel model)
        {
            var dbBo = ValidDuplicationListService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = ValidDuplicationListService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Valid Duplication List"
                        );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }

            LoadPage();
            return View(model);
        }

        // GET: ValidDuplicationList/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ValidDuplicationListService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new ValidDuplicationListViewModel(bo));
        }

        // POST: ValidDuplicationList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, ValidDuplicationListViewModel model)
        {
            var bo = ValidDuplicationListService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = ValidDuplicationListService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Valid Duplication List"
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

        public void LoadPage()
        {
            var entity = new ValidDuplicationList();
            var cedantPlanCodeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedantPlanCode");
            ViewBag.CedantPlanCodeMaxLength = cedantPlanCodeCodeMaxLengthAttr != null ? cedantPlanCodeCodeMaxLengthAttr.Length : 255;
            var insuredNameMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("InsuredName");
            ViewBag.InsuredNameMaxLength = insuredNameMaxLengthAttr != null ? insuredNameMaxLengthAttr.Length : 255;
            var policyNumberMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("PolicyNumber");
            ViewBag.PolicyNumberMaxLength = policyNumberMaxLengthAttr != null ? policyNumberMaxLengthAttr.Length : 50;
            var cedingBenefitRiskCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitRiskCode");
            ViewBag.CedingBenefitRiskCodeMaxLength = cedingBenefitRiskCodeMaxLengthAttr != null ? cedingBenefitRiskCodeMaxLengthAttr.Length : 100;
            var cedingBenefitTypeCodeMaxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("CedingBenefitTypeCode");
            ViewBag.CedingBenefitTypeCodeMaxLength = cedingBenefitTypeCodeMaxLengthAttr != null ? cedingBenefitTypeCodeMaxLengthAttr.Length : 100;

            DropDownTreatyCode(foreign: false);
            DropDownInsuredGenderCode();
            DropDownBenefit();
            DropDownFundsAccountingTypeCode();
            DropDownReinsBasisCode();
            DropDownTransactionTypeCode();
            //DropDownTreatyCode();
            //DropDownBenefit();

            SetViewBagMessage();
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? TreatyCodeId,
            string InsuredName,
            string InsuredDateOfBirth,
            string PolicyNumber,
            string CedantPlanCode,
            int? MLReBenefitCodeId
        )
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;

            var query = _db.ValidDuplicationLists.Select(ValidDuplicationListViewModel.Expression());

            var insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);

            if (type == 2)
            {
                if (TreatyCodeId.HasValue) query = query.Where(q => q.TreatyCodeId == TreatyCodeId);
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
                if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.PolicyNumber == PolicyNumber);
                if (!string.IsNullOrEmpty(CedantPlanCode)) query = query.Where(q => q.CedantPlanCode == CedantPlanCode);
                if (MLReBenefitCodeId.HasValue) query = query.Where(q => q.MLReBenefitCodeId == MLReBenefitCodeId);
            }
            if (type == 3)
            {
                query = null;
            }
            var export = new ExportValidDuplicationList();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new ValidDuplicationListBo
                {
                    Id = x.Id,
                    TreatyCode = x.TreatyCode.Code,
                    CedantPlanCode = x.CedantPlanCode,
                    InsuredName = x.InsuredName,
                    InsuredDateOfBirth = x.InsuredDateOfBirth,
                    PolicyNumber = x.PolicyNumber,
                    InsuredGenderCodePickListDetailId = x.InsuredGenderCodePickListDetailId,
                    InsuredGenderCodePickList = x.InsuredGenderCodePickListDetail.Code,
                    MLReBenefitCodeId = x.MLReBenefitCodeId,
                    MLReBenefitCode = x.MLReBenefitCode.Code,
                    CedingBenefitRiskCode = x.CedingBenefitRiskCode,
                    CedingBenefitTypeCode = x.CedingBenefitTypeCode,
                    ReinsuranceEffectiveDate = x.ReinsuranceEffectiveDate,
                    FundsAccountingTypePickListDetailId = x.FundsAccountingTypePickListDetailId,
                    FundsAccountingTypePickList = x.FundsAccountingTypePickListDetail.Code,
                    ReinsBasisCodePickListDetailId = x.ReinsBasisCodePickListDetailId,
                    ReinsBasisCodePickList = x.ReinsBasisCodePickListDetail.Code,
                    TransactionTypePickListDetailId = x.TransactionTypePickListDetailId,
                    TransactionTypePickList = x.TransactionTypePickListDetail.Code
                }));

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var process = new ProcessValidDuplicationListing()
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


    }
}