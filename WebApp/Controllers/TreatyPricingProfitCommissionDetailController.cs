using BusinessObject.TreatyPricing;
using Newtonsoft.Json;
using Services.TreatyPricing;
using Shared.Trails;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class TreatyPricingProfitCommissionDetailController : BaseController
    {
        public static void Save(string json, int parentId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingProfitCommissionDetailBo> bos = new List<TreatyPricingProfitCommissionDetailBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingProfitCommissionDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingProfitCommissionVersionId = parentId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                TreatyPricingProfitCommissionDetailService.Save(ref bo, ref trail);
            }
        }
    }
}