using Services;
using Services.Retrocession;
using System.Linq;
using System.Web.Mvc;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class PerLifeRetroConfigurationTreatyController : BaseController
    {
        public const string Controller = "PerLifeRetroConfiguration";

        // GET: PerLifeRetroConfigurationTreaty/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            LoadPage();
            return View(new PerLifeRetroConfigurationTreatyViewModel());
        }

        // POST: PerLifeRetroConfigurationTreaty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, PerLifeRetroConfigurationTreatyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bo = model.FormBo(AuthUserId, AuthUserId);

                var trail = GetNewTrailObject();
                Result = PerLifeRetroConfigurationTreatyService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    model.Id = bo.Id;

                    CreateTrail(
                        bo.Id,
                        "Create Per Life Retro Configuration Treaty"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroConfigurationTreaty/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            var bo = PerLifeRetroConfigurationTreatyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 1 });
            }

            LoadPage();
            return View(new PerLifeRetroConfigurationTreatyViewModel(bo));
        }

        // POST: PerLifeRetroConfigurationTreaty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, PerLifeRetroConfigurationTreatyViewModel model)
        {
            var dbBo = PerLifeRetroConfigurationTreatyService.Find(id);
            if (dbBo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 1 });
            }

            if (ModelState.IsValid)
            {
                var bo = model.FormBo(dbBo.CreatedById, AuthUserId);
                bo.Id = dbBo.Id;

                var trail = GetNewTrailObject();

                Result = PerLifeRetroConfigurationTreatyService.Update(ref bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(
                        bo.Id,
                        "Update Per Life Retro Configuration Treaty"
                    );

                    SetUpdateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = bo.Id });
                }
                AddResult(Result);
            }

            LoadPage();
            return View(model);
        }

        // GET: PerLifeRetroConfigurationTreaty/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            var bo = PerLifeRetroConfigurationTreatyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 1 });
            }

            return View(new PerLifeRetroConfigurationTreatyViewModel(bo));
        }

        // POST: PerLifeRetroConfigurationTreaty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, PerLifeRetroConfigurationTreatyViewModel model)
        {
            var bo = PerLifeRetroConfigurationTreatyService.Find(id);
            if (bo == null)
            {
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 1 });
            }

            var trail = GetNewTrailObject();
            Result = PerLifeRetroConfigurationTreatyService.Delete(bo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    bo.Id,
                    "Delete Per Life Retro Configuration Treaty"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index", "PerLifeRetroConfiguration", new { TabIndex = 1 });
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = bo.Id });
        }

        public JsonResult FindTreatyCode(int? treatyCodeId = null)
        {
            var treatyCodeBo = TreatyCodeService.Find(treatyCodeId);

            return Json(new { TreatyCodeBo = treatyCodeBo });
        }

        public void LoadPage()
        {
            DropDownTreatyCode(isUniqueCode: true, foreign: false);
            DropDownTreatyType();
            DropDownFundsAccountingTypeCode();
            DropDownYesNo();

            SetViewBagMessage();
        }

        [HttpPost]
        public JsonResult GetByParams(int? treatyCodeId, int? treatyTypeId, bool? isToAggregate, int? retroTreatyId)
        {
            var treatyCodeBo = TreatyCodeService.Find(treatyCodeId);
            string error = null;


            var bos = PerLifeRetroConfigurationTreatyService.GetByParam(treatyCodeBo?.Code, treatyTypeId, isToAggregate, retroTreatyId);
            bool success = bos.Count() > 0;
            if (!success)
            {
                if (PerLifeRetroConfigurationTreatyService.CountByParam(treatyCodeBo?.Code, treatyTypeId, isToAggregate) > 0)
                {
                    error = "Treaty Code has been selected";
                }
                else
                {
                    error = "Treaty Code not found";
                }
            }


            return Json(new { bos, success, error });
        }
    }
}