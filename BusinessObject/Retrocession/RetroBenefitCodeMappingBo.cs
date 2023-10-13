using System;

namespace BusinessObject.Retrocession
{
    public class RetroBenefitCodeMappingBo
    {
        public int Id { get; set; }

        public int BenefitId { get; set; }

        public BenefitBo BenefitBo { get; set; }

        public bool IsPerAnnum { get; set; }

        public string TreatyCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
