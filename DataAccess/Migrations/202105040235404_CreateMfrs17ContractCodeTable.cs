namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMfrs17ContractCodeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mfrs17ContractCodeDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mfrs17ContractCodeId = c.Int(nullable: false),
                        ContractCode = c.String(maxLength: 255),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Mfrs17ContractCodes", t => t.Mfrs17ContractCodeId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.Mfrs17ContractCodeId)
                .Index(t => t.ContractCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
            CreateTable(
                "dbo.Mfrs17ContractCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CedingCompanyId = c.Int(nullable: false),
                        ModifiedContractCode = c.String(maxLength: 255),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdatedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cedants", t => t.CedingCompanyId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CedingCompanyId)
                .Index(t => t.ModifiedContractCode)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "Mfrs17ContractCodeId", "dbo.Mfrs17ContractCodes");
            DropForeignKey("dbo.Mfrs17ContractCodes", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ContractCodes", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.Mfrs17ContractCodes", "CedingCompanyId", "dbo.Cedants");
            DropForeignKey("dbo.Mfrs17ContractCodeDetails", "CreatedById", "dbo.Users");
            DropIndex("dbo.Mfrs17ContractCodes", new[] { "UpdatedById" });
            DropIndex("dbo.Mfrs17ContractCodes", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17ContractCodes", new[] { "ModifiedContractCode" });
            DropIndex("dbo.Mfrs17ContractCodes", new[] { "CedingCompanyId" });
            DropIndex("dbo.Mfrs17ContractCodeDetails", new[] { "UpdatedById" });
            DropIndex("dbo.Mfrs17ContractCodeDetails", new[] { "CreatedById" });
            DropIndex("dbo.Mfrs17ContractCodeDetails", new[] { "ContractCode" });
            DropIndex("dbo.Mfrs17ContractCodeDetails", new[] { "Mfrs17ContractCodeId" });
            DropTable("dbo.Mfrs17ContractCodes");
            DropTable("dbo.Mfrs17ContractCodeDetails");
        }
    }
}
