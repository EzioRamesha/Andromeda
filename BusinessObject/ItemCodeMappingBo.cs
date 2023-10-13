using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject
{
    public class ItemCodeMappingBo
    {
        public int Id { get; set; }

        public int ItemCodeId { get; set; }

        public ItemCodeBo ItemCodeBo { get; set; }

        public string ItemCode { get; set; }

        public int ReportingType { get; set; }

        public int? InvoiceFieldPickListDetailId { get; set; }

        public PickListDetailBo InvoiceFieldPickListDetailBo { get; set; }

        public string InvoiceField{ get; set; }

        public string TreatyType { get; set; }

        public string TreatyCode { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int? BusinessOriginPickListDetailId { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public string BusinessOrigin { get; set; }

        public const int ColumnId = 1;
        public const int ColumnInvoiceField = 2;
        public const int ColumnTreatyType = 3;
        public const int ColumnTreatyCode = 4;
        public const int ColumnBusinessOrigin = 5;
        public const int ColumnItemCode = 6;
        public const int ColumnReportingType = 7;
        public const int ColumnAction = 8;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Invoice Field",
                    ColIndex = ColumnInvoiceField,
                    Property = "InvoiceField",
                },
                new Column
                {
                    Header = "Treaty Type",
                    ColIndex = ColumnTreatyType,
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Business Origin",
                    ColIndex = ColumnBusinessOrigin,
                    Property = "BusinessOrigin",
                },
                new Column
                {
                    Header = "Item Code",
                    ColIndex = ColumnItemCode,
                    Property = "ItemCode",
                },
                new Column
                {
                    Header = "Reporting Type",
                    ColIndex = ColumnReportingType,
                    Property = "ReportingType",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }
    }
}
