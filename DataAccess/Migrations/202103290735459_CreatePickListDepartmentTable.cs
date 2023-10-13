namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePickListDepartmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickListDepartments",
                c => new
                    {
                        PickListId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => new { t.PickListId, t.DepartmentId })
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.PickLists", t => t.PickListId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PickListId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
            AddColumn("dbo.PickLists", "UsedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PickListDepartments", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PickListDepartments", "PickListId", "dbo.PickLists");
            DropForeignKey("dbo.PickListDepartments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.PickListDepartments", "CreatedById", "dbo.Users");
            DropIndex("dbo.PickListDepartments", new[] { "UpdatedById" });
            DropIndex("dbo.PickListDepartments", new[] { "CreatedById" });
            DropIndex("dbo.PickListDepartments", new[] { "DepartmentId" });
            DropIndex("dbo.PickListDepartments", new[] { "PickListId" });
            DropColumn("dbo.PickLists", "UsedBy");
            DropTable("dbo.PickListDepartments");
        }
    }
}
