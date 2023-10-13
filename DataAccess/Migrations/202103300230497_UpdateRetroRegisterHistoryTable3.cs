namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RetroRegisterHistories", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RetroRegisterHistories", "Frequency", c => c.Int());
        }
    }
}
