namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnnuityFactorDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnnuityFactorDetails", "InsuredGenderCodePickListDetailId", c => c.Int());
            AddColumn("dbo.AnnuityFactorDetails", "InsuredTobaccoUsePickListDetailId", c => c.Int());
            AddColumn("dbo.AnnuityFactorDetails", "InsuredAttainedAge", c => c.Int());
            AddColumn("dbo.AnnuityFactorDetails", "PolicyTerm", c => c.Double());
            CreateIndex("dbo.AnnuityFactorDetails", "InsuredGenderCodePickListDetailId");
            CreateIndex("dbo.AnnuityFactorDetails", "InsuredTobaccoUsePickListDetailId");
            CreateIndex("dbo.AnnuityFactorDetails", "InsuredAttainedAge");
            CreateIndex("dbo.AnnuityFactorDetails", "PolicyTerm");
            AddForeignKey("dbo.AnnuityFactorDetails", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.AnnuityFactorDetails", "InsuredTobaccoUsePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnnuityFactorDetails", "InsuredTobaccoUsePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.AnnuityFactorDetails", "InsuredGenderCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.AnnuityFactorDetails", new[] { "PolicyTerm" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "InsuredAttainedAge" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "InsuredTobaccoUsePickListDetailId" });
            DropIndex("dbo.AnnuityFactorDetails", new[] { "InsuredGenderCodePickListDetailId" });
            DropColumn("dbo.AnnuityFactorDetails", "PolicyTerm");
            DropColumn("dbo.AnnuityFactorDetails", "InsuredAttainedAge");
            DropColumn("dbo.AnnuityFactorDetails", "InsuredTobaccoUsePickListDetailId");
            DropColumn("dbo.AnnuityFactorDetails", "InsuredGenderCodePickListDetailId");
        }
    }
}
