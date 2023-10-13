namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRiDataTable14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "InsuredAttainedAgeCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MaxExpiryAgeCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "PolicyIssueAgeCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MinIssueAgeCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MaxIssueAgeCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MaxUwRatingCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "ApLoadingCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "EffectiveDateCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MinAarCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "MaxAarCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "CorridorLimitCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "AblCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "RetentionCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "AarCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "ValidityDayCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "SumAssuredOfferedCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "UwRatingCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "FlatExtraAmountCheck", c => c.Boolean());
            AlterColumn("dbo.RiData", "FlatExtraDurationCheck", c => c.Boolean());
        }

        public override void Down()
        {
            AlterColumn("dbo.RiData", "FlatExtraDurationCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "FlatExtraAmountCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "UwRatingCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "SumAssuredOfferedCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "ValidityDayCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "AarCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "RetentionCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "AblCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "CorridorLimitCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MaxAarCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MinAarCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "EffectiveDateCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "ApLoadingCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MaxUwRatingCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MaxIssueAgeCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MinIssueAgeCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "PolicyIssueAgeCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "MaxExpiryAgeCheck", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "InsuredAttainedAgeCheck", c => c.Boolean(nullable: false));
        }
    }
}
