using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.ProcessDatas;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Claims;
using Services.Identity;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimDataController : BaseController
    {
        public const string Controller = "ClaimData";

        // GET: ClaimData
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(int? CedantId, string UploadDate, string TreatyId, string Quarter, string PersonInCharge, int? Status, string SortOrder, int? Page)
        {
            // TODO: Add No of Records
            IndexPage();

            DateTime? uploadDate = Util.GetParseDateTime(UploadDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["UploadDate"] = uploadDate.HasValue ? UploadDate : null,
                ["TreatyId"] = TreatyId,
                ["Quarter"] = Quarter,
                ["PersonInCharge"] = PersonInCharge,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortUploadDate = GetSortParam("UploadDate");
            ViewBag.SortTreatyId = GetSortParam("TreatyId");
            ViewBag.SortQuarter = GetSortParam("Quarter");
            ViewBag.SortPersonInCharge = GetSortParam("PersonInCharge");

            var query = _db.GetClaimDataBatches().Select(ClaimDataBatchViewModel.Expression());
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (uploadDate.HasValue) query = query.Where(q => q.UploadDate >= uploadDate);
            if (!string.IsNullOrEmpty(TreatyId))
            {
                string[] treatyIds = TreatyId.Split(',');
                query = query.Where(q => treatyIds.Contains(q.TreatyId.ToString()));
            }
            if (!string.IsNullOrEmpty(Quarter)) query = query.Where(q => q.Quarter == Quarter);
            if (Status != null) query = query.Where(q => q.Status == Status);
            if (!string.IsNullOrEmpty(PersonInCharge)) query = query.Where(q => q.PersonInCharge.FullName.Contains(PersonInCharge));

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("UploadDate")) query = query.OrderBy(q => q.UploadDate);
            else if (SortOrder == Html.GetSortDsc("UploadDate")) query = query.OrderByDescending(q => q.UploadDate);
            else if (SortOrder == Html.GetSortAsc("TreatyId")) query = query.OrderBy(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortDsc("TreatyId")) query = query.OrderByDescending(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortAsc("Quarter")) query = query.OrderBy(q => q.Quarter);
            else if (SortOrder == Html.GetSortDsc("Quarter")) query = query.OrderByDescending(q => q.Quarter);
            else if (SortOrder == Html.GetSortAsc("PersonInCharge")) query = query.OrderBy(q => q.PersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("PersonInCharge")) query = query.OrderByDescending(q => q.PersonInCharge.FullName);
            else query = query.OrderBy(q => q.Quarter);

            ViewBag.Total = query.Count();
            CheckWorkgroupPower();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            if (!CheckWorkgroupPower())
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Index");
            }

            var model = new ClaimDataBatchViewModel
            {
                PersonInChargeId = AuthUserId,
                PersonInChargeBo = UserService.Find(AuthUserId)
            };
            LoadPage(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimDataBatchViewModel model)
        {
            if (!CheckWorkgroupPower())
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "create"));
                return RedirectToAction("Index");
            }

            Dictionary<string, string> errors = model.GetOverrideProperties(form);
            if (errors.Count() != 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (ModelState.IsValid)
            {
                var claimDataBatchBo = new ClaimDataBatchBo
                {
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId
                };

                model.Status = ClaimDataBatchBo.StatusPending;
                model.Get(ref claimDataBatchBo);

                var trail = GetNewTrailObject();
                Result = ClaimDataBatchService.Create(ref claimDataBatchBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = claimDataBatchBo.Id;
                    model.Status = claimDataBatchBo.Status;
                    model.ProcessStatusHistory(form, AuthUserId, ref trail);
                    if (model.Upload != null)
                        model.ProcessFileUpload(AuthUserId, ref trail);

                    if (claimDataBatchBo.SoaDataBatchId.HasValue)
                    {
                        SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(model.SoaDataBatchId.Value);
                        if (soaDataBatchBo != null)
                        {
                            soaDataBatchBo.ClaimDataBatchId = claimDataBatchBo.Id;
                            SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                        }
                    }

                    CreateTrail(claimDataBatchBo.Id, "Create Claim Data Batch");

                    SetCreateSuccessMessage("Claim Data Batch");
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                AddResult(Result);
            }
            model.Upload = null;
            model.PersonInChargeBo = UserService.Find(AuthUserId);
            LoadPage(model, true);
            return View(model);
        }

        public ActionResult Edit(
            int id,
            string MlreEventCode,
            string ClaimCode,
            string PolicyNumber,
            string TreatyType,
            string InsuredName,
            int? InsuredGenderCodeId,
            string Layer1SumRein,
            int? MappingStatus,
            int? PreComputationStatus,
            int? PreValidationStatus,
            string SortOrder,
            int? Page
        )
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var claimDataBatchBo = ClaimDataBatchService.Find(id);
            if (claimDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ClaimDataBatchViewModel(claimDataBatchBo);
            LoadPage(model);
            LoadClaimDataList(
                id,
                Page,
                MlreEventCode,
                ClaimCode,
                PolicyNumber,
                TreatyType,
                InsuredName,
                InsuredGenderCodeId,
                Layer1SumRein,
                MappingStatus,
                PreComputationStatus,
                PreValidationStatus,
                SortOrder
            );

            return View(model);
        }

        // POST: ClaimData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, FormCollection form, ClaimDataBatchViewModel model)
        {
            var claimDataBatchBo = ClaimDataBatchService.Find(id);
            if (claimDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(claimDataBatchBo.CedantId) || !CheckWorkgroupPower(model.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("Edit", new { id });
            }


            Dictionary<string, string> errors = new Dictionary<string, string>();
            bool statusChanged = false;
            int originalStatus = claimDataBatchBo.Status;
            // Status Changed
            if (model.Status != claimDataBatchBo.Status)
            {
                if (model.Status == ClaimDataBatchBo.StatusSubmitForReportClaim && model.Upload != null && model.Upload[0] != null)
                {
                    model.Status = claimDataBatchBo.Status;
                    ModelState.AddModelError("", "You cannot upload a new file and submit for reporting claim");
                }
                else
                {
                    if (claimDataBatchBo.ClaimDataConfigBo.Status != ClaimDataConfigBo.StatusApproved)
                        model.Status = claimDataBatchBo.Status;
                    else
                        statusChanged = true;
                }

                if (model.Status == ClaimDataBatchBo.StatusSubmitForProcessing)
                    errors = model.GetOverrideProperties(form);
                else
                    model.OverridePropertiesStr = claimDataBatchBo.OverrideProperties;
            }
            else
            {
                if (model.Status == ClaimDataBatchBo.StatusSuccess && model.Upload != null && model.Upload[0] != null)
                {
                    model.Status = ClaimDataBatchBo.StatusPending;
                    statusChanged = true;
                }
                errors = model.GetOverrideProperties(form);
            }

            if (errors.Count() > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                int? oldSoaDataBatchId = claimDataBatchBo.SoaDataBatchId;
                model.Get(ref claimDataBatchBo);
                claimDataBatchBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = ClaimDataBatchService.Update(ref claimDataBatchBo, ref trail);
                if (Result.Valid)
                {
                    if (statusChanged)
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);
                    model.ProcessExistingFile(AuthUserId, ref trail);
                    if (claimDataBatchBo.CanUploadFile && model.Upload != null)
                        model.ProcessFileUpload(AuthUserId, ref trail);

                    // If Soa Data Batch changed
                    if (oldSoaDataBatchId != claimDataBatchBo.SoaDataBatchId)
                    {
                        if (oldSoaDataBatchId.HasValue)
                        {
                            SoaDataBatchBo oldSoaDataBatchBo = SoaDataBatchService.Find(oldSoaDataBatchId.Value);
                            if (oldSoaDataBatchBo != null && oldSoaDataBatchBo.ClaimDataBatchId == model.Id)
                            {
                                oldSoaDataBatchBo.ClaimDataBatchId = null;
                                SoaDataBatchService.Update(ref oldSoaDataBatchBo, ref trail);
                            }
                        }

                        if (model.SoaDataBatchId.HasValue)
                        {
                            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(model.SoaDataBatchId.Value);
                            if (soaDataBatchBo != null)
                            {
                                soaDataBatchBo.ClaimDataBatchId = model.Id;
                                SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                            }
                        }
                    }

                    CreateTrail(claimDataBatchBo.Id, "Update Claim Data Batch");

                    switch (claimDataBatchBo.Status)
                    {
                        case ClaimDataBatchBo.StatusSubmitForProcessing:
                            SetSuccessSessionMsg(MessageBag.ClaimDataSubmittedForProcessing);
                            break;
                        case ClaimDataBatchBo.StatusSubmitForReportClaim:
                            SetSuccessSessionMsg(MessageBag.ClaimDataSubmittedForReportClaim);
                            break;
                        default:
                            SetUpdateSuccessMessage("Claim Data Batch");
                            break;
                    }
                    return RedirectToAction("Edit", new { Id = id });
                }
                AddResult(Result);
            }

            model.Status = originalStatus;
            model.Upload = null;
            model.SetBos(claimDataBatchBo);
            LoadPage(model, true);
            LoadClaimDataList(id, Page);
            return View(model);
        }

        // GET: ClaimData/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(id);
            if (claimDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            else if (!CheckWorkgroupPower(claimDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Edit", new { id });
            }
            return View(new ClaimDataBatchViewModel(claimDataBatchBo));
        }

        // POST: ClaimData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(id);
            if (claimDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            else if (!CheckWorkgroupPower(claimDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Edit", new { id });
            }

            if (!ClaimDataBatchBo.CanDelete(claimDataBatchBo.Status))
            {
                SetErrorSessionMsg("This Claim Data Batch has already been Reported");
                return RedirectToAction("Delete", new { id = claimDataBatchBo.Id });
            }

            claimDataBatchBo.Status = ClaimDataBatchBo.StatusPendingDelete;
            claimDataBatchBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = ClaimDataBatchService.Update(ref claimDataBatchBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(claimDataBatchBo.Id, "Update Claim Data Batch");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = claimDataBatchBo.Id });
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int Id,
            string MlreEventCode,
            string ClaimCode,
            string PolicyNumber,
            string TreatyType,
            string InsuredName,
            int? InsuredGenderCodeId,
            string Layer1SumRein,
            int? MappingStatus,
            int? PreComputationStatus,
            int? PreValidationStatus)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = Id;
            Params.MlreEventCode = MlreEventCode;
            Params.ClaimCode = ClaimCode;
            Params.PolicyNumber = PolicyNumber;
            Params.TreatyType = TreatyType;
            Params.InsuredName = InsuredName;
            Params.Layer1SumRein = Layer1SumRein;
            if (InsuredGenderCodeId != null)
            {
                Params.InsuredGenderCode = PickListDetailService.Find(InsuredGenderCodeId)?.Code;
            }
            Params.MappingStatus = MappingStatus;
            Params.PreComputationStatus = PreComputationStatus;
            Params.PreValidationStatus = PreValidationStatus;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportClaimData(Id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public void IndexPage()
        {
            ViewBag.CutOffReadOnly = CheckCutOffReadOnly(Controller);

            DropDownEmpty();
            DropDownCedant();
            DropDownStatus();
            SetViewBagMessage();
        }

        public void LoadPage(ClaimDataBatchViewModel model, bool isReload = false)
        {
            int? selectedCedantId = model.Id == 0 ? (int?)null : model.CedantId;

            DropDownEmpty();
            DropDownCedant(CedantBo.StatusActive, selectedCedantId, checkWorkgroup: true);
            DropDownCurrencyCode();
            DropDownDelimiter();
            DropDownYesNo();
            DropDownClaimTransactionType();
            DropDownStatus();

            // For filter Claim Data
            DropDownInsuredGenderCode();
            DropDownMappingStatus();
            DropDownPreComputationStatus();
            DropDownPreValidationStatus();
            DropDownReportingStatus();

            model.GetOverrideProperties();
            OverrideDropDownItems(model);
            ViewBag.AuthUserName = AuthUser.FullName;

            if (model.Id != 0)
            {
                IsDisabled(model.Status);
                LoadChildItems(model, isReload);

                ViewBag.CanProcess = true;
                if (model.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
                if (model.ClaimDataConfigBo.Status != ClaimDataConfigBo.StatusApproved)
                {
                    AddWarningMsg(string.Format(MessageBag.ClaimDataConfigStatusInactive, ClaimDataConfigBo.GetStatusName(model.ClaimDataConfigBo.Status)));
                    ViewBag.CanProcess = false;
                }
            }
            CheckWorkgroupPower(model.CedantId);
            SetViewBagMessage();
        }

        public void LoadClaimDataFilePage(ClaimDataFileViewModel model)
        {
            int? selectedCedantId = model.Id == 0 ? (int?)null : model.CedantId;

            DropDownEmpty();
            DropDownCedant(CedantBo.StatusActive, selectedCedantId, checkWorkgroup: true);
            DropDownCurrencyCode();
            DropDownDelimiter();
            DropDownYesNo();
            CheckWorkgroupPower(model.CedantId);

            model.GetOverrideProperties();
            OverrideDropDownItems(model);

            SetViewBagMessage();
        }

        public void LoadChildItems(ClaimDataBatchViewModel model, bool isReload = false)
        {
            ViewBag.ClaimDataFileBos = model.GetExistingFile(isReload);
            ViewBag.ClaimDataBatchStatusFileBos = ClaimDataBatchStatusFileService.GetByClaimDataBatchId(model.Id);
            ViewBag.StatusHistoryBos = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(model.ModuleId, model.Id);
            ViewBag.RemarkBos = RemarkService.GetByModuleIdObjectId(model.ModuleId, model.Id);
        }

        public void LoadClaimDataList(
            int claimDataBatchId,
            int? Page,
            string MlreEventCode = null,
            string ClaimCode = null,
            string PolicyNumber = null,
            string TreatyType = null,
            string InsuredName = null,
            int? InsuredGenderCodeId = null,
            string Layer1SumRein = null,
            int? MappingStatus = null,
            int? PreComputationStatus = null,
            int? PreValidationStatus = null,
            string SortOrder = null)
        {
            double? layer1SumRein = Util.StringToDouble(Layer1SumRein);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["MlreEventCode"] = MlreEventCode,
                ["ClaimCode"] = ClaimCode,
                ["PolicyNumber"] = PolicyNumber,
                ["TreatyType"] = TreatyType,
                ["InsuredName"] = InsuredName,
                ["InsuredGenderCodeId"] = InsuredGenderCodeId,
                ["Layer1SumRein"] = layer1SumRein.HasValue ? Layer1SumRein : null,
                ["MappingStatus"] = MappingStatus,
                ["PreComputationStatus"] = PreComputationStatus,
                ["PreValidationStatus"] = PreValidationStatus,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortClaimId = GetSortParam("ClaimId");
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortTreatyType = GetSortParam("TreatyType");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortLayer1SumRein = GetSortParam("Layer1SumRein");

            _db.Database.CommandTimeout = 0; // Mass data (e.g. 2.7m records)

            var query = _db.ClaimData.Where(q => q.ClaimDataBatchId == claimDataBatchId).Select(ClaimDataListViewModel.Expression());
            if (!string.IsNullOrEmpty(MlreEventCode)) query = query.Where(q => !string.IsNullOrEmpty(q.MlreEventCode) && q.MlreEventCode.Contains(MlreEventCode));
            if (!string.IsNullOrEmpty(ClaimCode)) query = query.Where(q => !string.IsNullOrEmpty(q.ClaimCode) && q.ClaimCode.Contains(ClaimCode));
            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(TreatyType)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyType) && q.TreatyType.Contains(TreatyType));
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
            if (InsuredGenderCodeId != null)
            {
                var pickListDetailBo = PickListDetailService.Find(InsuredGenderCodeId);
                if (pickListDetailBo != null) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredGenderCode) && q.InsuredGenderCode == pickListDetailBo.Code);
            }
            if (layer1SumRein.HasValue) query = query.Where(q => q.Layer1SumRein == layer1SumRein);
            if (MappingStatus != null) query = query.Where(q => q.PreComputationStatus == MappingStatus);
            if (PreComputationStatus != null) query = query.Where(q => q.PreComputationStatus == PreComputationStatus);
            if (PreValidationStatus != null) query = query.Where(q => q.PreValidationStatus == PreValidationStatus);

            if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("TreatyType")) query = query.OrderBy(q => q.TreatyType);
            else if (SortOrder == Html.GetSortDsc("TreatyType")) query = query.OrderByDescending(q => q.TreatyType);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("Layer1SumRein")) query = query.OrderBy(q => q.Layer1SumRein);
            else if (SortOrder == Html.GetSortDsc("Layer1SumRein")) query = query.OrderByDescending(q => q.Layer1SumRein);
            else query = query.OrderBy(q => q.PolicyNumber);

            ViewBag.ClaimDataTotal = query.Count();
            ViewBag.ClaimDataList = query.ToPagedList(Page ?? 1, PageSize);
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataBatchBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownMappingStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataBo.MappingStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataBo.GetMappingStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownMappingStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownPreComputationStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataBo.PreComputationStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataBo.GetPreComputationStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPreComputationStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownPreValidationStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataBo.PreValidationStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataBo.GetPreValidationStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPreValidationStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownReportingStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataBo.ReportingStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataBo.GetReportingStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownReportingStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownDelimiter()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataConfigBo.MaxDelimiter; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataConfigBo.GetDelimiterName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDelimiters = items;
            return items;
        }

        public void OverrideDropDownItems(object model)
        {
            Dictionary<string, List<SelectListItem>> items = new Dictionary<string, List<SelectListItem>>();
            foreach (KeyValuePair<string, object> overrideProperty in (Dictionary<string, object>)model.GetPropertyValue("OverrideProperties"))
            {
                int type = int.Parse(overrideProperty.Key);
                int dataType = StandardClaimDataOutputBo.GetDataTypeByType(type);
                if (dataType != StandardOutputBo.DataTypeDropDown)
                    continue;

                List<SelectListItem> selectList = GetEmptyDropDownList();
                foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByStandardClaimDataOutputId(type))
                {
                    string text = string.Format("{0} - {1}", pickListDetailBo.Code, pickListDetailBo.Description);
                    string value = overrideProperty.Value?.ToString();
                    selectList.Add(new SelectListItem { Text = text, Value = pickListDetailBo.Code, Selected = pickListDetailBo.Code == value });
                }
                items[overrideProperty.Key] = selectList;
            }
            ViewBag.OverrideDropDownItems = items;
        }

        public void LoadClaimStandardOutputList()
        {
            IList<StandardClaimDataOutputBo> bos = StandardClaimDataOutputService.Get();
            Dictionary<string, List<SelectListItem>> items = new Dictionary<string, List<SelectListItem>>();

            foreach (StandardClaimDataOutputBo bo in bos.Where(q => q.DataType == StandardOutputBo.DataTypeDropDown).ToList())
            {
                List<SelectListItem> selectList = GetEmptyDropDownList();
                foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByStandardClaimDataOutputId(bo.Id))
                {
                    string text = string.Format("{0} - {1}", pickListDetailBo.Code, pickListDetailBo.Description);
                    selectList.Add(new SelectListItem { Text = text, Value = pickListDetailBo.Code });
                }
                items[bo.Id.ToString()] = selectList;
            }

            ViewBag.StandardClaimOutputDropDownItems = items;
            ViewBag.ClaimOutputList = bos;

            SetViewBagMessage();
        }

        public void IsDisabled(int status)
        {
            bool cutOffReadOnly = CheckCutOffReadOnly(Controller);
            ViewBag.Disabled = !ClaimDataBatchBo.CanProcess(status) || cutOffReadOnly;
            ViewBag.CanReportClaim = ClaimDataBatchBo.CanReportClaim(status) || !cutOffReadOnly;
            ViewBag.RemoveExistingData = status == ClaimDataBatchBo.StatusSuccess;
            ViewBag.CanDelete = ClaimDataBatchBo.CanDelete(status);
        }

        public ActionResult EditClaimData(int id, int claimDataBatchId)
        {
            ClaimDataBo claimDataBo;
            ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(claimDataBatchId);
            bool checkCutOffReadOnly = CheckCutOffReadOnly(Controller);
            bool checkWorkgroup = CheckWorkgroupPower(claimDataBatchBo.CedantId);

            ViewBag.CustomFields = "";
            if (id != 0)
            {
                claimDataBo = ClaimDataService.Find(id);
                if (claimDataBo == null)
                {
                    return RedirectToAction("Edit", new { Id = claimDataBatchId });
                }

                if (!string.IsNullOrEmpty(claimDataBo.CustomField))
                {
                    ViewBag.CustomFields = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimDataBo.CustomField);
                }

                var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimDataBo.Errors ?? "");
                if (errors != null)
                {
                    errors.TryGetValue("PreValidationError", out dynamic preValidationError);
                    if (preValidationError is string)
                    {
                        ViewBag.PreValidationError = preValidationError;
                    }
                    else if (preValidationError is IEnumerable preValidationErrors)
                    {
                        List<string> s = new List<string> { };
                        foreach (var e in preValidationErrors)
                        {
                            if (e is string @string)
                                s.Add(@string);
                            else if (e is Newtonsoft.Json.Linq.JValue @jvalue)
                                s.Add(@jvalue.ToString());
                        }
                        ViewBag.PreValidationError = string.Join("\n", s.ToArray());
                    }
                }
                ViewBag.Errors = errors;
            }
            else
            {
                if (!checkWorkgroup)
                {
                    SetErrorSessionMsg(MessageBag.AccessDenied);
                    return RedirectToAction("Edit", new { Id = claimDataBatchId });
                }

                claimDataBo = new ClaimDataBo
                {
                    ClaimDataBatchId = claimDataBatchId
                };
            }

            claimDataBo.ClaimDataBatchBo = claimDataBatchBo;

            ViewBag.CanEdit = ClaimDataBatchBo.CanReportClaim(claimDataBatchBo.Status) && checkWorkgroup && !ClaimDataBatchBo.IsReported(claimDataBatchBo.Status) && !checkCutOffReadOnly;
            LoadClaimStandardOutputList();

            return View(new ClaimDataViewModel(claimDataBo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClaimData(int id, ClaimDataViewModel model, FormCollection form)
        {
            ClaimDataBo claimDataBo;
            if (id != 0)
            {
                claimDataBo = ClaimDataService.Find(id);
                if (claimDataBo == null)
                {
                    return RedirectToAction("Edit", new { Id = model.ClaimDataBatchId });
                }
            }
            else
            {
                claimDataBo = new ClaimDataBo
                {
                    ClaimDataBatchId = model.ClaimDataBatchId
                };
            }

            ClaimDataBatchBo claimDataBatchBo = ClaimDataBatchService.Find(claimDataBo.ClaimDataBatchId);
            bool checkCutOffReadOnly = CheckCutOffReadOnly(Controller);
            bool checkWorkgroup = CheckWorkgroupPower(claimDataBatchBo.CedantId);
            if (!checkWorkgroup)
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("EditClaimData", new { id });
            }

            if (ClaimDataBatchBo.IsReported(claimDataBatchBo.Status))
            {
                SetErrorSessionMsg("This Claim Data has already been reported");
                return RedirectToAction("EditClaimData", new { id });
            }

            foreach (var property in typeof(ClaimDataViewModel).GetProperties())
            {
                string propertyName = property.Name;
                if (propertyName.Contains("Status") || propertyName == "Errors")
                    continue;

                object value = model.GetPropertyValue(propertyName);

                claimDataBo.SetPropertyValue(propertyName, value);
            }

            TrailObject trail = GetNewTrailObject();
            claimDataBo.CreatedById = AuthUserId;
            claimDataBo.UpdatedById = AuthUserId;
            claimDataBo.ClaimDataBatchId = model.ClaimDataBatchId;

            Result = ClaimDataService.Save(ref claimDataBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(claimDataBo.Id, string.Format("{0} Claim Data Details", id == 0 ? "Create" : "Update"));
                SetCreateSuccessMessage("Claim Data Details");

                return RedirectToAction("EditClaimData", new { id = claimDataBo.Id, claimdataBatchId = claimDataBo.ClaimDataBatchId });
            }

            AddResult(Result);
            ViewBag.CanEdit = ClaimDataBatchBo.CanReportClaim(claimDataBo.ClaimDataBatchBo.Status) && checkWorkgroup && !checkCutOffReadOnly;
            LoadClaimStandardOutputList();

            return View(model);
        }

        public ActionResult DeleteClaimData(int id)
        {
            SetViewBagMessage();
            ClaimDataBo claimDataBo = ClaimDataService.Find(id);
            if (claimDataBo == null)
            {
                return RedirectToAction("Index");
            }

            claimDataBo.ClaimDataBatchBo = ClaimDataBatchService.Find(claimDataBo.ClaimDataBatchId);
            if (!CheckWorkgroupPower(claimDataBo.ClaimDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDenied));
                return RedirectToAction("EditClaimData", new { id = claimDataBo.Id, claimdataBatchId = claimDataBo.ClaimDataBatchId });
            }

            return View(new ClaimDataViewModel(claimDataBo));
        }

        [HttpPost]
        public ActionResult DeleteClaimData(int id, ClaimDataViewModel model)
        {
            ClaimDataBo claimDataBo = ClaimDataService.Find(id);
            if (claimDataBo == null)
            {
                return RedirectToAction("Index");
            }

            claimDataBo.ClaimDataBatchBo = ClaimDataBatchService.Find(claimDataBo.ClaimDataBatchId);
            if (!CheckWorkgroupPower(claimDataBo.ClaimDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDenied));
                return RedirectToAction("EditClaimData", new { id = claimDataBo.Id, claimdataBatchId = claimDataBo.ClaimDataBatchBo.Id });
            }

            TrailObject trail = GetNewTrailObject();
            Result = ClaimDataService.Delete(claimDataBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    claimDataBo.Id,
                    "Delete Claim Data"
                );
            }
            else
            {
                if (Result.MessageBag.Errors.Count > 1)
                    SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
                else
                    SetErrorSessionMsg(Result.MessageBag.Errors[0]);
                return RedirectToAction("DeleteClaimData", new { id });
            }

            SetDeleteSuccessMessage("Claim Data");
            return RedirectToAction("Edit", new { id = claimDataBo.ClaimDataBatchId });
        }

        public ActionResult DownloadClaimDataFile(int rawFileId)
        {
            RawFileBo rawFileBo = RawFileService.Find(rawFileId);
            DownloadFile(rawFileBo.GetLocalPath(), rawFileBo.FileName);
            return null;
        }

        public ActionResult EditClaimDataFile(int id)
        {
            ClaimDataFileBo claimDataFileBo = ClaimDataFileService.Find(id);
            if (claimDataFileBo == null)
            {
                return RedirectToAction("Index");
            }

            ClaimDataFileViewModel model = new ClaimDataFileViewModel(claimDataFileBo);
            CheckCutOffReadOnly(Controller);
            LoadClaimDataFilePage(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClaimDataFile(int id, FormCollection form, ClaimDataFileViewModel model)
        {
            CheckCutOffReadOnly(Controller);

            ClaimDataFileBo claimDataFileBo = ClaimDataFileService.Find(id);
            if (claimDataFileBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(claimDataFileBo.ClaimDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("EditClaimDataFile", new { id });
            }

            int configStatus = claimDataFileBo.ClaimDataBatchBo.ClaimDataConfigBo.Status;
            if (claimDataFileBo.ClaimDataBatchBo.CanUpdateDataFile && configStatus == ClaimDataConfigBo.StatusApproved)
            {
                Dictionary<string, string> errors = model.GetOverrideProperties(form);
                if (errors.Count() != 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }

                if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
                {
                    model.Get(ref claimDataFileBo);
                    claimDataFileBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();
                    Result = ClaimDataFileService.Update(ref claimDataFileBo, ref trail);
                    if (Result.Valid)
                    {
                        CreateTrail(
                            claimDataFileBo.Id,
                            "Update Claim Data File"
                        );

                        SetUpdateSuccessMessage("Claim Data File");
                        return RedirectToAction("EditClaimDataFile", new { id });
                    }
                    AddResult(Result);
                }
            }

            LoadClaimDataFilePage(model);

            model.CedantBo = CedantService.Find(model.CedantId);
            model.TreatyBo = TreatyService.Find(model.TreatyId);
            model.PersonInChargeBo = UserService.Find(model.PersonInChargeId);

            return View(model);
        }

        public void DownloadFile(string filePath, string fileName)
        {
            try
            {
                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(filePath);
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

        [HttpPost]
        public JsonResult GetClaimDataConfigByCedant(int cedantId, int claimDataConfigId)
        {
            IList<ClaimDataConfigBo> claimDataConfigBos = ClaimDataConfigService.GetByCedantIdStatus(cedantId, ClaimDataConfigBo.StatusApproved, claimDataConfigId);
            return Json(new { claimDataConfigBos });
        }

        [HttpPost]
        public JsonResult GetClaimDataConfig(int? id)
        {
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(id);
            return Json(new { claimDataConfigBo });
        }
    }
}