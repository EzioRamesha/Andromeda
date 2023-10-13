using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionBenefitBo
    {
        public int Id { get; set; }

        public int TreatyPricingPerLifeRetroVersionId { get; set; }
        //public TreatyPricingPerLifeRetroVersionBo TreatyPricingPerLifeRetroVersionBo { get; set; }

        public int BenefitId { get; set; }
        public BenefitBo BenefitBo { get; set; }
        public string BenefitCode { get; set; }
        public string BenefitName { get; set; }

        public int? ArrangementRetrocessionnaireTypePickListDetailId { get; set; }
        public virtual PickListDetailBo ArrangementRetrocessionnaireTypePickListDetailBo { get; set; }

        public string TotalMortality { get; set; }

        public string MlreRetention { get; set; }

        public string RetrocessionnaireShare { get; set; }

        public int? AgeBasisPickListDetailId { get; set; }
        public virtual PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public string MaxExpiryAge { get; set; }

        public string RetrocessionaireDiscount { get; set; }

        public string RateTablePercentage { get; set; }

        public string ClaimApprovalLimit { get; set; }

        public string AutoBindingLimit { get; set; }

        public bool IsProfitCommission { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public static List<string> Validate(string benefit)
        {
            List<string> errors = new List<string> { };

            if (string.IsNullOrEmpty(benefit))
                return null;

            List<TreatyPricingPerLifeRetroVersionBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingPerLifeRetroVersionBenefitBo>>(benefit);
            foreach(var bo in bos)
            {
                //if (!string.IsNullOrEmpty(bo.RetrocessionnaireShareStr) && !Util.IsValidDouble(bo.RetrocessionnaireShareStr, out double?  d, out _))
                //    errors.Add(string.Format("Retrocessionaire's Share invalid amount input: {0}", bo.RetrocessionnaireShareStr));
            }
            return errors;
        }
    }
}
