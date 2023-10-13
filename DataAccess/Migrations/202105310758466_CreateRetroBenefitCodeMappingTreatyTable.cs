namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRetroBenefitCodeMappingTreatyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RetroBenefitCodeMappingTreaties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RetroBenefitCodeMappingId = c.Int(nullable: false),
                        TreatyCode = c.String(maxLength: 35),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroBenefitCodeMappings", t => t.RetroBenefitCodeMappingId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroBenefitCodeMappingId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetroBenefitCodeMappingTreaties", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RetroBenefitCodeMappingTreaties", "RetroBenefitCodeMappingId", "dbo.RetroBenefitCodeMappings");
            DropForeignKey("dbo.RetroBenefitCodeMappingTreaties", "CreatedById", "dbo.Users");
            DropIndex("dbo.RetroBenefitCodeMappingTreaties", new[] { "UpdatedById" });
            DropIndex("dbo.RetroBenefitCodeMappingTreaties", new[] { "CreatedById" });
            DropIndex("dbo.RetroBenefitCodeMappingTreaties", new[] { "RetroBenefitCodeMappingId" });
            DropTable("dbo.RetroBenefitCodeMappingTreaties");
        }
    }
}
