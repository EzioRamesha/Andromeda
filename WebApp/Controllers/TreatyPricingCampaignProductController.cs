using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingCampaignProductController : BaseController
    {
        [HttpPost]
        public JsonResult Search(int ParentId, int? TreatyPricingCedantId, int? ProductTypeId, string TargetSegments, string DistributionChannels, string UnderwritingMethods,
            string ProductIdName, string QuotationName)
        {
            var bos = TreatyPricingCampaignProductService.GetProductBySearchParams(ParentId, TreatyPricingCedantId, ProductTypeId, TargetSegments,
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
                if (TreatyPricingCampaignProductService.IsExists(ParentId, productId))
                    continue;

                var bo = new TreatyPricingCampaignProductBo()
                {
                    TreatyPricingCampaignId = ParentId,
                    TreatyPricingProductId = productId,
                };

                TreatyPricingCampaignProductService.Create(ref bo);
            }

            var bos = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(ParentId);

            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult Remove(int ParentId, int ProductId)
        {
            var bo = TreatyPricingCampaignProductService.Find(ParentId, ProductId);
            if (bo != null)
            {
                TreatyPricingCampaignProductService.Delete(bo);
            }

            var bos = TreatyPricingCampaignProductService.GetByTreatyPricingCampaignId(ParentId);

            return Json(new { bos });
        }
    }
}