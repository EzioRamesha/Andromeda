namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyDiscountDetailTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyDiscountTableDetails", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "BenefitId" });
            AddColumn("dbo.TreatyDiscountTableDetails", "BenefitCode", c => c.String(nullable: false));
            DropColumn("dbo.TreatyDiscountTableDetails", "BenefitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyDiscountTableDetails", "BenefitId", c => c.Int(nullable: false));
            DropColumn("dbo.TreatyDiscountTableDetails", "BenefitCode");
            CreateIndex("dbo.TreatyDiscountTableDetails", "BenefitId");
            AddForeignKey("dbo.TreatyDiscountTableDetails", "BenefitId", "dbo.Benefits", "Id");
        }
    }
}
