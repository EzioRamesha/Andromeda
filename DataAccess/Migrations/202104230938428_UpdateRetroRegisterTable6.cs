namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegister", "Status", c => c.Int());
            CreateIndex("dbo.RetroRegister", "Status");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegister", new[] { "Status" });
            DropColumn("dbo.RetroRegister", "Status");
        }
    }
}
