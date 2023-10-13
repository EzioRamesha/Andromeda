using Shared;
using System;

namespace BusinessObject
{
    public class CutOffBo
    {
        public int Id { get; set; }

        public int Status { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
        public DateTime? CutOffDateTime { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public const int StatusInitiated = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusCompleted = 4;
        public const int StatusToRecover = 5;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusInitiated:
                    return "Initiated";
                case StatusSubmitForProcessing:
                    return "Submit for Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusCompleted:
                    return "Completed";
                case StatusToRecover:
                    return "To Recover";
                default:
                    return "";
            }
        }

        public string GetQuarterWithDate()
        {
            var dateStr = CutOffDateTime?.ToString(Util.GetDateFormat());
            if (dateStr == null)
                return Quarter;

            return string.Format("{0} - {1}", Quarter, dateStr);
        }
    }
}
