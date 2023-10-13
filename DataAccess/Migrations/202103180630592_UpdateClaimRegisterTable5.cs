namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegister", "DrProvisionStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.ClaimRegister", "DrProvisionStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegister", new[] { "DrProvisionStatus" });
            DropColumn("dbo.ClaimRegister", "DrProvisionStatus");
        }
    }
}
