namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataCorrectionTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataCorrections", new[] { "InsuredGenderCodePickListDetailId" });
            AlterColumn("dbo.RiDataCorrections", "InsuredGenderCodePickListDetailId", c => c.Int());
            AlterColumn("dbo.RiDataCorrections", "InsuredDateOfBirth", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiDataCorrections", "InsuredName", c => c.String(maxLength: 128));
            CreateIndex("dbo.RiDataCorrections", "InsuredGenderCodePickListDetailId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataCorrections", new[] { "InsuredGenderCodePickListDetailId" });
            AlterColumn("dbo.RiDataCorrections", "InsuredName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RiDataCorrections", "InsuredDateOfBirth", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiDataCorrections", "InsuredGenderCodePickListDetailId", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataCorrections", "InsuredGenderCodePickListDetailId");
        }
    }
}
