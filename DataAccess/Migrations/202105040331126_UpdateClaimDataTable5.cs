namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataTable5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimData", new[] { "TempS1" });
            AlterColumn("dbo.ClaimData", "TempS1", c => c.String(maxLength: 150));
            CreateIndex("dbo.ClaimData", "TempS1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimData", new[] { "TempS1" });
            AlterColumn("dbo.ClaimData", "TempS1", c => c.String(maxLength: 50));
            CreateIndex("dbo.ClaimData", "TempS1");
        }
    }
}
