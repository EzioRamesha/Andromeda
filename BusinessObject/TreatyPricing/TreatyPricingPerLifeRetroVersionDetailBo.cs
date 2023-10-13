using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingPerLifeRetroVersionId { get; set; }

        public TreatyPricingPerLifeRetroVersionBo TreatyPricingPerLifeRetroVersionBo { get; set; }

        public int SortIndex { get; set; }

        public int Item { get; set; }

        public string Component { get; set; }

        public bool IsComponentEditable { get; set; } = false;

        public string ComponentDescription { get; set; }

        public bool IsComponentDescriptionEditable { get; set; } = false;

        public bool IsDropDown { get; set; } = false;

        public int? DropDownValue { get; set; }

        public bool? IsEnabled { get; set; }

        public bool IsNetGrossRequired { get; set; } = false;

        public bool? IsNetGross { get; set; }

        public string Value { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ItemIncome = 1;
        public const int ItemOutgo = 2;

        public const int ItemMax = 2;

        public const int DropDownString = 1;
        public const int DropDownYear = 2;

        public const int DropDownMax = 2;

        public static string GetItemName(int key)
        {
            switch (key)
            {
                case ItemIncome:
                    return "Income";
                case ItemOutgo:
                    return "Outgo";
                default:
                    return "";
            }
        }

        public static string GetDropDownName(int key)
        {
            switch (key)
            {
                case DropDownString:
                    return "Loses shall be carried forward indefinitely.";
                case DropDownYear:
                    return "Year(s)";
                default:
                    return "";
            }
        }

        public static List<TreatyPricingPerLifeRetroVersionDetailBo> GetDefaultRow(int parentId)
        {
            return new List<TreatyPricingPerLifeRetroVersionDetailBo> {
                // Income
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 1,
                    Item = ItemIncome,
                    Component = "Reserve for unexpired risk as at the beginning of the year",
                    ComponentDescription = "% of the reinsurance premiums net of cancellation corresponding to the preceding year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 2,
                    Item = ItemIncome,
                    Component = "Reserve for unpaid claims as at the beginning of the year",
                    ComponentDescription = "= Reserve for unpaid claims corresponding to the preceding year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 3,
                    Item = ItemIncome,
                    Component = "Reinsurance premiums net of cancellation received during the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 4,
                    Item = ItemIncome,
                    Component = "IBNR reserve as at the beginning of the year",
                    ComponentDescription = "% of the reinsurance premiums net of cancellation corresponding to the preceding year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 5,
                    Item = ItemIncome,
                    IsComponentEditable = true,
                    IsComponentDescriptionEditable = true,
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 6,
                    Item = ItemIncome,
                    IsComponentEditable = true,
                    IsComponentDescriptionEditable = true,
                    IsEnabled = false,
                },
                // outgo
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 7,
                    Item = ItemOutgo,
                    Component = "Claims and legal expenses paid during the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 8,
                    Item = ItemOutgo,
                    Component = "Reinsurer's administrative expenses",
                    ComponentDescription = "% of the reinsurance premiums set out under item (3) above",
                    IsEnabled = false,
                    IsNetGrossRequired = true,
                    IsNetGross = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 9,
                    Item = ItemOutgo,
                    Component = "Reserve for unexpired risk as at the end of the year",
                    ComponentDescription = "% of the reinsurance premiums net of cancellation reveived during the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 10,
                    Item = ItemOutgo,
                    Component = "Reserve for unpaid claims as at the end of the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 11,
                    Item = ItemOutgo,
                    Component = "Reinsurance discount paid during the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 12,
                    Item = ItemOutgo,
                    Component = "IBNR reserve as at the end of the year",
                    ComponentDescription = "% of the reinsurance premiums net of cancellation reveived during the year",
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 13,
                    Item = ItemOutgo,
                    Component = "Any loss carried forward from the Profit Commission Statement of the preceding years, where [Option]",
                    IsDropDown = true,
                    DropDownValue = DropDownString,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 14,
                    Item = ItemOutgo,
                    IsComponentEditable = true,
                    IsComponentDescriptionEditable = true,
                    IsEnabled = false,
                },
                new TreatyPricingPerLifeRetroVersionDetailBo
                {
                    TreatyPricingPerLifeRetroVersionId = parentId,
                    SortIndex = 15,
                    Item = ItemOutgo,
                    IsComponentEditable = true,
                    IsComponentDescriptionEditable = true,
                    IsEnabled = false,
                },
            };
        }

        public static string GetJsonDefaultRow(int parentId)
        {
            return JsonConvert.SerializeObject(GetDefaultRow(parentId));
        }
    }
}
