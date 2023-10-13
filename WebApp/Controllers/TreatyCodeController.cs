using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyCodeController : BaseController
    {
        [HttpPost]
        public JsonResult Index(int CedantId, int? SelectedId, int? Status = TreatyCodeBo.StatusActive, bool indexItem = false, bool foreign = true)
        {
            if (indexItem)
            {
                return Json(new { TreatyCodes = TreatyCodeService.GetIndexByCedantId(CedantId, foreign) });
            }
            return Json(new { TreatyCodes = TreatyCodeService.GetByCedantId(CedantId, Status, SelectedId) });
        }

        [HttpPost]
        public JsonResult GetByCedantCode(string CedantCode, string SelectedCode = null, int? Status = TreatyCodeBo.StatusActive)
        {
            return Json(new { TreatyCodes = TreatyCodeService.GetByCedantCode(CedantCode, Status, SelectedCode) });
        }

        [HttpPost]
        public JsonResult GetByTreatyId(int? treatyId, int? status = TreatyCodeBo.StatusActive)
        {
            IList<TreatyCodeBo> bos = new List<TreatyCodeBo>();
            if (treatyId.HasValue)
            {
                bos = TreatyCodeService.GetByTreatyId(treatyId.Value, status.Value);
            }

            return Json(new { TreatyCodes = bos });
        }

        [HttpPost]
        public JsonResult GetDropDownByCedantCode(string CedantCode, int? Status = TreatyCodeBo.StatusActive)
        {
            var cedantBo = CedantService.FindByCode(CedantCode);
            var treatyCodes = new List<SelectListItem> { };

            if (cedantBo != null)
            {
                treatyCodes = DropDownTreatyCode(Status, cedantId: cedantBo.Id, codeAsValue: true);
            }
            return Json(new { TreatyCodes = treatyCodes });
        }

        [HttpPost]
        public JsonResult CodeValue(int? CedantId = null, int? Status = TreatyCodeBo.StatusActive)
        {
            List<string> treatyCodes = new List<string>();
            if (CedantId.HasValue)
            {
                foreach (var treatyCodeBo in TreatyCodeService.GetByCedantId(CedantId.Value, Status))
                {
                    treatyCodes.Add(treatyCodeBo.Code);
                }
                return Json(new { TreatyCodes = treatyCodes });
            }
            else
            {
                foreach (var treatyCodeBo in TreatyCodeService.Get())
                {
                    treatyCodes.Add(treatyCodeBo.Code);
                }
                return Json(new { TreatyCodes = treatyCodes });
            }
        }
    }
}