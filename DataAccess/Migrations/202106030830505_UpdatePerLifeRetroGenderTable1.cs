namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeRetroGenderTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeRetroGenders", new[] { "Gender" });
            AddColumn("dbo.PerLifeRetroGenders", "InsuredGenderCodePickListDetailId", c => c.Int());
            AlterColumn("dbo.PerLifeRetroGenders", "Gender", c => c.String(maxLength: 15));
            CreateIndex("dbo.PerLifeRetroGenders", "InsuredGenderCodePickListDetailId");
            CreateIndex("dbo.PerLifeRetroGenders", "Gender");
            AddForeignKey("dbo.PerLifeRetroGenders", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeRetroGenders", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.PerLifeRetroGenders", new[] { "Gender" });
            DropIndex("dbo.PerLifeRetroGenders", new[] { "InsuredGenderCodePickListDetailId" });
            AlterColumn("dbo.PerLifeRetroGenders", "Gender", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.PerLifeRetroGenders", "InsuredGenderCodePickListDetailId");
            CreateIndex("dbo.PerLifeRetroGenders", "Gender");
        }
    }
}
