using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingProductBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string TreatyPricingCedantCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string EffectiveDateStr { get; set; }

        public string Summary { get; set; }

        public string QuotationName { get; set; }

        public string UnderwritingMethod { get; set; }

        public bool HasPerLifeRetro { get; set; }

        public string PerLifeRetroTreatyCode { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingProductId { get; set; }
        public int? DuplicateTreatyPricingProductVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingProductVersionBo> TreatyPricingProductVersionBos { get; set; }

        public TreatyPricingProductVersionBo LatestTreatyPricingProductVersionBo { get; set; }

        public TreatyPricingWorkflowObjectBo LatestWorkflowObjectBo { get; set; }

        #region Product & Benefit Comparison
        //Product section
        public string UnderwritingMethodStr { get; set; }

        public string HasPerLifeRetroStr { get; set; }

        public string FinalDocuments { get; set; }

        public TreatyPricingProductVersionBo ComparisonTreatyPricingProductVersionBo { get; set; }

        public TreatyPricingQuotationWorkflowVersionBo ComparisonTreatyPricingQuotationWorkflowVersionBo { get; set; }

        public int? BDPersonInChargeId { get; set; }

        public string BDPersonInChargeName { get; set; }

        //Product Details section
        public string ProductType { get; set; }

        public string MedicalTableInfo { get; set; }

        public string FinancialTableInfo { get; set; }

        public string UwQuestionnaireInfo { get; set; }

        public string AdvantageProgramInfo { get; set; }

        public string ProfitCommInfo { get; set; }

        public string BusinessOrigin { get; set; }

        public string BusinessType { get; set; }

        public string ReinsuranceArrangement { get; set; }

        public string ReinsurancePremiumPayment { get; set; }

        public string UnearnedPremiumRefund { get; set; }

        public string ResidenceCountry { get; set; }

        //Benefits section
        public IList<TreatyPricingProductBenefitBo> ComparisonTreatyPricingProductBenefitBos { get; set; }
        #endregion

        public TreatyPricingProductBo()
        {
            HasPerLifeRetro = false;
        }
    }
}
