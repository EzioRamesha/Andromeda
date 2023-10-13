namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataComputationTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
