namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeClaimTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeClaims", "PersonInChargeId", c => c.Int());
            AddColumn("dbo.PerLifeClaims", "ProcessingDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.PerLifeClaims", "PersonInChargeId");
            CreateIndex("dbo.PerLifeClaims", "ProcessingDate");
            AddForeignKey("dbo.PerLifeClaims", "PersonInChargeId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeClaims", "PersonInChargeId", "dbo.Users");
            DropIndex("dbo.PerLifeClaims", new[] { "ProcessingDate" });
            DropIndex("dbo.PerLifeClaims", new[] { "PersonInChargeId" });
            DropColumn("dbo.PerLifeClaims", "ProcessingDate");
            DropColumn("dbo.PerLifeClaims", "PersonInChargeId");
        }
    }
}
