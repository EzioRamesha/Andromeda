namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataComputationTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClaimDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClaimDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
