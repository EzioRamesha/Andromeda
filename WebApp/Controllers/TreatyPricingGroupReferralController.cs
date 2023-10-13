using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.RawFiles.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using Newtonsoft.Json;
using PagedList;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class TreatyPricingGroupReferralController : BaseController
    {
        public const string Controller = "TreatyPricingGroupReferral";

        // GET: TreatyPricingGroupReferral
        public ActionResult Index(
            int? GRCedantId,
            int? GRInsuredGroupNameId,
            int? IndustryNameId,
            int? ReferredTypeId,
            int? RiGroupSlipPIC,
            int? RiGroupSlipStatusId,
            int? GroupMasterLetterId,
            int? StatusId,
            int? RequestTypeId,
            int? VersionNo,
            int? GroupReferralPIC,
            int? QuotationTAT,
            int? InternalTAT,
            int? Score,
            int? WorkflowStatusId,
            int? ChecklistPending,
            int? InternalTeam,
            int? InternalTeamStatusId,
            string GroupReferralCode,
            string GroupReferralDescription,
            string RiGroupSlipCode,
            string RiGroupSlipConfirmationDate,
            string FirstReferralDate,
            string CoverageStartDate,
            string CoverageEndDate,
            string GroupSize,
            string GrossRiskPremium,
            string ReinsurancePremium,
            string WonVersion,
            string InternalTeamPIC,
            string QuotationSentDate,
            string RequestReceivedYear,
            string SortOrder,
            int? Results1Page,
            int? Results2Page,
            int? TabIndex
        )
        {
            DateTime? riGroupSlipConfirmationDate = Util.GetParseDateTime(RiGroupSlipConfirmationDate);
            DateTime? firstReferralDate = Util.GetParseDateTime(FirstReferralDate);
            DateTime? coverageStartDate = Util.GetParseDateTime(CoverageStartDate);
            DateTime? coverageEndDate = Util.GetParseDateTime(CoverageEndDate);
            double? grossRiskPremium = Util.StringToDouble(GrossRiskPremium);
            double? reinsurancePremium = Util.StringToDouble(ReinsurancePremium);
            double? groupSize = Util.StringToDouble(GroupSize);

            if (!TabIndex.HasValue) TabIndex = TreatyPricingGroupReferralBo.ActiveTabList;

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["GRCedantId"] = GRCedantId,
                ["GRInsuredGroupNameId"] = GRInsuredGroupNameId,
                ["IndustryNameId"] = IndustryNameId,
                ["ReferredTypeId"] = ReferredTypeId,
                ["RiGroupSlipPIC"] = RiGroupSlipPIC,
                ["RiGroupSlipStatusId"] = RiGroupSlipStatusId,
                ["GroupMasterLetterId"] = GroupMasterLetterId,
                ["StatusId"] = StatusId,
                ["RequestTypeId"] = RequestTypeId,
                ["VersionNo"] = VersionNo,
                ["GroupReferralPIC"] = GroupReferralPIC,
                ["GroupReferralCode"] = GroupReferralCode,
                ["GroupReferralDescription"] = GroupReferralDescription,
                ["RiGroupSlipCode"] = RiGroupSlipCode,
                ["RiGroupSlipConfirmationDate"] = riGroupSlipConfirmationDate.HasValue ? RiGroupSlipConfirmationDate : null,
                ["FirstReferralDate"] = firstReferralDate.HasValue ? FirstReferralDate : null,
                ["CoverageStartDate"] = coverageStartDate.HasValue ? CoverageStartDate : null,
                ["CoverageEndDate"] = coverageEndDate.HasValue ? CoverageEndDate : null,
                ["GroupSize"] = groupSize.HasValue ? GroupSize : null,
                ["GrossRiskPremium"] = grossRiskPremium.HasValue ? GrossRiskPremium : null,
                ["ReinsurancePremium"] = reinsurancePremium.HasValue ? ReinsurancePremium : null,
                ["WonVersion"] = WonVersion,
                ["QuotationTAT"] = QuotationTAT,
                ["InternalTAT"] = InternalTAT,
                ["Score"] = Score,
                ["WorkflowStatusId"] = WorkflowStatusId,
                ["ChecklistPending"] = ChecklistPending,
                ["QuotationSentDate"] = QuotationSentDate,
                ["RequestReceivedYear"] = RequestReceivedYear,

                ["SortOrder"] = SortOrder,

                ["ActiveTab"] = TabIndex,
                ["Results1Page"] = Results1Page,
                ["Results2Page"] = Results2Page,

                ["InternalTeam"] = InternalTeam,
                ["InternalTeamPIC"] = InternalTeamPIC,
                ["InternalTeamStatusId"] = InternalTeamStatusId,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortGRCedantId = GetSortParam("GRCedantId");
            ViewBag.SortGRInsuredGroupNameId = GetSortParam("GRInsuredGroupNameId");
            ViewBag.SortIndustryNameId = GetSortParam("IndustryNameId");
            ViewBag.SortReferredTypeId = GetSortParam("ReferredTypeId");
            ViewBag.SortRiGroupSlipPIC = GetSortParam("RiGroupSlipPIC");
            ViewBag.SortRiGroupSlipStatusId = GetSortParam("RiGroupSlipStatusId");
            ViewBag.SortGroupMasterLetterId = GetSortParam("GroupMasterLetterId");
            ViewBag.SortRequestTypeId = GetSortParam("RequestTypeId");
            ViewBag.SortVersionNo = GetSortParam("VersionNo");
            ViewBag.SortGroupReferralPIC = GetSortParam("GroupReferralPIC");
            ViewBag.SortGroupReferralCode = GetSortParam("GroupReferralCode");
            ViewBag.SortGroupReferralDescription = GetSortParam("GroupReferralDescription");
            ViewBag.SortRiGroupSlipCode = GetSortParam("RiGroupSlipCode");
            ViewBag.SortRiGroupSlipConfirmationDat = GetSortParam("RiGroupSlipConfirmationDate");
            ViewBag.SortFirstReferralDate = GetSortParam("FirstReferralDate");
            ViewBag.SortCoverageStartDate = GetSortParam("CoverageStartDate");
            ViewBag.SortCoverageEndDate = GetSortParam("CoverageEndDate");
            ViewBag.SortGroupSize = GetSortParam("GroupSize");
            ViewBag.SortGrossRiskPremium = GetSortParam("GrossRiskPremium");
            ViewBag.SortReinsurancePremium = GetSortParam("ReinsurancePremium");
            ViewBag.SortQuotationTAT = GetSortParam("QuotationTAT");
            ViewBag.SortInternalTAT = GetSortParam("InternalTAT");
            ViewBag.SortScore = GetSortParam("Score");
            ViewBag.SortWorkflowStatus = GetSortParam("WorkflowStatus");

            var query = _db.TreatyPricingGroupReferralVersions.GroupBy(p => p.TreatyPricingGroupReferralId)
                  .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault())
                  .Select(TreatyPricingGroupReferralViewModel.Expression());
            if (GRCedantId.HasValue) query = query.Where(q => q.GRCedantId == GRCedantId);
            if (GRInsuredGroupNameId.HasValue) query = query.Where(q => q.InsuredGroupId == GRInsuredGroupNameId);
            if (IndustryNameId.HasValue) query = query.Where(q => q.IndustryNameId == IndustryNameId);
            if (ReferredTypeId.HasValue) query = query.Where(q => q.ReferredTypeId == ReferredTypeId);
            if (RiGroupSlipPIC.HasValue) query = query.Where(q => q.RiGroupSlipPersonInChargeId == RiGroupSlipPIC);
            if (RiGroupSlipStatusId.HasValue) query = query.Where(q => q.RiGroupSlipStatus == RiGroupSlipStatusId);
            if (GroupMasterLetterId.HasValue) query = query.Where(q => q.GroupMasterLetterId == GroupMasterLetterId);
            if (StatusId.HasValue) query = query.Where(q => q.Status == StatusId);
            if (RequestTypeId.HasValue) query = query.Where(q => q.RequestTypePickListDetailId == RequestTypeId);
            if (VersionNo.HasValue) query = query.Where(q => q.Version == VersionNo);


            if (GroupReferralPIC.HasValue)
            {
                if (GroupReferralPIC == 9999999) query = query.Where(q => !q.GroupReferralPersonInChargeId.HasValue);
                else query = query.Where(q => q.GroupReferralPersonInChargeId == GroupReferralPIC);
            }
            if (!string.IsNullOrEmpty(GroupReferralCode)) query = query.Where(q => q.Code.Contains(GroupReferralCode));
            if (!string.IsNullOrEmpty(GroupReferralDescription)) query = query.Where(q => q.Description.Contains(GroupReferralDescription));
            if (!string.IsNullOrEmpty(RiGroupSlipCode)) query = query.Where(q => q.RiGroupSlipIdCode.Contains(RiGroupSlipCode));
            if (riGroupSlipConfirmationDate.HasValue) query = query.Where(q => q.RiGroupSlipConfirmationDate == riGroupSlipConfirmationDate);
            if (firstReferralDate.HasValue) query = query.Where(q => q.FirstReferralDate == firstReferralDate);
            if (coverageStartDate.HasValue) query = query.Where(q => q.CoverageStartDate == coverageStartDate);
            if (coverageEndDate.HasValue) query = query.Where(q => q.CoverageEndDate == coverageEndDate);
            if (groupSize.HasValue) query = query.Where(q => q.GroupSize == groupSize);
            if (grossRiskPremium.HasValue) query = query.Where(q => q.GrossRiskPremium == grossRiskPremium);
            if (reinsurancePremium.HasValue) query = query.Where(q => q.ReinsurancePremium == reinsurancePremium);
            if (!string.IsNullOrEmpty(WonVersion)) query = query.Where(q => q.WonVersion == WonVersion);
            if (Score.HasValue) query = query.Where(q => q.AverageScore == Score);

            if (QuotationTAT.HasValue)
            {
                if (QuotationTAT == 4) query = query.Where(q => q.QuotationTAT.Value > 3);
                else query = query.Where(q => q.QuotationTAT == QuotationTAT);
            }

            if (InternalTAT.HasValue)
            {
                if (InternalTAT == 4) query = query.Where(q => q.InternalTAT.Value > 3);
                else query = query.Where(q => q.InternalTAT == InternalTAT);
            }

            if (WorkflowStatusId.HasValue)
            {
                if (WorkflowStatusId == 999)
                    query = query.Where(q => q.WorkflowStatus != TreatyPricingGroupReferralBo.WorkflowStatusPendingClient);
                else
                    query = query.Where(q => q.WorkflowStatus == WorkflowStatusId);
            }
            if (InternalTeam.HasValue)
            {
                var ids = TreatyPricingGroupReferralChecklistService.GetIdsByInternalTeam(InternalTeam.Value, InternalTeamPIC, InternalTeamStatusId.Value);
                if (ids != null) query = query.Where(q => ids.Contains(q.Id));
            }

            if (ChecklistPending.HasValue)
            {
                if (ChecklistPending == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting) query = query.Where(q => q.ChecklistPendingUnderwriting == true);
                if (ChecklistPending == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHealth) query = query.Where(q => q.ChecklistPendingHealth == true);
                if (ChecklistPending == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamClaims) query = query.Where(q => q.ChecklistPendingClaims == true);
                if (ChecklistPending == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamBD) query = query.Where(q => q.ChecklistPendingBD == true);
                if (ChecklistPending == TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR) query = query.Where(q => q.ChecklistPendingCR == true);
            }

            if (!string.IsNullOrEmpty(QuotationSentDate))
            {
                if (QuotationSentDate == "true")
                {
                    query = query.Where(q => !q.QuotationSentDate.HasValue);
                }
            }

            if (!string.IsNullOrEmpty(RequestReceivedYear))
            {
                var requestReceivedYear = int.Parse(RequestReceivedYear);

                var yearStart = new DateTime(requestReceivedYear, 1, 1);
                var yearEnd = new DateTime(requestReceivedYear, 12, 31);

                query = query.Where(q => q.RequestReceivedDate >= yearStart && q.RequestReceivedDate <= yearEnd);
            }

            if (SortOrder == Html.GetSortAsc("GRInsuredGroupNameId")) query = query.OrderBy(q => q.InsuredGroupName.Name);
            else if (SortOrder == Html.GetSortDsc("GRInsuredGroupNameId")) query = query.OrderByDescending(q => q.InsuredGroupName.Name);
            else if (SortOrder == Html.GetSortAsc("IndustryNameId")) query = query.OrderBy(q => q.IndustryNamePickListDetail.Description);
            else if (SortOrder == Html.GetSortDsc("IndustryNameId")) query = query.OrderByDescending(q => q.IndustryNamePickListDetail.Description);
            else if (SortOrder == Html.GetSortAsc("GRCedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("GRCedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortAsc("ReferredTypeId")) query = query.OrderBy(q => q.ReferredTypePickListDetail.Description);
            else if (SortOrder == Html.GetSortDsc("ReferredTypeId")) query = query.OrderByDescending(q => q.ReferredTypePickListDetail.Description);
            else if (SortOrder == Html.GetSortAsc("RiGroupSlipPIC")) query = query.OrderBy(q => q.RiGroupSlipPersonInCharge.FullName);
            else if (SortOrder == Html.GetSortDsc("RiGroupSlipPIC")) query = query.OrderByDescending(q => q.RiGroupSlipPersonInCharge.FullName);
            else if (SortOrder == Html.GetSortAsc("GroupMasterLetterId")) query = query.OrderBy(q => q.GroupMasterLetter.Code);
            else if (SortOrder == Html.GetSortDsc("GroupMasterLetterId")) query = query.OrderByDescending(q => q.GroupMasterLetter.Code);
            else if (SortOrder == Html.GetSortAsc("RiGroupSlipStatusId")) query = query.OrderBy(q => q.RiGroupSlipStatus);
            else if (SortOrder == Html.GetSortDsc("RiGroupSlipStatusId")) query = query.OrderByDescending(q => q.RiGroupSlipStatus);
            else if (SortOrder == Html.GetSortAsc("VersionNo")) query = query.OrderBy(q => q.Version);
            else if (SortOrder == Html.GetSortDsc("VersionNo")) query = query.OrderByDescending(q => q.Version);
            else if (SortOrder == Html.GetSortAsc("RequestTypeId")) query = query.OrderBy(q => q.Version);
            else if (SortOrder == Html.GetSortDsc("RequestTypeId")) query = query.OrderByDescending(q => q.RequestTypePickListDetail.Description);
            else if (SortOrder == Html.GetSortAsc("GroupReferralPIC")) query = query.OrderBy(q => q.RequestTypePickListDetail.Description);
            else if (SortOrder == Html.GetSortDsc("GroupReferralPIC")) query = query.OrderByDescending(q => q.GroupReferralPersonInCharge.FullName);
            else if (SortOrder == Html.GetSortAsc("GroupReferralCode")) query = query.OrderBy(q => q.Code);
            else if (SortOrder == Html.GetSortDsc("GroupReferralCode")) query = query.OrderByDescending(q => q.Code);
            else if (SortOrder == Html.GetSortAsc("GroupReferralDescription")) query = query.OrderBy(q => q.Description);
            else if (SortOrder == Html.GetSortDsc("GroupReferralDescription")) query = query.OrderByDescending(q => q.Description);
            else if (SortOrder == Html.GetSortAsc("RiGroupSlipCode")) query = query.OrderBy(q => q.RiGroupSlipIdCode);
            else if (SortOrder == Html.GetSortDsc("RiGroupSlipCode")) query = query.OrderByDescending(q => q.RiGroupSlipIdCode);
            else if (SortOrder == Html.GetSortAsc("RiGroupSlipConfirmationDate")) query = query.OrderBy(q => q.RiGroupSlipConfirmationDate);
            else if (SortOrder == Html.GetSortDsc("RiGroupSlipConfirmationDate")) query = query.OrderByDescending(q => q.RiGroupSlipConfirmationDate);
            else if (SortOrder == Html.GetSortAsc("FirstReferralDate")) query = query.OrderBy(q => q.FirstReferralDate);
            else if (SortOrder == Html.GetSortDsc("FirstReferralDate")) query = query.OrderByDescending(q => q.FirstReferralDate);
            else if (SortOrder == Html.GetSortAsc("CoverageStartDate")) query = query.OrderBy(q => q.CoverageStartDate);
            else if (SortOrder == Html.GetSortDsc("CoverageStartDate")) query = query.OrderByDescending(q => q.CoverageStartDate);
            else if (SortOrder == Html.GetSortAsc("CoverageEndDate")) query = query.OrderBy(q => q.CoverageEndDate);
            else if (SortOrder == Html.GetSortDsc("CoverageEndDate")) query = query.OrderByDescending(q => q.CoverageEndDate);
            else if (SortOrder == Html.GetSortAsc("GroupSize")) query = query.OrderBy(q => q.GroupSize);
            else if (SortOrder == Html.GetSortDsc("GroupSize")) query = query.OrderByDescending(q => q.GroupSize);
            else if (SortOrder == Html.GetSortAsc("GrossRiskPremium")) query = query.OrderBy(q => q.GrossRiskPremium);
            else if (SortOrder == Html.GetSortDsc("GrossRiskPremium")) query = query.OrderByDescending(q => q.GrossRiskPremium);
            else if (SortOrder == Html.GetSortAsc("ReinsurancePremium")) query = query.OrderBy(q => q.ReinsurancePremium);
            else if (SortOrder == Html.GetSortDsc("ReinsurancePremium")) query = query.OrderByDescending(q => q.ReinsurancePremium);
            else if (SortOrder == Html.GetSortAsc("QuotationTAT")) query = query.OrderBy(q => q.QuotationTAT);
            else if (SortOrder == Html.GetSortDsc("QuotationTAT")) query = query.OrderByDescending(q => q.QuotationTAT);
            else if (SortOrder == Html.GetSortAsc("InternalTAT")) query = query.OrderBy(q => q.InternalTAT);
            else if (SortOrder == Html.GetSortDsc("InternalTAT")) query = query.OrderByDescending(q => q.InternalTAT);
            else if (SortOrder == Html.GetSortAsc("Score")) query = query.OrderBy(q => q.AverageScore);
            else if (SortOrder == Html.GetSortDsc("Score")) query = query.OrderByDescending(q => q.AverageScore);
            else query = query.OrderByDescending(q => q.FirstReferralDate);

            //_db.Database.CommandTimeout = 0;
            var queryB = _db.TreatyPricingGroupReferralFiles.AsNoTracking().Where(q => q.UploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeFile)
                    .Select(TreatyPricingGroupReferralFileService.Expression())
                    .OrderByDescending(q => q.CreatedAt);
            ViewBag.UploadedList = queryB.ToPagedList(Results2Page ?? 1, PageSize);
            ViewBag.TotalUploaded = queryB.Count();

            IndexPage();

            if (!GRCedantId.HasValue)
                if (ModelState.ContainsKey("GRCedantId"))
                    ModelState["GRCedantId"].Errors.Clear();

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Results1Page ?? 1, PageSize));
        }

        // POST: TreatyPricingGroupReferral/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(TreatyPricingGroupReferralBo groupReferralBo, bool HasDuplicateGroupReferral, int? DuplicateGroupReferralId, string Benefits, FormCollection form)
        {
            Result = TreatyPricingGroupReferralService.Result();
            if (groupReferralBo.CedantId == 0)
                Result.AddError("Ceding Company is Required.");
            if (!groupReferralBo.RiArrangementPickListDetailId.HasValue)
                Result.AddError("RI Arrangement is Required.");
            if (!string.IsNullOrEmpty(groupReferralBo.PrimaryTreatyPricingProductSelect))
                Result.AddError("Primary Product is Required.");
            if (groupReferralBo.RiArrangementPickListDetailId.HasValue)
            {
                var RiArrangementId = PickListDetailService.FindByPickListIdCode(PickListBo.RiArrangement, PickListDetailBo.RiArrangementCoinsuranceYRT);
                if (RiArrangementId != null)
                {
                    if (RiArrangementId.Id == groupReferralBo.RiArrangementPickListDetailId && string.IsNullOrEmpty(groupReferralBo.SecondaryTreatyPricingProductSelect))
                        Result.AddError("Secondary Product is Required.");
                }
            }
            if (groupReferralBo.InsuredGroupNameId == 0)
                Result.AddError("Insured Group Name is Required.");
            if (!string.IsNullOrEmpty(groupReferralBo.Description))
                Result.AddError("Group Referral Description is Required.");

            TreatyPricingGroupReferralVersionBo versionBo = new TreatyPricingGroupReferralVersionBo();
            if (HasDuplicateGroupReferral)
            {
                if (!DuplicateGroupReferralId.HasValue)
                    Result.AddError("Group Referral ID / Name is Required.");
                else
                {
                    TreatyPricingGroupReferralBo duplicate = TreatyPricingGroupReferralService.Find(DuplicateGroupReferralId.Value);
                    if (duplicate == null)
                        Result.AddError("No Group Referral found.");
                    else
                    {
                        groupReferralBo.IndustryNamePickListDetailId = duplicate.IndustryNamePickListDetailId;
                        groupReferralBo.ReferredTypePickListDetailId = duplicate.ReferredTypePickListDetailId;

                        TreatyPricingGroupReferralVersionBo duplicateVersion = duplicate.TreatyPricingGroupReferralVersionBos.OrderByDescending(q => q.Version).First();
                        versionBo = new TreatyPricingGroupReferralVersionBo(duplicateVersion)
                        {
                            Id = 0
                        };
                    }
                }
            }

            groupReferralBo.Code = TreatyPricingGroupReferralService.GetNextGroupReferralCode(groupReferralBo.CedantId);
            groupReferralBo.FirstReferralDate = Util.GetParseDateTime(groupReferralBo.FirstReferralDateStr);
            groupReferralBo.CoverageStartDate = Util.GetParseDateTime(groupReferralBo.CoverageStartDateStr);
            groupReferralBo.CoverageEndDate = Util.GetParseDateTime(groupReferralBo.CoverageEndDateStr);
            groupReferralBo.Status = TreatyPricingGroupReferralBo.StatusQuoting;
            groupReferralBo.WorkflowStatus = TreatyPricingGroupReferralBo.WorkflowStatusQuoting;
            groupReferralBo.CedantBo = CedantService.Find(groupReferralBo.CedantId);
            groupReferralBo.RiGroupSlipSharePointFolderPath = TreatyPricingGroupReferralService.GetSharePointPath(groupReferralBo);
            groupReferralBo.ReplySharePointFolderPath = TreatyPricingGroupReferralService.GetSharePointPath(groupReferralBo);
            groupReferralBo.CreatedById = AuthUserId;
            groupReferralBo.UpdatedById = AuthUserId;
            groupReferralBo.SetSelectValues();

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingGroupReferralService.Create(ref groupReferralBo, ref trail);
            if (Result.Valid)
            {
                versionBo.TreatyPricingGroupReferralId = groupReferralBo.Id;
                versionBo.Version = 1;
                //versionBo.RequestReceivedDate = DateTime.Now;
                versionBo.IsCompulsoryOrVoluntary = TreatyPricingGroupReferralVersionBo.IsCompulsory;
                versionBo.HasPerLifeRetro = true;
                versionBo.CreatedById = AuthUserId;
                versionBo.UpdatedById = AuthUserId;
                TreatyPricingGroupReferralVersionService.Create(ref versionBo, ref trail);

                if (!string.IsNullOrEmpty(Benefits))
                    TreatyPricingGroupReferralVersionBenefitService.Create(Benefits, versionBo.Id, groupReferralBo.RiArrangementPickListDetailId, AuthUserId, ref trail);

                var checklist = TreatyPricingGroupReferralChecklistBo.GetJsonDefaultRow(versionBo.Id);
                TreatyPricingGroupReferralChecklistService.Save(checklist, versionBo.Id, AuthUserId, ref trail);

                var checklistDetail = TreatyPricingGroupReferralChecklistDetailBo.GetJsonDefaultRow(versionBo.Id);
                TreatyPricingGroupReferralChecklistDetailService.Save(checklistDetail, versionBo.Id, AuthUserId, ref trail);

                CreateTrail(
                    groupReferralBo.Id,
                    "Create Treaty Pricing Group Referral"
                );

                SetCreateSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            SetErrorSessionMsgArr(Result.ToErrorArray().OfType<string>().ToList());
            return RedirectToAction("Index");
        }

        // GET: TreatyPricingGroupReferral/Edit/5
        public ActionResult Edit(int id, int versionId = 0, bool isEditMode = false)
        {
            if (!CheckObjectLockReadOnly(Controller, id, isEditMode) && (!CheckPower(Controller, AccessMatrixBo.PowerCompleteChecklist)
                || !CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverGroup) || !CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverReviewer)
                || !CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverHod) || !CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverCeo)))
                return RedirectDashboard();

            TreatyPricingGroupReferralBo bo = TreatyPricingGroupReferralService.Find(id, false);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TreatyPricingGroupReferralViewModel(bo);
            GetProductDetail(model);
            LoadPage(model, versionId);
            return View(model);
        }

        // POST: TreatyPricingGroupReferral/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyPricingGroupReferralViewModel model, FormCollection form, int versionId = 0)
        {
            var dbBo = TreatyPricingGroupReferralService.Find(id, false);
            var status = dbBo.Status;
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }

            if (!CheckObjectLock(Controller, id))
                return RedirectToAction("Edit", new { id, versionId });

            TreatyPricingGroupReferralVersionBo updatedVersionBo = null;
            dbBo.SetVersionObjects(dbBo.TreatyPricingGroupReferralVersionBos);
            if (dbBo.EditableVersion != model.CurrentVersion)
            {
                ModelState.AddModelError("", "You can only update details for the latest version");
            }
            else
            {
                updatedVersionBo = model.GetVersionBo((TreatyPricingGroupReferralVersionBo)dbBo.CurrentVersionObject);
                dbBo.AddVersionObject(updatedVersionBo);

                updatedVersionBo.TreatyPricingGroupReferralVersionBenefit = model.GetBenefits(updatedVersionBo.TreatyPricingGroupReferralVersionBenefit, form);
                foreach (string error in TreatyPricingGroupReferralVersionBenefitService.Validate(updatedVersionBo.TreatyPricingGroupReferralVersionBenefit))
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                dbBo = model.FormBo(dbBo.CreatedById, AuthUserId);
                dbBo.Status = status;
                dbBo.AverageScore = TreatyPricingGroupReferralVersionService.CalculateAverageScore(dbBo.Id);
                updatedVersionBo.QuotationTAT = TreatyPricingGroupReferralVersionService.GenerateQuotationTat(updatedVersionBo);
                if (!string.IsNullOrEmpty(model.ClientReplyDateStr) && !string.IsNullOrEmpty(model.EnquiryToClientDateStr)) // For "Current Internal TAT", if there is no "Client Reply Date" and "Enquiry to Client Date", follow "Current Quotation TAT"
                    updatedVersionBo.InternalTAT = TreatyPricingGroupReferralVersionService.GenerateInternalTat(updatedVersionBo);
                else
                    updatedVersionBo.InternalTAT = updatedVersionBo.QuotationTAT;
                updatedVersionBo.QuotationValidityDate = TreatyPricingGroupReferralVersionService.GenerateQuotationValidityDate(updatedVersionBo);
                updatedVersionBo.FirstQuotationSentWeek = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentWeek(updatedVersionBo);
                updatedVersionBo.FirstQuotationSentMonth = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentMonth(updatedVersionBo);
                updatedVersionBo.FirstQuotationSentQuarter = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentQuarter(updatedVersionBo);
                updatedVersionBo.FirstQuotationSentYear = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentYear(updatedVersionBo);
                dbBo.WorkflowStatus = TreatyPricingGroupReferralService.GenerateWorkflowStatus(model.Checklists, updatedVersionBo, dbBo);
                updatedVersionBo.Score = TreatyPricingGroupReferralVersionService.GenerateScore(updatedVersionBo.InternalTAT);
                dbBo.UpdatedById = AuthUserId;

                if (model.HasRiGroupSlip && string.IsNullOrEmpty(model.RiGroupSlipIdCode))
                    dbBo.RiGroupSlipCode = TreatyPricingGroupReferralService.GetNextRiGroupSlipCode(model.GRCedantId);

                var trail = GetNewTrailObject();
                Result = TreatyPricingGroupReferralService.Update(ref dbBo, ref trail);
                if (Result.Valid)
                {
                    if (updatedVersionBo != null)
                    {
                        updatedVersionBo.UpdatedById = AuthUserId;
                        string checklists = model.Checklists;
                        if (!string.IsNullOrEmpty(checklists))
                        {
                            List<TreatyPricingGroupReferralChecklistBo> checklistBos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistBo>>(checklists);
                            // Set Checklist Pending fields
                            foreach (var checklistBo in checklistBos.Where(q => q.TreatyPricingGroupReferralVersionId == updatedVersionBo.Id && q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview))
                            {
                                switch (checklistBo.InternalTeam)
                                {
                                    case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting:
                                        updatedVersionBo.ChecklistPendingUnderwriting = true;
                                        break;
                                    case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHealth:
                                        updatedVersionBo.ChecklistPendingHealth = true;
                                        break;
                                    case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamClaims:
                                        updatedVersionBo.ChecklistPendingClaims = true;
                                        break;
                                    case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamBD:
                                        updatedVersionBo.ChecklistPendingBD = true;
                                        break;
                                    case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR:
                                        updatedVersionBo.ChecklistPendingCR = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        TreatyPricingGroupReferralVersionService.Update(ref updatedVersionBo, ref trail);
                        TreatyPricingGroupReferralVersionBenefitService.Update(updatedVersionBo.TreatyPricingGroupReferralVersionBenefit, AuthUserId, ref trail);
                        TreatyPricingGroupReferralChecklistService.Update(model.Checklists, AuthUserId, ref trail);
                        TreatyPricingGroupReferralChecklistDetailService.Update(model.ChecklistDetails, AuthUserId, ref trail);
                    }

                    CreateTrail(
                        dbBo.Id,
                        "Update Treaty Pricing Group Referral"
                    );

                    ObjectLockController.DeleteObjectLock(Controller, id, AuthUserId);

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id, versionId });
                }
                AddResult(Result);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            }

            model.CurrentVersion = dbBo.CurrentVersion;
            model.CurrentVersionObject = dbBo.CurrentVersionObject;
            model.VersionObjects = dbBo.VersionObjects;
            LoadPage(model, versionId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditIndex(string RiGroupSlipItems)
        {
            var items = JsonConvert.DeserializeObject<IList<TreatyPricingGroupReferralBo>>(RiGroupSlipItems);
            foreach (var item in items)
            {
                if (item.HasRiGroupSlip)
                {
                    if (item.RiGroupSlipStatus.HasValue || item.RiGroupSlipPersonInChargeId.HasValue || item.RiGroupSlipConfirmationDate.HasValue)
                    {
                        var dbBo = TreatyPricingGroupReferralService.Find(item.Id);
                        if (dbBo == null)
                        {
                            SetErrorSessionMsg("Error occured during saving Ri Group Slip values");
                            return RedirectToAction("Index");
                        }

                        dbBo.RiGroupSlipPersonInChargeId = item.RiGroupSlipPersonInChargeId;
                        dbBo.RiGroupSlipStatus = item.RiGroupSlipStatus;
                        dbBo.RiGroupSlipConfirmationDate = item.RiGroupSlipConfirmationDate;
                        dbBo.UpdatedById = AuthUserId;
                        TreatyPricingGroupReferralService.Update(ref dbBo);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult UploadIndex(int? Page)
        {
            _db.Database.CommandTimeout = 0;
            var query = _db.TreatyPricingGroupReferralFiles.AsNoTracking().Where(q => q.UploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeFile)
                    .Select(TreatyPricingGroupReferralFileService.Expression())
                    .OrderBy(q => q.CreatedAt);
            ViewBag.Total = query.Count();

            //DropDownCedant();
            return PartialView("_UploadTab", query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownEmpty();
            DropDownRiArrangement();
            DropDownIndustryName();
            DropDownReferredType();
            DropDownRequest();
            DropDownWonVersion();

            // List
            DropDownCedant();
            DropDownInsuredGroupName();
            DropDownRiGroupSlipStatus();
            DropDownStatus();
            DropDownGroupMasterLetter();
            DropDownWorkflowStatus();
            DropDownTATDays();
            DropDownScore();
            DropDownPendingChecklist();

            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            ViewBag.UsersGR = DropDownUser(departmentId: departmentBD);
            ViewBag.UsersRiGroupSlip = DropDownUser(departmentId: departmentBD);

            // Add New
            ViewBag.DropDownActiveCedants = DropDownCedant(CedantBo.StatusActive);
            ViewBag.DropDownActiveInsuredGroupName = DropDownInsuredGroupName(InsuredGroupNameBo.StatusActive);

            ViewBag.RIarrangementCoinsuranceYRT = PickListDetailService.FindByPickListIdCode(PickListBo.RiArrangement, PickListDetailBo.RiArrangementCoinsuranceYRT)?.Id;

            var entity = new TreatyPricingGroupReferral();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            ViewBag.EnableCompleteChecklist = CheckPower(Controller, AccessMatrixBo.PowerCompleteChecklist);
            ViewBag.EnableUltimaApproverGroupChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverGroup);
            ViewBag.EnableUltimaApproverReviewerChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverReviewer);
            ViewBag.EnableUltimaApproverHodChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverHod);
            ViewBag.EnableUltimaApproverCeoChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverCeo);
            ViewBag.EnableEdit = CheckPower(Controller, AccessMatrixBo.AccessMatrixCRUD.C.ToString());

            SetViewBagMessage();
        }

        public void LoadPage(TreatyPricingGroupReferralViewModel model, int versionId = 0)
        {
            AuthUserName();
            DropDownEmpty();
            DropDownIndustryName();
            DropDownReferredType();
            DropDownRequest();
            DropDownPremiumType();
            DropDownRiGroupSlipStatus();
            DropDownVersion(model);
            DropDownInsuredGroupName(InsuredGroupNameBo.StatusActive, model.InsuredGroupId);
            DropDownMonth();
            DropDownYear();
            DropDownQuarter();
            DropDownUltimateApprover();
            DropDownStatus();
            //DropDownBenefit();
            DropDownProductBenefit(model.PrimaryTreatyPricingProductSelect);
            DropDownAgeBasis();
            DropDownProfitComm();
            DropDownRiArrangement();
            DropDownOtherSpecialReinsuranceAgreement();
            DropDownCompulsoryVoluntary();
            //GetBenefits();

            ViewBag.UnderwritingMethodCodes = GetPickListDetailCodeDescription(PickListBo.UnderwritingMethod);

            DropDownTreatyPricingUnderwritingLimit(model.GRCedantId);

            ViewBag.LatestVersion = model.CurrentVersion;
            // Remark & Document
            string downloadDocumentUrl = Url.Action("Download", "Document");
            GetRemarkDocument(model.ModuleId, model.Id, downloadDocumentUrl);
            GetRemarkSubjects();

            //ViewBag.ChecklistStatusHistoryBos = GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString());
            ViewBag.StatusHistoryBos = GetStatusHistories(model.ModuleId, model.Id, downloadDocumentUrl);

            ViewBag.UploadBos = TreatyPricingGroupReferralFileService.GetByUploadedTypeTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.UploadedTypeTable, model.Id);
            ViewBag.TableTypes = GetPickListDetailIdDropDownByPickListId(PickListBo.TableType);
            ViewBag.RiGroupSlipTemplates = DropDownGRTemplate(model.GRCedantId, "Ri Group Slip", model.RiGroupSlipTemplateId);
            ViewBag.ReplyTemplates = DropDownGRTemplate(model.GRCedantId, "Reply Template", model.ReplyTemplateId);

            // Checklist Team & GroupPOV list
            ViewBag.Groups = TreatyPricingGroupReferralChecklistDetailBo.GetItems();
            ViewBag.Teams = TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeams();

            // Emails
            ViewBag.RecipientEmails = UserService.GetEmailUsers();

            // Checklist user list
            int departmentUW = Util.GetConfigInteger("Underwriting");
            int departmentHealth = Util.GetConfigInteger("Health");
            int departmentClaims = Util.GetConfigInteger("Claims");
            int departmentBD = Util.GetConfigInteger("BDAndGroup");
            int departmentCR = Util.GetConfigInteger("ComplianceAndRisk");

            ViewBag.UsersUnderwriting = GetUsers(departmentId: departmentUW);
            ViewBag.UsersHealth = GetUsers(departmentId: departmentHealth);
            ViewBag.UsersClaim = GetUsers(departmentId: departmentClaims);
            ViewBag.UsersBD = GetUsers(departmentId: departmentBD);
            ViewBag.UsersCR = GetUsers(departmentId: departmentCR);
            ViewBag.UsersGroup = UserService.GetUsersByModulePower(model.ModuleId, AccessMatrixBo.PowerUltimaApproverGroup);
            ViewBag.UsersReviewer = UserService.GetUsersByModulePower(model.ModuleId, AccessMatrixBo.PowerUltimaApproverReviewer);
            ViewBag.UsersHOD = UserService.GetUsersByModulePower(model.ModuleId, AccessMatrixBo.PowerUltimaApproverHod);
            ViewBag.UsersCEO = UserService.GetUsersByModulePower(model.ModuleId, AccessMatrixBo.PowerUltimaApproverCeo);

            // GR & Ri Group Slip user list
            ViewBag.UsersGR = DropDownUser(UserBo.StatusActive, model.GroupReferralPersonInChargeId, departmentId: departmentBD);
            ViewBag.UsersRiGroupSlip = DropDownUser(UserBo.StatusActive, model.RiGroupSlipPersonInChargeId, departmentId: departmentBD);

            // Changelog
            GetObjectVersionChangelog(ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString(), model.Id, model);
            DropDownWorkflowObjectTypes();
            DropDownWorkflowStatus();
            if (versionId > 0)
            {
                model.SetCurrentVersionObject(versionId);
            }

            var entity = new TreatyPricingGroupReferral();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Remarks");
            ViewBag.RemarksMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 128;

            ViewBag.EnableView = CheckPower(Controller, AccessMatrixBo.AccessMatrixCRUD.R.ToString());

            SetViewBagMessage();
        }

        public void GetProductDetail(TreatyPricingGroupReferralViewModel model)
        {
            List<int> ProductVersionIds = new List<int> { model.PrimaryTreatyPricingProductVersionId };
            if (model.SecondaryTreatyPricingProductVersionId.HasValue)
                ProductVersionIds.Add(model.SecondaryTreatyPricingProductVersionId.Value);

            var query = _db.TreatyPricingProductVersions.AsNoTracking()
                   .Where(q => ProductVersionIds.Any(i => i == q.Id))
                   .Select(TreatyPricingGroupReferralViewModel.ExpressionProducts())
                   .ToList();

            var primaryProduct = query.Where(x => x.TreatyPricingProductVersionId == model.PrimaryTreatyPricingProductVersionId).FirstOrDefault();
            if (primaryProduct != null)
            {
                model.PrimaryTreatyPricingProductCode = primaryProduct.ProductCode;
                model.PrimaryTreatyPricingProductName = primaryProduct.ProductName;
                model.PrimaryTreatyPricingProductVersion = primaryProduct.VersionNo;
                model.QuotationnName = primaryProduct.ProductQuotationnName;
            }

            if (model.SecondaryTreatyPricingProductVersionId.HasValue)
            {
                var secondaryProduct = query.Where(x => x.TreatyPricingProductVersionId == model.SecondaryTreatyPricingProductVersionId.Value).FirstOrDefault();
                if (secondaryProduct != null)
                {
                    model.SecondaryTreatyPricingProductCode = secondaryProduct.ProductCode;
                    model.SecondaryTreatyPricingProductName = secondaryProduct.ProductName;
                    model.SecondaryTreatyPricingProductVersion = secondaryProduct.VersionNo;
                }
            }
        }

        public List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownGRStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownWorkflowStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralBo.WorkflowStatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralBo.GetWorkflowStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownWorkflowStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownRequest()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RequestType);
            ViewBag.DropDownRequests = items;
            return items;
        }

        public List<SelectListItem> DropDownRiGroupSlipStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralBo.RiGroupSlipStatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralBo.GetRiGroupSlipStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRiGroupSlipStatus = items;
            return items;
        }

        public List<SelectListItem> DropDownOtherSpecialReinsuranceAgreement()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralVersionBenefitBo.OtherSpecialReinsuranceArrangementMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralVersionBenefitBo.GetOtherSpecialReinsuranceArrangementName(i), Value = i.ToString() });
            }
            ViewBag.DropDownOtherSpecialReinsuranceAgreements = items;
            return items;
        }

        public List<SelectListItem> DropDownGRTemplate(int? cedantId = null, string type = "", int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            IList<TemplateBo> templateBos = new List<TemplateBo>();

            if (cedantId.HasValue && cedantId > 0)
            {
                templateBos = TemplateService.GetByCedantAndDocumentType(cedantId ?? 0, type);
            }
            else
            {
                templateBos = TemplateService.Get();
            }

            foreach (var template in templateBos)
            {
                var selected = template.Id == selectedId;
                items.Add(new SelectListItem { Text = template.Code, Value = template.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownGRTemplates = items;
            return items;
        }

        public List<SelectListItem> DropDownQuarter()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "Q1", Value = "Q1" });
            items.Add(new SelectListItem { Text = "Q2", Value = "Q2" });
            items.Add(new SelectListItem { Text = "Q3", Value = "Q3" });
            items.Add(new SelectListItem { Text = "Q4", Value = "Q4" });
            ViewBag.DropDownQuarters = items;
            return items;
        }

        public List<SelectListItem> DropDownUltimateApprover()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralChecklistDetailBo.MaxUltimateApprover))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralChecklistDetailBo.GetUltimateApproverName(i), Value = i.ToString() });
            }
            ViewBag.DropDownUltimateApprovers = items;
            return items;
        }

        public List<SelectListItem> DropDownGroupMasterLetter()
        {
            var items = GetEmptyDropDownList();
            foreach (var bo in TreatyPricingGroupMasterLetterService.Get())
            {
                items.Add(new SelectListItem { Text = bo.Code, Value = bo.Id.ToString() });
            }
            ViewBag.DropDownGroupMasterLetters = items;
            return items;
        }

        public List<SelectListItem> DropDownWonVersion()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "Loss", Value = "Loss" });
            items.Add(new SelectListItem { Text = "Won", Value = "Won" });
            ViewBag.DropDownWonVersions = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingUnderwritingLimit(int? cedantId)
        {
            var items = GetEmptyDropDownList();

            if (cedantId.HasValue)
            {
                bool withVersion = true;
                var query = _db.TreatyPricingUwLimitVersions.AsNoTracking()
                    .Where(q => q.TreatyPricingUwLimit.TreatyPricingCedant.CedantId == cedantId)
                    .OrderBy(q => q.TreatyPricingUwLimitId).ThenBy(q => q.Version)
                    .Select(TreatyPricingGroupReferralViewModel.ExpressionUwLimits());

                foreach (var uwLimit in query.ToList())
                {
                    string value = uwLimit.TreatyPricingUwLimitVersionId.ToString();
                    string text = string.Format("{0} - {1}", uwLimit.UwLimitId, uwLimit.UwLimitName);
                    if (withVersion)
                    {
                        value = string.Format("{0}|{1}", value, uwLimit.TreatyPricingUwLimitId.ToString());
                        text = string.Format("{0} v{1}.0", text, uwLimit.VersionNo.ToString());
                    }

                    items.Add(new SelectListItem { Text = text, Value = value });
                }
            }

            ViewBag.DropDownTreatyPricingUnderwritingLimits = items;
            return items;
        }

        public List<SelectListItem> DropDownCompulsoryVoluntary()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingGroupReferralVersionBo.IsCompulsoryOrVoluntaryMax))
            {
                items.Add(new SelectListItem { Text = TreatyPricingGroupReferralVersionBo.GetCompulsoryOrVoluntaryName(i), Value = i.ToString() });
            }
            ViewBag.DropDownCompulsoryVoluntary = items;
            return items;
        }

        public List<SelectListItem> DropDownPendingChecklist()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting, TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR))
            {
                items.Add(new SelectListItem { Text = string.Format("Pending {0}", TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeamName(i)), Value = i.ToString() });
            }
            ViewBag.DropDownPendingChecklists = items;
            return items;
        }

        public ActionResult CreateVersion(TreatyPricingGroupReferralBo bo, bool duplicatePreviousVersion = false)
        {
            bo.SetVersionObjects(TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralId(bo.Id));
            TreatyPricingGroupReferralVersionBo nextVersionBo;
            TreatyPricingGroupReferralVersionBo previousVersionBo = (TreatyPricingGroupReferralVersionBo)bo.CurrentVersionObject;
            List<TreatyPricingGroupReferralVersionBenefitBo> nextVersionBenefitBo = new List<TreatyPricingGroupReferralVersionBenefitBo>();

            if (duplicatePreviousVersion)
            {
                nextVersionBo = new TreatyPricingGroupReferralVersionBo(previousVersionBo)
                {
                    Id = 0,
                };
            }
            else
            {
                nextVersionBo = new TreatyPricingGroupReferralVersionBo()
                {
                    TreatyPricingGroupReferralId = bo.Id,
                };
            }

            nextVersionBo.Version = bo.EditableVersion + 1;
            nextVersionBo.CreatedById = AuthUserId;
            nextVersionBo.UpdatedById = AuthUserId;

            ObjectVersionChangelog changelog = null;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingGroupReferralVersionService.Create(ref nextVersionBo, ref trail);
            if (Result.Valid)
            {
                List<TreatyPricingGroupReferralVersionBenefitBo> previousVersionBenefitBo = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(bo.CurrentVersionObjectId).ToList();

                foreach (var verBenefit in previousVersionBenefitBo)
                {
                    verBenefit.Id = 0;
                    verBenefit.TreatyPricingGroupReferralVersionId = nextVersionBo.Id;
                    verBenefit.CreatedById = AuthUserId;
                    verBenefit.UpdatedById = AuthUserId;
                    nextVersionBenefitBo.Add(verBenefit);
                    TreatyPricingGroupReferralVersionBenefitService.Create(verBenefit);
                }
                if (!nextVersionBenefitBo.IsNullOrEmpty()) nextVersionBo.TreatyPricingGroupReferralVersionBenefit = JsonConvert.SerializeObject(nextVersionBenefitBo);

                //ViewBag.VersionBenefits = bo.TreatyPricingGroupReferralVersionBenefitBos;
                var checklist = TreatyPricingGroupReferralChecklistBo.GetJsonDefaultRow(nextVersionBo.Id);
                TreatyPricingGroupReferralChecklistService.Save(checklist, nextVersionBo.Id, AuthUserId, ref trail);

                var checklistDetail = TreatyPricingGroupReferralChecklistDetailBo.GetJsonDefaultRow(nextVersionBo.Id);
                TreatyPricingGroupReferralChecklistDetailService.Save(checklistDetail, nextVersionBo.Id, AuthUserId, ref trail);

                UserTrailBo userTrail = CreateTrail(
                    nextVersionBo.Id,
                    "Create Treaty Pricing Group Referral Version"
                );

                changelog = new ObjectVersionChangelog(nextVersionBo, userTrail);
                changelog.FormatBetweenVersionTrail(previousVersionBo);
            }

            bo.AddVersionObject(nextVersionBo);
            var versions = DropDownVersion(bo);

            return Json(new { bo, versions });
        }

        public ActionResult DownloadTrackingCase(
            string downloadToken,
            int? CedantId,
            string CoverageStartDate,
            string CoverageEndDate,
            string RequestReceivedStartDate,
            string RequestReceivedEndDate,
            bool CoverageBlankDate,
            bool RequestReceivedBlankDate)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.CedantId = CedantId;
            Params.CoverageStartDate = CoverageStartDate;
            Params.CoverageEndDate = CoverageEndDate;
            Params.RequestReceivedStartDate = RequestReceivedStartDate;
            Params.RequestReceivedEndDate = RequestReceivedEndDate;
            Params.CoverageBlankDate = CoverageBlankDate;
            Params.RequestReceivedBlankDate = RequestReceivedBlankDate;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportGroupReferralTrackingCase(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public ActionResult DownloadListing(
            string downloadToken,
            int type,
            int? GRCedantId,
            int? GRInsuredGroupNameId,
            int? IndustryNameId,
            int? ReferredTypeId,
            int? RiGroupSlipPIC,
            int? RiGroupSlipStatusId,
            int? GroupMasterLetterId,
            int? StatusId,
            int? RequestTypeId,
            int? VersionNo,
            int? GroupReferralPIC,
            int? QuotationTAT,
            int? InternalTAT,
            int? Score,
            int? WorkflowStatusId,
            int? ChecklistPending,
            string GroupReferralCode,
            string GroupReferralDescription,
            string RiGroupSlipCode,
            string RiGroupSlipConfirmationDate,
            string FirstReferralDate,
            string CoverageStartDate,
            string CoverageEndDate,
            string GroupSize,
            string GrossRiskPremium,
            string ReinsurancePremium,
            string WonVersion)
        {
            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            dynamic Params = new ExpandoObject();
            Params.CedantId = GRCedantId;
            Params.InsuredGroupNameId = GRInsuredGroupNameId;
            Params.IndustryNameId = IndustryNameId;
            Params.ReferredTypeId = ReferredTypeId;
            Params.RiGroupSlipPICId = RiGroupSlipPIC;
            Params.RiGroupSlipStatusId = RiGroupSlipStatusId;
            Params.GroupMasterLetterId = GroupMasterLetterId;
            Params.Status = StatusId;
            Params.QuotationTAT = QuotationTAT;
            Params.InternalTAT = InternalTAT;
            Params.Score = Score;
            Params.WorkflowStatusId = WorkflowStatusId;
            Params.GroupReferralCode = GroupReferralCode;
            Params.Description = GroupReferralDescription;
            Params.RiGroupSlipCode = RiGroupSlipCode;
            Params.RiGroupSlipConfirmationDate = RiGroupSlipConfirmationDate;
            Params.FirstReferralDate = FirstReferralDate;
            Params.CoverageStartDate = CoverageStartDate;
            Params.CoverageEndDate = CoverageEndDate;
            Params.RequestTypeId = RequestTypeId;
            Params.VersionNo = VersionNo;
            Params.GroupReferralPIC = GroupReferralPIC;
            Params.GroupSize = GroupSize;
            Params.GrossRiskPremium = GrossRiskPremium;
            Params.ReinsurancePremium = ReinsurancePremium;
            Params.WonVersion = WonVersion;
            Params.ChecklistPending = ChecklistPending;

            var export = new GenerateExportData();
            ExportBo exportBo = export.CreateExportGroupReferral(AuthUserId, Params);

            if (exportBo.Total <= export.ExportInstantDownloadLimit)
            {
                export.Process(ref exportBo, _db);
                Session.Add("DownloadExport", true);
            }

            return RedirectToAction("Edit", "Export", new { exportBo.Id });
        }

        public ActionResult DownloadUpload(int id)
        {
            TreatyPricingGroupReferralFileBo bo = TreatyPricingGroupReferralFileService.Find(id);
            DownloadFile(bo.GetLocalPath(), bo.FileName);
            return null;
        }

        [HttpPost]
        public ActionResult DeleteUpload(int id)
        {
            List<string> errors = new List<string>();

            TreatyPricingGroupReferralFileBo bo = TreatyPricingGroupReferralFileService.Find(id);
            if (bo != null)
            {
                try
                {
                    if (System.IO.File.Exists(bo.GetLocalPath()))
                        System.IO.File.Delete(bo.GetLocalPath());

                    TreatyPricingGroupReferralFileService.Delete(bo);
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
                }
            }
            else
            {
                errors.Add(MessageBag.NoRecordFound);
            }

            return Json(new { errors });
        }

        [HttpPost]
        public ActionResult UploadedTableFiles()
        {
            List<string> errors = new List<string>();
            IList<TreatyPricingGroupReferralFileBo> treatyPricingGroupReferralFileBos = null;

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    string treatyPricingGroupReferralId = Request["treatyPricingGroupReferralId"];
                    string tableTypeId = Request["tableTypeId"];
                    string uploadType = Request["uploadedType"];

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingGroupReferralFileService.Result();

                    int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                    if (fileSize >= 2)
                        Result.AddError("Uploaded file's size exceeded 2 GB");

                    if (Result.Valid)
                    {
                        var treatyPricingGroupReferralFileBo = new TreatyPricingGroupReferralFileBo
                        {
                            UploadedType = int.Parse(uploadType),
                            FileName = fileName,
                            Status = TreatyPricingGroupReferralFileBo.StatusPending,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        if (!string.IsNullOrEmpty(treatyPricingGroupReferralId)) treatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId = int.Parse(treatyPricingGroupReferralId);
                        if (!string.IsNullOrEmpty(tableTypeId)) treatyPricingGroupReferralFileBo.TableTypePickListDetailId = int.Parse(tableTypeId);

                        treatyPricingGroupReferralFileBo.FormatHashFileName();
                        string path = treatyPricingGroupReferralFileBo.GetLocalPath();
                        Util.MakeDir(path);

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingGroupReferralFileService.Create(ref treatyPricingGroupReferralFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                treatyPricingGroupReferralFileBo.Id,
                                "Create Treaty Pricing Group Referral File"
                            );
                            file.SaveAs(path);
                        }
                    }
                    errors = Result.ToErrorArray().OfType<string>().ToList();

                    if (!string.IsNullOrEmpty(treatyPricingGroupReferralId))
                        treatyPricingGroupReferralFileBos = TreatyPricingGroupReferralFileService.GetByUploadedTypeTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.UploadedTypeTable, int.Parse(treatyPricingGroupReferralId));
                    else
                        treatyPricingGroupReferralFileBos = TreatyPricingGroupReferralFileService.GetByUploadedTypeTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.UploadedTypeFile);
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
                }
            }
            else
            {
                errors.Add("No files selected.");
            }

            return Json(new { errors, treatyPricingGroupReferralFileBos });
        }

        [HttpPost]
        public ActionResult Upload()
        {
            List<string> errors = new List<string>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];
                    string fileName = Path.GetFileName(file.FileName);

                    Result = TreatyPricingGroupReferralFileService.Result();

                    string[] extensions = { ".xls", ".xlsx" };
                    if (!extensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                        Result.AddError("Allowed file of type: .xls, .xlsx");
                    else
                    {
                        int fileSize = file.ContentLength / 1024 / 1024 / 1024;
                        if (fileSize >= 2)
                            Result.AddError("Uploaded file's size exceeded 2 GB");
                    }

                    if (Result.Valid)
                    {
                        var treatyPricingGroupReferralFileBo = new TreatyPricingGroupReferralFileBo
                        {
                            UploadedType = TreatyPricingGroupReferralFileBo.UploadedTypeFile,
                            FileName = fileName,
                            Status = TreatyPricingGroupReferralFileBo.StatusPending,
                            CreatedById = AuthUserId,
                            UpdatedById = AuthUserId,
                        };

                        treatyPricingGroupReferralFileBo.FormatHashFileName();
                        string path = treatyPricingGroupReferralFileBo.GetLocalPath();
                        Util.MakeDir(path);

                        TrailObject trail = GetNewTrailObject();
                        Result = TreatyPricingGroupReferralFileService.Create(ref treatyPricingGroupReferralFileBo, ref trail);
                        if (Result.Valid)
                        {
                            CreateTrail(
                                treatyPricingGroupReferralFileBo.Id,
                                "Create Treaty Pricing Group Referral File"
                            );
                            file.SaveAs(path);
                        }
                    }
                    errors = Result.ToErrorArray().OfType<string>().ToList();
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("Error occurred. Error details: ", ex.Message));
                }
            }
            else
            {
                errors.Add("No files selected.");
            }

            return Json(new { errors });
        }

        [HttpPost]
        public ActionResult GetVersionDetails(int groupReferralId, int groupReferralVersion, bool isEditMode)
        {
            var treatyPricingGroupReferralVersionId = TreatyPricingGroupReferralVersionService.GetVersionId(groupReferralId, groupReferralVersion);
            var latestVersionBo = TreatyPricingGroupReferralVersionService.GetLatestVersionByTreatyPricingGroupReferralId(groupReferralId);

            var checklistBos = TreatyPricingGroupReferralChecklistService.GetByTreatyPricingGroupReferralVersionId(treatyPricingGroupReferralVersionId);
            var checklistDetailBos = TreatyPricingGroupReferralChecklistDetailService.GetByTreatyPricingGroupReferralVersionId(treatyPricingGroupReferralVersionId);
            var checklistRemark = TreatyPricingGroupReferralVersionService.Find(treatyPricingGroupReferralVersionId).ChecklistRemark;

            //Checklist permissions
            string username = AuthUser.UserName;
            int? departmentId = AuthUser.DepartmentId;
            bool isLatestVersion = latestVersionBo?.Version == groupReferralVersion;

            bool enableCompleteChecklist = CheckPower(Controller, AccessMatrixBo.PowerCompleteChecklist);
            bool enableUltimaApproverGroupChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverGroup);
            bool enableUltimaApproverReviewerChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverReviewer);
            bool enableUltimaApproverHodChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverHod);
            bool enableUltimaApproverCeoChecklist = CheckPower(Controller, AccessMatrixBo.PowerUltimaApproverCeo);
            bool enableEdit = CheckPower(Controller, AccessMatrixBo.AccessMatrixCRUD.U.ToString()) && isEditMode;

            if (departmentId != null)
            {
                foreach (var bo in checklistBos)
                {
                    bo.DisableRequest = !enableEdit;

                    bo.DisableButtons = true;
                    bo.DisablePersonInCharge = true;

                    if (enableEdit)
                    {
                        bo.DisableRequest = !isLatestVersion;
                        bo.DisableButtons = !isLatestVersion;
                        bo.DisablePersonInCharge = !isLatestVersion;
                    }
                    else
                    {
                        switch (bo.InternalTeam)
                        {
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting:
                                if (isLatestVersion && departmentId == DepartmentBo.DepartmentUnderwriting && enableCompleteChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHealth:
                                if (isLatestVersion && departmentId == DepartmentBo.DepartmentHealth && enableCompleteChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamClaims:
                                if (isLatestVersion && departmentId == DepartmentBo.DepartmentClaim && enableCompleteChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamBD:
                                if (isLatestVersion && departmentId == DepartmentBo.DepartmentBD && enableCompleteChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR:
                                if (isLatestVersion && departmentId == DepartmentBo.DepartmentComplianceRisk && enableCompleteChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamGroup:
                                if (isLatestVersion && enableUltimaApproverGroupChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamReviewer:
                                if (isLatestVersion && enableUltimaApproverReviewerChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHOD:
                                if (isLatestVersion && enableUltimaApproverHodChecklist)
                                    bo.DisableButtons = false;
                                break;
                            case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCEO:
                                if (isLatestVersion && enableUltimaApproverCeoChecklist && enableUltimaApproverGroupChecklist)
                                    bo.DisableButtons = false;
                                break;
                            default:
                                break;
                        }
                    }

                    if (bo.DisableButtons == false)
                    {
                        bo.DisableButtons = true;

                        if (!string.IsNullOrEmpty(bo.InternalTeamPersonInCharge))
                        {
                            var internalTeamPersonInCharge = bo.InternalTeamPersonInCharge.Split(',').ToList();

                            foreach (string personInCharge in internalTeamPersonInCharge)
                            {
                                if (personInCharge.Trim() == username)
                                    bo.DisableButtons = false;
                            }
                        }
                    }
                }
            }

            return Json(new { checklistBos, checklistDetailBos, checklistRemark });
        }

        [HttpPost]
        public ActionResult GetChecklistHistoriesVersion(int groupReferralId, int groupReferralVersion)
        {
            string downloadDocumentUrl = Url.Action("Download", "Document");
            var moduleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString()).Id;

            var checklistStatusHistoryBos = GetStatusHistories(moduleId, groupReferralId, downloadDocumentUrl, ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString());
            if (!checklistStatusHistoryBos.IsNullOrEmpty())
                checklistStatusHistoryBos = checklistStatusHistoryBos.Where(q => q.Version == groupReferralVersion).ToList();

            return Json(new { checklistStatusHistoryBos });
        }

        [HttpPost]
        public ActionResult DropDownVersion(int treatyPricingGroupReferralId)
        {
            var items = GetEmptyDropDownList(displaySelect: false);
            var items2 = GetEmptyDropDownList(displaySelect: false);

            var query = _db.TreatyPricingGroupReferralVersions.AsNoTracking()
                    .Where(q => q.TreatyPricingGroupReferralId == treatyPricingGroupReferralId)
                    .Select(TreatyPricingGroupReferralViewModel.ExpressionVersions());

            foreach (var bo in query.ToList())
            {
                items.Add(new SelectListItem { Text = string.Format("{0}.0", bo.VersionNo), Value = bo.TreatyPricingGroupReferralVersionId.ToString() });
                items2.Add(new SelectListItem { Text = string.Format("{0}.0", bo.VersionNo), Value = bo.VersionNo.ToString() });
            }
            ViewBag.DropDownVersions = items;
            ViewBag.DropDownVersions2 = items2;
            return Json(new { versions = ViewBag.DropDownVersions, versions2 = ViewBag.DropDownVersions2 });
        }

        [HttpPost]
        public ActionResult GenerateRiGroupCode(int cedantId)
        {
            var CedantCount = TreatyPricingGroupReferralService.CountByCedantIdHasRiGroupSlip(cedantId, true) + 1;
            var CedantBo = CedantService.Find(cedantId);

            var Code = string.Format("{0}-{1}-{2}", CedantBo.Code, DateTime.Now.Year, CedantCount.ToString("D4"));
            return Json(new { RiGroupSlipCode = Code });
        }

        [HttpPost]
        public ActionResult GetDropDownProduct(int? cedantId)
        {
            var items = DropDownTreatyPricingProduct(cedantId);
            return Json(new { DropDownProducts = items });
        }

        [HttpPost]
        public ActionResult DropDownDuplicateGroupReferral(int? cedantId)
        {
            var items = GetEmptyDropDownList();
            if (cedantId.HasValue)
            {
                foreach (var group in TreatyPricingGroupReferralService.GetByCedantId(cedantId, false))
                {
                    string text = group.Code;
                    if (group.InsuredGroupNameBo != null)
                        text = string.Format("{0}-{1}", text, group.InsuredGroupNameBo.Name);

                    items.Add(new SelectListItem { Text = text, Value = group.Id.ToString() });

                }
            }
            return Json(new { DropDownDuplicateGroupReferrals = items });

        }

        [HttpPost]
        public ActionResult GetProductBenefits(string primaryProductSelect)
        {
            List<ProductBenefitBo> BenefitBos = new List<ProductBenefitBo>() { };

            string[] primaryProductValues = primaryProductSelect.ToString().Split('|');
            if (primaryProductValues.Length == 2)
            {
                int? versionId = Util.GetParseInt(primaryProductValues[0]);
                int? objectId = Util.GetParseInt(primaryProductValues[1]);

                if (versionId.HasValue && objectId.HasValue)
                {
                    var PrimaryProductBo = TreatyPricingProductService.Find(objectId);
                    var PrimaryProductVersion = TreatyPricingProductVersionService.Find(versionId);
                    if (PrimaryProductVersion != null)
                    {
                        if (!string.IsNullOrEmpty(PrimaryProductVersion.TreatyPricingProductBenefit))
                        {
                            var PrimaryProductBenefitBos = JsonConvert.DeserializeObject<List<TreatyPricingProductBenefitBo>>(PrimaryProductVersion.TreatyPricingProductBenefit);
                            if (PrimaryProductBenefitBos.Any())
                            {
                                foreach (var bo in PrimaryProductBenefitBos)
                                {
                                    BenefitBos.Add(new ProductBenefitBo
                                    {
                                        BenefitId = bo.Id,
                                        BenefitCode = bo.BenefitCode,
                                        BenefitName = bo.Name,
                                        ProductCode =
                                        PrimaryProductBo.Code,
                                        ProductName = PrimaryProductBo.Name,
                                        ProductVersion = PrimaryProductVersion.Version
                                    });
                                }
                            }
                        }
                    }
                }

            }

            return Json(new { ProductBenefits = BenefitBos });
        }

        public List<SelectListItem> DropDownProductBenefit(string primaryProductSelect)
        {
            var items = GetEmptyDropDownList();
            IList<TreatyPricingProductBenefitBo> BenefitBos = null;

            string[] primaryProductValues = primaryProductSelect.ToString().Split('|');
            if (primaryProductValues.Length == 2)
            {
                int? versionId = Util.GetParseInt(primaryProductValues[0]);
                int? objectId = Util.GetParseInt(primaryProductValues[1]);

                if (versionId.HasValue && objectId.HasValue)
                    BenefitBos = TreatyPricingProductBenefitService.GetByProductVersionIdTreatyPricingProductIdForDropDownProductBenefit(versionId.Value, objectId.Value, foreign: false);

                foreach (var benefit in BenefitBos)
                {
                    items.Add(new SelectListItem { Text = benefit.BenefitBo.ToString(), Value = benefit.BenefitBo.Id.ToString() });
                }
            }

            ViewBag.DropDownProductBenefits = BenefitBos;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingProduct(int? cedantId)
        {
            var items = GetEmptyDropDownList();
            if (cedantId.HasValue)
            {
                bool withVersion = true;
                var query = _db.TreatyPricingProductVersions.AsNoTracking()
                    .Where(q => q.TreatyPricingProduct.TreatyPricingCedant.CedantId == cedantId)
                    .Select(TreatyPricingGroupReferralViewModel.ExpressionProducts())
                    .OrderBy(q => q.TreatyPricingProductVersionId).ThenBy(q => q.VersionNo);

                foreach (var product in query.ToList())
                {
                    string value = product.TreatyPricingProductVersionId.ToString();
                    string text = string.Format("{0} - {1}", product.ProductCode, product.ProductName);
                    if (withVersion)
                    {
                        value = string.Format("{0}|{1}", value, product.TreatyPricingProductId.ToString());
                        text = string.Format("{0} v{1}.0", text, product.VersionNo.ToString());
                    }

                    items.Add(new SelectListItem { Text = text, Value = value });
                }
            }
            ViewBag.DropDownProducts = items;
            return items;
        }

        public List<SelectListItem> DropDownTATDays()
        {
            var items = GetEmptyDropDownList();

            items.Add(new SelectListItem { Text = "0 days", Value = "0" });
            items.Add(new SelectListItem { Text = "1 day", Value = "1" });
            items.Add(new SelectListItem { Text = "2 days", Value = "2" });
            items.Add(new SelectListItem { Text = "3 days", Value = "3" });
            items.Add(new SelectListItem { Text = "more than 3 days", Value = "4" });

            ViewBag.DropDownTATDays = items;
            return items;
        }

        public List<SelectListItem> DropDownScore()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, 5))
            {
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.DropDownScores = items;
            return items;
        }

        [HttpPost]
        public ActionResult GetUploadErrors(int uploadId)
        {
            TreatyPricingGroupReferralFileBo bo = TreatyPricingGroupReferralFileService.Find(uploadId);
            return Json(new { bo });
        }

        #region SharePoint
        [HttpPost]
        public JsonResult GenerateSharePointFile(int versionId, string type, string typeFull, int templateCode, string sharePointFolderPath, string sharePointLink)
        {
            List<string> errors = new List<string>();
            List<string> confirmations = new List<string>();
            bool callGenerate = false;
            bool isCampaignSpec = false;
            string fileName = "";
            string path = "";
            string editLink = "";

            LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - " + HttpContext.Request.RawUrl);

            try
            {
                LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Group Referral version bo");
                TreatyPricingGroupReferralVersionBo versionBo = TreatyPricingGroupReferralVersionService.Find(versionId);
                LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateSharePointFile - Group Referral version bo");
                LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Group Referral bo");
                TreatyPricingGroupReferralBo bo = TreatyPricingGroupReferralService.Find(versionBo.TreatyPricingGroupReferralId);
                LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateSharePointFile - Group Referral bo");

                if (string.IsNullOrEmpty(sharePointFolderPath))
                {
                    if (type == "RiGroupSlip")
                    {
                        bo.RiGroupSlipSharePointFolderPath = TreatyPricingGroupReferralService.GetSharePointPath(bo);
                        sharePointFolderPath = bo.RiGroupSlipSharePointFolderPath;
                    }

                    if (type == "Reply")
                    {
                        bo.ReplySharePointFolderPath = TreatyPricingGroupReferralService.GetSharePointPath(bo);
                        sharePointFolderPath = bo.ReplySharePointFolderPath;
                    }
                }

                TemplateBo templateBo = TemplateService.Find(templateCode);

                if (templateBo != null)
                {
                    TemplateDetailBo templateDetailBo = TemplateDetailService.GetLatestByTemplateId(templateBo.Id);

                    if (templateDetailBo != null)
                    {
                        path = Util.GetUploadPath("Template") + @"\" + templateDetailBo.HashFileName;
                        fileName = templateDetailBo.FileName;

                        if (sharePointLink != null && sharePointLink != "")
                        {
                            confirmations.Add(typeFull + " file has been created for this version.");
                        }

                        LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Check Folder/File exist in SharePoint");
                        using (var sp = new SharePointContext())
                        {
                            if (!sp.FileFolderExists(sharePointFolderPath))
                            {
                                confirmations.Add("This folder doesn't exist in SharePoint.");
                            }
                            else if (sp.FileFolderExists(sharePointFolderPath + "/" + fileName))
                            {
                                confirmations.Add("File already exist in SharePoint.");
                            }
                            else
                            {
                                callGenerate = true;
                            }

                            LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Update Group Referral template Id");
                            if (type == "RiGroupSlip")
                            {
                                bo.RiGroupSlipTemplateId = templateCode;
                                bo.RiGroupSlipVersionId = versionId;
                            }

                            if (type == "Reply")
                            {
                                bo.ReplyTemplateId = templateCode;
                                bo.ReplyVersionId = versionId;
                            }

                            TreatyPricingGroupReferralService.Update(ref bo);
                            LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Update Group Referral template Id");
                        }
                        LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateSharePointFile - Check Folder/File exist in SharePoint");

                        LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Go to GenerateFileInSharePoint");
                        if (callGenerate)
                        {
                            GenerateFileInSharePoint(versionId, type, path, fileName, sharePointFolderPath, isCampaignSpec, ref errors, ref editLink);
                        }
                        LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateSharePointFile - Go to GenerateFileInSharePoint");
                    }
                    else
                    {
                        errors.Add("Template details not found.");
                    }
                }
                else
                {
                    errors.Add("Template not found.");
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
                LogHelper("Group Referral ver id: " + versionId + ", (GET)GenerateSharePointFile - Exception: " + ex.Message);
            }

            LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateSharePointFile");

            return Json(new { errors, confirmations, fileName, path, editLink });
        }

        [HttpPost]
        public JsonResult GenerateSharePointFileConfirmed(int versionId, string type, string newFileName, string sharePointFolderPath, string localPath, bool isCampaignSpec)
        {
            List<string> errors = new List<string>();
            string editLink = "";

            GenerateFileInSharePoint(versionId, type, localPath, newFileName, sharePointFolderPath, isCampaignSpec, ref errors, ref editLink);

            return Json(new { errors, editLink });
        }

        public void GenerateFileInSharePoint(int versionId, string type, string localPath, string newFileName, string sharePointFolderPath, bool isCampaignSpec, ref List<string> errors, ref string editLink)
        {
            var processSuccess = true;

            LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - " + HttpContext.Request.RawUrl);
            try
            {
                LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - Group Referral version bo");
                TreatyPricingGroupReferralVersionBo versionBo = TreatyPricingGroupReferralVersionService.Find(versionId);
                LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - Group Referral version bo");
                LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - Group Referral bo");
                TreatyPricingGroupReferralBo bo = TreatyPricingGroupReferralService.Find(versionBo.TreatyPricingGroupReferralId);
                LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - Group Referral bo");

                LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - Copy file");
                //Copy file
                string folderPath = Util.GetTreatyPricingGroupReferralUploadPath(type) + "_Copied";
                string sourceFile = localPath;
                string destinationFile = Path.Combine(folderPath, Path.GetFileName(localPath));
                Util.MakeDir(destinationFile);
                System.IO.File.Copy(sourceFile, destinationFile, true);
                LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - Copy file");

                //Start processing file
                if (type == "RiGroupSlip")
                {
                    LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - ProcessGroupReferralRiGroupSlip");
                    var process = new ProcessGroupReferralRiGroupSlip()
                    {
                        FilePath = destinationFile,
                        TreatyPricingGroupReferralBo = bo,
                        TreatyPricingGroupReferralVersionBo = versionBo,

                    };
                    processSuccess = process.Process();

                    if (!processSuccess)
                    {
                        if (process.Errors.Count() > 0 && process.Errors != null)
                        {
                            errors.AddRange(process.Errors.ToList());
                        }
                    }

                    LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - ProcessGroupReferralRiGroupSlip");
                }

                if (type == "Reply")
                {
                    LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - ProcessGroupReferralReply");
                    var process = new ProcessGroupReferralReply()
                    {
                        FilePath = destinationFile,
                        TreatyPricingGroupReferralBo = bo,
                        TreatyPricingGroupReferralVersionBo = versionBo,
                    };
                    processSuccess = process.Process();

                    if (!processSuccess)
                    {
                        if (process.Errors.Count() > 0 && process.Errors != null)
                        {
                            errors.AddRange(process.Errors.ToList());
                        }
                    }
                    LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - ProcessGroupReferralReply");
                }

                if (processSuccess)
                {
                    LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - Upload to SharePoint");
                    //Upload to SharePoint
                    using (var sp = new SharePointContext())
                    {
                        LogHelper("Group Referral ver id: " + versionId + ", Start (GET)GenerateFileInSharePoint - Path: destinationFile: " + sharePointFolderPath + ", newFileName: " + newFileName + ", sharePointFolderPath: " + sharePointFolderPath);
                        string fullSharePointPath = sharePointFolderPath + "/" + newFileName;

                        sp.AddNewFolder(sharePointFolderPath);
                        sp.UploadFile(destinationFile
                            , newFileName
                            , (sharePointFolderPath == null ? "" : sharePointFolderPath));

                        //editLink = sp.GetCopyLinkURL(fullSharePointPath);
                        editLink = sp.GetCopyLinkURL(sharePointFolderPath);
                        LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - Path");

                        LogHelper("Group Referral ver id: " + versionId + ", Start (POST)GenerateFileInSharePoint - Update Group Referral SharePointLink");
                        if (type == "RiGroupSlip")
                        {
                            bo.RiGroupSlipSharePointLink = editLink;
                        }

                        if (type == "Reply")
                        {
                            bo.ReplySharePointLink = editLink;
                        }

                        TreatyPricingGroupReferralService.Update(ref bo);
                        LogHelper("Group Referral ver id: " + versionId + ", End (POST)GenerateFileInSharePoint - Update Group Referral SharePointLink");
                    }
                    LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint - Upload to SharePoint");
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Error occurred. Error details: {0}", ex.Message));
                LogHelper("Group Referral ver id: " + versionId + ", (GET)GenerateFileInSharePoint - Exception: " + ex.Message);
            }
            LogHelper("Group Referral ver id: " + versionId + ", End (GET)GenerateFileInSharePoint");
        }
        #endregion

        #region Quotation checklist
        public JsonResult GetChecklistStatusName(int status)
        {
            string statusName = TreatyPricingGroupReferralChecklistBo.GetStatusName(status);
            return Json(new { statusName });
        }

        public JsonResult NotifyQuotationChecklist(string referralCode, string internalTeam)
        {
            List<int?> userIds = new List<int?>();
            List<string> usernames = internalTeam.Split(',').ToList();

            int id = TreatyPricingGroupReferralService.FindByCode(referralCode).Id;
            string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingGroupReferral", new { id = id });

            foreach (string username in usernames)
            {
                int userId = UserService.FindByUsername(username.Trim()).Id;
                userIds.Add(userId);
            }

            List<string> errors = new List<string>();

            for (int i = 0; i < userIds.Count; i++)
            {
                int? userId = userIds[i];
                if (!userId.HasValue)
                {
                    errors.Insert(i, "User not selected");
                    continue;
                }

                UserBo userBo = UserService.Find(userId);
                if (userBo == null)
                {
                    errors.Insert(i, "User not found");
                    continue;
                }

                GetNewEmail(EmailBo.TypeNotifyGroupReferralChecklist, userBo.Email, userBo.Id);
                EmailBo.AddData(userBo.FullName);
                EmailBo.AddData(referralCode);
                EmailBo.AddData(link);

                if (!GenerateMail(showWarning: false))
                    errors.Insert(i, MessageBag.EmailError);

                EmailBo bo = EmailBo;
                Services.EmailService.Create(ref bo);
            }

            return Json(new { errors });
        }
        #endregion

        #region Save Benefits
        [HttpPost]
        public JsonResult SaveBenefits(List<TreatyPricingGroupReferralVersionBenefitBo> versionBenefitBos, int? empty = 0, int? deleteVersionId = 0, bool? checkDuplicate = false)
        {
            var error = "";
            if (empty == 0 && checkDuplicate == false)
            {
                var benefitIds = versionBenefitBos.Select(a => a.BenefitId).ToList();
                var benefitToBeDeletedBo = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(versionBenefitBos[0].TreatyPricingGroupReferralVersionId).Where(a => !benefitIds.Contains(a.BenefitId)).ToList();

                var duplicateItems = benefitIds.GroupBy(x => x).SelectMany(g => g.Skip(1)).Distinct().ToList();
                if (duplicateItems.Count > 0)
                {
                    var duplicateItem = "";
                    foreach (var item in duplicateItems)
                    {
                        duplicateItem = item + ", " + duplicateItem;
                    }
                    error = "Duplicate Benefits is Found: " + duplicateItem;
                    return Json(new { error });
                }

                if (benefitToBeDeletedBo.Count > 0)
                {
                    foreach (var deleteBenefitBo in benefitToBeDeletedBo)
                    {
                        TreatyPricingGroupReferralVersionBenefitService.Delete(deleteBenefitBo);
                    }
                }

                foreach (var versionBenefitBo in versionBenefitBos)
                {
                    var dbBenefit = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionId(versionBenefitBo.TreatyPricingGroupReferralVersionId).Where(a => a.BenefitId == versionBenefitBo.BenefitId).FirstOrDefault();

                    if (dbBenefit != null)
                    {
                        versionBenefitBo.UpdatedById = AuthUserId;
                        TreatyPricingGroupReferralVersionBenefitService.Update(versionBenefitBo);
                    }
                    else
                    {
                        versionBenefitBo.CreatedById = AuthUserId;
                        versionBenefitBo.UpdatedById = AuthUserId;
                        TreatyPricingGroupReferralVersionBenefitService.Create(versionBenefitBo);
                    }
                }
            }
            else if (empty == 1 && checkDuplicate == false)
            {
                if (deleteVersionId.HasValue && deleteVersionId != 0)
                    TreatyPricingGroupReferralVersionBenefitService.DeleteAllByTreatyPricingGroupReferralVersionId(deleteVersionId.Value);
            }
            else if (checkDuplicate == true)
            {
                var benefitIds = versionBenefitBos.Select(a => a.BenefitId).ToList();
                var duplicateItems = benefitIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();


                if (duplicateItems.Count == 1)
                {
                    error = "Duplicate Benefits is Found: " + BenefitService.Find(duplicateItems[0]).ToString();
                    return Json(new { error });
                }
                else if (duplicateItems.Count > 1)
                {
                    var duplicateItem = "";
                    foreach (var item in duplicateItems)
                    {
                        duplicateItem = item + ", " + BenefitService.Find(item).ToString();
                    }
                    error = "Duplicate Benefits is Found: " + duplicateItem;
                }
            }


            return Json(new { error });
        }

        #endregion

        #region Status History
        [HttpPost]
        public JsonResult UpdateStatus(StatusHistoryBo statusHistoryBo)
        {
            TreatyPricingGroupReferralBo bo = TreatyPricingGroupReferralService.Find(statusHistoryBo.ObjectId, false);
            if (bo == null)
                return Json(new { });

            string[] emails = null;
            if (!string.IsNullOrEmpty(statusHistoryBo.Emails))
            {
                List<string> error = new List<string>();

                emails = statusHistoryBo.Emails.Split(',').Select(e => e.Trim()).ToArray();
                foreach (string email in emails)
                {
                    if (!email.Trim().IsValidEmail())
                    {
                        error.Add(string.Format(MessageBag.InvalidEmail, email));
                    }
                }

                if (error.Count() > 0)
                    return Json(new { error });
            }

            var verBo = TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralId(bo.Id).OrderByDescending(a => a.Version).FirstOrDefault();
            var checklistBos = TreatyPricingGroupReferralChecklistService.GetByTreatyPricingGroupReferralVersionId(verBo.Id);
            var checklistBosStr = JsonConvert.SerializeObject(checklistBos);

            bo.Status = statusHistoryBo.Status;
            bo.UpdatedById = AuthUserId;
            bo.WorkflowStatus = TreatyPricingGroupReferralService.GenerateWorkflowStatus(checklistBosStr, verBo, bo);

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingGroupReferralService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                statusHistoryBo = StatusHistoryController.Create(statusHistoryBo, AuthUserId, AuthUser.UserName, ref trail);

                if (emails != null)
                {
                    List<string> recipientNames = new List<string>();

                    string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Edit", "TreatyPricingGroupReferral", new { id = bo.Id });

                    foreach (string email in emails)
                    {
                        UserBo userBo = UserService.FindByEmail(email);
                        string fullName = "";
                        if (userBo != null)
                            fullName = " " + userBo.FullName;

                        GetNewEmail(EmailBo.TypeNotifyGroupReferralStatusUpdate, email, userBo?.Id);

                        EmailBo.AddData(fullName);
                        EmailBo.AddData(bo.Code);
                        EmailBo.AddData(statusHistoryBo.StatusName);
                        EmailBo.AddData(link);
                        EmailBo.AddData(statusHistoryBo.RemarkBo?.Content ?? "");
                        GenerateMail(showWarning: false);

                        EmailBo.ModuleController = ModuleBo.ModuleController.StatusHistory.ToString();
                        EmailBo.ObjectId = statusHistoryBo.Id;
                        SaveEmail(ref trail);

                        recipientNames.Add(userBo?.FullName ?? email);
                    }

                    statusHistoryBo.RecipientNames = string.Join(", ", recipientNames);
                }

                CreateTrail(
                    bo.Id,
                    "Update Treaty Pricing Group Referral Status"
                );

                return Json(new { statusHistoryBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }

        [HttpPost]
        public JsonResult UpdateChecklistStatus(StatusHistoryBo statusHistoryBo, string checklistDetails)
        {
            int subObjectId = statusHistoryBo.SubObjectId ?? default(int);
            TreatyPricingGroupReferralChecklistBo bo = TreatyPricingGroupReferralChecklistService.Find(subObjectId);
            if (bo == null)
                return Json(new { });

            bo.Status = statusHistoryBo.Status;
            bo.InternalTeamPersonInCharge = statusHistoryBo.PersonInCharge;
            bo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = TreatyPricingGroupReferralChecklistService.Update(ref bo, ref trail);
            if (Result.Valid)
            {
                if (!string.IsNullOrEmpty(checklistDetails))
                    TreatyPricingGroupReferralChecklistDetailService.Update(checklistDetails, AuthUserId, ref trail);

                var verBo = TreatyPricingGroupReferralVersionService.Find(bo.TreatyPricingGroupReferralVersionId);
                var checklistBos = TreatyPricingGroupReferralChecklistService.GetByTreatyPricingGroupReferralVersionId(verBo.Id);
                var checklistBosStr = JsonConvert.SerializeObject(checklistBos);
                var grBo = TreatyPricingGroupReferralService.Find(verBo.TreatyPricingGroupReferralId, false);

                grBo.UpdatedById = AuthUserId;
                grBo.WorkflowStatus = TreatyPricingGroupReferralService.GenerateWorkflowStatus(checklistBosStr, verBo, grBo);
                TreatyPricingGroupReferralService.Update(ref grBo, ref trail);

                // Set Checklist Pending fields
                foreach (var checklistBo in checklistBos.Where(q => q.Status == TreatyPricingGroupReferralChecklistBo.StatusPendingReview))
                {
                    switch (checklistBo.InternalTeam)
                    {
                        case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamUnderwriting:
                            verBo.ChecklistPendingUnderwriting = true;
                            break;
                        case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamHealth:
                            verBo.ChecklistPendingHealth = true;
                            break;
                        case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamClaims:
                            verBo.ChecklistPendingClaims = true;
                            break;
                        case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamBD:
                            verBo.ChecklistPendingBD = true;
                            break;
                        case TreatyPricingGroupReferralChecklistBo.DefaultInternalTeamCR:
                            verBo.ChecklistPendingCR = true;
                            break;
                        default:
                            break;
                    }
                }
                TreatyPricingGroupReferralVersionService.Update(ref verBo, ref trail);

                statusHistoryBo = StatusHistoryController.Create(statusHistoryBo, AuthUserId, AuthUser.UserName, ref trail);

                CreateTrail(
                    bo.Id,
                    "Update Treaty Pricing Group Referral Checklist Status"
                );

                statusHistoryBo.Department = TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeamName(bo.InternalTeam);
                statusHistoryBo.StatusName = TreatyPricingGroupReferralChecklistBo.GetStatusName(statusHistoryBo.Status);
                statusHistoryBo.PersonInCharge = AuthUser.UserName;
                return Json(new { statusHistoryBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }
        #endregion

        public ActionResult Details(int id)
        {
            ViewBag.HipsTableBos = TreatyPricingGroupReferralHipsTableService.GetByTreatyPricingGroupReferralId(id);
            ViewBag.GtlTableBos = TreatyPricingGroupReferralGtlTableService.GetByTreatyPricingGroupReferralId(id);
            ViewBag.GhsTableBos = TreatyPricingGroupReferralGhsTableService.GetByTreatyPricingGroupReferralId(id);

            ViewBag.GroupReferralID = id;
            return View("UploadedTableDetails");
        }

        public ActionResult DownloadTableTypeExcelFile(string tableType)
        {
            string filepath = "";
            switch (tableType)
            {
                case PickListDetailBo.TableTypeHips:
                    //filepath = Util.GetWebAppDocumentFilePath("HIPS_Template.xlsx");
                    var process = new ProcessTreatyPricingGroupReferral();
                    filepath = process.GenerateHipsTemplate();
                    break;
                case PickListDetailBo.TableTypeGtlClaim:
                    filepath = Util.GetWebAppDocumentFilePath("GTLClaimExperience_Template.xlsx");
                    break;
                case PickListDetailBo.TableTypeGtlRate:
                    filepath = Util.GetWebAppDocumentFilePath("GTLTakeoverRates_UnitRates_Template.xlsx");
                    break;
                case PickListDetailBo.TableTypeGtlAge:
                    filepath = Util.GetWebAppDocumentFilePath("GTLTakeoverRates_AgeBanded_Template.xlsx");
                    break;
                case PickListDetailBo.TableTypeGtlSa:
                    filepath = Util.GetWebAppDocumentFilePath("GTLBasisOfSA_Template.xlsx");
                    break;
                case PickListDetailBo.TableTypeGhsClaim:
                    filepath = Util.GetWebAppDocumentFilePath("GHSClaimExperience_Template.xlsx");
                    break;
            }

            return File(filepath, MimeMapping.GetMimeMapping(filepath), Path.GetFileName(filepath));
        }

        public void DownloadFile(string filePath, string fileName)
        {
            // For download big file
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

        public class ProductBenefitBo
        {
            public int BenefitId { get; set; }
            public string BenefitCode { get; set; }
            public string BenefitName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public int ProductVersion { get; set; }
        }

        public static void LogHelper(string message)
        {
            bool lockObj = Convert.ToBoolean(Util.GetConfig("Logging"));
            string filePath = string.Format("{0}/TreatyPricingGroupReferralViewSummary".AppendDateFileName(".txt"), Util.GetLogPath("TreatyPricingGroupReferralView"));

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

    }
}