using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Mfrs17ContractCodeDetailBo
    {
        public int Id { get; set; }

        public int Mfrs17ContractCodeId { get; set; }
        public Mfrs17ContractCodeBo Mfrs17ContractCodeBo { get; set; }

        public string ContractCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
