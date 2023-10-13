using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class GroupDiscountController : Controller
    {
        [HttpPost]
        public JsonResult Index(int? CedantId = null, bool isDistinctCode = false)
        {
            if (CedantId.HasValue)
            {
                return Json(new { GroupDiscounts = GroupDiscountService.GetByCedantId(CedantId.Value, isDistinctCode) });
            }
            return Json(new { GroupDiscounts = new List<GroupDiscountBo> { } });
        }

        [HttpPost]
        public JsonResult ValidateDelete(int id)
        {
            bool valid = true;
            //if (RateTableService.CountByGroupDiscountId(id) > 0)
            //{
            //    valid = false;
            //}

            return Json(new { Valid = valid });
        }
    }
}