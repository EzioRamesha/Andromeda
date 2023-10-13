namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegister", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimRegister", "PolicyTerm", c => c.Double());
            CreateIndex("dbo.ClaimRegister", "PolicyTerm");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegister", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimRegister", "PolicyTerm", c => c.Int());
            CreateIndex("dbo.ClaimRegister", "PolicyTerm");
        }
    }
}
