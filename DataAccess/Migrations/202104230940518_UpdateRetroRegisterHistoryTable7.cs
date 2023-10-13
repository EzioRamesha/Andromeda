namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegisterHistories", "Status", c => c.Int());
            CreateIndex("dbo.RetroRegisterHistories", "Status");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegisterHistories", new[] { "Status" });
            DropColumn("dbo.RetroRegisterHistories", "Status");
        }
    }
}
