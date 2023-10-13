using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ReferralRiDataFileBo
    {
        public int Id { get; set; }

        public int RawFileId { get; set; }

        public virtual RawFileBo RawFileBo { get; set; }

        public int Records { get; set; }

        public int UpdatedRecords { get; set; }

        public string Error { get; set; }

        public List<string> Errors { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public int? UpdatedById { get; set; }

        public ReferralRiDataFileBo()
        {
            Records = 0;
            UpdatedRecords = 0;
        }
    }
}
