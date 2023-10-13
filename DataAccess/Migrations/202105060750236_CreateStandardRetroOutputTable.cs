namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateStandardRetroOutputTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StandardRetroOutputs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    DataType = c.Int(nullable: false),
                    Code = c.String(maxLength: 128),
                    Name = c.String(maxLength: 128),
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
            DropForeignKey("dbo.StandardRetroOutputs", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.StandardRetroOutputs", "CreatedById", "dbo.Users");
            DropIndex("dbo.StandardRetroOutputs", new[] { "UpdatedById" });
            DropIndex("dbo.StandardRetroOutputs", new[] { "CreatedById" });
            DropTable("dbo.StandardRetroOutputs");
        }
    }
}
