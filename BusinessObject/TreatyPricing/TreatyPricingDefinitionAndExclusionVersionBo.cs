using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingDefinitionAndExclusionVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingDefinitionAndExclusionId { get; set; }

        public virtual TreatyPricingDefinitionAndExclusionBo TreatyPricingDefinitionAndExclusionBo { get; set; }

        public int Version { get; set; }

        public string VersionStr { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public string EffectiveAtStr { get; set; }

        public string Definitions { get; set; }
        
        public string Exclusions { get; set; }

        public string DeclinedRisk { get; set; }

        public string ReferredRisk { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingDefinitionAndExclusionVersionBo()
        {

        }

        public TreatyPricingDefinitionAndExclusionVersionBo(TreatyPricingDefinitionAndExclusionVersionBo bo)
        {
            TreatyPricingDefinitionAndExclusionId = bo.TreatyPricingDefinitionAndExclusionId;
            PersonInChargeId = bo.PersonInChargeId;
            PersonInChargeName = bo.PersonInChargeName;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            Definitions = bo.Definitions;
            Exclusions = bo.Exclusions;
            DeclinedRisk = bo.DeclinedRisk;
            ReferredRisk = bo.ReferredRisk;
        }
    }
}
