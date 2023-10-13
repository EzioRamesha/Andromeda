using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AccountCodeBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int ReportingType { get; set; }

        public string Description { get; set; }

        public int? Type { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;

        public const int ReportingTypeMax = 2;

        public const int TypeDaa = 1;
        public const int TypeRetro = 2;

        public const int TypeMax = 2;

        public static string GetReportingTypeName(int key)
        {
            switch (key)
            {
                case ReportingTypeIFRS17:
                    return "IFRS17";
                case ReportingTypeIFRS4:
                    return "IFRS4";
                default:
                    return "";
            }
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeDaa:
                    return "DA&A";
                case TypeRetro:
                    return "Retro";
                default:
                    return "";
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
                return string.Format("{0} - {1}", Code, GetReportingTypeName(ReportingType));
            return string.Format("{0} - {1} - {2}", Code, GetReportingTypeName(ReportingType), Description);
        }
    }
}
