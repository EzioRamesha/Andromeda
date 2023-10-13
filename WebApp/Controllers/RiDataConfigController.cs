using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public class RiDataConfigController : BaseController
    {
        public const string Controller = "RiDataConfig";

        // GET: RiDataConfig
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            string Name,
            int? CedantId,
            string TreatyId,
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

            var query = _db.RiDataConfigs.Select(RiDataConfigViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Name)) query = query.Where(q => q.Name.Contains(Name));
            if (CedantId != null) query = query.Where(q => q.CedantId == CedantId);
            if (!string.IsNullOrEmpty(TreatyId)) {
                string[] TreatyIds = TreatyId.Split(',');
                query = query.Where(q => TreatyIds.Contains(q.TreatyId.ToString()));
            }
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

        // GET: RiDataConfig/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            RiDataConfigViewModel model = new RiDataConfigViewModel();
            IsDisabled(model.Status);
            LoadPage(model);
            return View(model);
        }

        // POST: RiDataConfig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RiDataConfigViewModel model)
        {
            model.CurrentForm = form;
            if (ModelState.IsValid)
            {
                var riDataConfigBo = new RiDataConfigBo
                {
                    CedantId = model.CedantId,
                    TreatyId = model.TreatyId == 0 ? null : model.TreatyId,
                    Status = model.Status,
                    Code = model.Code?.Trim(),
                    Name = model.Name?.Trim(),
                    FileType = model.FileType,
                    RiDataFileConfig = new RiDataFileConfig
                    {
                        HasHeader = model.HasHeader,
                        HeaderRow = model.HeaderRow,
                        Worksheet = model.Worksheet,
                        Delimiter = model.Delimiter,
                        RemoveQuote = model.RemoveQuote,
                        StartRow = model.StartRow,
                        EndRow = model.EndRow,
                        StartColumn = model.StartColumn,
                        EndColumn = model.EndColumn,
                        IsColumnToRowMapping = model.IsColumnToRowMapping,
                        NumberOfRowMapping = model.NumberOfRowMapping,
                        IsDataCorrection = model.IsDataCorrection,
                    },
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                bool validateFormula = riDataConfigBo.Status == RiDataConfigBo.StatusDraft ? false : true;

                model.SetItems();
                Result childResult = model.ValidateChildItems(validateFormula);

                if (childResult.Valid)
                {
                    TrailObject trail = GetNewTrailObject();
                    Result = RiDataConfigService.Create(ref riDataConfigBo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = riDataConfigBo.Id;
                        model.Status = riDataConfigBo.Status;
                        model.SaveChildItems(AuthUserId, ref trail);

                        RemarkController.SaveRemarks(RemarkController.GetRemarks(form), model.ModuleId, riDataConfigBo.Id, AuthUserId, ref trail);

                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(
                            riDataConfigBo.Id,
                            "Create RI Data Config"
                        );

                        model.Id = riDataConfigBo.Id;

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { riDataConfigBo.Id });
                    }
                }
                else
                {
                    Result = childResult;
                }
                AddResult(Result);
            }

            model.Status = RiDataConfigBo.StatusDraft;
            IsDisabled(model.Status);
            LoadPage(model, form);
            return View(model);
        }

        // GET: RiDataConfig/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }
            RiDataConfigViewModel model = new RiDataConfigViewModel(riDataConfigBo);
            CheckWorkgroupPower(model.CedantId);
            IsDisabled(riDataConfigBo.Status, riDataConfigBo.Id);
            LoadPage(model);
            return View(model);
        }

        // POST: RiDataConfig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RiDataConfigViewModel model)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            List<string> msg = new List<string>();
            model.CurrentForm = form;
            if (ModelState.IsValid)
            {
                riDataConfigBo.CedantId = model.CedantId;
                riDataConfigBo.TreatyId = model.TreatyId == 0 ? null : model.TreatyId;
                riDataConfigBo.Code = model.Code?.Trim();
                riDataConfigBo.Name = model.Name?.Trim();
                riDataConfigBo.FileType = model.FileType;
                riDataConfigBo.RiDataFileConfig.HasHeader = model.HasHeader;
                riDataConfigBo.RiDataFileConfig.HeaderRow = model.HeaderRow;
                riDataConfigBo.RiDataFileConfig.Worksheet = model.FileType == RiDataConfigBo.FileTypeExcel ? model.Worksheet : null;
                riDataConfigBo.RiDataFileConfig.Delimiter = model.FileType == RiDataConfigBo.FileTypePlainText ? model.Delimiter : null;
                riDataConfigBo.RiDataFileConfig.RemoveQuote = model.RemoveQuote;
                riDataConfigBo.RiDataFileConfig.StartRow = model.StartRow;
                riDataConfigBo.RiDataFileConfig.EndRow = model.EndRow;
                riDataConfigBo.RiDataFileConfig.StartColumn = model.StartColumn;
                riDataConfigBo.RiDataFileConfig.EndColumn = model.EndColumn;
                riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping = model.IsColumnToRowMapping;
                riDataConfigBo.RiDataFileConfig.NumberOfRowMapping = model.NumberOfRowMapping;
                riDataConfigBo.RiDataFileConfig.IsDataCorrection = model.IsDataCorrection;
                riDataConfigBo.UpdatedById = AuthUserId;

                bool validateFormula = model.Status == RiDataConfigBo.StatusDraft ? false : true;

                model.SetItems();
                Result childResult = model.ValidateChildItems(validateFormula);

                if (childResult.Valid)
                {
                    riDataConfigBo.Status = model.Status;

                    TrailObject trail = GetNewTrailObject();
                    Result = RiDataConfigService.Update(ref riDataConfigBo, ref trail);
                    if (Result.Valid)
                    {
                        model.SaveChildItems(AuthUserId, ref trail);
                        model.ProcessStatusHistory(form, AuthUserId, ref trail);

                        CreateTrail(
                            riDataConfigBo.Id,
                            "Update RiDataConfig"
                        );

                        if (riDataConfigBo.Status == RiDataConfigBo.StatusDraft)
                        {
                            SetUpdateSuccessMessage(Controller);
                        }
                        else
                        {
                            SetSuccessSessionMsg(string.Format(MessageBag.ObjectSubmitted, Controller));
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

            model.Status = riDataConfigBo.Status;
            CheckWorkgroupPower(model.CedantId);
            IsDisabled(riDataConfigBo.Status, riDataConfigBo.Id);
            LoadPage(model, form);
            return View(model);
        }

        // POST: RiDataConfig/Approve
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "A")]
        public ActionResult Approve(int id, FormCollection form)
        {
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            riDataConfigBo.Status = RiDataConfigBo.StatusApproved;
            riDataConfigBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = RiDataConfigService.Update(ref riDataConfigBo, ref trail);
            if (Result.Valid)
            {
                var model = new RiDataConfigViewModel
                {
                    Id = id,
                    Status = riDataConfigBo.Status
                };
                model.ProcessStatusHistory(form, AuthUserId, ref trail);

                CreateTrail(
                    riDataConfigBo.Id,
                    "Update RiDataConfig"
                );

                SetSuccessSessionMsg(string.Format(MessageBag.ObjectApproved, Controller));
            }

            return RedirectToAction("Edit", new { Id = id });
        }

        // POST: RiDataConfig/Reject
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "A")]
        public ActionResult Reject(int id, FormCollection form)
        {
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            riDataConfigBo.Status = RiDataConfigBo.StatusRejected;
            riDataConfigBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = RiDataConfigService.Update(ref riDataConfigBo, ref trail);
            if (Result.Valid)
            {
                var model = new RiDataConfigViewModel
                {
                    Id = id,
                    Status = riDataConfigBo.Status
                };
                model.ProcessStatusHistory(form, AuthUserId, ref trail);

                CreateTrail(
                    riDataConfigBo.Id,
                    "Update RiDataConfig"
                );

                SetSuccessSessionMsg(string.Format(MessageBag.ObjectRejected, Controller));
            }

            return RedirectToAction("Edit", new { Id = id });
        }

        // POST: RiDataConfig/Disable
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Disable(int id, FormCollection form)
        {
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            riDataConfigBo.Status = RiDataConfigBo.StatusDisabled;
            riDataConfigBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = RiDataConfigService.Update(ref riDataConfigBo, ref trail);
            if (Result.Valid)
            {
                var model = new RiDataConfigViewModel
                {
                    Id = id,
                    Status = riDataConfigBo.Status
                };
                model.ProcessStatusHistory(form, AuthUserId, ref trail);

                CreateTrail(
                    riDataConfigBo.Id,
                    "Update RiDataConfig"
                );

                SetSuccessSessionMsg(string.Format(MessageBag.ObjectDisabled, Controller));
            }

            return RedirectToAction("Edit", new { Id = id });
        }

        // GET: RiDataConfig/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            List<string> msg = new List<string>();
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }
            RiDataConfigViewModel model = new RiDataConfigViewModel(riDataConfigBo);
            ViewBag.Disabled = true;
            LoadPage(model);
            return View(model);
        }

        // POST: RiDataConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, RiDataConfigViewModel model)
        {
            RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(id);
            if (riDataConfigBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = RiDataConfigService.Delete(riDataConfigBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    riDataConfigBo.Id,
                    "Delete RiDataConfig"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = riDataConfigBo.Id });
        }

        [HttpPost]
        public JsonResult GetTreatyByCedant(int cedantId)
        {
            IList<TreatyBo> treatyBos = TreatyService.GetByCedantId(cedantId);
            return Json(new { treatyBos });
        }

        [HttpPost]
        public JsonResult EvaluateObjects(List<Dictionary<string, object>> bos, bool enableOriginal)
        {
            //List<object> bos = (List<object>)JsonConvert.DeserializeObject(json);
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
                if (mode == RiDataComputationBo.ModeFormula)
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
                    var soe = new StandardOutputEval
                    {
                        Condition = condition,
                        EnableOriginal = enableOriginal
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
        public JsonResult GetEvaluateVariables(string script, bool enableOriginal)
        {
            bool success = true;
            bool hasQuarter = false;
            bool hasOriQuarter = false;
            List<string> errors = new List<string>();
            List<StandardOutputBo> standardOutputBos = new List<StandardOutputBo>();

            try
            {
                standardOutputBos = StandardOutputEval.GetVariables(script, ref hasQuarter, ref hasOriQuarter, enableOriginal);
            }
            catch (Exception e)
            {
                success = false;
                errors.Add(e.Message);
            }

            return Json(new { success, standardOutputBos, hasQuarter, hasOriQuarter, errors });
        }

        [HttpPost]
        public JsonResult GetEvaluateResult(string condition, string formula, List<StandardOutputBo> standardOutputBos, string quarter, string oriQuarter, bool enableOriginal)
        {
            //string condition, string formula, Dictionary< string,string> variables
            bool success = true;
            List<string> errors = new List<string>();
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (standardOutputBos == null)
                    standardOutputBos = new List<StandardOutputBo>();

                RiDataBo riDataBo = new RiDataBo();
                RiDataBo originalRiDataBo = new RiDataBo();
                List<int> numberDataTypes = new List<int>
                {
                    StandardOutputBo.DataTypeAmount,
                    StandardOutputBo.DataTypePercentage,
                };

                foreach (StandardOutputBo standardOutputBo in standardOutputBos)
                {
                    var property = StandardOutputBo.GetPropertyNameByType(standardOutputBo.Type);

                    if (standardOutputBo.IsOriginal)
                    {
                        originalRiDataBo.SetRiData(standardOutputBo.DataType, property, standardOutputBo.DummyValue);
                    }
                    else
                    {
                        riDataBo.SetRiData(standardOutputBo.DataType, property, standardOutputBo.DummyValue);
                    }
                }

                var soe = new StandardOutputEval()
                {
                    Condition = condition,
                    Formula = formula,

                    RiDataBo = riDataBo,
                    OriginalRiDataBo = originalRiDataBo,

                    Quarter = quarter,
                    OriginalQuarter = oriQuarter,
                    EnableOriginal = enableOriginal
                };

                var valid = soe.EvalCondition();
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
        public FileResult Export(FormCollection form, RiDataConfigViewModel model)
        {
            model.CurrentForm = form;
            RiDataConfigBo riDataConfigBo = null;
            if (model.Id != 0)
            {
                riDataConfigBo = RiDataConfigService.Find(model.Id);
                if (riDataConfigBo != null && IsDisabled(riDataConfigBo.Status, riDataConfigBo.Id))
                {
                    riDataConfigBo.RiDataMappingBos = RiDataMappingService.GetByRiDataConfigId(model.Id);
                    riDataConfigBo.RiDataComputationBos = RiDataComputationService.GetByRiDataConfigId(model.Id);
                    riDataConfigBo.RiDataPreValidationBos = RiDataPreValidationService.GetByRiDataConfigId(model.Id);
                }
                else
                {
                    riDataConfigBo = null;
                }
            }

            if (riDataConfigBo == null)
            {
                CedantBo cedantBo = CedantService.Find(model.CedantId);
                TreatyBo treatyBo = TreatyService.Find(model.TreatyId);

                model.SetItems();
                riDataConfigBo = new RiDataConfigBo
                {
                    CedantName = cedantBo != null ? cedantBo.Name : null,
                    TreatyIdCode = treatyBo != null ? treatyBo.TreatyIdCode : null,
                    Code = model.Code,
                    Name = model.Name,
                    FileType = model.FileType,
                    FileTypeName = RiDataConfigBo.GetFileTypeName(model.FileType),
                    RiDataFileConfig = new RiDataFileConfig
                    {
                        HasHeader = model.HasHeader,
                        HeaderRow = model.HeaderRow,
                        Worksheet = model.Worksheet,
                        Delimiter = model.Delimiter,
                        DelimiterName = RiDataConfigBo.GetDelimiterName(model.Delimiter),
                        RemoveQuote = model.RemoveQuote,
                        StartRow = model.StartRow,
                        EndRow = model.EndRow,
                        StartColumn = model.StartColumn,
                        EndColumn = model.EndColumn,
                        IsColumnToRowMapping = model.IsColumnToRowMapping,
                        NumberOfRowMapping = model.NumberOfRowMapping,
                        IsDataCorrection = model.IsDataCorrection,
                    },
                    RiDataMappingBos = (IList<RiDataMappingBo>)model.GetChildBos("RiDataMapping"),
                    RiDataComputationBos = (IList<RiDataComputationBo>)model.GetChildBos("RiDataComputation"),
                    RiDataPreValidationBos = (IList<RiDataPreValidationBo>)model.GetChildBos("RiDataPreValidation"),
                };
            }

            string output = JsonConvert.SerializeObject(riDataConfigBo);
            var contentType = "application/json";
            return File(Encoding.ASCII.GetBytes(output), contentType, string.Format("RIDataConfig_{0}", riDataConfigBo.Code).AppendDateTimeFileName(".json"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = "RiDataConfig", Power = "R")]
        public ActionResult Import(FormCollection form, RiDataConfigViewModel model)
        {
            ModelState.Clear();
            model.CurrentForm = form;

            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            model.SetItems();
            List<RiDataMappingBo> riDataMappingBos = (List<RiDataMappingBo>)model.GetChildBos("RiDataMapping");
            List<RiDataComputationBo> riDataComputationBos = (List<RiDataComputationBo>)model.GetChildBos("RiDataComputation");
            List<RiDataPreValidationBo> riDataPreValidationBos = (List<RiDataPreValidationBo>)model.GetChildBos("RiDataPreValidation");

            //var importFile = form.Get("importFile");
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

                    RiDataConfigBo riDataConfigBo = JsonConvert.DeserializeObject<RiDataConfigBo>(content);

                    CedantBo cedantBo = CedantService.FindByName(riDataConfigBo.CedantName);
                    if (cedantBo != null)
                    {
                        model.CedantId = cedantBo.Id;
                        model.CedantName = cedantBo.Name;
                    }

                    TreatyBo treatyBo = TreatyService.FindByCode(riDataConfigBo.TreatyIdCode);
                    if (treatyBo != null)
                    {
                        model.TreatyId = treatyBo.Id;
                        model.TreatyIdCode = treatyBo.TreatyIdCode;
                    }

                    model.Code = riDataConfigBo.Code;
                    model.Name = riDataConfigBo.Name;
                    model.FileType = riDataConfigBo.FileType;
                    model.HasHeader = riDataConfigBo.RiDataFileConfig.HasHeader;
                    model.HeaderRow = riDataConfigBo.RiDataFileConfig.HeaderRow;
                    model.Worksheet = riDataConfigBo.RiDataFileConfig.Worksheet;
                    model.Delimiter = riDataConfigBo.RiDataFileConfig.Delimiter;
                    model.RemoveQuote = riDataConfigBo.RiDataFileConfig.RemoveQuote;
                    model.StartRow = riDataConfigBo.RiDataFileConfig.StartRow;
                    model.EndRow = riDataConfigBo.RiDataFileConfig.EndRow;
                    model.StartColumn = riDataConfigBo.RiDataFileConfig.StartColumn;
                    model.EndColumn = riDataConfigBo.RiDataFileConfig.EndColumn;
                    model.IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping;
                    model.NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping;
                    model.IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection;

                    foreach (RiDataMappingBo riDataMappingBo in riDataConfigBo.RiDataMappingBos)
                    {
                        int standardOutputId = StandardOutputBo.GetTypeByCode(riDataMappingBo.StandardOutputCode); // NOTE: Type == StandardOutputId
                        riDataMappingBo.StandardOutputId = standardOutputId;
                        riDataMappingBo.StandardOutputBo = StandardOutputService.Find(standardOutputId);

                        if (riDataMappingBo.IsDefaultValuePickList())
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardOutputIdCode(standardOutputId, riDataMappingBo.DefaultValue);
                            riDataMappingBo.DefaultObjectId = pickListDetailBo?.Id;
                        }
                        else if (riDataMappingBo.IsDefaultValueBenefit())
                        {
                            BenefitBo benefitBo = BenefitService.FindByCode(riDataMappingBo.DefaultValue);
                            riDataMappingBo.DefaultObjectId = benefitBo?.Id;
                        }
                        else if (riDataMappingBo.IsDefaultValueTreatyCode())
                        {
                            TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(riDataMappingBo.DefaultValue);
                            riDataMappingBo.DefaultObjectId = treatyCodeBo?.Id;
                        }

                        if (riDataMappingBo.RiDataMappingDetailBos != null)
                        {
                            foreach (RiDataMappingDetailBo riDataMappingDetailBo in riDataMappingBo.RiDataMappingDetailBos)
                            {
                                PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardOutputIdCode(standardOutputId, riDataMappingDetailBo.PickListDetailCode);
                                riDataMappingDetailBo.PickListDetailId = pickListDetailBo != null ? pickListDetailBo.Id : (int?)null;
                            }
                        }
                    }

                    foreach (RiDataComputationBo riDataComputationBo in riDataConfigBo.RiDataComputationBos)
                    {
                        int key = StandardOutputBo.GetTypeByCode(riDataComputationBo.StandardOutputCode);
                        StandardOutputBo standardOutputBo = StandardOutputService.Find(key);
                        riDataComputationBo.StandardOutputId = standardOutputBo != null ? standardOutputBo.Id : 0;
                    }

                    riDataMappingBos = new List<RiDataMappingBo>(riDataConfigBo.RiDataMappingBos);
                    riDataComputationBos = new List<RiDataComputationBo>(riDataConfigBo.RiDataComputationBos);
                    riDataPreValidationBos = new List<RiDataPreValidationBo>(riDataConfigBo.RiDataPreValidationBos);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            ViewBag.RiDataMappingBos = riDataMappingBos;
            ViewBag.RiDataPreComputation1Bos = riDataComputationBos.Where(q => q.Step == RiDataComputationBo.StepPreComputation1).ToList(); 
            ViewBag.RiDataPreComputation2Bos = riDataComputationBos.Where(q => q.Step == RiDataComputationBo.StepPreComputation2).ToList(); 
            ViewBag.RiDataPreValidationBos = riDataPreValidationBos.Where(q => q.Step == RiDataPreValidationBo.StepPreValidation).ToList();
            ViewBag.RiDataPostComputationBos = riDataComputationBos.Where(q => q.Step == RiDataComputationBo.StepPostComputation).ToList();
            ViewBag.RiDataPostValidationBos = riDataPreValidationBos.Where(q => q.Step == RiDataPreValidationBo.StepPostValidation).ToList(); 

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiDataConfig.ToString());
                GetStatusHistory(model.ModuleId, model.Id);
                GetRemark(model.ModuleId, model.Id);
            }

            LoadContent(model);
            SetDefaultObjectList();
            IsDisabled(model.Status);
            return View(view, model);
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownCedant();
            //DropDownTreaty();
            DropDownFileType();
            DropDownStatus();
            SetViewBagMessage();
        }

        public void LoadContent(RiDataConfigViewModel model)
        {
            DropDownCedant(CedantBo.StatusActive, model.CedantId, "0", true);
            if (model.CedantBo != null && model.CedantBo.Status == CedantBo.StatusInactive)
                AddWarningMsg(MessageBag.CedantStatusInactive);

            DropDownTreaty(model.TreatyId, model.CedantId);

            DropDownFileType();
            DropDownDelimiter(model.Delimiter);
            GetTransformFormulas();
            DropDownComputationMode();
            DropDownComputationTable();
            DropDownComputationRiskDate();
            GetBenefits();

            ViewBag.StdOutputList = StandardOutputService.Get();
            ViewBag.AuthUserName = AuthUser.FullName;
            ViewBag.ApprovalPower = CheckPower(Controller, AccessMatrixBo.PowerApproval);

            SetViewBagMessage();
        }

        public void LoadPage(RiDataConfigViewModel model, FormCollection form = null)
        {
            LoadContent(model);
            LoadChildItems(model, form);
            SetDefaultObjectList();
            GetStatusHistory(model.ModuleId, model.Id);
            GetRemark(model.ModuleId, model.Id);
            DropDownStatus();

            SetViewBagMessage();
        }

        protected List<SelectListItem> DropDownFileType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataConfigBo.MaxFileType; i++)
            {
                items.Add(new SelectListItem { Text = RiDataConfigBo.GetFileTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownFileTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownDelimiter(int? delimiter)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataConfigBo.MaxDelimiter; i++)
            {
                var selected = delimiter == i;
                items.Add(new SelectListItem { Text = RiDataConfigBo.GetDelimiterName(i), Value = i.ToString(), Selected = selected });
            }
            ViewBag.DropDownDelimiters = items;
            return items;
        }

        protected List<List<string>> GetTransformFormulas()
        {
            List<List<string>> items = new List<List<string>> { };
            for (int i = 1; i <= StandardOutputBo.DataTypeMax; i++)
            {
                List<string> formulaList = new List<string>(new string[RiDataMappingBo.TransformFormulaMax]);
                foreach (int transformFormula in RiDataMappingBo.GetTransformFormulaListByFieldType(i))
                {
                    formulaList.Insert(transformFormula, RiDataMappingBo.GetTransformFormulaName(transformFormula));
                }

                items.Add(formulaList);
            }
            ViewBag.TransformFormulaList = items;
            return items;
        }

        protected List<string> DropDownComputationMode()
        {
            var items = new List<string>();
            items.Add("Please select");
            for (int i = 1; i <= RiDataComputationBo.MaxMode; i++)
            {
                items.Add(RiDataComputationBo.GetModeName(i));
            }
            ViewBag.DropDownComputationMode = items;
            return items;
        }

        protected List<SelectListItem> DropDownComputationTable()
        {
            var items = new List<SelectListItem>();
            for (int i = 1; i <= RiDataComputationBo.MaxTable; i++)
            {
                items.Add(new SelectListItem { Text = RiDataComputationBo.GetTableName(i), Value = i.ToString() });
            }
            var sortedItems = items.OrderBy(q => q.Text).ToList();
            sortedItems.Insert(0, new SelectListItem { Text = "Please select", Value = "" });

            ViewBag.DropDownComputationTable = sortedItems;
            return sortedItems;
        }

        protected List<SelectListItem> DropDownComputationRiskDate()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataComputationBo.MaxRiskDateOption; i++)
            {
                items.Add(new SelectListItem { Text = RiDataComputationBo.GetRiskDateOptionName(i), Value = i.ToString() });
            }
            ViewBag.DropDownComputationRiskDate = items;
            return items;
        }

        protected List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataConfigBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = RiDataConfigBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
            return items;
        }

        public void LoadChildItems(RiDataConfigViewModel model, FormCollection form = null)
        {
            IList<RiDataMappingBo> riDataMappingBos;
            IList<RiDataComputationBo> riDataPreComputation1Bos;
            IList<RiDataComputationBo> riDataPreComputation2Bos;
            IList<RiDataPreValidationBo> riDataPreValidationBos;
            IList<RiDataComputationBo> riDataPostComputationBos;
            IList<RiDataPreValidationBo> riDataPostValidationBos;

            if (form != null)
            {
                model.SetItems();
                riDataMappingBos = (IList<RiDataMappingBo>)model.GetChildBos("RiDataMapping");
                riDataPreComputation1Bos = model.GetComputationBosWithStep(RiDataComputationBo.StepPreComputation1);
                riDataPreComputation2Bos = model.GetComputationBosWithStep(RiDataComputationBo.StepPreComputation2);
                riDataPreValidationBos = model.GetValidationBosWithStep(RiDataPreValidationBo.StepPreValidation);
                riDataPostComputationBos = model.GetComputationBosWithStep(RiDataComputationBo.StepPostComputation);
                riDataPostValidationBos = model.GetValidationBosWithStep(RiDataPreValidationBo.StepPostValidation);
            }
            else if (model.Id != 0)
            {
                riDataMappingBos = RiDataMappingService.GetByRiDataConfigId(model.Id);
                riDataPreComputation1Bos = RiDataComputationService.GetByRiDataConfigId(model.Id, RiDataComputationBo.StepPreComputation1);
                riDataPreComputation2Bos = RiDataComputationService.GetByRiDataConfigId(model.Id, RiDataComputationBo.StepPreComputation2);
                riDataPreValidationBos = RiDataPreValidationService.GetByRiDataConfigId(model.Id, RiDataPreValidationBo.StepPreValidation);
                riDataPostComputationBos = RiDataComputationService.GetByRiDataConfigId(model.Id, RiDataComputationBo.StepPostComputation);
                riDataPostValidationBos = RiDataPreValidationService.GetByRiDataConfigId(model.Id, RiDataPreValidationBo.StepPostValidation);
            }
            else
            {
                List<RiDataComputationBo> defaultComputations = RiDataComputationBo.GetDefault();
                List<RiDataPreValidationBo> defaultValidations = RiDataPreValidationBo.GetDefault();

                riDataMappingBos = RiDataMappingBo.GetDefault();
                riDataPreComputation1Bos = defaultComputations.Where(q => q.Step == RiDataComputationBo.StepPreComputation1).ToList();
                riDataPreComputation2Bos = defaultComputations.Where(q => q.Step == RiDataComputationBo.StepPreComputation2).ToList();
                riDataPreValidationBos = defaultValidations.Where(q => q.Step == RiDataPreValidationBo.StepPreValidation).ToList();
                riDataPostComputationBos = defaultComputations.Where(q => q.Step == RiDataComputationBo.StepPostComputation).ToList();
                riDataPostValidationBos = defaultValidations.Where(q => q.Step == RiDataPreValidationBo.StepPostValidation).ToList();
            }

            ViewBag.RiDataMappingBos = riDataMappingBos;
            ViewBag.RiDataPreComputation1Bos = riDataPreComputation1Bos;
            ViewBag.RiDataPreComputation2Bos = riDataPreComputation2Bos;
            ViewBag.RiDataPreValidationBos = riDataPreValidationBos;
            ViewBag.RiDataPostComputationBos = riDataPostComputationBos;
            ViewBag.RiDataPostValidationBos = riDataPostValidationBos;
        }

        public void SetDefaultObjectList()
        {
            List<RiDataMappingBo> riDataMappingBos = ViewBag.RiDataMappingBos;
            foreach (RiDataMappingBo riDataMappingBo in riDataMappingBos)
            {
                if (riDataMappingBo.IsDefaultValuePickList())
                {
                    List<PickListDetailBo> defaultObjectList = PickListDetailService.GetByStandardOutputId(riDataMappingBo.StandardOutputId).ToList();
                    riDataMappingBo.DefaultObjectList = defaultObjectList.Cast<dynamic>().ToList();
                }
            }
            ViewBag.RiDataMappingBos = riDataMappingBos;
        }

        public bool IsDisabled(int status, int? id = null)
        {
            ViewBag.Disabled = false;
            ViewBag.InUse = false;
            if (status == RiDataConfigBo.StatusPending)
            {
                ViewBag.Disabled = true;
            }
            else if (status == RiDataConfigBo.StatusApproved && 
                id != null && 
                RiDataFileService.CountByRiDataConfigIdStatus(id.Value, new int[] { RiDataBatchBo.StatusSubmitForPreProcessing, RiDataBatchBo.StatusPreProcessing, RiDataBatchBo.StatusSubmitForPostProcessing, RiDataBatchBo.StatusPostProcessing }) > 0
            )
            {
                AddWarningMsg(string.Format(MessageBag.ObjectInUse, "RI Data Config"));
                ViewBag.Disabled = true;
                ViewBag.InUse = true;
            }
            return ViewBag.Disabled;
        }
    }
}
