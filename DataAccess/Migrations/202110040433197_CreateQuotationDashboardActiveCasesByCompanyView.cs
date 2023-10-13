namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuotationDashboardActiveCasesByCompanyView : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE OR ALTER VIEW [dbo].[QuotationDashboardActiveCasesByCompany]" +
            " AS" +
            " SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.CedantName, t.ReinsuranceTypeName ASC)) AS Id, t.* FROM" +
            " (SELECT DISTINCT " +
                " c.[Name] AS CedantName, c.Id AS CedantId, " +
                " p.[Description] AS ReinsuranceTypeName, p.Id AS ReinsuranceTypeId, " +
                " (SELECT COUNT(*) FROM TreatyPricingQuotationWorkflows q1 WHERE q1.CedantId = q.CedantId AND " +
                " q1.ReinsuranceTypePickListDetailId = q.ReinsuranceTypePickListDetailId AND q1.[Status] = 1) AS QuotingCaseCount, " +
                " (SELECT COUNT(*) FROM TreatyPricingQuotationWorkflows q1 WHERE q1.CedantId = q.CedantId AND " +
                " q1.ReinsuranceTypePickListDetailId = q.ReinsuranceTypePickListDetailId AND q1.[Status] = 2 AND " +
                " dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](q1.FinaliseDate, GETDATE()) > 14) AS QuotedExceeded14Days, " +
                " (SELECT ISNULL(SUM(TRY_CONVERT(FLOAT,REPLACE(ISNULL(pv.ExpectedRiPremium,0),',',''))),0) FROM TreatyPricingQuotationWorkflows q1 " +
                " LEFT JOIN TreatyPricingWorkflowObjects o ON o.WorkflowId = q1.Id " +
                " LEFT JOIN TreatyPricingProductVersions pv ON pv.Id = o.ObjectVersionId " +
                " WHERE q1.CedantId = q.CedantId AND q1.ReinsuranceTypePickListDetailId = q.ReinsuranceTypePickListDetailId AND " +
                " o.ObjectType = 8) AS ExpectedRiPremium " +
            " FROM TreatyPricingQuotationWorkflows q " +
            " LEFT JOIN Cedants c ON c.Id = q.CedantId " +
            " LEFT JOIN PickListDetails p ON p.Id = q.ReinsuranceTypePickListDetailId) t");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[QuotationDashboardActiveCasesByCompany]");
        }
    }
}
