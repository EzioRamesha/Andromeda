namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataBatchTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataBatches", "IsClaimDataAutoCreate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataBatches", "IsClaimDataAutoCreate");
        }
    }
}
