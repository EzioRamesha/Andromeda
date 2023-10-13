namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterId", "dbo.InvoiceRegister");
            DropForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches");
            DropForeignKey("dbo.InvoiceRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.InvoiceRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
            AddForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterBatchId", "dbo.InvoiceRegisterBatches", "Id");
            AddForeignKey("dbo.InvoiceRegisterHistories", "InvoiceRegisterId", "dbo.InvoiceRegister", "Id");
        }
    }
}
