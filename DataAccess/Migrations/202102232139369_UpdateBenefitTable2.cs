namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBenefitTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Benefits", "BenefitCategoryPickListDetailId", c => c.Int());
            CreateIndex("dbo.Benefits", "BenefitCategoryPickListDetailId");
            AddForeignKey("dbo.Benefits", "BenefitCategoryPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Benefits", "BenefitCategoryPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.Benefits", new[] { "BenefitCategoryPickListDetailId" });
            DropColumn("dbo.Benefits", "BenefitCategoryPickListDetailId");
        }
    }
}
