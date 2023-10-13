using System;
namespace BusinessObject.Retrocession
{
    public class GstMaintenanceBo
    {
        public int Id { get; set; }

        public DateTime? EffectiveStartDate { get; set; }
        public string EffectiveStartDateStr { get; set; }

        public DateTime? EffectiveEndDate { get; set; }
        public string EffectiveEndDateStr { get; set; }

        public DateTime? RiskEffectiveStartDate { get; set; }
        public string RiskEffectiveStartDateStr { get; set; }

        public DateTime? RiskEffectiveEndDate { get; set; }
        public string RiskEffectiveEndDateStr { get; set; }

        public double Rate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

    }
}
