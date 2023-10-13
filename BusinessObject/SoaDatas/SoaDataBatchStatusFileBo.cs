using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.SoaDatas
{
    public class SoaDataBatchStatusFileBo
    {
        public int Id { get; set; }

        public int SoaDataBatchId { get; set; }

        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int StatusHistoryId { get; set; }

        public virtual StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
