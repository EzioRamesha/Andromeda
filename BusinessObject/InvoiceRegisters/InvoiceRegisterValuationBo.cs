using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterValuationBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterId { get; set; }

        public virtual InvoiceRegisterBo InvoiceRegisterBo { get; set; }

        public int ValuationBenefitCodeId { get; set; }

        public virtual BenefitBo ValuationBenefitCodeBo { get; set; }

        public double? Amount { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
