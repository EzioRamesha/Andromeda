using BusinessObject.TreatyPricing;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Auth]
    public class TreatyPricingTreatyDashboardController : BaseController
    {
        public const string Controller = "TreatyPricingTreatyDashboard";

        // GET: TreatyPricingTreatyDashboard
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index()
        {
            LoadIndex();
            return View();
        }

        public void LoadIndex()
        {
            var bos = TreatyPricingTreatyWorkflowVersionService.GetLatestVersionOfTreatyWorkflow();
            GetDraftCalendar(bos);
            GetDraftingStatusOverviews(bos);
            GetDraftingStatusOverviewByPic(bos);
            GetDraftingStatusOverviewByReviewer(bos);
            GetDraftingStatusOverviewByPendingHod(bos);
            GetDraftingStatusOverviewByPendingDepartment(bos);
            GetDraftingStatusOverviewWithin2WeeksFromTargetSentDate(bos);
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftCalendar(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            bos = bos.Where(q => q.TargetSentDate.HasValue);

            var publicHoliday = PublicHolidayDetailService.Get();
            var outputCalendar = new List<TreatyPricingTreatyWorkflowBo>();
            foreach (var i in bos)
            {
                var output = new TreatyPricingTreatyWorkflowBo();
                output.Title = i.TreatyPricingTreatyWorkflowBo.DocumentId;
                output.Start = i.TargetSentDate.Value.ToString("yyyy-MM-dd");
                outputCalendar.Add(output);
            }
            foreach (var i in publicHoliday)
            {
                var output = new TreatyPricingTreatyWorkflowBo();
                output.Title = "Public Holiday: " + i.Description;
                output.Start = i.PublicHolidayDate.ToString("yyyy-MM-dd");
                outputCalendar.Add(output);
            }

            ViewBag.DraftCalendar = outputCalendar;

            return outputCalendar;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviews(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            var statuses = new List<int>();
            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();

            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryUnassigned))
                statuses.Add(i);

            foreach (var status in statuses)
            {
                var bo = new TreatyPricingTreatyWorkflowBo();
                bo.DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(status);
                bo.DraftingStatusCategory = status;

                var draftingBos = bos.Where(a => a.TreatyPricingTreatyWorkflowBo.DraftingStatusCategory == status);
                bo.AllCount = draftingBos.Count();
                bo.LessThan6MonthCount = draftingBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months").Count();
                bo.LessThan12MonthCount = draftingBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months").Count();
                bo.MoreThan12MonthCount = draftingBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months").Count();

                draftOverview.Add(bo);
            }

            ViewBag.DraftingStatusOverviews = draftOverview;

            return draftOverview;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviewByPic(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            var distinctPic = bos.Select(a => a.PersonInChargeId).Distinct().ToList();

            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();
            int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

            foreach (var pic in distinctPic)
            {
                if (pic != null && pic != 0)
                {
                    var bo = new TreatyPricingTreatyWorkflowBo();
                    bo.PersonInChargeName = UserService.Find(pic).FullName;
                    bo.PersonInChargeId = pic;

                    var picBos = bos.Where(a => a.PersonInChargeId == pic);
                    bo.LessThan6MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan12MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.MoreThan12MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan6MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan12MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.MoreThan12MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan6MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.LessThan12MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.MoreThan12MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.TotalCountPic = bo.LessThan6MonthCountInTreaty + bo.LessThan12MonthCountInTreaty + bo.MoreThan12MonthCountInTreaty
                        + bo.LessThan6MonthCountInAddendum + bo.LessThan12MonthCountInAddendum + bo.MoreThan12MonthCountInAddendum
                        + bo.LessThan6MonthCountInOther + bo.LessThan12MonthCountInOther + bo.MoreThan12MonthCountInOther;

                    draftOverview.Add(bo);
                }
            }

            var total = new TreatyPricingTreatyWorkflowBo();
            total.TotalLessThan6MonthsCountInTreaty = draftOverview.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
            total.TotalLessThan12MonthsCountInTreaty = draftOverview.Select(a => a.LessThan12MonthCountInTreaty).ToList().Sum();
            total.TotalMoreThan12MonthsCountInTreaty = draftOverview.Select(a => a.MoreThan12MonthCountInTreaty).ToList().Sum();
            total.TotalLessThan6MonthsCountInAddendum = draftOverview.Select(a => a.LessThan6MonthCountInAddendum).ToList().Sum();
            total.TotalLessThan12MonthsCountInAddendum = draftOverview.Select(a => a.LessThan12MonthCountInAddendum).ToList().Sum();
            total.TotalMoreThan12MonthsCountInAddendum = draftOverview.Select(a => a.MoreThan12MonthCountInAddendum).ToList().Sum();
            total.TotalLessThan6MonthsCountInOther = draftOverview.Select(a => a.LessThan6MonthCountInOther).ToList().Sum();
            total.TotalLessThan12MonthsCountInOther = draftOverview.Select(a => a.LessThan12MonthCountInOther).ToList().Sum();
            total.TotalMoreThan12MonthsCountInOther = draftOverview.Select(a => a.MoreThan12MonthCountInOther).ToList().Sum();
            total.TotalCountForAll = draftOverview.Select(a => a.TotalCountPic).ToList().Sum();

            ViewBag.DraftingStatusOverviewByPic = draftOverview;
            ViewBag.TotalDraftingStatusOverviewByPic = total;
            return draftOverview;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviewByReviewer(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            bos = bos.Where(q => q.TreatyPricingTreatyWorkflowBo.DraftingStatusCategory == TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryInternalReview);

            var allReviewer = bos.Where(a => !string.IsNullOrEmpty(a.TreatyPricingTreatyWorkflowBo.Reviewer)).Select(a => a.TreatyPricingTreatyWorkflowBo.Reviewer).ToList();

            var allReviewers = new List<string>();
            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();
            int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

            var total = new TreatyPricingTreatyWorkflowBo();
            if (allReviewer.Count > 0)
            {
                foreach (var reviewers in allReviewer)
                {
                    foreach (var reviewer in reviewers.Split(','))
                    {
                        allReviewers.Add(reviewer.Trim());
                    }
                }

                var distinctReviewer = allReviewers.Distinct();

                foreach (var reviewer in distinctReviewer)
                {
                    var reviewerId = int.Parse(reviewer.Trim());
                    var bo = new TreatyPricingTreatyWorkflowBo();
                    if (reviewerId != 0)
                    {
                        bo.PersonInChargeName = UserService.Find(reviewerId).FullName;
                        bo.Reviewer = reviewerId.ToString();
                    }

                    var reviewerBos = bos.Where(a => !string.IsNullOrEmpty(a.TreatyPricingTreatyWorkflowBo.Reviewer) && a.TreatyPricingTreatyWorkflowBo.Reviewer.Split(',').Select(r => r.Trim()).ToArray().Contains(reviewer));
                    bo.LessThan6MonthCountInTreaty = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan12MonthCountInTreaty = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.MoreThan12MonthCountInTreaty = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan6MonthCountInAddendum = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan12MonthCountInAddendum = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.MoreThan12MonthCountInAddendum = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan6MonthCountInOther = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.LessThan12MonthCountInOther = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.MoreThan12MonthCountInOther = reviewerBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.TotalCountPic = bo.LessThan6MonthCountInTreaty + bo.LessThan12MonthCountInTreaty + bo.MoreThan12MonthCountInTreaty
                        + bo.LessThan6MonthCountInAddendum + bo.LessThan12MonthCountInAddendum + bo.MoreThan12MonthCountInAddendum
                        + bo.LessThan6MonthCountInOther + bo.LessThan12MonthCountInOther + bo.MoreThan12MonthCountInOther;

                    draftOverview.Add(bo);
                }

                total.TotalLessThan6MonthsCountInTreaty = draftOverview.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
                total.TotalLessThan12MonthsCountInTreaty = draftOverview.Select(a => a.LessThan12MonthCountInTreaty).ToList().Sum();
                total.TotalMoreThan12MonthsCountInTreaty = draftOverview.Select(a => a.MoreThan12MonthCountInTreaty).ToList().Sum();
                total.TotalLessThan6MonthsCountInAddendum = draftOverview.Select(a => a.LessThan6MonthCountInAddendum).ToList().Sum();
                total.TotalLessThan12MonthsCountInAddendum = draftOverview.Select(a => a.LessThan12MonthCountInAddendum).ToList().Sum();
                total.TotalMoreThan12MonthsCountInAddendum = draftOverview.Select(a => a.MoreThan12MonthCountInAddendum).ToList().Sum();
                total.TotalLessThan6MonthsCountInOther = draftOverview.Select(a => a.LessThan6MonthCountInOther).ToList().Sum();
                total.TotalLessThan12MonthsCountInOther = draftOverview.Select(a => a.LessThan12MonthCountInOther).ToList().Sum();
                total.TotalMoreThan12MonthsCountInOther = draftOverview.Select(a => a.MoreThan12MonthCountInOther).ToList().Sum();
                total.TotalCountForAll = draftOverview.Select(a => a.TotalCountPic).ToList().Sum();

            }

            ViewBag.DraftingStatusOverviewByPendingReviewer = draftOverview;
            ViewBag.TotalDraftingStatusOverviewByPendingReviewer = total;
            return draftOverview;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviewByPendingHod(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            var pendingHodBos = bos.Where(q => q.TreatyPricingTreatyWorkflowBo.DraftingStatusCategory == TreatyPricingTreatyWorkflowBo.DraftingStatusPendingHODReview);

            var distinctPic = pendingHodBos.Select(a => a.PersonInChargeId).Distinct().ToList();

            int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };
            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();
            var total = 0;

            foreach (var pic in distinctPic)
            {
                var bo = new TreatyPricingTreatyWorkflowBo();
                if (pic != null)
                {
                    bo.PersonInChargeName = UserService.Find(pic).FullName;
                    bo.PersonInChargeId = pic;

                    var picBos = bos.Where(a => a.PersonInChargeId == pic);
                    bo.LessThan6MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan12MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.MoreThan12MonthCountInTreaty = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan6MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan12MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.MoreThan12MonthCountInAddendum = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan6MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.LessThan12MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.MoreThan12MonthCountInOther = picBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.TotalCountPic = bo.LessThan6MonthCountInTreaty + bo.LessThan12MonthCountInTreaty + bo.MoreThan12MonthCountInTreaty
                            + bo.LessThan6MonthCountInAddendum + bo.LessThan12MonthCountInAddendum + bo.MoreThan12MonthCountInAddendum
                            + bo.LessThan6MonthCountInOther + bo.LessThan12MonthCountInOther + bo.MoreThan12MonthCountInOther;

                    if (bo.TotalCountPic > 0)
                    {
                        draftOverview.Add(bo);
                    }
                }

            }

            total = draftOverview.Select(a => a.TotalCountPic).ToList().Sum();

            ViewBag.DraftingStatusOverviewByPendingHod = draftOverview;
            ViewBag.TotalDraftingStatusOverviewByPendingHod = total;
            return draftOverview;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviewByPendingDepartment(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            string[] departments = new string[] { "Claims", "Underwriting", "Health", "RGA", "Compliance & Risk", "Business Development" };
            int[] treatyAndAddendum = { TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty, TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum };

            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();
            var total = new TreatyPricingTreatyWorkflowBo();

            foreach (var department in departments)
            {
                var bo = new TreatyPricingTreatyWorkflowBo();
                var dep = 0;
                bo.Department = department;

                if (department == "Claims")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingClaimReview;
                }
                else if (department == "Underwriting")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingUWReview;
                }
                else if (department == "Health")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingHealthReview;
                }
                else if (department == "RGA")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingRGAReview;
                }
                else if (department == "Compliance & Risk")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingCAndRReview;
                }
                else if (department == "Business Development")
                {
                    dep = TreatyPricingTreatyWorkflowBo.DraftingStatusPendingBDReview;
                }

                bo.DraftingStatus = dep;

                if (bos != null && bos.Any())
                {
                    var depBos = bos.Where(q => q.TreatyPricingTreatyWorkflowBo.DraftingStatus == dep);
                    bo.LessThan6MonthCountInTreaty = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan12MonthCountInTreaty = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.MoreThan12MonthCountInTreaty = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeTreaty).Count();
                    bo.LessThan6MonthCountInAddendum = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan12MonthCountInAddendum = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.MoreThan12MonthCountInAddendum = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && a.TreatyPricingTreatyWorkflowBo.DocumentType == TreatyPricingTreatyWorkflowBo.DocumentTypeAddendum).Count();
                    bo.LessThan6MonthCountInOther = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 6 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.LessThan12MonthCountInOther = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "<= 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.MoreThan12MonthCountInOther = depBos.Where(a => a.TreatyPricingTreatyWorkflowBo.OrionGroupStr == "> 12 months" && !treatyAndAddendum.Contains(a.TreatyPricingTreatyWorkflowBo.DocumentType)).Count();
                    bo.TotalCountPic = bo.LessThan6MonthCountInTreaty + bo.LessThan12MonthCountInTreaty + bo.MoreThan12MonthCountInTreaty
                        + bo.LessThan6MonthCountInAddendum + bo.LessThan12MonthCountInAddendum + bo.MoreThan12MonthCountInAddendum
                        + bo.LessThan6MonthCountInOther + bo.LessThan12MonthCountInOther + bo.MoreThan12MonthCountInOther;
                }
                else
                {
                    bo.LessThan6MonthCountInTreaty = 0;
                    bo.LessThan12MonthCountInTreaty = 0;
                    bo.MoreThan12MonthCountInTreaty = 0;
                    bo.LessThan6MonthCountInAddendum = 0;
                    bo.LessThan12MonthCountInAddendum = 0;
                    bo.MoreThan12MonthCountInAddendum = 0;
                    bo.LessThan6MonthCountInOther = 0;
                    bo.LessThan12MonthCountInOther = 0;
                    bo.MoreThan12MonthCountInOther = 0;
                    bo.TotalCountPic = 0;
                }

                //if (bo.TotalCountPic > 0)
                //{
                draftOverview.Add(bo);
                //}
            }

            total.TotalLessThan6MonthsCountInTreaty = draftOverview.Select(a => a.LessThan6MonthCountInTreaty).ToList().Sum();
            total.TotalLessThan12MonthsCountInTreaty = draftOverview.Select(a => a.LessThan12MonthCountInTreaty).ToList().Sum();
            total.TotalMoreThan12MonthsCountInTreaty = draftOverview.Select(a => a.MoreThan12MonthCountInTreaty).ToList().Sum();
            total.TotalLessThan6MonthsCountInAddendum = draftOverview.Select(a => a.LessThan6MonthCountInAddendum).ToList().Sum();
            total.TotalLessThan12MonthsCountInAddendum = draftOverview.Select(a => a.LessThan12MonthCountInAddendum).ToList().Sum();
            total.TotalMoreThan12MonthsCountInAddendum = draftOverview.Select(a => a.MoreThan12MonthCountInAddendum).ToList().Sum();
            total.TotalLessThan6MonthsCountInOther = draftOverview.Select(a => a.LessThan6MonthCountInOther).ToList().Sum();
            total.TotalLessThan12MonthsCountInOther = draftOverview.Select(a => a.LessThan12MonthCountInOther).ToList().Sum();
            total.TotalMoreThan12MonthsCountInOther = draftOverview.Select(a => a.MoreThan12MonthCountInOther).ToList().Sum();
            total.TotalCountForAll = draftOverview.Select(a => a.TotalCountPic).ToList().Sum();

            ViewBag.DraftingStatusOverviewByPendingDepartment = draftOverview;
            ViewBag.TotalDraftingStatusOverviewByPendingDepartment = total;
            return draftOverview;
        }

        public List<TreatyPricingTreatyWorkflowBo> GetDraftingStatusOverviewWithin2WeeksFromTargetSentDate(IEnumerable<TreatyPricingTreatyWorkflowVersionBo> bos)
        {
            var twoWeeksFromNow = DateTime.Now.Date.AddDays(14);
            var today = DateTime.Now.Date;
            var twBos = new List<TreatyPricingTreatyWorkflowBo>();

            if (bos != null && bos.Any())
            {
                bos = bos.Where(q => q.TargetSentDate.HasValue).Where(q => q.TargetSentDate >= today && q.TargetSentDate <= twoWeeksFromNow);
                foreach (var verBo in bos)
                {
                    var bo = verBo.TreatyPricingTreatyWorkflowBo;
                    bo.TargetSentDateStr = verBo.TargetSentDateStr;
                    twBos.Add(bo);
                }
            }

            ViewBag.DraftOverviewWithin2Weeks = twBos;
            ViewBag.TotalDraftOverviewWithin2Weeks = twBos.Count();

            return twBos;
        }

        public JsonResult GetDraftingStatusOverviewByPicDetail(int? picId)
        {
            var draftOverview = new List<TreatyPricingTreatyWorkflowBo>();

            var total = new TreatyPricingTreatyWorkflowBo();

            if (picId.HasValue)
            {
                total.PersonInChargeName = UserService.Find(picId).FullName;
                foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryUnassigned))
                {
                    var bo = new TreatyPricingTreatyWorkflowBo();
                    bo.DraftingStatusCategory = i;
                    bo.DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(i);
                    bo.LessThan6MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(picId, "<= 6 months", null, i);
                    bo.LessThan12MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(picId, "<= 12 months", null, i);
                    bo.MoreThan12MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(picId, "> 12 months", null, i);
                    bo.TotalCountPic = bo.LessThan6MonthCount + bo.LessThan12MonthCount + bo.MoreThan12MonthCount;
                    if (bo.TotalCountPic > 0)
                        draftOverview.Add(bo);
                }
            }
            else
            {
                total.PersonInChargeName = "Overall";
                foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryUnassigned))
                {
                    var bo = new TreatyPricingTreatyWorkflowBo();
                    bo.DraftingStatusCategoryName = TreatyPricingTreatyWorkflowBo.GetDraftingStatusCategoryName(i);
                    bo.DraftingStatusCategory = i;
                    bo.LessThan6MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(null, "<= 6 months", null, i);
                    bo.LessThan12MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(null, "<= 12 months", null, i);
                    bo.MoreThan12MonthCount = TreatyPricingTreatyWorkflowService.GetCountDraftOverviewByPic(null, "> 12 months", null, i);
                    bo.TotalCountPic = bo.LessThan6MonthCount + bo.LessThan12MonthCount + bo.MoreThan12MonthCount;
                    if (bo.TotalCountPic > 0)
                        draftOverview.Add(bo);
                }
            }

            total.TotalLessThan6MonthsCount = draftOverview.Select(a => a.LessThan6MonthCount).ToList().Sum();
            total.TotalLessThan12MonthsCount = draftOverview.Select(a => a.LessThan12MonthCount).ToList().Sum();
            total.TotalMoreThan12MonthsCount = draftOverview.Select(a => a.MoreThan12MonthCount).ToList().Sum();
            total.TotalCountForAll = total.TotalLessThan6MonthsCount + total.TotalLessThan12MonthsCount + total.TotalMoreThan12MonthsCount;

            return Json(new { draftingStatusCategoryCount = draftOverview, total = total });
        }

    }
}