namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegister", "GstPayable", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroRegister", "GstPayable");
        }
    }
}
