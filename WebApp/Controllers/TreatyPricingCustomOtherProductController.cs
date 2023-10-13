using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingCustomOtherProductController : BaseController
    {
        [HttpPost]
        public JsonResult Search(int ParentId, int? TreatyPricingCedantId, int? ProductTypeId, string TargetSegments, string DistributionChannels, string UnderwritingMethods,
            string ProductIdName, string QuotationName)
        {
            var bos = TreatyPricingCustomOtherProductService.GetProductBySearchParams(ParentId, TreatyPricingCedantId, ProductTypeId, TargetSegments,
                DistributionChannels, UnderwritingMethods, ProductIdName, QuotationName);

            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult Add(int ParentId, List<int> TreatyPricingProductIds)
        {
            var errors = new List<string>();
            if (TreatyPricingProductIds == null || TreatyPricingProductIds.Count() == 0)
            {
                errors.Add("No Products Selected");
                return Json(new { errors });
            }

            foreach (int productId in TreatyPricingProductIds)
            {
                if (TreatyPricingCustomOtherProductService.IsExists(ParentId, productId))
                    continue;

                var bo = new TreatyPricingCustomOtherProductBo()
                {
                    TreatyPricingCustomOtherId = ParentId,
                    TreatyPricingProductId = productId,
                };

                TreatyPricingCustomOtherProductService.Create(ref bo);
            }

            var bos = TreatyPricingCustomOtherProductService.GetByTreatyPricingCustomOtherId(ParentId);

            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult Remove(int ParentId, int ProductId)
        {
            var bo = TreatyPricingCustomOtherProductService.Find(ParentId, ProductId);
            if (bo != null)
            {
                TreatyPricingCustomOtherProductService.Delete(bo);
            }

            var bos = TreatyPricingCustomOtherProductService.GetByTreatyPricingCustomOtherId(ParentId);

            return Json(new { bos });
        }
    }
}