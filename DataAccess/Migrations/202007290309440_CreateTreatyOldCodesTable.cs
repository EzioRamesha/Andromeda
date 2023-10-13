namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTreatyOldCodesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TreatyOldCodes",
                c => new
                    {
                        TreatyCodeId = c.Int(nullable: false),
                        OldTreatyCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TreatyCodeId, t.OldTreatyCodeId })
                .ForeignKey("dbo.TreatyCodes", t => t.OldTreatyCodeId)
                .ForeignKey("dbo.TreatyCodes", t => t.TreatyCodeId)
                .Index(t => t.TreatyCodeId)
                .Index(t => t.OldTreatyCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyOldCodes", "TreatyCodeId", "dbo.TreatyCodes");
            DropForeignKey("dbo.TreatyOldCodes", "OldTreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.TreatyOldCodes", new[] { "OldTreatyCodeId" });
            DropIndex("dbo.TreatyOldCodes", new[] { "TreatyCodeId" });
            DropTable("dbo.TreatyOldCodes");
        }
    }
}
