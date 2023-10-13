using System;

namespace BusinessObject
{
    public class AccountCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int AccountCodeMappingId { get; set; }

        public AccountCodeMappingBo AccountCodeMappingBo { get; set; }

        public string TreatyType { get; set; }

        public string ClaimCode { get; set; }

        public string BusinessOrigin { get; set; }

        public string InvoiceField { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public AccountCodeMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
