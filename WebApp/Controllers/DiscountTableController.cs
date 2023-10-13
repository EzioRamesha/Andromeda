using BusinessObject;
using PagedList;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
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
    public class DiscountTableController : BaseController
    {
        public const string Controller = "DiscountTable";

        // GET: DiscountTable
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            int? CedantId,
            string SortOrder,
            int? Page)
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["CedantId"] = CedantId,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortCedant = GetSortParam("CedantId");

            var query = _db.DiscountTables.Select(DiscountTableViewModel.Expression());

            if (CedantId.HasValue) query = query.Where(q => q.CedantId == CedantId);

            if (SortOrder == Html.GetSortAsc("CedantId")) query = query.OrderBy(q => q.Cedant.Code);
            else if (SortOrder == Html.GetSortDsc("CedantId")) query = query.OrderByDescending(q => q.Cedant.Code);
            else query = query.OrderByDescending(q => q.Id);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: DiscountTable/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            DiscountTableViewModel model = new DiscountTableViewModel();

            ViewBag.Disabled = false;
            LoadPage(model);
            return View(model);
        }

        // POST: DiscountTable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, DiscountTableViewModel model)
        {
            model.Form = form;
            if (ModelState.IsValid)
            {
                var discountTableBo = new DiscountTableBo();
                Result = DiscountTableService.Result();

                model.Get(ref discountTableBo);
                discountTableBo.CreatedById = AuthUserId;
                discountTableBo.UpdatedById = AuthUserId;

                model.SetChildItems();

                Result childResult = model.ValidateChildItems();
                if (childResult.Valid)
                {
                    childResult = model.ValidateDuplicate();
                    if (childResult.Valid)
                    {
                        TrailObject trail = GetNewTrailObject();
                        Result = DiscountTableService.Create(ref discountTableBo, ref trail);
                        if (Result.Valid)
                        {
                            model.Id = discountTableBo.Id;
                            model.SaveChildItems(AuthUserId, ref trail);

                            CreateTrail(
                                discountTableBo.Id,
                                "Create Discount Table"
                            );

                            SetCreateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { discountTableBo.Id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            ViewBag.Disabled = false;
            LoadPage(model, form);
            return View(model);
        }

        // GET: DiscountTable/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            DiscountTableBo discountTableBo = DiscountTableService.Find(id);
            if (discountTableBo == null)
            {
                return RedirectToAction("Index");
            }
            DiscountTableViewModel model = new DiscountTableViewModel(discountTableBo);

            ViewBag.Disabled = false;
            LoadPage(model);
            return View(model);
        }

        // POST: DiscountTable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form, DiscountTableViewModel model)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            DiscountTableBo discountTableBo = DiscountTableService.Find(id);
            if (discountTableBo == null)
            {
                return RedirectToAction("Index");
            }

            model.Form = form;
            if (ModelState.IsValid)
            {
                Result = DiscountTableService.Result();
                model.Get(ref discountTableBo);
                model.SetChildItems();

                Result childResult = model.ValidateChildItems();
                discountTableBo.UpdatedById = AuthUserId;

                if (childResult.Valid)
                {
                    childResult = model.ValidateDuplicate();
                    if (childResult.Valid)
                    {
                        TrailObject trail = GetNewTrailObject();
                        Result = DiscountTableService.Update(ref discountTableBo, ref trail);
                        if (Result.Valid)
                        {
                            model.SaveChildItems(AuthUserId, ref trail);

                            CreateTrail(
                                discountTableBo.Id,
                                "Update Discount Table"
                            );

                            SetUpdateSuccessMessage(Controller);
                            return RedirectToAction("Edit", new { Id = id });
                        }
                    }
                }
                Result.AddErrorRange(childResult.ToErrorArray());
                AddResult(Result);
            }

            ViewBag.Disabled = false;
            LoadPage(model, form);
            return View(model);
        }

        // GET: DiscountTable/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            DiscountTableBo discountTableBo = DiscountTableService.Find(id);
            if (discountTableBo == null)
            {
                return RedirectToAction("Index");
            }
            DiscountTableViewModel model = new DiscountTableViewModel(discountTableBo);
            ViewBag.Disabled = true;
            LoadPage(model);
            return View(model);
        }

        // POST: DiscountTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, DiscountTableViewModel model)
        {
            var bo = DiscountTableService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index");
            }

            var trail = GetNewTrailObject();
            Result = DiscountTableService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Discount Table"
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
        public ActionResult UploadRiDiscount(FormCollection form, DiscountTableViewModel model)
        {
            ModelState.Clear();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            model.Form = form;
            List<Column> cols = RiDiscountBo.GetColumns();

            Type objectType = typeof(RiDiscountBo);
            string objectTypeName = objectType.Name.Replace("Bo", "");

            string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

            List<RiDiscountBo> riDiscountBos;
            if (string.IsNullOrEmpty(maxIndexStr))
                riDiscountBos = null;

            int maxIndex = int.Parse(maxIndexStr);
            riDiscountBos = (List<RiDiscountBo>)model.GetChildItem(objectType, maxIndex);

            HttpPostedFileBase postedFile = Request.Files["RiDiscountFile"];
            try
            {
                TextFile textFile = new TextFile(postedFile.InputStream);
                while (textFile.GetNextRow() != null)
                {
                    if (textFile.RowIndex == 1)
                        continue; // Skip header row

                    var rd = new RiDiscountBo
                    {
                        Id = 0,
                        DiscountCode = textFile.GetColValue(RiDiscountBo.TypeDiscountCode),
                        DurationFromStr = textFile.GetColValue(RiDiscountBo.TypeDurationFrom),
                        DurationToStr = textFile.GetColValue(RiDiscountBo.TypeDurationTo),
                        DiscountStr = textFile.GetColValue(RiDiscountBo.TypeDiscount)
                    };

                    var effectiveStartDate = textFile.GetColValue(RiDiscountBo.TypeEffectiveStartDate);
                    if (!string.IsNullOrEmpty(effectiveStartDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveStartDate);
                        rd.EffectiveStartDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    var effectiveEndDate = textFile.GetColValue(RiDiscountBo.TypeEffectiveEndDate);
                    if (!string.IsNullOrEmpty(effectiveEndDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveEndDate);
                        rd.EffectiveEndDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    bool isUpdate = false;

                    string idStr = textFile.GetColValue(RiDiscountBo.TypeId);
                    if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                    {
                        if (riDiscountBos.Any(q => q.Id == id))
                        {
                            rd.Id = id;
                            int index = riDiscountBos.FindIndex(q => q.Id == id);
                            riDiscountBos[index] = rd;
                            isUpdate = true;
                        }
                    }

                    if (!isUpdate)
                    {
                        riDiscountBos.Add(rd);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                CheckEditPageReadOnly(Controller);
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            ViewBag.Disabled = false;
            LoadPage(model, form, riDiscountList: riDiscountBos);
            return View(view, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult UploadLargeDiscount(FormCollection form, DiscountTableViewModel model)
        {
            ModelState.Clear();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            model.Form = form;
            List<Column> cols = LargeDiscountBo.GetColumns();

            Type objectType = typeof(LargeDiscountBo);
            string objectTypeName = objectType.Name.Replace("Bo", "");

            string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

            List<LargeDiscountBo> largeDiscountBos;
            if (string.IsNullOrEmpty(maxIndexStr))
                largeDiscountBos = null;

            int maxIndex = int.Parse(maxIndexStr);
            largeDiscountBos = (List<LargeDiscountBo>)model.GetChildItem(objectType, maxIndex);

            HttpPostedFileBase postedFile = Request.Files["LargeDiscountFile"];
            try
            {
                TextFile textFile = new TextFile(postedFile.InputStream);
                while (textFile.GetNextRow() != null)
                {
                    if (textFile.RowIndex == 1)
                        continue; // Skip header row

                    var ld = new LargeDiscountBo
                    {
                        Id = 0,
                        DiscountCode = textFile.GetColValue(LargeDiscountBo.TypeDiscountCode),
                        AarFromStr = textFile.GetColValue(LargeDiscountBo.TypeAarFrom),
                        AarToStr = textFile.GetColValue(LargeDiscountBo.TypeAarTo),
                        DiscountStr = textFile.GetColValue(LargeDiscountBo.TypeDiscount)
                    };

                    var effectiveStartDate = textFile.GetColValue(LargeDiscountBo.TypeEffectiveStartDate);
                    if (!string.IsNullOrEmpty(effectiveStartDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveStartDate);
                        ld.EffectiveStartDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    var effectiveEndDate = textFile.GetColValue(LargeDiscountBo.TypeEffectiveEndDate);
                    if (!string.IsNullOrEmpty(effectiveEndDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveEndDate);
                        ld.EffectiveEndDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    bool isUpdate = false;

                    string idStr = textFile.GetColValue(LargeDiscountBo.TypeId);
                    if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                    {
                        if (largeDiscountBos.Any(q => q.Id == id))
                        {
                            ld.Id = id;
                            int index = largeDiscountBos.FindIndex(q => q.Id == id);
                            largeDiscountBos[index] = ld;
                            isUpdate = true;
                        }
                    }

                    if (!isUpdate)
                    {
                        largeDiscountBos.Add(ld);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                CheckEditPageReadOnly(Controller);
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            ViewBag.Disabled = false;
            LoadPage(model, form, largeDiscountList: largeDiscountBos);
            return View(view, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult UploadGroupDiscount(FormCollection form, DiscountTableViewModel model)
        {
            ModelState.Clear();
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            model.Form = form;
            List<Column> cols = GroupDiscountBo.GetColumns();

            Type objectType = typeof(GroupDiscountBo);
            string objectTypeName = objectType.Name.Replace("Bo", "");

            string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

            List<GroupDiscountBo> groupDiscountBos;
            if (string.IsNullOrEmpty(maxIndexStr))
                groupDiscountBos = null;

            int maxIndex = int.Parse(maxIndexStr);
            groupDiscountBos = (List<GroupDiscountBo>)model.GetChildItem(objectType, maxIndex);

            HttpPostedFileBase postedFile = Request.Files["GroupDiscountFile"];
            try
            {
                TextFile textFile = new TextFile(postedFile.InputStream);
                while (textFile.GetNextRow() != null)
                {
                    if (textFile.RowIndex == 1)
                        continue; // Skip header row

                    var gd = new GroupDiscountBo
                    {
                        Id = 0,
                        DiscountCode = textFile.GetColValue(GroupDiscountBo.TypeDiscountCode),
                        GroupSizeFromStr = textFile.GetColValue(GroupDiscountBo.TypeGroupSizeFrom),
                        GroupSizeToStr = textFile.GetColValue(GroupDiscountBo.TypeGroupSizeTo),
                        DiscountStr = textFile.GetColValue(GroupDiscountBo.TypeDiscount)
                    };

                    var effectiveStartDate = textFile.GetColValue(GroupDiscountBo.TypeEffectiveStartDate);
                    if (!string.IsNullOrEmpty(effectiveStartDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveStartDate);
                        gd.EffectiveStartDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    var effectiveEndDate = textFile.GetColValue(GroupDiscountBo.TypeEffectiveEndDate);
                    if (!string.IsNullOrEmpty(effectiveEndDate))
                    {
                        DateTime? dt = Util.GetParseDateTime(effectiveEndDate);
                        gd.EffectiveEndDateStr = dt?.ToString(Util.GetDateFormat());
                    }

                    bool isUpdate = false;

                    string idStr = textFile.GetColValue(GroupDiscountBo.TypeId);
                    if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                    {
                        if (groupDiscountBos.Any(q => q.Id == id))
                        {
                            gd.Id = id;
                            int index = groupDiscountBos.FindIndex(q => q.Id == id);
                            groupDiscountBos[index] = gd;
                            isUpdate = true;
                        }
                    }

                    if (!isUpdate)
                    {
                        groupDiscountBos.Add(gd);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            string view = "Create";
            if (model.Id != 0)
            {
                view = "Edit";
                CheckEditPageReadOnly(Controller);
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            ViewBag.Disabled = false;
            LoadPage(model, form, groupDiscountList: groupDiscountBos);
            return View(view, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadRiDiscount(FormCollection form, DiscountTableViewModel model, int? type = null)
        {
            model.Form = form;
            List<Column> cols = RiDiscountBo.GetColumns();
            string filename = "RiDiscount".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            string filePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(filePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"RiDiscount*");

            // Header
            ExportWriteLine(filePath, string.Join(",", cols.Select(p => p.Header)));

            if (type == 1)
            {
                Type objectType = typeof(RiDiscountBo);
                string objectTypeName = objectType.Name.Replace("Bo", "");

                string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

                if (!string.IsNullOrEmpty(maxIndexStr))
                {
                    int maxIndex = int.Parse(maxIndexStr);
                    IList<RiDiscountBo> riDiscountBos = (IList<RiDiscountBo>)model.GetChildItem(objectType, maxIndex);

                    foreach (var riDiscountBo in riDiscountBos)
                    {
                        List<string> values = new List<string> { };
                        foreach (var col in cols)
                        {
                            if (string.IsNullOrEmpty(col.Property))
                            {
                                values.Add("");
                                continue;
                            }

                            string value = "";
                            object v = null;

                            switch (col.ColIndex)
                            {
                                case RiDiscountBo.TypeId:
                                    v = riDiscountBo.GetPropertyValue(col.Property).ToString() != "0" ? riDiscountBo.GetPropertyValue(col.Property) : null;
                                    break;
                                default:
                                    v = riDiscountBo.GetPropertyValue(col.Property);
                                    break;
                            }

                            if (v != null)
                            {
                                if (v is DateTime d)
                                {
                                    value = d.ToString(Util.GetDateFormat());
                                }
                                else if (v is double doubleValue)
                                {
                                    value = Util.DoubleToString(doubleValue);
                                }
                                else
                                {
                                    value = v.ToString();
                                }
                            }

                            values.Add(string.Format("\"{0}\"", value));
                        }
                        string line = string.Join(",", values.ToArray());
                        ExportWriteLine(filePath, line);
                    }
                }
            }

            return File(filePath, "text/csv", Path.GetFileName(filePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadLargeDiscount(FormCollection form, DiscountTableViewModel model, int? type = null)
        {
            model.Form = form;
            List<Column> cols = LargeDiscountBo.GetColumns();
            string filename = "LargeDiscount".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            string filePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(filePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"LargeDiscount*");

            // Header
            ExportWriteLine(filePath, string.Join(",", cols.Select(p => p.Header)));

            if (type == 1)
            {
                Type objectType = typeof(LargeDiscountBo);
                string objectTypeName = objectType.Name.Replace("Bo", "");

                string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

                if (!string.IsNullOrEmpty(maxIndexStr))
                {
                    int maxIndex = int.Parse(maxIndexStr);
                    IList<LargeDiscountBo> largeDiscountBos = (IList<LargeDiscountBo>)model.GetChildItem(objectType, maxIndex);

                    foreach (var largeDiscountBo in largeDiscountBos)
                    {
                        List<string> values = new List<string> { };
                        foreach (var col in cols)
                        {
                            if (string.IsNullOrEmpty(col.Property))
                            {
                                values.Add("");
                                continue;
                            }

                            string value = "";
                            object v = null;

                            switch (col.ColIndex)
                            {
                                case LargeDiscountBo.TypeId:
                                    v = largeDiscountBo.GetPropertyValue(col.Property).ToString() != "0" ? largeDiscountBo.GetPropertyValue(col.Property) : null;
                                    break;
                                default:
                                    v = largeDiscountBo.GetPropertyValue(col.Property);
                                    break;
                            }

                            if (v != null)
                            {
                                if (v is DateTime d)
                                {
                                    value = d.ToString(Util.GetDateFormat());
                                }
                                else if (v is double doubleValue)
                                {
                                    value = Util.DoubleToString(doubleValue);
                                }
                                else
                                {
                                    value = v.ToString();
                                }
                            }

                            values.Add(string.Format("\"{0}\"", value));
                        }
                        string line = string.Join(",", values.ToArray());
                        ExportWriteLine(filePath, line);
                    }
                }
            }

            return File(filePath, "text/csv", Path.GetFileName(filePath));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult DownloadGroupDiscount(FormCollection form, DiscountTableViewModel model, int? type = null)
        {
            model.Form = form;
            List<Column> cols = GroupDiscountBo.GetColumns();
            string filename = "GroupDiscount".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            string filePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(filePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"GroupDiscount*");

            // Header
            ExportWriteLine(filePath, string.Join(",", cols.Select(p => p.Header)));

            if (type == 1)
            {
                Type objectType = typeof(GroupDiscountBo);
                string objectTypeName = objectType.Name.Replace("Bo", "");

                string maxIndexStr = model.Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

                if (!string.IsNullOrEmpty(maxIndexStr))
                {
                    int maxIndex = int.Parse(maxIndexStr);
                    IList<GroupDiscountBo> groupDiscountBos = (IList<GroupDiscountBo>)model.GetChildItem(objectType, maxIndex);

                    foreach (var groupDiscountBo in groupDiscountBos)
                    {
                        List<string> values = new List<string> { };
                        foreach (var col in cols)
                        {
                            if (string.IsNullOrEmpty(col.Property))
                            {
                                values.Add("");
                                continue;
                            }

                            string value = "";
                            object v = null;

                            switch (col.ColIndex)
                            {
                                case GroupDiscountBo.TypeId:
                                    v = groupDiscountBo.GetPropertyValue(col.Property).ToString() != "0" ? groupDiscountBo.GetPropertyValue(col.Property) : null;
                                    break;
                                default:
                                    v = groupDiscountBo.GetPropertyValue(col.Property);
                                    break;
                            }

                            if (v != null)
                            {
                                if (v is DateTime d)
                                {
                                    value = d.ToString(Util.GetDateFormat());
                                }
                                else if (v is double doubleValue)
                                {
                                    value = Util.DoubleToString(doubleValue);
                                }
                                else
                                {
                                    value = v.ToString();
                                }
                            }

                            values.Add(string.Format("\"{0}\"", value));
                        }
                        string line = string.Join(",", values.ToArray());
                        ExportWriteLine(filePath, line);
                    }
                }
            }

            return File(filePath, "text/csv", Path.GetFileName(filePath));
        }

        public void ExportWriteLine(string filePath, object line)
        {
            using (var textFile = new TextFile(filePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public void IndexPage()
        {
            DropDownCedant();
            SetViewBagMessage();
        }

        public void LoadPage(DiscountTableViewModel model, FormCollection form = null, List<RiDiscountBo> riDiscountList = null, List<LargeDiscountBo> largeDiscountList = null, List<GroupDiscountBo> groupDiscountList = null)
        {
            model.Form = form;
            LoadChildItems(model, riDiscountList, largeDiscountList, groupDiscountList);

            if (model.Id == 0)
            {
                DropDownCedant(CedantBo.StatusActive);
            }
            else
            {
                DropDownCedant(CedantBo.StatusActive, model.CedantId);
                if (model.CedantBo != null && model.CedantBo.Status == CedantBo.StatusInactive)
                {
                    AddWarningMsg(MessageBag.CedantStatusInactive);
                }
            }

            SetViewBagMessage();
        }

        public void LoadChildItems(DiscountTableViewModel model, List<RiDiscountBo> riDiscountList = null, List<LargeDiscountBo> largeDiscountList = null, List<GroupDiscountBo> groupDiscountList = null)
        {
            IList<RiDiscountBo> riDiscountBos = riDiscountList ?? null;
            IList<LargeDiscountBo> largeDiscountBos = largeDiscountList ?? null;
            IList<GroupDiscountBo> groupDiscountBos = groupDiscountList ?? null;

            if (model.Form != null)
            {
                model.SetChildItems();
                riDiscountBos = riDiscountList ?? model.RiDiscountBos;
                largeDiscountBos = largeDiscountList ?? model.LargeDiscountBos;
                groupDiscountBos = groupDiscountList ?? model.GroupDiscountBos;
            }
            else if (model.Id != 0)
            {
                riDiscountBos = riDiscountList ?? RiDiscountService.GetByDiscountTableId(model.Id);
                largeDiscountBos = largeDiscountList ?? LargeDiscountService.GetByDiscountTableId(model.Id);
                groupDiscountBos = groupDiscountList ?? GroupDiscountService.GetByDiscountTableId(model.Id);
            }

            ViewBag.RiDiscountBos = riDiscountBos;
            ViewBag.LargeDiscountBos = largeDiscountBos;
            ViewBag.GroupDiscountBos = groupDiscountBos;
        }
    }
}
