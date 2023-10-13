using System;

namespace BusinessObject
{
    public class ItemCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int ItemCodeMappingId { get; set; }

        public ItemCodeMappingBo ItemCodeMappingBo { get; set; }

        public string TreatyType { get; set; }

        public string TreatyCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public ItemCodeMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
