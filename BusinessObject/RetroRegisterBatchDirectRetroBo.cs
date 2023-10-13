namespace BusinessObject
{
    public class RetroRegisterBatchDirectRetroBo
    {
        public int RetroRegisterBatchId { get; set; }

        public virtual RetroRegisterBatchBo RetroRegisterBatchBo { get; set; }

        public int DirectRetroId { get; set; }

        public virtual DirectRetroBo DirectRetroBo { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}", RetroRegisterBatchId, DirectRetroId);
        }
    }
}
