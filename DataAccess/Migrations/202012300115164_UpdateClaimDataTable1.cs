namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimData", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimData", "PolicyTerm", c => c.Double());
            CreateIndex("dbo.ClaimData", "PolicyTerm");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimData", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimData", "PolicyTerm", c => c.Int());
            CreateIndex("dbo.ClaimData", "PolicyTerm");
        }
    }
}
