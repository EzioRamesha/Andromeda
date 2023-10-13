using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.ProcessDatas;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class RiDataController : BaseController
    {
        public const string Controller = "RiData";

        // GET: RiData
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? StatusId,
            int? ProcessWarehouseStatus,
            int? CedantId,
            string TreatyId,
            string UploadDate,
            string Quarter,
            string PersonInCharge,
            string SortOrder,
            int? Page
        )
        {
            IndexPage();

            DateTime? uploadDate = Util.GetParseDateTime(UploadDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["StatusId"] = StatusId,
                ["ProcessWarehouseStatus"] = ProcessWarehouseStatus,
                ["CedantId"] = CedantId,
                ["TreatyId"] = TreatyId,
                ["UploadDate"] = uploadDate.HasValue ? UploadDate : null,
                ["Quarter"] = Quarter,
                ["PersonInCharge"] = PersonInCharge,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortUploadDate = GetSortParam("UploadDate");
            ViewBag.SortTreatyId = GetSortParam("TreatyId");
            ViewBag.SortQuarter = GetSortParam("Quarter");
            ViewBag.SortPersonInCharge = GetSortParam("PersonInCharge");

            var query = _db.GetRiDataBatches().Select(RiDataBatchViewModel.Expression());
            if (StatusId != null) query = query.Where(q => q.Status == StatusId);
            if (ProcessWarehouseStatus != null) query = query.Where(q => q.ProcessWarehouseStatus == ProcessWarehouseStatus);
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(TreatyId))
            {
                string[] TreatyIds = TreatyId.Split(',');
                query = query.Where(q => TreatyIds.Contains(q.TreatyId.ToString()));
            }
            if (!string.IsNullOrEmpty(Quarter)) query = query.Where(q => q.Quarter == Quarter);
            if (uploadDate.HasValue) query = query.Where(q => q.UploadDate >= uploadDate);
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

        // GET: RiData/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            if (!CheckWorkgroupPower())
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Index");
            }

            var model = new RiDataBatchViewModel
            {
                PersonInChargeId = AuthUserId,
                PersonInChargeBo = UserService.Find(AuthUserId)
            };

            LoadPage();
            ListOverrideProperties("");
            return View(model);
        }

        // POST: RiData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RiDataBatchViewModel model)
        {
            if (!CheckWorkgroupPower())
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "create"));
                return RedirectToAction("Index");
            }

            DisableSessionTimeout();

            Result childResult = new Result();
            string overridePropsStr = model.GetOverrideProperties(form, ref childResult);

            if (ModelState.IsValid)
            {
                RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(model.ConfigId);
                RiDataBatchBo riDataBatchBo = new RiDataBatchBo
                {
                    CedantId = model.CedantId,
                    TreatyId = model.TreatyId,
                    RiDataConfigId = model.ConfigId,
                    RiDataFileConfig = new RiDataFileConfig
                    {
                        HasHeader = model.HasHeader,
                        HeaderRow = model.HeaderRow,
                        Worksheet = model.Worksheet,
                        Delimiter = model.Delimiter,
                        DelimiterName = RiDataConfigBo.GetDelimiterName(model.Delimiter),
                        RemoveQuote = riDataConfigBo.RiDataFileConfig.RemoveQuote,
                        StartRow = model.StartRow,
                        EndRow = model.EndRow,
                        StartColumn = model.StartColumn,
                        EndColumn = model.EndColumn,
                        IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping,
                        NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping,
                        IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection,
                    },
                    Status = RiDataBatchBo.StatusPending,
                    Quarter = model.Quarter,
                    RecordType = model.RecordType,
                    ReceivedAt = DateTime.Parse(model.ReceivedAtStr),
                    SoaDataBatchId = model.SoaDataBatchId,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                    OverrideProperties = overridePropsStr,
                };

                if (childResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = RiDataBatchService.Create(ref riDataBatchBo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = riDataBatchBo.Id;
                        model.Status = riDataBatchBo.Status;
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(riDataBatchBo.Id, "Create RI Data Batch");
                    }

                    if (model.Upload != null)
                    {
                        foreach (var uploadItem in model.Upload)
                        {
                            if (uploadItem != null)
                            {
                                RawFileBo rawFileBo = new RawFileBo
                                {
                                    Type = RawFileBo.TypeRiData,
                                    Status = RawFileBo.StatusPending,
                                    FileName = uploadItem.FileName,
                                    CreatedById = AuthUserId,
                                    UpdatedById = AuthUserId,
                                };

                                rawFileBo.FormatHashFileName();

                                string path = rawFileBo.GetLocalPath();
                                Util.MakeDir(path);
                                uploadItem.SaveAs(path);

                                trail = GetNewTrailObject();
                                Result = RawFileService.Create(ref rawFileBo, ref trail);
                                if (Result.Valid)
                                {
                                    CreateTrail(rawFileBo.Id, "Create Raw File");
                                }

                                RiDataFileBo riDataFileBo = new RiDataFileBo
                                {
                                    RiDataBatchId = model.Id,
                                    RawFileId = rawFileBo.Id,
                                    TreatyId = model.TreatyId,
                                    RiDataConfigId = model.ConfigId,
                                    RiDataFileConfig = riDataBatchBo.RiDataFileConfig,
                                    OverrideProperties = overridePropsStr,
                                    Mode = RiDataFileBo.ModeInclude,
                                    Status = RiDataFileBo.StatusPending,
                                    RecordType = model.RecordType,
                                    CreatedById = AuthUserId,
                                    UpdatedById = AuthUserId,
                                };


                                trail = GetNewTrailObject();
                                Result = RiDataFileService.Create(ref riDataFileBo, ref trail);
                                if (Result.Valid)
                                {
                                    CreateTrail(riDataFileBo.Id, "Create RI Data File");
                                }
                            }
                        }
                    }

                    if (model.SoaDataBatchId.HasValue)
                    {
                        SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(model.SoaDataBatchId.Value);
                        if (soaDataBatchBo != null)
                        {
                            soaDataBatchBo.RiDataBatchId = model.Id;
                            SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                        }
                    }

                    EnableSessionTimeout();

                    SetCreateSuccessMessage("RI Data Batch");
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                else
                {
                    Result = childResult;
                }
                AddResult(Result);
            }

            model.Upload = null;
            model.PersonInChargeBo = UserService.Find(AuthUserId);
            LoadPage();
            ListOverrideProperties(overridePropsStr);

            EnableSessionTimeout();

            return View(model);
        }

        // GET: RiData/Edit/5
        public ActionResult Edit(
            int id,
            string PolicyNumber,
            string TreatyCode,
            string InsuredName,
            int? TransactionTypeCodeId,
            int? ReinsBasicCodeId,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            int? RiskPeriodMonth,
            int? RiskPeriodYear,
            int? PreComputation1Status,
            int? PreComputation2Status,
            int? PreValidationStatus,
            string ConflictType,
            int? PostComputationStatus,
            int? PostValidationStatus,
            int? MappingStatus,
            int? FinaliseStatus,
            int? WarehouseProcessStatus,
            bool? IgnoreFinalise,
            string IgnoreFinaliseIds,
            string SortOrder,
            int? Page,
            int? tabIndex
        )
        {
            LogHelper("RI Batch " + id + " Start (GET)" + HttpContext.Request.RawUrl);

            LogHelper("RI Batch " + id + " Start (GET)Check User Power for Read & Update");
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();
            LogHelper("RI Batch " + id + " End (GET)Check User Power for Read & Update");

            LogHelper("RI Batch " + id + " Start (GET)Find RiDataBatch Bo by Id");
            RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(id);
            if (riDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            LogHelper("RI Batch " + id + " End (GET)Find RiDataBatch Bo by Id");

            var model = new RiDataBatchViewModel(riDataBatchBo);
            if (!string.IsNullOrEmpty(IgnoreFinaliseIds)) model.IgnoreFinaliseIds = IgnoreFinaliseIds;

            // For RiData listing table
            ListRiData(
                id,
                Page,
                PolicyNumber,
                TreatyCode,
                InsuredName,
                TransactionTypeCodeId,
                ReinsBasicCodeId,
                ReportPeriodMonth,
                ReportPeriodYear,
                RiskPeriodMonth,
                RiskPeriodYear,
                PreComputation1Status,
                PreComputation2Status,
                PreValidationStatus,
                ConflictType,
                PostComputationStatus,
                PostValidationStatus,
                MappingStatus,
                FinaliseStatus,
                WarehouseProcessStatus,
                IgnoreFinalise,
                IgnoreFinaliseIds,
                SortOrder,
                riDataBatchBo.TreatyBo.TreatyIdCode.Split(' ')[0],
                riDataBatchBo.CedantBo.Code
            );

            LogHelper("RI Batch " + id + " Start (GET)RiData File List");
            IList<RiDataFileBo> fileHistories = RiDataFileService.GetByRiDataBatchId(id, false, true);
            ViewBag.FileHistories = fileHistories;
            ViewBag.CheckedExcludes = fileHistories.Where(q => q.Mode == RiDataFileBo.ModeExclude).Select(q => q.Id).ToArray();
            LogHelper("RI Batch " + id + " End (GET)RiData File List");

            ViewBag.ActiveTab = tabIndex;
            LoadPage(riDataBatchBo);
            SetViewBagMessage();
            return View(new RiDataBatchViewModel(riDataBatchBo));
        }

        // POST: RiData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, int? Page, FormCollection form, RiDataBatchViewModel model)
        {
            RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(id);
            if (riDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(riDataBatchBo.CedantId) || !CheckWorkgroupPower(model.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("Edit", new { id });
            }

            DisableSessionTimeout();

            Result childResult = new Result();
            string overridePropertiesStr = model.GetOverrideProperties(form, ref childResult);

            if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
            {
                int? oldSoaDataBatchId = riDataBatchBo.SoaDataBatchId;

                if (riDataBatchBo.Status != RiDataBatchBo.StatusFinalised)
                {
                    riDataBatchBo.CedantId = model.CedantId;
                    riDataBatchBo.Quarter = model.Quarter;
                    riDataBatchBo.TreatyId = model.TreatyId;
                    riDataBatchBo.RiDataConfigId = model.ConfigId;
                    RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(model.ConfigId);
                    riDataBatchBo.RiDataFileConfig = new RiDataFileConfig
                    {
                        HasHeader = model.HasHeader,
                        HeaderRow = model.HeaderRow,
                        Worksheet = model.Worksheet,
                        Delimiter = model.Delimiter,
                        DelimiterName = RiDataConfigBo.GetDelimiterName(model.Delimiter),
                        RemoveQuote = riDataConfigBo.RiDataFileConfig.RemoveQuote,
                        StartRow = model.StartRow,
                        EndRow = model.EndRow,
                        StartColumn = model.StartColumn,
                        EndColumn = model.EndColumn,
                        IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping,
                        NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping,
                        IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection,
                    };
                    //bool process = false;
                    if (model.Status == RiDataBatchBo.StatusSubmitForFinalise && riDataConfigBo.Status == RiDataConfigBo.StatusApproved)
                    {
                        riDataBatchBo.Status = RiDataBatchBo.StatusSubmitForFinalise;
                    }
                    else if (model.Status == RiDataBatchBo.StatusSubmitForPreProcessing && riDataConfigBo.Status == RiDataConfigBo.StatusApproved) // for Status = (Success, Failed, Pending)
                    {
                        riDataBatchBo.Status = RiDataBatchBo.StatusSubmitForPreProcessing;
                        riDataBatchBo.OverrideProperties = overridePropertiesStr;
                        //process = true;
                    }
                    else if (model.Status == RiDataBatchBo.StatusSubmitForPostProcessing && riDataConfigBo.Status == RiDataConfigBo.StatusApproved) // for Status = (Success, Failed, Pending)
                    {
                        riDataBatchBo.Status = RiDataBatchBo.StatusSubmitForPostProcessing;
                        riDataBatchBo.OverrideProperties = overridePropertiesStr;
                        //process = true;
                    }
                    else if ((model.Status == RiDataBatchBo.StatusPreSuccess || model.Status == RiDataBatchBo.StatusPostSuccess || model.Status == RiDataBatchBo.StatusPostFailed) && (model.Upload != null && model.Upload[0] != null))
                    {
                        riDataBatchBo.Status = RiDataBatchBo.StatusPending;
                        riDataBatchBo.OverrideProperties = overridePropertiesStr;
                    }
                    else
                    {
                        riDataBatchBo.OverrideProperties = overridePropertiesStr;
                    }

                    riDataBatchBo.RecordType = model.RecordType;
                    riDataBatchBo.ReceivedAt = DateTime.Parse(model.ReceivedAtStr);
                    riDataBatchBo.SoaDataBatchId = model.SoaDataBatchId;
                }
                else if (riDataBatchBo.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusFailed && model.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessFailed)
                {
                    riDataBatchBo.ProcessWarehouseStatus = model.ProcessWarehouseStatus;
                }
                riDataBatchBo.UpdatedById = AuthUserId;

                if (childResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = RiDataBatchService.Update(ref riDataBatchBo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = id;
                        model.Status = riDataBatchBo.Status;
                        // DO NOT delete RiData
                        // Because it will catch SqlException: Execution Timeout Expired
                        // When the total numbe of record more than 1k (estimated)
                        //if (process) RiDataService.DeleteAllByRiDataBatchId(id); // DO NOT TRAIL
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(riDataBatchBo.Id, "Update RI Data Batch");
                    }

                    // If Soa Data Batch changed
                    if (oldSoaDataBatchId != model.SoaDataBatchId)
                    {
                        if (oldSoaDataBatchId.HasValue)
                        {
                            SoaDataBatchBo oldSoaDataBatchBo = SoaDataBatchService.Find(oldSoaDataBatchId.Value);
                            if (oldSoaDataBatchBo != null && oldSoaDataBatchBo.RiDataBatchId == model.Id)
                            {
                                oldSoaDataBatchBo.RiDataBatchId = null;
                                SoaDataBatchService.Update(ref oldSoaDataBatchBo, ref trail);
                            }
                        }

                        if (model.SoaDataBatchId.HasValue)
                        {
                            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(model.SoaDataBatchId.Value);
                            if (soaDataBatchBo != null)
                            {
                                soaDataBatchBo.RiDataBatchId = model.Id;
                                SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                            }
                        }
                    }
                    else
                    {
                        // Update RiDataBatchId when SOA Data didn't link to RI Data batch cause of RI Data is deleted
                        if (model.SoaDataBatchId.HasValue)
                        {
                            SoaDataBatchBo soaDataBatchBo = SoaDataBatchService.Find(model.SoaDataBatchId.Value);
                            if (soaDataBatchBo != null && SoaDataBatchService.FindByRiDataBatchId(model.Id))
                            {
                                soaDataBatchBo.RiDataBatchId = model.Id;
                                SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
                            }
                        }
                    }

                    if (model.Status == RiDataBatchBo.StatusPreSuccess ||
                        model.Status == RiDataBatchBo.StatusPreFailed ||
                        model.Status == RiDataBatchBo.StatusPending ||
                        model.Status == RiDataBatchBo.StatusPostSuccess ||
                        model.Status == RiDataBatchBo.StatusPostFailed
                    )
                    {
                        if (!string.IsNullOrEmpty(model.Mode))
                        {
                            List<int> checkExclude = model.Mode.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                            foreach (int i in checkExclude)
                            {
                                RiDataFileBo riDataFileBo = RiDataFileService.Find(i);
                                riDataFileBo.Mode = RiDataFileBo.ModeExclude;
                                riDataFileBo.CreatedById = AuthUserId;
                                riDataFileBo.UpdatedById = AuthUserId;

                                trail = GetNewTrailObject();
                                Result = RiDataFileService.Update(ref riDataFileBo, ref trail);
                                if (Result.Valid) CreateTrail(riDataFileBo.Id, "Update RI Data File");
                            }

                            IList<RiDataFileBo> riDataFileBos = RiDataFileService.GetByRiDataBatchIdExcept(id, checkExclude);
                            if (riDataFileBos.Count() > 0)
                            {
                                foreach (RiDataFileBo bo in riDataFileBos)
                                {
                                    RiDataFileBo riDataFileBo = bo;
                                    riDataFileBo.Mode = RiDataFileBo.ModeInclude;
                                    riDataFileBo.UpdatedById = AuthUserId;

                                    trail = GetNewTrailObject();
                                    Result = RiDataFileService.Update(ref riDataFileBo, ref trail);
                                    if (Result.Valid) CreateTrail(riDataFileBo.Id, "Update RI Data File");
                                }
                            }
                        }
                        else
                        {
                            IList<RiDataFileBo> riDataFileBos = RiDataFileService.GetByRiDataBatchIdMode(id, RiDataFileBo.ModeExclude);
                            if (riDataFileBos.Count() > 0)
                            {
                                foreach (RiDataFileBo bo in riDataFileBos)
                                {
                                    RiDataFileBo riDataFileBo = bo;
                                    riDataFileBo.Mode = RiDataFileBo.ModeInclude;
                                    riDataFileBo.UpdatedById = AuthUserId;

                                    trail = GetNewTrailObject();
                                    Result = RiDataFileService.Update(ref riDataFileBo, ref trail);
                                    if (Result.Valid) CreateTrail(riDataFileBo.Id, "Update RI Data File");
                                }
                            }
                        }

                        if (model.Upload != null)
                        {
                            foreach (var uploadItem in model.Upload)
                            {
                                if (uploadItem != null)
                                {
                                    RawFileBo rawFileBo = new RawFileBo
                                    {
                                        Type = RawFileBo.TypeRiData,
                                        Status = RawFileBo.StatusPending,
                                        FileName = uploadItem.FileName,
                                        CreatedById = AuthUserId,
                                        UpdatedById = AuthUserId,
                                    };

                                    rawFileBo.FormatHashFileName();

                                    string path = rawFileBo.GetLocalPath();
                                    Util.MakeDir(path);
                                    uploadItem.SaveAs(path);

                                    trail = GetNewTrailObject();
                                    Result = RawFileService.Create(ref rawFileBo, ref trail);
                                    if (Result.Valid) CreateTrail(rawFileBo.Id, "Create Raw File");

                                    RiDataFileBo riDataFileBo = new RiDataFileBo
                                    {
                                        RiDataBatchId = id,
                                        RawFileId = rawFileBo.Id,
                                        TreatyId = model.TreatyId,
                                        RiDataConfigId = model.ConfigId,
                                        RiDataFileConfig = riDataBatchBo.RiDataFileConfig,
                                        OverrideProperties = overridePropertiesStr,
                                        Mode = RiDataFileBo.ModeInclude,
                                        Status = RiDataFileBo.StatusPending,
                                        RecordType = model.RecordType,
                                        CreatedById = AuthUserId,
                                        UpdatedById = AuthUserId,
                                    };

                                    trail = GetNewTrailObject();
                                    Result = RiDataFileService.Create(ref riDataFileBo, ref trail);
                                    if (Result.Valid) CreateTrail(riDataFileBo.Id, "Create RI Data File");
                                }
                            }
                        }
                    }

                    if (model.Status == RiDataBatchBo.StatusPreSuccess || model.Status == RiDataBatchBo.StatusPostSuccess || model.Status == RiDataBatchBo.StatusFinaliseFailed)
                    {
                        model.UpdateRiData(AuthUserId);
                    }

                    if (riDataBatchBo.Status == RiDataBatchBo.StatusSubmitForPreProcessing)
                    {
                        SetSuccessSessionMsg(MessageBag.RiDataSubmittedForPreProcessing);
                    }
                    else if (riDataBatchBo.Status == RiDataBatchBo.StatusSubmitForPostProcessing)
                    {
                        SetSuccessSessionMsg(MessageBag.RiDataSubmittedForPostProcessing);
                    }
                    else if (riDataBatchBo.Status == RiDataBatchBo.StatusSubmitForFinalise)
                    {
                        SetSuccessSessionMsg(MessageBag.RiDataSubmittedForFinalise);
                    }
                    else if (riDataBatchBo.ProcessWarehouseStatus == RiDataBatchBo.ProcessWarehouseStatusProcessFailed)
                    {
                        SetSuccessSessionMsg(MessageBag.RiDataSubmittedForProcessWarehouse);
                    }
                    else
                    {
                        SetUpdateSuccessMessage("RI Data Batch");
                    }

                    EnableSessionTimeout();

                    return RedirectToAction("Edit", new { id });
                }
                else
                {
                    Result = childResult;
                }
                AddResult(Result);
            }

            IList<RiDataFileBo> fileHistories = RiDataFileService.GetByRiDataBatchId(id, false, true);
            ViewBag.FileHistories = fileHistories;
            ViewBag.CheckedExcludes = (string.IsNullOrEmpty(model.Mode) ? null : model.Mode.Split(',').Select(n => Convert.ToInt32(n)).ToArray());

            model.Upload = null;
            LoadPage(riDataBatchBo);
            ListOverrideProperties(overridePropertiesStr);
            SetViewBagMessage();

            // For RiData listing table
            ListRiData(id, Page, TreatyCodeQuery: riDataBatchBo.TreatyBo.TreatyIdCode.Split(' ')[0], TreatyCodeQuery2: riDataBatchBo.CedantBo.Code);
            LogHelper("RI Batch " + id + " End (POST)" + HttpContext.Request.RawUrl);

            EnableSessionTimeout();

            return View(new RiDataBatchViewModel(riDataBatchBo));
        }

        public ActionResult EditRiDataFile(int id)
        {
            RiDataFileBo riDataFileBo = RiDataFileService.Find(id);
            if (riDataFileBo == null)
            {
                return RedirectToAction("Index");
            }

            LoadEditFilePage(riDataFileBo);
            SetViewBagMessage();
            return View(new RiDataFileViewModel(riDataFileBo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRiDataFile(int id, FormCollection form, RiDataFileViewModel model)
        {
            RiDataFileBo riDataFileBo = RiDataFileService.Find(id);
            if (riDataFileBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(riDataFileBo.RiDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("EditRiDataFile", new { id });
            }

            int configStatus = riDataFileBo.RiDataBatchBo.RiDataConfigBo.Status;
            int status = riDataFileBo.RiDataBatchBo.Status;
            if ((status == RiDataBatchBo.StatusPending ||
                status == RiDataBatchBo.StatusPreSuccess ||
                status == RiDataBatchBo.StatusPreFailed ||
                status == RiDataBatchBo.StatusFinaliseFailed ||
                status == RiDataBatchBo.StatusPostSuccess ||
                status == RiDataBatchBo.StatusPostFailed) &&
                configStatus == RiDataConfigBo.StatusApproved)
            {

                Result childResult = new Result();
                string overridePropsString = model.GetOverrideProperties(form, ref childResult);

                if (ModelState.IsValid && !CheckCutOffReadOnly(Controller, riDataFileBo.RiDataBatchId))
                {
                    riDataFileBo.TreatyId = model.TreatyId;
                    riDataFileBo.RiDataConfigId = model.ConfigId;
                    RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(model.ConfigId);
                    riDataFileBo.RiDataFileConfig = new RiDataFileConfig
                    {
                        HasHeader = model.HasHeader,
                        HeaderRow = model.HeaderRow,
                        Worksheet = model.Worksheet,
                        Delimiter = model.Delimiter,
                        DelimiterName = RiDataConfigBo.GetDelimiterName(model.Delimiter),
                        RemoveQuote = riDataConfigBo.RiDataFileConfig.RemoveQuote,
                        StartRow = model.StartRow,
                        EndRow = model.EndRow,
                        StartColumn = model.StartColumn,
                        EndColumn = model.EndColumn,
                        IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping,
                        NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping,
                        IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection,
                    };
                    riDataFileBo.OverrideProperties = overridePropsString;
                    riDataFileBo.RecordType = model.RecordType;
                    riDataFileBo.UpdatedById = AuthUserId;

                    if (childResult.Valid)
                    {
                        TrailObject trail = GetNewTrailObject();
                        Result = RiDataFileService.Update(ref riDataFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                riDataFileBo.Id,
                                "Update RI Data File"
                            );

                            SetUpdateSuccessMessage("RI Data File");
                            return RedirectToAction("EditRiDataFile", new { id });
                        }
                    }
                    else
                    {
                        Result = childResult;
                    }
                    AddResult(Result);
                }
            }

            LoadEditFilePage(riDataFileBo);
            return View(new RiDataFileViewModel(riDataFileBo));
        }

        // GET: RiData/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(id);
            if (riDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            else if (!CheckWorkgroupPower(riDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Edit", new { id });
            }
            return View(new RiDataBatchViewModel(riDataBatchBo));
        }

        // POST: RiData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            RiDataBatchBo riDataBatchBo = RiDataBatchService.Find(id);
            if (riDataBatchBo == null)
            {
                return RedirectToAction("Index");
            }
            else if (!CheckWorkgroupPower(riDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(MessageBag.AccessDenied);
                return RedirectToAction("Edit", new { id });
            }

            riDataBatchBo.Status = RiDataBatchBo.StatusPendingDelete;
            riDataBatchBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = RiDataBatchService.Update(ref riDataBatchBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(riDataBatchBo.Id, "Update RI Data Batch");

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = riDataBatchBo.Id });
        }

        public ActionResult Details(int id, int ridataBatchId, int? originalId)
        {
            // Type = 1 (New Adjustment), 2 (Edit)            
            RiDataBo riDataBo = new RiDataBo();

            ViewBag.PreValidationError = "";
            ViewBag.PostValidationError = "";
            ViewBag.FinaliseError = "";
            ViewBag.ProcessWarehouseError = "";
            ViewBag.CustomFields = "";

            bool cutOffReadOnly = CheckCutOffReadOnly(Controller);

            if (originalId != null)
            {
                DetailsPage();
                IsEdit(riDataBo.RiDataBatchBo.Status);
                if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId) || cutOffReadOnly)
                    ViewBag.EditRiData = false;
                return View(new RiDataViewModel(riDataBo));
            }

            if (id != 0)
            {
                riDataBo = RiDataService.Find(id);
                if (riDataBo == null)
                {
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(riDataBo.Errors))
                {
                    var Errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(riDataBo.Errors);
                    Errors.TryGetValue("PreValidationError", out dynamic preValidationError);
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

                    Errors.TryGetValue("PostValidationError", out dynamic postValidationError);
                    if (postValidationError is string)
                    {
                        ViewBag.PostValidationError = postValidationError;
                    }
                    else if (postValidationError is IEnumerable postValidationErrors)
                    {
                        List<string> s = new List<string> { };
                        foreach (var e in postValidationErrors)
                        {
                            if (e is string @string)
                                s.Add(@string);
                            else if (e is Newtonsoft.Json.Linq.JValue @jvalue)
                                s.Add(@jvalue.ToString());
                        }
                        ViewBag.PostValidationError = string.Join("\n", s.ToArray());
                    }

                    Errors.TryGetValue("FinaliseError", out dynamic finaliseError);
                    if (finaliseError is string)
                    {
                        ViewBag.FinaliseError = finaliseError;
                    }
                    else if (finaliseError is IEnumerable finaliseErrors)
                    {
                        List<string> s = new List<string> { };
                        foreach (var e in finaliseErrors)
                        {
                            if (e is string @string)
                                s.Add(@string);
                            else if (e is Newtonsoft.Json.Linq.JValue @jvalue)
                                s.Add(@jvalue.ToString());
                        }
                        ViewBag.FinaliseError = string.Join("\n", s.ToArray());
                    }


                    Errors.TryGetValue("ProcessWarehouseError", out dynamic processWarehouseError);
                    if (processWarehouseError is string)
                    {
                        ViewBag.ProcessWarehouseError = processWarehouseError;
                    }
                    else if (processWarehouseError is IEnumerable processWarehouseErrors)
                    {
                        List<string> s = new List<string> { };
                        foreach (var e in processWarehouseErrors)
                        {
                            if (e is string @string)
                                s.Add(@string);
                            else if (e is Newtonsoft.Json.Linq.JValue @jvalue)
                                s.Add(@jvalue.ToString());
                        }
                        ViewBag.ProcessWarehouseError = string.Join("\n", s.ToArray());
                    }
                    ViewBag.Errors = Errors;
                }

                if (!string.IsNullOrEmpty(riDataBo.CustomField))
                {
                    var CustomFields = JsonConvert.DeserializeObject<Dictionary<string, object>>(riDataBo.CustomField);
                    ViewBag.CustomFields = CustomFields;
                }
            }
            else
            {
                riDataBo.RiDataBatchId = ridataBatchId;
                riDataBo.RiDataBatchBo = RiDataBatchService.Find(ridataBatchId);

                if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId))
                {
                    SetErrorSessionMsg(MessageBag.AccessDenied);
                    return RedirectToAction("Edit", new { id = riDataBo.RiDataBatchBo.Id });
                }

                riDataBo.PostComputationStatus = RiDataBo.PostComputationStatusPending;
                riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusPending;
            }

            DetailsPage();
            IsEdit(riDataBo.RiDataBatchBo.Status);
            if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId) || cutOffReadOnly)
                ViewBag.EditRiData = false;

            SetViewBagMessage();
            return View(new RiDataViewModel(riDataBo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id, RiDataViewModel model, FormCollection form)
        {
            RiDataBo riDataBo = new RiDataBo();

            if (id != 0)
            {
                riDataBo = RiDataService.Find(id);
                if (riDataBo == null)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                riDataBo.RiDataBatchId = model.RiDataBatchId;
                riDataBo.RiDataBatchBo = RiDataBatchService.Find(model.RiDataBatchId);
            }

            if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDeniedWithName, "update"));
                return RedirectToAction("Edit", new { id = riDataBo.RiDataBatchBo.Id });
            }

            int status = riDataBo.RiDataBatchBo.Status;
            if (status == RiDataBatchBo.StatusPreSuccess || status == RiDataBatchBo.StatusPostSuccess || status == RiDataBatchBo.StatusPostFailed || status == RiDataBatchBo.StatusFinaliseFailed)
            {
                if (ModelState.IsValid && !CheckCutOffReadOnly(Controller))
                {
                    var properties = StandardOutputBo.GetPropertyNames();

                    // Loop form collection              
                    foreach (string key in form.AllKeys)
                    {
                        // remove end of string
                        string property = (key.EndsWith("Str") ? key.Substring(0, key.LastIndexOf("Str")) : key);
                        int type = properties.FindIndex(q => q == property);
                        if (type == -1)
                            continue;

                        type += 1; // this is index and start with zero. Thus, have to add one.
                        int datatype = StandardOutputBo.GetDataTypeByType(type);
                        string value = form.Get(key);
                        object output = null;

                        if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                        {
                            riDataBo.SetPropertyValue(property, null);
                            continue;
                        }

                        switch (datatype)
                        {
                            case StandardOutputBo.DataTypeDate:
                                Util.TryParseDateTime(value, out DateTime? datetime, out string _);
                                output = datetime;
                                break;
                            case StandardOutputBo.DataTypeString:
                                output = value;
                                break;
                            case StandardOutputBo.DataTypeAmount:
                                output = Util.StringToDouble(value);
                                break;
                            case StandardOutputBo.DataTypePercentage:
                                output = Util.StringToDouble(value);
                                break;
                            case StandardOutputBo.DataTypeInteger:
                                if (int.TryParse(value, out int integer))
                                {
                                    output = integer;
                                }
                                break;
                            case StandardOutputBo.DataTypeDropDown:
                                output = value;
                                break;
                            case StandardOutputBo.DataTypeBoolean:
                                if (bool.TryParse(value, out bool bl))
                                {
                                    output = bl;
                                }
                                else
                                {
                                    output = null;
                                }
                                break;
                        }
                        riDataBo.SetPropertyValue(property, output);
                    }
                    riDataBo.IgnoreFinalise = model.IgnoreFinalise;
                    riDataBo.OriginalEntryId = model.OriginalEntryId;


                    TrailObject trail = GetNewTrailObject();
                    if (id == 0)
                    {
                        riDataBo.CreatedById = AuthUserId;
                        riDataBo.RiDataBatchId = model.RiDataBatchId;

                        Result = RiDataService.Create(ref riDataBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(riDataBo.Id, "Create RI Data Details");
                            SetCreateSuccessMessage("RI Data Details");
                            return RedirectToAction("Details", new { id = riDataBo.Id, ridataBatchId = riDataBo.RiDataBatchId });
                        }
                    }
                    else
                    {
                        riDataBo.UpdatedById = AuthUserId;

                        Result = RiDataService.Update(ref riDataBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(riDataBo.Id, "Update RI Data Details");
                            SetUpdateSuccessMessage("RI Data Details");
                            return RedirectToAction("Details", new { id = riDataBo.Id, ridataBatchId = riDataBo.RiDataBatchId });
                        }
                    }
                    AddResult(Result);
                }
            }

            DetailsPage();
            IsEdit(riDataBo.RiDataBatchBo.Status);
            if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId))
                ViewBag.EditRiData = false;
            return View(new RiDataViewModel(riDataBo));
        }

        public ActionResult DeleteRiDataDetails(int id)
        {
            SetViewBagMessage();
            RiDataBo riDataBo = RiDataService.Find(id);
            if (riDataBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDenied));
                return RedirectToAction("Details", new { id = riDataBo.Id, ridataBatchId = riDataBo.RiDataBatchBo.Id });
            }

            return View(new RiDataViewModel(riDataBo));
        }

        [HttpPost]
        public ActionResult DeleteRiDataDetails(int id, RiDataViewModel model)
        {
            RiDataBo riDataBo = RiDataService.Find(id);
            if (riDataBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckWorkgroupPower(riDataBo.RiDataBatchBo.CedantId))
            {
                SetErrorSessionMsg(string.Format(MessageBag.AccessDenied));
                return RedirectToAction("Details", new { id = riDataBo.Id, ridataBatchId = riDataBo.RiDataBatchBo.Id });
            }

            TrailObject trail = GetNewTrailObject();
            Result = RiDataService.Delete(riDataBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    riDataBo.Id,
                    "Delete RI Data Details"
                );
            }
            else
            {
                if (Result.MessageBag.Errors.Count > 1)
                    SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
                else
                    SetErrorSessionMsg(Result.MessageBag.Errors[0]);
                return RedirectToAction("DeleteRiDataDetails", new { id });
            }

            SetDeleteSuccessMessage("RI Data Details");
            return RedirectToAction("Edit", new { id = riDataBo.RiDataBatchId });
        }

        // For RiData CSV download
        public ActionResult Download(
            string downloadToken,
            int type,
            int Id,
            string PolicyNumber,
            string TreatyCode,
            string InsuredName,
            int? TransactionTypeCodeId,
            int? ReinsBasicCodeId,
            int? ReportPeriodMonth,
            int? ReportPeriodYear,
            int? RiskPeriodMonth,
            int? RiskPeriodYear,
            int? PreComputation1Status,
            int? PreComputation2Status,
            int? PreValidationStatus,
            string ConflictType,
            int? PostComputationStatus,
            int? PostValidationStatus,
            int? MappingStatus,
            int? FinaliseStatus
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.Id = Id;
            Params.PolicyNumber = PolicyNumber;
            Params.TreatyCode = TreatyCode;
            Params.InsuredName = InsuredName;
            Params.TransactionTypeCodeId = TransactionTypeCodeId;
            Params.ReinsBasicCodeId = ReinsBasicCodeId;
            Params.ReportPeriodMonth = ReportPeriodMonth;
            Params.ReportPeriodYear = ReportPeriodYear;
            Params.RiskPeriodMonth = RiskPeriodMonth;
            Params.RiskPeriodYear = RiskPeriodYear;
            Params.PreComputation1Status = PreComputation1Status;
            Params.PreComputation2Status = PreComputation2Status;
            Params.PreValidationStatus = PreValidationStatus;
            Params.ConflictType = ConflictType;
            Params.PostComputationStatus = PostComputationStatus;
            Params.PostValidationStatus = PostValidationStatus;
            Params.MappingStatus = MappingStatus;
            Params.FinaliseStatus = FinaliseStatus;
            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportRiData(Id, AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });

            /*
            _db.Database.CommandTimeout = 0;
            var query = _db.RiData.Where(q => q.RiDataBatchId == Id).Select(RiDataService.Expression());
            if (type == 2) // filtered dowload 
            {
                if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Contains(TreatyCode));
                if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
                if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
                if (InsuredGenderCodeId != null)
                {
                    PickListDetailBo pickListDetailBo = PickListDetailService.Find(InsuredGenderCodeId);
                    query = query.Where(q => !string.IsNullOrEmpty(q.InsuredGenderCode) && q.InsuredGenderCode.Contains(pickListDetailBo.Code));
                }
                if (!string.IsNullOrEmpty(PolicyTerm)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyTerm.ToString()) && q.PolicyTerm.ToString().Contains(PolicyTerm));
                if (PreComputation1Status != null) query = query.Where(q => q.PreComputation1Status == PreComputation1Status);
                if (PreComputation2Status != null) query = query.Where(q => q.PreComputation2Status == PreComputation2Status);
                if (PreValidationStatus != null) query = query.Where(q => q.PreValidationStatus == PreValidationStatus);
                if (PostComputationStatus != null) query = query.Where(q => q.PostComputationStatus == PostComputationStatus);
                if (PostValidationStatus != null) query = query.Where(q => q.PostValidationStatus == PostValidationStatus);
                if (MappingStatus != null) query = query.Where(q => q.MappingStatus == MappingStatus);
                if (FinaliseStatus != null) query = query.Where(q => q.FinaliseStatus == FinaliseStatus);
            }

            var export = new ExportRiData();
            export.HandleTempDirectory();
            export.Process(query);

            return File(export.FilePath, "text/csv", System.IO.Path.GetFileName(export.FilePath));
            */
        }

        public ActionResult LogFileDownload(int riDataBatchId, int statusHistoryId)
        {
            RiDataBatchStatusFileBo riDataBatchStatusFileBo = RiDataBatchStatusFileService.FindByRiDataBatchIdStatusHistoryId(riDataBatchId, statusHistoryId);
            return File(System.IO.File.ReadAllBytes(riDataBatchStatusFileBo.GetFilePath()), "text/plain", riDataBatchStatusFileBo.GetFilePath().Split('/').Last());
        }

        public ActionResult DebugLogFileDownload(int riDataBatchId, int statusHistoryId)
        {
            RiDataBatchStatusFileBo riDataBatchStatusFileBo = RiDataBatchStatusFileService.FindByRiDataBatchIdStatusHistoryId(riDataBatchId, statusHistoryId);
            return File(System.IO.File.ReadAllBytes(riDataBatchStatusFileBo.GetDebugSummaryFilePath()), "text/plain", riDataBatchStatusFileBo.GetDebugSummaryFilePath().Split('/').Last());
        }


        public ActionResult FileDownload(int rawFileId)
        {
            RawFileBo rawFileBo = RawFileService.Find(rawFileId);
            DownloadFile(rawFileBo.GetLocalPath(), rawFileBo.FileName);
            return null;
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

        public void IndexPage()
        {
            ViewBag.CutOffReadOnly = CheckCutOffReadOnly(Controller);

            DropDownEmpty();
            DropDownCedant();
            DropDownBatchStatus();
            DropDownBatchProcessWarehouseStatus();
            //DropDownTreaty();

            SetViewBagMessage();
        }

        public void LoadPage(RiDataBatchBo riDataBatchBo = null)
        {
            DropDownEmpty();
            DropDownDelimiter();
            DropDownHasHeader();
            DropDownRiskMonth();
            DropDownRecordType();
            DropDownBatchStatus();

            // For filter RI Data
            DropDownInsuredGenderCode();
            DropDownMappingStatus();
            DropDownPreComputation1Status();
            DropDownPreComputation2Status();
            DropDownPreValidationStatus();
            DropDownPostComputationStatus();
            DropDownPostValidationStatus();
            DropDownFinaliseStatus();
            DropDownProcessWarehouseStatus();
            DropDownTransactionTypeCode();
            DropDownReinsBasisCode();
            DropDownPeriodMonth();
            DropDownConflictType();

            if (riDataBatchBo == null)
            {
                DropDownCedant(CedantBo.StatusActive, checkWorkgroup: true);
            }
            else
            {
                LogHelper("RI Batch " + riDataBatchBo.Id + " Start (GET)Override Property List");
                ListOverrideProperties(riDataBatchBo.OverrideProperties);
                LogHelper("RI Batch " + riDataBatchBo.Id + " End (GET)Override Property List");
                DropDownConfig(riDataBatchBo.CedantId, riDataBatchBo.RiDataConfigId);

                if (riDataBatchBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
                if (riDataBatchBo.RiDataConfigBo.Status != RiDataConfigBo.StatusApproved)
                {
                    AddWarningMsg(string.Format(MessageBag.RiDataConfigStatusInactive, RiDataConfigBo.GetStatusName(riDataBatchBo.RiDataConfigBo.Status)));
                }

                List<string> statusHistoryStatusList = new List<string>();
                statusHistoryStatusList.Add("Please select");
                for (int i = 1; i <= RiDataBatchBo.StatusFinaliseFailed; i++)
                {
                    statusHistoryStatusList.Add(RiDataBatchBo.GetStatusName(i));
                }
                ViewBag.StatusHistoryStatusList = statusHistoryStatusList;

                List<string> uploadModeList = new List<string>();
                uploadModeList.Add("Please select");
                for (int i = 1; i <= RiDataFileBo.ModeExclude; i++)
                {
                    uploadModeList.Add(RiDataFileBo.GetModeName(i));
                }
                ViewBag.UploadModeList = uploadModeList;

                LogHelper("RI Batch " + riDataBatchBo.Id + " Start (GET)Status History List");
                ModuleBo moduleBo = GetModuleByController(ModuleBo.ModuleController.RiData.ToString());
                IList<StatusHistoryBo> statusHistories = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleBo.Id, riDataBatchBo.Id);
                ViewBag.StatusHistories = statusHistories;
                LogHelper("RI Batch " + riDataBatchBo.Id + " End (GET)Status History List");
                LogHelper("RI Batch " + riDataBatchBo.Id + " Start (GET)RiData Batch Status File History List");
                IList<RiDataBatchStatusFileBo> riDataBatchStatusFiles = RiDataBatchStatusFileService.GetRIDataBatchStatusByRiDataBatchId(riDataBatchBo.Id);
                ViewBag.RIDataBatchStatusFileHistories = riDataBatchStatusFiles;
                GetDebugLogFile(riDataBatchStatusFiles);
                LogHelper("RI Batch " + riDataBatchBo.Id + " End (GET)RiData Batch Status File History List");
                LogHelper("RI Batch " + riDataBatchBo.Id + " Start (GET)Remarks List");
                IList<RemarkBo> remarkBos = RemarkService.GetByModuleIdObjectId(moduleBo.Id, riDataBatchBo.Id);
                ViewBag.RemarkBos = remarkBos;
                LogHelper("RI Batch " + riDataBatchBo.Id + " End (GET)Remarks List");
                ViewBag.AuthUserName = AuthUser.FullName;

                bool cutOffReadOnly = CheckCutOffReadOnly(Controller);
                IsDisabled(riDataBatchBo.Status);
                if (cutOffReadOnly)
                    ViewBag.Disabled = true;

                IsEdit(riDataBatchBo.Status);
                ConfigDisabled(riDataBatchBo.RiDataConfigBo.Status);
                if (!CheckWorkgroupPower(riDataBatchBo.CedantId) || cutOffReadOnly)
                    ViewBag.EditRiData = false;

                // If SOA Data Batch Status = Conditional status
                // Enable submit for finalise button including the RI Data batch status is Pre-Failed/Post-Failed
                if (riDataBatchBo.SoaDataBatchId.HasValue)
                {
                    var SoaDataBatch = SoaDataBatchService.Find(riDataBatchBo.SoaDataBatchId);
                    if (SoaDataBatch != null)
                    {
                        var SoaDataBatchStatus = SoaDataBatch.Status;
                        if ((SoaDataBatchStatus == SoaDataBatchBo.StatusConditionalApproval || SoaDataBatchStatus == SoaDataBatchBo.StatusProvisionalApproval)
                                && (riDataBatchBo.Status == RiDataBatchBo.StatusPreFailed || riDataBatchBo.Status == RiDataBatchBo.StatusPostFailed)
                                && SoaDataBatch.RiDataBatchId == riDataBatchBo.Id)
                            ViewBag.DisabledSubmitForFinalise = true;
                    }
                }
            }
        }

        public void LoadEditFilePage(RiDataFileBo riDataFileBo)
        {
            DropDownEmpty();
            DropDownDelimiter();
            DropDownHasHeader();
            DropDownRecordType();
            DropDownConfig(riDataFileBo.RiDataBatchBo.CedantId, riDataFileBo.RiDataBatchBo.RiDataConfigId);
            ListOverrideProperties(riDataFileBo.OverrideProperties);

            if (riDataFileBo.RiDataBatchBo.CedantBo.Status == CedantBo.StatusInactive)
            {
                AddWarningMsg(MessageBag.CedantStatusInactive);
            }
            if (riDataFileBo.RiDataBatchBo.RiDataConfigBo.Status != RiDataConfigBo.StatusApproved)
            {
                AddWarningMsg(string.Format(MessageBag.RiDataConfigStatusInactive, RiDataConfigBo.GetStatusName(riDataFileBo.RiDataBatchBo.RiDataConfigBo.Status)));
            }

            bool cutOffReadOnly = CheckCutOffReadOnly(Controller);
            IsDisabled(riDataFileBo.RiDataBatchBo.Status);
            if (cutOffReadOnly)
                ViewBag.Disabled = true;

            ConfigDisabled(riDataFileBo.RiDataBatchBo.RiDataConfigBo.Status);
            CheckWorkgroupPower(riDataFileBo.RiDataBatchBo.CedantId);
        }

        public List<SelectListItem> DropDownConfig(int cedantId, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            IList<RiDataConfigBo> riDataConfigBos = RiDataConfigService.GetByCedantIdStatus(cedantId, RiDataBo.StatusApproved, selectedId);
            foreach (RiDataConfigBo riDataConfigBo in riDataConfigBos)
            {
                items.Add(new SelectListItem { Text = string.Format("{0} - {1}", riDataConfigBo.Code, riDataConfigBo.Name), Value = riDataConfigBo.Id.ToString() });
            }
            ViewBag.ConfigItems = items;
            return items;
        }

        public List<SelectListItem> DropDownBatchStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in RiDataBatchBo.DropDownStatusOrder())
            {
                items.Add(new SelectListItem { Text = RiDataBatchBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.StatusItems = items;
            return items;
        }

        public List<SelectListItem> DropDownDelimiter()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataConfigBo.MaxDelimiter; i++)
            {
                items.Add(new SelectListItem { Text = RiDataConfigBo.GetDelimiterName(i), Value = i.ToString() });
            }
            ViewBag.DelimiterItems = items;
            return items;
        }

        public List<SelectListItem> DropDownHasHeader()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "No", Value = "false" });
            items.Add(new SelectListItem { Text = "Yes", Value = "true" });
            ViewBag.HasHeaderItems = items;
            return items;
        }

        public List<SelectListItem> DropDownRiskMonth()
        {
            var items = GetEmptyDropDownList();
            items.AddRange(DateTimeFormatInfo.InvariantInfo.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .Select((monthName, index) => new SelectListItem
                {
                    Value = (index + 1).ToString(),
                    Text = monthName
                }).ToList());

            ViewBag.RiskMonthItems = items;
            return items;
        }

        public List<SelectListItem> DropDownPeriodMonth()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, 12))
            {
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.PeriodMonthItems = items;
            return items;
        }

        public List<SelectListItem> DropDownRecordType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBatchBo.RecordTypeMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBatchBo.GetRecordTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRecordTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownMappingStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.MappingStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetMappingStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownMappingStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownPreComputation1Status()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.PreComputation1StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetPreComputation1StatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPreComputation1Status = items;
            return items;
        }

        public List<SelectListItem> DropDownPreComputation2Status()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.PreComputation2StatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetPreComputation2StatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPreComputation2Status = items;
            return items;
        }

        public List<SelectListItem> DropDownPreValidationStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.PreValidationStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetPreValidationStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPreValidationStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownPostComputationStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.PostComputationStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetPostComputationStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPostComputationStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownPostValidationStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.PostValidationStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetPostValidationStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownPostValidationStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownFinaliseStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.FinaliseStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetFinaliseStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownFinaliseStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownBatchProcessWarehouseStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBatchBo.ProcessWarehouseStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBatchBo.GetProcessWarehouseStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProcessWarehouseStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownProcessWarehouseStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.ProcessWarehouseStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetProcessWarehouseStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProcessWarehouseStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownConflictType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in RiDataBo.GetConflictTypeOrder())
            {
                items.Add(new SelectListItem { Text = RiDataBo.GetConflictTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownConflictTypes = items;
            return items;
        }

        public Dictionary<string, object> ListOverrideProperties(string overrideProperties)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();

            string[] items = Util.GetConfig("RiDataOverrideProperties").Split(',').ToArray();

            Dictionary<string, object> itemPairs = null;
            if (!string.IsNullOrEmpty(overrideProperties))
            {
                itemPairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(overrideProperties);
            }
            foreach (string item in items)
            {
                object itemVal = null;
                int type = int.Parse(item);
                if (itemPairs != null && itemPairs.ContainsKey(item) && itemPairs[item] != null)
                {
                    var value = itemPairs[item];
                    string vStr = value.ToString();
                    int datatype = StandardOutputBo.GetDataTypeByType(type);
                    switch (datatype)
                    {
                        case StandardOutputBo.DataTypeDate:
                            Util.TryParseDateTime(vStr, out DateTime? date, out string _);
                            itemVal = date?.ToString(Util.GetDateFormat());
                            break;
                        case StandardOutputBo.DataTypeString:
                            itemVal = vStr;
                            break;
                        case StandardOutputBo.DataTypeAmount:
                            itemVal = Util.DoubleToString(value);
                            break;
                        case StandardOutputBo.DataTypePercentage:
                            itemVal = Util.DoubleToString(value);
                            break;
                        case StandardOutputBo.DataTypeInteger:
                            itemVal = vStr;
                            break;
                        case StandardOutputBo.DataTypeDropDown:
                            itemVal = vStr;
                            break;
                    }
                }
                else if (itemPairs == null)
                {
                    switch (type)
                    {
                        case StandardOutputBo.TypeCurrencyCode:
                            itemVal = PickListDetailBo.CurrencyCodeMyr;
                            break;
                        case StandardOutputBo.TypeCurrencyRate:
                            itemVal = Util.DoubleToString(1);
                            break;
                        default:
                            break;
                    }
                }
                list.Add(item, itemVal);
            }
            ViewBag.OverridePropertiesList = list;
            return list;
        }

        public void ListRiData(
            int id,
            int? Page,
            string PolicyNumber = null,
            string TreatyCode = null,
            string InsuredName = null,
            int? TransactionTypeCodeId = null,
            int? ReinsBasicCodeId = null,
            int? ReportPeriodMonth = null,
            int? ReportPeriodYear = null,
            int? RiskPeriodMonth = null,
            int? RiskPeriodYear = null,
            int? PreComputation1Status = null,
            int? PreComputation2Status = null,
            int? PreValidationStatus = null,
            string ConflictType = null,
            int? PostComputationStatus = null,
            int? PostValidationStatus = null,
            int? MappingStatus = null,
            int? FinaliseStatus = null,
            int? WarehouseProcessStatus = null,
            bool? IgnoreFinalise = null,
            string IgnoreFinaliseIds = null,
            string SortOrder = null,
            string TreatyCodeQuery = null,
            string TreatyCodeQuery2 = null
        )
        {
            LogHelper("Ri Batch " + id + " Start (GET)RiData List");

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["PolicyNumber"] = PolicyNumber,
                ["TreatyCode"] = TreatyCode,
                ["InsuredName"] = InsuredName,
                ["TransactionTypeCodeId"] = TransactionTypeCodeId,
                ["ReinsBasicCodeId"] = ReinsBasicCodeId,
                ["ReportPeriodMonth"] = ReportPeriodMonth,
                ["ReportPeriodYear"] = ReportPeriodYear,
                ["RiskPeriodMonth"] = RiskPeriodMonth,
                ["RiskPeriodYear"] = RiskPeriodYear,
                ["PreComputation1Status"] = PreComputation1Status,
                ["PreComputation2Status"] = PreComputation2Status,
                ["PreValidationStatus"] = PreValidationStatus,
                ["ConflictType"] = ConflictType,
                ["PostComputationStatus"] = PostComputationStatus,
                ["PostValidationStatus"] = PostValidationStatus,
                ["MappingStatus"] = MappingStatus,
                ["FinaliseStatus"] = FinaliseStatus,
                ["WarehouseProcessStatus"] = WarehouseProcessStatus,
                ["IgnoreFinalise"] = IgnoreFinalise,
                ["IgnoreFinaliseIds"] = IgnoreFinaliseIds,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortPolicyNumber = GetSortParam("PolicyNumber");
            ViewBag.SortTreatyCode = GetSortParam("TreatyCode");
            ViewBag.SortTransactionTypeCode = GetSortParam("TransactionTypeCode");
            ViewBag.SortReinsBasicCode = GetSortParam("ReinsBasicCode");
            ViewBag.SortInsuredName = GetSortParam("InsuredName");
            ViewBag.SortReportPeriodMonth = GetSortParam("ReportPeriodMonth");
            ViewBag.SortReportPeriodYear = GetSortParam("ReportPeriodYear");
            ViewBag.SortRiskPeriodMonth = GetSortParam("RiskPeriodMonth");
            ViewBag.SortRiskPeriodYear = GetSortParam("RiskPeriodYear");

            LogHelper("Ri Batch " + id + " Start (GET)RiData List query");

            _db.Database.CommandTimeout = 0;
            var distinctTreatyCodes = RiDataBatchService.GetDistinctTreatyCodesById(id);

            var query = _db.RiData.AsNoTracking().Where(q => q.RiDataBatchId == id &&
                (distinctTreatyCodes.Contains(q.TreatyCode) || string.IsNullOrEmpty(q.TreatyCode)))
                .Select(RiDataListingViewModel.Expression());

            if (!string.IsNullOrEmpty(PolicyNumber)) query = query.Where(q => !string.IsNullOrEmpty(q.PolicyNumber) && q.PolicyNumber.Contains(PolicyNumber));
            if (!string.IsNullOrEmpty(TreatyCode)) query = query.Where(q => !string.IsNullOrEmpty(q.TreatyCode) && q.TreatyCode.Contains(TreatyCode));
            if (TransactionTypeCodeId != null)
            {
                var pickListDetailBo = PickListDetailService.Find(TransactionTypeCodeId);
                if (pickListDetailBo != null) query = query.Where(q => !string.IsNullOrEmpty(q.TransactionTypeCode) && q.TransactionTypeCode == pickListDetailBo.Code);
            }
            if (ReinsBasicCodeId != null)
            {
                var pickListDetailBo = PickListDetailService.Find(ReinsBasicCodeId);
                if (pickListDetailBo != null) query = query.Where(q => !string.IsNullOrEmpty(q.ReinsBasicCode) && q.ReinsBasicCode == pickListDetailBo.Code);
            }
            if (!string.IsNullOrEmpty(InsuredName)) query = query.Where(q => !string.IsNullOrEmpty(q.InsuredName) && q.InsuredName.Contains(InsuredName));
            if (ReportPeriodMonth != null) query = query.Where(q => q.ReportPeriodMonth == ReportPeriodMonth);
            if (ReportPeriodYear != null) query = query.Where(q => q.ReportPeriodYear == ReportPeriodYear);
            if (RiskPeriodMonth != null) query = query.Where(q => q.RiskPeriodMonth == RiskPeriodMonth);
            if (RiskPeriodYear != null) query = query.Where(q => q.RiskPeriodYear == RiskPeriodYear);
            if (PreComputation1Status != null) query = query.Where(q => q.PreComputation1Status == PreComputation1Status);
            if (PreComputation2Status != null) query = query.Where(q => q.PreComputation2Status == PreComputation2Status);
            if (PreValidationStatus != null) query = query.Where(q => q.PreValidationStatus == PreValidationStatus);

            if (!string.IsNullOrEmpty(ConflictType))
            {
                string[] ConflictTypes = Util.ToArraySplitTrim(ConflictType);
                query = query.Where(q => ConflictTypes.Contains(q.ConflictType.ToString()));
            }

            if (PostComputationStatus != null) query = query.Where(q => q.PostComputationStatus == PostComputationStatus);
            if (PostValidationStatus != null) query = query.Where(q => q.PostValidationStatus == PostValidationStatus);
            if (MappingStatus != null) query = query.Where(q => q.MappingStatus == MappingStatus);
            if (FinaliseStatus != null) query = query.Where(q => q.FinaliseStatus == FinaliseStatus);
            if (WarehouseProcessStatus != null) query = query.Where(q => q.ProcessWarehouseStatus == WarehouseProcessStatus);
            if (IgnoreFinalise != null) query = query.Where(q => q.IgnoreFinalise == IgnoreFinalise);

            if (SortOrder == Html.GetSortAsc("PolicyNumber")) query = query.OrderBy(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortDsc("PolicyNumber")) query = query.OrderByDescending(q => q.PolicyNumber);
            else if (SortOrder == Html.GetSortAsc("TreatyCode")) query = query.OrderBy(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortDsc("TreatyCode")) query = query.OrderByDescending(q => q.TreatyCode);
            else if (SortOrder == Html.GetSortAsc("TransactionTypeCode")) query = query.OrderBy(q => q.TransactionTypeCode);
            else if (SortOrder == Html.GetSortDsc("TransactionTypeCode")) query = query.OrderByDescending(q => q.TransactionTypeCode);
            else if (SortOrder == Html.GetSortAsc("ReinsBasicCode")) query = query.OrderBy(q => q.ReinsBasicCode);
            else if (SortOrder == Html.GetSortDsc("ReinsBasicCode")) query = query.OrderByDescending(q => q.ReinsBasicCode);
            else if (SortOrder == Html.GetSortAsc("InsuredName")) query = query.OrderBy(q => q.InsuredName);
            else if (SortOrder == Html.GetSortDsc("InsuredName")) query = query.OrderByDescending(q => q.InsuredName);
            else if (SortOrder == Html.GetSortAsc("ReportPeriodMonth")) query = query.OrderBy(q => q.ReportPeriodMonth);
            else if (SortOrder == Html.GetSortDsc("ReportPeriodMonth")) query = query.OrderByDescending(q => q.ReportPeriodMonth);
            else if (SortOrder == Html.GetSortAsc("ReportPeriodYear")) query = query.OrderBy(q => q.ReportPeriodYear);
            else if (SortOrder == Html.GetSortDsc("ReportPeriodYear")) query = query.OrderByDescending(q => q.ReportPeriodYear);
            else if (SortOrder == Html.GetSortAsc("RiskPeriodMonth")) query = query.OrderBy(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortDsc("RiskPeriodMonth")) query = query.OrderByDescending(q => q.RiskPeriodMonth);
            else if (SortOrder == Html.GetSortAsc("RiskPeriodYear")) query = query.OrderBy(q => q.RiskPeriodYear);
            else if (SortOrder == Html.GetSortDsc("RiskPeriodYear")) query = query.OrderByDescending(q => q.RiskPeriodYear);
            else query = query.OrderBy(q => q.Id);

            LogHelper("Ri Batch " + id + " End (GET)RiData List query");

            LogHelper("Ri Batch " + id + " Start (GET)RiData List Total");
            ViewBag.RiDataTotal = query.Count();
            LogHelper("Ri Batch " + id + " End (GET)RiData List Total " + ViewBag.RiDataTotal);

            LogHelper("Ri Batch " + id + " Start (GET)RiData List pagination");
            ViewBag.RiDataList = query.ToPagedList(Page ?? 1, PageSize);
            LogHelper("Ri Batch " + id + " End (GET)RiData List pagination");

            LogHelper("Ri Batch " + id + " End (GET)RiData List");
        }

        [HttpPost]
        public JsonResult GetConfigByCedant(int cedantId)
        {
            IList<RiDataConfigBo> riDataConfigBos = RiDataConfigService.GetByCedantIdStatus(cedantId, RiDataBo.StatusApproved);
            return Json(new { riDataConfigBos });
        }

        [HttpPost]
        public JsonResult GetConfigDetails(int id)
        {
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            return Json(new { riDataConfigBo });
        }

        [HttpPost]
        public JsonResult GetTreatyByCedant(int cedantId)
        {
            IList<TreatyBo> treatyBos = TreatyService.GetByCedantId(cedantId);
            return Json(new { treatyBos });
        }

        public IList<RiDataBatchStatusFileBo> GetDebugLogFile(IList<RiDataBatchStatusFileBo> riDataBatchStatusFileBos)
        {
            IList<RiDataBatchStatusFileBo> riDataBatchDebugStatusFiles = new List<RiDataBatchStatusFileBo>();
            foreach (RiDataBatchStatusFileBo riDataBatchStatusFileBo in riDataBatchStatusFileBos)
            {
                string debugLogFilePath = riDataBatchStatusFileBo.GetDebugSummaryFilePath();
                if (!string.IsNullOrEmpty(debugLogFilePath))
                {
                    if (System.IO.File.Exists(debugLogFilePath))
                    {
                        riDataBatchDebugStatusFiles.Add(riDataBatchStatusFileBo);
                    }
                }
            }
            ViewBag.RIDataBatchStatusDebugFileHistories = riDataBatchDebugStatusFiles;
            return riDataBatchDebugStatusFiles;
        }

        public void IsDisabled(int status)
        {
            ViewBag.Disabled = false;
            switch (status)
            {
                case RiDataBatchBo.StatusSubmitForPreProcessing:
                case RiDataBatchBo.StatusPreProcessing:
                case RiDataBatchBo.StatusSubmitForFinalise:
                case RiDataBatchBo.StatusFinalising:
                case RiDataBatchBo.StatusFinalised:
                case RiDataBatchBo.StatusSubmitForPostProcessing:
                case RiDataBatchBo.StatusPostProcessing:
                    ViewBag.Disabled = true;
                    break;
            }

            ViewBag.ShowFinaliseStatus = false;
            switch (status)
            {
                case RiDataBatchBo.StatusSubmitForFinalise:
                case RiDataBatchBo.StatusFinalising:
                case RiDataBatchBo.StatusFinalised:
                case RiDataBatchBo.StatusFinaliseFailed:
                    ViewBag.ShowFinaliseStatus = true;
                    break;
            }

            ViewBag.ShowProcessWarehouseStatus = status == RiDataBatchBo.StatusFinalised ? true : false;

            ViewBag.NoticeRemoveRiDataMessage = false;
            switch (status)
            {
                case RiDataBatchBo.StatusPreSuccess:
                case RiDataBatchBo.StatusPostSuccess:
                    ViewBag.NoticeRemoveRiDataMessage = true;
                    break;
            }

            ViewBag.DisabledSubmitForPostProccessing = false;
            switch (status)
            {
                case RiDataBatchBo.StatusPreSuccess:
                case RiDataBatchBo.StatusPostSuccess:
                case RiDataBatchBo.StatusPostFailed:
                case RiDataBatchBo.StatusFinaliseFailed:
                    ViewBag.DisabledSubmitForPostProccessing = true;
                    break;
            }

            ViewBag.DisabledSubmitForFinalise = false;
            switch (status)
            {
                case RiDataBatchBo.StatusPostSuccess:
                case RiDataBatchBo.StatusFinaliseFailed:
                    ViewBag.DisabledSubmitForFinalise = true;
                    break;
            }
        }

        public void IsEdit(int? status)
        {
            ViewBag.EditRiData = false;
            switch (status)
            {
                case RiDataBatchBo.StatusPreSuccess:
                case RiDataBatchBo.StatusPostSuccess:
                case RiDataBatchBo.StatusPostFailed:
                case RiDataBatchBo.StatusFinaliseFailed:
                    ViewBag.EditRiData = true;
                    break;
            }
        }

        public void ConfigDisabled(int status)
        {
            ViewBag.DisabledSubmitByConfigStatus = false;
            switch (status)
            {
                case RiDataConfigBo.StatusDraft:
                case RiDataConfigBo.StatusPending:
                case RiDataConfigBo.StatusRejected:
                case RiDataConfigBo.StatusDisabled:
                    ViewBag.DisabledSubmitByConfigStatus = true;
                    break;
            }
        }

        public static void LogHelper(string message)
        {
            bool lockObj = Convert.ToBoolean(Util.GetConfig("Logging"));
            string filePath = string.Format("{0}/RiDataBatchViewSummary".AppendDateFileName(".txt"), Util.GetLogPath("RiDataBatchView"));

            if (lockObj)
            {
                Util.MakeDir(filePath);

                try
                {
                    using (var textFile = new TextFile(filePath, true, true))
                    {
                        textFile.WriteLine(string.Format("{0}   {1}   {2}", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss:ffff"), System.Web.HttpContext.Current.Session.SessionID, message));
                        textFile.WriteLine();
                        textFile.WriteLine();
                    }
                }
                catch { }
            }
        }

        [HttpPost]
        public JsonResult GetRiDataByMatchCombination(string policyNo, string planCode, string quarter, string riskQuarter, string mlreBenefitCode, string cedingBenefitTypeCode, int? riskPeriodMonth, int? riskPeriodYear, string treatyCode, int? riderNumber)
        {
            IList<RiDataWarehouseBo> riDataBos = RiDataWarehouseService.GetByRiDataParam(policyNo, planCode, quarter, riskQuarter, mlreBenefitCode, cedingBenefitTypeCode, riskPeriodMonth, riskPeriodYear, treatyCode, riderNumber);
            if (riDataBos.IsNullOrEmpty())
            {
                // Search by old treaty code
                var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(treatyCode);
                if (!string.IsNullOrEmpty(oldTreatyCode))
                {
                    riDataBos = RiDataWarehouseService.GetByRiDataParam(policyNo, planCode, quarter, riskQuarter, mlreBenefitCode, cedingBenefitTypeCode, riskPeriodMonth, riskPeriodYear, oldTreatyCode, riderNumber);
                }
            }

            return Json(new { riDataBos });
        }

        [HttpPost]
        public ActionResult GetOriginalRiDataById(int id, int ridataBatchId, RiDataWarehouseBo bo)
        {
            RiDataViewModel model = new RiDataViewModel();
            model.SetBos(bo);
            model.Id = id;
            model.RiDataBatchId = ridataBatchId;

            return Json(new { model });
        }

        public void DetailsPage()
        {
            ViewBag.StandardOutputList = StandardOutputService.Get();

            List<string> precomputation1StatusList = new List<string>();
            precomputation1StatusList.Add("");
            for (int i = 1; i <= RiDataBo.PreComputation1StatusMax; i++)
            {
                precomputation1StatusList.Add(RiDataBo.GetPreComputation1StatusName(i));
            }
            ViewBag.PreComputation1StatusList = precomputation1StatusList;

            List<string> prevalidationStatusList = new List<string>();
            prevalidationStatusList.Add("");
            for (int i = 1; i <= RiDataBo.PreValidationStatusMax; i++)
            {
                prevalidationStatusList.Add(RiDataBo.GetPreValidationStatusName(i));
            }
            ViewBag.PreValidationStatusList = prevalidationStatusList;

            List<string> recordTypeList = new List<string>();
            recordTypeList.Add("");
            for (int i = 1; i <= RiDataBo.RecordTypeMax; i++)
            {
                recordTypeList.Add(RiDataBo.GetRecordTypeName(i));
            }
            ViewBag.RecordTypeList = recordTypeList;

            DropDownBenefit(codeAsValue: true);
            DropDownTreatyCode(codeAsValue: true);
            DropDownPeriodMonth();
            DropDownYear();
            DropDownValuationBenefitCode();
        }

        public List<SelectListItem> DropDownValuationBenefitCode()
        {
            var items = GetEmptyDropDownList();
            var pickListDetailBos = PickListDetailService.GetByPickListId(PickListBo.ValuationBenefitCode);
            foreach (PickListDetailBo pickListDetailBo in pickListDetailBos)
            {
                items.Add(new SelectListItem { Text = pickListDetailBo.Code, Value = pickListDetailBo.Code.ToString() });
            }
            ViewBag.ValuationBenefitCodeItems = items;
            return items;
        }

        [HttpPost]
        public JsonResult SearchSoaDataBatch(int id, int? CedantId, int? TreatyId, string Quarter)
        {
            IList<SoaDataBatchBo> soaDataBatchBos = SoaDataBatchService.GetNotSOAApproved(CedantId, TreatyId, Quarter);
            soaDataBatchBos = soaDataBatchBos.Where(q => !q.RiDataBatchId.HasValue || q.RiDataBatchId == id).ToList();
            return Json(new { soaDataBatchBos });
        }

        [HttpPost]
        public JsonResult CreateSoaDataBatch(int? Id, int? CedantId, int? TreatyId, string Quarter)
        {
            bool success = true;
            int? resultId = null;
            string message = "";
            try
            {
                var treatyBo = TreatyService.Find(TreatyId);
                if (treatyBo.BusinessOriginPickListDetailId == null)
                {
                    success = false;
                    message = "Business Origin in Treaty Id is empty";
                }
                else
                {
                    ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());
                    PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.CurrencyCode, PickListDetailBo.CurrencyCodeMyr);

                    SoaDataBatchBo b = new SoaDataBatchBo();
                    b.CedantId = CedantId.Value;
                    b.TreatyId = TreatyId;
                    b.Quarter = Quarter;
                    b.Status = SoaDataBatchBo.StatusSubmitForProcessing;
                    b.DataUpdateStatus = SoaDataBatchBo.DataUpdateStatusPending;
                    b.StatementReceivedAt = DateTime.Parse(DateTime.Now.ToString(Util.GetDateFormat()));
                    if (DirectRetroConfigurationService.CountByTreatyId(TreatyId.Value) > 0)
                        b.DirectStatus = DirectRetroBo.RetroStatusPending;
                    if (Id != 0)
                        b.RiDataBatchId = Id;
                    b.IsRiDataAutoCreate = true;
                    b.CurrencyCodePickListDetailId = pickListDetailBo?.Id;
                    b.CurrencyRate = 1;
                    b.CreatedById = AuthUserId;
                    b.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();
                    Result = SoaDataBatchService.Create(ref b, ref trail);
                    if (Result.Valid)
                    {
                        StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                        {
                            ModuleId = moduleBo.Id,
                            ObjectId = b.Id,
                            Status = b.Status,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };
                        StatusHistoryService.Save(ref statusHistoryBo, ref trail);

                        resultId = b.Id;
                    }
                    else
                    {
                        success = false;
                        message = Result.MessageBag.Errors[0];
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.ToString();
            }

            return Json(new { success, resultId, message });
        }
    }
}
