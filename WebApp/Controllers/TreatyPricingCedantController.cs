using PagedList;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;
using Services.TreatyPricing;
using BusinessObject.TreatyPricing;
using BusinessObject;
using Services;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingCedantController : BaseController
    {
        public const string Controller = "TreatyPricingCedant";

        // GET: TreatyPricingCedant
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            int? CedantId,
            int? ReinsuranceTypePickListDetailId,
            int? NoOfProduct,
            int? NoOfDocument,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["CedantId"] = CedantId,
                ["ReinsuranceTypePickListDetailId"] = ReinsuranceTypePickListDetailId,
                ["NoOfProduct"] = NoOfProduct,
                ["NoOfDocument"] = NoOfDocument,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortCedantId = GetSortParam("CedantId");
            ViewBag.SortReinsuranceTypePickListDetailId = GetSortParam("ReinsuranceTypePickListDetailId");
            ViewBag.SortNoOfProduct = GetSortParam("NoOfProduct");
            ViewBag.SortNoOfDocument = GetSortParam("NoOfDocument");

            var query = _db.TreatyPricingCedants.Select(TreatyPricingCedantViewModel.Expression());

            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code == Code);
            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
            if (ReinsuranceTypePickListDetailId.HasValue) query = query.Where(q => q.ReinsuranceTypePickListDetailId == ReinsuranceTypePickListDetailId);
            if (NoOfProduct.HasValue) query = query.Where(q => q.NoOfProduct == NoOfProduct);
            if (NoOfDocument.HasValue) query = query.Where(q => q.NoOfDocument == NoOfDocument);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceTypePickListDetailId")) query = query.OrderBy(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceTypePickListDetailId")) query = query.OrderByDescending(q => q.ReinsuranceTypePickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("NoOfProduct")) query = query.OrderBy(q => q.NoOfProduct);
            else if (SortOrder == Html.GetSortDsc("NoOfProduct")) query = query.OrderByDescending(q => q.NoOfProduct);
            else if (SortOrder == Html.GetSortAsc("NoOfDocument")) query = query.OrderBy(q => q.NoOfDocument);
            else if (SortOrder == Html.GetSortDsc("NoOfDocument")) query = query.OrderByDescending(q => q.NoOfDocument);
            else query = query.OrderBy(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // Post: TreatyPricingCedant
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult AddCedant(
            int? NewCedantId,
            int? NewReinsuranceTypePickListDetailId
        )
        {
            Result = TreatyPricingCedantService.Result();
            if (!NewCedantId.HasValue)
                Result.AddError("Ceding Company is Required.");
            if (!NewReinsuranceTypePickListDetailId.HasValue)
                Result.AddError("Reinsurance Type is Required.");

            var cedantBo = CedantService.Find(NewCedantId);
            var reinsuranceTypePickListDetailBo = PickListDetailService.Find(NewReinsuranceTypePickListDetailId);

            if (Result.Valid)
            {
                if (cedantBo == null)
                    Result.AddError("Ceding Company not found.");
                if (reinsuranceTypePickListDetailBo == null)
                    Result.AddError("Reinsurance Type not found.");
            }

            if (Result.Valid)
            {
                TreatyPricingCedantBo bo = new TreatyPricingCedantBo
                {
                    CedantId = NewCedantId.Value,
                    CedantBo = cedantBo,
                    ReinsuranceTypePickListDetailId = NewReinsuranceTypePickListDetailId.Value,
                    ReinsuranceTypePickListDetailBo = reinsuranceTypePickListDetailBo,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                bo.Code = string.Format("{0}-{1}", cedantBo.Code, reinsuranceTypePickListDetailBo.Code);

                var trail = GetNewTrailObject();
                Result = TreatyPricingCedantService.Create(ref bo, ref trail);

                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Create Treaty Pricing Cedant"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Index");
                }
            }

            SetErrorSessionMsgArr(Result.ToErrorArray().OfType<string>().ToList());
            return RedirectToAction("Index");
        }

        // GET: TreatyPricingCedant/Edit/5
        public ActionResult Edit(int id, int? TabIndex, int? Page)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = TreatyPricingCedantService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingCedantViewModel(bo);
            LoadPage(model, TabIndex, Page);
            ViewBag.ActiveTab = TabIndex;
            return View(model);
        }

        // POST: TreatyPricingCedant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, TreatyPricingCedantViewModel model)
        {
            var dbBo = TreatyPricingCedantService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = TreatyPricingCedantService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Cedant"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model);
            return View(model);
        }

        //// GET: TreatyPricingCedant/Delete/5
        //[Auth(Controller = Controller, Power = "D")]
        //public ActionResult Delete(int id)
        //{
        //    SetViewBagMessage();
        //    var bo = TreatyPricingCedantService.Find(id);
        //    if (bo == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View(new TreatyPricingCedantViewModel(bo));
        //}

        //// POST: TreatyPricingCedant/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Auth(Controller = Controller, Power = "D")]
        //public ActionResult DeleteConfirmed(int id, TreatyPricingCedantViewModel model)
        //{
        //    var bo = TreatyPricingCedantService.Find(id);
        //    if (bo == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    var trail = GetNewTrailObject();
        //    Result = TreatyPricingCedantService.Delete(bo, ref trail);
        //    if (Result.Valid)
        //    {
        //        CreateTrail(
        //            bo.Id,
        //            "Delete Treaty Pricing Cedant"
        //        );

        //        SetDeleteSuccessMessage(Controller);
        //        return RedirectToAction("Index");
        //    }

        //    if (Result.MessageBag.Errors.Count > 1)
        //        SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
        //    else
        //        SetErrorSessionMsg(Result.MessageBag.Errors[0]);

        //    return RedirectToAction("Delete", new { id = bo.Id });
        //}

        public void IndexPage()
        {
            DropDownCedant();
            DropDownNewCedant();
            DropDownReinsuranceType();
            SetViewBagMessage();
        }

        public void LoadPage(TreatyPricingCedantViewModel model, int? TabIndex = null, int? Page = null)
        {
            AuthUserName();
            DropDownReinsuranceType();

            if (model.Id == 0)
            {
                // Create
            }
            else
            {
                //Edit
                DropDownCedant(CedantBo.StatusActive, model.CedantId);
                DropDownReinsuranceType();
                DropDownTreatyPricingCedant(); //claim approval punye
                DropDownTreatyPricingObjectModules(); // quotation workflow

                ViewBag.TreatyPricingCedantId = model.Id;

                if (model.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }

                switch (TabIndex)
                {
                    case TreatyPricingCedantBo.IndexRateTableGroup:
                        GetRateTableGroup(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexUwLimit:
                        GetUWLimit(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexMedicalTable:
                        GetMedicalTable(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexFinancialTable:
                        GetFinancialTable(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexUwQuestionnaire:
                        GetUWQuestionnaire(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexAdvantageProgram:
                        GetAdvantageProgram(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexClaimApprovalLimit:
                        GetClaimApprovalLimit(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexDefinitionAndExclusion:
                        GetDefinitionAndExclusion(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexCampaign:
                        GetCampaign(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexProfitCommission:
                        GetProfitCommission(model.Id, Page);
                        break;
                    case TreatyPricingCedantBo.IndexCustomOther:
                        GetCustomOther(model.Id, Page);
                        break;
                    default:
                        GetProducts(model.Id, Page);
                        break;
                }

                DropDownTreatyWorkflowDocumentType(); // treatyworkflow
                DropDownRetroParty(); // treatyperlife & treatyworkflow

                // Check User 'C' Power By Sub Module
                IsEnabledAddNewBySubModule();
            }

            GetFullBaseUrl();

            SetViewBagMessage();
        }

        public void GetProducts(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingProducts = TreatyPricingProductService.GetByTreatyPricingCedantId(treatyPricingCedantId, true);
            ViewBag.TreatyPricingProducts = treatyPricingProducts.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingProductTotal = treatyPricingProducts.Count();

            DropDownWorkflowStatus();
        }

        public void GetRateTableGroup(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingRateTableGroups = TreatyPricingRateTableGroupService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingRateTableGroups = treatyPricingRateTableGroups.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingRateTableGroupTotal = treatyPricingRateTableGroups.Count();

            DropDownRateTableGroupStatus();
        }

        public void GetClaimApprovalLimit(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingClaimApprovalLimits = TreatyPricingClaimApprovalLimitService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingClaimApprovalLimits = treatyPricingClaimApprovalLimits.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingClaimApprovalLimitTotal = treatyPricingClaimApprovalLimits.Count();

            DropDownClaimApprovalLimitStatus();
        }
        public void GetUWQuestionnaire(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingUwQuestionnaires = TreatyPricingUwQuestionnaireService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingUwQuestionnaires = treatyPricingUwQuestionnaires.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingUwQuestionnaireTotal = treatyPricingUwQuestionnaires.Count();

            DropDownQuestionnaireType();
            DropDownUwQuestionnaireStatus();
        }

        public void GetUWLimit(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingUwLimits = TreatyPricingUwLimitService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingUwLimits = treatyPricingUwLimits.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingUwLimitTotal = treatyPricingUwLimits.Count();

            DropDownUwLimitBenefitCode(treatyPricingUwLimits);
            DropDownUwLimitStatus();
        }

        public void GetMedicalTable(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingMedicalTables = TreatyPricingMedicalTableService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingMedicalTables = treatyPricingMedicalTables.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingMedicalTableTotal = treatyPricingMedicalTables.Count();

            DropDownMedicalTableStatus();
        }

        public void GetFinancialTable(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingFinancialTables = TreatyPricingFinancialTableService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingFinancialTables = treatyPricingFinancialTables.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingFinancialTableTotal = treatyPricingFinancialTables.Count();

            DropDownFinancialTableStatus();
        }

        public void GetAdvantageProgram(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingAdvantagePrograms = TreatyPricingAdvantageProgramService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingAdvantagePrograms = treatyPricingAdvantagePrograms.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingAdvantageProgramTotal = treatyPricingAdvantagePrograms.Count();

            DropDownAdvantageProgramBenefitCode(treatyPricingAdvantagePrograms);
            DropDownAdvantageProgramStatus();
        }

        public void GetDefinitionAndExclusion(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingDefinitionAndExclusions = TreatyPricingDefinitionAndExclusionService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingDefinitionAndExclusions = treatyPricingDefinitionAndExclusions.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingDefinitionAndExclusionTotal = treatyPricingDefinitionAndExclusions.Count();

            DropDownDefinitionAndExclusionStatus();
        }

        public void GetProfitCommission(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingProfitCommissions = TreatyPricingProfitCommissionService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingProfitCommissions = treatyPricingProfitCommissions.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingProfitCommissionTotal = treatyPricingProfitCommissions.Count();

            DropDownProfitCommissionStatus();
        }

        public void GetCustomOther(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingCustomOthers = TreatyPricingCustomOtherService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingCustomOthers = treatyPricingCustomOthers.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingCustomOtherTotal = treatyPricingCustomOthers.Count();

            DropDownCustomOtherStatus();
            DropDownCampaignType();
        }

        public void GetCampaign(int treatyPricingCedantId, int? Page)
        {
            var treatyPricingCampaigns = TreatyPricingCampaignService.GetByTreatyPricingCedantId(treatyPricingCedantId);
            ViewBag.TreatyPricingCampaigns = treatyPricingCampaigns.ToPagedList(Page ?? 1, PageSize);
            ViewBag.TreatyPricingCampaignTotal = treatyPricingCampaigns.Count();

            DropDownCampaignType();
        }

        public void IsEnabledAddNewBySubModule()
        {
            ViewBag.IsEnabledAddProduct = CheckPower(ModuleBo.ModuleController.TreatyPricingProduct.ToString(), "C");
            ViewBag.IsEnabledAddRateTableGroup = CheckPower(ModuleBo.ModuleController.TreatyPricingRateTableGroup.ToString(), "C");
            ViewBag.IsEnabledAddUwLimit = CheckPower(ModuleBo.ModuleController.TreatyPricingUwLimit.ToString(), "C");
            ViewBag.IsEnabledAddMedicalTable = CheckPower(ModuleBo.ModuleController.TreatyPricingMedicalTable.ToString(), "C");
            ViewBag.IsEnabledAddFinancialTable = CheckPower(ModuleBo.ModuleController.TreatyPricingFinancialTable.ToString(), "C");
            ViewBag.IsEnabledAddUwQuestionnaire = CheckPower(ModuleBo.ModuleController.TreatyPricingUwQuestionnaire.ToString(), "C");
            ViewBag.IsEnabledAddAdvantageProgram = CheckPower(ModuleBo.ModuleController.TreatyPricingAdvantageProgram.ToString(), "C");
            ViewBag.IsEnabledAddClaimApprovalLimit = CheckPower(ModuleBo.ModuleController.TreatyPricingClaimApprovalLimit.ToString(), "C");
            ViewBag.IsEnabledAddDefinitionAndExclusion = CheckPower(ModuleBo.ModuleController.TreatyPricingDefinitionAndExclusion.ToString(), "C");
            ViewBag.IsEnabledAddCampaign = CheckPower(ModuleBo.ModuleController.TreatyPricingCampaign.ToString(), "C");
            ViewBag.IsEnabledAddProfitCommission = CheckPower(ModuleBo.ModuleController.TreatyPricingProfitCommission.ToString(), "C");
            ViewBag.IsEnabledAddCustomOther = CheckPower(ModuleBo.ModuleController.TreatyPricingCustomOther.ToString(), "C");
        }

        public List<SelectListItem> DropDownAdvantageProgramBenefitCode(IList<TreatyPricingAdvantageProgramBo> bos)
        {
            var items = GetEmptyDropDownList();

            var benefitCodes = new List<string>();
            foreach (var bo in bos)
            {
                if (!string.IsNullOrEmpty(bo.BenefitCodeNames))
                {
                    if (bo.BenefitCodeNames.Contains(','))
                        benefitCodes.AddRange(bo.BenefitCodeNames.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        benefitCodes.Add(bo.BenefitCodeNames);
                }
            }

            foreach (var benefitCode in benefitCodes.Distinct())
            {
                items.Add(new SelectListItem { Text = benefitCode, Value = benefitCode });
            }
            ViewBag.DropDownAdvantageProgramBenefitCodes = items;
            return items;
        }

        public List<SelectListItem> DropDownAdvantageProgramStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingAdvantageProgramBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingAdvantageProgramBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownAdvantageProgramStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownRateTableGroupStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingRateTableGroupBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingRateTableGroupBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRateTableGroupStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownProfitCommissionStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingProfitCommissionBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingProfitCommissionBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProfitCommissionStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownUwLimitStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwLimitBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwLimitBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownUwLimitStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownUwLimitBenefitCode(IList<TreatyPricingUwLimitBo> bos)
        {
            var items = GetEmptyDropDownList();

            var benefitCodes = new List<string>();
            foreach (var bo in bos)
            {
                if (!string.IsNullOrEmpty(bo.BenefitCode))
                {
                    if (bo.BenefitCode.Contains(','))
                        benefitCodes.AddRange(bo.BenefitCode.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList());
                    else
                        benefitCodes.Add(bo.BenefitCode);
                }
            }

            foreach (var benefitCode in benefitCodes.Distinct())
            {
                items.Add(new SelectListItem { Text = benefitCode, Value = benefitCode });
            }
            ViewBag.DropDownUwLimitBenefitCodes = items;
            return items;
        }

        public List<SelectListItem> DropDownMedicalTableStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingMedicalTableBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingMedicalTableBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownMedicalTableStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownFinancialTableStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingFinancialTableBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingFinancialTableBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownFinancialTableStatuses = items;
            return items;
        }


        public List<SelectListItem> DropDownClaimApprovalLimitStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingClaimApprovalLimitBo.StatusInactive))
            {
                items.Add(new SelectListItem { Text = TreatyPricingClaimApprovalLimitBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownClaimApprovalLimitStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownCustomOtherStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingCustomOtherBo.StatusInactive))
            {
                items.Add(new SelectListItem { Text = TreatyPricingCustomOtherBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownCustomOtherStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownDefinitionAndExclusionStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingDefinitionAndExclusionBo.StatusInactive))
            {
                items.Add(new SelectListItem { Text = TreatyPricingDefinitionAndExclusionBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDefinitionAndExclusionStatuses = items;
            return items;
        }
        public List<SelectListItem> DropDownQuestionnaireType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwQuestionnaireVersionBo.TypeMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireVersionBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownQuestionnaireTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownUwQuestionnaireStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingUwQuestionnaireBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingUwQuestionnaireBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownUwQuestionnaireStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownCampaignType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingCampaignBo.TypeUnderwritingProgram))
            {
                items.Add(new SelectListItem { Text = TreatyPricingCampaignBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownCampaignTypes = items;
            return items;
        }

    }
}