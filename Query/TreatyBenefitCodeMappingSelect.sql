
/*
SELECT * FROM [TreatyBenefitCodeMappings]
WHERE [CedantId] = 2;
*/

/*
SELECT COUNT(*) as 'TOTAL' FROM [TreatyBenefitCodeMappingDetails]
WHERE [TreatyBenefitCodeMappingId] IN (
	SELECT [Id] FROM [TreatyBenefitCodeMappings]
	WHERE [CedantId] = 1
);
*/


SELECT
	[TreatyBenefitCodeMappings].[CedantId],
	count([TreatyBenefitCodeMappingDetails].[TreatyBenefitCodeMappingId]) as 'Total'
FROM [TreatyBenefitCodeMappingDetails]
LEFT JOIN [TreatyBenefitCodeMappings] ON [TreatyBenefitCodeMappingDetails].[TreatyBenefitCodeMappingId] = [TreatyBenefitCodeMappings].[Id]
GROUP BY [TreatyBenefitCodeMappings].[CedantId];
