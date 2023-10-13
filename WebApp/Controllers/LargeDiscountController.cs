using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LargeDiscountController : Controller
    {
        [HttpPost]
        public JsonResult Index(int? CedantId = null, bool isDistinctCode = false)
        {
            if (CedantId.HasValue)
            {
                return Json(new { LargeDiscounts = LargeDiscountService.GetByCedantId(CedantId.Value, isDistinctCode) });
            }
            return Json(new { LargeDiscounts = new List<LargeDiscountBo> { } });
        }

        [HttpPost]
        public JsonResult ValidateDelete(int id)
        {
            bool valid = true;
            //if (RateTableService.CountByLargeDiscountId(id) > 0)
            //{
            //    valid = false;
            //}

            return Json(new { Valid = valid });
        }
    }
}