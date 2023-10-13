using Shared;

namespace BusinessObject.RiDatas
{
    public class RiDataBatchStatusFileBo
    {
        public int Id { get; set; }

        public int RiDataBatchId { get; set; }

        public virtual RiDataBatchBo RiDataBatchBo { get; set; }

        public int StatusHistoryId { get; set; }

        public virtual StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string GetProcessFilePath()
        {
            return string.Format("{0}/RiDataBatch.{1}.process.summary.log.txt", Util.GetLogPath("RiDataBatchSummary"), Id);
        }

        // this is to log all computation formula and pre validation condition
        public string GetDebugSummaryFilePath()
        {
            if (StatusHistoryBo != null && (StatusHistoryBo.Status == RiDataBatchBo.StatusPreProcessing || StatusHistoryBo.Status == RiDataBatchBo.StatusPostProcessing))
            {
                return string.Format("{0}/RiDataBatch.{1}.process.debug.summary.log.txt", Util.GetLogPath("RiDataBatchSummary"), Id);
            }
            return null;
        }

        public string GetFinaliseFilePath()
        {
            return string.Format("{0}/RiDataBatch.{1}.finalise.summary.log.txt", Util.GetLogPath("RiDataBatchSummary"), Id);
        }

        public string GetFilePath()
        {
            if (StatusHistoryBo != null)
            {
                switch (StatusHistoryBo.Status)
                {
                    case RiDataBatchBo.StatusPreProcessing:
                    case RiDataBatchBo.StatusPostProcessing:
                        return GetProcessFilePath();
                    case RiDataBatchBo.StatusFinalising:
                        return GetFinaliseFilePath();
                }
            }
            return null;
        }
    }
}
