using BusinessObject.TreatyPricing;
using Newtonsoft.Json;
using Services.TreatyPricing;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingTierProfitCommissionController : BaseController
    {
        public static void Save(string json, int parentId, int? profitSharing, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingTierProfitCommissionBo> bos = new List<TreatyPricingTierProfitCommissionBo>();
            List<int> savedIds = new List<int>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingTierProfitCommissionBo>>(json);

            if (profitSharing.HasValue && profitSharing == TreatyPricingProfitCommissionVersionBo.ProfitSharingMax)
            {
                foreach (var detailBo in bos)
                {
                    var bo = detailBo;
                    if (resetId)
                        bo.Id = 0;

                    if (string.IsNullOrEmpty(bo.Col1) && string.IsNullOrEmpty(bo.Col2))
                        continue;

                    bo.TreatyPricingProfitCommissionVersionId = parentId;
                    bo.UpdatedById = authUserId;

                    if (bo.Id == 0)
                        bo.CreatedById = authUserId;

                    TreatyPricingTierProfitCommissionService.Save(ref bo, ref trail);

                    savedIds.Add(bo.Id);
                }
            }
            

            TreatyPricingTierProfitCommissionService.DeleteByParentExcept(parentId, savedIds, ref trail);
        }
    }
}