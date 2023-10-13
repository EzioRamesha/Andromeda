namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RetroRegister", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RetroRegister", "Frequency", c => c.Int());
        }
    }
}
