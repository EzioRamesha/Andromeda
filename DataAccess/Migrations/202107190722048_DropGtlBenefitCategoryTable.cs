namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropGtlBenefitCategoryTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GtlBenefitCategories", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.GtlBenefitCategories", "UpdatedById", "dbo.Users");
            DropIndex("dbo.GtlBenefitCategories", new[] { "Category" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "Benefit" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "CreatedById" });
            DropIndex("dbo.GtlBenefitCategories", new[] { "UpdatedById" });
            DropTable("dbo.GtlBenefitCategories");

            Sql(
            @"
                DELETE FROM AccessMatrices
                WHERE ModuleId = (SELECT Id FROM Modules WHERE controller = 'GtlBenefitCategory');
            ");

            Sql(
            @"
                DELETE FROM Modules WHERE controller = 'GtlBenefitCategory';
            ");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GtlBenefitCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(nullable: false, maxLength: 255),
                        Benefit = c.String(nullable: false, maxLength: 255),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.GtlBenefitCategories", "UpdatedById");
            CreateIndex("dbo.GtlBenefitCategories", "CreatedById");
            CreateIndex("dbo.GtlBenefitCategories", "Benefit");
            CreateIndex("dbo.GtlBenefitCategories", "Category");
            AddForeignKey("dbo.GtlBenefitCategories", "UpdatedById", "dbo.Users", "Id");
            AddForeignKey("dbo.GtlBenefitCategories", "CreatedById", "dbo.Users", "Id");
        }
    }
}
