using BusinessObject.Retrocession;
using System;

namespace BusinessObject
{
    public class RetroBenefitCodeMappingTreatyBo
    {
        public int Id { get; set; }

        public int RetroBenefitCodeMappingId { get; set; }

        public RetroBenefitCodeMappingBo RetroBenefitCodeMappingBo { get; set; }

        public string TreatyCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
