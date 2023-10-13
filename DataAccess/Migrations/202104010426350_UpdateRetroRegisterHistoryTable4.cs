namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegisterHistories", "GstPayable", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroRegisterHistories", "GstPayable");
        }
    }
}
