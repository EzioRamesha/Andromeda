namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModuleTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "HideParameters", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "HideParameters");
        }
    }
}
