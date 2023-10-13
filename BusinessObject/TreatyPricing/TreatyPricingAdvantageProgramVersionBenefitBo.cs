using Shared;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingAdvantageProgramVersionBenefitBo
    {
        public int Id { get; set; }

        public int TreatyPricingAdvantageProgramVersionId { get; set; }
        public TreatyPricingAdvantageProgramVersionBo TreatyPricingAdvantageProgramVersionBo { get; set; }

        public int BenefitId { get; set; }
        public BenefitBo BenefitBo { get; set; }

        public double? ExtraMortality { get; set; }

        public double? SumAssured { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string ExtraMortalityStr { get; set; }
        public string SumAssuredStr { get; set; }
        public string BenefitCode { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (string.IsNullOrEmpty(ExtraMortalityStr) || string.IsNullOrWhiteSpace(ExtraMortalityStr))
                errors.Add(string.Format(MessageBag.Required, "Extra Mortality"));
            else if (!Util.IsValidDouble(ExtraMortalityStr, out double? d, out _))
                errors.Add(string.Format("Invalid Amount Input: {0}", ExtraMortalityStr));

            if (string.IsNullOrEmpty(SumAssuredStr) || string.IsNullOrWhiteSpace(SumAssuredStr))
                errors.Add(string.Format(MessageBag.Required, "Sum Assured Not Exceeding"));
            else if (!Util.IsValidDouble(SumAssuredStr, out double? d, out _))
                errors.Add(string.Format("Invalid Amount Input: {0}", SumAssuredStr));

            return errors;
        }
    }
}
