namespace BusinessObject.Retrocession
{
    public class PerLifeSoaSummariesByTreatyBo
    {
        public int Id { get; set; }

        public int PerLifeSoaId { get; set; }
        public PerLifeSoaBo PerLifeSoaBo { get; set; }

        public string TreatyCode { get; set; }

        public string ProcessingPeriod { get; set; }

        public double? TotalRetroAmount { get; set; }

        public double? TotalGrossPremium { get; set; }

        public double? TotalNetPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? TotalPolicyCount { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }


        public string TotalRetroAmountStr { get; set; }
        public string TotalGrossPremiumStr { get; set; }
        public string TotalNetPremiumStr { get; set; }
        public string TotalDiscountStr { get; set; }
        public string ProcessingPeriodYear { get; set; }

        // Previous Quarter
        public double? PrevTotalRetroAmount { get; set; }
        public double? PrevTotalGrossPremium { get; set; }
        public double? PrevTotalNetPremium { get; set; }
        public double? PrevTotalDiscount { get; set; }
        public double? PrevTotalPolicyCount { get; set; }
        public string PrevTotalRetroAmountStr { get; set; }
        public string PrevTotalGrossPremiumStr { get; set; }
        public string PrevTotalNetPremiumStr { get; set; }
        public string PrevTotalDiscountStr { get; set; }

        // Movement
        public double? MovementTotalRetroAmount { get; set; }
        public double? MovementTotalGrossPremium { get; set; }
        public double? MovementTotalNetPremium { get; set; }
        public double? MovementTotalDiscount { get; set; }
        public double? MovementTotalPolicyCount { get; set; }
        public string MovementTotalRetroAmountStr { get; set; }
        public string MovementTotalGrossPremiumStr { get; set; }
        public string MovementTotalNetPremiumStr { get; set; }
        public string MovementTotalDiscountStr { get; set; }
    }
}
