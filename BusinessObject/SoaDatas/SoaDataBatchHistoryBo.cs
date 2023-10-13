using System;

namespace BusinessObject.SoaDatas
{
    public class SoaDataBatchHistoryBo
    {
        public int Id { get; set; }

        public int CutOffId { get; set; }
        public virtual CutOffBo CutOff { get; set; }

        public int SoaDataBatchId { get; set; }
        public int CedantId { get; set; }
        public virtual CedantBo CedantBo { get; set; }
        public int? TreatyId { get; set; }

        public int? CurrencyCodePickListDetailId { get; set; }

        public double? CurrencyRate { get; set; }

        public int Status { get; set; }

        public int DataUpdateStatus { get; set; }

        public string Quarter { get; set; }

        public int Type { get; set; }

        public DateTime StatementReceivedAt { get; set; }

        public int? RiDataBatchId { get; set; }

        public int? ClaimDataBatchId { get; set; }

        public int DirectStatus { get; set; }
        public int InvoiceStatus { get; set; }

        public int TotalRecords { get; set; }
        public int TotalMappingFailedStatus { get; set; }

        public bool IsRiDataAutoCreate { get; set; } = false;

        public bool IsClaimDataAutoCreate { get; set; } = false;

        public bool IsProfitCommissionData { get; set; } = false;

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
