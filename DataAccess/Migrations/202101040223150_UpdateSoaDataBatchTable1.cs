namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataBatchTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataBatches", "IsAutoCreate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataBatches", "IsAutoCreate");
        }
    }
}
