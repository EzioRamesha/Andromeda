using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionFormatNameBo
    {
        public int Id { get; set; }

        public int SanctionId { get; set; }

        public SanctionBo SanctionBo { get; set; }

        public int SanctionNameId { get; set; }

        public SanctionNameBo SanctionNameBo { get; set; }

        public int Type { get; set; }

        public int? TypeIndex { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public const int TypeSymbolRemoval = 1;
        public const int TypeKeywordReplacement = 2;
        public const int TypeGroupKeyword = 3;
        public const int TypeGroupKeywordReplacement = 4;
        public const int TypeMax = 4;
    }
}
