namespace BusinessObject.Retrocession
{
    public class PerLifeSoaSummariesSoaBo
    {
        public int Id { get; set; }

        public int PerLifeSoaId { get; set; }
        public PerLifeSoaBo PerLifeSoaBo { get; set; }

        public int PremiumClaim { get; set; }

        public string RowLabel { get; set; }

        public double? Individual { get; set; }

        public double? Group { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public double? Total { get; set; }

        public string IndividualStr { get; set; }
        public string GroupStr { get; set; }
        public string TotalStr { get; set; }

        public double? PrevIndividual { get; set; }
        public double? PrevGroup { get; set; }
        public double? PrevTotal { get; set; }
        public string PrevIndividualStr { get; set; }
        public string PrevGroupStr { get; set; }
        public string PrevTotalStr { get; set; }

        public double? Movement { get; set; }
        public string MovementStr { get; set; }


        public const int PremiumClaimPremium = 1;
        public const int PremiumClaimClaim = 2;
        public const int PremiumClaimPC = 3;
        public const int PremiumClaimMax = 3;
    }
}
