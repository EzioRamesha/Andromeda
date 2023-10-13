namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnnuityFactorDetailTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AnnuityFactorDetails", new[] { "PolicyTermRemain" });
            AlterColumn("dbo.AnnuityFactorDetails", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.AnnuityFactorDetails", "PolicyTermRemain");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AnnuityFactorDetails", new[] { "PolicyTermRemain" });
            AlterColumn("dbo.AnnuityFactorDetails", "PolicyTermRemain", c => c.Double(nullable: false));
            CreateIndex("dbo.AnnuityFactorDetails", "PolicyTermRemain");
        }
    }
}
