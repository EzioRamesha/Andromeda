using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TreatyDiscountTableBo
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

        public double? AARFrom { get; set; }

        public double? AARTo { get; set; }

        public double Discount { get; set; }

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
        public const int ColumnAarFrom = 11;
        public const int ColumnAarTo = 12;
        public const int ColumnDiscount = 13;
        public const int ColumnDetailAction = 14;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Treaty Discount Table ID",
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
                    Header = "Type",
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
                    Header = "Action (Treaty Discount Table)",
                    ColIndex = ColumnAction,
                },
                new Column
                {
                    Header = "Treaty Discount Table Detail ID",
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
                    Header = "AAR From",
                    ColIndex = ColumnAarFrom,
                    Property = "AARFrom",
                },
                new Column
                {
                    Header = "AAR To",
                    ColIndex = ColumnAarTo,
                    Property = "AARTo",
                },
                new Column
                {
                    Header = "Treaty Discount",
                    ColIndex = ColumnDiscount,
                    Property = "Discount",
                },
                new Column
                {
                    Header = "Action (Treaty Discount Table Detail)",
                    ColIndex = ColumnDetailAction,
                },
            };
        }
    }
}
