using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FacMasterListingDetailBo
    {
        public int Id { get; set; }

        public int FacMasterListingId { get; set; }

        public FacMasterListingBo FacMasterListingBo { get; set; }

        public string PolicyNumber { get; set; }

        public string BenefitCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public FacMasterListingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
