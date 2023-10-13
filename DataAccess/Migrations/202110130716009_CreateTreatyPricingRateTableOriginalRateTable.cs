namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTreatyPricingRateTableOriginalRateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TreatyPricingRateTableOriginalRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TreatyPricingRateTableVersionId = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        MaleNonSmoker = c.Double(),
                        MaleSmoker = c.Double(),
                        FemaleNonSmoker = c.Double(),
                        FemaleSmoker = c.Double(),
                        Male = c.Double(),
                        Female = c.Double(),
                        Unisex = c.Double(),
                        UnitRate = c.Double(),
                        OccupationClass = c.Double(),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.TreatyPricingRateTableVersions", t => t.TreatyPricingRateTableVersionId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.TreatyPricingRateTableVersionId)
                .Index(t => t.Age)
                .Index(t => t.MaleNonSmoker)
                .Index(t => t.MaleSmoker)
                .Index(t => t.FemaleNonSmoker)
                .Index(t => t.FemaleSmoker)
                .Index(t => t.Male)
                .Index(t => t.Female)
                .Index(t => t.Unisex)
                .Index(t => t.UnitRate)
                .Index(t => t.OccupationClass)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyPricingRateTableOriginalRates", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingRateTableOriginalRates", "TreatyPricingRateTableVersionId", "dbo.TreatyPricingRateTableVersions");
            DropForeignKey("dbo.TreatyPricingRateTableOriginalRates", "CreatedById", "dbo.Users");
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "UpdatedById" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "CreatedById" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "OccupationClass" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "UnitRate" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "Unisex" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "Female" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "Male" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "FemaleSmoker" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "FemaleNonSmoker" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "MaleSmoker" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "MaleNonSmoker" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "Age" });
            DropIndex("dbo.TreatyPricingRateTableOriginalRates", new[] { "TreatyPricingRateTableVersionId" });
            DropTable("dbo.TreatyPricingRateTableOriginalRates");
        }
    }
}
