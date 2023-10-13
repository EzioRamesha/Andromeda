namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeSoaTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeSoa", "InvoiceStatus", c => c.Int(nullable: false));
            AddColumn("dbo.PerLifeSoa", "PersonInChargeId", c => c.Int());
            AddColumn("dbo.PerLifeSoa", "ProcessingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.PerLifeSoa", "IsProfitCommissionData", c => c.Boolean(nullable: false));
            CreateIndex("dbo.PerLifeSoa", "InvoiceStatus");
            CreateIndex("dbo.PerLifeSoa", "PersonInChargeId");
            CreateIndex("dbo.PerLifeSoa", "ProcessingDate");
            AddForeignKey("dbo.PerLifeSoa", "PersonInChargeId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoa", "PersonInChargeId", "dbo.Users");
            DropIndex("dbo.PerLifeSoa", new[] { "ProcessingDate" });
            DropIndex("dbo.PerLifeSoa", new[] { "PersonInChargeId" });
            DropIndex("dbo.PerLifeSoa", new[] { "InvoiceStatus" });
            DropColumn("dbo.PerLifeSoa", "IsProfitCommissionData");
            DropColumn("dbo.PerLifeSoa", "ProcessingDate");
            DropColumn("dbo.PerLifeSoa", "PersonInChargeId");
            DropColumn("dbo.PerLifeSoa", "InvoiceStatus");
        }
    }
}
