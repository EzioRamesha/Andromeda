using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PublicHolidayBo
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
