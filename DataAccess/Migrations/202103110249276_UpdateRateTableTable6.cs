namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRateTableTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateTables", "ReportingStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.RateTables", "ReportingEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.RateTables", "ReportingStartDate");
            CreateIndex("dbo.RateTables", "ReportingEndDate");
        }

        public override void Down()
        {
            DropIndex("dbo.RateTables", new[] { "ReportingEndDate" });
            DropIndex("dbo.RateTables", new[] { "ReportingStartDate" });
            DropColumn("dbo.RateTables", "ReportingEndDate");
            DropColumn("dbo.RateTables", "ReportingStartDate");
        }
    }
}
