namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegisterHistories", "DrProvisionStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.ClaimRegisterHistories", "DrProvisionStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "DrProvisionStatus" });
            DropColumn("dbo.ClaimRegisterHistories", "DrProvisionStatus");
        }
    }
}
