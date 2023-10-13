using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionKeywordDetailBo
    {
        public int Id { get; set; }

        public int SanctionKeywordId { get; set; }

        public SanctionKeywordBo SanctionKeywordBo { get; set; }

        public string Keyword { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
