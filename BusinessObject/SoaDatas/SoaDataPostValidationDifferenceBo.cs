using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.SoaDatas
{
    public class SoaDataPostValidationDifferenceBo
    {
        public int Id { get; set; }


        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }


        public int Type { get; set; }


        public string BusinessCode { get; set; }
        public string TreatyCode { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }
        public int? RiskMonth { get; set; }


        public double? GrossPremium { get; set; } = 0;
        public double? DifferenceNetTotalAmount { get; set; } = 0;
        public double? DifferencePercetage { get; set; } = 0;

        public string Remark { get; set; }
        public string Check { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }


        public const int TypeMlreShare = 1;
        public const int TypeLayer1Share = 2;
        public const int TypeRetakaful = 3;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeMlreShare:
                    return "MLRe's Share Difference";
                case TypeLayer1Share:
                    return "Layer1's Share Difference";
                case TypeRetakaful:
                    return "Retakaful Difference";
                default:
                    return "";
            }
        }

        public double? GetDifferencePercentage()
        {
            if (GrossPremium == 0)
                return 0;
            if (DifferenceNetTotalAmount == 0)
                return 0;

            DifferencePercetage = (DifferenceNetTotalAmount.GetValueOrDefault() / GrossPremium.GetValueOrDefault()) * 100;
            return DifferencePercetage;
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Soa Qtr",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "Gross Premium",
                    Property = "GrossPremium",
                },
                new Column
                {
                    Header = "Net Difference",
                    Property = "DifferenceNetTotalAmount",
                },
                 new Column
                {
                    Header = "Percentage Difference",
                    Property = "DifferencePercetage",
                },
                new Column
                {
                    Header = "Remark",
                    Property = "Remark",
                },
                new Column
                {
                    Header = "Check",
                    Property = "Check",
                },
            };
            return columns;
        }
    }
}
