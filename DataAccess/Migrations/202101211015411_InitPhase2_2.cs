namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitPhase2_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Remarks", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Remarks", "FollowUpUserId", "dbo.Users");
            DropForeignKey("dbo.UnderwritingRemarks", "ClaimRegisterId", "dbo.ClaimRegister");
            DropForeignKey("dbo.UnderwritingRemarks", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.UnderwritingRemarks", "UpdatedById", "dbo.Users");
            DropIndex("dbo.Documents", new[] { "DepartmentId" });
            DropIndex("dbo.Remarks", new[] { "DepartmentId" });
            DropIndex("dbo.Remarks", new[] { "FollowUpUserId" });
            DropIndex("dbo.UnderwritingRemarks", new[] { "ClaimRegisterId" });
            DropIndex("dbo.UnderwritingRemarks", new[] { "CreatedById" });
            DropIndex("dbo.UnderwritingRemarks", new[] { "UpdatedById" });
            CreateTable(
                "dbo.ObjectPermissions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ObjectId = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    DepartmentId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.ObjectId)
                .Index(t => t.Type)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.RemarkFollowUps",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RemarkId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    FollowUpAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    FollowUpUserId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.FollowUpUserId)
                .ForeignKey("dbo.Remarks", t => t.RemarkId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RemarkId)
                .Index(t => t.FollowUpUserId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            AddColumn("dbo.Documents", "SubModuleController", c => c.String());
            AddColumn("dbo.Documents", "SubObjectId", c => c.Int());
            AddColumn("dbo.Remarks", "SubModuleController", c => c.String());
            AddColumn("dbo.Remarks", "SubObjectId", c => c.Int());
            CreateIndex("dbo.Documents", "SubObjectId");
            CreateIndex("dbo.Remarks", "SubObjectId");
            DropColumn("dbo.Documents", "IsPrivate");
            DropColumn("dbo.Documents", "DepartmentId");
            DropColumn("dbo.Remarks", "IsPrivate");
            DropColumn("dbo.Remarks", "DepartmentId");
            DropColumn("dbo.Remarks", "HasFollowUp");
            DropColumn("dbo.Remarks", "FollowUpStatus");
            DropColumn("dbo.Remarks", "FollowUpAt");
            DropColumn("dbo.Remarks", "FollowUpUserId");
            DropTable("dbo.UnderwritingRemarks");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.UnderwritingRemarks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimRegisterId = c.Int(nullable: false),
                    Content = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Remarks", "FollowUpUserId", c => c.Int());
            AddColumn("dbo.Remarks", "FollowUpAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Remarks", "FollowUpStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Remarks", "HasFollowUp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Remarks", "DepartmentId", c => c.Int());
            AddColumn("dbo.Remarks", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "DepartmentId", c => c.Int());
            AddColumn("dbo.Documents", "IsPrivate", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.RemarkFollowUps", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.RemarkFollowUps", "RemarkId", "dbo.Remarks");
            DropForeignKey("dbo.RemarkFollowUps", "FollowUpUserId", "dbo.Users");
            DropForeignKey("dbo.RemarkFollowUps", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.ObjectPermissions", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.ObjectPermissions", "CreatedById", "dbo.Users");
            DropIndex("dbo.RemarkFollowUps", new[] { "UpdatedById" });
            DropIndex("dbo.RemarkFollowUps", new[] { "CreatedById" });
            DropIndex("dbo.RemarkFollowUps", new[] { "FollowUpUserId" });
            DropIndex("dbo.RemarkFollowUps", new[] { "RemarkId" });
            DropIndex("dbo.ObjectPermissions", new[] { "CreatedById" });
            DropIndex("dbo.ObjectPermissions", new[] { "DepartmentId" });
            DropIndex("dbo.ObjectPermissions", new[] { "Type" });
            DropIndex("dbo.ObjectPermissions", new[] { "ObjectId" });
            DropIndex("dbo.Remarks", new[] { "SubObjectId" });
            DropIndex("dbo.Documents", new[] { "SubObjectId" });
            DropColumn("dbo.Remarks", "SubObjectId");
            DropColumn("dbo.Remarks", "SubModuleController");
            DropColumn("dbo.Documents", "SubObjectId");
            DropColumn("dbo.Documents", "SubModuleController");
            DropTable("dbo.RemarkFollowUps");
            DropTable("dbo.ObjectPermissions");
            CreateIndex("dbo.UnderwritingRemarks", "UpdatedById");
            CreateIndex("dbo.UnderwritingRemarks", "CreatedById");
            CreateIndex("dbo.UnderwritingRemarks", "ClaimRegisterId");
            CreateIndex("dbo.Remarks", "FollowUpUserId");
            CreateIndex("dbo.Remarks", "DepartmentId");
            CreateIndex("dbo.Documents", "DepartmentId");
            AddForeignKey("dbo.UnderwritingRemarks", "UpdatedById", "dbo.Users", "Id");
            AddForeignKey("dbo.UnderwritingRemarks", "CreatedById", "dbo.Users", "Id");
            AddForeignKey("dbo.UnderwritingRemarks", "ClaimRegisterId", "dbo.ClaimRegister", "Id");
            AddForeignKey("dbo.Remarks", "FollowUpUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Remarks", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments", "Id");
        }
    }
}
