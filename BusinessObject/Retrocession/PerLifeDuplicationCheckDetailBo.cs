using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeDuplicationCheckDetailBo
    {
        public int Id { get; set; }

        public int PerLifeDuplicationCheckId { get; set; }
        public PerLifeDuplicationCheckBo PerLifeDuplicationCheckBo { get; set; }

        public string TreatyCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public PerLifeDuplicationCheckDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
