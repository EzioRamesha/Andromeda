namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyBenefitCodeMappingTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappings", "ProfitCommPickListDetailId", c => c.Int());
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ProfitCommPickListDetailId");
            AddForeignKey("dbo.TreatyBenefitCodeMappings", "ProfitCommPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyBenefitCodeMappings", "ProfitCommPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ProfitCommPickListDetailId" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "ProfitCommPickListDetailId");
        }
    }
}
