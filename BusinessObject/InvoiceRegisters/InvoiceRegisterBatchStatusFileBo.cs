using Shared;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBatchStatusFileBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchId { get; set; }

        public InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        public int StatusHistoryId { get; set; }

        public StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string GetProcessFilePath()
        {
            return string.Format("{0}/InvoiceRegisterBatch.{1}.process.summary.log.txt", Util.GetLogPath("InvoiceRegisterBatchSummary"), Id);
        }

        public string GetFilePath()
        {
            if (StatusHistoryBo != null)
            {
                switch (StatusHistoryBo.Status)
                {
                    case InvoiceRegisterBatchBo.StatusProcessing:
                    case InvoiceRegisterBatchBo.StatusGenerating:
                        return GetProcessFilePath();
                }
            }
            return null;
        }
    }
}
