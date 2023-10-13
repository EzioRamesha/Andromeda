using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using PagedList;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
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
    public class FacMasterListingController : BaseController
    {
        public const string Controller = "FacMasterListing";

        // GET: FacMasterListing
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string UniqueId,
            int? EwarpNumber,
            string InsuredName,
            string InsuredDateOfBirth,
            int? InsuredGenderCodePickListDetailId,
            int? CedantId,
            string PolicyNumber,
            string FlatExtraAmountOffered,
            int? FlatExtraDuration,
            string BenefitCode,
            string SumAssuredOffered,
            string EwarpActionCode,
            string UwRatingOffered,
            string OfferLetterSentDate,
            string UwOpinion,
            string Remark,
            string CedingBenefitTypeCode,
            string SortOrder,
            string UploadCreatedAt,
            string UploadFileName,
            int? UploadSubmittedBy,
            int? UploadStatus,
            string UploadSortOrder,
            int? UploadPage,
            int? Page,
            int? ActiveTab)
        {
            DateTime? insuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirth);
            DateTime? offerLetterSentDate = Util.GetParseDateTime(OfferLetterSentDate);

            double? flatExtraAmountOffered = Util.StringToDouble(FlatExtraAmountOffered);
            double? sumAssuredOffered = Util.StringToDouble(SumAssuredOffered);
            double? uwRatingOffered = Util.StringToDouble(UwRatingOffered);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["UniqueId"] = UniqueId,
                ["EwarpNumber"] = EwarpNumber,
                ["InsuredName"] = InsuredName,
                ["InsuredDateOfBirth"] = insuredDateOfBirth.HasValue ? InsuredDateOfBirth : null,
                ["InsuredGenderCodePickListDetailId"] = InsuredGenderCodePickListDetailId,
                ["CedantId"] = CedantId,
                ["PolicyNumber"] = PolicyNumber,
                ["FlatExtraAmountOffered"] = flatExtraAmountOffered.HasValue ? FlatExtraAmountOffered : null,
                ["FlatExtraDuration"] = FlatExtraDuration,
                ["BenefitCode"] = BenefitCode,
                ["SumAssuredOffered"] = sumAssuredOffered.HasValue ? SumAssuredOffered : null,
                ["EwarpActionCode"] = EwarpActionCode,
                ["UwRatingOffered"] = uwRatingOffered.HasValue ? UwRatingOffered : null,
                ["OfferLetterSentDate"] = offerLetterSentDate.HasValue ? OfferLetterSentDate : null,
                ["UwOpinion"] = UwOpinion,
                ["Remark"] = Remark,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["SortOrder"] = SortOrder,

                ["UploadCreatedAt"] = UploadCreatedAt,
                ["UploadFileName"] = UploadFileName,
                ["UploadCreatedById"] = UploadSubmittedBy,
                ["UploadStatus"] = UploadStatus,
                ["UploadSortOrder"] = UploadSortOrder,
                ["ActiveTab"] = ActiveTab,
            };
            ViewBag.SortOrder = ActiveTab == 2 ? UploadSortOrder : SortOrder;
            ViewBag.SortUniqueId = GetSortParam("UniqueId");
            ViewBag.SortEwarpNumber = GetSortParam("EwarpNumber");
            ViewBag.SortInsuredDateOfBirth = GetSortParam("InsuredDateOfBirth");
            ViewBag.SortInsuredGenderCodePickListDetailId = GetSortParam("InsuredGenderCodePickListDetailId");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortFlatExtraAmountOffered = GetSortParam("FlatExtraAmountOffered");
            ViewBag.SortFlatExtraDuration = GetSortParam("FlatExtraDuration");
            ViewBag.SortSumAssuredOffered = GetSortParam("SumAssuredOffered");
            ViewBag.SortEwarpActionCode = GetSortParam("EwarpActionCode");
            ViewBag.SortUwRatingOffered = GetSortParam("UwRatingOffered");
            ViewBag.SortOfferLetterSentDate = GetSortParam("OfferLetterSentDate");

            ViewBag.UploadSortOrder = UploadSortOrder;
            ViewBag.SortUploadCreatedAt = GetSortParam("UploadCreatedAt");
            ViewBag.SortUploadFileName = GetSortParam("UploadFileName");
            ViewBag.SortUploadCreatedById = GetSortParam("UploadCreatedById");
            ViewBag.SortUploadStatus = GetSortParam("UploadStatus");

            var query = _db.FacMasterListings.Select(FacMasterListingViewModel.Expression());

            if (!string.IsNullOrEmpty(UniqueId)) query = query.Where(q => q.UniqueId == UniqueId);
            if (EwarpNumber.HasValue) query = query.Where(q => q.EwarpNumber == EwarpNumber);
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => q.InsuredName.Contains(InsuredName));
            if (insuredDateOfBirth.HasValue) query = query.Where(q => q.InsuredDateOfBirth == insuredDateOfBirth);
            if (InsuredGenderCodePickListDetailId.HasValue) query = query.Where(q => q.InsuredGenderCodePickListDetailId == InsuredGenderCodePickListDetailId);
            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => q.FacMasterListingDetails.Any(d => d.PolicyNumber == PolicyNumber));
            if (flatExtraAmountOffered.HasValue) query = query.Where(q => q.FlatExtraAmountOffered == flatExtraAmountOffered);
            if (FlatExtraDuration.HasValue) query = query.Where(q => q.FlatExtraDuration == FlatExtraDuration);
            if (!string.IsNullOrEmpty(BenefitCode)) query = query.Where(q => q.FacMasterListingDetails.Any(d => d.BenefitCode == BenefitCode));
            if (sumAssuredOffered.HasValue) query = query.Where(q => q.SumAssuredOffered == sumAssuredOffered);
            if (!string.IsNullOrEmpty(EwarpActionCode)) query = query.Where(q => q.EwarpActionCode.Contains(EwarpActionCode));
            if (uwRatingOffered.HasValue) query = query.Where(q => q.UwRatingOffered == uwRatingOffered);
            if (offerLetterSentDate.HasValue) query = query.Where(q => q.OfferLetterSentDate == offerLetterSentDate);
            if (!string.IsNullOrEmpty(UwOpinion)) query = query.Where(q => q.UwOpinion.Contains(UwOpinion));
            if (!string.IsNullOrEmpty(Remark)) query = query.Where(q => q.Remark.Contains(Remark));
            if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.FacMasterListingDetails.Any(d => d.CedingBenefitTypeCode == CedingBenefitTypeCode));

            if (SortOrder == Html.GetSortAsc("UniqueId")) query = query.OrderBy(q => q.UniqueId);
            else if (SortOrder == Html.GetSortDsc("UniqueId")) query = query.OrderByDescending(q => q.UniqueId);
            else if (SortOrder == Html.GetSortAsc("EwarpNumber")) query = query.OrderBy(q => q.EwarpNumber);
            else if (SortOrder == Html.GetSortDsc("EwarpNumber")) query = query.OrderByDescending(q => q.EwarpNumber);
            else if (SortOrder == Html.GetSortAsc("InsuredDateOfBirth")) query = query.OrderBy(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortDsc("InsuredDateOfBirth")) query = query.OrderByDescending(q => q.InsuredDateOfBirth);
            else if (SortOrder == Html.GetSortAsc("InsuredGenderCodePickListDetailId")) query = query.OrderBy(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("InsuredGenderCodePickListDetailId")) query = query.OrderByDescending(q => q.InsuredGenderCodePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("FlatExtraAmountOffered")) query = query.OrderBy(q => q.FlatExtraAmountOffered);
            else if (SortOrder == Html.GetSortDsc("FlatExtraAmountOffered")) query = query.OrderByDescending(q => q.FlatExtraAmountOffered);
            else if (SortOrder == Html.GetSortAsc("FlatExtraDuration")) query = query.OrderBy(q => q.FlatExtraDuration);
            else if (SortOrder == Html.GetSortDsc("FlatExtraDuration")) query = query.OrderByDescending(q => q.FlatExtraDuration);
            else if (SortOrder == Html.GetSortAsc("SumAssuredOffered")) query = query.OrderBy(q => q.SumAssuredOffered);
            else if (SortOrder == Html.GetSortDsc("SumAssuredOffered")) query = query.OrderByDescending(q => q.SumAssuredOffered);
            else if (SortOrder == Html.GetSortAsc("EwarpActionCode")) query = query.OrderBy(q => q.EwarpActionCode);
            else if (SortOrder == Html.GetSortDsc("EwarpActionCode")) query = query.OrderByDescending(q => q.EwarpActionCode);
            else if (SortOrder == Html.GetSortAsc("UwRatingOffered")) query = query.OrderBy(q => q.UwRatingOffered);
            else if (SortOrder == Html.GetSortDsc("UwRatingOffered")) query = query.OrderByDescending(q => q.UwRatingOffered);
            else if (SortOrder == Html.GetSortAsc("OfferLetterSentDate")) query = query.OrderBy(q => q.OfferLetterSentDate);
            else if (SortOrder == Html.GetSortDsc("OfferLetterSentDate")) query = query.OrderByDescending(q => q.OfferLetterSentDate);
            query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();

            DropDownUploadStatus();
            DropDownUser(UserBo.StatusActive);

            ViewBag.ActiveTab = ActiveTab;

            //Upload tab
            GetUploadData(UploadPage, UploadCreatedAt, UploadFileName, UploadSubmittedBy, UploadStatus, UploadSortOrder);

            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        private void GetUploadData(int? UploadPage,
            string UploadCreatedAt,
            string UploadFileName,
            int? UploadSubmittedBy,
            int? UploadStatus,
            string UploadSortOrder)
        {
            DateTime? createdAtFrom = Util.GetParseDateTime(UploadCreatedAt);
            DateTime? createdAtTo = null;

            if (createdAtFrom.HasValue)
                createdAtTo = createdAtFrom.Value.AddDays(1);

            var query = _db.FacMasterListingUpload.Select(FacMasterListingUploadViewModel.Expression());
            if (createdAtFrom.HasValue) query = query.Where(q => q.CreatedAt >= createdAtFrom && q.CreatedAt < createdAtTo);
            if (!string.IsNullOrEmpty(UploadFileName)) query = query.Where(q => q.FileName == UploadFileName);
            if (UploadSubmittedBy.HasValue) query = query.Where(q => q.CreatedById == UploadSubmittedBy);
            if (UploadStatus.HasValue) query = query.Where(q => q.Status == UploadStatus);

            if (UploadSortOrder == Html.GetSortAsc("CreatedAt")) query = query.OrderBy(q => q.CreatedAt);
            else if (UploadSortOrder == Html.GetSortDsc("CreatedAt")) query = query.OrderByDescending(q => q.CreatedAt);
            else if (UploadSortOrder == Html.GetSortAsc("FileName")) query = query.OrderBy(q => q.FileName);
            else if (UploadSortOrder == Html.GetSortDsc("FileName")) query = query.OrderByDescending(q => q.FileName);
            else if (UploadSortOrder == Html.GetSortAsc("CreatedById")) query = query.OrderBy(q => q.CreatedById);
            else if (UploadSortOrder == Html.GetSortDsc("CreatedById")) query = query.OrderByDescending(q => q.CreatedById);
            else if (UploadSortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (UploadSortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.UploadTotal = query.Count();
            ViewBag.UploadList = query.ToPagedList(UploadPage ?? 1, PageSize);
        }

        // GET: FacMasterListing/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new FacMasterListingViewModel());
        }

        // POST: FacMasterListing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FacMasterListingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = FacMasterListingService.Result();
                var mappingResult = FacMasterListingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = FacMasterListingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        FacMasterListingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create FAC Master Listing"
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

        // GET: FacMasterListing/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = FacMasterListingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(bo);
            return View(new FacMasterListingViewModel(bo));
        }

        // POST: FacMasterListing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FacMasterListingViewModel model)
        {
            var dbBo = FacMasterListingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = FacMasterListingService.Result();
                var mappingResult = FacMasterListingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = FacMasterListingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        FacMasterListingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Update FAC Master Listing"
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

        // GET: FacMasterListing/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = FacMasterListingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new FacMasterListingViewModel(bo));
        }

        // POST: FacMasterListing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, FacMasterListingViewModel model)
        {
            var bo = FacMasterListingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = FacMasterListingService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete FAC Master Listing"
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
                    string fileExtension = Path.GetExtension(upload.FileName);
                    if (fileExtension != ".csv")
                    {
                        SetErrorSessionMsg("Allowed file of type: .csv");
                        return RedirectToAction("Index");
                    }

                    var trail = GetNewTrailObject();
                    FacMasterListingUploadBo facMasterListingUploadBo = new FacMasterListingUploadBo
                    {
                        Status = FacMasterListingUploadBo.StatusPendingProcess,
                        FileName = upload.FileName,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };

                    facMasterListingUploadBo.FormatHashFileName();

                    string path = facMasterListingUploadBo.GetLocalPath();
                    Util.MakeDir(path);
                    upload.SaveAs(path);

                    FacMasterListingUploadService.Create(ref facMasterListingUploadBo, ref trail);

                    SetSuccessSessionMsg("File uploaded and pending processing.");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            string UniqueId,
            int? EwarpNumber,
            string InsuredName,
            string InsuredDateOfBirth,
            int? InsuredGenderCodePickListDetailId,
            int? CedantId,
            string PolicyNumber,
            string FlatExtraAmountOffered,
            int? FlatExtraDuration,
            string BenefitCode,
            string SumAssuredOffered,
            string EwarpActionCode,
            string UwRatingOffered,
            string OfferLetterSentDate,
            string UwOpinion,
            string Remark)
        {
            if (type == 3) // template download
            {
                var process = new ProcessFacMasterListing();
                IEnumerable<FacMasterListingBo> template = null;

                process.ExportToCsv(template);

                return File(process.FilePath, "text/csv", Path.GetFileName(process.FilePath));
            }
            else
            {
                Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
                Session["lastActivity"] = DateTime.Now.Ticks;

                dynamic Params = new ExpandoObject();
                Params.Type = type;
                Params.UniqueId = UniqueId;
                Params.EwarpNumber = EwarpNumber;
                Params.InsuredName = InsuredName;
                Params.InsuredDateOfBirth = InsuredDateOfBirth;
                Params.InsuredGenderCodePickListDetailId = InsuredGenderCodePickListDetailId;
                Params.CedantId = CedantId;
                Params.PolicyNumber = PolicyNumber;
                Params.FlatExtraAmountOffered = FlatExtraAmountOffered;
                Params.FlatExtraDuration = FlatExtraDuration;
                Params.BenefitCode = BenefitCode;
                Params.SumAssuredOffered = SumAssuredOffered;
                Params.EwarpActionCode = EwarpActionCode;
                Params.UwRatingOffered = UwRatingOffered;
                Params.OfferLetterSentDate = OfferLetterSentDate;
                Params.UwOpinion = UwOpinion;
                Params.Remark = Remark;

                var export = new GenerateExportData();
                ExportBo exportBo = export.CreateExportFacMasterListing(AuthUserId, Params);

                if (exportBo.Total <= export.ExportInstantDownloadLimit)
                {
                    export.Process(ref exportBo, _db);
                    Session.Add("DownloadExport", true);
                }

                return RedirectToAction("Edit", "Export", new { exportBo.Id });
            }
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownInsuredGenderCode();
            DropDownCedant(emptyValue: "");
            SetViewBagMessage();
        }

        public void LoadPage(FacMasterListingBo facMasterListingBo = null)
        {
            DropDownInsuredGenderCode();
            GetBenefitCodes();
            GetCedingBenefitTypeCode();

            if (facMasterListingBo == null)
            {
                // Create
                DropDownBenefit(BenefitBo.StatusActive);
                DropDownCedant(CedantBo.StatusActive, null, "");
            }
            else
            {
                // Edit
                if (!string.IsNullOrEmpty(facMasterListingBo.BenefitCode))
                {
                    string[] benefitCodes = facMasterListingBo.BenefitCode.ToArraySplitTrim();
                    foreach (string benefitCodeStr in benefitCodes)
                    {
                        var benefitCode = BenefitService.FindByCode(benefitCodeStr);
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

                DropDownCedant(CedantBo.StatusActive, facMasterListingBo.CedantId, "");

                if (facMasterListingBo.CedantBo != null && facMasterListingBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
            }
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownUploadStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, FacMasterListingUploadBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = FacMasterListingUploadBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.UploadStatusItems = items;
            return items;
        }

        public void DownloadError(int id)
        {
            try
            {
                var rtmuBo = FacMasterListingUploadService.Find(id);
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine(rtmuBo.Errors.Replace(",", Environment.NewLine));
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping("FacMasterListingUploadError");
                Response.AddHeader("Content-Disposition", "attachment; filename=FacMasterListingUploadError.txt");
                Response.BinaryWrite(bytes);
                //Response.TransmitFile(filePath);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            catch
            {
                Response.Flush();
                Response.Close();
                Response.End();
            }
        }
    }
}
