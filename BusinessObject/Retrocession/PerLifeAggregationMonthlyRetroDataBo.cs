using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationMonthlyRetroDataBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationMonthlyDataId { get; set; }

        public PerLifeAggregationMonthlyDataBo PerLifeAggregationMonthlyDataBo { get; set; }

        public string RetroParty { get; set; }

        public double? RetroAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public double? RetroGst { get; set; }

        public double? MlreShare { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
