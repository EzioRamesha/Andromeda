using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionNameBo
    {
        public int Id { get; set; }

        public int SanctionId { get; set; }

        public SanctionBo SanctionBo { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
