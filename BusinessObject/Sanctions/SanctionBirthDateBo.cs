using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionBirthDateBo
    {
        public int Id { get; set; }

        public int SanctionId { get; set; }

        public SanctionBo SanctionBo { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthStr { get; set; }

        public int? YearOfBirth { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
