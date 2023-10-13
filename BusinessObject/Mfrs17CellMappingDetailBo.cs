using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Mfrs17CellMappingDetailBo
    {
        public int Id { get; set; }

        public int Mfrs17CellMappingId { get; set; }

        public virtual Mfrs17CellMappingBo Mfrs17CellMappingBo { get; set; }

        public string Combination { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public string TreatyCode { get; set; }

        public Mfrs17CellMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
