using BusinessObject.TreatyPricing;
using Newtonsoft.Json;
using Services.TreatyPricing;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Controllers
{
    public class TreatyPricingProductDetailController : BaseController
    {
        public static void Save(string json, int parentId, int type, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingProductDetailBo> bos = new List<TreatyPricingProductDetailBo>();
            List<int> savedIds = new List<int>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingProductDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingProductVersionId = parentId;
                bo.Type = type;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                TreatyPricingProductDetailService.Save(ref bo, ref trail);

                savedIds.Add(bo.Id);
            }

            TreatyPricingProductDetailService.DeleteByParentExcept(parentId, type, savedIds, ref trail);
        }
    }
}