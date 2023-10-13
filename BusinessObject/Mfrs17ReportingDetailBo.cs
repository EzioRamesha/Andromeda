using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Mfrs17ReportingDetailBo
    {
        public int Id { get; set; }

        public int Mfrs17ReportingId { get; set; }

        public virtual Mfrs17ReportingBo Mfrs17ReportingBo { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public string TreatyCode { get; set; }

        public int PremiumFrequencyCodePickListDetailId { get; set; }

        public virtual PickListDetailBo PremiumFrequencyCodePickListDetailBo { get; set; }

        public string RiskQuarter { get; set; }

        public DateTime LatestDataStartDate { get; set; }

        public DateTime LatestDataEndDate { get; set; }

        public string LatestDataStartDateStr { get; set; }

        public string LatestDataEndDateStr { get; set; }

        public int Record { get; set; }

        public string Mfrs17TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public int? Status { get; set; }

        public bool IsModified { get; set; }

        public IList<int> RiDataIds { get; set; }

        public int? GenerateStatus { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusProcessed = 2;
        public const int StatusReprocess = 3;
        public const int StatusPendingDelete = 4;
        public const int StatusDeleted = 5;
        public const int StatusMax = 5;

        public const int GenerateStatusPending = 1;
        public const int GenerateStatusSuccess = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusProcessed:
                    return "Processed";
                case StatusReprocess:
                    return "Reprocess";
                case StatusPendingDelete:
                    return "Pending Delete";
                case StatusDeleted:
                    return "Deleted";
                default:
                    return "";
            }
        }
    }
}
