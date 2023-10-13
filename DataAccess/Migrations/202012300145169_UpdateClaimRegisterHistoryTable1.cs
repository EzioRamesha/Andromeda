namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimRegisterHistories", "PolicyTerm", c => c.Double());
            CreateIndex("dbo.ClaimRegisterHistories", "PolicyTerm");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "PolicyTerm" });
            AlterColumn("dbo.ClaimRegisterHistories", "PolicyTerm", c => c.Int());
            CreateIndex("dbo.ClaimRegisterHistories", "PolicyTerm");
        }
    }
}
