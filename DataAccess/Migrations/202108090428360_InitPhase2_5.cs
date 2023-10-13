namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitPhase2_5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RateTables", "GroupDiscountId", "dbo.GroupDiscounts");
            DropForeignKey("dbo.RateTables", "LargeDiscountId", "dbo.LargeDiscounts");
            DropForeignKey("dbo.RateTables", "RiDiscountId", "dbo.RiDiscounts");
            DropIndex("dbo.Mfrs17CellMappings", new[] { "RateTable" });
            DropIndex("dbo.RateTables", new[] { "RiDiscountId" });
            DropIndex("dbo.RateTables", new[] { "LargeDiscountId" });
            DropIndex("dbo.RateTables", new[] { "GroupDiscountId" });
            AddColumn("dbo.RateTables", "RiDiscountCode", c => c.String(maxLength: 30));
            AddColumn("dbo.RateTables", "LargeDiscountCode", c => c.String(maxLength: 30));
            AddColumn("dbo.RateTables", "GroupDiscountCode", c => c.String(maxLength: 30));
            AlterColumn("dbo.ClaimDataComputations", "Condition", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.ClaimDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.ClaimDataValidations", "Condition", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.Mfrs17CellMappings", "RateTable", c => c.String(maxLength: 128));
            AlterColumn("dbo.RiDataComputations", "Condition", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.RiDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 512));
            AlterColumn("dbo.RiDataPreValidations", "Condition", c => c.String(nullable: false, maxLength: 512));
            CreateIndex("dbo.Mfrs17CellMappings", "RateTable");
            CreateIndex("dbo.RateTables", "RiDiscountCode");
            CreateIndex("dbo.RateTables", "LargeDiscountCode");
            CreateIndex("dbo.RateTables", "GroupDiscountCode");
            AddForeignKey("dbo.RiDataWarehouse", "EndingPolicyStatus", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", "dbo.PickListDetails");
            DropForeignKey("dbo.RiDataWarehouse", "EndingPolicyStatus", "dbo.PickListDetails");
            DropIndex("dbo.RateTables", new[] { "GroupDiscountCode" });
            DropIndex("dbo.RateTables", new[] { "LargeDiscountCode" });
            DropIndex("dbo.RateTables", new[] { "RiDiscountCode" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "RateTable" });
            AlterColumn("dbo.RiDataPreValidations", "Condition", c => c.String(nullable: false));
            AlterColumn("dbo.RiDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.RiDataComputations", "Condition", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.Mfrs17CellMappings", "RateTable", c => c.String(maxLength: 50));
            AlterColumn("dbo.ClaimDataValidations", "Condition", c => c.String(nullable: false));
            AlterColumn("dbo.ClaimDataComputations", "CalculationFormula", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ClaimDataComputations", "Condition", c => c.String(nullable: false, unicode: false, storeType: "text"));
            DropColumn("dbo.RateTables", "GroupDiscountCode");
            DropColumn("dbo.RateTables", "LargeDiscountCode");
            DropColumn("dbo.RateTables", "RiDiscountCode");
            CreateIndex("dbo.RateTables", "GroupDiscountId");
            CreateIndex("dbo.RateTables", "LargeDiscountId");
            CreateIndex("dbo.RateTables", "RiDiscountId");
            CreateIndex("dbo.Mfrs17CellMappings", "RateTable");
            AddForeignKey("dbo.RateTables", "RiDiscountId", "dbo.RiDiscounts", "Id");
            AddForeignKey("dbo.RateTables", "LargeDiscountId", "dbo.LargeDiscounts", "Id");
            AddForeignKey("dbo.RateTables", "GroupDiscountId", "dbo.GroupDiscounts", "Id");
        }
    }
}
