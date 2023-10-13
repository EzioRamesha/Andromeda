using DataAccess.Entities.Retrocession;
using Services.Retrocession;
using Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeRetroConfigurationRatioController : BaseController
    {
        public const string Controller = "PerLifeRetroConfiguration";

        // GET: PerLifeRetroConfigurationRatio/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeRetroConfigurationRatioViewModel());
        }

        // POST: PerLifeRetroConfigurationRatio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeRetroConfigurationRatioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = PerLifeRetroConfigurationRatioService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Retro Configuration Ratio"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroConfigurationRatio/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeRetroConfigurationRatioService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 2 });
            }

            LoadPage();
            return View(new PerLifeRetroConfigurationRatioViewModel(bo));
        }

        // POST: PerLifeRetroConfigurationRatio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeRetroConfigurationRatioViewModel model)
        {
            var dbBo = PerLifeRetroConfigurationRatioService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 2 });
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeRetroConfigurationRatioService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Retro Configuration Ratio"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroConfigurationRatio/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeRetroConfigurationRatioService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 2 });
            }

            return View(new PerLifeRetroConfigurationRatioViewModel(bo));
        }

        // POST: PerLifeRetroConfigurationRatio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeRetroConfigurationRatioViewModel model)
        {
            var bo = PerLifeRetroConfigurationRatioService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 2 });
            }

            var trail = GetNewTrailObject();
            Result = PerLifeRetroConfigurationRatioService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Retro Configuration Ratio"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 2 });
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public void LoadPage()
        {
            DropDownTreatyCode(isUniqueCode: true, foreign: false);

            var entity = new PerLifeRetroConfigurationRatio();
            var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
            ViewBag.DescriptionMaxLength = maxLengthAttr != null ? maxLengthAttr.Length : 255;

            SetViewBagMessage();
        }
    }
}