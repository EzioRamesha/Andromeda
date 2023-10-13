using BusinessObject.RiDatas;

namespace BusinessObject
{
    public class Mfrs17ReportingDetailRiDataBo
    {
        public int Mfrs17ReportingDetailId { get; set; }

        public virtual Mfrs17ReportingDetailBo Mfrs17ReportingDetailBo { get; set; }

        public int RiDataId { get; set; }

        public int RiDataWarehouseId { get; set; }

        public int RiDataWarehouseHistoryId { get; set; }

        public virtual RiDataWarehouseHistoryBo RiDataWarehouseHistoryBo { get; set; }

        public int CutOffId { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}{2}", Mfrs17ReportingDetailId, RiDataWarehouseId, CutOffId);
        }
    }
}
