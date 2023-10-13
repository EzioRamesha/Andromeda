using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Retrocession
{
    public class RetroTreatyBo
    {
        public int Id { get; set; }

        public int? RetroPartyId { get; set; }

        public RetroPartyBo RetroPartyBo { get; set; }

        public int Status { get; set; }

        //public string Party { get; set; }

        public string Code { get; set; }

        public int TreatyTypePickListDetailId { get; set; }

        public virtual PickListDetailBo TreatyTypePickListDetailBo { get; set; }

        public bool IsLobAutomatic { get; set; } = false;

        public bool IsLobFacultative { get; set; } = false;

        public bool IsLobAdvantageProgram { get; set; } = false;

        public double? RetroShare { get; set; }

        public int? TreatyDiscountTableId { get; set; }

        public virtual TreatyDiscountTableBo TreatyDiscountTableBo { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Download and Detail
        public string RetroPartyParty { get; set; }
        public string TreatyType { get; set; }
        public string TreatyDiscountRule { get; set; }

        public int? DetailId { get; set; }

        // PerLifeRetroConfigurationTreaty
        public string DetailConfigTreatyCode { get; set; }

        public string DetailConfigTreatyType { get; set; }

        public string DetailConfigFundsAccountingType { get; set; }

        public DateTime? DetailConfigEffectiveStartDate { get; set; }

        public DateTime? DetailConfigEffectiveEndDate { get; set; }

        public DateTime? DetailConfigRiskQuarterStartDate { get; set; }

        public DateTime? DetailConfigRiskQuarterEndDate { get; set; }

        public bool? DetailConfigIsToAggregate { get; set; }

        public string DetailConfigRemark { get; set; }

        // End
        public string DetailPremiumSpreadRule { get; set; }

        public string DetailTreatyDiscountRule { get; set; }

        public double? DetailMlreShare { get; set; }

        public string DetailGrossRetroPremium { get; set; }

        public string DetailTreatyDiscount { get; set; }

        public string DetailNetRetroPremium { get; set; }

        public string DetailRemark { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;
        public const int MaxStatus = 2;

        public const string StatusActiveName = "Active";
        public const string StatusInactiveName = "Inactive";

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return StatusActiveName;
                case StatusInactive:
                    return StatusInactiveName;
                default:
                    return "";
            }
        }

        public static int GetStatusKey(string name)
        {
            switch (name)
            {
                case StatusActiveName:
                    return StatusActive;
                case StatusInactiveName:
                    return StatusInactive;
                default:
                    return 0;
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "status-success-badge";
                case StatusInactive:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetLineOfBusiness(bool isAutomatic, bool isFacultative, bool isAdvantageProgram)
        {
            List<string> lob = new List<string>();
            if (isAutomatic) lob.Add("AUTO");
            if (isFacultative) lob.Add("FAC");
            if (isAdvantageProgram) lob.Add("AP");

            return string.Join(", ", lob);
        }

        public const int ColumnId = 1;
        public const int ColumnRetroPartyParty = 2;
        public const int ColumnStatus = 3;
        public const int ColumnCode = 4;
        public const int ColumnTreatyType = 5;
        public const int ColumnLineOfBusiness = 6;
        public const int ColumnRetroShare = 7;
        public const int ColumnTreatyDiscountRule = 8;
        public const int ColumnEffectiveStartDate = 9;
        public const int ColumnEffectiveEndDate = 10;
        public const int ColumnAction = 11;

        public const int ColumnDetailId = 12;
        public const int ColumnDetailConfigTreatyCode = 13;
        public const int ColumnDetailConfigTreatyType = 14;
        public const int ColumnDetailConfigFundsAccountingType = 15;
        public const int ColumnDetailConfigEffectiveStartDate = 16;
        public const int ColumnDetailConfigEffectiveEndDate = 17;
        public const int ColumnDetailConfigRiskQuarterStartDate = 18;
        public const int ColumnDetailConfigRiskQuarterEndDate = 19;
        public const int ColumnDetailConfigIsToAggregate = 20;
        public const int ColumnDetailConfigRemark = 21;
        public const int ColumnDetailPremiumSpreadRule = 22;
        public const int ColumnDetailTreatyDiscountRule = 23;
        public const int ColumnDetailMlreShare = 24;
        public const int ColumnDetailGrossRetroPremium = 25;
        public const int ColumnDetailTreatyDiscount = 26;
        public const int ColumnDetailNetRetroPremium = 27;
        public const int ColumnDetailRemark = 28;
        public const int ColumnDetailAction = 29;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Retro Treaty ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Retro Party",
                    ColIndex = ColumnRetroPartyParty,
                    Property = "RetroPartyParty",
                },
                new Column
                {
                    Header = "Status",
                    ColIndex = ColumnStatus,
                    Property = "Status",
                },
                new Column
                {
                    Header = "Code",
                    ColIndex = ColumnCode,
                    Property = "Code",
                },
                new Column
                {
                    Header = "Treaty Type",
                    ColIndex = ColumnTreatyType,
                    Property = "TreatyType",
                },
                new Column
                {
                    Header = "Line of Business",
                    ColIndex = ColumnLineOfBusiness,
                    Property = "LineOfBusiness",
                },
                new Column
                {
                    Header = "Retro Share",
                    ColIndex = ColumnRetroShare,
                    Property = "RetroShare",
                },
                new Column
                {
                    Header = "Treaty Discount Rule",
                    ColIndex = ColumnTreatyDiscountRule,
                    Property = "TreatyDiscountRule",
                },
                new Column
                {
                    Header = "Effective Start Date",
                    ColIndex = ColumnEffectiveStartDate,
                    Property = "EffectiveStartDate",
                },
                new Column
                {
                    Header = "Effective End Date",
                    ColIndex = ColumnEffectiveEndDate,
                    Property = "EffectiveEndDate",
                },
                new Column
                {
                    Header = "Action (Retro Treaty)",
                    ColIndex = ColumnAction,
                },
                new Column
                {
                    Header = "Retro Treaty Detail ID",
                    ColIndex = ColumnDetailId,
                    Property = "DetailId",
                },
                new Column
                {
                    Header = "Config's Treaty Code",
                    ColIndex = ColumnDetailConfigTreatyCode,
                    Property = "DetailConfigTreatyCode",
                },
                new Column
                {
                    Header = "Config's Treaty Type",
                    ColIndex = ColumnDetailConfigTreatyType,
                    Property = "DetailConfigTreatyType",
                },
                new Column
                {
                    Header = "Config's Funds Accounting Type",
                    ColIndex = ColumnDetailConfigFundsAccountingType,
                    Property = "DetailConfigFundsAccountingType",
                },
                new Column
                {
                    Header = "Config's Effective Start Date",
                    ColIndex = ColumnDetailConfigEffectiveStartDate,
                    Property = "DetailConfigEffectiveStartDate",
                },
                new Column
                {
                    Header = "Config's Effective End Date",
                    ColIndex = ColumnDetailConfigEffectiveEndDate,
                    Property = "DetailConfigEffectiveEndDate",
                },
                new Column
                {
                    Header = "Config's Risk Quarter Start Date",
                    ColIndex = ColumnDetailConfigRiskQuarterStartDate,
                    Property = "DetailConfigRiskQuarterStartDate",
                },
                new Column
                {
                    Header = "Config's Risk Quarter End Date",
                    ColIndex = ColumnDetailConfigRiskQuarterEndDate,
                    Property = "DetailConfigRiskQuarterEndDate",
                },
                new Column
                {
                    Header = "Config's To Aggregate",
                    ColIndex = ColumnDetailConfigIsToAggregate,
                    Property = "DetailConfigIsToAggregate",
                },
                new Column
                {
                    Header = "Config's Remark",
                    ColIndex = ColumnDetailConfigRemark,
                    Property = "DetailConfigRemark",
                },
                new Column
                {
                    Header = "Detail's Premium Spread Rule",
                    ColIndex = ColumnDetailPremiumSpreadRule,
                    Property = "DetailPremiumSpreadRule",
                },
                new Column
                {
                    Header = "Detail's Treaty Discount Rule",
                    ColIndex = ColumnDetailTreatyDiscountRule,
                    Property = "DetailTreatyDiscountRule",
                },
                new Column
                {
                    Header = "Detail's MLRe Share",
                    ColIndex = ColumnDetailMlreShare,
                    Property = "DetailMlreShare",
                },
                new Column
                {
                    Header = "Detail's Gross Retro Premium",
                    ColIndex = ColumnDetailGrossRetroPremium,
                    Property = "DetailGrossRetroPremium",
                },
                new Column
                {
                    Header = "Detail's Treaty Discount",
                    ColIndex = ColumnDetailTreatyDiscount,
                    Property = "DetailTreatyDiscount",
                },
                new Column
                {
                    Header = "Detail's Net Retro Premium",
                    ColIndex = ColumnDetailNetRetroPremium,
                    Property = "DetailNetRetroPremium",
                },
                new Column
                {
                    Header = "Detail's Remark",
                    ColIndex = ColumnDetailRemark,
                    Property = "DetailRemark",
                },
                new Column
                {
                    Header = "Action (Retro Treaty Detail)",
                    ColIndex = ColumnDetailAction,
                },
            };
        }
    }
}
