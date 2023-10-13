namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeSoaDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeAggregationDetailDataId" });
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeClaimDataId" });
            AlterColumn("dbo.PerLifeSoaData", "PerLifeAggregationDetailDataId", c => c.Int());
            AlterColumn("dbo.PerLifeSoaData", "PerLifeClaimDataId", c => c.Int());
            CreateIndex("dbo.PerLifeSoaData", "PerLifeAggregationDetailDataId");
            CreateIndex("dbo.PerLifeSoaData", "PerLifeClaimDataId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeClaimDataId" });
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeAggregationDetailDataId" });
            AlterColumn("dbo.PerLifeSoaData", "PerLifeClaimDataId", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeSoaData", "PerLifeAggregationDetailDataId", c => c.Int(nullable: false));
            CreateIndex("dbo.PerLifeSoaData", "PerLifeClaimDataId");
            CreateIndex("dbo.PerLifeSoaData", "PerLifeAggregationDetailDataId");
        }
    }
}
