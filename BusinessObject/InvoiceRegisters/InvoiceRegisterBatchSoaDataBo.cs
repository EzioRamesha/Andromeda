using BusinessObject.SoaDatas;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBatchSoaDataBo
    {
        public int InvoiceRegisterBatchId { get; set; }

        public virtual InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        public int SoaDataBatchId { get; set; }

        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}", InvoiceRegisterBatchId, SoaDataBatchId);
        }
    }
}