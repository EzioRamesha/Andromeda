namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateEmailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RecipientUserId = c.Int(),
                    ModuleController = c.String(maxLength: 64),
                    ObjectId = c.Int(),
                    Type = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    EmailAddress = c.String(maxLength: 256),
                    Data = c.String(storeType: "ntext"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.RecipientUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RecipientUserId)
                .Index(t => t.ModuleController)
                .Index(t => t.ObjectId)
                .Index(t => t.Type)
                .Index(t => t.Status)
                .Index(t => t.EmailAddress)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Emails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Emails", "RecipientUserId", "dbo.Users");
            DropForeignKey("dbo.Emails", "CreatedById", "dbo.Users");
            DropIndex("dbo.Emails", new[] { "UpdatedById" });
            DropIndex("dbo.Emails", new[] { "CreatedById" });
            DropIndex("dbo.Emails", new[] { "EmailAddress" });
            DropIndex("dbo.Emails", new[] { "Status" });
            DropIndex("dbo.Emails", new[] { "Type" });
            DropIndex("dbo.Emails", new[] { "ObjectId" });
            DropIndex("dbo.Emails", new[] { "ModuleController" });
            DropIndex("dbo.Emails", new[] { "RecipientUserId" });
            DropTable("dbo.Emails");
        }
    }
}
