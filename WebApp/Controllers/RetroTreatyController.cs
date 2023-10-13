using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Services.Retrocession;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
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
    public class RetroTreatyController : BaseController
    {
        public const string Controller = "RetroTreaty";

        // GET: RetroTreaty
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? RetroPartyId,
            string Code,
            int? TreatyTypePickListDetailId,
            string LineOfBusiness,
            string RetroShare,
            int? TreatyDiscountTableId,
            string EffectiveStartDate,
            string EffectiveEndDate,
            int? Status,
            string SortOrder,
            int? Page)
        {
            DateTime? effectiveStartDate = Util.GetParseDateTime(EffectiveStartDate);
            DateTime? effectiveEndDate = Util.GetParseDateTime(EffectiveEndDate);

            double? retroShare = Util.StringToDouble(RetroShare);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["RetroPartyId"] = RetroPartyId,
                ["Code"] = Code,
                ["TreatyTypePickListDetailId"] = TreatyTypePickListDetailId,
                ["LineOfBusiness"] = LineOfBusiness,
                ["RetroShare"] = retroShare.HasValue ? RetroShare : null,
                ["TreatyDiscountTableId"] = TreatyDiscountTableId,
                ["EffectiveStartDate"] = effectiveStartDate.HasValue ? EffectiveStartDate : null,
                ["EffectiveEndDate"] = effectiveEndDate.HasValue ? EffectiveEndDate : null,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortRetroPartyId = GetSortParam("RetroPartyId");
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortTreatyTypePickListDetailId = GetSortParam("TreatyTypePickListDetailId");
            ViewBag.SortRetroShare = GetSortParam("RetroShare");
            ViewBag.SortTreatyDiscountTableId = GetSortParam("TreatyDiscountTableId");
            ViewBag.SortEffectiveStartDate = GetSortParam("EffectiveStartDate");
            ViewBag.SortEffectiveEndDate = GetSortParam("EffectiveEndDate");
            ViewBag.SortStatus = GetSortParam("Status");

            var query = _db.RetroTreaties.Select(RetroTreatyViewModel.Expression());

            //if (!string.IsNullOrEmpty(Party)) query = query.Where(q => q.Party.Contains(Party));
            if (RetroPartyId.HasValue) query = query.Where(q => q.RetroPartyId == RetroPartyId);
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
            if (TreatyTypePickListDetailId.HasValue) query = query.Where(q => q.TreatyTypePickListDetailId == TreatyTypePickListDetailId);
            if (retroShare.HasValue) query = query.Where(q => q.RetroShare == retroShare);
            if (TreatyDiscountTableId.HasValue) query = query.Where(q => q.TreatyDiscountTableId == TreatyDiscountTableId);
            if (effectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate == effectiveStartDate);
            if (effectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate == effectiveEndDate);
            if (Status.HasValue) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("RetroPartyId")) query = query.OrderBy(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortDsc("RetroPartyId")) query = query.OrderByDescending(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("TreatyTypePickListDetailId")) query = query.OrderBy(q => q.TreatyTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyTypePickListDetailId")) query = query.OrderByDescending(q => q.TreatyTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("RetroShare")) query = query.OrderBy(q => q.RetroShare);
            else if (SortOrder == Html.GetSortDsc("RetroShare")) query = query.OrderByDescending(q => q.RetroShare);
            else if (SortOrder == Html.GetSortAsc("TreatyDiscountTableId")) query = query.OrderBy(q => q.TreatyDiscountTable.Rule);
            else if (SortOrder == Html.GetSortDsc("TreatyDiscountTableId")) query = query.OrderByDescending(q => q.TreatyDiscountTable.Rule);
            else if (SortOrder == Html.GetSortAsc("EffectiveStartDate")) query = query.OrderBy(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveStartDate")) query = query.OrderByDescending(q => q.EffectiveStartDate);
            else if (SortOrder == Html.GetSortAsc("EffectiveEndDate")) query = query.OrderBy(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveEndDate")) query = query.OrderByDescending(q => q.EffectiveEndDate);
            else if (SortOrder == Html.GetSortAsc("Status")) query = query.OrderBy(q => q.Status);
            else if (SortOrder == Html.GetSortDsc("Status")) query = query.OrderByDescending(q => q.Status);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: RetroParty/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new RetroTreatyViewModel());
        }

        // POST: RetroParty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, RetroTreatyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId);

                var trail = GetNewTrailObject();
                Result = RetroTreatyService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;
                    CreateTrail(
                        bo.Id,
                        "Create Retro Treaty"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: RetroParty/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroTreatyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            LoadPage(bo);
            return View(new RetroTreatyViewModel(bo));
        }

        // POST: RetroParty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, RetroTreatyViewModel model)
        {
            var dbBo = RetroTreatyService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                var trail = GetNewTrailObject();

                Result = RetroTreatyService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Retro Treaty"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(dbBo);
            return View(model);
        }

        // GET: RetroParty/Edit/5
        public ActionResult EditRetroTreatyDetail(int id, int retroTreatyId)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = RetroTreatyDetailService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Edit", new { id = retroTreatyId });
            }

            LoadDetailPage(bo);
            return View(new RetroTreatyDetailViewModel(bo));
        }

        // POST: RetroParty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult EditRetroTreatyDetail(int id, int RetroTreatyId, FormCollection form, RetroTreatyDetailViewModel model)
        {
            var dbBo = RetroTreatyDetailService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index", new { id = RetroTreatyId });
            }

            Dictionary<string, string> formulas = new Dictionary<string, string>()
            {
                { "Gross Retro Premium", model.GrossRetroPremium },
                { "Treaty Discount", model.TreatyDiscount},
                { "Net Retro Premium", model.NetRetroPremium }
            };
            List<string> formulaErrors = EvaluateFormulas(formulas, model.MlreShareStr, model.PremiumSpreadTableId, model.TreatyDiscountTableId);

            foreach (string formulaError in formulaErrors)
            {
                ModelState.AddModelError("", formulaError);
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, dbBo);

                var trail = GetNewTrailObject();

                Result = RetroTreatyDetailService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Retro Treaty Detail"
                    );

                    SetUpdateSuccessMessage("RetroTreatyDetail");
                    return RedirectToAction("EditRetroTreatyDetail", new { id = bo.Id, retroTreatyId = bo.RetroTreatyId });
                }
                AddResult(Result);
            }

            LoadDetailPage(dbBo);
            return View(model);
        }

        // GET: AccountCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var retroTreatyBo = RetroTreatyService.Find(id);
            if (retroTreatyBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new RetroTreatyViewModel(retroTreatyBo));
        }

        // POST: AccountCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var retroTreatyBo = RetroTreatyService.Find(id);
            if (retroTreatyBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = RetroTreatyService.Delete(retroTreatyBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    retroTreatyBo.Id,
                    "Delete Retro Treaty"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = retroTreatyBo.Id });
        }

        // GET: AccountCode/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteRetroTreatyDetail(int id, int retroTreatyId)
        {
            SetViewBagMessage();
            var retroTreatyDetailBo = RetroTreatyDetailService.Find(id);
            if (retroTreatyDetailBo == null)
            {
                return RedirectToAction("Edit", new { id = retroTreatyId });
            }
            return View(new RetroTreatyDetailViewModel(retroTreatyDetailBo));
        }

        // POST: AccountCode/Delete/5
        [HttpPost, ActionName("DeleteRetroTreatyDetail")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteRetroTreatyDetailConfirmed(int id, int RetroTreatyId)
        {
            var retroTreatyDetailBo = RetroTreatyDetailService.Find(id);
            if (retroTreatyDetailBo == null)
            {
                return RedirectToAction("Edit", new { id = RetroTreatyId });
            }

            TrailObject trail = GetNewTrailObject();
            Result = RetroTreatyDetailService.Delete(retroTreatyDetailBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    retroTreatyDetailBo.Id,
                    "Delete Retro Treaty Detail"
                );

                SetDeleteSuccessMessage("Retro Treaty Detail");
                return RedirectToAction("Edit", new { id = RetroTreatyId });
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = retroTreatyDetailBo.Id, retroTreatyId = retroTreatyDetailBo.RetroTreatyId });
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
                    var process = new ProcessRetroTreaty()
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
            int? RetroPartyId,
            string Code,
            int? TreatyTypePickListDetailId,
            string LineOfBusiness,
            string RetroShare,
            int? TreatyDiscountTableId,
            string EffectiveStartDate,
            string EffectiveEndDate,
            int? Status
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.RetroTreaties.LeftOuterJoin(_db.RetroTreatyDetails, t => t.Id, d => d.RetroTreatyId, (t, d) => new RetroTreatyWithDetail { Treaty = t, Detail = d })
                .Select(RetroTreatyService.ExpressionWithDetail());

            if (type == 2) // filtered dowload
            {
                DateTime? effectiveStartDate = Util.GetParseDateTime(EffectiveStartDate);
                DateTime? effectiveEndDate = Util.GetParseDateTime(EffectiveEndDate);

                double? retroShare = Util.StringToDouble(RetroShare);

                if (RetroPartyId.HasValue) query = query.Where(q => q.RetroPartyId == RetroPartyId);
                if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code.Contains(Code));
                if (TreatyTypePickListDetailId.HasValue) query = query.Where(q => q.TreatyTypePickListDetailId == TreatyTypePickListDetailId);
                if (retroShare.HasValue) query = query.Where(q => q.RetroShare == retroShare);
                if (TreatyDiscountTableId.HasValue) query = query.Where(q => q.TreatyDiscountTableId == TreatyDiscountTableId);
                if (effectiveStartDate.HasValue) query = query.Where(q => q.EffectiveStartDate == effectiveStartDate);
                if (effectiveEndDate.HasValue) query = query.Where(q => q.EffectiveEndDate == effectiveEndDate);
                if (Status.HasValue) query = query.Where(q => q.Status == Status);
            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportRetroTreaty();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query);

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownRetroParty();
            DropDownStatus();
            DropDownTreatyDiscountTable(TreatyDiscountTableBo.TypePerLife);
            DropDownTreatyType();
            SetViewBagMessage();
        }

        public void LoadPage(RetroTreatyBo bo = null)
        {
            // Filters
            ViewBag.DropDownTreatyCodeFilter = DropDownTreatyCode(TreatyCodeBo.StatusActive, codeAsValue: true, isUniqueCode: true);
            ViewBag.DropDownTreatyTypeFilter = DropDownTreatyType(true);
            ViewBag.DropDownYesNoFilter = DropDownYesNoWithSelect(true);
            DropDownFundsAccountingTypeCode(true);
            ViewBag.DropDownPremiumSpreadTableFilter = DropDownPremiumSpreadTable(PremiumSpreadTableBo.TypePerLife, true);
            ViewBag.DropDownTreatyDiscountTableFilter = DropDownTreatyDiscountTable(TreatyDiscountTableBo.TypePerLife, true);


            DropDownStatus();
            DropDownRetroParty();
            DropDownTreatyDiscountTable(TreatyDiscountTableBo.TypePerLife);
            DropDownTreatyCode(TreatyCodeBo.StatusActive, isUniqueCode: true);
            DropDownTreatyType();
            DropDownYesNoWithSelect();

            if (bo != null)
            {
                ViewBag.RetroTreatyDetailBos = RetroTreatyDetailService.GetByRetroTreatyId(bo.Id);
            }

            SetViewBagMessage();
        }

        public void LoadDetailPage(RetroTreatyDetailBo bo = null)
        {
            DropDownPremiumSpreadTable(PremiumSpreadTableBo.TypePerLife);
            DropDownTreatyDiscountTable(TreatyDiscountTableBo.TypePerLife);

            ViewBag.StandardRetroOutputs = StandardRetroOutputService.Get();
            ViewBag.OtherOutputs = new List<StandardRetroOutputBo>()
            {
                new StandardRetroOutputBo() { Code = "MLRE_SHARE", DataTypeName = StandardOutputBo.GetDataTypeName(StandardOutputBo.DataTypePercentage) },
                new StandardRetroOutputBo() { Code = "PREMIUM_SPREAD", DataTypeName = StandardOutputBo.GetDataTypeName(StandardOutputBo.DataTypeAmount) },
                new StandardRetroOutputBo() { Code = "DISCOUNT", DataTypeName = StandardOutputBo.GetDataTypeName(StandardOutputBo.DataTypeAmount) },
            };

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RetroTreatyBo.MaxStatus; i++)
            {
                items.Add(new SelectListItem { Text = RetroTreatyBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatuses = items;
            return items;
        }

        [HttpPost]
        public JsonResult CreateDetails(int retroTreatyId, string configIds)
        {
            var bos = new List<RetroTreatyDetailBo>();
            foreach (string configIdStr in configIds.Split(','))
            {
                int? configId = Util.GetParseInt(configIdStr);
                if (!configId.HasValue)
                    continue;

                if (RetroTreatyDetailService.IsExists(retroTreatyId, configId.Value))
                    continue;

                RetroTreatyDetailBo bo = new RetroTreatyDetailBo()
                {
                    RetroTreatyId = retroTreatyId,
                    PerLifeRetroConfigurationTreatyId = configId.Value,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId
                };

                var trail = GetNewTrailObject();
                Result = RetroTreatyDetailService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(bo.Id, "Create Retro Treaty Detail");

                    bo.PerLifeRetroConfigurationTreatyBo = PerLifeRetroConfigurationTreatyService.Find(bo.PerLifeRetroConfigurationTreatyId);
                    bo.MlreShareStr = Util.DoubleToString(bo.MlreShare);
                    bos.Add(bo);
                }
            }

            return Json(new { bos });
        }

        public List<string> EvaluateFormulas(Dictionary<string, string> formulas, string mlreShareStr, int? premiumSpreadTableId, int? treatyDiscountTableId)
        {
            List<string> errors = new List<string>();

            var sroe = new StandardRetroOutputEval
            {
                MlreShare = Util.StringToDouble(mlreShareStr),
                PremiumSpread = premiumSpreadTableId.HasValue ? new Random().Next(100) : (double?)null,
                Discount = treatyDiscountTableId.HasValue ? new Random().Next(100) : (double?)null,
            };

            foreach (var row in formulas)
            {
                sroe.Errors = new List<string>();

                string formula = row.Value;
                if (string.IsNullOrEmpty(formula))
                    continue;

                string formulaName = row.Key;

                sroe.Formula = formula;
                sroe.EvalFormula();

                foreach (string error in sroe.Errors)
                {
                    errors.Add(string.Format("{0} at {1}", error, formulaName));
                }
            }

            return errors;
        }

        [HttpPost]
        public JsonResult EvaluateFormulasJson(Dictionary<string, string> formulas, string mlreShareStr, int? premiumSpreadTableId, int? treatyDiscountTableId)
        {
            bool success = true;
            List<string> errors = new List<string>();

            if (formulas == null)
                return Json(new { success, errors });

            errors = EvaluateFormulas(formulas, mlreShareStr, premiumSpreadTableId, treatyDiscountTableId);
            success = errors.IsNullOrEmpty();

            return Json(new { success, errors });
        }

        [HttpPost]
        public JsonResult GetEvaluateVariables(string script, string mlreShareStr)
        {
            bool success = true;
            List<string> errors = new List<string>();
            List<StandardRetroOutputBo> standardRetroOutputBos = new List<StandardRetroOutputBo>();

            try
            {
                var sroe = new StandardRetroOutputEval
                {
                    MlreShare = Util.StringToDouble(mlreShareStr)
                };
                standardRetroOutputBos = sroe.GetVariables(script);
            }
            catch (Exception e)
            {
                success = false;
                errors.Add(e.Message);
            }

            return Json(new { success, standardRetroOutputBos, errors });
        }

        [HttpPost]
        public JsonResult GetEvaluateResult(string formula, List<StandardRetroOutputBo> standardRetroOutputBos)
        {
            //string condition, string formula, Dictionary< string,string> variables
            bool success = true;
            List<string> errors = new List<string>();
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (standardRetroOutputBos == null)
                    standardRetroOutputBos = new List<StandardRetroOutputBo>();

                PerLifeAggregationMonthlyDataBo bo = new PerLifeAggregationMonthlyDataBo();
                List<int> numberDataTypes = new List<int>
                {
                    StandardOutputBo.DataTypeAmount,
                    StandardOutputBo.DataTypePercentage,
                };

                var sroe = new StandardRetroOutputEval()
                {
                    Formula = formula
                };

                foreach (StandardRetroOutputBo standardRetroOutputBo in standardRetroOutputBos)
                {
                    dynamic dummyValue = standardRetroOutputBo.DummyValue;
                    switch (standardRetroOutputBo.Code)
                    {
                        case "MLRE_SHARE":
                            sroe.MlreShare = double.Parse(dummyValue);
                            break;
                        case "PREMIUM_SPREAD":
                            sroe.PremiumSpread = double.Parse(dummyValue);
                            break;
                        case "DISCOUNT":
                            sroe.Discount = double.Parse(dummyValue);
                            break;
                        case "RETRO_GROSS_PREMIUM":
                            sroe.RetroGrossPremium = double.Parse(dummyValue);
                            break;
                        case "RETRO_DISCOUNT":
                            sroe.RetroDiscount = double.Parse(dummyValue);
                            break;
                        case "RETRO_NET_PREMIUM":
                            sroe.RetroNetPremium = double.Parse(dummyValue);
                            break;
                        default:
                            var property = StandardRetroOutputBo.GetPropertyNameByType(standardRetroOutputBo.Type);
                            bo.SetData(standardRetroOutputBo.DataType, property, standardRetroOutputBo.DummyValue);
                            break;
                    }
                }

                sroe.PerLifeAggregationMonthlyDataBo = bo;
                var finalResult = sroe.EvalFormula();

                if (finalResult == null)
                {
                    success = false;
                    errors.AddRange(sroe.Errors);
                }
                else
                {
                    result["Formula"] = sroe.Formula;
                    result["Formatted Formula"] = sroe.FormattedFormula;
                    result["Result"] = finalResult != null ? finalResult.ToString() : "";
                }
            }
            catch (Exception e)
            {
                success = false;
                errors.Add(e.Message);
            }

            return Json(new { success, result, errors });
        }
    }

}