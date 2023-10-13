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
    public class TreatyPricingRateTableDetailController : BaseController
    {
        public static void Save(string json, int parentId, int type, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingRateTableDetailBo> bos = new List<TreatyPricingRateTableDetailBo>();
            List<int> savedIds = new List<int>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingRateTableDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingRateTableVersionId = parentId;
                bo.Type = type;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                TreatyPricingRateTableDetailService.Save(ref bo, ref trail);

                savedIds.Add(bo.Id);
            }

            TreatyPricingRateTableDetailService.DeleteByTreatyPricingRateTableDetailIdExcept(parentId, type, savedIds, ref trail);
        }
    }
}