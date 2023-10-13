using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Claims;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class ClaimDataConfigController : BaseController
    {
        public const string Controller = "ClaimDataConfig";

        // GET: ClaimDataConfig
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Name,
            int? CedantId,
            int? TreatyId,
            int? FileType,
            int? Status,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["Name"] = Name,
                ["CedantId"] = CedantId,
                ["TreatyId"] = TreatyId,
                ["FileType"] = FileType,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortName = GetSortParam("Name");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortTreatyId = GetSortParam("TreatyId");
            ViewBag.SortFileType = GetSortParam("FileType");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.ClaimDataConfigs.Select(ClaimDataConfigViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (TreatyId != null) query = query.Where(q => q.TreatyId == TreatyId);
            if (FileType != null) query = query.Where(q => q.FileType == FileType);
            if (Status != null) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("Name")) query = query.OrderBy(q => q.Name);
            else if (SortOrder == Html.GetSortDsc("Name")) query = query.OrderByDescending(q => q.Name);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Name);
            else if (SortOrder == Html.GetSortAsc("TreatyId")) query = query.OrderBy(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortDsc("TreatyId")) query = query.OrderByDescending(q => q.Treaty.TreatyIdCode);
            else if (SortOrder == Html.GetSortAsc("FileType")) query = query.OrderBy(q => q.FileType);
            else if (SortOrder == Html.GetSortDsc("FileType")) query = query.OrderByDescending(q => q.FileType);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Code);

            ViewBag.Total = query.Count();
            IndexPage();
            CheckWorkgroupPower();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: ClaimDataConfig/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClaimDataConfig/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            ClaimDataConfigViewModel model = new ClaimDataConfigViewModel();
            IsDisabled(model.Status);
            LoadPage(model);
            return View(model);
        }

        // POST: ClaimDataConfig/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, ClaimDataConfigViewModel model)
        {
            model.CurrentForm = form;
            if (ModelState.IsValid)
            {
                var claimDataConfigBo = new ClaimDataConfigBo();
                model.Get(ref claimDataConfigBo);
                claimDataConfigBo.CreatedById = AuthUserId;
                claimDataConfigBo.UpdatedById = AuthUserId;

                model.SetChildItems();

                bool validateFormula = claimDataConfigBo.Status == ClaimDataConfigBo.StatusPending;
                Result childResult = model.ValidateChildItems(claimDataConfigBo.ClaimDataFileConfig, validateFormula);
                if (childResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = ClaimDataConfigService.Create(ref claimDataConfigBo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = claimDataConfigBo.Id;
                        model.Status = claimDataConfigBo.Status;
                        model.SaveChildItems(AuthUserId, ref trail);

                        RemarkController.SaveRemarks(RemarkController.GetRemarks(form), model.ModuleId, claimDataConfigBo.Id, AuthUserId, ref trail);

                        model.ProcessStatusHistory(AuthUserId, ref trail);

                        CreateTrail(
                            claimDataConfigBo.Id,
                            "Create ClaimDataConfig"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { claimDataConfigBo.Id });
                    }
                }
                else
                {
                    Result = childResult;
                }
                AddResult(Result);
            }

            model.Status = ClaimDataConfigBo.StatusDraft;
            IsDisabled(model.Status);
            LoadPage(model, form);
            return View(model);
        }

        // GET: ClaimDataConfig/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(id);
            if (claimDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }
            ClaimDataConfigViewModel model = new ClaimDataConfigViewModel(claimDataConfigBo);
            CheckWorkgroupPower(claimDataConfigBo.CedantId);
            IsDisabled(claimDataConfigBo.Status, claimDataConfigBo.Id);
            LoadPage(model);
            return View(model);
        }

        // POST: ClaimDataConfig/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, ClaimDataConfigViewModel model)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(id);
            if (claimDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            model.CurrentForm = form;
            int currentStatus = claimDataConfigBo.Status;
            int[] changeStatusOnly = new int[] { ClaimDataConfigBo.StatusRejected, ClaimDataConfigBo.StatusApproved, ClaimDataConfigBo.StatusDisabled };
            if (ModelState.IsValid || changeStatusOnly.Contains(model.Status))
            {
                Result childResult = new Result();
                if (changeStatusOnly.Contains(model.Status))
                {
                    claimDataConfigBo.Status = model.Status;
                }
                else
                {
                    model.Get(ref claimDataConfigBo);
                    model.SetChildItems();

                    bool validateFormula = claimDataConfigBo.Status == ClaimDataConfigBo.StatusPending;
                    childResult = model.ValidateChildItems(claimDataConfigBo.ClaimDataFileConfig, validateFormula);
                }
                claimDataConfigBo.UpdatedById = AuthUserId;

                if (childResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = ClaimDataConfigService.Update(ref claimDataConfigBo, ref trail);
                    if (Result.Valid)
                    {
                        if (!changeStatusOnly.Contains(model.Status))
                        {
                            model.SaveChildItems(AuthUserId, ref trail);
                        }
                        model.ProcessStatusHistory(AuthUserId, ref trail);

                        CreateTrail(
                            claimDataConfigBo.Id,
                            "Update ClaimDataConfig"
                        );

                        switch (claimDataConfigBo.Status)
                        {
                            case ClaimDataConfigBo.StatusDraft:
                                SetUpdateSuccessMessage(Controller);
                                break;
                            case ClaimDataConfigBo.StatusPending:
                                SetSuccessSessionMsg(string.Format(MessageBag.ObjectSubmitted, Controller));
                                break;
                            case ClaimDataConfigBo.StatusApproved:
                                SetSuccessSessionMsg(string.Format(MessageBag.ObjectApproved, Controller));
                                break;
                            case ClaimDataConfigBo.StatusRejected:
                                SetSuccessSessionMsg(string.Format(MessageBag.ObjectRejected, Controller));
                                break;
                            case ClaimDataConfigBo.StatusDisabled:
                                SetSuccessSessionMsg(string.Format(MessageBag.ObjectDisabled, Controller));
                                break;
                        }
                        return RedirectToAction("Edit", new { Id = id });
                    }
                }
                else
                {
                    Result = childResult;
                }
                AddResult(Result);
            }

            model.Status = currentStatus;
            IsDisabled(model.Status);
            CheckWorkgroupPower(model.CedantId);
            LoadPage(model, form);
            return View(model);
        }

        // GET: ClaimDataConfig/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            List<string> msg = new List<string>();
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(id);
            if (claimDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }
            ClaimDataConfigViewModel model = new ClaimDataConfigViewModel(claimDataConfigBo);
            ViewBag.Disabled = true;
            LoadPage(model);
            return View(model);
        }

        // POST: ClaimDataConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RiDataConfigViewModel model)
        {
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(id);
            if (claimDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = ClaimDataConfigService.Delete(claimDataConfigBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    claimDataConfigBo.Id,
                    "Delete ClaimDataConfig"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = claimDataConfigBo.Id });
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            DropDownTreaty();
            DropDownFileType();
            DropDownStatus();
            SetViewBagMessage();
        }

        public void LoadPage(ClaimDataConfigViewModel model, FormCollection form = null)
        {
            model.CurrentForm = form;

            LoadContent(model);
            LoadChildItems(model);
            SetDefaultObjectList();
            GetStatusHistory(model.ModuleId, model.Id);
            GetRemark(model.ModuleId, model.Id);
            DropDownStatus();

            ViewBag.AuthUserName = AuthUser.FullName;

            SetViewBagMessage();
        }

        public void LoadContent(ClaimDataConfigViewModel model)
        {
            DropDownCedant(CedantBo.StatusActive, model.CedantId, "0", true);
            if (model.CedantBo != null && model.CedantBo.Status == CedantBo.StatusInactive)
                AddWarningMsg(MessageBag.CedantStatusInactive);

            DropDownTreaty(model.TreatyId, model.CedantId);

            DropDownFileType();
            DropDownDelimiter(model.Delimiter);
            GetTransformFormulas();
            GetBenefits();
            DropDownComputationMode();
            DropDownComputationTable();

            ViewBag.StdOutputList = StandardOutputService.Get();
            ViewBag.StdClaimDataOutputList = StandardClaimDataOutputService.Get();
            ViewBag.AuthUserName = AuthUser.FullName;
            ViewBag.ApprovalPower = CheckPower(Controller, AccessMatrixBo.PowerApproval);

            SetViewBagMessage();
        }

        public void LoadChildItems(ClaimDataConfigViewModel model)
        {
            IList<ClaimDataMappingBo> claimDataMappingBos;
            IList<ClaimDataComputationBo> claimDataComputationBos;
            IList<ClaimDataValidationBo> claimDataPreValidationBos;
            IList<ClaimDataComputationBo> claimDataPostComputationBos;
            IList<ClaimDataValidationBo> claimDataPostValidationBos;

            if (model.CurrentForm != null)
            {
                model.SetChildItems();
                claimDataMappingBos = (IList<ClaimDataMappingBo>)model.GetChildBos("ClaimDataMapping");
                claimDataComputationBos = model.GetComputationBosWithStep(ClaimDataComputationBo.StepPreComputation);
                claimDataPreValidationBos = model.GetValidationBosWithStep(ClaimDataValidationBo.StepPreValidation);
                claimDataPostComputationBos = model.GetComputationBosWithStep(ClaimDataComputationBo.StepPostComputation);
                claimDataPostValidationBos = model.GetValidationBosWithStep(ClaimDataValidationBo.StepPostValidation);
            }
            else if (model.Id != 0)
            {
                claimDataMappingBos = ClaimDataMappingService.GetByClaimDataConfigId(model.Id);
                claimDataComputationBos = ClaimDataComputationService.GetByClaimDataConfigId(model.Id, ClaimDataComputationBo.StepPreComputation);
                claimDataPreValidationBos = ClaimDataValidationService.GetByClaimDataConfigId(model.Id, ClaimDataValidationBo.StepPreValidation);
                claimDataPostComputationBos = ClaimDataComputationService.GetByClaimDataConfigId(model.Id, ClaimDataComputationBo.StepPostComputation);
                claimDataPostValidationBos = ClaimDataValidationService.GetByClaimDataConfigId(model.Id, ClaimDataValidationBo.StepPostValidation);
            }
            else
            {
                List<ClaimDataComputationBo> defaultComputations = ClaimDataComputationBo.GetDefault();
                List<ClaimDataValidationBo> defaultValidations = ClaimDataValidationBo.GetDefault();

                claimDataMappingBos = ClaimDataMappingBo.GetDefault();
                claimDataComputationBos = defaultComputations.Where(q => q.Step == ClaimDataComputationBo.StepPreComputation).ToList();
                claimDataPreValidationBos = defaultValidations.Where(q => q.Step == ClaimDataValidationBo.StepPreValidation).ToList();
                claimDataPostComputationBos = defaultComputations.Where(q => q.Step == ClaimDataComputationBo.StepPostComputation).ToList();
                claimDataPostValidationBos = defaultValidations.Where(q => q.Step == ClaimDataValidationBo.StepPostValidation).ToList();
            }

            ViewBag.ClaimDataMappingBos = claimDataMappingBos;
            ViewBag.ClaimDataComputationBos = claimDataComputationBos;
            ViewBag.ClaimDataPreValidationBos = claimDataPreValidationBos;
            ViewBag.ClaimDataPostComputationBos = claimDataPostComputationBos;
            ViewBag.ClaimDataPostValidationBos = claimDataPostValidationBos;
        }

        protected List<SelectListItem> DropDownFileType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataConfigBo.MaxFileType; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataConfigBo.GetFileTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownFileTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownDelimiter(int? delimiter)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataConfigBo.MaxDelimiter; i++)
            {
                var selected = delimiter == i;
                items.Add(new SelectListItem { Text = ClaimDataConfigBo.GetDelimiterName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.DropDownDelimiters = items;
            return items;
        }

        protected List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimDataConfigBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = ClaimDataConfigBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
            return items;
        }

        protected List<string> DropDownComputationMode()
        {
            var items = new List<string>();
            items.Add("Please select");
            for (int i = 1; i <= ClaimDataComputationBo.MaxMode; i++)
            {
                items.Add(ClaimDataComputationBo.GetModeName(i));
            }
            ViewBag.DropDownComputationMode = items;
            return items;
        }

        protected List<string> DropDownComputationTable()
        {
            var items = new List<string>();
            items.Add("Please select");
            for (int i = 1; i <= ClaimDataComputationBo.MaxTable; i++)
            {
                items.Add(ClaimDataComputationBo.GetTableName(i));
            }
            ViewBag.DropDownComputationTable = items;
            return items;
        }

        protected List<List<string>> GetTransformFormulas()
        {
            List<List<string>> items = new List<List<string>> { };
            for (int i = 1; i <= StandardOutputBo.DataTypeMax; i++)
            {
                List<string> formulaList = new List<string>(new string[ClaimDataMappingBo.TransformFormulaMax]);
                foreach (int transformFormula in ClaimDataMappingBo.GetTransformFormulaListByFieldType(i))
                {
                    formulaList.Insert(transformFormula, ClaimDataMappingBo.GetTransformFormulaName(transformFormula));
                }

                items.Add(formulaList);
            }
            ViewBag.TransformFormulaList = items;
            return items;
        }

        public void SetDefaultObjectList()
        {
            List<ClaimDataMappingBo> claimDataMappingBos = ViewBag.ClaimDataMappingBos;
            foreach (ClaimDataMappingBo claimDataMappingBo in claimDataMappingBos)
            {
                if (claimDataMappingBo.IsDefaultValuePickList())
                {
                    List<PickListDetailBo> defaultObjectList = PickListDetailService.GetByStandardClaimDataOutputId(claimDataMappingBo.StandardClaimDataOutputId).ToList();
                    claimDataMappingBo.DefaultObjectList = defaultObjectList.Cast<dynamic>().ToList();
                }
            }
            ViewBag.RiDataMappingBos = claimDataMappingBos;
        }

        public bool IsDisabled(int status, int? id = null)
        {
            ViewBag.Disabled = false;
            ViewBag.InUse = false;
            if (status == ClaimDataConfigBo.StatusPending)
            {
                ViewBag.Disabled = true;
            }
            else if (status == ClaimDataConfigBo.StatusApproved && id != null)
            {
                int countUseInClaimData = ClaimDataFileService.CountByClaimDataConfigIdStatus(id.Value, new int[] { ClaimDataBatchBo.StatusSubmitForProcessing, ClaimDataBatchBo.StatusProcessing });
                int countUseInClaimRegister = ClaimRegisterService.CountByClaimDataConfigIdStatus(id.Value, ClaimRegisterBo.StatusReported);
                if (countUseInClaimData > 0 || countUseInClaimRegister > 0)
                {
                    AddWarningMsg(string.Format(MessageBag.ObjectInUse, "Claim Data Config"));
                    ViewBag.Disabled = true;
                    ViewBag.InUse = true;
                }
            }
            return ViewBag.Disabled;
        }

        [HttpPost]
        public JsonResult GetTreatyByCedant(int cedantId)
        {
            IList<TreatyBo> treatyBos = TreatyService.GetByCedantId(cedantId);
            return Json(new { treatyBos });
        }

        [HttpPost]
        public JsonResult EvaluateObjects(List<Dictionary<string, object>> bos, bool enableRiData, bool enableOriginal)
        {
            bool success = true;
            int index = 1;
            List<string> errors = new List<string>();

            if (bos == null)
                return Json(new { success, errors });

            foreach (Dictionary<string, object> bo in bos)
            {
                List<string> errorList = new List<string>();

                string condition = "";
                if (bo.TryGetValue("Condition", out object con))
                {
                    condition = (string)con;
                }

                int? mode = null;
                if (bo.TryGetValue("Mode", out object result))
                {
                    mode = (int)result;
                }
                bool isEmpty = false;

                if (string.IsNullOrEmpty(condition))
                {
                    isEmpty = true;
                    errorList.Add("Condition is empty");
                }

                string calculationFormula = null;
                if (mode == ClaimDataComputationBo.ModeFormula)
                {
                    if (bo.TryGetValue("CalculationFormula", out object calcFormula))
                    {
                        calculationFormula = (string)calcFormula;
                    }

                    if (string.IsNullOrEmpty(calculationFormula))
                    {
                        isEmpty = true;
                        errorList.Add("Formula is empty");
                    }
                }

                if (!isEmpty)
                {
                    var soe = new StandardClaimDataOutputEval
                    {
                        Condition = condition,
                        Validate = true,
                        EnableRiData = enableRiData,
                        EnableOriginal = enableOriginal,
                    };

                    soe.EvalCondition();

                    if (mode == RiDataComputationBo.ModeFormula)
                    {
                        soe.Formula = calculationFormula;
                        soe.EvalFormula();
                    }

                    errorList = new List<string> { };
                    errorList.AddRange(soe.Errors);
                }

                foreach (string error in errorList)
                {
                    errors.Add(string.Format("{0} at #{1}", error, index));
                }
                index++;
            }

            if (errors.Count() > 0)
                success = false;

            return Json(new { success, errors });
        }

        [HttpPost]
        public JsonResult GetEvaluateVariables(string script, bool enableRiData, bool enableOriginal)
        {
            bool success = true;
            List<string> errors = new List<string>();
            List<StandardClaimDataOutputBo> standardClaimDataOutputBos = new List<StandardClaimDataOutputBo>();

            try
            {
                standardClaimDataOutputBos = StandardClaimDataOutputEval.GetVariables(script, enableRiData, enableOriginal);
            }
            catch (Exception e)
            {
                success = false;
                errors.Add(e.Message);
            }

            return Json(new { success, standardClaimDataOutputBos, errors });
        }

        [HttpPost]
        public JsonResult GetEvaluateResult(string condition, string formula, List<StandardClaimDataOutputBo> bos, bool enableRiData, bool enableOriginal)
        {
            //string condition, string formula, Dictionary< string,string> variables
            bool success = true;
            List<string> errors = new List<string>();
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (bos == null)
                    bos = new List<StandardClaimDataOutputBo>();

                ClaimDataBo claimDataBo = new ClaimDataBo();
                ClaimDataBo originalClaimDataBo = new ClaimDataBo();
                RiDataBo riDataBo = new RiDataBo();
                List<int> numberDataTypes = new List<int>
                {
                    StandardOutputBo.DataTypeAmount,
                    StandardOutputBo.DataTypePercentage,
                };

                foreach (StandardClaimDataOutputBo bo in bos)
                {
                    if (bo.IsRiData)
                    {
                        var property = StandardOutputBo.GetPropertyNameByType(bo.Type);
                        riDataBo.SetRiData(bo.DataType, property, bo.DummyValue);
                    }
                    else
                    {
                        var property = StandardClaimDataOutputBo.GetPropertyNameByType(bo.Type);
                        if (bo.IsOriginal)
                        {
                            originalClaimDataBo.SetClaimData(bo.DataType, property, bo.DummyValue);
                        }
                        else
                        {
                            claimDataBo.SetClaimData(bo.DataType, property, bo.DummyValue);
                        }
                    }

                }

                var soe = new StandardClaimDataOutputEval()
                {
                    Condition = condition,
                    Formula = formula,
                    EnableRiData = enableRiData,
                    EnableOriginal = enableOriginal,

                    ClaimDataBo = claimDataBo,
                    OriginalClaimDataBo = originalClaimDataBo,
                    RiDataBo = riDataBo,
                };

                var valid = soe.EvalCondition();
                if (soe.Errors.Count > 0)
                {
                    throw new Exception(soe.Errors[0]);
                }
                result["Condition"] = soe.Condition;
                result["Formatted Condition"] = soe.FormattedCondition;
                result["Condition Result"] = valid.ToString();

                if (valid && soe.Formula != null && soe.Formula.Length > 0)
                {
                    result[""] = "";

                    var finalResult = soe.EvalFormula();

                    if (finalResult == null)
                    {
                        success = false;
                        errors.AddRange(soe.Errors);
                    }
                    else
                    {
                        result["Formula"] = soe.Formula;
                        result["Formatted Formula"] = soe.FormattedFormula;
                        result["Result"] = finalResult != null ? finalResult.ToString() : "";
                    }
                }
            }
            catch (Exception e)
            {
                success = false;
                errors.Add(e.Message);
            }

            return Json(new { success, result, errors });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public FileResult Export(FormCollection form, ClaimDataConfigViewModel model)
        {
            model.CurrentForm = form;
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(model.Id);
            if (claimDataConfigBo != null && IsDisabled(claimDataConfigBo.Status, claimDataConfigBo.Id))
            {
                claimDataConfigBo.ClaimDataMappingBos = ClaimDataMappingService.GetByClaimDataConfigId(model.Id);
                claimDataConfigBo.ClaimDataComputationBos = ClaimDataComputationService.GetByClaimDataConfigId(model.Id);
                claimDataConfigBo.ClaimDataValidationBos = ClaimDataValidationService.GetByClaimDataConfigId(model.Id);
            }
            else
            {
                CedantBo cedantBo = CedantService.Find(model.CedantId);
                TreatyBo treatyBo = TreatyService.Find(model.TreatyId);

                model.SetChildItems();

                claimDataConfigBo = new ClaimDataConfigBo();
                model.Get(ref claimDataConfigBo);

                claimDataConfigBo.CedantName = cedantBo?.Name;
                claimDataConfigBo.TreatyIdCode = treatyBo?.TreatyIdCode;
                claimDataConfigBo.FileTypeName = ClaimDataConfigBo.GetFileTypeName(model.FileType);
                claimDataConfigBo.ClaimDataFileConfig.DelimiterName = ClaimDataConfigBo.GetDelimiterName(model.Delimiter);
            }

            string output = JsonConvert.SerializeObject(claimDataConfigBo);
            var contentType = "application/json";
            return File(Encoding.ASCII.GetBytes(output), contentType, string.Format("ClaimDataConfig_{0}", claimDataConfigBo.Code).AppendDateTimeFileName(".json"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Import(FormCollection form, ClaimDataConfigViewModel model)
        {
            ModelState.Clear();
            model.CurrentForm = form;
            int id = model.Id;

            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            model.SetChildItems();

            HttpPostedFileBase configurationFile = Request.Files["importFile"];
            try
            {
                using (StreamReader reader = new StreamReader(configurationFile.InputStream))
                {
                    string content = reader.ReadToEnd();
                    if ((!content.StartsWith("{") || !content.EndsWith("}")))
                    {
                        throw new Exception("There is an error with the format of the configuration file");
                    }

                    ClaimDataConfigBo claimDataConfigBo = JsonConvert.DeserializeObject<ClaimDataConfigBo>(content);
                    claimDataConfigBo.CedantBo = CedantService.FindByName(claimDataConfigBo.CedantName);
                    claimDataConfigBo.TreatyBo = TreatyService.FindByCode(claimDataConfigBo.TreatyIdCode);

                    ClaimDataConfigViewModel newModel = new ClaimDataConfigViewModel(claimDataConfigBo);
                    newModel.SetChildItems(claimDataConfigBo);
                    model = newModel;
                    model.Id = id;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            ViewBag.ClaimDataMappingBos = (IList<ClaimDataMappingBo>)model.GetChildBos("ClaimDataMapping");
            ViewBag.ClaimDataComputationBos = model.GetComputationBosWithStep(ClaimDataComputationBo.StepPreComputation);
            ViewBag.ClaimDataPreValidationBos = model.GetValidationBosWithStep(ClaimDataValidationBo.StepPreValidation);
            ViewBag.ClaimDataPostComputationBos = model.GetComputationBosWithStep(ClaimDataComputationBo.StepPostComputation);
            ViewBag.ClaimDataPostValidationBos = model.GetValidationBosWithStep(ClaimDataValidationBo.StepPostValidation);

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                //ModuleBo moduleBo = ModuleService.FindByController(Controller);
                GetStatusHistory(model.ModuleId, model.Id);
                GetRemark(model.ModuleId, model.Id);
            }

            LoadContent(model);
            SetDefaultObjectList();
            IsDisabled(model.Status);
            return View(view, model);
        }
    }
}
