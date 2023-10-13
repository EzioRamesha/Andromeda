namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataMappingTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataMappings", "DefaultObjectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataMappings", "DefaultObjectId");
        }
    }
}
