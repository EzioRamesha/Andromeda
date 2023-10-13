using Shared;

namespace BusinessObject
{
    public class RetroRegisterBatchStatusFileBo
    {
        public int Id { get; set; }

        public int RetroRegisterBatchId { get; set; }

        public RetroRegisterBatchBo RetroRegisterBatchBo { get; set; }

        public int StatusHistoryId { get; set; }

        public StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string GetProcessFilePath()
        {
            return string.Format("{0}/RetroRegisterBatch.{1}.process.summary.log.txt", Util.GetLogPath("RetroRegisterBatchSummary"), Id);
        }

        public string GetFilePath()
        {
            if (StatusHistoryBo != null)
            {
                switch (StatusHistoryBo.Status)
                {
                    case RetroRegisterBatchBo.StatusProcessing:
                    case RetroRegisterBatchBo.StatusGenerating:
                        return GetProcessFilePath();
                }
            }
            return null;
        }
    }
}
