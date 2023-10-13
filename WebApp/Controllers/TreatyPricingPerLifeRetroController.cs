using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using PagedList;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingPerLifeRetroController : BaseController
    {
        public const string Controller = "TreatyPricingPerLifeRetro";

        // GET: TreatyPricingPerLifeRetro
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Code,
            int? RetroPartyId,
            int? Type,
            double? RetrocessionaireShare,
            string SortOrder,
            int? Page
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Code"] = Code,
                ["RetroPartyId"] = RetroPartyId,
                ["Type"] = Type,
                ["RetrocessionaireShare"] = RetrocessionaireShare,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCode = GetSortParam("Code");
            ViewBag.SortRetroParty = GetSortParam("RetroPartyId");
            ViewBag.SortType = GetSortParam("Type");
            ViewBag.SortRetrocessionaireShare = GetSortParam("RetrocessionaireShare");

            var query = _db.TreatyPricingPerLifeRetro.Select(TreatyPricingPerLifeRetroViewModel.Expression());
            if (!string.IsNullOrEmpty(Code)) query = query.Where(q => q.Code == Code);
            if (RetroPartyId.HasValue) query = query.Where(q => q.RetroPartyId == RetroPartyId);
            if (Type.HasValue) query = query.Where(q => q.Type == Type);
            if (RetrocessionaireShare.HasValue) query = query.Where(q => q.RetrocessionaireShare == RetrocessionaireShare);

            if (SortOrder == Html.GetSortAsc("Code")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("Code")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("RetroPartyId")) query = query.OrderBy(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortDsc("RetroPartyId")) query = query.OrderByDescending(q => q.RetroParty.Code);
            else if (SortOrder == Html.GetSortAsc("Type")) query = query.OrderBy(q => q.Type);
            else if (SortOrder == Html.GetSortDsc("Type")) query = query.OrderByDescending(q => q.Type);
            else if (SortOrder == Html.GetSortAsc("RetrocessionaireShare")) query = query.OrderBy(q => q.RetrocessionaireShare);
            else if (SortOrder == Html.GetSortDsc("RetrocessionaireShare")) query = query.OrderByDescending(q => q.RetrocessionaireShare);
            else query = query.OrderBy(q => q.Code);

            IndexPage();

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // POST: TreatyPricingPerLifeRetro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(
            string PerLifeRetroCode,
            bool IsDuplicateExisting,
            int? DuplicatePerLifeRetroId
        )
        {
            Result = TreatyPricingPerLifeRetroService.Result();

            TreatyPricingPerLifeRetroBo existingPerLifeRetroBo = null;
            TreatyPricingPerLifeRetroVersionBo existingPerLifeRetroVersionBo = null;
            string existingProfitCommissionDetail = null;
            string existingTierProfitCommission = null;

            try
            {
                if (IsDuplicateExisting == true && !DuplicatePerLifeRetroId.HasValue)
                {
                    Result.AddError("Per Life Treaty ID / Code is Required.");
                }
                else if (IsDuplicateExisting == true && DuplicatePerLifeRetroId.HasValue)
                {
                    existingPerLifeRetroBo = TreatyPricingPerLifeRetroService.Find(DuplicatePerLifeRetroId);
                    if (existingPerLifeRetroBo == null)
                    {
                        Result.AddError("Per Life Retro not found");
                    }
                    else
                    {
                        existingPerLifeRetroVersionBo = TreatyPricingPerLifeRetroVersionService.FindLatestByTreatyPricingPerLifeRetroId(existingPerLifeRetroBo.Id);
                        if (existingPerLifeRetroVersionBo == null)
                        {
                            Result.AddError("Selected Per Life Retro's version not found");
                        }
                        else
                        {
                            existingProfitCommissionDetail = TreatyPricingProfitCommissionDetailService.GetJsonByParent(existingPerLifeRetroVersionBo.Id);
                            existingTierProfitCommission = TreatyPricingTierProfitCommissionService.GetJsonByParent(existingPerLifeRetroVersionBo.Id);
                        }
                    }
                }

                if (Result.Valid)
                {
                    TreatyPricingPerLifeRetroBo perLifeRetroBo = new TreatyPricingPerLifeRetroBo();
                    if (existingPerLifeRetroBo != null)
                        perLifeRetroBo = existingPerLifeRetroBo;

                    perLifeRetroBo.Code = PerLifeRetroCode;
                    perLifeRetroBo.CreatedById = AuthUserId;
                    perLifeRetroBo.UpdatedById = AuthUserId;

                    TrailObject trail = GetNewTrailObject();

                    Result = TreatyPricingPerLifeRetroService.Create(ref perLifeRetroBo, ref trail);
                    if (Result.Valid)
                    {
                        if (IsDuplicateExisting)
                        {
                            existingPerLifeRetroVersionBo.Id = 0;
                            existingPerLifeRetroVersionBo.TreatyPricingPerLifeRetroId = perLifeRetroBo.Id;
                            existingPerLifeRetroVersionBo.Version = 1;
                            existingPerLifeRetroVersionBo.PersonInChargeId = AuthUserId;
                            existingPerLifeRetroVersionBo.CreatedById = AuthUserId;
                            existingPerLifeRetroVersionBo.UpdatedById = AuthUserId;
                            TreatyPricingPerLifeRetroVersionService.Create(ref existingPerLifeRetroVersionBo, ref trail);
                            TreatyPricingPerLifeRetroVersionDetailService.Save(existingProfitCommissionDetail, existingPerLifeRetroVersionBo.Id, AuthUserId, ref trail, true);
                            TreatyPricingPerLifeRetroVersionTierService.Save(existingTierProfitCommission, existingPerLifeRetroVersionBo.Id, existingPerLifeRetroVersionBo.ProfitSharing, AuthUserId, ref trail, true);
                        }
                        else
                        {
                            var perLifeRetroVersionBo = new TreatyPricingPerLifeRetroVersionBo()
                            {
                                TreatyPricingPerLifeRetroId = perLifeRetroBo.Id,
                                Version = 1,
                                PersonInChargeId = AuthUserId,
                                CreatedById = AuthUserId,
                                UpdatedById = AuthUserId,
                            };
                            TreatyPricingPerLifeRetroVersionService.Create(ref perLifeRetroVersionBo, ref trail);

                            var profitCommissionDetail = TreatyPricingPerLifeRetroVersionDetailBo.GetJsonDefaultRow(perLifeRetroVersionBo.Id);
                            TreatyPricingPerLifeRetroVersionDetailService.Save(profitCommissionDetail, perLifeRetroVersionBo.Id, AuthUserId, ref trail);
                        }

                        CreateTrail(
                            perLifeRetroBo.Id,
                            "Create Treaty Pricing Per Life Retro"
                        );

                        SetCreateSuccessMessage(Controller);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Result.AddError(string.Format("Error occurred. Error details: {0}", ex.Message));
            }

            SetErrorSessionMsgArr(Result.ToErrorArray().OfType<string>().ToList());
            return RedirectToAction("Index");
        }

        // GET: TreatyPricingPerLifeRetro/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode))
                return RedirectDashboard();

            TreatyPricingPerLifeRetroBo bo = TreatyPricingPerLifeRetroService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingPerLifeRetroViewModel(bo);
            LoadPage(model, versionId);
            return View(model);
        }

        // POST: TreatyPricingPerLifeRetro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingPerLifeRetroViewModel model, int versionId = 0)
        {
            var dbBo = TreatyPricingPerLifeRetroService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", new { id, versionId });

            TreatyPricingPerLifeRetroVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingPerLifeRetroVersionBos);
            if (dbBo.EditableVersion != model.CurrentVersion)
            {
                ModelState.AddModelError("", "You can only update details for the latest version");
            }
            else
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingPerLifeRetroVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);

                //foreach (string error in TreatyPricingPerLifeRetroVersionBenefitBo.Validate(updatedVersionBo.Benefits))
                //{
                //    ModelState.AddModelError("", error);
                //}
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();
                Result = TreatyPricingPerLifeRetroService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        TreatyPricingPerLifeRetroVersionService.Update(ref updatedVersionBo, ref trail);
                        TreatyPricingPerLifeRetroVersionDetailService.Save(updatedVersionBo.ProfitCommissionDetail, updatedVersionBo.Id, AuthUserId, ref trail);
                        TreatyPricingPerLifeRetroVersionTierService.Save(updatedVersionBo.TierProfitCommission, updatedVersionBo.Id, updatedVersionBo.ProfitSharing, AuthUserId, ref trail);
                        TreatyPricingPerLifeRetroVersionBenefitService.Save(updatedVersionBo.Benefits, updatedVersionBo.Id, AuthUserId, ref trail);
                    }

                    CreateTrail(
                        bo.Id,
                        "Update Treaty Pricing Per Life Retro"
                    );

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id, versionId });
                }
                AddResult(Result);
            }

            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            LoadPage(model, versionId);
            return View(model);
        }

        public void IndexPage()
        {
            DropDownPerLifeRetroCode();
            DropDownRetroParty();
            DropDownType();

            SetViewBagMessage();
        }

        public void LoadPage(TreatyPricingPerLifeRetroViewModel model, int versionId = 0)
        {
            AuthUserName();
            DropDownVersion(model);
            DropDownUser(UserBo.StatusActive, model.PersonInChargeId);
            DropDownRetroParty(RetroPartyBo.StatusActive, model.RetroPartyId);
            DropDownRetrocessionaire(RetroPartyBo.StatusActive, model.RetrocessionaireRetroPartyId);
            DropDownType();
            DropDownAgeBasis();
            DropDownProfitSharing();
            DropDownDropDown();
            GetItemList();
            DropDownTreatyPricingCedant();
            DropDownProductType();
            DropDownProductQuotation();
            DropDownCurrencyCode();

            GetBenefits();

            ViewBag.DropDownPaymentRetrocessionairePremium = GetPickListDetailIdDropDownByPickListId(PickListBo.PaymentRetrocessionairePremium);
            ViewBag.DropDownArrangementRetrocessionnaireType = GetPickListDetailIdDropDownByPickListId(PickListBo.ArrangementRetrocessionnaireType);
            ViewBag.TargetSegmentCodes = GetPickListDetailCodeDescription(PickListBo.TargetSegment);
            ViewBag.DistributionChannelCodes = GetPickListDetailCodeDescription(PickListBo.DistributionChannel);
            ViewBag.UnderwritingMethodCodes = GetPickListDetailCodeDescription(PickListBo.UnderwritingMethod);
            ViewBag.PerLifeRetroProducts = TreatyPricingProductPerLifeRetroService.GetByPerLifeRetroId(model.Id);

            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl);
            GetRemarkSubjects();

            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingPerLifeRetro.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();

            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownPerLifeRetroCode()
        {
            var list = new List<string>();
            var items = GetEmptyDropDownList();
            foreach (string code in TreatyPricingPerLifeRetroService.GetCodes())
            {
                items.Add(new SelectListItem { Text = code, Value = code });
                list.Add(code);
            }
            ViewBag.DropDownPerLifeRetroCodes = items;
            ViewBag.CodeList = list;
            return items;
        }

        public List<SelectListItem> DropDownType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= TreatyPricingPerLifeRetroBo.TypeMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingPerLifeRetroBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownRetrocessionaire(int? status = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var retroParty in RetroPartyService.GetByStatus(status, selectedId))
            {
                var selected = retroParty.Id == selectedId;
                items.Add(new SelectListItem { Text = retroParty.ToString(), Value = retroParty.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownRetrocessionaires = items;
            return items;
        }

        public List<SelectListItem> DropDownProfitSharing()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= TreatyPricingPerLifeRetroVersionBo.ProfitSharingMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingPerLifeRetroVersionBo.GetProfitSharingName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProfitSharings = items;
            return items;
        }

        public void GetItemList()
        {
            var list = new List<string>();
            for (int i = 0; i <= TreatyPricingPerLifeRetroVersionDetailBo.ItemMax; i++)
            {
                list.Add(TreatyPricingPerLifeRetroVersionDetailBo.GetItemName(i));
            }
            ViewBag.ItemList = list;
        }

        public List<SelectListItem> DropDownDropDown()
        {
            var items = GetEmptyDropDownList(false);
            for (int i = 1; i <= TreatyPricingPerLifeRetroVersionDetailBo.DropDownMax; i++)
            {
                items.Add(new SelectListItem { Text = TreatyPricingPerLifeRetroVersionDetailBo.GetDropDownName(i), Value = i.ToString() });
            }
            ViewBag.DropDownDropDowns = items;
            return items;
        }

        public List<SelectListItem> DropDownProduct()
        {
            var items = GetEmptyDropDownList();
            foreach (var productBo in TreatyPricingProductService.Get())
            {
                items.Add(new SelectListItem { Text = string.Format("{0} - {1}", productBo.Code, productBo.Name), Value = productBo.Id.ToString() });
            }
            ViewBag.DropDownProducts = items;
            return items;
        }

        public List<SelectListItem> DropDownProductQuotation()
        {
            var items = GetEmptyDropDownList();
            foreach (var productBo in TreatyPricingProductService.Get())
            {
                items.Add(new SelectListItem { Text = productBo.QuotationName, Value = productBo.QuotationName });
            }
            ViewBag.DropDownProductQuotations = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingPerLifeRetroBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingPerLifeRetroVersionService.GetByTreatyPricingPerLifeRetroId(bo.Id));
            TreatyPricingPerLifeRetroVersionBo nextVersionBo;
            TreatyPricingPerLifeRetroVersionBo previousVersionBo = (TreatyPricingPerLifeRetroVersionBo)bo.CurrentVersionObject;

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingPerLifeRetroVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingPerLifeRetroVersionBo()
                {
                    TreatyPricingPerLifeRetroId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.PersonInChargeId = AuthUserId;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingPerLifeRetroVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                if (!duplicatePreviousVersion)
                {
                    nextVersionBo.ProfitCommissionDetail = TreatyPricingPerLifeRetroVersionDetailBo.GetJsonDefaultRow(nextVersionBo.Id);
                }
                TreatyPricingPerLifeRetroVersionDetailService.Save(nextVersionBo.ProfitCommissionDetail, nextVersionBo.Id, AuthUserId, ref trail, true);
                TreatyPricingPerLifeRetroVersionTierService.Save(nextVersionBo.TierProfitCommission, nextVersionBo.Id, nextVersionBo.ProfitSharing, AuthUserId, ref trail, true);
                TreatyPricingPerLifeRetroVersionBenefitService.Save(nextVersionBo.Benefits, nextVersionBo.Id, AuthUserId, ref trail, true);
                nextVersionBo.ProfitCommissionDetail = TreatyPricingPerLifeRetroVersionDetailService.GetJsonByParent(nextVersionBo.Id);
                nextVersionBo.TierProfitCommission = TreatyPricingPerLifeRetroVersionTierService.GetJsonByParent(nextVersionBo.Id);                
                nextVersionBo.Benefits = TreatyPricingPerLifeRetroVersionBenefitService.GetJsonByVersionId(nextVersionBo.Id);

                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Per Life Retro Version"
                );

                userTrail.CreatedByBo = UserService.Find(userTrail.CreatedById);
                userTrail.CreatedAtStr = userTrail.CreatedAt.ToString(Util.GetDateTimeFormat());

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions });
        }

        [HttpPost]
        public JsonResult GetProductData(int? cedantId, int? treatyPricingCedantId, string quotationName, string underwritingMethods, string distributionChannels, string targetSegments, int? productType)
        {
            var queryProductBos = TreatyPricingProductVersionService.GetBySearchParams(cedantId, treatyPricingCedantId, quotationName, underwritingMethods, distributionChannels, targetSegments, productType);
            var productBos = new List<TreatyPricingProductBo>();

            foreach (var productVer in queryProductBos)
            {
                var productBo = TreatyPricingProductService.Find(productVer.TreatyPricingProductId);
                productBo.TreatyPricingProductVersionBos = null;
                productBo.LatestTreatyPricingProductVersionBo = productVer;
                productBos.Add(productBo);
            }

            return Json(new { productBos });
        }
    }
}