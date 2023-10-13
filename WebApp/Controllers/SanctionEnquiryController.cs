using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using PagedList;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class SanctionEnquiryController : BaseController
    {
        public const string Controller = "SanctionEnquiry";

        // GET: SanctionEnquiry
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string Category,
            string InsuredName,
            string IdNumber,
            string DateOfBirth,
            int? IsSearch,
            int? Page
        )
        {
            DateTime? dateOfBirth = Util.GetParseDateTime(DateOfBirth);

            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["Category"] = Category,
                ["InsuredName"] = InsuredName,
                ["IdNumber"] = IdNumber,
                ["DateOfBirth"] = dateOfBirth.HasValue ? DateOfBirth : null,
            };

            _db.Database.CommandTimeout = 0;
            DateTime today = DateTime.Parse(DateTime.Today.ToShortDateString() + " " + Util.GetConfig("SanctionFormatNameTime", "05:00"));
            DateTime? keywordUpdatedAt = null;
            if (_db.SanctionKeywords.Any())
            {
                keywordUpdatedAt = _db.SanctionKeywords.Max(q => q.UpdatedAt);
            }
            DateTime? exclusionUpdatedAt = null;
            if (_db.SanctionExclusions.Any())
            {
                exclusionUpdatedAt = _db.SanctionExclusions.Max(q => q.UpdatedAt);
            }
            if ((keywordUpdatedAt != null && keywordUpdatedAt > today) || (exclusionUpdatedAt != null && exclusionUpdatedAt > today))
            {
                SetWarningSessionMsg("Latest changes on keyword exclusion and keyword grouping might not be reflected yet");
            }

            List<string> errors = new List<string>();

            var query = _db.Sanctions.Select(SanctionViewModel.Expression());
            if (!IsSearch.HasValue)
            {
                query = query.Where(q => q.Id == 0);
            }
            else if (string.IsNullOrEmpty(InsuredName))
            {
                errors.Add(string.Format("Insured Name is required"));
                query = query.Where(q => q.Id == 0);
            }
            else
            {
                StoredProcedure search = new StoredProcedure(StoredProcedure.SanctionVerificationSearch);
                search.AddParameter("Category", string.IsNullOrEmpty(Category) ? null : Category);
                search.AddParameter("InsuredName", InsuredName);
                search.AddParameter("InsuredDateOfBirth", dateOfBirth.HasValue ? dateOfBirth : null);
                search.AddParameter("InsuredIcNumber", string.IsNullOrEmpty(IdNumber) ? null : IdNumber);
                search.AddParameter("IgnoreSanctionIds", 1);
                search.AddParameter("ResultSanctionIds", isOutputParam: true);
                search.Execute();

                bool isMatched = false;
                if (search.Success)
                {
                    ViewBag.ReturnResult = search.ReturnResult;
                    isMatched = search.ReturnResult != "0" ? true : false;
                }
                else if (!string.IsNullOrEmpty(search.Result))
                {
                    errors.Add(search.Result);
                }

                if (isMatched)
                {
                    string[] ids = Util.ToArraySplitTrim(search.Outputs["ResultSanctionIds"]);
                    int[] sanctionIds = Array.ConvertAll(ids, s => int.Parse(s));
                    query = query.Where(q => sanctionIds.Contains(q.Id));
                }
                else
                {
                    query = query.Where(q => q.Id == 0);
                }
            }

            //if (IsSearch.HasValue && errors.Count() == 0)
            //{
            //    List<string> categories = new List<string> { SanctionBo.GetCategoryName(SanctionBo.CategoryIndividual), SanctionBo.GetCategoryName(SanctionBo.CategoryEntity) };
            //    if (!string.IsNullOrEmpty(Category))
            //    {
            //        categories = new List<string> { Category };
            //    }

            //    query = query.Where(q => categories.Contains(q.Category));

            //    if (query.Count() != 0)
            //    {
            //        if (!string.IsNullOrEmpty(IdNumber))
            //        {
            //            var idQuery = query.Where(q => q.SanctionIdentities.Any(s => s.IdNumber.Trim() == IdNumber.Trim()));

            //            if (idQuery.Count() > 0)
            //            {
            //                isMatched = true;
            //                query = idQuery;
            //            }
            //        }

            //        if (!isMatched)
            //        {
            //            if (dateOfBirth.HasValue)
            //            {
            //                var dobQuery = query.Where(q => q.SanctionBirthDates.Any(s =>
            //                    (s.DateOfBirth.HasValue && DbFunctions.TruncateTime(s.DateOfBirth.Value) == DbFunctions.TruncateTime(dateOfBirth.Value)) ||
            //                    (!s.DateOfBirth.HasValue && s.YearOfBirth.HasValue && dateOfBirth.Value.Year == s.YearOfBirth)
            //                ));

            //                query = dobQuery;
            //            }

            //            if (!string.IsNullOrEmpty(InsuredName))
            //            {
            //                Regex symbol = new Regex(@"[^a-zA-Z0-9 ]+");
            //                string nonSymbolName = symbol.Replace(InsuredName, string.Empty);
            //                string formattedName = nonSymbolName.Replace(" ", string.Empty);

            //                string[] names = nonSymbolName.Split(' ').Select(q => q.Trim().ToUpper()).ToArray();
            //                List<string> excludedNames = SanctionExclusionService.FormatName(names);
            //                string excludedName = string.Join(string.Empty, excludedNames);

            //                if (!isMatched)
            //                {
            //                    var nameQuery = query.Where(q => q.SanctionFormatNames
            //                    .Where(s => s.Type == SanctionFormatNameBo.TypeSymbolRemoval)
            //                    .Any(s => s.Name.Equals(formattedName.Trim(), StringComparison.OrdinalIgnoreCase)));

            //                    if (nameQuery.Count() > 0)
            //                    {
            //                        isMatched = true;
            //                        //sanctionBos = SanctionService.FormBos(nameQuery.ToList(), true);
            //                        query = nameQuery;
            //                    }
            //                }

            //                if (!isMatched)
            //                {
            //                    var formattedNameQuery = query.Where(q => q.SanctionFormatNames
            //                    .Where(s => s.Type == SanctionFormatNameBo.TypeKeywordReplacement)
            //                    .Any(s => s.Name.Equals(excludedName.Trim(), StringComparison.OrdinalIgnoreCase)));

            //                    if (formattedNameQuery.Count() > 0)
            //                    {
            //                        isMatched = true;
            //                        //sanctionBos = SanctionService.FormBos(formattedNameQuery.ToList(), true);
            //                        query = formattedNameQuery;
            //                    }
            //                }

            //                if (!isMatched)
            //                {
            //                    var sanctionsIds = _db.SanctionFormatNames
            //                        .Where(q => q.Type == SanctionFormatNameBo.TypeGroupKeyword)
            //                        .Where(q => excludedNames.Contains(q.Name))
            //                        .GroupBy(q => q.SanctionNameId)
            //                        .Where(r => r.Count() >= 3)
            //                        .Select(q => q.FirstOrDefault().SanctionId)
            //                        .ToList();

            //                    if (sanctionsIds != null && sanctionsIds.Count() > 0)
            //                    {
            //                        var groupNameQuery = query.Where(q => sanctionsIds.Contains(q.Id));
            //                        if (groupNameQuery.Count() > 0)
            //                        {
            //                            isMatched = true;
            //                            //sanctionBos = SanctionService.FormBos(groupNameQuery.ToList(), true);
            //                            query = groupNameQuery;
            //                        }
            //                    }
            //                }
            //            }
            //            //else if (query.Count() > 0)
            //            //{
            //            //    isMatched = true;
            //            //    sanctionBos = SanctionService.FormBos(query.ToList(), true);
            //            //}
            //        }
            //    }
            //}
            //else if (IsSearch.HasValue && errors.Count() > 0)
            //{
            //    SetErrorSessionMsgArr(errors);
            //}

            //if (!isMatched)
            //    query = query.Where(q => q.Id == 0);

            int count = query.Count();
            if (IsSearch.HasValue)
            {
                if (errors.Count() > 0)
                    SetErrorSessionMsgArr(errors);
                else if (count == 0)
                    SetErrorSessionMsg(MessageBag.NoRecordFound);
                else
                    SetSuccessSessionMsg(MessageBag.RecordFound);
            }

            query = query.OrderBy(q => q.Id);

            IndexPage();
            ViewBag.Total = query.Count();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        public void IndexPage()
        {
            DropDownCategory();
            SetViewBagMessage();
        }

        public List<SelectListItem> DropDownCategory()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= SanctionBo.CategoryMax; i++)
            {
                items.Add(new SelectListItem { Text = SanctionBo.GetCategoryName(i), Value = SanctionBo.GetCategoryName(i) });
            }
            ViewBag.DropDownCategories = items;
            return items;
        }
    }
}