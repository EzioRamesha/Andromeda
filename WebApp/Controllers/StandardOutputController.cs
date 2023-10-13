using BusinessObject;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class StandardOutputController : BaseController
    {
        public const string Controller = "StandardOutput";

        // GET: StandardOutput
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(string name, string sortOrder, int? page)
        {
            var query = _db.StandardOutputs.Select(q => new StandardOutputViewModel
            {
                Id = q.Id,
                Type = q.Type,
                DataType = q.DataType,
                Name = q.Name,
            });

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(q => q.Name.Contains(name));
                ViewBag.NameFilter = name;
            }

            ViewBag.SortOrder = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";

            switch (sortOrder)
            {
                case "name":
                    query = query.OrderBy(q => q.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(q => q.Name);
                    break;
                default:
                    query = query.OrderBy(q => q.Type);
                    break;
            }

            int pageNumber = (page ?? 1);

            ViewBag.Total = query.Count();
            return View(query.ToPagedList(pageNumber, PageSize));
        }

        // GET: StandardOutput/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            return RedirectToAction("Index");
            /*
            GetTypeList();
            GetDataTypeList();
            return View(new StandardOutputViewModel());
            */
        }

        // POST: StandardOutput/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, StandardOutputViewModel model)
        {
            return RedirectToAction("Index");
            /*
            if (ModelState.IsValid)
            {
                var standardOutputBo = new StandardOutputBo
                {
                    Type = StandardOutputBo.TypeCustomField,
                    DataType = model.DataType,
                    Name = model.Name,
                    Code = model.Code,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = StandardOutputService.Create(ref standardOutputBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        standardOutputBo.Id,
                        "Create Standard Output"
                    );

                    model.Id = standardOutputBo.Id;
                    return RedirectToAction("Index");
                }
                AddResult(Result);
            }
            GetTypeList();
            GetDataTypeList();
            return View(model);
            */
        }

        // GET: StandardOutput/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Index");
            /*
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            StandardOutputBo standardOutputBo = StandardOutputService.Find(id);
            if (standardOutputBo == null)
            {
                return RedirectToAction("Index");
            }
            GetTypeList(standardOutputBo.Type);
            GetDataTypeList(standardOutputBo.DataType);
            return View(new StandardOutputViewModel(standardOutputBo));
            */
        }

        // POST: StandardOutput/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, StandardOutputViewModel model)
        {
            return RedirectToAction("Index");
            /*
            StandardOutputBo standardOutputBo = StandardOutputService.Find(id);
            if (standardOutputBo == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                standardOutputBo.Type = model.Type;
                standardOutputBo.DataType = model.DataType;
                standardOutputBo.Name = model.Name;
                standardOutputBo.Code = model.Code;
                standardOutputBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = StandardOutputService.Update(ref standardOutputBo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        standardOutputBo.Id,
                        "Update Standard Output"
                    );

                    return RedirectToAction("Index");
                }
                AddResult(Result);
            }
            GetTypeList(standardOutputBo.Type);
            GetDataTypeList(standardOutputBo.DataType);
            return View(model);
            */
        }

        // GET: StandardOutput/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
            /*
            StandardOutputBo standardOutputBo = StandardOutputService.Find(id);
            if (standardOutputBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new StandardOutputViewModel(standardOutputBo));
            */
        }

        // POST: StandardOutput/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, StandardOutputViewModel model)
        {
            return RedirectToAction("Index");
            /*
            StandardOutputBo standardOutputBo = StandardOutputService.Find(id);
            if (standardOutputBo == null)
            {
                return RedirectToAction("Index");
            }

            TrailObject trail = GetNewTrailObject();
            Result = StandardOutputService.Delete(standardOutputBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    standardOutputBo.Id,
                    "Delete Standard Output"
                );
            }
            return RedirectToAction("Index");
            */
        }

        public void GetTypeList(int type = 0)
        {
            List<SelectListItem> items = new List<SelectListItem> { };
            items.Add(new SelectListItem { Text = "Please select", Value = "" });
            for (int i = 1; i <= StandardOutputBo.TypeMax; i++)
            {
                items.Add(new SelectListItem { Selected = i == type, Text = StandardOutputBo.GetTypeName(i), Value = i.ToString() });
            }
            ViewBag.TypeItems = items;
        }

        public void GetDataTypeList(int type = 0)
        {
            List<SelectListItem> items = new List<SelectListItem> { };
            items.Add(new SelectListItem { Text = "Please select", Value = "" });
            for (int i = 1; i <= StandardOutputBo.DataTypeMax; i++)
            {
                items.Add(new SelectListItem { Selected = i == type, Text = StandardOutputBo.GetDataTypeName(i), Value = i.ToString() });
            }
            ViewBag.DataTypeItems = items;
        }
    }
}