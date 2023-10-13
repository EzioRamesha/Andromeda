namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouse", "MaxApLoading", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreInsuredAttainedAgeAtCurrentMonth", c => c.Int());
            AddColumn("dbo.RiDataWarehouse", "MlreInsuredAttainedAgeAtPreviousMonth", c => c.Int());
            AddColumn("dbo.RiDataWarehouse", "InsuredAttainedAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MaxExpiryAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MlrePolicyIssueAge", c => c.Int());
            AddColumn("dbo.RiDataWarehouse", "PolicyIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MinIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MaxIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MaxUwRatingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "ApLoadingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "EffectiveDateCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MinAarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MaxAarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "CorridorLimitCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "AblCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "RetentionCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "AarCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "MlreStandardPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreSubstandardPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreFlatExtraPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreGrossPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreStandardDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreSubstandardDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreLargeSaDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreGroupSizeDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreVitalityDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreTotalDiscount", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreNetPremium", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "NetPremiumCheck", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "ServiceFeePercentage", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "ServiceFee", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreBrokerageFee", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "MlreDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "ValidityDayCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "SumAssuredOfferedCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "UwRatingCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "FlatExtraAmountCheck", c => c.Boolean());
            AddColumn("dbo.RiDataWarehouse", "FlatExtraDurationCheck", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataWarehouse", "FlatExtraDurationCheck");
            DropColumn("dbo.RiDataWarehouse", "FlatExtraAmountCheck");
            DropColumn("dbo.RiDataWarehouse", "UwRatingCheck");
            DropColumn("dbo.RiDataWarehouse", "SumAssuredOfferedCheck");
            DropColumn("dbo.RiDataWarehouse", "ValidityDayCheck");
            DropColumn("dbo.RiDataWarehouse", "MlreDatabaseCommission");
            DropColumn("dbo.RiDataWarehouse", "MlreBrokerageFee");
            DropColumn("dbo.RiDataWarehouse", "ServiceFee");
            DropColumn("dbo.RiDataWarehouse", "ServiceFeePercentage");
            DropColumn("dbo.RiDataWarehouse", "NetPremiumCheck");
            DropColumn("dbo.RiDataWarehouse", "MlreNetPremium");
            DropColumn("dbo.RiDataWarehouse", "MlreTotalDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreVitalityDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreGroupSizeDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreLargeSaDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreSubstandardDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreStandardDiscount");
            DropColumn("dbo.RiDataWarehouse", "MlreGrossPremium");
            DropColumn("dbo.RiDataWarehouse", "MlreFlatExtraPremium");
            DropColumn("dbo.RiDataWarehouse", "MlreSubstandardPremium");
            DropColumn("dbo.RiDataWarehouse", "MlreStandardPremium");
            DropColumn("dbo.RiDataWarehouse", "AarCheck");
            DropColumn("dbo.RiDataWarehouse", "RetentionCheck");
            DropColumn("dbo.RiDataWarehouse", "AblCheck");
            DropColumn("dbo.RiDataWarehouse", "CorridorLimitCheck");
            DropColumn("dbo.RiDataWarehouse", "MaxAarCheck");
            DropColumn("dbo.RiDataWarehouse", "MinAarCheck");
            DropColumn("dbo.RiDataWarehouse", "EffectiveDateCheck");
            DropColumn("dbo.RiDataWarehouse", "ApLoadingCheck");
            DropColumn("dbo.RiDataWarehouse", "MaxUwRatingCheck");
            DropColumn("dbo.RiDataWarehouse", "MaxIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouse", "MinIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouse", "PolicyIssueAgeCheck");
            DropColumn("dbo.RiDataWarehouse", "MlrePolicyIssueAge");
            DropColumn("dbo.RiDataWarehouse", "MaxExpiryAgeCheck");
            DropColumn("dbo.RiDataWarehouse", "InsuredAttainedAgeCheck");
            DropColumn("dbo.RiDataWarehouse", "MlreInsuredAttainedAgeAtPreviousMonth");
            DropColumn("dbo.RiDataWarehouse", "MlreInsuredAttainedAgeAtCurrentMonth");
            DropColumn("dbo.RiDataWarehouse", "MaxApLoading");
        }
    }
}
