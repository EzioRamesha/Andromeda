namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UpdatedById", "dbo.Users");
            DropIndex("dbo.Users", new[] { "UpdatedById" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "UpdatedById");
            AddForeignKey("dbo.Users", "UpdatedById", "dbo.Users", "Id");
        }
    }
}
