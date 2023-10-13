namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroRegister", "Remark", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RetroRegister", "Remark");
        }
    }
}
