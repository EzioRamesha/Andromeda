namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable9 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegister", new[] { "TempS1" });
            AlterColumn("dbo.ClaimRegister", "TempS1", c => c.String(maxLength: 150));
            CreateIndex("dbo.ClaimRegister", "TempS1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClaimRegister", new[] { "TempS1" });
            AlterColumn("dbo.ClaimRegister", "TempS1", c => c.String(maxLength: 50));
            CreateIndex("dbo.ClaimRegister", "TempS1");
        }
    }
}
