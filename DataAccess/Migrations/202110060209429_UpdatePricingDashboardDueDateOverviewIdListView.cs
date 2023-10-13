namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePricingDashboardDueDateOverviewIdListView : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE OR ALTER VIEW [dbo].[PricingDashboardDueDateOverviewIdList] AS " +
                "SELECT TRY_CONVERT(INT,ROW_NUMBER() OVER(ORDER BY t.PricingTeamPickListDetailId, t.DueDateOverviewType ASC)) AS Id, t.* FROM " +
                "(SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'PP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'PP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "1 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 5 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "2 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 5 " +
                "AND dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) <= 10 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL " +
                "UNION ALL " +
                "SELECT (SELECT TOP 1 Id FROM PickListDetails where PickListId = 57 AND Code = 'GP') AS PricingTeamPickListDetailId, " +
                "3 AS DueDateOverviewType, q.Id AS QuotationId FROM TreatyPricingQuotationWorkflows q " +
                "LEFT JOIN PickListDetails p ON p.Id = q.PricingTeamPickListDetailId " +
                "WHERE dbo.[CalculateDateRangeExcludeWeekendsPublicHolidays](ISNULL(q.PricingDueDate,GETDATE()), GETDATE()) > 10 " +
                "AND p.Code = 'GP' AND q.PersonInChargeId IS NOT NULL) t ");
        }
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[PricingDashboardDueDateOverviewIdList]");
        }
    }
}
