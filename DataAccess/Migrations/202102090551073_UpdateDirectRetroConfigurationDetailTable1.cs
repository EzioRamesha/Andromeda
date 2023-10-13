namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateDirectRetroConfigurationDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "PremiumSpreadTableId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "TreatyDiscountTableId" });
            AlterColumn("dbo.DirectRetroConfigurationDetails", "RiskPeriodStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "RiskPeriodEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "IssueDatePolStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "IssueDatePolEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "PremiumSpreadTableId", c => c.Int());
            AlterColumn("dbo.DirectRetroConfigurationDetails", "TreatyDiscountTableId", c => c.Int());
            CreateIndex("dbo.DirectRetroConfigurationDetails", "RiskPeriodStartDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "RiskPeriodEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "IssueDatePolStartDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "IssueDatePolEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolStartDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "PremiumSpreadTableId");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "TreatyDiscountTableId");
        }

        public override void Down()
        {
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "TreatyDiscountTableId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "PremiumSpreadTableId" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "IssueDatePolStartDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodEndDate" });
            DropIndex("dbo.DirectRetroConfigurationDetails", new[] { "RiskPeriodStartDate" });
            AlterColumn("dbo.DirectRetroConfigurationDetails", "TreatyDiscountTableId", c => c.Int(nullable: false));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "PremiumSpreadTableId", c => c.Int(nullable: false));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "IssueDatePolEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "IssueDatePolStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "RiskPeriodEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DirectRetroConfigurationDetails", "RiskPeriodStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.DirectRetroConfigurationDetails", "TreatyDiscountTableId");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "PremiumSpreadTableId");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "ReinsEffDatePolStartDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "IssueDatePolEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "IssueDatePolStartDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "RiskPeriodEndDate");
            CreateIndex("dbo.DirectRetroConfigurationDetails", "RiskPeriodStartDate");
        }
    }
}
