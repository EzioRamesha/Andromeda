using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataBatchStatusFileBo
    {
        public int Id { get; set; }

        public int ClaimDataBatchId { get; set; }

        public virtual ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int StatusHistoryId { get; set; }

        public virtual StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string GetProcessFilePath()
        {
            return string.Format("{0}/ClaimDataBatch.{1}.process.summary.log.txt", Util.GetLogPath("ClaimDataBatchSummary"), Id);
        }

        public string GetReportClaimFilePath()
        {
            return string.Format("{0}/ClaimDataBatch.{1}.reporting-claim.summary.log.txt", Util.GetLogPath("ClaimDataBatchSummary"), Id);
        }

        public string GetFilePath()
        {
            if (StatusHistoryBo != null)
            {
                switch (StatusHistoryBo.Status)
                {
                    case ClaimDataBatchBo.StatusProcessing:
                        return GetProcessFilePath();
                    case ClaimDataBatchBo.StatusReportingClaim:
                        return GetReportClaimFilePath();
                }
            }
            return null;
        }
    }
}
