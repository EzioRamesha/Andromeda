namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateObjectLocksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ObjectLocks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModuleId = c.Int(nullable: false),
                    ObjectId = c.Int(nullable: false),
                    LockedById = c.Int(nullable: false),
                    ExpiresAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.LockedById)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.ModuleId)
                .Index(t => t.ObjectId)
                .Index(t => t.LockedById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ObjectLocks", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.ObjectLocks", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.ObjectLocks", "LockedById", "dbo.Users");
            DropForeignKey("dbo.ObjectLocks", "CreatedById", "dbo.Users");
            DropIndex("dbo.ObjectLocks", new[] { "UpdatedById" });
            DropIndex("dbo.ObjectLocks", new[] { "CreatedById" });
            DropIndex("dbo.ObjectLocks", new[] { "LockedById" });
            DropIndex("dbo.ObjectLocks", new[] { "ObjectId" });
            DropIndex("dbo.ObjectLocks", new[] { "ModuleId" });
            DropTable("dbo.ObjectLocks");
        }
    }
}
