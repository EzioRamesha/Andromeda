using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingProductPerLifeRetroController : BaseController
    {
        public static void Save(bool hasPerLifeRetro, int productId, string perLifeRetroTreatyCodes, int authUserId, ref TrailObject trail)
        {
            List<string> savedCodes = new List<string>();

            if (hasPerLifeRetro)
            {
                if (string.IsNullOrEmpty(perLifeRetroTreatyCodes))
                    perLifeRetroTreatyCodes = "";

                string[] codes = perLifeRetroTreatyCodes.Split(',');
                foreach (string code in codes)
                {
                    if (!TreatyPricingProductPerLifeRetroService.IsExists(productId, code))
                    {
                        TreatyPricingPerLifeRetroBo perLifeRetroBo = TreatyPricingPerLifeRetroService.FindByCode(code);
                        if (perLifeRetroBo == null)
                            continue;

                        var bo = new TreatyPricingProductPerLifeRetroBo()
                        {
                            TreatyPricingProductId = productId,
                            TreatyPricingPerLifeRetroId = perLifeRetroBo.Id,
                            CreatedById = authUserId
                        };

                        TreatyPricingProductPerLifeRetroService.Create(ref bo, ref trail);
                    }

                    savedCodes.Add(code);
                }
            }

            TreatyPricingProductPerLifeRetroService.DeleteByProductIdExcept(productId, savedCodes, ref trail);
        }

        [HttpPost]
        public JsonResult Search(int ParentId, int? TreatyPricingCedantId, int? ProductTypeId, string TargetSegments, string DistributionChannels, string UnderwritingMethods,
            string ProductIdName, string QuotationName)
        {
            var bos = TreatyPricingProductPerLifeRetroService.GetProductBySearchParams(ParentId, TreatyPricingCedantId, ProductTypeId, TargetSegments, 
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
                if (TreatyPricingProductPerLifeRetroService.IsExists(productId, ParentId))
                    continue;

                var trail = GetNewTrailObject();

                var bo = new TreatyPricingProductPerLifeRetroBo()
                {
                    TreatyPricingProductId = productId,
                    TreatyPricingPerLifeRetroId = ParentId,
                    CreatedById = AuthUserId
                };

                Result = TreatyPricingProductPerLifeRetroService.Create(ref bo, ref trail);
                if (Result.Valid)
                {
                    var productBo = TreatyPricingProductService.Find(productId);
                    if (productBo.HasPerLifeRetro == false)
                    {
                        productBo.HasPerLifeRetro = true;
                        productBo.UpdatedById = AuthUserId;
                        TreatyPricingProductService.Update(ref productBo, ref trail);
                    }

                    CreateTrail(bo.Id, "Link Product to Per Life Retro");
                }
            }

            var bos = TreatyPricingProductPerLifeRetroService.GetByPerLifeRetroId(ParentId);

            return Json(new { bos });
        }

        [HttpPost]
        public JsonResult Remove(int ParentId, int ProductId)
        {
            var bo = TreatyPricingProductPerLifeRetroService.Find(ProductId, ParentId);
            if (bo != null)
            {
                var trail = GetNewTrailObject();

                Result = TreatyPricingProductPerLifeRetroService.Delete(bo, ref trail);
                if (Result.Valid)
                {
                    CreateTrail(bo.Id, "Unlink Product from Per Life Retro");
                }
            }

            var bos = TreatyPricingProductPerLifeRetroService.GetByPerLifeRetroId(ParentId);

            return Json(new { bos });
        }
    }
}