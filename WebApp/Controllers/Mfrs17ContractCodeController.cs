using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class Mfrs17ContractCodeController : BaseController
    {
        public const string Controller = "Mfrs17ContractCode";

        // GET: Mfrs17ContractCode
        public ActionResult Index(
            int? CedingCompanyId, 
            string ModifiedContractCode, 
            string Mfrs17ContractCode, 
            string SortOrder, 
            int? Page
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedingCompanyId"] = CedingCompanyId,
                ["ModifiedContractCode"] = ModifiedContractCode,
                ["Mfrs17ContractCode"] = Mfrs17ContractCode,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedingCompanyId = GetSortParam("CedingCompanyId");
            ViewBag.SortModifiedContractCode = GetSortParam("ModifiedContractCode");
            ViewBag.SortMfrs17ContractCode = GetSortParam("Mfrs17ContractCode");

            var query = _db.Mfrs17ContractCodes.LeftOuterJoin(_db.Mfrs17ContractCodeDetails, c => c.Id, d => d.Mfrs17ContractCodeId, (c, d) => new Mfrs17ContractCodeWithDetail { ContractCode = c, ContractCodeDetail = d })
                .Select(Mfrs17ContractCodeViewModel.ExpressionWithDetail());

            if (CedingCompanyId != null) query = query.Where(q => q.CedingCompanyId == CedingCompanyId);
            if (!string.IsNullOrEmpty(ModifiedContractCode)) query = query.Where(q => q.ModifiedContractCode.Contains(ModifiedContractCode));
            if (!string.IsNullOrEmpty(Mfrs17ContractCode)) query = query.Where(q => q.Mfrs17ContractCode.Contains(Mfrs17ContractCode));

            if (SortOrder == Html.GetSortAsc("CedingCompanyId")) query = query.OrderBy(q => q.CedingCompanyId);
            else if (SortOrder == Html.GetSortDsc("CedingCompanyId")) query = query.OrderByDescending(q => q.CedingCompanyId);
            else if (SortOrder == Html.GetSortAsc("ModifiedContractCode")) query = query.OrderBy(q => q.ModifiedContractCode);
            else if (SortOrder == Html.GetSortDsc("ModifiedContractCode")) query = query.OrderByDescending(q => q.ModifiedContractCode);
            else if (SortOrder == Html.GetSortAsc("Mfrs17ContractCode")) query = query.OrderBy(q => q.Mfrs17ContractCode);
            else if (SortOrder == Html.GetSortDsc("Mfrs17ContractCode")) query = query.OrderByDescending(q => q.Mfrs17ContractCode);
            else query = query
                    .OrderBy(q => q.CedingCompanyId)
                    .ThenBy(q => q.ModifiedContractCode)
                    .ThenBy(q => q.Mfrs17ContractCode);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownCedant(CedantBo.StatusActive);
            SetViewBagMessage();
        }

        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new Mfrs17ContractCodeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, Mfrs17ContractCodeViewModel model)
        {
            Result childResult = new Result();
            var mfrs17ContractCodeDetailBos = model.GetMfrs17ContractCodeDetails(form, ref childResult);

            var mfrs17ContractCodeBo = new Mfrs17ContractCodeBo
            {
                CedingCompanyId = model.CedingCompanyId,
                ModifiedContractCode = model.ModifiedContractCode?.Trim(),
                CreatedById = AuthUserId,
                UpdatedById = AuthUserId,
            };

            if (ModelState.IsValid)
            {
                TrailObject trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    Result = Mfrs17ContractCodeService.Create(ref mfrs17ContractCodeBo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = mfrs17ContractCodeBo.Id;
                        model.ProcessMfrs17ContractCodeDetails(mfrs17ContractCodeDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            mfrs17ContractCodeBo.Id,
                            "Create MFRS17 Contract Code"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = mfrs17ContractCodeBo.Id });
                    }
                    AddResult(Result);
                }
                AddResult(childResult);
            }
            LoadPage(mfrs17ContractCodeBo, mfrs17ContractCodeDetailBos);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = Mfrs17ContractCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new Mfrs17ContractCodeViewModel(bo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, Mfrs17ContractCodeViewModel model)
        {
            Mfrs17ContractCodeBo mfrs17ContractCodeBo = Mfrs17ContractCodeService.Find(id);
            if (mfrs17ContractCodeBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<Mfrs17ContractCodeDetailBo> mfrs17ContractCodeDetailBos = model.GetMfrs17ContractCodeDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                mfrs17ContractCodeBo.CedingCompanyId = model.CedingCompanyId;
                mfrs17ContractCodeBo.ModifiedContractCode = model.ModifiedContractCode;
                mfrs17ContractCodeBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    Result = Mfrs17ContractCodeService.Update(ref mfrs17ContractCodeBo, ref trail);
                    if (Result.Valid)
                    {

                        model.ProcessMfrs17ContractCodeDetails(mfrs17ContractCodeDetailBos, AuthUserId, ref trail);
                        CreateTrail(
                            mfrs17ContractCodeBo.Id,
                            "Update MFRS17 Contract Code"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = mfrs17ContractCodeBo.Id });
                    }
                    AddResult(Result);
                }
                AddResult(childResult);
            }
            LoadPage(mfrs17ContractCodeBo, mfrs17ContractCodeDetailBos);
            return View(model);
        }

        public void LoadPage(Mfrs17ContractCodeBo bo = null, List<Mfrs17ContractCodeDetailBo> mfrs17ContractCodeDetailBos = null)
        {
            ViewBag.MaxLength = 255;
            DropDownCedant(CedantBo.StatusActive);

            if (bo == null)
            {
                ViewBag.Mfrs17ContractCodeDetailBos = new List<Mfrs17ContractCodeDetailBo>();
            }
            else
            {
                if (mfrs17ContractCodeDetailBos == null || mfrs17ContractCodeDetailBos.Count == 0)
                {
                    mfrs17ContractCodeDetailBos = Mfrs17ContractCodeDetailService.GetByMfrs17ContractCode(bo.Id).ToList();
                }
                ViewBag.Mfrs17ContractCodeDetailBos = mfrs17ContractCodeDetailBos;
            }
            SetViewBagMessage();
        }

        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = Mfrs17ContractCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Mfrs17ContractCodeViewModel(bo));
        }

        // POST: ValidDuplicationList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, Mfrs17ContractCodeViewModel model)
        {
            var bo = Mfrs17ContractCodeService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = Mfrs17ContractCodeService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete MFRS17 Contract Code"
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
                    var process = new ProcessMfrs17ContractCode()
                    {
                        PostedFile = upload,
                        AuthUserId = AuthUserId,
                    };
                    process.Process();

                    if (process.Errors.Count() > 0 && process.Errors != null)
                    {
                        SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    }

                    int createParent = process.GetProcessCount("Create Parent");
                    int updateParent = process.GetProcessCount("Update Parent");
                    int deleteParent = process.GetProcessCount("Delete Parent");
                    int createChild = process.GetProcessCount("Create Child");
                    int updateChild = process.GetProcessCount("Update Child");
                    int deleteChild = process.GetProcessCount("Delete Child");

                    SetSuccessSessionMsgArr(new List<string>
                    {
                        string.Format("Process File Successful, Total Parent Created: {0}, Total Parent Updated: {1}, Total Parent Deleted: {2}", createParent, updateParent, deleteParent),
                        string.Format("Process File Successful, Total Child Created: {0}, Total Child Updated: {1}, Total Child Deleted: {2}", createChild, updateChild, deleteChild),
                    });
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? CedingCompanyId,
            string ModifiedContractCode,
            string Mfrs17ContractCode
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.Mfrs17ContractCodes.LeftOuterJoin(_db.Mfrs17ContractCodeDetails, c => c.Id, d => d.Mfrs17ContractCodeId, (c, d) => new Mfrs17ContractCodeWithDetail { ContractCode = c, ContractCodeDetail = d })
                .Select(Mfrs17ContractCodeService.ExpressionWithDetail());

            if (type == 2) // filtered dowload
            {

                if (CedingCompanyId != null) query = query.Where(q => q.CedingCompanyId == CedingCompanyId);
                if (!string.IsNullOrEmpty(ModifiedContractCode)) query = query.Where(q => q.ModifiedContractCode.Contains(ModifiedContractCode));
                if (!string.IsNullOrEmpty(Mfrs17ContractCode)) query = query.Where(q => q.Mfrs17ContractCode.Contains(Mfrs17ContractCode));
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportMfrs17ContractCode();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        [HttpPost]
        public JsonResult ValidateDetailDelete(int mfrs17ContractCodeDetailId)
        {
            bool valid = true;

            if (Mfrs17CellMappingService.CountByMfrs17ContractCodeDetailId(mfrs17ContractCodeDetailId) > 0)
                valid = false;

            return Json(new { IsValid = valid });
        }
    }
}