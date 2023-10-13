using System;

namespace BusinessObject.Retrocession
{
    public class RetroBenefitCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int RetroBenefitCodeMappingId { get; set; }

        public RetroBenefitCodeMappingBo RetroBenefitCodeMappingBo { get; set; }

        public int RetroBenefitCodeId { get; set; }

        public RetroBenefitCodeBo RetroBenefitCodeBo { get; set; }

        public bool IsComputePremium { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
