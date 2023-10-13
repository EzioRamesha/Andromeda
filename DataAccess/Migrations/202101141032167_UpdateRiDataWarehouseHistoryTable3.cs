namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouseHistories", "MaxApLoading", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreInsuredAttainedAgeAtCurrentMonth", c => c.Int());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreInsuredAttainedAgeAtPreviousMonth", c => c.Int());
            AddColumn("dbo.RiDataWarehouseHistories", "InsuredAttainedAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MaxExpiryAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MlrePolicyIssueAge", c => c.Int());
            AddColumn("dbo.RiDataWarehouseHistories", "PolicyIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MinIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MaxIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MaxUwRatingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "ApLoadingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "EffectiveDateCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MinAarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MaxAarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "CorridorLimitCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "AblCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "RetentionCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "AarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreStandardPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreSubstandardPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreFlatExtraPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreGrossPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreStandardDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreSubstandardDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreLargeSaDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreGroupSizeDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreVitalityDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreTotalDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreNetPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "NetPremiumCheck", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "ServiceFeePercentage", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "ServiceFee", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreBrokerageFee", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "MlreDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "ValidityDayCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "SumAssuredOfferedCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "UwRatingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "FlatExtraAmountCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouseHistories", "FlatExtraDurationCheck", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataWarehouseHistories", "FlatExtraDurationCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "FlatExtraAmountCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "UwRatingCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "SumAssuredOfferedCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "ValidityDayCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreDatabaseCommission");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreBrokerageFee");
            DropColumn("dbo.RiDataWarehouseHistories", "ServiceFee");
            DropColumn("dbo.RiDataWarehouseHistories", "ServiceFeePercentage");
            DropColumn("dbo.RiDataWarehouseHistories", "NetPremiumCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreNetPremium");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreTotalDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreVitalityDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreGroupSizeDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreLargeSaDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreSubstandardDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreStandardDiscount");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreGrossPremium");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreFlatExtraPremium");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreSubstandardPremium");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreStandardPremium");
            DropColumn("dbo.RiDataWarehouseHistories", "AarCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "RetentionCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "AblCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "CorridorLimitCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MaxAarCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MinAarCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "EffectiveDateCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "ApLoadingCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MaxUwRatingCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MaxIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MinIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "PolicyIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MlrePolicyIssueAge");
            DropColumn("dbo.RiDataWarehouseHistories", "MaxExpiryAgeCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "InsuredAttainedAgeCheck");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreInsuredAttainedAgeAtPreviousMonth");
            DropColumn("dbo.RiDataWarehouseHistories", "MlreInsuredAttainedAgeAtCurrentMonth");
            DropColumn("dbo.RiDataWarehouseHistories", "MaxApLoading");
        }
    }
}
