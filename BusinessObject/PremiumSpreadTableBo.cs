using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject
{
    public class PremiumSpreadTableBo
    {
        public int Id { get; set; }

        public string Rule { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Detail
        public int DetailId { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public int? AgeFrom { get; set; }

        public int? AgeTo { get; set; }

        public double PremiumSpread { get; set; }

        public const int TypeDirectRetro = 1;
        public const int TypePerLife = 2;

        public const string TypeDirectRetroName = "Direct Retro";
        public const string TypePerLifeName = "Per Life";

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeDirectRetro:
                    return TypeDirectRetroName;
                case TypePerLife:
                    return TypePerLifeName;
                default:
                    return "";
            }
        }

        public static int GetTypeKey(string name)
        {
            switch (name)
            {
                case TypeDirectRetroName:
                    return TypeDirectRetro;
                case TypePerLifeName:
                    return TypePerLife;
                default:
                    return 0;
            }
        }

        public const int ColumnId = 1;
        public const int ColumnRule = 2;
        public const int ColumnType = 3;
        public const int ColumnDescription = 4;
        public const int ColumnAction = 5;
        public const int ColumnDetailId = 6;
        public const int ColumnCedingPlanCode = 7;
        public const int ColumnBenefitCode = 8;
        public const int ColumnAgeFrom = 9;
        public const int ColumnAgeTo = 10;
        public const int ColumnPremiumSpread = 11;
        public const int ColumnDetailAction = 12;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Premium Spread Table ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Rule",
                    ColIndex = ColumnRule,
                    Property = "Rule",
                },
                new Column
                {
                    Header = "Type (Direct Retro/Per Life)",
                    ColIndex = ColumnType,
                    Property = "Type",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                    Property = "Description",
                },
                new Column
                {
                    Header = "Action (Premium Spread Table)",
                    ColIndex = ColumnAction,
                },
                new Column
                {
                    Header = "Premium Spread Table Detail ID",
                    ColIndex = ColumnDetailId,
                    Property = "DetailId",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    ColIndex = ColumnCedingPlanCode,
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Age From",
                    ColIndex = ColumnAgeFrom,
                    Property = "AgeFrom",
                },
                new Column
                {
                    Header = "Age To",
                    ColIndex = ColumnAgeTo,
                    Property = "AgeTo",
                },
                new Column
                {
                    Header = "Premium Spread",
                    ColIndex = ColumnPremiumSpread,
                    Property = "PremiumSpread",
                },
                new Column
                {
                    Header = "Action (Premium Spread Table Detail)",
                    ColIndex = ColumnDetailAction,
                },
            };
        }
    }
}
