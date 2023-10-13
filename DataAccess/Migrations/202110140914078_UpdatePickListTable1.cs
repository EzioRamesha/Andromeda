namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePickListTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PickLists", "UsedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PickLists", "UsedBy", c => c.String());
        }
    }
}
