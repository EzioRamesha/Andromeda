using BusinessObject;
using BusinessObject.Identity;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.ProcessDatas.Exports;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using System;
using System.Data;
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
    public class ProductFeatureMappingController : BaseController
    {
        public const string Controller = "ProductFeatureMapping";

        public const string ClearSessionValue = "NULL";
        public const string SessionCedantIdName = "TreatyBenefitCodeMapping.CedantId";

        // GET: TreatyBenefitCodeMapping
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string CedantId,
            string CedingPlanCode,
            string Description,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string CedingTreatyCode,
            string CampaignCode,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            int? ReinsBasisCodePickListDetailId,
            int? AttainedAgeFrom,
            int? AttainedAgeTo,
            string ReportingStartDate,
            string ReportingEndDate,
            string UnderwriterRatingFrom,
            string UnderwriterRatingTo,
            string OriSumAssuredFrom,
            string OriSumAssuredTo,
            int? ReinsuranceIssueAgeFrom,
            int? ReinsuranceIssueAgeTo,
            int? BenefitId,
            string TreatyCodeId,
            int? ProfitCommPickListDetailId,
            int? MaxExpiryAge,
            int? MinIssueAge,
            int? MaxIssueAge,
            string MaxUwRating,
            string ApLoading,
            string MinAar,
            string MaxAar,
            string AblAmount,
            string RetentionShare,
            string RetentionCap,
            string RiShare,
            string RiShareCap,
            string RiShare2,
            string RiShareCap2,
            string ServiceFee,
            string WakalahFee,
            string EffectiveDate,
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
            if (!string.IsNullOrEmpty(CedantId))
            {
                if (CedantId == ClearSessionValue)
                {
                    CedantId = null;
                    Session.Remove(SessionCedantIdName);
                }
                else
                {
                    Session[SessionCedantIdName] = CedantId;
                }
            }
            var sessionCedantId = (string)Session[SessionCedantIdName];
            if (!string.IsNullOrEmpty(sessionCedantId))
            {
                CedantId = sessionCedantId;
            }

            int? cid = null;
            if (int.TryParse(CedantId, out int outId))
                cid = outId;

            DateTime? polStart = Util.GetParseDateTime(ReinsEffDatePolStartDate);
            DateTime? polEnd = Util.GetParseDateTime(ReinsEffDatePolEndDate);

            DateTime? reportingStart = Util.GetParseDateTime(ReportingStartDate);
            DateTime? reportingEnd = Util.GetParseDateTime(ReportingEndDate);

            double? maxUwRating = Util.StringToDouble(MaxUwRating);
            double? apLoading = Util.StringToDouble(ApLoading);
            double? minAar = Util.StringToDouble(MinAar);
            double? maxAar = Util.StringToDouble(MaxAar);
            double? ablAmount = Util.StringToDouble(AblAmount);
            double? retentionShare = Util.StringToDouble(RetentionShare);
            double? retentionCap = Util.StringToDouble(RetentionCap);
            double? riShare = Util.StringToDouble(RiShare);
            double? riShareCap = Util.StringToDouble(RiShareCap);
            double? serviceFee = Util.StringToDouble(ServiceFee);
            double? wakalahFee = Util.StringToDouble(WakalahFee);
            double? underwriterRatingFrom = Util.StringToDouble(UnderwriterRatingFrom);
            double? underwriterRatingTo = Util.StringToDouble(UnderwriterRatingTo);
            double? riShare2 = Util.StringToDouble(RiShare2);
            double? riShareCap2 = Util.StringToDouble(RiShareCap2);

            double? oriSumAssuredFrom = Util.StringToDouble(OriSumAssuredFrom);
            double? oriSumAssuredTo = Util.StringToDouble(OriSumAssuredTo);

            DateTime? effectiveDate = Util.GetParseDateTime(EffectiveDate);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = cid.HasValue ? CedantId : null,
                ["CedingPlanCode"] = CedingPlanCode,
                ["Description"] = Description,
                ["CedingBenefitTypeCode"] = CedingBenefitTypeCode,
                ["CedingBenefitRiskCode"] = CedingBenefitRiskCode,
                ["CedingTreatyCode"] = CedingTreatyCode,
                ["CampaignCode"] = CampaignCode,
                ["ReinsEffDatePolStartDate"] = polStart.HasValue ? ReinsEffDatePolStartDate : null,
                ["ReinsEffDatePolEndDate"] = polEnd.HasValue ? ReinsEffDatePolEndDate : null,
                ["ReinsBasisCodePickListDetailId"] = ReinsBasisCodePickListDetailId,
                ["AttainedAgeFrom"] = AttainedAgeFrom,
                ["AttainedAgeTo"] = AttainedAgeTo,
                ["ReportingStartDate"] = reportingStart.HasValue ? ReportingStartDate : null,
                ["ReportingEndDate"] = reportingEnd.HasValue ? ReportingEndDate : null,
                ["UnderwriterRatingFrom"] = underwriterRatingFrom.HasValue ? UnderwriterRatingFrom : null,
                ["UnderwriterRatingTo"] = underwriterRatingTo.HasValue ? UnderwriterRatingTo : null,
                ["OriSumAssuredFrom"] = oriSumAssuredFrom.HasValue ? OriSumAssuredFrom : null,
                ["OriSumAssuredTo"] = oriSumAssuredTo.HasValue ? OriSumAssuredTo : null,
                ["ReinsuranceIssueAgeFrom"] = ReinsuranceIssueAgeFrom,
                ["ReinsuranceIssueAgeTo"] = ReinsuranceIssueAgeTo,
                ["BenefitId"] = BenefitId,
                ["TreatyCodeId"] = TreatyCodeId,
                ["ProfitCommPickListDetailId"] = ProfitCommPickListDetailId,
                ["MaxExpiryAge"] = MaxExpiryAge,
                ["MinIssueAge"] = MinIssueAge,
                ["MaxIssueAge"] = MaxIssueAge,
                ["MaxUwRating"] = maxUwRating.HasValue ? MaxUwRating : null,
                ["ApLoading"] = apLoading.HasValue ? ApLoading : null,
                ["MinAar"] = minAar.HasValue ? MinAar : null,
                ["MaxAar"] = maxAar.HasValue ? MaxAar : null,
                ["AblAmount"] = ablAmount.HasValue ? AblAmount : null,
                ["RetentionShare"] = retentionShare.HasValue ? RetentionShare : null,
                ["RetentionCap"] = retentionCap.HasValue ? RetentionCap : null,
                ["RiShare"] = riShare.HasValue ? RiShare : null,
                ["RiShareCap"] = riShareCap.HasValue ? RiShareCap : null,
                ["ServiceFee"] = serviceFee.HasValue ? ServiceFee : null,
                ["WakalahFee"] = wakalahFee.HasValue ? WakalahFee : null,
                ["RiShare2"] = riShare2.HasValue ? RiShare2 : null,
                ["RiShareCap2"] = riShareCap2.HasValue ? RiShareCap2 : null,
                ["EffectiveDate"] = effectiveDate.HasValue ? EffectiveDate : null,
                ["SortOrder"] = SortOrder,

                ["UploadCreatedAt"] = UploadCreatedAt,
                ["UploadFileName"] = UploadFileName,
                ["UploadCreatedById"] = UploadSubmittedBy,
                ["UploadStatus"] = UploadStatus,
                ["UploadSortOrder"] = UploadSortOrder,
                ["ActiveTab"] = ActiveTab,
            };
            ViewBag.SortOrder = ActiveTab == 2 ? UploadSortOrder : SortOrder;
            ViewBag.SortReinsEffDatePolStartDate = GetSortParam("ReinsEffDatePolStartDate");
            ViewBag.SortReinsEffDatePolEndDate = GetSortParam("ReinsEffDatePolEndDate");
            ViewBag.SortAttainedAgeFrom = GetSortParam("AttainedAgeFrom");
            ViewBag.SortAttainedAgeTo = GetSortParam("AttainedAgeTo");
            ViewBag.SortReportingStartDate = GetSortParam("ReportingStartDate");
            ViewBag.SortReportingEndDate = GetSortParam("ReportingEndDate");
            ViewBag.SortUnderwriterRatingFrom = GetSortParam("UnderwriterRatingFrom");
            ViewBag.SortUnderwriterRatingTo = GetSortParam("UnderwriterRatingTo");
            ViewBag.SortOriSumAssuredFrom = GetSortParam("OriSumAssuredFrom");
            ViewBag.SortOriSumAssuredTo = GetSortParam("OriSumAssuredTo");
            ViewBag.SortReinsuranceIssueAgeFrom = GetSortParam("ReinsuranceIssueAgeFrom");
            ViewBag.SortReinsuranceIssueAgeTo = GetSortParam("ReinsuranceIssueAgeTo");
            ViewBag.SortBenefitId = GetSortParam("BenefitId");
            ViewBag.SortTreatyCodeId = GetSortParam("TreatyCodeId");
            ViewBag.SortProfitCommPickListDetailId = GetSortParam("ProfitCommPickListDetailId");
            ViewBag.SortMaxExpiryAge = GetSortParam("MaximumAgeAtExpiry");
            ViewBag.SortMinIssueAge = GetSortParam("MinimumIssueAge");
            ViewBag.SortMaxIssueAge = GetSortParam("MaximumIssueAge");
            ViewBag.SortMaxUwRating = GetSortParam("MaximumUnderwritingRating");
            ViewBag.SortApLoading = GetSortParam("ApLoading");
            ViewBag.SortMinAar = GetSortParam("MinimumAar");
            ViewBag.SortMaxAar = GetSortParam("MaximumAar");
            ViewBag.SortAblAmount = GetSortParam("AblAmount");
            ViewBag.SortRetentionShare = GetSortParam("RetentionShare");
            ViewBag.SortRetentionCap = GetSortParam("RetentionCap");
            ViewBag.SortRiShare = GetSortParam("RiShare");
            ViewBag.SortRiShareCap = GetSortParam("RiShareCap");
            ViewBag.SortRiShare2 = GetSortParam("RiShare2");
            ViewBag.SortRiShareCap2 = GetSortParam("RiShareCap2");
            ViewBag.SortServiceFee = GetSortParam("ServiceFee");
            ViewBag.SortWakalahFee = GetSortParam("WakalahFee");
            ViewBag.SortEffectiveDate = GetSortParam("EffectiveDate");

            ViewBag.UploadSortOrder = UploadSortOrder;
            ViewBag.SortUploadCreatedAt = GetSortParam("UploadCreatedAt");
            ViewBag.SortUploadFileName = GetSortParam("UploadFileName");
            ViewBag.SortUploadCreatedById = GetSortParam("UploadCreatedById");
            ViewBag.SortUploadStatus = GetSortParam("UploadStatus");

            var query = _db.TreatyBenefitCodeMappings.Select(TreatyBenefitCodeMappingViewModel.Expression());

            if (cid.HasValue) query = query.Where(q => q.CedantId == cid);
            if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
            if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingBenefitTypeCode == CedingBenefitTypeCode));
            if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingBenefitRiskCode == CedingBenefitRiskCode));
            if (!string.IsNullOrEmpty(CedingTreatyCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingTreatyCode == CedingTreatyCode));
            if (!string.IsNullOrEmpty(CampaignCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CampaignCode == CampaignCode));

            if (polStart.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= polStart);
            if (polEnd.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= polEnd);

            if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);

            if (AttainedAgeFrom.HasValue) query = query.Where(q => q.AttainedAgeFrom >= AttainedAgeFrom);
            if (AttainedAgeTo.HasValue) query = query.Where(q => q.AttainedAgeTo <= AttainedAgeTo);

            if (reportingStart.HasValue) query = query.Where(q => q.ReportingStartDate >= reportingStart);
            if (reportingEnd.HasValue) query = query.Where(q => q.ReportingEndDate <= reportingEnd);

            // Phase 2
            if (ProfitCommPickListDetailId.HasValue) query = query.Where(q => q.ProfitCommPickListDetailId == ProfitCommPickListDetailId);
            if (MaxExpiryAge.HasValue) query = query.Where(q => q.MaxExpiryAge == MaxExpiryAge);

            if (MinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge >= MinIssueAge);
            if (MaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge <= MaxIssueAge);

            if (maxUwRating.HasValue) query = query.Where(q => q.MaxUwRating == maxUwRating);
            if (apLoading.HasValue) query = query.Where(q => q.ApLoading == apLoading);

            if (minAar.HasValue) query = query.Where(q => q.MinAar >= minAar);
            if (maxAar.HasValue) query = query.Where(q => q.MaxAar <= maxAar);

            if (ablAmount.HasValue) query = query.Where(q => q.AblAmount <= ablAmount);
            if (retentionShare.HasValue) query = query.Where(q => q.RetentionShare <= retentionShare);
            if (retentionCap.HasValue) query = query.Where(q => q.RetentionCap <= retentionCap);
            if (riShare.HasValue) query = query.Where(q => q.RiShare <= riShare);
            if (riShareCap.HasValue) query = query.Where(q => q.RiShareCap <= riShareCap);
            if (serviceFee.HasValue) query = query.Where(q => q.ServiceFee <= serviceFee);
            if (wakalahFee.HasValue) query = query.Where(q => q.WakalahFee <= wakalahFee);

            if (underwriterRatingFrom.HasValue) query = query.Where(q => q.UnderwriterRatingFrom >= underwriterRatingFrom);
            if (underwriterRatingTo.HasValue) query = query.Where(q => q.UnderwriterRatingTo <= underwriterRatingTo);
            if (riShare2.HasValue) query = query.Where(q => q.RiShare2 <= riShare);
            if (riShareCap2.HasValue) query = query.Where(q => q.RiShareCap2 <= riShareCap);

            if (oriSumAssuredFrom.HasValue) query = query.Where(q => q.OriSumAssuredFrom >= oriSumAssuredFrom);
            if (oriSumAssuredTo.HasValue) query = query.Where(q => q.OriSumAssuredTo <= oriSumAssuredTo);

            if (ReinsuranceIssueAgeFrom.HasValue) query = query.Where(q => q.ReinsuranceIssueAgeFrom >= ReinsuranceIssueAgeFrom);
            if (ReinsuranceIssueAgeTo.HasValue) query = query.Where(q => q.ReinsuranceIssueAgeTo <= ReinsuranceIssueAgeTo);

            if (effectiveDate.HasValue) query = query.Where(q => q.EffectiveDate == effectiveDate);

            if (BenefitId.HasValue) query = query.Where(q => q.BenefitId == BenefitId);
            if (!string.IsNullOrEmpty(TreatyCodeId))
            {
                string[] TreatyCodeIds = TreatyCodeId.Split(',');
                query = query.Where(q => TreatyCodeIds.Contains(q.TreatyCodeId.ToString()));
            }

            if (SortOrder == Html.GetSortAsc("ReinsEffDatePolStartDate")) query = query.OrderBy(q => q.ReinsEffDatePolStartDate);
            else if (SortOrder == Html.GetSortDsc("ReinsEffDatePolStartDate")) query = query.OrderByDescending(q => q.ReinsEffDatePolStartDate);
            else if (SortOrder == Html.GetSortAsc("ReinsEffDatePolEndDate")) query = query.OrderBy(q => q.ReinsEffDatePolEndDate);
            else if (SortOrder == Html.GetSortDsc("ReinsEffDatePolEndDate")) query = query.OrderByDescending(q => q.ReinsEffDatePolEndDate);
            else if (SortOrder == Html.GetSortAsc("AttainedAgeFrom")) query = query.OrderBy(q => q.AttainedAgeFrom);
            else if (SortOrder == Html.GetSortDsc("AttainedAgeFrom")) query = query.OrderByDescending(q => q.AttainedAgeFrom);
            else if (SortOrder == Html.GetSortAsc("AttainedAgeTo")) query = query.OrderBy(q => q.AttainedAgeTo);
            else if (SortOrder == Html.GetSortDsc("AttainedAgeTo")) query = query.OrderByDescending(q => q.AttainedAgeTo);
            else if (SortOrder == Html.GetSortAsc("ReportingStartDate")) query = query.OrderBy(q => q.ReportingStartDate);
            else if (SortOrder == Html.GetSortDsc("ReportingStartDate")) query = query.OrderByDescending(q => q.ReportingStartDate);
            else if (SortOrder == Html.GetSortAsc("ReportingEndDate")) query = query.OrderBy(q => q.ReportingEndDate);
            else if (SortOrder == Html.GetSortDsc("ReportingEndDate")) query = query.OrderByDescending(q => q.ReportingEndDate);

            else if (SortOrder == Html.GetSortAsc("UnderwriterRatingFrom")) query = query.OrderBy(q => q.UnderwriterRatingFrom);
            else if (SortOrder == Html.GetSortDsc("UnderwriterRatingFrom")) query = query.OrderByDescending(q => q.UnderwriterRatingFrom);
            else if (SortOrder == Html.GetSortAsc("UnderwriterRatingTo")) query = query.OrderBy(q => q.UnderwriterRatingTo);
            else if (SortOrder == Html.GetSortDsc("UnderwriterRatingTo")) query = query.OrderByDescending(q => q.UnderwriterRatingTo);

            else if (SortOrder == Html.GetSortAsc("OriSumAssuredFrom")) query = query.OrderBy(q => q.OriSumAssuredFrom);
            else if (SortOrder == Html.GetSortDsc("OriSumAssuredFrom")) query = query.OrderByDescending(q => q.OriSumAssuredFrom);
            else if (SortOrder == Html.GetSortAsc("OriSumAssuredTo")) query = query.OrderBy(q => q.OriSumAssuredTo);
            else if (SortOrder == Html.GetSortDsc("OriSumAssuredTo")) query = query.OrderByDescending(q => q.OriSumAssuredTo);

            else if (SortOrder == Html.GetSortAsc("ReinsuranceIssueAgeFrom")) query = query.OrderBy(q => q.ReinsuranceIssueAgeFrom);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceIssueAgeFrom")) query = query.OrderByDescending(q => q.ReinsuranceIssueAgeFrom);
            else if (SortOrder == Html.GetSortAsc("ReinsuranceIssueAgeTo")) query = query.OrderBy(q => q.ReinsuranceIssueAgeTo);
            else if (SortOrder == Html.GetSortDsc("ReinsuranceIssueAgeTo")) query = query.OrderByDescending(q => q.ReinsuranceIssueAgeTo);

            else if (SortOrder == Html.GetSortAsc("BenefitId")) query = query.OrderBy(q => q.Benefit.Code);
            else if (SortOrder == Html.GetSortDsc("BenefitId")) query = query.OrderByDescending(q => q.Benefit.Code);
            else if (SortOrder == Html.GetSortAsc("TreatyCodeId")) query = query.OrderBy(q => q.TreatyCode.Code);
            else if (SortOrder == Html.GetSortDsc("TreatyCodeId")) query = query.OrderByDescending(q => q.TreatyCode.Code);

            else if (SortOrder == Html.GetSortAsc("ProfitCommPickListDetailId")) query = query.OrderBy(q => q.ProfitCommPickListDetail.Code);
            else if (SortOrder == Html.GetSortDsc("ProfitCommPickListDetailId")) query = query.OrderByDescending(q => q.ProfitCommPickListDetail.Code);
            else if (SortOrder == Html.GetSortAsc("MaxExpiryAge")) query = query.OrderBy(q => q.MaxExpiryAge);
            else if (SortOrder == Html.GetSortDsc("MaxExpiryAge")) query = query.OrderByDescending(q => q.MaxExpiryAge);
            else if (SortOrder == Html.GetSortAsc("MinIssueAge")) query = query.OrderBy(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortDsc("MinIssueAge")) query = query.OrderByDescending(q => q.MinIssueAge);
            else if (SortOrder == Html.GetSortAsc("MaxIssueAge")) query = query.OrderBy(q => q.MaxIssueAge);
            else if (SortOrder == Html.GetSortDsc("MaxIssueAge")) query = query.OrderByDescending(q => q.MaxIssueAge);

            else if (SortOrder == Html.GetSortAsc("MaxUwRating")) query = query.OrderBy(q => q.MaxUwRating);
            else if (SortOrder == Html.GetSortDsc("MaxUwRating")) query = query.OrderByDescending(q => q.MaxUwRating);
            else if (SortOrder == Html.GetSortAsc("ApLoading")) query = query.OrderBy(q => q.ApLoading);
            else if (SortOrder == Html.GetSortDsc("ApLoading")) query = query.OrderByDescending(q => q.ApLoading);
            else if (SortOrder == Html.GetSortAsc("MinAar")) query = query.OrderBy(q => q.MinAar);
            else if (SortOrder == Html.GetSortDsc("MinAar")) query = query.OrderByDescending(q => q.MinAar);
            else if (SortOrder == Html.GetSortAsc("MaxAar")) query = query.OrderBy(q => q.MaxAar);
            else if (SortOrder == Html.GetSortDsc("MaxAar")) query = query.OrderByDescending(q => q.MaxAar);
            else if (SortOrder == Html.GetSortAsc("AblAmount")) query = query.OrderBy(q => q.AblAmount);
            else if (SortOrder == Html.GetSortDsc("AblAmount")) query = query.OrderByDescending(q => q.AblAmount);
            else if (SortOrder == Html.GetSortAsc("RetentionShare")) query = query.OrderBy(q => q.RetentionShare);
            else if (SortOrder == Html.GetSortDsc("RetentionShare")) query = query.OrderByDescending(q => q.RetentionShare);
            else if (SortOrder == Html.GetSortAsc("RetentionCap")) query = query.OrderBy(q => q.RetentionCap);
            else if (SortOrder == Html.GetSortDsc("RetentionCap")) query = query.OrderByDescending(q => q.RetentionCap);
            else if (SortOrder == Html.GetSortAsc("RiShare")) query = query.OrderBy(q => q.RiShare);
            else if (SortOrder == Html.GetSortDsc("RiShare")) query = query.OrderByDescending(q => q.RiShare);
            else if (SortOrder == Html.GetSortAsc("RiShareCap")) query = query.OrderBy(q => q.RiShareCap);
            else if (SortOrder == Html.GetSortDsc("RiShareCap")) query = query.OrderByDescending(q => q.RiShareCap);
            else if (SortOrder == Html.GetSortAsc("ServiceFee")) query = query.OrderBy(q => q.ServiceFee);
            else if (SortOrder == Html.GetSortDsc("ServiceFee")) query = query.OrderByDescending(q => q.ServiceFee);
            else if (SortOrder == Html.GetSortAsc("WakalahFee")) query = query.OrderBy(q => q.WakalahFee);
            else if (SortOrder == Html.GetSortDsc("WakalahFee")) query = query.OrderByDescending(q => q.WakalahFee);

            else if (SortOrder == Html.GetSortAsc("RiShare2")) query = query.OrderBy(q => q.RiShare2);
            else if (SortOrder == Html.GetSortDsc("RiShare2")) query = query.OrderByDescending(q => q.RiShare2);
            else if (SortOrder == Html.GetSortAsc("RiShareCap2")) query = query.OrderBy(q => q.RiShareCap2);
            else if (SortOrder == Html.GetSortDsc("RiShareCap2")) query = query.OrderByDescending(q => q.RiShareCap2);

            else if (SortOrder == Html.GetSortAsc("EffectiveDate")) query = query.OrderBy(q => q.EffectiveDate);
            else if (SortOrder == Html.GetSortDsc("EffectiveDate")) query = query.OrderByDescending(q => q.EffectiveDate);

            else query = query.OrderByDescending(q => q.Id);


            ViewBag.ListingTotal = query.Count();
            ViewBag.ListingList = query.ToPagedList(Page ?? 1, PageSize);
            IndexPage(cid);

            DropDownUploadStatus();
            DropDownUser(UserBo.StatusActive);

            ViewBag.ActiveTab = ActiveTab;

            //Upload tab
            GetUploadData(UploadPage, UploadCreatedAt, UploadFileName, UploadSubmittedBy, UploadStatus, UploadSortOrder);

            return View();
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

            var query = _db.TreatyBenefitCodeMappingUpload.Select(TreatyBenefitCodeMappingUploadViewModel.Expression());
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

        public List<SelectListItem> DropDownUploadStatus()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyBenefitCodeMappingUploadBo.StatusMax))
            {
                items.Add(new SelectListItem { Text = TreatyBenefitCodeMappingUploadBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.UploadStatusItems = items;
            return items;
        }

        // GET: TreatyBenefitCodeMapping/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            var model = new TreatyBenefitCodeMappingViewModel();
            var sessionCedantId = (string)Session[SessionCedantIdName];
            if (!string.IsNullOrEmpty(sessionCedantId))
            {
                if (int.TryParse(sessionCedantId, out int cid))
                    model.CedantId = cid;
            }
            LoadPage();
            return View(model);
        }

        // POST: TreatyBenefitCodeMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(TreatyBenefitCodeMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);
                var trail = GetNewTrailObject();
                Result = TreatyBenefitCodeMappingService.Result();
                var mappingResult = TreatyBenefitCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    // Validate Success
                    Result = TreatyBenefitCodeMappingService.Create(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        model.Id = bo.Id;
                        TreatyBenefitCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            bo.Id,
                            "Create Product Feature Mapping"
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

        // GET: TreatyBenefitCodeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = TreatyBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            LoadPage(bo);
            return View(new TreatyBenefitCodeMappingViewModel(bo));
        }

        // POST: TreatyBenefitCodeMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, TreatyBenefitCodeMappingViewModel model)
        {
            var dbBo = TreatyBenefitCodeMappingService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = id;

                var trail = GetNewTrailObject();
                Result = TreatyBenefitCodeMappingService.Result();
                var mappingResult = TreatyBenefitCodeMappingService.ValidateMapping(bo);
                if (!mappingResult.Valid)
                {
                    Result.AddErrorRange(mappingResult.ToErrorArray());
                }
                else
                {
                    Result = TreatyBenefitCodeMappingService.Update(ref bo, ref trail);
                    if (Result.Valid)
                    {
                        TreatyBenefitCodeMappingService.ProcessMappingDetail(bo, AuthUserId); // DO NOT TRAIL
                        CreateTrail(
                            dbBo.Id,
                            "Update Product Feature Mapping"
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

        // GET: TreatyBenefitCodeMapping/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = TreatyBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new TreatyBenefitCodeMappingViewModel(bo));
        }

        // POST: TreatyBenefitCodeMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id)
        {
            var bo = TreatyBenefitCodeMappingService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = TreatyBenefitCodeMappingService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Product Feature Mapping"
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
                    TreatyBenefitCodeMappingUploadBo treatyBenefitCodeMappingUploadBo = new TreatyBenefitCodeMappingUploadBo
                    {
                        Status = TreatyBenefitCodeMappingUploadBo.StatusPendingProcess,
                        FileName = upload.FileName,
                        CreatedById = AuthUserId,
                        UpdatedById = AuthUserId,
                    };

                    treatyBenefitCodeMappingUploadBo.FormatHashFileName();

                    string path = treatyBenefitCodeMappingUploadBo.GetLocalPath();
                    Util.MakeDir(path);
                    upload.SaveAs(path);

                    TreatyBenefitCodeMappingUploadService.Create(ref treatyBenefitCodeMappingUploadBo, ref trail);

                    SetSuccessSessionMsg("File uploaded and pending processing.");
                    //var process = new ProcessProductFeatureMapping()
                    //{
                    //    PostedFile = upload,
                    //    AuthUserId = AuthUserId,
                    //};
                    //process.Process();

                    //if (process.Errors.Count() > 0 && process.Errors != null)
                    //{
                    //    SetErrorSessionMsgArr(process.Errors.Take(50).ToList());
                    //}

                    //int create = process.GetProcessCount("Create");
                    //int update = process.GetProcessCount("Update");
                    //int delete = process.GetProcessCount("Delete");

                    //SetSuccessSessionMsg(string.Format("Process File Successful, Total Create: {0}, Total Update: {1}, Total Delete: {2}", create, update, delete));
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(
            string downloadToken,
            int type,
            int? CedantId,
            string CedingPlanCode,
            string Description,
            string CedingBenefitTypeCode,
            string CedingBenefitRiskCode,
            string CedingTreatyCode,
            string CampaignCode,
            string ReinsEffDatePolStartDate,
            string ReinsEffDatePolEndDate,
            int? ReinsBasisCodePickListDetailId,
            int? AttainedAgeFrom,
            int? AttainedAgeTo,
            string ReportingStartDate,
            string ReportingEndDate,
            string UnderwriterRatingFrom,
            string UnderwriterRatingTo,
            string OriSumAssuredFrom,
            string OriSumAssuredTo,
            int? ReinsuranceIssueAgeFrom,
            int? ReinsuranceIssueAgeTo,
            int? BenefitId,
            string TreatyCodeId,
            int? ProfitCommPickListDetailId,
            int? MaxExpiryAge,
            int? MinIssueAge,
            int? MaxIssueAge,
            string MaxUwRating,
            string ApLoading,
            string MinAar,
            string MaxAar,
            string AblAmount,
            string RetentionShare,
            string RetentionCap,
            string RiShare,
            string RiShareCap,
            string RiShare2,
            string RiShareCap2,
            string ServiceFee,
            string WakalahFee,
            string EffectiveDate
        )
        {
            // type 1 = all
            // type 2 = filtered download
            // type 3 = template download

            Response.SetCookie(new HttpCookie("downloadToken", downloadToken));
            Session["lastActivity"] = DateTime.Now.Ticks;

            _db.Database.CommandTimeout = 0;
            var query = _db.TreatyBenefitCodeMappings.Select(TreatyBenefitCodeMappingViewModel.Expression());
            if (type == 2) // filtered dowload
            {
                DateTime? polStart = Util.GetParseDateTime(ReinsEffDatePolStartDate);
                DateTime? polEnd = Util.GetParseDateTime(ReinsEffDatePolEndDate);

                DateTime? reportingStart = Util.GetParseDateTime(ReportingStartDate);
                DateTime? reportingEnd = Util.GetParseDateTime(ReportingEndDate);

                double? maxUwRating = Util.StringToDouble(MaxUwRating);
                double? apLoading = Util.StringToDouble(ApLoading);
                double? minAar = Util.StringToDouble(MinAar);
                double? maxAar = Util.StringToDouble(MaxAar);
                double? ablAmount = Util.StringToDouble(AblAmount);
                double? retentionShare = Util.StringToDouble(RetentionShare);
                double? retentionCap = Util.StringToDouble(RetentionCap);
                double? riShare = Util.StringToDouble(RiShare);
                double? riShareCap = Util.StringToDouble(RiShareCap);
                double? serviceFee = Util.StringToDouble(ServiceFee);
                double? wakalahFee = Util.StringToDouble(WakalahFee);
                double? underwriterRatingFrom = Util.StringToDouble(UnderwriterRatingFrom);
                double? underwriterRatingTo = Util.StringToDouble(UnderwriterRatingTo);
                double? riShare2 = Util.StringToDouble(RiShare2);
                double? riShareCap2 = Util.StringToDouble(RiShareCap2);
                double? oriSumAssuredFrom = Util.StringToDouble(OriSumAssuredFrom);
                double? oriSumAssuredTo = Util.StringToDouble(OriSumAssuredTo);

                DateTime? effectiveDate = Util.GetParseDateTime(EffectiveDate);

                if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);
                if (!string.IsNullOrEmpty(CedingPlanCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingPlanCode == CedingPlanCode));
                if (!string.IsNullOrEmpty(Description)) query = query.Where(q => q.Description.Contains(Description));
                if (!string.IsNullOrEmpty(CedingBenefitTypeCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingBenefitTypeCode == CedingBenefitTypeCode));
                if (!string.IsNullOrEmpty(CedingBenefitRiskCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingBenefitRiskCode == CedingBenefitRiskCode));
                if (!string.IsNullOrEmpty(CedingTreatyCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CedingTreatyCode == CedingTreatyCode));
                if (!string.IsNullOrEmpty(CampaignCode)) query = query.Where(q => q.TreatyBenefitCodeMappingDetails.Any(d => d.CampaignCode == CampaignCode));

                if (polStart.HasValue) query = query.Where(q => q.ReinsEffDatePolStartDate >= polStart);
                if (polEnd.HasValue) query = query.Where(q => q.ReinsEffDatePolEndDate <= polEnd);

                if (ReinsBasisCodePickListDetailId.HasValue) query = query.Where(q => q.ReinsBasisCodePickListDetailId == ReinsBasisCodePickListDetailId);

                if (AttainedAgeFrom.HasValue) query = query.Where(q => q.AttainedAgeFrom >= AttainedAgeFrom);
                if (AttainedAgeTo.HasValue) query = query.Where(q => q.AttainedAgeTo <= AttainedAgeTo);

                if (reportingStart.HasValue) query = query.Where(q => q.ReportingStartDate >= reportingStart);
                if (reportingEnd.HasValue) query = query.Where(q => q.ReportingEndDate <= reportingEnd);

                // Phase 2
                if (ProfitCommPickListDetailId.HasValue) query = query.Where(q => q.ProfitCommPickListDetailId == ProfitCommPickListDetailId);
                if (MaxExpiryAge.HasValue) query = query.Where(q => q.MaxExpiryAge == MaxExpiryAge);

                if (MinIssueAge.HasValue) query = query.Where(q => q.MinIssueAge >= MinIssueAge);
                if (MaxIssueAge.HasValue) query = query.Where(q => q.MaxIssueAge <= MaxIssueAge);

                if (maxUwRating.HasValue) query = query.Where(q => q.MaxUwRating == maxUwRating);
                if (apLoading.HasValue) query = query.Where(q => q.ApLoading == apLoading);

                if (minAar.HasValue) query = query.Where(q => q.MinAar >= minAar);
                if (maxAar.HasValue) query = query.Where(q => q.MaxAar <= maxAar);

                if (ablAmount.HasValue) query = query.Where(q => q.AblAmount <= ablAmount);
                if (retentionShare.HasValue) query = query.Where(q => q.RetentionShare <= retentionShare);
                if (retentionCap.HasValue) query = query.Where(q => q.RetentionCap <= retentionCap);
                if (riShare.HasValue) query = query.Where(q => q.RiShare <= riShare);
                if (riShareCap.HasValue) query = query.Where(q => q.RiShareCap <= riShareCap);
                if (serviceFee.HasValue) query = query.Where(q => q.ServiceFee <= serviceFee);
                if (wakalahFee.HasValue) query = query.Where(q => q.WakalahFee <= wakalahFee);

                if (underwriterRatingFrom.HasValue) query = query.Where(q => q.UnderwriterRatingFrom >= underwriterRatingFrom);
                if (underwriterRatingTo.HasValue) query = query.Where(q => q.UnderwriterRatingTo <= underwriterRatingTo);
                if (riShare2.HasValue) query = query.Where(q => q.RiShare2 <= riShare);
                if (riShareCap2.HasValue) query = query.Where(q => q.RiShareCap2 <= riShareCap);

                if (oriSumAssuredFrom.HasValue) query = query.Where(q => q.OriSumAssuredFrom >= oriSumAssuredFrom);
                if (oriSumAssuredTo.HasValue) query = query.Where(q => q.OriSumAssuredTo <= oriSumAssuredTo);

                if (ReinsuranceIssueAgeFrom.HasValue) query = query.Where(q => q.ReinsuranceIssueAgeFrom >= ReinsuranceIssueAgeFrom);
                if (ReinsuranceIssueAgeTo.HasValue) query = query.Where(q => q.ReinsuranceIssueAgeTo <= ReinsuranceIssueAgeTo);

                if (effectiveDate.HasValue) query = query.Where(q => q.EffectiveDate == effectiveDate);

                if (BenefitId.HasValue) query = query.Where(q => q.BenefitId == BenefitId);
                if (!string.IsNullOrEmpty(TreatyCodeId))
                {
                    string[] TreatyCodeIds = TreatyCodeId.Split(',');
                    query = query.Where(q => TreatyCodeIds.Contains(q.TreatyCodeId.ToString()));
                }

            }
            if (type == 3)
            {
                query = null;
            }

            var export = new ExportProductFeatureMapping();
            export.HandleTempDirectory();

            if (query != null)
                export.SetQuery(query.Select(x => new TreatyBenefitCodeMappingBo
                {
                    Id = x.Id,
                    CedantId = x.CedantId,
                    CedantCode = x.Cedant.Code,
                    BenefitId = x.BenefitId,
                    BenefitCode = x.Benefit.Code,
                    TreatyCodeId = x.TreatyCodeId,
                    TreatyCode = x.TreatyCode.Code,
                    CedingPlanCode = x.CedingPlanCode,
                    Description = x.Description,
                    CedingBenefitTypeCode = x.CedingBenefitTypeCode,
                    CedingBenefitRiskCode = x.CedingBenefitRiskCode,
                    CedingTreatyCode = x.CedingTreatyCode,
                    CampaignCode = x.CampaignCode,
                    ReinsEffDatePolStartDate = x.ReinsEffDatePolStartDate,
                    ReinsEffDatePolEndDate = x.ReinsEffDatePolEndDate,
                    ReinsBasisCodePickListDetailId = x.ReinsBasisCodePickListDetailId,
                    ReinsBasisCode = x.ReinsBasisCodePickListDetail.Code,
                    AttainedAgeFrom = x.AttainedAgeFrom,
                    AttainedAgeTo = x.AttainedAgeTo,
                    ReportingStartDate = x.ReportingStartDate,
                    ReportingEndDate = x.ReportingEndDate,
                    ProfitCommPickListDetailId = x.ProfitCommPickListDetailId,
                    ProfitComm = x.ProfitCommPickListDetail.Code,
                    MaxExpiryAge = x.MaxExpiryAge,
                    MinIssueAge = x.MinIssueAge,
                    MaxIssueAge = x.MaxIssueAge,
                    MaxUwRating = x.MaxUwRating,
                    ApLoading = x.ApLoading,
                    MinAar = x.MinAar,
                    MaxAar = x.MaxAar,
                    AblAmount = x.AblAmount,
                    RetentionShare = x.RetentionShare,
                    RetentionCap = x.RetentionCap,
                    RiShare = x.RiShare,
                    RiShareCap2 = x.RiShareCap2,
                    ServiceFee = x.ServiceFee,
                    WakalahFee = x.WakalahFee,
                    UnderwriterRatingFrom = x.UnderwriterRatingFrom,
                    UnderwriterRatingTo = x.UnderwriterRatingTo,
                    RiShareCap = x.RiShareCap,
                    RiShare2 = x.RiShare2,
                    OriSumAssuredFrom = x.OriSumAssuredFrom,
                    OriSumAssuredTo = x.OriSumAssuredTo,
                    EffectiveDate = x.EffectiveDate,
                    ReinsuranceIssueAgeFrom = x.ReinsuranceIssueAgeFrom,
                    ReinsuranceIssueAgeTo = x.ReinsuranceIssueAgeTo,
                }));

            export.Process();

            return File(export.FilePath, "text/csv", Path.GetFileName(export.FilePath));
        }

        public void IndexPage(int? cedantId = null)
        {
            DropDownEmpty();
            DropDownCedant(selectedId: cedantId, emptyValue: ClearSessionValue);
            DropDownReinsBasisCode();
            DropDownBenefit();
            //DropDownYN();
            DropDownProfitComm();
            //DropDownTreatyCode();
            SetViewBagMessage();
        }

        public void LoadPage(TreatyBenefitCodeMappingBo treatyBenefitCodeMappingBo = null)
        {
            DropDownEmpty();
            DropDownReinsBasisCode();
            GetCedingBenefitTypeCode();
            //DropDownYN();
            DropDownProfitComm();

            if (treatyBenefitCodeMappingBo == null)
            {
                // Create
                var sessionCedantId = (string)Session[SessionCedantIdName];
                if (int.TryParse(sessionCedantId, out int cid))
                    DropDownCedant(CedantBo.StatusActive, cid);
                else
                    DropDownCedant(CedantBo.StatusActive);
                DropDownBenefit(BenefitBo.StatusActive);
                DropDownTreatyCode(TreatyCodeBo.StatusActive);
            }
            else
            {
                // Edit
                DropDownCedant(CedantBo.StatusActive, treatyBenefitCodeMappingBo.CedantId);
                DropDownBenefit(BenefitBo.StatusActive, treatyBenefitCodeMappingBo.BenefitId);
                DropDownTreatyCode(TreatyCodeBo.StatusActive, treatyBenefitCodeMappingBo.TreatyCodeId);

                if (treatyBenefitCodeMappingBo.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
                if (treatyBenefitCodeMappingBo.BenefitBo.Status == BenefitBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.BenefitStatusInactive);
                }
                if (treatyBenefitCodeMappingBo.TreatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.TreatyCodeStatusInactive);
                }
            }
            SetViewBagMessage();
        }

        public void DownloadError(int id)
        {
            try
            {
                var tbcmuBo = TreatyBenefitCodeMappingUploadService.Find(id);
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine(tbcmuBo.Errors.Replace(",", Environment.NewLine));
                tw.Flush();
                byte[] bytes = ms.ToArray();
                ms.Close();

                Response.ClearContent();
                Response.Clear();
                //Response.Buffer = true;                                                                                                                   
                //Response.BufferOutput = false;
                Response.ContentType = MimeMapping.GetMimeMapping("ProductFeatureMappingUploadError");
                Response.AddHeader("Content-Disposition", "attachment; filename=ProductFeatureMappingUploadError.txt");
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
