using BusinessObject;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ExportController : BaseController
    {
        public const string Controller = "Export";

        // GET: Export/Edit/5
        public ActionResult Edit(int id)
        {
            SetViewBagMessage();
            //if (!CheckEditPageReadOnly(Controller))
            //    return RedirectDashboard();

            var bo = ExportService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.AutoDownload = false;
            if (Session["DownloadExport"] is bool @bool && @bool)
            {
                ViewBag.AutoDownload = true;
                Session.Remove("DownloadExport");
            }

            string path = string.Format("{0}/WebAppLog".AppendDateFileName(".txt"), Util.GetLogPath("WebApp"));
            Util.MakeDir(path);
            using (var textFile = new TextFile(path, true, true))
            {
                textFile.WriteLine(string.Format("{0}   {1}", DateTime.Now.ToString(Util.GetDateTimeConsoleFormat()), bo));
                textFile.WriteLine();
                textFile.WriteLine();
            }

            bo.ConvertParametersDic();
            
            int value;
            if (bo.ParameterDic != null && bo.ParameterDic.ContainsKey("CutOffId") && bo.ParameterDic["CutOffId"] != null && int.TryParse(bo.ParameterDic["CutOffId"].ToString(), out value))
            {
                var cutOffBo = CutOffService.Find(value);
                if (cutOffBo != null)
                {
                    bo.ParameterDic["CutOffId"] = cutOffBo.GetQuarterWithDate();
                }
            }

            return View(new ExportViewModel(bo));
        }

        // GET: Cedant/Delete/5
        //[Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = ExportService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            bo.ConvertParametersDic();

            return View(new ExportViewModel(bo));
        }

        // POST: Cedant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bo = ExportService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ExportService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Export"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Dashboard", "Home");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public ActionResult Download(int Id)
        {
            var bo = ExportService.Find(Id);
            if (bo == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            if (!bo.IsFileExists())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            string filePath = bo.GetPath();
            return File(filePath, bo.GetContentType(), System.IO.Path.GetFileName(filePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suspend(int Id)
        {
            var bo = ExportService.Find(Id);
            if (bo == null)
                return RedirectToAction("Dashboard", "Home");
            if (bo.Status != ExportBo.StatusGenerating)
                return RedirectToAction("Edit", new { id = bo.Id });

            bo.Status = ExportBo.StatusSuspended;
            bo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = ExportService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Suspend Export"
                );

                SetSuccessSessionMsg(MessageBag.ExportSuccessfullySuspended);
            }
            AddResult(Result);

            return RedirectToAction("Edit", new { id = bo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForProcessing(int Id)
        {
            var bo = ExportService.Find(Id);
            if (bo == null)
                return RedirectToAction("Dashboard", "Home");
            switch (bo.Status)
            {
                case ExportBo.StatusPending:
                case ExportBo.StatusGenerating:
                    return RedirectToAction("Edit", new { id = bo.Id });
            }

            bo.Status = ExportBo.StatusPending;
            bo.Processed = 0;
            bo.GenerateStartAt = null;
            bo.GenerateEndAt = null;
            bo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = ExportService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Export - Submit for Processing"
                );

                SetSuccessSessionMsg(MessageBag.ExportSuccessfullySubmitForProcess);
            }
            AddResult(Result);

            return RedirectToAction("Edit", new { id = bo.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelled(int Id)
        {
            var bo = ExportService.Find(Id);
            if (bo == null)
                return RedirectToAction("Dashboard", "Home");
            if (bo.Status != ExportBo.StatusPending)
                return RedirectToAction("Edit", new { id = bo.Id });

            bo.Status = ExportBo.StatusCancelled;
            bo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = ExportService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Cancel Export"
                );

                SetSuccessSessionMsg(MessageBag.ExportSuccessfullyCancelled);
            }
            AddResult(Result);

            return RedirectToAction("Edit", new { id = bo.Id });
        }
    }
}
