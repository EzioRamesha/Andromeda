using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralHipsTableBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralId { get; set; }
        public TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int? TreatyPricingGroupReferralFileId { get; set; }
        public TreatyPricingGroupReferralFileBo TreatyPricingGroupReferralFileBo { get; set; }

        public int? HipsCategoryId { get; set; }
        public HipsCategoryBo HipsCategoryBo { get; set; }

        public int? HipsSubCategoryId { get; set; }
        public HipsCategoryDetailBo HipsSubCategoryBo { get; set; }

        public DateTime? CoverageStartDate { get; set; }

        public string Description { get; set; }

        public string PlanA { get; set; }
        public string PlanB { get; set; }
        public string PlanC { get; set; }
        public string PlanD { get; set; }
        public string PlanE { get; set; }
        public string PlanF { get; set; }
        public string PlanG { get; set; }
        public string PlanH { get; set; }
        public string PlanI { get; set; }
        public string PlanJ { get; set; }
        public string PlanK { get; set; }
        public string PlanL { get; set; }
        public string PlanM { get; set; }
        public string PlanN { get; set; }
        public string PlanO { get; set; }
        public string PlanP { get; set; }
        public string PlanQ { get; set; }
        public string PlanR { get; set; }
        public string PlanS { get; set; }
        public string PlanT { get; set; }
        public string PlanU { get; set; }
        public string PlanV { get; set; }
        public string PlanW { get; set; }
        public string PlanX { get; set; }
        public string PlanY { get; set; }
        public string PlanZ { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public string CoverageStartDateStr { get; set; }
        public string CategoryCode { get; set; }
        public string SubCategoryCode { get; set; }


        public const int ColumnCoverageStartDate = 1;
        public const int ColumnCategory = 2;
        public const int ColumnSubCategory = 3;
        public const int ColumnDescription = 4;
        public const int ColumnPlanA = 5;
        public const int ColumnPlanB = 6;
        public const int ColumnPlanC = 7;
        public const int ColumnPlanD = 8;
        public const int ColumnPlanE = 9;
        public const int ColumnPlanF = 10;
        public const int ColumnPlanG = 11;
        public const int ColumnPlanH = 12;
        public const int ColumnPlanI = 13;
        public const int ColumnPlanJ = 14;
        public const int ColumnPlanK = 15;
        public const int ColumnPlanL = 16;
        public const int ColumnPlanM = 17;
        public const int ColumnPlanN = 18;
        public const int ColumnPlanO = 19;
        public const int ColumnPlanP = 20;
        public const int ColumnPlanQ = 21;
        public const int ColumnPlanR = 22;
        public const int ColumnPlanS = 23;
        public const int ColumnPlanT = 24;
        public const int ColumnPlanU = 25;
        public const int ColumnPlanV = 26;
        public const int ColumnPlanW = 27;
        public const int ColumnPlanX = 28;
        public const int ColumnPlanY = 29;
        public const int ColumnPlanZ = 30;

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Coverage Start Date",
                    ColIndex = ColumnCoverageStartDate,
                    Property = "CoverageStartDate",
                },
                new Column
                {
                    Header = "Category",
                    ColIndex = ColumnCategory,
                    Property = "HipsCategoryId",
                },
                new Column
                {
                    Header = "Subcategory",
                    ColIndex = ColumnSubCategory,
                    Property = "HipsSubCategoryId",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                    Property = "Description",
                },
                new Column
                {
                    Header = "Plan A",
                    ColIndex = ColumnPlanA,
                    Property = "PlanA",
                },
                new Column
                {
                    Header = "Plan B",
                    ColIndex = ColumnPlanB,
                    Property = "PlanB",
                },
                new Column
                {
                    Header = "Plan C",
                    ColIndex = ColumnPlanC,
                    Property = "PlanC",
                },
                new Column
                {
                    Header = "Plan D",
                    ColIndex = ColumnPlanD,
                    Property = "PlanD",
                },
                new Column
                {
                    Header = "Plan E",
                    ColIndex = ColumnPlanE,
                    Property = "PlanE",
                },
                new Column
                {
                    Header = "Plan F",
                    ColIndex = ColumnPlanF,
                    Property = "PlanF",
                },
                new Column
                {
                    Header = "Plan G",
                    ColIndex = ColumnPlanG,
                    Property = "PlanG",
                },
                new Column
                {
                    Header = "Plan H",
                    ColIndex = ColumnPlanH,
                    Property = "PlanH",
                },
                new Column
                {
                    Header = "Plan I",
                    ColIndex = ColumnPlanI,
                    Property = "PlanI",
                },
                new Column
                {
                    Header = "Plan J",
                    ColIndex = ColumnPlanJ,
                    Property = "PlanJ",
                },
                new Column
                {
                    Header = "Plan K",
                    ColIndex = ColumnPlanK,
                    Property = "PlanK",
                },
                new Column
                {
                    Header = "Plan L",
                    ColIndex = ColumnPlanL,
                    Property = "PlanL",
                },
                new Column
                {
                    Header = "Plan M",
                    ColIndex = ColumnPlanM,
                    Property = "PlanM",
                },
                new Column
                {
                    Header = "Plan N",
                    ColIndex = ColumnPlanN,
                    Property = "PlanN",
                },
                new Column
                {
                    Header = "Plan O",
                    ColIndex = ColumnPlanO,
                    Property = "PlanO",
                },
                new Column
                {
                    Header = "Plan P",
                    ColIndex = ColumnPlanP,
                    Property = "PlanP",
                },new Column
                {
                    Header = "Plan Q",
                    ColIndex = ColumnPlanQ,
                    Property = "PlanQ",
                },
                new Column
                {
                    Header = "Plan R",
                    ColIndex = ColumnPlanR,
                    Property = "PlanR",
                },
                new Column
                {
                    Header = "Plan S",
                    ColIndex = ColumnPlanS,
                    Property = "PlanS",
                },
                new Column
                {
                    Header = "Plan T",
                    ColIndex = ColumnPlanT,
                    Property = "PlanT",
                },
                new Column
                {
                    Header = "Plan U",
                    ColIndex = ColumnPlanU,
                    Property = "PlanU",
                },
                new Column
                {
                    Header = "Plan V",
                    ColIndex = ColumnPlanV,
                    Property = "PlanV",
                },
                new Column
                {
                    Header = "Plan W",
                    ColIndex = ColumnPlanW,
                    Property = "PlanW",
                },
                new Column
                {
                    Header = "Plan X",
                    ColIndex = ColumnPlanX,
                    Property = "PlanX",
                },
                new Column
                {
                    Header = "Plan Y",
                    ColIndex = ColumnPlanY,
                    Property = "PlanY",
                },
                new Column
                {
                    Header = "Plan Z",
                    ColIndex = ColumnPlanZ,
                    Property = "PlanZ",
                },
            };

            return columns;
        }
    }
}
