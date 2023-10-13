using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingFinancialTableVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingFinancialTableId { get; set; }

        public virtual TreatyPricingFinancialTableBo TreatyPricingFinancialTableBo { get; set; }

        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public string Remarks { get; set; }

        public string AggregationNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string EffectiveAtStr { get; set; }
        public string PersonInChargeName { get; set; }

        public TreatyPricingFinancialTableVersionBo()
        {

        }

        public TreatyPricingFinancialTableVersionBo(TreatyPricingFinancialTableVersionBo bo)
        {
            TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId;
            PersonInChargeId = bo.PersonInChargeId;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            Remarks = bo.Remarks;
            AggregationNote = bo.AggregationNote;
        }
    }
}
