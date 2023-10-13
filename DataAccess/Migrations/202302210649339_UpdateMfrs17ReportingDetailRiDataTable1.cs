namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingDetailRiDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId" });
            DropPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas");
            AddColumn("dbo.Mfrs17ReportingDetailRiDatas", "CutOffId", c => c.Int(nullable: true));
            AlterColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseId", c => c.Int(nullable: true));
            //AddPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId", "RiDataWarehouseId", "CutOffId" });
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "Mfrs17ReportingDetailId");
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseId");
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "CutOffId");
            AddForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "CutOffId", "dbo.CutOff", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mfrs17ReportingDetailRiDatas", "CutOffId", "dbo.CutOff");
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "CutOffId" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "RiDataWarehouseId" });
            DropIndex("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId" });
            //DropPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas");
            AlterColumn("dbo.Mfrs17ReportingDetailRiDatas", "RiDataWarehouseId", c => c.Int());
            DropColumn("dbo.Mfrs17ReportingDetailRiDatas", "CutOffId");
            AddPrimaryKey("dbo.Mfrs17ReportingDetailRiDatas", new[] { "Mfrs17ReportingDetailId", "RiDataWarehouseHistoryId" });
            CreateIndex("dbo.Mfrs17ReportingDetailRiDatas", "Mfrs17ReportingDetailId");
        }
    }
}
