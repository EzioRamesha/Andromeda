using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimChecklistDetailBo
    {
        public int Id { get; set; }

        public int ClaimChecklistId { get; set; }

        public ClaimChecklistBo ClaimChecklistBo { get; set; }

        public string Name { get; set; }

        public string NameToLower { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const string RemarkCode = "Remark";
    }
}
