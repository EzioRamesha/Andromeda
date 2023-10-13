namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RetroRegister", new[] { "RetroRegisterBatchId" });
            AddColumn("dbo.RetroRegister", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.RetroRegister", "RetroRegisterBatchId", c => c.Int());
            CreateIndex("dbo.RetroRegister", "Type");
            CreateIndex("dbo.RetroRegister", "RetroRegisterBatchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegister", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.RetroRegister", new[] { "Type" });
            AlterColumn("dbo.RetroRegister", "RetroRegisterBatchId", c => c.Int(nullable: false));
            DropColumn("dbo.RetroRegister", "Type");
            CreateIndex("dbo.RetroRegister", "RetroRegisterBatchId");
        }
    }
}
