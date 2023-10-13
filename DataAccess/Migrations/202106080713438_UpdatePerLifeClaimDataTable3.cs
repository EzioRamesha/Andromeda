namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeClaimDataTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeClaimData", new[] { "LateInterestShareFlag" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ExGratiaShareFlag" });
            AddColumn("dbo.PerLifeClaimData", "IsLateInterestShare", c => c.Boolean(nullable: false));
            AddColumn("dbo.PerLifeClaimData", "IsExGratiaShare", c => c.Boolean(nullable: false));
            CreateIndex("dbo.PerLifeClaimData", "IsLateInterestShare");
            CreateIndex("dbo.PerLifeClaimData", "IsExGratiaShare");
            DropColumn("dbo.PerLifeClaimData", "LateInterestShareFlag");
            DropColumn("dbo.PerLifeClaimData", "ExGratiaShareFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PerLifeClaimData", "ExGratiaShareFlag", c => c.String(maxLength: 255));
            AddColumn("dbo.PerLifeClaimData", "LateInterestShareFlag", c => c.String(maxLength: 255));
            DropIndex("dbo.PerLifeClaimData", new[] { "IsExGratiaShare" });
            DropIndex("dbo.PerLifeClaimData", new[] { "IsLateInterestShare" });
            DropColumn("dbo.PerLifeClaimData", "IsExGratiaShare");
            DropColumn("dbo.PerLifeClaimData", "IsLateInterestShare");
            CreateIndex("dbo.PerLifeClaimData", "ExGratiaShareFlag");
            CreateIndex("dbo.PerLifeClaimData", "LateInterestShareFlag");
        }
    }
}
