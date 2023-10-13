using Services.Retrocession;
using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using BusinessObject.Retrocession;
using PagedList;
using Shared;
using System.Web.Routing;
using WebApp.Middleware;
using BusinessObject;
using Services;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeClaimRetroDataController : BaseController
    {
        public const string Controller = "PerLifeClaim";
        // GET: ClaimRetroData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimRetroDataService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            PerLifeClaimRetroDataViewModel model = new PerLifeClaimRetroDataViewModel(bo);
            LoadPage(model);
            return View(model);
        } 

        public void LoadPage(PerLifeClaimRetroDataViewModel model)
        {
            // LoadShared to load ClaimRegister Shared _GeneralTab and _RiDataTab
            //ViewBag.PreviousQuarter = PerLifeClaimDataService.FindPreviousQuarter(model.PerLifeClaimDataId);
            ViewBag.PreviousQuarter = PerLifeClaimRetroDataService.FindPreviousQuarter(model.Id);
            ViewBag.CurrentQuarter = PerLifeClaimRetroDataService.Find(model.Id);

            // back button
            ViewBag.EditPerLifeClaimData = Url.Action("ClaimRetroData/" + model.PerLifeClaimDataBo.PerLifeClaimId, "PerLifeCLaim");

            // Drop Down
            DropDownDraftingStatusCategory();
            DropDownUser(UserBo.StatusActive);
            AuthUserName();
            DropDownClaimCode(ClaimCodeBo.StatusActive, selectedCode: model.ClaimCode, codeAsValue: true);
            DropDownInsuredGenderCode(true);
            DropDownInsuredTobaccoUse(true);
            ViewBag.DropDownClaimUsers = DropDownPicClaim(UserBo.StatusActive);
            DropDownClaimAssessor(UserBo.StatusActive, model.ClaimAssessorId);
            DropDownClaimReason(ClaimReasonBo.TypeClaimDeclinePending);
            DropDownPicClaim(UserBo.StatusActive, model.PicClaimId);

            // Standard output for RI Data
            ViewBag.StandardOutputList = StandardOutputService.Get();

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.PerLifeClaimDataId, downloadDocumentUrl, BusinessObject.ModuleBo.ModuleController.PerLifeClaimRetroData.ToString(), model.Id);
            GetRemarkSubjects();
            //// Remark & Document
            //string downloadDocumentUrl = Url.Action("Download", "Document");
            //GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl);
            //GetRemarkSubjects();

            // Retro Recovery Details
            ViewBag.RecoveryDetails = PerLifeClaimRetroDataService.GetByPerLifeClaimDataId(model.PerLifeClaimDataId);
            ViewBag.PersonInChargeId = AuthUserId;
        }

        public ActionResult EditRecoveryDetails(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeClaimRetroDataService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Edit");
            }

            PerLifeClaimRetroDataViewModel model = new PerLifeClaimRetroDataViewModel(bo);
            RecoveryDetailsLoadPage(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult EditRecoveryDetails(int id, FormCollection form, PerLifeClaimRetroDataViewModel model)
        {
            var dbBo = PerLifeClaimRetroDataService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeClaimRetroDataService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Claim Retro Data"
                    );


                    SetUpdateSuccessMessage("Per Life Claim Retro Data Recovery Details", false);
                    return RedirectToAction("EditRecoveryDetails", new { id = bo.Id });
                }
                AddResult(Result);
            }

            RecoveryDetailsLoadPage(model);
            return View(model);
        }

        public void RecoveryDetailsLoadPage(PerLifeClaimRetroDataViewModel model)
        {   // back button
            ViewBag.EditPerLifeClaimData = Url.Action("ClaimRetroData/" + model.PerLifeClaimDataBo.PerLifeClaimId, "PerLifeCLaim");
            ViewBag.EditPerLifeClaimData2 = Url.Action("Edit/" + model.Id, "PerLifeClaimRetroData");

            // Drop Down
            DropDownDraftingStatusCategory();
            DropDownRetroTreaty();
            DropDownClaimCategory();
            DropDownPerLifeClaimRetroDataClaimCategory();

            ViewBag.RecoveryDetails = PerLifeClaimRetroDataService.GetByPerLifeClaimDataId(model.PerLifeClaimDataId);
            ViewBag.PersonInChargeId = AuthUserId;
        }

        public List<SelectListItem> DropDownDraftingStatusCategory()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, PerLifeClaimDataBo.ClaimRecoveryDecisionMax))
            {
                items.Add(new SelectListItem { Text = PerLifeClaimDataBo.GetClaimRecoveryDecisionName(i), Value = i.ToString() });
            }
            ViewBag.DropDownCurrentQuarterDecision = items;
            return items;
        }
        public List<SelectListItem> DropDownPerLifeClaimRetroDataClaimCategory()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, PerLifeClaimDataBo.ClaimCategoryStatusMax))
            {
                items.Add(new SelectListItem { Text = PerLifeClaimDataBo.GetClaimCategoryName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPerLifeClaimRetroDataClaimCategories = items;
            return items;
        }
    }
}