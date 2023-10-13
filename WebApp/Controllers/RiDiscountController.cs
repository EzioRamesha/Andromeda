using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RiDiscountController : Controller
    {
        [HttpPost]
        public JsonResult Index(int? CedantId = null, bool isDistinctCode = false)
        {
            if (CedantId.HasValue)
            {
                return Json(new { RiDiscounts = RiDiscountService.GetByCedantId(CedantId.Value, isDistinctCode) });
            }
            return Json(new { RiDiscounts = new List<RiDiscountBo> { } });
        }

        [HttpPost]
        public JsonResult ValidateDelete(int id)
        {
            bool valid = true;
            //if (RateTableService.CountByRiDiscountId(id) > 0)
            //{
            //    valid = false;
            //}

            return Json(new { Valid = valid });
        }
    }
}