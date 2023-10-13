namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralRiDataTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralRiData", "MaxApLoading", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreInsuredAttainedAgeAtCurrentMonth", c => c.Int());
            AddColumn("dbo.ReferralRiData", "MlreInsuredAttainedAgeAtPreviousMonth", c => c.Int());
            AddColumn("dbo.ReferralRiData", "InsuredAttainedAgeCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MaxExpiryAgeCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MlrePolicyIssueAge", c => c.Int());
            AddColumn("dbo.ReferralRiData", "PolicyIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MinIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MaxIssueAgeCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MaxUwRatingCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "ApLoadingCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "EffectiveDateCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MinAarCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MaxAarCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "CorridorLimitCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "AblCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "RetentionCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "AarCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "MlreStandardPremium", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreSubstandardPremium", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreFlatExtraPremium", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreGrossPremium", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreStandardDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreSubstandardDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreLargeSaDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreGroupSizeDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreVitalityDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreTotalDiscount", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreNetPremium", c => c.Double());
            AddColumn("dbo.ReferralRiData", "NetPremiumCheck", c => c.Double());
            AddColumn("dbo.ReferralRiData", "ServiceFeePercentage", c => c.Double());
            AddColumn("dbo.ReferralRiData", "ServiceFee", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreBrokerageFee", c => c.Double());
            AddColumn("dbo.ReferralRiData", "MlreDatabaseCommission", c => c.Double());
            AddColumn("dbo.ReferralRiData", "ValidityDayCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "SumAssuredOfferedCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "UwRatingCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "FlatExtraAmountCheck", c => c.Boolean());
            AddColumn("dbo.ReferralRiData", "FlatExtraDurationCheck", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferralRiData", "FlatExtraDurationCheck");
            DropColumn("dbo.ReferralRiData", "FlatExtraAmountCheck");
            DropColumn("dbo.ReferralRiData", "UwRatingCheck");
            DropColumn("dbo.ReferralRiData", "SumAssuredOfferedCheck");
            DropColumn("dbo.ReferralRiData", "ValidityDayCheck");
            DropColumn("dbo.ReferralRiData", "MlreDatabaseCommission");
            DropColumn("dbo.ReferralRiData", "MlreBrokerageFee");
            DropColumn("dbo.ReferralRiData", "ServiceFee");
            DropColumn("dbo.ReferralRiData", "ServiceFeePercentage");
            DropColumn("dbo.ReferralRiData", "NetPremiumCheck");
            DropColumn("dbo.ReferralRiData", "MlreNetPremium");
            DropColumn("dbo.ReferralRiData", "MlreTotalDiscount");
            DropColumn("dbo.ReferralRiData", "MlreVitalityDiscount");
            DropColumn("dbo.ReferralRiData", "MlreGroupSizeDiscount");
            DropColumn("dbo.ReferralRiData", "MlreLargeSaDiscount");
            DropColumn("dbo.ReferralRiData", "MlreSubstandardDiscount");
            DropColumn("dbo.ReferralRiData", "MlreStandardDiscount");
            DropColumn("dbo.ReferralRiData", "MlreGrossPremium");
            DropColumn("dbo.ReferralRiData", "MlreFlatExtraPremium");
            DropColumn("dbo.ReferralRiData", "MlreSubstandardPremium");
            DropColumn("dbo.ReferralRiData", "MlreStandardPremium");
            DropColumn("dbo.ReferralRiData", "AarCheck");
            DropColumn("dbo.ReferralRiData", "RetentionCheck");
            DropColumn("dbo.ReferralRiData", "AblCheck");
            DropColumn("dbo.ReferralRiData", "CorridorLimitCheck");
            DropColumn("dbo.ReferralRiData", "MaxAarCheck");
            DropColumn("dbo.ReferralRiData", "MinAarCheck");
            DropColumn("dbo.ReferralRiData", "EffectiveDateCheck");
            DropColumn("dbo.ReferralRiData", "ApLoadingCheck");
            DropColumn("dbo.ReferralRiData", "MaxUwRatingCheck");
            DropColumn("dbo.ReferralRiData", "MaxIssueAgeCheck");
            DropColumn("dbo.ReferralRiData", "MinIssueAgeCheck");
            DropColumn("dbo.ReferralRiData", "PolicyIssueAgeCheck");
            DropColumn("dbo.ReferralRiData", "MlrePolicyIssueAge");
            DropColumn("dbo.ReferralRiData", "MaxExpiryAgeCheck");
            DropColumn("dbo.ReferralRiData", "InsuredAttainedAgeCheck");
            DropColumn("dbo.ReferralRiData", "MlreInsuredAttainedAgeAtPreviousMonth");
            DropColumn("dbo.ReferralRiData", "MlreInsuredAttainedAgeAtCurrentMonth");
            DropColumn("dbo.ReferralRiData", "MaxApLoading");
        }
    }
}
