using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
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
    public class AnnuityFactorController : BaseController
    {
        public const string Controller = "AnnuityFactor";

        // GET: AnnuityFactor
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CedantId,
            string CedingPlanCode,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            string SortOrder,
            int? Page)
        {
            DateTime? start = Util.GetParseDateTime(ReinsEffDatePolStartDate);
            DateTime? end = Util.GetParseDateTime(ReinsEffDatePolEndDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["CedingPlanCode"] = CedingPlanCode,
                ["ReinsEffDatePolStartDate"] = start.HasValue ? ReinsEffDatePolStartDate : null,
                ["ReinsEffDatePolEndDate"] = end.HasValue ? ReinsEffDatePolEndDate : null,
                ["SortOrder"] = SortOrder,
            };

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");

            var query = _db.AnnuityFactors.Select(AnnuityFactorViewModel.Expression());

            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.AnnuityFactorMappings.Any(d => d.CedingPlanCode == CedingPlanCode));

            if (start.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= start);
            if (end.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= end);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: AnnuityFactor/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            AnnuityFactorViewModel model = new AnnuityFactorViewModel();
            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: AnnuityFactor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, AnnuityFactorViewModel model)
        {
            Result childResult = new Result();
            List<AnnuityFactorDetailBo> annuityFactorDetailBos = model.GetAnnuityFactorDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                Result = AnnuityFactorService.Result();
                var mappingResult = AnnuityFactorService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }

                if (!childResult.Valid)
                {
                    Result.AddErrorRange(childResult.ToErrorArray());
                }
                else
                {
                    model.ValidateDuplicate(annuityFactorDetailBos, ref childResult);
                    if (!childResult.Valid)
                    {
                        Result.AddErrorRange(childResult.ToErrorArray());
                    }
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = AnnuityFactorService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        AnnuityFactorService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        model.ProcessAnnuityFactorDetails(annuityFactorDetailBos, AuthUserId, ref trail);

                        CreateTrail(
                            bo.Id,
                            "Create Annuity Factor"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                else
                {
                    AddResult(Result);
                    LoadPage(model, annuityFactorDetailBos);
                    ViewBag.Disabled = false;
                    return View(model);
                }

                AddResult(Result);
            }
            LoadPage(model, annuityFactorDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: AnnuityFactor/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = AnnuityFactorService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            AnnuityFactorViewModel model = new AnnuityFactorViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = false;
            return View(model);
        }

        // POST: AnnuityFactor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, AnnuityFactorViewModel model)
        {
            var dbBo = AnnuityFactorService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<AnnuityFactorDetailBo> annuityFactorDetailBos = model.GetAnnuityFactorDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                Result = AnnuityFactorService.Result();
                var mappingResult = AnnuityFactorService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }

                if (!childResult.Valid)
                {
                    Result.AddErrorRange(childResult.ToErrorArray());
                }
                else
                {
                    model.ValidateDuplicate(annuityFactorDetailBos, ref childResult);
                    if (!childResult.Valid)
                    {
                        Result.AddErrorRange(childResult.ToErrorArray());
                    }
                }

                if (Result.Valid)
                {
                    var trail = GetNewTrailObject();
                    Result = AnnuityFactorService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        AnnuityFactorService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        model.ProcessAnnuityFactorDetails(annuityFactorDetailBos, AuthUserId, ref trail);
                        CreateTrail(
                            bo.Id,
                            "Update Annuity Factor"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = bo.Id });
                    }
                }
                else
                {
                    AddResult(Result);
                    LoadPage(model, annuityFactorDetailBos);
                    ViewBag.Disabled = false;
                    return View(model);
                }
                AddResult(Result);
            }

            LoadPage(model, annuityFactorDetailBos);
            ViewBag.Disabled = false;
            return View(model);
        }

        // GET: AnnuityFactor/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = AnnuityFactorService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            AnnuityFactorViewModel model = new AnnuityFactorViewModel(bo);
            LoadPage(model);
            ViewBag.Disabled = true;
            return View(model);
        }

        // POST: AnnuityFactor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, AnnuityFactorViewModel model)
        {
            var bo = AnnuityFactorService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = AnnuityFactorService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Annuity Factor"
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
        public ActionResult UploadDetail(FormCollection form, AnnuityFactorViewModel model)
        {
            ModelState.Clear();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var dbBo = AnnuityFactorService.Find(model.Id);

            Result childResult = new Result();
            List<AnnuityFactorDetailBo> annuityFactorDetailBos = model.GetAnnuityFactorDetails(form, ref childResult);

            HttpPostedFileBase postedFile = Request.Files["DetailFile"];
            try
            {
                TextFile textFile = new TextFile(postedFile.InputStream);
                while (textFile.GetNextRow() != null)
                {
                    if (textFile.RowIndex == 1)
                        continue; // Skip header row

                    var afd = new AnnuityFactorDetailBo
                    {
                        Id = 0,
                        PolicyTermRemainStr = textFile.GetColValue(AnnuityFactorDetailBo.TypePolicyTermRemain),
                        InsuredGenderCode = textFile.GetColValue(AnnuityFactorDetailBo.TypeInsuredGenderCode),
                        InsuredTobaccoUse = textFile.GetColValue(AnnuityFactorDetailBo.TypeInsuredTobaccoUse),
                        PolicyTermStr = textFile.GetColValue(AnnuityFactorDetailBo.TypePolicyTerm),
                        AnnuityFactorValueStr = textFile.GetColValue(AnnuityFactorDetailBo.TypeAnnuityFactorValue)
                    };

                    if (!string.IsNullOrEmpty(afd.PolicyTermRemainStr))
                    {
                        if (int.TryParse(afd.PolicyTermRemainStr, out int attainedAge))
                        {
                            afd.PolicyTermRemain = attainedAge;
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format("The Term Remain is not numeric: {0} at row {1}", afd.PolicyTermRemainStr, textFile.RowIndex));
                        }
                    }

                    if (!string.IsNullOrEmpty(afd.InsuredGenderCode))
                    {
                        PickListDetailBo igc = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, afd.InsuredGenderCode);
                        if (igc == null)
                        {
                            afd.InsuredGenderCode = null;
                            ModelState.AddModelError("", string.Format("The Insured Gender Code doesn't exists: {0} at row {1}", afd.InsuredGenderCode, textFile.RowIndex));
                        }
                        else
                        {
                            afd.InsuredGenderCodePickListDetailId = igc.Id;
                            afd.InsuredGenderCodePickListDetailBo = igc;
                        }
                    }

                    if (!string.IsNullOrEmpty(afd.InsuredTobaccoUse))
                    {
                        PickListDetailBo itu = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredTobaccoUse, afd.InsuredTobaccoUse);
                        if (itu == null)
                        {
                            afd.InsuredTobaccoUse = null;
                            ModelState.AddModelError("", string.Format("The Insured Tobacco Use doesn't exists: {0} at file row {1}", afd.InsuredGenderCode, textFile.RowIndex));
                        }
                        else
                        {
                            afd.InsuredTobaccoUsePickListDetailId = itu.Id;
                            afd.InsuredTobaccoUsePickListDetailBo = itu;
                        }
                    }

                    var insuredAttainedAgeStr = textFile.GetColValue(AnnuityFactorDetailBo.TypeInsuredAttainedAge);
                    if (!string.IsNullOrEmpty(insuredAttainedAgeStr))
                    {
                        if (int.TryParse(insuredAttainedAgeStr, out int insuredAttainedAge))
                        {
                            afd.InsuredAttainedAge = insuredAttainedAge;
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format("The Insured Attained Age is not a numeric: {0} at file row {1}", insuredAttainedAgeStr, textFile.RowIndex));
                        }
                    }

                    if (!string.IsNullOrEmpty(afd.PolicyTermStr))
                    {
                        if (Util.IsValidDouble(afd.PolicyTermStr, out double? d, out string _))
                        {
                            afd.PolicyTerm = d.Value;
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format("The Policy Term is not valid double: {0} at row {1}", afd.PolicyTermStr, textFile.RowIndex));
                        }
                    }

                    if (!string.IsNullOrEmpty(afd.AnnuityFactorValueStr))
                    {
                        if (Util.IsValidDouble(afd.AnnuityFactorValueStr, out double? d, out string _))
                        {
                            afd.AnnuityFactorValue = d.Value;
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Format("The Annuity Factor is not valid double: {0} at row {1}", afd.AnnuityFactorValueStr, textFile.RowIndex));
                        }
                    }

                    bool isUpdate = false;

                    string idStr = textFile.GetColValue(AnnuityFactorDetailBo.TypeAnnuityFactorDetailId);
                    if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                    {
                        if (annuityFactorDetailBos.Any(q => q.Id == id))
                        {
                            afd.Id = id;
                            int index = annuityFactorDetailBos.FindIndex(q => q.Id == id);
                            annuityFactorDetailBos[index] = afd;
                            isUpdate = true;
                        }
                    }

                    if (!isUpdate)
                    {
                        annuityFactorDetailBos.Add(afd);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                CheckEditPageReadOnly(Controller);
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            ViewBag.Disabled = false;
            LoadPage(model, annuityFactorDetailBos);
            return View(view, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadDetail(FormCollection form, AnnuityFactorViewModel model, int? type = null)
        {
            List<Column> cols = AnnuityFactorDetailBo.GetColumns();
            string filename = "AnnuityFactor".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            string filePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(filePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"AnnuityFactor*");

            // Header
            ExportWriteLine(filePath, string.Join(",", cols.Select(p => p.Header)));

            if (type == 1)
            {
                Result childResult = new Result();
                List<AnnuityFactorDetailBo> annuityFactorDetailBos = model.GetAnnuityFactorDetails(form, ref childResult);

                foreach (var annuityFactorDetail in annuityFactorDetailBos)
                {
                    List<string> values = new List<string> { };
                    foreach (var col in cols)
                    {
                        if (string.IsNullOrEmpty(col.Property))
                        {
                            values.Add("");
                            continue;
                        }

                        string value = "";
                        object v = null;

                        switch (col.ColIndex)
                        {
                            case AnnuityFactorDetailBo.TypeAnnuityFactorDetailId:
                                v = annuityFactorDetail.GetPropertyValue(col.Property).ToString() != "0" ? annuityFactorDetail.GetPropertyValue(col.Property) : null;
                                break;
                            case AnnuityFactorDetailBo.TypeInsuredGenderCode:
                                v = PickListDetailService.Find(annuityFactorDetail.InsuredGenderCodePickListDetailId)?.Code;
                                break;
                            case AnnuityFactorDetailBo.TypeInsuredTobaccoUse:
                                v = PickListDetailService.Find(annuityFactorDetail.InsuredTobaccoUsePickListDetailId)?.Code;
                                break;
                            default:
                                v = annuityFactorDetail.GetPropertyValue(col.Property);
                                break;
                        }

                        if (v != null)
                        {
                            if (v is DateTime d)
                            {
                                value = d.ToString(Util.GetDateFormat());
                            }
                            else
                            {
                                value = v.ToString();
                            }
                        }

                        values.Add(string.Format("\"{0}\"", value));
                    }
                    string line = string.Join(",", values.ToArray());
                    ExportWriteLine(filePath, line);
                }
            }

            return File(filePath, "text/csv", Path.GetFileName(filePath));
        }

        public void ExportWriteLine(string filePath, object line)
        {
            using (var textFile = new TextFile(filePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            SetViewBagMessage();
        }

        public void LoadPage(AnnuityFactorViewModel model, List<AnnuityFactorDetailBo> annuityFactorDetailBos = null)
        {
            DropDownInsuredGenderCode();
            DropDownInsuredTobaccoUse();

            if (model.Id == 0)
            {
                // Create
                DropDownCedant(CedantBo.StatusActive);
            }
            else
            {
                // Edit
                DropDownCedant(CedantBo.StatusActive, model.CedantId);

                if (model.CedantBo != null && model.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                if (annuityFactorDetailBos == null || annuityFactorDetailBos.Count == 0)
                {
                    annuityFactorDetailBos = AnnuityFactorDetailService.GetByAnnuityFactorId(model.Id).ToList();
                }
            }

            ViewBag.AnnuityFactorDetailBos = annuityFactorDetailBos;
            SetViewBagMessage();
        }
    }
}
