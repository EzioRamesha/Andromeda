using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Services.Identity;
using Services.RiDatas;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class PickListController : BaseController
    {
        public const string Controller = "PickList";

        // GET: PickList
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(int? DepartmentId, string FieldName, int? UsedBy, string SortOrder, int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["DepartmentId"] = DepartmentId,
                ["FieldName"] = FieldName,
                ["UsedBy"] = UsedBy,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortDepartmentId = GetSortParam("DepartmentId");
            ViewBag.SortFieldName = GetSortParam("FieldName");
            ViewBag.SortUsedBy = GetSortParam("UsedBy");

            var query = _db.PickLists.Select(PickListViewModel.Expression());
            if (DepartmentId != null) query = query.Where(q => q.DepartmentId == DepartmentId);
            if (!string.IsNullOrEmpty(FieldName)) query = query.Where(q => q.FieldName.Contains(FieldName));
            if (UsedBy.HasValue) query = query.Where(q => q.PickListDepartments.Any(a => a.DepartmentId == UsedBy));

            if (SortOrder == Html.GetSortAsc("DepartmentId")) query = query.OrderBy(q => q.Department.Name);
            else if (SortOrder == Html.GetSortDsc("DepartmentId")) query = query.OrderByDescending(q => q.Department.Name);
            else if (SortOrder == Html.GetSortAsc("FieldName")) query = query.OrderBy(q => q.FieldName);
            else if (SortOrder == Html.GetSortDsc("FieldName")) query = query.OrderByDescending(q => q.FieldName);
            else if (SortOrder == Html.GetSortAsc("UsedBy")) query = query.OrderBy(q => q.PickListDepartments);
            else if (SortOrder == Html.GetSortDsc("UsedBy")) query = query.OrderByDescending(q => q.PickListDepartments);
            else query = query.OrderBy(q => q.FieldName);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: PickList/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            return RedirectToAction("Index");
            //LoadPage();
            //return View(new PickListViewModel());
        }

        // POST: PickList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PickListViewModel model)
        {
            /*
            if (ModelState.IsValid)
            {
                var pickListBo = new PickListBo
                {
                    DepartmentId = model.DepartmentId,
                    StandardOutputId = model.StandardOutputId,
                    FieldName = model.FieldName,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };
            
                TrailObject trail = GetNewTrailObject();
                Result = PickListService.Create(ref pickListBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = pickListBo.Id;
                    model.ProcessPickListDetails(form, AuthUserId, ref trail);

                    CreateTrail(
                        pickListBo.Id,
                        "Create Pick List"
                    );
                    
                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = pickListBo.Id });
                }
                AddResult(Result);
            }
            //LoadPage();
            return View(model);
            */
            return RedirectToAction("Index");
        }

        // GET: PickList/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            PickListBo pickListBo = PickListService.Find(id);
            if (pickListBo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(pickListBo);
            return View(new PickListViewModel(pickListBo));
        }

        // POST: PickList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PickListViewModel model)
        {
            PickListBo pickListBo = PickListService.Find(id);
            if (pickListBo == null)
            {
                return RedirectToAction("Index");
            }

            Result childResult = new Result();
            List<PickListDetailBo> pickListDetailBos = model.GetPickListDetails(form, ref childResult);

            if (ModelState.IsValid)
            {
                pickListBo = model.FormBo(pickListBo.CreatedById, AuthUserId);

                TrailObject trail = GetNewTrailObject();

                if (childResult.Valid)
                {
                    Result = PickListService.Update(ref pickListBo, ref trail);
                    if (Result.Valid)
                    {
                        model.ProcessPickListDetails(pickListDetailBos, AuthUserId, ref trail);
                        CreateTrail(
                            pickListBo.Id,
                            "Update Pick List"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = pickListBo.Id });
                    }
                    AddResult(Result);
                }
                AddResult(childResult);
            }
            LoadPage(pickListBo, pickListDetailBos);
            return View(model);
        }

        // GET: PickList/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
            /*
            SetViewBagMessage();
            PickListBo pickListBo = PickListService.Find(id);
            if (pickListBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new PickListViewModel(pickListBo));
            */
        }

        // POST: PickList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PickListViewModel model)
        {
            /*
            PickListBo pickListBo = PickListService.Find(id);
            if (pickListBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = PickListService.Delete(pickListBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    pickListBo.Id,
                    "Delete Pick List"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = pickListBo.Id });
            */
            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? DepartmentId,
            string FieldName
        )
        {
            // type 1 = all
            // type 2 = filtered download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.PickLists.Select(PickListService.Expression());
            if (type == 2) // filtered dowload
            {
                if (DepartmentId != null) query = query.Where(q => q.DepartmentId == DepartmentId);
                if (!string.IsNullOrEmpty(FieldName)) query = query.Where(q => q.FieldName.Contains(FieldName));
            }

            var export = new ExportPickList();
            export.HandleTempDirectory();
            export.SetQuery(query);
            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage()
        {
            DropDownDepartment();
            GetDepartments();
            SetViewBagMessage();
        }

        public void LoadPage(PickListBo bo = null, List<PickListDetailBo> pickListDetailBos = null)
        {
            var usedBy = "";

            var pickListDepartments = PickListDepartmentService.GetByPickListId(bo.Id, true, true);

            if (pickListDepartments.Count > 0)
            {
                foreach (var dept in pickListDepartments)
                {
                    if (string.IsNullOrEmpty(usedBy))
                    {
                        usedBy = dept.DepartmentBo.Name;
                    }
                    else
                    {
                        usedBy = usedBy + ", " + dept.DepartmentBo.Name;
                    }
                }
            }
            

            ViewBag.UsedByDepartments = usedBy;

            ViewBag.MaxLength = 32;
            ViewBag.EnabledDelete = false;
            DropDownDepartment();
            GetDepartments();
            if (bo == null)
            {
                DropDownDepartment();
                ViewBag.EnabledDelete = true;
                // Create
            }
            else
            {
                // Edit
                if (bo.StandardOutputId != null)
                {
                    var property = StandardOutputBo.GetPropertyNameByType(bo.StandardOutputId.Value);
                    var maxLengthAttr = (new RiDataBo()).GetAttributeFrom<MaxLengthAttribute>(property);
                    if (maxLengthAttr != null)
                    {
                        ViewBag.MaxLength = maxLengthAttr.Length;
                    }
                }

                if (pickListDetailBos == null || pickListDetailBos.Count == 0)
                {
                    pickListDetailBos = PickListDetailService.GetByPickListId(bo.Id).ToList();
                }
                ViewBag.PickListDetailBos = pickListDetailBos;

                DepartmentBo departmentBo = DepartmentService.Find(bo.DepartmentId);
                ViewBag.DepartmentName = departmentBo?.Name;

                if (bool.TryParse(Util.GetConfig("EnabledDeletePickListDetail"), out bool result))
                {
                    ViewBag.EnabledDelete = result;
                }

                var editable = false;
                var editableDept = false;
                var saveable = false;

                if (UserService.Find(AuthUserId).DepartmentId == bo.DepartmentId)
                {
                    editable = true;

                    if (bo.IsEditable == false)
                    {
                        editable = false;
                    }
                }
                else if (bo.DepartmentId == DepartmentBo.DepartmentShared)
                {
                    editable = true;

                    if (bo.IsEditable == false)
                    {
                        editable = false;
                    }
                }

                if (UserService.Find(AuthUserId).DepartmentId == DepartmentBo.DepartmentIT)
                {
                    editableDept = true;
                }

                if (editable || editableDept)
                {
                    saveable = true;
                }


                ViewBag.IsEditable = editable;
                ViewBag.IsEditableDept = editableDept;
                ViewBag.IsSaveable = saveable;

            }
            SetViewBagMessage();
        }

        [HttpPost]
        public JsonResult ValidatePickListDetailDelete(int pickListId, int pickListDetailId)
        {
            bool valid = true;

            if (RiDataMappingService.CountByPickListDetailId(pickListDetailId) > 0 ||
                RiDataMappingDetailService.CountByPickListDetailId(pickListDetailId) > 0)
            {
                valid = false;
            }
            else
            {
                switch (pickListId)
                {
                    case PickListBo.InsuredGenderCode:
                        if (
                            RiDataCorrectionService.CountByInsuredGenderCodePickListDetailId(pickListDetailId) > 0 ||
                            RateDetailService.CountByInsuredGenderCodePickListDetailId(pickListDetailId) > 0 ||
                            FacMasterListingService.CountByInsuredGenderCodePickListDetailId(pickListDetailId) > 0
                        )
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.ReinsBasisCode:
                        if (
                            RiDataCorrectionService.CountByReinsBasisCodePickListDetailId(pickListDetailId) > 0 ||
                            TreatyBenefitCodeMappingService.CountByReinsBasisCodePickListDetailId(pickListDetailId) > 0 ||
                            Mfrs17CellMappingService.CountByReinsBasisCodePickListDetailId(pickListDetailId) > 0 ||
                            RateTableService.CountByReinsBasisCodePickListDetailId(pickListDetailId) > 0
                        )
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.PremiumFrequencyCode:
                        // Just set to false due to MFRS17 reporting
                        // Else have to count rateTable and Mfrs17reportingDetail
                        valid = false;
                        break;
                    case PickListBo.InsuredOccupationCode:
                        if (RateDetailService.CountByCedingOccupationCodePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.InsuredTobaccoUse:
                        if (RateDetailService.CountByCedingTobaccoUsePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.Mfrs17BasicRider:
                        if (Mfrs17CellMappingService.CountByBasicRiderPickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.CedingBenefitTypeCode:
                        PickListDetailBo pickListDetailBo = PickListDetailService.Find(pickListDetailId);
                        if (TreatyBenefitCodeMappingService.CountByCedingBenefitTypeCode(pickListDetailBo?.Code) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.TreatyType:
                        if (
                            AccountCodeMappingDetailService.CountByTreatyTypeId(pickListDetailId) > 0 ||
                            ItemCodeMappingDetailService.CountByTreatyTypeId(pickListDetailId) > 0 ||
                            TreatyCodeService.CountByTreatyTypePickListDetailId(pickListDetailId) > 0
                        )
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.TreatyStatus:
                        if (
                            TreatyCodeService.CountByTreatyStatusPickListDetailId(pickListDetailId) > 0
                        )
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.ValuationBenefitCode:
                        if (BenefitService.CountByValuationBenefitCodePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.CedingCompanyType:
                        if (CedantService.CountByCedingCompanyTypePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.BusinessOrigin:
                        if (
                            TreatyService.CountByBusinessOriginPickListDetailId(pickListDetailId) > 0 ||
                            ItemCodeService.CountByBusinessOriginPickListDetailId(pickListDetailId) > 0
                        )
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.LineOfBusiness:
                        if (TreatyCodeService.CountByLineOfBusinessPickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.CountryCode:
                        if (RetroPartyService.CountByCountryCodePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.ReinsuranceType:
                        if (TreatyPricingCedantService.CountByReinsuranceTypePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.AgeBasis:
                        if (TreatyPricingRateTableVersionService.CountByAgeBasisPickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    case PickListBo.RateGuarantee:
                        if (TreatyPricingRateTableVersionService.CountByRateGuaranteePickListDetailId(pickListDetailId) > 0)
                        {
                            valid = false;
                        }
                        break;
                    default:
                        break;
                }
            }

            return Json(new { valid });
        }

        private List<string> GetDepartments()
        {
            List<string> departments = new List<string>();
            foreach (var departmentBo in DepartmentService.Get())
            {
                departments.Add(departmentBo.Name);
            }
            ViewBag.Departments = departments;
            return departments;
        }

        [HttpPost]
        public JsonResult GetPickListDetailByStandardOutput(int standardOutputId)
        {
            IList<PickListDetailBo> pickListDetailBos = PickListDetailService.GetByStandardOutputId(standardOutputId);

            return Json(new { pickListDetailBos });
        }

        [HttpPost]
        public JsonResult GetPickListDetailByStandardClaimDataOutput(int standardClaimDataOutputId)
        {
            IList<PickListDetailBo> pickListDetailBos = PickListDetailService.GetByStandardClaimDataOutputId(standardClaimDataOutputId);

            return Json(new { pickListDetailBos });
        }
    }
}
