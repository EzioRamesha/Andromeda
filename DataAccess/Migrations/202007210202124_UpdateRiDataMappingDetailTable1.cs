namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataMappingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RiDataMappingDetails", "StandardOutputId", "dbo.StandardOutputs");
            DropIndex("dbo.RiDataMappingDetails", new[] { "StandardOutputId" });
            DropIndex("dbo.RiDataMappingDetails", new[] { "PickListDetailId" });
            AddColumn("dbo.RiDataMappingDetails", "IsPickDetailIdEmpty", c => c.Boolean(nullable: false));
            AddColumn("dbo.RiDataMappingDetails", "IsRawValueEmpty", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiDataMappingDetails", "PickListDetailId", c => c.Int());
            AlterColumn("dbo.RiDataMappingDetails", "RawValue", c => c.String(maxLength: 128));
            CreateIndex("dbo.RiDataMappingDetails", "PickListDetailId");
            DropColumn("dbo.RiDataMappingDetails", "StandardOutputId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RiDataMappingDetails", "StandardOutputId", c => c.Int(nullable: false));
            DropIndex("dbo.RiDataMappingDetails", new[] { "PickListDetailId" });
            AlterColumn("dbo.RiDataMappingDetails", "RawValue", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RiDataMappingDetails", "PickListDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.RiDataMappingDetails", "IsRawValueEmpty");
            DropColumn("dbo.RiDataMappingDetails", "IsPickDetailIdEmpty");
            CreateIndex("dbo.RiDataMappingDetails", "PickListDetailId");
            CreateIndex("dbo.RiDataMappingDetails", "StandardOutputId");
            AddForeignKey("dbo.RiDataMappingDetails", "StandardOutputId", "dbo.StandardOutputs", "Id");
        }
    }
}
