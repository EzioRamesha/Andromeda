using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingMedicalTableVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingMedicalTableId { get; set; }

        public virtual TreatyPricingMedicalTableBo TreatyPricingMedicalTableBo { get; set; }

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

        public TreatyPricingMedicalTableVersionBo()
        {

        }

        public TreatyPricingMedicalTableVersionBo(TreatyPricingMedicalTableVersionBo bo)
        {
            TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId;
            PersonInChargeId = bo.PersonInChargeId;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            Remarks = bo.Remarks;
            AggregationNote = bo.AggregationNote;
        }
    }
}
