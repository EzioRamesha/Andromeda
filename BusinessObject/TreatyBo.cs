using System;

namespace BusinessObject
{
    public class TreatyBo
    {
        public int Id { get; set; }

        public string TreatyIdCode { get; set; }

        public int CedantId { get; set; }

        public string Description { get; set; }       

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        // Phase 2
        public int? BusinessOriginPickListDetailId { get; set; }

        public string BlockDescription { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public string ToString(bool withDescription = true)
        {
            if (string.IsNullOrEmpty(Description) || !withDescription)
            {
                return TreatyIdCode;
            }
            return string.Format("{0} - {1}", TreatyIdCode, Description);
        }
    }
}
