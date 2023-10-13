namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateExportTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ObjectId = c.Int(),
                        Total = c.Int(nullable: false),
                        Processed = c.Int(nullable: false),
                        Parameters = c.String(storeType: "ntext"),
                        GenerateStartAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        GenerateEndAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exports", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Exports", "CreatedById", "dbo.Users");
            DropIndex("dbo.Exports", new[] { "UpdatedById" });
            DropIndex("dbo.Exports", new[] { "CreatedById" });
            DropTable("dbo.Exports");
        }
    }
}
