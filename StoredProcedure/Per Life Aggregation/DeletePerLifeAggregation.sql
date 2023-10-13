CREATE OR ALTER PROCEDURE [dbo].[DeletePerLifeAggregation](@PerLifeAggregationId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

-- Delete Data
-- Delete MonthlyRetroData
DELETE FROM PerLifeAggregationMonthlyRetroData
WHERE PerLifeAggregationMonthlyDataId IN 
(SELECT Id FROM PerLifeAggregationMonthlyData WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId))));

-- Delete MonthlyData
DELETE FROM PerLifeAggregationMonthlyData
WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN 
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId)));

-- Delete DetailData
DELETE FROM PerLifeAggregationDetailData
WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN 
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId));

-- Delete DetailTreaty
DELETE FROM PerLifeAggregationDetailTreaties
WHERE PerLifeAggregationDetailId IN (SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId);

-- Delete Detail
DELETE FROM PerLifeAggregationDetails
WHERE PerLifeAggregationId = @PerLifeAggregationId;

SET @Result = '{ "success": 1 }';
