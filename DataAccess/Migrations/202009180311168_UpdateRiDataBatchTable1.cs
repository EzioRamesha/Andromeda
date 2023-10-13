namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataBatchTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataBatches", "TotalComputationFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalMappingFailedStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalPreValidationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.RiDataBatches", "TotalFinaliseFailedStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataBatches", "TotalFinaliseFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalPreValidationStatus");
            DropColumn("dbo.RiDataBatches", "TotalMappingFailedStatus");
            DropColumn("dbo.RiDataBatches", "TotalComputationFailedStatus");
        }
    }
}
