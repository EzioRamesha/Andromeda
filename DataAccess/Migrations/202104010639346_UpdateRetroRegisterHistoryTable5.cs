namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegisterHistories", "Remark", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroRegisterHistories", "Remark");
        }
    }
}
