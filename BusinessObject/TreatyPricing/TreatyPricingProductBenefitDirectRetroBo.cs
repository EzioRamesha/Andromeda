using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductBenefitDirectRetroBo
    {
        public int Id { get; set; }

        public int TreatyPricingProductBenefitId { get; set; }
        public virtual TreatyPricingProductBenefitBo TreatyPricingProductBenefitBo { get; set; }

        public int RetroPartyId { get; set; }
        public virtual RetroPartyBo RetroPartyBo { get; set; }
        public string RetroPartyCode { get; set; }

        public int? ArrangementRetrocessionTypePickListDetailId { get; set; }
        public virtual PickListDetailBo ArrangementRetrocessionTypePickListDetailBo { get; set; }

        public string MlreRetention { get; set; }

        public string RetrocessionShare { get; set; }

        public bool IsRetrocessionProfitCommission { get; set; }

        public bool IsRetrocessionAdvantageProgram { get; set; }

        public string RetrocessionRateTable { get; set; }

        public string NewBusinessRateGuarantee { get; set; }

        public string RenewalBusinessRateGuarantee { get; set; }

        public string RetrocessionDiscount { get; set; }

        public string AdditionalDiscount { get; set; }

        public string AdditionalLoading { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        #region Product & Benefit Comparison
        public string ArrangementRetrocessionType { get; set; }

        public string IsRetrocessionProfitCommissionStr { get; set; }

        public string IsRetrocessionAdvantageProgramStr { get; set; }
        #endregion
    }
}
